using UnityEngine;
using UnityEngine.InputSystem;

public class WaterGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;

    public void FireBullet(InputAction.CallbackContext context)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRigidbody = bulletObject.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = -bulletSpawnPoint.up * bulletSpeed;
    }
}
