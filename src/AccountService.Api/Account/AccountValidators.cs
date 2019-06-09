namespace AccountService.Api.Account
{
    using System;
    using Be.Vlaanderen.Basisregisters.Api.Localization;
    using Requests;
    using FluentValidation;
    using Microsoft.Extensions.Localization;

    public class AccountValidatorsResources
    {
        public string RequiredMessage => "{PropertyName} is required.";
        public string MaxLengthMessage => "{PropertyName} cannot be longer than {MaxLength} characters.";
    }

    public static class AccountValidators
    {
        private static readonly IStringLocalizer<AccountValidatorsResources> Localizer =
            GlobalStringLocalizer.Instance.GetLocalizer<AccountValidatorsResources>();

        public static IRuleBuilderOptions<T, Guid?> Required<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage))
                .NotEqual(Guid.Empty).WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, Language?> Required<T>(this IRuleBuilder<T, Language?> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage))
                .IsInEnum().WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, AccountName> Required<T>(this IRuleBuilder<T, AccountName> ruleBuilder)
            => ruleBuilder
                .NotEmpty().WithMessage(Localizer.GetString(x => x.RequiredMessage));

        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
            => ruleBuilder
                .Length(0, length).WithMessage(Localizer.GetString(x => x.MaxLengthMessage));
    }
}
