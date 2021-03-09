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

        [Column("IS_ACTIVE")]
        [Display(Name = "Ativo?")]
        public bool IsActive { get; set; }

        [Column("IS_CORE")]
        [Display(Name = "Base Core?")]
        public bool IsCore { get; set; }

        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [ForeignKey("DevDbIp")]
        [Column("ID_DEV_DB_IP")]
        [Display(Name = "Db Ip [DEV]")]
        public Int64 IdDevDbIp { get; set; }

        [Column("NM_DEV_DATABASE")]
        [Display(Name = "Database [DEV]")]
        public string NmDevDatabase { get; set; }


        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [ForeignKey("PrdDbIp")]
        [Column("ID_PRD_DB_IP")]
        [Display(Name = "Db Ip [PRD]")]
        public Int64 IdPrdDbIp { get; set; }

        [Column("NM_PRD_DATABASE")]
        [Display(Name = "Database [PRD]")]
        public string NmPrdDatabase { get; set; }


        //propriedades de navegação

        public virtual DbIp DevDbIp { get; set; }
        public virtual DbIp PrdDbIp { get; set; }

        //propriedades para vizualização
        [Display(Name = "Database")]
        public virtual string FullNmDatabase
        {
            get { return getFullNmDatabase(); }
        }

        public string getFullNmDatabase()
        {
            PackerContext db = new PackerContext();
            DbIp dbIp = db.DbIp.Find(IdDevDbIp);
            string nmDbIp = dbIp.NmIp;

            return "DEV [" + nmDbIp + "] "+ NmDevDatabase + "";
            
        }

        
    }
}


