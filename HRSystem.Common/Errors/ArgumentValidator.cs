using System;

namespace HRSystem.Global.Errors
{
    public static class ArgumentValidator
    {
        public static void EnsureNotNull<T>(string paramName, T argument) 
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
        
        public static void EnsureNotNullOrEmpty(string paramName, string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException($"{paramName} can't be null or empty.");
            }
        }
    }
}