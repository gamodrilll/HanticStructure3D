using UnityEngine;
using System.Collections;

public class CanvScript : MonoBehaviour {

    void Update()
    {
        if (SavePicScript.click)
        {
            SavePicScript.click = false;
            Application.CaptureScreenshot("pic" + ".png");
            this.gameObject.SetActive(true);
        }

    }
}
