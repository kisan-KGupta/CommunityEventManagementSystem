using System.Collections.Generic;

namespace CommunityEventManagement.Helper
{
    public class NavigationItem
    {
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public NavigationItem()
        {
        }

        public NavigationItem(string title, string url, string icon)
        {
            Title = title;
            Url = url;
            Icon = icon;
        }
    }

    public static class NavigationProvider
    {
        public static List<NavigationItem> GetMenu(string role)
        {
            return role switch
            {
                "Admin" => new List<NavigationItem>
                {
                    new NavigationItem("Dashboard", "/", "bi-speedometer2"),
                    new NavigationItem("Manage Events", "/admin/events", "bi-calendar-event"),
                    new NavigationItem("Manage Venues", "/admin/venues", "bi-building"),
                    new NavigationItem("Manage Activities", "/admin/activities", "bi-list-stars"),
                    new NavigationItem("View Registrations", "/admin/registrations", "bi-people")
                },

                "Participant" => new List<NavigationItem>
                {
                    new NavigationItem("Dashboard", "/", "bi-speedometer2"),
                    new NavigationItem("Browse Events", "/participant/events", "bi-search"),
                    new NavigationItem("My Registrations", "/participant/registrations", "bi-ticket-detailed")
                },

                _ => new List<NavigationItem>()
            };
        }
    }
}
