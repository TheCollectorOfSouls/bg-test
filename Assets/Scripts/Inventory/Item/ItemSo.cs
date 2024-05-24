#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Inventory.Item
{
	[CreateAssetMenu(fileName = "ItemSO", menuName = "Item", order = 0)]
	public class ItemSo : ScriptableObject
	{

		#region Variables / Properties

		[SerializeField] private Sprite uiIcon;
		[SerializeField] private Texture2D animationTexture;
		[SerializeField] private Sprite[] animationSprites;
		[SerializeField] private Slot itemSlot;
		[SerializeField] private int basePrice = 10;

		public Sprite[] AnimationSprites => animationSprites;
		public Sprite UIIcon => uiIcon;
		public int BasePrice => basePrice;
		public int CurrentPrice { get; private set; } = 0;
		public Slot ItemSlot => itemSlot;

		#endregion

		#region Enum

		public enum Slot
		{
			Head,
			Body
		}

		#endregion

		#region Setter

		public void SetPrice(int price)
		{
			CurrentPrice = price;
		}

		#endregion

#if UNITY_EDITOR
		[ContextMenu("Load Sprites")]
		public void SetSpritesFromTexture()
		{
			if (animationTexture == null) return;
			string textureName = animationTexture.name;
			Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(animationTexture));
			if (data == null) return;
			animationSprites = new Sprite[data.Length -1];

			for (int i = 0; i < data.Length; i++)
			{
				Object obj = data[i];
			
				if (obj is Sprite sprite)
				{
					var positionString = sprite.name.Replace($"{textureName}_", "");
					bool success = int.TryParse(positionString, out int position);
					if (success)
						animationSprites[position] = sprite;
				}
			}
		}
#endif
	}
}
