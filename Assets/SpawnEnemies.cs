using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    public float spawnPadding = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval) //Spawn stuff
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Camera cam = Camera.main;

        float halfHeight = cam.orthographicSize; // Gets the camera's height
        float halfWidth = halfHeight * cam.aspect; // Gets the camera's width using height and aspect (idk why but apperently u do ts)

        float left = cam.transform.position.x - halfWidth; // Position - width
        float right = cam.transform.position.x + halfWidth; //Position + width
        float top = cam.transform.position.y + halfHeight; //Position - height
        float bottom = cam.transform.position.y - halfHeight; // Position + height

        int side = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        if (side == 0) // left
        {
            spawnPos.x = left - spawnPadding;
            spawnPos.y = Random.Range(bottom, top);
        }
        else if (side == 1) // right
        {
            spawnPos.x = right + spawnPadding;
            spawnPos.y = Random.Range(bottom, top);
        }
        else if (side == 2) // top
        {
            spawnPos.y = top + spawnPadding;
            spawnPos.x = Random.Range(left, right);
        }
        else // bottom
        {
            spawnPos.y = bottom - spawnPadding;
            spawnPos.x = Random.Range(left, right);
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
