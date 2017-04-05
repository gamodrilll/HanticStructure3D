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


    void deScale(ref Vector3 vect)
    {
        vect.Set(vect.x / Info.a, vect.y / Info.b, vect.z / Info.c);
    }

    void OnMouseDown()
    {
        Vector3 vect = new Vector3(this.transform.localPosition.x,
              this.transform.localPosition.y, this.transform.localPosition.z);
        YZSwap(ref vect);
        transToOrt(ref vect);
        deScale(ref vect);

        if (((Time.time - doubleClickStart) < 0.3f))
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
        GameObject[] oxygens = Info.oxygens.ToArray();
        float[] dists = new float[oxygens.Length];
        for (int i = 0; i < oxygens.Length; i++)
        {
            dists[i] = distance(this.transform.position, oxygens[i].transform.position);
        }
        Array.Sort(dists, oxygens);
        if (this.gameObject.tag == "La"  || this.gameObject.tag == "Nd")
        #region La 
        {
            for (int i = 0; i < 6; i++)
            {
                oxygens[i].SetActive(true);
            }
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
        if ( this.gameObject.tag == "Sc")
        {
            for (int i = 0; i < 6; i++)
            {
                oxygens[i].SetActive(true);
            }
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
                dist[i] = (ox[i].transform.localPosition).magnitude;
            Array.Sort(dist, ox);
            Vector3 vec = getVec(ox[2]) - getVec(ox[1]), vec2 = getVec(ox[3]) - getVec(ox[2]);
            if (vec2.magnitude<vec.magnitude)
            {
                GameObject temp = ox[1];
                ox[1] = ox[3];
                ox[3] = temp;
            }
            vec = getVec(ox[3]) - getVec(ox[2]); vec2 = getVec(ox[4]) - getVec(ox[2]);
            if (vec2.magnitude < vec.magnitude)
            {
                GameObject temp = ox[4];
                ox[4] = ox[3];
                ox[3] = temp;
            }

            GameObject[] ar1 = new GameObject[3];
            Vector3[] coords = new Vector3[]
            {
                getVec(ox[0]),getVec(ox[1]), getVec(ox[2]),
                getVec(ox[0]),getVec(ox[3]),getVec(ox[4]),
                getVec(ox[0]),getVec(ox[4]),
                getVec(ox[5]),getVec(ox[3]),getVec(ox[2]),
                getVec(ox[5]),getVec(ox[1]),getVec(ox[4])
            };
            Info.DrawLine(this.gameObject, coords);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                oxygens[i].SetActive(true);
            }
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
        Info.scr.setCenter(transform.localPosition);
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
        foreach (var scandium in Info.scandiums)
            if (scandium.transform.position != this.transform.position)
                scandium.SetActive(false);
        foreach (var la in Info.lantans)
            if (la.transform.position != this.transform.position)
                la.SetActive(false);
        Info.borders.SetActive(false);
    }
}
