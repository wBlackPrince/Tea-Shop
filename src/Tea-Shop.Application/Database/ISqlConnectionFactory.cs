using System.Data;

namespace Tea_Shop.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}