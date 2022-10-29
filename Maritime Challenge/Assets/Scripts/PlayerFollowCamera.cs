using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviourSingleton<PlayerFollowCamera>
{
    private Camera cam;
    private GameObject followTarget;

    //private bool in_anim = false;
    private float defaultOrthoSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultOrthoSize = cam.orthographicSize;
    }

    void FixedUpdate()
    {
        if (followTarget != null)
        {
            Vector3 newPos;
            newPos.x = Mathf.Lerp(transform.position.x, followTarget.transform.position.x, 0.1f);
            newPos.y = Mathf.Lerp(transform.position.y, followTarget.transform.position.y, 0.1f);
            newPos.z = transform.position.z;
            transform.position = newPos;

            ConstraintToWorldBounds();
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
        if (startSize == endSize)
            yield break;


       
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

    public void FlipCamera(float anim_time)
    {
        StartCoroutine(RotateCameraAnim(180.0f, anim_time));
    }

    public void RotateCamera(float theta, float anim_time)
    {
        StartCoroutine(RotateCameraAnim(theta, anim_time));
    }

    IEnumerator RotateCameraAnim(float theta, float anim_time)
    {
        if (cam.transform.rotation.eulerAngles.z == theta)
            yield break;

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

    }

    public void ResetAll(float anim_time)
    {
        StartCoroutine(RotateCameraAnim(0, anim_time));
        ResetCameraZoom(anim_time);
        SetFollowTarget(PlayerData.MyPlayer.gameObject);
    }

    private void ConstraintToWorldBounds()
    {
        Vector3 pos = transform.position;

        float cam_size_x = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        pos.x = Mathf.Clamp(transform.position.x, GameSettings.WORLD_MIN_X + cam_size_x, GameSettings.WORLD_MAX_X - cam_size_x);
        pos.y = Mathf.Clamp(transform.position.y, GameSettings.WORLD_MIN_Y + cam.orthographicSize, GameSettings.WORLD_MAX_Y - cam.orthographicSize);

        transform.position = pos;
    }
}
