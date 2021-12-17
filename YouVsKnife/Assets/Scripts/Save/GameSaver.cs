/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using Alubecki.GameSaver;


public class GameSaver : BaseGameSaver<SaveV1> {


    //singleton
    public static readonly GameSaver instance = new GameSaver();


    private GameSaver() : base(new SaveV1(), new SaveMethod("save", "XXXXXXXXX")) {
    }

}