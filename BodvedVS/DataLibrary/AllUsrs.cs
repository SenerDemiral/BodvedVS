using System.Collections.Concurrent;

namespace BodvedVS.DataLibrary;

public interface IAllUsrs
{
	int GetConnCnt();
	UsrInf TryGetUsr(string tkn);
	void ClearExpiredUsrs();
}

public class AllUsrs : IAllUsrs
{

	private ConcurrentDictionary<string, UsrInf> Usrs;
	private int ConnCnt;
	private IDataAccess db;

	public AllUsrs(IDataAccess dataAccess)
	{
		db = dataAccess;
		Usrs = new();
	}

	public int GetConnCnt() => ConnCnt;

	public UsrInf TryGetUsr(string tkn)
	{
		UsrInf usrInf = new();

		if (Usrs.ContainsKey(tkn))
		{
			Usrs[tkn].LastUpdTS = DateTime.Now;
			Usrs.TryGetValue(tkn, out usrInf);
		}
		else
		{
			// get Usr, TotalCnt from DB
			var usrInfDB = db.LoadRec<UsrInfDB, dynamic>("select * from CONN_GIR(@Tkn)", new { Tkn = tkn });

			usrInf.Id = usrInfDB.PPId;
			usrInf.Ad = usrInfDB.Ad;
			usrInf.IsAdm = usrInfDB.RoleAdm == 0 ? false : true;
			usrInf.IsTnm = usrInfDB.RoleTnm == 0 ? false : true;
			usrInf.IsSnc = usrInfDB.RoleSnc == 0 ? false : true;

			Usrs.TryAdd(tkn, usrInf);
			ConnCnt = usrInfDB.ConnCnt;
		}

		return usrInf;
	}

	public void ClearExpiredUsrs()
	{
		var Now = DateTime.Now;
		foreach (var item in Usrs.Where(kvp => (Now - kvp.Value.LastUpdTS).TotalMinutes > 59).ToList())
		{
			Usrs.TryRemove(item.Key, out var _);
		}
	}

}

public class UsrInf
{
	public int Id { get; set; } = 0;
	public string Ad { get; set; } = "Guest";
	public bool IsAdm { get; set; } = false;
	public bool IsTnm { get; set; } = false;
	public bool IsSnc { get; set; } = false;
	public DateTime LastUpdTS { get; set; } = DateTime.Now;

}

public class UsrInfDB
{
	public int PPId { get; set; } = 0;
	public string Ad { get; set; } = "Guest";
	public int RoleAdm { get; set; }
	public int RoleTnm { get; set; }
	public int RoleSnc { get; set; }
	public int ConnCnt { get; set; }

}
