/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using Alubecki.GameSaver;


[Serializable]
public struct SaveV1 : ISaveData {


    public int totalPlayCount;
    public int maxScore;
    public int maxTimeSec;
    public int totalTimeSec;
    public int totalShareCount;


    int ISaveData.getSaveVersion() {
        return 1;
    }

    ISaveVersionHandler ISaveData.getVersionHandler() {
        return null;
    }

}
