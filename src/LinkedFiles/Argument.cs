using System.Collections.Generic;
using System.Linq;

namespace System
{
    internal static class Argument
    {
        private const string NullMessage = "Argument is null.";
        private const string EmptyMessage = "Argument is empty or consists only of white-space characters.";
        private const string EmptyList = "Specified list contains no elements.";
        private const string ListContainsNull = "Specified list contains null element.";
        private const string ListContainsEmptyString = "Specified list of strings contains empty element.";
        private const string ValueIsNegative = "Argument is negative.";

        public static void IsNotNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName, NullMessage);
            }
        }

        public static void StringNotEmpty(string argument, string argumentName)
        {
            if (String.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException(EmptyMessage, argumentName);
            }
        }

        public static void ListIsNotEmpty<T>(IEnumerable<T> list)
        {
            if (list == null || !list.Any())
            {
                throw new ArgumentException(EmptyList);
            }
        }

        public static void ElementsNotNull<T>(IEnumerable<T> list) where T : class
        {
            if (list == null)
            {
                throw new ArgumentException(NullMessage);
            }

            if (list.Any(item => item == null))
            {
                throw new ArgumentException(ListContainsNull);
            }
        }

        public static void ElementsNotEmpty(IEnumerable<string> list)
        {
            if (list == null)
            {
                throw new ArgumentException(NullMessage);
            }

            if (list.Any(String.IsNullOrWhiteSpace))
            {
                throw new ArgumentException(ListContainsEmptyString);
            }
        }

        public static void NotNegative(Int32 argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentException(ValueIsNegative);
            }
        }
    }
}