using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKinectLibrary.Enums
{
    /// <summary>
    /// Identifica o estado da pose que é inserido como parâmetro no evento
    /// </summary>
    public enum PoseEventStatus
    {
        /// <summary>
        /// Quando a pose é reconhecida
        /// </summary>
        recognized,
        /// <summary>
        /// Quando a pose é interrompida
        /// </summary>
        interrupted,
        /// <summary>
        /// Quando a pose esta em progresso
        /// </summary>
        in_progress
    }
}
