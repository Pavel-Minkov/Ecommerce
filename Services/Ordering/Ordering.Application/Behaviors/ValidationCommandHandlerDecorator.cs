using FluentValidation;
using Ordering.Application.Abstractions;

namespace Ordering.Application.Behaviors
{
    public class ValidationCommandHandlerDecorator<TCommand, TResult>(ICommandHandler<TCommand, TResult> inner, IEnumerable<IValidator<TCommand>> validators) 
        : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {

        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TCommand>(command);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
            return await inner.Handle(command, cancellationToken);
        }
    }
}
