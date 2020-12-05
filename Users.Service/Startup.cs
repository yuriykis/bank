using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.Service.Authorization.Helpers;
using Users.Service.Messaging.Options;
using Users.Service.Messaging.Sender;
using Users.Service.Messaging.Sender.Create;
using Users.Service.Messaging.Sender.Delete;
using Users.Service.Models;
using Users.Service.Persistence;
using Users.Service.Services;

namespace Users.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<PrimaryContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<UserService>();
            
            services.AddTransient<IUserAccountDeleteSender, UserAccountDeleteSender>();
            services.AddTransient<IUserAccountCreateSender, UserAccountCreateSender>();
            services.AddMediatR(typeof(Startup));

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            // app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            InitializeUsers(app).Wait();
        }
        private async Task InitializeUsers(IApplicationBuilder app)
        {
            UpgradeDatabase(app);
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PrimaryContext>();
                if (context != null)
                {
                    await context.Database.EnsureCreatedAsync();
                    if (!context.Users.Any())
                    {
                        var users = new User[]
                        {
                            new User {Username = "ykis", FirstName = "Yuriy", LastName = "Kis", Password = "password"},
                            new User {Username = "tkielczewski", FirstName = "Tomasz", LastName = "Kiełczewski", Password = "password"},
                            new User {Username = "jkowalski", FirstName = "Jan", LastName = "Kowalski", Password = "password"},
                            new User {Username = "gadamczewski", FirstName = "Grzegorz", LastName = "Adamczewski", Password = "password"},
                            new User {Username = "knowak", FirstName = "Krzysztof", LastName = "Nowak", Password = "password"}
                        };
                        foreach (User u in users)
                        {
                            await context.Users.AddAsync(u);
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
        
        private void UpgradeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PrimaryContext>();
                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
            } 
        }
        
    }
}
