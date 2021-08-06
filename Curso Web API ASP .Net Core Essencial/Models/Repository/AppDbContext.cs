using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Models.Repository
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CategoriaEntity> Tb_Categorias { get; private set; }
        public DbSet<ProdutoEntity> Tb_Produtos { get; private set; }
    }
}
