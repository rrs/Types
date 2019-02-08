using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Rrs.Types
{
    interface IDualInterface<T1,T2>
    {
    }

    class DualGenericImplementation<T> : IDualInterface<IGenericInterface<T>, T>
    {
    }
}
