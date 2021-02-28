using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace bela
{
    public class Result
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MiResult { get; set; }
        public int ViResult { get; set; }
        public int MiZvanje { get; set; }
        public int ViZvanje { get; set; }
        public int Zvali { get; set; }
    }
}
