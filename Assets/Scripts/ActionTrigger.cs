using UnityEngine;
using UnityEngine.Events;

public class ActionTrigger : MonoBehaviour
{
    // TODO: implement prefab to create active LifeCycleManager to Win

    [Tooltip("If this trigger touch this tag, will invoke event")]
    public string winConditionTag = "Player";

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;
    public UnityEvent OnTriggerStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(winConditionTag))
            OnTriggerEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(winConditionTag))
            OnTriggerExit.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(winConditionTag))
            OnTriggerStay.Invoke();
    }
}
