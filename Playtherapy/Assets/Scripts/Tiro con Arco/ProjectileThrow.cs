using UnityEngine;
//using UnityEngine.InputSystem;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    TrajectoryPredictor trajectoryPredictor;

    [SerializeField]
    GameObject newArrow;

    [SerializeField, Range(0.0f, 50.0f)]
    float force;

    //[SerializeField, Range(0.0f, 100.0f)]
    //float extraForceMultiplier;

    [SerializeField]
    Transform StartPosition;

    public bool isTensioning;

    //public InputAction fire;

    void OnEnable()
    {

        trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (StartPosition == null)
            StartPosition = transform;

        //fire.Enable();
        //fire.performed += ThrowObject;
    }

    void Update()
    {

        Predict();
        if (isTensioning)
        {
            trajectoryPredictor.SetTrajectoryVisible(true);
        }
        else
        {
            trajectoryPredictor.SetTrajectoryVisible(false);
        }

    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = newArrow.GetComponent<Rigidbody>();

        //float angle = -transform.eulerAngles.x; // Invertimos el ángulo
        //float adjustedForce = force + Mathf.Sin(angle * Mathf.Deg2Rad) * extraForceMultiplier;

        properties.direction = StartPosition.forward;
        properties.initialPosition = StartPosition.position;
        properties.initialSpeed = force;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }

    public void ThrowObject()
    {
        GameObject arrow = Instantiate(newArrow, StartPosition.position, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Vector3 shootDirection = transform.forward;
        rb.AddForce(shootDirection * force, ForceMode.Impulse);

    }

    public void updateIsTensioning(bool _isTensioning)
    {
        isTensioning = _isTensioning;
    }
}