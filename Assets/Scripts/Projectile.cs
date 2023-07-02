using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    // Awake는 Start와 달리 오브젝트 생성 즉시(Instantiate가 호출될 때) 호출
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f) // 월드 중앙까지의 거리 > 1000.0f
        {
            Destroy(gameObject);
        }

        // Vector3.Distance (a,b) 함수로 캐릭터와 톱니간 거리를 측정하여 a 위치와 b 위치의 거리를 계산하는 방법도 가능
        // 타이머 시간 계산 방법으로 Destroy도 가능 
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Projectile Collision With " + other.gameObject);

        EnemyController enemy = other.collider.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.Fix();
        }

        Destroy(gameObject);
    }
}
