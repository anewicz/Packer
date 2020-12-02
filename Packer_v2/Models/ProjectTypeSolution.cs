using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("PROJECT_TYPE_SOLUTION")]
    public class ProjectTypeSolution
    {
        [Key]
        [Column("ID_PROJECT_TYPE_SOLUTION")]
        public Int64 IdProjectTypeIntegration { get; set; }

        [Column("NM_TYPE_SOLUTION")]
        [Display(Name = "Nome da Solução")]
        public string NmTypeIntegration { get; set; }

        [Column("DE_SOLUTION")]
        [Display(Name = "Descrição da Solução")]
        public string DeIntegration { get; set; }

        [ForeignKey("TypeSolution")]
        [Column("ID_TYPE_SOLUTION")]
        [Display(Name = "Tipo da Solução")]
        public Int64 IdTypeSolution { get; set; }

        [ForeignKey("Project")]
        [Column("ID_PROJECT")]
        [Display(Name = "Projeto")]
        public Int64 IdProject { get; set; }


        //propriedades de navegação
        public virtual Project Project { get; set; }
        public virtual TypeSolution TypeSolution { get; set; }
    }
}