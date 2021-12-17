/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;


namespace Alubecki.GameSaver {

    public class BaseGameSaver<T> where T : ISaveData {


        private ISaveMethod saveMethod;
        private T referenceData;

        public bool wasLoadedFromFileOnce { get; private set; }

        public T data;


        public BaseGameSaver(T referenceData, ISaveMethod saveMethod) {

            if (referenceData == null) {
                throw new ArgumentException();
            }

            this.referenceData = referenceData;

            this.saveMethod = saveMethod ?? throw new ArgumentException();
        }

        public void load() {

            ISaveData loadedData = null;

            try {
                
                loadedData = saveMethod.load();
                if (loadedData == null) {
                    throw new Exception("No loaded data");
                }

                data = (T) convertSaveToCurrentVersion(loadedData, referenceData);

                wasLoadedFromFileOnce = true;

            } catch (Exception e) {

                saveMethod.saveCopy();

                UnityEngine.Debug.LogWarning(e);

                //no previous save
                data = referenceData;
            }
        }

        private ISaveData convertSaveToCurrentVersion(ISaveData currentData, ISaveData nextReferenceData) {
            
            var nextVersion = nextReferenceData.getSaveVersion();
            var currentVersion = currentData.getSaveVersion();

            if (currentVersion > nextVersion) {
                //loaded file more advanced than the current one
                throw new InvalidOperationException("The current save is more advanced than the save version in the code");
            }

            if (currentVersion == nextVersion) {
                //ok
                return currentData;
            }

            var nextVersionHandler = nextReferenceData.getVersionHandler();
            if (nextVersionHandler == null) {
                throw new InvalidOperationException("The data to fill must have a version handler to convert the file to the current version");
            }

            var previousData = nextVersionHandler.newPreviousData();
            previousData = convertSaveToCurrentVersion(currentData, previousData);
                
            return nextVersionHandler.convertPreviousDataToNext(previousData);
        }

        public void save() {

            try {
                saveMethod.save(data);

            } catch (Exception e) {

                UnityEngine.Debug.LogWarning(e);
            }
        }

    }

}
