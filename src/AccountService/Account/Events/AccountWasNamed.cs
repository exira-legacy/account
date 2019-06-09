namespace AccountService.Account.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;

    [EventName("AccountWasNamed")]
    [EventDescription("The account was named in a specific language.")]
    public class AccountWasNamed
    {
        public Guid AccountId { get; }

        public string Name { get; }

        public Language Language { get; }

        public AccountWasNamed(
            AccountId accountId,
            AccountName accountName)
        {
            AccountId = accountId;

            Language = accountName.Language;
            Name = accountName.Name;
        }

        [JsonConstructor]
        private AccountWasNamed(
            Guid accountId,
            string name,
            Language language)
            : this(
                new AccountId(accountId),
                new AccountName(name, language)) { }
    }
}
