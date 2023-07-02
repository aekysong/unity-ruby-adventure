using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public bool vertical;
    public float changeTime = 3.0f;
    public float speed = 3.0f; // 이동속도

    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;
    bool broken = true;
    
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30; // 초당 30 프레임으로 렌더링

        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (broken == false)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigidbody2d.position;

        if (vertical == true)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false; // 물리 시스템 시뮬레이션에서 해당 Rigidbody를 제거

        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
}
