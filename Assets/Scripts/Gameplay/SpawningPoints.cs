using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoints : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> m_TopSpawningPoints = new List<Transform>();

    [HideInInspector]
    public List<Transform> m_BottomSpawningPoints = new List<Transform>();

    [HideInInspector]
    public List<Transform> m_RightSpawningPoints = new List<Transform>();

    [HideInInspector]
    public List<Transform> m_LeftSpawningPoints = new List<Transform>();

    [HideInInspector]
    public List<List<Transform>> m_SpawningPointGroups = new List<List<Transform>>();

    [HideInInspector]
    public int m_LeftPointsCount;

    [HideInInspector]
    public int m_RightPointsCount;

    [HideInInspector]
    public int m_TopPointsCount;

    [HideInInspector]
    public int m_BottomPointsCount;

    void Awake()
    {
        // initialize spawning points from code to avoid assigning it manually in the inspector
        Transform topParent = transform.GetChild(0);
        Transform bottomParent = transform.GetChild(1);
        Transform rightParent = transform.GetChild(2);
        Transform leftParent = transform.GetChild(3);

        foreach(Transform point in topParent)
        {
            m_TopSpawningPoints.Add(point);
        }

        foreach (Transform point in bottomParent)
        {
            m_BottomSpawningPoints.Add(point);
        }

        foreach (Transform point in rightParent)
        {
            m_RightSpawningPoints.Add(point);
        }

        foreach (Transform point in leftParent)
        {
            m_LeftSpawningPoints.Add(point);
        }

        // .. Add all spawning point groups to a list in order to acces it easily in the EnemyWaveController.cs
        m_SpawningPointGroups.Add(m_TopSpawningPoints);
        m_SpawningPointGroups.Add(m_BottomSpawningPoints);
        m_SpawningPointGroups.Add(m_RightSpawningPoints);
        m_SpawningPointGroups.Add(m_LeftSpawningPoints);

        // .. Cach the count of each spawning points group
        m_TopPointsCount = m_TopSpawningPoints.Count;
        m_BottomPointsCount = m_BottomSpawningPoints.Count;
        m_RightPointsCount = m_RightSpawningPoints.Count;
        m_LeftPointsCount = m_LeftSpawningPoints.Count;
    }
}
