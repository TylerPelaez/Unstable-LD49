using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CanFlipSprite : ICanFlipSprite
{
    [Tooltip("Usado para indicar direção horizontal (Pode usar para Flip Sprite)")]
    [SerializeField] private bool _movingRight = true;

    public bool MovingRight
    {
        get => _movingRight;
        set
        {
            _movingRight = value;
            Sprite.flipX = _movingRight;
        }
    }

    private SpriteRenderer _sprite;
    public SpriteRenderer Sprite { get => _sprite; set => _sprite = value; }

}