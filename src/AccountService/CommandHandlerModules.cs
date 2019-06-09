namespace AccountService
{
    using Autofac;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Account;

    public static class CommandHandlerModules
    {
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<AccountCommandHandlerModule>()
                .Named<CommandHandlerModule>(typeof(AccountCommandHandlerModule).FullName)
                .As<CommandHandlerModule>();
        }
    }
}
