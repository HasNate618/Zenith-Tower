using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    ////// PATHFINDING //////

    public NavMeshAgent agent;

    public static Transform player;
    static PlayerHealth playerHealth;
    static Interactables interactables;

    public LayerMask whatIsGround, whatIsPlayer, combinedLayerMask;

    [SerializeField] private Enemy self;

    [SerializeField] string attackType;
    [SerializeField] Transform projectile;

    //Patroling
    [SerializeField] Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private int walkPointMinTime, stillMinTime;
    private float walkPointSetTime, stillTime;
    [SerializeField] float walkPointRange, stayTime;
    private float timeSinceStay;

    //Attacking

    [System.Serializable]
    public class EnemyAttack
    {
        public string animation, sound;
        public GameObject particleEffect;
        public float damage;

        public Transform origin;
        public float zOffset, radius, rotationOffset;
    }

    [SerializeField] EnemyAttack[] attacks;
    private EnemyAttack activeAttack;
    [SerializeField] float timeBetweenAttacks;
    private bool alreadyAttacked;
    [SerializeField] float explosionRadius, explosionDamage;

    //States
    [SerializeField] float sightRange, attackRange;
    [SerializeField] bool playerInAttackRange, playerInSight;
    [SerializeField] Vector3 eyeHeight;

    ////// ANIMATING //////

    public Animator animator;
    private static int walkingHash, attackingHash;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Character").transform;
            playerHealth = player.GetComponent<PlayerHealth>();
            interactables = player.GetComponentInChildren<Interactables>();

            walkingHash = Animator.StringToHash("isWalking");
            attackingHash = Animator.StringToHash("isAttacking");
        }
    }

    private void Awake()
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null) health.enabled = true;
    }

    private void Update()
    {
        if (player == null)
        {
            enabled = false;
            return;
        }

        //Check for sight and attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (Physics.Raycast(transform.position + eyeHeight, player.position - transform.position, out RaycastHit hitInfo, sightRange, combinedLayerMask))
        {
            playerInSight = hitInfo.transform == player && !interactables.openingDoor;
        }
        //Debug.DrawRay(transform.position + eyeHeight, player.position - transform.position, Color.green, 0, false);

        if (!playerInSight && !playerInAttackRange) Patroling();
        if (playerInSight && !playerInAttackRange) ChasePlayer();
        if (playerInSight && playerInAttackRange) StartAttack();

        animator.SetBool(walkingHash, agent.velocity.magnitude > 0);
    }

    void OnDrawGizmos()
    {
        if (attackType == "Projectile" || activeAttack == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.forward * activeAttack.zOffset + transform.position, activeAttack.radius);
    }

    private void Patroling()
    {
        if (!walkPointSet && timeSinceStay <= 0)
        {
            SearchWalkPoint();
        }
        
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        else
        {
            agent.velocity = Vector3.zero;
        }

        if (agent.velocity.magnitude > 0)
        {
            if (walkPointSetTime > 0)
            {
                walkPointSetTime -= Time.deltaTime;
            }
            else
            {
                walkPointSetTime = walkPointMinTime;
                walkPointSet = false;
                SearchWalkPoint();
            }
        }
        else
        {
            if (stillTime > 0)
            {
                stillTime -= Time.deltaTime;
            }
            else
            {
                stillTime = stillMinTime;
                walkPointSet = false;
                SearchWalkPoint();
            }
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walk point reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            timeSinceStay -= Time.deltaTime;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, out RaycastHit hitInfo, 2f, whatIsGround))
        {
            if (hitInfo.transform.GetComponentInParent<RoomSetup>() == self.roomInside)
            {
                walkPointSet = true;
                timeSinceStay = stayTime;
                walkPointSetTime = walkPointMinTime;
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void StartAttack()
    {
        if (playerHealth.dead) return;

        if (!alreadyAttacked)
        {
            //Choose random attack
            activeAttack = attacks[Random.Range(0, attacks.Length)];

            //Attack code here
            animator.SetBool(activeAttack.animation, true);
            enabled = false;
            //

            alreadyAttacked = true;
        }

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        float rotationOffset = 0;
        if (activeAttack != null) rotationOffset = activeAttack.rotationOffset;
        transform.rotation *= Quaternion.Euler(0, rotationOffset, 0);

    }

    public void Attack()
    {
        switch (attackType)
        {
            case "Melee":
                if (Physics.CheckSphere(activeAttack.origin.position + activeAttack.zOffset * transform.forward, activeAttack.radius, whatIsPlayer))
                {
                    playerHealth.TakeDamage(activeAttack.damage);
                }
                /*if (Physics.CheckSphere(transform.forward * attackDistance + transform.position, attackRadius, whatIsPlayer))
                {
                    playerHealth.TakeDamage(attackDamage);
                }*/
                break;
            case "Projectile":
                GameObject projectile = Instantiate(this.projectile.gameObject, activeAttack.origin.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().Setup(transform.rotation.eulerAngles.y, activeAttack.radius, activeAttack.origin.position, activeAttack.damage, null, "Player");
                PlaySound(activeAttack.sound);
                break;
            case "Kamikaze":
                EnemyHealth health = GetComponent<EnemyHealth>();
                health.canvas.SetActive(false);
                health.minimapIndicator.SetActive(false);
                GetComponentInChildren<KamikazeTrigger>().active = true;
                break;
        }
    }

    public void EndAttack()
    {
        enabled = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        animator.SetBool(activeAttack.animation, false);
        activeAttack = null;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void OnExplosion(Vector3 position)
    {
        if (Physics.CheckSphere(position, explosionRadius, whatIsPlayer)) playerHealth.TakeDamage(explosionDamage);
    }

    public AudioSource audioSource;
    public void PlaySound(string name)
    {
        Audio.PlaySound(name, audioSource);
    }

    public void StartParticleEffect()
    {
        if (activeAttack.particleEffect != null)
        {
            activeAttack.particleEffect.SetActive(true);
        }
    }

    public void EndParticleEffect()
    {
        if (activeAttack.particleEffect != null)
        {
            activeAttack.particleEffect.SetActive(false);
        }
    }
}
