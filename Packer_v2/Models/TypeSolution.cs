using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("TYPE_SOLUTION")]
    public class TypeSolution
    {
        [Key]
        [Column("ID_TYPE_SOLUTION")]
        public Int64 IdTypeSolution { get; set; }

        [Column("NM_TYPE_SOLUTION")]
        [Display(Name = "Tipo de Solução")]
        public string NmTypeSolution { get; set; }

    }
}