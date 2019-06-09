namespace AccountService.Tests
{
    using Exceptions;
    using Xunit;

    public class AccountNameTests
    {
        [Fact]
        public void cannot_be_empty()
        {
            void NullName() => new AccountName(null, Language.Dutch);

            var ex = Record.Exception(NullName);

            Assert.NotNull(ex);
            Assert.IsType<NoAccountNameException>(ex);
        }

        [Fact]
        public void cannot_be_too_long()
        {
            void LongName() => new AccountName(new string('a', AccountName.MaxLength + 1), Language.Dutch);

            var ex = Record.Exception(LongName);

            Assert.NotNull(ex);
            Assert.IsType<AccountNameTooLongException>(ex);
        }

        [Theory]
        [InlineData("Hallo", Language.Dutch)]
        [InlineData("Hello", Language.English)]
        [InlineData("Bonjour", Language.French)]
        [InlineData("Hai", Language.German)]
        public void must_be_valid(string name, Language language)
        {
            void ValidName() => new AccountName(name, language);

            var ex = Record.Exception(ValidName);

            Assert.Null(ex);
        }
    }
}
