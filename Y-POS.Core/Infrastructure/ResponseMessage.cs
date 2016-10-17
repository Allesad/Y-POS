namespace Y_POS.Core.Infrastructure
{
    public class ResponseMessage
    {
        #region Constants

        public const int EMPTY_ERROR_CODE = -1;

        #endregion

        #region Properties

        public bool IsSuccess { get; }
        public int ErrorCode { get; }
        public string ErrorMessage { get; }

        #endregion
        
        public ResponseMessage(bool isSuccess = true, int errorCode = EMPTY_ERROR_CODE, string errorMessage = default(string))
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        #region Static methods

        public static ResponseMessage Success()
        {
            return new ResponseMessage();
        }

        public static ResponseMessage<TData> Success<TData>(TData data)
        {
            return new ResponseMessage<TData>(data);
        }

        public static ResponseMessage Fail(int errorCode)
        {
            return new ResponseMessage(false, errorCode: errorCode);
        }

        public static ResponseMessage Fail(string errorMessage)
        {
            return new ResponseMessage(false, errorMessage: errorMessage);
        }

        public static ResponseMessage Fail(int errorCode, string errorMessage)
        {
            return new ResponseMessage(false, errorCode: errorCode, errorMessage: errorMessage);
        }

        public static ResponseMessage<T> Fail<T>(int errorCode)
        {
            return new ResponseMessage<T>(errorCode: errorCode);
        }

        public static ResponseMessage<T> Fail<T>(string errorMessage)
        {
            return new ResponseMessage<T>(errorMessage: errorMessage);
        }

        public static ResponseMessage<T> Fail<T>(int errorCode, string errorMessage)
        {
            return new ResponseMessage<T>(errorCode: errorCode, errorMessage: errorMessage);
        } 

        #endregion
    }

    public class ResponseMessage<T> : ResponseMessage
    {
        #region Properties

        public T Data { get; }

        #endregion

        #region Constructors

        public ResponseMessage(T data)
        {
            Data = data;
        }

        public ResponseMessage(int errorCode = EMPTY_ERROR_CODE, string errorMessage = null) 
            : base(false, errorCode, errorMessage)
        {
        }

        #endregion
    }
}
