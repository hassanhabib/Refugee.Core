// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class RefugeeValidationException : Xeption
    {
        public RefugeeValidationException(Xeption innerException)
            : base(message: "Refugee validation exception occurred, fix the errors and try again",
                  innerException)
        { }
    }
}
