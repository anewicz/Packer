using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("TICKET")]
    public class Ticket
    {
        [Key]
        [Column("ID_TICKET")]
        public Int64 IdTicket { get; set; }

        [Column("DT_REGISTER")]
        [Display(Name = "Dt Cadastro")]
        public DateTime? DtRegister { get; set; }


        [Column("DT_LAST_MODIFICATION")]
        [Display(Name = "Dt Ultima Modificação")]
        public DateTime? DtLastModification { get; set; }


        [Column("DE_TITLE")]
        [Display(Name = "Titulo Ticket")]
        public string DeTitle { get; set; }


        [Column("TICKET_LINK")]
        [Display(Name = "Link Ticket")]
        public string TicketLink { get; set; }

        [Column("NM_TICKET")]
        [Display(Name = "Cod Ticket")]
        public string NmTicket { get; set; }

        [ForeignKey("Solution")]
        [Column("ID_SOLUTION")]
        [Display(Name = "Solução")]
        public Int64 IdSolution { get; set; }

        [ForeignKey("Status")]
        [Column("ID_STATUS")]
        [Display(Name = "Status")]
        public Int64 IdStatus { get; set; }

        [Column("DE_TICKET")]
        [Display(Name = "Descrição Roteiro")]
        [DataType(DataType.MultilineText)]
        public string DeTicket { get; set; }

        [Column("DE_NOTE")]
        [Display(Name = "Observação")]
        [DataType(DataType.MultilineText)]
        public string DeNote { get; set; }

        [Column("DE_IMPACT")]
        [Display(Name = "Impactos")]
        [DataType(DataType.MultilineText)]
        public string DeImpact { get; set; }

        [Column("DE_RISK_OF_NOT_DOING")]
        [Display(Name = "Risco de Não Fazer")]
        [DataType(DataType.MultilineText)]
        public string DeRiskOfNotDoing { get; set; }

        [Column("DE_RISK_OF_DOING")]
        [Display(Name = "Risco de Fazer")]
        [DataType(DataType.MultilineText)]
        public string DeRiskOfDoing { get; set; }

        [Column("DE_CONTINGENCY")]
        [Display(Name = "Contingência")]
        [DataType(DataType.MultilineText)]
        public string DeContingency { get; set; }

        [Column("DE_PREREQUISITES")]
        [Display(Name = "Pré-Requisitos")]
        [DataType(DataType.MultilineText)]
        public string DePrerequisites { get; set; }

        [Column("DE_UNAVAILABILITY")]
        [Display(Name = "Indisponibilidades")]
        [DataType(DataType.MultilineText)]
        public string DeUnavailability { get; set; }

        [Column("DE_RUNTIME")]
        [Display(Name = "Tempo de Execução")]
         [DataType(DataType.Time, ErrorMessage = "Hora em formato inválido")]
        public TimeSpan DeRuntime { get; set; }


        //propriedades de navegação
        public virtual Solution Solution { get; set; }

        public virtual Status Status { get; set; }


    }
}