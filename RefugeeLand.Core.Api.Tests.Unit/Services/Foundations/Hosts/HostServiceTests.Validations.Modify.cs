using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsNullAndLogItAsync()
        {
            // given
            Host nullHost = null;
            var nullHostException = new NullHostException();

            var expectedHostValidationException =
                new HostValidationException(nullHostException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(nullHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            // then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidHost = new Host
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidHostException = new InvalidHostException();

            invalidHostException.AddData(
                key: nameof(Host.Id),
                values: "Id is required");

            //invalidHostException.AddData(
            //    key: nameof(Host.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Host model

            invalidHostException.AddData(
                key: nameof(Host.CreatedDate),
                values: "Date is required");

            invalidHostException.AddData(
                key: nameof(Host.CreatedByUserId),
                values: "Id is required");

            invalidHostException.AddData(
                key: nameof(Host.UpdatedDate),
                values: "Date is required");

            invalidHostException.AddData(
                key: nameof(Host.UpdatedByUserId),
                values: "Id is required");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);

            // when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);

            //then
            actualHostValidationException.Should().BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}