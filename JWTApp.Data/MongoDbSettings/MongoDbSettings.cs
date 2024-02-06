﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Data.MongoDbSettings
{
    public class MongoDbSettings
    {
        public string UserCollection { get; set; }
        
        public string MovieCollection { get; set; }
        
        public string TokenCollection { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
