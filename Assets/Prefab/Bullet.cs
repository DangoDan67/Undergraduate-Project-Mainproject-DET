using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private World world;
    // Start is called before the first frame update

    public int dirtBulletDamage = 100;
    public int stone1BulletDamage = 150;
    public int stone2BulletDamage = 150;
    public int woodBulletDamage = 125;
    public int iceBulletDamage = 100;
    public int fireBulletDamage = 250;
    public int corrosiveBulletDamage = 40;  // das wird 5 mal separat ausgeführt d.h die insgesamte demage ist 100

    public enum Weapon { Dirt, Stone1, Stone2, Wood, IceBlock, FireBlock, CorrosiveBlock };

    public Weapon currentWeapon;
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        checkCollider();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
        {
            switch (currentWeapon)
            {
                case Weapon.Dirt:
                    demageEnemy(dirtBulletDamage, collision);
                    break;
                case Weapon.Stone1:
                    demageEnemy(stone1BulletDamage, collision);
                    break;
                case Weapon.Stone2:
                    demageEnemy(stone2BulletDamage, collision);
                    break;
                case Weapon.Wood:
                    demageEnemy(woodBulletDamage, collision);
                    break;
                case Weapon.IceBlock:
                    collision.collider.gameObject.GetComponent<EnemyController>().freezEnemy();
                    demageEnemy(iceBulletDamage, collision);
                    break;
                case Weapon.FireBlock:
                    demageEnemy(fireBulletDamage, collision);
                    break;
                case Weapon.CorrosiveBlock:
                    demageEnemy(corrosiveBulletDamage, collision);
                    break;


            }
        }

    }



    void demageEnemy(int demage, Collision collision)
    {
        GetComponent<Collider>().enabled = false;
        EnemyBehavior enemy = collision.collider.gameObject.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            if (currentWeapon == Weapon.CorrosiveBlock)
            {
                enemy.TakeDemageContinously(corrosiveBulletDamage);
            }
            else
            {
                enemy.TakeDamage(demage);
            }
            
        }
        Destroy(gameObject);
    }

    void checkCollider()
    {
        // bool up = checkUpSpeed(0.1f);
        //bool down = checkDownSpeed(0.1f);
        bool fr = front(0.1f);
        bool inWorld = IsBulletInWorld(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        if (fr || !inWorld)
        {
            Destroy(gameObject);
        }
    }

    bool IsBulletInWorld(Vector3 pos)
    {

        if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
            return true;
        else
            return false;

    }


    private bool checkDownSpeed(float downSpeed)
    {

        if (
            world.CheckForVoxel(new Vector3(transform.position.x - downSpeed, transform.position.y + downSpeed, transform.position.z - downSpeed)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + downSpeed, transform.position.y + downSpeed, transform.position.z - downSpeed)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + downSpeed, transform.position.y + downSpeed, transform.position.z + downSpeed)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - downSpeed, transform.position.y + downSpeed, transform.position.z + downSpeed))
           )
        {

            return true;

        }
        else
        {
            return false;

        }

    }

    private bool checkUpSpeed(float upSpeed)
    {

        if (
            world.CheckForVoxel(new Vector3(transform.position.x - upSpeed, transform.position.y + 2f + upSpeed, transform.position.z - upSpeed)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + upSpeed, transform.position.y + 2f + upSpeed, transform.position.z - upSpeed)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + upSpeed, transform.position.y + 2f + upSpeed, transform.position.z + upSpeed)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - upSpeed, transform.position.y + 2f + upSpeed, transform.position.z + upSpeed))
           )
        {

            return true;

        }
        else
        {

            return false;

        }

    }

    private bool front(float width)
    {
        if (
            world.CheckForVoxel(new Vector3(transform.position.x, transform.position.y,
                transform.position.z + width)) ||
            world.CheckForVoxel(new Vector3(transform.position.x, transform.position.y + 1f,
                transform.position.z + width))
        )
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void changeWeapon(Weapon weapon)
    {
        this.currentWeapon = weapon;
    }

}
