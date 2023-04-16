using System;
using System.Collections;

namespace IEnumerate
{
    public class Person
    {
        public string firstName { get; }
        public string lastName { get; }

        public Person(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }
        
    }

    public class People : IEnumerable
    {
        private Person[] people;
        public People(Person[] pArray)
        {
            people = new Person[pArray.Length];

            for(int i = 0; i < pArray.Length; i++)
            {
                people[i] = pArray[i];
            }

        }

        public IEnumerator GetEnumerator()
        {
            return new PeopleEnum(people);
        }

    }

    public class PeopleEnum : IEnumerator
    {
        public Person[] people;
        int position = -1;

        public PeopleEnum(Person[] list)
        {
            people = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < people.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Person Current
        {
            get
            {
                try
                {
                    return people[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
    class App
    {
        static void Main()
        {
            Person[] peopleArray = new Person[3]
            {
            new Person("Tom", "and Jerry"),
            new Person("Jim", "Carry"),
            new Person("Chat", "Gpt"),
            };

            People peopleList = new People(peopleArray);
            foreach (Person p in peopleList)
            {
                Console.WriteLine(p.firstName + " " + p.lastName);
            }
            
        }
    }
}

