// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupService
    {
        private delegate ValueTask<RefugeeGroup> ReturningRefugeeGroupFunction();
        
        private async ValueTask<RefugeeGroup> TryCatch(ReturningRefugeeGroupFunction returningRefugeeGroupFunction)
        {
            try
            {
                return await returningRefugeeGroupFunction();
            }
            catch (NullRefugeeGroupException nullRefugeeGroupException)
            {
                throw CreateAndLogValidationException(nullRefugeeGroupException);
            }
        }

        private RefugeeGroupValidationException CreateAndLogValidationException(Xeption exception)
        {
            var refugeeGroupValidationException = new RefugeeGroupValidationException(exception);
            this.loggingBroker.LogError(refugeeGroupValidationException);

            return refugeeGroupValidationException;
        }
    }
}