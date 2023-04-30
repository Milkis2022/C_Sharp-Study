using System;

namespace Extension_study
{
    public class Calculator
    {
        public void Add()
        {
            Console.WriteLine("Add() function called");
        }
        public void Subtract()
        {
            Console.WriteLine("Subtract() function called");
        }

        public void Multiply()
        {
            Console.WriteLine("Multiply() function called");
        }
    }

    public static class Extension
    {   

        // static 메서드로 선언하며 매개변수로는 (this ClassName Name) 형식으로 작성
        // 정적클래스에 선언하며 정적 메서드로 선언하지만 인스턴스 메서드처럼 사용함
        public static void Divide(this Calculator cal)
        {
            Console.WriteLine("Divide() function called");
        }
        
        // string 에 대한 확장 메서드

        public static void Hello(this string str)
        {
            Console.WriteLine($"{str} Hello!");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Calculator cal = new Calculator();
            cal.Add();
            cal.Divide();               // 확장 메서드로 추가한 메서드

            string jerry = "Jerry";
            jerry.Hello();              // 확장 메서드로 추가한 메서드
        }
    }

}

