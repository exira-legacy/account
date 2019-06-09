namespace AccountService.Infrastructure.Modules
{
    using Autofac;
    using Account;
    using Repositories;

    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<Accounts>()
                .As<IAccounts>();
        }
    }
}
