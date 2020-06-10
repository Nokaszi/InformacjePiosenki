using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiosenkiInfo
{
    class SQLHelperUser
    {
        static object locker = new object();
        SQLiteConnection database;
        public SQLHelperUser()
        {
            database = GetConnection();
            database.CreateTable<User>();
        }
        public SQLiteConnection GetConnection()
        {
            SQLiteConnection sqlitConnection;
            var sgliteFilename = "users.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sgliteFilename);
            sqlitConnection = new SQLite.SQLiteConnection(path);
            return sqlitConnection;
        }
        public IEnumerable<User> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<User>() select i).ToList();
            }
        }
        public User GetItem(string name)
        {
            lock (locker)
            {
                return database.Table<User>().FirstOrDefault(x => x.Name == name);
            }
        }
        public int SaveItem(User item)
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    database.Update(item);
                    return item.ID;

                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<User>(id);
            }
        }
    }
}
