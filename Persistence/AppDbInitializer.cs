using System.Threading.Tasks;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence {
  public partial class AppDbInitializer {
    private IServiceScope _serviceScope;
    private string _path;
    private IAppDbContext _context;
    public AppDbInitializer() { }

    public static async Task Initialize(AppDbContext context, IServiceScope serviceScope, string contentPath) {
      var initializer = new AppDbInitializer();
      initializer._serviceScope = serviceScope;
      initializer._path = contentPath;
      initializer._context = context;
      await initializer.SeedAuthEverything(context, serviceScope, contentPath);
      //await initializer.SeedEverything();
    }
  }
}