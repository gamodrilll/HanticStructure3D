using UnityEngine;
using UnityEngine.UI;
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

        if (((Time.time - doubleClickStart) < 0.3f) && vect.x != 0 && vect.x != 1
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

    static float distance(Vector3 v1, Vector3 v2)
    {
        float d = Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x)
            + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));

        return d;
    }

    static float distance(Vector2 v1, Vector2 v2)
    {
        float d = Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x)
            + (v1.y - v2.y) * (v1.y - v2.y));

        return d;
    }

    void OnDoubleClick()
    {
        HideElements();
        GameObject[] oxygens = GameObject.FindGameObjectsWithTag("Oxygen");
        float[] dists = new float[oxygens.Length];
        for (int i = 0; i < oxygens.Length; i++)
        {
            dists[i] = distance(this.transform.position, oxygens[i].transform.position);
        }
        Array.Sort(dists, oxygens);
        if (this.gameObject.tag == "Lantan" || this.gameObject.tag == "Scandium")
        #region La Sc
        {
            for (int i = 6; i < oxygens.Length; i++)
            {
                oxygens[i].SetActive(false);
            }
            GameObject[] ox = new GameObject[6];
            for (int i = 0; i <= 5; i++)
            {
                ox[i] = oxygens[i];
            }
            float[] dist = new float[6];
            for (int i = 0; i <= 5; i++)
                dist[i] = ox[i].transform.localPosition.y;
            Array.Sort(dist, ox);

            GameObject[] ar1 = new GameObject[3];
            GameObject[] ar2 = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                ar1[i] = ox[i];
                Vector2 A, B1, B2, B3;
                A = new Vector2(ox[i].transform.localPosition.x, ox[i].transform.localPosition.z);
                B1 = new Vector2(ox[3].transform.localPosition.x, ox[3].transform.localPosition.z);
                B2 = new Vector2(ox[4].transform.localPosition.x, ox[4].transform.localPosition.z);
                B3 = new Vector2(ox[5].transform.localPosition.x, ox[5].transform.localPosition.z);
                ar2[i] = distance(A, B1) < distance(A, B2) ? distance(A, B1) < distance(A, B3) ?
                    ox[3] : ox[5] : distance(A, B2) < distance(A, B3) ? ox[4] : ox[5];
            }
            Vector3[] coords = new Vector3[]
            {
                getVec(ar1[0]),getVec(ar1[1]), getVec(ar2[1]),
                getVec(ar1[1]),getVec(ar1[2]),getVec(ar2[2]),
                getVec(ar1[2]),getVec(ar1[0]),getVec(ar2[0]),
                getVec(ar2[1]),getVec(ar2[2]),getVec(ar2[0])
            };
            Info.DrawLine(this.gameObject, coords);
        }
        #endregion
        else
        {
            for (int i = 3; i < oxygens.Length; i++)
            {
                oxygens[i].SetActive(false);
            }

            Vector3[] coords = new Vector3[]
            {
                getVec(oxygens[0]),getVec(oxygens[1]), getVec(oxygens[2]),
                getVec(oxygens[0])};
            Info.DrawLine(this.gameObject, coords);
        }
    }

    private Vector3 getVec(GameObject gameObject)
    {
        return gameObject.transform.localPosition;
    }

    private void HideElements()
    {
        foreach (var bor in Info.bors1)
            if (bor.transform.position != this.transform.position)
                bor.SetActive(false);
        foreach (var bor in Info.bors2)
            if (bor.transform.position != this.transform.position)
                bor.SetActive(false);
        GameObject[] scandiums = GameObject.FindGameObjectsWithTag("Scandium");
        foreach (var scandium in scandiums)
            if (scandium.transform.position != this.transform.position)
                scandium.SetActive(false);
        GameObject[] lantans = GameObject.FindGameObjectsWithTag("Lantan");
        foreach (var la in lantans)
            if (la.transform.position != this.transform.position)
                la.SetActive(false);
        Info.borders.SetActive(false);
    }
}
