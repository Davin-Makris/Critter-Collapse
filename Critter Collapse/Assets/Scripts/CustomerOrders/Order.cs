using System.Collections.Generic;
using UnityEngine;

public class Order : ScriptableObject
{
    public int ID; // the unique ID of the order
    public Dictionary<string, int> orderPlants = new Dictionary<string, int>(); //a dict of the plants and how many are in the order
    public bool isComplete = false; // if this order has been completed or not

    //returns the text for this order to put in the OrderText field
    public string getOrderText()
    {
        string orderText = "I want:";
        int step = 0;

        foreach (var (plant, count) in orderPlants)
        {
            string tempText = "";
            
            Debug.Log(step);

            if (count == 0) continue; // if there are none of this plant, move to the next iteration

            // if we are at the end
            if (step >= orderPlants.Count - 1)
            {
                tempText = $" and {count} {plant}s.";
            }
            else
            {
                tempText = $" {count} {plant}s,";
            }
            orderText += tempText;
            step++;
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

    // sets the ID of this order
    public void setID(int IDnum)
    {
        ID = IDnum;
    }

    //sets this order as complete (True) or incomplete (False)
    public void setAsComplete(bool complete)
    {
        isComplete = complete; 
    }
}
