using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using MyQueue_Implementation.Modeling.MyEntities;
// System.Collections используется для IEnumerator

namespace MyQueue_Implementation.Core.MyCollections
{
    [DebuggerTypeProxy(typeof(MyQueue.MyQueueDebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [Serializable]
    public class MyQueue
    {
        private const int MinimumGrow = 4;

        private readonly int _growFactor; // 100 == 1.0, 130 == 1.3, 200 == 2.0

        private int _head;       // First valid element in the queue
        private int _tail;       // Last valid element in the queue
        private int _size;       // Number of elements.
        private int _version;
        private Person[] _array;

        
        public MyQueue() : this(32, (float)2.0) { }
        public MyQueue(int capacity) : this(capacity, (float)2.0) {}
        public MyQueue(int capacity, float growFactor)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Размерность массива не должна быть меньше нуля");
            if (!(growFactor >= 1.0 && growFactor <= 10.0))
                throw new ArgumentOutOfRangeException(nameof(growFactor), "Фактор роста не должен превышать 1 и быть меньше 10");

            _array = new Person[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
            _growFactor = (int)(growFactor * 100);
        }

        public int Count => _size;

        internal Person GetElement(int i)
        {
            return _array[(_head + i) % _array.Length];
        }

        private void SetCapacity(int capacity)
        {
            Person[] newArray = new Person[capacity];
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

        public bool IsEmpty()
        {
            return Count <= 0;
        }
        public bool IsFull()
        {
            return this._size >= Count;
        }
        public void SortById()
        {
            _array = _array.OrderBy(o => o.Id).ToArray();
        }
        public void SortByPhoneNumber()
        {
            _array = _array.OrderBy(o => o.PhoneNumber).ToArray();
        }
        public Person[] GetNElements(int n)
        {
            if (n > _size)
            {
                throw new ArgumentOutOfRangeException("n", "Количество показываемых не должно превышать длины массива");
            }

            return this._array.Take(n).ToArray();
        }
        public virtual Person[] ToArray()
        {
            Person[] arr = new Person[_size];
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
        public virtual void Enqueue(Person obj)
        {
            if (IsFull())
            {
                int newCapacity = (int)((long)_array.Length * (long)_growFactor / 100);
                if (newCapacity < _array.Length + MinimumGrow)
                {
                    newCapacity = _array.Length + MinimumGrow;
                }
                SetCapacity(newCapacity);
            }

            _array[_tail] = obj;
            _tail = (_tail + 1) % _array.Length;
            _size++;
            _version++;
        }
        public virtual Person Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("");
            Contract.EndContractBlock();

            Person removed = _array[_head];
            _array[_head] = null;
            _head = (_head + 1) % _array.Length;
            _size--;
            _version++;
            return removed;
        }

        [Serializable]
        private sealed class MyQueueEnumerator : IEnumerator, ICloneable
        {
            private readonly MyQueue _q;
            private int _index;
            private readonly int _version;
            private object _currentElement;

            internal MyQueueEnumerator(MyQueue q)
            {
                _q = q;
                _version = _q._version;
                _index = 0;
                _currentElement = _q._array;
                if (_q._size == 0)
                    _index = -1;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }

            public bool MoveNext()
            {
                if (_version != _q._version) throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");

                if (_index < 0)
                {
                    _currentElement = _q._array;
                    return false;
                }

                _currentElement = _q.GetElement(_index);
                _index++;

                if (_index == _q._size)
                    _index = -1;
                return true;
            }

            public object Current
            {
                get
                {
                    if (_currentElement != _q._array)
                        return _currentElement;

                    if (_index == 0)
                        throw new InvalidOperationException("Enum not started");
                    else
                        throw new InvalidOperationException("Enum Ended");
                }
            }

            public void Reset()
            {
                if (_version != _q._version) throw new InvalidOperationException("Enum failed version");
                if (_q._size == 0)
                    _index = -1;
                else
                    _index = 0;
                _currentElement = _q._array;
            }
        }

        internal class MyQueueDebugView
        {
            private readonly MyQueue _queue;

            public MyQueueDebugView(MyQueue queue)
            {
                this._queue = queue ?? throw new ArgumentNullException(nameof(queue));
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Person[] Items => _queue.ToArray();
        }

        public virtual IEnumerator GetEnumerator()
        {
            return new MyQueueEnumerator(this);
        }
    }
}
