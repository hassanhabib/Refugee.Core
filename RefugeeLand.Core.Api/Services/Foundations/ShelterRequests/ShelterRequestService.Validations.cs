// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestService
    {
        private void ValidateShelterRequestOnAdd(ShelterRequest shelterRequest)
        {
            ValidateShelterRequestIsNotNull(shelterRequest);

            Validate(
                (Rule: IsInvalid(shelterRequest.Id), Parameter: nameof(ShelterRequest.Id)),

                (Rule: IsInvalid(shelterRequest.RefugeeApplicantId), Parameter: nameof(ShelterRequest.RefugeeApplicantId)),
                (Rule: IsInvalid(shelterRequest.RefugeeGroupId), Parameter: nameof(ShelterRequest.RefugeeGroupId)),
                (Rule: IsInvalid(shelterRequest.ShelterOfferId), Parameter: nameof(ShelterRequest.ShelterOfferId)),  
                (Rule: IsInvalid(shelterRequest.StartDate), Parameter: nameof(ShelterRequest.StartDate)),
                (Rule: IsInvalid(shelterRequest.EndDate), Parameter: nameof(ShelterRequest.EndDate)),
                (Rule: IsInvalid(shelterRequest.Status), Parameter: nameof(ShelterRequest.Status)),
                
                (Rule: IsInvalid(shelterRequest.CreatedDate), Parameter: nameof(ShelterRequest.CreatedDate)),
                (Rule: IsInvalid(shelterRequest.CreatedByUserId), Parameter: nameof(ShelterRequest.CreatedByUserId)),
                (Rule: IsInvalid(shelterRequest.UpdatedDate), Parameter: nameof(ShelterRequest.UpdatedDate)),
                (Rule: IsInvalid(shelterRequest.UpdatedByUserId), Parameter: nameof(ShelterRequest.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: shelterRequest.UpdatedDate,
                    secondDate: shelterRequest.CreatedDate,
                    secondDateName: nameof(ShelterRequest.CreatedDate)),
                Parameter: nameof(ShelterRequest.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: shelterRequest.UpdatedByUserId,
                    secondId: shelterRequest.CreatedByUserId,
                    secondIdName: nameof(ShelterRequest.CreatedByUserId)),
                Parameter: nameof(ShelterRequest.UpdatedByUserId)),

                (Rule: IsNotRecent(shelterRequest.CreatedDate), Parameter: nameof(ShelterRequest.CreatedDate)));
        }

        private void ValidateShelterRequestOnModify(ShelterRequest shelterRequest)
        {
            ValidateShelterRequestIsNotNull(shelterRequest);

            Validate(
                (Rule: IsInvalid(shelterRequest.Id), Parameter: nameof(ShelterRequest.Id)),

                (Rule: IsInvalid(shelterRequest.RefugeeApplicantId), Parameter: nameof(ShelterRequest.RefugeeApplicantId)),
                (Rule: IsInvalid(shelterRequest.RefugeeGroupId), Parameter: nameof(ShelterRequest.RefugeeGroupId)),
                (Rule: IsInvalid(shelterRequest.ShelterOfferId), Parameter: nameof(ShelterRequest.ShelterOfferId)),  
                (Rule: IsInvalid(shelterRequest.StartDate), Parameter: nameof(ShelterRequest.StartDate)),
                (Rule: IsInvalid(shelterRequest.EndDate), Parameter: nameof(ShelterRequest.EndDate)),
                (Rule: IsInvalid(shelterRequest.Status), Parameter: nameof(ShelterRequest.Status)),

                (Rule: IsInvalid(shelterRequest.CreatedDate), Parameter: nameof(ShelterRequest.CreatedDate)),
                (Rule: IsInvalid(shelterRequest.CreatedByUserId), Parameter: nameof(ShelterRequest.CreatedByUserId)),
                (Rule: IsInvalid(shelterRequest.UpdatedDate), Parameter: nameof(ShelterRequest.UpdatedDate)),
                (Rule: IsInvalid(shelterRequest.UpdatedByUserId), Parameter: nameof(ShelterRequest.UpdatedByUserId)),

                (Rule: IsSame(
                    firstDate: shelterRequest.UpdatedDate,
                    secondDate: shelterRequest.CreatedDate,
                    secondDateName: nameof(ShelterRequest.CreatedDate)),
                Parameter: nameof(ShelterRequest.UpdatedDate)),

                (Rule: IsNotRecent(shelterRequest.UpdatedDate), Parameter: nameof(shelterRequest.UpdatedDate)));
        }

        public void ValidateShelterRequestId(Guid shelterRequestId) =>
            Validate((Rule: IsInvalid(shelterRequestId), Parameter: nameof(ShelterRequest.Id)));

        private static void ValidateStorageShelterRequest(ShelterRequest maybeShelterRequest, Guid shelterRequestId)
        {
            if (maybeShelterRequest is null)
            {
                throw new NotFoundShelterRequestException(shelterRequestId);
            }
        }

        private static void ValidateShelterRequestIsNotNull(ShelterRequest shelterRequest)
        {
            if (shelterRequest is null)
            {
                throw new NullShelterRequestException();
            }
        }

        private static void ValidateAgainstStorageShelterRequestOnModify(ShelterRequest inputShelterRequest, ShelterRequest storageShelterRequest)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputShelterRequest.CreatedDate,
                    secondDate: storageShelterRequest.CreatedDate,
                    secondDateName: nameof(ShelterRequest.CreatedDate)),
                Parameter: nameof(ShelterRequest.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: inputShelterRequest.CreatedByUserId,
                    secondId: storageShelterRequest.CreatedByUserId,
                    secondIdName: nameof(ShelterRequest.CreatedByUserId)),
                Parameter: nameof(ShelterRequest.CreatedByUserId)),

                (Rule: IsSame(
                    firstDate: inputShelterRequest.UpdatedDate,
                    secondDate: storageShelterRequest.UpdatedDate,
                    secondDateName: nameof(ShelterRequest.UpdatedDate)),
                Parameter: nameof(ShelterRequest.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(ShelterRequestStatus status) => new
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
            var invalidShelterRequestException = new InvalidShelterRequestException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidShelterRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidShelterRequestException.ThrowIfContainsErrors();
        }
    }
}