using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Meshgenerator : MonoBehaviour {

	
	Mesh mesh;
	public float altura;
	public float noisex;
	public float noiseZ;

	public float scrollSpeed = 0;
	Renderer rend;

	public Material material;

	Vector3[] vertices;
	int[] triangles;
	Color[] colors;

	public int xSize = 100;
	public int zSize = 100;

	public Gradient gradient;

	float minTerrainHeight;
	float maxTerrainHeight;


	// Use this for initialization
	void Start () {
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;	
		rend = GetComponent<Renderer> ();
		
		
	}

	void Update(){
		UpdateMesh();
		CreateShape();
	}

	void UpdateMesh(){
		mesh.Clear();

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();

		mesh.colors = colors;
	}
	
	void CreateShape(){
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];		

		for (int i = 0, z = 0; z <= zSize; z++){
			for (int x = 0; x <= xSize; x++){
				float y = Mathf.PerlinNoise(x * noisex,z * noiseZ) * altura;
				vertices[i] = new Vector3(x,y, z);
				if(y > maxTerrainHeight){
					maxTerrainHeight = y;
				}	
				if(y < minTerrainHeight){
					minTerrainHeight = y;
				}			
				i++;
			}
		}



		int vert =0;
		int tris = 0;
		triangles = new int[xSize * zSize * 6];

		for (int z =0; z < xSize; z++){
			for (int x = 0; x < xSize; x++){
			
			triangles[0 + tris] = vert + 0;
			triangles[1 + tris] = vert + xSize + 1;
			triangles[2 + tris] = vert + 1;
			triangles[3 + tris] = vert + 1;
			triangles[4 + tris] = vert + xSize + 1;
			triangles[5 + tris] = vert + xSize + 2;

			vert++;
			tris += 6;

		
			}
		vert++;
		}

		colors = new Color[vertices.Length];
		for (int i = 0, z = 0; z <= zSize; z++){
			for (int x = 0; x <= xSize; x++){
				float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
				colors[i] = gradient.Evaluate(height);
				i++;
			}
		}
		

		

	}
}
