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
                (Rule: IsInvalid(host.UpdatedByUserId), Parameter: nameof(host.UpdatedByUserId)));
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