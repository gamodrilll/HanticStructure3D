using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO;

public class SavePicScript : MonoBehaviour {

    public GameObject canvas;

    public static bool click = true;

    void clickSave()
    {
        string name = "pic";
        int n = 0;
        if (File.Exists("pic.png"))
        {
            n++;
            while (File.Exists(name + n.ToString() + ".png"))
                n++;
        }
        canvas.SetActive(false);
        Application.CaptureScreenshot("pic" + (n==0 ? "" : n.ToString()) + ".png");
        canvas.SetActive(true);
    }

    // Use this for initialization
    void Start () {
        this.GetComponent<Button>().onClick.AddListener(clickSave);
    }
}
