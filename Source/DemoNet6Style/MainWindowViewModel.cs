using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemoNet6Style;

public class MainWindowViewModel
{
    private ILogger<MainWindowViewModel> _logger;
    private readonly IConfiguration _configuration;
    private readonly MySettings _mySettings;

    public MainWindowViewModel(IOptions<MySettings> mySettings, IConfiguration configuration, ILogger<MainWindowViewModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _mySettings = mySettings.Value;
        var foo = _configuration.GetValue<string>("foo");
        var key = _configuration.GetValue<string>("key");
        logger.LogInformation($"new {nameof(MainWindowViewModel)}");
    }

    public string Message => _mySettings.StringSetting;
}