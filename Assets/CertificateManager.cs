using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CertificateManager : MonoBehaviour
{
    public string Address { get; private set; }

    public Button claimCertificateButton;
    public Button backToMainMenu;

    public TextMeshProUGUI certificateClaimingStatusText;

    string CertificateAddressSmartContract = "0x4250237aC0C2D0111D740811417BB007E0222Eab";

    private void Start()
    {
        claimCertificateButton.gameObject.SetActive(false);
        backToMainMenu.gameObject.SetActive(false);
        certificateClaimingStatusText.gameObject.SetActive(false);

        CheckCertificateStatus();
    }

    private async void CheckCertificateStatus()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(CertificateAddressSmartContract);
        try
        {
            List<NFT> nftList = await contract.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                claimCertificateButton.gameObject.SetActive(true);
            }
            else
            {
                claimCertificateButton.gameObject.SetActive(false);
                certificateClaimingStatusText.text = "You already have the Certificate";
                certificateClaimingStatusText.gameObject.SetActive(true);
                backToMainMenu.gameObject.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error retrieving NFTs: " + ex.Message);
            // Optionally, display a message to the user
            certificateClaimingStatusText.text = "An error occurred while checking certificates.";
            certificateClaimingStatusText.gameObject.SetActive(true);
            claimCertificateButton.gameObject.SetActive(true);
        }        
    }


    public async void ClaimNFTPass()
    {
        certificateClaimingStatusText.text = "Claiming Certificate...";
        claimCertificateButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract(CertificateAddressSmartContract);
        try
        {
            var result = await contract.ERC721.ClaimTo(Address, 1);
            certificateClaimingStatusText.text = "You claimed the Certificate";
            certificateClaimingStatusText.gameObject.SetActive(true);

            claimCertificateButton.gameObject.SetActive(false);
            backToMainMenu.gameObject.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error claiming the certificate: " + ex.Message);
            // Optionally, display an error message to the user
            certificateClaimingStatusText.text = "An error occurred while claiming the certificate.";
            certificateClaimingStatusText.gameObject.SetActive(true);
            claimCertificateButton.gameObject.SetActive(true);  // Allow the user to try again
        }
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("ConnectWallet");
    }

}
