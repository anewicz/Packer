using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("TYPE_SOLUTION_WAY")]
    public class TypeSolutionWay
    {
        [Key]
        [Column("ID_TYPE_SOLUTION_WAY")]
        public Int64 IdTypeSolutionDefaultWay { get; set; }

        [ForeignKey("TypeSolution")]
        [Column("ID_TYPE_SOLUTION")]
        [Display(Name = "Tipo de Solução")]
        public Int64 IdTypeSolution { get; set; }

        [ForeignKey("WayType")]
        [Column("ID_WAT_TYPE")]
        [Display(Name = "Tipo de Caminho")]
        public Int64 IdWayType { get; set; }

        //propriedades de navegação
        public virtual TypeSolution TypeSolution { get; set; }
        public virtual WayType WayType { get; set; }

    }
}