using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public List<GameObject> ObjectsToActivate; 
    public List<MonoBehaviour> ScriptsToActivate; 

    void OnTap()
    {
        foreach (GameObject obj in ObjectsToActivate)
            obj.SetActive(true);
        foreach (MonoBehaviour obj in ScriptsToActivate)
            obj.enabled = true;

        enabled = false;
    }

    void Update()
    {
        if (Input.anyKeyDown)
            OnTap();
    }
}
