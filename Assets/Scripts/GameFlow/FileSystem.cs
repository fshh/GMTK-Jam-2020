using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileSystem : MonoBehaviour
{
    public static string ROOT_FILE_NAME = "Root";
    
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

    // Start is called before the first frame update
    void Start()
    {
        RefreshStructure();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Building the tree

    //Re-builds the tree from the current StreamingAssets folder
    private void RefreshStructure()
    {
        root = new FileNode();
        root.path = Application.streamingAssetsPath + "/" + ROOT_FILE_NAME + "/";
        root.type = FileType.Folder;

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

                newFile.print();

                filesInDirectory.Add(newFile);
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


            newDir.print();

            filesInDirectory.Add(newDir);
        }

        return filesInDirectory;
    }

    #endregion
}
