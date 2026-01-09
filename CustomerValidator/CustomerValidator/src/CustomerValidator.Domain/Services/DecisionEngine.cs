namespace CustomerValidator.Domain.Services;
public class DecisionEngine
{
    public bool Evaluate(int? score, decimal amount)
    {
        if (score == null)
        {
            return false;
        }

        if (score >= 700)
        {
            return true;
        }

        if (score >= 500 && score<= 699 && amount < 1000)
        {
            return true;
        }

        return false;
    }
}
