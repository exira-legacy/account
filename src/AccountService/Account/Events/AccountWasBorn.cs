namespace AccountService.Account.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("AccountWasBorn")]
    [EventDescription("The account was born!")]
    public class AccountWasBorn
    {
        public Guid AccountId { get; }

        public AccountWasBorn(
            AccountId accountId)
        {
            AccountId = accountId;
        }

        [JsonConstructor]
        private AccountWasBorn(
            Guid accountId)
            : this(
                new AccountId(accountId)) { }
    }
}
