using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public int ID; // the unique ID of the order
    public Dictionary<string, int> orderPlants = new Dictionary<string, int>(); //a dict of the plants and how many are in the order
    public bool isComplete = false; // if this order has been completed or not

    //returns the text for this order to put in the OrderText field
    public string getOrderText()
    {
        string orderText = "I want:";

        foreach (var (plant, count) in orderPlants)
        {
            if (count == 0) continue;
            string tempText = $" {count} {plant}s,";
            orderText += tempText;
        }
        return orderText;
    }

    public void printOrderText()
    {
        Debug.Log(getOrderText());
    }

    // adds a plant to the dictionary with the type and quantity
    public void addPlant(string type, int quantity)
    {
        orderPlants.Add(type, quantity);
    }
}
