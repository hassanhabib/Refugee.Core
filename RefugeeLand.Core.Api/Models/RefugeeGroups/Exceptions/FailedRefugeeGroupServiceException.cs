// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class FailedRefugeeGroupServiceException : Xeption
    {
        public FailedRefugeeGroupServiceException(Exception innerException) 
            : base(message: "Failed RefugeeGroup service occured, please contact support.", innerException)
        { }
    }
}