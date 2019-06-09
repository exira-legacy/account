namespace AccountService.Account.Commands
{
    public class NameAccount
    {
        public AccountId AccountId { get; }

        public AccountName AccountName { get; }

        public NameAccount(
            AccountId accountId,
            AccountName accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }
    }
}
