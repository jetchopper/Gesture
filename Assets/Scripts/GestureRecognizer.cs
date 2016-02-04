using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

public class GestureRecognizer : MonoBehaviour {
	
	public Transform particles;

	private Panel panel;
	private List<Gesture> trainingSet = new List<Gesture>();
	private List<Point> points = new List<Point>();
	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;
	private Vector3 bufferedLinePoint;
	private bool recognized;
	
	void Start () 
	{
		panel = FindObjectOfType(typeof(Panel)) as Panel;
		drawArea = new Rect(Screen.width / 3, 0, Screen.width - Screen.width / 3, Screen.height);
		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Figures/");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
	}

	void Update () 
	{
		if (Input.touchCount > 0) 
		{
			virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			if (drawArea.Contains(virtualKeyPosition)) 
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began) 
				{
					//set all to start
					if (recognized) 
					{
						recognized = false;
						points.Clear();
					}
				}
				if (Input.GetTouch(0).phase == TouchPhase.Moved) 
				{
					//add points for conparison
					points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y));
					//attach particles
					bufferedLinePoint = Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10));
					Transform tmpParticles = Instantiate(particles, bufferedLinePoint, Quaternion.identity) as Transform;
					tmpParticles.GetComponent<ParticleSystem>().Play();
					Destroy(tmpParticles.gameObject, 0.5f);
				}
				if (Input.GetTouch(0).phase == TouchPhase.Ended) 
				{
					recognized = true;
					Gesture candidate = new Gesture(points.ToArray(), "");
					if (!float.IsNaN(candidate.Points[0].X))
					{
						Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
						panel.SetResult(gestureResult.GestureClass, gestureResult.Score);
					}
				}
			}
		}
	}

	public Gesture GetRandomGesture()
	{
		return trainingSet.ToArray()[UnityEngine.Random.Range(0, trainingSet.Count)];
	}

	public void StopGame()
	{
		drawArea = new Rect(0, 0, 0, 0);
	}
}
