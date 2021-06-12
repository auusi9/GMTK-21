using Services;
using UnityEngine;

namespace Notifications
{
    public class NotificationCenter : MonoBehaviour
    {
        [SerializeField] private Notification _notificationPrefab;
        [SerializeField] private Transform _container;

        public void CreateNotification(Call call)
        {
            Notification notification = Instantiate(_notificationPrefab, _container);
            notification.SetCall(call);
        }

    }
}