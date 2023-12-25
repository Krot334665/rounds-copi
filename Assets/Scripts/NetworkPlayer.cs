using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D rb;
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public float speed = 5f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        public LayerMask groundLayer;

        private bool isGrounded;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Spawn();
            }
        }

        public void Spawn()
        {
            if (IsOwner && NetworkManager.Singleton.IsHost)
            {
                Position.Value = new Vector3(7, -1.7f, 0);
                transform.position = Position.Value;
                _spriteRenderer.color = new Color(1f, 0.5f, 0.1f, 1);
            }else if(IsOwner)
            {
                transform.position = new Vector3(-6, -1.7f, 0);
                _spriteRenderer.color = Color.blue;
            }
        }


        void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            
         //    if (NetworkManager.Singleton.IsHost)
         //    {
         //        Position.Value = transform.position;
         //    }

            
                // Проверка, находится ли персонаж на земле
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

                // Обработка ввода для движения и прыжка
                float horizontalInput = Input.GetAxis("Horizontal");
                HandleMovement(horizontalInput);

                if (isGrounded && Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
        }

        private void HandleMovement(float horizontalInput)
        {
            // Движение влево/вправо
            Vector2 movement = new Vector2(horizontalInput * speed, rb.velocity.y);
            rb.velocity = movement;
        }

        private void Jump()
        {
            // Прыжок
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}