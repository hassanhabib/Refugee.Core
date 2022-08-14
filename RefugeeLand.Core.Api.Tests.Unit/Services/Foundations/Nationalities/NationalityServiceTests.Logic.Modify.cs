using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.Nationalities;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.Nationalities
{
    public partial class NationalityServiceTests
    {
        [Fact]
        public async Task ShouldModifyNationalityAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Nationality randomNationality = CreateRandomModifyNationality(randomDateTimeOffset);
            Nationality inputNationality = randomNationality;
            Nationality storageNationality = inputNationality.DeepClone();
            storageNationality.UpdatedDate = randomNationality.CreatedDate;
            Nationality updatedNationality = inputNationality;
            Nationality expectedNationality = updatedNationality.DeepClone();
            Guid nationalityId = inputNationality.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateNationalityAsync(inputNationality))
                    .ReturnsAsync(updatedNationality);

            // when
            Nationality actualNationality =
                await this.nationalityService.ModifyNationalityAsync(inputNationality);

            // then
            actualNationality.Should().BeEquivalentTo(expectedNationality);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateNationalityAsync(inputNationality),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}