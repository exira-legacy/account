namespace AccountService
{
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Exceptions;

    public class AccountName : ValueObject<AccountName>
    {
        public const int MaxLength = 200;

        public string Name { get; }

        public Language Language { get; }

        public AccountName(string name, Language language)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NoAccountNameException();

            if (name.Length > MaxLength)
                throw new AccountNameTooLongException();

            Name = name;
            Language = language;
        }

        protected override IEnumerable<object> Reflect()
        {
            yield return Name;
            yield return Language;
        }

        public override string ToString() => $"{Name} ({Language})";
    }
}
