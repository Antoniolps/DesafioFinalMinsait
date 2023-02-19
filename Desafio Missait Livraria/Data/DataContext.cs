using Desafio_Missait_Livraria.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Missait_Livraria.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
    }
}
