using Microsoft.EntityFrameworkCore;
using BetDND.Models;
using BetDND.Enums;
using BetDND.Services;

namespace BetDND.Data
{


    public class DataContext : DbContext {
       public DbSet<User> Users { get; set; }
       public DbSet<Bet> Bets { get; set; }
       public DbSet<BetDetail> BetDetails { get; set; }
       public DbSet<Match> Matches { get; set; }
       public DbSet<MainBetOptions> MainBetOptions { get; set; }
       public DbSet<SubBetCategory> SubBetCategories { get; set; }
       public DbSet<SubBetOption> SubBetOptions { get; set; }
       public DbSet<SubBetSelect> SubBetSelects { get; set; }
       private PasswordService passwordService;
       
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        this.passwordService = new PasswordService();
        this.ConfigureModel(modelBuilder);
    }

    private void ConfigureModel(ModelBuilder modelBuilder) {
        modelBuilder.Entity<SubBetOption>()
        .HasOne(subBetOption => subBetOption.SubBetSelectWinner)
        .WithMany()
        .HasForeignKey(subBetOption => subBetOption.SubBetSelectWinnerId)
        .OnDelete(DeleteBehavior.Restrict);
        this.AddSeedData(modelBuilder);
    }

    private void AddSeedData(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>().HasData(
            new User {
                Id = 1,
                NameSurname = "İsmail Bilinmeyensoyad",
                Email = "admin@mail.com",
                Password = this.passwordService.HashPassword("12345678"),
                Balance = 1000,
                IsAdmin = true,
                IsBanned = false
            },
            new User {
                Id = 2,
                NameSurname = "Ajdar Muzcuyev",
                Email = "user@mail.com",
                Password = this.passwordService.HashPassword("12345678"),
                Balance = 1000,
                IsAdmin = false,
                IsBanned = false
            }
        );
        modelBuilder.Entity<Match>().HasData(
            new Match {
                Id = 1,
                Sport = MatchSports.Football,
                Mbs = MBSRule.One,
                HomeTeam = "Galatasaray",
                AwayTeam = "Fenerbahçe",
                Country = "Türkiye",
                CountryCode = "TR",
                Date = "2020-12-20",
                Time = "20:00",
                League = "Süper Lig",
                IsFinished = false
            },
            new Match {
                Id = 2,
                Sport = MatchSports.Football,
                Mbs = MBSRule.One,
                HomeTeam = "Beşiktaş",
                AwayTeam = "Trabzonspor",
                Country = "Türkiye",
                CountryCode = "TR",
                Date = "2020-12-20",
                Time = "20:00",
                League = "Süper Lig",
                IsFinished = true
            }
        );
        modelBuilder.Entity<MainBetOptions>().HasData(
            new MainBetOptions {
                Id = 1,
                HomeOdd = 1.75,
                AwayOdd = 2.30,
                DrawOdd = 3.58,
                MatchId = 1
            },
            new MainBetOptions {
                Id = 2,
                HomeOdd = 2.29,
                AwayOdd = 2.51,
                DrawOdd = 2.55,
                MatchId = 2,
                MainBetWinner = MainBetWinner.Away
            }
        );
        modelBuilder.Entity<Bet>().HasData(
            new Bet {
                Id = 1,
                Status = BetStatus.Pending,
                Amount = 100,
                UserId = 2
            },
            new Bet {
                Id = 2,
                Status = BetStatus.Lost,
                Amount = 100,
                UserId = 2
            }
        );
        modelBuilder.Entity<BetDetail>().HasData(
            new BetDetail {
                Id = 1,
                IsMainBet = true,
                Odd = 3.58,
                BetId = 1,
                MatchId = 1,
                MainBetOptionsType = MainBetOptionsType.Draw
            },
            new BetDetail {
                Id = 2,
                IsMainBet = false,
                Odd = 2.29,
                BetId = 2,
                MatchId = 2,
                SubBetCategoryId = 2,
                SubBetOptionId = 2,
                SubBetSelectId = 3
            }
        );
        modelBuilder.Entity<SubBetCategory>().HasData(
            new SubBetCategory {
                Id = 1,
                Name = "Gol",
                MatchId = 1
            },
            new SubBetCategory {
                Id = 2,
                Name = "Gol",
                MatchId = 2 
            }
        );
        modelBuilder.Entity<SubBetOption>().HasData(
            new SubBetOption {
                Id = 1,
                Name = "2.5 Gol",
                Description = "2.5 Seçeneği",
                SubBetCategoryId = 1
            },

            new SubBetOption {
                Id = 2,
                Name = "2.5 Gol",
                Description = "2.5 Seçeneği",
                SubBetCategoryId = 2
            }
        );
        modelBuilder.Entity<SubBetSelect>().HasData(
            new SubBetSelect {
                Id = 1,
                Name = "Alt",
                Odd = 1.75,
                SubBetOptionId = 1
            },
            new SubBetSelect {
                Id = 2,
                Name = "Üst",
                Odd = 2.30,
                SubBetOptionId = 1
            },

            new SubBetSelect {
                Id = 3,
                Name = "Alt",
                Odd = 2.29,
                SubBetOptionId = 2
            },
            new SubBetSelect {
                Id = 4,
                Name = "Üst",
                Odd = 2.51,
                SubBetOptionId = 2
            }
        );

            
                
    }
        
            
  }
}