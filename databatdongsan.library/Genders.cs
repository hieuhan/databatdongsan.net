using databatdongsan.helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace databatdongsan.library
{
    public class Genders
    {
        #region Properties

        public string ActBy { get; set; }
        public byte GenderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime CrDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Genders()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Genders(string connection)
        {
            _connectionString = connection;
        }

        ~Genders()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<List<Genders>> GetList()
        {
            List<Genders> listGenders;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                listGenders = await connection.QueryAsync<Genders>("SELECT * FROM [dbo].[Genders]") as List<Genders>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return listGenders;
        }

        #endregion

        #region Static Methods

        public static async Task<List<Genders>> Static_GetList()
        {
            Genders genders = new Genders();
            return await genders.GetList();
        }

        public static Genders Static_Get(byte genderId, List<Genders> list)
        {
            Genders retVal = new Genders();
            if (genderId > 0 && list != null && list.Count > 0)
            {
                retVal = list.Find(i => i.GenderId == genderId) ?? new Genders();
            }
            return retVal;
        }

        #endregion
    }
}
