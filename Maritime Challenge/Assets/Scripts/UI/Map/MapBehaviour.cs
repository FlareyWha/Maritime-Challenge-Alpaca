using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBehaviour : MonoBehaviour
{
    [SerializeField]
    private Camera mapCamera;

    private Vector2 previousTapPoint;

    private float previousDoubleTapDistance;

    private bool isHeld, firstTouch = true, doubleTapFirstTouch = true;

    [SerializeField]
    private float scrollSpeed = 25f;

    [SerializeField]
    private float minCameraZoom = 5f, maxCameraZoom = 300f;

    [SerializeField]
    private Vector2 cameraPositionLowerLimit = new Vector2(-250, -250), cameraPositionUpperLimit = new Vector2(250, 250);

    [SerializeField]
    private RectTransform mapImageRectTransform;

    private float mapImageWidth;
    private float mapImageHeight;

    private void Start()
    {
        mapImageWidth = mapImageRectTransform.rect.width;
        mapImageHeight = mapImageRectTransform.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeMapSize();
    }

    void ChangeMapSize()
    {
        if (!isHeld && InputManager.InputActions.Main.Tap.WasPressedThisFrame())
        {
            isHeld = true;
            firstTouch = true;
            doubleTapFirstTouch = true;
        }
        
        if (isHeld)
        {
            //Find tap point
            Vector2 tapPoint = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();

            if (!CheckTouchWithinBounds(tapPoint))
            {
                isHeld = false;
                return;
            }

            //Do zooming in and out if two taps are detected
            if (InputManager.InputActions.Main.Tap2.IsPressed())
            {
                //Gets the distance between the 2 touch positions
                float distance = Vector2.Distance(tapPoint, InputManager.InputActions.Main.TouchPosition2.ReadValue<Vector2>());

                //Dont account for the first touch as things will break then
                if (!doubleTapFirstTouch)
                {
                    //Change the ortho size of camera based on the difference b/n current and previous distance
                    float deltaDistance = distance - previousDoubleTapDistance;
                    mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize - (deltaDistance * 0.1f), minCameraZoom, maxCameraZoom);
                }
                else
                    doubleTapFirstTouch = false;

                previousDoubleTapDistance = distance;
            }
            //Reset some values when second tap is released
            else if (InputManager.InputActions.Main.Tap2.WasReleasedThisFrame())
            {
                doubleTapFirstTouch = true;
                firstTouch = true;
            }
            //Do moving of the camera if only one tap is detected
            else
            {
                //Dont account for first touch as it might break again
                if (!firstTouch)
                {
                    //Find direction to move camera in
                    Vector2 direction = previousTapPoint - tapPoint;

                    //Move camera
                    mapCamera.transform.position += new Vector3(direction.x, direction.y, 0) * mapCamera.orthographicSize * 0.1f * Time.deltaTime;

                    //Clamp the camera to the lower and upper limits
                    float tempX = Mathf.Clamp(mapCamera.transform.position.x, cameraPositionLowerLimit.x, cameraPositionUpperLimit.x);
                    float tempY = Mathf.Clamp(mapCamera.transform.position.y, cameraPositionLowerLimit.y, cameraPositionUpperLimit.y);
                    mapCamera.transform.position = new Vector3(tempX, tempY, -10);
                }
                else
                    firstTouch = false;

                previousTapPoint = tapPoint;
            }

            if (InputManager.InputActions.Main.Tap.WasReleasedThisFrame())
                isHeld = false;
        }
    }

    bool CheckTouchWithinBounds(Vector2 tapPoint)
    {
        float minX, maxX, minY, maxY;

        minX = mapImageRectTransform.position.x - mapImageWidth * 0.5f;
        maxX = mapImageRectTransform.position.x + mapImageWidth * 0.5f;
        minY = mapImageRectTransform.position.y - mapImageHeight * 0.5f;
        maxY = mapImageRectTransform.position.y + mapImageHeight * 0.5f;

        if (tapPoint.x < maxX && tapPoint.x > minX
            && tapPoint.y > minY && tapPoint.y < maxY)
        {
            return true;
        }

        return false;
    }
}
