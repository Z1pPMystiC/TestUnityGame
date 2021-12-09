using UnityEngine;
using TMPro;

public class FloppyLauncherScript : MonoBehaviour
{
    public GameObject projectile;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap, ammoLeft, fullAmmo;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public bool shooting, readyToShoot, reloading;
    public bool allowInvoke = true;

    public Camera Camera;
    public Transform attackPoint;
    public Animator weaponAnim;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoDisplay;
    
    private void Awake()
    {
        bulletsLeft = magazineSize;
        ammoLeft = fullAmmo;
        readyToShoot = true;
    }
    void Update()
    {
        MyInput();

        if(ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft + " / " + ammoLeft);
        }
    }

    private void MyInput()
    {
        shooting = Input.GetKeyDown(KeyCode.Mouse0);
        
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
        
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithoutSpread.normalized;
        currentBullet.transform.up = new Vector3(0f, -1f, 0f);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
   
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        ammoLeft -= (magazineSize - bulletsLeft);
        bulletsLeft = magazineSize;
        reloading = false;
    }
    
}

