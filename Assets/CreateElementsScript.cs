using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;

public static class Info
{
    public static StreamWriter logFile = new 
        System.IO.StreamWriter(@"log.txt",false);
    public static List<GameObject> bors1;
    public static List<GameObject> bors2;
    public static List<GameObject> scandiums;
    public static List<GameObject> lantans;
    public static List<GameObject> oxygens;
    public static GameObject borders;
    public static float a = 9.819f;
    public static float b = 9.819f;
    public static float c = 7.987f;

    public static void DrawLine(GameObject obj, Vector3[] points)
    {
        LineRenderer lr;
        if (obj.GetComponent<LineRenderer>() == null)
        {
            lr = obj.AddComponent<LineRenderer>();
        } else
        {
            lr = obj.GetComponent<LineRenderer>();
            lr.enabled = true;
        }
        lr.material = Info.borders.GetComponent<Material>();
        lr.material.color = Color.black;
        lr.SetWidth(0.05F, 0.05F);
        lr.SetVertexCount(points.Length);
        lr.SetPositions(points);
    }

    public static float distance(Vector3 v1, Vector3 v2)
    {
        float d = Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x)
            + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));

        return d;
    }
    public static CreateElementsScript scr = null;
}

public class CreateElementsScript : MonoBehaviour {

    CreateElementsScript():base()
    {
        Info.bors1 = new List<GameObject>();
        Info.bors2 = new List<GameObject>();
        Info.lantans = new List<GameObject>();
        Info.scandiums = new List<GameObject>();
        Info.oxygens = new List<GameObject>();
        compounds = new List<Compound>();
    }

