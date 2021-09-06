using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    private GameObject parentOb;
    private GhostChild[] ghosties;
    [Range(1,100)]
    public int numGhosties, currGhosty;
    [Range(0.0001f,5f)]
    public float waitTime;
    public GameObject ghostPrefab;
    // Start is called before the first frame update
    void Start()
    {
        parentOb = new GameObject();
        parentOb.AddComponent<RectTransform>();
        parentOb.GetComponent<RectTransform>().SetParent(transform.parent);
        GetComponent<RectTransform>().SetParent(parentOb.transform);
        parentOb.transform.localScale = Vector3.one;
        parentOb.name = "Cursor Ghost Parent (Created at runtime)";
        transform.localScale = Vector3.one;

        ghosties = new GhostChild[numGhosties];
        for (int i = 0; i < numGhosties; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab, parentOb.transform);
            ghosties[i] = ghost.GetComponent<GhostChild>();
        }

        StartCoroutine(ManageGhosts());
    }

    private IEnumerator ManageGhosts()
    {
        while (true)
        {
            ghosties[currGhosty].transform.SetAsLastSibling();
            transform.SetAsLastSibling();
            ghosties[currGhosty].SetPosition(GetComponent<RectTransform>());
            yield return new WaitForSeconds(waitTime);
            currGhosty = (currGhosty + 1) % numGhosties;
        }
    }
}
