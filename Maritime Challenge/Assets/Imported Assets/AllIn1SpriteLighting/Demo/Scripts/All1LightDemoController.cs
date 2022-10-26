using UnityEngine;
using UnityEngine.UI;

public class All1LightDemoController : MonoBehaviour
{
    [SerializeField] private All1LightExpositor expositor;
    [SerializeField] private Text expositorsTitle = null;
    [SerializeField] private Transform expositorCam;
    [SerializeField] private float camZ;

    void Start()
    {
        SetExpositorText();
        expositorCam.position = new Vector3(0f, 0f, camZ);
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            expositor.ChangeTarget(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            expositor.ChangeTarget(1);
        }
    }

    private void SetExpositorText()
    {
        expositorsTitle.text = expositor.name;
    }
}