using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWithASPNET5.Model.Context;

namespace RestWithASPNET5.Model
{
  public static class MigrationManager
  {
    public static IHost MigrateDatabase(this IHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        using (var appContext = scope.ServiceProvider.GetRequiredService<SQLServerContext>())
        {
          try
          {
            appContext.Database.Migrate();
          }
          catch (Exception ex)
          {
            //Log errors or do anything you think it's needed
            throw;
          }
        }
      }
      return host;
    }
  }
}
