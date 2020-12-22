using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("DB_IP")]
    public class DbIp
    {
        [Key]
        [Column("ID_DB_IP")]
        public Int64 IdDbIp { get; set; }

        //[Required(ErrorMessage = "O Nome EPS é Obrigatório")]
        [Column("NM_IP")]
        [Display(Name = "Database IP")]
        public string NmIp { get; set; }

        [Column("IS_ACTIVE")]
        [Display(Name = "Ativo?")]
        public bool IsActive { get; set; }

    }
}