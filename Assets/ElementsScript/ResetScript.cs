using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

    private void ShowAll()
    {
        List<GameObject> allElements = new List<GameObject>();
        allElements.AddRange(Info.bors1);
        allElements.AddRange(Info.bors2);
        allElements.AddRange(Info.scandiums);
        allElements.AddRange(Info.lantans);
        allElements.AddRange(Info.oxygens);
        foreach (var el in allElements)
        {
            Vector3 loc = new Vector3(el.transform.localPosition.x, el.transform.localPosition.y,
                el.transform.localPosition.z);
            Info.YZSwap(ref loc);
            Info.transToOrt(ref loc);
            Info.deScale(ref loc);
            if (loc.x >= -0.001 && loc.x <= 1.001 && loc.y >= -0.001
                && loc.y <= 1.001 && loc.z >= -0.001 && loc.z <= 1.001)
                el.SetActive(true);
            else
                el.SetActive(false);
        }
        Info.borders.SetActive(true);

    }


    // Update is called once per frame
    void Update () {
        if (Server.msg == "Reset")
        {
            ShowAll();
            #region Hide All Lines
            foreach (var la in Info.lantans)
            {
                LineRenderer[] ar = la.GetComponents<LineRenderer>();
                foreach (var lr in ar)
                {
                    lr.enabled = false;
                }
            }
            foreach (var bor in Info.bors1)
            {
                LineRenderer[] ar = bor.GetComponents<LineRenderer>();
                foreach (var lr in ar)
                {
                    lr.enabled = false;
                }
            }
            foreach (var bor in Info.bors2)
            {
                LineRenderer[] ar = bor.GetComponents<LineRenderer>();
                foreach (var lr in ar)
                {
                    lr.enabled = false;
                }
            }
            foreach (var sc in Info.scandiums)
            {
                LineRenderer[] ar = sc.GetComponents<LineRenderer>();
                foreach (var lr in ar)
                {
                    lr.enabled = false;
                }
            }
            #endregion
            Info.scr.setCenter();
            Server.msg = "";
        }
    }
}
