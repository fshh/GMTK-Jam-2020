using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileButton : MonoBehaviour
{
    public FileExplorer parentExplorer;
    public FileTicket ticket;
    public TextMeshProUGUI fileText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string newText)
    {
        fileText.text = newText;
    }

    public void askExplorer()
    {
        parentExplorer.clickedFile(ticket);
    }
}
