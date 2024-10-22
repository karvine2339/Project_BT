using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    public GameObject coinPrefab;
    public float force = 0.5f;
    public float upwardForce = 2.0f; // 위쪽으로의 추가적인 힘

    public static CoinDropper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DropCoins(Vector3 position)
    {
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

            float angle1 = Random.Range(0, Mathf.PI * 2); 
            float angle2 = Random.Range(0, Mathf.PI / 2); 

            Vector3 randomDirection = new Vector3(
                Mathf.Cos(angle1) * Mathf.Sin(angle2),
                Mathf.Sin(angle2),
                Mathf.Sin(angle1) * Mathf.Sin(angle2)
            );

            coin.GetComponent<Rigidbody>().AddForce(randomDirection * force, ForceMode.VelocityChange);
        }
    }
}
