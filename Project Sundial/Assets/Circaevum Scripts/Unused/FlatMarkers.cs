using UnityEngine;
using System.Collections;

public class FlatMarkers : MonoBehaviour
{

    /// <summary>
    /// Input path coordinates here
    /// Coordinates are in 3D space, so specify a X, Y and Z values for each path coordinate
    /// </summary>
    public Vector3[] coordinates;
    private Sundial sundial;
    /// <summary>
    /// Path size
    /// Change this if you want to scale path size
    /// Default value of 1 unit
    /// </summary>
    public float pathSize = 0.1f;

    /// <summary>
    /// Mesh filter holds information about the mesh
    /// </summary>
    MeshFilter meshFilter;

    /// <summary>
    /// Mesh renderer component renders the mesh
    /// </summary>
    MeshRenderer meshRenderer;

    /// <summary>
    /// Mesh containing vertices and triangles to draw path geometry
    /// </summary>
    Mesh pathMesh;

    public float distance = 93f;
    public float radius = 4f;
    public int monthCount = 0;
    public Transform cam;
    /// <summary>
    /// Method called at gameobject initialization
    /// </summary>
	void Start()
    {
        sundial = GameObject.Find("Sundial").GetComponent<Sundial>();
        coordinates = new Vector3[26];
        //timeFrame = new Vector3[52560];
        for (int i = 0; i<= 24; i+=2)
        {
            monthCount += 30;
            coordinates[i] = new Vector3(0,0,0);
            coordinates[i+1] = new Vector3((distance * Mathf.Sin(monthCount * Mathf.PI/180)),0,-(distance * Mathf.Cos(monthCount * Mathf.PI/180)));
        }
        // Build a gameobject with geometry information
        GameObject eventPath = new GameObject("Path mesh");

        // This new gameobject will be parented to gameobject running this script
        eventPath.transform.SetParent(this.transform);

        // Add Mesh Filter component to new gameobject
        meshFilter = eventPath.AddComponent<MeshFilter>();

        // Add Mesh Renderer component to new gameobject
        meshRenderer = eventPath.AddComponent<MeshRenderer>();
        // Change Mesh Renderer material to eventPath material
        meshRenderer.material = Resources.Load<Material>("Path");

        // Instantiate a new mesh for the eventPath
        pathMesh = new Mesh();
        // Path mesh name
        pathMesh.name = "Path mesh";

        // Call function to build path geometry at startup
        CreatePath(coordinates);
        //CreatePath(coordinates2);
        //CreatePath(timeFrame);
    }

    /// <summary>
    /// This code runs every frame
    /// </summary>
    void Update()
    {
        // Build path every frame
        // This way, its possible to insert new coordinates on Unity at runtime and see changes
        CreatePath(coordinates);
        transform.position = new Vector3(0, cam.position.y , 0);
    }

