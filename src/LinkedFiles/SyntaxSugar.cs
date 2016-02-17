namespace System
{
    internal static class CastingSyntaxSugar
    {
        public static T Of<T>(this object o)
        {
            return (T)o;
        }

        public static bool Is<T>(this object o)
        {
            return o is T;
        }

        public static T As<T>(this object o) where T : class
        {
            return o as T;
        }
    }

    internal static class ArraySugarSyntax 
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            for (int index = 0; index < array.Length; ++index)
            {
                action(array[index]);
            }
        }
    }
}