using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour {

    [HideInInspector]
    public int m_WaveNumber = 1;        // Current number of wave, start with zero
    private int m_GroupCount = 8;       // Each wave has 8 groups to spawn at the start 
    private int m_ObjectCount = 5;      // Each group has 5 object at the start

    // Number of NPC objects per group
    private int m_MinObjectCount = 4;
    private int m_MaxObjectCount = 12;

    // Number of groups per wave
    private int m_MinGroupCount = 8;
    private int m_MaxGroupCount = 9;

    // Duration between each group instantiation
    private float m_MinDurationBetweenGroups = 3f; 
    private float m_MaxDurationBetweenGroups = 7f;

    // Duration between each object instantiation in a group
    private float m_MinDurationBetweenObjects = .15f;
    private float m_MaxDurationBetweenObjects = 2f;

    public float m_MinAsteroidSpeed = 100f;
    public float m_MaxAsteroidSpeed = 400f;

    public float m_MinAsteroidTurnSpeed = 35f;
    public float m_MaxAsteroidTurnSpeed = 120f;

    public int m_MinAsteroidHP = 1;
    public int m_MaxAsteroidHP = 3;

    public float m_MinShipSpeed = 800f;
    public float m_MaxShipSpeed = 2300f;

    private int m_ChangePointSideRate = 2;     // every 2 spawned objects change the points size

    private int m_AsteroidsRatio = 80;  // ratio of spawning an enemy asteroid relative to the enemy spacehip 80% to 20%
    private int m_PickupRatio = 5;      // Spawn pickup probability
    private bool m_SpawnPickupOneTime = false;

    // .. This is the final formation that will be instanitated at a wave
    [HideInInspector]
    public List<NPCGroup> m_WorldGroup = new List<NPCGroup>();

    private SpawningPoints m_SpawningPointsScript;

    void Start()
    {
        m_SpawningPointsScript = GetComponent<SpawningPoints>();

        // .. When player consumes the pickup, don't spawn it again
        GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerShipUpgrader>().m_ShipUpgradeEvent.AddListener(OnShipUpgraded);

        // .. Generate a world objects formation at the start of the game
        GenerateFormation();
    }

    public void GenerateFormation()
    {
        m_WorldGroup.Clear();       // Clear the old world group before creating a new one

        m_GroupCount = Random.Range(m_MinGroupCount, m_MaxGroupCount);
        m_ObjectCount = Random.Range(m_MinObjectCount, m_MaxObjectCount);

        for (int i = 0; i < m_GroupCount; i++)
        {
            // .. Create a spawning group
            NPCGroup g = new NPCGroup();

            // .. Select a random number between 0 and 3 which represents the top/bottom/right/and left point groups
            int randomPointsSide = Random.Range(0, 4);

            // .. Select a random number between 0 and 5 from the different 5 spawning points
            int randomPointsPos = Random.Range(0, 5);

            // .. Create number of objects in that spawning group
            for (int j = 0; j < m_ObjectCount; j++)
            {
                // .. Create a spawning object instance
                NPC obj = new NPC();

                // .. The ratio of spawning an asteroid to space enemyship is 80% to 20%
                bool spawnAsteroid = Random.Range(0, 101) < m_AsteroidsRatio ? true : false;

                if (spawnAsteroid)
                {
                    if (CanSpawnPickup())
                    {
                        obj.m_Type = WorldObjectType.WeaponUpgrade;
                    }
                    else
                    {
                        obj.m_Type = WorldObjectType.Asteroid;
                    }
                }
                else
                {
                    obj.m_Type = WorldObjectType.EnemyShip;
                }

                // .. Set position
                obj.m_Position = m_SpawningPointsScript.m_SpawningPointGroups[randomPointsSide][randomPointsPos].position;

                // .. Set rotation
                obj.m_Rotation = GetRandomAngle(randomPointsSide);

                // .. Set next object spawn duration
                obj.m_NextSpawnDuration = Random.Range(m_MinDurationBetweenObjects, m_MaxDurationBetweenObjects);

                // .. Add the configured spawning world object
                g.ObjectType.Add(obj);

                // .. Change the side rate based on number of objects in the group
                ChangeSideRate();

                // .. Get a new spawning point 
                randomPointsPos = GetRandomSpawningPointIndex();

                if (m_ChangePointSideRate >= j)
                {
                    randomPointsSide = (randomPointsSide + 1) % 4;
                }
            }

            // .. Change the world objects count for each group
            m_ObjectCount = Random.Range(m_MinObjectCount, m_MaxObjectCount);

            // .. Add this group to the world group
            m_WorldGroup.Add(g);
        }
    }

    private bool CanSpawnPickup()
    {
        if (!m_SpawnPickupOneTime)
        {
            bool spawnPickup = Random.Range(0, 101) < m_PickupRatio ? true : false;

            if (spawnPickup)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// Get a random direction with respect to the side we will spawn at
    /// </summary>
    /// <param name="side">0 = Top, 1 = Bottom, 2 = Right, 3 = Left</param>
    /// <returns>Random rotation</returns>
    private Quaternion GetRandomAngle(int side)
    {
        Quaternion quat = Quaternion.identity;

        switch(side)
        {
            case 0:      // Top side
                {
                    quat = Quaternion.Euler(0f, 0f, Random.Range(140f, 230f));
                }
                break;
            case 1:     // Bottom side
                {
                    quat = Quaternion.Euler(0f, 0f, Random.Range(-40f, 50f));
                }
                break;
            case 2:     // Right side
                {
                    quat = Quaternion.Euler(0f, 0f, Random.Range(50f, 140f));
                }
                break;
            case 3:     // Left side
                {
                    quat = Quaternion.Euler(0f, 0f, Random.Range(-140f, -50f));
                }
                break;
        }

        return quat;
    }

    private void ChangeSideRate()
    {
        if (m_ObjectCount <= 4)
            m_ChangePointSideRate = 1;

        else if (m_ObjectCount <= 8)
            m_ChangePointSideRate = 2;

        else if (m_ObjectCount <= 12)
            m_ChangePointSideRate = 3;
    }

    private int GetRandomSpawningPointIndex()
    {
        int r = 0;

        if (m_ObjectCount <= 8)
        {
            r = (Random.Range(1, r + 1) + r) % 6;
        }
        else
        {
            r = (r + 1) % 6;
        }

        return r;
    }

    private void OnShipUpgraded()
    {
        m_SpawnPickupOneTime = true;
    }
}

public class NPCGroup
{
    public List<NPC> ObjectType = new List<NPC>();
}

public class NPC
{
    public WorldObjectType m_Type;
    public Vector3 m_Position;
    public Quaternion m_Rotation;
    public float m_NextSpawnDuration;
}
