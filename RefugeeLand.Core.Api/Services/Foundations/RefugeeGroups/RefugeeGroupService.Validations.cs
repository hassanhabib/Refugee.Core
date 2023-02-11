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
        private void ValidateRefugeeGroup(RefugeeGroup refugeeGroup)
        {
            ValidateRefugeeGroupIsNotNull(refugeeGroup);
            Validate(
                (Rule: IsInvalid(refugeeGroup.Id), Parameter: nameof(RefugeeGroup.Id)),
                (Rule: IsInvalid(refugeeGroup.Name), Parameter: nameof(RefugeeGroup.Name)),
                (Rule: IsInvalid(refugeeGroup.CreatedByUserId), Parameter: nameof(RefugeeGroup.CreatedByUserId)),
                (Rule: IsInvalid(refugeeGroup.UpdatedByUserId), Parameter: nameof(RefugeeGroup.UpdatedByUserId)),
                (Rule: IsInvalid(refugeeGroup.CreatedDate), Parameter: nameof(RefugeeGroup.CreatedDate)),
                (Rule: IsInvalid(refugeeGroup.UpdatedDate), Parameter: nameof(RefugeeGroup.UpdatedDate)),

                (Rule: IsInvalid(refugeeGroup.RefugeeGroupMainRepresentativeId),
                    Parameter: nameof(RefugeeGroup.RefugeeGroupMainRepresentativeId)),

                (Rule: IsNotSame(
                        firstDate: refugeeGroup.UpdatedDate,
                        secondDate: refugeeGroup.CreatedDate,
                        secondDateName: nameof(RefugeeGroup.CreatedDate)),
                    Parameter: nameof(RefugeeGroup.UpdatedDate)),
                
                (Rule: IsNotRecent(refugeeGroup.CreatedDate), Parameter: nameof(RefugeeGroup.CreatedDate)));
        }

        public void ValidateRefugeeGroupId(Guid refugeeGroupId) =>
            Validate((Rule: IsInvalid(refugeeGroupId), Parameter: nameof(RefugeeGroup.Id)));

        private static void ValidateRefugeeGroupIsNotNull(RefugeeGroup refugeeGroup)
        {
            if (refugeeGroup is null)
            {
                throw new NullRefugeeGroupException();
            }
        }
        
        private static void ValidateStorageRefugeeGroup(RefugeeGroup maybeRefugeeGroup, Guid refugeeGroupId)
        {
            if (maybeRefugeeGroup is null)
            {
                throw new NotFoundRefugeeGroupException(refugeeGroupId);
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