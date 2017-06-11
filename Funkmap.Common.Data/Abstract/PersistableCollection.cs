using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Funkmap.Common.Data.Abstract
{
    [ComplexType]
    public abstract class PersistableCollection<T> : ICollection<T>
    {
        private const string DefaultValueSeperator = "|";
        private List<T> Data { get; set; }

        protected PersistableCollection()
        {
            Data = new List<T>();
        }

        protected PersistableCollection(IEnumerable<T> source)
        {
            Data = new List<T>(source);
        }
        protected abstract T ConvertSingleValueToRuntime(string rawValue);
        
        protected abstract string ConvertSingleValueToPersistable(T value);
        
        protected virtual string ValueSeperator => DefaultValueSeperator;
        
        protected virtual string[] ValueSeperators { get; } = { DefaultValueSeperator };
        
        public string SerializedValue
        {
            get
            {
                var serializedValue = string.Join(ValueSeperator, Data.Select(ConvertSingleValueToPersistable).ToArray());
                return serializedValue;
            }
            set
            {
                Data.Clear();

                if (string.IsNullOrEmpty(value)) return;
                Data = new List<T>(value.Split(ValueSeperators, StringSplitOptions.None)
                    .Select(ConvertSingleValueToRuntime));
            }
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            Data.Add(item);
        }

        public void Clear()
        {
            Data.Clear();
        }

        public bool Contains(T item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Data.CopyTo(array, arrayIndex);
        }

        public int Count => Data.Count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            return Data.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        #endregion
    }
}
