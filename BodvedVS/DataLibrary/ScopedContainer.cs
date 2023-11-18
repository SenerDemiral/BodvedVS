using Microsoft.AspNetCore.Components;

namespace BodvedVS.DataLibrary;

public interface IScopedContainer
{
    bool IsOk { get; set; }
    int UsrId { get; set; }
    bool IsAdm { get; set; }
    bool IsTnm { get; set; }
    public int RoleSnc { get; set; }

    bool CheckLogin(int id, string pwd);
}

public class ScopedContainer : IScopedContainer
{
    IDataAccess db;
    public ScopedContainer(IDataAccess access)
    {
        db = access;
    }
    public int UsrId { get; set; } = 0;
    public bool IsOk { get; set; } = false;
    public bool IsAdm { get; set; } = false;
    public bool IsTnm { get; set; } = false;
    public int RoleSnc { get; set; } = 0;
    public List<int> AuthLigList { get; set; } = new();
    
    
    public bool CheckLogin(int id, string pwd)
    {
        LoginModel lm = db.LoadRec<LoginModel, dynamic>("select * from LOGIN(@Id, @Pwd)", new { Id = id, Pwd = pwd });

        if (lm.isOk > 0)
        {
            IsOk = true;
            UsrId = id;
            IsAdm = lm.RoleAdm > 0 ? true : false;
            IsTnm = lm.RoleTnm > 0 ? true : false;
            RoleSnc = lm.RoleSnc;
            return true;
        }
        return false;
    }
}
