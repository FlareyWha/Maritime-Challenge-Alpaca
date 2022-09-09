using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviourSingleton<PlayerFollowCamera>
{
    private Camera cam;
    private GameObject followTarget;

    private bool in_anim = false;
    private float defaultOrthoSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultOrthoSize = cam.orthographicSize;
    }

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

    public void ZoomCameraInOut(float orthoDis, float anim_time = 1.0f)
    {
        StartCoroutine(ZoomCameraAnim(cam.orthographicSize, orthoDis, anim_time));
    }

    public void ResetCameraZoom(float anim_time = 1.0f)
    {
        StartCoroutine(ZoomCameraAnim(cam.orthographicSize, defaultOrthoSize, anim_time));
    }

    IEnumerator ZoomCameraAnim(float startSize, float endSize, float anim_time)
    {
        cam.orthographicSize = startSize;

        float zoom_rate = (endSize - startSize) / anim_time;

        float timer = anim_time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cam.orthographicSize += zoom_rate * Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = endSize;


    }
}
