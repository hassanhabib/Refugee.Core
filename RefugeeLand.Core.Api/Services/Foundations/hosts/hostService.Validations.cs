using System;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.hosts
{
    public partial class hostService
    {
        private void ValidatehostOnAdd(host host)
        {
            ValidatehostIsNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(host.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(host.CreatedDate)),
                (Rule: IsInvalid(host.CreatedByUserId), Parameter: nameof(host.CreatedByUserId)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(host.UpdatedDate)),
                (Rule: IsInvalid(host.UpdatedByUserId), Parameter: nameof(host.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: host.UpdatedDate,
                    secondDate: host.CreatedDate,
                    secondDateName: nameof(host.CreatedDate)),
                Parameter: nameof(host.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: host.UpdatedByUserId,
                    secondId: host.CreatedByUserId,
                    secondIdName: nameof(host.CreatedByUserId)),
                Parameter: nameof(host.UpdatedByUserId)),

                (Rule: IsNotRecent(host.CreatedDate), Parameter: nameof(host.CreatedDate)));
        }

        private static void ValidatehostIsNotNull(host host)
        {
            if (host is null)
            {
                throw new NullhostException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
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
            var invalidhostException = new InvalidhostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidhostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidhostException.ThrowIfContainsErrors();
        }
    }
}