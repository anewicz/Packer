using Packer_v2.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("DTBASE")]
    public class Dtbase
    {
        [Key]
        [Column("ID_DTBASE")]
        public Int64 IdDtbase { get; set; }

        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [ForeignKey("DbIp")]
        [Column("ID_DB_IP")]
        [Display(Name = "Database IP")]
        public Int64 IdDbIp { get; set; }

        [Column("NM_DATABASE")]
        [Display(Name = "Database Nome")]
        public string NmDatabase { get; set; }

        [Column("IS_ACTIVE")]
        [Display(Name = "Ativo?")]
        public bool IsActive { get; set; }

        [Column("IS_CORE")]
        [Display(Name = "É Base Core?")]
        public bool IsCore { get; set; }

        [Column("IS_DEV")]
        [Display(Name = "É Base Dev?")]
        public bool IsDev { get; set; }


        //propriedades de navegação

        public virtual DbIp DbIp { get; set; }

        //propriedades para vizualização
        [Display(Name = "Database")]
        public virtual string FullNmDatabase
        {
            get { return getFullNmDatabase(IdDbIp); }
        }

        public string getFullNmDatabase(long idIP)
        {
            PackerContext db = new PackerContext();
            DbIp dbIp = db.DbIp.Find(idIP);
            string nmDbIp = dbIp.NmIp;

            string _isDev = IsDev ? "DEV" : "PRD";

            return _isDev + " [" + nmDbIp + "] "+ NmDatabase+ "";
            
        }

        
    }
}


