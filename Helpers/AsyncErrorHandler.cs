using System.Diagnostics;

namespace Tokero.Helpers
{
    /// <summary>
    /// Static class required by AsyncErrorHandler.Fody to handle exceptions in async methods
    /// </summary>
    public static class AsyncErrorHandler
    {
        public static void HandleException (Exception exception)
        {
            Debug.WriteLine("Exception occurred: " + exception.Message);
        }
    }
}