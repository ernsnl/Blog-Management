using System.Reflection;

namespace BlogApplication.Framework.Extensions.ListExtensions
{
    public static class ReflectionExtension
    {
        public static object GetPropertyValue<TResult>(this TResult source, string property) where TResult : class
        {
            return source.GetType().GetProperty(property, (BindingFlags.Public | BindingFlags.Instance)).GetValue(source);
        }

        public static string GetPropertyStringValue<TResult>(this TResult source, string property) where TResult : class
        {
            return source.GetType().GetProperty(property, (BindingFlags.Public | BindingFlags.Instance)).GetValue(source).ToString();
        }
    }
}