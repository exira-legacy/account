namespace AccountService.Api.Account.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentValidation;

    public class DetailAccountRequest
    {
        /// <summary>Id of the account to get details for.</summary>
        [Required]
        [Display(Name = "Id")]
        internal Guid? Id { get; set; }
    }

    public class DetailAccountRequestValidator : AbstractValidator<DetailAccountRequest>
    {
        public DetailAccountRequestValidator()
        {
            RuleFor(x => x.Id)
                .Required();
        }
    }
}
