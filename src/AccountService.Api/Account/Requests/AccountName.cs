namespace AccountService.Api.Account.Requests
{
    using System.ComponentModel.DataAnnotations;
    using FluentValidation;

    public class AccountName
    {
        /// <summary>Name of the account name.</summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>Language of the account name.</summary>
        [Required]
        [Display(Name = "Language")]
        public Language? Language { get; set; }
    }

    public class AccountNameValidator : AbstractValidator<AccountName>
    {
        public AccountNameValidator()
        {
            RuleFor(x => x.Name)
                .Required()
                .MaxLength(AccountService.AccountName.MaxLength);

            RuleFor(x => x.Language)
                .Required();
        }
    }
}
