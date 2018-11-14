using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp1
{
    internal class ConfigProvider : IExtensionConfigProvider
    {
        private readonly IConfiguration _config;

        public ConfigProvider(IConfiguration config) => _config = config;

        public void Initialize(ExtensionConfigContext context) => context.AddBindingRule<ConfigAttribute>().BindToInput(_ => _config);
    }
}
