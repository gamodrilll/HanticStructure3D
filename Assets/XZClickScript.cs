using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

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
        Debug.LogWarning("Work");
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
        foreach (var bor in Info.bors1)
            bor.SetActive(true);
        foreach (var bor in Info.bors2)
            bor.SetActive(true);
        foreach (var scandium in Info.scandiums)
            scandium.SetActive(true);
        foreach (var la in Info.lantans)
            la.SetActive(true);
        foreach (var ox in Info.oxygens)
            ox.SetActive(true);
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