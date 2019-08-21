using System;
using Informer.Models;
using SQLite;

namespace Informer.Utils
{
    public class LocalDatabase
    {
        readonly SQLiteAsyncConnection database;

        public LocalDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Item>().Wait();
        }
    }

}
