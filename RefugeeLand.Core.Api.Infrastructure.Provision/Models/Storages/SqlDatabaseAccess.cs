using Microsoft.Azure.Management.Sql.Fluent;

namespace RefugeeLand.Core.Api.Infrastructure.Provision.Models.Storages
{
    public class SqlDatabaseAccess
    {
        public string ConnectionString { get; set; }
        public ISqlDatabase Database { get; set; }
    }
}
