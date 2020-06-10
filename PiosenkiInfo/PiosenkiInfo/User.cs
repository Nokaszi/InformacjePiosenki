using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PiosenkiInfo
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }

        public string Password { get; set; }
    }

}
