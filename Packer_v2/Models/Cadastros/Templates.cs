using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Packer_v2.Models
{
    [Table("TEMPLATES")]
    public class Templates
    {
        [Key]
        [Column("ID_TEMPLATE")]
        public Int64 IdTemplate { get; set; }

        
        [Column("NM_TEMPLATE")]
        [Display(Name = "teste Editor")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Campo deve ter no mínimo 3 caracteres e no máximo 100 caracteres")]
        [DataType(DataType.Text)]
        [AllowHtml]
        public string NmTemplate { get; set; }

    }
}