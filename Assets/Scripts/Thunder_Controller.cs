using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStatus targetStatus;
    [SerializeField] private float speed;

    private int damage;

    private Animator anim;
    private bool triggered;


    void Start()
    {
        anim =  GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStatus _targetStatus)
    {
        damage = _damage;
        targetStatus = _targetStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetStatus)
            return;

        if(triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStatus.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStatus.transform.position;

        if(Vector2.Distance(transform.position, targetStatus.transform.position) < .1f)
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
        targetStatus.ApplyShock(true);
        targetStatus.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }

}
