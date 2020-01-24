using UnityEngine;

public class Hand : MonoBehaviour
{
    public Transform PlayerCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.GetComponent<Chunk>())
                {
                    Vector3 pos = hit.point - hit.normal / 2;
                    pos.x = Mathf.FloorToInt(pos.x);
                    pos.y = Mathf.FloorToInt(pos.y);
                    pos.z = Mathf.FloorToInt(pos.z);

                    World.instance.DamageBlockAt(pos, 1);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.GetComponent<Chunk>())
                {
                    Vector3 pos = hit.point + hit.normal / 2;
                    pos.x = Mathf.FloorToInt(pos.x);
                    pos.y = Mathf.FloorToInt(pos.y);
                    pos.z = Mathf.FloorToInt(pos.z);

                    if (!Physics.CheckBox(pos + Vector3.one / 2, Vector3.one / 4))
                        World.instance.SetBlockAt(pos, 1);
                }
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            /*
            RaycastHit hit;
            Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.GetComponent<Chunk>())
                {
                    Debug.Log(hit.textureCoord);
                    Debug.Log(hit.textureCoord2);
                }
            }
            */
        }
    }
}
