using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("DB_SOLUTION")]
    public class DbSolution
    {
        [Key]
        [Column("ID_DB_SOLUTION")]
        public Int64 IdDbSolution { get; set; }

        [ForeignKey("Dtbase")]
        [Column("ID_DB_IP")]
        [Display(Name = "Database")]
        public Int64 IdDtbase { get; set; }

        [ForeignKey("Solution")]
        [Column("ID_SOLUTION")]
        [Display(Name = "Solução")]
        public Int64 IdSolution { get; set; }

        //propriedades de navegação
        public virtual Solution Solution { get; set; }

        public virtual Dtbase Dtbase { get; set; }
    }
}