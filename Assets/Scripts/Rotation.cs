using UnityEngine;

public class Rotation : MonoBehaviour
{
   [SerializeField] private float rotateSpeed = 10f;
   
   private void Update()
   {
      transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
      transform.LookAt(Vector3.zero);
   }
}
