using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebRecreo.Models;

namespace WebRecreo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
    }
}
