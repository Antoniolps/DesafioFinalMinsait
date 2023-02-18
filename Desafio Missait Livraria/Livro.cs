using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Desafio_Missait_Livraria
{
    public class Livro
    {
        [Key]
        public Guid ID { get; set; }
        [Column(TypeName = "varchar(100)")]
        [Required]
        public string Titulo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string SubTitulo { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Resumo { get; set; }
        [Column(TypeName = "int")]
        [Required]
        public int QtdPaginas { get; set; }
        [Column(TypeName = "date")]
        [Required]
        public DateTime DataPublicacao { get; set; }
        [Column(TypeName = "varchar(150)")]
        [Required]
        public string Editora { get; set; }
        [Column(TypeName = "int")]
        [MaxLength(2)]
        public int Edicao { get; set; }
        [ForeignKey(nameof(Autor))]
        public List<Autor> Autores { get; set; }



    }
}
