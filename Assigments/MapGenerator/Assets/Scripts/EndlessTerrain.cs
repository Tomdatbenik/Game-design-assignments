using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndlessTerrain : MonoBehaviour
{
    static MapGenerator mapGenerator;
    public const float maxViewDst = 450;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictonary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();


    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        foreach(TerrainChunk terrainChunk in terrainChunksVisibleLastUpdate)
        {
            terrainChunk.SetVisible(false);
        }

        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewChunckCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(terrainChunkDictonary.ContainsKey(viewChunckCoord))
                {
                    terrainChunkDictonary[viewChunckCoord].UpdateTerrainChunk();

                    if(terrainChunkDictonary[viewChunckCoord].isVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictonary[viewChunckCoord]);
                    }

                }
                else
                {
                    terrainChunkDictonary.Add(viewChunckCoord, new TerrainChunk(viewChunckCoord,chunkSize,transform, mapMaterial));
                }
            }
        }
    }


    public class TerrainChunk 
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 postionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("TerainChunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();

            meshRenderer.material = material;
            meshObject.transform.position = postionV3;
            meshObject.transform.parent = parent;

            SetVisible(false);

            mapGenerator.RequestMapData(OnMapDataReceived);
        }

        public void UpdateTerrainChunk()
        {
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDistanceFromNearestEdge <= maxViewDst;
            SetVisible(visible);
        }

        private void OnMapDataReceived(MapData mapData)
        {
            mapGenerator.RequestMeshData(mapData, OnMeshDataReceived);
        }
        
        private void OnMeshDataReceived(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool isVisible()
        {
            return meshObject.activeSelf;
        }
    }
}
