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
            //Get the current config and merge it into a new ConfigurationBuilder to keep the old settings
            var configurationBuilder = new ConfigurationBuilder();
            var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }

            //build the config in order to access the appsettings for getting the key vault connection settings
            var config = configurationBuilder.Build();

            var vaultUrl = config["VaultUrl"];
            var vaultClientId = config["VaultClientId"];
            var vaultClientSecret = config["VaultClientSecret"];

            //add the key vault to the configuration builder
            configurationBuilder.AddAzureKeyVault(vaultUrl, vaultClientId, vaultClientSecret);

            //build the config again so it has the key vault provider
            config = configurationBuilder.Build();

            //replace the existing config with the new one
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));

            //add the ConfigProvider if you want to use IConfiguration in your function, the ConfigProvider is just an implementation of IExtensionConfigProvider to give you access to the current IConfiguration
            builder.AddExtension<ConfigProvider>();
        }
    }
}
