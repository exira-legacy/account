namespace AccountService.Api.Account.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Microsoft.EntityFrameworkCore;
    using Projections.Api;
    using Projections.Api.AccountList;

    public class AccountListQuery : Query<AccountList, AccountListFilter>
    {
        private readonly ApiProjectionsContext _context;

        protected override ISorting Sorting => new DomainSorting();

        public AccountListQuery(ApiProjectionsContext context) => _context = context;

        protected override IQueryable<AccountList> Filter(FilteringHeader<AccountListFilter> filtering)
        {
            var accounts = _context
                .AccountList
                .AsNoTracking();

            if (!filtering.ShouldFilter)
                return accounts;

            if (filtering.Filter.Id.HasValue)
                accounts = accounts.Where(m => m.Id == filtering.Filter.Id.Value);

            return accounts;
        }

        internal class DomainSorting : ISorting
        {
            public IEnumerable<string> SortableFields { get; } = new[]
            {
                nameof(AccountList.Id),
            };

            public SortingHeader DefaultSortingHeader { get; } = new SortingHeader(nameof(AccountList.Id), SortOrder.Ascending);
        }
    }

    public class AccountListFilter
    {
        public Guid? Id { get; set; }
    }
}
