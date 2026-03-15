using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace kf
{
    /// <summary>
    /// The circular buffer class from week 5
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularBuffer<T> : IEnumerable
    {
        long m_capacity = 0; // Maximum storage
        long m_size = 0;     // Currently used storage
        long m_start = 0;    // Start index
        T[] m_data;          // Internal storage of elements

        /// <summary>
        /// A circular / ring buffer class. It provides a List style interface but overwrites
        /// older entries when it runs out of space.
        /// </summary>
        public CircularBuffer()
        {
        }

        /// <summary>
        /// A circular / ring buffer class. It provides a List style interface but overwrites
        /// older entries when it runs out of space.
        /// </summary>
        /// <param name="capacity">The maximum number of elements that the buffer can hold. The capacity is preallocated on construction.</param>
        public CircularBuffer(long capacity)
        {
            m_capacity = capacity;
            m_data = new T[m_capacity];
        }

        /// <summary>
        /// Modify the capacity of the buffer.
        /// The buffer will be realllocated and existing content is deleted.
        /// </summary>
        /// <param name="reserveSize">The maximum number of elements that the buffer can hold. The capacity is preallocated.</param>
        /// <returns>Reference to the CircularBuffer for fluent coding.</returns>
        public CircularBuffer<T> Reserve(long reserveSize)
        {
            m_capacity = reserveSize;
            m_data = new T[reserveSize];
            m_start = 0;
            m_size = 0;
            return this;
        }

        /// <summary>
        /// Get or set the number of active elements in the buffer.
        /// Setting doesn't change capacity, an error will be thrown if the buffer can't fit the count.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Attempting to set the count to a value larger than the allocated capacity.</exception>
        public long Count 
        { 
            get 
            { 
                return m_size; 
            } 
            set 
            { 
                if (value > m_capacity) 
                    throw new IndexOutOfRangeException();
                m_size = value;
            } 
        }

        /// <summary>
        /// Get or set the capacity of the buffer. This is the maximum number of elements in the buffer when full.
        /// Changing the capacity will reallocate the buffer and clear the contents.
        /// </summary>
        public long Capacity 
        {
            get 
            { 
                return m_capacity; 
            } 
            set 
            { 
                Reserve(value); 
            } 
        }

        /// <summary>
        /// Index into the buffer.
        /// Negative indices index backwards from the end. So -1 is the last element, -2 is second last, etc.
        /// </summary>
        /// <param name="index">Index within the buffer. Positive indices go from 0 to Count-1. Negative indices (reverse direction) go from -1 to -Count.</param>
        /// <returns>Value at index.</returns>
        /// <exception cref="IndexOutOfRangeException">Attempting to index outside of the range of the buffer.</exception>
        public T this[long index]
        {
            get
            {
                if (m_size == 0)
                {
                    throw new IndexOutOfRangeException();
                }
                else if (index >= 0 && index < m_size)
                {
                    long i = (m_start + index) % m_capacity;
                    return m_data[i];
                }
                else if (index < 0 && index >= -m_size)
                {
                    long offset = (index + m_size);
                    long i = (m_start + offset) % m_capacity;
                    return m_data[i];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (m_size == 0)
                {
                    throw new IndexOutOfRangeException();
                }
                else if (index >= 0 && index < m_size)
                {
                    long i = (m_start + index) % m_capacity;
                    m_data[i] = value;
                }
                else if (index < 0 && index >= -m_size)
                {
                    long offset = (index + m_size);
                    long i = (m_start + offset) % m_capacity;
                    m_data[i] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Push a new element onto the end of the buffer.
        /// If the buffer isn't full, it goes in the next available slot.
        /// If the buffer is full, it overwrites the first element of the buffer and the second element becomes the new first.
        /// </summary>
        /// <param name="value">The value to push onto the end of the buffer.</param>
        /// <returns>Reference to the CircularBuffer for fluent coding.</returns>
        public CircularBuffer<T> Push(T value)
        {
            long i = (m_start + m_size) % m_capacity;
            m_data[i] = value;
            if (m_size < m_capacity)
                m_size++;
            else if (m_size == m_capacity)
            {
                m_start = (m_start + 1) % m_capacity;
            }
            return this;
        }

        /// <summary>
        /// Push a new element onto the beginning of the buffer.
        /// If the buffer isn't full, it goes before the first element.
        /// If the buffer is full, it overwrites the last element of the buffer and the second last element becomes the new last.
        /// </summary>
        /// <param name="value">The value to push onto the beginning of the buffer.</param>
        /// <returns>Reference to the CircularBuffer for fluent coding.</returns>
        public CircularBuffer<T> PushFront(T value)
        {
            long i = m_start == 0 ? m_capacity - 1 : m_start - 1;
            m_data[i] = value;
            m_start = i;
            if (m_size < m_capacity)
                m_size++;
            return this;
        }

        /// <summary>
        /// Remove the last element of the buffer and return it.
        /// The count of elements in the buffer decreases by 1.
        /// Capacity isn't changed.
        /// </summary>
        /// <returns>The last value in the buffer.</returns>
        /// <exception cref="IndexOutOfRangeException">Attempt to pop an empty buffer.</exception>
        public T Pop()
        {
            if (m_size > 0)
            {
                m_size--;
                long i = (m_start + m_size) % m_capacity;
                return m_data[i];
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Remove the first element of the buffer and return it.
        /// The count of elements in the buffer decreases by 1.
        /// Capacity isn't changed.
        /// </summary>
        /// <returns>The last value in the buffer.</returns>
        /// <exception cref="IndexOutOfRangeException">Attempt to pop an empty buffer.</exception>
        public T PopFront()
        {
            if (m_size > 0)
            {
                m_size--;
                long i = m_start;
                m_start = (m_start + 1) % m_capacity;
                return m_data[i];
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Predicate that checks if the buffer is empty.
        /// </summary>
        /// <returns>true if the buffer is empty, otherwise false.</returns>
        public bool Empty()
		{
			return m_size == 0;
		}

        // Is the buffer full?
        /// <summary>
        /// Predicate that checks if the buffer is full.
        /// </summary>
        /// <returns>true if the buffer is full, otherwise false.</returns>
        public bool Full()
		{
			return m_size == m_capacity;
		}

        /// <summary>
        /// Get a reference to the first element in the buffer.
        /// </summary>
        /// <returns>Reference to the first element.</returns>
        /// <exception cref="IndexOutOfRangeException">Attempt to access the first element of an empty buffer.</exception>
        public ref T Front()
        {
            if (m_size == 0)
            {
                throw new IndexOutOfRangeException();
            }
            return ref m_data[m_start];
        }

        /// <summary>
        /// Get a reference to the last element in the buffer.
        /// </summary>
        /// <returns>Reference to the last element.</returns>
        /// <exception cref="IndexOutOfRangeException">Attempt to access the last element of an empty buffer.</exception>
        public ref T Back()
        {
            if (m_size == 0)
            {
                throw new IndexOutOfRangeException();
            }
            long i = (m_start + m_size - 1) % m_capacity;
            return ref m_data[i];
        }


        /// <summary>
        /// Get an enumerator for the circular buffer class.
        /// </summary>
        /// <returns>A new CircularBufferEnumerator class.</returns>
        public IEnumerator GetEnumerator()
        {
            return new CircularBufferEnumerator<T>(this);
        }

        /// <summary>
        /// Enumerator for circuler buffer class. This allows foreach and similar functionality.
        /// </summary>
        /// <typeparam name="TE">Type of elements in the circular buffer.</typeparam>
        public class CircularBufferEnumerator<TE>:IEnumerator
        {
            CircularBuffer<TE> m_buffer;
            long m_position = -1;

            public CircularBufferEnumerator(CircularBuffer<TE> buffer)
            {
                m_buffer = buffer;
            }

            public bool MoveNext()
            {
                m_position++;
                return (m_position < m_buffer.Count);
            }

            //IEnumerator
            public void Reset()
            {
                m_position = -1;
            }

            //IEnumerator
            public object Current
            {
                get
                {
                    try
                    {
                        return m_buffer[m_position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}
