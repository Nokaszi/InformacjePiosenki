using SQLite;
using System.Collections.Generic;
using System.Linq;
using PCLStorage;
using Xamarin.Forms;

namespace DevEnvExe_LocalStorage
{
    class SQLHelper
    {
        static object locker = new object();
        SQLiteConnection database;
        public SQLHelper(string user)
        {
            database = GetConnection(user);
            database.CreateTable<SongEntity>();
        }
        public SQLiteConnection GetConnection(string user)
        {
            SQLiteConnection sqlitConnection;
            var sgliteFilename = user+".db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sgliteFilename);
            sqlitConnection = new SQLite.SQLiteConnection(path);
            return sqlitConnection;
        }
        public IEnumerable<SongEntity> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<SongEntity>() select i).ToList();
            }
        }
        public SongEntity GetItem(string id_song)
        {
            lock (locker)
            {
                return database.Table<SongEntity>().FirstOrDefault(x => x.Song_id == id_song);
            }
        }
        public int SaveItem(SongEntity item)
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
                return database.Delete<SongEntity>(id);
            }
        }
    }
}
