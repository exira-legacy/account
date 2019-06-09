namespace AccountService.Projections.Api.AccountDetail
{
    using System;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AccountDetail
    {
        public Guid Id { get; set; }

        public string NameDutch { get; set; }
        public string NameFrench { get; set; }
        public string NameEnglish { get; set; }
        public string NameGerman { get; set; }
    }

    public class AccountDetailConfiguration : IEntityTypeConfiguration<AccountDetail>
    {
        private const string TableName = "AccountDetails";

        public void Configure(EntityTypeBuilder<AccountDetail> b)
        {
            b.ToTable(TableName, Schema.Api)
                .HasKey(x => x.Id)
                .ForSqlServerIsClustered(false);

            b.Property(x => x.NameDutch)
                .HasMaxLength(AccountName.MaxLength);

            b.Property(x => x.NameFrench)
                .HasMaxLength(AccountName.MaxLength);

            b.Property(x => x.NameEnglish)
                .HasMaxLength(AccountName.MaxLength);

            b.Property(x => x.NameGerman)
                .HasMaxLength(AccountName.MaxLength);

            b.HasIndex(x => x.NameDutch)
                .ForSqlServerIsClustered();
        }
    }
}
