using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform playerPos;
    [SerializeField] float spawnForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 weaponPosition = playerPos.position;

                Vector2 direction = (mousePosition - weaponPosition).normalized;
                rb.velocity = direction * spawnForce;
            }else
            {
                Debug.Log("erorrr");
            }
        }
    }
}
