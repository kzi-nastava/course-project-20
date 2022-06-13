namespace HealthCareCenter.Core.Notifications
{
    public class Notification
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public bool Opened { get; set; }
        public int UserID { get; set; }

        public Notification() { }
        public Notification(int id, string message, int userID)
        {
            ID = id;
            Message = message;
            Opened = false;
            UserID = userID;
        }
    }
}
