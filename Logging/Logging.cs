namespace Ticketing.Logging;
public class Logging: ILogging
{
      public void Log(string message, string level){
            

            switch (level.ToLower())
            {
                  case "error": 
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{level.ToUpper()}: {message}");
                  break;

                  case "warning": 
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{level.ToUpper()}: {message}");
                  break;

                  case "info": 
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{level.ToUpper()}: {message}");
                  break;

                  default: 
                        Console.WriteLine($"{level.ToUpper()}: {message}");
                  break;
            }
      }
}