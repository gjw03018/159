using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * speed* Time.deltaTime, -10, 10),
                    transform.position.y,
                    transform.position.z);
            }
        }
    }
}
