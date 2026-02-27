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
        //localPlayerInput.actions.actionMaps
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
