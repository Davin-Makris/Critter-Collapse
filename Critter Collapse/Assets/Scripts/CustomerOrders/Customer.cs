using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //FLOWER KEY (how many tiles they take up):
    // Chocolate Cosmos FLower: 6
    // Firework Flower: 12
    // Large Lily: 9
    // Lily of the Valley: 7
    // Lotus: 6
    // Rose: 5
    // Sunflower: 12

    public Dictionary<int, Order> allOrders = new Dictionary<int, Order>(); //a dict of all possible orders
    private Order currentOrder; // a ref to the order we're currently on
    private GameObject orderText; // a ref to the order text in the canvas for updating

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //setUpOrders();
    }

    // sets all the orders with values for plants, # of plants, and ID
    private void setUpOrders()
    {
        Dictionary<string, int> orderOne = new Dictionary<string, int>();

    }

    // gets a random incomplete order from allOrders
    public Order getRandOrder()
    {
        return new Order();
    }

    // updates the orderText in the Canvas
    public void updateOrderText()
    {

    }

    // marks the order with the given ID as complete
    public void completeOrder(int ID)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
