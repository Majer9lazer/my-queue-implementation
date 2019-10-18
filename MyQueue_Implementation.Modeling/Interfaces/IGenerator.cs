using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQueue_Implementation.Modeling.Interfaces
{
    public interface IGenerator<out T>
    { 
        T[] GenerateArray(); 
        
        T GenerateSingle();
    }
}
