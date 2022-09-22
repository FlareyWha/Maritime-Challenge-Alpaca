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
        if (in_anim)
            return;

        StartCoroutine(ZoomCameraAnim(cam.orthographicSize, orthoDis, anim_time));
    }

    public void ResetCameraZoom(float anim_time = 1.0f)
    {
        if (in_anim)
            return;

        StartCoroutine(ZoomCameraAnim(cam.orthographicSize, defaultOrthoSize, anim_time));
    }

    IEnumerator ZoomCameraAnim(float startSize, float endSize, float anim_time)
    {
        if (startSize == endSize)
            yield break;



        in_anim = true;
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
        in_anim = false;


    }

    public void FlipCamera(float anim_time)
    {
        RotateCameraAnim(180.0f, anim_time);
    }

    IEnumerator RotateCameraAnim(float theta, float anim_time)
    {
        if (cam.transform.rotation.eulerAngles.z == theta)
            yield break;


        in_anim = true;

        float spin_rate = (theta - cam.transform.rotation.eulerAngles.z) / anim_time;

        float timer = anim_time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 
                cam.transform.rotation.eulerAngles.y,
                cam.transform.rotation.eulerAngles.z + (spin_rate * Time.deltaTime));
            yield return null;
        }

        cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 
            cam.transform.rotation.eulerAngles.y, theta);
        in_anim = false;


    }

    public void ResetAll(float anim_time)
    {
        StartCoroutine(RotateCameraAnim(0, anim_time));
        ResetCameraZoom(anim_time);
    }
}
