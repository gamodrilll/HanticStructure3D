using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class XZClickScript : MonoBehaviour
{
    public Camera mainCam;
    public Button XZBut;
    public Button XYBut;
    public Button ThreeDBut;
    public Button reset;

    // Use this for initialization
    void Start()
    {
        XZBut.onClick.AddListener(onClick);
        XYBut.onClick.AddListener(onClickXY);
        ThreeDBut.onClick.AddListener(onClick3D);
        reset.onClick.AddListener(resetScene);
    }


    void resetScene()
    {
        ShowAll();
        #region Hide All Lines
        foreach (var la in Info.lantans)
        {
            LineRenderer[] ar = la.GetComponents<LineRenderer>();
            foreach(var lr in ar)
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

    void onClick()
    {
        mainCam.transform.rotation = Quaternion.Euler(90, 210, 0);
        mainCam.transform.position = new Vector3(3f, 20f, 4.5f);
        mainCam.orthographicSize = 9;
    }

    void onClickXY()
    {
        mainCam.transform.rotation = Quaternion.Euler(0, 120, 0);
        mainCam.transform.position = new Vector3(-25f, 5f, 20f);
        mainCam.orthographicSize = 6;
    }

    void onClick3D()
    {
        mainCam.transform.rotation = Quaternion.Euler(30, -150, 0);
        mainCam.transform.position = new Vector3(23.4f, 28.1f, 38.4f);
        mainCam.orthographicSize = 10;
    }
}