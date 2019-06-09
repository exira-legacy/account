namespace AccountService.Projections.Api.AccountDetail
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;

    public static class AccountDetailExtensions
    {
        public static async Task<AccountDetail> FindAndUpdateAccountDetail(
            this ApiProjectionsContext context,
            Guid accountId,
            Action<AccountDetail> updateFunc,
            CancellationToken ct)
        {
            var account = await context
                .AccountDetails
                .FindAsync(accountId, cancellationToken: ct);

            if (account == null)
                throw DatabaseItemNotFound(accountId);

            updateFunc(account);

            return account;
        }

        private static ProjectionItemNotFoundException<AccountDetailProjections> DatabaseItemNotFound(Guid accountId)
            => new ProjectionItemNotFoundException<AccountDetailProjections>(accountId.ToString("D"));
    }
}
