# This is a RefugeeLand Coding Session
I need to implement a method in the refugeeService
- We're going to implement the RetrieveRefugeeById
- I almost forgot to create a new branch
- We want to create a failing test
  - The goal is to describe the steps to retrieve a refugee by id
  - We want to create a random refugee and mock the storage broker

I forgot the given, when, then
I need to find an error sound a bit nicer :P

We are using partial classes.
The service is injected in the constructor of an other file



We need to commit the failing test now

Now we need to make the test pass

I need to know if I should add more tests.
I guess there are some db exceptions I could catch if the id is invalid
Lets look at some example in Hassan's github

We can see a list of tests for exceptions

This is the work of Mehdi-Aghaei
He created a new test called
Should_Throw_Validation_Exception_On_RetrieveById_If_Id_Is_Invalid_And_Log_It_Async
in the file
/PostServiceTests.Validations.RetrieveById.cs

Ok this is the test, now lets see the actual validation.
It will not be implemented but we can take a look at the skeleton

Hoo he wrote the test without writing anything else
Lets take a look at other example to see if they did the same thing

Lets take a look at the exceptions we already have

I think those two exceptions are the ones we wanna use if the id is invalid

Lets continue to look at some example to be sure

Hoooo we could just the id on it own without relying on the db to send back an error

So the exception would be coming from the service itself

Now I'm more leaning into just checking the id in the service, lets look at the host we already have in the app

We already have the validation rule to check the id, so we just need to implement a test to see if the id we
are providing to the getRefugeeByIdAsync is a valid Id

Lets look at the host service once more

There is a stand alone method in the validations file we are going to use this to validate the Id

But first lets write the failing test.

I'm going to use Christo's code to write the test
Thanks mate
```cs
using System;
using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidCommentId = Guid.Empty;

            var invalidCommentException =
                new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.Id),
                values: "Id is required");

            var expectedCommentValidationException = new
                CommentValidationException(invalidCommentException);

            // when
            ValueTask<Comment> retrieveCommentByIdTask =
                this.commentService.RetrieveCommentByIdAsync(invalidCommentId);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                retrieveCommentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
```

















