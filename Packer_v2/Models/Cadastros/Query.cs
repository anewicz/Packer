using Packer_v2.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("QUERY")]
    public class Query
    {
        [Key]
        [Column("ID_QUERY")]
        [Display(Name = "ID Query")]
        public Int64 IdQuery { get; set; }

        [ForeignKey("Ticket")]
        [Column("ID_TICKET")]
        [Display(Name = "Ticket")]
        public Int64 IdTicket { get; set; }

        [ForeignKey("Dtbase")]
        [Column("ID_DTBASE")]
        [Display(Name = "DataBase")]
        public Int64 IdDtBase { get; set; }
        

        [Column("IS_ACTIVE")]
        [Display(Name = "Ativo?")]
        public bool IsActive { get; set; }

        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [Column("NM_SQL_OBJECT")]
        [Display(Name = "Nome Obj SQL")]
        public string NmSqlObject { get; set; }

        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [Column("ID_SQL_ITEN_TYPE")]
        [Display(Name = "Item SQL")]
        public Int64 IdSqlItenType { get; set; }

        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [Column("ID_SQL_COMAND_TYPE")]
        [Display(Name = "Comando SQL")]
        public Int64 IdSqlComandType { get; set; }


        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [Column("NM_FILE")]
        [Display(Name = "Arquivo")]
        public string NmFile { get; set; }

        //propriedades de navegação
        public virtual Ticket Ticket { get; set; }
        public virtual Dtbase Dtbase { get; set; }
    }

}