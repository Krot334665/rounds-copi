using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<ushort> health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private Slider _healthSlider;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // if (!IsServer)
        // {
            health.OnValueChanged += OnHealthChanged;
        // }
    }
    
    private void OnHealthChanged(ushort previousvalue, ushort newvalue)
    {
        // Debug.Log($"{previousvalue} and new {newvalue}");
        _healthSlider.value = newvalue;
        if (health.Value <= 0)
        {
            gameObject.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }

    public void TakeDamage()
    {
        health.Value -= 10;
    }
}
