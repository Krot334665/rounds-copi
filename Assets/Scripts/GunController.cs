using Unity.Netcode;
using UnityEngine;

public class GunController : NetworkBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform playerPos;
    [SerializeField] float spawnForce;
    private GameObject projectile;

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            InstantiateProjectileServerRpc(spawnPos.position);

            SetVelocityServerRpc();
        }
    }

    private void SpawnProjectile(Vector3 position)
    {
        projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile.GetComponent<NetworkObject>().Spawn();
    }

    private void Shoot()
    {
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 weaponPosition = playerPos.position;

            Vector2 direction = (mousePosition - weaponPosition).normalized;
            rb.velocity = direction * spawnForce;
        }
        else
        {
            Debug.Log("erorrr");
        }
    }

    [ServerRpc]
    public void SetVelocityServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            Shoot();
        }
    }
    
    [ServerRpc]
    public void InstantiateProjectileServerRpc(Vector3 position, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            Debug.Log($"pos {spawnPos.position}");
            SpawnProjectile(position);
        }
    }
}
