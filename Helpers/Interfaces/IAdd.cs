using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Interfaces
{
    public interface IAdd<T>
    {
        bool Add(T item);
    }
}
