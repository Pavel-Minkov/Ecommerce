namespace Catalog.Application.Exceptions
{
    public class CustomNotFoundException(string name, object key) : Exception($"{name} with Id \"{key}\" was not found.");
}
