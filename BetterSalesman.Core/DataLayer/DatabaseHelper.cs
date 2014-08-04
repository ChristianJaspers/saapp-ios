using BetterSalesman.Core.BusinessLayer.Contracts;
using SQLite;
using System.Linq;
using System.Collections.Generic;

namespace BetterSalesman.Core.DataLayer
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Replace the specified object.
        /// </summary>
        /// <param name="object">IBusinessEntity Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void Replace<T>(IBusinessEntity @object)
        {
            using (var conn = DatabaseProvider.OpenConnection())
            {
                conn.BeginTransaction();
                
                conn.Delete<T>(@object.Id);
                conn.Insert(@object);
                
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
            using (var conn = DatabaseProvider.OpenConnection())
            {
                conn.Insert(@object);
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
        
        /// <summary>
        /// Get all objects of given type 
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static TableQuery<T> GetAll<T>() where T : new()
        {
            TableQuery<T> obj = default(TableQuery<T>);

            using (var conn = DatabaseProvider.OpenConnection())
            {
                obj = conn.Table<T>();
            }

            return obj;
        }
        
        /// <summary>
        /// Replaces all entities of given type with transaction
        /// </summary>
        /// <param name="items">List of items.</param>
        public static void ReplaceAll<T>(List<T> items)
        {
            using (var conn = DatabaseProvider.OpenConnection())
            {
                conn.BeginTransaction();
                ReplaceAll(items, conn);
                conn.Commit();
            }
        }
        
        /// <summary>
        /// Replaces all entities of given type with given entities
        /// </summary>
        /// <param name="items">Items.</param>
        /// <param name="connection">Connection.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ReplaceAll<T>(List<T> items, SQLiteConnection connection)
        {
            connection.DeleteAll<T>();
            InsertAll<T>(connection, items);
        }
        
        /// <summary>
        /// Inserts all entities of given type
        /// </summary>
        /// <param name="connection">Connection.</param>
        /// <param name="objects">Enumerable list of objects.</param>
        private static void InsertAll<T>(SQLiteConnection connection, IEnumerable<T> objects)
        {
            if (objects != null)
            {
                foreach (var obj in objects)
                {
                    connection.Insert(obj);
                }
            }
        }
    }
}

