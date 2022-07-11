// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class RefugeeGroupServiceException : Xeption
    {
        public RefugeeGroupServiceException(Xeption innerException) 
            : base(message: "RefugeeGroup service error occured, please contact support", innerException)
        { }
    }
}