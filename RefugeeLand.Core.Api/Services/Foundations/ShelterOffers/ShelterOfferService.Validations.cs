// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferService
    {
        private void ValidateShelterOfferOnAdd(ShelterOffer shelterOffer)
        {
            ValidateShelterOfferIsNotNull(shelterOffer);

            Validate(
                (Rule: IsInvalid(shelterOffer.Id), Parameter: nameof(ShelterOffer.Id)),

                (Rule: IsInvalid(shelterOffer.StartDate), Parameter: nameof(ShelterOffer.StartDate)),
                (Rule: IsInvalid(shelterOffer.EndDate), Parameter: nameof(ShelterOffer.EndDate)),
                (Rule: IsInvalid(shelterOffer.Status), Parameter: nameof(ShelterOffer.Status)),
                (Rule: IsInvalid(shelterOffer.ShelterId), Parameter: nameof(ShelterOffer.ShelterId)),
                (Rule: IsInvalid(shelterOffer.HostId), Parameter: nameof(ShelterOffer.HostId)),

                (Rule: IsInvalid(shelterOffer.CreatedDate), Parameter: nameof(ShelterOffer.CreatedDate)),
                (Rule: IsInvalid(shelterOffer.CreatedByUserId), Parameter: nameof(ShelterOffer.CreatedByUserId)),
                (Rule: IsInvalid(shelterOffer.UpdatedDate), Parameter: nameof(ShelterOffer.UpdatedDate)),
                (Rule: IsInvalid(shelterOffer.UpdatedByUserId), Parameter: nameof(ShelterOffer.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: shelterOffer.UpdatedDate,
                    secondDate: shelterOffer.CreatedDate,
                    secondDateName: nameof(ShelterOffer.CreatedDate)),
                Parameter: nameof(ShelterOffer.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: shelterOffer.UpdatedByUserId,
                    secondId: shelterOffer.CreatedByUserId,
                    secondIdName: nameof(ShelterOffer.CreatedByUserId)),
                Parameter: nameof(ShelterOffer.UpdatedByUserId)),

                (Rule: IsNotRecent(shelterOffer.CreatedDate), Parameter: nameof(ShelterOffer.CreatedDate)),
                
                (Rule: IsLater(
                    firstDate: shelterOffer.StartDate,
                    secondDate: shelterOffer.EndDate,
                    secondDateName: nameof(ShelterOffer.EndDate)),
                Parameter: nameof(ShelterOffer.StartDate)));
        }

        private void ValidateShelterOfferOnModify(ShelterOffer shelterOffer)
        {
            ValidateShelterOfferIsNotNull(shelterOffer);

            Validate(
                (Rule: IsInvalid(shelterOffer.Id), Parameter: nameof(ShelterOffer.Id)),

                (Rule: IsInvalid(shelterOffer.StartDate), Parameter: nameof(ShelterOffer.StartDate)),
                (Rule: IsInvalid(shelterOffer.EndDate), Parameter: nameof(ShelterOffer.EndDate)),
                (Rule: IsInvalid(shelterOffer.Status), Parameter: nameof(ShelterOffer.Status)),
                (Rule: IsInvalid(shelterOffer.ShelterId), Parameter: nameof(ShelterOffer.ShelterId)),
                (Rule: IsInvalid(shelterOffer.HostId), Parameter: nameof(ShelterOffer.HostId)),

                (Rule: IsInvalid(shelterOffer.CreatedDate), Parameter: nameof(ShelterOffer.CreatedDate)),
                (Rule: IsInvalid(shelterOffer.CreatedByUserId), Parameter: nameof(ShelterOffer.CreatedByUserId)),
                (Rule: IsInvalid(shelterOffer.UpdatedDate), Parameter: nameof(ShelterOffer.UpdatedDate)),
                (Rule: IsInvalid(shelterOffer.UpdatedByUserId), Parameter: nameof(ShelterOffer.UpdatedByUserId)),

                (Rule: IsSame(
                    firstDate: shelterOffer.UpdatedDate,
                    secondDate: shelterOffer.CreatedDate,
                    secondDateName: nameof(ShelterOffer.CreatedDate)),
                Parameter: nameof(ShelterOffer.UpdatedDate)),

                (Rule: IsNotRecent(shelterOffer.UpdatedDate), Parameter: nameof(shelterOffer.UpdatedDate)));
        }

        public void ValidateShelterOfferId(Guid shelterOfferId) =>
            Validate((Rule: IsInvalid(shelterOfferId), Parameter: nameof(ShelterOffer.Id)));

        private static void ValidateStorageShelterOffer(ShelterOffer maybeShelterOffer, Guid shelterOfferId)
        {
            if (maybeShelterOffer is null)
            {
                throw new NotFoundShelterOfferException(shelterOfferId);
            }
        }

        private static void ValidateShelterOfferIsNotNull(ShelterOffer shelterOffer)
        {
            if (shelterOffer is null)
            {
                throw new NullShelterOfferException();
            }
        }

        private static void ValidateAgainstStorageShelterOfferOnModify(ShelterOffer inputShelterOffer, ShelterOffer storageShelterOffer)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputShelterOffer.CreatedDate,
                    secondDate: storageShelterOffer.CreatedDate,
                    secondDateName: nameof(ShelterOffer.CreatedDate)),
                Parameter: nameof(ShelterOffer.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: inputShelterOffer.CreatedByUserId,
                    secondId: storageShelterOffer.CreatedByUserId,
                    secondIdName: nameof(ShelterOffer.CreatedByUserId)),
                Parameter: nameof(ShelterOffer.CreatedByUserId)),

                (Rule: IsSame(
                    firstDate: inputShelterOffer.UpdatedDate,
                    secondDate: storageShelterOffer.UpdatedDate,
                    secondDateName: nameof(ShelterOffer.UpdatedDate)),
                Parameter: nameof(ShelterOffer.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        
        private static dynamic IsInvalid(ShelterOfferStatus status) => new
        {
            Condition = Enum.IsDefined(status) is false,
            Message = "Value is invalid"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsLater(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate > secondDate,
                Message =  $"Date is later than {nameof(ShelterOffer.EndDate)}"
            };


        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidShelterOfferException = new InvalidShelterOfferException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidShelterOfferException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidShelterOfferException.ThrowIfContainsErrors();
        }
    }
}