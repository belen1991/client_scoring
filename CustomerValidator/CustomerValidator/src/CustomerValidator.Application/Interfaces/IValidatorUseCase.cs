
using CustomerValidator.Application.Dtos;

namespace CustomerValidator.Application.Interfaces;
public interface IValidatorUseCase
{
    Task<ValidatorResponseDto> HandleAsync(
        ValidatorRequestDto request,
        CancellationToken ct);
}
