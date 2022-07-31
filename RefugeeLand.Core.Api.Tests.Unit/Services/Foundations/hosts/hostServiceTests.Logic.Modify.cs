using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public async Task ShouldModifyhostAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            host randomhost = CreateRandomModifyhost(randomDateTimeOffset);
            host inputhost = randomhost;
            host storagehost = inputhost.DeepClone();
            storagehost.UpdatedDate = randomhost.CreatedDate;
            host updatedhost = inputhost;
            host expectedhost = updatedhost.DeepClone();
            Guid hostId = inputhost.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelecthostByIdAsync(hostId))
                    .ReturnsAsync(storagehost);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatehostAsync(inputhost))
                    .ReturnsAsync(updatedhost);

            // when
            host actualhost =
                await this.hostService.ModifyhostAsync(inputhost);

            // then
            actualhost.Should().BeEquivalentTo(expectedhost);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelecthostByIdAsync(inputhost.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatehostAsync(inputhost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}