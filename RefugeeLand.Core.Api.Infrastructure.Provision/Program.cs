using System.Threading.Tasks;
using RefugeeLand.Core.Api.Infrastructure.Provision.Services.Processings;

namespace RefugeeLand.Core.Api.Infrastructure.Provision
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICloudManagementProcessingService cloudManagementProcessingService =
                new CloudManagementProcessingService();

            await cloudManagementProcessingService.ProcessAsync();
        }
    }
}