namespace AccountService.Api.Account
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Pagination;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.BasicApiProblem;
    using Infrastructure;
    using Infrastructure.Responses;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Converters;
    using Projections.Api;
    using Projections.Api.AccountDetail;
    using Query;
    using Requests;
    using Responses;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ProblemDetails;
    using ValidationProblemDetails = Be.Vlaanderen.Basisregisters.BasicApiProblem.ValidationProblemDetails;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("accounts")]
    [ApiExplorerSettings(GroupName = "Account")]
    public class AccountController : AccountServiceController
    {
        /// <summary>
        /// Create account.
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="commandId">Optional unique identifier for the request.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="202">If the request has been accepted.</response>
        /// <response code="400">If the request contains invalid data.</response>
        /// <response code="500">If an internal error has occurred.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(CreateAccountRequest), typeof(CreateAccountRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status202Accepted, typeof(EmptyResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> CreateAccount(
            [FromServices] ICommandHandlerResolver bus,
            [FromCommandId] Guid commandId,
            [FromBody] CreateAccountRequest request,
            CancellationToken cancellationToken = default)
        {
            await new CreateAccountRequestValidator()
                .ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            var command = CreateAccountRequestMapping.Map(request);

            return Accepted(
                $"/v1/accounts/{command.AccountId}",
                await bus.Dispatch(
                    commandId,
                    command,
                    GetMetadata(),
                    cancellationToken));
        }

        /// <summary>
        /// Update account.
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="accountId">Identificator of the account.</param>
        /// <param name="commandId">Optional unique identifier for the request.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="202">If the request has been accepted.</response>
        /// <response code="400">If the request contains invalid data.</response>
        /// <response code="500">If an internal error has occurred.</response>
        /// <returns></returns>
        [HttpPut("{accountId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(UpdateAccountRequest), typeof(UpdateAccountRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status202Accepted, typeof(EmptyResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> UpdateAccount(
            [FromServices] ICommandHandlerResolver bus,
            [FromCommandId] Guid commandId,
            [FromRoute] Guid accountId,
            [FromBody] UpdateAccountRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request != null)
                request.Id = accountId;

            await new UpdateAccountRequestValidator()
                .ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            var command = UpdateAccountRequestMapping.Map(request);

            return Accepted(
                $"/v1/accounts/{command.AccountId}",
                await bus.Dispatch(
                    commandId,
                    command,
                    GetMetadata(),
                    cancellationToken));
        }

        /// <summary>
        /// List accounts.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If accounts are found.</response>
        /// <response code="500">If an internal error has occurred.</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(AccountListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AccountListResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> ListAccounts(
            [FromServices] ApiProjectionsContext context,
            CancellationToken cancellationToken = default)
        {
            var filtering = Request.ExtractFilteringRequest<AccountListFilter>();
            var sorting = Request.ExtractSortingRequest();
            var pagination = Request.ExtractPaginationRequest();

            var pagedAccounts = new AccountListQuery(context)
                .Fetch(filtering, sorting, pagination);

            Response.AddPagedQueryResultHeaders(pagedAccounts);

            return Ok(
                new AccountListResponse
                {
                    Accounts = await pagedAccounts
                        .Items
                        .Select(x => new AccountListItemResponse(x))
                        .ToListAsync(cancellationToken)
                });
        }

        /// <summary>
        /// Get details of the account.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="accountId">Identificator of the account.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">If the account is found.</response>
        /// <response code="400">If the request contains invalid data.</response>
        /// <response code="404">If the account does not exist.</response>
        /// <response code="500">If an internal error has occured.</response>
        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(AccountDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AccountDetailResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(AccountNotFoundResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public async Task<IActionResult> DetailAccount(
            [FromServices] ApiProjectionsContext context,
            [FromRoute] Guid accountId,
            CancellationToken cancellationToken = default)
        {
            var request = new DetailAccountRequest
            {
                Id = accountId,
            };

            await new DetailAccountRequestValidator()
                .ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            var account = await FindAccountAsync(context, request.Id.Value, cancellationToken);

            return Ok(
                new AccountDetailResponse(account));
        }

        private static async Task<AccountDetail> FindAccountAsync(
            ApiProjectionsContext context,
            Guid accountId,
            CancellationToken cancellationToken)
        {
            var account = await context
                .AccountDetails
                .FindAsync(new object[] { accountId }, cancellationToken);

            if (account == null)
                throw new ApiException(AccountNotFoundResponseExamples.Message, StatusCodes.Status404NotFound);

            return account;
        }
    }
}
