using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Sprites
{
	[RequireComponent(typeof(SpriteRenderer))]

	public class CharacterSpriteChangeDetector : MonoBehaviour
	{

		#region Variables

		[SerializeField] private Texture2D characterTexture;
		[SerializeField] private Sprite[] sprites;

		private Dictionary<Sprite, int> _spriteToFrame = new Dictionary<Sprite, int>();

		#endregion

		#region Properties

		private SpriteRenderer SpRenderer => GetComponent<SpriteRenderer>();
		public int CurrentFrame { get; private set; }

		#endregion


		#region Events

		public UnityEvent<int> onFrameChanged;

		#endregion

		#region FrameCheck

		private void Awake()
		{
			Setup();
		}

		private void Update()
		{
			CheckFrame();
		}

		private void Setup()
		{
			_spriteToFrame.Clear();
			for (int i = 0; i < sprites.Length; i++)
			{
				_spriteToFrame.Add(sprites[i], i);
			}
		}

		private void CheckFrame()
		{
			if (_spriteToFrame.Count <= 0) return;

			bool success = _spriteToFrame.TryGetValue(SpRenderer.sprite, out var frame);

			if (!success) return;

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
			string textureName = characterTexture.name;
			Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(characterTexture));
			if (data == null) return;
			sprites = new Sprite[data.Length - 1];

			for (int i = 0; i < data.Length; i++)
			{
				Object obj = data[i];

				if (obj is Sprite sprite)
				{
					var positionString = sprite.name.Replace($"{textureName}_", "");
					bool success = int.TryParse(positionString, out int position);
					if (success)
						sprites[position] = sprite;
				}
			}
		}
#endif
	}
}
