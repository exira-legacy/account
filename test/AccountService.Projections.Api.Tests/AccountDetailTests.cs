namespace AccountService.Projections.Api.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Account.Events;
    using AccountDetail;
    using Infrastructure;
    using Xunit;
    using Xunit.Abstractions;
    using AccountService.Tests.Infrastructure;

    public class AccountDetailTests : ProjectionsTest
    {
        public Fixture Fixture { get; }

        public AccountDetailTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            Fixture = new Fixture();
            Fixture.CustomizeAccountName();
        }

        [Fact]
        public Task when_accounts_are_born()
        {
            var data = Fixture
                .CreateMany<AccountWasBorn>(new Random().Next(1, 100))
                .Select(@event =>
                {
                    var expectedRecord = new AccountDetail
                    {
                        Id = @event.AccountId
                    };

                    return new
                    {
                        AccountWasBorn = @event,
                        ExpectedRecord = expectedRecord
                    };
                }).ToList();

            return new AccountDetailProjections()
                .Scenario()
                .Given(data.Select(d => d.AccountWasBorn))
                .Expect(TestOutputHelper, data.Select(d => d.ExpectedRecord));
        }

        [Fact]
        public Task when_accounts_are_named()
        {
            var previousEvents = Fixture
                .CreateMany<AccountWasBorn>(new Random().Next(1, 100))
                .ToList();

            var events = previousEvents
                .Select(x =>
                    new AccountWasNamed(
                        new AccountId(x.AccountId),
                        Fixture.Create<AccountName>()))
                .ToList();

            var expected = events
                .Select(x =>
                {
                    var record = new AccountDetail { Id = x.AccountId };

                    switch (x.Language)
                    {
                        case Language.Dutch:
                            record.NameDutch = x.Name;
                            break;

                        case Language.French:
                            record.NameFrench = x.Name;
                            break;

                        case Language.German:
                            record.NameGerman = x.Name;
                            break;

                        case Language.English:
                            record.NameEnglish = x.Name;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    return record;
                })
                .ToList();

            return new AccountDetailProjections()
                .Scenario()
                .Given(previousEvents.Cast<object>().Union(events))
                .Expect(TestOutputHelper, expected);
        }
    }
}
