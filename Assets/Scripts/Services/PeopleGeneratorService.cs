using System;
using Code.Utils;

namespace Services
{
    public class PeopleGeneratorService
    {
        private static readonly string[] Ids = new[]
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "A", "B", "C", "D", "E", "F", "G", "H"
        };
        
        public static CallService.Person[] GeneratePeople()
        {
            PeopleSeeds peopleSeeds = GetJsonData<PeopleSeeds>.GetJsonInfo("NameSeeds");
            
            CallService.Person[] persons = new CallService.Person[Ids.Length];

            for (var i = 0; i < persons.Length; i++)
            {
                persons[i] = new CallService.Person
                {
                    Id = Ids[i],
                    Name = peopleSeeds.names[UnityEngine.Random.Range(0, peopleSeeds.names.Length)],
                    Surname = peopleSeeds.surnames[UnityEngine.Random.Range(0, peopleSeeds.surnames.Length)],
                    Address = peopleSeeds.address[UnityEngine.Random.Range(0, peopleSeeds.address.Length)]
                };
            }

            return persons;
        }
    }
    
    [Serializable]
    public class PeopleSeeds
    {
        public string[] names;
        public string[] surnames;
        public string[] address;
    }
}