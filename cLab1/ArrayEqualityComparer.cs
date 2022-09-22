namespace cLab1;
    public class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        private static readonly IEqualityComparer<T[]> defaultInstance = new ArrayEqualityComparer<T>();
        private readonly IEqualityComparer<T> elementComparer;

        /// <summary>
        /// Gets the default comparer.
        /// </summary>
        public static IEqualityComparer<T[]> Default => defaultInstance;

        /// <summary>
        /// Instantiates with default comparer for items in the array.
        /// </summary>
        public ArrayEqualityComparer()
            : this(EqualityComparer<T>.Default)
        { }

        /// <summary>
        /// Instantiates.
        /// </summary>
        /// <param name="elementComparer">comparer for items in the array</param>
        public ArrayEqualityComparer(IEqualityComparer<T> elementComparer)
        {
            this.elementComparer = elementComparer;
        }

        /// <inheritdoc/>
        public Boolean Equals(T[] x, T[] y)
        {
            if (Object.ReferenceEquals(x, y))
                return true;

            if (x == null! && y == null!)
                return true;

            if (x == null || y == null!)
                return false;

            if (x.Length != y.Length)
                return false;

            for (var i = 0; i < x.Length; i++)
            {
                if (!elementComparer.Equals(x[i], y[i]))
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public Int32 GetHashCode(T[] array)
        {
            if (array == null!)
                return 0;
            
            Int32 hash = 17;
            foreach (T item in array)
            {
                hash = hash * 23 + elementComparer.GetHashCode(item!);
            }
            return hash;
        }
    }