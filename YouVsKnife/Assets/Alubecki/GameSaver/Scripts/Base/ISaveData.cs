/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */


namespace Alubecki.GameSaver {

    public interface ISaveData {

        int getSaveVersion();

        ISaveVersionHandler getVersionHandler();

    }

}
