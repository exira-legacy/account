namespace AccountService.Api.Account.Responses
{
    using Be.Vlaanderen.Basisregisters.BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class AccountNotFoundResponseExamples : IExamplesProvider
    {
        public static string Message = "Non-existing account.";

        public object GetExamples()
            => new ProblemDetails
            {
                HttpStatus = StatusCodes.Status404NotFound,
                Title = ProblemDetails.DefaultTitle,
                Detail = Message,
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
