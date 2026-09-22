using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
namespace DataBase.Context;

public static class Seed
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        SeedGenres(modelBuilder);
        SeedLanguages(modelBuilder);
    }

    private static void SeedGenres(ModelBuilder modelBuilder)
    {
        var createdAt = new DateTime(2026, 1, 1);

        modelBuilder.Entity<Genre>().HasData(
            new Genre
            {
                Id = 1,
                Name = "اکشن",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 2,
                Name = "کمدی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 3,
                Name = "درام",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 4,
                Name = "ترسناک",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 5,
                Name = "علمی تخیلی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 6,
                Name = "عاشقانه",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 7,
                Name = "جنایی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 8,
                Name = "ماجراجویی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 9,
                Name = "فانتزی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 10,
                Name = "مستند",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 11,
                Name = "تاریخی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 12,
                Name = "جنگی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 13,
                Name = "خانوادگی",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Genre
            {
                Id = 14,
                Name = "انیمیشن",
                CreatedAt = createdAt,
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            }
        );
    }
    // ==================================================
    // Seed - Languages
    // ==================================================

    private static void SeedLanguages(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasData(
            new Language
            {
                Id = 1,
                Name = "فارسی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 2,
                Name = "انگلیسی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 3,
                Name = "فرانسوی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 4,
                Name = "آلمانی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 5,
                Name = "اسپانیایی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 6,
                Name = "ایتالیایی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 7,
                Name = "ژاپنی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 8,
                Name = "کره‌ای",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 9,
                Name = "چینی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 10,
                Name = "عربی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 11,
                Name = "ترکی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            },
            new Language
            {
                Id = 12,
                Name = "روسی",
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true,
                IsDeleted = false,
                IsArchived = false
            }
        );
    }
}