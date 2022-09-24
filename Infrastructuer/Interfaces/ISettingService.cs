using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Domain.Attributes;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using X.PagedList;

namespace Infrastructure.Interfaces {
  public interface ISettingService {
    Setting GetSettingByName(SettingKeys name);
  }

  public class SettingService : ISettingService {
    private readonly IServiceScope _serviceScope;
    private List<Setting> _setting;

    public SettingService(IServiceScopeFactory serviceScope) {
      _serviceScope = serviceScope.CreateScope();
      Task.FromResult(Initialize());
    }

    private async Task Initialize() {
      var context = GetContext();
      _setting = await context.Set<Setting>().ToListAsync();
    }

    private IAppDbContext GetContext() {
      return _serviceScope.ServiceProvider.GetService<IAppDbContext>();
    }

    public Setting GetSettingByName(SettingKeys name) {
      return _setting.FirstOrDefault(s => s.Name == name.GetAttribute<DescribeSettingAttribute>().Key);
    }
  }
}