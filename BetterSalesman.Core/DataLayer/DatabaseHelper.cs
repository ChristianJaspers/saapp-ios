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
			lock (DatabaseProvider.databaseWriteLocker)
			{
	            using (var conn = DatabaseProvider.OpenConnectionReadWrite())
	            {
	                conn.BeginTransaction();
	                
	                conn.Delete<T>(@object.Id);
	                conn.Insert(@object);
	                
	                conn.Commit();
	            }
			}
        }

        /// <summary>
        /// Insert the specified object and connection.
        /// </summary>
        /// <param name="object">IBusinessEntity Object.</param>
        /// <param name="connection">Connection.</param>
        public static void Insert(IBusinessEntity @object, SQLiteConnection connection = null)
        {
			lock (DatabaseProvider.databaseWriteLocker)
			{
	            using (var conn = DatabaseProvider.OpenConnectionReadWrite())
	            {
	                conn.Insert(@object);
	            }
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
            
            using (var conn = DatabaseProvider.OpenConnectionReadOnly())
            {
                obj = conn.Get<T>(pk);
            }
            
            return obj;
        }
        
        /// <summary>
        /// Get all objects of given type 
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> GetAll<T>() where T : new()
        {
            List<T> obj = default(List<T>);

            using (var conn = DatabaseProvider.OpenConnectionReadOnly())
            {
                obj = conn.Table<T>().ToList();
            }

            return obj;
        }
        
        /// <summary>
        /// Replaces all entities of given type with transaction
        /// </summary>
        /// <param name="items">List of items.</param>
        public static void ReplaceAll<T>(List<T> items)
        {
			lock (DatabaseProvider.databaseWriteLocker)
			{
	            using (var conn = DatabaseProvider.OpenConnectionReadWrite())
	            {
	                conn.BeginTransaction();
	                ReplaceAll(items, conn);
	                conn.Commit();
	            }
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
			lock (DatabaseProvider.databaseWriteLocker)
			{
	            connection.DeleteAll<T>();
	            InsertAll<T>(connection, items);
			}
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
				lock (DatabaseProvider.databaseWriteLocker)
				{
	                foreach (var obj in objects)
	                {
	                    connection.Insert(obj);
	                }
				}
            }
        }
    }
}

