// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class RefugeeGroupValidationException : Xeption
    {
        public RefugeeGroupValidationException(Xeption innerException)
            : base(message: "RefugeeGroup validation exception occurred, fix the errors and try again",
                  innerException)
        { }
    
    }
}