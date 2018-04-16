using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour {

    private Button clickableButton, buttonPrefab;

    public Line(Vector3 position, Vector3 size, int rotationCode)
    {
        clickableButton = Instantiate(buttonPrefab, new Vector3(0, 0, 1), Quaternion.identity) as Button;
        clickableButton.onClick.AddListener(() => OnClick());
        //Add itself to button
        //Set parent to panel
        //Set position size and scale also rotation
    }

    private void OnClick()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
