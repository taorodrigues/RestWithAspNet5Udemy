using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWithASPNET5.Model.Context;
using RestWithASPNET5.Business;
using RestWithASPNET5.Business.Implementations;
using RestWithASPNET5.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using RestWithASPNET5.Repository.Generic;
using Microsoft.Net.Http.Headers;
using RestWithASPNET5.Hypermedia.Filters;
using RestWithASPNET5.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using RestWithASPNET5.Services;
using RestWithASPNET5.Services.Implementations;
using RestWithASPNET5.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace RestWithASPNET5
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;

      Log.Logger = new LoggerConfiguration()
          .WriteTo.Console()
          .CreateLogger();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var tokenConfigurations = new TokenConfiguration();

      new ConfigureFromConfigurationOptions<TokenConfiguration>(
              Configuration.GetSection("TokenConfigurations")
          )
          .Configure(tokenConfigurations);

      services.AddSingleton(tokenConfigurations);

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = tokenConfigurations.Issuer,
          ValidAudience = tokenConfigurations.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
        };
      });

      services.AddAuthorization(auth =>
      {
        auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                  .RequireAuthenticatedUser().Build());
      });

      services.AddCors(options => options.AddDefaultPolicy(builder =>
      {
        builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
      }));

      services.AddControllers();

      // To use MySQL, uncomment the 2 lines below and intall Nuget package for MySql
      //var connection = Configuration["MySQLConnection:MySQLConnectionString"];
      //services.AddDbContext<MySQLContext>(options => options.UseMySql(connection));

      var connection = Configuration["SQLServerConnection:SQLServerConnectionString"];
      services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(connection));

      //if (Environment.IsDevelopment())
      //{
      //  MigrateDatabase(connection);
      //}

      services.AddMvc(options =>
      {
        options.RespectBrowserAcceptHeader = true;

        options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
        options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
      })
      .AddXmlSerializerFormatters();

      var filterOptions = new HyperMediaFilterOptions();
      filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
      filterOptions.ContentResponseEnricherList.Add(new BookEnricher());

      services.AddSingleton(filterOptions);

      //Versioning API
      services.AddApiVersioning();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
              Title = "REST API's with ASP.NET Core 5 ",
              Version = "v1",
              Description = "API RESTful developed in course 'REST API's From 0 to Azure with ASP.NET Core 5 and Docker'",
              Contact = new OpenApiContact
              {
                Name = "Leandro Costa",
                Url = new Uri("https://github.com/leandrocgsi")
              }
            });
      });

      //Dependency Injection

      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
      services.AddScoped<IBookBusiness, BookBusinessImplementation>();
      services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
      services.AddScoped<IFileBusiness, FileBusinessImplementation>();

      services.AddTransient<ITokenService, TokenService>();

      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPersonRepository, PersonRepository>();

      services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SQLServerContext dataContext)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      dataContext.Database.Migrate();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors();

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "REST API with ASP.NET Core 5 - v1");
      });

      var option = new RewriteOptions();
      option.AddRedirect("^$", "swagger");
      app.UseRewriter(option);

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
      });
    }
  }
}
