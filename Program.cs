using System;

namespace Assignment3
{
    class Program
    {
        // used to create a random selection of sample folders and files for the user to interact with
        static void init(FileSystem file)
        {
            file.AddDirectory("/home/seth/");
            file.AddDirectory("/home/seth/downloads/");
            file.AddDirectory("/home/seth/documents/");
            file.AddDirectory("/home/seth/pictures/");
            file.AddDirectory("/home/seth/videos/");

            file.AddDirectory("/home/seth/documents/school/");
            file.AddFile("/home/seth/documents/school/sup.txt");
            file.AddFile("/home/seth/documents/school/sup1.txt");
            file.AddFile("/home/seth/documents/school/sup2.txt");
            file.AddFile("/home/seth/documents/school/sup3.txt");

            file.AddDirectory("/home/seth/documents/school/COIS2020/");
            file.AddFile("/home/seth/documents/school/COIS2020/this.txt");
            file.AddFile("/home/seth/documents/school/COIS2020/is.txt");
            file.AddFile("/home/seth/documents/school/COIS2020/the.txt");
            file.AddFile("/home/seth/documents/school/COIS2020/best.txt");
            file.AddFile("/home/seth/documents/school/COIS2020/class.txt");
            file.AddFile("/home/seth/documents/school/COIS2020/ever.txt");
            

            file.AddDirectory("/home/seth/documents/school/COIS2830/");

            file.AddDirectory("/home/seth/documents/school/COIS1010/");
            file.AddFile("/home/seth/documents/school/COIS1010/sup.txt");
            file.AddFile("/home/seth/documents/school/COIS1010/sup1.txt");
            file.AddFile("/home/seth/documents/school/COIS1010/sup2.txt");
            file.AddFile("/home/seth/documents/school/COIS1010/sup3.txt");

            file.AddDirectory("/home/seth/documents/games/");
            file.AddDirectory("/home/seth/documents/games/terraria/");
            file.AddDirectory("/home/seth/documents/games/csgo/");
            file.AddDirectory("/home/seth/documents/games/insurgency/");
            file.AddDirectory("/home/seth/documents/games/borderlands/");
            file.AddDirectory("/home/seth/documents/games/minecraft/");
            file.AddDirectory("/home/seth/documents/games/frogger/");
        }


        static void RunParameterCommand(FileSystem file, string x)
        {
            // check the start of the string names to find out what parameter dependent command they are referencing and run the applicable command using the parameters as input.
            // it then prints out the return value of the command of either True or False
            if (x.Substring(0, x.IndexOf(' '))== "mkdir")
            {
                System.Console.WriteLine(file.AddDirectory(x.Substring(x.IndexOf(' ')+1)));
            }
            else if (x.Substring(0, x.IndexOf(' '))== "rmdir")
            {
                System.Console.WriteLine(file.RemoveDirectory(x.Substring(x.IndexOf(' ')+1)));
            }
            else if (x.Substring(0, x.IndexOf(' '))== "mkfile")
            {
                System.Console.WriteLine(file.AddFile(x.Substring(x.IndexOf(' ')+1)));
            }
            else if (x.Substring(0, x.IndexOf(' '))== "rmfile")
            {
                System.Console.WriteLine(file.RemoveFile(x.Substring(x.IndexOf(' ')+1)));
            }
            else // if none of the above cases are executed, remind the user that they can use help to learn the commands
            {
                System.Console.WriteLine("For help, type \"help\"");
            }
        }


        static void ParseCommand(FileSystem file, string x)
        {
            if (x== "print") // print command uses file.PrintFileSystem to print the system to the console
            {
                file.PrintFileSystem(file.root);
            }
            else if (x=="exit"){System.Environment.Exit(1);} // exit command exits the program
            else if (x=="filecount") // filecount command writes string gathering number of files in system from file.NumberFiles
            {
                System.Console.WriteLine("There are {0} files in the entire file system.", file.NumberFiles(file.root));
            }
            else if (x=="help") // help commmand prints out help menu
            {
                System.Console.WriteLine("Commands:");
                System.Console.WriteLine("print");
                System.Console.WriteLine("exit");
                System.Console.WriteLine("help");
                System.Console.WriteLine("filecount");
                System.Console.WriteLine("mkdir [directory]");
                System.Console.WriteLine("rmdir [directory]");
                System.Console.WriteLine("mkfile [filelocation]");
                System.Console.WriteLine("rmfile [filelocation]");
            }
            else if (x.IndexOf(' ')>0 && file.isValidCommand(x)) // if the command is a parameter dependent commands, use RunParameterCommand function to decide what to do
            {
                RunParameterCommand(file, x);
            }
            else
            {
                System.Console.WriteLine("For help, type \"help\""); // if none of the above cases are executed, remind the user that they can use help to learn the commands
            }
        }


        static void Main(string[] args)
        {
            
            FileSystem file = new FileSystem(); // initialize filesystem variable

            init(file); // initialize to sample filesystem

            System.Console.WriteLine("For help, type \"help\"\n\n");
            while (true)
            {
                System.Console.Write("<COMMAND> ");
                string x = System.Console.ReadLine();
                ParseCommand(file, x);
            }
        }
    }
}
