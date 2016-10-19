using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    //this will be handled by game / score manager (won't need this variable)

    private int CURRENT_LEVEL= 1;
    [Tooltip("Array of enemies spawned here per level")]
    public int[] m_enemiesPerLevel;
    [Tooltip("If current level > m_enemies.Count then\n enemiesSpawned = (int)(level* multiplier * highestEnemyCount)")]
    public float m_multiplier;
    public GameObject m_EnemyPrefab;

    private EnemyBase[] m_enemies;
	// Use this for initialization

    public void spawn()
    {
        int numEnemies = 0;
        if (CURRENT_LEVEL >= m_enemiesPerLevel.Length)
        {
            numEnemies = (int)(CURRENT_LEVEL * m_multiplier);
            if (m_enemiesPerLevel.Length > 0)
            {
                numEnemies *= m_enemiesPerLevel[m_enemiesPerLevel.Length - 1];
            }
        }
        else
        {
            numEnemies = m_enemiesPerLevel[CURRENT_LEVEL];
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
