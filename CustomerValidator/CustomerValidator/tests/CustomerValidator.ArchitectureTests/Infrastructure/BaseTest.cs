using System.Reflection;

namespace CustomerValidator.ArchitectureTests.Infrastructure;

public class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
    protected static readonly Assembly DomainAssembly = typeof(CustomerValidator.Domain.Abstractions.Result).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(CustomerValidator.Infrastructure.DependencyInjection).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
