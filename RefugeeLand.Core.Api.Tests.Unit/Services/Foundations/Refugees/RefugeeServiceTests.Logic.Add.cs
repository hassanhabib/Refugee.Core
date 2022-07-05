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
        public async Task ShouldAddRefugeeAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Refugee randomRefugee = CreateRandomRefugee(randomDateTime);
            Refugee inputRefugee = randomRefugee;
            Refugee insertedRefugee = inputRefugee;
            Refugee expectedRefugee = insertedRefugee.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRefugeeAsync(inputRefugee))
                    .ReturnsAsync(insertedRefugee);

            // when
            Refugee actualRefugee =
                await this.refugeeService.AddRefugeeAsync(inputRefugee);

            // then
            actualRefugee.Should().BeEquivalentTo(expectedRefugee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRefugeeAsync(inputRefugee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
