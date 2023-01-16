using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private int mana;
	private int health;

	public float speed = 10.0f;
	public Camera mainCamera;
	private AudioSource cameraAudio;
	public AudioClip gameWonClip;
	public AudioClip gameLostClip;

	public Text manaCounter;
	public Text healthCounter;
	
	private Vector3 v3PlayerDirections;
	private Animator playerAnimator;

	public GameObject spellProjectile;

	private GameManager gameManager;

	public const float invulnerabilityPeriod = 1.0f;
	private float invulnerability = 0.0f;

	public Renderer r;
	private float fOpacity = 1.0f;

	// Start is called before the first frame update
	void Start()
    {
        v3PlayerDirections = new Vector3(1, 0, 1);
		playerAnimator = GetComponent<Animator>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		cameraAudio = mainCamera.GetComponent<AudioSource>();
		
		mana = 10;
		health = 5;
	}

    // Update is called once per frame
    void Update()
    {
		if(invulnerability > 0) {
			if(invulnerability <= 0.1f || fOpacity == 0.2f) { fOpacity = 1.0f; } else /*if(fOpacity == 1.0f) */{ fOpacity = 0.2f; }
			Color c = new Color(r.material.color.r, r.material.color.g, r.material.color.b, fOpacity);
			r.material.color = c;
			r.materials[1].SetColor("_Color", c);
		}

		manaCounter.text = "Magic: " + mana;
		healthCounter.text = "Health: " + health;

		if(!gameManager.gameOver) {
			if(Input.GetKeyDown(KeyCode.Space) && (gameManager.infiniteMagic || mana > 0)) {
				CastSpell();
				--mana;
			}

			// magnitudes of player movement
			float fHorizontal = Input.GetAxis("Horizontal");
			float fVertical = Input.GetAxis("Vertical");

			if(fHorizontal < 0 && fVertical == 0) {             // moving left
				this.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
				//dirtParticle.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
				//dirtParticle.Play();
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fHorizontal > 0 && fVertical == 0) {      // moving right
				this.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
				//dirtParticle.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
				//dirtParticle.Play();
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fVertical > 0 && fHorizontal == 0) {      // moving up
				this.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fVertical < 0 && fHorizontal == 0) {      // moving down
				this.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fVertical > 0 && fHorizontal > 0) {           // moving up-right
				this.transform.rotation = Quaternion.AngleAxis(45, Vector3.up);
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fVertical > 0 && fHorizontal < 0) {       // moving up-left
				this.transform.rotation = Quaternion.AngleAxis(-45, Vector3.up);
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fVertical < 0 && fHorizontal > 0) {       // moving down-right
				this.transform.rotation = Quaternion.AngleAxis(135, Vector3.up);
				playerAnimator.SetFloat("Speed_f", 5);
			} else if(fVertical < 0 && fHorizontal < 0) {       // moving down-left
				this.transform.rotation = Quaternion.AngleAxis(-135, Vector3.up);
				playerAnimator.SetFloat("Speed_f", 5);
			} else {
				playerAnimator.SetFloat("Speed_f", 0);
			}

			// translates relative to world space by a vector with fHorizontal on x and fVertical on z, multiplied by:
			// Time.deltaTime == the time between frames, and the player's speed
			this.transform.Translate(new Vector3(fHorizontal, 0, fVertical) * Time.deltaTime * speed, Space.World);

			Vector3 cameraVector = new Vector3(this.transform.position.x, this.transform.position.y + 10, this.transform.position.z - 10f);
			mainCamera.transform.SetPositionAndRotation(cameraVector, mainCamera.transform.rotation);

			// little note: Quaternions are like the Vector3's of rotation transforms
			// they define the degrees of rotation around the x, y, and z axes
		} else {
			playerAnimator.SetFloat("Speed_f", 0);
			if(transform.position.y < 0.1f) {
				playerAnimator.SetBool("Jump_b", false);
				playerAnimator.SetInteger("Animation_int", 2);
			}
		}
	}

	private void CastSpell() {
		//Debug.Log("Spell was cast!");
		GameObject projectile = Instantiate(spellProjectile);
		projectile.transform.rotation = this.transform.rotation;
		projectile.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Jack-O-Lantern")) {
			Destroy(other.gameObject);
			GameManager.pumpkinsFound += 1;
		} else if (other.gameObject.CompareTag("ManaPotion")) {
			Destroy(other.gameObject);
			mana += 5;
		} else if(other.gameObject.CompareTag("HealthPotion")) {
			Destroy(other.gameObject);
			health += 1;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if(!gameManager.gameOver) {
			if(collision.gameObject.CompareTag("Monster")) {
				// currently, take 1 point of damage.
				// BACKLOG: if new monsters are added, implement damage variable in monster class
				if(invulnerability <= 0) {
					invulnerability = invulnerabilityPeriod;
					if(!gameManager.godMode)
						--health;
					if(health <= 0) {
						health = 0;
						Die();
					} else {
						//UnityEngine.AI.NavMeshAgent navAgent = collision.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
						if(!gameManager.noKnockback)
							GetComponent<Rigidbody>().AddForce((transform.position - collision.transform.position) * 10f, ForceMode.Impulse);
						StartCoroutine(InvulnerabilityCountdown());
					}
				}
			}
		}
	}

	IEnumerator InvulnerabilityCountdown() {
		Color original = r.material.color;
		Color flash = new Color(255, 255, 255, 5);
		bool flashOff = true;
		while(invulnerability > 0) {
			//if(invulnerability <= 0.1f || fOpacity == 0.2f) { fOpacity = 1.0f; } else /*if(fOpacity == 1.0f) */{ fOpacity = 0.2f; }
			if(flashOff) {
				r.material.color = flash;
				r.materials[1].color = flash;
				flashOff = false;
			} else {
				r.material.color = original;
				r.materials[1].color = original;
				flashOff = true;
			}
			//Color c = new Color(255, 255, 255, fOpacity * 255);
			//r.material.color = c;
			//r.materials[1].color = c;
			yield return new WaitForSeconds(0.1f);
			invulnerability -= 0.1f;
		}
		r.material.color = original;
		r.materials[1].color = original;
	}

	public void Die() {
		playerAnimator.SetBool("Death_b", true);
		playerAnimator.SetInteger("DeathType_int", 2);
		gameManager.EndGame();
		cameraAudio.Stop();
	}

	public void VictoryBounce() {
		GetComponent<Rigidbody>().AddForce(Vector3.up * 10);
		playerAnimator.SetBool("Jump_b", true);
	}
}
