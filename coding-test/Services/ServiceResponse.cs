namespace coding_test
{
    public enum ResultState : byte
    {
        Faulted,
        Success
    }
    public readonly struct ServiceResponse<T>
    {
        internal readonly ResultState State;
        readonly Exception? Exception;
        readonly T? Response;

        /// <summary>
        /// Constructor of a concrete value
        /// </summary>
        /// <param name="value"></param>
        public ServiceResponse(T value)
        {
            State = ResultState.Success;
            Response = value;
            Exception = null;
        }

        /// <summary>
        /// Constructor of an error value
        /// </summary>
        /// <param name="e"></param>
        public ServiceResponse(Exception e)
        {
            State = ResultState.Faulted;
            Exception = e;
            Response = default;
        }

        /// <summary>
        /// True if the struct is in an invalid state
        /// </summary>
        public bool IsFaulted =>
          State == ResultState.Faulted;

        /// <summary>
        /// True if the struct is in an success
        /// </summary>
        public bool IsSuccess =>
           State == ResultState.Success;

        /// <summary>
        /// Return the actual exception message generated.
        /// </summary>
        public Exception ExceptionMessage =>
          Exception;

        /// <summary>
        /// Resturn the actual response recorded.
        /// </summary>
        public T ResponseObject =>
         Response;


        /// <summary>
        /// Implicit Value returned is boolean
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator bool(ServiceResponse<T> d) => d.IsSuccess;
    }
}
