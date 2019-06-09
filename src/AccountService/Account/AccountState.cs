namespace AccountService.Account
{
    using System.Collections.Generic;
    using Events;

    public partial class Account
    {
        private AccountId _accountId;

        private readonly Dictionary<Language, AccountName> _names
            = new Dictionary<Language, AccountName>();

        private Account()
        {
            Register<AccountWasBorn>(When);
            Register<AccountWasNamed>(When);
        }

        private void When(AccountWasBorn @event)
        {
            _accountId = new AccountId(@event.AccountId);
        }

        private void When(AccountWasNamed @event)
        {
            _names[@event.Language] = new AccountName(@event.Name, @event.Language);
        }
    }
}
