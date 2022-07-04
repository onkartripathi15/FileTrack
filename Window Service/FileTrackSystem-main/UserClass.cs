using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleService
{
    internal class UserClass
    {
        [Key]
        public int Id { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
    }
}
