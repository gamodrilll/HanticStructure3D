using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistClickScript : MonoBehaviour {

    static int pos = 2;
    static bool ClickEl = false;
    static Vector3 a;
    static string infA;
    static Vector3 b;
    static string infB;
    public Text result;
    static Text text;
    static Text res;

    public static float CalculateDist()
    {
        return Info.distance(a, b);
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
            text.text = "Distance";
            b = vec;
            infB = inf;
            res.text = "Dist between " + infA + " and " + infB + " is " 
                + CalculateDist().ToString("F3");
            Info.logFile.WriteLine(res.text);
            return;
        }

    }

    void clickDistButton()
    {
        result.text = "";
        if (pos == 2)
        {
            pos = 0;
            text.text = "Click first element";
            return;
        }

        if (pos == 0 || pos == 1 )
        {
            pos = 2;
            text.text = "Distance";
            return;
        }
    }

    // Use this for initialization
    void Start () {
        res = result;
        result.text = "";
        this.GetComponent<Button>().onClick.AddListener(clickDistButton);
        text = this.GetComponent<Button>().GetComponentInChildren<Text>();

    }
}
