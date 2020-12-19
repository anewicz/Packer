class RegisterForm {

    constructor(form, pUrlSave, pUrlDelete) {
        this.form = form;
        this.urlSave = pUrlSave;
        this.urlDelete = pUrlDelete;
        this.typeProcess = "NEW";
    }

    EditForm(pId) {
        let line = document.getElementById(`tr_${pId}`);
        let columns = line.getElementsByTagName('td')

        for (var c = 0; c < columns.length; c++) {
            let nameColumn = columns[c].id;

            if (nameColumn == undefined || nameColumn == "")
                continue;

            let value = columns[c].innerHTML;
            let element = this.form.find(`#${nameColumn}`);

            element.val(value);
        }

        $("html, body").animate({ scrollTop: 0 }, "slow");
    }

    ConfirmDelete(pId) {
        if (pId == undefined || pId == 0) {
            toast("Atenção", "Selecione o registro que deseja excluir.", "error");
            return;
        }

        $("#confirmText").html("Confirma exclusão do registro?")
        $('#modalConfirm').modal('show');
        this.typeProcess = "DELETE";
    }

    ConfirmSave() {
        if (!this.form.valid()) {
            return;
        }

        $("#confirmText").html("Confirma salvar do registro?")
        $('#modalConfirm').modal('show');
        this.typeProcess = "SAVE";
    }

    ConfirmProcess() {
        if (this.typeProcess == "DELETE") {
            this.Delete();
        } else if (this.typeProcess == "SAVE") {
            this.Save();
        }
    }

    Save() {
        if (!this.form.valid()) {
            return;
        }

        $.ajax({
            async: true,
            "url": this.urlSave,
            "type": "POST",
            data: this.form.serialize(),
        })
            .done(function (msg) {
                if (msg.status.includes("nok")) {
                    let results = "Ocorreram erros ao salvar o registro!";

                    if (msg.mesage != undefined && msg.mesage != '') {
                        results = msg.mesage;
                    }

                    toast("Atenção", results, "error");
                    console.log(msg.error);
                } else {

                    if (msg.partialView != undefined && msg.partialView != '') {
                        $("#div-tab-result").html(msg.partialView);
                        $("#dataTables-report").DataTable({
                            responsive: true,
                            "order": []
                        })
                    }

                    let results = "Registro salvo com sucesso!";
                    toast("Atenção", results, "info");
                    let form = document.getElementById("form");
                    clearForm(form);
                }
            })
            .fail(function (jqXHR, textStatus, msg) {
                console.log(`${textStatus} - ${msg} - ${jqXHR}`);
            });
    }

    Delete() {
        $.ajax({
            async: true,
            "url": this.urlDelete,
            "type": "POST",
            data: this.form.serialize(),
        })
            .done(function (msg) {
                if (msg.status.includes("nok")) {
                    let results = "Ocorreram erros ao excluir o registro!";
                    toast("Atenção", results, "error");
                    console.log(msg.error);
                } else {
                    if (msg.partialView != undefined && msg.partialView != '') {
                        $("#div-tab-result").html(msg.partialView);
                        $("#dataTables-report").DataTable({
                            responsive: true,
                            "order": []
                        })
                    }

                    let results = "Registro excluído com sucesso!";
                    toast("Atenção", results, "info");
                    let form = document.getElementById("form");
                    clearForm(form);

                }
            })
            .fail(function (jqXHR, textStatus, msg) {
                console.log(`${textStatus} - ${msg} - ${jqXHR}`);
            });
    }
}