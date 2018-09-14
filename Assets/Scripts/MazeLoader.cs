using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeLoader : MonoBehaviour {
	public int mazeRows, mazeColumns;
	public GameObject wall;
	public Material wallMaterial;
	public Material floorMaterial;
	public GameObject prefabCoin;
	public GameObject[] coins;
	public float size = 2f;

	private MazeCell[,] mazeCells;

	// Use this for initialization
	void Start () {
		InitializeMaze ();
		MazeAlgorithm ma = new HuntAndKillMazeAlgorithm (mazeCells);
		ma.CreateMaze ();
		//MergeLabirynth();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void InitializeMaze() {

		mazeCells = new MazeCell[mazeRows,mazeColumns];

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				mazeCells [r, c] = new MazeCell ();

				// For now, use the same wall object for the floor!
				mazeCells [r, c] .floor = Instantiate (wall, new Vector3 (r*size, -(size/2f), c*size), Quaternion.identity) as GameObject;
				mazeCells [r, c].floor.transform.parent = transform;
				mazeCells [r, c] .floor.name = "Floor " + r + "," + c;
				mazeCells [r, c] .floor.transform.Rotate (Vector3.right, 90f);
				mazeCells [r, c].floor.GetComponent<MeshRenderer>().material = floorMaterial;

				if(Random.Range(0, 1) < 0.5){
					var newCoin = Instantiate(prefabCoin, new Vector3(r*size, 0, c*size), Quaternion.identity );
					newCoin.transform.Rotate (Vector3.right, 90f);
					newCoin.transform.parent = transform;

				}


				if (c == 0) {
					mazeCells[r,c].westWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) - (size/2f)), Quaternion.identity) as GameObject;
					mazeCells [r, c].westWall.name = "West Wall " + r + "," + c;
					mazeCells [r, c].westWall.transform.parent = transform;
				}

				mazeCells [r, c].eastWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) + (size/2f)), Quaternion.identity) as GameObject;
				mazeCells [r, c].eastWall.name = "East Wall " + r + "," + c;
				mazeCells [r, c].eastWall.transform.parent = transform;

				if (r == 0) {
					mazeCells [r, c].northWall = Instantiate (wall, new Vector3 ((r*size) - (size/2f), 0, c*size), Quaternion.identity) as GameObject;
					mazeCells [r, c].northWall.name = "North Wall " + r + "," + c;
					mazeCells [r, c].northWall.transform.Rotate (Vector3.up * 90f);
					mazeCells [r, c].northWall.transform.parent = transform;
				}

				mazeCells[r,c].southWall = Instantiate (wall, new Vector3 ((r*size) + (size/2f), 0, c*size), Quaternion.identity) as GameObject;
				mazeCells [r, c].southWall.name = "South Wall " + r + "," + c;
				mazeCells [r, c].southWall.transform.Rotate (Vector3.up * 90f);
				mazeCells [r, c].southWall.transform.parent = transform;
			}
		}
	}

	private void MergeLabirynth(){
		List<GameObject> floorObjects = new List<GameObject>();
		List<GameObject> wallObjects = new List<GameObject>();

		for (var r = 0; r < this.mazeRows; r++) {
			for (var c = 0; c < this.mazeColumns; c++) {
				
				floorObjects.Add(mazeCells[r, c].floor);
				
				wallObjects.Add(mazeCells[r, c].northWall);
				wallObjects.Add(mazeCells[r, c].southWall);
				wallObjects.Add(mazeCells[r, c].westWall);
				wallObjects.Add(mazeCells[r, c].eastWall);
				
			}
		}
		MergeMeshes(gameObject.transform.Find("FloorContainer").gameObject, floorObjects);
		MergeMeshes(gameObject.transform.Find("WallContainer").gameObject, wallObjects);
	}

	private void MergeMeshes(GameObject mainObject, List<GameObject> objects){
		List <MeshFilter> meshFilters = new List<MeshFilter> {};
		Material mat = null;

		foreach(GameObject obj in objects){
			if(obj != null){
		 		MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
				meshFilters.Add(meshFilter);
				if(mat == null)
					mat = obj.GetComponent<MeshRenderer>().material;
			}
		}
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        mainObject.GetComponent<MeshFilter>().mesh = new Mesh();
        mainObject.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        mainObject.GetComponent<MeshRenderer>().material = mat;
        mainObject.SetActive(true);
	}
}
