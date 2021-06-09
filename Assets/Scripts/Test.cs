using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        TestServerRpc();
    }

    [ServerRpc]
    private void TestServerRpc()
    {
        print("Hello Server");
        TestClientRpc();
    }
    
    [ClientRpc]
    private void TestClientRpc()
    {
        if (NetworkManager.Singleton.IsServer) return;
        print("Hello Client");
    }
}