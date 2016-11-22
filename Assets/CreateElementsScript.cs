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

    /* Получает на вход вектор с координатами 0..1
    * А возвращет новый вектор (0..a,0..b,0..c)
    */
    public static void Scale(ref Vector3 vect)
    {
        vect.Set(vect.x * Info.a, vect.y * Info.b, vect.z * Info.c);
    }

    public static void normalize(ref Vector3 vect)
    {
        vect.x = vect.x < 0 ? vect.x + 1 : vect.x > 1 ? vect.x - 1 : vect.x;
        vect.y = vect.y < 0 ? vect.y + 1 : vect.y > 1 ? vect.y - 1 : vect.y;
        vect.z = vect.z < 0 ? vect.z + 1 : vect.z > 1 ? vect.z - 1 : vect.z;
    }

    public static float alpha = 120;

    public static void transToUSC(ref Vector3 vect)
    {
        float alphaInRad = 2 * Mathf.PI / 360 * alpha;
        vect.x = vect.x + vect.y * Mathf.Cos(alphaInRad);
        vect.y = vect.y * Mathf.Sin(alphaInRad);
    }

    public static void YZSwap(ref Vector3 vect)
    {
        float k = vect.z;
        vect.z = vect.y;
        vect.y = k;
    }

    public static void transToOrt(ref Vector3 vect)
    {
        float alphaInRad = 2 * Mathf.PI / 360 * alpha;
        vect.y = vect.y / Mathf.Sin(alphaInRad);
        vect.x = vect.x - vect.y * Mathf.Cos(alphaInRad);

    }

    public static void deScale(ref Vector3 vect)
    {
        vect.Set(vect.x / Info.a, vect.y / Info.b, vect.z / Info.c);
    }
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

   


    List<GameObject> ReplicateItem(GameObject obj,Vector3 locate, bool addHiden)
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
            Info.normalize(ref el);
            Info.Scale(ref el);
            Info.transToUSC(ref el);
            Info.YZSwap(ref el);
            if (HaveElement(el))
                continue;
            if (addHiden == false)
            {
                GameObject NewObj = (GameObject)Instantiate(obj, el, Quaternion.identity);
                NewObj.name = "";
                NewObj.name = NewObj.tag + " " + listInd[i - 1].ToString();
                NewObj.transform.parent = this.transform;
                thisAtoms.Add(NewObj);
            }
            else
            {
                for (float x = -1; x <= 1; x++)
                    for (float y = -1; y <= 1; y++)
                        for (float z = -1; z <= 1; z++)
                        {
                            Vector3 add = new Vector3(x, y, z);
                            Info.Scale(ref add);
                            Info.transToUSC(ref add);
                            Info.YZSwap(ref add);
                            add += el;
                            GameObject NewObj = (GameObject)Instantiate(obj, add, Quaternion.identity);
                            NewObj.name = "";
                            Vector3 addSc = new Vector3(add.x,add.y,add.z);
                            Info.YZSwap(ref addSc);
                            Info.transToOrt(ref addSc);
                            Info.deScale(ref addSc);
                            if (addSc.x >= 0 && addSc.x <= 1 && addSc.y >= 0 
                                && addSc.y <= 1 && addSc.z >= 0 && addSc.z <= 1)
                                NewObj.SetActive(true);
                            else
                                NewObj.SetActive(false);
                            NewObj.name = NewObj.tag + " " + listInd[i - 1].ToString();
                            NewObj.transform.parent = this.transform;
                            thisAtoms.Add(NewObj);
                 
                        }
            }
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

    public void setCenter(Vector3 vec)
    {
        hantic.transform.localPosition = vec;
        elements.transform.localPosition = -vec;
        borders.transform.localPosition = -vec;
    }

    public void setCenter()
    {
        Vector3 vec = new Vector3(0.5f, 0.5f, 0.5f);
        Info.Scale(ref vec);
        Info.transToUSC(ref vec);
        Info.YZSwap(ref vec);
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
        bool f = true;
        LaPrefab.name = la.elementName;
        ScPrefab.name = sc.elementName;
        Info.lantans = ReplicateItem(LaPrefab, new Vector3(la.x, la.y,la.z),f);
        Info.scandiums = ReplicateItem(ScPrefab, new Vector3(sc.x, sc.y, sc.z),f);
        Info.bors1 = ReplicateItem(B1Prefab, new Vector3(b1.x, b1.y, b1.z),f);
        Info.bors2 = ReplicateItem(B2Prefab, new Vector3(b2.x, b2.y, b2.z),f);
        Info.oxygens = ReplicateItem(OPrefab, new Vector3(o[0].x, o[0].y, o[0].z),f);
        for(int i = 1; i < o.Count; i++)
        {
            Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(o[i].x, o[i].y, o[i].z),f));
        }
        setCenter();
    }
}
