using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<ushort> health = new (100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private Slider _healthSlider;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            health.OnValueChanged += OnHealthChanged;
        }
    }
    
    private void OnHealthChanged(ushort previousvalue, ushort newvalue)
    {
        _healthSlider.value = newvalue;
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        health.Value -= 10;
    }
}
