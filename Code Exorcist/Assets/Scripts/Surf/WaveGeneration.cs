using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveGeneration : MonoBehaviour 
{

	private TerrainData myTerrainData;
    public int xSize, ySize, zSize;
    public Vector3 worldSize;
	public int resolution;			// number of vertices along X and Z axes
	float[,] heightArray;
    Vector3 originate; 
    public float deltaZ;

    public float ranger1;
    public float ranger2;
   
    float range = 3.28f;

    // keeps track of the time in a way
    int spellTimer = 0;

    // the image array
    public Texture2D[] waveImages;

    // a test image for now, but later we'll be loading up arrays of images (spellcards)
    public Texture2D testImage;

    // the attack coroutines
    Coroutine generateTerrain = null;

    IEnumerator GenerateTerrain(Texture2D[] terrainDataImages)
    {
        while (true)
        {
            if (spellTimer < waveImages.Length)
            {
                FromImage(waveImages[spellTimer]);
                spellTimer += 1;
            }
            else
            {
                spellTimer = 0;
                StopGenerating();
            }
            Debug.Log(spellTimer);
            myTerrainData.SetHeights(0, 0, heightArray);
            yield return new WaitForSeconds(0.04f);
        }
    }

    void Start () 
	{
        myTerrainData = gameObject.GetComponent<TerrainCollider>().terrainData;
        xSize = 200;
        zSize = 200;
        ySize = 100;
        worldSize = new Vector3 (xSize, ySize, zSize);
		myTerrainData.size = worldSize;
		myTerrainData.heightmapResolution = resolution;
		heightArray = new float[resolution, resolution];
        deltaZ = .0002f;

        originate = new Vector3(Random.Range(0.0f, 100.0f), 0, Random.Range(0.0f, 100.0f)) ; // start sampling from a random location in the "Sea of PerlinNoise"

        //Perlin(originate, resolution, range);
        FromImage(waveImages[spellTimer]);

		// Assign values from heightArray into the terrain object's heightmap
		myTerrainData.SetHeights(0, 0, heightArray);
        transform.Translate(-xSize/2, -ySize/2, -zSize/2); //center the Terrain about the origin

        waveImages = Resources.LoadAll<Texture2D>("data32");
    }

    public void Update()
    {
        //originate.z += deltaZ;

        //Perlin(originate, resolution, range);
        if (spellTimer == 0)
        {
            generateTerrain = StartCoroutine(GenerateTerrain(waveImages));
        }
    }
    public bool isGenerating { get { return generateTerrain != null; } }
    public void StopGenerating()
    {
        if (isGenerating)
        {
            StopCoroutine(generateTerrain);
        }
        generateTerrain = null;
    }
    void FromImage(Texture2D image)
    {
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                float height = (.499f + pixel.grayscale);
                //Debug.Log(height);
                heightArray[i, j] = height;
            }
        }
    }

    void Perlin(Vector3 originate, int resolution, float range)
    {

        float xIndex = originate.x;
        float zIndex = originate.z;
        // range = 1.28f; //how far to go out into the "Sea of PerlinNoise" to collect samples
        float stepSize = range / (resolution - 1); //separation between sample spots


        for (int k = 0; k < resolution; k++)
        {
            for (int i = 0; i < resolution; i++)
            {

                xIndex += stepSize;
                float localHeight = Mathf.PerlinNoise(xIndex, zIndex);
                if (localHeight < ranger1)
                {
                    localHeight = ranger2;
                }
                heightArray[k, i] = localHeight;
            }

            xIndex = originate.x;  //reset to sweep out along the beginning of the next row

            zIndex += stepSize;  //step forward to the next row
        }
    }

    public void ShiftOriginate(float xIncrement)
    {
        //if (xIncrement != 0f)
        originate.x += xIncrement;
       
        Perlin(originate, resolution, range);
        myTerrainData.SetHeights(0, 0, heightArray);
    }

    public void AdjustSpeed(bool faster)
    {
        if (faster)
            deltaZ += .00001f;
        else
            deltaZ -= .00001f;
    }

    public void AdjustHeight(int deltaHeight)
    {
        ySize += deltaHeight;
        if (ySize < 0) ySize = 0;
        worldSize.y = ySize;
        myTerrainData.size = worldSize;
        transform.position = new Vector3(-xSize / 2, -ySize / 2, -zSize / 2);  //this re-centers the Terrain around the origin
    }

    public void ReRange(bool stretch)
    {
        if (stretch)
            range = range * 1.10f; //10% increase in range
        else
            range = range * .90f; //10% decrease in range
        Perlin(originate, resolution, range);
        myTerrainData.SetHeights(0, 0, heightArray);
    }

}