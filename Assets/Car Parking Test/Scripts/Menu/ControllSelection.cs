using UnityEngine;
using System.Collections;

public class ControllSelection : MonoBehaviour {

	public void SelectControl (int id) 
	{

		PlayerPrefs.SetInt ("ControlType", id);
	}
	public void SetFalse(GameObject target)
	{
		target.SetActive (false);
	}


}
