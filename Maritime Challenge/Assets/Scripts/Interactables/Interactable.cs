using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private float InteractRadius = 3.0f;


    private Player myPlayer = null;

    protected bool in_range = false;
    protected static List<Interactable> inRangeList = new List<Interactable>();


    void Start()
    {
        StartCoroutine(InteractableInits());
    }

    IEnumerator InteractableInits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;

        myPlayer = PlayerData.MyPlayer;
    }

    void Update()
    {
        if (in_range && UIManager.Instance.IsInteractButtonClicked())
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enable Interact Button
            // ...

            in_range = true;
            inRangeList.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            in_range = false;
            inRangeList.Remove(this);
            if (inRangeList.Count == 0)
            {

                // Disable Interact Button if no others
            }
        }
    }
}
