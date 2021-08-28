using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileExplorer : MonoBehaviour
{
    public GameObject buttonHolder;
    public GameObject fileButton; //must have a fileButton script attached
    public ApplicationSO TextApp;

    private List<GameObject> buttons;

    private Stack<FileTicket> previousOpenedFolders;
    private FileTicket currentlyOpen;

    private void Awake()
    {
        previousOpenedFolders = new Stack<FileTicket>();
        buttons = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentlyOpen = FileSystem.instance.GetRootDirectory();
        OpenFolder(FileSystem.instance.GetRootDirectory());
    }

    public void clickedFile(FileTicket ticket)
    {
        FileSystem.FileType type = FileSystem.instance.TicketToType(ticket);

        if(type == FileSystem.FileType.Folder)
        {
            OpenFolder(ticket);
        } else if (type == FileSystem.FileType.Text) 
        {
            Window textWindow = TextApp.OpenWindow(FileSystem.instance.GetName(ticket));
            //TODO: make the name of the window be FileSystem.instance.GetName(ticket); Ask andrew how to accomplish this
            textWindow.content.GetComponent<TextApp>().TextContent = "Temporary, Ezra is working on a fix."; //FileSystem.instance.GetName(ticket);
        } else 
        {
            //TODO unstub
            Debug.Log("Wasn't a folder, don't know what to do with this yet (Ezra)");
        }
    }

    public void Back()
    {
        if(FileSystem.instance.IsRoot(currentlyOpen))
        {
            //if we're in the root folder, return
            return;

        } else
        {
            OpenFolder(previousOpenedFolders.Pop(), true);
        }
        
    }
    
    public void OpenFolder(FileTicket ticket, bool goingBack = false)
    {
        if (!goingBack)
        {
            previousOpenedFolders.Push(currentlyOpen);
        }
        currentlyOpen = ticket;

        FileTicket[] newFiles = FileSystem.instance.GetChildTickets(ticket);

        foreach(GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();

        foreach (FileTicket oneTicket in newFiles)
        {
            FileButton currButton = Instantiate(fileButton, buttonHolder.transform).GetComponent<FileButton>();
            currButton.ticket = oneTicket;
            currButton.parentExplorer = this;
            currButton.SetText(FileSystem.instance.GetName(oneTicket));

            buttons.Add(currButton.gameObject);
        }


    }
}