    public List<Compound> compounds;
    public GameObject LaPrefab;
    public GameObject NdPrefab;
    public GameObject ScPrefab;
    public GameObject B1Prefab;
    public GameObject B2Prefab;
    public GameObject OPrefab;
    public GameObject hantic;
    public GameObject elements;
    public GameObject borders;
    public Dropdown dr;

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

   
    /* Получает на вход вектор с координатами 0..1
     * А возвращет новый вектор (0..a,0..b,0..c)
     */
    void Scale(ref Vector3 vect)
    {
        vect.Set(vect.x * Info.a, vect.y * Info.b, vect.z * Info.c);
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

    List<GameObject> ReplicateItem(GameObject obj,Vector3 locate)
    {
        List<GameObject> thisAtoms = new List<GameObject>();
        List<Vector3> list = new List<Vector3>();
        List<int> listInd = new List<int>();
        foreach (var rep in replications)
            list.Add(rep(locate));
        for (int i = 1; i <= replications.Count; i++)
            listInd.Add(i);
        
        list = addAxesElement(list, listInd);
        Debug.Log(list.ToArray().Length);
        for(int i=1; i <=list.Count; i++)
        {
            Vector3 el = list[i-1];
            normalize(ref el);
            Scale(ref el);
            transToUSC(ref el);
            YZSwap(ref el);
            if (HaveElement(el))
                continue;
            GameObject NewObj = (GameObject)Instantiate(obj, el, Quaternion.identity);
            NewObj.name = "";
            NewObj.name = NewObj.tag +" " + listInd[i-1].ToString();
            NewObj.transform.parent = this.transform;
            thisAtoms.Add(NewObj);
        }
        return thisAtoms;
    }

    private List<Vector3> addAxesElement(List<Vector3> list, List<int> ind)
    {
        List<Vector3> nList = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
            nList.Add(list[i]);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].x == 0)
            {
                nList.Add(new Vector3(1, list[i].y, list[i].z));
                ind.Add(i+1);
            }
            if (list[i].y == 0)
            {
                nList.Add(new Vector3(list[i].x, 1, list[i].z));
                ind.Add(i+1);
            }
            if (list[i].z == 0)
            {
                nList.Add(new Vector3(list[i].x, list[i].y, 1));
                ind.Add(i+1);
            }
            if (list[i].x == 0 && list[i].y == 0)
            {
                nList.Add(new Vector3(1, 1, list[i].z));
                ind.Add(i+1);
            }
            if (list[i].x == 0 && list[i].z == 0)
            {
                nList.Add(new Vector3(1, list[i].y, 1));
                ind.Add(i+1);
            }
            if (list[i].y == 0 && list[i].z == 0)
            {
                nList.Add(new Vector3(list[i].x, 1, 1));
                ind.Add(i+1);
            }
            if (list[i].x == 1)
            {
                nList.Add(new Vector3(0, list[i].y, list[i].z));
                ind.Add(i+1);
            }
            if (list[i].y == 1)
            {
                nList.Add(new Vector3(list[i].x, 0, list[i].z));
                ind.Add(i+1);
            }
            if (list[i].z == 1)
            {
                nList.Add(new Vector3(list[i].x, list[i].y, 0));
                ind.Add(i+1);
            }
            if (list[i].x == 1 && list[i].y == 1)
            {
                nList.Add(new Vector3(0, 0, list[i].z));
                ind.Add(i+1);
            }
            if (list[i].x == 1 && list[i].z == 1)
            {
                nList.Add(new Vector3(0, list[i].y, 0));
                ind.Add(i+1);
            }
            if (list[i].y == 1 && list[i].z == 1)
            {
                nList.Add(new Vector3(list[i].x, 0, 0));
                ind.Add(i+1);
            }
            if (list[i].y == 1 && list[i].z == 1)
            {
                nList.Add(new Vector3(list[i].x, 0, 0));
                ind.Add(i+1);
            }
            if (list[i].x == 0 && list[i].y == 0 && list[i].z == 0)
            {
                nList.Add(new Vector3(1, 1, 1));
                ind.Add(i + 1);
            }
            if (list[i].x == 1 && list[i].y == 1 && list[i].z == 1)
            {
                nList.Add(new Vector3(0, 0, 0));
                ind.Add(i + 1);
            }
        }
        return nList;
    }

    private bool HaveElement(Vector3 el)
    {
        int count = this.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            Transform tr = this.transform.GetChild(j);
            if (Math.Abs(tr.localPosition.x - el.x) < 0.05
                && Math.Abs(tr.localPosition.y - el.y) < 0.05 
                && Math.Abs(tr.localPosition.z - el.z) < 0.05 )
                return true;
        }
        return false;
    }

    public void CreateLantans()
    {
        var children = new List<GameObject>();
        foreach (Transform child in elements.transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
        Info.a = 9.819f;
        Info.b = 9.819f;
        Info.c = 7.987f;
        Info.lantans.Clear();
        Info.scandiums.Clear();
        Info.bors1.Clear();
        Info.bors2.Clear();
        Info.oxygens.Clear();
        Info.lantans = ReplicateItem(LaPrefab, new Vector3(1f / 3, 2f / 3, 2f / 3));
        Info.scandiums = ReplicateItem(ScPrefab, new Vector3(0.1179f, 1f / 3, 1f / 3));
        Info.bors1 = ReplicateItem(B1Prefab, new Vector3(0f, 0.5487f, 1f / 2));
        Info.bors2 = ReplicateItem(B2Prefab, new Vector3(0f, 0f, 1f / 2));
        Info.oxygens = ReplicateItem(OPrefab, new Vector3(0.1406f, 0.6863f, 0.4818f));
        Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(0f, 0.412f, 0.5f)));
        Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(0.140f, 0.1398f, 0.5f)));
        setCenter(); 
    }

    public void CreateNeodims()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
        Info.a = 9.76333f;
        Info.b = 9.76333f;
        Info.c = 7.91922f;
        Info.lantans.Clear();
        Debug.Log("lc =" + Info.lantans.Count);
        Info.scandiums.Clear();
        Info.bors1.Clear();
        Info.bors2.Clear();
        Info.oxygens.Clear();
        Debug.Log(this.transform.childCount);
        Info.lantans = ReplicateItem(NdPrefab, new Vector3(0f, 0f, 0f));
        Debug.Log("lc =" + Info.lantans.Count);
        Info.scandiums = ReplicateItem(ScPrefab, new Vector3(0.54572f, 0f, 0f));
        Info.bors1 = ReplicateItem(B1Prefab, new Vector3(0f, 0f, 1f / 2));
        Info.bors2 = ReplicateItem(B2Prefab, new Vector3(0.44682f, 0f, 1f / 2));
        Info.oxygens = ReplicateItem(OPrefab, new Vector3(0.587313f, 0f, 0.5f));
        Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(0.45796f, 0.14626f, 0.52047f)));
        Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(0.140f, 0f, 0.5f)));
    }

    public void setCenter(Vector3 vec)
    {
        hantic.transform.localPosition = vec;
        elements.transform.localPosition = -vec;
        borders.transform.localPosition = -vec;
    }

    public void setCenter()
    {
        Vector3 vec = new Vector3(0.5f, 0.5f, 0.5f);
        Scale(ref vec);
        transToUSC(ref vec);
        YZSwap(ref vec);
        hantic.transform.localPosition = vec;
        elements.transform.localPosition = -vec;
        borders.transform.localPosition = -vec;
    }

    // Use this for initialization
    void Start ()
    {
        Info.scr = this;
        repCreating();
        Info.borders = GameObject.FindGameObjectWithTag("Borders");
        FindCompounds();
        CreateCompound(0);
        Info.logFile.AutoFlush = true;
    }

    private void FindCompounds()
    {
        XmlSerializer ser = new XmlSerializer(typeof(Compound));
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory()+@"\Elements\", "*.el");
        Debug.LogWarning(files.Length);
        foreach (var fileName in files)
        {
            using (Stream s = File.OpenRead(fileName))
            {
                Compound x = (Compound)ser.Deserialize(s);
                compounds.Add(x);
            }
        }
        dr = (Dropdown)FindObjectOfType(typeof(Dropdown));
        List<string> options = new List<string>();
        foreach (var i in compounds)
            options.Add(i.name);
        dr.AddOptions(options);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    internal void CreateCompound(int val)
    {
        var children = new List<GameObject>();
        foreach (Transform child in elements.transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
        Compound cur = compounds[val];
        Info.a = cur.a;
        Info.b = cur.b;
        Info.c = cur.c;
        Info.lantans.Clear();
        Info.scandiums.Clear();
        Info.bors1.Clear();
        Info.bors2.Clear();
        Info.oxygens.Clear();

        Element la = cur.elList.Find((Element i) => i.type == elementType.Lantanoid);
        Element sc = cur.elList.Find((Element i) => i.type == elementType.Scandium);
        Element b1 = cur.elList.Find((Element i) => i.type == elementType.Bor1);
        Element b2 = cur.elList.Find((Element i) => i.type == elementType.Bor2);
        List<Element> o = cur.elList.FindAll((Element i) => i.type == elementType.Oxygen);
        LaPrefab.name = la.elementName;
        ScPrefab.name = sc.elementName;
        Info.lantans = ReplicateItem(LaPrefab, new Vector3(la.x, la.y,la.z));
        Info.scandiums = ReplicateItem(ScPrefab, new Vector3(sc.x, sc.y, sc.z));
        Info.bors1 = ReplicateItem(B1Prefab, new Vector3(b1.x, b1.y, b1.z));
        Info.bors2 = ReplicateItem(B2Prefab, new Vector3(b2.x, b2.y, b2.z));
        Info.oxygens = ReplicateItem(OPrefab, new Vector3(o[0].x, o[0].y, o[0].z));
        for(int i = 1; i < o.Count; i++)
        {
            Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(o[i].x, o[i].y, o[i].z)));
        }
        setCenter();
    }
}
