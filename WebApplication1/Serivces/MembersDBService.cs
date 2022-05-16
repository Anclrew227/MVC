using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Serivces
{
    public class MembersDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        #region 註冊
        public void Register(Members newmember)
        {
            newmember.Password = HashPassword(newmember.Password);
            string sql = $@"INSERT INTO Members(Account,Password,Name,Email,Image,AuthCode,IsAdmin) VALUES ('{newmember.Account}','{newmember.Password}','{newmember.Name}','{newmember.Email}','{newmember.Image}','{newmember.AuthCode}','0'); ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        #region Hash密碼
        public string HashPassword(string Password)
        {
            string salt = "1q2w3e4r5t6y7u";
            string passwordAndsalt = string.Concat(Password, salt);
            SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();
            byte[] PasswordData = Encoding.Default.GetBytes(passwordAndsalt);
            byte[] HashData = SHA256.ComputeHash(PasswordData);
            return Convert.ToBase64String(HashData);
        }
        #endregion
        #region 搜尋一筆資料
        private Members GetDataByAccount(string Account)
        {
            Members Data = new Members();
            string sql = $@" SELECT * FROM Members WHERE Account = '{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Email = dr["Email"].ToString();
                Data.AuthCode = dr["AuthCode"].ToString();
                Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
            }
            catch(Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion
        #region 全區搜尋一筆資料
        public Members GetDatabyAccount(string Account)
        {
            Members Data = new Members();
            string sql = $@" SELECT * FROM Members WHERE Account = '{Account}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Name = dr["Name"].ToString();
                Data.Image = dr["Image"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion
        #region 帳號驗證
        public bool AccountCheck(string Account)
        {
            Members Data = GetDataByAccount(Account);
            return (Data == null);
        }
        #endregion
        #region 信箱驗證
        public string EmailValidate (string Account,string AuthCode)
        {
            Members member = GetDataByAccount(Account);
            if(member != null)
            {
                if(member.AuthCode == AuthCode)
                {
                    string sql = $@"UPDATE Members SET AuthCode = '{string.Empty}' WHERE Account = '{Account}'; ";
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return "驗證成功";
                }
                return "驗證碼錯誤";
            }
            return "資料傳輸錯誤";
        }
        #endregion
        #region 登入
        public string Login(string Account,string Password)
        {
            Members LoginMember = GetDataByAccount(Account);
            if(LoginMember != null)
            {
                if (string.IsNullOrWhiteSpace(LoginMember.AuthCode))
                {
                    if (CheckPassword(LoginMember, Password))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                }
                return "帳號尚未驗證";
            }
            return "帳號尚未註冊";
        }
        #endregion
        #region 確認密碼
        public bool CheckPassword(Members member ,string Password)
        {
            return (member.Password.Equals(HashPassword(Password)));
        }
        public string GetRole(string Account)
        {
            Members RoleMember = GetDataByAccount(Account);
            string RoleStr = "User";
            if (RoleMember.IsAdmin)
            {
                RoleStr += ",Admin";
            }
            return RoleStr;
        }
        #endregion
        #region 修改密碼
        public string ChangePassword(string Account,string Password,string newPassword)
        {
            Members member = GetDataByAccount(Account);
            if (CheckPassword(member, Password))
            {
                string sql = $@"UPDATE Members SET Password = '{HashPassword(newPassword)}' WHERE Account = '{Account}'; ";
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }
                finally
                {
                    conn.Close();
                }
                return "";
            }
            return "舊密碼輸入錯誤";
        }
        #endregion
        #region 確認大頭照
        public bool CheckImage(string Imagecontent)
        {
            switch (Imagecontent)
            {
                case "image/jpg":
                case "image/png":
                case "image/jpeg":
                    return true;
            }
            return false;
        }
        #endregion
        #region 查詢陣列資料
        public List<Members> GetDataList (ForPaging Paging,string Search)
        {
            List<Members> DataList = new List<Members>();
            SetMaxPage(Paging, Search);
            DataList = GetAllDataList(Paging, Search);
            return DataList ;
        }
        private void SetMaxPage(ForPaging Paging,string Search)
        {
            int row = 0;
            string sql = "";
            if (string.IsNullOrWhiteSpace(Search))
            {
                sql = $@"select * from Members; ";
            }
            else
            {
                sql = $@"select * from Members where Account LIKE '%{Search}%' or Name LIKE '%{Search}%' ;";
            }
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    row++;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Paging.ItemNum));
            Paging.SetRightPage();
        }
        private List<Members> GetAllDataList(ForPaging Paging,string Search)
        {
            List<Members> DataList = new List<Members>();
            string sql = "";
            if (string.IsNullOrWhiteSpace(Search))
            {
                sql = $@"select * from (select row_number() over (order by Account) as sort,* from Members)m where m.sort between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum} ;";
            }
            else
            {
                sql = $@"select * from (select row_number() over (order by Account) as sort,* from Members where Account like '%{Search}%' or Name like '%{Search}%')m where m.sort between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum} ;";
            }
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Members Data = new Members();
                    Data.Account = dr["Account"].ToString();
                    Data.Name = dr["Name"].ToString();
                    DataList.Add(Data);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        #endregion
    }
}