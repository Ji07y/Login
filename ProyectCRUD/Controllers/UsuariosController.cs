using Microsoft.AspNetCore.Mvc;
using ProyectCRUD.Models;
using ProyectCRUD.Data;
using ProyectCRUD.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace ProyectCRUD.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public UsuariosController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddUsuariosViewModel viewModel)
        {
            var usuarios = new Usuarios
            {
                usuario = viewModel.usuario,
                clave = viewModel.clave
                
            };
            await dbContext.Usuarios.AddAsync(usuarios);

            await dbContext.SaveChangesAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var usuarios = await dbContext.Usuarios.ToListAsync();
            return View(usuarios);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var usuarios = await dbContext.Usuarios.FindAsync(Id);
            return View(usuarios);  
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Usuarios viewModel)

        {
            var usuarios = await dbContext.Usuarios.FindAsync(viewModel.Id);

            if (usuarios is not null)
            {
                usuarios.usuario = viewModel.usuario;
                usuarios.clave = viewModel.clave;

                await dbContext.SaveChangesAsync();
            }

                   
            return RedirectToAction("List", "Usuarios");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Usuarios viewModel)
        {
            var usuarios = await dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(X => X.Id == viewModel.Id);
            if (usuarios is not null)
            {
                dbContext.Usuarios.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Usuarios");
        }
    }
}
