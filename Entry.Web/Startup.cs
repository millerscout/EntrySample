using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Entries.Core;
using Bank.Entries.Core.Interfaces;
using Bank.Entries.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Entry.Web
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
            services.AddSwaggerDocument();

            services.AddScoped<ITransferService, TransferService>();
            services.AddSingleton<ITransferRepository, NoDBTransferRepository>();
            services.AddSingleton<IConnection>((serv) =>
            {

                var fact = new ConnectionFactory()
                {
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    HostName = "127.0.0.1"
                };
                return fact.CreateConnection(Contants.ProjectName);

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
