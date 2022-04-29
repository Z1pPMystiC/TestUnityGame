using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupMaxAmmo : MonoBehaviour
{
    public TextMeshProUGUI centerText;
    public RaycastGun tesla;
    public FloppyLauncherScript floppyLauncher;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 100f * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            floppyLauncher.ammoLeft = floppyLauncher.fullAmmo;
            tesla.ammoLeft = tesla.fullAmmo;
            centerText.SetText("Max Ammo.\nAmmo has been replenished");
            Invoke("ClearText", 2f);
            gameObject.SetActive(false);
        }
    }

    public void ClearText()
    {
        if (centerText != null)
        {
            centerText.SetText("");
        }
        Destroy(gameObject);
    }
}
