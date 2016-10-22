using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    //this will be handled by game / score manager (won't need this variable)
    [Tooltip("Array of enemies spawned here per level")]
    public int[] m_enemiesPerLevel;
    [Tooltip("If current level > m_enemies.Count then\n enemiesSpawned = (int)(level* multiplier * highestEnemyCount)")]
    public float m_multiplier;
    public float m_wanderRadius;
    public int m_maxEnemies;
    public GameObject m_EnemyPrefab;
    private TVManz[] m_enemies;
	// Use this for initialization

    public void spawn()
    { 
        int numEnemies = 0;
        if (GameManager.currentStage >= m_enemiesPerLevel.Length)
        {

            //if (m_enemiesPerLevel.Length > 0)
            //{
            //    numEnemies = (int)(m_enemiesPerLevel[m_enemiesPerLevel.Length - 1] + (m_multiplier * (GameManager.currentStage + 1)));
            //}
            //else
            //{
                numEnemies = (int)((GameManager.currentStage + 1) * m_multiplier);
                
            //}
        }
        else
        {
            numEnemies = m_enemiesPerLevel[GameManager.currentStage];
        }

        numEnemies = numEnemies <= m_maxEnemies ? numEnemies : m_maxEnemies;
        
        m_enemies = new TVManz[numEnemies];
        for (int i = 0; i < numEnemies; ++i)
        {
            GameObject enemy = Instantiate(m_EnemyPrefab, transform.position, transform.rotation) as GameObject;
            m_enemies[i] = enemy.GetComponent<TVManz>();
            enemy.transform.parent = transform;
            m_enemies[i].m_wanderRadius = m_wanderRadius;
        }
    }
}
