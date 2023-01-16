using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
	public int health;
	public float speed;

	protected NavMeshAgent agent;
	public Transform target;

	protected Animator monsterAnimator;

	// whenever collision with player or other effect causes knock-back, the monster should be incapacitated temporarily
	public bool incapacitated = false;
	public float incapTime = 0.0f;

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
	}

	public Monster(int health, float speed) {
		this.health = health;
		this.speed = speed;
	}

	public void TakeDamage(int amount) {
		health -= amount;
		if(health <= 0) { // if you are dead, then die
			Die();
		}
	}

	public void Die() {
		// animation goes here but for now the very concept of the target's existence is just obliterated when health <= 0
		Destroy(gameObject);
	}

	//private void OnCollisionEnter(Collision collision) {
	//	if(collision.gameObject.CompareTag("Player")) {
	//		//agent.isStopped = true;
	//		//agent.SetDestination((transform.position - collision.transform.position) * 10);
	//		//agent.speed = speed * 20;
	//		incapacitated = true;
	//		//agent.isStopped = true;
	//		agent.enabled = false;
	//		GetComponent<Rigidbody>().AddForce((transform.position - collision.transform.position) * 10, ForceMode.Impulse);
	//		incapTime = 1.0f;
	//		StartCoroutine(IncapTimer());
	//	}
	//}

	IEnumerator IncapTimer() {
		//incapTime = 1.0f;
		while(incapTime > 0) {
			yield return new WaitForSeconds(0.1f);
			incapTime -= 0.1f;
		}
		incapacitated = false;
	}
}
