// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions
{
    public class FailedRefugeeStorageException : Xeption
    {
        public FailedRefugeeStorageException(Exception innerException)
            : base(message: "Failed refugee storage error occurred, contact support",
                  innerException)
        { }
    }
}
