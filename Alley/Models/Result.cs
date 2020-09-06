namespace Alley.Models
{
    internal class Result<T> : ResultBase
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
            ErrorMessage = errorMessage;
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

    internal abstract class ResultBase
    {
        public bool IsSuccess { get; protected set; }
        
        public bool IsFailure => !IsSuccess;
        
        public string ErrorMessage { get; protected set; }
    }

    internal class Result : ResultBase
    {

        private Result()
        {
            IsSuccess = true;
        }
        
        private Result(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }
        
        public static Result Success()
        {
            return new Result();
        }

        public static Result Failure(string message)
        {
            return new Result(message);
        }
    }
    
}