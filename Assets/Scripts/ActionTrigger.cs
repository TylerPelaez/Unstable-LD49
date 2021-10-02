using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTrigger : MonoBehaviour
{
    // TODO: implement prefab to create active LifeCycleManager to Win

    [Tooltip("If this trigger touch this tag, will invoke event")]
    public string TAG_that_active = "Player";

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;
    public UnityEvent OnTriggerStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TAG_that_active)
            OnTriggerEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TAG_that_active)
            OnTriggerExit.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == TAG_that_active)
            OnTriggerStay.Invoke();
    }
}
