
using CustomerValidator.Application.Dtos;
using CustomerValidator.Application.Interfaces;
using CustomerValidator.Domain.Services;
using CustomerValidator.Domain.Models;

namespace CustomerValidator.Application.UseCases;
public class ValidatorUseCase : IValidatorUseCase
{
    private IFinScore _finScoreClient;
    private IValidatorRepository _validatorRepository;
    private DecisionEngine _decisionEngine;
    public ValidatorUseCase(
        IFinScore finScoreClient,
        IValidatorRepository validatorRepository,
        DecisionEngine decisionEngine)
    {
        _decisionEngine = decisionEngine;
        _finScoreClient = finScoreClient;
        _validatorRepository = validatorRepository;
    }

    public async Task<ValidatorResponseDto> HandleAsync(
        ValidatorRequestDto request,
        CancellationToken ct)
    {
        var score = await _finScoreClient.GetScoreAsync(
            request.Name,
            request.Identification,
            ct);

        var result = _decisionEngine.Evaluate(score, request.Amount);

        var validationResult = new ValidationResult
        {
            Decision = result,
            Score = score,
            Request = new RequestData
            {
                Identification = request.Identification,
                Name = request.Name
            }
        };

        await _validatorRepository.SaveAsync(
            validationResult);

        return new ValidatorResponseDto
        {
            Decision = result,
            Score = score,
        };
    }
}
