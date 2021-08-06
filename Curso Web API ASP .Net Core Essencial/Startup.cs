using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var SQLconexao = Config.GetConnectionString("Default");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(SQLconexao));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddSingleton<IConfiguration>(Config);

            var key = Encoding.ASCII. GetBytes(Config["JwT:Key"]);
               
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            services.AddControllers().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Curso Web API ASP.Net Core Essencial",
                    Version = "v1",
                    Description = "API de cadastro de produtos e categorias de produtos",

                    Contact = new OpenApiContact
                    {
                        Name = "Austregíselo Junior",
                        Email = "austregiselosvj@hotmail.com"
                    },
                });

                var caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                var nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                var caminhoDocumentacao = Path.Combine(caminhoAplicacao, $"{ nomeAplicacao}.xml");
                c.IncludeXmlComments(caminhoDocumentacao);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Curso Web API ASP .Net Core Essencial v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
