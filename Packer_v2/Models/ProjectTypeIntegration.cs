using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("PROJECT_TYPE_INTEGRATION")]
    public class ProjectTypeIntegration
    {
        [Key]
        [Column("ID_PROJECT_TYPE_INTEGRATION")]
        public Int64 IdProjectTypeIntegration { get; set; }

        [Column("NM_TYPE_INTEGRATION")]
        [Display(Name = "Nome da Integração")]
        public string NmTypeIntegration { get; set; }

        [Column("DE_INTEGRATION")]
        [Display(Name = "Descrição Integração")]
        public string DeIntegration { get; set; }

        [ForeignKey("TypeIntegration")]
        [Column("ID_TYPE_INTEGRATION")]
        [Display(Name = "Tipo da Integração")]
        public Int64 IdTypeIntegration { get; set; }

        [ForeignKey("Project")]
        [Column("ID_PROJECT")]
        [Display(Name = "Id Project")]
        public Int64 IdProject { get; set; }


        //propriedades de navegação
        public virtual Project Project { get; set; }
        public virtual TypeIntegration TypeIntegration { get; set; }
    }
}