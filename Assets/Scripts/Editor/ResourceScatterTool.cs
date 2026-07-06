using UnityEngine;
using UnityEditor;

public class ResourceScatterTool : EditorWindow
{
    private Terrain targetTerrain;
    private GameObject prefabToScatter;
    private Transform parentObject;

    private int scatterCount = 50;
    private float rotationRandomness = 360f;
    private float scaleRandomness = 0.2f;
    private Vector3 scaleRange = Vector3.one;
    private float heightOffset = 0f;
    private bool randomizeRotation = true;
    private bool randomizeScale = true;
    private float minimumDistance = 0f;
    private bool useMinimumDistance = false;

    [MenuItem("Tools/Resource Scatter Tool")]
    public static void ShowWindow()
    {
        GetWindow<ResourceScatterTool>("Scatter Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Resource Scatter Settings", EditorStyles.boldLabel);

        targetTerrain = EditorGUILayout.ObjectField("Target Terrain", targetTerrain, typeof(Terrain), true) as Terrain;
        prefabToScatter = EditorGUILayout.ObjectField("Prefab to Scatter", prefabToScatter, typeof(GameObject), false) as GameObject;
        parentObject = EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true) as Transform;

        GUILayout.Space(10);
        GUILayout.Label("Scatter Parameters", EditorStyles.boldLabel);

        scatterCount = EditorGUILayout.IntSlider("Scatter Count", scatterCount, 1, 1000);
        heightOffset = EditorGUILayout.FloatField("Height Offset", heightOffset);

        GUILayout.Space(10);
        GUILayout.Label("Randomization", EditorStyles.boldLabel);

        randomizeRotation = EditorGUILayout.Toggle("Randomize Rotation", randomizeRotation);
        if (randomizeRotation)
        {
            rotationRandomness = EditorGUILayout.Slider("Rotation Randomness", rotationRandomness, 0f, 360f);
        }

        randomizeScale = EditorGUILayout.Toggle("Randomize Scale", randomizeScale);
        if (randomizeScale)
        {
            scaleRange = EditorGUILayout.Vector3Field("Scale Range", scaleRange);
            scaleRandomness = EditorGUILayout.Slider("Scale Randomness", scaleRandomness, 0f, 1f);
        }

        GUILayout.Space(10);
        GUILayout.Label("Spacing", EditorStyles.boldLabel);

        useMinimumDistance = EditorGUILayout.Toggle("Use Minimum Distance", useMinimumDistance);
        if (useMinimumDistance)
        {
            minimumDistance = EditorGUILayout.FloatField("Minimum Distance", minimumDistance);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Scatter Objects", GUILayout.Height(40)))
        {
            ScatterObjects();
        }

        if (GUILayout.Button("Clear All Scattered Objects", GUILayout.Height(30)))
        {
            ClearScatteredObjects();
        }
    }

    private void ScatterObjects()
    {
        if (targetTerrain == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a Terrain.", "OK");
            return;
        }

        if (prefabToScatter == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a Prefab to scatter.", "OK");
            return;
        }

        GameObject scatterContainer = parentObject != null ? parentObject.gameObject : new GameObject("ScatteredObjects");
        if (parentObject == null)
        {
            Undo.RegisterCreatedObjectUndo(scatterContainer, "Create Scatter Container");
        }

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.GetPosition();
        Vector3 terrainSize = terrainData.size;

        System.Collections.Generic.List<Vector3> placedPositions = new System.Collections.Generic.List<Vector3>();
        int placed = 0;
        int maxAttempts = scatterCount * 10;
        int attempts = 0;

        while (placed < scatterCount && attempts < maxAttempts)
        {
            attempts++;
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);

            Vector3 worldPos = terrainPos + new Vector3(randomX, 0, randomZ);
            float terrainHeight = targetTerrain.SampleHeight(worldPos);
            worldPos.y = terrainHeight + heightOffset;

            if (useMinimumDistance && !IsPositionValid(worldPos, placedPositions))
            {
                continue;
            }

            GameObject instance = Instantiate(prefabToScatter, worldPos, Quaternion.identity, scatterContainer.transform);
            Undo.RegisterCreatedObjectUndo(instance, "Scatter Object");
            placedPositions.Add(worldPos);
            placed++;

            if (randomizeRotation)
            {
                float randomRotY = Random.Range(0f, rotationRandomness);
                instance.transform.Rotate(0, randomRotY, 0);
            }

            if (randomizeScale)
            {
                Vector3 randomScale = new Vector3(
                    Random.Range(scaleRange.x - scaleRandomness, scaleRange.x + scaleRandomness),
                    Random.Range(scaleRange.y - scaleRandomness, scaleRange.y + scaleRandomness),
                    Random.Range(scaleRange.z - scaleRandomness, scaleRange.z + scaleRandomness)
                );
                instance.transform.localScale = randomScale;
            }
        }

        EditorUtility.DisplayDialog("Success", $"Scattered {placed} out of {scatterCount} objects on terrain.", "OK");
    }

    private void ClearScatteredObjects()
    {
        if (parentObject == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a Parent Object to clear.", "OK");
            return;
        }

        if (EditorUtility.DisplayDialog("Confirm", "Delete all scattered objects?", "Yes", "No"))
        {
            while (parentObject.childCount > 0)
            {
                Undo.DestroyObjectImmediate(parentObject.GetChild(0).gameObject);
            }

            EditorUtility.DisplayDialog("Success", "Cleared all scattered objects.", "OK");
        }
    }

    private bool IsPositionValid(Vector3 position, System.Collections.Generic.List<Vector3> placedPositions)
    {
        foreach (Vector3 placedPos in placedPositions)
        {
            if (Vector3.Distance(position, placedPos) < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }
}
