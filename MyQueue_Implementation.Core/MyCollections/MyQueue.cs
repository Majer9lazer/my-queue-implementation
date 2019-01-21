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
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class MyQueue
    {
        private int _head;       // First valid element in the queue
        private int _tail;       // Last valid element in the queue
        private int _size;       // Number of elements.
        private readonly int _growFactor; // 100 == 1.0, 130 == 1.3, 200 == 2.0
        private int _version;

        private const int MinimumGrow = 4;
        private Person[] _array;
        public MyQueue()
            : this(32, (float)2.0)
        {
        }


        public MyQueue(int capacity)
            : this(capacity, (float)2.0)
        {
        }


        public MyQueue(int capacity, float growFactor)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "Размерность массива не должна быть меньше нуля");
            if (!(growFactor >= 1.0 && growFactor <= 10.0))
                throw new ArgumentOutOfRangeException("growFactor", "Фактор роста не должен превышать 1 и быть меньше 10");


            _array = new Person[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
            _growFactor = (int)(growFactor * 100);
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

        private void SetCapacity(int capacity)
        {
            Person[] newarray = new Person[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newarray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
                }
            }
            _array = newarray;
            _head = 0;
            _tail = (_size == capacity) ? 0 : _size;
            _version++;
        }


        public int Count
        {
            get { return _size; }
        }
        internal Person GetElement(int i)
        {
            return _array[(_head + i) % _array.Length];
        }
        [Serializable]
        private class MyQueueEnumerator : IEnumerator, ICloneable
        {
            private MyQueue _q;
            private int _index;
            private int _version;
            private Object currentElement;

            internal MyQueueEnumerator(MyQueue q)
            {
                _q = q;
                _version = _q._version;
                _index = 0;
                currentElement = _q._array;
                if (_q._size == 0)
                    _index = -1;
            }

            public Object Clone()
            {
                return MemberwiseClone();
            }

            public virtual bool MoveNext()
            {
                if (_version != _q._version) throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");

                if (_index < 0)
                {
                    currentElement = _q._array;
                    return false;
                }

                currentElement = _q.GetElement(_index);
                _index++;

                if (_index == _q._size)
                    _index = -1;
                return true;
            }

            public virtual Object Current
            {
                get
                {
                    if (currentElement == _q._array)
                    {
                        if (_index == 0)
                            throw new InvalidOperationException("Enum not started");
                        else
                            throw new InvalidOperationException("Enum Ended");
                    }
                    return currentElement;
                }
            }

            public virtual void Reset()
            {
                if (_version != _q._version) throw new InvalidOperationException("Enum failed version");
                if (_q._size == 0)
                    _index = -1;
                else
                    _index = 0;
                currentElement = _q._array;
            }
        }
        internal class MyQueueDebugView
        {
            private MyQueue queue;

            public MyQueueDebugView(MyQueue queue)
            {
                // ReSharper disable once InvocationIsSkipped
                Contract.EndContractBlock();

                this.queue = queue ?? throw new ArgumentNullException("queue");
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Person[] Items
            {
                get
                {
                    return queue.ToArray();
                }
            }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return new MyQueueEnumerator(this);
        }
    }
}
