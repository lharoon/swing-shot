using System.Collections;
using UnityEngine;

/// <summary>
/// Allows object to pivot around anchor
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HookBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2d;

    private Collider2D hookCollider;
    private HingeJoint2D anchor;
    private PlayerInfo playerInfo;

    private void Start()
    {
        hookCollider = GetComponent<Collider2D>();
        playerInfo = GetComponentInParent<PlayerInfo>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(GameControls.fireKey) || Input.GetMouseButtonDown(0))
        {
            hookCollider.enabled = true;
        }
        else if (Input.GetKeyUp(GameControls.fireKey) || Input.GetMouseButtonUp(0))
        {
            if (anchor != null)
                Unhook();
            EnableHookColliderAfterT(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("pivot")) return;

        GetComponent<Collider2D>().enabled = false;

        Hook(collision);
    }

    private void Hook(Collider2D collision)
    {
        anchor = collision.GetComponent<HingeJoint2D>();
        anchor.connectedBody = rb2d;

        playerInfo.IsHooked = true;
    }

    private void Unhook()
    {
        anchor.connectedBody = null;
        anchor = null;

        playerInfo.IsHooked = false;
    }

    /// <summary>
    /// Wait for a short amount of time to prevent player from hooking
    /// quickly onto the same target
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator EnableHookColliderAfterT(float t)
    {
        yield return new WaitForSeconds(t);
        GetComponent<Collider2D>().enabled = true;
    }
}
