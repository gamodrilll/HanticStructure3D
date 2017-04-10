using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ValChangedScript : MonoBehaviour {


    void valueChanged(int val)
    {
        Info.borders.SetActive(true);
        //Info.scr.CreateCompound(val);
    }

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Dropdown>().onValueChanged.AddListener(valueChanged);	
	}
	
}
