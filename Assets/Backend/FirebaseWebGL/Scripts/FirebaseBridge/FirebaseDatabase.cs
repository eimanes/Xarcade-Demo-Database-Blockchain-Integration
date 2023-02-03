using System.Runtime.InteropServices;

namespace Backend.FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        [DllImport("__Internal")]
        public static extern void GetJSON(string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetTotalScore(string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetTokens(string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void PostLogin(string path, string value1, string value2, string objectName, string callback,
            string fallback);

        [DllImport("__Internal")]
        public static extern void PostCurrentScore(string path, int value3, string callback,
            string fallback);

        [DllImport("__Internal")]
        public static extern void PostTotalScore(string path, int value4, string value5, string callback,
            string fallback);

        [DllImport("__Internal")]
        public static extern void PostTokensReq(string path, int value6, string objectName, string callback,
            string fallback);

        [DllImport("__Internal")]
        public static extern void PostTokensClaim(string path, int value7, string value8, string objectName, string callback,
            string fallback);


        
    }
}