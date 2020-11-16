using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure {

    private World world;
    private Vector3 position;

    private void Start() {

        world = GameObject.Find("World").GetComponent<World>();

    }

    public static Queue<VoxelMod> GenerateMajorFlora (int index, Vector3 position, int minTrunkHeight, int maxTrunkHeight) {

        switch (index) {

            case 0:
                return MakeTree(position, minTrunkHeight, maxTrunkHeight);

            case 1:
                return MakeCacti(position, minTrunkHeight, maxTrunkHeight);

        }

        return new Queue<VoxelMod>();

    }

    public void GenerateCaves(){

        int cavesCount, caveLength;
        //start coordinates
        float caveX, caveY, caveZ;
        //these variables serve as random values to shift caveX,Y,Z. A2 changes A1, B2 changes B1
        float shiftFactorA1, shiftFactorA2, shiftFactorB1, shiftFactorB2;
        //radiusModifier modifies the cave raidus in each iteration
        float caveRadius, radiusModifier;
        //center coordinates for each iteration
        int centerX, centerY, centerZ;

        cavesCount = VoxelData.WorldSizeInChunks / 3;

        for(int i = 0; i < cavesCount; i++){
            
            caveX = (float)(Random.Range(0, VoxelData.WorldWidth));
            caveY = (float)(Random.Range(0, VoxelData.ChunkHeight));
            caveZ = (float)(Random.Range(0, VoxelData.WorldLength));

            caveLength = (int)(Random.Range(2, 31));
            shiftFactorA1 = Random.value * 2f * Mathf.PI;
            shiftFactorA2 = 0f;
            shiftFactorB1 = Random.value * 2f * Mathf.PI;
            shiftFactorB2 = 0f;

            caveRadius = (float)(Random.Range(1, 8));

            for(int j = 0; j < caveLength; j++){

                caveX += (float)Mathf.Sin(shiftFactorA1) * (float)Mathf.Cos(shiftFactorB1);
                caveY += (float)Mathf.Sin(shiftFactorB1);
                caveZ += (float)Mathf.Cos(shiftFactorA1) * (float)Mathf.Cos(shiftFactorB1);

                shiftFactorA1 += shiftFactorA2 * 0.2f;
                shiftFactorA2 *= 0.9f + Random.value - Random.value;
                shiftFactorB1 *= 0.5f + shiftFactorB2 * 0.25f;
                shiftFactorB2 *= 0.75f + Random.value - Random.value;

                if(Random.value < 0.25f) continue;

                centerX = (int)(caveX + (Random.Range(0, 4) - 2) * 0.2f);
                centerY = (int)(caveY + (Random.Range(0, 4) - 2) * 0.2f);
                centerZ = (int)(caveZ + (Random.Range(0, 4) - 2) * 0.2f);

                radiusModifier = (VoxelData.ChunkHeight - centerY) / (float)VoxelData.ChunkHeight;
                radiusModifier = 1.2f + (radiusModifier + 3.5f + 1f) * caveRadius;
                radiusModifier *= Mathf.Sin(j * Mathf.PI / caveLength);

                CarveCaves(centerX, centerY, centerZ, radiusModifier, 0);
            }
        }

    }

    public void CarveCaves(int x, int y, int z, float radius, byte blockID){

        int xBeg = (int)Mathf.Max(x - radius, 0);

        int xEnd = (int)Mathf.Min(x + radius, VoxelData.WorldWidth);

        int yBeg = (int)Mathf.Max(y - radius, 0);

        int yEnd = (int)Mathf.Min(y + radius, VoxelData.ChunkHeight);

        int zBeg = (int)Mathf.Max(z - radius, 0);

        int zEnd = (int)Mathf.Min(z + radius, VoxelData.WorldLength);

        float radiusSq = radius * radius;

        int xx, yy, zz, dx, dy, dz;

        for (yy = yBeg; yy <= yEnd; yy++) { 
            
            dy = yy - y;

            for (zz = zBeg; zz <= zEnd; zz++) { 
                
                dz = zz - z;

                for (xx = xBeg; xx <= xEnd; xx++) { 
                    
                    dx = xx - x;

                    if ((dx * dx + 2 * dy * dy + dz * dz) < radiusSq) {

                        Vector3 position = new Vector3(xx, yy, zz);
                        world.GetChunkFromVector3(position).EditVoxel(position, 0);
                    }

                }

            }

        }
    }

    public static Queue<VoxelMod> MakeTree (Vector3 position, int minTrunkHeight, int maxTrunkHeight) {

        Queue<VoxelMod> queue = new Queue<VoxelMod>();

        int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 250f, 3f));

        if (height < minTrunkHeight)
            height = minTrunkHeight;

        for (int i = 1; i < height; i++)
            queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 6));

        for (int x = -3; x < 4; x++) {
            for (int y = 0; y < 7; y++) {
                for (int z = -3; z < 4; z++) {
                    queue.Enqueue(new VoxelMod(new Vector3(position.x + x, position.y + height + y, position.z + z), 7));
                }
            }
        }

        return queue;

    }

    public static Queue<VoxelMod> MakeCacti (Vector3 position, int minTrunkHeight, int maxTrunkHeight) {

        Queue<VoxelMod> queue = new Queue<VoxelMod>();

        int height = (int)(maxTrunkHeight * Noise.Get2DPerlin(new Vector2(position.x, position.z), 23456f, 2f));

        if (height < minTrunkHeight)
            height = minTrunkHeight;

        for (int i = 1; i <= height; i++)
            queue.Enqueue(new VoxelMod(new Vector3(position.x, position.y + i, position.z), 8));

        return queue;

    }

}
