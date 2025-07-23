using Domain.Enums;

namespace Applications.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public ErrorType ErrorType { get; }
     
        protected Result(bool isSuccess, string error, ErrorType errorType = ErrorType.NotFound)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result Success => new Result(true, null!);
        public static Result Failure(string error, ErrorType errorType) => new Result(false, error, errorType);
    }
 }
