// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class RefugeeDependencyException : Xeption
    {
        public RefugeeDependencyException(Xeption innerException) 
            : base (message : "Refugee dependency error occurred, contact support", innerException)
        { }
    }
}
