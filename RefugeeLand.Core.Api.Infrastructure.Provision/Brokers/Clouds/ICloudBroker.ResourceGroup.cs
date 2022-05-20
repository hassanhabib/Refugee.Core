using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace RefugeeLand.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        public partial interface ICloudBroker
        {
            ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName);
            ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName);
            ValueTask DeleteResourceGroupAsync(string resourceGroupName);
        }
    }
}
