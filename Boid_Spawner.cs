using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

public class Boid_Spawner : MonoBehaviour
{
    public int boidsPerInterval;
    public int boidsToSpawn;
    public float interval;
    public float cohesionBias;
    public float separationBias;
    public float alignmentBias;
    public float targetBias;
    public float perceptionRadius;
    public float3 target;
    public Material material;
    public Mesh mesh;
    public float maxSpeed;
    public float step;
    public int cellSize;
    private EntityManager entitymanager;
    private Entity entity;
    private float elapsedTime;
    private int totalSpawnedBoids;
    private EntityArchetype ea;
    private float3 currentPosition;

    private void Start()
    {
        totalSpawnedBoids = 0;
        entitymanager = World.DefaultGameObjectInjectionWorld.EntityManager;
        currentPosition = this.transform.position;
        ea = entitymanager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(Boid_ComponentData)
            );
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= interval)
        {
            elapsedTime = 0;
            for (int i=0; i<=boidsPerInterval; i++)
            {
                if (totalSpawnedBoids == boidsToSpawn)
                {
                    break;
                }
                Entity e = entitymanager.CreateEntity(ea);

                entitymanager.AddComponentData(e, new Translation
                {
                    Value = currentPosition
                });
                entitymanager.AddComponentData(e, new Boid_ComponentData
                {
                    velocity = math.normalize(UnityEngine.Random.insideUnitSphere) * maxSpeed,
                    perceptionRadius = perceptionRadius,
                    speed = maxSpeed,
                    step = step,
                    cohesionBias = cohesionBias,
                    separationBias = separationBias,
                    alignmentBias = alignmentBias,
                    target = target,
                    targetBias = targetBias,
                    cellSize = cellSize
                });
                entitymanager.AddSharedComponentData(e, new RenderMesh
                {
                    mesh = mesh,
                    material = material,
                    castShadows = UnityEngine.Rendering.ShadowCastingMode.On
                });
                totalSpawnedBoids++;
            }
        }
    }
}
