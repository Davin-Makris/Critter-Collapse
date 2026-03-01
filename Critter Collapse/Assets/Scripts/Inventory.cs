using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class Inventory : NetworkBehaviour
{   // you can only have one thing in this inventoy at a time
    public bool inventoryFull = false;
    //NOTE: REMOVE STATIC FROM HERE AND TILE.CS
    public GameObject focusedOn; // a ref to the obj the player this inventory is attached to is focused on 
    public GameObject inInventory; // a ref to the game obj currently in our inventory
    private NetworkVariable<ulong> inInventoryNOID = new NetworkVariable<ulong>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); // the obj currently in the inventory if it's networked
    [SerializeField] GameObject inventory; // this player's inventory

    void Start()
    {
        focusedOn = gameObject.GetComponent<PlayerInteractor>().focusedOnGameObj;
    }

    // on awake: find game object inventory and set it to the inventory var for this script
    void Awake()
    {
         inventory = gameObject.transform.Find("Inventory").gameObject; // get the inventory game object on this player object and set it
         //inventory.GetComponent<NetworkObject>().Spawn(); // spawn the inventory network object
         //AddToPlayerServerRpc(inventory.GetComponent<NetworkObject>().NetworkObjectId); // set the inventory back to the parent object
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            gameObject.GetComponent<PlayerInput>().enabled = false;
        }
        if (IsServer && IsOwner)
        {
            gameObject.name = "Player Server";
        }
        else
        {
            gameObject.name = "Player Client";
        }
    }

    public void addToInventory()
    {
        //inInventory = InteractableObject.lastObject; // update our inInventory to the last object we interacted with
        focusedOn = gameObject.GetComponent<PlayerInteractor>().focusedOnGameObj;
        inInventory = focusedOn; // update out inventory with the last obj we interacted with (or focused on)
        NetworkObject inInventoryNO;
        if (inventoryFull) // if we already have something in the inventory, drop it
        {
            // if we're working with a networked object
            if (inInventory.TryGetComponent<NetworkObject>(out inInventoryNO))
            {
                if (!IsOwner) { return; }
                Debug.Log(gameObject.name + " is removing Networked object");

                DropItemServerRPC(inInventoryNO.NetworkObjectId);
                inventoryFull = false;
                //inInventoryNO.transform.position = gameObject.transform.position; // on drop, set the pos of the object to the player's pos
            }
            // otherwise
            else
            {
                Debug.Log(gameObject.name + " is removing regular object");
                inInventory.transform.SetParent(null); // drop the object
            }
            inInventory = null;
            inventoryFull = false;
        }
        else // otherwise pick it up
        {
            // if this is a networked object
            
            if (inInventory.TryGetComponent<NetworkObject>(out inInventoryNO)) // GetComponentInParent
            {
                inInventoryNOID.Value = inInventoryNO.NetworkObjectId;
                if (!IsOwner) { return; }
                Debug.Log(gameObject.name + " is trying to pick up network Object");
                ulong senderID = gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                ulong itemID = focusedOn.GetComponent<NetworkObject>().NetworkObjectId;
                PickUpItemServerRPC(senderID, itemID);
                inventoryFull = true;
            }
            // otherwise
            else
            {
                Debug.Log(gameObject.name + " is trying to pick up a NON-Networked Object");
                inInventory.transform.SetParent(inventory.transform); // pickup the last object we interacted with
                inventoryFull = true;
            }
        }
    }

    // if(!IsOwner) return; when setting a focused object

    // only the server has permissions to reparent items, so this does that work on the server side
    // adds the object as a child of the player using the inventory as a holdpoint
    [ServerRpc]
    void PickUpItemServerRPC(ulong senderID, ulong itemID)
    {
        GameObject sender = NetworkManager.Singleton.SpawnManager.SpawnedObjects[senderID].gameObject;
        NetworkObject inventoryItem = NetworkManager.Singleton.SpawnManager.SpawnedObjects[itemID].GetComponent<NetworkObject>();

        Debug.Log("InventoryItem: " + inventoryItem + "\nSender: " + sender); //Inventory Item is Null
        inventoryItem.TrySetParent(sender);
        
    }

    // drops the object server-side
    [ServerRpc]
    void DropItemServerRPC(ulong itemID)
    {
        NetworkObject inventoryItem = NetworkManager.Singleton.SpawnManager.SpawnedObjects[itemID].GetComponent<NetworkObject>();
        //if (!inventoryFull)
            //if (inventoryFull)
        Debug.Log("Try Remove Parent: " + inventoryItem.TryRemoveParent());
        inventoryFull = false;
        
    }
}
