using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject Fruit_Whole;
    public GameObject Fruit_Sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem juiceParticleEffect;

    private void Awake() {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force) {
        FindObjectOfType<GameManager>().IncreaseScore();

        Fruit_Whole.SetActive(false);
        Fruit_Sliced.SetActive(true);

        fruitCollider.enabled = false;
        juiceParticleEffect.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Fruit_Sliced.transform.rotation = Quaternion.Euler (0f, 0f, angle);

        Rigidbody[] slices = Fruit_Sliced.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody slice in slices) {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction*force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }
}
