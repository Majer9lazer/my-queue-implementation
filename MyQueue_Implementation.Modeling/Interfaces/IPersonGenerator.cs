using System.Collections.Generic;
using MyQueue_Implementation.Modeling.MyEntities;

namespace MyQueue_Implementation.Modeling.Interfaces
{
    public interface IPersonGenerator : IGenerator<Person>
    {
        IEnumerable<Person> GenerateEnumerablePeople(int count);

        Person GenerateSingle(int id);
    }
}