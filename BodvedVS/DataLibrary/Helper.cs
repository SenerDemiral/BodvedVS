namespace BodvedVS.DataLibrary;

public static class Helper
{
    public static string B1e = "⚜️";  //&#x269C;&#xFE0F;
    public static string B2e = "🎗️";    //&#x1F397;&#xFE0F;
    public static string B3e = "⚔️";  //&#x2694;&#xFE0F;
    public static string B4e = "🏆";  // &#x1F3C6;
    public static string B5e = "👑";  //&#x1F451;
    public static string B6e = "🏓";   //&#x1F3D3;
    public static string B7e = "⚖️";  //&#x2696;&#xFE0F;
    public static string B8e = "🧢";  //&#x1F9E2;

    public static string B1n = $"{B1e} kulüp üyeliği";
    public static string B2n = $"{B2e} lisanslı oyuncu";
    public static string B3n = $"{B3e} bodved lig katılım sayısı";
    public static string B4n = $"{B4e} takım lig birinciliği";
    public static string B5n = $"{B5e} ferdi turnuva birinciliği";
    public static string B6n = $"{B6e} antrenör belgesi";
    public static string B7n = $"{B7e} hakem belgesi";
    public static string B8n = $"{B8e} takım kaptanı";

    public static string Belge(int B1, int B2, int B3, int B4, int B5, int B6, int B7, int B8)
    {
        string B1s = B1 == 0 ? "" : $"{B1e}{(B1 == 1 ? "" : "<sub>" + B1 + "</sub>")}";
        string B2s = B2 == 0 ? "" : $"{B2e}{(B2 == 1 ? "" : "<sub>" + B2 + "</sub>")}";
        string B3s = B3 == 0 ? "" : $"{B3e}{(B3 == 1 ? "" : "<sub>" + B3 + "</sub>")}";
        string B4s = B4 == 0 ? "" : $"{B4e}{(B4 == 1 ? "" : "<sub>" + B4 + "</sub>")}";
        string B5s = B5 == 0 ? "" : $"{B5e}{(B5 == 1 ? "" : "<sub>" + B5 + "</sub>")}";
        string B6s = B6 == 0 ? "" : $"{B6e}{(B6 == 1 ? "" : "<sub>" + B6 + "</sub>")}";
        string B7s = B7 == 0 ? "" : $"{B7e}{(B7 == 1 ? "" : "<sub>" + B7 + "</sub>")}";
        string B8s = B8 == 0 ? "" : $"{B8e}{(B8 == 1 ? "" : "<sub>" + B8 + "</sub>")}";

        return $"{B1s} {B2s} {B3s} {B4s} {B5s} {B6s} {B7s} {B8s}";
    }
}
