using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject dirtBulletPrefab;
    public GameObject stone1BulletPrefab;
    public GameObject stone2BulletPrefab;
    public GameObject woodBulletPrefab;
    public GameObject iceBulletPrefab;
    public GameObject fireBulletPrefab;
    public GameObject corrosiveBulletPrefab;


    public Transform firePoint;

    public Toolbar toolbar;

    public PlayerEnergie playerEnergie;

    // Start is called before the first frame update
    void Start()
    {
        //cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            shoot();
        }

    }

    void shoot()
    {
        if (playerEnergie.getEnergie() >= 20)
        {
            if (toolbar.slots[toolbar.slotIndex].HasItem)
            {
                byte voxelId = toolbar.slots[toolbar.slotIndex].itemSlot.stack.id;
                switch (voxelId)
                {
                    case 2:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(stone1BulletPrefab, Bullet.Weapon.Stone1);
                        break;
                    case 3:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(dirtBulletPrefab, Bullet.Weapon.Dirt);
                        break;
                    case 5:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(dirtBulletPrefab, Bullet.Weapon.Dirt);
                        break;
                    case 6:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(woodBulletPrefab, Bullet.Weapon.Wood);
                        break;
                    case 8:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(stone2BulletPrefab, Bullet.Weapon.Stone2);
                        break;
                    case 9:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(iceBulletPrefab, Bullet.Weapon.IceBlock);
                        break;
                    case 10:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(fireBulletPrefab, Bullet.Weapon.FireBlock);
                        break;
                    case 11:
                        toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                        initiateAndShootBullet(corrosiveBulletPrefab, Bullet.Weapon.CorrosiveBlock);
                        break;
                }

            }
        }

    }
    void initiateAndShootBullet(GameObject bulletPrefab, Bullet.Weapon weapon)
    {
        playerEnergie.subtractEnergie(20);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().changeWeapon(weapon);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * 3f, ForceMode.Impulse);
    }

}