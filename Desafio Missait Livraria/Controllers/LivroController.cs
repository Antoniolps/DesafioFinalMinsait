using Desafio_Missait_Livraria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
            var titulo = await _context.Livros.Where(l => l.Titulo.Equals(request.Titulo)).FirstOrDefaultAsync();

            if (titulo != null)
                return BadRequest("Livro já existe");

            var autorNovo = new Autor();
            var novoLivro = new Livro
            {
                ID = Guid.NewGuid(),
                Titulo = request.Titulo,
                SubTitulo = request.SubTitulo,
                Resumo = request.Resumo,
                QtdPaginas = request.QtdPaginas,
                DataPublicacao = request.DataPublicacao,
                Editora = request.Editora,
                Edicao = request.Edicao
            };

            _context.Livros.Add(novoLivro);
            await _context.SaveChangesAsync();

            var autor = await _context.Autores
                .Where(autor => autor.Nome.Equals(request.autorNome))
                .FirstOrDefaultAsync();
            
            if(autor == null)
            {
                autorNovo = new Autor
                {
                    ID = Guid.NewGuid(),
                    Nome = request.autorNome
                };
                _context.Autores.Add(autorNovo);
                await _context.SaveChangesAsync();
            }
            else
            {
                autorNovo = autor;
            }
            

            var autorLivro = new AutordoLivroDto
            {
                IDLivro = novoLivro.ID,
                IDAutor = autorNovo.ID
            };

            await AddAutorLivro(autorLivro);

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

        [HttpPut]
        public async Task<ActionResult<Livro>> AtualizarLivro(AlterarLivroDto request)
        {
            var livro = await _context.Livros
                .Where(l => l.ID == request.ID)
                .Include(l => l.Autores)
                .FirstOrDefaultAsync();

            if (livro == null)
                return NotFound();

            livro.Titulo = request.Titulo;
            livro.SubTitulo = request.SubTitulo;
            livro.Resumo = request.Resumo;
            livro.QtdPaginas = request.QtdPaginas;
            livro.DataPublicacao = request.DataPublicacao;
            livro.Editora = request.Editora;
            livro.Edicao = request.Edicao;
            await _context.SaveChangesAsync();

            return Ok(livro);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Livro>>> Delete(Guid id)
        {
            var dbLivro = await _context.Livros.FindAsync(id);
            if(dbLivro== null) 
                return NotFound();

            _context.Livros.Remove(dbLivro);
            await _context.SaveChangesAsync();

            return Ok(await _context.Livros.ToListAsync());
        }

        [HttpPost("ApagarAutor")]
        public async Task<ActionResult<Livro>> Delete(AutordoLivroDto request)
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

            livro.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return Ok(livro);
        }



    }
}
