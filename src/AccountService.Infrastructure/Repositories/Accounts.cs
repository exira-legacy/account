namespace AccountService.Infrastructure.Repositories
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Account;
    using SqlStreamStore;

    public class Accounts : Repository<Account, AccountId>, IAccounts
    {
        public Accounts(ConcurrentUnitOfWork unitOfWork, IStreamStore eventStore, EventMapping eventMapping, EventDeserializer eventDeserializer)
            : base(Account.Factory, unitOfWork, eventStore, eventMapping, eventDeserializer) { }
    }
}
