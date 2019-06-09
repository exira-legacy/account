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

    public class UpdateAccountTests : ApiTest
    {
        public UpdateAccountTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void should_validate_request()
        {
            var validator = new UpdateAccountRequestValidator();

            validator.ShouldHaveValidationErrorFor(x => x.Name, null as AccountName);

            var nameValidator = new AccountNameValidator();

            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Name, new string('a', AccountService.AccountName.MaxLength + 10));

            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, null as Language?);
            nameValidator.ShouldHaveValidationErrorFor(x => x.Language, (Language)666);

            var validRequest = new UpdateAccountRequest
            {
                Name = new AccountName
                {
                    Name = "Bla",
                    Language = Language.English
                }
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Name, validRequest);
            validator.ShouldNotHaveValidationErrorFor(x => x.Name.Language, validRequest);
        }

        [Fact]
        public async Task should_create_a_correct_command()
        {
            var id = Guid.NewGuid();

            var request = new UpdateAccountRequest
            {
                Name = new AccountName
                {
                    Name = "Bla",
                    Language = Language.Dutch
                }
            };

            var commands = await Put($"/v1/accounts/{id}", request);

            Assert.True(commands.Count == 1);

            commands[0].IsEqual(
                new NameAccount(
                    new AccountId(id),
                    new AccountService.AccountName(request.Name.Name, request.Name.Language.Value)));
        }

        [Fact]
        public async Task should_fail_on_invalid_id()
        {
            var request = new UpdateAccountRequest
            {
                Name = new AccountName
                {
                    Name = string.Empty,
                    Language = (Language)666
                }
            };

            var commands = await Put("/v1/accounts/bla", request);

            Assert.True(commands.Count == 0);
        }

        [Fact]
        public async Task should_fail_on_invalid_data()
        {
            var request = new UpdateAccountRequest
            {
                Name = new AccountName
                {
                    Name = string.Empty,
                    Language = (Language)666
                }
            };

            var commands = await Put($"/v1/accounts/{Guid.NewGuid()}", request);

            Assert.True(commands.Count == 0);
        }
    }
}
