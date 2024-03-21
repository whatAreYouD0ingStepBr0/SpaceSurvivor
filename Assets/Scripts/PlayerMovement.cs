using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;



public class PlayerMovement : NetworkBehaviour {
    public const float MOVE_SPEED = 5f;
    bool canDash = true;
    bool canMove = true;
    public float dashSpeed = 15f;
    public float dashTime = 0.25f;
    public float dashCooldown = 1f;
    float currentDashTime;
    float currentCooldown;
    public AudioSource audioData;

    //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);


    public Rigidbody2D rb;
    public Camera cam;

    Vector2 moveDir;
    Vector2 mousePos;

    // Update is called once per frame

    public override void OnNetworkSpawn()
    {
        float xPos = 2 * OwnerClientId;
        transform.position = new Vector3(xPos, 0, 0);

    }

    void Start()
    {
        audioData.GetComponent<AudioSource>();
    }
    void Update() {
        if (!IsOwner) return;
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");

        // mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space) && canDash) {
            StartCoroutine(Dash(moveDir));
        }
    }

    void FixedUpdate() {
        if (canMove) 
            rb.velocity = moveDir * MOVE_SPEED;

        // Vector2 lookDir = mousePos - rb.position;
        // float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        // rb.rotation = angle;
    }

    private IEnumerator Dash(Vector2 direction) {
        canDash = false;
        canMove = false;
        currentDashTime = dashTime;
        audioData.Play();
        while (currentDashTime > 0f) {
            currentDashTime -= Time.deltaTime;
            
            if (direction.x != 0 && direction.y != 0) {
                float diagSpeed = (dashSpeed / 2) + 1;
                rb.velocity = direction * diagSpeed;
            } else {
                rb.velocity = direction * dashSpeed;
            }
                
            yield return null;
        }
        
        rb.velocity = new Vector2(0f, 0f);
        
        currentCooldown = dashCooldown;
        while (currentCooldown > 0f) {
            currentCooldown -= Time.deltaTime;
            canMove = true;
            yield return null;
        }
        canDash = true;
    }
}
