using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private EnemySpawner enemySpawner;
    [SerializeField] GameObject explosionPrefab;

    [Header("SFX")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0f, 1f)] float deathSoundVolume = 0.5f;
    [Header("")]
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0f, 1f)] float shootSoundVolume = 0.5f;
    [Header("")]
    [SerializeField] AudioClip explodeSound;
    [SerializeField] [Range(0f, 1f)] float explodeSoundVolume = 0.5f;

    #region Common variables
    [Header("General")]
    [SerializeField] int maxHealth = 1;
    int health = 0;
    [SerializeField] int scoreValue = 10;
    private Player playerScript;
    private ScoreSystem scoreSystemScript;
    private DamageAnim dmgAnim;

    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 6f;
    [SerializeField] float minShootInterval = 0f;
    [SerializeField] float maxShootInterval = 2f;
    #endregion

    void Start()
    {
        health = maxHealth;
        StartCoroutine(Shooting());
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        scoreSystemScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ScoreSystem>();
        dmgAnim = GetComponent<DamageAnim>();
    }

    void Update()
    {
        if (health <= 0) { Die(); }
    }

    public void SetEnemySpawner(EnemySpawner e)
    {
        enemySpawner = e;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other.gameObject);
        dmgAnim.AnimateDamage();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckCollision(other.gameObject);
        dmgAnim.AnimateDamage();
    }

    private void CheckCollision(GameObject obj)
    {
        var damageDealer = obj.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
        {
            health -= damageDealer.GetDamage();
        }
    }

    private void Die()
    {
        enemySpawner.DecreaseEnemyCount();
        GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        AudioSource.PlayClipAtPoint(explodeSound, Camera.main.transform.position, explodeSoundVolume);
        scoreSystemScript.IncreaseScore(scoreValue);
        Destroy(gameObject);
    }

    float waitTime;
    IEnumerator Shooting()
    {
        while (true) 
        {
            waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);

            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -projectileSpeed, 0f);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }
}
