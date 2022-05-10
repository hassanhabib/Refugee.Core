// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.Refugees;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Foundations
{
    public partial class RefugeeServiceTests
    {
        [Fact]
        public async Task ShouldAddRefugeeAsync()
        {
            // given
            Refugee randomRefugee = CreateRandomRefugee();
            Refugee inputRefugee = randomRefugee;
            Refugee insertedRefugee = inputRefugee;
            Refugee expectedRefugee = insertedRefugee.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeAsync(inputRefugee))
                    .ReturnsAsync(insertedRefugee);

            // when
            Refugee actualRefugee =
                await this.refugeeService.AddRefugeeAsync(inputRefugee);

            // then
            actualRefugee.Should().BeEquivalentTo(expectedRefugee);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeAsync(inputRefugee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
