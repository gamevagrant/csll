using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLottoCardMessage : NetMessage {

    public DungeonLottoCardMessageData data;
    public class DungeonLottoCardMessageData
    {
        /// <summary>
        /// 大小牌 0小牌 1大牌
        /// </summary>
        public int card_type;////大小牌 0小牌 1大牌
    }
}
