namespace EducationSystem.Application.Common.Models
{
    public class Result
    {
        internal Result(bool isSucceeded, IEnumerable<string> errors)
        {
            IsSucceeded = isSucceeded;
            Errors = errors?.ToArray() ?? Array.Empty<string>();
        }

        public string[] Errors { get; set; }
        public bool IsSucceeded { get; set; }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(params string[] errors)
        {
            return new Result(false, errors);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }

    public class Result<T> : Result
    {
        internal Result(bool isSucceeded, T data) : base(isSucceeded, null)
        {
            Data = data;
        }

        internal Result(bool isSucceeded, IEnumerable<string> errors) : base(isSucceeded, errors)
        {
        }

        public T Data { get; set; }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data);
        }

        public static Result<T> Failure(params string[] errors)
        {
            return new Result<T>(false, errors);
        }

        public static new Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, errors);
        }
    }
}
