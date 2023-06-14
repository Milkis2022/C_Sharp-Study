## C# 형변환 키워드

### explicit
명시적인 형변환을 할 때 사용한다. 예시를 살펴보자
다음과 같이 섭씨와 화씨로 클래스를 만들었다고 가정하자 이 떄 섭씨에서 화씨로 혹은 화씨에서 섭씨로 변환이 되게 하고싶다.
```C#
class Celsius
    {
        private float degress;

        public Celsius(float degress)
        {
            this.degress = degress;
        }

        public float Degress
        {
            get { return this.degress; }
        }
    }

class Fahrenheit
    {
        private float degress;

        public Fahrenheit(float degress)
        {
            this.degress = degress;
        }

        public float Degress
        {
            get { return degress; }
        }
    }
```
위와 같은 클래스가 있다. 이 때 다음과 같은 형변환 시도는 불가능하다.

```C#
    Fahrenheit f = new Fahrenheit(100.0f);
    Celsius c = (Celsius)f;
```
이럴 때 다음과 같이 explicit operator를 정의해서 형변환을 시켜줄 수 있다.

```C#
        //Fahrenheit -> Celsius 명시적 형변환.
        public static explicit operator Celsius(Fahrenheit fahr)
        {
            return new Celsius((5.0f / 9.0f) * (fahr.degress - 32));
        }
```

전체 코드는

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Celsius
    {
        private float degress;

        public Celsius(float degress)
        {
            this.degress = degress;
        }
        //Celsius -> Fahrenheit 명시적 형변환.
        public static explicit operator Fahrenheit(Celsius c)
        {
            return new Fahrenheit((9.0f / 5.0f) * c.degress + 32);
        }
        public float Degress
        {
            get { return this.degress; }
        }
    }

    class Fahrenheit
    {
        private float degress;

        public Fahrenheit(float degress)
        {
            this.degress = degress;
        }
        //Fahrenheit -> Celsius 명시적 형변환.
        public static explicit operator Celsius(Fahrenheit fahr)
        {
            return new Celsius((5.0f / 9.0f) * (fahr.degress - 32));
        }
        public float Degress
        {
            get { return degress; }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {   
            // 명시적인 형변환이 가능하다.
            Fahrenheit f = new Fahrenheit(100.0f);
            Celsius c = (Celsius)f;
            Fahrenheit f2 = (Fahrenheit)c;
        }
    }
}

```


### implicit