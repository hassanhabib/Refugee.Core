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
        public void ShouldRetrieveAllRefugeeGroups()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<RefugeeGroup> randomRefugeeGroups = CreateRandomRefugeeGroups(randomDateTime);
            IQueryable<RefugeeGroup> storageRefugeeGroups = randomRefugeeGroups;
            IQueryable<RefugeeGroup> expectedRefugeeGroups = storageRefugeeGroups;

            this.storageBrokerMock.Setup(broker => 
                broker.SelectAllRefugeeGroups())
                    .Returns(storageRefugeeGroups);

            //when
            IQueryable<RefugeeGroup> actualRefugeeGroups =
                this.refugeeGroupService.RetrieveAllRefugeeGroups();

            //then
            actualRefugeeGroups.Should().BeEquivalentTo(expectedRefugeeGroups);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugeeGroups(),
                    Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}