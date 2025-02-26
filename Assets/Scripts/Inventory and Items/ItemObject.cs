using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    //[SerializeField] private Vector2 velocity;
    

    private void SetUpItemData()
    {
        if(itemData == null)
            return;
        
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.name;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            rb.velocity = velocity;
    }*/
    // item + velocity
    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        
        SetUpItemData();
    }
    
    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            AudioManager.instance.PlaySFx(0, transform);
            rb.velocity = new Vector2(0, 8);
            PlayerManager.instance.player.fx.CreatePopUpText("Bag Not Enough");
            return;
        }
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
