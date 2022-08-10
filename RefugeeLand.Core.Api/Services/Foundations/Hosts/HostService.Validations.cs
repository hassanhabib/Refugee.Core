using System;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private void ValidateHostOnAdd(Host host)
        {
            ValidateHostIsNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),
                (Rule: IsInvalid(host.CreatedByUserId), Parameter: nameof(Host.CreatedByUserId)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),
                (Rule: IsInvalid(host.UpdatedByUserId), Parameter: nameof(Host.UpdatedByUserId)));
        }

        private static void ValidateHostIsNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
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
            var invalidHostException = new InvalidHostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHostException.ThrowIfContainsErrors();
        }
    }
}