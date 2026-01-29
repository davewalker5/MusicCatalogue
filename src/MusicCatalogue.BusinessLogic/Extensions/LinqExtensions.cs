namespace MusicCatalogue.BusinessLogic.Extensions
{
    public static class LinqExtensions
    {
        public static TResult Let<T, TResult>(this T value, Func<T, TResult> func)
            => func(value);
    }
}