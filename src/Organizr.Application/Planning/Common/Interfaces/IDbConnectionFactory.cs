using System.Data;

namespace Organizr.Application.Planning.Common.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create(string connectionString);
    }
}