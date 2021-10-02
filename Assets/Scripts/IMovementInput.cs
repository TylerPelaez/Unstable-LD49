using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IMovementInput : IMovement, IMovementMonitoring
{
    /// <summary>
    /// Move a partir dos dados de entrada
    /// </summary>
    void Move();
}