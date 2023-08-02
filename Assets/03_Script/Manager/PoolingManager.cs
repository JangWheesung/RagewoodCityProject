using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;

    [SerializeField] private GameObject[] obj;

    private Dictionary<string, int> pools = new Dictionary<string, int>();

    private void Awake()
    {
        instance = this;

        GameObject beenObj = new GameObject("PoolingData");
        Destroy(beenObj);

        for (int i = 0; i < obj.Length; i++)
        {
            pools.Add(obj[i].name, i);
            Instantiate(beenObj, transform);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Pop("Police", Vector2.zero);
    }

    public void Push(GameObject obj)
    {
        if (!pools.ContainsKey(obj.name)) return;

        obj.transform.SetParent(transform.GetChild(pools[obj.name]));
        obj.SetActive(false);
    }

    public void Pop(string name, Vector3 point)
    {
        if (!pools.ContainsKey(name)) return;

        Transform poolTransform = transform.GetChild(pools[name]);
        GameObject poolObj;

        if (poolTransform.childCount > 0)
        {
            poolObj = poolTransform.GetChild(0).gameObject;
            poolObj.SetActive(true);
            poolObj.transform.SetParent(null);
        }
        else
        {
            poolObj = Instantiate(obj[pools[name]]);
        }
        poolObj.transform.position = point;
        poolObj.name = name;
    }
}
