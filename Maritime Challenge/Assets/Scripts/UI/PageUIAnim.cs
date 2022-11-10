using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIAnim : MonoBehaviour
{
    [SerializeField]
    private RectTransform RefRectTransform;

    [SerializeField]
    private float AnimTime = 0.2f;

   

    public void FlyInFromLeft()
    {
        UIManager.Instance.ToggleFlyInAnim(RefRectTransform, gameObject, Vector2.left, Vector3.zero, AnimTime, null);
    }
    public void FlyInFromRight()
    {
        UIManager.Instance.ToggleFlyInAnim(RefRectTransform, gameObject, Vector2.right, Vector3.zero, AnimTime, null);
    }

    public void FlyInFromAbove()
    {
        UIManager.Instance.ToggleFlyInAnim(RefRectTransform, gameObject, Vector2.up, Vector3.zero, AnimTime, null);
    }
    public void FlyInFromBelow()
    {
        UIManager.Instance.ToggleFlyInAnim(RefRectTransform, gameObject, Vector2.down, Vector3.zero, AnimTime, null);
    }

    public void FlyOutToLeft()
    {
        UIManager.Instance.ToggleFlyOutAnim(RefRectTransform, gameObject, Vector2.left, Vector3.zero, AnimTime, null);
    }
    public void FlyOutToRight()
    {
        UIManager.Instance.ToggleFlyOutAnim(RefRectTransform, gameObject, Vector2.right, Vector3.zero, AnimTime, null);
    }

    public void FlyOutToAbove()
    {
        UIManager.Instance.ToggleFlyOutAnim(RefRectTransform, gameObject, Vector2.up, Vector3.zero, AnimTime, null);
    }
    public void FlyOutToBelow()
    {
        UIManager.Instance.ToggleFlyOutAnim(RefRectTransform, gameObject, Vector2.down, Vector3.zero, AnimTime, null);
    }
}
