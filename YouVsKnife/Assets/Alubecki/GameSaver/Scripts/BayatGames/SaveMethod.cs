/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using System.IO;
using System.Text;
using UnityEngine;
using Alubecki.GameSaver;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;


public class SaveMethod : BaseSaveMethod {


    public SaveMethod(string fileName = "save", string pwSalt = "XXXXXX") : base(fileName, pwSalt) {

        SaveGame.Serializer = new SaveGameBinarySerializer();
    }

    protected override ISaveData loadFile(string fileName, string pw) {

        return SaveGame.Load<ISaveData>(
            fileName,
            null,
            false,
            pw,
            SaveGame.Serializer,
            SaveGame.Encoder,
            Encoding.UTF8,
            SaveGamePath.PersistentDataPath
        );
    }

    protected override void saveFile(string fileName, string pw, ISaveData data) {

        SaveGame.Save(
            fileName,
            data,
            false,
            pw,
            SaveGame.Serializer,
            SaveGame.Encoder,
            Encoding.UTF8,
            SaveGamePath.PersistentDataPath
        );
    }

    protected override void saveFileCopy(string originalFileName, string copyFileName) {

        var originalPath = Application.persistentDataPath + "/" + originalFileName;
        var copyPath = Application.persistentDataPath + "/" + copyFileName;

        if (!SaveGame.IsFilePath(originalPath)) {
            Debug.LogWarning("Couldn't copy save " + originalFileName);
            return;
        }

        try {
            File.Copy(originalPath, copyPath, true);

        } catch (Exception e) {

            Debug.LogWarning(e);
        }
    }

}