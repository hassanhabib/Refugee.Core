// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class InvalidRefugeeGroupReferenceException : Xeption
    {
        public InvalidRefugeeGroupReferenceException(Exception innerException) 
            : base(message:"Invalid RefugeeGroup reference error occured", innerException)
        { }
    }
}