using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barbearia.Models;
using Microsoft.EntityFrameworkCore;


namespace Barbearia.Data
{
    public class BarbeariaContext : DbContext
    {
        public DbSet<Agendamento> Agendamentos {  get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BarbeariaDB;Trusted_Connection=True;");
        }

    }
}
