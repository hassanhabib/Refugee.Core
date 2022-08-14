using System;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public partial class NationalityService
    {
        private void ValidateNationalityOnAdd(Nationality nationality)
        {
            ValidateNationalityIsNotNull(nationality);

            Validate(
                (Rule: IsInvalid(nationality.Id), Parameter: nameof(Nationality.Id)),

                (Rule: IsInvalid(nationality.Name), Parameter: nameof(Nationality.Name)),
                (Rule: IsInvalid(nationality.Country), Parameter: nameof(Nationality.Country)),
                (Rule: IsInvalid(nationality.CreatedDate), Parameter: nameof(Nationality.CreatedDate)),
                (Rule: IsInvalid(nationality.CreatedByUserId), Parameter: nameof(Nationality.CreatedByUserId)),
                (Rule: IsInvalid(nationality.UpdatedDate), Parameter: nameof(Nationality.UpdatedDate)),
                (Rule: IsInvalid(nationality.UpdatedByUserId), Parameter: nameof(Nationality.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: nationality.UpdatedDate,
                    secondDate: nationality.CreatedDate,
                    secondDateName: nameof(Nationality.CreatedDate)),
                Parameter: nameof(Nationality.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: nationality.UpdatedByUserId,
                    secondId: nationality.CreatedByUserId,
                    secondIdName: nameof(Nationality.CreatedByUserId)),
                Parameter: nameof(Nationality.UpdatedByUserId)),

                (Rule: IsNotRecent(nationality.CreatedDate), Parameter: nameof(Nationality.CreatedDate)));
        }

        public void ValidateNationalityId(Guid nationalityId) =>
            Validate((Rule: IsInvalid(nationalityId), Parameter: nameof(Nationality.Id)));

        private static void ValidateStorageNationality(Nationality maybeNationality, Guid nationalityId)
        {
            if (maybeNationality is null)
            {
                throw new NotFoundNationalityException(nationalityId);
            }
        }

        private static void ValidateNationalityIsNotNull(Nationality nationality)
        {
            if (nationality is null)
            {
                throw new NullNationalityException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        
        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
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
            var invalidNationalityException = new InvalidNationalityException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidNationalityException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidNationalityException.ThrowIfContainsErrors();
        }
    }
}