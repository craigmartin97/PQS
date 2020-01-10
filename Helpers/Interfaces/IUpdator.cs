using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Interfaces
{
    public interface IUpdator<T>
    {
        /// <summary>
        /// Update the details of the given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Update(T item);
    }
}
