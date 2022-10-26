using UnityEngine;

public class DemoCamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;
    private float xPos;

    void Start()
    {
        xPos = transform.position.x;
    }

    void LateUpdate()
    {
        xPos = Mathf.Lerp(xPos, target.position.x, followSpeed * Time.deltaTime);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}