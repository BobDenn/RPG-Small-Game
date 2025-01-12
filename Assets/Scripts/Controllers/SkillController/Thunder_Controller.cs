using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Thunder_Controller : MonoBehaviour
{
    [FormerlySerializedAs("targetStatus")] [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;

    private int damage;

    private Animator anim;
    private bool triggered;


    void Start()
    {
        anim =  GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats targetStats)
    {
        damage = _damage;
        this.targetStats = targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetStats)
            return;

        if(triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        // 右转？
        transform.right = transform.position - targetStats.transform.position;

        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
                                        // ?
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;

            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true; // transform animation
            anim.SetTrigger("Hit");

        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }

}
