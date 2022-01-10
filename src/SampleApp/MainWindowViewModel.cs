using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SampleApp;

public class MainWindowViewModel
{
    private readonly MySettings _mySettings;

    public MainWindowViewModel(IOptions<MySettings> mySettings, IConfiguration configuration, ILogger<MainWindowViewModel> logger)
    {
        _mySettings = mySettings.Value;
        logger.LogInformation($"new {nameof(MainWindowViewModel)}");
        logger.LogInformation($"key:foo value:{configuration.GetValue<string>("foo")}");
        logger.LogInformation($"key:key value:{configuration.GetValue<string>("key")}");
    }

    public string Message => _mySettings.StringSetting;
}