using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class ShippingCrate : NetworkBehaviour
{
    private PlayerInput localPlayerInput = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void swapPlayerInput()
    {
        if (localPlayerInput == null)
        {
            //References player that owns the script. So client -> Client Player or Server -> Server Player
            localPlayerInput = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInput>();
        }
        Debug.Log("Swapping Player " + NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.name + "'s Input");
        localPlayerInput.SwitchCurrentActionMap("Ship Crate");
        Debug.Log("Current Player Input " + localPlayerInput.currentActionMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
