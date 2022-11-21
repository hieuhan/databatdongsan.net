using Dapper;
using databatdongsan.helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace databatdongsan.library
{
    public class Users
    {
        #region Properties

        public string ActBy { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte GetByUserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte GetByEmail { get; set; }
        public string Mobile { get; set; }
        public byte GetByMobile { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public DateTime BirthDay { get; set; }
        public string Comment { get; set; }
        public byte GenderId { get; set; }
        public byte UserStatusId { get; set; }
        public byte BuildIn { get; set; }
        public int Counter { get; set; }
        public string RoleName { get; set; }
        public DateTime CrDateTime { get; set; }
        public DateTime LastLoginAt { get; set; }
        public string LastPasswordChangeBy { get; set; }
        public DateTime LastPasswordChangeAt { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Users()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Users(string connection)
        {
            _connectionString = connection;
        }

        ~Users()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<Tuple<string, string>> Insert()
        {
            try
            {
                return await InsertOrUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<string, string>> Update()
        {
            try
            {
                return await InsertOrUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<string, string>> InsertOrUpdate()
        {
            string actionStatus = string.Empty, actionMessage = string.Empty;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                if (!string.IsNullOrEmpty(this.UserName))
                    param.Add("@UserName", this.UserName.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Password))
                    param.Add("@Password", this.Password.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.FullName))
                    param.Add("@FullName", this.FullName.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Email))
                    param.Add("@Email", this.Email.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Mobile))
                    param.Add("@Mobile", this.Mobile.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Address))
                    param.Add("@Address", this.Address.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Avatar))
                    param.Add("@Avatar", this.Avatar.InjectionString(), DbType.String);
                if (this.BirthDay != DateTime.MinValue)
                    param.Add("@BirthDay", this.BirthDay, DbType.Date);
                if (!string.IsNullOrEmpty(this.Comment))
                    param.Add("@Comment", this.Comment.InjectionString(), DbType.String);
                param.Add("@GenderId", this.GenderId, DbType.Byte);
                param.Add("@UserStatusId", this.UserStatusId, DbType.Byte);
                param.Add("@BuildIn", this.BuildIn, DbType.Byte);
                param.Add("@UserId", this.UserId, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                await connection.ExecuteAsync("Users_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                this.UserId = param.Get<int>("UserId");
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }

        public async Task<Tuple<string, string>> ChangePassword()
        {
            string actionStatus, actionMessage;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.Password))
                    param.Add("@Password", this.Password, DbType.String);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                await connection.ExecuteAsync("Users_Update_Password", param, commandType: CommandType.StoredProcedure);
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }
        public async Task<Tuple<string, string>> Delete()
        {
            string actionStatus = string.Empty, actionMessage = string.Empty;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                await connection.ExecuteAsync("Users_Delete", param, commandType: CommandType.StoredProcedure);
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }

        public async Task<Users> Get()
        {
            Users retVal = new Users();
            string keywords = string.Empty, orderBy = string.Empty;
            int pageIndex = 0, pageSize = 1;
            Tuple<List<Users>, int> tuple;
            try
            {
                tuple = await GetPage(keywords, orderBy, pageIndex, pageSize);
                if (tuple.Item1 != null && tuple.Item1.Count > 0) retVal = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public async Task<Tuple<List<Users>, int>> GetPage(string searchKeyword, string orderBy, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Users> listUsers;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                if (!string.IsNullOrEmpty(searchKeyword))
                    param.Add("@SearchKeyword", searchKeyword.InjectionString(), DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.UserName))
                    param.Add("@UserName", this.UserName.InjectionString(), DbType.String);
                param.Add("@GetByUserName", this.GetByUserName, DbType.Byte);
                if (!string.IsNullOrEmpty(this.Password))
                    param.Add("@Password", this.Password.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.FullName))
                    param.Add("@FullName", this.FullName.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Email))
                    param.Add("@Email", this.Email.InjectionString(), DbType.String);
                param.Add("@GetByEmail", this.GetByEmail, DbType.Byte);
                if (!string.IsNullOrEmpty(this.Mobile))
                    param.Add("@Mobile", this.Mobile.InjectionString(), DbType.String);
                param.Add("@GetByMobile", this.GetByMobile, DbType.Byte);
                if (!string.IsNullOrEmpty(this.Address))
                    param.Add("@Address", this.Address.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Avatar))
                    param.Add("@Avatar", this.Avatar.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Comment))
                    param.Add("@Comments", this.Comment.InjectionString(), DbType.String);
                param.Add("@GenderId", this.GenderId, DbType.Byte);
                param.Add("@UserStatusId", this.UserStatusId, DbType.Byte);
                if (!string.IsNullOrWhiteSpace(orderBy))
                    param.Add("@OrderBy", orderBy.InjectionString(), DbType.String);
                param.Add("@PageSize", pageSize, DbType.Int32);
                param.Add("@PageIndex", pageIndex, DbType.Int32);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                listUsers = await connection
                    .QueryAsync<Users>("Users_GetPage", param, commandType: CommandType.StoredProcedure)
                     as List<Users>;
                rowCount = param.Get<int>("RowCount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(listUsers, rowCount);
        }


        public byte HasPriv(string path = "")
        {
            byte hasPriv = 0;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (!string.IsNullOrEmpty(path))
                    param.Add("@Path", path, DbType.String);
                param.Add("@HasPriv", dbType: DbType.Byte, direction: ParameterDirection.Output);
                connection.Execute("Users_HasPriv", param, commandType: CommandType.StoredProcedure);
                hasPriv = param.Get<byte>("HasPriv");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return hasPriv;
        }
        public async Task<Tuple<string, string>> Update_LastLoginAt()
        {
            string actionStatus, actionMessage;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                await connection.ExecuteAsync("Users_Update_LastLoginAt", param, commandType: CommandType.StoredProcedure);
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }

        public async Task<Tuple<string, string>> Update_LastPasswordChangeAt()
        {
            string actionStatus, actionMessage;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.Password))
                    param.Add("@Password", this.Password, DbType.String);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                await connection.ExecuteAsync("Users_Update_LastPasswordChangeAt", param, commandType: CommandType.StoredProcedure);
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }

        #endregion

        #region Static Methods

        public static byte Static_GetHasPriv(string actBy, int userId, string path)
        {
            Users users = new Users
            {
                ActBy = actBy,
                UserId = userId
            };
            return users.HasPriv(path);
        }

        public static async Task<Users> Static_GetById(string actBy, int userId)
        {
            Users users = new Users
            {
                ActBy = actBy,
                UserId = userId
            };
            return await users.Get();
        }

        public static async Task<Users> Static_GetById(string actBy, int userId, byte getByEmail)
        {
            Users users = new Users
            {
                ActBy = actBy,
                UserId = userId,
                GetByEmail = getByEmail
            };
            return await users.Get();
        }

        public static async Task<Tuple<string, string>> Static_Update_LastLoginAt(string actBy, int userId)
        {
            return await new Users
            {
                ActBy = actBy,
                UserId = userId
            }.Update_LastLoginAt();
        }

        public static async Task<Tuple<string, string>> Static_Update_LastPasswordChangeAt(string actBy, int userId, string password)
        {
            return await new Users
            {
                ActBy = actBy,
                UserId = userId,
                Password = password
            }.Update_LastPasswordChangeAt();
        }

        public static async Task<Users> Static_GetByUserName(string userName)
        {
            return await new Users
            {
                UserName = userName,
                GetByUserName = 1
            }.Get();
        }

        public static async Task<Users> Static_GetByEmail(string actBy, string email)
        {
            return await new Users
            {
                ActBy = actBy,
                Email = email,
                GetByEmail = 1
            }.Get();
        }

        public static async Task<Users> Static_GetByMobile(string actBy, string mobile)
        {
            return await new Users
            {
                ActBy = actBy,
                Mobile = mobile,
                GetByMobile = 1
            }.Get();
        }

        #endregion
    }
}
