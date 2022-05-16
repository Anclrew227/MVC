using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Serivces
{
    public class MessageDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public List<Message> GetDataList(int A_Id, ForPaging Paging)
        {
            List<Message> DataList = new List<Message>();
            SetMaxPage(A_Id, Paging);
            DataList = GetAllDataList(A_Id, Paging);
            return DataList;
        }
       private void SetMaxPage(int A_Id, ForPaging Paging)
        {
            int row = 0;
            string sql = $@"select * from Message m inner join Members d on m.Account = d.Account where A_Id = '{A_Id}' ;";
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
        private List<Message> GetAllDataList(int A_Id, ForPaging Paging)
        {
            List<Message> DataList = new List<Message>();
            string sql = $@"select * from (select row_number() over (order by M_Id) as sort,* from Message where A_Id = '{A_Id}') m inner join Members d on m.Account = d.Account where sort between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum} ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Message Data = new Message();
                    Data.A_Id = Convert.ToInt32(dr["A_Id"]);
                    Data.M_Id = Convert.ToInt32(dr["M_Id"]);
                    Data.Account = dr["Account"].ToString();
                    Data.Content = dr["Content"].ToString();
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
        public void InsertMessage(Message Data)
        {
            string sql = $@"insert into Message(A_Id,Account,Content,CreateTime) values ('{Data.A_Id}','{Data.Account}','{Data.Content}','{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}') ;";
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
        public void UpdateMessage(Message Data)
        {
            string sql = $@"update Message set Content = '{Data.Content}' where A_Id = '{Data.A_Id}' and M_Id = '{Data.M_Id}' ;";
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
        public void DeleteMessage(int A_Id,int M_Id)
        {
            string sql = $@"delete from Message where A_Id = '{A_Id}' and M_Id = '{M_Id}' ;";
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
    }
}