namespace MyQueue_Implementation.Modeling.Interfaces
{
    public interface IStrNumberGenerator : IGenerator<string>
    {
        string GenerateByCustomFormat(string format, char separator);
    }
}