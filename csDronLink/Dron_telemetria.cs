using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csDronLink
{
    public partial class Dron
    {
        // Telemetria
        public void EnviarDatosTelemetria(Action<List<(string nombre, float valor)>> f)
        {
            // Adaptamos la firma de la función para que coincida con el tipo esperado
            this.ProcesarTelemetria = f;
        }
        public void DetenerDatosTelemetria()
        {
            // El cliente ya no quiere datos de telemetría
            this.ProcesarTelemetria = null;
        }

    }
}