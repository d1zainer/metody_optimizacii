using org.mariuszgromada.math.mxparser;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OptimizationApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            double a, b, epsilon;
            if (!double.TryParse(TextBoxA.Text, out a) ||
                !double.TryParse(TextBoxB.Text, out b) ||
                !double.TryParse(TextBoxEpsilon.Text, out epsilon))
            {
                ResultTextBlock.Text = "Неверный формат входных данных!";
                return;
            }

            string functionString = TextBoxFunction.Text;
            Function userFunction = new Function("f(x) = " + functionString);

            Func<double, double> userFunc = (x) => userFunction.calculate(x);

            double resultDichotomy = DichotomyMethod(userFunc, a, b, epsilon);
            double resultFibonacci = FibonacciMethod(userFunc, a, b, (int)Math.Log((b - a) / epsilon, 1.618) - 1, epsilon);
            resultDichotomy = Math.Round(resultDichotomy, 3);
            resultFibonacci = Math.Round(resultFibonacci, 3);

            ResultTextBlock.Text = $"Минимум функции (дихотомия) на интервале [{a}, {b}] равен {resultDichotomy}\n" +
                                $"Минимум функции (Фибоначчи) на интервале [{a}, {b}] равен {resultFibonacci}";
        }


        static double DichotomyMethod(Func<double, double> func, double a, double b, double epsilon)
        {
            while (b - a > epsilon)
            {
                double c = (a + b) / 2;
                double delta = epsilon / 3;
                double f1 = func(c - delta);
                double f2 = func(c + delta);

                if (f1 < f2)
                    b = c;
                else
                    a = c;
            }

            return (a + b) / 2;
        }

        static double Fibonacci(int n)
        {
            if (n <= 1)
                return n;
            else
                return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        static double FibonacciMethod(Func<double, double> func, double a, double b, int numIterations, double epsilon)
        {
            for (int i = 0; i < numIterations; i++)
            {
                double fibNMinus2 = Fibonacci(numIterations - 2);
                double fibNMinus1 = Fibonacci(numIterations - 1);
                double fibN = fibNMinus1 + fibNMinus2;
                double x1 = a + (fibNMinus2 / fibN) * (b - a);
                double x2 = a + (fibNMinus1 / fibN) * (b - a);
                double fX1 = func(x1);
                double fX2 = func(x2);

                if (fX1 < fX2)
                {
                    b = x2;
                }
                else
                {
                    a = x1;
                }
            }

            return (a + b) / 2;
        }

        
    }
}
