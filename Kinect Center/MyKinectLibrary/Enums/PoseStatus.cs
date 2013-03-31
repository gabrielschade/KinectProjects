using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKinectLibrary.Enums
{
    /// <summary>
    /// Enumeração utilizada para definir o estado da pose
    /// </summary>
    public enum PoseStatus
    {
        /// <summary>
        /// Rastreamento não iniciado
        /// </summary>
        not_started = 0,
        /// <summary>
        /// Rastreamento em progresso
        /// </summary>
        in_progress = 1,
        /// <summary>
        /// Rastreamento aceito
        /// </summary>
        accepted = 2,
    }
}
