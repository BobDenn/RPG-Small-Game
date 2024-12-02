using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxGround : MonoBehaviour
{
    private GameObject _cam;

    [SerializeField] private float parallaxEffect;

    private float _xPosition;
    private float _length;
    
    void Start()
    {
        _cam = GameObject.Find("Main Camera");

        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _xPosition = transform.position.x;
    }

    
    void Update()
    {
        float distanceMoved = _cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = _cam.transform.position.x * parallaxEffect;
        
        transform.position = new Vector3(_xPosition + distanceToMove, transform.position.y);

        
        if (distanceMoved < _xPosition - _length)
            _xPosition += _length;
        else if (distanceMoved < _xPosition - _length)
            _xPosition -= _length;
    }
}
