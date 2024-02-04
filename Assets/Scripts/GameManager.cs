using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    
    public void StartGame()
    {
        NetworkManager.Singleton.StartHost();
        _menuCanvas.SetActive(false);
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : "Client";
        Debug.Log(mode);
    }
    
    public void FindGame()
    {
        NetworkManager.Singleton.StartClient();
        _menuCanvas.SetActive(false);
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : "Client";
        Debug.Log(mode);
    }
}
