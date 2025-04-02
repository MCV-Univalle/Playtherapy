using UnityEngine;
//using UnityEngine.InputSystem;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    TrajectoryPredictor trajectoryPredictor;

    [SerializeField]
    Rigidbody objectToThrow;

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
            trajectoryPredictor.SetTrajectoryVisible(true);
        }
    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

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
        Rigidbody thrownObject = Instantiate(objectToThrow, StartPosition.position, Quaternion.identity);

        // Obtener la dirección ajustada del arco
        //float angle = -transform.eulerAngles.x;
        //float adjustedForce = force + Mathf.Sin(angle * Mathf.Deg2Rad) * extraForceMultiplier;

        // Aplicar la fuerza con la dirección correcta
        Vector3 launchDirection = StartPosition.forward;
        // launchDirection.y = Mathf.Sin(angle * Mathf.Deg2Rad); // Ajuste de altura

        thrownObject.AddForce(launchDirection * force, ForceMode.Impulse);
        thrownObject.transform.rotation = Quaternion.LookRotation(thrownObject.velocity.normalized);
    }

    public void updateIsTensioning(bool _isTensioning)
    {
        isTensioning = _isTensioning;
    }
}