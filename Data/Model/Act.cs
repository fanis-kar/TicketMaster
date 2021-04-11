namespace TicketMaster.Data.Model
{
    /*
     * This class contains info on the artist, band, theater company + play, film etc.
     * The same act can be performed in various venues but during different dates
    */

    public class Act : Item
    {
        public string Name { get; set; }
    }
}
