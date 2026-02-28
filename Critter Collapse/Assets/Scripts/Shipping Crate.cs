using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class ShippingCrate : NetworkBehaviour
{
    private PlayerInput localPlayerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //References player that owns the script. So client -> Client Player or Server -> Server Player
        localPlayerInput = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInput>();
        
    }

    public void swapPlayerInput()
    {
        Debug.Log("Swapping Player " + NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.name + "'s Input");
        localPlayerInput.SwitchCurrentActionMap("Player Ship Crate");
        Debug.Log("Current Player Input " + localPlayerInput.currentActionMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
