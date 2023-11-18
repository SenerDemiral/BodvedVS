namespace BodvedVS.DataLibrary;

public sealed class LoginModel
{
    public int isOk { get; set; } = 0;
    public int RoleAdm { get; set; } = 0;
    public int RoleTnm { get; set; } = 0;
    public int RoleSnc { get; set; } = 0;
}

public sealed class UsrPLS
{
    public int usrid { get; set; }
    public string usrpwd { get; set; }
}
