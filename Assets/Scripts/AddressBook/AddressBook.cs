using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace AddressBook
{
    public class AddressBook : MonoBehaviour
    {
        [SerializeField] private List<Address> _addresses;
        [SerializeField] private CallService _callService;
        [SerializeField] private GameObject _nameLabel;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        
        private int _pageNum = 0;
        private List<List<CallService.Person>> _pages;

        private void Start()
        {
            _rightButton.onClick.AddListener(Right);
            _leftButton.onClick.AddListener(Left);

            CallService.Person[] people = _callService.People;
            _pages = new List<List<CallService.Person>>();

            int currentPage = 0;
            people = people.OrderBy(x =>
            {
                int number;
                if (int.TryParse(x.Id, out number))
                    return number;
                return 100;
            }).ToArray();

            
            _pages.Add(new List<CallService.Person>());
            foreach (var person in people)
            {
                if (_pages[currentPage].Count >= _addresses.Count)
                {
                    _pages.Add(new List<CallService.Person>());
                    currentPage++;
                }
                
                _pages[currentPage].Add(person);
            }

            SetCurrentPage();
        }

        private void OnDestroy()
        {
            _rightButton.onClick.RemoveListener(Right);
            _leftButton.onClick.RemoveListener(Left);
        }

        private void SetCurrentPage()
        {
            List<CallService.Person> _currentPage = _pages[_pageNum];
            
            for (var i = 0; i < _addresses.Count; i++)
            {
                var address = _addresses[i];
                if (i >= _currentPage.Count)
                {
                    address.gameObject.SetActive(false);
                    continue;
                }
                
                address.gameObject.SetActive(true);
                address.SetPerson(_currentPage[i]);
            }
            
            _nameLabel.SetActive(!_currentPage[0].IsCity);
        }

        private void Right()
        {
            _pageNum++;

            if (_pageNum >= _pages.Count)
            {
                _pageNum = 0;
            }
            
            SetCurrentPage();
        }
        
        private void Left()
        {
            _pageNum--;

            if (_pageNum < 0)
            {
                _pageNum = _pages.Count - 1;
            }
            
            SetCurrentPage();
        }
    }
}