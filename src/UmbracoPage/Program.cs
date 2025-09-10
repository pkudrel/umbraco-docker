using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace UmbracoPage;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Resolve Umbraco's DataDirectory: /app/umbraco/Data
        var dataDir = Path.Combine(builder.Environment.ContentRootPath, "umbraco", "Data");

        // (Optional but recommended) make the |DataDirectory| token resolve here ASAP
        AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

        // Ensure /umbraco/Data/db exists right now (idempotent)
        Directory.CreateDirectory(Path.Combine(dataDir, "db"));

        builder.CreateUmbracoBuilder()
            .AddBackOffice()
            .AddWebsite()
            .AddComposers()
            .Build();

        var app = builder.Build();

        await app.BootUmbracoAsync();

        app.UseUmbraco()
            .WithMiddleware(u =>
            {
                u.UseBackOffice();
                u.UseWebsite();
            })
            .WithEndpoints(u =>
            {
                u.UseBackOfficeEndpoints();
                u.UseWebsiteEndpoints();
            });

        await app.RunAsync();
    }
}