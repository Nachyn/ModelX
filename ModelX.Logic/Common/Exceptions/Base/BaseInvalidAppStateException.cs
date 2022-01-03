namespace ModelX.Logic.Common.Exceptions.Base;

public class BaseInvalidAppStateException : BaseException
{
    public BaseInvalidAppStateException(string message)
        : base(message)
    {
    }

    public BaseInvalidAppStateException()
    {
    }
}