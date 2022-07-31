using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class LockedhostException : Xeption
    {
        public LockedhostException(Exception innerException)
            : base(message: "Locked host record exception, please try again later", innerException)
        {
        }
    }
}