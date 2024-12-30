using UnityEngine;

public class ItemObject : MonoBehaviour
{
    
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Once touched items, then it disappeared
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
