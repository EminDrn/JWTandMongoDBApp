﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Core.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string MovieName { get; set; }

        public string MovieDescription { get; set; }

        public double Rating { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string MoviePhoto {  get; set; }



    }
}