﻿using FileTaskApiCore.Services;
using FileTaskApiCore.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileTaskApiCore
{
    public class Startup
    {
        private const string ANGULAR_ALLOW_POLICY_NAME = "AngularFront";
        private const string FILE_SERVICE_CONFIG_SECTION_NAME = "FileServiceSettings";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(ANGULAR_ALLOW_POLICY_NAME,
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                        });
            });

            var fileServiceSettingsSection = Configuration.GetSection(FILE_SERVICE_CONFIG_SECTION_NAME);
            services.Configure<FileServiceSettings>(fileServiceSettingsSection);
            services.AddScoped<IFileService, FileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(ANGULAR_ALLOW_POLICY_NAME);
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endponts =>
            {
                endponts.MapControllers();
            });
        }
    }
}
