using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys
{
    [Table("Tb_Categorias")]
    public class CategoriaEntity
    {
        [Key]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(80)]
        public string Nome { get; set; }
        [Required]
        [Url(ErrorMessage ="A string deve ter o formato de URL")]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }
        public ICollection<ProdutoEntity> Produtos { get; set; }
    }
}
