using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Desafio_Missait_Livraria.Models
{
    public class Autor
    {
        [Key]
        public Guid ID { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Nome { get; set; }

        [ForeignKey(nameof(Livro))]
        [JsonIgnore]
        public List<Livro> Livros { get; set; }

    }
}
