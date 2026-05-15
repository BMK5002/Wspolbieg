using Data;

namespace Model
{
    public static class Physics
    {
        public static double GetDistance(Ball a, Ball b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static void ResolveElasticCollision(Ball a, Ball b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < 1e-10)
            {
                return;
            }

            double nx = dx / distance;
            double ny = dy / distance;

            double vRelX = a.VelocityX - b.VelocityX;
            double vRelY = a.VelocityY - b.VelocityY;

            double vRelN = vRelX * nx + vRelY * ny;

            if (vRelN >= 0)
            {
                return;
            }

            double massA = a.Mass;
            double massB = b.Mass;

            double impulse = -2.0 * vRelN / (1.0 / massA + 1.0 / massB);

            a.VelocityX += (impulse / massA) * nx;
            a.VelocityY += (impulse / massA) * ny;

            b.VelocityX -= (impulse / massB) * nx;
            b.VelocityY -= (impulse / massB) * ny;
        }

        public static void SeparateOverlappingBalls(Ball a, Ball b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < 1e-10)
            {
                return;
            }

            double nx = dx / distance;
            double ny = dy / distance;

            double minDistance = a.R + b.R;
            double penetration = minDistance - distance;

            if (penetration <= 0)
            {
                return;
            }

            double totalInverseMass = 1.0 / a.Mass + 1.0 / b.Mass;
            double separationA = (penetration / 2.0) * (1.0 / a.Mass) / totalInverseMass;
            double separationB = (penetration / 2.0) * (1.0 / b.Mass) / totalInverseMass;

            a.X += nx * separationA;
            a.Y += ny * separationA;

            b.X -= nx * separationB;
            b.Y -= ny * separationB;
        }
    }
}
