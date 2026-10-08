namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException(string name, object key) : Exception($"Entity \"{name}\" with key \"{key}\" was not found.");
}
