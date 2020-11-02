﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CGA4CameraEffect : MonoBehaviour
{

	private Material material;

	// Creates a private material used to the effect
	void Awake()
	{
		material = new Material(Shader.Find("Hidden/CGAHalftone"));
	}

	// Postprocess the image
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
