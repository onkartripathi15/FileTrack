using System;
using Topshelf;

namespace SimpleService
{
    internal class Program
    {
        static void Main(string[] args)
        {
          var exitCode =HostFactory.Run(x=>
          {
              x.Service<FileTrack>(s=>
                  {
                      s.ConstructUsing(FileTrack => new FileTrack());
                      s.WhenStarted(heartbear => heartbear.Start());
                      s.WhenStopped(heartbear => heartbear.Stop());
                  } );

              x.RunAsLocalSystem();

              x.SetServiceName("FileTrack");
              x.SetDisplayName("File Track Service");
              x.SetDescription("A file track service ehich keeps an eye on folder incoming file");
          });
            int exitCodeValue=(int)Convert.ChangeType(exitCode,exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;

        }
    }
}
