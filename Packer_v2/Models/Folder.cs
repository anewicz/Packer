using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packer_v2.Models
{
    [Table("FOLDER")]
    public class Folder
    {
        [Key]
        [Column("ID_FOLDER")]
        public Int64 IdFolder { get; set; }

        [Column("NM_FOLDER")]
        [Display(Name = "Nome da Pasta")]
        public string NmFolder { get; set; }

        [Column("DE_FOLDER")]
        [Display(Name = "Descrição da Pasta")]
        public string DeFolder { get; set; }
    }
}