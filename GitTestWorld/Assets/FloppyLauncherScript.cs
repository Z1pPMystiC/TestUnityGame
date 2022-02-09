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
    public PauseMenu pauseMenu;
    
    private void Awake()
    {
        bulletsLeft = magazineSize;
        ammoLeft = fullAmmo;
        readyToShoot = true;
    }
    void Update()
    {
        if (!pauseMenu.gameIsPaused)
        {
            MyInput();
        }

        if(ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft + " / " + ammoLeft);
        }
    }

    private void MyInput()
    {
        shooting = Input.GetKeyDown(KeyCode.Mouse0);
        
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && ammoLeft > 0)
        {
            Reload();
        }
        
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            FindObjectOfType<AudioManager>().Play("EmptyMag");
            Invoke("StopEmpty", 0.5f);
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

        FindObjectOfType<AudioManager>().Play("ProjectileShot");

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
        PlayForTime(reloadTime);
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        if (ammoLeft < magazineSize)
        {
            bulletsLeft += ammoLeft;
            ammoLeft = 0;
        }
        else
        {
            ammoLeft -= (magazineSize - bulletsLeft);
            bulletsLeft = magazineSize;
        }
        reloading = false;
    }

    public void PlayForTime(float time)
    {
        FindObjectOfType<AudioManager>().Play("Reload");
        Invoke("StopAudio", time);
 }

    private void StopAudio()
    {
        FindObjectOfType<AudioManager>().Stop("Reload");
    }

    private void StopEmpty()
    {
        FindObjectOfType<AudioManager>().Stop("EmptyMag");
    }
}

