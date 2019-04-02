using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TransitionApp.Domain.Events;

namespace TransitionApp.Domain.Notifications
{
    public class DomainNotification:Event
    {
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public object Value { get; private set; }
        public int Version { get; private set; }
        public TypeNotification TypeNotification { get; private set; }


        public DomainNotification(string key, object value, TypeNotification typeNotification)
        {
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
            TypeNotification = typeNotification;
        }
    }
    public enum TypeNotification:int
    {
       [Display(Name ="Successfully")]
       Success = 1,
       [Display(Name = "Fails")]
       Fail = 2,
       [Display(Name = "Create vehicle")]
       Vehicle = 3
    
    }
}
