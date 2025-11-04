using Loja.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text; 

namespace EstoquePerfumes.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<ItemPedido> ItemPedidos { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaDB;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes("123");
            var hashedPassword = sha.ComputeHash(asByteArray);


            modelBuilder.Entity<Funcionario>().HasData(
                new Funcionario
                {
                    Id = 1,
                    Nome = "Administrador",
                    Email = "admin@admin.com",
                    Cargo = "Admin",
                    Telefone = "(00) 00000-0000",
                    Endereco = "Sistema"
                }
            );

            modelBuilder.Entity<Login>().HasData(
                new Login
                {
                    Id = 1,
                    IdFuncionario = 1,
                    Usuario = "admin@admin.com",
                    Senha = Convert.ToBase64String(hashedPassword), 
                }
            );
            modelBuilder.Entity<Pedido>()
                .Property(p => p.DataPedido)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<Pedido>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ItemPedido>()
                .HasOne<Pedido>()
                .WithMany()
                .HasForeignKey(i => i.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPedido>()
                .HasOne<Produto>()
                .WithMany()
                .HasForeignKey(i => i.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pagamento>()
                .HasOne<Pedido>()
                .WithMany()
                .HasForeignKey(p => p.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pagamento>()
                .Property(p => p.DataPagamento)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Estoque>()
                .HasOne<Produto>()
                .WithMany()
                .HasForeignKey(e => e.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}