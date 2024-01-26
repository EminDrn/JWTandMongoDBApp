using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Core.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string MovieName { get; set; }

        public string MovieDescription { get; set; }
        public string MoviePhoto {  get; set; }

    }
}
