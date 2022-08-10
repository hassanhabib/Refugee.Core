using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.Hosts;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldModifyHostAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Host randomHost = CreateRandomModifyHost(randomDateTimeOffset);
            Host inputHost = randomHost;
            Host storageHost = inputHost.DeepClone();
            storageHost.UpdatedDate = randomHost.CreatedDate;
            Host updatedHost = inputHost;
            Host expectedHost = updatedHost.DeepClone();
            Guid hostId = inputHost.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateHostAsync(inputHost))
                    .ReturnsAsync(updatedHost);

            // when
            Host actualHost =
                await this.hostService.ModifyHostAsync(inputHost);

            // then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(inputHost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}