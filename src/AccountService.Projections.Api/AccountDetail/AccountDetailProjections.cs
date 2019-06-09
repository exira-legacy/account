namespace AccountService.Projections.Api.AccountDetail
{
    using System;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Account.Events;

    public class AccountDetailProjections : ConnectedProjection<ApiProjectionsContext>
    {
        public AccountDetailProjections()
        {
            When<Envelope<AccountWasBorn>>(async (context, message, ct) =>
            {
                await context
                    .AccountDetails
                    .AddAsync(
                        new AccountDetail
                        {
                            Id = message.Message.AccountId,
                        }, ct);
            });

            When<Envelope<AccountWasNamed>>(async (context, message, ct) =>
                await context.FindAndUpdateAccountDetail(
                    message.Message.AccountId,
                    account =>
                    {
                        switch (message.Message.Language)
                        {
                            case Language.Dutch:
                                account.NameDutch = message.Message.Name;
                                break;

                            case Language.French:
                                account.NameFrench = message.Message.Name;
                                break;

                            case Language.German:
                                account.NameGerman = message.Message.Name;
                                break;

                            case Language.English:
                                account.NameEnglish = message.Message.Name;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(message.Message.Language));
                        }
                    },
                    ct));
        }
    }
}
