using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using LittleBB.iOS;

[assembly: Dependency(typeof(SQLiteDb))]

namespace LittleBB.iOS
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "LittleBB.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}