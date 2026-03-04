using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro; 

public class Customer : NetworkBehaviour
{
    //FLOWER KEY (how many tiles they take up):
    // Chocolate Cosmos: 6
    // Firework: 12
    // Forget me not: 5
    // Large Lily: 9
    // Lily of the Valley: 7
    // Lotus: 6
    // Rose: 5
    // Sunflower: 12

    public Dictionary<int, Order> allOrders = new Dictionary<int, Order>(); //a dict of all possible orders
    private Order currentOrder; // a ref to the order we're currently on
    [SerializeField] public TMP_Text orderText; // a ref to the order text in the canvas for updating
    private NetworkVariable<FixedString512Bytes> textToSet = new NetworkVariable<FixedString512Bytes>(); // string of the text to set to the order text
    //"not null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server

    // used to spawn plants after harvesting them
    // represents the number of plants we have already spawned
    Dictionary<string, int> hasSpawned = new Dictionary<string, int>();
    [SerializeField] public GameObject plantPrefab; // ref to the plant prefab to instantiate and update sprites

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!IsSpawned)
        {
            Debug.Log("I'M NOT SPAWNED SOB SOB SOB");
        }
        setUpOrders();
        orderText.text = "Testing";

        // set up hasSpawned
        hasSpawned.Add("Chocolate Cosmos", 0);
        hasSpawned.Add("Fireworks", 0);
        hasSpawned.Add("ForgetMeNots", 0);
        hasSpawned.Add("LargeLilies", 0);
        hasSpawned.Add("LilyOfTheValleys", 0);
        hasSpawned.Add("Lotus", 0);
        hasSpawned.Add("Roses", 0);
        hasSpawned.Add("Sunflowers", 0);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    // sets all the orders with values for plants, # of plants, and ID
    private void setUpOrders()
    {
        List<Order> tempOrders = new List<Order>();

        Order orderOne = setOneOrder(1, 0, 2, 1, 1, 1, 1, 1);
        Order orderTwo = setOneOrder(0, 1, 1, 0, 1, 2, 0, 0);
        Order orderThree = setOneOrder(0, 0, 2, 1, 2, 1, 0, 0);
        Order orderFour = setOneOrder(1, 0, 1, 1, 0, 2, 2, 1);

        tempOrders.Add(orderOne);
        tempOrders.Add(orderTwo);
        tempOrders.Add(orderThree);
        tempOrders.Add(orderFour);

        // set all unique int IDs for the orders & add to the dict allOrders for later
        for (int x = 0; x < tempOrders.Count; x++)
        {
            tempOrders[x].setID(x); // set the ID
            allOrders.Add(x, tempOrders[x]); // add the order to the dict using the ID as the key
        }
    }

    // sets and returns ONE Order given the param values
    // params stand for the number of flowers of each type, if there are none the number must be 0, and they must be in order
    // Order is as defined at the top of this script or in alphabetical order
    private Order setOneOrder(int choc, int firework, int forgetmenot, int largeLily, int lilyValley, int lotus, int rose, int sun)
    {
        //Order temp = new Order();
        Order temp = ScriptableObject.CreateInstance<Order>();

        temp.addPlant("Chocolate Cosmos", choc);
        temp.addPlant("Fireworks", firework);
        temp.addPlant("ForgetMeNots", forgetmenot);
        temp.addPlant("LargeLilies", largeLily);
        temp.addPlant("LilyOfTheValleys", lilyValley);
        temp.addPlant("Lotus", lotus);
        temp.addPlant("Roses", rose);
        temp.addPlant("Sunflowers", sun);

        return temp; 
    }

    // gets a random order from the list and updates the order text on New Order button click
    public void NewOrderButtonClick()
    {
        getRandOrderServerRPC();
        //updateOrderText();
    }

    // spawns a plant based on the current order
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void spawnPlantFromOrderServerRPC()
    {
        if (currentOrder == null)
        {
            Debug.Log("Can't spawn plant without an order");
        }

        Debug.Log("Trying to spawn plant");
        foreach (var (plant, count) in hasSpawned)
        {
            Debug.Log("In for loop");
            // if what we have spawned is less than how many we need
            if (count < currentOrder.orderPlants[plant])
            {
                Debug.Log("Spawning 1 of " + plant);
                //spawn one more
                spawnOnePlant(plant);
                hasSpawned[plant] = count + 1; // update the count in hasSpawned
                break; // exit the loop
            }
            else
            {
                Debug.Log("Spawned " + count + " " + plant + " out of " + currentOrder.orderPlants[plant]);
            }
        }
    }

    // spawns one plant using the given key (plant name)
    private void spawnOnePlant(string plantName)
    {
        if (!IsServer) return; // Only the server/host can spawn network objects
        Sprite newSprite = Resources.Load<Sprite>(plantName); // get the plant sprite from assets
        GameObject instance = Instantiate(plantPrefab); // instantiate
        instance.GetComponent<SpriteRenderer>().sprite = newSprite; // set sprite
        instance.GetComponent<NetworkObject>().Spawn(); // Sync across clients
        Debug.Log(plantName + " spawned");
    }

    // completes the current order
    public void CompleteOrderButtonClick()
    {
        // ADD code to delete all flowers in the shipping container
        completeOrder(currentOrder.ID);
    }

    // gets a random incomplete order from allOrders
    // Random.Range(x, y) gives a range from x (inclusinve) to y (exclusive) aka x to y - 1
    //(InvokePermission = RpcInvokePermission.Everyone
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void getRandOrderServerRPC()
    {
        bool orderFound = false; // flag check

        foreach (var (id, order) in allOrders)
        {
            int randomTemp = Random.Range(0, 4); // 0 - # of orders, randomTemp is the order ID
            //orderFound = false;

            // if it's not already completed, we can use it
            if (!allOrders[randomTemp].isComplete)
            {
                currentOrder = allOrders[randomTemp];
                orderFound = true;
                break; // break out of loop since we have what we need
            }
        }

        // if we went though all orders but all are complete
        // either reset all orders to play again or end the game
        if (orderFound is false)
        {
            orderText.text = "Congrats! You completed all orders! The End :)";
            return;
        }

        Debug.Log(currentOrder.getOrderText());
        Debug.Log(textToSet.Value);
        textToSet.Value = currentOrder.getOrderText();
        orderText.text = textToSet.Value.ToString();
    }

    // updates the orderText in the Canvas
    public void updateOrderText()
    {
        Debug.Log(currentOrder.getOrderText());
        Debug.Log(textToSet.Value);
        
    }

    // marks the order with the given ID as complete
    public void completeOrder(int ID)
    {
        allOrders[ID].setAsComplete(true);
    }
}
