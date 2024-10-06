using UnityEngine;
using System.Collections;

public class T_Roof : MonoBehaviour {

	public int carID;

	public GameObject[] Roofs;

	void Start()
	{
		SetRoof (PlayerPrefs.GetInt (carID.ToString()+"RoofID"), true);
	}

	public void SetRoof(int id,bool state)
	{

		// We have 6 roofs, and first de activate all roofs
		for (int a = 0; a < Roofs.Length; a++)
			Roofs [a].SetActive (false);
		
		if (Roofs[id]) 
			Roofs [id].SetActive (state);



	}
}
