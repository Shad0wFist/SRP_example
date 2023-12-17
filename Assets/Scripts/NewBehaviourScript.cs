using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class DeformableMesh : MonoBehaviour
{
    private Mesh originalMesh;
    private Mesh deformedMesh;
    private Vector3[] originalVertices;
    private Vector3[] deformedVertices;

    public float deformationForce = 0.1f;

    void Start()
    {
        originalMesh = GetComponent<MeshFilter>().mesh;
        deformedMesh = Instantiate(originalMesh); // Создаем копию оригинальной сетки для деформации
        GetComponent<MeshFilter>().mesh = deformedMesh;
        originalVertices = originalMesh.vertices;
        deformedVertices = deformedMesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Получаем направление столкновения
        Vector3 collisionDirection = collision.contacts[0].point - transform.position;

        // Применяем силу деформации к вершинам сетки в направлении столкновения
        for (int i = 0; i < deformedVertices.Length; i++)
        {
            float distanceToCollision = Vector3.Dot(originalVertices[i] - transform.position, collisionDirection.normalized);
            deformedVertices[i] = originalVertices[i] + collisionDirection.normalized * -Mathf.Clamp(deformationForce / (distanceToCollision + 1), 0, deformationForce);
        }

        // Обновляем сетку
        deformedMesh.vertices = deformedVertices;
        deformedMesh.RecalculateNormals();
        deformedMesh.RecalculateBounds();
    }
}