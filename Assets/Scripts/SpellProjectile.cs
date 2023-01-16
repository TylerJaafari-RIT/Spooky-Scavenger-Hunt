using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
	public float speed;
	public int damage = 1;
	private float bounds = 200f; // TEMPORARY until border colliders are added

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // just fly forward
		transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
		
		// if out of bounds, self-destruct
		if(Mathf.Abs(transform.position.x) >= bounds || Mathf.Abs(transform.position.z) >= bounds) {
			Destroy(gameObject);
		}
    }

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Monster")) {
			// DAMAGE THE MONSTER, I DON'T KNOWW\
			Monster target = other.gameObject.GetComponent<Monster>();
			target.TakeDamage(damage);
		}
		else if(!other.gameObject.CompareTag("Player")) {
			Destroy(gameObject);
		}
	}
	
}
