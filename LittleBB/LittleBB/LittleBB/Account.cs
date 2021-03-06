﻿using System;
using SQLite;

namespace LittleBB
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        //Can add additional notes if deemed neccessary in the future
        public string A_Note { get; set; }
        public string B_Note { get; set; }
        public string C_Note { get; set; }
        public string D_Note { get; set; }
        public string E_Note { get; set; }

        //Way to differentiate between new and existing contacts
        public string Status { get; set; }
    }
}
