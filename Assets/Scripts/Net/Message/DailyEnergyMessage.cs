using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyEnergyMessage : NetMessage {

    public DailyEnergyData data;

	public class DailyEnergyData
    {
        public int daily_energy;
        public int energy;
        public long money;
    }
}
