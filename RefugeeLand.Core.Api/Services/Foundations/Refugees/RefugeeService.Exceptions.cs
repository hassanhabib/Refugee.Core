// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch(SqlException sqlException)
            {
                var failedRefugeeStorageException = new FailedRefugeeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedRefugeeStorageException);
            }
            catch(DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistRefugeeException =
                    new AlreadyExistRefugeeException(duplicateKeyException);
 
                throw CreateAndLogDependencyValidationException(alreadyExistRefugeeException);
            }
            catch(DbUpdateException dbUpdateException)
            { 
                var failedRefugeeStorageException =
                    new FailedRefugeeStorageException(dbUpdateException);

                throw CreateAndLogDependencyValidationException(failedRefugeeStorageException);
            }
        }

        private Exception CreateAndLogDependencyValidationException(Xeption exception)
        {
            var refugeeDependencyValidationException =
                new RefugeeDependencyValidationException(exception);

            this.loggingBroker.LogError(refugeeDependencyValidationException);

            return refugeeDependencyValidationException;
        }

        private RefugeeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var refugeeValidationException = new RefugeeValidationException(exception);
            this.loggingBroker.LogError(refugeeValidationException);

            return refugeeValidationException;
        }

        private RefugeeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var refugeeDependencyException = new RefugeeDependencyException(exception);
            this.loggingBroker.LogCritical(refugeeDependencyException);

            return refugeeDependencyException;
        }
    }
}