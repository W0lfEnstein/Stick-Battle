using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public float inertialSpeed;
    public Vector3 maxDistance;

    private Vector3 defaultPosition;

    private PlayerController pc;
    private Transform trans;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        trans = GetComponent<Transform>();

        defaultPosition = trans.localPosition;
    }

    private void Update()
    {
        #region Inertial Movement Settings - Update

        if (pc.Directing != Vector3.zero || pc.Rotating != Vector2.zero)
        {
            Vector3 inertialDirection = new Vector3(Mathf.Clamp(pc.Directing.x + pc.Rotating.x, -1, 1), Mathf.Clamp(pc.Directing.y + pc.Rotating.y, -1, 1), pc.Directing.z);
            Vector3 inertialPosition = new Vector3
                (defaultPosition.x - maxDistance.x * inertialDirection.x, defaultPosition.y - maxDistance.y * inertialDirection.y, defaultPosition.z - maxDistance.z * inertialDirection.z);

            trans.localPosition = Vector3.Lerp(trans.localPosition, inertialPosition, inertialSpeed * Time.deltaTime); 
        }
        else
        {
            trans.localPosition = Vector3.Lerp(trans.localPosition, defaultPosition, inertialSpeed * Time.deltaTime);
        }

        #endregion
    }
}
