using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;
using System.IO;

namespace WebApplication1.Serivces
{
    public class AlbumDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public Album GetAlbumById(int Alb_Id)
        {
            Album Data = new Album();
            string sql = $@"select m.*,d.Name from Album m inner join Members d on m.Account = d.Account where Alb_Id = '{Alb_Id}' ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Alb_Id = Convert.ToInt32(dr["Alb_Id"]);
                Data.FileName = dr["FileName"].ToString();
                Data.Url = dr["Url"].ToString();
                Data.Size = Convert.ToInt32(dr["Size"]);
                Data.Type = dr["Type"].ToString();
                Data.Account = dr["Account"].ToString();
                Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                Data.Member.Name = dr["Name"].ToString();
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
        public List<Album> GetDataList(ForPaging Paging)
        {
            SetMaxPage(Paging);
            List<Album> DataList = new List<Album>();
            string sql = $@"select m.*,d.Name from (select row_number() over (order by CreateTime desc) as sort ,* from Album)m inner join Members d on m.Account = d.Account where m.sort between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum} ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Album Data = new Album();
                    Data.Alb_Id = Convert.ToInt32(dr["Alb_Id"]);
                    Data.FileName = dr["FileName"].ToString();
                    Data.Url = dr["Url"].ToString();
                    Data.Size = Convert.ToInt32(dr["Size"]);
                    Data.Type = dr["Type"].ToString();
                    Data.Account = dr["Account"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Member.Name = dr["Name"].ToString();
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        private void SetMaxPage(ForPaging Paging)
        {
            int row = 0;
            string sql = $@"select * from Album ;";
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
        public void UploadFile(string FileName, string Url, int Size, string Type, string Account)
        {
            string sql = $@"insert into Album(FileName,Url,Size,Type,Account,CreateTime) values ('{FileName}','{Url}','{Size}','{Type}','{Account}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}') ;";
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
        }
        public void Delete(int Alb_Id)
        {
            string sql = $@"delete from Album where Alb_Id = '{Alb_Id}' ;";
            try
            {
                Album Data = GetAlbumById(Alb_Id);
                File.Delete(Data.Url);
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
        }
    }
}