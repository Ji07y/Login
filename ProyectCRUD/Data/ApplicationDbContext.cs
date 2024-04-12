using Microsoft.EntityFrameworkCore;
using ProyectCRUD.Models.Entities;

namespace ProyectCRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Usuarios> Usuarios { get; set; }

    }
}
