using Unity.Netcode;
using UnityEngine;

public class GunController : NetworkBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform playerPos;

    private Vector2 _directionProjectile;
    private GameObject _projectile;

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            CalculateDirection();
            InstantiateProjectileServerRpc();
        }
    }

    private void CalculateDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 weaponPosition = playerPos.position;

        Vector2 direction = (mousePosition - weaponPosition).normalized;
        _directionProjectile = direction;
    }

    private void SpawnProjectile()
    {
        _projectile = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
        _projectile.GetComponent<Projectile>().SetVelocity(_directionProjectile);
        _projectile.GetComponent<NetworkObject>().Spawn();
        
    }

    [ServerRpc]
    public void InstantiateProjectileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            SpawnProjectile();
        }
    }
}
