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
        public static bool CopyInitialDatabase;

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
        public static string DatabaseFilename
        {
            get { return "db.sqlite3"; }
        }

        /// <summary>
        /// Full path to database file.
        /// </summary>
        /// <returns>The path.</returns>
        public static string DatabaseFileFullPath = Path.Combine(Filepath, DatabaseFilename);

        /// <summary>
        /// Full synchronisation of database
        /// </summary>
        /// <param name = "containerData">Contains DTO object to save</param>
        /// <param name="filePath">Path to database file.</param>
        public static void FullSync(T containerData, string filePath = "")
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                Filepath = filePath;
            }

			#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
			#endif

            try
            {
                SaveDataFromMemory(containerData);
            }
            catch (Exception e)
            {
                throw e;
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
            if ( CopyInitialDatabase )
            {
                #if SILVERLIGHT
                string resourcePath = string.Format("Assets/Data/{0}.sqlite3", HttpConfig.Lang);
                CopyFileIfNotExists(resourcePath, DatabaseFileFullPath);
                #elif NETFX_CORE
                TODO - implement on Windows Desktop and RT
                #elif __IOS__
                string resourcePath = Path.Combine(Environment.CurrentDirectory, DatabaseFilename);
                CopyFileIfNotExists(resourcePath, DatabaseFileFullPath);
                #elif __ANDROID__
                // Android has platform specfic mechanism
                #endif
            }
            
            // Automatic migration purpose if DB changed beetwen different app versions
            using (var connection = OpenConnection())
            {
                CreateTablesIfNotExist(connection);
            }
        }

        /// <summary>
        /// Database connection instance
        /// </summary>
        public static SQLiteConnection OpenConnection()
        {
			return new SQLiteConnection(DatabaseFileFullPath, SQLiteOpenFlags.ReadWrite);
        }

        private static void CopyFileIfNotExists(string sourcePath, string destinationPath)
        {
            if (!File.Exists(destinationPath))
            {
                Debug.WriteLine("Database file not found. Copying database from resources...");
                File.Copy(sourcePath, destinationPath);
            }
        }

        /// <summary>
        /// Saves the data from memory.
        /// </summary>
        private static void SaveDataFromMemory(T containerData)
        {
			if (containerData == null)
            {
                Debug.WriteLine("ERROR! EventContainer is null.");
                return;
            }

            using (var connection = OpenConnection())
            {
                try
                {
                    connection.BeginTransaction();

                    CreateTablesIfNotExist(connection);

                    InsertRecords(connection, containerData);

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

        private static void CreateTablesIfNotExist(SQLiteConnection connection)
        {
            Debug.WriteLine("DB create if not exists");

            connection.CreateTable<User>();
            connection.CreateTable<ProductGroup>();
            connection.CreateTable<Argument>();

            Debug.WriteLine("DB finished creating tables if not exist");
        }
			
        private static void InsertRecords(SQLiteConnection connection, T containerData)
        {
            DatabaseHelper.ReplaceAll(containerData.Users,connection);
            DatabaseHelper.ReplaceAll(containerData.ProductGroups,connection);
            DatabaseHelper.ReplaceAll(containerData.Arguments,connection);
        }
    }
}