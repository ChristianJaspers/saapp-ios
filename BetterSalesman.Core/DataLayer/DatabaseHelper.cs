using BetterSalesman.Core.BusinessLayer.Contracts;
using SQLite;

namespace BetterSalesman.Core.DataLayer
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Replace the specified object.
        /// </summary>
        /// <param name="object">IBusinessEntity Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static void Replace<T>(IBusinessEntity @object)
        {
            using (var conn = DatabaseProvider.OpenConnection())
            {
                conn.BeginTransaction();
                
                conn.Delete<T>(@object.Id);
                Insert(@object,conn);
                
                conn.Commit();
            }
        }

        /// <summary>
        /// Insert the specified object and connection.
        /// </summary>
        /// <param name="object">IBusinessEntity Object.</param>
        /// <param name="connection">Connection.</param>
        public static void Insert(IBusinessEntity @object, SQLiteConnection connection = null)
        {
            if (connection != null)
            {
                connection.Insert(@object);
            }
            else
            {
                using (var conn = DatabaseProvider.OpenConnection())
                {
                    conn.Insert(@object);
                }
            }
        }
        
        /// <summary>
        /// Insert or update given entity
        /// </summary>
        /// <param name="object">IBusinessEntity Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void InsertOrUpdate<T>(IBusinessEntity @object) where T : new()
        {
            var findObject = Get<T>(@object.Id);
            if ( !findObject.Equals(default(T)) )
            {
                Replace<T>(@object);
            } else
            {
                Insert(@object);
            }
        }
        
        /// <summary>
        /// Get the specified object 
        /// </summary>
        /// <param name="pk">Primary key</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Get<T>(object pk) where T : new()
        {
            T obj;
            
            using (var conn = DatabaseProvider.OpenConnection())
            {
                obj = conn.Get<T>(pk);
            }
            
            return obj;
        }
    }
}

