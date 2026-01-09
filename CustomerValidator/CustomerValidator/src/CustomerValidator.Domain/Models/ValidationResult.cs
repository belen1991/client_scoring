namespace CustomerValidator.Domain.Models;
public class ValidationResult
{
    public int? Score { get; set; }
    public bool Decision { get; set; }
    public RequestData Request { get; set; }
    public string Response { get; set; }
}

public class RequestData
{
    public string Name { get; set; }
    public string Identification { get; set; }
    public decimal Amount { get; set; }
}
