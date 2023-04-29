// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
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
        public async Task ShouldRetrieveRefugeeGroupByIdAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            RefugeeGroup inputRefugeeGroup = randomRefugeeGroup;
            RefugeeGroup storageRefugeeGroup = randomRefugeeGroup;
            RefugeeGroup expectedRefugeeGroup = storageRefugeeGroup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRefugeeGroupByIdAsync(inputRefugeeGroup.Id))
                    .ReturnsAsync(storageRefugeeGroup);

            // when
            RefugeeGroup actualRefugeeGroup =
                await this.refugeeGroupService.RetrieveRefugeeGroupByIdAsync(inputRefugeeGroup.Id);

            // then
            actualRefugeeGroup.Should().BeEquivalentTo(expectedRefugeeGroup);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(inputRefugeeGroup.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}