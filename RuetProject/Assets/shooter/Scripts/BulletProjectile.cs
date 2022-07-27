using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody bulletRigidbody;

    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    // hybrid of raycast and bullet physics projectile;
    /*private Vector3 targetPosition;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

     private void Update()
     {
         float DistanceBefore = Vector3.Distance(transform.position, targetPosition);

         Vector3 moveDir = (targetPosition - transform.position).normalized;
         float moveSpeed = 200f;
         transform.position += moveDir * moveSpeed * Time.deltaTime;

         float distanceAfter = Vector3.Distance(transform.position, targetPosition);

         if(DistanceBefore < distanceAfter)
         {
             Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
             transform.Find("Trail").SetParent(null);
             Destroy(gameObject);
         }

     }*/

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<BulletTarget>() != null) {

            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        } else {
           
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}