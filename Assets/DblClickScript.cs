using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DblClickScript : MonoBehaviour
{

    float doubleClickStart = -100;

    private Text text;

    void Start()
    {
        text = GameObject.Find("Canvas/ElInfo").GetComponent<Text>();
        text.text = "";
    }

    void YZSwap(ref Vector3 vect)
    {
        float k = vect.z;
        vect.z = vect.y;
        vect.y = k;
    }

    private float alpha = 120;
    void transToOrt(ref Vector3 vect)
    {
        float alphaInRad = 2 * Mathf.PI / 360 * alpha;
        vect.y = vect.y / Mathf.Sin(alphaInRad);
        vect.x = vect.x - vect.y * Mathf.Cos(alphaInRad);

    }


    private float a = 9.819f;
    private float b = 9.819f;
    private float c = 7.987f;

    void deScale(ref Vector3 vect)
    {
        vect.Set(vect.x / a, vect.y / b, vect.z / c);
    }

    void OnMouseDown()
    {
        Vector3 vect = new Vector3(this.transform.localPosition.x,
              this.transform.localPosition.y, this.transform.localPosition.z);
        YZSwap(ref vect);
        transToOrt(ref vect);
        deScale(ref vect);

        if (((Time.time - doubleClickStart) < 0.3f)&& vect.x != 0 && vect.x != 1 
            && vect.y != 0 && vect.y != 1 && vect.z != 0 && vect.z != 1)
        {
            OnDoubleClick();
            doubleClickStart = -1;
        }
        else
        {
            doubleClickStart = Time.time;
        }
    }

    static float destination(Vector3 v1, Vector3 v2)
    {
        float d = Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x)
            + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));

        return d;
    }

    void OnDoubleClick()
    {
        HideElements();
        GameObject[] oxygens = GameObject.FindGameObjectsWithTag("Oxygen");
        float[] dists = new float[oxygens.Length];
        for (int i = 0; i < oxygens.Length; i++)
        {
            dists[i] = destination(this.transform.position, oxygens[i].transform.position);
            Debug.LogWarning(dists[i]);
        }
        Array.Sort(dists, oxygens);
        for (int i = 6; i < oxygens.Length; i++)
        {
            oxygens[i].SetActive(false);
        }
    }

    private void HideElements()
    {
        GameObject[] bors = GameObject.FindGameObjectsWithTag("Bor");
        foreach (var bor in bors)
            bor.SetActive(false);
        GameObject[] scandiums = GameObject.FindGameObjectsWithTag("Scandium");
        foreach (var scandium in scandiums)
            scandium.SetActive(false);
        GameObject[] lantans = GameObject.FindGameObjectsWithTag("Lantan");
        foreach (var la in lantans)
            if (la.transform.position != this.transform.position)
                la.SetActive(false);
    }
}
