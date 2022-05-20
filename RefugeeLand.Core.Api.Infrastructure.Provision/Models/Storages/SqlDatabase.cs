using Microsoft.Azure.Management.CosmosDB.Fluent;

namespace RefugeeLand.Core.Api.Infrastructure.Provision.Models.Storages
{
    public class SqlDatabase
    {
        public string ConnectionString { get; set; }
        public ISqlDatabase Database { get; set; }
    }
}
