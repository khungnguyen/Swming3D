using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MPlayerController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject viewPoint;
    public float mouseSensitivy = 1f;
    public float moveSpeed = 10f;

    [SerializeField]
    CharacterController charContr;

    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    Transform checkGroundTrans;

    [SerializeField]
    LayerMask layerGroundMask;

    Camera camera;
    private Vector3 movementNomalize;
    private Vector3 velocity;

    private bool isGrounded = false;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            camera = Camera.main;
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Debug.Log("Mouse Click");
            }
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Look();
                Movement();
                Shoot();
            }
        }

    }
    private void Look()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivy;
        //rotateion Up an Down
        // Only rotate Viewport
        viewPoint.transform.rotation = Quaternion.Euler(viewPoint.transform.rotation.eulerAngles.x - direction.y, viewPoint.transform.rotation.eulerAngles.y, viewPoint.transform.rotation.eulerAngles.z);

        Vector3 curEulers = transform.rotation.eulerAngles;
        //rotate boday left and right
        transform.rotation = Quaternion.Euler(curEulers.x, curEulers.y + direction.x, curEulers.z);
        camera.gameObject.transform.SetPositionAndRotation(viewPoint.transform.position, viewPoint.transform.rotation);
    }
    private void Movement()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = (transform.forward * input.z + transform.right * input.x).normalized;
        float currentY = velocity.y;
        velocity = direction * moveSpeed;
        velocity.y = currentY;
        float jump = 0;
        isGrounded = Physics.Raycast(checkGroundTrans.position, Vector3.down, 0.2f, layerGroundMask);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump = jumpForce;
        }
        velocity.y += jump + Physics.gravity.y * Time.deltaTime;
        if (charContr.isGrounded)
        {
            velocity.y = 0;
        }
        charContr.Move(velocity * Time.deltaTime);
    }
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(camera.rect.width / 2, camera.rect.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.gameObject.name);
                PhotonNetwork.Instantiate("ImpactVFX", hit.point, Quaternion.identity);
                if (hit.collider.gameObject.tag == "Player")
                {
                    hit.collider.gameObject.GetPhotonView()?.RPC("TakeDame", RpcTarget.All, photonView.Owner.NickName);
                    Destroy(hit.collider.gameObject,3);
                }

            }

        }

    }
    [PunRPC]
    public void TakeDame(string m)
    {
        Debug.Log("Hit by " + m);
    }
}
