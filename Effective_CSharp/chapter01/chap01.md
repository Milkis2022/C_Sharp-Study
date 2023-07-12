## 지역변수를 선언할 떄는 var를 사용하는 것이 낫다.
지역변수의 타입을 암시적으로 선언하는 것이 좋은 이유는 C#이라는 언어가 익명 타입을 지원하기 위해서 타입을 암시적으로 선언할 수 있는 쉬운 방법을 제공해주기 떄문입니다. 또한 일부 쿼리 구문의 경우 IEnumerable<T>를 반환하는 경우도 있지만 IQueryable<T>를 반환하기도 하는데, 정확한 반환 타입을 알지 못한 채 올바르지 않은 타입을 명시적으로 지정하게 되면 득보다 실이 많습니다. 코드를 읽을 떄도 var를 사용하여 암시적으로 변수를 선언한 코드가 더 잘 읽힙니다.
예를 들어 **Dictionary<int, Queue<string>>** 과 같이 정확히 선언되어 있는 타입 보다 jobsQueueByRegion과 같이 타입을 유추할 수 있는 변수의 이름이 더 큰 도움이 됩니다.

지역변수에 대한 타입 추론이 C#같은 정적 타이핑언어의 고유 특성을 훼손하는 것은 아닙니다.
이를 이해하기 위해 먼저 지역변수에 대한 타입 추론과 동적 타이핑이 서로 다른 것임을 알아야 합니다.
C#에서 특정 변수를 var로 선언하면 동적 타이핑이 수행되는 것이 아니라 할당 연산자 오른쪽의 타입을 확인하여 왼쪽 변수의 타입을 결정하게 된다. 컴파일러에게 변수의 타입을 명시적으로 알려주지 않아도 개발자를 대신하여 올바른 타입을 추론해주는 것이다.

타입을 명시적으로 지정하면 잘못된 동작을 미연에 방지하는 효과가 있을 때도 있다.하지만 대체로 var를 사용하여 컴파일러에게 적절한 타입을 선택하도록 위임하는 편이 더 나은 결과를 보여줄 때가 많다. 간혹 var를 과도하게 사용한 나머지 코드의 가독성을 해치는 경우도 있고, 내부적으로 이루어지는 자동 타입 변환 과정으로 인해 발견하기 어려운 버그를 만들 경우도 없지 않다.

가독성 문제를 유발하는 몇가지 예를 확인해보자.
```C#
var foo = new MyType();
```
경험 있는 개발자라면 앞의 예에서 foo가 어떤 타입으로 추론될지 쉽게 짐작할 수 있을 것이다.
팩토리 메서드를 사용하는 경우에도 지역변수가 어떤 타입으로 추론될지 어렵지 않게 유추할 수 있다.

```C#
var thing = AccountFactory.CreateSavingsAccount();
```

이와 달리 메서드 이름만으로는 반환 타입을 짐작하기 어려운 경우도 있다.
```C#
var result = someObject.DoSomething(parameter);
```
물론 위 코드보다 훨씬 명확하게 메서드의 이름을 작명할 것이며 또는 그래야 한다. 하지만 변수명을 조금 달리한다면 그 의미를 더욱 명확하게 전달할 수 있다.

```C#
var HighestSellingProduct = someObject.Dosomething(parameter);
```

위 코드에서 타입과 관련된 정보가 없지만 변수명에서 Product 타입임을 짐작할 수 있다.
물론 메서드를 어떻게 작성했냐에 따라서 Product 타입이 아닐 수도 Product 타입을 상속한 다른 타입일 수도 또는 Product 인터페이스를 구현한 타입일 수도 있다.하지만 컴파일러는 위 DoSomething 메서드 정의에 부합하도록 HighestSellingProduct 변수의 타입을 추론한다. var를 사용한 경우에는 어떤 타입으로 추론될지를 직접 눈으로 확인할 수는 없다.간혹 개발자가 짐작한 타입과 컴파일러가 추론한 타입이 일치하지 않아서 문제가 되는 경우도 있다. 이로 인해 미묘한 버그가 발생해 코드 수정이 쉽지 않을 수 있다.

위 예시로 내장 숫자 타입과 var를 함꼐 사용한 경우를 살펴보자. 원시 타입들 간에는 다양한 변환 연산이 자동으로 수행된다.
float에서 double로의 변환과 같이 확대 변환은 항상 안전하게 수행된다. 반면 long에서 int로의 변환과 같이 축소 변환은 정밀도에서 손실이 발생한다.

```C#
        public static float GetMagicNumber()
        {
            return 3.14f;
        }

        public static void Main(string[] args) 
        {
            var f = GetMagicNumber();
            var total = 100 * f / 6;
            Console.WriteLine($"Declared Type: {total.GetType().Name}, Value:{total}");
        }
```

위 코드에서 total은 무슨 타입일까? total의 타입은 GetMagicNumber() 메서드의 반환 타입에 의해 결정될 것이다.
컴파일러는 변수 f의 타입을 GetMagicNumber() 메서드의 반환 타입으로 f의 타입을 결정한다. 
```C#
        public static double GetMagicNumber()
        {
            return 3.14f;
        }

        public static void Main(string[] args) 
        {
            var f = GetMagicNumber();
            var total = 100 * f / 6;
            Console.WriteLine($"Declared Type: {total.GetType().Name}, Value:{total}");
        }
```
[결과값]
```bash
Declared Type: Single, Value:52.33334
Declared Type: Double, Value:52.3333350817362
```

total 계산 시에 사용한 상수는 모두 리터럴이므로 컴파일러가 이 상수들을 f와 동일한 타입으로 변환한 후 계산하게 되는데 이런 이유로 결괏값에 차이가 생긴다. 이는 언어 차원의 문제는 아니다. C# 컴파일러 입장에서는 개발자의 요청을 정확히 수행했다고 볼 수 있다. 개발자가 var를 사용하여 타입 추론을 컴파일러에게 위임한 경우, 컴파일러는 할당문 오른쪽의 내용을 기반으로 타입을 결정하기 때문이다. 이러한 이유로 원시 타입과 var를 함께 사용할 때는 항상 주의해야한다.
앞의 코드만 들여다 봤을 때는 GetMagicNumber() 메서드의 반환 타입을 정확히 추론하기 어렵고, 내장된 형변환 기능이 함께 동작하기 때문에 그 결과를 예상하기 힘들다.

total의 타입을 명시적으로 선언하면 이 문제를 해결할 수 있긴하다.
```C#
    var f = GetMagicNumber();
    double total = 100 * f / 6;
    Console.WriteLine($"Declared Type: {total.GetType().Name}, Value:{total}");
```

위 코드에서 total의 타입은 double이다.하지만 GetMagicNumber() 메서드에서 정수 타입을 반환하면 잘림이 발생할 수 있다.
여기에서 문제는 위 코드만 보고선 GetMagicNumber()의 반환 타입을 알 수 없으므로 어떤 반환이 수행될지 짐작할 수 없다는 점이다.

이처럼 var를 사용하면 코드의 유지보수가 더 어려운 경우도 발생할 수 있다. 컴파일러는 일관된 방식으로 타입 추론을 수행하겠지만 개발자 입장에서는 내부적으로 이뤄지는 타입 추론 과정과 암시적인 형변환 과정을 이해하기 어렵기 때문이다. 이러한 이유로 지연변수에 대해 var를 사용하는 것이 적절하지 않다고 말하곤 하지만 이는 너무 가혹하다.

때로는 변수의 타입을 컴파일러에게 추론하게 맡기는 편이 더 낫기 떄문이다.다음 코드를 살펴보자

```C#


```
