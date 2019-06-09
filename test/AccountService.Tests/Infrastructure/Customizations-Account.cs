namespace AccountService.Tests.Infrastructure
{
    using AutoFixture;

    public static partial class Customizations
    {
        public static void CustomizeAccountName(this IFixture fixture) =>
            fixture.Customize<AccountName>(composer =>
                composer.FromFactory(generator =>
                    new AccountName(new string(
                            (char) generator.Next(97, 123), // a-z
                            generator.Next(1, AccountName.MaxLength)),
                        Language.Dutch)));
    }
}
