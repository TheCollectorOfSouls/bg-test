using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;
using Object = UnityEngine.Object;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterSpriteChangeDetector : MonoBehaviour
{

	#region Variables
	
	[SerializeField] private Texture2D characterTexture;
	[SerializeField] private List<Sprite> sprites = new List<Sprite>();
	private OutfitSprite _controller;

	#endregion

	#region Properties

	private SpriteRenderer SpRenderer => GetComponent<SpriteRenderer>();
	public int CurrentFrame { get; private set; }

	#endregion


	#region Events

	public UnityEvent<int> onFrameChanged;
	
	#endregion

	#region FrameCheck

	private void Update()
	{
		CheckFrame();
	}

	private void CheckFrame()
	{
		if (sprites.Count <= 0) return;
		int frame = sprites.IndexOf(SpRenderer.sprite);
		
		if (CurrentFrame != frame)
		{
			CurrentFrame = frame;
			onFrameChanged?.Invoke(CurrentFrame);
		}
	}
	
	#endregion

#if UNITY_EDITOR
	[ContextMenu("Load Sprites")]
	public void SetSpritesFromTexture()
	{
		if (characterTexture == null) return;
		sprites = new List<Sprite>();
		Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(characterTexture));
		if (data == null) return;
		for (int i = 0; i < data.Length; i++)
		{
			Object obj = data[i];
				
			if (obj is Sprite sprite)
			{
				sprites.Add(sprite);
			}
		}
	}
#endif
}
