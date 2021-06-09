using MLAPI.Messaging;

public interface IPickable
{
    public void PickupServerRpc(ulong playerId, ServerRpcParams serverRpcParams = default);
}