using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Tbot.Model;

namespace Tbot
{

    /* Stewie 24/09/2021
     * 
     * Class to manage the data on log database and on bot database
     */
    class clSQL
    {
        SQLiteConnection xDbMasterConnection = null;
        SQLiteConnection xDbLogConnection = null;
        string sDbMasterConnectionString = "";
        string sDbLogConnectionString = "";
        public enEsitoFunzione mInit()
        {
            try
            {
                sDbMasterConnectionString = "Data Source = " + AppContext.BaseDirectory + "\\DB\\dbBot.sqlite;Version=3";
                sDbLogConnectionString = "Data Source = " + AppContext.BaseDirectory + "\\DB\\dbBot_log.sqlite;Version=3";
                //if (xDbMasterConnection == null)
                //    xDbMasterConnection = new SQLiteConnection(sDbMasterConnectionString);

                //xDbMasterConnection.Open();

                //Check if the log database exist.
                //If it doesn't exist the method create the file
                mCheckExistDbLog();
                //Set the connection
                if (xDbLogConnection == null)
                    xDbLogConnection = new SQLiteConnection(sDbLogConnectionString);
                //Open the connection
                xDbLogConnection.Open();
                //Check if the table tbLog exist
                if (mCheckTbLog() == enEsitoFunzione.kKo)
                    //Create the table
                    mCreateTbLog();



                return enEsitoFunzione.kOk;
            }
            catch(Exception ex)
            {
                return enEsitoFunzione.kKo;
            }
        }

        private void mCheckExistDbLog()
        {
            try
            {
                if (System.IO.Directory.Exists(AppContext.BaseDirectory + "\\DB") == false)
                    System.IO.Directory.CreateDirectory(AppContext.BaseDirectory + "\\DB");

                if (System.IO.File.Exists(AppContext.BaseDirectory + "\\DB\\dbBot_Log.sqlite") == false)
                    SQLiteConnection.CreateFile(AppContext.BaseDirectory + "\\DB\\dbBot_Log.sqlite");
                
            }
            catch(Exception ex)
            {

            }
        }

        private enEsitoFunzione mCheckTbLog()
        {
            try
            {
                string sSQL = @"SELECT Name
                              FROM  sqlite_master
                              WHERE type='table' AND Name ='tbLog'";

                SQLiteCommand xLocCommand = new SQLiteCommand(sSQL, xDbLogConnection);
                object obj = xLocCommand.ExecuteScalar();

                if (obj != null)
                {
                    if (obj.ToString() != "")
                        return enEsitoFunzione.kOk;
                    else
                        return enEsitoFunzione.kKo;
                }
                else
                    return enEsitoFunzione.kKo;
            }
            catch (Exception ex)
            {
                //mLog("mCheckTableCelestial", -1, 0, "Exception: " + ex.Message);
                return enEsitoFunzione.DoNothing;
            }
        }
        private int mCreateTbLog()
        {
            try
            {
                string sSQL = @"CREATE TABLE tbLog (
                                pLogId       INTEGER          NOT NULL
                                                              PRIMARY KEY AUTOINCREMENT,
                                sApplication [NVARCHAR] (50),
                                sMethod      [NVARCHAR] (50),
                                nCodeI1      [INT],
                                nCodeI2      [INT],
                                sDescription [NVARCHAR] (200) 
                            );";
                SQLiteCommand xLocCommand = new SQLiteCommand(sSQL, xDbLogConnection);

                xLocCommand.ExecuteNonQuery();

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public enEsitoFunzione mLog(string sMethod, int nCodeI1, int nCodeI2, string sException)
        {
            try
            {
                string sSQL = @"INSERT INTO tbLog (sApplication,sMethod,nCodeI1,nCodeI2,sDescription)
                                        VALUES (@sApplication,@sMethod,@nCodeI1,@nCodeI2,@sDescription)";

                SQLiteCommand xLocCommand = new SQLiteCommand(sSQL, xDbLogConnection);

                xLocCommand.Parameters.AddWithValue("@sApplication", "T-Bot");
                xLocCommand.Parameters.AddWithValue("@sMethod", sMethod);
                xLocCommand.Parameters.AddWithValue("@nCodeI1", nCodeI1);
                xLocCommand.Parameters.AddWithValue("@nCodeI2", nCodeI2);
                xLocCommand.Parameters.AddWithValue("@sDescription", sException);

                xLocCommand.ExecuteNonQuery();

                return enEsitoFunzione.kOk;
            }
            catch (Exception ex)
            {
                return enEsitoFunzione.kKo;
            }
        }
    }

    /*****************START ENUM******************/
    

    

    /*****************END ENUM******************/
}
