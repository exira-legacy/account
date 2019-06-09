namespace AccountService.Exceptions
{
    public class AccountIdException : AccountServiceException
    {
        public AccountIdException(string message) : base(message) { }
    }

    public class NoAccountIdException : AccountIdException
    {
        public NoAccountIdException() : base("AccountId cannot be empty.") { }
    }
}
