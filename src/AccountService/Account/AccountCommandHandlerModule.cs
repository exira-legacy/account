namespace AccountService.Account
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Commands;
    using SqlStreamStore;

    public sealed class AccountCommandHandlerModule : CommandHandlerModule
    {
        public AccountCommandHandlerModule(
            Func<IStreamStore> getStreamStore,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            Func<IAccounts> getAccounts)
        {
            For<NameAccount>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .Handle(async (message, ct) =>
                {
                    var accounts = getAccounts();

                    var accountId = message.Command.AccountId;
                    var possibleAccount = await accounts.GetOptionalAsync(accountId, ct);

                    if (!possibleAccount.HasValue)
                    {
                        possibleAccount = new Optional<Account>(Account.Register(accountId));
                        accounts.Add(accountId, possibleAccount.Value);
                    }

                    var account = possibleAccount.Value;

                    account.NameAccount(message.Command.AccountName);
                });
        }
    }
}
