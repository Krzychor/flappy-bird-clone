using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectPool : MonoBehaviour
{
    [Min(0)]
    public int startingSize = 0;
    public GameObject PooledObject;

    List<GameObject> objects = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < startingSize; i++)
            objects.Add(PooledObject);
    }
    
    public void ReturnToPool(GameObject G)
    {
        G.SetActive(false);
        objects.Add(G);
    }

    public GameObject GetFromPool()
    {
        if (objects.Count == 0)
            return Instantiate(PooledObject);
        GameObject G = objects[objects.Count-1];
        G.SetActive(true);
        objects.RemoveAt(objects.Count - 1);
        return G;
    }
}
