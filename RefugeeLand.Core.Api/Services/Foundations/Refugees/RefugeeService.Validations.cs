// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using RefugeeLand.Core.Api.Models.Enums;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using System;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public partial class RefugeeService
    {
        private void ValidateRefugeeOnAdd(Refugee refugee)
        {
            ValidateRefugeeIsNotNull(refugee);

            Validate(
                (Rule: IsInvalid(refugee.Id), Parameter:nameof(Refugee.Id)),
                (Rule: IsInvalid(refugee.FirstName), Parameter:nameof(Refugee.FirstName)),
                (Rule: IsInvalid(refugee.LastName), Parameter:nameof(Refugee.LastName)),
                (Rule: IsInvalid(refugee.BirthDate), Parameter:nameof(Refugee.BirthDate)),
                (Rule: IsInvalid(refugee.Email), Parameter:nameof(Refugee.Email)),
                (Rule: IsInvalid(refugee.CreatedDate), Parameter:nameof(Refugee.CreatedDate)),
                (Rule: IsInvalid(refugee.UpdatedDate), Parameter:nameof(Refugee.UpdatedDate)));
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
            Condition = string.IsNullOrWhiteSpace(text) ,
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Gender gender) => new
        {
            Condition = Enum.IsDefined(gender) is false,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
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
    }
}
