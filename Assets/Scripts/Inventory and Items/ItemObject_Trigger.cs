using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{

    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Once touched items, then it disappeared
        if (collision.GetComponent<Player>() != null)
        {
            myItemObject.PickUpItem();
        }
        
    }
}
