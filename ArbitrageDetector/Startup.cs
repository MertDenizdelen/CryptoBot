using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitrageDetector.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArbitrageDetector
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
            services.AddMvc();

            // Register arbitrageConfiguration from appsettings.json
            services.AddTransient((serviceProvider) => Configuration.GetSection("ArbitrageConfiguration").Get<ArbitrageConfiguration>());

            // Register InMemoryPriceRepository to hold prices in memory
            services.AddSingleton<IInMemoryPriceRepository, InMemoryPriceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
