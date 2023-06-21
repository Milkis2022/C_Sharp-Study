### Property
C#에서는 프로퍼티라는 문법을 지원합니다. 기본적으로 객체지향 프로그래밍에서 캡슐화에 대한 편리한 문법을 제공합니다.
다음과 같은 클래스가 있습니다.

```C#
    class Person
    {
        public int age;
        public string name;

        public void Introduce()
        {
            Console.WriteLine($"My name is {name}. I'm {age} years old.");
        }
    }
```

위 코드에서 코드상에서 문제점은 없다. 하지만 객체지향 프로그래밍의 패러다임으로 볼 떄 문제가 있는 코드입니다.
일단 기본적으로 생성자를 정의하지 않으면 디폴트 생성자가 정의가 됩니다. 위 코드에서 생성자가 없어도 멤버 필드에 접근할 수 가 있습니다. 하지만 이렇게 접근하는 행위가 객체를 생성할떄만 일어나는게 아닌 어느때나 일어날 수 있고 위 멤버필드는 public으로 선언되어 있기 떄문에 어느때나 접근해서 수정할 수 있습니다. 이는 정보은닉 원칙에 위배가 됩니다.

다음과 같이 private으로 멤버필드의 접근 제한자를 수정하고 게터와 세터를 정의합니다.
```C#
        private int age;
        private string name;

        public int GetAge()
        {
            return age;
        }

        public void SetAge(int age)
        {
            this.age = age;
        }
        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
```

한눈에 봐도 코드가 굉장히 길다는것을 알 수 있습니다. 필드가 2개여서 그렇지 더 많다면 엄청나게 긴 **boilerplate code**를 생성해야합니다. 이 때 지원하는것이 C#의 프로퍼티입니다. 다음과 같이 사용합니다.

```C#
        private int age;
        private string name;

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
```

어느정도 긴 코드가 타이핑이 많이 줄었습니다.

```C#
접근제한자 반환값 프로퍼티명
public string Name
{
    get
    {
        return name;
    }
    set
    {
        name = value;
    }
}
```
와 같이 사용합니다. 여기서 set에 사용되는 value 는 암묵적으로 들어온 매개변수에 매칭됩니다.

```C#
    Person p1 = new Person();
    p1.Name = "Jerry";      // set { name = value; } "Jerry"가 value에 매칭된다.
```

하지만 위에 코드도 상당히 깁니다. 이를 위해 **자동구현 프로퍼티**라는것도 지원합니다.
```C#
    public int Age { get; set;}
    public string Name { get; set; }
```

위에 상당히 줄였던 프로퍼티 문법을 더욱 짧게 구현할 수 있게 되었습니다.
또한 위 프로퍼티는 get및 set 접근자를 통해서만 액세스할 수 있는 전용 익명 지원 필드를 만듭니다.
아래에 있는 int age; 와 프로퍼티 Age는 다른 녀석입니다.
```C#
    class Person
    {
        private int age;

        public int Age { get; set;}
        public string Name { get; set; }
    }
```

위 코드에서 멤버 필드 Name이 없음에도 아래 자동구현 프로퍼티가 get과 set로 접근 가능한 필드를 만들고 
p1.Name = name; 으로 접근해도 메서드로 접근하는 동작을 합니다. 또한 다음과 같이 자동 구현 프로퍼티를 초기화할 수도 있습니다.

```C#
    public string Name { get; set; } = "Jerry";
```

자동구현 프로퍼티로 생성을 해도 외부에서 접근이 불가능한 private 한 필드라던지 get만 구현을 한다던지 하는 작업이 가능합니다.


### 프로퍼티 접근제어자
다음과 같은 코드를 살펴봅시다.
```C#
public int Age { get; set;}
```
위 프로퍼티는 public으로 선언되어 있는 프로퍼티입니다. 내부에서는 컴파일러가 필드에 대한 세부 구현을 숨기고 자동으로 필드를 생성하고 값을 관리합니다.

하지만 값을 읽기만 가능하고 수정은 불가능한 read-only같은 멤버를 만들고 싶을 수 있습니다.

```C#
    public int Age { get; set;}
    public string Name { get; private set; } = "Merry";
```

위 프로퍼티에서 Name 프로퍼티의 set은 private 이기 때문에 외부에서 Name = "string"; 이런식으로 수정이 불가능합니다.
```C#
    p1.Age = 10;
    // p1.Name = "Merry"; private set; 이기 떄문에 오류 발생
```

또한 다음과 같이 전체 접근제어자를 변경할 수 있습니다.
```C#
    public int Age { get; set;}
    public string Name { get; private set; } = "Jerry";
    private string Gender { get; set; } = "Male";

    public void Introduce()
    {
        Console.WriteLine($"My name is {Name}. I'm {Age} years old.");
        Console.WriteLine($"Gender: {Gender}");
    }
```

Gender 프로퍼티는 private으로 선언되어 있기 때문에 
다음과 같이 외부에서 접근할 수 없습니다.
```C#
    p1.Gender = "Female";   // 오류
    Console.WriteLine(p1.Gender);       // 오류
```
