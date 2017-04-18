using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;

public class ClickScript : MonoBehaviour {

    private Text toolTip;
    private GameObject toolTipP;


    public string El;


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
        ClickData c = new ClickData();

        c.name = El;
        CoordScript scr = GetComponent<CoordScript>();
        c.a = Info.a;
        c.b = Info.b;
        c.c = Info.c;
        c.x = scr.x;
        c.y = scr.y;
        c.z = scr.z;
        c.beta = false;

        Info.sendClickDatatoWin(c);
    }

}
