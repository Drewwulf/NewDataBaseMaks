namespace MaksGym.Models.ViewModels
{
    public class RoomViewModel
    {
        public Room NewRoom { get; set; } = new Room();  

        public List<Room> RoomList { get; set; } = new List<Room>(); 
    }
}
