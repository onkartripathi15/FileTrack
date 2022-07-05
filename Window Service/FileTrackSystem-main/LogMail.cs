using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FileTrackService
{
    public class LogMail
    {
        public int Id { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
   

    }
}
