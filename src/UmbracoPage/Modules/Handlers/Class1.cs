using System.IO;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.Notifications;

namespace UmbracoPage.Modules.Handlers;

public sealed class EnsureDataSubdir : INotificationHandler<UmbracoApplicationStartingNotification>
{
    private readonly IHostingEnvironment _env;
    private readonly ILogger<EnsureDataSubdir> _log;

    public EnsureDataSubdir(IHostingEnvironment env, ILogger<EnsureDataSubdir> log)
    {
        _env = env;
        _log = log;
    }

    public void Handle(UmbracoApplicationStartingNotification n)
    {
        // Physical path to umbraco/Data

        var dataRoot = _env.MapPathContentRoot(Constants.SystemDirectories.Data);
        var myDir = Path.Combine(dataRoot, "db"); // e.g. /umbraco/Data/db
        if (Directory.Exists(myDir) == false)
            Directory.CreateDirectory(myDir);
        _log.LogInformation("Ensured data sqlight db dir exists at {Path}", myDir);
    }
}

public class Register : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, EnsureDataSubdir>();
    }
}