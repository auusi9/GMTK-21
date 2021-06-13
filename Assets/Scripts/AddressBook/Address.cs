using Services;
using TMPro;
using UnityEngine;

namespace AddressBook
{
    public class Address : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _addressText;

        public void SetPerson(CallService.Person person)
        {
            _idText.text = person.Id;
            _nameText.text = person.Name + " " + person.Surname;
            _addressText.text = person.Address;
            _nameText.gameObject.SetActive(!person.IsCity);
        }
    }
}