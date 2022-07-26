Imports System.IO
Imports System.Text
Imports System.Configuration
Imports System.Web
Imports MySqlConnector
'' Data Access Class: Maintain a DB connection to database.
'' Execute query in transaction or non-transaction mode.
'' Retrive data from database to external structure storage [dataset,recordset,collection,array,etc]
Namespace AgoraNEO.EAD
    Public Class DBCom
        Public gcnConnOle As OleDb.OleDbConnection
        Private gcmComOle As OleDb.OleDbCommand
        Private gdapDaOle As OleDb.OleDbDataAdapter
        Private gtraRecOle As OleDb.OleDbTransaction
        Private gdrdReadOle As OleDb.OleDbDataReader

        Public gcnConn As MySqlConnection
        Private gcmCom As MySqlCommand
        Private gdapDa As MySqlDataAdapter
        Private gtraRec As MySqlTransaction
        Private gdrdRead As MySqlDataReader

        Public gds As DataSet
        Public gstrConn As String
        Public gstrConnOle As String
        Private gstrSQL As String
        Private gstrErrMsg As String
        Private lngLoop As Long
        Private gExp As Exception


        ''Function  : Constructor for DBAccess Class. It will accept global connection string 
        ''          : for the priority of pass input string->global existing value->config file variable[path]
        Public Sub New(Optional ByVal pstrConn As String = vbNullString)
            If pstrConn <> vbNullString Then
                gstrConn = pstrConn
            Else
                If gstrConn Is Nothing Then
                    gstrConn = ConfigurationManager.AppSettings("Path")
                    'gstrConn = ConfigurationSettings.AppSettings("eProcurePath")
                    'gstrConn = "server=127.0.0.1;UID=root;pwd=p@ssw0rd;database=eprocure"
                End If
            End If

        End Sub

        ''Function  : Establish a connection to server.
        Private Function ConnDB() As Boolean
            ConnDB = True
            Try
                If gcnConn Is Nothing Then
                    gcnConn = New MySqlConnection(gstrConn)
                    'gstrConn = "workstation id=DSB3000136AC;packet size=4096;user id=sa;password=manager01;data source=SCSDSB;initial catalog=eAuction"
                    'gcnConn1 = New SqlClient.SqlConnection(gstrConn)

                    gcnConn.Open()
                End If
            Catch exp As Exception
                gstrErrMsg = exp.Message
                TrwExp(exp)
                Return False
            End Try
        End Function

        Private Function ConnDBOle() As Boolean
            ConnDBOle = True
            Try
                If gcnConnOle Is Nothing Then
                    gcnConnOle = New OleDb.OleDbConnection(gstrConnOle)
                    'gstrConn = "workstation id=DSB3000136AC;packet size=4096;user id=sa;password=manager01;data source=SCSDSB;initial catalog=eAuction"
                    'gcnConn1 = New SqlClient.SqlConnection(gstrConn)

                    gcnConnOle.Open()
                End If
            Catch exp As Exception
                gstrErrMsg = exp.Message
                TrwExp(exp)
                Return False
            End Try
        End Function

        ''Function  : check database connection to server.
        Public Function ConnState() As Boolean
            ConnState = True
            Try
                If gcnConn Is Nothing Then
                    gcnConn = New MySqlConnection(gstrConn)
                    gcnConn.Open()
                End If
                Return True
            Catch exp As Exception
                Return False
            End Try
        End Function

        Public Function ConnStateOle() As Boolean
            ConnStateOle = True
            Try
                If gcnConnOle Is Nothing Then
                    gcnConnOle = New OleDb.OleDbConnection(gstrConnOle)
                    gcnConnOle.Open()
                End If
                Return True
            Catch exp As Exception
                Return False
            End Try
        End Function

        ''Function  : close database connection to server.
        Public Function DisConn() As Boolean
            DisConn = True
            Try
                If Not gcnConn Is Nothing Then
                    'If gcnConn.State.ToString = ConnectionState.Open.ToString Or _
                    'gcnConn.State.ToString = ConnectionState.Connecting.ToString Then
                    gcnConn.Close()
                    'End If
                    gcnConn = Nothing
                End If
                System.GC.Collect()
            Catch exp As Exception
                DisConn = False
            End Try
        End Function

        Public Function DisConnOle() As Boolean
            DisConnOle = True
            Try
                If Not gcnConnOle Is Nothing Then
                    'If gcnConn.State.ToString = ConnectionState.Open.ToString Or _
                    'gcnConn.State.ToString = ConnectionState.Connecting.ToString Then
                    gcnConnOle.Close()
                    'End If
                    gcnConnOle = Nothing
                End If
                System.GC.Collect()
            Catch exp As Exception
                DisConnOle = False
            End Try
        End Function

        Public Sub CallSPeRFP(ByVal coyID As String, ByVal userID As String, ByVal rfpid As Integer, ByVal versionNo As Integer, ByVal pending As String, ByVal procurement As String)

            Try
                If ConnDB() Then

                    Dim cmdStPro As New MySqlCommand
                    'Dim pending2 As Char

                    'pending2 = CChar(pending)

                    coyID = HttpContext.Current.Session("CompanyId")
                    userID = HttpContext.Current.Session("UserId")

                    cmdStPro = CallSP("RFP_DOC2")

                    cmdStPro.Parameters.Add(New MySqlParameter("@rfpid", MySqlDbType.Int32, 9))
                    cmdStPro.Parameters("@rfpid").Value = rfpid

                    cmdStPro.Parameters.Add(New MySqlParameter("@versionNo", MySqlDbType.Int32))
                    cmdStPro.Parameters("@versionNo").Value = versionNo

                    cmdStPro.Parameters.Add(New MySqlParameter("@pending", MySqlDbType.VarChar, 1))
                    cmdStPro.Parameters("@pending").Value = pending

                    cmdStPro.Parameters.Add(New MySqlParameter("@coyID", MySqlDbType.VarChar, 20))
                    cmdStPro.Parameters("@coyID").Value = coyID

                    cmdStPro.Parameters.Add(New MySqlParameter("@procurement", MySqlDbType.VarChar, 10))
                    cmdStPro.Parameters("@procurement").Value = procurement

                    ExecuteSP(cmdStPro)


                End If
            Catch expErr As Exception

                DisConn()

            Finally

            End Try
        End Sub

        Private Function CallSP(ByVal sp_name As String) As MySqlCommand

            Try
                If ConnDB() Then

                    gcmCom = New MySqlCommand(sp_name, gcnConn)
                    gcmCom.CommandType = Data.CommandType.StoredProcedure
                    Return gcmCom

                End If
            Catch expErr As Exception

                DisConn()
                Return Nothing
            Finally

            End Try
        End Function

        Private Sub ExecuteSP(ByVal SP_COMMAND As MySqlCommand)
            SP_COMMAND.ExecuteNonQuery()
            DisConn()
        End Sub

        Public Function GetLatestInsertedID(ByVal sTableName As String) As String
            Dim stConvertedQry As String = ""
            stConvertedQry = "(SELECT LAST_INSERT_ID() FROM " & sTableName & " LIMIT 1)"
            'FOR MSSQL "SELECT TOP 1 IDENT_CURRENT('RFQ_MSTR') FROM RFQ_MSTR"
            GetLatestInsertedID = stConvertedQry
        End Function

        Private Function sConvertStringToMYSQL(ByVal stQry As String) As String
            Dim stMYSQLQry As String = stQry
            stMYSQLQry = Replace(stMYSQLQry, "ISNULL", "IFNULL", 1, -1, CompareMethod.Text)
            stMYSQLQry = Replace(stMYSQLQry, "VARCHAR(100)", "CHAR", 1, -1, CompareMethod.Text)
            stMYSQLQry = Replace(stMYSQLQry, "GETDATE()", "CURRENT_DATE()", 1, -1, CompareMethod.Text)
            'Add by Joon on 23 Sept 2010
            stMYSQLQry = Replace(stMYSQLQry, "DATEADD(YEAR,1000,CURRENT_DATE())", "DATE_ADD(CURRENT_DATE(),INTERVAL 1000 YEAR)", 1, -1, CompareMethod.Text)
            stMYSQLQry = Replace(stMYSQLQry, "CONVERT(MONEY,'0.00')", "CONVERT('0.00',DECIMAL(19,4))", 1, -1, CompareMethod.Text)

            'Michelle (21/9/2010) - Replace [] with '
            'Michelle (22/12/2010) - comment off the replacement of [] with ' if not, it will encountered error when user enter a '[' in the Proudct description
            'If Not InStr(stMYSQLQry, "'[") > 0 Then
            '    stMYSQLQry = Replace(stMYSQLQry, "[", "'")
            'End If
            'If Not InStr(stMYSQLQry, "]'") > 0 Then
            '    stMYSQLQry = Replace(stMYSQLQry, "]", "'")
            'End If

            'stMYSQLQry = Replace(stMYSQLQry, "IDENT_CURRENT", "LAST_INSERT_ID", 1, -1, CompareMethod.Text)
            sConvertStringToMYSQL = stMYSQLQry
        End Function

        ''Function  : To Execute a single SQL query.
        Public Function Execute(ByVal pstrSQL As String) As Boolean
            Execute = True
            Try
                If ConnDB() Then
                    pstrSQL = sConvertStringToMYSQL(pstrSQL)
                    gcmCom = New MySqlCommand
                    gtraRec = gcnConn.BeginTransaction
                    gcmCom.Transaction = gtraRec

                    gcmCom.Connection = gcnConn
                    gcmCom.CommandText = pstrSQL
                    gcmCom.ExecuteNonQuery()
                    gtraRec.Commit()
                    DisConn()
                End If
            Catch expErr As Exception
                If Not gtraRec Is Nothing Then gtraRec.Rollback()
                TrwExp(expErr, pstrSQL)
                DisConn()
                Return False
            Finally

            End Try
        End Function

        ''Function  : To Execute a batch of SQL query in the one dimension array .
        Public Function BatchExecute(ByRef pstrSQL() As String,
                                     Optional ByVal pblnTran As Boolean = True) As Boolean
            BatchExecute = True
            Dim strSQL As String = ""
            Try
                ConnDB()
                gcmCom = New MySqlCommand
                If pblnTran Then
                    'Try
                    gtraRec = gcnConn.BeginTransaction
                    gcmCom.Transaction = gtraRec
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                    gtraRec.Commit()
                    'Catch exp As Exception
                    'If Not gtraRec Is Nothing Then gtraRec.Rollback()
                    'TrwExp(exp, pstrSQL(lngLoop))
                    'Return False
                    'Finally
                    '    DisConn()
                    'End Try
                Else
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                End If
            Catch expErr As Exception
                TrwExp(expErr, pstrSQL(lngLoop))
                Return False
            Finally
                DisConn()
            End Try
        End Function

        Public Function BatchExecuteNew(ByRef pstrSQL() As String,
                                     Optional ByVal pblnTran As Boolean = True, Optional ByRef pTranNo As String = "", Optional ByVal pTranVal As String = "") As Boolean
            BatchExecuteNew = True
            Dim strSQL As String = ""
            Try
                ConnDB()
                gcmCom = New MySqlCommand
                If pblnTran Then
                    'Try
                    gtraRec = gcnConn.BeginTransaction
                    gcmCom.Transaction = gtraRec
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                    gtraRec.Commit()

                    If pTranVal <> "" Then
                        pTranNo = GetVal(" SELECT @" & pTranVal & "; ")
                    End If

                    'Catch exp As Exception
                    'If Not gtraRec Is Nothing Then gtraRec.Rollback()
                    'TrwExp(exp, pstrSQL(lngLoop))
                    'Return False
                    'Finally
                    '    DisConn()
                    'End Try
                Else
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                End If
            Catch expErr As Exception
                TrwExp(expErr, pstrSQL(lngLoop))
                Return False
            Finally
                DisConn()
            End Try
        End Function

        Public Function BatchExecuteForDup(ByRef pstrSQL() As String,
                                     Optional ByVal pblnTran As Boolean = True, Optional ByRef pTranNo As String = "", Optional ByVal pTranVal As String = "") As Boolean
            BatchExecuteForDup = True
            Dim strSQL As String = ""
            Dim pDup_NO As String = ""
            Try
                ConnDB()
                gcmCom = New MySqlCommand
                If pblnTran Then
                    'Try
                    gtraRec = gcnConn.BeginTransaction
                    gcmCom.Transaction = gtraRec
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next

                    '20210612: remark: seem @DUPLICATE_CHK not setted
                    'pDup_NO = CStr(GetValNoCloseConn(" SELECT CAST(IFNULL(@DUPLICATE_CHK,'') AS CHAR(1000)); "))

                    If pDup_NO = "" Then
                        gtraRec.Commit()
                        If pTranVal <> "" Then
                            pTranNo = GetVal(" SELECT @" & pTranVal & "; ")
                        End If
                    Else
                        pTranNo = "Generated"
                        gtraRec.Rollback()
                    End If
                Else
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                End If
            Catch expErr As Exception
                TrwExp(expErr, pstrSQL(lngLoop))
                Return False
            Finally
                DisConn()
            End Try
        End Function

        Public Function BatchExecuteForGRNDup(ByRef pstrSQL() As String,
                                     Optional ByVal pblnTran As Boolean = True, Optional ByRef pTranNo As String = "", Optional ByVal pTranVal As String = "",
                                     Optional ByRef pTranNo2 As String = "", Optional ByRef pTranVal2 As String = "") As Boolean
            BatchExecuteForGRNDup = True
            Dim strSQL As String = ""
            Dim pDup_NO As String = ""
            Try
                ConnDB()
                gcmCom = New MySqlCommand
                If pblnTran Then
                    'Try
                    gtraRec = gcnConn.BeginTransaction
                    gcmCom.Transaction = gtraRec
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next

                    'pDup_NO = CStr(GetValNoCloseConn(" SELECT CAST(IFNULL(@DUPLICATE_CHK,'') AS CHAR(1000)); "))

                    If pDup_NO = "" Then
                        gtraRec.Commit()
                        If pTranVal <> "" Then
                            'pTranNo = GetVal(" SELECT @" & pTranVal & "; ")
                            pTranNo = GetVal(" SELECT CAST(@" & pTranVal & " AS CHAR(20)); ")
                        End If
                        If pTranVal2 <> "" Then
                            'pTranNo2 = GetVal(" SELECT @" & pTranVal2 & "; ")
                            pTranNo2 = GetVal(" SELECT CAST(IFNULL(@" & pTranVal2 & ",'') AS CHAR(255)); ")
                        End If
                    Else
                        If pDup_NO = "ACK" Then
                            pTranNo = "exist2"
                        Else
                            pTranNo = "Generated"
                        End If

                        gtraRec.Rollback()
                    End If
                Else
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                End If
            Catch expErr As Exception
                TrwExp(expErr, pstrSQL(lngLoop))
                Return False
            Finally
                DisConn()
            End Try
        End Function

        Public Function BatchExecuteForIPP(ByRef pstrSQL() As String,
                                     Optional ByVal pblnTran As Boolean = True, Optional ByRef pTranNo As String = "", Optional ByVal role As String = "") As Boolean
            BatchExecuteForIPP = True
            Dim strSQL As String = ""
            Dim pDup_NO As String = ""
            Try
                ConnDB()
                gcmCom = New MySqlCommand
                If pblnTran Then
                    'Try
                    gtraRec = gcnConn.BeginTransaction
                    gcmCom.Transaction = gtraRec
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next

                    pDup_NO = CStr(GetValNoCloseConn(" SELECT CAST(IFNULL(@DUPLICATE_CHK,'') AS CHAR(1000)); "))

                    If (pDup_NO = "") Then
                        gtraRec.Commit()
                    Else
                        pTranNo = "Generated"
                        gtraRec.Rollback()
                    End If
                Else
                    gcmCom.Connection = gcnConn
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmCom.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmCom.ExecuteNonQuery()
                    Next
                End If
            Catch expErr As Exception
                TrwExp(expErr, pstrSQL(lngLoop))
                Return False
            Finally
                DisConn()
            End Try
        End Function

        Public Function BatchExecuteOle(ByRef pstrSQL() As String,
                                     Optional ByVal pblnTran As Boolean = True) As Boolean
            BatchExecuteOle = True
            Dim strSQL As String = ""
            Try
                ConnDBOle()
                gcmComOle = New OleDb.OleDbCommand
                If pblnTran Then
                    'Try
                    gtraRecOle = gcnConnOle.BeginTransaction
                    gcmComOle.Transaction = gtraRecOle
                    gcmComOle.Connection = gcnConnOle
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmComOle.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmComOle.ExecuteNonQuery()

                        'gcmComOle.CommandText = "CREATE TABLE EmployeeData (Id MEMO , Name char(255), BirthDate date)"
                        'gcmComOle.ExecuteNonQuery()
                        'gcmComOle.CommandText = "INSERT INTO [EmployeeData$] values ('led: 30pcs ir ledswarranty: 2 yearsa/ 30pcs F8 IR leds b/ IR distance 80M c/ MIN lllumination 0.001Lux/F1.2 d/ 4mm 6mm 8mm 12mm 16mm lens OPTION e/ ce fccFor FULL specification details: pls refer ledswarranty: 2 yearsa/ 30pcs F8 IR leds b/ IR distance 80M c/ MIN lllumination 0.001Lux/F1.2 d/ 4mm 6mm 8mm 12mm 16mm lens OPTION e/ ce fccFor FULL specification details: plr TO FILE attachment', 'Andrew', '12/4/1955')"
                        'gcmComOle.ExecuteNonQuery()
                    Next
                    gtraRecOle.Commit()
                    'Catch exp As Exception
                    'If Not gtraRec Is Nothing Then gtraRec.Rollback()
                    'TrwExp(exp, pstrSQL(lngLoop))
                    'Return False
                    'Finally
                    '    DisConn()
                    'End Try
                Else
                    gcmComOle.Connection = gcnConnOle
                    For lngLoop = 0 To UBound(pstrSQL)
                        gcmComOle.CommandText = sConvertStringToMYSQL(pstrSQL(lngLoop))
                        gcmComOle.ExecuteNonQuery()
                    Next
                End If
            Catch expErr As Exception
                TrwExp(expErr, pstrSQL(lngLoop))
                Return False
            Finally
                DisConnOle()
            End Try
        End Function

        ''Function  : Return a new Dataset from SQL retrive query .
        Public Function FillDs(ByVal pstrSQL As String, Optional ByVal pAcceptChange As Boolean = True) As DataSet
            Try
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                Dim ds As New DataSet
                ConnDB()
                gdapDa = New MySqlDataAdapter(pstrSQL, gcnConn)
                gdapDa.AcceptChangesDuringFill = pAcceptChange
                gdapDa.Fill(ds, "table")
                gdapDa.Dispose()
                Return ds
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return Nothing
            Finally
                DisConn()
            End Try
        End Function
        'Michelle (14/5/2014) - To cater for Billing with timeout issue
        Public Function FillDsT(ByVal pstrSQL As String, Optional ByVal pAcceptChange As Boolean = True) As DataSet
            Try
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                Dim ds As New DataSet
                ConnDB()
                gdapDa = New MySqlDataAdapter(pstrSQL, gcnConn)
                gdapDa.SelectCommand.CommandTimeout = 10000
                gdapDa.AcceptChangesDuringFill = pAcceptChange
                gdapDa.Fill(ds, "table")
                gdapDa.Dispose()
                Return ds
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return Nothing
            Finally
                DisConn()
            End Try
        End Function

        Public Function FillDsOle(ByVal pstrSQL As String, Optional ByVal pAcceptChange As Boolean = True) As DataSet
            Try
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                Dim ds As New DataSet
                ConnDBOle()
                gdapDaOle = New OleDb.OleDbDataAdapter(pstrSQL, gcnConnOle)
                gdapDaOle.AcceptChangesDuringFill = pAcceptChange
                gdapDaOle.Fill(ds, "table")
                gdapDaOle.Dispose()
                Return ds
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return Nothing
            Finally
                DisConnOle()
            End Try
        End Function

        ''Function: special fill method for dataset , remain data in the dataset.
        Public Function FillDsIn(ByRef pds As DataSet,
                                 ByVal pstrSQL As String,
                                 ByVal pTableName As String) As Boolean
            FillDsIn = True
            Try
                ConnDB()
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                gdapDa = New MySqlDataAdapter(pstrSQL, gcnConn)
                gdapDa.Fill(pds, pTableName)
                gdapDa.Dispose()
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return False
            Finally
                DisConn()
            End Try
        End Function

        ''Function  : Return a new DataTable from SQL retrive query .
        Public Function FillDt(ByVal pstrSQL As String) As DataTable
            Dim ds As New DataSet
            Dim dt As New DataTable
            Try
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                ds = FillDs(pstrSQL)
                dt = ds.Tables("table")
                Return dt
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return Nothing
            Finally
            End Try
        End Function

        ''Function  : Return a new DataView from SQL retrive query .
        Public Function GetView(ByVal pstrSQL As String) As DataView
            Try
                Dim ds As New DataSet
                ConnDB()
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                gdapDa = New MySqlDataAdapter(pstrSQL, gcnConn)
                gdapDa.Fill(ds, "table")
                gdapDa.Dispose()
                If ds.Tables(0).Rows.Count <> 0 Then
                    Return ds.Tables(0).DefaultView
                Else
                    Return Nothing
                End If
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return Nothing
            Finally
                DisConn()
            End Try
        End Function

        ''Function  : Return a new DataView from SQL retrive query .
        'Public Function GetReader(ByVal pstrSQL As String) As MySqlDataReader
        '    Try
        '        ConnDB()
        '        gcmCom = New MySqlCommand(pstrSQL, gcnConn)
        '        gdrdRead = gcmCom.ExecuteReader(CommandBehavior.CloseConnection)
        '        Return gdrdRead
        '    Catch exp As Exception
        '        DisConn()
        '        TrwExp(exp, pstrSQL)
        '        Return Nothing
        '    Finally
        '        'DisConn()
        '        gcnConn = Nothing
        '    End Try
        'End Function

        ''Function  : Check whether the SQL query return any data. 
        ''exist false = 0 ,exist true = 1,error = -1
        Public Function Exist(ByVal pstrSQL As String) As Integer
            Try
                ConnDB()
                pstrSQL = sConvertStringToMYSQL(pstrSQL)
                Dim ds As New DataSet
                gdapDa = New MySqlDataAdapter(pstrSQL, gcnConn)
                gdapDa.Fill(ds, "table")
                If ds.Tables(0).Rows.Count = 0 Then
                    ds = Nothing
                    Return 0
                Else
                    ds = Nothing
                    Return 1
                End If
            Catch exp As Exception
                TrwExp(exp, pstrSQL)
                Return -1
            Finally
                DisConn()
            End Try
        End Function

        ''Function : Fill the input dataset with current table schema
        Public Function FillSchema(ByVal pTable As String,
                                  ByRef pDs As DataSet,
                                  Optional ByVal pList As String = "",
                                  Optional ByVal pTableNo As String = "") As Boolean
            Try
                FillSchema = True
                ConnDB()
                Dim strSQL As String
                If pList = "" Then
                    strSQL = "SELECT * FROM " & pTable
                Else
                    strSQL = "SELECT " & pList & " FROM " & pTable
                End If
                gdapDa = New MySqlDataAdapter(strSQL, gcnConn)
                If pTableNo = "" Then
                    gdapDa.FillSchema(pDs, SchemaType.Source, pTable)
                Else
                    gdapDa.FillSchema(pDs, SchemaType.Source, pTableNo)
                End If
            Catch exp As Exception
                TrwExp(exp)
                Return False
            Finally
                DisConn()
            End Try
        End Function

        ''Function : Get the next sequence key from a table.
        Public Function GetNext(ByVal pTable As String,
                                ByRef pKey As String) As Long
            Dim strSQL As String = ""
            Try
                ConnDB()
                Dim i As Long
                strSQL = " SELECT MAX(" & pKey & ") FROM " & pTable
                Dim gcmCom As MySqlCommand = New MySqlCommand(strSQL, gcnConn)
                gcmCom.CommandText = strSQL
                i = IIf(IsDBNull(gcmCom.ExecuteScalar()), 0, gcmCom.ExecuteScalar())
                Return i + 1
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return 1
            Finally
                DisConn()
            End Try

        End Function

        ''Function : Get max record count number from a table
        Public Function GetMax(ByVal pTable As String, ByVal pField As String,
                                 Optional ByVal pCondition As String = "") As Long
            Dim strSQL As String
            Try
                ConnDB()
                Dim i As Long
                strSQL = " SELECT max(" & pField & ") FROM " & pTable & " " & pCondition
                Dim gcmCom As MySqlCommand = New MySqlCommand(strSQL, gcnConn)
                gcmCom.CommandText = strSQL
                If Not IsDBNull(gcmCom.ExecuteScalar()) Then
                    i = gcmCom.ExecuteScalar()
                Else
                    i = 0
                End If
                Return i
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return -1
            Finally
                DisConn()
            End Try

        End Function

        ''Function : Get the record count number from a table
        Public Function GetCount(ByVal pTable As String,
                                 Optional ByVal pCondition As String = "") As Long
            Dim strSQL As String
            Try
                ConnDB()
                Dim i As Long
                strSQL = " SELECT count('*') FROM " & pTable & " " & pCondition
                Dim gcmCom As MySqlCommand = New MySqlCommand(strSQL, gcnConn)
                gcmCom.CommandText = strSQL
                i = gcmCom.ExecuteScalar()
                Return i
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return -1
            Finally
                DisConn()
            End Try

        End Function
        'Example: SELECT RM_RFQ_ID FROM RFQ_MSTR ORDER BY RM_RFQ_ID DESC LIMIT 1
        Public Function GetLatestInsertedID2(ByVal pTable As String, ByVal pPrimaryKey As String) As Long
            Dim strSQL As String = ""
            Try
                ConnDB()
                Dim i As Long
                strSQL = " SELECT " & pPrimaryKey & " FROM " & pTable & " ORDER BY " & pPrimaryKey & " DESC LIMIT 1"
                Dim gcmCom As MySqlCommand = New MySqlCommand(strSQL, gcnConn)
                gcmCom.CommandText = strSQL
                i = gcmCom.ExecuteScalar()
                Return i
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return -1
            Finally
                DisConn()
            End Try
        End Function

        ''Function  : Return a single Dataset from SQL retrive query .
        Public Function Fill1Ds(ByVal pTable As String, ByVal pFieldName As String, Optional ByVal pCondition As String = "") As DataSet
            Dim strSQL As String = ""
            strSQL = " SELECT " & pFieldName & " FROM " & pTable & " " & pCondition & " LIMIT 1"
            Try
                Dim ds As New DataSet
                ConnDB()
                gdapDa = New MySqlDataAdapter(strSQL, gcnConn)
                gdapDa.AcceptChangesDuringFill = True
                gdapDa.Fill(ds, "table")
                gdapDa.Dispose()
                Return ds
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return Nothing
            Finally
                DisConn()
            End Try
        End Function

        'SELECT RM_RFQ_No FROM RFQ_MSTR LIMIT 1
        Public Function Get1Column(ByVal pTable As String, ByVal pFieldName As String, Optional ByVal pCondition As String = "") As String
            Dim strSQL As String = ""
            Dim reader As MySqlDataReader
            reader = Nothing
            Try
                ConnDB()
                Dim sReturn As String
                strSQL = " SELECT " & pFieldName & " FROM " & pTable & pCondition & " LIMIT 1"
                Dim cmd As New MySqlCommand(sConvertStringToMYSQL(strSQL), gcnConn)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If Not IsDBNull(reader.GetUInt32(0)) Then
                        sReturn = CStr(reader.GetUInt32(0))
                    Else
                        sReturn = ""
                    End If
                Else
                    sReturn = ""
                End If
                Return sReturn
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return ""
            Finally
                DisConn()
            End Try
        End Function
        Public Function Get1ColumnCheckNull(ByVal pTable As String, ByVal pFieldName As String, Optional ByVal pCondition As String = "") As String
            Dim strSQL As String = ""
            Dim reader As MySqlDataReader
            reader = Nothing
            Try
                ConnDB()
                Dim sReturn As String
                strSQL = " SELECT " & pFieldName & " FROM " & pTable & pCondition & " LIMIT 1"
                Dim cmd As New MySqlCommand(sConvertStringToMYSQL(strSQL), gcnConn)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If IsDBNull(reader.GetValue(0)) Then
                        sReturn = ""
                    Else
                        If Not IsDBNull(reader.GetString(0)) Then
                            sReturn = CStr(reader.GetString(0))
                        Else
                            sReturn = ""
                        End If
                    End If
                Else
                    sReturn = ""
                End If
                Return sReturn
            Catch exp As Exception
                TrwExp(exp, strSQL)
                Return ""
            Finally
                DisConn()
            End Try
        End Function

        ''Function : Get the single value from SQL
        Public Function GetVal(ByVal pSQL As String) As String
            Dim str As String = ""
            Dim reader As MySqlDataReader
            reader = Nothing
            Try
                ConnDB()
                pSQL = sConvertStringToMYSQL(pSQL)
                Dim cmd As New MySqlCommand(pSQL, gcnConn)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If Not IsDBNull(reader.GetValue(0)) Then
                        str = CStr(reader.GetString(0))
                    Else
                        str = ""
                    End If
                Else
                    str = ""
                End If

                Return str
            Catch exp As Exception
                TrwExp(exp, str)
                Return Nothing
            Finally
                If Not reader Is Nothing Then reader.Close()
                DisConn()
            End Try

        End Function
        Public Function GetValInt(ByVal pSQL As String) As String
            Dim str As String = ""
            Dim reader As MySqlDataReader
            reader = Nothing
            Try
                ConnDB()
                pSQL = sConvertStringToMYSQL(pSQL)
                Dim cmd As New MySqlCommand(pSQL, gcnConn)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If Not IsDBNull(reader.GetValue(0)) Then
                        str = CStr(reader.GetInt32(0))
                    Else
                        str = ""
                    End If
                Else
                    str = ""
                End If

                Return str
            Catch exp As Exception
                TrwExp(exp, str)
                Return Nothing
            Finally
                If Not reader Is Nothing Then reader.Close()
                DisConn()
            End Try

        End Function
        'Andhika 22-04-2021'
        Public Function GetValDecimal(ByVal pSQL As String) As Decimal
            Dim str As String = ""
            Dim reader As MySqlDataReader
            reader = Nothing
            Try
                ConnDB()
                pSQL = sConvertStringToMYSQL(pSQL)
                Dim cmd As New MySqlCommand(pSQL, gcnConn)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If Not IsDBNull(reader.GetDecimal(0)) Then
                        str = CDec(reader.GetDecimal(0))
                    Else
                        str = ""
                    End If
                Else
                    str = ""
                End If

                Return str
            Catch exp As Exception
                TrwExp(exp, str)
                Return Nothing
            Finally
                If Not reader Is Nothing Then reader.Close()
                DisConn()
            End Try

        End Function
        Public Function GetValNoCloseConn(ByVal pSQL As String) As String
            Dim str As String = ""
            Dim reader As MySqlDataReader
            reader = Nothing
            Try
                ConnDB()
                pSQL = sConvertStringToMYSQL(pSQL)
                Dim cmd As New MySqlCommand(pSQL, gcnConn)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    If Not IsDBNull(reader.GetString(0)) Then
                        str = CStr(reader.GetString(0))
                    Else
                        str = ""
                    End If
                Else
                    str = ""
                End If

                Return str
            Catch exp As Exception
                TrwExp(exp, str)
                Return Nothing
            Finally
                If Not reader Is Nothing Then reader.Close()
                'DisConn()
            End Try

        End Function

        ''Function : Encoding algorithm
        Public Function Encode(ByVal pValue As String) As String
            Dim iloop As Integer
            Dim pKey As String = "5"
            Dim StrResult As New StringBuilder("")
            If Mid(pValue, 1, 1) = "@" Then
                For iloop = 1 To Len(pValue) - 1
                    Dim instr As Char '= Mid(pValue, iloop + 1, 1)
                    instr = Chr(CLng(Asc(Mid(pValue, iloop + 1, 1))) + CLng(pKey))
                    StrResult.Append(instr)
                Next
            Else
                For iloop = 0 To Len(pValue) - 1
                    Dim instr As Char '= Mid(pValue, iloop + 1, 1)
                    instr = Chr(CLng(Asc(Mid(pValue, iloop + 1, 1))) - CLng(pKey))
                    StrResult.Append(instr)
                Next
                StrResult.Insert(0, "@")
            End If
            Return StrResult.ToString
        End Function

        Private Function EncodeString(ByVal pPath As String,
                                      ByVal pFilename As String) As String
            Dim objStreamWriter As StreamWriter
            Dim objStreamReader As StreamReader
            Dim ReadStr As String
            Dim WriteStr As String
            Dim NwReadStr As String
            objStreamReader = New StreamReader(pPath & "\" & pFilename & ".txt", True)
            ReadStr = objStreamReader.ReadLine
            If ReadStr <> "" Then
                If Mid(ReadStr, 1, 1) <> "@" Then
                    NwReadStr = Encode(ReadStr)
                    WriteStr = NwReadStr
                    objStreamWriter = New StreamWriter(pPath & "\" & pFilename & ".txt", False)
                    objStreamWriter.WriteLine(WriteStr)
                    objStreamWriter.Close()
                    objStreamWriter = Nothing
                Else
                    ReadStr = Encode(ReadStr)
                End If
            End If
            objStreamReader.Close()
            objStreamReader = Nothing
            Return ReadStr
        End Function

        Private Function TrwExp(ByVal pExp As Exception, Optional ByVal pIn As String = "") As Exception
            Dim strMessage As String
            If pIn = "" Then
                strMessage = pExp.Message
            Else
                strMessage = "[" & pIn.ToString & "]--" & pExp.Message
            End If
            'Throw
            Throw New Exception(strMessage, pExp)
        End Function

        Private Function TrwExp(ByVal pIn As String) As Exception
            Dim expError As New Exception("customized expection : " & pIn)
            HttpContext.Current.Session("ErrMsg") = "No Error"
            'HttpContext.Current.Response.Redirect("ErrorPage.aspx")
            'Throw expError
            'Throw New Exception("test-", pExp)
        End Function

        Public Function WriteLog(ByVal Msg As String,
                                         Optional ByVal strPath As String = "") As Boolean

            WriteLog = True
            Dim objStreamWriter As StreamWriter
            Dim WriteStr As String = ""
            Try
                WriteStr &= "SQL     :" & Msg & vbCrLf
                'WriteStr &= "SOURCE  :" & Exp.Source & vbCrLf
                'WriteStr &= "MESSAGE :" & Exp.Message & vbCrLf
                'WriteStr &= "STACK   :" & Exp.StackTrace & vbCrLf
                WriteStr &= "DATE/TIME :" & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf

                objStreamWriter = New StreamWriter(strPath & "\Error.log", True)
                objStreamWriter.WriteLine(WriteStr)

            Catch Meexp As Exception
                WriteLog = False
            Finally

                objStreamWriter.Close()
                objStreamWriter = Nothing
            End Try
        End Function

        Public Function Concat(ByVal sPara1 As String, ByVal sPara2 As String, ByVal sField1 As String, Optional ByVal sField2 As String = "", Optional ByVal sField3 As String = "", Optional ByVal sField4 As String = "", Optional ByVal sField5 As String = "") As String
            'Add by Joon on 23 Sept 2010
            'Mod by Sam on 24 Sept 2010

            'Example Usage
            'If 1 parameter -space
            'CONCAT(AM_ADDR_LINE1, ' ', AM_ADDR_LINE2)
            'If 2 different parameter - ( & )
            'CONCAT('(',AM_ADDR_LINE1,')')
            Concat = ""
            Try
                If sPara2 = "" Then
                    If Not sField2 = "" Then
                        sField2 = ", '" & sPara1 & "'" & "," & sField2
                    End If
                    If Not sField3 = "" Then
                        sField3 = ", '" & sPara1 & "'" & "," & sField3
                    End If
                    If Not sField4 = "" Then
                        sField4 = ", '" & sPara1 & "'" & "," & sField4
                    End If
                    If Not sField5 = "" Then
                        sField5 = ", '" & sPara1 & "'" & "," & sField5
                    End If
                    Concat = "CONCAT(" & sField1 & sField2 & sField3 & sField4 & sField5 & ")"
                Else
                    Concat = "CONCAT('" & sPara1 & "'," & sField1 & ",'" & sPara2 & "')"
                End If


            Catch ex As Exception

            End Try
        End Function

        Public Function Concat2(ByVal sPara1 As String, ByVal sPara2 As String, ByVal sField1 As String, ByVal sField2 As String, ByVal sField3 As String) As String
            'Add by Joon on 23 Sept 2010
            'Mod by Sam on 24 Sept 2010

            'Example Usage
            'If 1 parameter -space
            'CONCAT(AM_ADDR_LINE1, ' ', AM_ADDR_LINE2)
            'If 2 different parameter - ( & )
            'CONCAT('(',AM_ADDR_LINE1,')')
            'For different parameters
            Concat2 = ""
            Try

                Concat2 = "CONCAT(" & sField1 & ",'" & sPara1 & "'," & sField2 & ",'" & sPara2 & "'," & sField3 & ")"

            Catch ex As Exception

            End Try
        End Function


        Public Function Concat3(ByVal sPara1 As String, ByVal sPara2 As String, ByVal sField1 As String, ByVal sField2 As String) As String
            'Add by Sam on 9 Oct 2010

            'For Tax usage
            Concat3 = ""
            Try
                Concat3 = "CAST(CONCAT(" & sField1 & ",'" & sPara1 & "'," & sField2 & ",'" & sPara2 & "') AS CHAR)"
            Catch ex As Exception

            End Try
        End Function

        'Michelle (27/9/2010) - To cater for multiple fields & param 
        Public Function MultiConcat(ByVal sField1 As String, ByVal sPara1 As String, ByVal sField2 As String, Optional ByVal sPara2 As String = "", Optional ByVal sField3 As String = "", Optional ByVal sPara3 As String = "", Optional ByVal sField4 As String = "", Optional ByVal sPara4 As String = "", Optional ByVal sField5 As String = "", Optional ByVal sPara5 As String = "", Optional ByVal sField6 As String = "", Optional ByVal sPara6 As String = "", Optional ByVal sField7 As String = "", Optional ByVal sPara7 As String = "", Optional ByVal sField8 As String = "") As String
            MultiConcat = ""
            Try

                'Concat2 = "CONCAT(" & sField1 & ",'" & sPara1 & "'," & sField2 & ",'" & sPara2 & "'," & sField3 & ")"
                MultiConcat = "CONCAT(" & sField1 & ",'" & sPara1 & "'," & sField2 & ",'" & sPara2 & "'," &
                          sField3 & ",'" & sPara4 & "'," & sField4 & ",'" & sPara5 & "'," & sField5 & ",'" & sPara6 & "'," &
                           sField7 & ",'" & sPara7 & "'," & sField8 & ")"

            Catch ex As Exception

            End Try
        End Function

        Public Function ReturnArrayValue(ByVal dv As Array, ByVal intMaxIndex As Integer) As String
            Dim i As Integer = 0

            ReturnArrayValue = ""

            For i = 0 To intMaxIndex
                ReturnArrayValue = ReturnArrayValue & Chr(dv(i))
            Next
        End Function
    End Class
End Namespace