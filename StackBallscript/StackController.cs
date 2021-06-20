using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField]
    private StackPartController[] stackPartControllers;
    
    public void ShatterAllParts()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
            FindObjectOfType<Player>().IncreaseBrokenStacks();
        }

        foreach(var i in stackPartControllers)
        {
            i.Shatter();
        }
        StartCoroutine(RemoveParts());
    }

    IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
