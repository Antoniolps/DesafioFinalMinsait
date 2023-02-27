using Desafio_Missait_Livraria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Missait_Livraria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly DataContext _context;
        public AutorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await _context.Autores.Include(a => a.Livros).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<List<Livro>>> Create(CriarAutorDto request)
        {
            LivroController livroController = new LivroController(_context);

            var novoAutor = new Autor();
            var pesquisaAutor = await _context.Autores
                .Where(autor => autor.Nome.Equals(request.Nome))
                .FirstOrDefaultAsync();

         
            if (pesquisaAutor == null)
            {
                novoAutor = new Autor
                {
                    ID = Guid.NewGuid(),
                    Nome = request.Nome
                };
                _context.Autores.Add(novoAutor);
                await _context.SaveChangesAsync();
            }
            else
            {
                novoAutor = pesquisaAutor;
            }

            var associar = new AutordoLivroDto
            {
                IDLivro = request.idLivro,
                IDAutor = novoAutor.ID
            };

            await livroController.AddAutorLivro(associar);

            return Ok(await _context.Livros.Include(l => l.Autores).ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Livro>>> AtualizarAutor(AlterarAutorDto request)
        {
            var autor = await _context.Autores
                .Where(a => a.ID == request.ID)
                .Include(a => a.Livros)
                .FirstOrDefaultAsync();

            if (autor == null)
                return NotFound();

            autor.Nome = request.Nome;
            await _context.SaveChangesAsync();

            return Ok(await _context.Livros.Include(l => l.Autores).ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Livro>>> Delete(Guid id)
        {
            var dbAutor = await _context.Autores.FindAsync(id);
            if (dbAutor == null)
                return NotFound();

            _context.Autores.Remove(dbAutor);
            await _context.SaveChangesAsync();

            return Ok(await _context.Livros.Include(l => l.Autores).ToListAsync());
        }
    }
}
