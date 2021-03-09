using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packer_v2.Models
{
    [Table("WAY_TYPE")]
    public class WayType
    {
        [Key]
        [Column("ID_WAY_TYPE")]
        public Int64 IdWayType { get; set; }

        [Column("IS_WAY_DEV")]
        [Display(Name = "Caminho DEV?")]
        public bool IsWayDev{ get; set; }

        [Column("NM_WAY_TYPE")]
        [Display(Name = "Tipo do Caminho")]
        public string NmProject { get; set; }

        [ForeignKey("Folder")]
        [Column("ID_FOLDER")]
        [Display(Name = "Pasta do Pacote")]
        public Int64 IdFolder { get; set; }

        //propriedades de navegação
        public virtual Folder Folder { get; set; }

    }
}