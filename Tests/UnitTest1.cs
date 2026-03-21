using System;
using Model;
using Xunit;

namespace Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_ReturnsSum_ForDoubles()
        {
            var calc = new Calculator(1.5, 2.25);
            var result = calc.add();
            Assert.Equal(3.75, result, 5);
        }

        [Fact]
        public void Substract_ReturnsDifference_ForDoubles()
        {
            var calc = new Calculator(5.5, 2.0);
            var result = calc.substract();
            Assert.Equal(3.5, result, 5);
        }

        [Fact]
        public void Multiply_ReturnsProduct_ForDoubles()
        {
            var calc = new Calculator(2.5, 4.0);
            var result = calc.multiply();
            Assert.Equal(10.0, result, 5);
        }

        [Fact]
        public void Divide_ReturnsQuotient_ForDoubles()
        {
            var calc = new Calculator(7.5, 2.5);
            var result = calc.divide();
            Assert.Equal(3.0, result, 5);
        }

        [Fact]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            var calc = new Calculator(1.0, 0.0);
            Assert.Throws<DivideByZeroException>(() => calc.divide());
        }

        [Fact]
        public void SetValues_UpdateCalculation()
        {
            var calc = new Calculator(1.0, 1.0);
            calc.setValue1(3.0);
            calc.setValue2(4.0);
            Assert.Equal(7.0, calc.add(), 5);
            Assert.Equal(-1.0, calc.substract(), 5);
            Assert.Equal(12.0, calc.multiply(), 5);
            Assert.Equal(0.75, calc.divide(), 5);
        }
    }
}
