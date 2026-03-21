namespace Model
{
    public class Calculator
    {
        private double value1;
        private double value2;
        public Calculator(double value1, double value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public double add()
        {
            return value1 + value2;
        }

        public double substract()
        {
            return value1 - value2;
        }

        public double multiply()
        {
            return value1 * value2;
        }

        public double divide()
        {
            if (value2 == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return value1 / value2;
        }

        public void setValue1(double value)
        {
            value1 = value;
        }

        public void setValue2(double value)
        { 
            value2 = value;
        }
    }
}
