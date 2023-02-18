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
        public async Task<ActionResult<List<Autor>>> Create(CriarAutorDto request)
        {
            var novoAutor = new Autor
            {
                ID = Guid.NewGuid(),
                Nome = request.nome
            };

            _context.Autores.Add(novoAutor);
            await _context.SaveChangesAsync();

            return Ok(novoAutor);
        }
    }
}
