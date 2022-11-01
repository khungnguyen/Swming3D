using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MSLIMA.Serializer;

public struct IBoneDataPackage
{
    public int index;
    public Vector3 rotation;
    public Vector3 position;
    public static object Deserialize(byte[] bytes)
    {
        IBoneDataPackage handData = new IBoneDataPackage();
        int offset = 0;
        handData.index = Serializer.DeserializeInt(bytes, ref offset);
        handData.rotation = Serializer.DeserializeVector3(bytes, ref offset);
        handData.position = Serializer.DeserializeVector3(bytes, ref offset);
        return handData;
    }

    public static byte[] Serialize(object customType)
    {
        var c = (IBoneDataPackage)customType;
        byte[] bytes = new byte[0];
        Serializer.Serialize(c.index, ref bytes);
        Serializer.Serialize(c.rotation, ref bytes);
        Serializer.Serialize(c.position, ref bytes);
        return bytes;
    }
}
public class HandSynchronization : MonoBehaviourPun, IPunObservable
{
    public List<Transform> fingerBones = new List<Transform>();

    public static bool registerCustomType = false;

    const int Max = 2;

    private float speed = 2f;

    const bool USE_PHOTON_VIEW = false;
    public void Awake()
    {
        if (!registerCustomType)
        {
            registerCustomType = true;
            Serializer.RegisterCustomType<IBoneDataPackage>((byte)'A');
        }
    }
    void Start()
    {
        if (USE_PHOTON_VIEW)
        {
            enabled = false;

        }
        else
        {
            RemoveAllPhotonView();
            RemoveAllPhotonTransformView();
        }

    }
    void RemoveAllPhotonView()
    {
        PhotonView[] allView = transform.GetComponentsInChildren<PhotonView>();
        new List<PhotonView>(allView).ForEach(e =>
        {
            if (e.transform.name != transform.name)
            {
                Destroy(e);
            }

        });
    }
    void RemoveAllPhotonTransformView()
    {
        PhotonTransformView[] allView = transform.GetComponentsInChildren<PhotonTransformView>();
        new List<PhotonTransformView>(allView).ForEach(e =>
        {
            if (e.transform.name != transform.name)
            {
                Destroy(e);
            }

        });
    }
    /**
    * Send/Recieve a all finger bones as array data
    */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            int index = 0;
            fingerBones.ForEach(e =>
            {
                IBoneDataPackage boneData = new IBoneDataPackage();
                boneData.index = index;
                boneData.rotation = e.rotation.eulerAngles;
                boneData.position = e.position;
                stream.SendNext(boneData);
                index++;
            });
        }
        else // interpolation
        {
            fingerBones.ForEach(e =>
            {
                IBoneDataPackage boneData = (IBoneDataPackage)stream.ReceiveNext();

                e.SetPositionAndRotation(
                    Vector3.Lerp(e.position, boneData.position, 1)
                    , Quaternion.Lerp(e.rotation, Quaternion.Euler(boneData.rotation)
                    , 1));
            });
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            speed += 0.5f;
            Debug.LogError("Increase speed" + speed);
        }
    }

}
