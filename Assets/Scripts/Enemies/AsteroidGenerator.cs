using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidGenerator : MonoBehaviour
{
    private const float ASTEROID_MULTIPLIER = .3f;     // asteroid sizes is a multiplier of this value e.g. .4 , .8, and 1.2

    private const int MIN_GENERATED_ASTEROIDS = 1;
    private const int MAX_GENERATED_ASTEROIDS = 3;

    private const float MIN_MOVEMENT_SPEED = 15f;
    private const float MAX_MOVEMENT_SPEED = 200f;

    private const float MIN_TURNING_SPEED = 45f;
    private const float MAX_TURNING_SPEED = 180f;

    public GameObject m_ExplosionPrefabSmall;   // Small explosion
    public GameObject m_ExplosionPrefabMedium;  // Medium explosion
    public GameObject m_ExplosionPrefabLarge;   // Large explosion

    [SerializeField]
    private List<GameObject> m_AsteroidPrefabs;

    private Vector3 m_AdditionalTranslation;          // added to the new asteroid generated to avoid creaintg all asteroids at the same position

    void Start()
    {
        // .. Subscribe to the hit event to be able to generate new small asteroids if HP > 0
        GetComponent<AsteroidController>().m_ObjectHitEvent.AddListener(OnAsteroidHit);
    }

    private void OnAsteroidHit(int newHP)
    {
        GameObject explosion = null;

        // .. If it's a big asteroid, generate random mini asteroids and destroy it after that
        if (newHP > 0)
        {
            int miniAsteroidsCount = 0;
            int miniAsteroidsHP = 0;
            float newScale = 0f;

            // .. Generate 1 to 3 mini asteroids
            if (newHP == 2)
            {
                miniAsteroidsCount = 2;
                miniAsteroidsHP = 2/*Random.Range(1, miniAsteroidsHP + 3)*/;
                newScale = ASTEROID_MULTIPLIER * 2;

                explosion = m_ExplosionPrefabMedium;
            }
            // .. Generate 1 to 2 mini asteroids
            else if (newHP == 1)
            {
                miniAsteroidsCount = 2;
                miniAsteroidsHP = 1;
                newScale = ASTEROID_MULTIPLIER;

                explosion = m_ExplosionPrefabSmall;
            }

            for (int i = 0; i < miniAsteroidsCount; i++)
            {
                // .. Give every mini asteroid a different starting angle
                Quaternion startingAngle = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                m_AdditionalTranslation = startingAngle * Vector3.up;
                m_AdditionalTranslation *= Random.Range(30f, 60f);

                // .. Instantiate the mini asteroid
                GameObject randomAsteroid = m_AsteroidPrefabs[Random.Range(0, 4)];
                GameObject miniAsteroid = GameObject.Instantiate(randomAsteroid, transform.position + m_AdditionalTranslation, startingAngle);

                // .. Set the mini asteroids scale based on their assigned HP
                miniAsteroid.transform.localScale = new Vector3(newScale, newScale, newScale);

                // .. Set new mini asteroid properties
                float movementSpeed = Random.Range(MIN_MOVEMENT_SPEED, MAX_MOVEMENT_SPEED);
                float turningSpeed = Random.Range(MIN_TURNING_SPEED, MAX_TURNING_SPEED);
                Quaternion angle = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                miniAsteroid.GetComponent<AsteroidController>().SetAsteroid(movementSpeed, turningSpeed, angle, miniAsteroidsHP, explosion);
            }

            // .. Play sound effect
            SoundManager.Instance.PlaySoundEffect(SoundEffectName.EXPLODE, true);

            // .. After generating new mini asteroids, Destroy the hitted asteroid
            GetComponent<MovingObjectCollision>().Explode();
        }
        else
        {
            // .. Destroy the asteroid if its HP less than or equal 0
            GetComponent<MovingObjectCollision>().Explode();
        }
    }
}
