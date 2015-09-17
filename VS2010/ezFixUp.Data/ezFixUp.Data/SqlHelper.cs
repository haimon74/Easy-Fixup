using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class SqlHelper
    {
        public static Database GetDB(){
            return DatabaseFactory.CreateDatabase();
        }
    }
}