    /// <summary>
    /// Builds a path geometry based on supplied coordinates
    /// </summary>
    /// <param name="coordinates">Array of coordinates in Vector3 format</param>
    void CreatePath(Vector3[] coordinates)
    {
        // If no coordinates are supplied, or, if coordinates supplied are less than the minimum necessary to estabilish a path
        // Log error and exit method
        if (coordinates == null || coordinates.Length < 2)
        {
            Debug.LogError("A path must have at least two coordinates.");
            return;
        }

        // Build vertices array
        // Length will be based on number of coordinates
        // Since path will be build based on a "cube", each "coordinate" in the path has 4 vertices
        Vector3[] vertices = new Vector3[coordinates.Length * 4];
        // Initialize vertex index
        int vertIndex = 0;
        // Pass through each coordinate
        foreach (Vector3 coordinate in coordinates)
        {
            // Build up vertices for this coordinate
            vertices[vertIndex] = new Vector3(-0.5f * pathSize + coordinate.x, 0.5f * pathSize + coordinate.y, -0.5f * pathSize + coordinate.z);
            vertices[vertIndex + 1] = new Vector3(0.5f * pathSize + coordinate.x, 0.5f * pathSize + coordinate.y, 0.5f * pathSize + coordinate.z);
            vertices[vertIndex + 2] = new Vector3(-0.5f * pathSize + coordinate.x, -0.5f * pathSize + coordinate.y, -0.5f * pathSize + coordinate.z);
            vertices[vertIndex + 3] = new Vector3(0.5f * pathSize + coordinate.x, -0.5f * pathSize + coordinate.y, 0.5f * pathSize + coordinate.z);
            // Update vertices index for next loop
            vertIndex += 4;
        }

        // Assign vertices to path mesh
        pathMesh.vertices = vertices;

        // Build up array of integers containing vertices that will be in each triangle rendered
        // Array size will be based on:
        // 3 vertices per triangle
        // 2 triangles per face
        // 6 faces per cube
        // Number of cubes vary based on number of coordinates
        int[] triangles = new int[3 * 2 * 6 * (coordinates.Length - 1)];
        // Loop through each path segment and build a cube
        for (int cube = 0; cube < coordinates.Length - 1; cube++)
        {
            // Front face
            // Triangle 1
            triangles[cube * 36 + 0] = cube * 4 + 2;
            triangles[cube * 36 + 1] = cube * 4 + 1;
            triangles[cube * 36 + 2] = cube * 4 + 0;

            // Triangle 2
            triangles[cube * 36 + 3] = cube * 4 + 2;
            triangles[cube * 36 + 4] = cube * 4 + 3;
            triangles[cube * 36 + 5] = cube * 4 + 1;

            // Top face
            // Triangle 1
            triangles[cube * 36 + 6] = cube * 4 + 1;
            triangles[cube * 36 + 7] = cube * 4 + 4;
            triangles[cube * 36 + 8] = cube * 4 + 0;

            // Triangle 2
            triangles[cube * 36 + 9] = cube * 4 + 5;
            triangles[cube * 36 + 10] = cube * 4 + 4;
            triangles[cube * 36 + 11] = cube * 4 + 1;

            // Bottom face
            // Triangle 1
            triangles[cube * 36 + 12] = cube * 4 + 3;
            triangles[cube * 36 + 13] = cube * 4 + 2;
            triangles[cube * 36 + 14] = cube * 4 + 6;

            // Triangle 2
            triangles[cube * 36 + 15] = cube * 4 + 3;
            triangles[cube * 36 + 16] = cube * 4 + 6;
            triangles[cube * 36 + 17] = cube * 4 + 7;

            // Left face
            // Triangle 1
            triangles[cube * 36 + 18] = cube * 4 + 2;
            triangles[cube * 36 + 19] = cube * 4 + 0;
            triangles[cube * 36 + 20] = cube * 4 + 4;

            // Triangle 2
            triangles[cube * 36 + 21] = cube * 4 + 4;
            triangles[cube * 36 + 22] = cube * 4 + 6;
            triangles[cube * 36 + 23] = cube * 4 + 2;

            // Right face
            // Triangle 1
            triangles[cube * 36 + 24] = cube * 4 + 1;
            triangles[cube * 36 + 25] = cube * 4 + 3;
            triangles[cube * 36 + 26] = cube * 4 + 5;

            // Triangle 2
            triangles[cube * 36 + 27] = cube * 4 + 3;
            triangles[cube * 36 + 28] = cube * 4 + 7;
            triangles[cube * 36 + 29] = cube * 4 + 5;

            // Rear face
            // Triangle 1
            triangles[cube * 36 + 30] = cube * 4 + 7;
            triangles[cube * 36 + 31] = cube * 4 + 6;
            triangles[cube * 36 + 32] = cube * 4 + 4;

            // Triangle 2
            triangles[cube * 36 + 33] = cube * 4 + 4;
            triangles[cube * 36 + 34] = cube * 4 + 5;
            triangles[cube * 36 + 35] = cube * 4 + 7;
        }

        // Assign triangles array to path mesh
        pathMesh.triangles = triangles;

        //Update orbital ring to follow camera on y-axis
        transform.position = new Vector3(cam.position.x, cam.position.y, cam.position.z);

        // Assign new path mesh to mesh filter to be rendered
        meshFilter.mesh = pathMesh;
    }
}
