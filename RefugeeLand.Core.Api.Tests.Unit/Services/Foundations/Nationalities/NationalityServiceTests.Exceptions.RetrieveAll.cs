// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedNationalityStorageException(sqlException);

            var expectedNationalityDependencyException =
                new NationalityDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllNationalities())
                    .Throws(sqlException);

            // when
            Action retrieveAllNationalitiesAction = () =>
                this.nationalityService.RetrieveAllNationalities();

            NationalityDependencyException actualNationalityDependencyException =
                Assert.Throws<NationalityDependencyException>(retrieveAllNationalitiesAction);

            // then
            actualNationalityDependencyException.Should()
                .BeEquivalentTo(expectedNationalityDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllNationalities(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedNationalityDependencyException))),
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

            var failedNationalityServiceException =
                new FailedNationalityServiceException(serviceException);

            var expectedNationalityServiceException =
                new NationalityServiceException(failedNationalityServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllNationalities())
                    .Throws(serviceException);

            // when
            Action retrieveAllNationalitiesAction = () =>
                this.nationalityService.RetrieveAllNationalities();

            NationalityServiceException actualNationalityServiceException =
                Assert.Throws<NationalityServiceException>(retrieveAllNationalitiesAction);

            // then
            actualNationalityServiceException.Should()
                .BeEquivalentTo(expectedNationalityServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllNationalities(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedNationalityServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}