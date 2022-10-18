using Microsoft.Extensions.Configuration;
using Nest;

namespace DemoLogging.Log
{
    public class ElasticSearchClientProvider
    {

        public ElasticClient ElasticClient { get; }
        public string ElasticSearchHost { get; set; }

        public ElasticSearchClientProvider(IConfiguration configuration)
        {
            ElasticSearchHost = configuration["ElasticSearchConnectionSettings:url"];
            ElasticClient = CreateClient();
        }

        private ElasticClient CreateClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .DisablePing()
                .DisableDirectStreaming(true)
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

            return new ElasticClient(connectionSettings);
        }

        public ElasticClient CreateClientWithIndex(string defaultIndex)
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .DisablePing()
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false)
                .DefaultIndex(defaultIndex);

            return new ElasticClient(connectionSettings);
        }

    
    }
}
