using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.ModulesHelpers
{
    public class ModuleChanger
    {
        protected DateTime? GetDisabledDateTime(bool isModuleDisabled)
        {
            if (isModuleDisabled) return DateTime.Now;
            else return null;
        }
    }
}
