namespace AccountService
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public abstract class AccountServiceException : DomainException
    {
        protected AccountServiceException() { }

        protected AccountServiceException(string message) : base(message) { }

        protected AccountServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
