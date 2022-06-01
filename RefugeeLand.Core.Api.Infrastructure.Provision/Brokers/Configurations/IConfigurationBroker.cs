using RefugeeLand.Core.Api.Infrastructure.Provision.Models.Configurations;

namespace RefugeeLand.Core.Api.Infrastructure.Provision.Brokers.Configurations
{
    public interface IConfigurationBroker
    {
        CloudManagementConfiguration GetConfigurations();
    }
}
