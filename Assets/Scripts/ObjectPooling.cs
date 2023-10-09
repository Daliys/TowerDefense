using System.Collections.Generic;
using Enemies;
using Towers;
using UnityEngine;

/// <summary>
///  Object pooling class. Used to create and manage object pools.
/// </summary>
public class ObjectPooling : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the ObjectPooling class.
    /// </summary>
    public static ObjectPooling instance;

    [SerializeField] private GameObject parent; // parent for all objects in pool 
    [SerializeField] private GameObject[] enemiesPrefabs; // prefabs for enemies
    [SerializeField] private GameObject[] bulletsPrefabs; // prefabs for bullets

    private Dictionary<EnemyType, Queue<GameObject>> _enemiesPool; // pool for enemies keeping them in queues 
    private Dictionary<TowerType, Queue<GameObject>> _bulletsPool; // pool for bullets keeping them in queues

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _enemiesPool = new Dictionary<EnemyType, Queue<GameObject>>();
        _bulletsPool = new Dictionary<TowerType, Queue<GameObject>>();

        // Creating pools for enemies and bullets and adding at least one object to each pool
        foreach (var enemyPrefab in enemiesPrefabs)
        {
            GameObject enemyGm = Instantiate(enemyPrefab, parent.transform);
            Enemy enemy = enemyGm.GetComponent<Enemy>();
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(enemyGm);
            _enemiesPool.Add(enemy.GetEnemyType(), queue);
            enemyGm.SetActive(false);
        }

        foreach (var bulletPrefab in bulletsPrefabs)
        {
            GameObject bulletGm = Instantiate(bulletPrefab, parent.transform);
            Bullet bullet = bulletGm.GetComponent<Bullet>();
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(bulletGm);
            _bulletsPool.Add(bullet.GetBelongTowerType(), queue);
            bulletGm.SetActive(false);
        }
    }

    /// <summary>
    /// Gets an instance of an enemy object from the object pool based on the enemy type.
    /// </summary>
    /// <param name="enemyType">The type of enemy.</param>
    /// <returns>The enemy GameObject from the object pool.</returns>
    public GameObject GetObject(EnemyType enemyType)
    {
        if (_enemiesPool[enemyType].Count == 0)
        {
            CreateObject(enemyType);
        }

        GameObject enemyGameObject = _enemiesPool[enemyType].Dequeue();
        enemyGameObject.SetActive(true);
        return enemyGameObject;
    }

    /// <summary>
    /// Gets an instance of a bullet object from the object pool based on the tower type.
    /// </summary>
    /// <param name="towerType">The type of tower.</param>
    /// <returns>The bullet GameObject from the object pool.</returns>
    public GameObject GetObject(TowerType towerType)
    {
        if (_bulletsPool[towerType].Count == 0)
        {
            CreateObject(towerType);
        }

        GameObject bulletGameObject = _bulletsPool[towerType].Dequeue();
        bulletGameObject.SetActive(true);
        return bulletGameObject;
    }

    /// <summary>
    ///  Creates a bullet object and adds it to the object pool.
    /// </summary>
    /// <param name="towerType"> The type of tower.</param>
    private void CreateObject(TowerType towerType)
    {
        foreach (var bulletsPrefab in bulletsPrefabs)
        {
            if (bulletsPrefab.GetComponent<Bullet>().GetBelongTowerType() != towerType) continue;

            GameObject bulletGm = Instantiate(bulletsPrefab, parent.transform);
            _bulletsPool[towerType].Enqueue(bulletGm);
            return;
        }
    }

    /// <summary>
    ///  Creates an enemy object and adds it to the object pool.
    /// </summary>
    /// <param name="enemyType"> The type of enemy.</param>
    private void CreateObject(EnemyType enemyType)
    {
        foreach (var enemyPrefab in enemiesPrefabs)
        {
            if (enemyPrefab.GetComponent<Enemy>().GetEnemyType() != enemyType) continue;

            GameObject enemyGm = Instantiate(enemyPrefab, parent.transform);
            _enemiesPool[enemyType].Enqueue(enemyGm);
            return;
        }
    }

    /// <summary>
    /// Returns an enemy object to the object pool.
    /// </summary>
    /// <param name="enemy">The enemy object to be returned to the pool.</param>
    public void ReturnToPool(Enemy enemy)
    {
        if (!_enemiesPool.ContainsKey(enemy.GetEnemyType()))
        {
            throw new System.Exception("No such enemy type in pool");
        }

        _enemiesPool[enemy.GetEnemyType()].Enqueue(enemy.gameObject);
        enemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns a bullet object to the object pool.
    /// </summary>
    /// <param name="bullet">The bullet object to be returned to the pool.</param>
    public void ReturnToPool(Bullet bullet)
    {
        if (!_bulletsPool.ContainsKey(bullet.GetBelongTowerType()))
        {
            throw new System.Exception("No such bullet type in pool");
        }

        _bulletsPool[bullet.GetBelongTowerType()].Enqueue(bullet.gameObject);
        bullet.gameObject.SetActive(false);
    }
}