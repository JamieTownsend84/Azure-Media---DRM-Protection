using VideoPlayer.Models;
using VideoPlayer.Repository;
using VideoPlayer.Repository.Interfaces;
using VideoPlayer.Services;
using VideoPlayer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IMediaService, MediaService>();
builder.Services.AddTransient<IAzureMediaServicesRepository, AzureMediaServicesRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var azureMediaServicesConfiguration = builder.Configuration.GetSection("AzureMediaServices").Get<AzureMediaServicesConfiguration>();
builder.Services.AddSingleton(azureMediaServicesConfiguration);
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();