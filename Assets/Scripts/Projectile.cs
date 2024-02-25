using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public NetworkVariable<Vector2> velocity = new(Vector2.one, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _shootForce = 3f;
    
    private Collision2D _collisionInteraction;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _rigidbody2D.velocity = velocity.Value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsOwner)
        {
            return;
        }
        _collisionInteraction = collision;
        DespawnProjectileServerRpc();
    }

     private void DespawnProjectile(Collision2D collisionInteraction)
     {
         if (collisionInteraction.gameObject.CompareTag("Player"))
         {
             collisionInteraction.gameObject.GetComponent<PlayerHealth>().TakeDamage();
         }

        gameObject.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

     [ServerRpc]
     public void DespawnProjectileServerRpc(ServerRpcParams serverRpcParams = default)
     {
         var clientId = serverRpcParams.Receive.SenderClientId;
         if (NetworkManager.ConnectedClients.ContainsKey(clientId))
         {
             var client = NetworkManager.ConnectedClients[clientId];
             DespawnProjectile(_collisionInteraction);
         }
     }
     
     public void SetVelocity(Vector2 direction)
     {
         Debug.Log($"{direction}");
         velocity.Value = direction * _shootForce;
     }
}
