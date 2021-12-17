/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */


namespace Alubecki.GameSaver {

    public interface ISaveMethod {

        ISaveData load();

        void save(ISaveData data);

        void saveCopy();

    }
}