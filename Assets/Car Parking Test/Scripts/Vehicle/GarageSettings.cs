﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GarageSettings : MonoBehaviour {

	public string garageSceneName = "Garage";

	public GameObject cameraParent;

	void Start () 
	{
		
		if (SceneManager.GetActiveScene ().name.Contains (garageSceneName))
			cameraParent.SetActive (false);
		else
			cameraParent.SetActive (true);
		
	}
}
