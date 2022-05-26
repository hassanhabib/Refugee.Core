// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Refugees.Exceptions;

public class AlreadyExistRefugeeException : Xeption
{
    public AlreadyExistRefugeeException(Exception innerException)
        : base(message:"Refugee already exists", innerException)
    { }
}