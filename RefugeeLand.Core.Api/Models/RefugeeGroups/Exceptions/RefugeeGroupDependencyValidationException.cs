// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class RefugeeGroupDependencyValidationException : Xeption
    {
        public RefugeeGroupDependencyValidationException(Xeption innerException) 
            : base (message:"RefugeeGroup dependency validation occured, please try again.", innerException)
        { }
    }
}