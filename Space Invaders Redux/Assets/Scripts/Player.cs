using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Common variables
    [Header("General")]
    [SerializeField] [Range(1f, 10f)] float movementSpeed = 5f;
    public string dieScene = "GameOver";
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float deathDelay = 1f;

    [Header("Projectile")]
    public GameObject projectile;
    [SerializeField] [Range(1f, 20f)] float projectileSpeed = 6f;
    [SerializeField] [Range(0f, 2f)] float fireInterval = 0.2f;
    [SerializeField] float padding = 1f;

    float horAxis;
    float verAxis;
    bool readyFire = true;
    bool pressedFire = false;
    bool startedDeathCorot = false;
    Camera camera;
    Quaternion projectileRotation;
    private GameObject levelManager;
    private ScoreSystem scoreSystemScript;
    private HealthSystem healthSystem;
    private DamageAnim dmgAnim;

    [Header("SFX")]
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0f, 1f)] float shootSoundVolume = 0.5f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0f, 1f)] float explosionSoundVolume = 0.5f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //finds gameobject with tag "MainCamera"
        camera = Camera.main;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        projectileRotation = Quaternion.Euler(0f, 0f, 180f);
        scoreSystemScript = this.GetComponent<ScoreSystem>();
        healthSystem = GetComponent<HealthSystem>();
        scoreSystemScript.ResetScore();
        dmgAnim = GetComponent<DamageAnim>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        if (healthSystem.GetHealth() <= 0 && !startedDeathCorot)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        HandleInput(); //used double input checking, apparently more accurate?
        Move();
        Fire();
    }

    private void HandleInput()
    {
        horAxis = Input.GetAxisRaw("Horizontal");
        verAxis = Input.GetAxisRaw("Vertical");
        pressedFire = Input.GetButton("Fire1");
    }

    private void Move()
    {
        var deltaX = horAxis * Time.fixedDeltaTime;
        var addX = (deltaX * movementSpeed);

        var deltaY = verAxis * Time.fixedDeltaTime;
        var addY = (deltaY * movementSpeed);

        this.transform.position += new Vector3(addX, addY, 0f);
        RestrictPosition(transform.position.x, transform.position.y);
    }

    private void Fire()
    {
        if (pressedFire && readyFire)
        {
            GameObject proj = Instantiate(projectile, transform.position, projectileRotation) as GameObject;
            proj.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, projectileSpeed, 0f);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

            readyFire = false;
            StartCoroutine(ReloadShot());
        }
    }

    IEnumerator ReloadShot()
    {
        yield return new WaitForSeconds(fireInterval);
        readyFire = true;
    }

    private void RestrictPosition(float x, float y)
    {
        //distance of the point from the camera
        float d = Mathf.Abs(camera.transform.position.z - transform.position.z);
        float westB = camera.ViewportToWorldPoint(new Vector3(0f, 0.5f, d)).x + padding;
        float eastB = camera.ViewportToWorldPoint(new Vector3(1f, 0.5f, d)).x - padding;
        float northB = camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, d)).y - padding;
        float southB = camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, d)).y + padding;
        this.transform.position = new Vector3(Mathf.Clamp(x, westB, eastB),
                                              Mathf.Clamp(y, southB, northB),
                                              0f);
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
            healthSystem.TakeDamage(damageDealer.GetDamage());
        }
    }

    private void Die()
    {
        StartCoroutine(CorotDeath());
    }

    IEnumerator CorotDeath()
    {
        //disable movement, visibility & shooting
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        projectile = null;

        startedDeathCorot = true;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionSoundVolume);

        yield return new WaitForSeconds(deathDelay);

        Destroy(this.gameObject);
        if (levelManager)
        {
            levelManager.GetComponent<LevelManager>().LoadScene(dieScene);
        }
    }

}

 
