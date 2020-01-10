using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Interfaces
{
    public interface IDeletor<T>
    {
        bool Delete(T item);
    }
}
