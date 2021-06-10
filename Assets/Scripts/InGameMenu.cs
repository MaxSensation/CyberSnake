using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using TMPro;
using UnityEngine;

public class InGameMenu : NetworkBehaviour
{
    public static Action onRestartEvent;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject restart;
    private NetworkVariableInt _alivePlayers = new NetworkVariableInt(new NetworkVariableSettings{WritePermission = NetworkVariablePermission.ServerOnly});
    private bool _alive = true;
    private void Start()
    {
        PlaneController.onPlayerKilled += DecreasePlayers;
        PlaneController.onLocalPlayerStart += DisableMenu;
        PlaneController.onLocalPlayerKilled += EnableMenu;
        PlaneController.onPlayerWon += EnableWinningMenu;
        if (IsServer)
        {
            _alivePlayers.Value = NetworkManager.ConnectedClients.Count;
        }
    }

    private void DecreasePlayers()
    {
        if (IsServer)
        {
            _alivePlayers.Value -= 1;
        }
    }

    private void Update()
    {
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (_alivePlayers.Value == 1 && _alive)
        {
            EnableWinningMenu();
        }
    }

    private void EnableWinningMenu()
    {
        title.GetComponent<TMP_Text>().SetText("Winner!");
        title.SetActive(true);
        exit.SetActive(true);
        if (IsServer)
        {
            restart.SetActive(true);
        }
    }

    private void EnableMenu()
    {
        _alive = false;
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
        if (IsServer)
        {
            _alivePlayers.Value = NetworkManager.ConnectedClients.Count;
        }
        _alive = true;
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
