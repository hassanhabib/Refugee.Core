using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using RefugeeLand.Core.Api.Infrastructure.Provision.Models.Storages;

namespace RefugeeLand.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<ISqlServer> CreateSqlServerAsync(
        string sqlServerName,
        IResourceGroup resourceGroup);

        ValueTask<ISqlDatabase> CreateSqlDatabaseAsync(
            string sqlDatabasename,
            ISqlServer sqlServer);

        SqlDatabaseAccess GetAdminAccess();
    }
}
