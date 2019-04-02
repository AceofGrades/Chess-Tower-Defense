﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour {

	Transform[] children;

	Vector3 originalPos;
	Quaternion originalRot;

	Vector3 originalPos2;
	Quaternion originalRot2;

	Vector3 velocity;
	Vector3 negVel;

	Vector3 velocity2;
	Vector3 negVel2;

	float timer = 0f;
	float prevTime = 0f;

	void Start() {
		children = GetComponentsInChildren<Transform>();
		originalPos = children[2].position;
		originalRot = children[2].rotation;
		velocity = new Vector3(4.6f, 0f, -2.4f);
		negVel = -velocity;

		originalPos2 = children[4].position;
		originalRot2 = children[4].rotation;
		velocity2 = new Vector3(4.6f, 0f, 2.4f);
		negVel2 = -velocity2;
	}

	void Update() {

		timer += Time.deltaTime;

		if (timer >= 0) {
			if (prevTime < 1 && 1 <= timer) {
				children[2].GetComponent<Boomerang>().enemies.Clear();
				children[4].GetComponent<Boomerang>().enemies.Clear();
			}
			if (timer >= 2) {

				timer = -1;
				velocity = new Vector3(4.6f, 0f, -2.4f);
				negVel = -velocity;
				children[2].position = originalPos;
				children[2].rotation = originalRot;

				velocity2 = new Vector3(4.6f, 0f, 2.4f);
				negVel2 = -velocity2;
				children[4].position = originalPos2;
				children[4].rotation = originalRot2;
			}

			//children[2].rotation *= Quaternion.Euler(135f*Time.deltaTime, 45f*Time.deltaTime, 90f*Time.deltaTime);
			//children[2].localEulerAngles = new Vector3(135f * Time.deltaTime, 45f * Time.deltaTime, 90f * Time.deltaTime);
			children[2].Rotate(new Vector3(135f, 45f, 90f), 360 * Time.deltaTime);
			children[2].position += velocity * Time.deltaTime;
			velocity += negVel * Time.deltaTime;

			//children[3].rotation *= Quaternion.Euler(135f * Time.deltaTime, 45f * Time.deltaTime, -90f * Time.deltaTime);
			children[4].Rotate(new Vector3(135f, 45f, -90f), 360 * Time.deltaTime);
			children[4].position += velocity2 * Time.deltaTime;
			velocity2 += negVel2 * Time.deltaTime;
		}

		prevTime = timer;
	}

}