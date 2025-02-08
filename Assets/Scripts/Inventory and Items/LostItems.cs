using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostItems : MonoBehaviour
{
    public int remainingSouls;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.souls += remainingSouls;
            Destroy(this.gameObject);
        }
    }
}
