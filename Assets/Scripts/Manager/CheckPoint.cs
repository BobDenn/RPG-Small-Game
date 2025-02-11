using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool status;

    private void Awake()
    {
        
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate check point id")]
    private void GenerateId()
    {
        id = Guid.NewGuid().ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
            ActivateCheckPoint();
    }

    public void ActivateCheckPoint()
    {
        AudioManager.instance.PlaySFx(12,null);
        status = true;
        anim.SetBool("Active", true);
    }
}
