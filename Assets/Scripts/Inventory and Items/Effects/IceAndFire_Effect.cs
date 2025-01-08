using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/IceFire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, enemyPosition.position, player.transform.rotation);
            
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
            
            Destroy(newIceAndFire, 3);
        }
        
    }
}
