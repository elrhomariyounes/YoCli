using System.Collections.Generic;

namespace YoCli.Commons
{
    interface IContext<T>
    {
        public List<T> Data { get; }

        void SaveChanges();
    }
}
