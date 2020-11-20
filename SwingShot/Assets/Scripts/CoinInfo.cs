using UnityEngine;

public class CoinInfo : MonoBehaviour
{
    public int CoinIdx { get; set; }

    private void Awake()
    {
        var lm = FindObjectOfType<LevelManager>();
        CoinIdx = lm.CoinCount++;
    }
}
