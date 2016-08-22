using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreateElementsScript : MonoBehaviour {
    public GameObject LaPrefab;
    public GameObject ScPrefab;
    public GameObject B1Prefab;
    public GameObject B2Prefab;
    public GameObject OPrefab;

    public delegate Vector3 ReplicationItem(Vector3 vect);

    public List<ReplicationItem> replications = new List<ReplicationItem>();

    void repCreating()
    {
        replications.Add((Vector3 vect) => (new Vector3(vect.x, vect.y, vect.z)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.y,vect.x - vect.y , vect.z)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.x+vect.y,-vect.x ,vect.z )));
        replications.Add((Vector3 vect) => (new Vector3(vect.y,vect.x,-vect.z)));
        replications.Add((Vector3 vect) => (new Vector3(vect.x - vect.y,-vect.y ,-vect.z)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.x,-vect.x+vect.y ,-vect.z)));

        replications.Add((Vector3 vect) => (new Vector3(vect.x + 2.0f / 3, vect.y + 1.0f / 3, vect.z + 1.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.y + 2.0f / 3, vect.x - vect.y + 1.0f / 3, vect.z + 1.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.x + vect.y + 2.0f / 3, -vect.x + 1.0f / 3, vect.z + 1.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(vect.y + 2.0f / 3, vect.x + 1.0f / 3, -vect.z + 1.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(vect.x - vect.y + 2.0f / 3, -vect.y + 1.0f / 3, -vect.z+ 1.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.x + 2.0f / 3, -vect.x + vect.y + 1.0f / 3, -vect.z + 1.0f / 3)));

        replications.Add((Vector3 vect) => (new Vector3(vect.x + 1.0f / 3, vect.y + 2.0f / 3, vect.z + 2.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.y + 1.0f / 3, vect.x - vect.y + 2.0f / 3, vect.z + 2.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.x + vect.y + 1.0f / 3, -vect.x + 2.0f / 3, vect.z + 2.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(vect.y + 1.0f / 3, vect.x + 2.0f / 3, -vect.z + 2.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(vect.x - vect.y + 1.0f / 3, -vect.y + 2.0f / 3, -vect.z + 2.0f / 3)));
        replications.Add((Vector3 vect) => (new Vector3(-vect.x + 1.0f / 3, -vect.x + vect.y + 2.0f / 3, -vect.z + 2.0f / 3)));
    
    }

    private float a = 9.819f;
    private float b = 9.819f;
    private float c = 7.987f;
    /* Получает на вход вектор с координатами 0..1
     * А возвращет новый вектор (0..a,0..b,0..c)
     */
    void Scale(ref Vector3 vect)
    {
        vect.Set(vect.x * a, vect.y * b, vect.z * c);
    }

    void normalize(ref Vector3 vect)
    {
        vect.x = vect.x < 0 ? vect.x + 1 : vect.x > 1 ? vect.x - 1 : vect.x;
        vect.y = vect.y < 0 ? vect.y + 1 : vect.y > 1 ? vect.y - 1 : vect.y;
        vect.z = vect.z < 0 ? vect.z + 1 : vect.z > 1 ? vect.z - 1 : vect.z;
    }
    private float alpha = 120;
    void transToUSC(ref Vector3 vect)
    {
        float alphaInRad = 2*Mathf.PI/360*alpha;
        vect.x = vect.x + vect.y * Mathf.Cos(alphaInRad);
        vect.y = vect.y * Mathf.Sin(alphaInRad);
    }

    void YZSwap(ref Vector3 vect)
    {
        float k = vect.z;
        vect.z = vect.y;
        vect.y = k;
    }

    void ReplicateItem(GameObject obj,Vector3 locate)
    {
        List<Vector3> list = new List<Vector3>();
        foreach (var rep in replications)
            list.Add(rep(locate));
        foreach (var i in list)
        {
            Vector3 el = i;
            normalize(ref el);
            Scale(ref el);
            transToUSC(ref el);
            YZSwap(ref el);
            GameObject NewObj = (GameObject)Instantiate(obj, el, Quaternion.identity);
            NewObj.transform.parent = this.transform;
        }

    }


	// Use this for initialization
	void Start () {
        repCreating();
        ReplicateItem(LaPrefab, new Vector3(1f / 3, 2f / 3, 2f / 3));
        ReplicateItem(ScPrefab, new Vector3(0.1179f, 1f / 3, 1f / 3));
        ReplicateItem(B1Prefab, new Vector3(0f, 0.5487f, 1f / 2));
        ReplicateItem(B2Prefab, new Vector3(0f, 0f, 1f / 2));
        ReplicateItem(OPrefab, new Vector3(0.1406f, 0.6863f,0.4818f));
        ReplicateItem(OPrefab, new Vector3(0f, 0.412f, 0.5f));
        ReplicateItem(OPrefab, new Vector3(0.140f, 0.1398f, 0.5f));


	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
