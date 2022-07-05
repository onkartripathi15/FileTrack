using FileTrackService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Timers;

namespace SimpleService
{
    internal class FileTrack
    {

        private readonly Timer _timer;
        public FileTrack()
        {
            _timer = new Timer(5000) { AutoReset = true };   //it will check folder after every 5 sec if any new file is added

            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {

            using (var results = new ApplicationDbContext())
            {
                int k = 0;
                var st = results.UserPath3.ToList();

                string sourceFile = "";
                string destinationFolderName = "";
                string mailTo = "";
                foreach (UserClass stobj in st)
                {

                    sourceFile = stobj.source.ToString();
                    destinationFolderName = stobj.destination.ToString();
                    mailTo=stobj.MailTo.ToString();

                }



                //    string sourceFile = @"C:\Users\thinksysuser\Desktop\Data\OldData";  
                //string destinationFolderName = @"C:\Users\thinksysuser\Desktop\Data\NewData\";
                string[] getAllFiles = Directory.GetFiles(sourceFile);

                //string[] getAllFiles = Directory.GetFiles(sourceFile, "*.txt");
                int fileCount = Directory.GetFiles(sourceFile, "*.*", SearchOption.AllDirectories).Length;

                while (fileCount > 0)
                {
                    if (fileCount < 25)
                    {
                        for (int i = 0; i < fileCount; i++) ///for archiving files
                        //foreach (var file in getAllFiles)
                        {

                            var file = getAllFiles[i];
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            var extension = Path.GetExtension(file);
                            var destFileName = destinationFolderName + fileName + extension;
                            File.Move(file, destFileName);

                        }

                        string[] txtFiles; //for combining all file into 1
                        txtFiles = Directory.GetFiles(destinationFolderName);
                        using (StreamWriter writer = new StreamWriter(@"C:\Users\thinksysuser\Desktop\Data\NewData\Combine\" + @"\allfiles.txt"))
                        {
                            for (int i = 0; i < fileCount; i++)
                            {
                                using (StreamReader reader = File.OpenText(txtFiles[i]))
                                {
                                    writer.Write(reader.ReadToEnd());
                                }
                                fileCount--;
                            }


                        }
                 
                    }


                    else if (fileCount > 25)
                    {
                        for (int i = 0; i < 25; i++)              //moving 25 files to a new folder (archieving)

                        {

                            var file = getAllFiles[i];
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            var extension = Path.GetExtension(file);
                            var destFileName = destinationFolderName + fileName + extension;
                            File.Move(file, destFileName);

                        }

                        string[] txtFiles; //cpombine 25 files into 1
                        txtFiles = Directory.GetFiles(destinationFolderName);
                        using (StreamWriter writer = new StreamWriter(@"C:\Users\thinksysuser\Desktop\Data\NewData\Combine\" + @"\allfiles.txt"))
                        {
                            for (int i = 0; i < 25; i++)
                            {
                                using (StreamReader reader = File.OpenText(txtFiles[i]))
                                {
                                    writer.Write(reader.ReadToEnd());
                                }

                            }
                        }
                   



                        fileCount = fileCount - 25;
                    }
                }
                //httpost
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44395/api/");
                    var mail = new MailModel() { ToEmail = mailTo,Subject="here is a random subject", Body = File.ReadAllText(@"C:\Users\thinksysuser\Desktop\Data\NewData\Combine\allfiles.txt") };
              
                    var postTask = client.PostAsJsonAsync<MailModel>("mail/send", mail);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        using (var context = new ApplicationDbContext())
                        {
                            LogMail log = new LogMail { ToEmail = mail.ToEmail, Subject = mail.Subject, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay };
                            context.LogEmails1.Add(log);
                            context.SaveChanges();
                        }
                        Console.WriteLine("suceess");
                    }
                    else
                    {
                        Console.WriteLine(result.StatusCode);
                    }

                }

            }
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
    }
}
