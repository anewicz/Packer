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

        [Required(ErrorMessage = "O Projeto Obrigatório")]
        
        [Column("ID_PROJECT_AYTY")]
        [Display(Name = "PROJETO")]
        public Int64 IdProjectAyty { get; set; }

        [Required(ErrorMessage = "O Nome do Projeto Obrigatório")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "O Nome do Projeto Obrigatório")]
        [Column("NM_PROJECT")]
        [Display(Name = "Nome do Projeto")]
        public string NmProject { get; set; }

        [Required(ErrorMessage = "A EPS é Obrigatória")]
        [ForeignKey("Eps")]
        [Column("ID_EPS")]
        [Display(Name = "EPS")]
        public Int64 idEps { get; set; }


        [StringLength(255, MinimumLength = 2, ErrorMessage = "Caminho do TTC")]
        [Column("WAY_TTC")]
        [Display(Name = "Caminho do TTC")]
        public string WayPatch { get; set; }

        //propriedades de navegação
        public virtual Eps Eps { get; set; }

    }
}