// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class FailedRefugeeGroupStorageException : Xeption
    {
        public FailedRefugeeGroupStorageException(Exception innerException) 
            : base(message: "Failed RefugeeGroup storage error occurred, contact support.", innerException)
        { }
    }
}