using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfhostIsNullAndLogItAsync()
        {
            // given
            host nullhost = null;

            var nullhostException =
                new NullhostException();

            var expectedhostValidationException =
                new hostValidationException(nullhostException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(nullhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    addhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfhostIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidhost = new host
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidhostException =
                new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.Id),
                values: "Id is required");

            //invalidhostException.AddData(
            //    key: nameof(host.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the host model

            invalidhostException.AddData(
                key: nameof(host.CreatedDate),
                values: "Date is required");

            invalidhostException.AddData(
                key: nameof(host.CreatedByUserId),
                values: "Id is required");

            invalidhostException.AddData(
                key: nameof(host.UpdatedDate),
                values: "Date is required");

            invalidhostException.AddData(
                key: nameof(host.UpdatedByUserId),
                values: "Id is required");

            var expectedhostValidationException =
                new hostValidationException(invalidhostException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(invalidhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    addhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            host randomhost = CreateRandomhost(randomDateTimeOffset);
            host invalidhost = randomhost;

            invalidhost.UpdatedDate =
                invalidhost.CreatedDate.AddDays(randomNumber);

            var invalidhostException = new InvalidhostException();

            invalidhostException.AddData(
                key: nameof(host.UpdatedDate),
                values: $"Date is not the same as {nameof(host.CreatedDate)}");

            var expectedhostValidationException =
                new hostValidationException(invalidhostException);

            // when
            ValueTask<host> addhostTask =
                this.hostService.AddhostAsync(invalidhost);

            hostValidationException actualhostValidationException =
                await Assert.ThrowsAsync<hostValidationException>(
                    addhostTask.AsTask);

            // then
            actualhostValidationException.Should()
                .BeEquivalentTo(expectedhostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedhostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InserthostAsync(It.IsAny<host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}