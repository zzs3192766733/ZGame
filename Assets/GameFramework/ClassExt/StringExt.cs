namespace GameFramework.ClassExt
{
    public static class StringExt
    {
        public static bool ToBool(this string str)
        {
            return str == "True" || str == "true" || str == "1";
        }

        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out var val))
                return val;
            return -1;
        }

        public static float ToFloat(this string str)
        {
            if (float.TryParse(str, out var val))
                return val;
            return -1f;
        }

        public static long ToLong(this string str)
        {
            if (long.TryParse(str, out var val))
                return val;
            return -1;
        }

        public static short ToShort(this string str)
        {
            if (short.TryParse(str, out var val))
                return val;
            return -1;
        }

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static T ToEnum<T>(this string str) => (T) System.Enum.Parse(typeof(T), str);
    }
}