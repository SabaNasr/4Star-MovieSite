using DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Service.Admin.Blog.BlogCategory;
using Service.Admin.Blog.BlogComment;
using Service.Admin.Blog.BlogPost;
using Service.Admin.Content.AboutUs;
using Service.Admin.Content.AboutUsComment;
using Service.Admin.Content.AboutUsSocialLink;
using Service.Admin.Content.AboutUsTeamMember;
using Service.Admin.Content.ContactUs;
using Service.Admin.Content.ContactUsCard;
using Service.Admin.Content.FAQ;
using Service.Admin.Content.PrivacyPolicies;
using Service.Admin.Content.TermsAndConditions;
using Service.Admin.Content.UserQuestion;
using Service.Admin.Movies.CastMember;
using Service.Admin.Movies.Genre;
using Service.Admin.Movies.Language;
using Service.Admin.Movies.MovieAdditionalInfo;
using Service.Admin.Movies.MovieDescription;
using Service.Admin.Movies.Review;
using Service.Admin.TvSeries.Episodes;
using Service.Admin.TvSeries.Seasons;
using Service.Admin.TvSeries.Series;
using Service.Admin.TvSeries.SeriesAdditionalInfo;
using Service.Admin.TvSeries.SeriesDescription;
using Service.Admin.TvSeries.SeriesReview;
using Service.Admin.TVShows.TvShow;
using Service.Admin.TVShows.TVShowAdditionalInfo;
using Service.Admin.TVShows.TVShowDescription;
using Service.Admin.TVShows.TVShowEpisode;
using Service.Admin.TVShows.TVShowReview;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IMovieService,MovieService>();
builder.Services.AddScoped<IGenreService,GenreService>();
builder.Services.AddScoped<ILanguageService,LanguageService>();
builder.Services.AddScoped<ICastMemberService,CastMemberService>();
builder.Services.AddScoped<IReviewService,ReviewService>();
builder.Services.AddScoped<IMovieDescriptionService,MovieDescriptionService>();
builder.Services.AddScoped<IMovieAdditionalInfoService,MovieAdditionalInfoService>();
builder.Services.AddScoped<IBlogPostService,BlogPostService>();
builder.Services.AddScoped<IBlogCategoryService,BlogCategoryService>();
builder.Services.AddScoped<IBlogCommentService,BlogCommentService>();
builder.Services.AddScoped<ISeriesService,SeriesService>();
builder.Services.AddScoped<ISeasonService,SeasonService>();
builder.Services.AddScoped<IEpisodeService,EpisodeService>();
builder.Services.AddScoped<ISeriesDescriptionService,SeriesDescriptionService>();
builder.Services.AddScoped<ISeriesAdditionalInfoService,SeriesAdditionalInfoService>();
builder.Services.AddScoped<ISeriesReviewService,SeriesReviewService>();
builder.Services.AddScoped<ITVShowService,TVShowService>();
builder.Services.AddScoped<ITVShowEpisodeService,TVShowEpisodeService>();
builder.Services.AddScoped<ITVShowDescriptionService,TVShowDescriptionService>();
builder.Services.AddScoped<ITVShowAdditionalInfoService,TVShowAdditionalInfoService>();
builder.Services.AddScoped<ITVShowReviewService,TVShowReviewService>();
builder.Services.AddScoped<IPrivacyPolicyService,PrivacyPolicyService>();
builder.Services.AddScoped<ITermsAndConditionsService,TermsAndConditionsService>();
builder.Services.AddScoped<IContactUsService,ContactUsService>();
builder.Services.AddScoped<IContactUsCardService,ContactUsCardService>();
builder.Services.AddScoped<IFAQService,FAQService>();
builder.Services.AddScoped<IUserQuestionService,UserQuestionService>();
builder.Services.AddScoped<IAboutUsService,AboutUsService>();
builder.Services.AddScoped<IAboutUsCommentService,AboutUsCommentService>();
builder.Services.AddScoped<IAboutUsTeamMemberService,AboutUsTeamMemberService>();
builder.Services.AddScoped<IAboutUsSocialLinkService,AboutUsSocialLinkService>();





builder.Services.AddDbContext<MyContext>(option =>
{

    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});//
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
