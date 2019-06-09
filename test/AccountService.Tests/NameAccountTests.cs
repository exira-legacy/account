namespace AccountService.Tests
{
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Account.Commands;
    using Account.Events;
    using Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    public class NameAccountTests : AccountServiceTest
    {
        public Fixture Fixture { get; }

        public NameAccountTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture = new Fixture();
            Fixture.CustomizeAccountName();
        }

        [Fact]
        public void should_have_been_created()
        {
            var command = Fixture.Create<NameAccount>();

            Assert(new Scenario()
                .GivenNone()
                .When(command)
                .Then(command.AccountId,
                    new AccountWasBorn(command.AccountId),
                    new AccountWasNamed(command.AccountId, command.AccountName)));
        }

        [Fact]
        public void should_be_named_twice()
        {
            var id = Fixture.Create<AccountId>();
            var name = Fixture.Create<AccountName>();
            var name2 = Fixture.Create<AccountName>();
            var command = new NameAccount(id, name2);

            Assert(new Scenario()
                .Given(id,
                    new AccountWasBorn(id),
                    new AccountWasNamed(id, name))
                .When(command)
                .Then(command.AccountId,
                    new AccountWasNamed(command.AccountId, command.AccountName)));
        }
    }
}
