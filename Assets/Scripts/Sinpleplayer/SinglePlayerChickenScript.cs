using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class SinglePlayerChickenScript : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    int range;
    NavMeshAgent nav;
    [SerializeField]
    int score;
    [SerializeField]
    AudioClip sound1, sound2;
    AudioSource aS;

    GameObject player;
    bool played;
    float timefornextSound;
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        aS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        Wander();
    }

    void Update()
    {


        // code for changing direction when player spotted nearby
       /* RaycastHit hit;
        if (Physics.SphereCast(transform.position, 5, transform.forward, out hit, 2))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Wander();
            }
        }*/
        if ((transform.position - nav.destination).magnitude < 3)
        {
            Wander();

        }
        if (player != null)
        {
            if ((transform.position - player.transform.position).magnitude < 10)
            {
                if (timefornextSound <= 0)
                {
                    aS.clip = sound1;
                    aS.Play();
                    timefornextSound = Random.Range(2, 6);
                }
                else
                {
                    timefornextSound -= Time.deltaTime;
                }
            }
            else
            {
                timefornextSound = 0;
            }
        }
        //transform.LookAt(nav.destination);

    }
    Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
    void Wander()
    {
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(GetRandomLocation(), path);
        nav.SetPath(path);


    }

    private void OnTriggerEnter(Collider other)
    {

        if ((other.CompareTag("Wall") || other.CompareTag("MapObject")))
        {
            Wander();
        }
        if (other.CompareTag("Player"))
        {
            aS.clip = sound2;
            aS.Play();
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            GetComponent<BoxCollider>().enabled = false;
            SinglePlayerScoreBoardScript.instance.AnimChickenCollected();
            Invoke("Collected", 1f);
        }
        if (other.CompareTag("Bot"))
        {
            Collected();
        }
    }


    public void Collected()
    {
        Destroy(gameObject);
    }

  



}
