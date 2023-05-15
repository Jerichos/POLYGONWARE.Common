using Unity.Netcode;
using UnityEngine;

namespace POLYGONWARE.Common
{
public class Client : NetworkBehaviour
{
    [SerializeField] private Controllable _defaultControllable;
    
    public override void OnNetworkSpawn()
    {
        Debug.Log($"OnNetwork spawn IsServer: {IsServer} IsClient: {IsClient} IsOwner: {IsOwner}");
        if (!IsServer && IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            TestServerRpc(0, NetworkObjectId);
        }

        if (IsClient && IsOwner)
        {
            if(_defaultControllable)
                PlayerController.Local.Control(_defaultControllable);
        }
    }

    [ClientRpc]
    void TestClientRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        if (IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            TestServerRpc(value + 1, sourceNetworkObjectId);
        }
    }

    [ServerRpc]
    void TestServerRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        TestClientRpc(value, sourceNetworkObjectId);
    }
}
}