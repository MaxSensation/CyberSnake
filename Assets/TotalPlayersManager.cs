using System;
using MLAPI;
using TMPro;
using UnityEngine;

public class TotalPlayersManager : MonoBehaviour
{
    public static Action onPlayerConnect;
    [SerializeField] private TMP_Text amountText;

    private void Start()
    {
        UpdateText(0);
        NetworkManager.Singleton.OnClientConnectedCallback += UpdateText;
    }

    private void UpdateText(ulong playerID)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            amountText.SetText(NetworkManager.Singleton.ConnectedClients.Count.ToString());
        } else if (NetworkManager.Singleton.IsClient)
        {
            amountText.SetText((NetworkManager.Singleton.ConnectedClients.Count + 1).ToString());
        }
        if(NetworkManager.Singleton.ConnectedClients.Count > 0) onPlayerConnect?.Invoke();
    }
}
