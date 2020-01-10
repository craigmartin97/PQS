using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Interfaces
{
    public interface IAdvancedRetrievor<T, R>
    {
        IList<R> GetFilteredList(T item);
    }
}
