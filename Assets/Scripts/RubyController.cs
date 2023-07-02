using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    public float speed = 6.0f;          // 이동속도
    public int maxHealth = 5;           // 최대 체력
    public float timeInvicible = 2.0f;  // 무적 시간
    public GameObject projectilePrefab; // 투사물 프리팹
    public AudioClip throwClip;        // 투척 오디오클립 

    int currentHealth;      // 현재 체력
    bool isInvicible;       // 무적여부
    float invicibleTimer;   // 무적 타이머 

    // 체력 프로퍼티 
    public int health { get { return currentHealth; } }

    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 20; //초당 30 프레임으로 렌더링
        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); //direction
        float vertical = Input.GetAxis("Vertical");
        //Debug.Log(horizontal);

        Vector2 move = new Vector2(horizontal, vertical);

        if (Mathf.Approximately(move.x, 0.0f) == false || Mathf.Approximately(move.y, 0.0f) == false)
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = rigidbody2d.position;
        position = position + move * speed * Time.deltaTime; // Time.deltaTime Unity가 한 프레임을 렌더링하는데 걸리는 시간 변수

        //position.x = position.x + speed * horizontal * Time.deltaTime; 
        //position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        // 무적상태 해제
        if (isInvicible == true)
        {
            invicibleTimer -= Time.deltaTime; // 시간 거꾸로 세기
            if (invicibleTimer < 0)
            {
                isInvicible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);

                NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();

                if (npc != null)
                {
                    npc.DisplayDialog();
                }
            }
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); // Quaternion.identity = 회전 없음 

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwClip);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvicible == true)
            {
                return;
            }

            // 무적상태 설정
            isInvicible = true;
            invicibleTimer = timeInvicible;

            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
