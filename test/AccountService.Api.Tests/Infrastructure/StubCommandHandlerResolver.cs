namespace AccountService.Api.Tests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.CommandHandling;

    public class StubCommandHandlerResolver : ICommandHandlerResolver
    {
        public List<object> ReceivedCommands = new List<object>();

        public IEnumerable<Type> KnownCommandTypes { get; }

        public ReturnHandler<CommandMessage<TCommand>> Resolve<TCommand>() where TCommand : class
        {
            #pragma warning disable 1998
            ReturnHandler<CommandMessage<TCommand>> handler = async (msg, ct) =>
            {
                ReceivedCommands.Add(msg);

                return -1L;
            };
            #pragma warning restore 1998

            return handler;
        }
    }
}
