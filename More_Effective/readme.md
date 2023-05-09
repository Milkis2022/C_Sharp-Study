
### 데이터 타입

#### 접근 가능한 데이터 멤버 대신 속성을 사용하라
속성은 C# 초기부터 지원한 기능이지만 여러 측면에서 보완되면서 지금은 표현력이 더 풍부해졌다. 예를 들면 게터와 세터의 접근 제한을 다르게 설정할 수 있게 되었다. 자동 속성을 사용하면 데이터 멤버를 명시적으로 선언할 필요가 없어서 타이핑의 수고로움을 덜어준다. 식 본문 멤버는 구문을 더 간결하게 작성할 수 있게 도와준다.
아직도 타입의 필드를 public으로 선언한다면 그만두는 게 좋다. get,set 메서드를 직접 작성하고 있었다면 이 역시 그만두는 것이 좋다. 프로퍼티를 사용하면 데이터 멤버를 public 으로 노출하면서 객체 지향에 필요한 캡슐화를 유지할 수 있다. 속성은 데이터 멤버처럼 접근할 수 있지만 실제로는 메서드로 구현된 언어 요소이다.

```C#
public class Customer
{
    private string name;
    public string Name
    {
        get => name;
        set
        {
            if(string.IsNullOrWhitespace(value))
                throw new ArgumentException(
                    "Name cannot the blank",nameof(Name));
            
            name = value;
        }
        // dosomething..
    }
}
```
만약 public 데이터 멤버를 사용했다면 전체 코드를 살펴보고 고객 이름을 설정하는 코드를 모두 찾아 수정해야 하는데 코드를 수정하는 시간보다 수정해야 하는 코드를 찾는 시간이 훨씬 많이 소비될 것이다.
속성은 메서드로 구현되므로 멀티스레드도 쉽게 지원할 수 있다. get과 set 접근자에 동기화 기능을 구현해주기만 하면 된다.
```C#
public class Customer
{
    private string name;
    public string Name
    {
        get
        {
            lock(syncHandle)
                return name;
        }
        set
        {
            if(string.IsNullOrWhitespace(value))
                throw new ArgumentException(
                    "Name cannot the blank",nameof(Name));
            
            lock(syncHandle)
                name = value;
        }
        // dosomething..
    }
}
```

속성은 메서드와 매우 유사해서 속성을 virtual로도 설정할 수 있다.

```C#
public class Customer
{
    public virtual string Name
    {
        get;
        set;
    }
}
```

앞의 몇몇 예제에서 사용한 암묵적 속성에 대해서도 살펴보자 데이터를 저장하는 저장소를 속성을 이용하여 감싸는 방식은 일반적인 패턴인데, 게터와 세터에서 값의 유효성을 검사하는 로직이 필요 없을 수도 있다. 이 경우 C#의 암묵적 속성 문법을 사용하면 필드 속성으로 감싸기 위한 코드를 크게 줄일 수 있다.
컴파일러는 데이터를 실제로 저장할 private 필드와 get/set 접근자를 자동으로 생성해준다.

인터페이스를 정의할 때에도 속성을 사용할 수 있다. 이 경우 암묵적 속성과 비슷한 문법을 사용한다.

```C#
    public interface INameValuePair<T>
    {
        string Name { get; }
        T value { get; set; }
    }
```

속성은 내부 데이터에 접근하거나 그 값을 수정하기 위해서 메서드의 기능을 확장한 C# 언어의 기본 요소이다.
따라서 멤버 함수로 할 수 있는 작업이라면 속성으로도 할 수 있다.게다가 ref나 out 키워드를 사용하는 메서드에 속성을 직접 전달할 수 없으므로 필드를 사용할 때 내부 데이터가 외부로 노출되는 중요한 결점 하나를 해결해주기도 한다.

속성의 접근자는 두 개의 독립된 메서드로 컴파일된다. C#에서는 속성의 get과 set 각각에 대해 서로 다른 접근 한정자를 지정할 수도 있다.

```C#
public class Cutomer
{
    public virtual string Name
    {
        get;
        protected set;
    }
}
```
