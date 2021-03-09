using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("STATUS")]
    public class Status
    {
        [Key]
        [Column("ID_STATUS")]
        public Int64 IdStatus { get; set; }

        [Column("NM_STATUS")]
        [Display(Name = "Status")]
        public string NmStatus { get; set; }

        [Column("DE_STATUS")]
        [Display(Name = "Descrição Status")]
        [DataType(DataType.MultilineText)]
        public string DeStatus { get; set; }

        [Column("IS_ACTIVE")]
        [Display(Name = "Ativo?")]
        public bool IsActive { get; set; }

        [Column("IS_FINALIZER")]
        [Display(Name = "Finalizador?")]
        public bool IsFinalizer { get; set; }

    }
}