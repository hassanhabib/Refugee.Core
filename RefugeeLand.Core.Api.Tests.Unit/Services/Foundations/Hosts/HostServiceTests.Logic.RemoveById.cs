// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        public async Task ShouldRemoveHostByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputHostId = randomId;
            Host randomHost = CreateRandomHost();
            Host storageHost = randomHost;
            Host expectedInputHost = storageHost;
            Host deletedHost = expectedInputHost;
            Host expectedHost = deletedHost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectHostByIdAsync(inputHostId))
                    .ReturnsAsync(storageHost);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteHostAsync(expectedInputHost))
                    .ReturnsAsync(deletedHost);

            // when
            Host actualHost = await this.hostService
                .RemoveHostByIdAsync(inputHostId);

            // then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectHostByIdAsync(inputHostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(expectedInputHost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}