using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBig : MonoBehaviour
{
    // Enemy Big: slow, strong, IA, few spawn. IA: follow player, avoid world edges (not yet)
    public float speed = 10.0f;
    public float rotationSpeed = 10f;
    public float cannonBallSpeed = 10f;
    public GameObject bigHead;
    public GameObject bigHeadEye;
    public GameObject bigCannon;
    public GameObject cannonBallPrefab;
    public AudioClip bigEnemyFire;
    public ParticleSystem smokeParticle;
    private Control control;
    private Rigidbody headRb;
    private GameObject player;
    private AudioSource enemyAudio;
    private Vector3 lookDirection;
    private Vector3 moveDirection;
    private Vector3 cannonBallOffset = new Vector3(0f, 0.4f, 0.9f); // 0f, 0.5f, 1.1f);

    void Start()
    {
        control = GameObject.Find("Control").GetComponent<Control>();
        headRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemyAudio = GetComponent<AudioSource>();
        StartCoroutine(CannonFireRoutine());
    }

    void FixedUpdate()
    {
        if (control.isActionOn)
        {
            // Rotate Enemy and follow Player
            lookDirection = (player.transform.position - transform.position).normalized;
            moveDirection = new Vector3(lookDirection.x, 0, lookDirection.z); // prevents "moving up"
            transform.rotation = Quaternion.LookRotation(moveDirection);
            headRb.AddForce(moveDirection * speed, ForceMode.Impulse);

            // Child cilinder-head, rotation effect
            bigHead.transform.Rotate(Vector3.forward * Time.deltaTime * (rotationSpeed * 3));
        }
        if (transform.position.y < -5)
        {
            control.ScoreUpdate(Control.Scores.scoreEnemyBig);
            Destroy(gameObject);
        }
    }

    IEnumerator CannonFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (control.isActionOn)
            {
                enemyAudio.PlayOneShot(bigEnemyFire, control.audioVolume);
                smokeParticle.Play();
                GameObject cannonBall = Instantiate(cannonBallPrefab, transform.position, transform.rotation);
                cannonBall.transform.SetParent(transform);
                cannonBall.transform.Translate(cannonBallOffset);
                cannonBall.GetComponent<Rigidbody>().AddForce(lookDirection * cannonBallSpeed, ForceMode.Impulse);
            }
        }
    }

}
