/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */


namespace Alubecki.GameSaver {

    public interface ISaveVersionHandler {

        ISaveData newPreviousData();

        ISaveData convertPreviousDataToNext(ISaveData previous);

    }

}