using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickScript : MonoBehaviour {

    private Text text;

    public string El;
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

	// Use this for initialization
	void Start () {
        text = GameObject.Find("Canvas/ElInfo").GetComponent<Text>();
        text.text = "";
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
            this.transform.localPosition.y,this.transform.localPosition.z);
        YZSwap(ref vect);
        transToOrt(ref vect);
        //deScale(ref vect);
        text.text =El +" x: " +  vect.x.ToString("F3") + " y: "
            + vect.y.ToString("F3") + " z: " +  vect.z.ToString("F3")
            + " x/a: " + (vect.x/a) .ToString("F3") + " y/b: "
            + (vect.y/b).ToString("F3") + " z/c: " + (vect.z/c).ToString("F3");
    }

}
