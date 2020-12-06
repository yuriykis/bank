using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transactions.Service.Authorization.Helpers;
using Transactions.Service.Messaging.Options;
using Transactions.Service.Messaging.Sender;
using Transactions.Service.Models;
using Transactions.Service.Persistence;
using Transactions.Service.Services;

namespace Transactions.Service
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
            services.AddOptions();
            services.AddDbContext<PrimaryContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<TransactionService>();
            services.AddTransient<ITransactionUpdateSender, TransactionUpdateSender>();
            services.AddMediatR(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

            InitializeTransactions(app).Wait();
        }

        private async Task InitializeTransactions(IApplicationBuilder app)
        {
            UpgradeDatabase(app);
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PrimaryContext>();
                if (context != null)
                {
                    await context.Database.EnsureCreatedAsync();
                    if (!context.Transactions.Any())
                    {
                        var transactions = new Transaction[]
                        {
                            new Transaction {ReceiverAccountId = "1", SenderAccountId = "2", Amount = 200}
                        };
                        foreach (Transaction t in transactions)
                        {
                            await context.Transactions.AddAsync(t);
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
