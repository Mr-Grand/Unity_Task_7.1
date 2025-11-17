using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Duplicator : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _forceStrength = 5;
    [SerializeField, Range(0.1f, 2f)] private float _timeToDestroy = 0.5f;
    
    private Coroutine _cleanerCoroutine;
    private List<GameObject> _cubes = new List<GameObject>();

    private IEnumerator CleanerCoroutine()
    {
        while (true)
        {
             yield return new WaitForSeconds(_timeToDestroy);
             ClearObjects();
        }
    }

    // Из-за малого размера кубов 0.25 и ниже, они часто пролетают через коллайдеры стен и пола
    // в интернете говорят, что это частая проблема и я не смог её решить, поэтому
    // добавил метод удаления кубов, упавших ниже оси пола, то есть нуля.
    // Точку минимума указал 0, но можно добавить объект пола и считывать с него эти координаты
    private void Awake()
    {
        StartCoroutine(CleanerCoroutine());
    }
    
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
                float targetChanceToDuplicate = targetCubeBehaviour.ChanceToDuplicate;

                DuplicateCube(target, targetChanceToDuplicate, targetScale, hit);
            }
        }
    }

    private void DuplicateCube(GameObject cube, float chance, Vector3 scale, RaycastHit hit)
    {
        if (Random.value <= chance)
        {
            Destroy(cube);
            _cubes.Remove(cube);
            
            List<GameObject> temporaryCubes = new List<GameObject>();

            int spawnCount = Random.Range(2, 7);
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * 0.3f; 
                if(randomPosition.z < 0)
                    randomPosition.z = -randomPosition.z;
                
                GameObject obj = Instantiate(_cubePrefab, hit.point + randomPosition, Quaternion.identity);
                
                CubeBehaviour cubeBehaviour = obj.GetComponent<CubeBehaviour>();
                cubeBehaviour.SetUpCube(scale, chance);
                
                temporaryCubes.Add(obj);
                
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.AddForce(Random.Range(-_forceStrength, _forceStrength),
                    Random.Range(-_forceStrength, _forceStrength),
                    Random.Range(-_forceStrength, _forceStrength), ForceMode.Impulse);
            }
            
            foreach(GameObject cubeX in temporaryCubes)
                _cubes.Add(cubeX);
            temporaryCubes.Clear();
        }
        else
        {
            Destroy(cube);
            _cubes.Remove(cube);
        }
    }

    private void ClearObjects()
    {
        foreach (GameObject cube in _cubes)
        {
            if (cube.transform.position.y < 0)
            {
                Debug.Log("Cube pierce collider and was deleted");
                Destroy(cube);
            }
        }
        _cubes.RemoveAll(cube => cube.transform.position.y < 0);
    }
}
