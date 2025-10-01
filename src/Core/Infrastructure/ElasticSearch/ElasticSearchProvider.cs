using Shared.FullTextSearch;

namespace ElasticSearch;

public class ElasticSearchProvider: ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query)
    {
        throw new NotImplementedException();
    }
}