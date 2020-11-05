namespace Alley.Utils.Models
{
    public class Result<T> : ResultBase<T>
    {
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

    public class Result : ResultBase
    {
        private Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static IResult Success(string message = null, bool handled = false)
        {
            return new Result(true, message) {IsHandled = handled};
        }

        public static IResult Failure(string errorMessage, bool handled = false)
        {
            return new Result(false, errorMessage) {IsHandled = handled};
        }
    }
}