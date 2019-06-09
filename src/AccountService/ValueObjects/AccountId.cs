namespace AccountService
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Exceptions;
    using Newtonsoft.Json;

    public class AccountId : GuidValueObject<AccountId>
    {
        public AccountId([JsonProperty("value")] Guid accountId) : base(accountId)
        {
            if (accountId == Guid.Empty)
                throw new NoAccountIdException();
        }
    }
}
