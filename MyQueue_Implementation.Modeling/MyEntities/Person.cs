using System;

namespace MyQueue_Implementation.Modeling.MyEntities
{
    /// <summary>
    /// Класс описывающий объект человека
    /// сделал sealed чтобы никто не наследовался на всякий случай
    /// </summary>
    public sealed class Person : IComparable
    {

        public Person(int id, string phoneNumber)
        {
            Id = id;
            PhoneNumber = phoneNumber;
        }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public int CompareTo(object obj)
        {
            Person person = (Person)obj;

            return this.Id == person.Id ? 0 : Id > person.Id ? 1 : Id < person.Id ? -1 : 0;
        }
    }
}
