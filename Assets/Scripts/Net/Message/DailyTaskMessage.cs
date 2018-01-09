using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTaskMessage : NetMessage {

    public DailyTask data;

    public class DailyTask
    {
        public DailyTaskData daily_task;
        public long money;
        public int energy;
    }
}

