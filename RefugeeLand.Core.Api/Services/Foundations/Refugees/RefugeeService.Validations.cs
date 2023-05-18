// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public partial class RefugeeService
    {
        private void ValidateRefugeeOnAdd(Refugee refugee)
        {
            ValidateRefugeeIsNotNull(refugee);

            Validate(
                (Rule: IsInvalid(refugee.Id), Parameter: nameof(Refugee.Id)),
                (Rule: IsInvalid(refugee.FirstName), Parameter: nameof(Refugee.FirstName)),
                (Rule: IsInvalid(refugee.LastName), Parameter: nameof(Refugee.LastName)),
                (Rule: IsInvalid(refugee.BirthDate), Parameter: nameof(Refugee.BirthDate)),
                (Rule: IsInvalid(refugee.CreatedDate), Parameter: nameof(Refugee.CreatedDate)),
                (Rule: IsInvalid(refugee.UpdatedDate), Parameter: nameof(Refugee.UpdatedDate)),

                (Rule: IsNotSameAs(
                    firstDate: refugee.CreatedDate,
                    secondDate: refugee.UpdatedDate,
                    firstDateName: nameof(Refugee.CreatedDate)),

                 Parameter: nameof(Refugee.UpdatedDate)),
                (Rule: IsNotRecent(refugee.CreatedDate), Parameter: nameof(Refugee.CreatedDate)),
                (Rule: IsInvalid(refugee.Gender), Parameter: nameof(Refugee.Gender)));
        }

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute.Duration();
        }

        private static void ValidateRefugeeIsNotNull(Refugee refugee)
        {
            if (refugee is null)
            {
                throw new NullRefugeeException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(RefugeeGender gender) => new
        {
            Condition = Enum.IsDefined(gender) is false,
            Message = "Value is invalid"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSameAs(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string firstDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {firstDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidRefugeeException =
                new InvalidRefugeeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidRefugeeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidRefugeeException.ThrowIfContainsErrors();
        }

        public void ValidateRefugeeId(Guid refugeeId) =>
            Validate((Rule: IsInvalid(refugeeId), Parameter: nameof(Refugee.Id)));
    }
}
