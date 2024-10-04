using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Ingredients.Class
{
    internal class ConnectionString
    {
        public static string GetConnectionString()
        {
            
            return "Server=localhost;Database=milestone;uid=root;Password=Agentfive5!;Pooling=true;Max Pool Size=100;";

        }


    }
}
