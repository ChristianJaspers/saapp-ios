using System;

namespace BetterSalesman.Core.DataLayer
{
    public static class DatabaseHelper
    {
        public static void Replace<T>(T @object)
        {
            using (var conn = DatabaseProvider.OpenConnection())
            {
                conn.DeleteAll<T>();
                Insert(@object);
            }
        }
        
        public static void Insert<T>(T @object)
        {
            using (var conn = DatabaseProvider.OpenConnection())
            {
                conn.Insert(@object);
            }
        }
    }
}

