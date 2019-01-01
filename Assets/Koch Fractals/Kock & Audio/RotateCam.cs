using UnityEngine;

public class RotateCam : MonoBehaviour
{
    public AudioPeer audioPeer;
    public Vector3 rotateAxis, rotateSpeed;

    private void Update()
    {
        transform.GetChild(0).transform.LookAt(this.transform); // get camera

        this.transform.Rotate(
            rotateAxis.x * rotateSpeed.x * Time.deltaTime * AudioPeer.amplitudeBuffer,
            rotateAxis.y * rotateSpeed.y * Time.deltaTime * AudioPeer.amplitudeBuffer,
            rotateAxis.z * rotateSpeed.z * Time.deltaTime * AudioPeer.amplitudeBuffer
            );
    }
}
