using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("PROJECT")]
    public class Project
    {
        [Key]
        [Column("ID_PROJECT")]
        public Int64 IdProject { get; set; }

        [Column("ID_PROJECT_AYTY")]
        [Display(Name = "PROJETO")]
        public Int64 IdProjectAyty { get; set; }

        [Column("NM_PROJECT")]
        [Display(Name = "Nome do Projeto")]
        public string NmProject { get; set; }

        [ForeignKey("Eps")]
        [Column("ID_EPS")]
        [Display(Name = "EPS")]
        public Int64 idEps { get; set; }

        [Column("WAY_TTC")]
        [Display(Name = "Caminho do TTC")]
        public string WayPatch { get; set; }

        //propriedades de navegação
        public virtual Eps Eps { get; set; }

    }
}