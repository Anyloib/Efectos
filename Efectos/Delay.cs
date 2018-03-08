using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;

        int offsetTiempoMS;

        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 1000;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        //Offset es el numero de muestras leidas
        public int Read(float[] buffer, int offset, int count)
        {
            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido = (float)offset / (float)fuente.WaveFormat.SampleRate;
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestraOffsetTiempo = (int) (((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            for (int i = 0; i < read; i++)
            {
                if(tiempoTranscurrido > offsetTiempoMS)
                {
                    buffer[offset + i] += buffer[offset + i - numMuestraOffsetTiempo];
                }
            }
            return read;
        }
    }
}
