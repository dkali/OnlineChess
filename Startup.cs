using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineChess.Data;
using Microsoft.AspNetCore.ResponseCompression;
using OnlineChess.Server.Hubs;
using EFChessData;

namespace OnlineChess
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // Create your database on startup
            using (var client = new ChessDataContext())
            {
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<LobbyHub>();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<LobbyService>();
            services.AddSingleton<SQLiteDataService>();
            services.AddScoped<PlayerDataService>();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            // Add your context to your services
            services.AddEntityFrameworkSqlite().AddDbContext<ChessDataContext>();
            services.AddScoped<NotifierService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // options.ClientId = Configuration["Google:ClientId"];
            // options.ClientSecret = Configuration["Google:ClientSecret"];

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<LobbyHub>("/lobbyhub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
