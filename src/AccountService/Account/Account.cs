namespace AccountService.Account
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Events;

    public partial class Account : AggregateRootEntity
    {
        public static readonly Func<Account> Factory = () => new Account();

        public static Account Register(AccountId accountId)
        {
            var account = Factory();
            account.ApplyChange(new AccountWasBorn(accountId));
            return account;
        }

        public void NameAccount(AccountName accountName)
        {
            ApplyChange(new AccountWasNamed(_accountId, accountName));
        }
    }
}
