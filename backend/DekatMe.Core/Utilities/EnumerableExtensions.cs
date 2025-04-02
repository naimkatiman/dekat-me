using System.Collections.ObjectModel;

namespace DekatMe.Core.Utilities
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyCollection<T>(source.ToList());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.OrderBy(_ => Guid.NewGuid());
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            
            return source.GroupBy(keySelector).Select(g => g.First());
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Select((item, index) => (item, index));
        }

        public static IEnumerable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
            if (leftKeySelector == null) throw new ArgumentNullException(nameof(leftKeySelector));
            if (rightKeySelector == null) throw new ArgumentNullException(nameof(rightKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            
            var leftLookup = left.ToLookup(leftKeySelector);
            var rightLookup = right.ToLookup(rightKeySelector);
            
            var leftKeys = leftLookup.Select(l => l.Key);
            var rightKeys = rightLookup.Select(r => r.Key);
            var allKeys = leftKeys.Union(rightKeys);
            
            return allKeys.SelectMany(key => 
                leftLookup[key].DefaultIfEmpty().SelectMany(leftItem => 
                    rightLookup[key].DefaultIfEmpty().Select(rightItem => 
                        resultSelector(leftItem, rightItem))));
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (batchSize <= 0) throw new ArgumentException("Batch size must be greater than 0.", nameof(batchSize));
            
            var batch = new List<T>(batchSize);
            
            foreach (var item in source)
            {
                batch.Add(item);
                
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }
            
            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        public static bool None<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) return Enumerable.Empty<T>();
            
            var queue = new Queue<T>(count);
            
            foreach (var item in source)
            {
                if (queue.Count == count)
                    queue.Dequeue();
                    
                queue.Enqueue(item);
            }
            
            return queue;
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) return source;
            
            var buffer = new Queue<T>(count + 1);
            
            foreach (var item in source)
            {
                buffer.Enqueue(item);
                
                if (buffer.Count > count)
                    yield return buffer.Dequeue();
            }
        }

        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            
            using var enumerator1 = first.GetEnumerator();
            using var enumerator2 = second.GetEnumerator();
            
            bool hasMore1, hasMore2;
            
            while ((hasMore1 = enumerator1.MoveNext()) | (hasMore2 = enumerator2.MoveNext()))
            {
                if (hasMore1)
                    yield return enumerator1.Current;
                    
                if (hasMore2)
                    yield return enumerator2.Current;
            }
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            
            var seenKeys = new HashSet<TKey>(comparer);
            
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            
            var keys = new HashSet<TKey>(second.Select(keySelector));
            return first.Where(item => !keys.Contains(keySelector(item)));
        }

        public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, IEnumerable<T3> third, Func<T1, T2, T3, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            
            using var e1 = source.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            
            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
            {
                yield return resultSelector(e1.Current, e2.Current, e3.Current);
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.SelectMany(x => x);
        }

        public static bool IsSubsetOf<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            
            var secondSet = new HashSet<T>(second);
            return first.All(item => secondSet.Contains(item));
        }

        public static bool IsSupersetOf<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            
            var firstSet = new HashSet<T>(first);
            return second.All(item => firstSet.Contains(item));
        }
    }
}
