using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _shootForce = 3f;
    private Collision2D collision2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsOwner)
        {
            return;
        }
        collision2D = collision;
        DespawnProjectileServerRpc();
    }

     private void DespawnProjectile(Collision2D collision)
     {
         if (!collision.gameObject.CompareTag("Map"))
         {
             collision.gameObject.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
             collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
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
             DespawnProjectile(collision2D);
         }
     }

     public void SetVelocity(Vector2 direction)
     {
         _rigidbody2D.velocity = direction * _shootForce;
     }
}
