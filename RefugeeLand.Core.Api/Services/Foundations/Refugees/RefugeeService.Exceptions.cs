// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public partial class RefugeeService
    {
        private delegate ValueTask<Refugee> ReturningRefugeeFunction();

        private async ValueTask<Refugee> TryCatch(ReturningRefugeeFunction returningRefugeeFunction)
        {
            try
            {
                return await returningRefugeeFunction();
            }
            catch (NullRefugeeException nullRefugeeException)
            {
                throw CreateAndLogValidationException(nullRefugeeException);
            }
            catch (InvalidRefugeeException invalidRefugeeException)
            {
                throw CreateAndLogValidationException(invalidRefugeeException);
            }
        }

        private RefugeeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var refugeeValidationException = new RefugeeValidationException(exception);
            this.loggingBroker.LogError(refugeeValidationException);

            return refugeeValidationException;
        }
    }
}