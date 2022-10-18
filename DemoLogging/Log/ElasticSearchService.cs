using DemoLogging.Log.LogModel;
using Nest;

namespace DemoLogging.Log
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {
        ElasticSearchClientProvider _provider;
        ElasticClient _client;
        public ElasticSearchService(ElasticSearchClientProvider provider)
        {
            _provider = provider;
            _client = _provider.ElasticClient;
        }

        public void CheckExistsAndInsertLog(T logModel, string indexName)
        {
            if (!_client.Indices.Exists(indexName).Exists) 
            {
                var newIndexName =  indexName + System.DateTime.Now.Ticks;
                var indexSettings = new IndexSettings();
                indexSettings.NumberOfReplicas = 1;
                indexSettings.NumberOfShards = 3;


                var response = _client.Indices.Create(newIndexName, index =>
                   index.Map<T>(m => m.AutoMap()
                          )
                  .InitializeUsing(new IndexState() { Settings = indexSettings })
                  .Aliases(a => a.Alias(indexName)));


            }

            IndexResponse responseIndex = _client.Index<T>(logModel, idx => idx.Index(indexName));
            int a = 0;
        }

        public IReadOnlyCollection<ErrorLogModel> SearchErrorLog()
        {
            throw new NotImplementedException();
        }
    }
}
