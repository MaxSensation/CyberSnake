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
    [SerializeField] private GameObject totalPlayers;
    [SerializeField] private GameObject hostButton;
    [SerializeField] private GameObject clientButton;
    [SerializeField] private GameObject inputFeild;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject postJoinText;
    
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        SetSearchingText();
        TotalPlayersManager.onPlayerConnect += SetConnectedText;
    }

    private void SetConnectedText()
    {
        postJoinText.GetComponent<TMP_Text>().SetText("Please wait until host starts the game");
    }
    
    private void SetSearchingText()
    {
        postJoinText.GetComponent<TMP_Text>().SetText("No Host found please go back and try again!");
    }

    public void StartHost()
    {
        totalPlayers.SetActive(true);
        hostButton.SetActive(false);
        clientButton.SetActive(false);
        inputFeild.SetActive(false);
        startButton.SetActive(true);
        backButton.SetActive(true);
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

    public void BackButton()
    {
        SetSearchingText();
        startButton.SetActive(false);
        hostButton.SetActive(true);
        clientButton.SetActive(true);
        inputFeild.SetActive(true);
        postJoinText.SetActive(false);
        totalPlayers.SetActive(false);
        backButton.SetActive(false);
        NetworkManager.Singleton.StopClient();
    }

    public void StartClient()
    {
        if (inputField.text != "")
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = inputField.text;
        }
        NetworkManager.Singleton.StartClient();
        hostButton.SetActive(false);
        totalPlayers.SetActive(true);
        clientButton.SetActive(false);
        inputFeild.SetActive(false);
        postJoinText.SetActive(true);
        backButton.SetActive(true);
    }
}
