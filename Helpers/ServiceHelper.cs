namespace Tokero.Helpers
{
    public static class ServiceHelper
    {
        private static IServiceProvider? services;

        public static void Initialize (IServiceProvider provider)
        {
            services = provider;
        }

        public static T? GetService<T> () where T : class
        {
            return services?.GetService(typeof(T)) as T;
        }
    }
}