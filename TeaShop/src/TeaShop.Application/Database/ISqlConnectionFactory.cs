using System.Data;

namespace TeaShop.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}