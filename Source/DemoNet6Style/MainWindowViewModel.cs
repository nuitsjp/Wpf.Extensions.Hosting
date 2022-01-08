using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemoNet6Style;

public class MainWindowViewModel
{
    //private ILogger<MainWindowViewModel> _logger;
    //private readonly IConfiguration _configuration;
    //private readonly MySettings _mySettings;

    public MainWindowViewModel(IOptions<MySettings> mySettings)
    {
    }

    //public string Message => _mySettings.StringSetting;
    public string Message => "Hello, Generic Host!";
}