// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Refugees
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllRefugees()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Refugee> randomRefugees = CreateRandomRefugees(randomDateTime);
            IQueryable<Refugee> storageRefugees = randomRefugees;
            IQueryable<Refugee> expectedRefugees = storageRefugees;

            this.storageBrokerMock.Setup(broker => 
                broker.SelectAllRefugees())
                    .Returns(storageRefugees);

            //when
            IQueryable<Refugee> actualRefugees =
                this.refugeeService.RetrieveAllRefugees();

            //then
            actualRefugees.Should().BeEquivalentTo(expectedRefugees);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRefugees(),
                    Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
