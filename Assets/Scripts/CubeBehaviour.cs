using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    [SerializeField] private float _chanceToDuplicate;
    [SerializeField] private float _scaleMultiplier;
    
    public float ChanceToDuplicate => _chanceToDuplicate;
    
    public void SetUpCube(Vector3 targetScale, float targetChanceToDuplicate)
    {
        Vector3 newScale = new Vector3(targetScale.x / 2, targetScale.y / 2, targetScale.z / 2);
        transform.localScale = newScale;
        _chanceToDuplicate = targetChanceToDuplicate / 2;
        GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
}
