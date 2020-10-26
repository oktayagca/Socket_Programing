using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KullanıcıKayıtRehberi
{
   public class VeriTabanı
    {
        public string conString = ("Data Source=OKTAY;Initial Catalog=DataCom;Integrated Security=True");
        String soru;
        String cevap;
        public String soruCek(int id)
        {
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand("Select SORU from SORULAR where Id=@perId", con);

            cmd.Parameters.Add("@perId", SqlDbType.Int).Value = id;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                soru = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }

            con.Close();

            return soru;
        }
        public String cevapCek(int id)
        {
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand("Select CEVAP from SORULAR where Id=@perId", con);

            cmd.Parameters.Add("@perId", SqlDbType.Int).Value = id;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cevap = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }

            con.Close();

            return cevap;
        }
    }
}
