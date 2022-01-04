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
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        Wander();
        anim.SetInteger("Walk", 1);
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
            SinglePlayerScoreBoardScript.instance.AnimChickenCollected();
            Collected();
        }
    }


    public void Collected()
    {
        Destroy(gameObject);
    }

  



}
