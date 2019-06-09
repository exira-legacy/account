namespace AccountService.Account
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public interface IAccounts : IAsyncRepository<Account, AccountId> { }
}
