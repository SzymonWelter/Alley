namespace Alley.Utils
{
    public class Result<T> : ResultBase
    {
        public T Value { get; }
        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(string errorMessage)
        {
            IsSuccess = false;
            Message = errorMessage;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>(message);
        }
    }

    public abstract class ResultBase : IResult
    {
        public bool IsSuccess { get; protected set; }
        
        public bool IsFailure => !IsSuccess;
        public bool IsHandled { get; set; }
        
        public bool IsNotHandled { get; }
        public string Message { get; protected set; }
    }

    public interface IResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
        bool IsHandled { get; }
        bool IsNotHandled { get; }
        string Message { get; }
    }

    public class Result : ResultBase
    {
        private Result()
        {
            IsSuccess = true;
        }
        
        private Result(string errorMessage)
        {
            IsSuccess = false;
            Message = errorMessage;
        }
        
        public static IResult Success()
        {
            return new Result();
        }

        public static IResult Failure(string message, bool handled = false)
        {
            return new Result(message){IsHandled = handled};
        }

        public static IResult Determine(bool isSuccess, string potentialErrorMessage, bool handled = false)
        {
            return isSuccess ? Success() : Failure(potentialErrorMessage, handled);
        }
    }
    
}