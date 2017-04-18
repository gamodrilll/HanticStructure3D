using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipScript : MonoBehaviour {

    private Text text;
    private Text toolTip;
    private GameObject toolTipP;

    // Use this for initialization
    void Start () {
        toolTip = GameObject.Find("Canvas/ToolTipP/ToolTip").GetComponent<Text>();
        toolTipP = GameObject.Find("Canvas/ToolTipP");
        toolTipP.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) //&& hit.collider.gameObject.name == "Yify")
        {
            string s = hit.collider.gameObject.name + " ";
            Vector3 vect = new Vector3(hit.collider.gameObject.transform.localPosition.x,
            hit.collider.gameObject.transform.localPosition.y, hit.collider.gameObject.transform.localPosition.z);
            Info.YZSwap(ref vect);
            Info.transToOrt(ref vect);
            s += " x/a: " + (vect.x / Info.a).ToString("F3") + " \n y/b: "
            + (vect.y / Info.b).ToString("F3") + " z/c: " + (vect.z / Info.c).ToString("F3");
            toolTip.text = s;
            toolTipP.SetActive(true);
            toolTipP.transform.Translate(Input.mousePosition - toolTipP.transform.position + new Vector3(-80, 25, 0));
        }
        else
        {
            toolTipP.SetActive(false);
            toolTip.text = "";
        }
    }
}
