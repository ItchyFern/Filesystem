using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Node
{
    public string directory {get;set;} = null;
    public List<string> file {get;set;} = new List<string>();
    public Node leftMostChild {get;set;} = null;
    public Node rightSibling {get;set;} = null;
}

public class FileSystem
{
    public Node root;
    // Creates a file system with a root directory
    public FileSystem( ) 
    {
        root = new Node (); // create root node
        root.directory = "/"; // set root nodes directory to "/"
    }

    // checks to see if string is a "valid" command
    public bool isValidCommand(string address)
    {
        if (Regex.IsMatch (address, @"^\w+\s{1}\/.*")) // regex to check if there is a word, then a space, then a / with anything after. ex. mkdir /home/seth/
        {
            return true;
        }
        return false;
    }

    private Node getFromFS(string directory)
    {
        List<string> dirs = getDirList(directory);
        Node navnode = root;
        foreach (string d in dirs)
        {
            if (getDirNode(navnode, d)!=null) // if directory exists
            {
                navnode = getDirNode(navnode, d); // set navnode to directory node
            }
            else 
            {
                return null;
            }
            if (d != directory) // if its not the last item in the list
            {
                if (navnode.leftMostChild==null){return null;} // check to see if a child exists, if it does not, return false
                navnode = navnode.leftMostChild; // switch to left child node
            }
            
        }
        return navnode;
    }

    //get directory node in current directory
    private Node getDirNode(Node nav, string directory)
    {
        while (nav!=null)// while navigation node exists
        {
            if (nav.directory == directory) // check if the navigation nodes directory is equal to the directory being searched 
            {
                return nav; // if it is, return the node
            }
            nav = nav.rightSibling; // cycle throught the siblings
        }
        return null; // if the directory being searched is not found in any of the siblings, return null
    }

    private string getDir(string address) // get full dir name from address
    {
        Match match = Regex.Match(address, @"^(\/(?:[\w\d,-]*\/)*?)([\w\d,-]+\.{0,1}[\w\d]*){0,1}$"); // regex for breaking up the string into directory and filename
        return match.Groups[1].Value; // return directory part of regex
    }

    private string getFile(string address) // get file name from address
    {
        Match match = Regex.Match(address, @"^(\/(?:[\w\d,-]*\/)*?)([\w\d,-]+\.{0,1}[\w\d]*){0,1}$"); // regex for breaking up the string into directory and filename
        return  match.Groups[2].Value; // file part of regex
    }

    private List<string> getDirList(string directory)
    {
        Match match = Regex.Match(directory, @"([\w\d,-]*\/)"); // split directory name into individual segments (ex. /home/seth/ = [/,home/,seth/])

        List<string> dirs = new List<string>(); // list to hold directory names for each step (ex. /home/seth/ = [/, /home/, /home/seth/])
        string currentdir = ""; // holds cwd name

        while (match.Success) // if there is another segment in the directory name
        {
            currentdir += match.Value; // add that segment to the cwd name
            dirs.Add(currentdir); // add the cwd to the dirs list
            match = match.NextMatch(); // move on to next segment in directory name
        }
        return dirs;
    }


    // Adds a file at the given address
    // Returns false if the file already exists or the path is undefined; true otherwise
    public bool AddFile(string address) 
    {
        //break up address into directory and file names using regex
        string directory = getDir(address);
        string file = getFile(address); 

        Node navnode = getFromFS(directory); // get node from filesystem, if it doesn't exist, it returns as null

        if (navnode != null)
        {

            foreach (string f in navnode.file) // check each file in the directory node file list
            {
                if (f == file){return false;} // if file already exists, return false
            }
            navnode.file.Add(file); // add file to the directory file list
            return true; // successful returns true
        }
        return false; // if the node searched for comes back null (if it doesn't exist), return false
        
    }


    // Removes the file at the given address
    // Returns false if the file is not found or the path is undefined; true otherwise
    public bool RemoveFile(string address) 
    {
        //break up address into directory and file names using regex
        string directory = getDir(address);
        string file = getFile(address);

        Node navnode = getFromFS(directory);// get node from filesystem, if it doesn't exist, it returns as null

        if (navnode != null)
        {
            
            foreach (string f in navnode.file) // check each file in the directory node file list
            {
                if (f == file){navnode.file.Remove(f);return true;} // if file is found in directory file list, delete file and return true
            }
        }
        return false; //  if the directory that user file is attempting to delete does not exist or the file in the directory doesn't exist, return false
    }


