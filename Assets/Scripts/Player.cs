using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float JumpForce = 3;
    public float MaxRotation = 30;
    public float MinRotation = -90;
    [Tooltip("velocity at which max rotation is achieved")]
    public float MaxRotVelocity = 0.5f;
    [Tooltip("velocity at which min rotation is achieved")]
    public float MinRotVelocity = -5;
    public AudioSource AudioSource;
    public AudioClip DieSound;
    public AudioClip JumpSound;
    public GameObject GameoverScreen;
    public GameMaster gameMaster;

    Rigidbody2D rigidbody;
    bool desireToJump = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = true;
    }

    void Jump()
    {
        AudioSource.PlayOneShot(JumpSound);
        rigidbody.velocity += new Vector2(0, JumpForce);
    }

    private void FixedUpdate()
    {
        if (desireToJump)
            Jump();

        desireToJump = false;
    }

    void Update()
    {
        if(Input.anyKeyDown)
            desireToJump = true;

        UpdateRotation();
    }

    void UpdateRotation()
    {
        //remapping velocity to rotation
        float t = (rigidbody.velocity.y - MinRotVelocity)/(MaxRotVelocity - MinRotVelocity);
        t = Mathf.Clamp(t, 0.0f, 1.0f);

        float rotation = t * (MaxRotation - MinRotation) + MinRotation;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    void Die()
    {
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
        rigidbody.velocity = new Vector2(0, JumpForce);
        rigidbody.angularVelocity = 120;
        enabled = false;
        AudioSource.PlayOneShot(DieSound);

        gameMaster.enabled = false;
        GameoverScreen.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("ceiling") && enabled)
            Die();
    }
}
