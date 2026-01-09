using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using CustomerValidator.Application.Interfaces;
using CustomerValidator.Application.Dtos;

namespace CustomerValidator.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ValidatorController : ControllerBase
{
    private readonly IValidatorUseCase _usecase;
    private readonly ILogger _logger;

    public ValidatorController(
        ILogger<ValidatorController> logger,
        IValidatorUseCase usecase)
    {
        _usecase = usecase;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] ValidatorRequestDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Inicio de Validator.Check con request: {@request}", request);

        var result = await _usecase.HandleAsync(request, cancellationToken);

        return Ok(result);
    }

}
