using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Serivces
{
    public class CartService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        #region 取得購物車內商品陣列
        public List<CartSave> GetItemFromCart(string Cart)
        {
            List<CartSave> DataList = new List<CartSave>();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Item d ON m.Item_Id = d.Id WHERE Cart_Id = '{Cart}'; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CartSave Data = new CartSave();
                    Data.Cart_Id = dr["Cart_Id"].ToString();
                    Data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
                    Data.Item.Id = Convert.ToInt32(dr["Id"]);
                    Data.Item.Name = dr["Name"].ToString();
                    Data.Item.Image = dr["Image"].ToString();
                    Data.Item.Price = Convert.ToInt32(dr["Price"]);
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
        #endregion
        #region 確認商品是否於購物車中
        public bool CheckInCart(string Cart, int Item_Id)
        {
            CartSave Data = new CartSave();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Item d ON m.Item_Id = d.Id WHERE Cart_Id = '{Cart}' AND Item_Id = '{Item_Id}' ;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
                Data.Item.Id = Convert.ToInt32(dr["Id"]);
                Data.Item.Name = dr["Name"].ToString();
                Data.Item.Image = dr["Image"].ToString();
                Data.Item.Price = Convert.ToInt32(dr["Price"]);
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return (Data != null);
        }
        #endregion
        #region 取得購物車保存
        public string GetCartSave(string Account)
        {
            CartSave Data = new CartSave();
            string sql = $@"SELECT * FROM CartSave m INNER JOIN Members d ON m.Account = d.Account WHERE m.Account = '{Account}'; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Cart_Id = dr["Cart_Id"].ToString();
                Data.Member.Name = dr["Name"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            if (Data != null)
            {
                return Data.Cart_Id;
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region 放入和保存購物車
        public void SaveCart(string Account, string Cart, int Id)
        {
            string sql = $@"INSERT INTO CartSave(Account,Cart_Id,Item_Id) VALUES('{Account}','{Cart}','{Id}'); ";
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
        #endregion
        #region 放出和取消保存購物車
        public void SaveCartRemove(string Cart, int Id)
        {
            string sql = $@"DELETE FROM CartSave WHERE Cart_Id = '{Cart}' AND Item_Id = '{Id}' ;";
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
        #endregion
    }
}