    // Adds a directory at the given address
    // Returns false if the directory already exists or the path is undefined; true otherwise
    public bool AddDirectory(string address) 
    {
        if (!address.EndsWith("/")){address+="/";} // if the directory doesn't have a / at the end add it
        string directory = getDir(address); // get directory name from address
        
        List<string> dirs = getDirList(directory); // get directories as a list 

        Node navnode = root; // set the navigation node to the root of the tree
        bool createDirFlag = false; //check to see if dir was created

        foreach (string d in dirs)
        {
            if (getDirNode(navnode, d)!=null) // if directory exists
            {
                navnode = getDirNode(navnode, d); // set navnode to directory node
            }
            else // if directory doesn't exist, make it
            { 
                if (navnode.directory == null) // if the node is a newly created node (like from below)
                {
                    navnode.directory = d; // just set that node to be the directory node
                    createDirFlag = true; // if a dir was created, flag = true
                }
                else
                {
                    while (navnode.rightSibling!=null){navnode = navnode.rightSibling;} //cycle through list to get to last node
                    navnode.rightSibling = new Node(); // create directory node
                    navnode.rightSibling.directory = d; // set directory nodes directory to the cwd
                    navnode = navnode.rightSibling; // set nav node to the directory node just made
                    createDirFlag = true; // if a dir was created, flag = true
                }
                
            }
            if (d != directory) // if its not the last item in the list
            {
                if (navnode.leftMostChild==null){navnode.leftMostChild = new Node();} // check to see if a child exists, if it does not, create it
                navnode = navnode.leftMostChild; // switch to left child node
            }
        }
        if (createDirFlag){return true;}
        return false;
    }





    // Removes the directory (and its subdirectories) at the given address
    // Returns false if the directory is not found or the path is undefined; true otherwise
    public bool RemoveDirectory(string address) 
    {
        if (!address.EndsWith("/")){address+="/";} // if the directory doesn't have a / at the end add it
        string directory = getDir(address); // get directory name from address

        List<string> dirs = getDirList(directory); // get directories as a list
        dirs.RemoveAt(0); //remove root directory from search


        Node navnode = root; // set the navigation node to the root of the tree

        foreach (string d in dirs) // for each directory in the list of directories (/,/home/,/home/seth/, etc.)
        {
            if (d != directory) // if its not the last item in the list
            {
                if (getDirNode(navnode.leftMostChild, d)!=null) // if directory exists as a subdirectory
                {
                    navnode = getDirNode(navnode.leftMostChild, d); // set navnode to directory node
                }
                else // if directory doesn't exist, break out
                {
                    return false;                
                }
            }
            else // if it is the last item in the list (deepest child directory)
            {
                if (getDirNode(navnode.leftMostChild, d)!=null)
                {
                    if (navnode.leftMostChild.directory == d)
                    {
                        navnode.leftMostChild = navnode.leftMostChild.rightSibling;
                        return true;
                    }
                    else
                    {
                        navnode = navnode.leftMostChild;
                        while (navnode.rightSibling!=null)
                        {
                            if (navnode.rightSibling.directory==d)
                            {
                                navnode.rightSibling = navnode.rightSibling.rightSibling;
                                return true;
                            }
                            navnode = navnode.rightSibling;
                        }
                    }
                }
                
            }
        }
        return false;
    }

    // Returns the number of files in the file system
    public int NumberFiles(Node nav, int filenum=0) 
    {
        filenum += nav.file.Count;
        if (nav.leftMostChild!=null)
        {
            return NumberFiles(nav.leftMostChild, filenum);
        }
        
        if (nav.rightSibling!=null)
        {
            return NumberFiles(nav.rightSibling, filenum);
        }
        return filenum;
    }

    // Prints the directories in a pre-order fashion along with their files
    public void PrintFileSystem(Node nav, string indent="") 
    {
        System.Console.WriteLine(indent+nav.directory);
        if (nav.leftMostChild!=null)
        {
            PrintFileSystem(nav.leftMostChild, indent+"  ");
        }
        foreach (string f in nav.file)
        {
            System.Console.WriteLine(indent+"  "+nav.directory+f);
        }
        if (nav.rightSibling!=null)
        {
            PrintFileSystem(nav.rightSibling, indent);
        }
        
    }

}