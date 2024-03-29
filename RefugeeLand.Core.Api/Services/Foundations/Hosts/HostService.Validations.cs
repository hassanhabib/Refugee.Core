// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
                
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.MiddleName), Parameter: nameof(Host.MiddleName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.AdditionalDetails), Parameter: nameof(Host.AdditionalDetails)),
                (Rule: IsInvalid(host.Gender), Parameter: nameof(Host.Gender)),
                (Rule: IsInvalid(host.BirthDate), Parameter: nameof(Host.BirthDate)),
                
                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),
                (Rule: IsInvalid(host.CreatedByUserId), Parameter: nameof(Host.CreatedByUserId)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),
                (Rule: IsInvalid(host.UpdatedByUserId), Parameter: nameof(Host.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: host.UpdatedDate,
                    secondDate: host.CreatedDate,
                    secondDateName: nameof(Host.CreatedDate)),
                Parameter: nameof(Host.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: host.UpdatedByUserId,
                    secondId: host.CreatedByUserId,
                    secondIdName: nameof(Host.CreatedByUserId)),
                Parameter: nameof(Host.UpdatedByUserId)),

                (Rule: IsNotRecent(host.CreatedDate), Parameter: nameof(Host.CreatedDate)));
        }

        private void ValidateHostOnModify(Host host)
        {
            ValidateHostIsNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(Host.Id)),

                (Rule: IsInvalid(host.FirstName), Parameter: nameof(Host.FirstName)),
                (Rule: IsInvalid(host.MiddleName), Parameter: nameof(Host.MiddleName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(Host.LastName)),
                (Rule: IsInvalid(host.AdditionalDetails), Parameter: nameof(Host.AdditionalDetails)),
                (Rule: IsInvalid(host.Gender), Parameter: nameof(Host.Gender)),
                (Rule: IsInvalid(host.BirthDate), Parameter: nameof(Host.BirthDate)),

                (Rule: IsInvalid(host.CreatedDate), Parameter: nameof(Host.CreatedDate)),
                (Rule: IsInvalid(host.CreatedByUserId), Parameter: nameof(Host.CreatedByUserId)),
                (Rule: IsInvalid(host.UpdatedDate), Parameter: nameof(Host.UpdatedDate)),
                (Rule: IsInvalid(host.UpdatedByUserId), Parameter: nameof(Host.UpdatedByUserId)),

                (Rule: IsSame(
                    firstDate: host.UpdatedDate,
                    secondDate: host.CreatedDate,
                    secondDateName: nameof(Host.CreatedDate)),
                Parameter: nameof(Host.UpdatedDate)),

                (Rule: IsNotRecent(host.UpdatedDate), Parameter: nameof(host.UpdatedDate)));
        }

        public void ValidateHostId(Guid hostId) =>
            Validate((Rule: IsInvalid(hostId), Parameter: nameof(Host.Id)));

        private static void ValidateStorageHost(Host maybeHost, Guid hostId)
        {
            if (maybeHost is null)
            {
                throw new NotFoundHostException(hostId);
            }
        }

        private static void ValidateHostIsNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
            }
        }

        private static void ValidateAgainstStorageHostOnModify(Host inputHost, Host storageHost)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputHost.CreatedDate,
                    secondDate: storageHost.CreatedDate,
                    secondDateName: nameof(Host.CreatedDate)),
                Parameter: nameof(Host.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: inputHost.CreatedByUserId,
                    secondId: storageHost.CreatedByUserId,
                    secondIdName: nameof(Host.CreatedByUserId)),
                Parameter: nameof(Host.CreatedByUserId)),

                (Rule: IsSame(
                    firstDate: inputHost.UpdatedDate,
                    secondDate: storageHost.UpdatedDate,
                    secondDateName: nameof(Host.UpdatedDate)),
                Parameter: nameof(Host.UpdatedDate)));
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

        private static dynamic IsInvalid(HostGender gender) => new
        {
            Condition = Enum.IsDefined(gender) is false,
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