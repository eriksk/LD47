namespace LD47.Collections
{
    public class Pool<T> where T : class, new()
    {
        private readonly int _capacity;
        private int _count;
        private T[] _items;

        public int Count => _count;

        public Pool(int capacity)
        {
            _capacity = capacity;
            _items = new T[capacity];
            for (var i = 0; i < capacity; i++)
            {
                _items[i] = new T();
            }
        }

        public void Clear()
        {
            _count = 0;
        }

        public T Pop()
        {
            if (_count >= _items.Length - 1)
            {
                return null;
            }
            return _items[_count++];
        }

        public void Push(int index)
        {
            var temp = _items[index];
            _items[index] = _items[_count - 1];
            _items[_count - 1] = temp;
            _count--;
        }

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
        }

    }
}