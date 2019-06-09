namespace AccountService.Api.Account.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using Infrastructure.Responses;
    using Projections.Api.AccountDetail;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Account", Namespace = "")]
    public class AccountDetailResponse
    {
        /// <summary>Id of the account.</summary>
        [DataMember(Name = "Id", Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 2)]
        public List<Link> Links { get; set; }

        public AccountDetailResponse(
            AccountDetail accountDetail)
        {
            Id = accountDetail.Id;

            Links = new List<Link>
            {
                new Link("/", Link.Relations.Home, WebRequestMethods.Http.Get),
                new Link("/accounts", Link.Relations.Accounts, WebRequestMethods.Http.Get)
            };
        }
    }

    public class AccountDetailResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new AccountDetailResponse(
                new AccountDetail
                {
                    Id = Guid.NewGuid()
                });
    }
}
