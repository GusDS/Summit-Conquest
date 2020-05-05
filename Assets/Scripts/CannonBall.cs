using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    // CannonBall: smaller yet version of Enemy Small, just hit the player at launch and after some seconds it explodes
    public float explosionForce;
    public float explosionRadius;
    public AudioClip cannonBallExplode;
    private Control control;
    private GameObject player;
    private GameObject enemiesGroup;
    private AudioSource enemyAudio;
    public ParticleSystem sparksParticle;

    void Start()
    {
        control = GameObject.Find("Control").GetComponent<Control>();
        player = GameObject.Find("Player");
        enemiesGroup = GameObject.Find("EnemiesGroup");
        enemyAudio = GetComponent<AudioSource>();
        StartCoroutine(ExplodeCannonBall());
    }

    void FixedUpdate()
    {
        if (transform.position.y < -5)
        {
            control.ScoreUpdate(Control.Scores.scoreCannonBall);
            Destroy(gameObject);
        }
    }

    IEnumerator ExplodeCannonBall()
    {
        // Short time after it left the cannon, change parent to static parent (so it doesn't keep moving with tank)
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(GameObject.Find("EnemiesGroup").transform);

        // A bit later, remove texture to expose imminent explosion animation
        yield return new WaitForSeconds(1.5f);
        GetComponent<Renderer>().materials[0].mainTexture = null;

        // Later, Explosion
        yield return new WaitForSeconds(3f);
        enemyAudio.PlayOneShot(cannonBallExplode, control.audioVolume);
        sparksParticle.Play();
        Rigidbody[] allEnemiesRb = enemiesGroup.GetComponentsInChildren<Rigidbody>();
        if (allEnemiesRb.Length > 0) {
            foreach (var enemyRb in allEnemiesRb) {
                enemyRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        player.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);

        // More wait time required before destroy object, for the audio explosion to finish...
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        control.ScoreUpdate(Control.Scores.scoreCannonBall);
        Destroy(gameObject);
    }
}
