namespace AccountService.Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using AccountService.Account.Commands;
    using Account.Requests;
    using FluentValidation.TestHelper;
    using Infrastructure;
    using Xunit;
    using Xunit.Abstractions;

    public class CreateAccountTests : ApiTest
    {
        public CreateAccountTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void should_validate_request()
        {
            var validator = new CreateAccountRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.Id, null as Guid?);
            validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty);

            validator.ShouldHaveValidationErrorFor(x => x.Name, null as AccountName);

            var nameValidator = new AccountNameValidator();

            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, new string('a', AccountService.AccountName.MaxLength + 10));

            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, null as Language?);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, (Language)666);

            var validRequest = new CreateAccountRequest
            {
                Id = Guid.NewGuid(),
                Name = new AccountName
                {
                    Name = "Bla",
                    Language = Language.Dutch
                }
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.Id, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Language, validRequest);
        }

        [Fact]
        public async Task should_create_a_correct_command()
        {
            var request = new CreateAccountRequest
            {
                Id = Guid.NewGuid(),
                Name = new AccountName
                {
                    Name = "Bla",
                    Language = Language.Dutch
                }
            };

            var commands = await Post("/v1/accounts", request);

            Assert.True(commands.Count == 1);

            commands[0].IsEqual(
                new NameAccount(
                    new AccountId(request.Id.Value),
                    new AccountService.AccountName(request.Name.Name, request.Name.Language.Value)));
        }

        [Fact]
        public async Task should_fail_on_invalid_data()
        {
            var request = new CreateAccountRequest
            {
                Id = Guid.NewGuid(),
                Name = new AccountName
                {
                    Name = string.Empty,
                    Language = (Language)666
                }
            };

            var commands = await Post("/v1/accounts", request);

            Assert.True(commands.Count == 0);
        }
    }
}
