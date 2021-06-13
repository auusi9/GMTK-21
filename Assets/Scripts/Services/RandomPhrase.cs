using System;
using System.ComponentModel;
using Code.Utils;
using Random = UnityEngine.Random;

namespace Services
{
    public class RandomPhrase
    {
        private static PhraseSeeds _phraseSeeds;
        
        private static void Initialize()
        {
            _phraseSeeds = GetJsonData<PhraseSeeds>.GetJsonInfo("PhrasesSeeds");
        }
        
        public static string GetPhrase(Call call)
        {
            if (_phraseSeeds == null)
            {
                Initialize();
            }
            
            string[] phrases;
            if (call.OutputPerson.IsCity)
            {
                return GetPhrases(2, call);
            }
            else
            {
                return GetPhrases(3, call);
            }
        }

        private static string GetPhrases(int max, Call call)
        {
            int i = Random.Range(0, max);
            switch (i)
            {
                case 0:
                    return string.Format(_phraseSeeds.byId[Random.Range(0, _phraseSeeds.byId.Length)],
                        call.OutputPerson.Id);
                case 1:
                    return string.Format(_phraseSeeds.byAddress[Random.Range(0, _phraseSeeds.byAddress.Length)],
                        call.OutputPerson.Address);
                case 2:
                    return string.Format(_phraseSeeds.byName[Random.Range(0, _phraseSeeds.byName.Length)],
                        call.OutputPerson.Name + " " + call.OutputPerson.Surname);
            }

            return "...";
        }
    }
    
    [Serializable]
    public class PhraseSeeds
    {
        public string[] byName;
        public string[] byAddress;
        public string[] byId;
    }
}