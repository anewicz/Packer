using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("SOLUTION")]
    public class Solution
    {
        [Key]
        [Column("ID_SOLUTION")]
        public Int64 IdSolution { get; set; }

        [Column("NM_SOLUTION")]
        [Display(Name = "Nome da Solução")]
        public string NmSolution { get; set; }

        [Column("DE_SOLUTION")]
        [Display(Name = "Descrição da Solução")]
        public string DeSolution { get; set; }

        [ForeignKey("TypeSolution")]
        [Column("ID_TYPE_SOLUTION")]
        [Display(Name = "Tipo da Solução")]
        public Int64 IdTypeSolution { get; set; }


        [ForeignKey("Project")]
        [Column("ID_PROJECT")]
        [Display(Name = "Projeto")]
        public Int64 IdProject { get; set; }

        [Column("DT_REGISTER")]
        [Display(Name = "Dt Cadastro")]
        public DateTime DtRegister { get; set; }

        [Column("DT_LAST_MODIFICATION")]
        [Display(Name = "Dt Ultima Modificação")]
        public DateTime DtLastModification { get; set; }


        //propriedades de navegação
        public virtual Project Project { get; set; }
        public virtual TypeSolution TypeSolution { get; set; }
    }
}