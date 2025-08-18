using Tea_Shop.Application.FullTextSearch;

namespace Tea_Shop.Infrastructure.ElasticSearch;

public class ElasticSearchProvider: ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query)
    {
        throw new NotImplementedException();
    }
}