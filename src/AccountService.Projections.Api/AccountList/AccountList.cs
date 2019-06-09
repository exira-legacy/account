namespace AccountService.Projections.Api.AccountList
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AccountList
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class AccountListConfiguration : IEntityTypeConfiguration<AccountList>
    {
        private const string TableName = "AccountList";

        public void Configure(EntityTypeBuilder<AccountList> b)
        {
            b.ToTable(TableName, Schema.Api)
                .HasKey(x => x.Id)
                .ForSqlServerIsClustered(false);

            b.Property(x => x.Name)
                .HasMaxLength(AccountName.MaxLength);

            b.HasIndex(x => x.Name)
                .ForSqlServerIsClustered();
        }
    }
}
