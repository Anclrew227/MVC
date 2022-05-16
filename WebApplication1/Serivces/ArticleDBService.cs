using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Serivces
{
    public class ArticleDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public Article GetArticleById(int A_Id)
        {
            Article Data = new Article();
            string sql = $@"select m.*,d.Name,d.Image from Article m inner join Members d on m.Account = d.Account where A_Id = '{A_Id}' ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                Data.Account = dr["Account"].ToString();
                Data.Title = dr["Title"].ToString();
                Data.Content = dr["Content"].ToString();
                Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                Data.Watch = Convert.ToInt32(dr["Watch"]);
                Data.Member.Name = dr["Name"].ToString();
                Data.Member.Image = dr["Image"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        public List<Article> GetDataList(string Account, string Search, ForPaging Paging)
        {
            List<Article> DataList = new List<Article>();
            SetMaxPage(Account, Search, Paging);
            DataList = GetAllDataList(Account, Search, Paging);
            return DataList;
        }
        private void SetMaxPage(string Account, string Search, ForPaging Paging)
        {
            int row = 0;
            string sql = "";
            if (string.IsNullOrWhiteSpace(Search))
            {
                sql = $@"select * from Article m inner join Members d on m.Account = d.Account where m.Account = '{Account}' ;";
            }
            else
            {
                sql = $@"select * from Article m inner join Members d on m.Account = d.Account where (Title LIKE '%{Search}%' or Content LIKE '%{Search}%') and m.Account = '{Account}' ;";
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
            catch (Exception e)
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
        private List<Article> GetAllDataList(string Account, string Search, ForPaging Paging)
        {
            List<Article> DataList = new List<Article>();
            string sql = "";
            if (string.IsNullOrWhiteSpace(Search))
            {
                sql = $@"select m.*,d.Name from (select row_number() over (order by A_Id) as sort,* from Article where Account = '{Account}' )m inner join Members d on m.Account = d.Account where sort between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum} ;";
            }
            else
            {
                sql = $@"select m.*,d.Name from (select row_number() over (order by A_Id) as sort,* from Article where (Title LIKE '%{Search}%' or Content LIKE '%{Search}%') and Account = '{Account}' )m inner join Members d on m.Account = d.Account where sort between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum} ;";
            }
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Article Data = new Article();
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Title = dr["Title"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Watch = Convert.ToInt32(dr["Watch"]);
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
        public void InsertArticle (Article Data)
        {
            string sql = $@"insert into Article(Account,Title,Content,CreateTime,Watch) values ('{Data.Account}','{Data.Title}','{Data.Content}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}','0') ;";
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
        public void UpdateArticle(Article Data)
        {
            string sql = $@"update Article set Content = '{Data.Content}' where A_Id = '{Data.A_Id}' ;";
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
        public void DeleteArticle(int A_Id)
        {
            string sql = $@"delete from Message where A_Id = '{A_Id}'
                            delete from Article where A_Id = '{A_Id}';";
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
        public bool CheckUpdate(int A_Id)
        {
            Article Data = GetArticleById(A_Id);
            int count = 0;
            string sql = $@"select * from Message where A_Id = '{A_Id}' ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    count++;
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
            return (Data != null && count.Equals(0));
        }
        public void AddWatch(int A_Id)
        {
            string sql = $@"update Article set Watch += 1 where A_Id = '{A_Id}' ;";
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
        public List<Article> ShowPopular(string Account)
        {
            List<Article> DataList = new List<Article>();
            string sql = $@"select top 5 * from Article m inner join Members d on m.Account = d.Account where m.Account = '{Account}' order by Watch desc ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Article Data = new Article();
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Title = dr["Title"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                    Data.Watch = Convert.ToInt32(dr["Watch"]);
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
    }
}