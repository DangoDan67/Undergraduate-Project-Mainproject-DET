﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Player : MonoBehaviour {

    public bool isGrounded;
    public bool isSprinting;

    private Transform cam;
    private World world;

    public GameObject stone1Prefab;
    public GameObject stone2Prefab;
    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject woodPrefab;
    public GameObject leavesPrefab;
    public GameObject icePrefab;
    public GameObject firePrefab;
    public GameObject corrosivePrefab;


    

    public float walkSpeed = 3.5f;
    public float sprintSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    public float playerWidth = 0.15f;
    public float boundsTolerance = 0.1f;

    private float horizontal;
    private float vertical;
    private float mouseHorizontal;
    private float mouseVertical;
    private Vector3 velocity;
    private float verticalMomentum = 0;
    private bool jumpRequest;

    public Transform highlightBlock;
    public Transform placeBlock;
    public float checkIncrement = 0.1f;
    public float reach = 8f;

    public Toolbar toolbar;
    public Transform firePoint;

    private void Start() {

        cam = GameObject.Find("Main Camera").transform;
        world = GameObject.Find("World").GetComponent<World>();

        world.inUI = false;

    }

    private void FixedUpdate() {
        
        if (!world.inUI) {

            CalculateVelocity();
            if (jumpRequest)
                Jump();

            transform.Rotate(Vector3.up * mouseHorizontal * world.settings.mouseSensitivity);
            cam.Rotate(Vector3.right * -mouseVertical * world.settings.mouseSensitivity);
            transform.Translate(velocity, Space.World);

        }

    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.I)) {

            world.inUI = !world.inUI;

        }

        if (!world.inUI) {
            GetPlayerInputs();
            placeCursorBlocks();
        }

    }

    void Jump () {

        verticalMomentum = jumpForce;
        isGrounded = false;
        jumpRequest = false;

    }


    private void CalculateVelocity () {

        // Affect vertical momentum with gravity.
        if (verticalMomentum > gravity)
            verticalMomentum += Time.fixedDeltaTime * gravity;

        // if we're sprinting, use the sprint multiplier.
        if (isSprinting)
            velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * sprintSpeed;
        else
            velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * walkSpeed;

        // Apply vertical momentum (falling/jumping).
        velocity += Vector3.up * verticalMomentum * Time.fixedDeltaTime;

        if ((velocity.z > 0 && front) || (velocity.z < 0 && back))
            velocity.z = 0;
        if ((velocity.x > 0 && right) || (velocity.x < 0 && left))
            velocity.x = 0;

        if (velocity.y < 0)
            velocity.y = checkDownSpeed(velocity.y);
        else if (velocity.y > 0)
            velocity.y = checkUpSpeed(velocity.y);


    }

    private void GetPlayerInputs () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Sprint"))
            isSprinting = true;
        if (Input.GetButtonUp("Sprint"))
            isSprinting = false;

        if (isGrounded && Input.GetButtonDown("Jump"))
            jumpRequest = true;

        if (highlightBlock.gameObject.activeSelf) {

            // Destroy block.
            if (Input.GetMouseButtonDown(0))
            {
                byte destroyedVoxelId = world.GetVoxelState(highlightBlock.position).id;
                world.GetChunkFromVector3(highlightBlock.position).EditVoxel(highlightBlock.position, 0);
                switch (destroyedVoxelId)
                {
                    case 2:
                        GameObject stone1Item = Instantiate(stone1Prefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody stone1rb = stone1Item.GetComponent<Rigidbody>();
                        stone1rb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(stone1Item, 1.5f);                    
                        break;
                    case 3:
                        GameObject grassItem = Instantiate(dirtPrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody grassrb = grassItem.GetComponent<Rigidbody>();
                        grassrb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(grassItem, 1.5f);
                        break;
                    case 4:
                        GameObject sandItem = Instantiate(sandPrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody sandrb = sandItem.GetComponent<Rigidbody>();
                        sandrb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(sandItem, 1.5f);                 
                        break;
                    case 5:
                        GameObject dirtItem = Instantiate(dirtPrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody dirtrb = dirtItem.GetComponent<Rigidbody>();
                        dirtrb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(dirtItem, 1.5f);                    
                        break;
                    case 6:
                        GameObject woodItem = Instantiate(woodPrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody woodrb = woodItem.GetComponent<Rigidbody>();
                        woodrb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse); 
                        Destroy(woodItem, 1.5f);                   
                        break;
                    case 7:
                        GameObject leavesItem = Instantiate(leavesPrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody leavesrb = leavesItem.GetComponent<Rigidbody>();
                        leavesrb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse); 
                        Destroy(leavesItem, 1.5f);                   
                        break;
                    case 8:
                        GameObject stone2Item = Instantiate(stone2Prefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody stone2rb = stone2Item.GetComponent<Rigidbody>();
                        stone2rb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse); 
                        Destroy(stone2Item, 1.5f);                   
                        break;
                    case 9:
                        GameObject iceItem = Instantiate(icePrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody icerb = iceItem.GetComponent<Rigidbody>();
                        icerb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(iceItem, 1.5f);                   
                        break;
                    case 10:
                        GameObject fireItem = Instantiate(firePrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody firerb = fireItem.GetComponent<Rigidbody>();
                        firerb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(fireItem, 1.5f);                   
                        break;
                    case 11:
                        GameObject corrosiveItem = Instantiate(corrosivePrefab, highlightBlock.position, highlightBlock.rotation);
                        Rigidbody corrosiverb = corrosiveItem.GetComponent<Rigidbody>();
                        corrosiverb.AddForce(-firePoint.forward * 1.5f, ForceMode.Impulse);
                        Destroy(corrosiveItem, 1.5f);                   
                        break;
                }
                addBlockToToolBar(destroyedVoxelId);
            }
              

            // Place block.
            if (Input.GetMouseButtonDown(1)) {
                if (toolbar.slots[toolbar.slotIndex].HasItem) {
                    // if(toolbar.slots[toolbar.slotIndex].itemSlot.stack.id == 3 || toolbar.slots[toolbar.slotIndex].itemSlot.stack.id == 5){
                    //     world.GetChunkFromVector3(placeBlock.position).EditVoxel(placeBlock.position, 5);
                    //     toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                    // }
                    world.GetChunkFromVector3(placeBlock.position).EditVoxel(placeBlock.position, toolbar.slots[toolbar.slotIndex].itemSlot.stack.id);
                    toolbar.slots[toolbar.slotIndex].itemSlot.Take(1);
                }
            }
        }

    }

    private void addBlockToToolBar(byte voxelId)
    {
        for (int i = 0; i < toolbar.slots.Length; i++)
        {
            if (toolbar.slots[i].HasItem)
            {
                if (toolbar.slots[i].itemSlot.stack.id == voxelId || toolbar.slots[i].itemSlot.stack.id == 3 && voxelId == 5 || toolbar.slots[i].itemSlot.stack.id == 5 && voxelId ==3)
                {
                    toolbar.slots[i].itemSlot.Put(1);
                    return;
                }
            }
        }

        // wenn das Item nicht im toolbar gefunden wurde 
        for (int i = 0; i < toolbar.slots.Length; i++)
        {
            if (!toolbar.slots[i].HasItem)
            {
                toolbar.slots[i].itemSlot.InsertStack(new ItemStack(voxelId, 1));
                return;
            }
        }
    }

    private void placeCursorBlocks () {

        float step = checkIncrement;
        Vector3 lastPos = new Vector3();

        while (step < reach) {

            Vector3 pos = cam.position + (cam.forward * step);

            if (world.CheckForVoxel(pos)) {

                highlightBlock.position = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
                placeBlock.position = lastPos;

                highlightBlock.gameObject.SetActive(true);
                placeBlock.gameObject.SetActive(true);

                return;

            }

            lastPos = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

            step += checkIncrement;

        }

        highlightBlock.gameObject.SetActive(false);
        placeBlock.gameObject.SetActive(false);

    }

    private float checkDownSpeed (float downSpeed) {

        if (
            world.CheckForVoxel(new Vector3(transform.position.x - playerWidth, transform.position.y + downSpeed, transform.position.z - playerWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + playerWidth, transform.position.y + downSpeed, transform.position.z - playerWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + playerWidth, transform.position.y + downSpeed, transform.position.z + playerWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - playerWidth, transform.position.y + downSpeed, transform.position.z + playerWidth))
           ) {

            isGrounded = true;
            return 0;

        } else {

            isGrounded = false;
            return downSpeed;

        }

    }

    private float checkUpSpeed (float upSpeed) {

        if (
            world.CheckForVoxel(new Vector3(transform.position.x - playerWidth, transform.position.y + 1.5f + upSpeed, transform.position.z - playerWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + playerWidth, transform.position.y + 1.5f + upSpeed, transform.position.z - playerWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x + playerWidth, transform.position.y + 1.5f + upSpeed, transform.position.z + playerWidth)) ||
            world.CheckForVoxel(new Vector3(transform.position.x - playerWidth, transform.position.y + 1.5f + upSpeed, transform.position.z + playerWidth))
           ) {

            return 0;

        } else {

            return upSpeed;

        }

    }

    public bool front {

        get {
            if (
                world.CheckForVoxel(new Vector3(transform.position.x, transform.position.y, transform.position.z + playerWidth)) ||
                world.CheckForVoxel(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + playerWidth))
                )
                return true;
            else
                return false;
        }

    }
    public bool back {

        get {
            if (
                world.CheckForVoxel(new Vector3(transform.position.x, transform.position.y, transform.position.z - playerWidth)) ||
                world.CheckForVoxel(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z - playerWidth))
                )
                return true;
            else
                return false;
        }

    }
    public bool left {

        get {
            if (
                world.CheckForVoxel(new Vector3(transform.position.x - playerWidth, transform.position.y, transform.position.z)) ||
                world.CheckForVoxel(new Vector3(transform.position.x - playerWidth, transform.position.y + 1f, transform.position.z))
                )
                return true;
            else
                return false;
        }

    }
    public bool right {

        get {
            if (
                world.CheckForVoxel(new Vector3(transform.position.x + playerWidth, transform.position.y, transform.position.z)) ||
                world.CheckForVoxel(new Vector3(transform.position.x + playerWidth, transform.position.y + 1f, transform.position.z))
                )
                return true;
            else
                return false;
        }

    }

}