namespace Alley.Utils.Models
{
    public abstract class ResultBase : IResult
    {
        public bool IsSuccess { get; protected set; }

        public bool IsFailure => !IsSuccess;
        public bool IsHandled { get; set; }

        public bool IsNotHandled => !IsHandled;
        public string Message { get; protected set; }
    }

    public abstract class ResultBase<T> : ResultBase, IResult<T>
    {
        public T Value { get; protected set; }
    }
}