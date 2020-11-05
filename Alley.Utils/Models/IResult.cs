namespace Alley.Utils.Models
{
    public interface IResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
        bool IsHandled { get; set; }
        bool IsNotHandled { get; }
        string Message { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}