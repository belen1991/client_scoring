namespace CustomerValidator.SharedKernel.Errors;

public record NotFoundError(string Code, string Message) : Error(Code, Message)
{
}
