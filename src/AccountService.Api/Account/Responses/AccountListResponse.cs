namespace AccountService.Api.Account.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;
    using Infrastructure.Responses;
    using Projections.Api.AccountList;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "Accounts", Namespace = "")]
    public class AccountListResponse
    {
        /// <summary>
        /// All accounts.
        /// </summary>
        [DataMember(Name = "Accounts", Order = 1)]
        public List<AccountListItemResponse> Accounts { get; set; }

        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 2)]
        public List<Link> Links { get; set; }

        public AccountListResponse()
        {
            Links = new List<Link>
            {
                new Link("/", Link.Relations.Home, WebRequestMethods.Http.Get),
                new Link("/accounts", Link.Relations.Accounts, WebRequestMethods.Http.Post)
            };
        }
    }

    [DataContract(Name = "Account", Namespace = "")]
    public class AccountListItemResponse
    {
        /// <summary>
        /// Id of the account.
        /// </summary>
        [DataMember(Name = "Id", Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Hypermedia links
        /// </summary>
        [DataMember(Name = "Links", Order = 2)]
        public List<Link> Links { get; set; }

        public AccountListItemResponse(
            AccountList accountList)
        {
            Id = accountList.Id;

            Links = new List<Link>
            {
                new Link($"/accounts/{accountList.Id}", Link.Relations.Account, WebRequestMethods.Http.Get),
            };
        }
    }

    public class AccountListResponseExamples : IExamplesProvider
    {
        public object GetExamples()
            => new AccountListResponse
            {
                Accounts = new List<AccountListItemResponse>
                {
                    new AccountListItemResponse(
                        new AccountList
                        {
                            Id = Guid.NewGuid()
                        }),
                    new AccountListItemResponse(
                        new AccountList
                        {
                            Id = Guid.NewGuid()
                        }),
                }
            };
    }
}
