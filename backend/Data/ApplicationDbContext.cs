using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicStreamingAPI.Models;

namespace MusicStreamingAPI.Data;

/// <summary>
/// DbContext principal da aplicação
/// </summary>
public class ApplicationDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Plano> Planos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurar relacionamento Usuario -> Plano
        builder.Entity<Usuario>()
            .HasOne(u => u.Plano)
            .WithMany()
            .HasForeignKey(u => u.PlanoId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seed de planos
        builder.Entity<Plano>().HasData(
            new Plano
            {
                Id = 1,
                Nome = "Individual",
                Descricao = "Plano para 1 pessoa",
                Preco = 19.90m,
                BeneficiosJson = System.Text.Json.JsonSerializer.Serialize(new List<string>
                {
                    "Acesso ilimitado a músicas",
                    "Reprodução offline",
                    "Sem anúncios"
                }),
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            },
            new Plano
            {
                Id = 2,
                Nome = "Família",
                Descricao = "Plano para até 6 pessoas",
                Preco = 34.90m,
                BeneficiosJson = System.Text.Json.JsonSerializer.Serialize(new List<string>
                {
                    "Até 6 contas premium",
                    "Acesso ilimitado a músicas",
                    "Reprodução offline",
                    "Sem anúncios",
                    "Controle parental"
                }),
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            },
            new Plano
            {
                Id = 3,
                Nome = "Estudante",
                Descricao = "50% de desconto para estudantes",
                Preco = 9.90m,
                BeneficiosJson = System.Text.Json.JsonSerializer.Serialize(new List<string>
                {
                    "Acesso ilimitado a músicas",
                    "Reprodução offline",
                    "Sem anúncios",
                    "Desconto especial"
                }),
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            }
        );

        // Seed de músicas de exemplo
        builder.Entity<Musica>().HasData(
            new Musica
            {
                Id = 1,
                Titulo = "Billie Jean",
                Artista = "Michael Jackson",
                Album = "Thriller",
                Genero = "Pop",
                Duracao = 294,
                ImagemUrl = "https://via.placeholder.com/300x300/8B008B/FFFFFF?text=Billie+Jean",
                AnoLancamento = 1982,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            },
            new Musica
            {
                Id = 2,
                Titulo = "Bohemian Rhapsody",
                Artista = "Queen",
                Album = "A Night at the Opera",
                Genero = "Rock",
                Duracao = 354,
                ImagemUrl = "https://via.placeholder.com/300x300/DC143C/FFFFFF?text=Bohemian+Rhapsody",
                AnoLancamento = 1975,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            },
            new Musica
            {
                Id = 3,
                Titulo = "Imagine",
                Artista = "John Lennon",
                Album = "Imagine",
                Genero = "Rock",
                Duracao = 183,
                ImagemUrl = "https://via.placeholder.com/300x300/20B2AA/FFFFFF?text=Imagine",
                AnoLancamento = 1971,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            }
        );
    }
}
