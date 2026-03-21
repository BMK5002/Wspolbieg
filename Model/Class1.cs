namespace Model
{
    public class Calculator
    {
        private int value1;
        private int value2;
        public Calculator(int value1, int value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public int add()
        {
            return value1 + value2;
        }

        public int substract()
        {
            return value1 - value2;
        }

        public int multiply()
        {
            return value1 * value2;
        }

        public int divide()
        {
            if (value2 == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return value1 / value2;
        }

        public void setValue1(int value)
        {
            value1 = value;
        }

        public void setValue2(int value)
        { 
            value2 = value;
        }
    }
}
