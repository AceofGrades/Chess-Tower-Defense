// Written by Sinclair

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Selector : MonoBehaviour {

	int layerMask = 1 << 2;
	int index = 0;

	[Header("Pieces")]
	public GameObject[] pieces;

	[Header("Holograms")]
	public GameObject[] holograms;

	int[] originalUnused = { 8, 3, 2, 2, 1 };
	int[] unused = { 8, 3, 2, 2, 1 };
	int unusedHologramIndex = -1;
	Vector3 unusedPos;
	List<GameObject> unusedPieces = new List<GameObject>();

	/*MeshRenderer tile;
	Material tileMat;
	Material redTileMat;*/

	void UpdateUnused() {

		foreach (GameObject piece in unusedPieces) {
			Destroy(piece);
		}

		unusedPieces.Clear();

		unusedPos = new Vector3(7f, -1f, -2f);
		for (int i = 0; i < pieces.Length; i++) {
			for (int j = 0; j < originalUnused[i]; j++) {
				if (i == 2 && j == 0) {
					unusedPos.x = 7;
					unusedPos.z = 9;
				}
				if (j < unused[i]) {
					if (i == unusedHologramIndex && j == unused[i] - 1) {
						GameObject clone = Instantiate(holograms[i], unusedPos, Quaternion.identity);
						unusedPieces.Add(clone);
					}
					else {
						GameObject clone = Instantiate(pieces[i], unusedPos, Quaternion.identity);
						Destroy(clone.GetComponent<Piece>());
						unusedPieces.Add(clone);
					}
				}
				if (unusedPos.x == 0) {
					unusedPos.x = 7;
					unusedPos.z -= 1 * (i < 2 ? 1 : -1);
				}
				else {
					unusedPos.x -= 1;
				}
			}
		}
	}

	void Start() {
		//redTileMat=Resources.Load<Material>("Materials/RedTile");
		UpdateUnused();
	}

	void Update() {

		unusedHologramIndex = -1;

		//if (tile) tile.material = redTileMat;

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			for (int i = pieces.Length - 1; i > -1; i--) {
				if (unused[(index + i) % pieces.Length] > 0) {
					index = (index + i) % pieces.Length;
					break;
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			for (int i = 1; i < pieces.Length + 1; i++) {
				if (unused[(index + i) % pieces.Length] > 0) {
					index = (index + i) % pieces.Length;
					break;
				}
			}
		}

		// Disable all holograms
		foreach (var h in holograms) {
			h.SetActive(false);
		}

		// Cast a ray from the camera
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(mouseRay, out hit, 250f, layerMask)) {
			Placeable p = hit.transform.GetComponent<Placeable>();
			if (p && p.enabled) {
				if (index == pieces.Length) {
					/*tile=p.gameObject.GetComponent<MeshRenderer>();
					tileMat = tile.material;
					tile.material=redTileMat;*/
				}
				else if (Input.GetMouseButtonDown(0)) {
					if (pieces[index].GetComponent<Queen>()) {
						// Epic workaround for strange bug
						Instantiate(pieces[index], hit.transform.position, Quaternion.Euler(0f, 9.7780001f, 0f), hit.transform);
					} else {
						Instantiate(pieces[index], hit.transform.position, Quaternion.identity, hit.transform);
					}
					p.enabled = false;
					unused[index] -= 1;
					if (unused[index] == 0) {
						bool empty = true;
						for (int i = 1; i < pieces.Length; i++) {
							if (unused[(index + i) % pieces.Length] > 0) {
								index = (index + i) % pieces.Length;
								empty = false;
								break;
							}
						}
						if (empty) {
							index = pieces.Length;
						}
					}
				}
				else {
					unusedHologramIndex = index;
					holograms[index].SetActive(true);
					holograms[index].transform.position = p.transform.position;
				}
			}
		}
		UpdateUnused();
	}
}
