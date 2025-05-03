using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class TrajectoryDraw : MonoBehaviour
{
    [Header("Projectile settings")]
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Rigidbody projectileRigitbody;
    [SerializeField] WeaponData weaponData;
    private float projectileMass;
    private float drag;
    private float launchForse;

    [Header("Visulazation settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int maxPoints = 100;          
    [SerializeField] private float timeStep = 0.05f;      
    [SerializeField] private float maxTime = 5f;
    [SerializeField] private LayerMask collisionMask = -1; // По умолчанию учитываются все слои

    // Start is called before the first frame update
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;  
            lineRenderer.endWidth = 0.1f;    
            lineRenderer.positionCount = 0;  
           
            // Создаем материал для линии
            Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
            lineMaterial.color = new Color(1f, 0.5f, 0f, 0.8f); // orange color
            lineRenderer.material = lineMaterial;
        }
        
        launchForse = weaponData.BulletForse;
        drag = projectileRigitbody.drag;
        projectileMass = projectileRigitbody.mass;
    }

    // Update is called once per frame
    private void Update()
    {
        // Получаем направление выстрела
        Vector3 launchDirection = bulletSpawnPoint.forward;
       
        // Предсказываем и рисуем траекторию
        PredictTrajectory(bulletSpawnPoint.position, launchDirection * launchForse, Physics.gravity, drag / projectileMass);
    }

    public void PredictTrajectory(Vector3 startPosition, Vector3 initialVelocity, Vector3 gravity, float dragFactor)
    {
        // r
        List<Vector3> trajectoryPoints = new List<Vector3>();
        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = initialVelocity;
        float timeElapsed = 0f;
        bool hitSomething = false;
        
        // Динамически увеличиваем максимальные параметры для высоких бросков
        float dynamicMaxTime = maxTime * 3f; 
        int dynamicMaxPoints = maxPoints * 3; 

        for (int i = 0; i < dynamicMaxPoints; i++)
        {
            // Добавляем текущую позицию в список
            trajectoryPoints.Add(currentPosition);
           
            // Увеличиваем время
            timeElapsed += timeStep;
           
            if (timeElapsed > dynamicMaxTime)
            {
                
                RaycastHit floorHit;
                if (Physics.Raycast(currentPosition, Vector3.down, out floorHit, 1000f, collisionMask))
                {
                    
                    Vector3 hitPoint = floorHit.point;
                    
                    Vector3 direction = hitPoint - currentPosition;
                    float distance = direction.magnitude;
                    int steps = Mathf.CeilToInt(distance / (currentVelocity.magnitude * timeStep));
                    steps = Mathf.Min(steps, 10); // Не более 10 промежуточных точек
                    
                    for (int j = 1; j <= steps; j++)
                    {
                        trajectoryPoints.Add(Vector3.Lerp(currentPosition, hitPoint, (float)j / steps));
                    }
                }
                break;
            }

            // Сохраняю предыдущую позицию для рейкаста
            Vector3 prevPosition = currentPosition;
            
            
            currentVelocity += (gravity - dragFactor * currentVelocity.magnitude * currentVelocity) * timeStep;
            currentPosition += currentVelocity * timeStep;
           
            // Проверка на столкновение с объектами
            RaycastHit hit;
            if (Physics.Raycast(prevPosition, currentVelocity.normalized,
                out hit, currentVelocity.magnitude * timeStep, collisionMask))
            {
                // Если снаряд столкнулся с чем-то, добавляем точку столкновения и прекращаем симуляцию
                trajectoryPoints.Add(hit.point);
                hitSomething = true;
                break;
            }
            
            // Дополнительная проверка когда он может пройти сквозь тонкие объекты
            if (i > 0 && Physics.Linecast(prevPosition, currentPosition, out hit, collisionMask))
            {
                trajectoryPoints.Add(hit.point);
                hitSomething = true;
                break;
            }
            
            // Проверка на столкновение с землей на нисходящей траектории
            if (i > 10 && currentVelocity.y < 0) 
            {
                if (Physics.Raycast(currentPosition, Vector3.down, out hit, 2f, collisionMask))
                {
                   
                    float distanceToGround = (currentPosition - hit.point).magnitude;
                    if (distanceToGround < currentVelocity.magnitude * timeStep)
                    {
                        trajectoryPoints.Add(hit.point);
                        hitSomething = true;
                        break;
                    }
                }
            }
        }
        
       
        if (!hitSomething && trajectoryPoints.Count > 0)
        {
            
            Vector3 lastPoint = trajectoryPoints[trajectoryPoints.Count - 1];
            RaycastHit finalHit;
            
            Vector3 rayDirection = currentVelocity.y < 0 ? Vector3.down : currentVelocity.normalized;
            if (Physics.Raycast(lastPoint, rayDirection, out finalHit, 1000f, collisionMask))
            {
                
                Vector3 direction = finalHit.point - lastPoint;
                float distance = direction.magnitude;
                int steps = Mathf.Min(10, Mathf.CeilToInt(distance / 20f)); 
                
                for (int j = 1; j <= steps; j++)
                {
                    trajectoryPoints.Add(Vector3.Lerp(lastPoint, finalHit.point, (float)j / steps));
                }
            }
        }
       
        // Обновляем LineRenderer
        lineRenderer.positionCount = trajectoryPoints.Count;
        lineRenderer.SetPositions(trajectoryPoints.ToArray());
    }
}
