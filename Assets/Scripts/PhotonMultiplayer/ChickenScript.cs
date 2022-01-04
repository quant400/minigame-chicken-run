using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class ChickenScript : MonoBehaviour
{
    PhotonView pv;
    Animator anim;
    [SerializeField]
    int speed ;
    Vector3 wayPoint;
    [SerializeField]
    int range ;
    NavMeshAgent nav;
    void  Start()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        nav = GetComponent<NavMeshAgent>();
        if (pv.IsMine)
        {
            Wander();
            anim.SetInteger("Walk", 1);
        }
        else
        {
            Destroy(GetComponent<Rigidbody>());
            //Destroy(GetComponent<Animator>());
        }
    }

    void Update()
    {
        //transform.position += transform.TransformDirection(Vector3.forward) * Speed * Time.deltaTime;

        if (pv.IsMine)
        {
            RaycastHit hit;
            if(Physics.SphereCast(transform.position,5,transform.forward,out hit,2))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    Wander();
                }
            }
            if ((transform.position - nav.destination).magnitude < 3)
            {
                Wander();

            }
            //transform.LookAt(nav.destination);
        }
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
    void  Wander()
    {
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(GetRandomLocation(), path);
        nav.SetPath(path);
        //nav.SetDestination(GetRandomLocation());

    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("MapObject")) && pv.IsMine)
        {
            Wander();
        }
        else if(collision.gameObject.CompareTag("Chicken") && pv.IsMine)
        {
            Wander();
        }
    }
    

    public void Collected(PhotonView p)
    {
        GameRoom.room.photonView.RPC("RPC_UpdateScore", RpcTarget.MasterClient,p.Owner , GameSetup.Gs.scoreOneChicken);
        pv.RPC("RPC_DestoryChicken", RpcTarget.MasterClient);
        Destroy(gameObject.GetComponent<Collider>());
        foreach(Transform x in transform )
        {
            Destroy(x.GetComponent<MeshRenderer>());
        }
       

    }

    [PunRPC]
    public void RPC_DestoryChicken()
    {
        PhotonNetwork.Destroy(gameObject);
    }


   
}
