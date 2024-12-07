using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float _cloneDuration;

    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _clonePosition)
    {

        // I don't know how to use Instantiate
        GameObject newClone = Instantiate(clonePrefab);

        // clone's position
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, _cloneDuration, canAttack);
    }

}
