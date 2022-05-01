// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Refugee.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: "v1", 
                    info: new OpenApiInfo {
                            Title = "Refugee.Core.Api",
                            Version = "v1" }
                    );
            });
        }

        public void Configure(
            IApplicationBuilder applicationBuilder, 
            IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
                applicationBuilder.UseSwagger();
                
                applicationBuilder.UseSwaggerUI(options => 
                    options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                        name: "Refugee.Core.Api v1"));
            }

            applicationBuilder.UseHttpsRedirection() ;
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}