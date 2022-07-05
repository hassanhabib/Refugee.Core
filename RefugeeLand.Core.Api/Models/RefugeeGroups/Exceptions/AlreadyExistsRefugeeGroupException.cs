// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class AlreadyExistsRefugeeGroupException : Xeption
    {
        public AlreadyExistsRefugeeGroupException(Exception innerException) 
            : base(message:"RefugeeGroup with the same id already exists.", innerException)
        { }
    }
}