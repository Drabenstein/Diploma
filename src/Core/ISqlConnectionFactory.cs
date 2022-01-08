using System.Data;

namespace Core;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync();
}