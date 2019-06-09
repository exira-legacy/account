namespace AccountService.Projections.Api.AccountList
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Account.Events;

    public class AccountListProjections : ConnectedProjection<ApiProjectionsContext>
    {
        public AccountListProjections()
        {
            When<Envelope<AccountWasBorn>>(async (context, message, ct) =>
            {
                await context
                    .AccountList
                    .AddAsync(
                        new AccountList
                        {
                            Id = message.Message.AccountId,
                        }, ct);
            });

            When<Envelope<AccountWasNamed>>(async (context, message, ct) =>
                await context.FindAndUpdateAccountList(
                    message.Message.AccountId,
                    account =>
                    {
                        // Contrived example, if the name is Dutch, we always take it.
                        // Otherwise we only write it if we dont have any yet.
                        if (message.Message.Language == Language.Dutch)
                            account.Name = message.Message.Name;

                        if (string.IsNullOrWhiteSpace(account.Name))
                            account.Name = message.Message.Name;
                    },
                    ct));
        }
    }
}
