// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class InvalidRefugeeGroupException : Xeption
    {
        public InvalidRefugeeGroupException() 
            : base(message:"Invalid RefugeeGroup. Please correct the errors and try again.")
        { }
    }
}