namespace AccountService.Exceptions
{
    public class AccountNameException : AccountServiceException
    {
        public AccountNameException(string message) : base(message) { }
    }

    public class NoAccountNameException : AccountNameException
    {
        public NoAccountNameException() : base("AccountName cannot be empty.") { }
    }

    public class AccountNameTooLongException : AccountNameException
    {
        public AccountNameTooLongException() : base($"AccountName cannot be longer than {AccountName.MaxLength} characters.") { }
    }
}
