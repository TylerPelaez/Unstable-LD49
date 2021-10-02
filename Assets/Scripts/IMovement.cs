using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IMovement
{
    /// <summary>
    /// Velocidade do movimento
    /// </summary>
    float Speed { get; set; }

    /// <summary>
    /// Habilita ou nao a possibilidade de mover
    /// </summary>
    bool EnableMove { get; set; }
}