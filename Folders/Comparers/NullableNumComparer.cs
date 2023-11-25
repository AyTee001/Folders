namespace Folders.Comparers
{
    public class NullableNumComparer
    {
        public static int CompareNullable<T>(T? x, T? y) where T : struct, IComparable<T>
        {
            if (x.HasValue && y.HasValue)
            {
                return x.Value.CompareTo(y.Value);
            }
            else if (x.HasValue)
            {
                return 1;
            }
            else if (y.HasValue)
            {
                return -1;
            }
            else
            {
                return 0; 
            }
        }
    }
}
