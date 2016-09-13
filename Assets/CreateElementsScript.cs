﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Info
{
    public static List<GameObject> bors1;
    public static List<GameObject> bors2;
    public static List<GameObject> scandiums;
    public static List<GameObject> lantans;
    public static List<GameObject> oxygens;
    public static GameObject borders;

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
}

public class CreateElementsScript : MonoBehaviour {

    CreateElementsScript():base()
    {
        Info.bors1 = new List<GameObject>();
        Info.bors2 = new List<GameObject>();
        Info.lantans = new List<GameObject>();
        Info.scandiums = new List<GameObject>();
        Info.oxygens = new List<GameObject>();
    }

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

    List<GameObject> ReplicateItem(GameObject obj,Vector3 locate)
    {
        List<GameObject> thisAtoms = new List<GameObject>();
        List<Vector3> list = new List<Vector3>();
        foreach (var rep in replications)
            list.Add(rep(locate));
        
        list = addAxesElement(list);
        for(int i=1; i <=list.ToArray().Length; i++)
        {
            Vector3 el = list[i-1];
            normalize(ref el);
            Scale(ref el);
            transToUSC(ref el);
            YZSwap(ref el);
            if (HaveElement(el))
                continue;
            GameObject NewObj = (GameObject)Instantiate(obj, el, Quaternion.identity);
            NewObj.name = NewObj.tag +" " + i.ToString();
            NewObj.transform.parent = this.transform;
            thisAtoms.Add(NewObj);
        }
        return thisAtoms;
    }

    private List<Vector3> addAxesElement(List<Vector3> list)
    {
        List<Vector3> nList = new List<Vector3>();
        foreach (var i in list)
        {
            nList.Add(i);
            if (i.x == 0)
                nList.Add(new Vector3(1, i.y, i.z));
            if (i.y == 0)
                nList.Add(new Vector3(i.x, 1, i.z));
            if (i.z == 0)
                nList.Add(new Vector3(i.x, i.y, 1));
            if (i.x == 0 && i.y == 0)
                nList.Add(new Vector3(1, 1, i.z));
            if (i.x == 0 && i.z == 0)
                nList.Add(new Vector3(1, i.y, 1));
            if (i.y == 0 && i.z == 0)
                nList.Add(new Vector3(i.x, 1, 1));
            if (i.x == 1)
                nList.Add(new Vector3(0, i.y, i.z));
            if (i.y == 1)
                nList.Add(new Vector3(i.x, 0, i.z));
            if (i.z == 1)
                nList.Add(new Vector3(i.x, i.y, 0));
            if (i.x == 1 && i.y == 1)
                nList.Add(new Vector3(0, 0, i.z));
            if (i.x == 1 && i.z == 1)
                nList.Add(new Vector3(0, i.y, 0));
            if (i.y == 1 && i.z == 1)
                nList.Add(new Vector3(i.x, 0, 0));
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
    // Use this for initialization
    void Start () {
        repCreating();
        Info.lantans = ReplicateItem(LaPrefab, new Vector3(1f / 3, 2f / 3, 2f / 3));
        Info.scandiums = ReplicateItem(ScPrefab, new Vector3(0.1179f, 1f / 3, 1f / 3));
        Info.bors1 = ReplicateItem(B1Prefab, new Vector3(0f, 0.5487f, 1f / 2));
        Info.bors2 = ReplicateItem(B2Prefab, new Vector3(0f, 0f, 1f / 2));
        Info.oxygens =  ReplicateItem(OPrefab, new Vector3(0.1406f, 0.6863f,0.4818f));
        Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(0f, 0.412f, 0.5f)));
        Info.oxygens.AddRange(ReplicateItem(OPrefab, new Vector3(0.140f, 0.1398f, 0.5f)));
        Info.borders = GameObject.FindGameObjectWithTag("Borders");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
