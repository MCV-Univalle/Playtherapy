﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndlessTerrain : MonoBehaviour {

    const float scale =2f;
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrviewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    public LODInfo[] detailLevels;
    public static float maxViewDst;
    public Transform viewer;
	public static Vector2 viewerPosition;
    Vector2 viewerPositionOld;

    static MapGenerator mapGenerator;
	public Material mapMaterial;
	int chunkSize;
	int chunckVisibleViewDst;
   
	static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();


	Dictionary<Vector2,TerrainChunk> terrainChunckDictionary = new Dictionary<Vector2,TerrainChunk>();




	void Start(){


		mapGenerator = FindObjectOfType<MapGenerator>();
        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstTreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
		chunckVisibleViewDst = Mathf.RoundToInt (maxViewDst / chunkSize);

        UpdateVisibleChunks();
	}
	void Update(){

       
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z)/scale;
        if ((viewerPositionOld-viewerPosition).sqrMagnitude>sqrviewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }

		

	}

	void UpdateVisibleChunks()
	{

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
			terrainChunksVisibleLastUpdate [i].SetVisible (false);

		}
		terrainChunksVisibleLastUpdate.Clear ();
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / chunkSize);

		for (int yOffset = -chunckVisibleViewDst; yOffset < chunckVisibleViewDst; yOffset++) {
			for (int xOffset = -chunckVisibleViewDst; xOffset < chunckVisibleViewDst; xOffset++) {
				Vector2 viewedChunckCoord = new Vector2 (currentChunkCoordX+xOffset, currentChunkCoordY+yOffset);


				if (terrainChunckDictionary.ContainsKey (viewedChunckCoord)) {
					terrainChunckDictionary [viewedChunckCoord].UpdateTerrainChunk ();
					
				} else {
					terrainChunckDictionary.Add (viewedChunckCoord, new TerrainChunk (viewedChunckCoord,chunkSize, detailLevels, transform,mapMaterial));
				}

			}
		}
	}
	public class TerrainChunk {

		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MapData mapData;
        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
		LODMesh collisionLODMesh; 


        MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		MeshCollider meshCollider;
        int previousLODIndex = -1;
        bool mapDataReceived;


		public TerrainChunk(Vector2 coord,int size,LODInfo[] detailLevels , Transform parent, Material material )
		{
            this.detailLevels = detailLevels;

            position = coord*size;
			bounds = new Bounds(position,Vector2.one*size);

			Vector3 positionV3 = new Vector3(position.x,0,position.y);
			meshObject = new GameObject("Terrain Chunk");
			meshObject.tag ="Terrain";
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshCollider = meshObject.AddComponent<MeshCollider>();
			meshRenderer.material = material;
			meshObject.transform.position = positionV3*scale;
            meshRenderer.transform.localScale = Vector3.one*scale;
			meshObject.transform.parent= parent;
			SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];

           for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod,UpdateTerrainChunk);
            
				if (detailLevels[i].useForCollider) {
					collisionLODMesh = lodMeshes[i];
				}
			
			}
            mapGenerator.RequestMapData(position,onMapDataReceived);

		}
		public void UpdateTerrainChunk()
		{
            if (mapDataReceived)
            {
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

                bool visible = viewerDstFromNearestEdge <= maxViewDst;
               
                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < detailLevels.Length; i++)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDstTreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (lodIndex != previousLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            meshFilter.mesh = lodMesh.mesh;

                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }

                    }
					if (lodIndex == 0) {
						if (collisionLODMesh.hasMesh) {
							meshCollider.sharedMesh = collisionLODMesh.mesh;
						}
						else if (!collisionLODMesh.hasRequestedMesh) {
							collisionLODMesh.RequestMesh (mapData);
						}
					} 
                    terrainChunksVisibleLastUpdate.Add(this);
                }
                SetVisible(visible);


            }


        }

		void onMapDataReceived(MapData mapData)
		{
            this.mapData = mapData;
            this.mapDataReceived = true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
		}
		
		public void SetVisible(bool visible){

			if (meshObject!=null) {
				meshObject.SetActive (visible);



			}


		}

		public bool isVisible(){

			return meshObject.activeSelf;

		}
	}
    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;
        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }
        void onMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.createMesh();
            hasMesh = true;
            updateCallback();
        }
        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod,onMeshDataReceived);
        }
    }
    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDstTreshold;
		public bool useForCollider;
    }
}
