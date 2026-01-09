
using CustomerValidator.Domain.Models;

namespace CustomerValidator.Application.Interfaces;
public interface IValidatorRepository
{
    Task SaveAsync(ValidationResult validationResult);
}
