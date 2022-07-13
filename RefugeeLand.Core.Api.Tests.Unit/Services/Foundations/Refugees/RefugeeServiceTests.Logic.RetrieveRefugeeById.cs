// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Refugees
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public async void ShouldRetrieveRefugeeByIdAsync()
        {
            //given
            Refugee someRefugee = CreateRandomRefugee();
            Refugee storageRefugee = someRefugee;
            Refugee expectedRefugee = someRefugee.DeepClone();

            this.storageBrokerMock.Setup(broker => 
                broker.SelectRefugeeByIdAsync(someRefugee.Id))
                    .ReturnsAsync(storageRefugee);

            //when
            IQueryable<Refugee> actualRefugee =
                this.refugeeService.RetrieveRefugeeByIdAsync();

            //then
            actualRefugee.Should().BeEquivalentTo(expectedRefugee);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeByIdAsync(),
                    Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
