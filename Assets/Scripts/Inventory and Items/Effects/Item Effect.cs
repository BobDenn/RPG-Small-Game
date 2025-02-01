using UnityEngine;

public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string itemDescription;
    public virtual void ExecuteEffect(Transform enemyPosition)
    {
        Debug.Log("Effect executed !");
    }
}
