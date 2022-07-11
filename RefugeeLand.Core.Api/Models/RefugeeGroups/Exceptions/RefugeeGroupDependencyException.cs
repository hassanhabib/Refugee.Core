// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class RefugeeGroupDependencyException : Xeption
    {
        public RefugeeGroupDependencyException(Xeption innerException)
            : base(message: "RefugeeGroup dependency error occurred, contact support.", innerException)
        { }
    }
}