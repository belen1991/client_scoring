using CustomerValidator.Domain.Services;

namespace CustomerValidator.Application.UnitTests
{
    public class DecisionEngineTests
    {
        [Fact]
        public void Evaluate_ShouldApprove_WhenScoreIsGreaterOrEqual700()
        {
            // Arrange
            var engine = new DecisionEngine();
            var score = 750;
            var amount = 500m;

            // Act
            var result = engine.Evaluate(score, amount);

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void Evaluate_ShouldApprove_WhenScoreBetween500And699_AndAmountLessThan1000()
        {
            // Arrange
            var engine = new DecisionEngine();
            var score = 600;
            var amount = 999m;

            // Act
            var result = engine.Evaluate(score, amount);

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public void Evaluate_ShouldDenied_WhenScore200()
        {
            // Arrange
            var engine = new DecisionEngine();
            var score = 200;
            var amount = 999m;

            // Act
            var result = engine.Evaluate(score, amount);

            // Assert
            Assert.Equal(false, result);
        }
    }
}
