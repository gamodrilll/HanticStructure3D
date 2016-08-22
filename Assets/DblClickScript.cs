using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DblClickScript : MonoBehaviour{

    float doubleClickStart = -100;

    private Text text;

    void Start()
    {
        text = GameObject.Find("Canvas/ElInfo").GetComponent<Text>();
        text.text = "";
    }

    void OnMouseDown()
  {
      if ((Time.time - doubleClickStart) < 0.3f)
      {
          this.OnDoubleClick();
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
        for (int i = 0; i < oxygens.Length;i++ )
        {
            dists[i] = destination(this.transform.position, oxygens[i].transform.position);
            Debug.LogWarning(dists[i]);
        }
        Array.Sort(dists,oxygens);
        for (int i = 6; i <oxygens.Length;i++ )
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
