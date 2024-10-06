using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorPicker : MonoBehaviour {

	// List of the colors
	public Color[] Colors;

	// Public function for changing color buttons
	public void SetColor (int id)
	{

			PlayerPrefs.SetInt ("CarColor" + PlayerPrefs.GetInt ("CarID").ToString (), id);

 			GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<ColorLoader>().mat.color = Colors [id];

	}
}
