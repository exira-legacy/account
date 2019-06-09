namespace AccountService.Api.Account.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AccountService.Account.Commands;
    using FluentValidation;
    using Swashbuckle.AspNetCore.Filters;

    public class UpdateAccountRequest
    {
        /// <summary>Id the account to update.</summary>
        [Required]
        [Display(Name = "Id")]
        internal Guid? Id { get; set; }

        /// <summary>Name of the account to update.</summary>
        [Required]
        [Display(Name = "Name")]
        public AccountName Name { get; set; }
    }

    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
    {
        public UpdateAccountRequestValidator()
        {
            RuleFor(x => x.Id)
                .Required();

            RuleFor(x => x.Name)
                .Required();
        }
    }

    public class UpdateAccountRequestExample : IExamplesProvider
    {
        public object GetExamples() =>
            new UpdateAccountRequest
            {
                Id = Guid.NewGuid(),
                Name = new AccountName
                {
                    Name = "Iets!",
                    Language = Language.Dutch
                }
            };
    }

    public static class UpdateAccountRequestMapping
    {
        public static NameAccount Map(UpdateAccountRequest message)
            => new NameAccount(
                new AccountId(message.Id.Value),
                new AccountService.AccountName(message.Name.Name, message.Name.Language.Value));
    }
}
