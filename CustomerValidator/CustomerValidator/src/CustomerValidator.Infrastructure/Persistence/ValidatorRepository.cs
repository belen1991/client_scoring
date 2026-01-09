using CustomerValidator.Domain.Models;

using CustomerValidator.Application.Interfaces;

namespace CustomerValidator.Infrastructure.Persistence;
public class ValidatorRepository : IValidatorRepository
{
    public Task SaveAsync(ValidationResult validationResult)
    {
        return Task.CompletedTask;
    }
}
