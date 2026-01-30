using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    public bool holding = false;
    GameObject closestItem;
    List<Collider2D> itemsInRange = new List<Collider2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float closestDist = 99f;
        foreach (Collider2D collision in itemsInRange)
        {
            //Get distance from us then if dist < closest dist: closestItem = collision.gameObject
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        itemsInRange.Add(collision);
    }

    void onInteract(InputValue value)
    {
        if (!holding)
        {
            closestItem.transform.SetParent(gameObject.transform);
        }
        else
        {
            closestItem.transform.SetParent(null, true);
        }
    }
}
