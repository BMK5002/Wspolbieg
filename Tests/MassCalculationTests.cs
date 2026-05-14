using Data;
using Model;

namespace PhysicsTests
{
    /// <summary>
    /// Tests for mass calculation and momentum-based physics.
    /// Verifies that mass is correctly calculated from radius using the formula: m = π * r²
    /// </summary>
    public class MassCalculationTests
    {
        private const double Epsilon = 1e-9;

        [Fact]
        public void MassCalculation_CorrectFormula_CircleArea()
        {
            // Arrange
            double radius = 5.0;
            double expectedMass = Math.PI * radius * radius;

            // Act
            var ball = new Ball(radius);

            // Assert
            Assert.Equal(expectedMass, ball.Mass, precision: 10);
        }

        [Fact]
        public void MassCalculation_VariousRadii_ProportionalToRadiusSquared()
        {
            // Arrange & Act & Assert
            var testCases = new[] { 1.0, 2.5, 5.0, 10.0, 20.0 };

            foreach (var radius in testCases)
            {
                var ball = new Ball(radius);
                double expectedMass = Math.PI * radius * radius;
                Assert.Equal(expectedMass, ball.Mass, precision: 10);
            }
        }

        [Fact]
        public void MassCalculation_SmallRadius_CorrectValue()
        {
            // Arrange
            double radius = 0.5;
            double expectedMass = Math.PI * 0.25;

            // Act
            var ball = new Ball(radius);

            // Assert
            Assert.Equal(expectedMass, ball.Mass, precision: 10);
        }

        [Fact]
        public void MassCalculation_LargeRadius_CorrectValue()
        {
            // Arrange
            double radius = 100.0;
            double expectedMass = Math.PI * 10000.0;

            // Act
            var ball = new Ball(radius);

            // Assert
            Assert.Equal(expectedMass, ball.Mass, precision: 8);
        }

        [Fact]
        public void MassRelationship_DoubleRadiusQuadruplesMass()
        {
            // Arrange
            var ball1 = new Ball(radius: 5.0);
            var ball2 = new Ball(radius: 10.0);

            // Act & Assert
            // mass = π * r²
            // If r doubles: m' = π * (2r)² = π * 4r² = 4m
            Assert.Equal(ball2.Mass, ball1.Mass * 4, precision: 10);
        }

        [Fact]
        public void InitialMomentumConstant_Defined()
        {
            // Assert
            Assert.Equal(100.0, Ball.InitialMomentum);
        }

        [Fact]
        public void MassIsReadOnly_CalculatedFromRadius()
        {
            // Arrange
            var ball = new Ball(radius: 5.0);
            double originalMass = ball.Mass;

            // Act: Modify radius
            ball.R = 10.0;
            double newMass = ball.Mass;

            // Assert: Mass recalculates based on new radius
            Assert.NotEqual(originalMass, newMass);
            Assert.Equal(Math.PI * 100.0, newMass, precision: 10);
        }
    }
}
