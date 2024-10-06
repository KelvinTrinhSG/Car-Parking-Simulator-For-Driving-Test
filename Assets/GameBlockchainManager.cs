using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameBlockchainManager : MonoBehaviour
{
    public string address { get; private set; }

    //Win Canvas
    public Text winTokenBalanceText;
    public Button wincheckTokenBalanceButton;
    public Button winNextButton;
    public Button winExitButton;

    //Failed Canvas
    public Text loseTokenBalanceText;
    public Button losecheckTokenBalanceButton;
    public Button loseRetryButton;
    public Button loseExitButton;

    string tokenAddressSmartContract = "0xa0e7f4d6A2449E4baDE87C7369C8B1659A398f39";

    private void Start()
    {
        winTokenBalanceText.gameObject.SetActive(false);
        loseTokenBalanceText.gameObject.SetActive(false);
    }

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

    public async void WinCheckTokenBalance()
    {
        wincheckTokenBalanceButton.interactable = false;
        winNextButton.interactable = false;
        winExitButton.interactable = false;

        winTokenBalanceText.text = "Checking!";
        winTokenBalanceText.gameObject.SetActive(true);

        address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);
        try
        {
            var tokenBalance = await contract.ERC20.BalanceOf(address);

            Debug.Log("Token Balance: " + tokenBalance.displayValue);

            int tokenBalanceInt = ConvertStringToInt(tokenBalance.displayValue);

            winTokenBalanceText.text = tokenBalanceInt.ToString();
            winTokenBalanceText.gameObject.SetActive(true);

            wincheckTokenBalanceButton.interactable = true;
            winNextButton.interactable = true;
            winExitButton.interactable = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching token balance: " + ex.Message);

            winTokenBalanceText.text = "Error!";
            winTokenBalanceText.gameObject.SetActive(true);

            wincheckTokenBalanceButton.interactable = true;
            winNextButton.interactable = true;
            winExitButton.interactable = true;
        }
    }

    public async void LoseCheckTokenBalance()
    {
        losecheckTokenBalanceButton.interactable = false;
        loseRetryButton.interactable = false;
        loseExitButton.interactable = false;

        loseTokenBalanceText.text = "Checking!";
        loseTokenBalanceText.gameObject.SetActive(true);

        address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);
        try
        {
            var tokenBalance = await contract.ERC20.BalanceOf(address);

            Debug.Log("Token Balance: " + tokenBalance.displayValue);

            int tokenBalanceInt = ConvertStringToInt(tokenBalance.displayValue);

            loseTokenBalanceText.text = tokenBalanceInt.ToString();
            loseTokenBalanceText.gameObject.SetActive(true);

            losecheckTokenBalanceButton.interactable = true;
            loseRetryButton.interactable = true;
            loseExitButton.interactable = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching token balance: " + ex.Message);

            loseTokenBalanceText.text = "Error!";
            loseTokenBalanceText.gameObject.SetActive(true);

            losecheckTokenBalanceButton.interactable = true;
            loseRetryButton.interactable = true;
            loseExitButton.interactable = true;
        }
    }
}
