namespace AccountService.Api.Account.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AccountService.Account.Commands;
    using FluentValidation;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateAccountRequest
    {
        /// <summary>Id the account to create.</summary>
        [Required]
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        /// <summary>Name of the account to create.</summary>
        [Required]
        [Display(Name = "Name")]
        public AccountName Name { get; set; }
    }

    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Id)
                .Required();

            RuleFor(x => x.Name)
                .Required()
                .SetValidator(new AccountNameValidator());
        }
    }

    public class CreateAccountRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new CreateAccountRequest
            {
                Id = Guid.NewGuid(),
                Name = new AccountName
                {
                    Name = "Something!",
                    Language = Language.English
                }
            };
    }

    public static class CreateAccountRequestMapping
    {
        public static NameAccount Map(CreateAccountRequest message)
            => new NameAccount(
                new AccountId(message.Id.Value),
                new AccountService.AccountName(message.Name.Name, message.Name.Language.Value));
    }
}
