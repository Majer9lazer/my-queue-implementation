using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace MyQueue_Implementation.Core.MyGenericCollections
{
    [DebuggerTypeProxy(typeof(MyGenericQueueDebugView<>))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class MyGenericQueue<T> : IEnumerable<T>
    {
        private T[] _array;
        private int _head;       // начало очереди
        private int _tail;       // конец очереди
        private int _size;       // количество элементов в массиве
        private int _version;

        private const int MinimumGrow = 1;   // коэфициент возрастания
        private const int GrowFactor = 100;  // double each time
        private const int DefaultCapacity = 4;
        static readonly T[] EmptyArray = new T[0];
        public MyGenericQueue()
        {
            _array = EmptyArray;
        }

        public MyGenericQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "размерность не должна быть меньше нуля");


            _array = new T[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        public MyGenericQueue(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "коллекция не должна быть пустой");

            _array = new T[DefaultCapacity];
            _size = 0;
            _version = 0;

            using (IEnumerator<T> en = collection.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    Enqueue(en.Current);
                }
            }
        }

        public int Count => _size;

        public void Enqueue(T item)
        {
            if (_size == _array.Length)
            {
                int newCapacity = (int)(_array.Length * (long)GrowFactor / 100);

                if (newCapacity < _array.Length + MinimumGrow)
                {
                    newCapacity = _array.Length + MinimumGrow;
                }

                SetCapacity(newCapacity);
            }

            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            _size++;
            _version++;
        }

        public T[] GetNElements(int n)
        {
            if (n > _size)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Количество показываемых не должно превышать длины массива");
            }

            return this._array.Take(n).ToArray();
        }

        public void SortBy<TSource>(Func<T, TSource> func)
        {
            _array = _array.Where(w => w != null).OrderBy(func).ToArray();
        }
        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue is empty");

            T removed = _array[_head];
            _array[_head] = default;
            _head = (_head + 1) % _array.Length;
            _size--;
            _version++;
            return removed;
        }

        public bool IsEmpty() => Count <= 0;

        public bool IsFull() => this._size >= Count;

        internal T GetElement(int i)
        {
            return _array[(_head + i) % _array.Length];
        }

        public T[] ToArray()
        {
            T[] arr = new T[_size];
            if (_size == 0)
                return arr;

            if (_head < _tail)
            {
                Array.Copy(_array, _head, arr, 0, _size);
            }
            else
            {
                Array.Copy(_array, _head, arr, 0, _array.Length - _head);
                Array.Copy(_array, 0, arr, _array.Length - _head, _tail);
            }

            return arr;
        }

        public T this[int index] => _array[index];

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new MyQueueEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MyQueueEnumerator(this);
        }

        private void SetCapacity(int capacity)
        {
            T[] newArray = new T[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newArray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newArray, _array.Length - _head, _tail);
                }
            }

            _array = newArray;
            _head = 0;
            _tail = (_size == capacity) ? 0 : _size;
            _version++;
        }

        [Serializable]
        public struct MyQueueEnumerator : IEnumerator<T>, IEnumerator
        {
            private readonly MyGenericQueue<T> _q;
            private int _index;   // -1 = not started, -2 = ended/disposed
            private readonly int _version;
            private T _currentElement;

            internal MyQueueEnumerator(MyGenericQueue<T> q)
            {
                _q = q;
                _version = _q._version;
                _index = -1;
                _currentElement = default;
            }

            public void Dispose()
            {
                _index = -2;
                _currentElement = default;
            }

            public bool MoveNext()
            {
                if (_version != _q._version)
                    throw new InvalidOperationException("EnumFailedVersion");

                if (_index == -2)
                    return false;

                _index++;

                if (_index == _q._size)
                {
                    _index = -2;
                    _currentElement = default;
                    return false;
                }

                _currentElement = _q.GetElement(_index);
                return true;
            }

            public T Current
            {
                get
                {
                    if (_index >= 0)
                        return _currentElement;

                    if (_index == -1)
                        throw new InvalidOperationException("InvalidOperation_EnumNotStarted");

                    throw new InvalidOperationException("InvalidOperation_EnumEnded");
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_index >= 0)
                        return _currentElement;

                    if (_index == -1)
                        throw new InvalidOperationException("InvalidOperation_EnumNotStarted");

                    throw new InvalidOperationException("InvalidOperation_EnumEnded");
                }
            }

            void IEnumerator.Reset()
            {
                if (_version != _q._version)
                    throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");

                _index = -1;
                _currentElement = default(T);
            }
        }

    }

    internal sealed class MyGenericQueueDebugView<T>
    {
        private readonly MyGenericQueue<T> _queue;

        public MyGenericQueueDebugView(MyGenericQueue<T> queue)
        {
            this._queue = queue ?? throw new ArgumentNullException(nameof(queue), "Очередь не должна быть пустой");
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => _queue.ToArray();
    }
}
