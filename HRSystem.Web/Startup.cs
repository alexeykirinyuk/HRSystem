using System.IO;
using HRSystem.Bll;
using HRSystem.Commands.SaveAttribute;
using HRSystem.Commands.SaveEmployee;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Data;
using HRSystem.Infrastructure;
using HRSystem.Queries.AttributeSavingInfo;
using HRSystem.Queries.EmployeeQuery;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace HRSystem.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["Data:connectionString"];
            services.AddDbContext<HrSystemDb>(options => options.UseSqlServer(connectionString));
            services.AddMvc();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<ICreateAttributeService, CreateAttributeService>();

            services.AddScoped<IValidator<SaveEmployeeCommand>, SaveEmployeeCommandValidator>();
            services.AddScoped<IValidator<SaveAttributeCommand>, SaveAttributeCommandValidator>();
            services.AddScoped<IValidator<AttributeSavingInfoQuery>, AttributeSavingInfoQueryValidator>();
            
            services.AddMediatR(typeof(EmployeeQuery), typeof(SaveEmployeeCommand));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/vendor")
            });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}