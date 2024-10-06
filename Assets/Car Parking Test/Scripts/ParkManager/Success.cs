using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Thirdweb;
using System;

public class Success : MonoBehaviour
{
	[Header("Success Menu Manager")]

	// Loading text for "Loading..."
	public Text LoadingTXT;

	// Parking Manager handler
	[HideInInspector]public ParkingManager manager;

	public string garageName = "Garage" ;

	// Activate parking place helper
	public void  ActiveHelper ()
	{
		manager.Helper.SetActive (!manager.Helper.activeSelf);
	}


	public IEnumerator Start ()
	{

		// Delay and find manager
		yield return new WaitForEndOfFrame();

		loseExitButton.gameObject.SetActive(false);

		manager = GameObject.FindGameObjectWithTag ("Manager").GetComponent<ParkingManager> ();
	}

	// SuccessMenu continue button
	public void Continue ()
	{
		LoadingTXT.text = "Loading...";

		PlayerPrefs.SetInt ("LevelID", PlayerPrefs.GetInt ("LevelID") + 1);

		if (PlayerPrefs.GetInt("LevelID") >= 20)
		{
			Debug.Log("You win");
			SceneManager.LoadScene("Certificate");
		}
		else {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public string address { get; private set; }
	//Failed Canvas
	public Text loseTokenBalanceText;
	public Button losecheckTokenBalanceButton;
	public Button loseRetryButton;
	public Button loseExitButton;

	string tokenAddressSmartContract = "0xa0e7f4d6A2449E4baDE87C7369C8B1659A398f39";

	private int ConvertStringToInt(string strNumber)
	{
		try
		{
			// Chuyển đổi chuỗi thành float
			float floatNumber = float.Parse(strNumber);

			// Chuyển thành int bằng cách làm tròn số gần nhất
			int intNumber = Mathf.RoundToInt(floatNumber);

			// Trả về giá trị int
			return intNumber;
		}
		catch (FormatException)
		{
			Debug.LogError("The string is not a valid number.");
			// Trả về giá trị mặc định khi lỗi
			return 0;
		}
	}

	public async void RetryLevel()
	{
		loseTokenBalanceText.text = "-1 Token To Retry!";
		loseTokenBalanceText.gameObject.SetActive(true);

		losecheckTokenBalanceButton.interactable = false;
		loseRetryButton.interactable = false;
		//loseExitButton.interactable = false;

		address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
		Debug.Log(address);
		Contract contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);

		try
		{
			var tokenBalance = await contract.ERC20.BalanceOf(address);
			Debug.Log("Token Balance: " + tokenBalance.displayValue);
			int tokenBalanceInt = ConvertStringToInt(tokenBalance.displayValue);
			if (tokenBalanceInt < 1)
			{
				loseTokenBalanceText.text = "You Failed The Test";
				loseTokenBalanceText.gameObject.SetActive(true);
				//loseExitButton.interactable = true;
				loseExitButton.gameObject.SetActive(true);
				loseRetryButton.gameObject.SetActive(false);
				losecheckTokenBalanceButton.interactable = true;
			}
			else {
				try
				{
					// Gọi hàm Burn token
					await contract.ERC20.Burn("1");
					Debug.Log("Tokens burned successfully.");

					loseTokenBalanceText.text = "Retry Now";
					loseTokenBalanceText.gameObject.SetActive(true);
					losecheckTokenBalanceButton.interactable = true;
					loseRetryButton.interactable = true;
					//loseExitButton.interactable = true;
					Retry();
				}
				catch (Exception ex)
				{
					// Bắt và hiển thị lỗi
					Debug.LogError("Error burning tokens: " + ex.Message);

					loseTokenBalanceText.text = "Network Error, Please retry again";
					loseTokenBalanceText.gameObject.SetActive(true);
					losecheckTokenBalanceButton.interactable = true;
					//loseExitButton.interactable = true;
					loseRetryButton.interactable = true;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Error fetching token balance: " + ex.Message);
			loseTokenBalanceText.text = "Error!";
			loseTokenBalanceText.gameObject.SetActive(true);
			losecheckTokenBalanceButton.interactable = true;
			loseRetryButton.interactable = true;
			//loseExitButton.interactable = true;
		}
	}


	// SuccessMenu retry button
	public void Retry ()
	{
		LoadingTXT.text = "Loading...";
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name   );
	}


	//SuccessMenu exit button    
	public void Exit (int indexValue)
	{
		if (indexValue == 0)
		{
			LoadingTXT.text = "Loading...";
			SceneManager.LoadScene(garageName);
		}
		else {
			LoadingTXT.text = "Loading...";
			SceneManager.LoadScene("ConnectWallet");
		}


	}
}
