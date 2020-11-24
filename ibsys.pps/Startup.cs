using IBSYS.PPS.Config;
using IBSYS.PPS.Models;
using IBSYS.PPS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace IBSYS.PPS
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
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy", builder => {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddControllers(options => 
            { 
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.Add(new XmlSerializerCustomOutputFormatter());
            })
                .AddXmlSerializerFormatters()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddEntityFrameworkNpgsql().AddDbContext<IbsysDatabaseContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("IbsysDatabaseContext"));
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<DataService>();

            // Adding the Swagger API to the project
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IBSYS 2 Planning Tool API",
                    Description = "Description of all public endpoints for further development",
                    Contact = new OpenApiContact
                    {
                        Name = "Julian Bücher",
                        Email = "buju1023@hs-karlsruhe.de"
                    },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable the Swagger Middleware to serve generated JSON
            app.UseSwagger(context =>
            {
                context.PreSerializeFilters.Add((swagger, httpRequest) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{httpRequest.Scheme}://{httpRequest.Host.Value}/backend" }
                    };
                });
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable Swagger Middleware to serve the UI
                app.UseSwaggerUI(context =>
                {
                    context.SwaggerEndpoint("/swagger/v1/swagger.json", "IBSYS Planning API");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                
                // Enable Swagger Middleware to serve the UI
                app.UseSwaggerUI(context =>
                {
                    context.SwaggerEndpoint("/swagger/v1/swagger.json", "IBSYS Planning API");
                    context.RoutePrefix = string.Empty;
                });
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}")
                .RequireCors("CorsPolicy");
            });
        }
    }
}