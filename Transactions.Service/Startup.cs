using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Transactions.Service.Authorization.Helpers;
using Transactions.Service.Messaging.Options;
using Transactions.Service.Messaging.Sender;
using Transactions.Service.Models;
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
            services.Configure<BankDatabaseSettings>(
                Configuration.GetSection(nameof(BankDatabaseSettings)));

            services.AddSingleton<IBankDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BankDatabaseSettings>>().Value);
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<TransactionService>();
            services.AddTransient<ITransactionUpdateSender, TransactionUpdateSender>();
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
        }
    }
}
