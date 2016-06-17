namespace CQRSalad.Dispatching.NEW.Context
{
    public class DispatchingContext
    {
        public object MessageInstance { get; }
        public object HandlerInstance { get; }
        public object Result { get; internal set; }

        internal DispatchingContext(object handlerInstance, object messageInstance)
        {
            HandlerInstance = handlerInstance;
            MessageInstance = messageInstance;
        }
    }
}
