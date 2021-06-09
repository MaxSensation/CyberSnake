using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class InGameMenu : NetworkBehaviour
{
    public static Action onRestartEvent;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject restart;
    private void Start()
    {
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
        NetworkManager.Singleton.StopClient();
    }

    public void RestartGame()
    {
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
