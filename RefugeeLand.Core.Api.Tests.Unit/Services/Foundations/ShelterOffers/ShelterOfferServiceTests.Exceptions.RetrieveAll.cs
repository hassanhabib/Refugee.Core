// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedShelterOfferStorageException(sqlException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShelterOffers())
                    .Throws(sqlException);

            // when
            Action retrieveAllShelterOffersAction = () =>
                this.shelterOfferService.RetrieveAllShelterOffers();

            ShelterOfferDependencyException actualShelterOfferDependencyException =
                Assert.Throws<ShelterOfferDependencyException>(retrieveAllShelterOffersAction);

            // then
            actualShelterOfferDependencyException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShelterOffers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedShelterOfferServiceException =
                new FailedShelterOfferServiceException(serviceException);

            var expectedShelterOfferServiceException =
                new ShelterOfferServiceException(failedShelterOfferServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShelterOffers())
                    .Throws(serviceException);

            // when
            Action retrieveAllShelterOffersAction = () =>
                this.shelterOfferService.RetrieveAllShelterOffers();

            ShelterOfferServiceException actualShelterOfferServiceException =
                Assert.Throws<ShelterOfferServiceException>(retrieveAllShelterOffersAction);

            // then
            actualShelterOfferServiceException.Should()
                .BeEquivalentTo(expectedShelterOfferServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShelterOffers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}