using Microsoft.Identity.Client;

namespace StayHub.Services
{
    public class InRoomService
    {

        public InRoomService() { 
        
        }

        public decimal GetRoomServiceFee(string ServiceName)
        {
            decimal price = 0;
            switch (ServiceName) {
                case "Room Cleaning":
                    price = 30;
                    break;
                case "Laundry":
                    price = 20;
                    break;
                case "Breakfast":
                    price = 1200;
                    break;
                case "Lunch":
                    price = 1500;
                    break;
                case "Dining":
                    price = 3000;
                    break;


            }
            return price;

        }
    }
}
