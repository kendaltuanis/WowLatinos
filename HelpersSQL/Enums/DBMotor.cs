using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpersSQL.Enums
{
    public enum DBMotor
    {
        Postgresql,
        SqlServer,
        // MySql, Este se hace desde un elemento llamado cuando se hace el insert LastInsertedId
        None
    }
}
