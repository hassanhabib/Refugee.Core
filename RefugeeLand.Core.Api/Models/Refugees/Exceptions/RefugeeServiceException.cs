// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class RefugeeServiceException : Xeption
    {
        public RefugeeServiceException(Xeption innerException) 
            : base(message : "Refugee service error occurred, contact support", innerException)
        { }
    }
}
