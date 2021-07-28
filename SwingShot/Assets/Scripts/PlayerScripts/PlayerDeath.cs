using DG.Tweening;
using UnityEngine;

/// <summary>
/// Destroys player on collision
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    public FlatFX explosionWhite, explosionBlack;
    public TrailKiller trail;
    public PlayerInfo playerInfo;
    public Transform camTargetPos;
    public AudioSource deathSound;
    public GameObject crashSoundPrefab;

    private FireInfo fireInfo;
    private bool isDestroyed;

    private float buffer = 100;

    private void Start()
    {
        fireInfo = FindObjectOfType<FireInfo>();
    }

    private void Update()
    {
        //if (trail.startWidth <= 0)
        //    KillPlayer();

        var playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (playerScreenPos.x < 0 - buffer || playerScreenPos.x > Screen.width + buffer ||
            playerScreenPos.y < 0 - buffer || playerScreenPos.y > Screen.height + buffer)
            KillPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("laser") && !isDestroyed)
            KillPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy-laser") && !isDestroyed)
            KillPlayer();

        if (collision.gameObject.CompareTag("debris"))
        {
            playerInfo.Health -= 0.5f;
            //playerInfo.Health -= 0.25f;
            //print("playerInfo.Health: " + playerInfo.Health);
            Destroy(Instantiate(crashSoundPrefab), 2f);
            if (playerInfo.Health <= 0)
                KillPlayer();
        }
    }

    public void KillPlayer()
    {
        playerInfo.Health = 0;

        if (fireInfo != null && fireInfo.IsOverPlayer)
            explosionBlack.AddEffect(transform.position, 2);
        else
            explosionWhite.AddEffect(transform.position, 2);

        //trail.transform.parent = null; // Kills trail
        //trail.KillTrail();

        deathSound.Play();

        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);

        isDestroyed = true;
    }

    //private void HideTrail()
    //{
    //    trail.transform.parent = null;
    //    DOTween.To(() => trail.startColor, x => trail.startColor = x, Color.clear, 1);
    //    DOTween.To(() => trail.endColor, x => trail.endColor = x, Color.clear, 1);
    //}
}
