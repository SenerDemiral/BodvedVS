namespace BodvedVS.DataLibrary;

public class PPmodel
{
    public int PPId { get; set; }
    public int GirDnm { get; set; }
    public int R0 { get; set; }
    public string Ad { get; set; }
    public string Sex { get; set; }
    public int DgmYil { get; set; }
    public string Tel { get; set; }
    public string Mail { get; set; }
    public string Info { get; set; }

    public int B1 { get; set; } // ⚜kulüp üyeliği
    public int B2 { get; set; } // 🎗lisanslı oyuncu
    public int B3 { get; set; } // ⚔bodved lig katılım sayısı
    public int B4 { get; set; } // 🏆takım lig birinciliği
    public int B5 { get; set; } // 👑ferdi turnuva birinciliği
    public int B6 { get; set; } // 🏓antrenör belgesi
    public int B7 { get; set; } // ⚖hakem belgesi
    public int B8 { get; set; } // 🧢takım kaptan

    public string? Belge => Helper.Belge(B1, B2, B3, B4, B5, B6, B7, B8);

}
