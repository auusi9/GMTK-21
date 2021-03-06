using System;
using System.Collections.Generic;
using Code.Utils;

namespace Services
{
    public class PeopleGeneratorService
    {
        private static readonly string[] Ids = new[]
        {
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30","11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "A", "B", "C", "D", "E", "F", "G", "H"
        };
        
        public static CallService.Person[] GeneratePeople()
        {
            PeopleSeeds peopleSeeds = GetJsonData<PeopleSeeds>.GetJsonInfo("NameSeeds");
            
            CallService.Person[] persons = new CallService.Person[Ids.Length];

            for (var i = 0; i < persons.Length; i++)
            {
                bool isCity = i >= 30;
                persons[i] = new CallService.Person
                {
                    Id = Ids[i],
                    IsCity =isCity,
                    Name = isCity ? " " : peopleSeeds.names[UnityEngine.Random.Range(0, peopleSeeds.names.Count)],
                    Surname = isCity ? " " : peopleSeeds.surnames[UnityEngine.Random.Range(0, peopleSeeds.surnames.Count)],
                    Address = isCity ? peopleSeeds.cities[UnityEngine.Random.Range(0, peopleSeeds.cities.Count)] : peopleSeeds.address[UnityEngine.Random.Range(0, peopleSeeds.address.Count)] 
                };

                if (isCity)
                {
                    peopleSeeds.cities.Remove(persons[i].Address);
                }
                else
                {
                    peopleSeeds.address.Remove(persons[i].Address);
                    peopleSeeds.names.Remove(persons[i].Name);
                    peopleSeeds.surnames.Remove(persons[i].Surname);
                }
            }

            return persons;
        }
    }
    
    [Serializable]
    public class PeopleSeeds
    {
        public List<string> names;
        public List<string> surnames;
        public List<string> address;
        public List<string> cities;
    }
}