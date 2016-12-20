using UnityEngine;
using System.Collections;

public class SkeletonBehaviour : MonoBehaviour
{

    public int Id;
    public int Health;
    public GameObject mWeaponBone;
    public PickUpSkull mSkull;
    public CapsuleCollider mCapsuleCollider;
    public SphereCollider mFieldOfView;
    public float fieldOfViewAngle = 110f;
    public bool PlayerInSight = false;
    public Vector3 lastPersonalSighting;

    private NavMeshAgent navMeshAgent;
    public GameObject player;
    private Vector3 previousSighting;
    private Animation mAnimation;
    private bool mIsAttacking = false;


    // Use this for initialization
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameObject.SetActive(false);
        mAnimation = GetComponent<Animation>();
        mCapsuleCollider = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInSight)
        {            
              navMeshAgent.SetDestination(player.transform.position);
            
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

            if (distance < 6.0f)
            {
                Attack();
                navMeshAgent.Stop();
            }
            else if (distance > 6.0f)
            {
                mAnimation.CrossFade("Walk");
                mAnimation["Walk"].speed = 2F;
                navMeshAgent.Resume();
            }
            
        }
        else
        {
            mAnimation.CrossFade("Idle");
        }

        if (Health == 0)
        {
            navMeshAgent.Stop();
            mAnimation.Play("Death");           
             
        }
    }

    void ActivateWeaponCollider()
    {
        mWeaponBone.GetComponent<BoxCollider>().enabled = true;
        Debug.Log("Activated");
    }

    void DeactivateWeaponCollider()
    {
        mWeaponBone.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Deactivated");
    }

    public void ActivateSkeleton()
    {
        gameObject.SetActive(true);
    }

    private void Attack()
    {
        mIsAttacking = true;
        mAnimation.Blend("Attack");
        mAnimation["Attack"].speed = 2F;
    }

    void AttackEnded()
    {
        mIsAttacking = false;
    }

    public void Death()
    {
        PlayerInSight = false;
        DeactivateWeaponCollider();
        mSkull.ShowUp();
        mSkull.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.5f, gameObject.transform.position.z);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {        
            if (collider.gameObject.tag == "Player")
            {
                PlayerInSight = true;
                transform.LookAt(collider.gameObject.transform);
            }      
    }

}
