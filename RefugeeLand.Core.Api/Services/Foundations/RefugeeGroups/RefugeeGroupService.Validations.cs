// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupService
    {
        private static void ValidateRefugeeGroup(RefugeeGroup refugeeGroup)
        {
            ValidateRefugeeGroupIsNotNull(refugeeGroup);
            Validate(
                (Rule: IsInvalid(refugeeGroup.Id), Parameter: nameof(RefugeeGroup.Id)),
                (Rule: IsInvalid(refugeeGroup.Name), Parameter: nameof(RefugeeGroup.Name)),
                (Rule: IsInvalid(refugeeGroup.CreatedBy), Parameter: nameof(RefugeeGroup.CreatedBy)),
                (Rule: IsInvalid(refugeeGroup.UpdatedBy), Parameter: nameof(RefugeeGroup.UpdatedBy)),
                (Rule: IsInvalid(refugeeGroup.CreatedDate), Parameter: nameof(RefugeeGroup.CreatedDate)),
                (Rule: IsInvalid(refugeeGroup.UpdatedDate), Parameter: nameof(RefugeeGroup.UpdatedDate)),

            (Rule: IsInvalid(refugeeGroup.RefugeeGroupMainRepresentativeId),
                Parameter: nameof(RefugeeGroup.RefugeeGroupMainRepresentativeId)),
            
                (Rule: IsNotSame(
                        firstDate: refugeeGroup.UpdatedDate,
                        secondDate: refugeeGroup.CreatedDate,
                        secondDateName: nameof(RefugeeGroup.CreatedDate)),
                        Parameter: nameof(RefugeeGroup.UpdatedDate)));
        }

        private static void ValidateRefugeeGroupIsNotNull(RefugeeGroup refugeeGroup)
        {
            if (refugeeGroup is null)
            {
                throw new NullRefugeeGroupException();
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

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not the same as {secondDateName}"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidRefugeeGroupException =
                new InvalidRefugeeGroupException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidRefugeeGroupException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidRefugeeGroupException.ThrowIfContainsErrors();
        }
    }
}