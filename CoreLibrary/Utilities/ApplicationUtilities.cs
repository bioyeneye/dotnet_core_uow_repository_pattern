using System.IO;
using Microsoft.Extensions.Configuration;

namespace CoreLibrary.Utilities
{
    public class ApplicationUtilities
    {
        public static IConfigurationRoot GetApplicationConfiguration(string configurationFile)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configurationFile)
                .Build();
        }
    }
}
