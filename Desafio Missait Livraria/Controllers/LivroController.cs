using Desafio_Missait_Livraria.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Desafio_Missait_Livraria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly DataContext _context;
        public LivroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Livro>>> Get()
        {
            return await _context.Livros.Include(l => l.Autores).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<List<Livro>>> Create(CriarLivroDto request)
        {
            var novoLivro = new Livro
            {
                ID = Guid.NewGuid(),
                Titulo = request.Titulo,
                SubTitulo = request.SubTitulo,
                Resumo = request.Resumo,
                QtdPaginas= request.QtdPaginas,
                DataPublicacao = request.DataPublicacao,
                Editora= request.Editora,
                Edicao = request.Edicao,
            };
            
            _context.Livros.Add(novoLivro);
            await _context.SaveChangesAsync();

            return Ok(novoLivro);
        }

        [HttpPost("Autor")]
        public async Task<ActionResult<Livro>> AddAutorLivro(AutordoLivroDto request)
        {
            var livro = await _context.Livros
                .Where(l => l.ID == request.IDLivro)
                .Include(l => l.Autores)
                .FirstOrDefaultAsync();

            if (livro == null)
                return NotFound();

            var autor = await _context.Autores.FindAsync(request.IDAutor);
            if (autor == null) 
                return NotFound();

            livro.Autores.Add(autor);
            await _context.SaveChangesAsync();

            return livro;
        }
    }
}
