﻿using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.ServiceAccessLayer;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BetterSalesman.Core.DataLayer
{
    /// <summary>
    /// Database provider.
    /// </summary>
    public static class DatabaseProvider
    {
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
            get { return HttpConfig.Lang + ".sqlite3"; }
        }

        /// <summary>
        /// Full path to database file.
        /// </summary>
        /// <returns>The path.</returns>
        public static string DatabaseFileFullPath = Path.Combine(Filepath, DatabaseFilename);

        /// <summary>
        /// Determinate status of procedure
        /// </summary>
        public static bool InProgress = false;

        /// <summary>
        /// Full synchronisation of database
        /// </summary>
        /// <param name="filePath">Path to database file.</param>
        public static void FullSync(/*Container containerData, */string filePath = "")
        {
            // TODO - remove stopwatch code
            Debug.WriteLine("In Full sync");
            if (InProgress)
            {
                Debug.WriteLine("DB Task in progress - skip");
                return;
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                Filepath = filePath;
            }

            InProgress = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                // TODO
//                SaveDataFromMemory(eventItem);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                sw.Stop();

                long microseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)) / 1000;

                Debug.WriteLine("-----> DB Task completed in : {0} ms", microseconds);

                InProgress = false;
            }
        }

        /// <summary>
        /// Copies the initial database.
        /// </summary>
        public static void CopyInitialDatabase()
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
            // Android has platform specfic mecha
            #endif
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
            return new SQLiteConnection(DatabaseFileFullPath);
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
        private static void SaveDataFromMemory(/*Container containerData*/)
        {
            // TODO
//            if (eventData == null)
//            {
//                Debug.WriteLine("ERROR! EventContainer is null.");
//                return;
//            }

            using (var connection = OpenConnection())
            {
                #if DEBUG
                //connection.Trace = true;
                //connection.TimeExecution = true;
                #endif

                try
                {
                    connection.BeginTransaction();

                    CreateTablesIfNotExist(connection);

                    DeleteAllRecords(connection);

//                    InsertRecords(connection, eventData);

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
            connection.CreateTable<Product>();
            connection.CreateTable<Argument>();

            Debug.WriteLine("DB ready to rumble if not exists");
        }

        private static void DeleteAllRecords(SQLiteConnection connection)
        {
            Debug.WriteLine("Deleting records...");

            connection.DeleteAll<Product>();
            connection.DeleteAll<Argument>();

            Debug.WriteLine("Finished deleting records");
        }

        // TODO fill container with data
        private static void InsertRecords(SQLiteConnection connection/*, Container containerData*/)
        {

        }

        private static void InsertAll(SQLiteConnection connection, IEnumerable objects)
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