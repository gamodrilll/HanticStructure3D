using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistClickScript : MonoBehaviour {

    static int pos = 2;
    public static bool ClickEl = false;
    public static Vector3 a;
    public static string infA;
    public static Vector3 b;
    public static string infB;

    public static float CalculateDist()
    {
        return Info.distance(a, b);
    }

    public static void clickElement(Vector3 vec)
    {

    }

    void clickDistButton()
    {
        if (pos == 2)
        {
            pos = 0;
            this.GetComponent<Button>().GetComponentInChildren<Text>().text = "Click first element";
            return;
        }

        if (pos == 0 || pos == 1 )
        {
            pos = 2;
            this.GetComponent<Button>().GetComponentInChildren<Text>().text = "Distance";
            return;
        }
    }

    // Use this for initialization
    void Start () {
        this.GetComponent<Button>().onClick.AddListener(clickDistButton);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
