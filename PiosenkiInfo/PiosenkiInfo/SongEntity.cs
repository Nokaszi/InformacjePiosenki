using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace DevEnvExe_LocalStorage
{  
    class SongEntity
    {
        public SongEntity()
        {

        }
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string Song_id { get; set; }
    }
}
