using Domain.Enums;

namespace Applications.Shared
{
    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, T value, string error, ErrorType errorType = ErrorType.NotFound) 
            : base(isSuccess, error, errorType)
        {
            Value = value;
        }

        public static new Result<T> Success(T value) => new Result<T>(true, value, null!);
        public static new Result<T> Failure(string error, ErrorType errorType) 
            => new Result<T>(false, default!, error, errorType);
    }
}
