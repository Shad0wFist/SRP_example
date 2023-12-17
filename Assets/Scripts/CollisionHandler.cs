using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public float minimumCollisionSpeed = 10f;
    public GameObject sparkPrefab;
    public float sparkLifetime = 2f;
	public float demolutionRange = 0.5f;
    public float radiusDeformate = 0.1f;
    public float multiplay = 0.1f;
	public MeshFilter[] MeshList;
	private MeshFilter[] meshfilters;
	private float sqrDemRange;
	

    //Save Vertex Data
    private struct permaVertsColl
    {
        public Vector3[] permaVerts;
    }
    private permaVertsColl[] originalMeshData;
    int i;
	public void Start()
	{   
        
        if(MeshList.Length>0)
        	meshfilters = MeshList;
        else
        	meshfilters = GetComponentsInChildren<MeshFilter>();

        LoadOriginalMeshData();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Repair();
    }

    void LoadOriginalMeshData()
    {
        originalMeshData = new permaVertsColl[meshfilters.Length];
        for( i = 0; i < meshfilters.Length; i++)
        {
            originalMeshData[i].permaVerts = meshfilters[i].mesh.vertices;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minimumCollisionSpeed)
        {
            // Определяем точку столкновения
            ContactPoint contactPoint = collision.contacts[0];
            Vector3 collisionPoint = contactPoint.point;

            // Определяем направление движения автомобиля
            Vector3 carVelocity = GetComponent<Rigidbody>().velocity;
            Vector3 sparkDirection = carVelocity.normalized;

            // Создаём искры в точке столкновения
            GameObject sparks = Instantiate(sparkPrefab, collisionPoint, Quaternion.LookRotation(sparkDirection));

            // Уничтожаем искры после указанного времени
            Destroy(sparks, sparkLifetime);
        }
            
            if (collision.relativeVelocity.magnitude > minimumCollisionSpeed)
            {
                for (int j=0; j<meshfilters.Length; j++)
                {
                    Vector3 [] verts = meshfilters[j].mesh.vertices;
                    for (int i = 0; i < meshfilters[j].mesh.vertexCount; i++)
                    {
                        for (int n = 0; n < collision.contacts.Length; n++)
                        {
                            Vector3 point = transform.InverseTransformPoint (collision.contacts [n].point);
                            Vector3 velocity = transform.InverseTransformVector (collision.relativeVelocity);
                            float distance = Vector3.Distance (point, verts [i]);
                            if (distance < radiusDeformate)
                            {
                                Vector3 deformate = velocity * (radiusDeformate - distance) * multiplay;
                                verts [i] += deformate;
                            }
                        }
                    }

                    meshfilters[j].mesh.vertices = verts;
                    meshfilters[j].mesh.RecalculateNormals();
                    meshfilters[j].mesh.RecalculateBounds();
                }
            }

    }

    
    void Repair()
    {
        for (int i = 0; i < meshfilters.Length; i++)
        {
            meshfilters[i].mesh.vertices = originalMeshData[i].permaVerts;
            meshfilters[i].mesh.RecalculateNormals();
            meshfilters[i].mesh.RecalculateBounds();
        }
    }
}