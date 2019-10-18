using System;
using System.Collections.Generic;
using System.Linq;
using MyQueue_Implementation.Modeling.Interfaces;
using MyQueue_Implementation.Modeling.MyEntities;

namespace MyQueue_Implementation.Core.Generator
{
    public class PersonGenerator : IPersonGenerator
    {
        private readonly Random _rnd;
        public int PersonCount { get; set; }
        private Person[] _array;

        public PersonGenerator()
        {
            _rnd = new Random();
            PersonCount = _rnd.Next(1, 5 + 1);
        }

        public PersonGenerator(int personCount)
        {
            PersonCount = personCount;
        }

        public Person[] GenerateArray()
        {
            _array = new Person[PersonCount];
            for (int i = 0; i < PersonCount; i++)
            {
                _array[i] = GenerateSingle(i);
            }
            return _array;
        }

        public Person GenerateSingle()
        {
            return new Person(_rnd.Next(1000), RandomNumber());
        }

        public Person GenerateSingle(int id)
        {
            return new Person(id, RandomNumber());
        }

        public IEnumerable<Person> GenerateEnumerablePeople(int count)
        {
            return Enumerable.Range(1, count).Select(s => new Person(s, RandomNumber()));
        }

        /// <summary>
        /// Возвращает случайный номер телефона.
        /// </summary>
        /// <returns></returns>
        private string RandomNumber()
        {
            //77 777 777
            return $"{_rnd.Next(10, 100)}{_rnd.Next(100, 1000)}{_rnd.Next(100, 1000)}";
        }
    }
}
