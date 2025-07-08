using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayLogic.GameService.InteractionItemModule.Items
{
    public enum ShapeType
    {
        One,
        Square,
        LineHorizontal2,
        LineHorizontal3,
        LineHorizontal4,
        LineVertical2,
        LineVertical3,
        LineVertical4,
        Rectangle
    }

    [CreateAssetMenu (fileName = "ItemSO", menuName = "Items")]
    public class ItemSO : ScriptableObject
    {
        public string ID => id;
        public string NameItem => nameItem;
        public bool IsStorable => isStorable;
        public bool IsStackable => isStackable;
        public int AmountStack => amountStack;
        public List<Vector2Int> Shape => _shape;
        public Sprite Icon => icon;
        
        public int AmountValue => amountValue;
        
        [SerializeField] private string id;
        [SerializeField] private string nameItem;
        [SerializeField] private bool isStorable;
        [SerializeField] private bool isStackable;
        [SerializeField] private int amountStack;
        [FormerlySerializedAs("value")] [SerializeField] private int amountValue;
        [SerializeField] private Sprite icon;
        [SerializeField] private ShapeType shapeType;
        
        
        private List<Vector2Int> _shape;
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = System.Guid.NewGuid().ToString();
            }
            
            switch (shapeType)
            {
                case ShapeType.One:
                    _shape = new List<Vector2Int> { new Vector2Int(0, 0) };
                    break;
                case ShapeType.Square:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 1)
                    };
                    break;
                case ShapeType.LineHorizontal2:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                    };
                    break;
                case ShapeType.LineHorizontal3:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(2, 0),
                    };
                    break;
                case ShapeType.LineHorizontal4:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(2, 0),
                        new Vector2Int(3, 0),
                    };
                    break;
                case ShapeType.Rectangle:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(2, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 1),
                        new Vector2Int(2, 1),
                    };
                    break;
                case ShapeType.LineVertical2:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(0,1),
                    };
                    break;
                case ShapeType.LineVertical3:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2)
                    };
                    break;
                case ShapeType.LineVertical4:
                    _shape = new List<Vector2Int>
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(0, 3),
                    };
                    break;
            }
        }
    }
}
