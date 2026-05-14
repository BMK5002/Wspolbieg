using Data;

namespace PhysicsTests
{
    /// <summary>
    /// Tests for Ball drawing properties and property notifications.
    /// Verifies that DrawX and DrawY correctly account for ball radius.
    /// </summary>
    public class BallDrawingTests
    {
        private const double Epsilon = 1e-9;

        [Fact]
        public void DrawX_CorrectFormula_PositionMinusRadius()
        {
            // Arrange
            var ball = new Ball { X = 50, R = 5 };

            // Act
            double drawX = ball.DrawX;

            // Assert: DrawX should be X - R
            Assert.Equal(45, drawX);
        }

        [Fact]
        public void DrawY_CorrectFormula_PositionMinusRadius()
        {
            // Arrange
            var ball = new Ball { Y = 100, R = 8 };

            // Act
            double drawY = ball.DrawY;

            // Assert: DrawY should be Y - R
            Assert.Equal(92, drawY);
        }

        [Fact]
        public void DrawX_UpdatesWhenXChanges()
        {
            // Arrange
            var ball = new Ball { X = 10, R = 5 };
            double initialDrawX = ball.DrawX;

            // Act
            ball.X = 20;

            // Assert
            double updatedDrawX = ball.DrawX;
            Assert.Equal(5, initialDrawX);
            Assert.Equal(15, updatedDrawX);
        }

        [Fact]
        public void DrawY_UpdatesWhenYChanges()
        {
            // Arrange
            var ball = new Ball { Y = 10, R = 5 };
            double initialDrawY = ball.DrawY;

            // Act
            ball.Y = 20;

            // Assert
            double updatedDrawY = ball.DrawY;
            Assert.Equal(5, initialDrawY);
            Assert.Equal(15, updatedDrawY);
        }

        [Fact]
        public void DrawX_UpdatesWhenRadiusChanges()
        {
            // Arrange
            var ball = new Ball { X = 50, R = 5 };
            double initialDrawX = ball.DrawX;

            // Act
            ball.R = 10;

            // Assert
            double updatedDrawX = ball.DrawX;
            Assert.Equal(45, initialDrawX);
            Assert.Equal(40, updatedDrawX);
        }

        [Fact]
        public void DrawY_UpdatesWhenRadiusChanges()
        {
            // Arrange
            var ball = new Ball { Y = 100, R = 5 };
            double initialDrawY = ball.DrawY;

            // Act
            ball.R = 10;

            // Assert
            double updatedDrawY = ball.DrawY;
            Assert.Equal(95, initialDrawY);
            Assert.Equal(90, updatedDrawY);
        }

        [Fact]
        public void DrawXY_VariousRadii_CorrectOffsets()
        {
            // Arrange
            var testCases = new[] { 1.0, 2.5, 5.0, 10.0, 20.0 };

            foreach (var radius in testCases)
            {
                // Act
                var ball = new Ball { X = 100, Y = 200, R = radius };

                // Assert
                Assert.Equal(100 - radius, ball.DrawX);
                Assert.Equal(200 - radius, ball.DrawY);
            }
        }

        [Fact]
        public void DrawX_NegativePosition_Correct()
        {
            // Arrange
            var ball = new Ball { X = -10, R = 5 };

            // Act
            double drawX = ball.DrawX;

            // Assert
            Assert.Equal(-15, drawX);
        }

        [Fact]
        public void DrawY_NegativePosition_Correct()
        {
            // Arrange
            var ball = new Ball { Y = -20, R = 5 };

            // Act
            double drawY = ball.DrawY;

            // Assert
            Assert.Equal(-25, drawY);
        }

        [Fact]
        public void DrawXY_WithConstructor_CorrectInitialization()
        {
            // Arrange & Act
            var ball = new Ball(radius: 5.0, x: 50, y: 75);

            // Assert
            Assert.Equal(45, ball.DrawX);
            Assert.Equal(70, ball.DrawY);
        }

        [Fact]
        public void PropertyNotification_XChanges_RaisesPropertyChanged()
        {
            // Arrange
            var ball = new Ball { R = 5 };
            bool xChanged = false;
            bool drawXChanged = false;

            ball.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Ball.X)) xChanged = true;
                if (e.PropertyName == nameof(Ball.DrawX)) drawXChanged = true;
            };

            // Act
            ball.X = 50;

            // Assert
            Assert.True(xChanged);
            Assert.True(drawXChanged);
        }

        [Fact]
        public void PropertyNotification_YChanges_RaisesPropertyChanged()
        {
            // Arrange
            var ball = new Ball { R = 5 };
            bool yChanged = false;
            bool drawYChanged = false;

            ball.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Ball.Y)) yChanged = true;
                if (e.PropertyName == nameof(Ball.DrawY)) drawYChanged = true;
            };

            // Act
            ball.Y = 100;

            // Assert
            Assert.True(yChanged);
            Assert.True(drawYChanged);
        }

        [Fact]
        public void PropertyNotification_RadiusChanges_NotifiesXAndY()
        {
            // Arrange
            var ball = new Ball { X = 50, Y = 100 };
            bool rChanged = false;
            bool drawXChanged = false;
            bool drawYChanged = false;

            ball.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Ball.R)) rChanged = true;
                if (e.PropertyName == nameof(Ball.DrawX)) drawXChanged = true;
                if (e.PropertyName == nameof(Ball.DrawY)) drawYChanged = true;
            };

            // Act
            ball.R = 10;

            // Assert
            Assert.True(rChanged);
            Assert.True(drawXChanged);
            Assert.True(drawYChanged);
        }

        [Fact]
        public void DrawProperties_NoNotificationIfValueUnchanged_XDoesNotNotify()
        {
            // Arrange
            var ball = new Ball { X = 50 };
            int notificationCount = 0;

            ball.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Ball.X)) notificationCount++;
            };

            // Act: Set same value
            ball.X = 50;

            // Assert: Should not notify if value is the same
            Assert.Equal(0, notificationCount);
        }
    }
}
