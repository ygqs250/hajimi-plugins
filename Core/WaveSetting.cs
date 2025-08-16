using Respawning.Waves;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.Core
{
    public class WaveSetting
    {
        //static List<WaveTimer>  waves = new List<WaveTimer>();

        public static List<WaveTimer> getRimeBasedWave()
        {
            List<SpawnableWaveBase> waves =  new();
            List<WaveTimer> ret = new();
            foreach (var SpawnableWaveBase in WaveManager.Waves)
            {
                if(SpawnableWaveBase is TimeBasedWave timeBasedWave)
                {
                    waves.Add(timeBasedWave);
                    ret.Add(timeBasedWave.Timer);
                }
            }
            return ret;
        }
        public static List<SpawnableWaveBase> getWaves()
        {
            List<SpawnableWaveBase> waves = new();
            foreach (var SpawnableWaveBase in WaveManager.Waves)
            {
                if (SpawnableWaveBase is TimeBasedWave timeBasedWave)
                {
                    waves.Add(timeBasedWave);
                }
            }
            return waves;
        }
    }
}
