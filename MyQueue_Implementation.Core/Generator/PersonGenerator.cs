using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyQueue_Implementation.Modeling.Interfaces;
using MyQueue_Implementation.Modeling.MyEntities;

namespace MyQueue_Implementation.Core.Generator
{
    public class PersonGenerator : IGenerator<Person>
    {
        private readonly Random _rnd;
        private const int MaxSeed = 5;
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

        private int RandomIterations()
        {
            return _rnd.Next(1, MaxSeed + 1);
        }

        public Person GenerateSingle(int id)
        {
            return new Person(id, RandomNumber());
        }
        /// <summary>
        /// Возвращает рандомный номер телефона по заданию
        /// использую string.Format; чтобы не конфликтовать с legacy проектами)
        /// </summary>
        /// <returns></returns>
        private string RandomNumber()
        {
            //77 777 777
            return string.Format("{0}{1}{2}", _rnd.Next(10, 100), _rnd.Next(100, 1000), _rnd.Next(100, 1000));
        }
    }
}
