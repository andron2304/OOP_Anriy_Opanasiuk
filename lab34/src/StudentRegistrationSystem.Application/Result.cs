namespace StudentRegistrationSystem.Application
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public object Data { get; }

        private Result(bool isSuccess, string error, object data)
        {
            IsSuccess = isSuccess;
            Error = error;
            Data = data;
        }

        public static Result Success(object data = null) => new Result(true, null, data);
        public static Result Failure(string error) => new Result(false, error, null);
    }
}