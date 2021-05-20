using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class FileTicket
{
    public int id;
}

public class FileSystem : MonoBehaviour
{
    public static string ROOT_FILE_NAME = "Root";

    private Dictionary<FileTicket, FileNode> ticketToNode;
    private Dictionary<FileNode, FileTicket> nodeToTicket;

    //this is a tree that stores the file structure
    private FileNode root; //TODO write some good setters and getters

    public enum FileType { Folder, Text };

    private class FileNode
    {
        public string path;
        public string name;
        public FileType type;
        public List<FileNode> childNodes;

        public void print()
        {
            string toPrint = "";

            toPrint += ("Printing " + name + " of type " + System.Enum.GetName(typeof(FileType), type) + "\n");
            toPrint += ("Path: " + path + "\n");

            if(childNodes != null)
            {
                toPrint += ("Children: " + "\n");

                foreach(FileNode child in childNodes)
                {
                    toPrint += (child.name + "\n");
                }
            }

            Debug.Log(toPrint);
        }
    }

    public FileType TicketToType(FileTicket ticket)
    {
        if (ticketToNode.ContainsKey(ticket))
        {
            return ticketToNode[ticket].type;
        } else
        {
            Debug.Log("Couldn't find that ticket in the dictionary");
        }

        return FileType.Text; //Default return value, shouldn't come to this
    }

    public string GetName(FileTicket ticket)
    {
        return ticketToNode[ticket].name;
    }

    public bool IsRoot(FileTicket ticket)
    {
        return ticketToNode[ticket] == root;
    }

    public FileTicket GetRootDirectory()
    {
        return nodeToTicket[root];
    }

    //returns tickets to all children of the node belonging to the ticket given
    public FileTicket[] GetChildTickets(FileTicket ticket)
    {
        List<FileTicket> toReturn = new List<FileTicket>();

        foreach(FileNode node in ticketToNode[ticket].childNodes)
        {
            toReturn.Add(nodeToTicket[node]);
        }

        return toReturn.ToArray();
    }

    //TODO make more robust singleton pattern that checks it's the only one
    public static FileSystem instance;

    private void Awake()
    {
        instance = this;
        RefreshStructure();
    }

    #region Building the tree

    private void AddNodeToDictionaries(FileNode toAdd)
    {
        FileTicket newTicket = new FileTicket();
        newTicket.id = ticketToNode.Count; //should be the same as the count for the other dictionary
        ticketToNode.Add(newTicket, toAdd);
        nodeToTicket.Add(toAdd, newTicket);
    }

    private void resetDictionaries()
    {
        ticketToNode = new Dictionary<FileTicket, FileNode>();
        ticketToNode.Clear();
        nodeToTicket = new Dictionary<FileNode, FileTicket>();
        nodeToTicket.Clear();
    }

    //Re-builds the tree from the current StreamingAssets folder
    private void RefreshStructure()
    {
        //TODO figure out how to do this without invalidating outstanding tickets in the desktop
        resetDictionaries();

        root = new FileNode();
        root.path = Application.streamingAssetsPath + "/" + ROOT_FILE_NAME + "/";
        root.type = FileType.Folder;
        AddNodeToDictionaries(root);

        DirectoryInfo info = new DirectoryInfo(root.path);
        root.childNodes = BuildDirectory(info, root.path);
    }

    //For recursively traversing the tree
    private List<FileNode> BuildDirectory(DirectoryInfo info, string path)
    {
        List<FileNode> filesInDirectory = new List<FileNode>();

        // Copy each file into it's new directory.
        // Note: only works on text files currently, may need to expand
        foreach (FileInfo file in info.GetFiles())
        {
            if (file.Extension == ".txt")
            {
                FileNode newFile = new FileNode();
                newFile.path = path + file.Name + "/";
                newFile.type = FileType.Text;
                newFile.name = file.Name;
                newFile.childNodes = null;

                filesInDirectory.Add(newFile);
                AddNodeToDictionaries(newFile);
            }
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo SubDir in info.GetDirectories())
        {
            FileNode newDir = new FileNode();
            newDir.path = path + SubDir.Name + "/";
            newDir.type = FileType.Folder;
            newDir.name = SubDir.Name;
            newDir.childNodes = BuildDirectory(SubDir, newDir.path);

            filesInDirectory.Add(newDir); 
            AddNodeToDictionaries(newDir);
        }

        return filesInDirectory;
    }

    #endregion
}
