// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldAddRefugeeGroupAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup(randomDateTime);
            RefugeeGroup inputRefugeeGroup = randomRefugeeGroup;
            RefugeeGroup insertedRefugeeGroup = inputRefugeeGroup;
            RefugeeGroup expectedRefugeeGroup = insertedRefugeeGroup.DeepClone();

            // Todo: We are going to need this when we build the structural validation
            // this.dateTimeBrokerMock.Setup(broker =>
            //     broker.GetCurrentDateTimeOffset())
            //         .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeGroupAsync(inputRefugeeGroup))
                    .ReturnsAsync(insertedRefugeeGroup);

            // when
            RefugeeGroup actualRefugeeGroup =
                await this.refugeeGroupService.AddRefugeeGroupAsync(inputRefugeeGroup);

            // then
            actualRefugeeGroup.Should().BeEquivalentTo(expectedRefugeeGroup);

            // Todo: We are going to need this when we build the structural validation
            // this.dateTimeBrokerMock.Verify(broker =>
            //     broker.GetCurrentDateTimeOffset(),
            //         Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeGroupAsync(inputRefugeeGroup),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}