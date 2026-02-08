using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [SerializeField] private GameObject whole;
    [SerializeField] private GameObject destroyed;

    [Header("Explosion")]
    [SerializeField] private float force = 500f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float upward = 0.5f;
    [ContextMenu("Destroy Wall")]
    private void DestroyFromMenu()
    {
        DestroyWall();
    }
    public void DestroyWall()
    {
        whole.SetActive(false);
        destroyed.SetActive(true);

        foreach (var rb in destroyed.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(
                force,
                transform.position,
                radius,
                upward,
                ForceMode.Impulse
            );
        }
    }
}
