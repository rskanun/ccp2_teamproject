using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("스폰 범위")]
    [SerializeField] 
    private Vector2 _spawnArea;
    public Vector2 SpawnArea
    {
        get { return _spawnArea; }
    }
    public Vector2 Pivot
    {
        get { return transform.position; }
    }

    private int playerCount = 0;
    public bool PlayerInArea
    {
        get { return playerCount > 0; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCount--;
        }
    }
}