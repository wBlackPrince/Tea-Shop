using TeaShop.Application.FullTextSearch;

namespace TeaShop.Infrastructure.ElasticSearch;

public class ElasticSearchProvider: ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query)
    {
        throw new NotImplementedException();
    }
}