﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;

namespace BodvedVS.DataLibrary;

public class FBDataAccess : IDataAccess
{
    string cnctStr = "";

    public FBDataAccess(IConfiguration config)
    {
        //_config = config;
        cnctStr = config.GetConnectionString("Default");
        FbConnectionStringBuilder csb = new FbConnectionStringBuilder(cnctStr);
        //csb.WireCrypt = FbWireCrypt.Disabled;
        //csb.ConnectionTimeout = 15;
        //csb.Pooling = false;
        csb.PacketSize = 16384;
        //csb.ServerType = FbServerType.Embedded;   // Hata
        //csb.Charset = FbCharset.Utf8.ToString(); //"WIN1254"; //Hata geliyor default: Utf8
        csb.ClientLibrary = "fbclient.dll"; //Default: fbembed
        cnctStr = csb.ConnectionString;
    }
    public FBDataAccess(string cnctString)
    {
        cnctStr = cnctString;
        //TableDefs.initTableDefs();

    }

    public async Task<T> LoadRecAsync<T, U>(string sql, U parameters)
    {
        using (IDbConnection cnct = new FbConnection(cnctStr))
        {
            return await cnct.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
    }

    public T LoadRec<T, U>(string sql, U parameters)
    {
        using (IDbConnection cnct = new FbConnection(cnctStr))
        {
            return cnct.QueryFirstOrDefault<T>(sql, parameters);
        }
    }

    public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string sql, U parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        return await cnct.QueryAsync<T>(sql, parameters); //.ConfigureAwait(false); bunu bekliyorsun
        //var bbb = aaa.ToList();
        //return aaa;
    }

    public IEnumerable<T> LoadData<T, U>(string sql, U parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        return cnct.Query<T>(sql, parameters); //.ConfigureAwait(false); bunu bekliyorsun
    }

    public IQueryable<T> LoadDataQ<T, U>(string sql, U parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        return cnct.Query<T>(sql, parameters).AsQueryable<T>();
    }

    /// <summary>
    /// Ins/Upd/Del StoreProcedure returns Row (multiple fields)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="storeProc"></param>
    /// <param name="parameters"></param>
    /// SP input params db de tanimlandigi sirasiyla gelmeli. Her ikisi de calisiyor
    /// "select * from Usr_Login(@LgnNme, @LgnPwd, @Ip)"
    /// "execute procedure Usr_Login(@LgnNme, @LgnPwd, @Ip)"
    /// https://www.tabsoverspaces.com/227021-firebird-net-provider-and-calling-stored-procedures-with-parameters
    /// <returns></returns>
    public async Task<T> StoreProcAsync<T, U>(string storeProc, U parameters)
    {
        // var params = new { UserName = username, Password = password };
        using IDbConnection cnct = new FbConnection(cnctStr);
        //var aaa = await connection.QueryAsync<T>(storeProc, parameters, commandType: CommandType.StoredProcedure).Sing
        //var aaa = await connection.QueryFirstOrDefault<T>(storeProc, parameters, commandType: CommandType.StoredProcedure);
        //var bbb = connection.Query<T>(storeProc, parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
        //return await cnct.QueryFirstOrDefaultAsync<T>(storeProc, parameters, commandType: CommandType.StoredProcedure);
        return await cnct.QueryFirstOrDefaultAsync<T>("execute procedure "+storeProc, parameters);
        //return await cnct.QueryFirstOrDefaultAsync<T>("execute procedure Usr_Login(@LgnNme, @LgnPwd, @Ip)", parameters);
    }
    public T StoreProc<T, U>(string storeProc, U parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        return cnct.QueryFirstOrDefault<T>("execute procedure " + storeProc, parameters);
    }

    public bool SaveRec<T>(string sql, T parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        var nora = cnct.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
        return nora == 0 ? false: true ;   // Affected NOR yoksa(0), Save edilemedi
    }

    public async Task<bool> SaveDataAsync<T>(string sql, T parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        var nora = await cnct.ExecuteAsync(sql, parameters);
        return nora == 0 ? false: true ;   // Affected NOR yoksa(0), Save edilemedi
    }
    public async Task SaveDataProc<T>(string sp, T parameters)
    {
        using IDbConnection cnct = new FbConnection(cnctStr);
        
        Console.WriteLine("connectionString");
        Console.WriteLine(cnctStr);
        
        await cnct.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure);
    }

    /*
    https://makolyte.com/csharp-adding-dynamic-query-parameters-with-dapper/
    //Built dynamically somewhere
    var query = "SELECT * FROM Movies WHERE Name=@Name AND YearOfRelease=@Year";
    var parameters = new Dictionary<string, object>()
    {
        ["Year"] = 1999
    };

    //Using hardcoded (known) parameters  + dynamic parameters
    using (var con = new SqlConnection(connectionString))
    {
        var dynamicParameters = new DynamicParameters(parameters);

        dynamicParameters.AddDynamicParams(new { name = "The Matrix" });

        var results = con.Query<Movie>(query, dynamicParameters);
        return results;
    }
     */

    public int GetTablePK(string tblName)
    {
        int pk = 0;
        using (IDbConnection connection = new FbConnection(cnctStr))
        {
            pk = connection.ExecuteScalar<int>($"select ID from Get_PK('{tblName}')");
        }
        return pk;
    }

    public int CONN_INC_GET(string usrGuid, int usrId)
    {
        int conn = 0;
        using (IDbConnection connection = new FbConnection(cnctStr))
        {
            conn = connection.ExecuteScalar<int>($"select Conn from CONN_INC_GET('{usrGuid}',{usrId})");
        }
        return conn;
    }

    public int CONN_GIR(string usrGuid, int usrId)
    {
        int conn = 0;
        using (IDbConnection connection = new FbConnection(cnctStr))
        {
            conn = connection.ExecuteScalar<int>($"select Conn from CONN_GIR('{usrGuid}',{usrId})");
        }
        return conn;
    }

    public void CONN_CIK(string usrGuid)
    {
        int conn = 0;
        using (IDbConnection connection = new FbConnection(cnctStr))
        {
            conn = connection.ExecuteScalar<int>($"select * from CONN_CIK('{usrGuid}')");
        }
    }
}
