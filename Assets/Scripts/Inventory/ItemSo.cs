using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[CreateAssetMenu(fileName = "ItemSO", menuName = "Item", order = 0)]
public class ItemSo : ScriptableObject
{

	#region Variables
	
	[SerializeField] private Sprite uiIcon;
	[SerializeField] private Texture2D animationTexture;
	[SerializeField] private List<Sprite> animationSprites = new List<Sprite>();
	[SerializeField] private Slot itemSlot;
	[SerializeField] private int buyPrice = 10;
	[SerializeField] private int sellPrice = 5;
	
	#endregion

	#region Properties
	
	public List<Sprite> AnimationSprites => animationSprites;
	public Sprite UIIcon => uiIcon;
	public int BuyPrice => buyPrice;
	public int SellPrice => sellPrice;
	public Slot ItemSlot => itemSlot;
	
	#endregion

	#region Enum
		
	public enum Slot
	{ 
		Head,
		Body
	}
	
	#endregion
		
#if UNITY_EDITOR
	[ContextMenu("Load Sprites")]
	public void SetSpritesFromTexture()
	{
		if (animationTexture == null) return;
		animationSprites = new List<Sprite>();
		Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(animationTexture));
		if (data == null) return;
		for (int i = 0; i < data.Length; i++)
		{
			Object obj = data[i];
				
			if (obj is Sprite sprite)
			{
				animationSprites.Add(sprite);
			}
		}
	}
#endif
}
