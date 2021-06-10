using System;
using MLAPI;
using MLAPI.Messaging;
using TMPro;
using UnityEngine;

public class InGameMenu : NetworkBehaviour
{
    public static Action onRestartEvent;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject restart;
    private void Start()
    {
        PlaneController.onLocalPlayerStart += DisableMenu;
        PlaneController.onLocalPlayerKilled += EnableMenu;
    }

    private void EnableMenu()
    {
        title.SetActive(true);
        exit.SetActive(true);
        if (IsServer)
        {
            restart.SetActive(true);
        }
    }

    public void ExitGame()
    {
        if (IsClient)
        {
            NetworkManager.Singleton.StopClient();   
        }
        if (IsServer)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                NetworkManager.Singleton.DisconnectClient(client.Key);
            }
        }
    }

    public void RestartGame()
    {
        title.GetComponent<TMP_Text>().SetText("GameOver!");
        DisableMenu();
        onRestartEvent?.Invoke();
        RestartGameClientRpc();
    }

    [ClientRpc]
    private void RestartGameClientRpc()
    {
        DisableMenu();
        onRestartEvent?.Invoke();
    }

    private void DisableMenu()
    {
        title.SetActive(false);
        exit.SetActive(false);
        restart.SetActive(false);
    }
}
