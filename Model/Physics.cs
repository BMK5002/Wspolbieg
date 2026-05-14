using Data;

namespace Model
{
    /// <summary>
    /// Minimal physics helper for ideal 2D elastic collisions (no friction, no energy loss).
    /// Assumes perfectly elastic collisions (restitution = 1.0) with momentum conservation.
    /// </summary>
    public static class Physics
    {
        /// <summary>
        /// Calculates the Euclidean distance between the centers of two balls.
        /// </summary>
        public static double GetDistance(Ball a, Ball b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Resolves an elastic collision between two balls using impulse-based response.
        /// Updates both balls' velocities to conserve momentum and energy.
        /// 
        /// For perfectly elastic collisions (e=1.0), the impulse scalar formula is:
        /// J = -2 * (vRel · n) / (1/mA + 1/mB)
        /// 
        /// where:
        /// - vRel = relative velocity (vA - vB)
        /// - n = collision normal (unit vector from B to A)
        /// - mA, mB = masses of balls A and B
        /// </summary>
        public static void ResolveElasticCollision(Ball a, Ball b)
        {
            // Compute collision normal (unit vector from b to a)
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Safety check: avoid division by zero
            if (distance < 1e-10)
            {
                return;
            }

            double nx = dx / distance;
            double ny = dy / distance;

            // Compute relative velocity (a relative to b)
            double vRelX = a.VelocityX - b.VelocityX;
            double vRelY = a.VelocityY - b.VelocityY;

            // Compute relative velocity along the collision normal
            double vRelN = vRelX * nx + vRelY * ny;

            // If relative velocity is non-negative, objects are separating (no collision response needed)
            if (vRelN >= 0)
            {
                return;
            }

            // Get masses
            double massA = a.Mass;
            double massB = b.Mass;

            // Compute impulse scalar for perfectly elastic collision (e = 1.0)
            // J = -2 * vRelN / (1/mA + 1/mB)
            double impulse = -2.0 * vRelN / (1.0 / massA + 1.0 / massB);

            // Apply impulse to velocities
            // vA += (J / mA) * n
            // vB -= (J / mB) * n
            a.VelocityX += (impulse / massA) * nx;
            a.VelocityY += (impulse / massA) * ny;

            b.VelocityX -= (impulse / massB) * nx;
            b.VelocityY -= (impulse / massB) * ny;
        }

        /// <summary>
        /// Separates two overlapping balls to prevent sticking.
        /// Moves each ball along the collision normal proportional to the inverse of its mass.
        /// </summary>
        public static void SeparateOverlappingBalls(Ball a, Ball b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Safety check: avoid division by zero
            if (distance < 1e-10)
            {
                return;
            }

            double nx = dx / distance;
            double ny = dy / distance;

            double minDistance = a.R + b.R;
            double penetration = minDistance - distance;

            // Only separate if there's actual penetration
            if (penetration <= 0)
            {
                return;
            }

            // Distribute separation inversely proportional to mass
            // Total separation: a moves by (penetration / 2) + (penetration / 2)
            // But we distribute based on mass ratio
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
