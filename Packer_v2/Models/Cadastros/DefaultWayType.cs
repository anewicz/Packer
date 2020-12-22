using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    [Table("DEFAULT_WAY_TYPE")]
    public class DefaultWayType
    {
        [Key]
        [Column("ID_DEFAULT_WAY_TYPE")]
        public Int64 IdDefaultWayType { get; set; }

        [Column("NM_DEFAULT_WAY_PATCH")]
        [Display(Name = "Nome do Caminho")]
        public string NmDefaultWayPatch { get; set; }

        [Column("DEFAULT_WAY_PATH")]
        [Display(Name = "Parte Padrão do Caminho")]
        public string DefaultWayPath { get; set; }

        [Column("REMOTE_IP")]
        [Display(Name = "Ip da Remota")]
        public string RemoteIp { get; set; }

        [ForeignKey("TypeSolution")]
        [Column("ID_TYPE_SOLUTION")]
        [Display(Name = "Tipo de Solução")]
        public Int64 IdTypeSolution { get; set; }

        [ForeignKey("WayType")]
        [Column("ID_WAY_TYPE")]
        [Display(Name = "Tipo do Caminho")]
        public Int64 IdWayType { get; set; }

        //propriedades de navegação
        public virtual TypeSolution TypeSolution { get; set; }
        public virtual WayType WayType { get; set; }

    }
}