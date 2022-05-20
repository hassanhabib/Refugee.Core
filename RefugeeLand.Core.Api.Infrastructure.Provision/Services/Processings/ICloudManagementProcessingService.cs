using System.Threading.Tasks;

namespace RefugeeLand.Core.Api.Infrastructure.Provision.Services.Processings
{
    public interface ICloudManagementProcessingService
    {
        ValueTask ProcessAsync();
    }
}
