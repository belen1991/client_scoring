namespace CustomerValidator.Application.Interfaces;
public interface IFinScore
{
    /// <summary>
    /// Method external that retreives the score of a client validation
    /// </summary>
    /// <param name="name"></param>
    /// <param name="identificator"></param>
    /// <param name="cs"></param>
    /// <returns></returns>
    Task<int?> GetScoreAsync(string name, string identificator, CancellationToken cs);
}
