using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.ServiceAccessLayer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;

namespace BetterSalesman.Core.DataLayer
{
    /// <summary>
    /// Database provider.
    /// </summary>
    public static class DatabaseProvider
    {
		/// <summary>
		/// Ensures that write access to the database is serialized and doesn't cause exceptions
		/// 
		/// @note Only use this object in DatabaseProvider and DatabaseHelper
		/// </summary>
		public static object databaseWriteLocker = new object();

        /// <summary>
        /// The filepath.
        /// </summary>
        #if SILVERLIGHT
        private static string Filepath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        #elif NETFX_CORE
        TODO - implement on Windows Desktop and RT
        #elif __IOS__
        static string Filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        #elif __ANDROID__
        private static string Filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        #else
        private static string Filepath = "";
        #endif

        /// <summary>
        /// The database file name.
        /// </summary>
		public const string DatabaseFilename = "db.sqlite3";

        /// <summary>
        /// Full path to database file.
        /// </summary>
        /// <returns>The path.</returns>
        public static string DatabaseFileFullPath = Path.Combine(Filepath, DatabaseFilename);

        /// <summary>
        /// This method overwites all existing data in the database with data from synchronization request
        /// </summary>
        /// <param name = "containerData">Contains DTO object to save. Note! It's caller's responsibility to ensure that containerData is not null when calling this method.</param>
		public static void FullSync(SynchronizationDataContainer containerData)
        {
			Debug.Assert(containerData != null, "FullSync method requires containerData to not be null");

			#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
			#endif

			lock (databaseWriteLocker)
			{
		        try
		        {
					using (var connection = OpenConnectionReadWrite())
					{
						try
						{
							connection.BeginTransaction();

							CreateTablesIfNotExist(connection);

							ReplaceRecords(connection, containerData);

							connection.Commit();
						}
						catch (Exception ex)
						{
							// try catch for dev purpose / easier debuging
							Debug.WriteLine("Message: " + ex.Message);
							Debug.WriteLine("Stack: " + ex.StackTrace);
							Debug.WriteLine("Source: " + ex.Source);
						}
					}
				}
		        catch (Exception e)
		        {
		            throw e;
		        }
			}

			#if DEBUG
			sw.Stop();
			long microseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)) / 1000;
			Debug.WriteLine("-----> DB Task completed in : {0} ms", microseconds);
			#endif
        }

        /// <summary>
        /// Setup this instance.
        /// </summary>
        public static void Setup()
        {   
			lock (databaseWriteLocker)
			{
	            using (var connection = OpenConnectionReadWrite())
	            {
	                CreateTablesIfNotExist(connection);
	            }
			}
        }

        /// <summary>
        /// Database connection instance
		///
		/// @note ALWAYS call lock (databaseWriteLocker) after calling this method and enclose all the code using that connection in the lock block
		/// 	  to ensure that there's no simultaneous write access to database and avoid exceptions caused by it
        /// </summary>
        public static SQLiteConnection OpenConnectionReadWrite()
        {
			return new SQLiteConnection(DatabaseFileFullPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
        }

		public static SQLiteConnection OpenConnectionReadOnly()
		{
			return new SQLiteConnection(DatabaseFileFullPath, SQLiteOpenFlags.ReadOnly);
		}

        private static void CreateTablesIfNotExist(SQLiteConnection connection)
        {
            Debug.WriteLine("DB create if not exists");

            connection.CreateTable<User>();
            connection.CreateTable<ProductGroup>();
            connection.CreateTable<Argument>();

            Debug.WriteLine("DB finished creating tables if not exist");
        }
			
		private static void ReplaceRecords(SQLiteConnection connection, SynchronizationDataContainer containerData)
        {
            DatabaseHelper.ReplaceAll(containerData.Users,connection);
            DatabaseHelper.ReplaceAll(containerData.ProductGroups,connection);
            DatabaseHelper.ReplaceAll(containerData.Arguments,connection);
        }
    }
}