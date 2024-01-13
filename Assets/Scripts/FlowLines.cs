using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowLines : MonoBehaviour {

    public static FlowLines instance { get; set; }

    public List<Transform> playerPositions = new List<Transform>();
    public List<Transform> playerUIpositions = new List<Transform>();

    // public Transform origin;
    public Color baseColor;
    public Material material;
    public Color[] colors;   

	void Awake () {
        if (instance == null)
            instance = this;
	}

    private void OnPostRender()
    {
   //     if (playerPositions.Count >= 1)
        RenderLines(playerPositions, colors);
    }

    void RenderLines(List<Transform> points, Color[] colors) {

        for (int i = 0; i < points.Count; i++) {
            if (playerUIpositions[i].position != null || playerPositions != null) {
                GL.Begin(GL.LINES);
                material.SetPass(0);
                GL.Color(baseColor);
                GL.Vertex(playerUIpositions[i].position);
                GL.Color(Color.white);
                Vector3 vertexTransform = new Vector3(points[i].position.x, points[i].position.y, points[i].position.z);
                GL.Vertex(vertexTransform);
            }     
        }

        GL.End();

       
    }
}
