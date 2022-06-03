// See https://aka.ms/new-console-template for more information


namespace Prog
{
    enum calc { one, two }
    public static class Program
    {
        static void Main(string[] args)
        {
            M(calc.two);
            Console.ReadLine();
        }
        static void M(calc calc)
        {
            Console.WriteLine(calc.ToString());
        }
    }
}
