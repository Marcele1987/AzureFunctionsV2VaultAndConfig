using System.Linq;
using FunctionApp1;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: WebJobsStartup(typeof(FunctionStartup))]
namespace FunctionApp1
{

    public class FunctionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }

            var config = configurationBuilder.Build();

            var vaultUrl = config["VaultUrl"];
            var vaultClientId = config["VaultClientId"];
            var vaultClientSecret = config["VaultClientSecret"];

            configurationBuilder.AddAzureKeyVault(vaultUrl, vaultClientId, vaultClientSecret);

            config = configurationBuilder.Build();

            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));
            builder.AddExtension<ConfigProvider>();
        }
    }
}
