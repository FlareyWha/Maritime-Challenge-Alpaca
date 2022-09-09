using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviourSingleton<PlayerFollowCamera>
{
    private GameObject followTarget;

  
    void Update()
    {
        if (followTarget != null)
        {
            Vector3 newPos;
            newPos.x = Mathf.Lerp(transform.position.x, followTarget.transform.position.x, 0.1f);
            newPos.y = Mathf.Lerp(transform.position.y, followTarget.transform.position.y, 0.1f);
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }

    public void SetFollowTarget(GameObject entity)
    {
        followTarget = entity;
    }

    public void DetachCamera()
    {
        followTarget = null;
    }
}
