namespace CommsManager.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    
    public NotFoundException(string entityName, Guid id) 
        : base($"{entityName} with ID '{id}' not found.") { }
}
