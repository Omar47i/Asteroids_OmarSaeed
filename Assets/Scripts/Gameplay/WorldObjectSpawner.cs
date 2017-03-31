
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<WorldObject> m_EnemyShips;

    [SerializeField]
    private List<WorldObject> m_Asteroids;

    [SerializeField]
    private List<WorldObject> m_Pickups;

    [SerializeField]
    private float m_StartDelay = 1f;

    [SerializeField]
    private float m_DurationBetweenGroups = 2f;

    [SerializeField]
    private float m_DurationBetweenWaves = 2.5f;

    private Transform m_PlayerShipTransform;
    private SpawningPoints m_SpawningPoints;      // this script holds all the spawning points
    private WorldObjectManager m_WaveController; // this script manages the generation formation of each wave
    private bool m_StopSpawning = false;
    void Start()
    {
        m_PlayerShipTransform = GameObject.FindGameObjectWithTag(Tags.Player).transform;

        m_SpawningPoints = GetComponent<SpawningPoints>();
        m_WaveController = GetComponent<WorldObjectManager>();

        // .. Listen to the game over event to stop spawning more objects
        GameManager.Instance.m_GameOverEvent.AddListener(OnGameOver);

        StartCoroutine(SpawnAfter());
    }

    IEnumerator SpawnAfter()
    {
        yield return new WaitForSeconds(m_StartDelay);

        StartCoroutine(SpawnWorldObjects());
    }


    private IEnumerator SpawnWorldObjects()
    {
        // .. For every wave group:
        for (int i = 0; i < m_WaveController.m_WorldGroup.Count; i++)
        {
            // .. Get this world object group and instantiate all its members
            NPCGroup group = m_WaveController.m_WorldGroup[i];

            // .. For every object in the world group, instantiate it at he world
            for (int j = 0; j < group.ObjectType.Count; j++)
            {
                // .. If player dies, break out of the coroutine
                if (m_StopSpawning)
                {
                    yield break;
                }

                // .. Create the NPC object
                if (group.ObjectType[j].m_Type == WorldObjectType.Asteroid)
                {
                    GameObject randomAsteroid = m_Asteroids[Random.Range(0, 4)].prefab;

                    GameObject worldObject = GameObject.Instantiate(randomAsteroid,
                    group.ObjectType[j].m_Position, group.ObjectType[j].m_Rotation);

                    worldObject.GetComponent<AsteroidController>().
                        SetAsteroid(Random.Range(m_WaveController.m_MinAsteroidSpeed, m_WaveController.m_MaxAsteroidSpeed),
                        Random.Range(m_WaveController.m_MinAsteroidTurnSpeed, m_WaveController.m_MaxAsteroidTurnSpeed),
                        group.ObjectType[j].m_Rotation,
                        Random.Range(m_WaveController.m_MinAsteroidHP, m_WaveController.m_MaxAsteroidHP));
                }
                else if (group.ObjectType[j].m_Type == WorldObjectType.EnemyShip)
                {
                    GameObject worldObject = GameObject.Instantiate(m_EnemyShips[0].prefab,
                    group.ObjectType[j].m_Position, group.ObjectType[j].m_Rotation);

                    worldObject.GetComponent<EnemyShipController>().
                        SetShip(Random.Range(m_WaveController.m_MinShipSpeed, m_WaveController.m_MaxShipSpeed),
                        group.ObjectType[j].m_Rotation,
                        1,
                        m_PlayerShipTransform);
                }
                else if (group.ObjectType[j].m_Type == WorldObjectType.WeaponUpgrade)
                {
                    GameObject worldObject = GameObject.Instantiate(m_Pickups[0].prefab,
                    group.ObjectType[j].m_Position, Quaternion.identity);

                    worldObject.GetComponent<CannonUpgradeController>().SetPickup(300f, group.ObjectType[j].m_Rotation);
                }

                // .. wait between each object spawn
                yield return new WaitForSeconds(group.ObjectType[j].m_NextSpawnDuration);
            }

            // .. wait between each group spawn
            yield return new WaitForSeconds(m_DurationBetweenGroups);
        }

        yield return new WaitForSeconds(m_DurationBetweenWaves);

        // .. Increase wave number by one
        m_WaveController.m_WaveNumber++;

        StartCoroutine(SpawnWorldObjects());
    }

    /// <summary>
    /// Called when player gets out of lives
    /// </summary>
    private void OnGameOver()
    {
        // .. Stop creating NPCs
        m_StopSpawning = true;
    }
}

[System.Serializable]
class WorldObject
{
    public WorldObjectType type;
    public GameObject prefab;
}

public enum WorldObjectType
{
    EnemyShip,
    Asteroid,
    WeaponUpgrade,
}