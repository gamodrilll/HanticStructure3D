using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickScript : MonoBehaviour {

    private Text text;
    private Text toolTip;
    private GameObject toolTipP;

    public string El;

    
	// Use this for initialization
	void Start () {
        text = GameObject.Find("Canvas/ElInfo").GetComponent<Text>();
	}

    void deScale(ref Vector3 vect)
    {
        vect.Set(vect.x / Info.a, vect.y / Info.b, vect.z / Info.c);
    }

    void OnMouseDown()
    {
        AnglButtonScript.clickElement(this.transform.localPosition, this.name);
        DistClickScript.clickElement(this.transform.localPosition, this.name);
        El = this.gameObject.name;
        Vector3 vect = new Vector3(this.transform.localPosition.x,
            this.transform.localPosition.y,this.transform.localPosition.z);
        Info.YZSwap(ref vect);
        Info.transToOrt(ref vect);
        //deScale(ref vect);
        text.text =El +" x: " +  vect.x.ToString("F3") + " y: "
            + vect.y.ToString("F3") + " z: " +  vect.z.ToString("F3")
            + " x/a: " + (vect.x/Info.a) .ToString("F3") + " y/b: "
            + (vect.y/ Info.b).ToString("F3") + " z/c: " + (vect.z/ Info.c).ToString("F3");
        Info.logFile.WriteLine(text.text);
    }

}
