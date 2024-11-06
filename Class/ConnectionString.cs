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
            
       //   return "Server=localhost;Database=milestone;uid=root;Password=arcamark_1127;Pooling=true;Max Pool Size=100;";

            //return "Server=DESKTOP-GIRK3UQ;Database=milestonedb;uid=Giebert;Password=giebert_123;Pooling=true;Max Pool Size=100;";

         return "Server=localhost;Database=milestonedb;uid=root;Password=adminspcg0612#;Pooling=true;Max Pool Size=100;";


        }


    }
}
