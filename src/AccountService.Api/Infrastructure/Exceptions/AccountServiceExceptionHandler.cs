namespace AccountService.Api.Infrastructure.Exceptions
{
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.BasicApiProblem;
    using Microsoft.AspNetCore.Http;

    public class AccountServiceExceptionHandler : DefaultExceptionHandler<AccountServiceException>
    {
        protected override ProblemDetails GetApiProblemFor(AccountServiceException exception) =>
            new ProblemDetails
            {
                HttpStatus = StatusCodes.Status400BadRequest,
                Title = ProblemDetails.DefaultTitle,
                Detail = exception.Message,
                ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
            };
    }
}
