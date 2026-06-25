using System.Collections.Generic;

namespace CommunityEventManagement.Helper
{
    public class NavigationItem
    {
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public NavigationItem()
        {
        }

        public NavigationItem(string title, string url)
        {
            Title = title;
            Url = url;
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
                    new NavigationItem("Dashboard", "/"),
                    new NavigationItem("Manage Events", "/admin/events"),
                    new NavigationItem("Manage Venues", "/admin/venues"),
                    new NavigationItem("Manage Activities", "/admin/activities"),
                    new NavigationItem("View Registrations", "/admin/registrations")
                },

                "Participant" => new List<NavigationItem>
                {
                    new NavigationItem("Dashboard", "/"),
                    new NavigationItem("Browse Events", "/participant/events"),
                    new NavigationItem("My Registrations", "/participant/registrations")
                },

                _ => new List<NavigationItem>()
            };
        }
    }
}
