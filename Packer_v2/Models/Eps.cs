using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("EPS")]
    public class Eps
    {
        [Key]
        [Column("ID_EPS")]
        [Display(Name = "EPS")]
        public Int64 IdEps { get; set; }

        [Column("NM_EPS")]
        [Display(Name = "Nome EPS")]
        public string NmEps { get; set; }

        [Display(Name = "Caminho Startup")]
        [Column("WAY_STARTUP")]
        public string WayStartup { get; set; }

    }
}