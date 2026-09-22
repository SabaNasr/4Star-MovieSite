using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataBase.Context;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<CastMember> CastMembers => Set<CastMember>();
    public DbSet<MovieDescription> MovieDescriptions => Set<MovieDescription>();
    public DbSet<MovieAdditionalInfo> MovieAdditionalInfos => Set<MovieAdditionalInfo>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    public DbSet<BlogComment> BlogComments => Set<BlogComment>();
    public DbSet<Series> Series => Set<Series>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<TVShow> TVShows => Set<TVShow>();
    public DbSet<TVShowEpisode> TVShowEpisodes=> Set<TVShowEpisode>();
    public DbSet<PrivacyPolicy> PrivacyPolicies => Set<PrivacyPolicy>();
    public DbSet<TermsAndConditions> TermsAndConditions => Set<TermsAndConditions>();
    public DbSet<ContactUs> ContactUs => Set<ContactUs>();
    public DbSet<ContactUsCard> ContactUsCards => Set<ContactUsCard>();
    public DbSet<FAQ> FAQs => Set<FAQ>();
    public DbSet<UserQuestion> UserQuestions => Set<UserQuestion>();

    public DbSet<AboutUs> AboutUs => Set<AboutUs>();

    public DbSet<AboutUsComment> AboutUsComments =>Set<AboutUsComment>();

    public DbSet<AboutUsTeamMember> AboutUsTeamMembers =>Set<AboutUsTeamMember>();

    public DbSet<AboutUsSocialLink> AboutUsSocialLinks =>Set<AboutUsSocialLink>();






    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // اعمال تمام Configurationها
        modelBuilder.ApplyConfigurationsFromAssembly(
            GetType().Assembly);

        Seed.SeedData(modelBuilder);


     //حذف نرم
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                && entityType.BaseType == null)
            {
                var parameter = Expression.Parameter(
                    entityType.ClrType,
                    "e");

                var property = Expression.Property(
                    parameter,
                    nameof(BaseEntity.IsDeleted));

                var condition = Expression.Equal(
                    property,
                    Expression.Constant(false));

                var lambda = Expression.Lambda(
                    condition,
                    parameter);

                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(lambda);
            }
        }


        base.OnModelCreating(modelBuilder);
    }


    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    // تاریخ ایجاد خودکار
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                    // هنگام ایجاد رکورد حذف شده نیست
                    entry.Entity.IsDeleted = false;

                    break;


                case EntityState.Modified:

                    // تاریخ آخرین ویرایش
                    entry.Entity.UpdatedAt = DateTime.UtcNow;

                    // جلوگیری از تغییر تاریخ ایجاد
                    entry.Property(x => x.CreatedAt).IsModified = false;

                    break;


                case EntityState.Deleted:

                    // جلوگیری از حذف فیزیکی
                    entry.State = EntityState.Modified;

                    // فعال کردن حذف نرم
                    entry.Entity.IsDeleted = true;

                    // تاریخ حذف
                    entry.Entity.DeletedAt = DateTime.UtcNow;

                    // تاریخ آخرین تغییر
                    entry.Entity.UpdatedAt = DateTime.UtcNow;

                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}