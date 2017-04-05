using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AnglButtonScript : MonoBehaviour {
    static int pos = 3;
    static bool ClickEl = false;
    static Vector3 a;
    static string infA;
    static Vector3 b;
    static string infB;
    static Vector3 c;
    static string infC;
    static Vector3 vec1;
    static Vector3 vec2;
    public Text result;
    static Text text;
    static Text res;

    public static Vector3 getVec(Vector3 a, Vector3 b)
    {   
        return new Vector3(b.x - a.x, b.y - a.y, b.z - a.z);
    }

    public static float getLeng(Vector3 a)
    {
        return (float)Math.Sqrt(a.x* a.x+a.y*a.y+a.z*a.z);
    }


    public static float getAng(Vector3 a, Vector3 b)
    {
        double cos = (a.x * b.x + a.y * b.y + a.z * b.z) /
            getLeng(a) / getLeng(b);
        return (float)(Math.Acos(cos)/Math.PI*180);
    }

    public static void clickElement(Vector3 vec, string inf)
    {
        if (pos == 0)
        {
            pos = 1;
            text.text = "Click second element";
            a = vec;
            infA = inf;
            return;
        }

        if (pos == 1)
        {
            pos = 2;
            text.text = "Click third element";
            b = vec;
            infB = inf;
            return;
        }

        if (pos == 2)
        {
            pos = 0;
            text.text = "Click first element";
            c = vec;
            infC = inf;
            vec1 = getVec(a, b);
            vec2 = getVec(c, b);
            res.text = "Angle between " + infA + ", " + infB +" and " + infC + " is "
                + getAng(vec1,vec2).ToString("F3");
            Info.logFile.WriteLine(res.text);
            return;
        }

    }

    void clickAnglButton()
    {
        result.text = "";
        if (pos == 3)
        {
            pos = 0;
            text.text = "Click first element";
            return;
        }

        if (pos == 0 || pos == 1 || pos == 2)
        {
            pos = 3;
            text.text = "Angle";
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        res = result;
        result.text = "";
        this.GetComponent<Button>().onClick.AddListener(clickAnglButton);
        text = this.GetComponent<Button>().GetComponentInChildren<Text>();

    }
}
