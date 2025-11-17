using UnityEngine;

public class Duplicator : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _forceStrength = 5;
    
    private void Update()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject target = hit.collider.gameObject;
                Vector3 targetScale = hit.collider.transform.localScale;
                CubeBehaviour targetCubeBehaviour = target.GetComponent<CubeBehaviour>();
                float targetChanceToDuplicate = targetCubeBehaviour._chanceToDuplicate;

                DuplicateCube(target, targetChanceToDuplicate, targetScale, hit);
            }
        }
    }

    private void DuplicateCube(GameObject cube, float chance, Vector3 scale, RaycastHit hit)
    {
        if (Random.value <= chance)
        {
            Destroy(cube);

            int spawnCount = Random.Range(2, 7);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * 0.3f; 
                if(randomPosition.z < 0)
                    randomPosition.z = -randomPosition.z;
                
                GameObject obj = Instantiate(_cubePrefab, hit.point + randomPosition, Quaternion.identity);
                CubeBehaviour cubeBehaviour = obj.GetComponent<CubeBehaviour>();
                
                cubeBehaviour.SetUpCube(scale, chance);
                
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.AddForce(Random.Range(-_forceStrength, _forceStrength),
                    Random.Range(-_forceStrength, _forceStrength),
                    Random.Range(-_forceStrength, _forceStrength), ForceMode.Impulse);
            }
        }
        else
        {
            Destroy(cube);
        }
    }
}
