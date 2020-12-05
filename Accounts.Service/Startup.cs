using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Accounts.Service.Authorization.Helpers;
using Accounts.Service.Commands;
using Accounts.Service.Handlers;
using Accounts.Service.Messaging.Options;
using Accounts.Service.Messaging.Receiver;
using Accounts.Service.Models;
using Accounts.Service.Persistence;
using Accounts.Service.Queries;
using Accounts.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Accounts.Service
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

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            
            services.AddDbContext<PrimaryContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
                
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(IAccountUpdateService).Assembly);

            services.AddScoped<AccountService>();
            services.AddTransient<IRequestHandler<GetAccountByIdQuery, Account>, GetAccountByIdHandler>();
            services.AddTransient<IRequestHandler<GetAllAccountsQuery, List<Account>>, GetAllAccountsHandler>();
            services.AddTransient<IRequestHandler<UpdateAccountCommand, bool>, UpdateAccountHandler>();
            services.AddTransient<IRequestHandler<DeleteAccountCommand, bool>, DeleteAccountHandler>();
            services.AddTransient<IRequestHandler<GetAccountByUserIdQuery, Account>, GetAccountByUserIdHandler>();
            services.AddTransient<IAccountUpdateService, AccountUpdateService>();
            
            services.AddHostedService<AccountsAmountUpdateReceiver>();
            
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            InitializeAccounts(app).Wait();
        }

        private async Task InitializeAccounts(IApplicationBuilder app)
        {
            UpgradeDatabase(app);
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PrimaryContext>();
                if (context != null)
                {
                    await context.Database.EnsureCreatedAsync();
                    if (!context.Accounts.Any())
                    {
                        var accounts = new Account[]
                        {
                            new Account {}
                        };
                        foreach (Account a in accounts)
                        {
                            await context.Accounts.AddAsync(a);
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
                context?.Database?.Migrate();
            } 
        }
    }
}
