using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    public GameObject coinPrefab;
    public float force = 0.5f;

    public static CoinDropper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DropCoins(Vector3 position)
    {
        for (int i = 0; i < Random.Range(5,10); i++)
        {
            GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;
            coin.GetComponent<Rigidbody>().AddForce(randomDirection * force, ForceMode.Impulse);
        }

        
    }
}
