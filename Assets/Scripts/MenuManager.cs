using System;
using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static Action onStart;
    [SerializeField] private GameObject hostButton;
    [SerializeField] private GameObject clientButton;
    [SerializeField] private GameObject inputFeild;
    [SerializeField] private GameObject startButton;
    
    [SerializeField] private TMP_InputField inputField;
    public void StartHost()
    {
        hostButton.SetActive(false);
        clientButton.SetActive(false);
        inputFeild.SetActive(false);
        startButton.SetActive(true);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Plane");
        callback(true, prefabHash, true, Vector3.zero, Quaternion.identity);
    }

    public void StartGame()
    {
        NetworkSceneManager.SwitchScene("Game");
        onStart?.Invoke();
    }

    public void StartClient()
    {
        // Fix check for online shit
        if (inputField.text != "")
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = inputField.text;
        }
        NetworkManager.Singleton.StartClient();
        hostButton.SetActive(false);
        clientButton.SetActive(false);
        inputFeild.SetActive(false);
    }
}
