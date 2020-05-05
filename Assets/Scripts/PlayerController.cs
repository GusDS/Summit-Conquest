using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 250f;
    public float pushPower = 10f;
    public GameObject fxContainer;
    public GameObject powerupIndicator;
    public float powerupTime = 10f;
    public float explosionForce = 500f;
    public float explosionRadius = 3f;
    public AudioClip jumpSound;
    public AudioClip bumpSound;
    public AudioClip powerupCollectSound;
    public AudioClip powerupPushSound;
    public AudioClip powerupExplodeSound;
    public ParticleSystem shockwaveParticle;
    public GameObject enemiesGroup;
    public Rigidbody playerRb;
    public bool isOnGround = true;
    public bool isExploding = false;
    public bool hasPowerupPush = false;
    public bool hasPowerupExplode = false;

    private float forwardInput;
    private float powerupTimeRemain = 0;
    private Color powerupExplodeOn = new Color32(0, 78, 255, 255); // 004EFF blue
    private Color powerupExplodeOff = new Color32(223, 255, 0, 255); // DFFF00 yellow

    private Control control;
    private Renderer playerRenderer;
    private GameObject focalPoint;
    private AudioSource playerAudio;

    void Start()
    {
        control = GameObject.Find("Control").GetComponent<Control>();
        playerRb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        playerRenderer.material.color = powerupExplodeOff;
        focalPoint = GameObject.Find("Camera Perspective");
        playerAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (control.isActionOn)
        {
            if (control.inputKeyboard)
            {
                forwardInput = Input.GetAxis("Vertical");
                playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed, ForceMode.Impulse);
            }

            if (control.inputMouse && Input.GetKey(KeyCode.Mouse0))
            {
                playerRb.AddForce(focalPoint.transform.forward * speed, ForceMode.Impulse);
            }

            if ((control.inputKeyboard && Input.GetKeyDown(KeyCode.Space)) || (control.inputMouse && Input.GetKeyDown(KeyCode.Mouse1))) {
                if (isOnGround)
                {
                    playerAudio.PlayOneShot(jumpSound, control.audioVolume);
                    playerRb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    isOnGround = false;
                }
                else if (hasPowerupExplode)
                {
                    playerRb.velocity = Vector3.zero;
                    playerRb.angularVelocity = Vector3.zero;
                    playerRb.AddForce(Vector3.down * 50, ForceMode.VelocityChange);
                    isExploding = true;
                }
            }
        }

        // If fall
        if (transform.position.y < -8 || transform.position.y > 25)
        {
            transform.position = Vector3.up * 15;
            if (control.livesLeft > 0)
            {
                // loose a life
                control.LivesUpdate(-1);
            }
        }
        // Powerup Indicator follow the player
        fxContainer.transform.position = transform.position;
    }

    IEnumerator PowerupCountdownRoutine(string powerup)
    {
        if (powerup == "PowerupPush")
        {
            powerupTimeRemain = powerupTime;
            while (powerupTimeRemain > 0)
            {
                yield return new WaitForSeconds(0.5f);
                powerupTimeRemain -= 0.5f;
            }
            hasPowerupPush = false;
            powerupIndicator.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPowerupExplode && !isOnGround && isExploding)
        {
            // PowerupExplode explosion
            playerAudio.PlayOneShot(powerupExplodeSound, control.audioVolume);
            shockwaveParticle.Play();
            playerRb.isKinematic = true;
            Rigidbody[] allEnemiesRb = enemiesGroup.GetComponentsInChildren<Rigidbody>();
            if (allEnemiesRb.Length > 0) {
                foreach (var enemyRb in allEnemiesRb) {
                    enemyRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
            playerRb.isKinematic = false;
            hasPowerupExplode = false;
            playerRenderer.material.color = powerupExplodeOff;
            isOnGround = true;
            isExploding = false;
        }
        else
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (hasPowerupPush) // PowerupPush push
                {
                    Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                    Vector3 pushBack = collision.gameObject.transform.position - transform.position;
                    playerAudio.PlayOneShot(powerupPushSound, control.audioVolume);
                    enemyRb.AddForce(pushBack * pushPower, ForceMode.Impulse);
                }
                else
                {
                    playerAudio.PlayOneShot(bumpSound, control.audioVolume);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerupPush"))
        {
            if (hasPowerupPush)
                powerupTimeRemain = powerupTime; // Powerup is active, only refresh time
            else {
                hasPowerupPush = true;
                StartCoroutine(PowerupCountdownRoutine(other.tag));
            }
            powerupIndicator.gameObject.SetActive(true);
            playerAudio.PlayOneShot(powerupCollectSound, control.audioVolume);
            control.ScoreUpdate(Control.Scores.scorePowerupPush);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("PowerupExplode"))
        {
            hasPowerupExplode = true;
            playerRenderer.material.color = powerupExplodeOn;
            playerAudio.PlayOneShot(powerupCollectSound, control.audioVolume);
            control.ScoreUpdate(Control.Scores.scorePowerupExplode);
            Destroy(other.gameObject);
            // StartCoroutine(PowerupCountdownRoutine(other.tag)); // Doesn't expires. Removed after use.
        }
    }
}
