using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data;

namespace Anh.License
{
    public class DbConnect
    {
        public DbConnect()
        {
            //
            // TODO: Add constructor logic here。
            //
        }

        #region Field variable group
        // Get connection string collection
        private ConnectionStringSettingsCollection _ConnSets = ConfigurationManager.ConnectionStrings;

        // Declaration and Instantiation of OracleConnection Object
        private OracleConnection _OracleConnection = new OracleConnection();

        // Declaration of OracleTransaction object
        private OracleTransaction _OracleTransaction;

        #endregion

        #region Property group
        /// <summary>
        /// Get OracleConnection object.
        /// </summary>
        public OracleConnection Conn
        {
            get
            {
                return _OracleConnection;
            }
        }

        /// <summary>
        /// Get OracleTransaction object.
        /// </summary>
        public OracleTransaction Tran
        {
            get
            {
                return _OracleTransaction;
            }
        }

        /// <summary>
        /// Get the connection status with the database. true: Connected false: Not connected
        /// This doesn't make much sense.
        /// Because this class is always instantiated and used, it will usually be unconnected
        /// </summary>
        public bool ConnState
        {
            get
            {
                ConnectionState state = Conn.State;
                switch (state)
                {
                    //supports ConnectionState.Closed, ConnectionState.Open seems
                    // The default value is ConnectionState.Closed
                    case ConnectionState.Open:
                        return true;
                    default:
                        return false;
                }
            }
        }
        #endregion

        #region DataBase methods

        #region Open：Database connection
        /// <summary>
        /// Connect to the database ※ We do not process when we are connected.
        /// </summary>
        public void Open()
        {
            Open("MainUser");
        }
        #endregion

        #region Open：Database connection (specified by user)
        /// <summary>
        /// Connect to the database ※ We do not process when we are connected.
        /// </summary>
        /// <param name="sUserName">Specify the connection destination user. Specify only if different from the main user.</param>
        public void Open(string asUserName)
        {
            // Only connect if not connected
            if (ConnState == false)
            {
                try
                {
                    // ConnectionString settings
                    _OracleConnection.ConnectionString = _ConnSets[asUserName].ConnectionString;

                    // OPEN the database
                    // An exception occurs if connection can not be made
                    Conn.Open();

                    return;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return;
        }
        #endregion

        #region Close：Database disconnection
        /// <summary>
        /// Disconnect from the database
        /// </summary>
        public void Close()
        {
            if (Conn != null)
            {
                Conn.Close();
            }
            return;
        }
        #endregion

        #region BeginTran：Transaction start
        /// <summary>
        /// Start a transaction
        /// </summary>
        public void BeginTran()
        {
            _OracleTransaction = Conn.BeginTransaction();
        }
        #endregion

        #region DisposeTran：Transaction end
        /// <summary>
        /// End the transaction
        /// </summary>
        public void DisposeTran()
        {
            // Is it not necessary to do it consciously because the database is disconnected?
            Tran.Dispose();
        }
        #endregion

        #region Commit：コミット
        /// <summary>
        /// Commit changes to the database.
        /// </summary>
        public void Commit()
        {
            Tran.Commit();
        }
        #endregion

        #region Rollback：roll back
        /// <summary>
        /// Roll back changes to the database.
        /// </summary>
        public void Rollback()
        {
            Tran.Rollback();
        }
        #endregion

        #region Static GetDBConnection
        public static OracleConnection GetDBConnection(string asAppName)
        {
            // Connection String.
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings[asAppName].ConnectionString;

            OracleConnection conn = new OracleConnection(connString);

            return conn;
        }
        #endregion
        #endregion
    }
}
