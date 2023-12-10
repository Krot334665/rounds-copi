using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Spawn();
            }
        }
        public void Spawn()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                transform.position = new Vector3(3, 1, 0);
                Position.Value = transform.position;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            if (IsOwner)
            {
                Position.Value = new Vector3(7, -1.7f, 0);
            }else
            {
                Position.Value = new Vector3(-6, -1.7f, 0);
            }
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}