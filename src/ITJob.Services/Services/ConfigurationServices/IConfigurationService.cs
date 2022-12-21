using ITJob.Services.ViewModels.Configuration;

namespace ITJob.Services.Services.ConfigurationServices;

public interface IConfigurationService
{
    public string Configuration(CongfigurationModel configurationModel);
    public Task<CongfigurationModel> GetConfiguration();
}