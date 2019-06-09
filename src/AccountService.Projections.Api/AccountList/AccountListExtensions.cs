namespace AccountService.Projections.Api.AccountList
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;

    public static class AccountListExtensions
    {
        public static async Task<AccountList> FindAndUpdateAccountList(
            this ApiProjectionsContext context,
            Guid accountId,
            Action<AccountList> updateFunc,
            CancellationToken ct)
        {
            var account = await context
                .AccountList
                .FindAsync(accountId, cancellationToken: ct);

            if (account == null)
                throw DatabaseItemNotFound(accountId);

            updateFunc(account);

            return account;
        }

        private static ProjectionItemNotFoundException<AccountListProjections> DatabaseItemNotFound(Guid accountId)
            => new ProjectionItemNotFoundException<AccountListProjections>(accountId.ToString("D"));
    }
}
