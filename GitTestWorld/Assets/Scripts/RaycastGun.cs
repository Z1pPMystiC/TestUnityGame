using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaycastGun : MonoBehaviour
{
    public Camera playerCamera;
    public Transform laserOrigin;
    public GameObject particles;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;
    public List<GameObject> myList = new List<GameObject>();
    public GameObject laserEnd;
    public float maxCooldown;
    public float cooldown;

    public float reloadTime;
    public int magazineSize, ammoLeft, fullAmmo;
    public int bulletsLeft, bulletsShot;
    public bool shooting, readyToShoot, reloading;

    public TextMeshProUGUI ammoDisplay;
    public playerMotor playerMotor;

    public PauseMenu pauseMenu;

    LineRenderer laserLine;
    float fireTimer;

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        bulletsLeft = magazineSize;
        ammoLeft = fullAmmo;
        readyToShoot = true;
        cooldown = maxCooldown;
        particles.SetActive(false);
    }

    private void Update()
    {
        if (!pauseMenu.gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && ammoLeft > 0)
            {
                Reload();
            }

            if (readyToShoot && !reloading && bulletsLeft > 0)
            {
                bulletsShot = 0;

                Shoot();
            }

            if (readyToShoot && !reloading && bulletsLeft <= 0)
            {
                FindObjectOfType<AudioManager>().Play("EmptyMag");
                Invoke("StopEmpty", 0.5f);
                laserLine.enabled = false;
                particles.SetActive(false);
                laserEnd.transform.position = new Vector3(0, 0, 0);
                myList.Clear();
            }

            if (ammoDisplay != null)
            {
                ammoDisplay.SetText(bulletsLeft + " / " + ammoLeft);
            }

            if (myList.Count > 0)
            {
                foreach (GameObject gameobject in myList)
                {
                    if (gameobject != null)
                    {
                        if (gameobject.tag == "Enemy")
                        {
                            var robot = gameobject.GetComponent<RobotMotion>();
                            robot.AttackEnemy(robot.teslaDamage);
                            playerMotor.enemyHit = true;
                        }
                        if (gameobject.tag == "Boss")
                        {
                            var boss = gameobject.GetComponent<BossMotion>();
                            boss.AttackEnemy(boss.teslaDamage);
                            playerMotor.enemyHit = true;
                        }
                    }
                    else
                    {
                        myList.Clear();
                        break;
                    }
                }
            }
        }
    }

    private void Reload()
    {
        laserLine.enabled = false;
        particles.SetActive(false);
        laserEnd.transform.position = new Vector3(0, 0, 0);
        myList.Clear();
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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            laserLine.enabled = true;
            particles.SetActive(true);
        }
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    public void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            laserLine.enabled = true;
            particles.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                laserEnd.transform.position = hit.point;
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
                laserEnd.transform.position = rayOrigin + (playerCamera.transform.forward * gunRange);
            }
            if(cooldown <= 0)
            {
                cooldown = maxCooldown;
                bulletsLeft--;
            }
            cooldown -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            laserLine.enabled = false;
            particles.SetActive(false);
            laserEnd.transform.position = new Vector3(0, 0, 0);
            myList.Clear();
        }
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
