using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class XZClickScript : MonoBehaviour {
    public Camera mainCam;
    public Button XZBut;
    public Button XYBut;
    public Button ThreeDBut;
    public Button reset;

    GameObject Z1;
    GameObject Z2;


    void deactiveCameraRot()
    {
        MouseOrbit orb = GameObject.Find("Main Camera").GetComponent<MouseOrbit>();
        orb.yaw.activate = false;
        orb.pitch.activate = false;
    }

    void activeCameraRot()
    {
        MouseOrbit orb = GameObject.Find("Main Camera").GetComponent<MouseOrbit>();
        orb.yaw.activate = true;
        orb.pitch.activate = true;
    }


	// Use this for initialization
	void Start () {
        Z1 = GameObject.Find("AxeZ1");
        Z2 = GameObject.Find("AxeZ2");
        XZBut.onClick.AddListener(onClick) ;
        XYBut.onClick.AddListener(onClickXY);
        ThreeDBut.onClick.AddListener(onClick3D);
        //reset.onClick.AddListener(resetScene);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void resetScene()
    {
        SceneManager.LoadScene(0);
    }
    void onClick()
    {
        deactiveCameraRot();
        mainCam.transform.rotation = Quaternion.Euler(90, 210, 0);
        mainCam.transform.position = new Vector3(3f,20f,4.5f);
        mainCam.orthographicSize = 9;
    }

    void onClickXY()
    {
        deactiveCameraRot();
        //setZActive(false);
        mainCam.transform.rotation = Quaternion.Euler(0, 120, 0);
        mainCam.transform.position = new Vector3(-25f, 5f, 20f);
        mainCam.orthographicSize = 6;
    }

    void onClick3D()
    {
        //setZActive(true);
        activeCameraRot();
        mainCam.transform.rotation = Quaternion.Euler(30, -150, 0);
        mainCam.transform.position = new Vector3(23.4f, 28.1f, 38.4f);
        mainCam.orthographicSize = 10;
    }
}
