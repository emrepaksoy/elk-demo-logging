using DemoLogging.Log.LogModel;

namespace DemoLogging.Log
{
    public interface IElasticSearchService<T> where T : class
    {
        public void CheckExistsAndInsertLog(T logModel, string indexName);
       
    }
}
