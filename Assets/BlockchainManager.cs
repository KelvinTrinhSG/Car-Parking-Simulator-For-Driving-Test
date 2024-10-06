using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BlockchainManager : MonoBehaviour
{
    public string address { get; private set; }


    public GameObject tokenAndStartPanel;
    public TMP_Text tokenBalanceText;
    public TMP_Text tokenClaimingStatusText;

    public Button startButton;
    public Button tokenButton;

    string tokenAddressSmartContract = "0xa0e7f4d6A2449E4baDE87C7369C8B1659A398f39";

    private void Start()
    {
        tokenAndStartPanel.SetActive(false);
        startButton.gameObject.SetActive(false);
        tokenButton.gameObject.SetActive(false);
        tokenClaimingStatusText.gameObject.SetActive(false);
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

    public async void Login()
    {
        address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);
        try
        {
            var tokenBalance = await contract.ERC20.BalanceOf(address);

            Debug.Log("Token Balance: " + tokenBalance.displayValue);

            int tokenBalanceInt = ConvertStringToInt(tokenBalance.displayValue);

            tokenBalanceText.text = tokenBalanceInt.ToString();

            if (tokenBalanceInt >= 5)
            {
                tokenClaimingStatusText.text = "You already have 5 Tokens";
                tokenClaimingStatusText.gameObject.SetActive(true);
                startButton.interactable = true;
                tokenButton.interactable = true;
                startButton.gameObject.SetActive(true);
                tokenButton.gameObject.SetActive(false);
            }
            else {
                tokenClaimingStatusText.text = "Claim 5 token to Start the Test";
                tokenClaimingStatusText.gameObject.SetActive(true);
                startButton.interactable = true;
                tokenButton.interactable = true;
                startButton.gameObject.SetActive(false);
                tokenButton.gameObject.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching token balance: " + ex.Message);
        }
    }

    public async void ClaimToken()
    {

        tokenClaimingStatusText.text = "Claiming 5 Tokens!";
        tokenClaimingStatusText.gameObject.SetActive(true);

        startButton.interactable = false;
        tokenButton.interactable = false;

        address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(tokenAddressSmartContract);
        try
        {
            var tokenBalance = await contract.ERC20.BalanceOf(address);

            Debug.Log("Token Balance: " + tokenBalance.displayValue);

            int tokenBalanceInt = ConvertStringToInt(tokenBalance.displayValue);

            if (tokenBalanceInt >= 5)
            {
                tokenClaimingStatusText.text = "You already have 5 Tokens";
                tokenClaimingStatusText.gameObject.SetActive(true);
                startButton.interactable = true;
                tokenButton.interactable = true;

                // Thiết lập LevelID bằng 0
                PlayerPrefs.SetInt("LevelID", 0);

                // Lưu lại các thiết lập
                PlayerPrefs.Save();

                return;
            }

            else if (tokenBalanceInt <= 0) {
                try
                {
                    // Gọi hàm Claim token với giá trị "5"
                    await contract.ERC20.Claim("5");
                    Debug.Log("Tokens claimed successfully.");

                    tokenClaimingStatusText.text = "You claimed 5 Tokens";
                    tokenClaimingStatusText.gameObject.SetActive(true);

                    // Thiết lập LevelID bằng 0
                    PlayerPrefs.SetInt("LevelID", 0);

                    // Lưu lại các thiết lập
                    PlayerPrefs.Save();

                    Login();

                }
                catch (Exception ex)
                {
                    // Bắt và hiển thị lỗi nếu có ngoại lệ
                    Debug.LogError("Error claiming tokens: " + ex.Message);

                    tokenClaimingStatusText.text = "Network Error, Please claim again";
                    tokenClaimingStatusText.gameObject.SetActive(true);
                    startButton.interactable = true;
                    tokenButton.interactable = true;
                }
            }

            else {
                try
                {
                    // Gọi hàm Burn token
                    await contract.ERC20.Burn(tokenBalance.displayValue);
                    Debug.Log("Tokens burned successfully.");
                    try
                    {
                        // Gọi hàm Claim token với giá trị "5"
                        await contract.ERC20.Claim("5");
                        Debug.Log("Tokens claimed successfully.");

                        tokenClaimingStatusText.text = "You claimed 5 Tokens";
                        tokenClaimingStatusText.gameObject.SetActive(true);

                        // Thiết lập LevelID bằng 0
                        PlayerPrefs.SetInt("LevelID", 0);

                        // Lưu lại các thiết lập
                        PlayerPrefs.Save();

                        Login();

                    }
                    catch (Exception ex)
                    {
                        // Bắt và hiển thị lỗi nếu có ngoại lệ
                        Debug.LogError("Error claiming tokens: " + ex.Message);

                        tokenClaimingStatusText.text = "Network Error, Please claim again";
                        tokenClaimingStatusText.gameObject.SetActive(true);
                        startButton.interactable = true;
                        tokenButton.interactable = true;
                    }
                }
                catch (Exception ex)
                {
                    // Bắt và hiển thị lỗi
                    Debug.LogError("Error burning tokens: " + ex.Message);

                    tokenClaimingStatusText.text = "Network Error, Please claim again";
                    tokenClaimingStatusText.gameObject.SetActive(true);
                    startButton.interactable = true;
                    tokenButton.interactable = true;
                }

            }

        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching token balance: " + ex.Message);

            tokenClaimingStatusText.text = "Network Error, Please claim again";
            tokenClaimingStatusText.gameObject.SetActive(true);
            startButton.interactable = true;
            tokenButton.interactable = true;
        }
    }

    public void StartTesting() {
        SceneManager.LoadScene("Garage");
    }
}
