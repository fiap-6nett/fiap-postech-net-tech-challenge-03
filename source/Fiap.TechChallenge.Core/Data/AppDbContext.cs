﻿using Fiap.TechChallenge.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fiap.TechChallenge.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContatoEntity> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TODO Adicione outras configurações de mapeamento, se necessário.
        }
    }
}
