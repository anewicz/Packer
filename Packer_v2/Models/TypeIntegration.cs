using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("TYPE_INTEGRATION")]
    public class TypeIntegration
    {
        [Key]
        [Column("ID_TYPE_INTEGRATION")]
        public Int64 IdTypeIntegration { get; set; }

        [Column("NM_TYPE_INTEGRATION")]
        [Display(Name = "Tipo de Integração")]
        public string NmTypeIntegration { get; set; }

        [Column("IS_OMNI")]
        [Display(Name = "É Omni?")]
        public bool IsOmni { get; set; }

    }
}