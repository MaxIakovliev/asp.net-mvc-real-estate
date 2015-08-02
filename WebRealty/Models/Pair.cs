using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRealty.Models
{
    public class Pair<T, K>
    {
        public Pair()
        {

        }
        public Pair(T key, K value)
        {
            this.Key = key;
            this.Value = value;
        }
        public T Key { get; set; }
        public K Value { get; set; }
    }
}