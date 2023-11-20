namespace BodvedVS.DataLibrary;

public class APIct
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Adres { get; set; }
    public string Konum { get; set; }
    public string Kaptan1 { get; set; }
    public string Kaptan2 { get; set; }
    public int TrnvId { get; set; }
    public string TrnvAd { get; set; }
    public List<APIctp> Oyuncular {  get; set; }
}

public class APIctp
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public int Rank { get; set; }

}
