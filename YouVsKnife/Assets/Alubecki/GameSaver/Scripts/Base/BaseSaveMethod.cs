/**
 * Alubecki GameSaver
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System.Text;


namespace Alubecki.GameSaver {

    public abstract class BaseSaveMethod : ISaveMethod {


        private readonly string fileName;
        private readonly string pwSalt;


        public BaseSaveMethod(string fileName, string pwSalt) {
            this.fileName = fileName;
            this.pwSalt = pwSalt;
        }

        private string pw {
            get {

                //generate strong password every time
                StringBuilder s = new StringBuilder();

                for (int i = 0; i < pwSalt.Length; i++) {
                    s.Append(pwSalt[i] + i);
                }

                return s.ToString();
            }
        }

        ISaveData ISaveMethod.load() {
            return loadFile(fileName, pw);
        }

        void ISaveMethod.save(ISaveData data) {
            saveFile(fileName, pw, data);
        }

        void ISaveMethod.saveCopy() {
            saveFileCopy(fileName, fileName + ".bak");
        }

        protected abstract ISaveData loadFile(string fileName, string pw);

        protected abstract void saveFile(string fileName, string pw, ISaveData data);

        protected abstract void saveFileCopy(string originalFileName, string copyFileName);

    }

}
