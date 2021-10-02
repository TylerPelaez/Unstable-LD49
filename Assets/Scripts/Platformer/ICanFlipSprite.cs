//#define STANDALONE_CONTROL 
/* ações executadas internada pela propria classe MonoBehavior;
 * Ex: Move sem a necessidade de outra classe externa a chama-la */

using UnityEngine;

public interface ICanFlipSprite
{
    /// <summary>
    /// Sprite a ser Flip
    /// </summary>
    SpriteRenderer Sprite { get; set; }

    /// <summary>
    /// Indicador de qual lado esta movendo
    /// </summary>
    bool MovingRight { get; set; }
}