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
        public async Task ShouldRetrieveRefugeeByIdAsync()
        {
            // given
            Refugee randomRefugee = CreateRandomRefugee();
            Refugee inputRefugee = randomRefugee;
            Refugee storageRefugee = randomRefugee;
            Refugee expectedRefugee = storageRefugee.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRefugeeByIdAsync(inputRefugee.Id))
                    .ReturnsAsync(storageRefugee);
            
            // when
            Refugee actualRefugee = 
                await this.refugeeService.RetrieveRefugeeByIdAsync(inputRefugee.Id);


            // then
            actualRefugee.Should().BeEquivalentTo(expectedRefugee);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeByIdAsync(inputRefugee.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
