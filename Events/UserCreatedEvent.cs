using MediatR;
using WebApp.Observer.Models;


namespace WebApp.Observer.Events;


public class UserCreatedEvent : INotification
{
    public AppUser AppUser { get; set; }
}