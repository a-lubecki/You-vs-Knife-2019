/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;


namespace Alubecki.GameSaver {
    
    public abstract class BaseSaveVersionHandler<PreviousData, NextData> : ISaveVersionHandler where PreviousData : ISaveData where NextData : ISaveData {


        protected abstract PreviousData newData();

        protected abstract NextData convert(PreviousData previous);


        ISaveData ISaveVersionHandler.newPreviousData() {
            return newData();
        }

        ISaveData ISaveVersionHandler.convertPreviousDataToNext(ISaveData previous) {

            if (!(previous is PreviousData)) {
                throw new InvalidCastException("Previous data must has invalid type : " + previous.GetType() + " in " + this.GetType());
            }

            return convert((PreviousData)previous);
        }

    }

}