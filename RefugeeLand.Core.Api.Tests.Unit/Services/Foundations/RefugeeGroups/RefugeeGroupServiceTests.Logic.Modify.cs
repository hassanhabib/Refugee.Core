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
        public async Task ShouldModifyRefugeeGroupAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            RefugeeGroup randomRefugeeGroup = CreateRandomModifyRefugeeGroup(randomDateTimeOffset);
            RefugeeGroup inputRefugeeGroup = randomRefugeeGroup;
            RefugeeGroup updatedRefugeeGroup = inputRefugeeGroup.DeepClone();
            RefugeeGroup expectedRefugeeGroup = updatedRefugeeGroup.DeepClone();
            updatedRefugeeGroup.UpdatedDate = updatedRefugeeGroup.UpdatedDate.AddDays(GetRandomNumber());
            expectedRefugeeGroup.UpdatedDate = updatedRefugeeGroup.UpdatedDate;
            
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRefugeeGroupAsync(inputRefugeeGroup))
                    .ReturnsAsync(updatedRefugeeGroup);

            // when
            RefugeeGroup actualRefugeeGroup =
                await this.refugeeGroupService.ModifyRefugeeGroupAsync(inputRefugeeGroup);

            // then
            actualRefugeeGroup.Should().BeEquivalentTo(expectedRefugeeGroup);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(inputRefugeeGroup),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}