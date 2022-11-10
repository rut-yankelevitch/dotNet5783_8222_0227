// See https://aka.ms/new-console-template for more information
namespace Targil0
{
 class Program 
    {
        Console.writeline("ttttt");
        static public int main(string args[])
        {
         Random rnd = new Random();
         
         TimeSpan timeSpent = TimeSpan.Zero;
         
         timeSpent += GetTimeBeforeLunch();
         timeSpent += GetTimeAfterLunch();
         
         Console.WriteLine("Total time: {0}", timeSpent);
         
        imeSpan GetTimeBeforeLunch()
        {
            return new TimeSpan(rnd.Next(3, 6), 0, 0);
        }

        TimeSpan GetTimeAfterLunch()
        {
            return new TimeSpan(rnd.Next(3, 6), 0, 0);
        }

        }
    }
}


