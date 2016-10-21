using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    //this will be handled by game / score manager (won't need this variable)
    [Tooltip("Array of enemies spawned here per level")]
    public int[] m_enemiesPerLevel;
    [Tooltip("If current level > m_enemies.Count then\n enemiesSpawned = (int)(level* multiplier * highestEnemyCount)")]
    public float m_multiplier;
    public int m_maxEnemies;
    public GameObject m_EnemyPrefab;
    private EnemyBase[] m_enemies;
	// Use this for initialization

    public void spawn()
    {
        Debug.Log("spawning dude");
        int numEnemies = 0;
        if (GameManager.currentStage >= m_enemiesPerLevel.Length)
        {
            numEnemies = (int)((GameManager.currentStage +1) * m_multiplier);
            if (m_enemiesPerLevel.Length > 0)
            {
                numEnemies *= m_enemiesPerLevel[m_enemiesPerLevel.Length - 1];
            }
        }
        else
        {
            numEnemies = m_enemiesPerLevel[GameManager.currentStage];
        }

        m_enemies = new EnemyBase[numEnemies];
        for (int i = 0; i < numEnemies; ++i)
        {
            GameObject enemy = Instantiate(m_EnemyPrefab);
            m_enemies[i] = enemy.GetComponent<EnemyBase>();
            enemy.transform.parent = transform;
        }
    }
}
