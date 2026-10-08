using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;

namespace Ordering.Application.Behaviors
{
    public class UnhandledExceptionCommandHandlerDecorator<TCommand, TResult>(ICommandHandler<TCommand, TResult> inner, ILogger<TCommand> logger) 
                : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                return await inner.Handle(command, cancellationToken);
            }
            catch (Exception)
            {
                var commandName = typeof(TCommand).Name;
                logger.LogError("An unhandled exception occurred while processing the command: {commandName}", commandName);
                throw;
            }
        }
    }
}
