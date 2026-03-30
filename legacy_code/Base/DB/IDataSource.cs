using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Base.DB
{
    interface IDataSource
    {
        DbConnection GetConnection();

        void FreeConnection(DbConnection conn);

        DbDataAdapter GetDataAdapter();

    }
}
