'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports System
Imports AgoraNEO
Imports SSO.Component
Imports System.Web.UI.WebControls
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Web

Namespace AgoraNEO
    Public Class Inventory
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals

        'Function GetErrorMessage(ByVal strErrNo As String) As String
        '    Dim strSQL As String
        '    Dim ds As DataSet

        '    strSQL = "SELECT EM_ERR_MESSAGE " _
        '            & "FROM err_message " _
        '            & "WHERE EM_ERR_NO='" & strErrNo & "'"
        '    ds = objDb.FillDs(strSQL)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        GetErrorMessage = ds.Tables(0).Rows(0).Item("EM_ERR_MESSAGE")
        '    End If
        '    ds = Nothing
        'End Function

        'Function GetLocation(ByVal strCoyID As String, ByVal blnDefault As Boolean, Optional ByVal strDefaultType As String = "") As Integer
        '    Dim strSQL As String
        '    Dim ds As DataSet

        '    strSQL = "SELECT UD_DEFAULT_VALUE " _
        '            & "FROM user_default " _
        '            & "WHERE UD_COY_ID='" & strCoyID & "'"

        '    If blnDefault = True Then
        '        strSQL = strSQL & " AND UD_DEFAULT_TYPE='WH'"

        '    Else
        '        If strDefaultType <> "" Then
        '            strSQL = strSQL & " AND UD_DEFAULT_TYPE='" & strDefaultType & "'"
        '        End If
        '    End If

        '    ds = objDb.FillDs(strSQL)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        GetLocation = ds.Tables(0).Rows(0)("UD_DEFAULT_VALUE")
        '    End If

        'End Function

        Public Function GetDafaultLocation(ByVal strCoyID As String, ByVal strUserID As String) As Integer
            Dim strSQL As String
            Dim ds As DataSet

            If strCoyID = "" Then
                strCoyID = HttpContext.Current.Session("CompanyID")
            End If
            If strUserID = "" Then
                strUserID = HttpContext.Current.Session("UserID")
            End If

            strSQL = "SELECT UD_DEFAULT_VALUE " _
                    & "FROM user_default " _
                    & "WHERE UD_COY_ID='" & Common.Parse(strCoyID) & "' " _
                    & "AND UD_USER_ID='" & Common.Parse(strUserID) & "' " _
                    & "AND UD_DEFAULT_TYPE='WH'"

            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then
                GetDafaultLocation = ds.Tables(0).Rows(0).Item("UD_DEFAULT_VALUE")
            End If
            ds = Nothing
        End Function

        Public Function GetLocationInfo(ByRef LocDesc As String, ByRef SubLocDesc As String, ByRef LocIndicator As Integer)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet

            'Get indicator
            strSQL = "SELECT * " _
                    & "FROM location_mstr " _
                    & "WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND (LM_SUB_LOCATION IS NOT NULL AND LM_SUB_LOCATION<>'')"

            ds = objDb.FillDs(strSQL)
            If ds.Tables(0).Rows.Count > 0 Then 'If sub location found
                LocIndicator = 2
            Else
                LocIndicator = 1
            End If

            'Get location & sub location description
            strSQL = "SELECT IF(CM_LOCATION_DESC IS NULL OR CM_LOCATION_DESC='','Location',CM_LOCATION_DESC) AS LocDesc," _
                & "IF(CM_SUB_LOCATION_DESC IS NULL OR CM_SUB_LOCATION_DESC='','Sub Location',CM_SUB_LOCATION_DESC) AS SubLocDesc " _
                & "FROM company_misc WHERE CM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                LocDesc = ds1.Tables(0).Rows(0).Item("LocDesc")
                SubLocDesc = ds1.Tables(0).Rows(0).Item("SubLocDesc")
            Else
                LocDesc = "Location"
                SubLocDesc = "Sub Location"
            End If

            ds = Nothing
            ds1 = Nothing
        End Function

        Public Function GetLocationDesc(ByRef LocDesc As String, ByRef SubLocDesc As String)
            Dim strSQL As String
            Dim ds1 As DataSet

            'Get location & sub location description
            'strSQL = "SELECT IF(CM_LOCATION_DESC IS NULL OR CM_LOCATION_DESC='','',CM_LOCATION_DESC) AS LocDesc," _
            '    & "IF(CM_SUB_LOCATION_DESC IS NULL OR CM_SUB_LOCATION_DESC='','',CM_SUB_LOCATION_DESC) AS SubLocDesc " _
            '    & "FROM company_misc WHERE CM_COY_ID='" & Common.Parse(strCoyID) & "'"
            strSQL = "SELECT IFNULL(CM_LOCATION_DESC,'') AS LocDesc,IFNULL(CM_SUB_LOCATION_DESC,'') AS SubLocDesc " _
                    & "FROM company_misc WHERE CM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                LocDesc = ds1.Tables(0).Rows(0).Item("LocDesc")
                SubLocDesc = ds1.Tables(0).Rows(0).Item("SubLocDesc")
            End If

            ds1 = Nothing
        End Function

        Public Function SaveLocationDesc(ByVal strLocDesc As String, ByVal strSubLocDesc As String) As String
            Dim strSQL As String

            strSQL = "SELECT * FROM company_misc WHERE CM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            If objDb.Exist(strSQL) = 0 Then ' record not exists, so insert
                strSQL = "INSERT INTO company_misc (CM_COY_ID,CM_LOCATION_DESC,CM_SUB_LOCATION_DESC,CM_ENT_BY,CM_ENT_DT,CM_MOD_BY,CM_MOD_DT) "
                strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                If Common.Parse(strLocDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strLocDesc) & "',"
                End If
                If Common.Parse(strSubLocDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strSubLocDesc) & "',"
                End If
                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    Return "I"
                End If

            Else
                strSQL = "UPDATE company_misc SET "
                If Common.Parse(strLocDesc) = "" Then
                    strSQL &= "CM_LOCATION_DESC=NULL, "
                Else
                    strSQL &= "CM_LOCATION_DESC='" & Common.Parse(strLocDesc) & "',"
                End If
                If Common.Parse(strSubLocDesc) = "" Then
                    strSQL &= "CM_SUB_LOCATION_DESC=NULL, "
                Else
                    strSQL &= "CM_SUB_LOCATION_DESC='" & Common.Parse(strSubLocDesc) & "',"
                End If
                strSQL &= "CM_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', CM_MOD_DT=" & Common.ConvertDate(Now()) _
                        & " WHERE CM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
                If objDb.Execute(strSQL) Then
                    Return "U"
                End If
            End If
        End Function

        Public Sub GetDefaultLocationDesc(ByVal userID As String, ByVal companyID As String, ByRef LocDesc As String, ByRef SubLocDesc As String)
            Dim strSQL As String

            Dim ds1 As DataSet

            'Get location & sub location description
            'strSQL = "SELECT IF(CM_LOCATION_DESC IS NULL OR CM_LOCATION_DESC='','',CM_LOCATION_DESC) AS LocDesc," _
            '    & "IF(CM_SUB_LOCATION_DESC IS NULL OR CM_SUB_LOCATION_DESC='','',CM_SUB_LOCATION_DESC) AS SubLocDesc " _
            '    & "FROM company_misc WHERE CM_COY_ID='" & Common.Parse(strCoyID) & "'"
            strSQL = "SELECT LM_LOCATION, LM_SUB_LOCATION " _
                    & "FROM user_default, location_mstr " _
                    & "WHERE location_mstr.LM_LOCATION_INDEX = user_default.UD_DEFAULT_VALUE " _
                    & "AND UD_COY_ID='" & Common.Parse(companyID) & "' " _
                    & "AND UD_USER_ID='" & Common.Parse(userID) & "' " _
                    & "AND UD_DEFAULT_TYPE='WH'"

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                LocDesc = Common.parseNull(ds1.Tables(0).Rows(0).Item("LM_LOCATION"))
                SubLocDesc = Common.parseNull(ds1.Tables(0).Rows(0).Item("LM_SUB_LOCATION"))
            End If

            ds1 = Nothing
        End Sub

        Public Function SaveLocation(ByVal strLocDesc As String, ByVal strSubLocDesc As String, ByVal strMode As String) As String
            Dim strSQL As String
            If strSubLocDesc = "" Then
                strSQL = "SELECT * FROM location_mstr WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND LM_LOCATION='" & Common.Parse(strLocDesc) & "' AND (LM_SUB_LOCATION='' OR LM_SUB_LOCATION IS NULL)"

            Else
                strSQL = "SELECT * FROM location_mstr WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND LM_LOCATION='" & Common.Parse(strLocDesc) & "' AND LM_SUB_LOCATION='" & Common.Parse(strSubLocDesc) & "'"

            End If
            If objDb.Exist(strSQL) = 0 Then ' record not exists
                If strMode = "Add" Then
                    strSQL = "INSERT INTO location_mstr (LM_COY_ID,LM_LOCATION,LM_SUB_LOCATION,LM_ENT_BY,LM_ENT_DATETIME,LM_MOD_BY,LM_MOD_DATETIME) "
                    strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                    If Common.Parse(strLocDesc) = "" Then
                        strSQL &= "NULL, "
                    Else
                        strSQL &= "'" & Common.Parse(strLocDesc) & "',"
                    End If
                    If Common.Parse(strSubLocDesc) = "" Then
                        strSQL &= "NULL, "
                    Else
                        strSQL &= "'" & Common.Parse(strSubLocDesc) & "',"
                    End If
                    strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                    If objDb.Execute(strSQL) Then
                        SaveLocation = "I"
                    Else
                        SaveLocation = WheelMsgNum.NotSave
                    End If

                ElseIf strMode = "Update" Then
                    strSQL = "UPDATE location_mstr SET "
                    If Common.Parse(strLocDesc) = "" Then
                        strSQL &= "LM_LOCATION=NULL, "
                    Else
                        strSQL &= "LM_LOCATION='" & Common.Parse(strLocDesc) & "',"
                    End If
                    If Common.Parse(strSubLocDesc) = "" Then
                        strSQL &= "LM_SUB_LOCATION=NULL, "
                    Else
                        strSQL &= "LM_SUB_LOCATION='" & Common.Parse(strSubLocDesc) & "',"
                    End If
                    strSQL &= "LM_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', LM_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                            & "WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
                    If objDb.Execute(strSQL) Then
                        SaveLocation = "U"
                    Else
                        SaveLocation = WheelMsgNum.NotSave
                    End If
                End If
            Else

                SaveLocation = WheelMsgNum.Duplicate '"Exists"
            End If

        End Function

        Public Function getOutstgGRNForQC() As DataSet
            Dim strSql, strSqlPOM, strSqlPOD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPOField As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT iv_grn_no, cm_coy_name, pom_po_no, dom_do_no, gm_created_by, " &
                     "gm_date_received FROM inventory_verify, grn_mstr, company_mstr, po_mstr, do_mstr " &
                     "WHERE gm_b_coy_id = '" & strCoyID & "' AND gm_b_coy_id = cm_coy_id AND " &
                     "gm_po_index = pom_po_index AND gm_do_index = dom_do_index AND " &
                     "iv_verify_status = 'P'"
            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function ChkDupLotNo(ByVal ds As DataSet) As DataSet
            Dim strSql, strPOLine As String
            Dim aryLotNo As New ArrayList
            Dim i, j As Integer
            Dim blnLot As Boolean

            'Store Duplicated Lot No 
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i)(2) = "N" Then
                    blnLot = False
                    For j = 0 To ds.Tables(0).Rows.Count - 1
                        If ds.Tables(0).Rows(j)(2) = "N" Then
                            If ds.Tables(0).Rows(i)(0) = ds.Tables(0).Rows(j)(0) Then
                                ds.Tables(0).Rows(j)(2) = "Y"
                            Else
                                If ds.Tables(0).Rows(i)(1) = ds.Tables(0).Rows(j)(1) Then
                                    ds.Tables(0).Rows(j)(2) = "Y"
                                    blnLot = True
                                End If
                            End If
                        End If
                    Next

                    If blnLot = True Then
                        aryLotNo.Add(ds.Tables(0).Rows(i)(1))
                    End If
                End If
            Next

            For i = 0 To aryLotNo.Count - 1
                For j = 0 To ds.Tables(0).Rows.Count - 1
                    If aryLotNo(i) = ds.Tables(0).Rows(j)(1) Then
                        strSql = "SELECT GL_PO_LINE FROM GRN_LOT WHERE GL_LOT_INDEX = '" & ds.Tables(0).Rows(j)(0) & "' LIMIT 1"
                        strPOLine = objDb.GetVal(strSql)
                        ds.Tables(0).Rows(j)(1) = ds.Tables(0).Rows(j)(1) & " - Line " & strPOLine
                    End If
                Next
            Next

            Return ds
        End Function

        Public Function PopLotNo(ByVal strItemCode As String) As DataSet
            Dim strSql As String
            Dim dsLot As DataSet
            Dim strCompId As String = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT DISTINCT IL_LOT_INDEX, " &
                    "CONCAT(DOL_LOT_NO, ' (', (SELECT GL_GRN_NO FROM GRN_LOT WHERE GL_LOT_INDEX = IL_LOT_INDEX LIMIT 1), ')') AS DOL_LOT_NO, 'N' AS DUP " &
                    "FROM INVENTORY_LOT " &
                    "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IL_INVENTORY_INDEX " &
                    "INNER JOIN DO_LOT DOL ON IL_LOT_INDEX =  DOL_LOT_INDEX " &
                    "WHERE (IL_LOT_QTY - IFNULL(IL_IQC_QTY, 0)) > 0 " &
                    "AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND IM_COY_ID = '" & strCompId & "' " &
                    "AND DOL_COY_ID <> 'SYSTEM' " &
                    "UNION " &
                    "SELECT DISTINCT IL_LOT_INDEX, DOL_LOT_NO, " &
                    "'N' AS DUP " &
                    "FROM INVENTORY_LOT " &
                    "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IL_INVENTORY_INDEX " &
                    "INNER JOIN DO_LOT DOL ON IL_LOT_INDEX =  DOL_LOT_INDEX " &
                    "WHERE(IL_LOT_QTY - IFNULL(IL_IQC_QTY, 0)) > 0 " &
                    "AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND IM_COY_ID = '" & strCompId & "' " &
                    "AND DOL_COY_ID = 'SYSTEM' " &
                    "ORDER BY DOL_LOT_NO "

            dsLot = objDb.FillDs(strSql)
            PopLotNo = dsLot
        End Function

        Public Function PopLocByLot(ByVal strLotIndex As String, ByVal strItemCode As String, Optional ByVal strLoc As String = "") As DataSet
            Dim strSql As String
            Dim dsLoc As DataSet

            If strLoc = "" Then
                strSql = " SELECT DISTINCT LM_LOCATION FROM INVENTORY_LOT " &
                        "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IL_INVENTORY_INDEX " &
                        "INNER JOIN LOCATION_MSTR ON IL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                        "WHERE (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0 AND IL_LOT_INDEX = '" & strLotIndex & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                        "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                        "ORDER BY LM_LOCATION_INDEX "
            Else
                strSql = " SELECT DISTINCT LM_SUB_LOCATION FROM INVENTORY_LOT " &
                        "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = IL_INVENTORY_INDEX " &
                        "INNER JOIN LOCATION_MSTR ON IL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                        "WHERE (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0 AND IL_LOT_INDEX = '" & strLotIndex & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                        "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                        "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' " &
                        "ORDER BY LM_LOCATION_INDEX "
            End If


            dsLoc = objDb.FillDs(strSql)
            PopLocByLot = dsLoc
        End Function

        Public Function PopulateLocation() As DataSet
            Dim strSql As String
            Dim dsLocation As DataSet

            strSql = "SELECT LM_LOCATION_INDEX,LM_LOCATION,LM_SUB_LOCATION " _
                    & "FROM location_mstr WHERE LM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                    & "ORDER BY LM_LOCATION_INDEX"

            dsLocation = objDb.FillDs(strSql)
            PopulateLocation = dsLocation

        End Function

        Public Function PopLocation() As DataSet
            'Yap 2011 Inv
            Dim strSql As String
            Dim dsLocation As DataSet

            strSql = "SELECT DISTINCT LM_LOCATION " _
                    & "FROM LOCATION_MSTR WHERE LM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                    & "ORDER BY LM_LOCATION "

            dsLocation = objDb.FillDs(strSql)
            PopLocation = dsLocation

        End Function

        Public Function PopLocationWItem(ByVal strItem As String) As DataSet
            'Yap 2011 Inv
            Dim strSql As String
            Dim dsLocation As DataSet

            strSql = " SELECT DISTINCT LM_LOCATION FROM INVENTORY_DETAIL, INVENTORY_MSTR, LOCATION_MSTR " _
                   & " WHERE  ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND " _
                   & " ID_LOCATION_INDEX = LM_LOCATION_INDEX AND " _
                   & " IM_ITEM_CODE = '" & strItem & "' AND " _
                   & " IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                   & " ORDER BY LM_LOCATION_INDEX "

            dsLocation = objDb.FillDs(strSql)
            PopLocationWItem = dsLocation

        End Function

        Public Function PopLocation(ByVal strLoc As String) As DataSet
            'Yap 2011 Inv
            Dim strSql As String
            Dim dsLocation As DataSet

            strSql = "SELECT DISTINCT IFNULL(LM_SUB_LOCATION,'') AS LM_SUB_LOCATION, LM_LOCATION_INDEX " _
                    & "FROM LOCATION_MSTR WHERE LM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                    & "AND LM_LOCATION='" & strLoc & "' " _
                    & "ORDER BY LM_SUB_LOCATION"

            dsLocation = objDb.FillDs(strSql)
            PopLocation = dsLocation

        End Function

        Public Function PopLocationWItem(ByVal strItem As String, ByVal strLoc As String) As DataSet
            'Yap 2011 Inv
            Dim strSql As String
            Dim dsLocation As DataSet

            strSql = " SELECT DISTINCT DISTINCT LM_SUB_LOCATION FROM INVENTORY_DETAIL, INVENTORY_MSTR, LOCATION_MSTR " _
                   & " WHERE  ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND " _
                   & " ID_LOCATION_INDEX = LM_LOCATION_INDEX AND " _
                   & " IM_ITEM_CODE = '" & strItem & "' AND " _
                   & " LM_LOCATION='" & strLoc & "' AND " _
                   & " IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                   & " ORDER BY LM_LOCATION_INDEX "

            dsLocation = objDb.FillDs(strSql)
            PopLocationWItem = dsLocation

        End Function

        Public Function searchInvItem(ByVal sortby As String) As DataSet
            Dim strsql As String
            Dim dss As DataSet
            'Yap 2011 Inv
            strsql &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME " &
                      " FROM INVENTORY_MSTR " &
                      " where IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' ORDER BY " & sortby & " "

            dss = objDb.FillDs(strsql)
            searchInvItem = dss
        End Function

        Public Function searchInvItemWO(ByVal sortby As String) As DataSet
            Dim strsql As String
            Dim dss As DataSet
            'Yap 2011 Inv
            'strsql &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME " & _
            '          " FROM INVENTORY_MSTR " & _
            '          " where IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' ORDER BY " & sortby & " "

            strsql &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME " &
                      " FROM INVENTORY_MSTR, INVENTORY_DETAIL, INVENTORY_LOT, LOCATION_MSTR, DO_LOT " &
                      " WHERE IM_INVENTORY_INDEX = ID_INVENTORY_INDEX And ID_LOCATION_INDEX = LM_LOCATION_INDEX " &
                      " AND IL_INVENTORY_INDEX = ID_INVENTORY_INDEX AND IL_LOCATION_INDEX = ID_LOCATION_INDEX " &
                      " AND DOL_LOT_INDEX = IL_LOT_INDEX " &
                      " AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0 GROUP BY IM_ITEM_CODE "

            dss = objDb.FillDs(strsql)
            searchInvItemWO = dss
        End Function

        Public Function searchInvItem2(ByVal sortby As String) As DataSet
            Dim strsql As String
            Dim ds As DataSet
            'Chee Hong 2013 Inv
            'strsql = " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME FROM " & _
            '        " (SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, SUM(IFNULL(ID_INVENTORY_QTY,0)) AS ID_INVENTORY_QTY, SUM(IFNULL(ID_IQC_QTY,0)) AS ID_IQC_QTY " & _
            '        " FROM INVENTORY_DETAIL ID, INVENTORY_MSTR IM, PRODUCT_MSTR PM  WHERE IM.IM_INVENTORY_INDEX = ID.ID_INVENTORY_INDEX " & _
            '        " AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
            '        " AND PM_ITEM_TYPE = 'ST' AND IM.IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' GROUP BY IM_INVENTORY_INDEX) tb " & _
            '        " WHERE (ID_INVENTORY_QTY - ID_IQC_QTY) > 0 ORDER BY " & sortby & " "

            strsql = "SELECT IM_ITEM_CODE, IM_INVENTORY_NAME FROM " &
                    "(SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, SUM(IFNULL(ID_INVENTORY_QTY,0)) AS ID_INVENTORY_QTY, SUM(IFNULL(ID_IQC_QTY,0)) AS ID_IQC_QTY " &
                    "FROM INVENTORY_DETAIL ID, INVENTORY_MSTR IM, PRODUCT_MSTR PM  WHERE IM.IM_INVENTORY_INDEX = ID.ID_INVENTORY_INDEX " &
                    "AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE " &
                    "AND PM_ITEM_TYPE = 'ST' AND IM.IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND (IFNULL(ID_INVENTORY_QTY,0) - IFNULL(ID_IQC_QTY,0)) > 0 " &
                    "GROUP BY IM_INVENTORY_INDEX) tb " &
                    "WHERE (ID_INVENTORY_QTY - ID_IQC_QTY) > 0 ORDER BY " & sortby & " "

            ds = objDb.FillDs(strsql)
            searchInvItem2 = ds
        End Function

        Function getInventoryItemFiltered(ByVal ItemCode As String, ByVal ItemName As String, ByVal Location As String, ByVal SubLocation As String) As Object
            Dim strSQL As String
            Dim ds As DataSet
            'Yap 2011 Inv
            strSQL &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, LM_LOCATION, LM_SUB_LOCATION, ID_INVENTORY_QTY FROM INVENTORY_MSTR, INVENTORY_DETAIL, LOCATION_MSTR " &
                      " WHERE IM_INVENTORY_INDEX = ID_INVENTORY_INDEX And ID_LOCATION_INDEX = LM_LOCATION_INDEX " &
                      " AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If ItemCode <> "" Then
                strSQL &= "AND IM_ITEM_CODE ='" & ItemCode & "' "
            End If
            If ItemName <> "" Then
                strSQL &= "AND IM_INVENTORY_NAME LIKE '%" & Common.Parse(ItemName) & "%' "
            End If
            If Location <> "" Then
                strSQL &= "AND LM_LOCATION ='" & Common.Parse(Location) & "' "
            End If
            If SubLocation <> "" Then
                strSQL &= "AND LM_SUB_LOCATION ='" & Common.Parse(SubLocation) & "' "
            End If

            ds = objDb.FillDs(strSQL)
            getInventoryItemFiltered = ds
        End Function

        Function getInventoryItemFilteredWO(ByVal ItemCode As String, ByVal ItemName As String, ByVal Location As String, ByVal SubLocation As String) As Object
            Dim strSQL As String
            Dim ds As DataSet
            'Yap 2011 Inv
            strSQL &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, DOL_LOT_NO, LM_LOCATION, LM_SUB_LOCATION, SUM(IL_LOT_QTY-IFNULL(IL_IQC_QTY,0)) AS ID_INVENTORY_QTY " &
                      " FROM INVENTORY_MSTR, INVENTORY_DETAIL, INVENTORY_LOT, LOCATION_MSTR, DO_LOT " &
                      " WHERE IM_INVENTORY_INDEX = ID_INVENTORY_INDEX And ID_LOCATION_INDEX = LM_LOCATION_INDEX " &
                      " AND IL_INVENTORY_INDEX = ID_INVENTORY_INDEX AND IL_LOCATION_INDEX = ID_LOCATION_INDEX " &
                      " AND DOL_LOT_INDEX = IL_LOT_INDEX " &
                      " AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND (IL_LOT_QTY-IFNULL(IL_IQC_QTY,0)) > 0 "

            If ItemCode <> "" Then
                strSQL &= "AND IM_ITEM_CODE ='" & ItemCode & "' "
            End If
            If ItemName <> "" Then
                strSQL &= "AND IM_INVENTORY_NAME LIKE '%" & Common.Parse(ItemName) & "%' "
            End If
            If Location <> "" Then
                strSQL &= "AND LM_LOCATION ='" & Common.Parse(Location) & "' "
            End If
            If SubLocation <> "" Then
                strSQL &= "AND LM_SUB_LOCATION ='" & Common.Parse(SubLocation) & "' "
            End If

            strSQL &= " GROUP BY IM_ITEM_CODE, IM_INVENTORY_NAME, DOL_LOT_NO, LM_LOCATION, LM_SUB_LOCATION "

            ds = objDb.FillDs(strSQL)
            getInventoryItemFilteredWO = ds
        End Function

        Function getInventoryItemFiltered2(ByVal ItemCode As String, ByVal ItemName As String, ByVal Location As String, ByVal SubLocation As String) As Object
            Dim strSQL As String
            Dim ds As DataSet

            strSQL &= "SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, PM_UOM, '' AS LM_LOCATION, '' AS LM_SUB_LOCATION, SUM(ID_INVENTORY_QTY) AS ID_INVENTORY_QTY " &
                    "FROM INVENTORY_MSTR, INVENTORY_DETAIL, LOCATION_MSTR, PRODUCT_MSTR " &
                    "WHERE IM_INVENTORY_INDEX = ID_INVENTORY_INDEX And ID_LOCATION_INDEX = LM_LOCATION_INDEX " &
                    "AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " &
                    "AND PM_ITEM_TYPE = 'ST' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND (ID_INVENTORY_QTY - IFNULL(ID_IQC_QTY,0)) > 0 "

            If ItemCode <> "" Then
                strSQL &= "AND IM_ITEM_CODE ='" & ItemCode & "' "
            End If
            If ItemName <> "" Then
                strSQL &= "AND IM_INVENTORY_NAME LIKE '%" & Common.Parse(ItemName) & "%' "
            End If
            If Location <> "" Then
                strSQL &= "AND LM_LOCATION ='" & Common.Parse(Location) & "' "
            End If
            If SubLocation <> "" Then
                strSQL &= "AND LM_SUB_LOCATION ='" & Common.Parse(SubLocation) & "' "
            End If

            strSQL &= " GROUP BY IM_INVENTORY_INDEX "

            ds = objDb.FillDs(strSQL)
            getInventoryItemFiltered2 = ds
        End Function

        Function getInventoryItemInfoFiltered(ByVal ItemInfo As String, Optional ByVal blnEnterprise As Boolean = False) As Object
            Dim strSQL As String
            Dim ds As DataSet
            'Yap 2011 Inv
            'strSQL &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, IT_TRANS_QTY, LM_LOCATION, LM_SUB_LOCATION " & _
            '          " FROM INVENTORY_TRANS, INVENTORY_MSTR, LOCATION_MSTR " & _
            '          " WHERE IT_INVENTORY_INDEX = IM_INVENTORY_INDEX And IT_FRM_LOCATION_INDEX = LM_LOCATION_INDEX " & _
            '          " AND IT_TRANS_REF_NO = '" & ItemInfo & "' AND IT_TRANS_TYPE = 'IR' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
            'Modified by Joon on 1 June 2011 - change IM_INVENTORY_NAME to IT_INVENTORY_NAME

            If blnEnterprise = True Then 'Chee Hong 07/06/2013 - eMRS enhancement
                strSQL = "SELECT IM_ITEM_CODE, IRD_INVENTORY_NAME, IRD_UOM, IRD_QTY, IRD_IR_MTHISSUE, IRD_IR_LAST3MTH, LM_LOCATION, LM_SUB_LOCATION " &
                        "FROM INVENTORY_REQUISITION_DETAILS " &
                        "INNER JOIN INVENTORY_MSTR ON IRD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                        "LEFT JOIN INVENTORY_TRANS ON IRD_INVENTORY_INDEX = IT_INVENTORY_INDEX AND IRD_IR_NO = IT_TRANS_REF_NO AND IT_TRANS_TYPE = 'IR' " &
                        "LEFT JOIN LOCATION_MSTR ON IT_FRM_LOCATION_INDEX = LM_LOCATION_INDEX " &
                        "WHERE IRD_IR_NO = '" & ItemInfo & "' AND IRD_IR_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            Else
                strSQL = " SELECT IM_ITEM_CODE, IT_INVENTORY_NAME, IT_TRANS_QTY, LM_LOCATION, LM_SUB_LOCATION " &
                     " FROM INVENTORY_TRANS, INVENTORY_MSTR, LOCATION_MSTR " &
                     " WHERE IT_INVENTORY_INDEX = IM_INVENTORY_INDEX And IT_FRM_LOCATION_INDEX = LM_LOCATION_INDEX " &
                     " AND IT_TRANS_REF_NO = '" & ItemInfo & "' AND IT_TRANS_TYPE = 'IR' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            End If


            ds = objDb.FillDs(strSQL)
            getInventoryItemInfoFiltered = ds
        End Function

        Function getInventoryTransferItemInfoFiltered(ByVal ItemInfo As String) As Object
            Dim strSQL As String
            Dim ds As DataSet
            'Yap 2011 Inv
            'strSQL &= " SELECT IM_ITEM_CODE, IM_INVENTORY_NAME, IT_TRANS_QTY, " & _
            '          " A.LM_LOCATION AS LM_LOCATION, " & _
            '          " A.LM_SUB_LOCATION AS LM_SUB_LOCATION, " & _
            '          " B.LM_LOCATION AS LM_LOCATION2, " & _
            '          " B.LM_SUB_LOCATION AS LM_SUB_LOCATION2 " & _
            '          " FROM INVENTORY_TRANS, INVENTORY_MSTR, LOCATION_MSTR A, LOCATION_MSTR B " & _
            '          " WHERE IT_INVENTORY_INDEX = IM_INVENTORY_INDEX And IT_FRM_LOCATION_INDEX = A.LM_LOCATION_INDEX And IT_TO_LOCATION_INDEX = B.LM_LOCATION_INDEX " & _
            '          " AND IT_TRANS_REF_NO = '" & ItemInfo & "' AND IT_TRANS_TYPE = 'TR' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
            'Modified by Joon on 1 June 2011 - change IM_INVENTORY_NAME to IT_INVENTORY_NAME
            strSQL &= " SELECT IM_ITEM_CODE, IT_INVENTORY_NAME, IT_TRANS_QTY, " &
                      " A.LM_LOCATION AS LM_LOCATION, " &
                      " A.LM_SUB_LOCATION AS LM_SUB_LOCATION, " &
                      " B.LM_LOCATION AS LM_LOCATION2, " &
                      " B.LM_SUB_LOCATION AS LM_SUB_LOCATION2 " &
                      " FROM INVENTORY_TRANS, INVENTORY_MSTR, LOCATION_MSTR A, LOCATION_MSTR B " &
                      " WHERE IT_INVENTORY_INDEX = IM_INVENTORY_INDEX And IT_FRM_LOCATION_INDEX = A.LM_LOCATION_INDEX And IT_TO_LOCATION_INDEX = B.LM_LOCATION_INDEX " &
                      " AND IT_TRANS_REF_NO = '" & ItemInfo & "' AND IT_TRANS_TYPE = 'TR' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            ds = objDb.FillDs(strSQL)
            getInventoryTransferItemInfoFiltered = ds
        End Function

        Function getInventoryTransFiltered(ByVal IRNo As String, ByVal Issue As String, ByVal DateS As String, ByVal DateE As String, ByVal Dept As String) As Object
            Dim strSQL As String
            Dim ds As DataSet
            Dim strTemp As String

            'Yap 2011 Inv
            strSQL &= " SELECT IT_TRANS_REF_NO, IT_TRANS_DATE AS IT_TRANS_DATE, " &
                      " IT_ADDITION_INFO AS IT_ADDITION_INFO, IT_ADDITION_INFO1 AS IT_ADDITION_INFO1, " &
                      " IT_REF_NO AS IT_REF_NO, IT_REMARK AS IT_REMARK FROM INVENTORY_TRANS, INVENTORY_MSTR " &
                      " WHERE IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                      " AND IT_TRANS_TYPE = 'IR' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If IRNo <> "" Then
                strTemp = Common.BuildWildCard(IRNo)
                strSQL &= "AND IT_TRANS_REF_NO" & Common.ParseSQL(strTemp)
            End If

            If Issue <> "" Then
                strTemp = Common.BuildWildCard(Issue)
                strSQL &= "AND IT_ADDITION_INFO" & Common.ParseSQL(strTemp)
            End If

            If Dept <> "" Then
                strTemp = Common.BuildWildCard(Dept)
                strSQL &= "AND IT_ADDITION_INFO1" & Common.ParseSQL(strTemp)
            End If

            If DateS <> "" Then
                strSQL &= "AND IT_TRANS_DATE >= " & Common.ConvertDate(DateS) & " "
            End If

            If DateE <> "" Then
                strSQL &= "AND IT_TRANS_DATE <= " & Common.ConvertDate(DateE & " 23:59:59.000") & " "
            End If

            strSQL &= "GROUP BY IT_TRANS_REF_NO "

            ds = objDb.FillDs(strSQL)
            getInventoryTransFiltered = ds
        End Function

        Function getInventoryTransFiltered(ByVal ITNo As String, ByVal DateS As String, ByVal DateE As String) As Object
            Dim strSQL As String
            Dim ds As DataSet
            Dim strTemp As String

            'Yap 2011 Inv
            strSQL &= " SELECT IT_TRANS_REF_NO, IT_TRANS_DATE AS IT_TRANS_DATE, IT_REF_NO AS IT_REF_NO, IT_REMARK AS IT_REMARK FROM INVENTORY_TRANS, INVENTORY_MSTR " &
                      " WHERE IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                      " AND IT_TRANS_TYPE = 'TR' AND IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "


            If ITNo <> "" Then
                strTemp = Common.BuildWildCard(ITNo)
                strSQL &= "AND IT_TRANS_REF_NO" & Common.ParseSQL(strTemp)
            End If

            If DateS <> "" Then
                strSQL &= "AND IT_TRANS_DATE >= " & Common.ConvertDate(DateS) & " "
            End If

            If DateE <> "" Then
                strSQL &= "AND IT_TRANS_DATE <= " & Common.ConvertDate(DateE & " 23:59:59.000") & " "
            End If

            strSQL &= " GROUP BY IT_TRANS_REF_NO "

            ds = objDb.FillDs(strSQL)
            getInventoryTransFiltered = ds
        End Function

        'Public Function insertIR(ByVal aryIRItem As ArrayList, ByRef strIRNo As String, ByRef IssueTo As String, ByRef RefNo As String, ByRef Remark As String, ByRef Code As String, ByRef Dept As String, ByRef RetLoc As String, ByRef RetSLoc As String) As Integer
        '    'Yap 2011 Inv
        '    Dim strPrefix
        '    Dim SqlQuery, PM_PRODUCT_INDEX, PM_LOC_INDEX, ID_INVENTORY_QTY As String
        '    Dim strAryQuery(0) As String
        '    Dim i As Integer
        '    Dim ItemName As String

        '    Dim aryItem As New ArrayList()
        '    aryItem = aryIRItem

        '    'GetLatestDocNo
        '    objGlobal.GetLatestDocNo("IR", strAryQuery, strIRNo, strPrefix)

        '    ' Group All Duplicate
        '    Dim tempItem, tempLoc, tempSLoc As String
        '    Dim tempQty As Integer = 0
        '    Dim j As Integer
        '    For i = 0 To aryItem.Count - 1
        '        tempItem = aryItem(i)(0)
        '        tempLoc = aryItem(i)(3)
        '        tempSLoc = aryItem(i)(4)
        '        For j = 0 To aryItem.Count - 1
        '            If aryItem(j)(5) <> "Done" And tempItem = aryItem(j)(0) And tempLoc = aryItem(j)(3) And tempSLoc = aryItem(j)(4) Then
        '                aryItem(j)(0) = ""
        '                aryItem(j)(5) = "Done"
        '                tempQty = tempQty + IIf(aryItem(j)(2) = "", 0, aryItem(j)(2))
        '            End If
        '        Next
        '        aryItem.Add(New String() {tempItem, "NAME", tempQty, tempLoc, tempSLoc, "Done"})
        '        tempQty = 0
        '    Next

        '    For i = 0 To aryItem.Count - 1
        '        If aryItem(i)(0) <> "---Select---" And aryItem(i)(0) <> "" And aryItem(i)(2) <> "0" Then
        '            Code = aryItem(i)(0)
        '            SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " & _
        '                           " WHERE  IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
        '            PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

        '            If Common.parseNull(aryItem(i)(4)) = "" Then
        '                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' "
        '                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
        '            Else
        '                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(4) & "' "
        '                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
        '            End If

        '            SqlQuery = " SELECT ID_INVENTORY_QTY FROM INVENTORY_DETAIL " & _
        '                       " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
        '            ID_INVENTORY_QTY = objDb.GetVal(SqlQuery)

        '            If CInt(aryItem(i)(2)) > CInt(IIf(ID_INVENTORY_QTY = "", 0, ID_INVENTORY_QTY)) Then
        '                insertIR = 10
        '                RetLoc = aryItem(i)(3)
        '                RetSLoc = aryItem(i)(4)
        '                Exit Function
        '            End If

        '            SqlQuery = " UPDATE INVENTORY_DETAIL " & _
        '                       " SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & aryItem(i)(2) & " " & _
        '                       " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
        '            Common.Insert2Ary(strAryQuery, SqlQuery)

        '            'Modified by Joon on 1 June 2011 - insert IM_INVENTORY_NAME into INVENTORY_TRANS table (IT_INVENTORY_NAME column)
        '            SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " & _
        '                          "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
        '            ItemName = objDb.GetVal(SqlQuery)

        '            SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_ADDITION_INFO, IT_ADDITION_INFO1, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
        '                       " VALUES ('" & strIRNo & "', " & _
        '                       " '" & PM_PRODUCT_INDEX & "', " & _
        '                       " 'IR', " & aryItem(i)(2) & ", " & _
        '                       " " & Common.ConvertDate(Now) & ", " & _
        '                       " '" & PM_LOC_INDEX & "', " & _
        '                       " '" & Common.Parse(RefNo) & "', '" & Common.Parse(Remark) & "', " & _
        '                       " '" & IssueTo & "', '" & Dept & "', '" & HttpContext.Current.Session("UserId") & "', " & _
        '                       " '" & Common.Parse(ItemName) & "') "
        '            Common.Insert2Ary(strAryQuery, SqlQuery)
        '        End If
        '    Next
        '    If objDb.BatchExecute(strAryQuery) Then
        '        insertIR = WheelMsgNum.Save
        '    Else
        '        insertIR = WheelMsgNum.NotSave
        '    End If
        'End Function

        Public Function insertIR(ByVal strCoyId As String, ByVal strUserName As String, ByVal strUserId As String, ByVal aryIRItem As ArrayList, ByRef strIRNo As String, ByRef IssueTo As String, ByRef RefNo As String, ByRef Remark As String, ByRef Code As String, ByRef Dept As String, ByRef RetLoc As String, ByRef RetSLoc As String, Optional ByVal blnEnterprise As Boolean = False, Optional ByVal blnStkLoc As String = "Y", Optional ByVal ReqName As String = "", Optional ByVal SecCode As String = "", Optional ByVal ChkUrgent As String = "N", Optional ByVal AppGrpIndex As String = "") As Integer
            'Yap 2011 Inv
            Dim strPrefix
            Dim SqlQuery, PM_PRODUCT_INDEX, PM_LOC_INDEX, ID_INVENTORY_QTY, IRM_IR_INDEX, strMgr As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            Dim IR_LINE As Integer = 1
            Dim ItemName, Uom As String
            Dim intIncrementNo As Integer = 0
            Dim strNewIRNo As String = ""
            Dim strStoreMgr() As String
            Dim dsApp As New DataSet
            Dim aryItem As New ArrayList()
            Dim dblMonthIssue As Decimal
            aryItem = aryIRItem

            'GetLatestDocNo
            'objGlobal.GetLatestDocNo("IR", strAryQuery, strIRNo, strPrefix)
            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'IR' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strIRNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                   & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                   & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'IR' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            ' Group All Duplicate
            Dim tempItem, tempLoc, tempSLoc As String
            Dim tempQty As Decimal = 0
            Dim j As Integer
            For i = 0 To aryItem.Count - 1
                tempItem = aryItem(i)(0)
                tempLoc = aryItem(i)(3)
                tempSLoc = aryItem(i)(4)
                For j = 0 To aryItem.Count - 1
                    If aryItem(j)(5) <> "Done" And tempItem = aryItem(j)(0) And tempLoc = aryItem(j)(3) And tempSLoc = aryItem(j)(4) Then
                        aryItem(j)(0) = ""
                        aryItem(j)(5) = "Done"
                        tempQty = tempQty + CDec(IIf(aryItem(j)(2) = "", 0, aryItem(j)(2)))
                    End If
                Next
                aryItem.Add(New String() {tempItem, "NAME", tempQty, tempLoc, tempSLoc, "Done"})
                tempQty = 0
            Next

            If blnEnterprise = True Then
                'Store record into inventory_requisition_mstr
                SqlQuery = "INSERT INTO INVENTORY_REQUISITION_MSTR(IRM_IR_COY_ID, IRM_IR_NO, IRM_IR_DATE, IRM_IR_REF_NO, IRM_IR_REQUESTOR_NAME, IRM_IR_ISSUE_TO, IRM_IR_DEPARTMENT, IRM_IR_SECTION, IRM_IR_REMARK, IRM_IR_STATUS, IRM_STATUS_CHANGED_BY, IRM_STATUS_CHANGED_ON, IRM_IR_URGENT, IRM_CREATED_BY, IRM_CREATED_DATE) " &
                            "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', " &
                            "" & strIRNo & ", " &
                            Common.ConvertDate(Now) & ", " &
                            "'" & Common.Parse(RefNo) & "', " &
                            "'" & Common.Parse(ReqName) & "', " &
                            "'" & IssueTo & "', " &
                            "'" & Dept & "', " &
                            "'" & Common.Parse(SecCode) & "', " &
                            "'" & Common.Parse(Remark) & "', " &
                            "'1', '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', " &
                            Common.ConvertDate(Now) & ", " &
                            "'" & ChkUrgent & "', " &
                            "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', " &
                            Common.ConvertDate(Now) & ") "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                For i = 0 To aryItem.Count - 1
                    If aryItem(i)(0) <> "---Select---" And aryItem(i)(0) <> "" And aryItem(i)(2) <> "0" Then
                        Code = aryItem(i)(0)
                        SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                       " WHERE  IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
                        PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                        SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " &
                                      "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
                        ItemName = objDb.GetVal(SqlQuery)

                        Uom = getInvItemUom(aryItem(i)(0))

                        If blnStkLoc = "Y" Then
                            If Common.parseNull(aryItem(i)(4)) = "" Then
                                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' AND LM_SUB_LOCATION IS NULL "
                                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                            Else
                                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(4) & "' "
                                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                            End If

                            SqlQuery = " SELECT ID_INVENTORY_QTY FROM INVENTORY_DETAIL " &
                               " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
                            ID_INVENTORY_QTY = objDb.GetVal(SqlQuery)

                            If CDec(aryItem(i)(2)) > CDec(IIf(ID_INVENTORY_QTY = "", 0, ID_INVENTORY_QTY)) Then
                                insertIR = 10
                                RetLoc = aryItem(i)(3)
                                RetSLoc = aryItem(i)(4)
                                Exit Function
                            End If

                            'SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_ADDITION_INFO, IT_ADDITION_INFO1, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
                            '           " VALUES (" & strIRNo & ", " & _
                            '           " '" & PM_PRODUCT_INDEX & "', " & _
                            '           " 'IR', " & aryItem(i)(2) & ", " & _
                            '           " " & Common.ConvertDate(Now) & ", " & _
                            '           " '" & PM_LOC_INDEX & "', " & _
                            '           " '" & Common.Parse(RefNo) & "', '" & Common.Parse(Remark) & "', " & _
                            '           " '" & IssueTo & "', '" & Dept & "', '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', " & _
                            '           " '" & Common.Parse(ItemName) & "') "
                            'Common.Insert2Ary(strAryQuery, SqlQuery)
                        Else
                            'SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_REF_NO, IT_REMARK, IT_ADDITION_INFO, IT_ADDITION_INFO1, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
                            '           " VALUES (" & strIRNo & ", " & _
                            '           " '" & PM_PRODUCT_INDEX & "', " & _
                            '           " 'IR', " & aryItem(i)(2) & ", " & _
                            '           " " & Common.ConvertDate(Now) & ", " & _
                            '           " '" & Common.Parse(RefNo) & "', '" & Common.Parse(Remark) & "', " & _
                            '           " '" & IssueTo & "', '" & Dept & "', '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', " & _
                            '           " '" & Common.Parse(ItemName) & "') "
                            'Common.Insert2Ary(strAryQuery, SqlQuery)
                        End If

                        'Get Month Issue Qty
                        dblMonthIssue = 0
                        dblMonthIssue = getMonthStockBalance(aryItem(i)(0), HttpContext.Current.Session("UserId"))

                        'Store record into inventory_requisition_detail
                        SqlQuery = "INSERT INTO INVENTORY_REQUISITION_DETAILS(IRD_IR_COY_ID, IRD_IR_NO, IRD_IR_LINE, IRD_INVENTORY_INDEX, IRD_INVENTORY_NAME, IRD_QTY, IRD_IR_MTHISSUE, IRD_IR_BEFOREAPP_MTHISSUE, IRD_UOM) " &
                                    "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', " &
                                    "" & strIRNo & ", " &
                                    "" & IR_LINE & ", " &
                                    "'" & PM_PRODUCT_INDEX & "', " &
                                    "'" & Common.Parse(ItemName) & "', " &
                                    aryItem(i)(2) & ", " &
                                    dblMonthIssue & ", " &
                                    dblMonthIssue & ", '" & Uom & "') "
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                        IR_LINE = IR_LINE + 1

                    End If
                Next

                IRM_IR_INDEX = " (SELECT MAX(IRM_IR_INDEX) AS IRM_IR_INDEX FROM INVENTORY_REQUISITION_MSTR) "

                'Store record into ir_approval
                SqlQuery = "INSERT INTO IR_APPROVAL(IRA_IR_INDEX, IRA_AO, IRA_A_AO, IRA_SEQ, IRA_AO_ACTION, IRA_APPROVAL_TYPE, IRA_APPROVAL_GRP_INDEX, IRA_RELIEF_IND) " &
                            "SELECT " & IRM_IR_INDEX & ", AGA_AO, AGA_A_AO, AGA_SEQ, 0, 1, AGA_GRP_INDEX, 'O' FROM APPROVAL_GRP_AO WHERE AGA_GRP_INDEX='" & AppGrpIndex & "'"
                Common.Insert2Ary(strAryQuery, SqlQuery)

            Else
                'FTN Version
                For i = 0 To aryItem.Count - 1
                    If aryItem(i)(0) <> "---Select---" And aryItem(i)(0) <> "" And aryItem(i)(2) <> "0" Then
                        Code = aryItem(i)(0)
                        SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                       " WHERE  IM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
                        PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                        If Common.parseNull(aryItem(i)(4)) = "" Then
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        Else
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(4) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        End If

                        SqlQuery = " SELECT ID_INVENTORY_QTY FROM INVENTORY_DETAIL " &
                               " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
                        ID_INVENTORY_QTY = objDb.GetVal(SqlQuery)

                        If CDec(aryItem(i)(2)) > CDec(IIf(ID_INVENTORY_QTY = "", 0, ID_INVENTORY_QTY)) Then
                            insertIR = 10
                            RetLoc = aryItem(i)(3)
                            RetSLoc = aryItem(i)(4)
                            Exit Function
                        End If

                        SqlQuery = " UPDATE INVENTORY_DETAIL " &
                              " SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & aryItem(i)(2) & " " &
                              " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                        'Modified by Joon on 1 June 2011 - insert IM_INVENTORY_NAME into INVENTORY_TRANS table (IT_INVENTORY_NAME column)
                        SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " &
                                      "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
                        ItemName = objDb.GetVal(SqlQuery)

                        SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_ADDITION_INFO, IT_ADDITION_INFO1, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                   " VALUES (" & strIRNo & ", " &
                                   " '" & PM_PRODUCT_INDEX & "', " &
                                   " 'IR', " & aryItem(i)(2) & ", " &
                                   " " & Common.ConvertDate(Now) & ", " &
                                   " '" & PM_LOC_INDEX & "', " &
                                   " '" & Common.Parse(RefNo) & "', '" & Common.Parse(Remark) & "', " &
                                   " '" & IssueTo & "', '" & Dept & "', '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', " &
                                   " '" & Common.Parse(ItemName) & "') "
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If
                Next
            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strCoyId, strUserId, strAryQuery, WheelModule.IRMod, WheelUserActivity.REQ_SubmitIR, strIRNo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strIRNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'IR' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewIRNo, "T_NO") Then 'objDb.BatchExecute(strAryQuery) Then
                strIRNo = strNewIRNo
                Dim intIRIndex As Integer
                Dim strchkEmail As String

                If blnEnterprise = True Then
                    Dim objMail As New Email
                    '//only send mail if transaction successfully created
                    SqlQuery = "SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_MSTR " &
                            "WHERE IRM_IR_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND IRM_IR_NO = '" & strIRNo & "'"
                    intIRIndex = objDb.GetVal(SqlQuery)

                    sendMailToSKForIR(strIRNo, intIRIndex, 1, ChkUrgent) 'Send to HOD
                    'If ChkUrgent = "Y" Then
                    '    objMail.sendNotification(EmailType.IRToSK, strUserId, strCoyId, "", strIRNo, "", "Y", strUserName)
                    'Else
                    '    objMail.sendNotification(EmailType.IRToSK, strUserId, strCoyId, "", strIRNo, "", "N", strUserName)
                    'End If
                    strchkEmail = objDb.GetVal("SELECT IFNULL(CM_URGENT_STOCK_EMAIL,'N') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

                    If ChkUrgent = "Y" And strchkEmail = "Y" Then
                        SqlQuery = "SELECT IRA_AO, IRA_A_AO, IRA_APPROVAL_GRP_INDEX FROM ir_approval WHERE IRA_IR_INDEX =" & intIRIndex
                        dsApp = objDb.FillDs(SqlQuery)

                        If dsApp.Tables(0).Rows.Count > 0 Then
                            SqlQuery = "SELECT AGM_MRS_EMAIL1 FROM APPROVAL_GRP_MSTR WHERE AGM_GRP_INDEX =" & dsApp.Tables(0).Rows(0)("IRA_APPROVAL_GRP_INDEX")
                            strMgr = objDb.GetVal(SqlQuery)
                            strMgr = Replace(strMgr, " ", "")
                            strStoreMgr = Split(strMgr, ";")

                            For i = 0 To strStoreMgr.Length - 1
                                If System.Text.RegularExpressions.Regex.IsMatch(strStoreMgr(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                                    sendMailToStoreMgr(strIRNo, strStoreMgr(i), HttpContext.Current.Session("UserName"))
                                    'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                                End If
                            Next
                        End If
                    End If

                End If
                insertIR = WheelMsgNum.Save
            Else
                insertIR = WheelMsgNum.NotSave
            End If
        End Function

        'Public Function insertIT(ByVal aryITItem As ArrayList, ByRef strITNo As String, ByRef RefNo As String, ByRef Remark As String, ByRef Code As String, ByRef RetLoc As String, ByRef RetSLoc As String) As Integer
        '    'Yap 2011 Inv
        '    Dim strPrefix
        '    Dim SqlQuery, PM_PRODUCT_INDEX, PM_LOC_INDEX, PM_LOC_INDEX2, ID_INVENTORY_QTY As String
        '    Dim ItemName As String
        '    Dim strAryQuery(0) As String
        '    Dim i As Integer

        '    Dim aryItem As New ArrayList()
        '    aryItem = aryITItem

        '    'GetLatestDocNo
        '    objGlobal.GetLatestDocNo("IT", strAryQuery, strITNo, strPrefix)

        '    ' Group All Duplicate
        '    Dim tempItem, tempLoc, tempSLoc, tempTLoc, tempTSLoc As String
        '    Dim tempQty As Integer = 0
        '    Dim j As Integer
        '    For i = 0 To aryItem.Count - 1
        '        tempItem = aryItem(i)(0)
        '        tempLoc = aryItem(i)(3)
        '        tempSLoc = aryItem(i)(4)
        '        tempTLoc = aryItem(i)(5)
        '        tempTSLoc = aryItem(i)(6)
        '        For j = 0 To aryItem.Count - 1
        '            If aryItem(j)(7) <> "Done" And tempItem = aryItem(j)(0) And tempLoc = aryItem(j)(3) And tempSLoc = aryItem(j)(4) Then
        '                aryItem(j)(0) = ""
        '                aryItem(j)(7) = "Done"
        '                tempQty = tempQty + IIf(aryItem(j)(2) = "", 0, aryItem(j)(2))
        '            End If
        '        Next
        '        aryItem.Add(New String() {tempItem, "NAME", tempQty, tempLoc, tempSLoc, tempTLoc, tempTSLoc, "Done"})
        '        tempQty = 0
        '    Next

        '    For i = 0 To aryItem.Count - 1
        '        If aryItem(i)(0) <> "---Select---" And aryItem(i)(0) <> "" And aryItem(i)(2) <> "0" _
        '            And aryItem(i)(5) <> "---Select---" And aryItem(i)(6) <> "---Select---" Then

        '            Code = aryItem(i)(0)

        '            SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " & _
        '                           " WHERE  IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
        '            PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

        '            If Common.parseNull(aryItem(i)(4)) = "" Then
        '                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' "
        '                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
        '            Else
        '                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(4) & "' "
        '                PM_LOC_INDEX = objDb.GetVal(SqlQuery)
        '            End If

        '            If Common.parseNull(aryItem(i)(6)) = "" Then
        '                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(5) & "' "
        '                PM_LOC_INDEX2 = objDb.GetVal(SqlQuery)
        '            Else
        '                SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(5) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(6) & "' "
        '                PM_LOC_INDEX2 = objDb.GetVal(SqlQuery)
        '            End If

        '            SqlQuery = " SELECT ID_INVENTORY_QTY FROM INVENTORY_DETAIL " & _
        '                       " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
        '            ID_INVENTORY_QTY = objDb.GetVal(SqlQuery)

        '            If CInt(aryItem(i)(2)) > CInt(IIf(ID_INVENTORY_QTY = "", 0, ID_INVENTORY_QTY)) Then
        '                insertIT = 10
        '                RetLoc = aryItem(i)(3)
        '                RetSLoc = aryItem(i)(4)
        '                Exit Function
        '            End If

        '            SqlQuery = " UPDATE INVENTORY_DETAIL " & _
        '                       " SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & aryItem(i)(2) & " " & _
        '                       " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
        '            Common.Insert2Ary(strAryQuery, SqlQuery)


        '            SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " & _
        '                           " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX2 & "' "

        '            If objDb.Exist(SqlQuery) Then
        '                SqlQuery = " UPDATE INVENTORY_DETAIL " & _
        '                           " SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & aryItem(i)(2) & " " & _
        '                           " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX2 & "' "
        '                Common.Insert2Ary(strAryQuery, SqlQuery)
        '            Else
        '                SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " & _
        '                           " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX2 & ", " & aryItem(i)(2) & ") "
        '                Common.Insert2Ary(strAryQuery, SqlQuery)
        '            End If

        '            'Modified by Joon on 1 June 2011 - insert IM_INVENTORY_NAME into INVENTORY_TRANS table (IT_INVENTORY_NAME column)
        '            SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " & _
        '                          "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
        '            ItemName = objDb.GetVal(SqlQuery)

        '            SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
        '                       " VALUES ('" & strITNo & "', " & _
        '                       " '" & PM_PRODUCT_INDEX & "', " & _
        '                       " 'TR', " & aryItem(i)(2) & ", " & _
        '                       " " & Common.ConvertDate(Now) & ", " & _
        '                       " '" & PM_LOC_INDEX & "', " & _
        '                       " '" & PM_LOC_INDEX2 & "', " & _
        '                       " '" & Common.Parse(RefNo) & "', '" & Common.Parse(Remark) & "', " & _
        '                       " '" & HttpContext.Current.Session("UserId") & "', " & _
        '                       " '" & Common.Parse(ItemName) & "') "
        '            Common.Insert2Ary(strAryQuery, SqlQuery)
        '        End If
        '    Next
        '    If objDb.BatchExecute(strAryQuery) Then
        '        insertIT = WheelMsgNum.Save
        '    Else
        '        insertIT = WheelMsgNum.NotSave
        '    End If
        'End Function

        Public Function insertIT(ByVal aryITItem As ArrayList, ByRef strITNo As String, ByRef RefNo As String, ByRef Remark As String, ByRef Code As String, ByRef RetLoc As String, ByRef RetSLoc As String) As Integer
            'Yap 2011 Inv
            Dim strPrefix
            Dim SqlQuery, PM_PRODUCT_INDEX, PM_LOC_INDEX, PM_LOC_INDEX2, ID_INVENTORY_QTY As String
            Dim ItemName As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            Dim intIncrementNo As Integer = 0
            Dim strNewITNo As String = ""

            Dim aryItem As New ArrayList()
            aryItem = aryITItem

            'GetLatestDocNo
            'objGlobal.GetLatestDocNo("IT", strAryQuery, strITNo, strPrefix)
            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'IT' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strITNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                   & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                   & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'IT' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            ' Group All Duplicate
            Dim tempItem, tempLoc, tempSLoc, tempTLoc, tempTSLoc As String
            Dim tempQty As Integer = 0
            Dim j As Integer
            For i = 0 To aryItem.Count - 1
                tempItem = aryItem(i)(0)
                tempLoc = aryItem(i)(3)
                tempSLoc = aryItem(i)(4)
                tempTLoc = aryItem(i)(5)
                tempTSLoc = aryItem(i)(6)
                For j = 0 To aryItem.Count - 1
                    If aryItem(j)(7) <> "Done" And tempItem = aryItem(j)(0) And tempLoc = aryItem(j)(3) And tempSLoc = aryItem(j)(4) Then
                        aryItem(j)(0) = ""
                        aryItem(j)(7) = "Done"
                        tempQty = tempQty + IIf(aryItem(j)(2) = "", 0, aryItem(j)(2))
                    End If
                Next
                aryItem.Add(New String() {tempItem, "NAME", tempQty, tempLoc, tempSLoc, tempTLoc, tempTSLoc, "Done"})
                tempQty = 0
            Next

            For i = 0 To aryItem.Count - 1
                If aryItem(i)(0) <> "---Select---" And aryItem(i)(0) <> "" And aryItem(i)(2) <> "0" _
                    And aryItem(i)(5) <> "---Select---" And aryItem(i)(6) <> "---Select---" Then

                    Code = aryItem(i)(0)

                    SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                   " WHERE  IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
                    PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                    If Common.parseNull(aryItem(i)(4)) = "" Then
                        SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' "
                        PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                    Else
                        SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(3) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(4) & "' "
                        PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                    End If

                    If Common.parseNull(aryItem(i)(6)) = "" Then
                        SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(5) & "' "
                        PM_LOC_INDEX2 = objDb.GetVal(SqlQuery)
                    Else
                        SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryItem(i)(5) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(6) & "' "
                        PM_LOC_INDEX2 = objDb.GetVal(SqlQuery)
                    End If

                    SqlQuery = " SELECT ID_INVENTORY_QTY FROM INVENTORY_DETAIL " &
                               " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
                    ID_INVENTORY_QTY = objDb.GetVal(SqlQuery)

                    If CInt(aryItem(i)(2)) > CInt(IIf(ID_INVENTORY_QTY = "", 0, ID_INVENTORY_QTY)) Then
                        insertIT = 10
                        RetLoc = aryItem(i)(3)
                        RetSLoc = aryItem(i)(4)
                        Exit Function
                    End If

                    SqlQuery = " UPDATE INVENTORY_DETAIL " &
                               " SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & aryItem(i)(2) & " " &
                               " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
                    Common.Insert2Ary(strAryQuery, SqlQuery)


                    SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " &
                                   " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX2 & "' "

                    If objDb.Exist(SqlQuery) Then
                        SqlQuery = " UPDATE INVENTORY_DETAIL " &
                                   " SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & aryItem(i)(2) & " " &
                                   " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX2 & "' "
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    Else
                        SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " &
                                   " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX2 & ", " & aryItem(i)(2) & ") "
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If

                    'Modified by Joon on 1 June 2011 - insert IM_INVENTORY_NAME into INVENTORY_TRANS table (IT_INVENTORY_NAME column)
                    SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " &
                                  "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & aryItem(i)(0) & "'"
                    ItemName = objDb.GetVal(SqlQuery)

                    SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TO_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                               " VALUES (" & strITNo & ", " &
                               " '" & PM_PRODUCT_INDEX & "', " &
                               " 'TR', " & aryItem(i)(2) & ", " &
                               " " & Common.ConvertDate(Now) & ", " &
                               " '" & PM_LOC_INDEX & "', " &
                               " '" & PM_LOC_INDEX2 & "', " &
                               " '" & Common.Parse(RefNo) & "', '" & Common.Parse(Remark) & "', " &
                               " '" & HttpContext.Current.Session("UserId") & "', " &
                               " '" & Common.Parse(ItemName) & "') "
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
            Next
            SqlQuery = " SET @T_NO = " & strITNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'IT' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewITNo, "T_NO") Then 'objDb.BatchExecute(strAryQuery) Then
                strITNo = strNewITNo
                insertIT = WheelMsgNum.Save
            Else
                insertIT = WheelMsgNum.NotSave
            End If
        End Function

        Public Function DeleteLocation(ByVal dtLoc As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim drLoc As DataRow

            For Each drLoc In dtLoc.Rows
                strSQL = "SELECT * FROM user_default WHERE UD_DEFAULT_TYPE='WH' AND UD_DEFAULT_VALUE='" & Common.Parse(drLoc("LocIndex")) & "'"
                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM inventory_detail WHERE ID_LOCATION_INDEX='" & Common.Parse(drLoc("LocIndex")) & "'"
                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "DELETE FROM location_mstr WHERE LM_LOCATION_INDEX='" & Common.Parse(drLoc("LocIndex")) & "'"
                Common.Insert2Ary(strAry, strSQL)
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function AddLocation(ByVal strLocDesc As String, ByVal strSubLocDesc As String) As String
            Dim strSQL As String
            If strSubLocDesc = "" Then
                strSQL = "SELECT * FROM location_mstr WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND LM_LOCATION='" & Common.Parse(strLocDesc) & "' AND (LM_SUB_LOCATION='' OR LM_SUB_LOCATION IS NULL)"

            Else
                strSQL = "SELECT * FROM location_mstr WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND LM_LOCATION='" & Common.Parse(strLocDesc) & "' AND LM_SUB_LOCATION='" & Common.Parse(strSubLocDesc) & "'"

            End If
            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "INSERT INTO location_mstr (LM_COY_ID,LM_LOCATION,LM_SUB_LOCATION,LM_ENT_BY,LM_ENT_DATETIME,LM_MOD_BY,LM_MOD_DATETIME) "
                strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                If Common.Parse(strLocDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strLocDesc) & "',"
                End If
                If Common.Parse(strSubLocDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strSubLocDesc) & "',"
                End If
                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    AddLocation = WheelMsgNum.Save
                Else
                    AddLocation = WheelMsgNum.NotSave
                End If

            Else
                AddLocation = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function ModLocation(ByVal strLocDesc As String, ByVal strSubLocDesc As String, ByVal intIndex As Integer) As String
            Dim strSQL As String
            If strSubLocDesc = "" Then
                strSQL = "SELECT * FROM location_mstr WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND LM_LOCATION='" & Common.Parse(strLocDesc) & "' AND (LM_SUB_LOCATION='' OR LM_SUB_LOCATION IS NULL) " _
                        & "AND LM_LOCATION_INDEX<>" & intIndex

            Else
                strSQL = "SELECT * FROM location_mstr WHERE LM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND LM_LOCATION='" & Common.Parse(strLocDesc) & "' AND LM_SUB_LOCATION='" & Common.Parse(strSubLocDesc) & "' " _
                        & "AND LM_LOCATION_INDEX<>" & intIndex

            End If
            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "UPDATE location_mstr SET "
                If Common.Parse(strLocDesc) = "" Then
                    strSQL &= "LM_LOCATION=NULL, "
                Else
                    strSQL &= "LM_LOCATION='" & Common.Parse(strLocDesc) & "',"
                End If
                If Common.Parse(strSubLocDesc) = "" Then
                    strSQL &= "LM_SUB_LOCATION=NULL, "
                Else
                    strSQL &= "LM_SUB_LOCATION='" & Common.Parse(strSubLocDesc) & "',"
                End If
                strSQL &= "LM_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', LM_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                        & "WHERE LM_LOCATION_INDEX=" & intIndex
                If objDb.Execute(strSQL) Then
                    ModLocation = WheelMsgNum.Save
                Else
                    ModLocation = WheelMsgNum.NotSave
                End If

            Else
                ModLocation = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function GetInvVerify() As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            SQLQuery = "SELECT IV_VERIFY_INDEX,IV_GRN_NO,IM_COY_ID,GM_S_COY_ID,CM_COY_NAME,POM_PO_INDEX,POM_PO_NO," _
                    & "DOM_DO_INDEX,DOM_DO_NO,GM_CREATED_DATE,GM_DATE_RECEIVED " _
                    & "FROM inventory_verify " _
                    & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IV_INVENTORY_INDEX " _
                    & "INNER JOIN grn_mstr ON IV_GRN_NO=GM_GRN_NO " _
                    & "INNER JOIN company_mstr ON GM_S_COY_ID=CM_COY_ID " _
                    & "INNER JOIN po_mstr ON POM_PO_INDEX=GM_PO_INDEX " _
                    & "INNER JOIN do_mstr ON DOM_DO_INDEX=GM_DO_INDEX " _
                    & "WHERE IV_VERIFY_STATUS='P' AND IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND GM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "GROUP BY IV_GRN_NO"

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function GetInvVerifyDetails(ByVal strGRNNo As String) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            SQLQuery = "SELECT IV_VERIFY_INDEX,IV_GRN_NO,IM_COY_ID,GM_S_COY_ID,CM_COY_NAME,GM_CREATED_DATE,GM_DATE_RECEIVED," _
                    & "IV_INVENTORY_INDEX,IM_ITEM_CODE,IM_INVENTORY_NAME," _
                    & "IV_LOCATION_INDEX,LM_LOCATION,LM_SUB_LOCATION,IV_RECEIVE_QTY " _
                    & "FROM inventory_verify " _
                    & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IV_INVENTORY_INDEX " _
                    & "INNER JOIN grn_mstr ON IV_GRN_NO=GM_GRN_NO " _
                    & "INNER JOIN company_mstr ON GM_S_COY_ID=CM_COY_ID " _
                    & "INNER JOIN location_mstr ON IV_LOCATION_INDEX=LM_LOCATION_INDEX " _
                    & "WHERE IV_VERIFY_STATUS='P' AND IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND GM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND IV_GRN_NO='" & Common.Parse(strGRNNo) & "'"

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function GetInvVerifyDetailsQty(ByVal strGRNNo As String, ByVal intInvIndex As Integer, ByVal intLocIndex As Integer, ByVal strType As String) As String
            Dim SQLQuery As String
            Dim dsInv As DataSet
            'SQLQuery = "SELECT SUM(IT_TRANS_QTY) AS Qty " _
            '        & "FROM inventory_trans " _
            '        & "INNER JOIN inventory_verify ON IV_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
            '        & "AND IV_LOCATION_INDEX=IT_TO_LOCATION_INDEX " _
            '        & "AND IT_TRANS_REF_NO=IV_GRN_NO AND IT_TRANS_TYPE='" & strType & "' " _
            '        & "AND IV_GRN_NO='" & strGRNNo & "'"
            SQLQuery = "SELECT SUM(IT_TRANS_QTY) AS Qty " _
                    & "FROM inventory_trans " _
                    & "INNER JOIN inventory_verify ON IV_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
                    & "AND IV_LOCATION_INDEX=IT_TO_LOCATION_INDEX AND IT_TRANS_REF_NO=IV_GRN_NO " _
                    & "WHERE IV_GRN_NO='" & Common.Parse(strGRNNo) & "' AND IT_INVENTORY_INDEX=" & intInvIndex _
                    & " AND IT_TO_LOCATION_INDEX=" & intLocIndex _
                    & " AND IT_TRANS_TYPE='" & Common.Parse(strType) & "' " _
                    & "GROUP BY IT_TRANS_REF_NO,IT_INVENTORY_INDEX,IT_TRANS_TYPE,IT_TO_LOCATION_INDEX"
            dsInv = objDb.FillDs(SQLQuery)
            If dsInv.Tables(0).Rows.Count > 0 Then
                GetInvVerifyDetailsQty = Common.parseNull(dsInv.Tables(0).Rows(0).Item(0))

            Else
                GetInvVerifyDetailsQty = ""
            End If
            dsInv = Nothing

        End Function

        Public Function GetInvVerifyItemName(ByVal strGRNNo As String, ByVal intInvIndex As Integer, ByVal intLocIndex As Integer) As String
            Dim SQLQuery As String
            Dim dsInv As DataSet
            'SQLQuery = "SELECT SUM(IT_TRANS_QTY) AS Qty " _
            '        & "FROM inventory_trans " _
            '        & "INNER JOIN inventory_verify ON IV_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
            '        & "AND IV_LOCATION_INDEX=IT_TO_LOCATION_INDEX " _
            '        & "AND IT_TRANS_REF_NO=IV_GRN_NO AND IT_TRANS_TYPE='" & strType & "' " _
            '        & "AND IV_GRN_NO='" & strGRNNo & "'"
            SQLQuery = "SELECT IT_INVENTORY_NAME " _
                    & "FROM inventory_trans " _
                    & "INNER JOIN inventory_verify ON IV_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
                    & "AND IV_LOCATION_INDEX = IT_TO_LOCATION_INDEX And IT_TRANS_REF_NO = IV_GRN_NO " _
                    & "WHERE IV_GRN_NO='" & Common.Parse(strGRNNo) & "' " _
                    & "AND IT_INVENTORY_INDEX=" & intInvIndex _
                    & " AND IT_TO_LOCATION_INDEX=" & intLocIndex _
                    & " AND IT_TRANS_TYPE IN ('VP','VF')"
            dsInv = objDb.FillDs(SQLQuery)
            If dsInv.Tables(0).Rows.Count > 0 Then
                GetInvVerifyItemName = Common.parseNull(dsInv.Tables(0).Rows(0).Item(0))

            Else
                GetInvVerifyItemName = ""
            End If
            dsInv = Nothing

        End Function

        Public Function SaveInventoryVerify(ByVal ds As DataSet) As Boolean
            Dim strSQL As String
            Dim dr As DataRow
            Dim strAryQuery(0) As String
            Dim dtMstr As DataTable
            Dim dtData As DataTable
            Dim dtDetails As DataTable

            dtMstr = ds.Tables(0)
            dtData = ds.Tables(1)
            dtDetails = ds.Tables(2)

            For Each dr In dtData.Rows
                'Modified by Joon on 1 June 2011 - insert IM_INVENTORY_NAME into INVENTORY_TRANS table (IT_INVENTORY_NAME column)
                strSQL = "INSERT INTO inventory_trans (IT_INVENTORY_INDEX,IT_TRANS_TYPE,IT_TRANS_REF_NO,IT_TRANS_QTY," _
                        & "IT_TO_LOCATION_INDEX,IT_REF_NO,IT_TRANS_DATE,IT_TRANS_USER_ID,IT_INVENTORY_NAME) " _
                        & "VALUES (" & dr("IT_INVENTORY_INDEX") & "," _
                        & "'" & Common.Parse(dr("IT_TRANS_TYPE")) & "'," _
                        & "'" & Common.Parse(dr("IT_TRANS_REF_NO")) & "'," _
                        & dr("IT_TRANS_QTY") & "," _
                        & dr("IT_TO_LOCATION_INDEX") & "," _
                        & "'" & Common.Parse(dr("IT_REF_NO")) & "'," _
                        & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," _
                        & "'" & Common.Parse(dr("IT_INVENTORY_NAME")) & "')"
                Common.Insert2Ary(strAryQuery, strSQL)
            Next

            For Each dr In dtMstr.Rows
                strSQL = "UPDATE inventory_verify SET IV_VERIFY_STATUS='" & Common.Parse(dr("IV_VERIFY_STATUS")) & "' " _
                        & "WHERE IV_VERIFY_INDEX=" & dr("IV_VERIFY_INDEX")
                Common.Insert2Ary(strAryQuery, strSQL)
            Next

            For Each dr In dtDetails.Rows
                strSQL = "UPDATE inventory_detail SET ID_INVENTORY_QTY=ID_INVENTORY_QTY+" & dr("ID_INVENTORY_QTY") _
                        & " WHERE ID_INVENTORY_INDEX=" & dr("ID_INVENTORY_INDEX") _
                        & " AND ID_LOCATION_INDEX=" & dr("ID_LOCATION_INDEX")
                Common.Insert2Ary(strAryQuery, strSQL)
            Next

            If objDb.BatchExecute(strAryQuery) Then
                Return True

            Else
                Return False
            End If

        End Function

        Public Function GetFullInvVerify(ByVal strGRNNo As String, ByVal strVendor As String, ByVal strStartDate As String, ByVal strEndDate As String) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            Dim strTemp As String

            SQLQuery = "SELECT IV_VERIFY_INDEX,IV_GRN_NO,IM_COY_ID,GM_S_COY_ID,CM_COY_NAME,GM_CREATED_DATE,GM_DATE_RECEIVED," _
                    & "IV_INVENTORY_INDEX,IM_ITEM_CODE,IM_INVENTORY_NAME," _
                    & "IV_LOCATION_INDEX,LM_LOCATION,LM_SUB_LOCATION,IV_RECEIVE_QTY " _
                    & "FROM inventory_verify " _
                    & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IV_INVENTORY_INDEX " _
                    & "INNER JOIN grn_mstr ON IV_GRN_NO=GM_GRN_NO " _
                    & "INNER JOIN company_mstr ON GM_S_COY_ID=CM_COY_ID " _
                    & "INNER JOIN location_mstr ON IV_LOCATION_INDEX=LM_LOCATION_INDEX " _
                    & "WHERE IV_VERIFY_STATUS='F' AND IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND GM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

            If strGRNNo <> "" Then
                strTemp = Common.BuildWildCard(strGRNNo)
                SQLQuery = SQLQuery & " AND IV_GRN_NO" & Common.ParseSQL(strTemp)
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                SQLQuery = SQLQuery & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                SQLQuery = SQLQuery & " AND GM_DATE_RECEIVED >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                SQLQuery = SQLQuery & " AND GM_DATE_RECEIVED <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function GetInvDetails(ByVal strItemCode As String, ByVal strItemName As String, ByVal strLoc As String, ByVal strSubLoc As String, Optional ByVal strSortBy As String = "", Optional ByVal chrQC As Char = Nothing) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            Dim strTemp As String
            SQLQuery = "SELECT ID_INVENTORY_INDEX, IM_ITEM_CODE, IM_INVENTORY_NAME," _
                & "ID_LOCATION_INDEX, LM_LOCATION, LM_SUB_LOCATION, ID_INVENTORY_QTY,IM_IQC_IND " _
                & "FROM inventory_detail " _
                & "INNER JOIN inventory_mstr ON ID_INVENTORY_INDEX=IM_INVENTORY_INDEX " _
                & "INNER JOIN location_mstr ON ID_LOCATION_INDEX=LM_LOCATION_INDEX " _
                & "WHERE IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                SQLQuery = SQLQuery & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strItemName <> "" Then
                strTemp = Common.BuildWildCard(strItemName)
                SQLQuery = SQLQuery & " AND IM_INVENTORY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strLoc <> "" And strLoc <> "---Select---" Then
                SQLQuery = SQLQuery & " AND LM_LOCATION ='" & Common.Parse(strLoc) & "'"
            End If

            If strSubLoc <> "" And strSubLoc <> "---Select---" Then
                SQLQuery = SQLQuery & " AND LM_SUB_LOCATION ='" & Common.Parse(strSubLoc) & "'"
            End If

            If chrQC <> Nothing Then
                SQLQuery = SQLQuery & " AND IM_IQC_IND='" & chrQC & "'"
            End If
            'If strSortBy <> "" Then
            '    SQLQuery = SQLQuery & " ORDER BY " & strSortBy
            'End If

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function SaveInventoryAdjustment(ByVal ds As DataSet) As Boolean
            Dim strSQL As String
            Dim dr As DataRow
            Dim strAryQuery(0) As String
            Dim dtMstr As DataTable
            Dim dtData As DataTable

            dtMstr = ds.Tables(0)
            dtData = ds.Tables(1)

            For Each dr In dtData.Rows
                'Modified by Joon on 1 June 2011 - insert IM_INVENTORY_NAME into INVENTORY_TRANS table (IT_INVENTORY_NAME column)
                strSQL = "INSERT INTO inventory_trans (IT_INVENTORY_INDEX,IT_TRANS_TYPE,IT_TRANS_QTY," _
                        & "IT_ADDITION_INFO,IT_TO_LOCATION_INDEX,IT_REMARK,IT_TRANS_DATE,IT_TRANS_USER_ID,IT_INVENTORY_NAME) " _
                        & "VALUES (" & dr("IT_INVENTORY_INDEX") & "," _
                        & "'" & Common.Parse(dr("IT_TRANS_TYPE")) & "'," _
                        & dr("IT_TRANS_QTY") & "," _
                        & "'" & Common.Parse(dr("IT_ADDITION_INFO")) & "'," _
                        & dr("IT_TO_LOCATION_INDEX") & "," _
                        & "'" & Common.Parse(dr("IT_REMARK")) & "'," _
                        & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," _
                        & "'" & Common.Parse(dr("IT_INVENTORY_NAME")) & "')"
                Common.Insert2Ary(strAryQuery, strSQL)

            Next

            For Each dr In dtMstr.Rows
                strSQL = "UPDATE inventory_detail SET ID_INVENTORY_QTY='" & Common.Parse(dr("ID_INVENTORY_QTY")) & "' " _
                        & "WHERE ID_INVENTORY_INDEX=" & dr("ID_INVENTORY_INDEX") _
                        & " AND ID_LOCATION_INDEX=" & dr("ID_LOCATION_INDEX")
                Common.Insert2Ary(strAryQuery, strSQL)
            Next

            If objDb.BatchExecute(strAryQuery) Then
                Return True

            Else
                Return False
            End If

        End Function

        Public Function GetInvAdjustmentList(ByVal strType As String, ByVal strItemCode As String, ByVal strItemName As String, ByVal strStartDate As String, ByVal strEndDate As String, ByVal strLoc As String, ByVal strSubLoc As String) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            Dim strTemp As String
            'Modified by Joon on 1 June 2011 - change IM_INVENTORY_NAME to IT_INVENTORY_NAME
            SQLQuery = "SELECT IM_INVENTORY_INDEX,IM_ITEM_CODE, IT_INVENTORY_NAME AS IM_INVENTORY_NAME," _
                    & "IT_TO_LOCATION_INDEX,LM_LOCATION,LM_SUB_LOCATION," _
                    & "IT_ADDITION_INFO,IT_TRANS_QTY,(IT_TRANS_QTY-IT_ADDITION_INFO) AS VarianceQty," _
                    & "IT_TRANS_DATE, IT_REMARK " _
                    & "FROM inventory_trans " _
                    & "INNER JOIN inventory_mstr ON IT_INVENTORY_INDEX=IM_INVENTORY_INDEX " _
                    & "INNER JOIN location_mstr ON IT_TO_LOCATION_INDEX=LM_LOCATION_INDEX " _
                    & "WHERE IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND IT_TRANS_TYPE='" & Common.Parse(strType) & "'"
            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                SQLQuery = SQLQuery & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            'Modified by Joon on 1 June 2011 - change IM_INVENTORY_NAME to IT_INVENTORY_NAME
            If strItemName <> "" Then
                strTemp = Common.BuildWildCard(strItemName)
                SQLQuery = SQLQuery & " AND IT_INVENTORY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                SQLQuery = SQLQuery & " AND IT_TRANS_DATE >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                SQLQuery = SQLQuery & " AND IT_TRANS_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            If strLoc <> "" And strLoc <> "---Select---" Then
                SQLQuery = SQLQuery & " AND LM_LOCATION ='" & Common.Parse(strLoc) & "'"
            End If

            If strSubLoc <> "" And strSubLoc <> "---Select---" Then
                SQLQuery = SQLQuery & " AND LM_SUB_LOCATION ='" & Common.Parse(strSubLoc) & "'"
            End If

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function GetInvTransHistory(ByVal intInvIndex As Integer, ByVal intLocIndex As Integer) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            'SQLQuery = "SELECT IT_TRANS_INDEX,IT_TRANS_DATE,IT_TRANS_TYPE,CODE_DESC," _
            '        & "IT_TRANS_USER_ID,UM_USER_NAME,IT_FRM_LOCATION_INDEX,IT_TO_LOCATION_INDEX," _
            '        & "IF(IT_FRM_LOCATION_INDEX IS NULL,IT_TRANS_QTY,-IT_TRANS_QTY) AS TRANS_QTY " _
            '        & "FROM inventory_trans " _
            '        & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
            '        & "INNER JOIN code_mstr ON IT_TRANS_TYPE=CODE_ABBR " _
            '        & "LEFT JOIN user_mstr ON IT_TRANS_USER_ID=UM_USER_ID " _
            '        & "WHERE CODE_CATEGORY='TT' AND IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '        & "AND UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '        & "AND IT_INVENTORY_INDEX=" & intInvIndex _
            '        & " AND (IT_FRM_LOCATION_INDEX=" & intLocIndex _
            '        & " OR IT_TO_LOCATION_INDEX=" & intLocIndex & ")"
            SQLQuery = "SELECT IT_TRANS_INDEX,IT_TRANS_DATE,IT_TRANS_TYPE,CODE_DESC,IT_TRANS_USER_ID,UM_USER_NAME," _
                    & "IT_FRM_LOCATION_INDEX,IT_TO_LOCATION_INDEX," _
                    & "(-IT_TRANS_QTY) AS TRANS_QTY " _
                    & "FROM inventory_trans " _
                    & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
                    & "INNER JOIN code_mstr ON IT_TRANS_TYPE=CODE_ABBR " _
                    & "LEFT JOIN user_mstr ON IT_TRANS_USER_ID=UM_USER_ID " _
                    & "WHERE CODE_CATEGORY='TT' AND IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND IT_INVENTORY_INDEX=" & intInvIndex _
                    & " AND IT_FRM_LOCATION_INDEX=" & intLocIndex _
                    & " UNION " _
                    & "SELECT IT_TRANS_INDEX,IT_TRANS_DATE,IT_TRANS_TYPE,CODE_DESC,IT_TRANS_USER_ID,UM_USER_NAME," _
                    & "IT_FRM_LOCATION_INDEX,IT_TO_LOCATION_INDEX,IT_TRANS_QTY AS TRANS_QTY " _
                    & "FROM inventory_trans " _
                    & "INNER JOIN inventory_mstr ON IM_INVENTORY_INDEX=IT_INVENTORY_INDEX " _
                    & "INNER JOIN code_mstr ON IT_TRANS_TYPE=CODE_ABBR " _
                    & "LEFT JOIN user_mstr ON IT_TRANS_USER_ID=UM_USER_ID " _
                    & "WHERE CODE_CATEGORY='TT' AND IM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND IT_INVENTORY_INDEX=" & intInvIndex _
                    & " AND IT_TO_LOCATION_INDEX=" & intLocIndex
            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv

        End Function

        Public Function GetInvVerifyLotApprList(ByVal strIQCNo As String, ByVal strVendor As String, ByVal strItemCode As String, ByVal strManu As String, ByVal strStartDate As String, ByVal strEndDate As String, ByVal strIQCType As String) As DataSet
            Dim SQLQuery As String
            Dim dsInv As DataSet
            Dim strTemp As String

            SQLQuery = "SELECT IQCA_AO, IQCA_A_AO, IVL_VERIFY_LOT_INDEX, IVL_IQC_NO, IM_ITEM_CODE, IM_INVENTORY_NAME, IV_GRN_NO, " &
                    "SUM(IVL_LOT_QTY) AS IVL_LOT_QTY, DOL_LOT_NO, DOL_DO_MANUFACTURER, GM_DATE_RECEIVED, PM_IQC_TYPE, CM_COY_NAME " &
                    "FROM IQC_APPROVAL IA, INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV, " &
                    "INVENTORY_MSTR IM, GRN_MSTR GM, DO_LOT DOL, PRODUCT_MSTR PM, COMPANY_MSTR CM " &
                    "WHERE IA.IQCA_IQC_INDEX = IVL.IVL_VERIFY_LOT_INDEX " &
                    "AND (IA.IQCA_AO = '" & HttpContext.Current.Session("UserID") & "' " &
                    "OR (IA.IQCA_A_AO = '" & HttpContext.Current.Session("UserID") & "' AND IQCA_RELIEF_IND='O')) AND (IA.IQCA_SEQ - 1 = IA.IQCA_AO_ACTION) " &
                    "AND IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                    "AND IV.IV_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                    "AND IV.IV_GRN_NO = GM.GM_GRN_NO " &
                    "AND IM.IM_COY_ID = GM.GM_B_COY_ID " &
                    "AND IVL.IVL_LOT_INDEX = DOL.DOL_LOT_INDEX " &
                    "AND IM.IM_ITEM_CODE = PM.PM_VENDOR_ITEM_CODE " &
                    "AND IM.IM_COY_ID = PM.PM_S_COY_ID " &
                    "AND GM.GM_S_COY_ID = CM.CM_COY_ID " &
                    "AND GM.GM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
                    "AND IVL_STATUS IS NULL "

            If strIQCNo <> "" Then
                strTemp = Common.BuildWildCard(strIQCNo)
                SQLQuery = SQLQuery & " AND IVL_IQC_NO" & Common.ParseSQL(strTemp)
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                SQLQuery = SQLQuery & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                SQLQuery = SQLQuery & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strManu <> "" Then
                strTemp = Common.BuildWildCard(strManu)
                SQLQuery = SQLQuery & " AND DOL_DO_MANUFACTURER" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                SQLQuery = SQLQuery & " AND GM_DATE_RECEIVED >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                SQLQuery = SQLQuery & " AND GM_DATE_RECEIVED <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            If strIQCType <> "" Then
                SQLQuery = SQLQuery & " AND PM_IQC_TYPE = '" & strIQCType & "' "
            End If

            SQLQuery &= " GROUP BY IVL_VERIFY_LOT_INDEX ORDER BY GM_DATE_RECEIVED, IVL_IQC_NO "

            dsInv = objDb.FillDs(SQLQuery)
            Return dsInv
        End Function

        Function getIQCInfo(ByVal strIQCNo As String) As DataSet
            Dim strSql, strSqlIQC, strSqlAttach, strCoyID As String
            Dim strIQCField As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")

            strIQCField = "GM_CREATED_DATE, GM_CREATED_BY, DOM_DO_NO, IM_INVENTORY_NAME, IM_ITEM_CODE, " &
                        "IVL_IQC_NO, IVL_LOT_INDEX, IVL_VERIFY_LOT_INDEX, SUM(IVL_LOT_QTY) AS IVL_LOT_QTY, IVL_STATUS, IM_INVOICE_NO, IM_CREATED_ON, " &
                        "DOL_LOT_NO, DOL_DO_MANUFACTURER, DOL_IQC_EXP_DT, DOL_IQC_MANU_DT, DOL_COY_ID, " &
                        "PM_UOM, PM_IQC_TYPE, POM_PO_NO, POM_VENDOR_CODE, POM_PO_DATE "

            strSqlIQC = " SELECT " & strIQCField & " FROM INVENTORY_VERIFY_LOT IVL " &
                        " INNER JOIN INVENTORY_VERIFY IV ON IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                        " INNER JOIN INVENTORY_MSTR IM ON IV.IV_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                        " INNER JOIN DO_LOT DOL ON IVL.IVL_LOT_INDEX = DOL.DOL_LOT_INDEX " &
                        " INNER JOIN GRN_MSTR GM ON IV.IV_GRN_NO = GM.GM_GRN_NO AND IM.IM_COY_ID = GM.GM_B_COY_ID " &
                        " INNER JOIN DO_MSTR DOM ON GM.GM_DO_INDEX = DOM.DOM_DO_INDEX " &
                        " INNER JOIN PO_MSTR POM ON GM.GM_PO_INDEX = POM.POM_PO_INDEX " &
                        " INNER JOIN PRODUCT_MSTR PM ON IM.IM_ITEM_CODE = PM.PM_VENDOR_ITEM_CODE AND IM.IM_COY_ID = PM.PM_S_COY_ID " &
                        " LEFT JOIN INVOICE_MSTR IV_M ON IV_M.IM_INVOICE_NO = GM.GM_INVOICE_NO AND IV_M.IM_S_COY_ID = GM.GM_S_COY_ID " &
                        " WHERE IVL_IQC_NO = '" & strIQCNo & "' AND IM_COY_ID = '" & strCoyID & "' " &
                        " GROUP BY IVL_IQC_NO "

            'strSqlIQC = "SELECT " & strIQCField & " FROM INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV, INVENTORY_MSTR IM, DO_LOT DOL, GRN_MSTR GM " _
            '& "LEFT JOIN INVOICE_MSTR IV_M ON IV_M.IM_INVOICE_NO = GM.GM_INVOICE_NO AND IV_M.IM_S_COY_ID = GM.GM_S_COY_ID, " _
            '& "DO_MSTR DOM, PO_MSTR POM, PRODUCT_MSTR PM " _
            '& "WHERE IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " _
            '& "AND IV.IV_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " _
            '& "AND IVL.IVL_LOT_INDEX = DOL.DOL_LOT_INDEX " _
            '& "AND IV.IV_GRN_NO = GM.GM_GRN_NO And IM.IM_COY_ID = GM.GM_B_COY_ID " _
            '& "AND GM.GM_DO_INDEX = DOM.DOM_DO_INDEX " _
            '& "AND GM.GM_PO_INDEX = POM.POM_PO_INDEX " _
            '& "AND IM.IM_ITEM_CODE = PM.PM_VENDOR_ITEM_CODE AND IM.IM_COY_ID = PM.PM_S_COY_ID " _
            '& "AND IVL_IQC_NO = '" & strIQCNo & "' AND IM_COY_ID = '" & strCoyID & "' " _
            '& "GROUP BY IVL_IQC_NO "
            ds = objDb.FillDs(strSqlIQC)

            Return ds
        End Function

        Function getIQCListForApproval(ByVal strIQCNo As String, ByVal strItemCode As String, ByVal strVendor As String, ByVal strManu As String, ByVal strIQCType As String,
                                    ByVal aryDate As ArrayList, ByVal aryMyStatus As ArrayList, ByVal aryIQCStatus As ArrayList, ByVal strRetest As String) As DataSet
            Dim strSql, strSql2, strTemp, strCoyID, strUserId As String
            Dim ds, ds1 As DataSet
            Dim i As Integer
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserId = HttpContext.Current.Session("UserId")

            strSql = "SELECT tb.*, CM_COY_NAME, CASE WHEN REMARK = 'App' THEN 'Approved' WHEN REMARK = 'Ver' THEN 'Verify' " &
                    "WHEN REMARK = 'Ret' THEN 'Retest' WHEN (REMARK = 'Wai' OR REMARK = 'Rep' OR REMARK = 'Rej') " &
                    "THEN 'Rejected' END AS APP_STATUS, CASE WHEN IVL_STATUS=0 THEN 'Outstanding' " &
                    "WHEN IVL_STATUS=1 THEN 'Closed (Approved)' WHEN IVL_STATUS=2 THEN 'Closed (Waived)' " &
                    "WHEN IVL_STATUS=3 THEN 'Closed (Replacement)' WHEN IVL_STATUS=4 THEN 'Rejected' END AS IQC_STATUS_DESC FROM " &
                    "(SELECT IVL_VERIFY_LOT_INDEX, IVL_IQC_NO, IM_ITEM_CODE, IQCA_AO_REMARK, " &
                    "LEFT(IQCA_AO_REMARK,3) AS REMARK, IM_COY_ID, IM_INVENTORY_NAME, " &
                    "GM_DATE_RECEIVED, IV_GRN_NO, SUM(IVL_LOT_QTY) AS IVL_LOT_QTY, DOL_LOT_NO, GM_S_COY_ID, DOL_DO_MANUFACTURER, " &
                    "DOL_IQC_EXP_DT, IFNULL(IVL_STATUS,0) AS IVL_STATUS, 'N' AS RETEST FROM IQC_APPROVAL " &
                    "INNER JOIN INVENTORY_VERIFY_LOT ON IQCA_IQC_INDEX = IVL_VERIFY_LOT_INDEX " &
                    "INNER JOIN INVENTORY_VERIFY ON IVL_VERIFY_INDEX = IV_VERIFY_INDEX " &
                    "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "INNER JOIN GRN_MSTR ON IV_GRN_NO = GM_GRN_NO AND IM_COY_ID = GM_B_COY_ID " &
                    "INNER JOIN DO_LOT ON IVL_LOT_INDEX = DOL_LOT_INDEX " &
                    "WHERE (IQCA_AO = '" & strUserId & "' OR (IQCA_A_AO = '" & strUserId & "' AND IQCA_RELIEF_IND='O')) AND IQCA_AO_ACTION <> 0 " &
                    "AND IQCA_AO_REMARK IS NOT NULL AND GM_B_COY_ID = '" & strCoyID & "' " &
                    "AND NOT IQCA_IQC_INDEX IN (SELECT DISTINCT IQCA_IQC_INDEX FROM IQC_APPROVAL_LOG) " &
                    "GROUP BY IVL_VERIFY_LOT_INDEX " &
                    "UNION ALL " &
                    "SELECT IVL_VERIFY_LOT_INDEX, IVL_IQC_NO, IM_ITEM_CODE, IQCA_AO_REMARK, " &
                    "CASE WHEN IQCA_AO_REMARK IS NULL THEN (SELECT LEFT(IQCA_AO_REMARK,3) FROM IQC_APPROVAL_LOG WHERE (IQCA_AO = '" & strUserId & "' OR (IQCA_A_AO = '" & strUserId & "' AND IQCA_RELIEF_IND='O')) " &
                    "AND IQCA_IQC_INDEX = IVL_VERIFY_LOT_INDEX ORDER BY IQCA_RETEST_COUNT DESC LIMIT 1) ELSE LEFT(IQCA_AO_REMARK,3) END AS REMARK, " &
                    "IM_COY_ID, IM_INVENTORY_NAME, GM_DATE_RECEIVED, IV_GRN_NO, SUM(IVL_LOT_QTY) AS IVL_LOT_QTY, DOL_LOT_NO, GM_S_COY_ID, DOL_DO_MANUFACTURER, " &
                    "DOL_IQC_EXP_DT, IFNULL(IVL_STATUS,0) AS IVL_STATUS, 'Y' AS RETEST FROM IQC_APPROVAL " &
                    "INNER JOIN INVENTORY_VERIFY_LOT ON IQCA_IQC_INDEX = IVL_VERIFY_LOT_INDEX " &
                    "INNER JOIN INVENTORY_VERIFY ON IVL_VERIFY_INDEX = IV_VERIFY_INDEX " &
                    "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "INNER JOIN GRN_MSTR ON IV_GRN_NO = GM_GRN_NO AND IM_COY_ID = GM_B_COY_ID " &
                    "INNER JOIN DO_LOT ON IVL_LOT_INDEX = DOL_LOT_INDEX " &
                    "WHERE (IQCA_AO = '" & strUserId & "' OR (IQCA_A_AO = '" & strUserId & "' AND IQCA_RELIEF_IND='O')) " &
                    "AND GM_B_COY_ID = '" & strCoyID & "' " &
                    "AND IQCA_IQC_INDEX IN (SELECT DISTINCT IQCA_IQC_INDEX FROM IQC_APPROVAL_LOG) " &
                    "GROUP BY IVL_VERIFY_LOT_INDEX) tb, " &
                    "PRODUCT_MSTR, COMPANY_MSTR WHERE IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " &
                    "AND GM_S_COY_ID = CM_COY_ID "

            If strIQCNo <> "" Then
                strTemp = Common.BuildWildCard(strIQCNo)
                strSql = strSql & " AND IVL_IQC_NO" & Common.ParseSQL(strTemp)
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql = strSql & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                strSql = strSql & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strIQCType <> "" Then
                strSql = strSql & " AND PM_IQC_TYPE = '" & strIQCType & "' "
            End If

            If strManu <> "" Then
                strTemp = Common.BuildWildCard(strManu)
                strSql = strSql & " AND DOL_DO_MANUFACTURER" & Common.ParseSQL(strTemp)
            End If

            If aryDate(0) <> "" Then
                strSql = strSql & " AND GM_DATE_RECEIVED >= " & Common.ConvertDate(aryDate(0))
            End If

            If aryDate(1) <> "" Then
                strSql = strSql & " AND GM_DATE_RECEIVED <= " & Common.ConvertDate(aryDate(1) & " 23:59:59.000")
            End If

            If aryDate(2) <> "" Then
                strSql = strSql & " AND DOL_IQC_EXP_DT >= " & Common.ConvertDate(aryDate(2))
            End If

            If aryDate(3) <> "" Then
                strSql = strSql & " AND DOL_IQC_EXP_DT <= " & Common.ConvertDate(aryDate(3) & " 23:59:59.000")
            End If

            'If Not aryMyStatus Is Nothing Then
            '    If aryMyStatus.Count > 0 Then
            '        For i = 0 To aryMyStatus.Count - 1
            '            If i = 0 Then
            '                strTemp = "STATUS = '" & aryMyStatus(i) & "' "
            '            Else
            '                strTemp &= "OR STATUS = '" & aryMyStatus(i) & "' "
            '            End If
            '        Next
            '        strTemp = "(" & strTemp & ") "
            '        If strRetest = "Y" Then
            '            strSql = strSql & "AND (" & strTemp & "OR retest='Y') "
            '        Else
            '            strSql = strSql & "AND " & strTemp
            '        End If
            '    Else
            '        If strRetest = "Y" Then
            '            strSql = strSql & "AND retest='Y' "
            '        End If
            '    End If
            'Else
            '    If strRetest = "Y" Then
            '        strSql = strSql & "AND retest='Y' "
            '    End If
            'End If

            If Not aryMyStatus Is Nothing Then
                If aryMyStatus.Count > 0 Then
                    For i = 0 To aryMyStatus.Count - 1
                        If i = 0 Then
                            strTemp = "REMARK = '" & aryMyStatus(i) & "' "
                        Else
                            strTemp &= "OR REMARK = '" & aryMyStatus(i) & "' "
                        End If
                    Next
                    strSql = strSql & "AND (" & strTemp & ") "
                End If
            End If

            If Not aryIQCStatus Is Nothing Then
                If aryIQCStatus.Count > 0 Then
                    For i = 0 To aryIQCStatus.Count - 1
                        If i = 0 Then
                            strTemp = "IVL_STATUS = '" & aryIQCStatus(i) & "' "
                        Else
                            strTemp &= "OR IVL_STATUS = '" & aryIQCStatus(i) & "' "
                        End If
                    Next
                    strSql = strSql & "AND (" & strTemp & ") "
                End If
            End If

            strSql &= " ORDER BY GM_DATE_RECEIVED, IVL_IQC_NO"
            'strSql = strSql & " GROUP BY IVL_IQC_NO ORDER BY GM_DATE_RECEIVED, IVL_IQC_NO "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Function getIQCInfoFromPO(ByVal strPONo As String, ByVal strItemCode As String) As DataSet
            Dim strSql, strCoyID, strTemp As String
            Dim ds As New DataSet

            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT POD_PUR_SPEC_NO, POD_SPEC1, POD_SPEC2, POD_SPEC3 FROM PO_DETAILS " &
                    "WHERE POD_PO_NO='" & strPONo & "' AND POD_COY_ID='" & strCoyID & "' AND POD_VENDOR_ITEM_CODE='" & strItemCode & "'"
            ds = objDb.FillDs(strSql)
            Return ds

        End Function

        Function IQCChkLotContinue(ByVal strVenId As String, ByVal strLotNo As String, ByVal strDONo As String) As String
            Dim strSql As String = ""
            Dim strTemp As String = ""

            strSql = "SELECT '*' FROM DO_LOT WHERE DOL_COY_ID = '" & strVenId & "' AND DOL_LOT_NO = '" & Common.Parse(strLotNo) & "' AND DOL_DO_NO <> '" & Common.Parse(strDONo) & "' "
            If objDb.Exist(strSql) Then
                strTemp = "Yes"
            Else
                strTemp = "No"
            End If

            IQCChkLotContinue = strTemp
        End Function

        Function getApprFlow(ByVal intIndex As Long, Optional ByVal intTracking As Integer = 0, Optional ByVal strType As String = "IQC", Optional ByVal strIndex2 As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            If strType = "IQC" Then
                If intTracking = 1 Then
                    strSql = "IQC_APPROVAL_LOG IQCA"
                Else
                    strSql = "IQC_APPROVAL IQCA"
                End If

                strSql = "SELECT IQCA.*, UMA.UM_USER_NAME AS AO_NAME, UMB.UM_USER_NAME AS AAO_NAME FROM " & strSql & " " &
                        "LEFT OUTER JOIN USER_MSTR UMA ON IQCA.IQCA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & strCoyId & "' " &
                        "LEFT OUTER JOIN USER_MSTR UMB ON IQCA.IQCA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID='" & strCoyId & "' " &
                        "WHERE IQCA_IQC_INDEX = " & intIndex & " "

                If intTracking = 1 Then
                    strSql = strSql & "ORDER BY IQCA_RETEST_COUNT, IQCA_SEQ"
                ElseIf intTracking = 0 Then
                    strSql = strSql & "ORDER BY IQCA_SEQ"
                End If

            ElseIf strType = "MRS" Then
                strSql = "SELECT IRA.*, UMA.UM_USER_NAME AS AO_NAME, UMB.UM_USER_NAME AS AAO_NAME, 'IR' AS TB FROM IR_APPROVAL IRA " &
                        "LEFT OUTER JOIN USER_MSTR UMA ON IRA.IRA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & strCoyId & "' " &
                        "LEFT OUTER JOIN USER_MSTR UMB ON IRA.IRA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID='" & strCoyId & "' " &
                        "WHERE IRA_IR_INDEX = " & intIndex & " "

                If strIndex2 <> "" Then
                    strSql &= "UNION ALL " &
                        "SELECT " & intIndex & " AS IRA_IR_INDEX, IRSM_BUYER_ID AS IRA_AO, NULL AS IRA_A_AO, " &
                        "(SELECT MAX(IRA_SEQ) + 1 FROM IR_APPROVAL WHERE IRA_IR_INDEX = " & intIndex & ") AS IRA_SEQ, " &
                        "IRSM_BUYER_ID AS IRA_ACTIVE_AO, " &
                        "CASE WHEN IRSM_IRS_STATUS = '6' THEN IRSM_STATUS_CHANGED_ON ELSE IRSM_IRS_APPROVED_DATE END AS IRA_ACTION_DATE, " &
                        "(SELECT MAX(IRA_SEQ) FROM IR_APPROVAL WHERE IRA_IR_INDEX = " & intIndex & ") AS IRA_AO_ACTION, " &
                        "CASE WHEN IRSM_IRS_STATUS = '6' THEN 'Rejected' " &
                        "WHEN (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4' " &
                        "OR IRSM_IRS_STATUS = '5') THEN 'Issued' " &
                        "Else '' END AS IRA_AO_REMARK, " &
                        "2 AS IRA_APPROVAL_TYPE, NULL AS IRA_APPROVAL_GRP_INDEX, NULL AS IRA_ON_BEHALFOF, NULL AS IRA_RELIEF_IND, " &
                        "UM_USER_NAME AS AO_NAME, NULL AS AAO_NAME, 'MRS' AS TB " &
                        "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                        "LEFT OUTER JOIN USER_MSTR ON IRSM_BUYER_ID = UM_USER_ID AND UM_COY_ID = '" & strCoyId & "' " &
                        "WHERE IRSM_IRS_INDEX = '" & strIndex2 & "' "
                End If
            End If


            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Sub updateAOAction(ByVal intIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByRef pQuery() As String, Optional ByVal DocType As String = "IQC")
            Dim strSql, strLoginUser As String
            strLoginUser = HttpContext.Current.Session("UserId")

            If DocType = "IQC" Then
                strSql = "UPDATE IQC_APPROVAL SET IQCA_AO_REMARK='" & Common.Parse(strAORemark) & "',IQCA_ACTION_DATE=" &
                         Common.ConvertDate(Now) & ",IQCA_ACTIVE_AO='" & strLoginUser & "' WHERE IQCA_IQC_INDEX=" & intIndex & " AND IQCA_SEQ=" & intCurrentSeq
            ElseIf DocType = "IR" Then
                strSql = "UPDATE IR_APPROVAL SET IRA_AO_REMARK='" & Common.Parse(strAORemark) & "',IRA_ACTION_DATE=" &
                         Common.ConvertDate(Now) & ",IRA_ACTIVE_AO='" & strLoginUser & "' WHERE IRA_IR_INDEX=" & intIndex & " AND IRA_SEQ=" & intCurrentSeq
            End If

            Common.Insert2Ary(pQuery, strSql)

            If DocType = "IQC" Then
                strSql = "UPDATE IQC_APPROVAL SET IQCA_AO_ACTION = " & intCurrentSeq & " WHERE IQCA_IQC_INDEX=" & intIndex
            ElseIf DocType = "IR" Then
                strSql = "UPDATE IR_APPROVAL SET IRA_AO_ACTION = " & intCurrentSeq & " WHERE IRA_IR_INDEX=" & intIndex
            End If


            Common.Insert2Ary(pQuery, strSql)
        End Sub

        Function VerifyIQC(ByVal strCoyID As String, ByVal strLoginUser As String, ByVal strIQCNo As String, ByVal intIQCIndex As Long, ByVal intCurrentSeq As Integer, ByVal blnHighestLevel As Boolean,
        ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal strApprType As String, ByVal strManuDt As String, ByVal strExpDt As String,
        ByVal strOfficerType As String, Optional ByVal strSKId As String = "") As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strMsg, strIQC, strIQC1, strVendor As String
            Dim intIQCStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname As String
            Dim strMsg1 As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strIQC_Type As String
            Dim strSqlLast As String
            Dim intLotIndex As Long
            Dim i As Integer

            If strOfficerType = "IQCV" Then
                strApprType = "verified"
            Else
                strApprType = "approved"
            End If

            strSql = "SELECT IFNULL(IVL_STATUS,0) AS IVL_STATUS FROM INVENTORY_VERIFY_LOT WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intIQCStatus = tDS.Tables(0).Rows(0).Item("IVL_STATUS")
                If intIQCStatus = IQCStatus_new.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Waived Then
                    strMsg = "You have already waived this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Replacement Then
                    strMsg = "You have already done replacement for this IQC."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            If intCurrentSeq = 1 Then
                intLotIndex = objDb.GetVal("SELECT IVL_LOT_INDEX FROM INVENTORY_VERIFY_LOT WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex)

                If strManuDt <> "" Then
                    strSql = "UPDATE DO_LOT SET DOL_IQC_MANU_DT='" & Format(CDate(strManuDt), "yyyy-MM-dd") & "' WHERE DOL_LOT_INDEX=" & intLotIndex
                    Common.Insert2Ary(strSqlAry, strSql)
                End If

                If strExpDt <> "" Then
                    strSql = "UPDATE DO_LOT SET DOL_IQC_EXP_DT='" & Format(CDate(strExpDt), "yyyy-MM-dd") & "' WHERE DOL_LOT_INDEX=" & intLotIndex
                    Common.Insert2Ary(strSqlAry, strSql)
                End If
            End If

            If blnHighestLevel Then
                Dim dsInv As New DataSet
                strSql = "SELECT IVL_LOT_QTY, IVL_LOT_INDEX, IV_INVENTORY_INDEX, IV_LOCATION_INDEX FROM INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV " &
                        "WHERE IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                        "AND IVL_VERIFY_LOT_INDEX =" & intIQCIndex

                dsInv = objDb.FillDs(strSql)
                If dsInv.Tables(0).Rows.Count > 0 Then
                    If dsInv.Tables(0).Rows.Count > 0 Then
                        For i = 0 To dsInv.Tables(0).Rows.Count - 1
                            strSql = "UPDATE INVENTORY_DETAIL SET ID_IQC_QTY = IFNULL(ID_IQC_QTY,0) - " & CDec(dsInv.Tables(0).Rows(i)("IVL_LOT_QTY")) &
                                    " WHERE ID_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(i)("IV_INVENTORY_INDEX") &
                                    " AND ID_LOCATION_INDEX =" & dsInv.Tables(0).Rows(i)("IV_LOCATION_INDEX")

                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE INVENTORY_LOT SET IL_IQC_QTY = IFNULL(IL_IQC_QTY,0) - " & CDec(dsInv.Tables(0).Rows(i)("IVL_LOT_QTY")) &
                                    " WHERE IL_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(i)("IV_INVENTORY_INDEX") &
                                    " AND IL_LOCATION_INDEX =" & dsInv.Tables(0).Rows(i)("IV_LOCATION_INDEX") &
                                    " AND IL_LOT_INDEX =" & dsInv.Tables(0).Rows(i)("IVL_LOT_INDEX")

                            Common.Insert2Ary(strSqlAry, strSql)
                        Next
                    End If

                    'strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & CDec(dsInv.Tables(0).Rows(0)("IVL_LOT_QTY")) & _
                    '        " WHERE ID_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(0)("IV_INVENTORY_INDEX") & _
                    '        " AND ID_LOCATION_INDEX =" & dsInv.Tables(0).Rows(0)("IV_LOCATION_INDEX")

                    'Common.Insert2Ary(strSqlAry, strSql)
                End If

                'Update IQC status to Approved/Replacement/Waived
                strSql = "UPDATE INVENTORY_VERIFY_LOT SET IVL_STATUS=" & IQCStatus_new.Approved &
                " WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex

                Common.Insert2Ary(strSqlAry, strSql)

                updateAOAction(intIQCIndex, intCurrentSeq, strAORemark, strSqlAry)

                'IQC Approved/Rejected
                strMsg = "IQC No. " & strIQCNo & " has been " & strApprType & ". "

            Else
                updateAOAction(intIQCIndex, intCurrentSeq, strAORemark, strSqlAry)
                strMsg = "IQC No. " & strIQCNo & " has been " & strApprType & ". "
            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.IQCMod, WheelUserActivity.AO_ApproveIQC, strIQCNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                Dim objMail As New Email
                If blnHighestLevel Then
                    objMail.sendNotification(EmailType.IQCApprovedToSK, strSKId, strCoyID, "", strIQCNo, "")
                Else
                    'Send Email to next AO
                    sendMailToAO(strIQCNo, intIQCIndex, intCurrentSeq + 1)
                End If
            End If

            Return strMsg
        End Function

        Function ApproveIQC(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strIQCNo As String, ByVal intIQCIndex As Long, ByVal intCurrentSeq As Integer, ByVal blnHighestLevel As Boolean,
        ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal strApprType As String, ByVal strOffType As String, Optional ByVal strSKId As String = "", Optional ByVal strAction As String = "approve") As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strMsg, strIQC, strIQC1, strVendor As String
            Dim intIQCStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname As String
            Dim strMsg1 As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strIQC_Type As String
            Dim strSqlLast As String
            Dim i As Integer

            If strApprType = "1" Then
                strApprType = "approved"
            End If


            strSql = "SELECT IFNULL(IVL_STATUS,0) AS IVL_STATUS FROM INVENTORY_VERIFY_LOT WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intIQCStatus = tDS.Tables(0).Rows(0).Item("IVL_STATUS")
                If intIQCStatus = IQCStatus_new.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Waived Then
                    strMsg = "You have already waived this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Replacement Then
                    strMsg = "You have already done replacement for this IQC."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            If blnHighestLevel Then
                'Update Qty to Inventory detail
                Dim dsInv As New DataSet
                strSql = "SELECT IVL_LOT_QTY, IVL_LOT_INDEX, IV_INVENTORY_INDEX, IV_LOCATION_INDEX FROM INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV " &
                        "WHERE IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                        "AND IVL_VERIFY_LOT_INDEX =" & intIQCIndex

                dsInv = objDb.FillDs(strSql)
                If dsInv.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsInv.Tables(0).Rows.Count - 1
                        strSql = "UPDATE INVENTORY_DETAIL SET ID_IQC_QTY = IFNULL(ID_IQC_QTY,0) - " & CDec(dsInv.Tables(0).Rows(i)("IVL_LOT_QTY")) &
                                " WHERE ID_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(i)("IV_INVENTORY_INDEX") &
                                " AND ID_LOCATION_INDEX =" & dsInv.Tables(0).Rows(i)("IV_LOCATION_INDEX")

                        Common.Insert2Ary(strSqlAry, strSql)

                        strSql = "UPDATE INVENTORY_LOT SET IL_IQC_QTY = IFNULL(IL_IQC_QTY,0) - " & CDec(dsInv.Tables(0).Rows(i)("IVL_LOT_QTY")) &
                                " WHERE IL_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(i)("IV_INVENTORY_INDEX") &
                                " AND IL_LOCATION_INDEX =" & dsInv.Tables(0).Rows(i)("IV_LOCATION_INDEX") &
                                " AND IL_LOT_INDEX =" & dsInv.Tables(0).Rows(i)("IVL_LOT_INDEX")

                        Common.Insert2Ary(strSqlAry, strSql)
                    Next
                End If

                'Update IQC status to Approved/Replacement/Waived
                strSql = "UPDATE INVENTORY_VERIFY_LOT SET IVL_STATUS=" & IQCStatus_new.Approved &
                " WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex

                Common.Insert2Ary(strSqlAry, strSql)

                updateAOAction(intIQCIndex, intCurrentSeq, strAORemark, strSqlAry)

                'IQC Approved/Rejected
                strMsg = "IQC No. " & strIQCNo & " has been " & strApprType & ". "

            Else
                updateAOAction(intIQCIndex, intCurrentSeq, strAORemark, strSqlAry)
                If strAction = "reject" Then
                    If strOffType = "IQCV" Then
                        strMsg = "IQC No. " & strIQCNo & " has been rejected from Verify level. "
                    ElseIf strOffType = "IQCPA" Then
                        strMsg = "IQC No. " & strIQCNo & " has been rejected from Production level. "
                    Else
                        strMsg = "IQC No. " & strIQCNo & " has been rejected from IQC Approval level. "
                    End If
                ElseIf strAction = "retest" Then
                    If strOffType = "IQCV" Then
                        strMsg = "IQC No. " & strIQCNo & " has been sent for re-testing from Verify level. "
                    ElseIf strOffType = "IQCPA" Then
                        strMsg = "IQC No. " & strIQCNo & " has been sent for re-testing from Production level. "
                    Else
                        strMsg = "IQC No. " & strIQCNo & " has been sent for re-testing from IQC Approval level. "
                    End If

                Else
                    strMsg = "IQC No. " & strIQCNo & " has been " & strApprType & ". "
                End If

            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.IQCMod, WheelUserActivity.AO_ApproveIQC, strIQCNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                Dim objMail As New Email
                If blnHighestLevel Then
                    objMail.sendNotification(EmailType.IQCApprovedToSK, strSKId, strCoyID, "", strIQCNo, "")
                Else
                    'Send Email to next AO
                    sendMailToAO(strIQCNo, intIQCIndex, intCurrentSeq + 1)
                End If
            End If

            Return strMsg
        End Function

        Function RejectIQC(ByVal strUserID As String, ByVal strCoyID As String, ByVal strIQCNo As String, ByVal intIQCIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal strRejStatus As String,
                ByVal strManuDt As String, ByVal strExpDt As String, ByVal blnHighestLevel As Boolean, Optional ByVal strSKId As String = "") As String
            Dim strSql, strSqlAry(0) As String
            Dim intIQCStatus, intCount, intLotIndex As Integer
            Dim strMsg, int As String
            Dim strNewIQCNo As String
            Dim intNewIQCIndex As Long
            Dim i As Integer

            strSql = "SELECT IFNULL(IVL_STATUS,0) AS IVL_STATUS FROM INVENTORY_VERIFY_LOT WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intIQCStatus = tDS.Tables(0).Rows(0).Item("IVL_STATUS")
                If intIQCStatus = IQCStatus_new.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Waived Then
                    strMsg = "You have already waived this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Replacement Then
                    strMsg = "You have already done replacement for this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Rejected Then
                    strMsg = "You have already rejected this IQC."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            Dim objUsers As New Users
            If strRejStatus = "r" Then
                objUsers.Log_UserActivity(strUserID, strCoyID, strSqlAry, WheelModule.IQCMod, WheelUserActivity.AO_ReplaceIQC, strIQCNo)
            ElseIf strRejStatus = "w" Then
                objUsers.Log_UserActivity(strUserID, strCoyID, strSqlAry, WheelModule.IQCMod, WheelUserActivity.AO_WaiveIQC, strIQCNo)
            Else
                objUsers.Log_UserActivity(strUserID, strCoyID, strSqlAry, WheelModule.IQCMod, WheelUserActivity.AO_RejectIQC, strIQCNo)
            End If
            objUsers = Nothing

            If strRejStatus = "" Then
                intIQCStatus = IQCStatus_new.Rejected
            Else
                If strRejStatus = "r" Then
                    intIQCStatus = IQCStatus_new.Replacement
                Else
                    intIQCStatus = IQCStatus_new.Waived
                End If
            End If


            strSql = "UPDATE INVENTORY_VERIFY_LOT SET IVL_STATUS=" & intIQCStatus &
                " WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex

            Common.Insert2Ary(strSqlAry, strSql)
            updateAOAction(intIQCIndex, intCurrentSeq, strAORemark, strSqlAry)

            intLotIndex = objDb.GetVal("SELECT IVL_LOT_INDEX FROM INVENTORY_VERIFY_LOT WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex)

            If strManuDt <> "" Then
                strSql = "UPDATE DO_LOT SET DOL_IQC_MANU_DT='" & Format(CDate(strManuDt), "yyyy-MM-dd") & "' WHERE DOL_LOT_INDEX=" & intLotIndex
                Common.Insert2Ary(strSqlAry, strSql)
            End If

            If strExpDt <> "" Then
                strSql = "UPDATE DO_LOT SET DOL_IQC_EXP_DT='" & Format(CDate(strExpDt), "yyyy-MM-dd") & "' WHERE DOL_LOT_INDEX=" & intLotIndex
                Common.Insert2Ary(strSqlAry, strSql)
            End If

            If strRejStatus = "r" Then
                If InStr(strIQCNo, "R(") > 0 And InStr(strIQCNo, ")") > 0 Then
                    intCount = strIQCNo.Length - 5
                    strNewIQCNo = Left(strIQCNo, intCount)
                Else
                    strNewIQCNo = strIQCNo
                End If

                strSql = "SELECT COUNT('*') FROM INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV, INVENTORY_MSTR IM " &
                        "WHERE(IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX) " &
                        "AND IV.IV_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                        "AND IM_COY_ID='" & strCoyID & "' " &
                        "AND IVL_IQC_NO LIKE '" & strNewIQCNo & "%' "
                intCount = objDb.GetVal(strSql)

                strNewIQCNo = strNewIQCNo & "R(" & Format(intCount, "00") & ")"
                intNewIQCIndex = get_IVL_id()

                strSql = "INSERT INTO INVENTORY_VERIFY_LOT (IVL_VERIFY_LOT_INDEX, IVL_VERIFY_INDEX, IVL_IQC_NO, IVL_LOT_INDEX, IVL_LOT_QTY) SELECT " &
                        intNewIQCIndex & ", IVL_VERIFY_INDEX, '" & strNewIQCNo & "', IVL_LOT_INDEX, IVL_LOT_QTY FROM INVENTORY_VERIFY_LOT WHERE " &
                        "IVL_VERIFY_LOT_INDEX=" & intIQCIndex

                Common.Insert2Ary(strSqlAry, strSql)

                strSql = "INSERT INTO IQC_APPROVAL (IQCA_IQC_INDEX, IQCA_AO, IQCA_A_AO, IQCA_SEQ, IQCA_AO_ACTION,IQCA_APPROVAL_TYPE, " &
                        "IQCA_APPROVAL_GRP_INDEX, IQCA_RELIEF_IND, IQCA_OFFICER_TYPE) SELECT " &
                        intNewIQCIndex & ", IQCA_AO, IQCA_A_AO, IQCA_SEQ, 0, IQCA_APPROVAL_TYPE, IQCA_APPROVAL_GRP_INDEX, IQCA_RELIEF_IND, IQCA_OFFICER_TYPE " &
                        "FROM IQC_APPROVAL WHERE " &
                        "IQCA_IQC_INDEX=" & intIQCIndex

                Common.Insert2Ary(strSqlAry, strSql)

                ''Update IQC status to Approved/Replacement/Waived
                'strSql = "UPDATE INVENTORY_VERIFY_LOT SET IVL_STATUS=" & IQCStatus_new.Replacement & _
                '" WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex

                'Common.Insert2Ary(strSqlAry, strSql)

            ElseIf strRejStatus = "w" Then
                Dim dsInv As New DataSet
                strSql = "SELECT IVL_LOT_QTY, IVL_LOT_INDEX, IV_INVENTORY_INDEX, IV_LOCATION_INDEX FROM INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY IV " &
                        "WHERE IVL.IVL_VERIFY_INDEX = IV.IV_VERIFY_INDEX " &
                        "AND IVL_VERIFY_LOT_INDEX =" & intIQCIndex

                dsInv = objDb.FillDs(strSql)
                If dsInv.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsInv.Tables(0).Rows.Count - 1
                        strSql = "UPDATE INVENTORY_DETAIL SET ID_IQC_QTY = IFNULL(ID_IQC_QTY,0) - " & CDec(dsInv.Tables(0).Rows(i)("IVL_LOT_QTY")) &
                                " WHERE ID_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(i)("IV_INVENTORY_INDEX") &
                                " AND ID_LOCATION_INDEX =" & dsInv.Tables(0).Rows(i)("IV_LOCATION_INDEX")

                        Common.Insert2Ary(strSqlAry, strSql)

                        strSql = "UPDATE INVENTORY_LOT SET IL_IQC_QTY = IFNULL(IL_IQC_QTY,0) - " & CDec(dsInv.Tables(0).Rows(i)("IVL_LOT_QTY")) &
                                " WHERE IL_INVENTORY_INDEX = " & dsInv.Tables(0).Rows(i)("IV_INVENTORY_INDEX") &
                                " AND IL_LOCATION_INDEX =" & dsInv.Tables(0).Rows(i)("IV_LOCATION_INDEX") &
                                " AND IL_LOT_INDEX =" & dsInv.Tables(0).Rows(i)("IVL_LOT_INDEX")

                        Common.Insert2Ary(strSqlAry, strSql)
                    Next

                End If
            End If

            If objDb.BatchExecute(strSqlAry) Then
                Dim objMail As New Email
                If strRejStatus = "r" Then
                    strMsg = "IQC No. " & strIQCNo & " has been rejected with replacement. New IQC No. " & strNewIQCNo & " has been generated."
                    sendMailToAO(strNewIQCNo, intNewIQCIndex, 1)
                    objMail.sendNotification(EmailType.IQCRejectedToSK, strSKId, strCoyID, "", strIQCNo, "", "r", strNewIQCNo)
                ElseIf strRejStatus = "w" Then
                    strMsg = "IQC No. " & strIQCNo & " has been waived."
                    objMail.sendNotification(EmailType.IQCRejectedToSK, strSKId, strCoyID, "", strIQCNo, "", "w")
                Else
                    strMsg = "IQC No. " & strIQCNo & " has been rejected with no replacement."
                    objMail.sendNotification(EmailType.IQCRejectedToSK, strSKId, strCoyID, "", strIQCNo, "", "", "SK")
                    objMail.sendNotification(EmailType.IQCRejectedToSK, strSKId, strCoyID, "", strIQCNo, "", "", "FM")

                    Dim dsAO As DataSet
                    If blnHighestLevel = True Then
                        strSql = "SELECT IQCA_AO AS AO_ID FROM IQC_APPROVAL WHERE IQCA_IQC_INDEX = " & intIQCIndex & " AND IQCA_SEQ <> " & intCurrentSeq & " " &
                                "UNION " &
                                "SELECT IQCA_A_AO AS AO_ID FROM IQC_APPROVAL WHERE IQCA_IQC_INDEX = " & intIQCIndex & " AND IQCA_SEQ <> " & intCurrentSeq & " AND IQCA_A_AO IS NOT NULL "
                        dsAO = objDb.FillDs(strSql)

                        For i = 0 To dsAO.Tables(0).Rows.Count - 1
                            objMail.sendNotification(EmailType.IQCRejectedToSK, dsAO.Tables(0).Rows(i)("AO_ID"), strCoyID, "", strIQCNo, "", "", "AO")
                        Next
                    End If
                End If
                objMail = Nothing
            Else
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            End If
            Return strMsg

        End Function

        Function ReTestIQC(ByVal strUserID As String, ByVal strCoyID As String, ByVal strIQCNo As String, ByVal intIQCIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal blnRelief As Boolean) As String
            Dim strSql, strSqlAry(0) As String
            Dim intIQCStatus, intIQCCount As Integer
            Dim strMsg As String

            strSql = "SELECT IFNULL(IVL_STATUS,0) AS IVL_STATUS FROM INVENTORY_VERIFY_LOT WHERE IVL_VERIFY_LOT_INDEX=" & intIQCIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intIQCStatus = tDS.Tables(0).Rows(0).Item("IVL_STATUS")
                If intIQCStatus = IQCStatus_new.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Waived Then
                    strMsg = "You have already waived this IQC."
                ElseIf intIQCStatus = IQCStatus_new.Replacement Then
                    strMsg = "You have already done replacement for this IQC."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strUserID, strCoyID, strSqlAry, WheelModule.IQCMod, WheelUserActivity.AO_ReTestIQC, strIQCNo)
            objUsers = Nothing

            'updateAOAction(intIQCIndex, intCurrentSeq, strAORemark, strSqlAry)
            strSql = "UPDATE IQC_APPROVAL SET IQCA_AO_REMARK='" & Common.Parse(strAORemark) & "',IQCA_ACTION_DATE=" &
                    Common.ConvertDate(Now) & ",IQCA_ACTIVE_AO='" & strUserID & "' WHERE IQCA_IQC_INDEX=" & intIQCIndex & " AND IQCA_SEQ=" & intCurrentSeq
            Common.Insert2Ary(strSqlAry, strSql)

            intIQCCount = objDb.GetVal("SELECT IFNULL(MAX(IQCA_RETEST_COUNT),0) + 1 FROM IQC_APPROVAL_LOG")

            'Update user attachment
            strSql = "UPDATE USER_ATTACHMENT SET UA_DOC_TYPE='IQCL', UA_DOC_NO=" & intIQCCount &
                    " WHERE UA_DOC_NO=" & intIQCIndex & " AND UA_DOC_TYPE='IQC' "
            Common.Insert2Ary(strSqlAry, strSql)

            'Insert into IQC_APPROVAL_LOG before update IQC_APPROVAL
            strSql = "INSERT INTO IQC_APPROVAL_LOG (IQCA_IQC_INDEX, IQCA_AO, IQCA_A_AO, IQCA_SEQ, IQCA_ACTIVE_AO, IQCA_ACTION_DATE, " &
                    "IQCA_AO_ACTION, IQCA_AO_REMARK, IQCA_APPROVAL_TYPE, IQCA_APPROVAL_GRP_INDEX, IQCA_ON_BEHALFOF, IQCA_RELIEF_IND, IQCA_OFFICER_TYPE, IQCA_RETEST_COUNT) " &
                    "SELECT IQCA_IQC_INDEX, IQCA_AO, IQCA_A_AO, IQCA_SEQ, IQCA_ACTIVE_AO, IQCA_ACTION_DATE, " &
                    "IQCA_AO_ACTION, IQCA_AO_REMARK, IQCA_APPROVAL_TYPE, IQCA_APPROVAL_GRP_INDEX, IQCA_ON_BEHALFOF, IQCA_RELIEF_IND, IQCA_OFFICER_TYPE, " & intIQCCount & " " &
                    "FROM IQC_APPROVAL WHERE " &
                    "IQCA_IQC_INDEX=" & intIQCIndex
            Common.Insert2Ary(strSqlAry, strSql)

            strSql = "UPDATE IQC_APPROVAL SET IQCA_AO_ACTION = 0, IQCA_AO_REMARK = NULL, IQCA_ACTION_DATE = NULL, IQCA_ACTIVE_AO = NULL " &
                    "WHERE IQCA_IQC_INDEX=" & intIQCIndex
            Common.Insert2Ary(strSqlAry, strSql)

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                strMsg = "IQC No. " & strIQCNo & " has been sent for re-testing."
                sendMailToAO(strIQCNo, intIQCIndex, 1) 'Send back to first level of AO
            End If
            Return strMsg

        End Function

        Public Function sendMailToStoreMgr(ByVal strDocNo As String, ByVal strEmail As String, ByVal strUserName As String, Optional ByVal strAction As String = "I", Optional ByVal strStage As String = "IR")
            Dim strsql, strcond As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim i As Integer

            If strStage = "IR" Then
                strBody &= "<P>An urgent Inventory Requisition (" & strDocNo & ") has been submitted by your Requester (" & strUserName & "). <BR>"
            ElseIf strStage = "MRS" Then
                If strAction = "I" Then
                    strBody &= "<P>The MRS (" & strDocNo & ") has been issued/ partial issued by your Storekeeper (" & strUserName & "). <BR>"
                ElseIf strAction = "R" Then
                    strBody &= "<P>The MRS (" & strDocNo & ") has been rejected by your Storekeeper (" & strUserName & "). <BR>"
                End If
            ElseIf strStage = "RO" Then
                strBody &= "<P>The Return Outward (" & strDocNo & ") has been submitted by your Storekeeper (" & strUserName & "). <BR>"
            ElseIf strStage = "RI" Then
                strBody &= "<P>The Return Inward (" & strDocNo & ") has been submitted by your Requester (" & strUserName & "). <BR>"
            ElseIf strStage = "WO" Then
                If strAction = "I" Then
                    strBody &= "<P>You have a Write Off (" & strDocNo & ") waiting for you action. <BR>"
                ElseIf strAction = "R" Then
                    strBody &= "<P>The Write Off (" & strDocNo & ") has been cancelled by your Storekeeper (" & strUserName & ").<BR>"
                End If
            End If

            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            Dim objMail As New AppMail
            objMail.MailTo = strEmail
            If strStage = "IR" Then
                objMail.Body = "Dear Manager, <BR>" & strBody
            ElseIf strStage = "MRS" Or strStage = "RI" Or strStage = "WO" Or strStage = "RO" Then
                objMail.Body = "Dear Store Manager, <BR>" & strBody
            End If

            If strStage = "IR" Then
                objMail.Subject = "IR Created"
            ElseIf strStage = "MRS" Then
                If strAction = "I" Then
                    objMail.Subject = "MRS Issued/ Partial Issued"
                ElseIf strAction = "R" Then
                    objMail.Subject = "MRS Rejected"
                End If
            ElseIf strStage = "RO" Then
                objMail.Subject = "Return Outward Created"
            ElseIf strStage = "RI" Then
                objMail.Subject = "Return Inward Created"
            ElseIf strStage = "WO" Then
                If strAction = "I" Then
                    objMail.Subject = "WO Created"
                ElseIf strAction = "R" Then
                    objMail.Subject = "WO Cancelled"
                End If
            End If

            objMail.SendMail()
            objCommon = Nothing
        End Function

        Public Function sendMailToAO(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, Optional ByVal blnTest As Boolean = False)
            Dim strsql, strcond As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim i As Integer

            If blnTest = True Then
                strBody &= "<P>You have an outstanding IQC (" & strDocNo & ") is waiting for further action. <BR>"
            Else
                strBody &= "<P>You have an outstanding IQC (" & strDocNo & ") is waiting for approval. <BR>"
            End If
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            strsql = "SELECT RAM_USER_ID FROM RELIEF_ASSIGNMENT_MSTR "
            strsql &= "WHERE RAM_USER_ROLE = 'Approving Officer' "
            strsql &= "AND RAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND GETDATE() BETWEEN RAM_START_DATE AND RAM_END_DATE + 1 "
            strsql &= "AND RAM_USER_ID = "

            'Michelle (20/2/2012) - Issue 1512
            strcond = " WHERE IQCA_IQC_INDEX = " & intIndex & " AND IQCA_SEQ = " & intSeq & " ORDER BY IQCA_SEQ "
            strsql &= "('" & objDB.Get1Column("IQC_APPROVAL", "IQCA_AO", strcond) & "')"
            If objDB.Exist(strsql) > 0 Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            strsql = "SELECT IQCA_AO, IFNULL(IQCA_A_AO, '') AS IQCA_A_AO, B.UM_EMAIL AS AO_EMAIL, ISNULL(C.UM_EMAIL, '') AS AAO_EMAIL, "
            strsql &= "B.UM_USER_NAME AS AO_NAME, ISNULL(C.UM_USER_NAME, '') AS AAO_NAME "
            strsql &= "FROM IQC_APPROVAL "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = IQCA_AO AND B.UM_DELETED <> 'Y' AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = IQCA_A_AO AND C.UM_DELETED <> 'Y' AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "WHERE IQCA_IQC_INDEX = " & intIndex & " AND IQCA_SEQ = " & intSeq
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objMail As New AppMail
                'If blnRelief Then
                '    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (IQC Officer), <BR>" & strBody
                '    Else
                '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                '        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (IQC Officer), <BR>" & strBody
                '    End If
                'Else
                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (IQC Officer), <BR>" & strBody
                'End If
                'i = 1 + 1
                objMail.Subject = "Agora : IQC Approval"
                objMail.SendMail()

                If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) <> "" Then
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (IQC Officer), <BR>" & strBody
                    objMail.Subject = "Agora : IQC Approval"
                    objMail.SendMail()
                End If

            End If
            objCommon = Nothing
        End Function

        Public Function sendMailToSKForIR(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, Optional ByVal strUrgent As String = "N", Optional ByVal strTitle As String = "Created")
            Dim strsql, strcond As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim i As Integer

            If strUrgent = "N" Then
                strBody &= "<P>You have an Inventory Requisition (" & strDocNo & ") waiting for your action. <BR>"
            Else
                strBody &= "<P>You have an urgent Inventory Requisition (" & strDocNo & ") waiting for your action. <BR>"
            End If

            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            strsql = "SELECT RAM_USER_ID FROM RELIEF_ASSIGNMENT_MSTR "
            strsql &= "WHERE RAM_USER_ROLE = 'Approving Officer' "
            strsql &= "AND RAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND GETDATE() BETWEEN RAM_START_DATE AND RAM_END_DATE + 1 "
            strsql &= "AND RAM_USER_ID = "

            'Michelle (20/2/2012) - Issue 1512
            strcond = " WHERE IRA_IR_INDEX = " & intIndex & " AND IRA_SEQ = " & intSeq & " ORDER BY IRA_SEQ "
            strsql &= "('" & objDB.Get1Column("IR_APPROVAL", "IRA_AO", strcond) & "')"
            If objDB.Exist(strsql) > 0 Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            strsql = "SELECT IRA_AO, ISNULL(IRA_A_AO, '') AS IRA_A_AO, B.UM_EMAIL AS AO_EMAIL, ISNULL(C.UM_EMAIL, '') AS AAO_EMAIL, "
            strsql &= "B.UM_USER_NAME AS AO_NAME, ISNULL(C.UM_USER_NAME, '') AS AAO_NAME "
            strsql &= "FROM IR_APPROVAL "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = IRA_AO AND B.UM_DELETED <> 'Y' AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = IRA_A_AO AND C.UM_DELETED <> 'Y' AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "WHERE IRA_IR_INDEX = " & intIndex & " AND IRA_SEQ = " & intSeq
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objMail As New AppMail
                If blnRelief Then
                    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (HOD), <BR>" & strBody
                    Else
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (HOD), <BR>" & strBody
                    End If
                Else
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (HOD), <BR>" & strBody
                End If
                i = 1 + 1
                objMail.Subject = "Agora : IR " & strTitle
                objMail.SendMail()
            End If
            objCommon = Nothing
        End Function

        Public Function get_IVL_id() As String
            Dim strsQL As String = "SELECT MAX(IVL_VERIFY_LOT_INDEX) FROM INVENTORY_VERIFY_LOT"

            get_IVL_id = objDb.GetVal(strsQL) + 1
        End Function

        Public Function getLotExpiryDt(ByVal strLotIndex As String) As String
            Dim strSql As String = "SELECT IFNULL(DATE_FORMAT(DOL_IQC_EXP_DT,'%d/%m/%Y'),'') FROM DO_LOT WHERE DOL_LOT_INDEX = '" & strLotIndex & "'"

            getLotExpiryDt = objDb.GetVal(strSql)
        End Function

        Public Function getSection() As DataView
            Dim strSql As String
            Dim drw As DataView

            strSql = "SELECT CS_SEC_CODE, CONCAT(CS_SEC_CODE, ' : ', CS_SEC_NAME) AS SEC_DESC FROM COMPANY_SECTION_BUYER " &
                    "INNER JOIN COMPANY_SECTION ON CSB_SECTION_INDEX = CS_SEC_INDEX " &
                    "WHERE CS_DELETED = 'N' AND CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND CSB_SECTION_BUYER = '" & HttpContext.Current.Session("UserId") & "'"

            drw = objDb.GetView(strSql)
            Return drw
        End Function

        Public Function getStockBalance(ByVal strItemCode As String) As Decimal
            Dim strSql As String
            Dim dblStkBal As Decimal

            strSql = "SELECT IFNULL(SUM(ID_INVENTORY_QTY - IFNULL(ID_IQC_QTY,0)),0) AS ID_INVENTORY_QTY FROM INVENTORY_DETAIL " &
                    "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = ID_INVENTORY_INDEX " &
                    "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (ID_INVENTORY_QTY - IFNULL(ID_IQC_QTY,0)) > 0 " &
                    "AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "'"

            'strSql = "SELECT SUM(ID_INVENTORY_QTY - IFNULL(ID_IQC_QTY,0)) AS ID_INVENTORY_QTY FROM INVENTORY_DETAIL " & _
            '        "INNER JOIN INVENTORY_MSTR ON IM_INVENTORY_INDEX = ID_INVENTORY_INDEX " & _
            '        "WHERE IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "

            dblStkBal = objDb.GetVal(strSql)
            getStockBalance = dblStkBal

        End Function

        Public Function getLotBalance(ByVal strItemCode As String, ByVal strLot As String, ByVal strLoc As String, ByVal strSubLoc As String, Optional ByVal blnRej As Boolean = False) As Decimal
            Dim strSql, strRej As String
            Dim dblStk As Decimal

            strRej = "N"
            If blnRej = True Then
                strSql = "SELECT IVL_LOT_QTY FROM INVENTORY_VERIFY " &
                        "INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " &
                        "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                        "INNER JOIN LOCATION_MSTR ON IV_LOCATION_INDEX = LM_LOCATION_INDEX " &
                        "WHERE IVL_LOT_INDEX = '" & strLot & "' AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND IVL_STATUS = '4' "

                If strSubLoc = "" Then
                    strSql &= "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' AND LM_SUB_LOCATION IS NULL "
                Else
                    strSql &= "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' AND LM_SUB_LOCATION = '" & Common.Parse(strSubLoc) & "' "
                End If

                If objDb.Exist(strSql) > 0 Then
                    strRej = "Y"
                End If
            End If

            If strRej = "N" Then
                strSql = "SELECT IFNULL(IL_LOT_QTY,0) - IFNULL(IL_IQC_QTY,0) "
            Else
                strSql = "SELECT IFNULL(IL_LOT_QTY,0) "
            End If

            strSql &= "FROM INVENTORY_LOT " &
                    "INNER JOIN INVENTORY_MSTR ON IL_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "INNER JOIN LOCATION_MSTR ON IL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                    "WHERE IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND IL_LOT_INDEX = '" & strLot & "' "

            If strSubLoc = "" Then
                strSql &= "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' AND LM_SUB_LOCATION IS NULL "
            Else
                strSql &= "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' AND LM_SUB_LOCATION = '" & Common.Parse(strSubLoc) & "' "
            End If

            strSql &= "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "



            dblStk = objDb.GetVal(strSql)
            getLotBalance = dblStk

        End Function

        Public Function getMonthStockBalance(ByVal strItemCode As String, Optional ByVal strUserId As String = "") As Decimal
            Dim strSql, IM_INVENTORY_INDEX As String
            Dim dblMonthStk As Decimal

            'strSql = "SELECT IFNULL(SUM(IRSD_QTY),0) AS IRSD_QTY FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, " & _
            '            "INVENTORY_REQUISITION_SLIP_DETAILS IRSD, INVENTORY_MSTR IM " & _
            '            "WHERE IRSM.IRSM_IRS_COY_ID = IRSD.IRSD_IRS_COY_ID AND IRSM.IRSM_IRS_NO = IRSD.IRSD_IRS_NO " & _
            '            "AND IRSD.IRSD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " & _
            '            "AND (IRSM_IRS_STATUS = 2 OR IRSM_IRS_STATUS = 3 OR IRSM_IRS_STATUS = 4 OR IRSM_IRS_STATUS = 7) AND IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
            '            "AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " & _
            '            "AND MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(CURDATE()) " & _
            '            "AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(CURDATE()) "

            strSql = "SELECT IFNULL(SUM(IRSD_QTY),0) AS IRSD_QTY FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                    "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSM_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSM_IRS_NO = IRSD_IRS_NO " &
                    "INNER JOIN INVENTORY_MSTR ON IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "WHERE (IRSM_IRS_STATUS = 2 Or IRSM_IRS_STATUS = 3 Or IRSM_IRS_STATUS = 4 Or IRSM_IRS_STATUS = 7) " &
                    "AND IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                    "AND MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(CURDATE()) " &
                    "AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(CURDATE()) "

            If strUserId <> "" Then
                strSql &= "AND IRSM_IRS_INDEX IN " &
                        "(SELECT DISTINCT IRD_IR_SLIP_INDEX FROM INVENTORY_REQUISITION_MSTR " &
                        "INNER JOIN INVENTORY_REQUISITION_DETAILS ON IRM_IR_COY_ID = IRD_IR_COY_ID AND IRM_IR_NO = IRD_IR_NO " &
                        "INNER JOIN INVENTORY_MSTR ON IRD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                        "WHERE IRM_CREATED_BY = '" & strUserId & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                        "AND IRM_IR_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                        "AND IRD_IR_SLIP_INDEX IS NOT NULL) "
            End If


            dblMonthStk = objDb.GetVal(strSql)
            getMonthStockBalance = dblMonthStk

        End Function

        Public Function getLast3MthAve(ByVal strItemCode As String, ByVal strDeptCode As String) As Decimal
            Dim strSql As String
            Dim dblStk As Decimal
            Dim dbl3MthStk As Decimal = 0
            Dim i As Integer

            For i = 0 To 2
                strSql = "SELECT IFNULL(SUM(IRSD_QTY),0) AS IRSD_QTY FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, " &
                        "INVENTORY_REQUISITION_SLIP_DETAILS IRSD, INVENTORY_MSTR IM " &
                        "WHERE IRSM.IRSM_IRS_COY_ID = IRSD.IRSD_IRS_COY_ID And IRSM.IRSM_IRS_NO = IRSD.IRSD_IRS_NO " &
                        "AND IRSD.IRSD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                        "AND IRSM_IRS_STATUS = 2 AND IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                        "AND IRSM_IRS_DEPARTMENT = '" & Common.Parse(strDeptCode) & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                        "AND MONTH(IRSM_IRS_APPROVED_DATE) = MONTH(DATE_ADD(NOW(), INTERVAL -" & (i + 1) & " MONTH)) " &
                        "AND YEAR(IRSM_IRS_APPROVED_DATE) = YEAR(DATE_ADD(NOW(), INTERVAL -" & (i + 1) & " MONTH)) "
                dblStk = objDb.GetVal(strSql)
                dbl3MthStk = dbl3MthStk + dblStk
            Next

            dbl3MthStk = dbl3MthStk / 3
            getLast3MthAve = dbl3MthStk
        End Function

        Public Function getAvailableLoc(ByVal strItemCode As String) As DataSet
            Dim strSql As String
            Dim dsLoc As DataSet

            strSql = "SELECT LM_LOCATION, LM_SUB_LOCATION FROM INVENTORY_MSTR, INVENTORY_DETAIL, LOCATION_MSTR " &
                    "WHERE IM_INVENTORY_INDEX = ID_INVENTORY_INDEX " &
                    "AND ID_LOCATION_INDEX = LM_LOCATION_INDEX AND (ID_INVENTORY_QTY - IFNULL(ID_IQC_QTY,0)) > 0 " &
                    "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "'"

            dsLoc = objDb.FillDs(strSql)
            getAvailableLoc = dsLoc
        End Function

        Public Function getIRApprFlow() As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGM_GRP_INDEX, AGM_GRP_NAME FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO " &
                    "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' AND AGM_TYPE = 'MRS'"

            ds = objDb.FillDs(strsql)
            getIRApprFlow = ds
        End Function

        Public Function getIRAOList(ByVal intIndex As Integer) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGA_SEQ, AGA_AO, IFNULL(AGA_A_AO, '') AS AGA_A_AO, AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME " &
                    "FROM APPROVAL_GRP_AO " &
                    "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX " &
                    "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX " &
                    "WHERE AGA_GRP_INDEX = " & intIndex & " " &
                    "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' " &
                    "AND AGM_TYPE = 'MRS' " &
                    "ORDER BY AGA_SEQ "

            ds = objDb.FillDs(strsql)
            getIRAOList = ds
        End Function

        Public Function getIRHeaderDetail(ByVal strIRNo As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT * FROM INVENTORY_REQUISITION_MSTR " &
                    "LEFT JOIN COMPANY_SECTION ON IRM_IR_SECTION = CS_SEC_CODE AND IRM_IR_COY_ID = CS_COY_ID " &
                    "WHERE IRM_IR_NO='" & strIRNo & "' " &
                    "AND IRM_IR_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
            ds = objDb.FillDs(strsql)
            getIRHeaderDetail = ds
        End Function

        Function getInventoryReqFiltered(ByVal IRNo As String, ByVal IssueTo As String, ByVal Dept As String, ByVal DateS As String, ByVal DateE As String, ByVal aryIRStatus As ArrayList, ByVal aryMRSStatus As ArrayList) As Object
            Dim strsql, strTemp As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT IRM_IR_INDEX, IRM_IR_NO, IRM_CREATED_DATE, IRM_IR_APPROVED_DATE, IRM_IR_ISSUE_TO, CDM_DEPT_NAME, IRM_IR_REF_NO, " &
                    "IRM_IR_REMARK, IRM_IR_STATUS, IRM_IR_URGENT FROM INVENTORY_REQUISITION_MSTR IRM, COMPANY_DEPT_MSTR CDM, INVENTORY_REQUISITION_DETAILS IRD " &
                    "LEFT JOIN INVENTORY_REQUISITION_SLIP_MSTR IRSM ON IRSM.IRSM_IRS_INDEX = IRD.IRD_IR_SLIP_INDEX " &
                    "WHERE IRM.IRM_IR_NO = IRD.IRD_IR_NO AND IRM.IRM_IR_COY_ID = IRD.IRD_IR_COY_ID " &
                    "AND IRM.IRM_IR_DEPARTMENT = CDM.CDM_DEPT_CODE AND IRM.IRM_IR_COY_ID = CDM.CDM_COY_ID " &
                    "AND IRM_IR_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IRM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' "

            If IRNo <> "" Then
                strTemp = Common.BuildWildCard(IRNo)
                strsql = strsql & "AND IRM_IR_NO " & Common.ParseSQL(strTemp) & " "
            End If

            If IssueTo <> "" Then
                strTemp = Common.BuildWildCard(IssueTo)
                strsql = strsql & "AND IRM_IR_ISSUE_TO " & Common.ParseSQL(strTemp) & " "
            End If

            If Dept <> "" Then
                strTemp = Common.BuildWildCard(Dept)
                strsql = strsql & "AND CDM_DEPT_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If DateS <> "" Then
                strsql &= "AND IRM_CREATED_DATE >= " & Common.ConvertDate(DateS) & " "
            End If

            If DateE <> "" Then
                strsql &= "AND IRM_CREATED_DATE <= " & Common.ConvertDate(DateE & " 23:59:59") & " "
            End If

            If aryIRStatus(0) = "Y" Or aryIRStatus(1) = "Y" Or aryIRStatus(2) = "Y" Or aryIRStatus(3) = "Y" Then
                strTemp = ""

                'Submitted
                If aryIRStatus(0) = "Y" Then
                    strTemp &= "IRM_IR_STATUS=1 "
                End If

                'Approved
                If aryIRStatus(1) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRM_IR_STATUS=2 "
                    Else
                        strTemp &= "OR IRM_IR_STATUS=2 "
                    End If
                End If

                'Pending Approval
                If aryIRStatus(2) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRM_IR_STATUS=3 "
                    Else
                        strTemp &= "OR IRM_IR_STATUS=3 "
                    End If
                End If

                'Rejected
                If aryIRStatus(3) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRM_IR_STATUS=4 "
                    Else
                        strTemp &= "OR IRM_IR_STATUS=4 "
                    End If
                End If

                strsql &= "AND (" & strTemp & ") "
            End If

            If aryMRSStatus(0) = "Y" Or aryMRSStatus(1) = "Y" Or aryMRSStatus(2) = "Y" Or aryMRSStatus(3) = "Y" Or aryMRSStatus(4) = "Y" Or aryMRSStatus(5) = "Y" Or aryMRSStatus(6) = "Y" Then
                strTemp = ""

                'New
                If aryMRSStatus(0) = "Y" Then
                    strTemp &= "IRSM_IRS_STATUS=1 "
                End If

                'Issued
                If aryMRSStatus(1) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRSM_IRS_STATUS=2 "
                    Else
                        strTemp &= "OR IRSM_IRS_STATUS=2 "
                    End If
                End If

                'Partial Issued
                If aryMRSStatus(2) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRSM_IRS_STATUS=7 "
                    Else
                        strTemp &= "OR IRSM_IRS_STATUS=7 "
                    End If
                End If

                'Acknowledged
                If aryMRSStatus(3) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRSM_IRS_STATUS=3 "
                    Else
                        strTemp &= "OR IRSM_IRS_STATUS=3 "
                    End If
                End If

                'Auto-Acknowledged
                If aryMRSStatus(4) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRSM_IRS_STATUS=4 "
                    Else
                        strTemp &= "OR IRSM_IRS_STATUS=4 "
                    End If
                End If

                'Cancelled
                If aryMRSStatus(5) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRSM_IRS_STATUS=5 "
                    Else
                        strTemp &= "OR IRSM_IRS_STATUS=5 "
                    End If
                End If

                'Rejected
                If aryMRSStatus(6) = "Y" Then
                    If strTemp = "" Then
                        strTemp &= "IRSM_IRS_STATUS=6 "
                    Else
                        strTemp &= "OR IRSM_IRS_STATUS=6 "
                    End If
                End If

                strsql &= "AND (" & strTemp & ") "
            End If

            strsql = strsql & "ORDER BY IRM_IR_INDEX "

            ds = objDb.FillDs(strsql)
            getInventoryReqFiltered = ds
        End Function

        Public Function GetMRSNoAllIR(ByVal strIRNo As String, Optional ByVal aryMRSStatus As ArrayList = Nothing) As DataSet
            Dim strsql, strTemp As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT IRSM_IRS_INDEX, IRSM_IRS_NO, IRSM_IRS_STATUS FROM INVENTORY_REQUISITION_MSTR IRM, INVENTORY_REQUISITION_DETAILS IRD " &
                    "LEFT JOIN INVENTORY_REQUISITION_SLIP_MSTR IRSM ON IRSM.IRSM_IRS_INDEX = IRD.IRD_IR_SLIP_INDEX " &
                    "WHERE IRM.IRM_IR_NO = IRD.IRD_IR_NO And IRM.IRM_IR_COY_ID = IRD.IRD_IR_COY_ID " &
                    "AND IRM_IR_NO = '" & strIRNo & "' AND IRSM_IRS_INDEX IS NOT NULL "

            If Not aryMRSStatus Is Nothing Then
                If aryMRSStatus(0) = "Y" Or aryMRSStatus(1) = "Y" Or aryMRSStatus(2) = "Y" Or aryMRSStatus(3) = "Y" Or aryMRSStatus(4) = "Y" Or aryMRSStatus(5) = "Y" Or aryMRSStatus(6) = "Y" Then
                    strTemp = ""

                    'New
                    If aryMRSStatus(0) = "Y" Then
                        strTemp &= "IRSM_IRS_STATUS=1 "
                    End If

                    'Issued
                    If aryMRSStatus(1) = "Y" Then
                        If strTemp = "" Then
                            strTemp &= "IRSM_IRS_STATUS=2 "
                        Else
                            strTemp &= "OR IRSM_IRS_STATUS=2 "
                        End If
                    End If

                    'Partial Issued
                    If aryMRSStatus(2) = "Y" Then
                        If strTemp = "" Then
                            strTemp &= "IRSM_IRS_STATUS=7 "
                        Else
                            strTemp &= "OR IRSM_IRS_STATUS=7 "
                        End If
                    End If

                    'Acknowledged
                    If aryMRSStatus(3) = "Y" Then
                        If strTemp = "" Then
                            strTemp &= "IRSM_IRS_STATUS=3 "
                        Else
                            strTemp &= "OR IRSM_IRS_STATUS=3 "
                        End If
                    End If

                    'Auto-Acknowledged
                    If aryMRSStatus(4) = "Y" Then
                        If strTemp = "" Then
                            strTemp &= "IRSM_IRS_STATUS=4 "
                        Else
                            strTemp &= "OR IRSM_IRS_STATUS=4 "
                        End If
                    End If

                    'Cancelled
                    If aryMRSStatus(5) = "Y" Then
                        If strTemp = "" Then
                            strTemp &= "IRSM_IRS_STATUS=5 "
                        Else
                            strTemp &= "OR IRSM_IRS_STATUS=5 "
                        End If
                    End If

                    'Rejected
                    If aryMRSStatus(6) = "Y" Then
                        If strTemp = "" Then
                            strTemp &= "IRSM_IRS_STATUS=6 "
                        Else
                            strTemp &= "OR IRSM_IRS_STATUS=6 "
                        End If
                    End If

                    strsql &= "AND (" & strTemp & ") "
                End If
            End If

            ds = objDb.FillDs(strsql)
            GetMRSNoAllIR = ds
        End Function

        Public Function GetInvReqApprList(ByVal strIRNo As String, ByVal strReqName As String, ByVal strIssueTo As String, ByVal strDept As String, ByVal strDateS As String, ByVal strDateE As String,
        Optional ByVal strAction As String = "new", Optional ByVal strStatus As String = "", Optional ByVal strInclude As String = "") As DataSet
            Dim strsql, strTemp, strCondition, strCondition1, strReliefOn As String
            Dim ds As New DataSet
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            If strAction = "new" Then
                strCondition = " AND (IRA.IRA_SEQ - 1 = IRA.IRA_AO_ACTION) AND (IRM.IRM_IR_STATUS = '3' OR IRM.IRM_IR_STATUS = '1')"
                strCondition1 = " AND (IRA.IRA_AO = '" & strUser & "' " &
                    "OR (IRA.IRA_A_AO = '" & strUser & "' AND IRA.IRA_RELIEF_IND = 'O'))"
            ElseIf strAction = "app" Then
                If strStatus = "" Then
                    strCondition = " AND (IRA.IRA_SEQ <= IRA.IRA_AO_ACTION) "
                Else
                    If strStatus = "Approved" Then
                        strCondition = " AND (IRA.IRA_SEQ <= IRA.IRA_AO_ACTION) AND (SUBSTRING(IRA_AO_REMARK,1,8) = 'Approved') "
                    Else
                        strCondition = " AND (IRA.IRA_SEQ <= IRA.IRA_AO_ACTION) AND (SUBSTRING(IRA_AO_REMARK,1,8) = 'Rejected') "
                    End If
                End If

                strCondition1 = " AND (((IRA.IRA_AO = '" & strUser & "' " &
                "OR IRA.IRA_A_AO = '" & strUser & "') AND IRA.IRA_RELIEF_IND = 'O') OR ((IRA.IRA_AO = '" & strUser & "' " &
                "OR IRA.IRA_ON_BEHALFOF = '" & strUser & "') AND IRA.IRA_RELIEF_IND<>'O'))"
                strReliefOn = ""
            End If

            strsql = "SELECT IRM_IR_INDEX, IRM_IR_NO, IRM_IR_DATE, IRM_IR_REF_NO, IRM_IR_REQUESTOR_NAME, " &
                    "IRM_IR_APPROVED_DATE, IRM_IR_ISSUE_TO, CDM_DEPT_NAME, IRM_IR_REMARK, IRM_IR_URGENT, IRM_IR_STATUS, " &
                    "(CASE WHEN IRM_IR_STATUS = '4' THEN 'Rejected' ELSE 'Approved' END) AS STAT, " &
                    "(CASE WHEN IRM_IR_STATUS = '1' THEN 'Submitted' WHEN IRM_IR_STATUS = '2' THEN 'Approved' " &
                    "WHEN IRM_IR_STATUS = '3' THEN 'Pending Approval' WHEN IRM_IR_STATUS = '4' THEN 'Rejected' " &
                    "ELSE '' END) AS STATUS_DESC " &
                    "FROM IR_APPROVAL IRA, INVENTORY_REQUISITION_MSTR IRM, COMPANY_DEPT_MSTR CDM " &
                    "WHERE IRA.IRA_IR_INDEX = IRM.IRM_IR_INDEX " &
                    "AND IRM.IRM_IR_COY_ID = CDM.CDM_COY_ID AND IRM.IRM_IR_DEPARTMENT = CDM.CDM_DEPT_CODE " &
                    "AND IRM.IRM_IR_COY_ID = '" & strCoyId & "' " &
                    strCondition &
                    strCondition1

            If strIRNo <> "" Then
                strTemp = Common.BuildWildCard(strIRNo)
                strsql = strsql & " AND IRM_IR_NO " & Common.ParseSQL(strTemp) & " "
            End If

            If strReqName <> "" Then
                strTemp = Common.BuildWildCard(strReqName)
                strsql = strsql & " AND IRM_IR_REQUESTOR_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If strIssueTo <> "" Then
                strTemp = Common.BuildWildCard(strIssueTo)
                strsql = strsql & " AND IRM_IR_ISSUE_TO " & Common.ParseSQL(strTemp) & " "
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strsql = strsql & " AND CDM_DEPT_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If strDateS <> "" Then
                strsql &= " AND IRM_IR_DATE >= " & Common.ConvertDate(strDateS) & " "
            End If

            If strDateE <> "" Then
                strsql &= " AND IRM_IR_DATE <= " & Common.ConvertDate(strDateE & " 23:59:59") & " "
            End If

            If (strStatus = "Approved" Or strStatus = "") And strInclude = "" Then
                strsql = strsql & " AND IRM.IRM_IR_STATUS NOT IN (4) "
            End If


            ds = objDb.FillDs(strsql)
            GetInvReqApprList = ds
        End Function

        Function MassApprovalIR(ByVal strLoginUser As String, ByVal strAryIRIndex() As String, ByVal strAO As String, ByRef strReturnMsg() As String, ByVal blnRelief As Boolean) As Boolean
            Dim strSql, strSql1, strAryQuery(0), strApprType As String
            Dim strCoyID, strAllIR, strAllIRIndex As String
            Dim intLoop, i As Integer
            Dim ds As DataSet
            Dim strMsg As String
            Dim dsTemp As DataSet
            Dim dsDetail As DataSet
            Dim dblMthStkIssed, dblLast3MthAve, dblReqQty, dblStkBal As Decimal

            strCoyID = HttpContext.Current.Session("CompanyId")

            For intLoop = 0 To strAryIRIndex.Length - 1
                If intLoop = 0 Then
                    strAllIRIndex = strAryIRIndex(intLoop)
                Else
                    strAllIRIndex = strAllIRIndex & "," & strAryIRIndex(intLoop)
                End If
            Next

            strSql = "SELECT IRM_IR_INDEX, IRM_IR_NO, IRM_IR_COY_ID, IRM_IR_STATUS, IRM_STATUS_CHANGED_BY, IRM_IR_DEPARTMENT FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX IN (" & strAllIRIndex & ") "
            strSql1 = "SELECT * FROM IR_APPROVAL WHERE IRA_IR_INDEX IN (" & strAllIRIndex & ") "
            'strSql = "SELECT POM_PO_INDEX,POM_PO_NO,POM_S_COY_NAME,POM_S_EMAIL,POM_PO_STATUS,POM_STATUS_CHANGED_BY,POM_BUYER_ID, POM_S_COY_ID FROM PO_MSTR WHERE POM_PO_NO IN (" & strAllPO & ") And POM_B_COY_ID='" & strCoyID & "'"
            'strSql1 = "SELECT * FROM PR_APPROVAL WHERE PRA_PR_INDEX IN (" & strAllPOIndex & ") AND PRA_FOR = 'PO' "

            ds = objDb.FillDs(strSql & ";" & strSql1)
            ds.Tables(0).TableName = "INVENTORY_REQUISITION_MSTR"
            ds.Tables(1).TableName = "IR_APPROVAL"

            If Not ds Is Nothing Then
                Dim parentCol As DataColumn
                Dim childCol As DataColumn
                Dim dvChildView, dvParentView As DataView
                Dim IRrow, IRApprrow As DataRow
                Dim intCurrentSeq, intLastSeq As Integer
                Dim blnHighestLevel, blnCanApprove As Boolean
                Dim strActiveAO As String

                parentCol = ds.Tables("INVENTORY_REQUISITION_MSTR").Columns("IRM_IR_INDEX")
                childCol = ds.Tables("IR_APPROVAL").Columns("IRA_IR_INDEX")

                ' Create DataRelation.
                Dim relIR As DataRelation
                relIR = New DataRelation("acct", parentCol, childCol)
                ' Add the relation to the DataSet.
                ds.Relations.Add(relIR)
                For Each IRrow In ds.Tables("INVENTORY_REQUISITION_MSTR").Rows
                    blnCanApprove = True
                    If IRrow("IRM_IR_STATUS") = IRStatus_new.PendingApproval Or IRrow("IRM_IR_STATUS") = IRStatus_new.Submitted Then
                        dsDetail = objDb.FillDs("SELECT * FROM INVENTORY_REQUISITION_DETAILS IRD, INVENTORY_MSTR IM " &
                                                "WHERE IRD.IRD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX AND IRD_IR_COY_ID ='" & IRrow("IRM_IR_COY_ID") & "' AND IRD_IR_NO='" & IRrow("IRM_IR_NO") & "'")
                        'dsDetail = objDb.FillDs("SELECT * FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_COY_ID ='" & IRrow("IRM_IR_COY_ID") & "' AND IRD_IR_NO='" & IRrow("IRM_IR_NO") & "'")

                        Dim dsIRD As New DataSet
                        Dim dtDetails As New DataTable
                        dtDetails.Columns.Add("IRNo", Type.GetType("System.String"))
                        dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
                        dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))
                        dtDetails.Columns.Add("MonthlyStockIssued", Type.GetType("System.String"))
                        dtDetails.Columns.Add("Last3MthAve", Type.GetType("System.String"))

                        Dim dtrd As DataRow
                        For i = 0 To dsDetail.Tables(0).Rows.Count - 1

                            dblMthStkIssed = getMonthStockBalance(dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE"))
                            dblLast3MthAve = getLast3MthAve(dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE"), IRrow("IRM_IR_DEPARTMENT"))
                            dblStkBal = getStockBalance(dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE"))
                            dblReqQty = CDec(dsDetail.Tables(0).Rows(i)("IRD_QTY"))

                            If dblReqQty > dblLast3MthAve Then
                                strMsg = IRrow("IRM_IR_NO") & " - Request qty is more than last 3 months average usage. Please enter Remarks."
                                Common.Insert2Ary(strReturnMsg, strMsg)
                                blnCanApprove = False
                                Exit For
                            End If

                            If dblMthStkIssed > dblLast3MthAve Then
                                strMsg = IRrow("IRM_IR_NO") & " - Monthly stock issued accumulative is more than last 3 months average usage. Please enter Remarks."
                                Common.Insert2Ary(strReturnMsg, strMsg)
                                blnCanApprove = False
                                Exit For
                            End If

                            If dblReqQty > dblStkBal Then
                                strMsg = IRrow("IRM_IR_COY_ID") & " - Quantity requested cannot be more than stock balance quantity."
                                Common.Insert2Ary(strReturnMsg, strMsg)
                                blnCanApprove = False
                                Exit For
                            End If

                            dtrd = dtDetails.NewRow()
                            dtrd("IRNo") = IRrow("IRM_IR_NO")
                            dtrd("Line") = dsDetail.Tables(0).Rows(i)("IRD_IR_LINE")
                            dtrd("ItemCode") = dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE")
                            dtrd("MonthlyStockIssued") = dblMthStkIssed
                            dtrd("Last3MthAve") = dblLast3MthAve
                            dtDetails.Rows.Add(dtrd)
                        Next

                        dsIRD.Tables.Add(dtDetails)

                        For Each IRApprrow In IRrow.GetChildRows(relIR)
                            dsTemp = GetInvReqApprList(IRrow("IRM_IR_NO"), "", "", "", "", "")
                            If dsTemp.Tables(0).Rows.Count = 0 Then
                                strMsg = "You have already approved IR No. " & IRrow("IRM_IR_NO") & ". Approving of this IR is not allowed."
                                Common.Insert2Ary(strReturnMsg, strMsg)
                                blnCanApprove = False
                                Exit For
                            End If
                            intLastSeq = IRApprrow("IRA_AO_Action")
                            intCurrentSeq = intLastSeq + 1

                            'check whether the PO was already approved by the same AO at the
                            'same approving level.
                            If IRApprrow("IRA_Seq") = intCurrentSeq Then
                                strApprType = IRApprrow("IRA_APPROVAL_TYPE")
                                strActiveAO = Common.parseNull(IRApprrow("IRA_ACTIVE_AO"))
                                If strActiveAO <> "" Then
                                    strMsg = "You have already approved IR No. " & IRrow("IRM_IR_NO") & ". Approving of this IR is not allowed."
                                    Common.Insert2Ary(strReturnMsg, strMsg)
                                    blnCanApprove = False
                                    Exit For
                                Else
                                    If Not (UCase(IRApprrow("IRA_AO")) = UCase(strAO) Or
                                     UCase(Common.parseNull(IRApprrow("IRA_A_AO"))) = UCase(strAO)) Then
                                        strMsg = "You are not a authorised person to approve IR No. " & IRrow("IRM_IR_NO")
                                        Common.Insert2Ary(strReturnMsg, strMsg)
                                        blnCanApprove = False
                                        Exit For
                                    End If
                                End If
                            End If

                            If intCurrentSeq = IRApprrow("IRA_SEQ") Then
                                blnHighestLevel = True
                            Else
                                blnHighestLevel = False
                            End If
                        Next
                        If blnCanApprove Then
                            strMsg = ApproveIR(strLoginUser, strCoyID, IRrow("IRM_IR_NO"), IRrow("IRM_IR_INDEX"), intCurrentSeq, blnHighestLevel, "Approved : ", blnRelief, strApprType, dsIRD)
                            Common.Insert2Ary(strReturnMsg, strMsg)
                        End If
                    Else
                        'IR Approved/Rejected
                        If IRrow("IRM_IR_STATUS") = IRStatus_new.Rejected Then
                            strMsg = "You have already rejected IR No. " & IRrow("IRM_IR_NO") & ". Approving of this IR is not allowed. "
                        ElseIf IRrow("IRM_IR_STATUS") = IRStatus_new.Approved Then
                            strMsg = "You have already approved IR No. " & IRrow("IRM_IR_NO") & ". Approving of this IR is not allowed."
                        End If
                        Common.Insert2Ary(strReturnMsg, strMsg)
                    End If
                Next
            End If
            Return True
        End Function

        Function ApproveIR(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strIRNo As String, ByVal intIRIndex As Long, ByVal intCurrentSeq As Integer, ByVal blnHighestLevel As Boolean,
        ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal strApprType As String, Optional ByVal dsIRD As DataSet = Nothing) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strMsg, strVendor As String
            Dim intIRStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname As String
            Dim strMsg1 As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim aryMRSNo As New ArrayList()
            Dim strSqlLast As String
            Dim i, j As Integer

            If strApprType = "1" Then
                strApprType = "approved"
            End If

            strSql = "SELECT IRM_IR_STATUS FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX = " & intIRIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intIRStatus = tDS.Tables(0).Rows(0).Item("IRM_IR_STATUS")
                If intIRStatus = IRStatus_new.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved this IR."
                ElseIf intIRStatus = IRStatus_new.Rejected Then
                    strMsg = "You have already rejected this IR."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            If Not dsIRD Is Nothing Then
                For i = 0 To dsIRD.Tables(0).Rows.Count - 1
                    strSql = " UPDATE INVENTORY_REQUISITION_DETAILS " &
                            " SET IRD_IR_MTHISSUE = " & Common.Parse(dsIRD.Tables(0).Rows(i)("MonthlyStockIssued")) & "," &
                            " IRD_IR_LAST3MTH = " & Common.Parse(dsIRD.Tables(0).Rows(i)("Last3MthAve")) &
                            " WHERE IRD_IR_COY_ID = '" & strCoyID & "' " &
                            " AND IRD_IR_NO = '" & dsIRD.Tables(0).Rows(i)("IRNo") & "' " &
                            " AND IRD_IR_LINE = '" & Common.Parse(dsIRD.Tables(0).Rows(i)("Line")) & "' "
                    Common.Insert2Ary(strSqlAry, strSql)
                Next
            End If

            If blnHighestLevel Then
                ''Update Qty to Inventory detail
                Dim dsInvReq As New DataSet
                Dim blnFound As Boolean

                Dim strNewMRSNo, strMRS, IRSM_IRS_INDEX As String
                strSql = "SELECT PM.PM_ACCT_CODE FROM INVENTORY_REQUISITION_MSTR IRM, INVENTORY_REQUISITION_DETAILS IRD, INVENTORY_MSTR IM " &
                        "LEFT JOIN PRODUCT_MSTR PM ON IM.IM_ITEM_CODE = PM.PM_VENDOR_ITEM_CODE AND IM.IM_COY_ID = PM.PM_S_COY_ID " &
                        "WHERE IRM.IRM_IR_COY_ID = IRD.IRD_IR_COY_ID And IRM.IRM_IR_NO = IRD.IRD_IR_NO " &
                        "AND IRD.IRD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                        "AND IRM_IR_INDEX = " & intIRIndex & " GROUP BY PM_ACCT_CODE ORDER BY PM_ACCT_CODE "

                dsInvReq = objDb.FillDs(strSql)
                'If dsInvReq.Tables(0).Rows.Count > 0 Then
                '    For i = 0 To dsInvReq.Tables(0).Rows.Count - 1
                '        If aryTemp Is Nothing Then
                '            aryTemp.Add(dsInvReq.Tables(0).Rows(i)("PM_ACCT_CODE"))
                '        Else
                '            For j = 0 To aryTemp.Count - 1
                '                If dsInvReq.Tables(0).Rows(i)("PM_ACCT_CODE") = aryTemp(j) Then
                '                    blnFound = True
                '                    Exit For
                '                Else
                '                    blnFound = False
                '                End If
                '            Next

                '            If blnFound = False Then
                '                aryTemp.Add(dsInvReq.Tables(0).Rows(i)("PM_ACCT_CODE"))
                '            End If
                '        End If
                '    Next
                'End If

                For i = 0 To dsInvReq.Tables(0).Rows.Count - 1
                    'Split IR No to MRS No (eg. IR000001 -> IR000001-1)
                    strNewMRSNo = strIRNo & "-" & i + 1
                    aryMRSNo.Add(strNewMRSNo)

                    'Store record into INVENTORY_REQUISITION_SLIP_MSTR
                    strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_MSTR(IRSM_IRS_COY_ID, IRSM_IRS_NO, IRSM_IRS_DATE, IRSM_IRS_REF_NO, IRSM_IRS_REQUESTOR_NAME, IRSM_IRS_ISSUE_TO, " &
                            "IRSM_IRS_DEPARTMENT, IRSM_IRS_SECTION, IRSM_IRS_REMARK, IRSM_IRS_STATUS, IRSM_STATUS_CHANGED_BY, IRSM_STATUS_CHANGED_ON, IRSM_IRS_URGENT, IRSM_IRS_ACCOUNT_CODE, IRSM_CREATED_BY, IRSM_CREATED_DATE) " &
                            "SELECT IRM_IR_COY_ID, '" & strNewMRSNo & "', " & Common.ConvertDate(Now) & ", IRM_IR_REF_NO, IRM_IR_REQUESTOR_NAME, IRM_IR_ISSUE_TO, " &
                            "IRM_IR_DEPARTMENT, IRM_IR_SECTION, IRM_IR_REMARK, " & MRSStatus_new.NewMRS & ", '" & strLoginUser & "', " & Common.ConvertDate(Now) & ", IRM_IR_URGENT, '" & Common.Parse(dsInvReq.Tables(0).Rows(i)("PM_ACCT_CODE")) & "', '" & strLoginUser & "', " & Common.ConvertDate(Now) &
                            " FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_COY_ID='" & strCoyID & "' AND IRM_IR_NO ='" & strIRNo & "' "
                    Common.Insert2Ary(strSqlAry, strSql)

                    'Store record into INVENTORY_REQUISITION_SLIP_DETAILS
                    strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_DETAILS(IRSD_IRS_COY_ID, IRSD_IRS_NO, IRSD_IRS_LINE, IRSD_INVENTORY_INDEX, IRSD_INVENTORY_NAME, IRSD_IRS_STATUS, IRSD_QTY, IRSD_UOM) " &
                            "SELECT IRD_IR_COY_ID, '" & strNewMRSNo & "', IRD_IR_LINE, IRD_INVENTORY_INDEX, IRD_INVENTORY_NAME, " & MRSStatus_new.NewMRS & ", IRD_QTY, IRD_UOM " &
                            "FROM INVENTORY_REQUISITION_DETAILS, INVENTORY_MSTR, PRODUCT_MSTR WHERE IRD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                            "AND IM_COY_ID = PM_S_COY_ID AND IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IRD_IR_NO = '" & strIRNo & "' " &
                            "AND IRD_IR_COY_ID = '" & strCoyID & "' AND PM_ACCT_CODE = '" & Common.Parse(dsInvReq.Tables(0).Rows(i)("PM_ACCT_CODE")) & "' "
                    Common.Insert2Ary(strSqlAry, strSql)

                    'Update MRS index into INVENTORY_REQUISITION_DETAILS
                    IRSM_IRS_INDEX = " (SELECT DISTINCT IRSM_IRS_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID ='" & strCoyID & "' AND IRSM_IRS_NO = '" & strNewMRSNo & "') "
                    strSql = "UPDATE INVENTORY_REQUISITION_DETAILS " &
                            "INNER JOIN INVENTORY_MSTR ON IRD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                            "INNER JOIN PRODUCT_MSTR ON IM_ITEM_CODE = PM_VENDOR_ITEM_CODE AND IM_COY_ID = PM_S_COY_ID " &
                            "SET IRD_IR_SLIP_INDEX = " & IRSM_IRS_INDEX & " WHERE IRD_IR_COY_ID = '" & strCoyID & "' " &
                            "AND IRD_IR_NO = '" & strIRNo & "' AND PM_ACCT_CODE = '" & Common.Parse(dsInvReq.Tables(0).Rows(i)("PM_ACCT_CODE")) & "'"
                    Common.Insert2Ary(strSqlAry, strSql)

                    'strSql = "UPDATE INVENTORY_REQUISITION_DETAILS SET IRD_IR_SLIP_INDEX = " & IRSM_IRS_INDEX & "WHERE IRD_IR_COY_ID = '" & strCoyID & "' " & _
                    '        "AND IRD_IR_NO = '" & strIRNo & "' "
                    'Common.Insert2Ary(strSqlAry, strSql)
                Next

                'Update IR status to Approved
                strSql = "UPDATE INVENTORY_REQUISITION_MSTR SET IRM_IR_APPROVED_DATE= " & Common.ConvertDate(Now) & ", IRM_IR_STATUS=" & IRStatus_new.Approved &
                        ", IRM_STATUS_CHANGED_BY = '" & strLoginUser & "', IRM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                        " WHERE IRM_IR_INDEX=" & intIRIndex
                Common.Insert2Ary(strSqlAry, strSql)

                updateAOAction(intIRIndex, intCurrentSeq, strAORemark, strSqlAry, "IR")

                'Final approve IR and generate new MRS No.
                Dim objUsers As New Users
                For i = 0 To aryMRSNo.Count - 1
                    If aryMRSNo.Count = 1 Then
                        strMsg = "MRS No. " & aryMRSNo(i) & " has been successfully created for " & strIRNo & ". "
                        objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.IRMod, WheelUserActivity.AO_ApproveIR, aryMRSNo(i), strIRNo)
                    Else
                        If i = 0 Then
                            strMRS = aryMRSNo(i)
                        Else
                            strMRS &= ", " & aryMRSNo(i)
                        End If

                        strMsg = "MRS No. " & strMRS & " has been successfully created for " & strIRNo & ". "
                        objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.IRMod, WheelUserActivity.AO_ApproveIR, aryMRSNo(i), strIRNo)
                    End If
                Next
                objUsers = Nothing
            Else
                'Update IR status to Pending Approval
                strSql = "UPDATE INVENTORY_REQUISITION_MSTR SET IRM_IR_STATUS=" & IRStatus_new.PendingApproval &
                ", IRM_STATUS_CHANGED_BY = '" & strLoginUser & "', IRM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                " WHERE IRM_IR_INDEX=" & intIRIndex
                Common.Insert2Ary(strSqlAry, strSql)

                updateAOAction(intIRIndex, intCurrentSeq, strAORemark, strSqlAry, "IR")
                strMsg = "IR No. " & strIRNo & " has been " & strApprType & ". "

                Dim objUsers As New Users
                objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.IRMod, WheelUserActivity.AO_ApproveIR, strIRNo)
                objUsers = Nothing
            End If

            'Dim objUsers As New Users
            'objUsers.Log_UserActivity(strSqlAry, WheelModule.IRMod, WheelUserActivity.AO_ApproveIR, strIRNo)
            'objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                Dim objMail As New Email
                Dim strUrgent As String
                If blnHighestLevel Then
                    For i = 0 To aryMRSNo.Count - 1
                        strUrgent = objDb.GetVal("SELECT IRSM_IRS_URGENT FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_NO = '" & aryMRSNo(i) & "'")
                        objMail.sendNotification(EmailType.MRSToSK, strLoginUser, strCoyID, "", aryMRSNo(i), "", strUrgent)
                    Next
                Else
                    'Send Email to next AO
                    sendMailToSKForIR(strIRNo, intIRIndex, intCurrentSeq + 1, "N", "Approved")
                End If
            End If

            Return strMsg
        End Function

        Function RejectIR(ByVal strUserID As String, ByVal strCoyID As String, ByVal strIRNo As String, ByVal intIRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal blnRelief As Boolean, Optional ByVal dsIRD As DataSet = Nothing) As String
            Dim strSql, strSqlAry(0) As String
            Dim intIRStatus, intCount, i As Integer
            Dim strMsg, int As String
            Dim strNewIRNo, strRequestorId As String
            Dim intNewIRIndex As Long

            strSql = "SELECT IRM_IR_STATUS FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX = " & intIRIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intIRStatus = tDS.Tables(0).Rows(0).Item("IRM_IR_STATUS")
                If intIRStatus = IRStatus_new.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved this IR."
                ElseIf intIRStatus = IRStatus_new.Rejected Then
                    strMsg = "You have already rejected this IR."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            If Not dsIRD Is Nothing Then
                For i = 0 To dsIRD.Tables(0).Rows.Count - 1
                    strSql = " UPDATE INVENTORY_REQUISITION_DETAILS " &
                            " SET IRD_IR_MTHISSUE = " & Common.Parse(dsIRD.Tables(0).Rows(i)("MonthlyStockIssued")) & "," &
                            " IRD_IR_LAST3MTH = " & Common.Parse(dsIRD.Tables(0).Rows(i)("Last3MthAve")) &
                            " WHERE IRD_IR_COY_ID = '" & Common.Parse(strCoyID) & "' " &
                            " AND IRD_IR_NO = '" & Common.Parse(dsIRD.Tables(0).Rows(i)("IRNo")) & "' " &
                            " AND IRD_IR_LINE = '" & Common.Parse(dsIRD.Tables(0).Rows(i)("Line")) & "' "
                    Common.Insert2Ary(strSqlAry, strSql)
                Next
            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strUserID, strCoyID, strSqlAry, WheelModule.IRMod, WheelUserActivity.AO_RejectIR, strIRNo)
            objUsers = Nothing

            strRequestorId = objDb.GetVal("SELECT IRM_CREATED_BY FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX =" & intIRIndex)

            'Update IR status to Rejected
            strSql = "UPDATE INVENTORY_REQUISITION_MSTR SET IRM_IR_STATUS=" & IRStatus_new.Rejected &
                ", IRM_STATUS_CHANGED_BY = '" & strUserID & "', IRM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                " WHERE IRM_IR_INDEX=" & intIRIndex
            Common.Insert2Ary(strSqlAry, strSql)

            updateAOAction(intIRIndex, intCurrentSeq, strAORemark, strSqlAry, "IR")

            If objDb.BatchExecute(strSqlAry) Then
                Dim objMail As New Email

                strMsg = "IR No. " & strIRNo & " has been rejected."
                objMail.sendNotification(EmailType.IRRejectedToRequestor, strRequestorId, strCoyID, "", strIRNo, "", HttpContext.Current.Session("UserName"))
                'objMail.sendNotification(EmailType.IQCRejectedToSK, strSKId, strCoyID, "", strIQCNo, "", "", "FM")
                'objMail.sendNotification(EmailType.IQCRejectedToSK, strSKId, strCoyID, "", strIQCNo, "", "", "AO")

                objMail = Nothing
            Else
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            End If
            Return strMsg

        End Function

        Function getIRForAppr(ByVal strIRNo As String) As DataSet
            Dim strSql, strSqlIRM, strSqlIRD, strCoyID As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")

            strSqlIRM = "SELECT * FROM INVENTORY_REQUISITION_MSTR " &
                        "LEFT JOIN COMPANY_SECTION ON IRM_IR_SECTION = CS_SEC_CODE AND IRM_IR_COY_ID = CS_COY_ID " &
                        "WHERE IRM_IR_COY_ID = '" & strCoyID & "' AND IRM_IR_NO = '" & strIRNo & "' "

            strSqlIRD = "SELECT IM.IM_ITEM_CODE, IRD.* " &
                        "FROM INVENTORY_REQUISITION_DETAILS IRD, INVENTORY_MSTR IM " &
                        "WHERE IRD.IRD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                        "AND IRD_IR_NO = '" & strIRNo & "' AND IRD_IR_COY_ID = '" & strCoyID & "'"

            strSql = strSqlIRM & ";" & strSqlIRD
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "INVENTORY_REQUISITION_MSTR"
            ds.Tables(1).TableName = "INVENTORY_REQUISITION_DETAILS"
            Return ds
        End Function

        Function getMRSForAppr(ByVal strMRSNo As String) As DataSet
            Dim strSql, strSqlIRSM, strSqlIRSD, strSqlAttach, strCoyID As String
            Dim ds, ds1 As DataSet
            strCoyID = HttpContext.Current.Session("CompanyId")

            strSqlIRSM = "SELECT INVENTORY_REQUISITION_SLIP_MSTR.*, CDM_DEPT_NAME, IRM_CREATED_BY, IRM_IR_INDEX, CS_SEC_NAME " &
                        "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                        "INNER JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                        "INNER JOIN INVENTORY_REQUISITION_DETAILS ON IRSM_IRS_INDEX = IRD_IR_SLIP_INDEX " &
                        "INNER JOIN INVENTORY_REQUISITION_MSTR ON IRD_IR_COY_ID = IRM_IR_COY_ID AND IRD_IR_NO = IRM_IR_NO " &
                        "LEFT JOIN COMPANY_SECTION ON IRSM_IRS_SECTION = CS_SEC_CODE AND IRSM_IRS_COY_ID = CS_COY_ID " &
                        "WHERE IRSM_IRS_COY_ID = '" & strCoyID & "' AND IRSM_IRS_NO = '" & strMRSNo & "'"

            strSqlIRSD = "SELECT IM.IM_ITEM_CODE, IRSD.* " &
                        "FROM INVENTORY_REQUISITION_SLIP_DETAILS IRSD, INVENTORY_MSTR IM " &
                        "WHERE IRSD.IRSD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX " &
                        "AND IRSD_IRS_NO = '" & strMRSNo & "' AND IRSD_IRS_COY_ID = '" & strCoyID & "'"

            strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strMRSNo & "' AND CDA_DOC_TYPE='MRS'"

            strSql = strSqlIRSM & ";" & strSqlIRSD & ";" & strSqlAttach
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "INVENTORY_REQUISITION_SLIP_MSTR"
            ds.Tables(1).TableName = "INVENTORY_REQUISITION_SLIP_DETAILS"
            ds.Tables(2).TableName = "COMPANY_DOC_ATTACHMENT"
            Return ds
        End Function

        Public Function GetMRSApprList(ByVal strMRSNo As String, ByVal strAccCode As String, ByVal strReqName As String, ByVal strIssueTo As String, ByVal strDept As String, ByVal strDateS As String, ByVal strDateE As String) As DataSet
            Dim strsql, strTemp, strCondition, strCondition1, strReliefOn As String
            Dim ds As New DataSet
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT IRSM.*, CDM.CDM_DEPT_NAME FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM WHERE " &
                    "IRSM_IRS_COY_ID = CDM_COY_ID And IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                    "AND IRSM_IRS_COY_ID = '" & strCoyId & "' AND IRSM_IRS_STATUS = '1' "

            If strMRSNo <> "" Then
                strTemp = Common.BuildWildCard(strMRSNo)
                strsql = strsql & " AND IRSM_IRS_NO " & Common.ParseSQL(strTemp) & " "
            End If

            If strAccCode <> "" Then
                strTemp = Common.BuildWildCard(strAccCode)
                strsql = strsql & " AND IRSM_IRS_ACCOUNT_CODE " & Common.ParseSQL(strTemp) & " "
            End If

            If strReqName <> "" Then
                strTemp = Common.BuildWildCard(strReqName)
                strsql = strsql & " AND IRSM_IRS_REQUESTOR_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If strIssueTo <> "" Then
                strTemp = Common.BuildWildCard(strIssueTo)
                strsql = strsql & " AND IRSM_IRS_ISSUE_TO " & Common.ParseSQL(strTemp) & " "
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strsql = strsql & " AND CDM_DEPT_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If strDateS <> "" Then
                strsql &= " AND IRSM_IRS_DATE >= " & Common.ConvertDate(strDateS) & " "
            End If

            If strDateE <> "" Then
                strsql &= " AND IRSM_IRS_DATE <= " & Common.ConvertDate(strDateE & " 23:59:59") & " "
            End If

            ds = objDb.FillDs(strsql)
            GetMRSApprList = ds
        End Function

        Public Function GetMRSList(ByVal strMRSNo As String, ByVal strAccCode As String, ByVal strIssueTo As String, ByVal strDept As String, ByVal strDateS As String, ByVal strDateE As String, ByVal strIssDateS As String, ByVal strIssDateE As String, ByVal aryRemark As ArrayList, Optional ByVal strAction As String = "All", Optional ByVal strInclude As String = "Y") As DataSet
            Dim strsql, strTemp As String
            Dim ds As New DataSet
            Dim i As Integer
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT tb.*, "

            If strAction = "All" Then
                strsql &= "SM.STATUS_DESC, "
            ElseIf strAction = "Ind" Then
                strsql &= "CASE WHEN SM.STATUS_DESC <> 'Rejected' THEN 'Issued' ELSE 'Rejected' END AS STATUS_DESC, "
            End If

            strsql &= "(SELECT DISTINCT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX) AS IRD_IR_NO FROM " &
                    "(SELECT IRSM.*, CDM.CDM_DEPT_NAME FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM WHERE " &
                    "IRSM_IRS_COY_ID = CDM_COY_ID And IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                    "AND IRSM_IRS_COY_ID = '" & strCoyId & "' "

            If strAction = "All" Then
                strsql &= " AND (IRSM_IRS_STATUS <> '6' AND IRSM_IRS_STATUS <> '1') "
            ElseIf strAction = "Ind" Then
                strsql &= " AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '3' OR IRSM_IRS_STATUS = '4' OR IRSM_IRS_STATUS = '5' OR IRSM_IRS_STATUS = '7') AND IRSM_BUYER_ID = '" & strUser & "' "
            End If

            If strIssDateS <> "" Then
                strsql &= " AND IRSM_IRS_APPROVED_DATE >= " & Common.ConvertDate(strIssDateS) & " "
            End If

            If strIssDateE <> "" Then
                strsql &= " AND IRSM_IRS_APPROVED_DATE <= " & Common.ConvertDate(strIssDateE & " 23:59:59") & " "
            End If

            strsql &= "UNION ALL " &
                    "SELECT IRSM.*, CDM.CDM_DEPT_NAME FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM WHERE " &
                    "IRSM_IRS_COY_ID = CDM_COY_ID And IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                    "AND IRSM_IRS_COY_ID = '" & strCoyId & "' "

            If strAction = "All" Then
                strsql &= " AND (IRSM_IRS_STATUS = '1' OR IRSM_IRS_STATUS = '6') "
            ElseIf strAction = "Ind" Then
                If strInclude = "N" Then
                    strsql &= " AND IRSM_IRS_STATUS = '6' AND IRSM_BUYER_ID = '" & strUser & "' "
                ElseIf strInclude = "Y" Then
                    strsql &= " AND IRSM_IRS_STATUS = '6' "
                End If
            End If

            strsql &= ") tb, STATUS_MSTR SM " &
                    "WHERE tb.IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' "

            If strMRSNo <> "" Then
                strTemp = Common.BuildWildCard(strMRSNo)
                strsql = strsql & " AND IRSM_IRS_NO " & Common.ParseSQL(strTemp) & " "
            End If

            If strAccCode <> "" Then
                strTemp = Common.BuildWildCard(strAccCode)
                strsql = strsql & " AND IRSM_IRS_ACCOUNT_CODE " & Common.ParseSQL(strTemp) & " "
            End If

            If strIssueTo <> "" Then
                strTemp = Common.BuildWildCard(strIssueTo)
                strsql = strsql & " AND IRSM_IRS_ISSUE_TO " & Common.ParseSQL(strTemp) & " "
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strsql = strsql & " AND CDM_DEPT_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If strDateS <> "" Then
                strsql &= " AND IRSM_IRS_DATE >= " & Common.ConvertDate(strDateS) & " "
            End If

            If strDateE <> "" Then
                strsql &= " AND IRSM_IRS_DATE <= " & Common.ConvertDate(strDateE & " 23:59:59") & " "
            End If

            If Not aryRemark Is Nothing Then
                If aryRemark.Count > 0 Then
                    For i = 0 To aryRemark.Count - 1
                        If i = 0 Then
                            strTemp = "STATUS_DESC = '" & aryRemark(i) & "' "
                        Else
                            strTemp &= "OR STATUS_DESC = '" & aryRemark(i) & "' "
                        End If

                    Next

                    strTemp = "AND (" & strTemp & ") "
                    strsql &= strTemp
                End If
            End If

            strsql &= "ORDER BY IRSM_IRS_INDEX "

            ds = objDb.FillDs(strsql)
            GetMRSList = ds
        End Function

        Public Function GetReqMRSList(ByVal strMRSNo As String, ByVal strAccCode As String, ByVal strIssueTo As String, ByVal strDept As String, ByVal strDateS As String, ByVal strDateE As String, ByVal strIssDateS As String, ByVal strIssDateE As String, Optional ByVal aryRemark As ArrayList = Nothing, Optional ByVal strAction As String = "All") As DataSet
            Dim strsql, strTemp As String
            Dim ds As New DataSet
            Dim i As Integer
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT tb.*, SM.STATUS_DESC, INVENTORY_REQUISITION_MSTR.* " &
                    "FROM (SELECT IRSM.*, CDM.CDM_DEPT_NAME, (SELECT DISTINCT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX) AS IRD_IR_NO " &
                    "FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM " &
                    "WHERE IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE AND IRSM_IRS_COY_ID = '" & strCoyId & "' "

            If strAction = "All" Then
                strsql &= "AND (IRSM_IRS_STATUS <> '6' AND IRSM_IRS_STATUS <> '1') "
            ElseIf strAction = "Ack" Then
                strsql &= "AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '7') "
            End If

            strsql &= "AND IRSM_IRS_APPROVED_DATE >= " & Common.ConvertDate(strIssDateS) & " AND IRSM_IRS_APPROVED_DATE <= " & Common.ConvertDate(strIssDateE & " 23:59:59") & " " &
                    "UNION " &
                    "SELECT IRSM.*, CDM.CDM_DEPT_NAME, (SELECT DISTINCT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX) AS IRD_IR_NO " &
                    "FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM " &
                    "WHERE IRSM_IRS_COY_ID = CDM_COY_ID And IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE AND IRSM_IRS_COY_ID = '" & strCoyId & "' "

            If strAction = "All" Then
                strsql &= "AND (IRSM_IRS_STATUS = '1' OR IRSM_IRS_STATUS = '6') "
            ElseIf strAction = "Ack" Then
                strsql &= "AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '7') "
            End If

            strsql &= " ) tb " &
                    "INNER JOIN STATUS_MSTR SM ON tb.IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' " &
                    "INNER JOIN INVENTORY_REQUISITION_MSTR ON tb.IRD_IR_NO = IRM_IR_NO AND IRSM_IRS_COY_ID = IRM_IR_COY_ID " &
                    "WHERE IRM_CREATED_BY = '" & strUser & "' "

            If strMRSNo <> "" Then
                strTemp = Common.BuildWildCard(strMRSNo)
                strsql = strsql & " AND IRSM_IRS_NO " & Common.ParseSQL(strTemp) & " "
            End If

            If strAccCode <> "" Then
                strTemp = Common.BuildWildCard(strAccCode)
                strsql = strsql & " AND IRSM_IRS_ACCOUNT_CODE " & Common.ParseSQL(strTemp) & " "
            End If

            If strIssueTo <> "" Then
                strTemp = Common.BuildWildCard(strIssueTo)
                strsql = strsql & " AND IRSM_IRS_ISSUE_TO " & Common.ParseSQL(strTemp) & " "
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strsql = strsql & " AND CDM_DEPT_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            If strDateS <> "" Then
                strsql &= " AND IRSM_IRS_DATE >= " & Common.ConvertDate(strDateS) & " "
            End If

            If strDateE <> "" Then
                strsql &= " AND IRSM_IRS_DATE <= " & Common.ConvertDate(strDateE & " 23:59:59") & " "
            End If

            If Not aryRemark Is Nothing Then
                If aryRemark.Count > 0 Then
                    For i = 0 To aryRemark.Count - 1
                        If i = 0 Then
                            strTemp = "STATUS_DESC = '" & aryRemark(i) & "' "
                        Else
                            strTemp &= "OR STATUS_DESC = '" & aryRemark(i) & "' "
                        End If

                    Next

                    strTemp = "AND (" & strTemp & ") "
                    strsql &= strTemp
                End If
            End If

            strsql &= " ORDER BY IRSM_IRS_INDEX "

            ds = objDb.FillDs(strsql)
            GetReqMRSList = ds
        End Function

        Public Function GetAckMRSList(ByVal strMRSNo As String, ByVal strAccCode As String, ByVal strIssueTo As String, ByVal strDept As String, ByVal strDateS As String, ByVal strDateE As String, ByVal strIssDateS As String, ByVal strIssDateE As String) As DataSet
            Dim strsql, strTemp As String
            Dim ds As New DataSet
            Dim i As Integer
            Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT tb.*, SM.STATUS_DESC, INVENTORY_REQUISITION_MSTR.* " &
                    "FROM (SELECT IRSM.*, CDM.CDM_DEPT_NAME, (SELECT DISTINCT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX) AS IRD_IR_NO " &
                    "FROM INVENTORY_REQUISITION_SLIP_MSTR IRSM, COMPANY_DEPT_MSTR CDM " &
                    "WHERE IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE AND IRSM_IRS_COY_ID = '" & strCoyId & "' " &
                    "AND (IRSM_IRS_STATUS = '2' OR IRSM_IRS_STATUS = '7')) tb " &
                    "INNER JOIN STATUS_MSTR SM ON tb.IRSM_IRS_STATUS = STATUS_NO AND STATUS_TYPE = 'MRS' " &
                    "INNER JOIN INVENTORY_REQUISITION_MSTR ON tb.IRD_IR_NO = IRM_IR_NO AND IRSM_IRS_COY_ID = IRM_IR_COY_ID " &
                    "WHERE IRM_CREATED_BY = '" & strUser & "' "

            If strDateS <> "" Then
                strsql &= " AND IRSM_IRS_DATE >= " & Common.ConvertDate(strDateS) & " "
            End If

            If strDateE <> "" Then
                strsql &= " AND IRSM_IRS_DATE <= " & Common.ConvertDate(strDateE & " 23:59:59") & " "
            End If

            If strIssDateS <> "" Then
                strsql &= " AND IRSM_IRS_APPROVED_DATE >= " & Common.ConvertDate(strIssDateS) & " "
            End If

            If strIssDateE <> "" Then
                strsql &= " AND IRSM_IRS_APPROVED_DATE <= " & Common.ConvertDate(strIssDateE & " 23:59:59") & " "
            End If

            If strMRSNo <> "" Then
                strTemp = Common.BuildWildCard(strMRSNo)
                strsql = strsql & " AND IRSM_IRS_NO " & Common.ParseSQL(strTemp) & " "
            End If

            If strAccCode <> "" Then
                strTemp = Common.BuildWildCard(strAccCode)
                strsql = strsql & " AND IRSM_IRS_ACCOUNT_CODE " & Common.ParseSQL(strTemp) & " "
            End If

            If strIssueTo <> "" Then
                strTemp = Common.BuildWildCard(strIssueTo)
                strsql = strsql & " AND IRSM_IRS_ISSUE_TO " & Common.ParseSQL(strTemp) & " "
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strsql = strsql & " AND CDM_DEPT_NAME " & Common.ParseSQL(strTemp) & " "
            End If

            strsql &= " ORDER BY IRSM_IRS_INDEX "

            ds = objDb.FillDs(strsql)
            GetAckMRSList = ds
        End Function

        Function MassApprovalMRS(ByVal strLoginUser As String, ByVal strLoginUserName As String, ByVal strAryMRSIndex() As String, ByRef strReturnMsg() As String) As Boolean
            Dim strSql, strSql1, strAryQuery(0), strApprType As String
            Dim strCoyID, strAllIR, strAllMRSIndex As String
            Dim intLoop, i As Integer
            Dim ds As DataSet
            Dim strMsg As String
            Dim dsTemp As DataSet
            Dim dsDetail As DataSet
            Dim dblMthStkIssed, dblLast3MthAve, dblReqQty, dblStkBal, dblSafetyLvl, dblMaxInvQty As Decimal

            strCoyID = HttpContext.Current.Session("CompanyId")

            For intLoop = 0 To strAryMRSIndex.Length - 1
                If intLoop = 0 Then
                    strAllMRSIndex = strAryMRSIndex(intLoop)
                Else
                    strAllMRSIndex = strAllMRSIndex & "," & strAryMRSIndex(intLoop)
                End If
            Next

            strSql = "SELECT IRSM_IRS_INDEX, IRSM_IRS_NO, IRSM_IRS_COY_ID, IRSM_IRS_STATUS, IRSM_STATUS_CHANGED_BY, IRSM_IRS_DEPARTMENT FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_INDEX IN (" & strAllMRSIndex & ") "

            ds = objDb.FillDs(strSql)

            If Not ds Is Nothing Then
                Dim MRSrow, MRSApprrow As DataRow
                Dim intCurrentSeq, intLastSeq As Integer
                Dim blnHighestLevel, blnCanApprove As Boolean
                Dim strActiveAO As String

                blnCanApprove = True
                For Each MRSrow In ds.Tables(0).Rows
                    If MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.NewMRS Then
                        strSql = "SELECT * FROM INVENTORY_REQUISITION_SLIP_DETAILS IRSD, INVENTORY_MSTR IM " &
                                 "WHERE IRSD.IRSD_INVENTORY_INDEX = IM.IM_INVENTORY_INDEX AND IRSD_IRS_COY_ID ='" & MRSrow("IRSM_IRS_COY_ID") & "' " &
                                 "AND IRSD_IRS_NO='" & MRSrow("IRSM_IRS_NO") & "'"

                        dsDetail = objDb.FillDs(strSql)
                        For i = 0 To dsDetail.Tables(0).Rows.Count - 1
                            dblStkBal = getStockBalance(dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE"))
                            dblReqQty = CDec(dsDetail.Tables(0).Rows(i)("IRSD_QTY"))
                            dblSafetyLvl = objDb.GetVal("SELECT IFNULL(PM_SAFE_QTY,0) AS PM_SAFE_QTY FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " &
                                                        "AND PM_S_COY_ID = '" & dsDetail.Tables(0).Rows(i)("IM_COY_ID") & "'")
                            dblMaxInvQty = objDb.GetVal("SELECT IFNULL(PM_MAX_INV_QTY,0) AS PM_MAX_INV_QTY FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsDetail.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " &
                                                        "AND PM_S_COY_ID = '" & dsDetail.Tables(0).Rows(i)("IM_COY_ID") & "'")

                            'As Lee Ling requested on eMRS Testing(4) 2013/11/25 - System allow user enter 0 value for both field when add new item
                            'If dblSafetyLvl = 0 Then
                            '    strMsg = MRSrow("IRSM_IRS_NO") & " - Mass approval is not allowed due to Safety Level is equal to zero."
                            '    Common.Insert2Ary(strReturnMsg, strMsg)
                            '    blnCanApprove = False
                            '    Exit For
                            'End If

                            'If dblMaxInvQty = 0 Then
                            '    strMsg = MRSrow("IRSM_IRS_NO") & " - Mass approval is not allowed due to Maximum Inventory Qty is equal to zero."
                            '    Common.Insert2Ary(strReturnMsg, strMsg)
                            '    blnCanApprove = False
                            '    Exit For
                            'End If

                            If (dblStkBal - dblSafetyLvl) < dblReqQty Then
                                strMsg = MRSrow("IRSM_IRS_NO") & " - Requested Qty cannot be more than (stock balance - safety qty)."
                                Common.Insert2Ary(strReturnMsg, strMsg)
                                blnCanApprove = False
                                Exit For
                            End If

                        Next

                        If blnCanApprove Then
                            If ApproveMRS(strLoginUser, strCoyID, strLoginUserName, MRSrow("IRSM_IRS_NO"), MRSrow("IRSM_IRS_INDEX"), strMsg) Then
                                Common.Insert2Ary(strReturnMsg, strMsg)
                            Else
                                Common.Insert2Ary(strReturnMsg, strMsg)
                            End If
                        End If
                    Else
                        'IR Approved/Rejected
                        If MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.Issued Then
                            strMsg = "You have already issued MRS No. " & MRSrow("IRSM_IRS_NO") & ". Issuing of this MRS is not allowed. "
                        ElseIf MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.Rejected Then
                            strMsg = "You have already rejected MRS No. " & MRSrow("IRSM_IRS_NO") & ". Issuing of this MRS is not allowed."
                        ElseIf MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.Cancelled Then
                            strMsg = "You have already cancelled MRS No. " & MRSrow("IRSM_IRS_NO") & ". Issuing of this MRS is not allowed."
                        End If
                        Common.Insert2Ary(strReturnMsg, strMsg)
                    End If
                Next

            End If
            Return True
        End Function

        Function ApproveMRS(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strLoginUserName As String, ByVal strMRSNo As String, ByVal intMRSIndex As Long, ByRef strMsg As String, Optional ByVal strRemark As String = "", Optional ByVal aryDetail As ArrayList = Nothing) As Boolean
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strIQC, strIQC1, strVendor, strRequestorId, strMgr As String
            'Dim strPrevQty, strPrevUPrice, strPrevCost, strIssueQty, strIssueUPrice, strIssueCost, strCloseQty, strCloseUPrice, strCloseCost As String
            Dim intMRSStatus As Integer
            Dim INVENTORY_INDEX, INVENTORY_NAME, LOT_INDEX, LOCATION_INDEX, IRM_IR_INDEX, LM_LOCATION_INDEX As String
            Dim strMsg1 As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim dsGRNQTY As New DataSet
            Dim dsApp As New DataSet
            Dim strSqlLast As String
            Dim strStoreMgr() As String
            Dim i, j, k As Integer
            Dim blnFound As Boolean
            Dim dblReqQty, dblTotal, dblIssueQty, dblReceiveQty, dblDiffQty, dblQty, dblLast3MthAve, dblMthStkIssed, dblSafety As Decimal
            Dim strGRNNo As String
            Dim aryTrans As New ArrayList()

            strSql = "SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_DETAILS, INVENTORY_REQUISITION_MSTR WHERE " &
                    "IRD_IR_COY_ID = IRM_IR_COY_ID And IRD_IR_NO = IRM_IR_NO " &
                    "AND IRD_IR_SLIP_INDEX = " & intMRSIndex & " LIMIT 1 "
            IRM_IR_INDEX = objDb.GetVal(strSql)

            strSql = "SELECT IRM_CREATED_BY FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX = " & intMRSIndex
            strRequestorId = objDb.GetVal(strSql)

            strSql = "SELECT IRSM_IRS_STATUS, IRSM_IRS_REF_NO, IRSM_IRS_ISSUE_TO, IRSM_IRS_DEPARTMENT FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_INDEX = " & intMRSIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intMRSStatus = tDS.Tables(0).Rows(0).Item("IRSM_IRS_STATUS")
                If intMRSStatus = MRSStatus_new.Issued Then
                    strMsg = "You have already issued this MRS."
                ElseIf intMRSStatus = MRSStatus_new.Rejected Then
                    strMsg = "You have already rejected this MRS."
                End If
            End If

            If strMsg <> "" Then
                Return False
            End If

            strSql = "SELECT * FROM INVENTORY_REQUISITION_SLIP_DETAILS, " &
                    "INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_MSTR " &
                    "WHERE IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO AND IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "AND IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSD_IRS_NO = '" & strMRSNo & "'"
            Dim dsIRSD As DataSet = objDb.FillDs(strSql)

            For i = 0 To dsIRSD.Tables(0).Rows.Count - 1
                dblMthStkIssed = getMonthStockBalance(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE"))
                dblLast3MthAve = getLast3MthAve(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE"), dsIRSD.Tables(0).Rows(i)("IRSM_IRS_DEPARTMENT"))

                strSql = "UPDATE INVENTORY_REQUISITION_SLIP_DETAILS SET IRSD_IRS_MTHISSUE = " & dblMthStkIssed & ", IRSD_IRS_LAST3MTH = " & dblLast3MthAve & " " &
                        "WHERE IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSD_IRS_NO = '" & strMRSNo & "' AND IRSD_IRS_LINE = '" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "'"
                Common.Insert2Ary(strSqlAry, strSql)

                blnFound = False

                If Not aryDetail Is Nothing Then
                    For j = 0 To aryDetail.Count - 1
                        If dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") = aryDetail(j)(5) And dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE") = aryDetail(j)(4) And aryDetail(j)(0) <> "---Select---" And aryDetail(j)(1) <> "" Then
                            blnFound = True
                            Exit For
                        End If
                    Next
                End If

                If blnFound = True Then
                    dblReqQty = dsIRSD.Tables(0).Rows(i)("IRSD_QTY")
                    For j = 0 To aryDetail.Count - 1
                        If dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") = aryDetail(j)(5) And dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE") = aryDetail(j)(4) And aryDetail(j)(0) <> "---Select---" And aryDetail(j)(1) <> "" Then

                            If aryDetail(j)(3) = "" Then
                                strSql = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_LOCATION = '" & aryDetail(j)(2) & "' AND LM_SUB_LOCATION IS NULL AND LM_COY_ID = '" & strCoyID & "'"
                            Else
                                strSql = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_LOCATION = '" & aryDetail(j)(2) & "' AND LM_SUB_LOCATION = '" & aryDetail(j)(3) & "' AND LM_COY_ID = '" & strCoyID & "'"
                            End If

                            LM_LOCATION_INDEX = objDb.GetVal(strSql)

                            'strSql = "SELECT GL_GRNLOT_INDEX, GM_B_COY_ID, GL_GRN_NO, GL_PO_LINE, POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UNIT_COST, GL_LOCATION_INDEX, GL_LOT_INDEX, " & _
                            '        "GL_LOT_RECEIVED_QTY, GL_LOT_RECEIVED_QTY_ISSUE, (GL_LOT_RECEIVED_QTY - GL_LOT_RECEIVED_QTY_ISSUE) AS DIFF_QTY " & _
                            '        "FROM GRN_LOT GL, GRN_MSTR GM, PO_MSTR POM, PO_DETAILS POD " & _
                            '        "WHERE GL_B_COY_ID = GM_B_COY_ID And GL_GRN_NO = GM_GRN_NO " & _
                            '        "AND GM_PO_INDEX = POM_PO_INDEX AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                            '        "AND GL_PO_LINE = POD_PO_LINE AND GL_LOT_RECEIVED_QTY > GL_LOT_RECEIVED_QTY_ISSUE " & _
                            '        "AND POD_VENDOR_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " & _
                            '        "AND GL_LOT_INDEX = '" & aryDetail(j)(0) & "' AND GL_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' " & _
                            '        "AND GM_B_COY_ID = '" & strCoyID & "' " & _
                            '        "ORDER BY GM_CREATED_DATE, GL_GRNLOT_INDEX "

                            strSql = "SELECT NULL AS GRN_LOT_INDEX, 'beta' AS COY_ID, 'SYSTEM' AS GRN_NO, CAST(DOL_PO_LINE AS UNSIGNED) AS PO_LINE, " &
                                    "DOL_ITEM_CODE AS ITEM_CODE, (SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = DOL_ITEM_CODE AND IM_COY_ID = 'beta') AS ITEM_NAME, " &
                                    "DOL_OPN_LOCATION_INDEX AS LOC_INDEX, DOL_LOT_INDEX AS LOT_INDEX, DOL_LOT_QTY AS LOT_RECEIVED_QTY, " &
                                    "DOL_OPN_QTY_ISSUE AS LOT_ISSUED_QTY, (DOL_LOT_QTY - DOL_OPN_QTY_ISSUE) AS DIFF_QTY " &
                                    "FROM DO_LOT WHERE DOL_COY_ID ='SYSTEM' AND DOL_LOT_QTY > DOL_OPN_QTY_ISSUE " &
                                    "AND DOL_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' AND DOL_LOT_INDEX = '" & aryDetail(j)(0) & "' " &
                                    "AND DOL_OPN_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' " &
                                    "UNION ALL " &
                                    "SELECT * FROM " &
                                    "(SELECT GL_GRNLOT_INDEX AS GRN_LOT_INDEX, GM_B_COY_ID AS COY_ID, GL_GRN_NO AS GRN_NO, " &
                                    "GL_PO_LINE AS PO_LINE, POD_VENDOR_ITEM_CODE AS ITEM_CODE, POD_PRODUCT_DESC AS ITEM_NAME, " &
                                    "GL_LOCATION_INDEX AS LOC_INDEX, GL_LOT_INDEX AS LOT_INDEX, GL_LOT_RECEIVED_QTY AS LOT_RECEIVED_QTY, " &
                                    "GL_LOT_RECEIVED_QTY_ISSUE AS LOT_ISSUED_QTY, (GL_LOT_RECEIVED_QTY - GL_LOT_RECEIVED_QTY_ISSUE) AS DIFF_QTY " &
                                    "FROM GRN_LOT GL, GRN_MSTR GM, PO_MSTR POM, PO_DETAILS POD " &
                                    "WHERE GL_B_COY_ID = GM_B_COY_ID AND GL_GRN_NO = GM_GRN_NO " &
                                    "AND GM_PO_INDEX = POM_PO_INDEX AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                                    "AND GL_PO_LINE = POD_PO_LINE AND GL_LOT_RECEIVED_QTY > GL_LOT_RECEIVED_QTY_ISSUE " &
                                    "AND POD_VENDOR_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " &
                                    "AND GL_LOT_INDEX = '" & aryDetail(j)(0) & "' AND GL_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' " &
                                    "AND GM_B_COY_ID = '" & strCoyID & "' " &
                                    "ORDER BY GM_CREATED_DATE, GL_GRNLOT_INDEX) tb "

                            dsGRNQTY = objDb.FillDs(strSql)
                            If dsGRNQTY.Tables(0).Rows.Count > 0 Then
                                dblTotal = 0
                                For k = 0 To dsGRNQTY.Tables(0).Rows.Count - 1
                                    dblTotal = dblTotal + CDec(dsGRNQTY.Tables(0).Rows(k)("DIFF_QTY"))
                                Next

                                If CDec(aryDetail(j)(1)) > dblTotal Then
                                    strMsg = "No stock for you to issued this MRS."
                                    Return False
                                End If

                                For k = 0 To dsGRNQTY.Tables(0).Rows.Count - 1
                                    strGRNNo = dsGRNQTY.Tables(0).Rows(k)("GRN_NO")
                                    dblIssueQty = dsGRNQTY.Tables(0).Rows(k)("LOT_ISSUED_QTY")
                                    dblReceiveQty = dsGRNQTY.Tables(0).Rows(k)("LOT_RECEIVED_QTY")
                                    dblDiffQty = dsGRNQTY.Tables(0).Rows(k)("DIFF_QTY")
                                    INVENTORY_INDEX = objDb.GetVal("SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & Common.Parse(dsGRNQTY.Tables(0).Rows(k)("ITEM_CODE")) & "'")
                                    INVENTORY_NAME = dsGRNQTY.Tables(0).Rows(k)("ITEM_NAME")
                                    LOT_INDEX = dsGRNQTY.Tables(0).Rows(k)("LOT_INDEX")
                                    LOCATION_INDEX = dsGRNQTY.Tables(0).Rows(k)("LOC_INDEX")

                                    If k = 0 Then
                                        dblQty = CDec(aryDetail(j)(1)) - dblDiffQty

                                        If dblQty > 0 Then
                                            If strGRNNo = "SYSTEM" Then
                                                strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_LOT_QTY WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            Else
                                                strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(k)("GRN_LOT_INDEX")
                                                Common.Insert2Ary(strSqlAry, strSql)

                                                strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblDiffQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                        "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(k)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(k)("COY_ID") & "' "
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            End If

                                            strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblDiffQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblDiffQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                                    "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblDiffQty & ")"
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblDiffQty, aryTrans)
                                        Else
                                            If strGRNNo = "SYSTEM" Then
                                                strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_OPN_QTY_ISSUE + " & CDec(aryDetail(j)(1)) & " WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            Else
                                                strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY_ISSUE + " & CDec(aryDetail(j)(1)) & " WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(k)("GRN_LOT_INDEX")
                                                Common.Insert2Ary(strSqlAry, strSql)

                                                strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & CDec(aryDetail(j)(1)) & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                        "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(k)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(k)("COY_ID") & "' "
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            End If

                                            strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & CDec(aryDetail(j)(1)) & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & CDec(aryDetail(j)(1)) & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                                    "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & CDec(aryDetail(j)(1)) & ")"
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, CDec(aryDetail(j)(1)), aryTrans)
                                            Exit For
                                        End If

                                    Else
                                        If (dblReceiveQty - dblIssueQty) > dblQty Then
                                            If strGRNNo = "SYSTEM" Then
                                                strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_OPN_QTY_ISSUE + " & dblQty & " WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            Else
                                                strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY_ISSUE + " & dblQty & " WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(k)("GRN_LOT_INDEX")
                                                Common.Insert2Ary(strSqlAry, strSql)

                                                strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                        "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(k)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(k)("COY_ID") & "' "
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            End If

                                            strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                                    "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblQty & ")"
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblQty, aryTrans)
                                            Exit For
                                        Else
                                            dblQty = dblQty - (dblReceiveQty - dblIssueQty)

                                            If strGRNNo = "SYSTEM" Then
                                                strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_LOT_QTY WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            Else
                                                strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(k)("GRN_LOT_INDEX")
                                                Common.Insert2Ary(strSqlAry, strSql)

                                                strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblDiffQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                        "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(k)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(k)("COY_ID") & "' "
                                                Common.Insert2Ary(strSqlAry, strSql)
                                            End If

                                            strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblDiffQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblDiffQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                                    "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblDiffQty & ")"
                                            Common.Insert2Ary(strSqlAry, strSql)

                                            aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblDiffQty, aryTrans)
                                        End If
                                    End If
                                Next
                            Else
                                strMsg = "No stock for you to issued this MRS."
                                Return False
                            End If
                        End If
                    Next

                Else
                    dblReqQty = dsIRSD.Tables(0).Rows(i)("IRSD_QTY")

                    'strSql = "SELECT GL_GRNLOT_INDEX, GM_B_COY_ID, GL_GRN_NO, GL_PO_LINE, POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UNIT_COST, GL_LOCATION_INDEX, GL_LOT_INDEX, " & _
                    '        "GL_LOT_RECEIVED_QTY, GL_LOT_RECEIVED_QTY_ISSUE, (GL_LOT_RECEIVED_QTY - GL_LOT_RECEIVED_QTY_ISSUE) AS DIFF_QTY " & _
                    '        "FROM GRN_LOT GL, GRN_MSTR GM, PO_MSTR POM, PO_DETAILS POD " & _
                    '        "WHERE GL_B_COY_ID = GM_B_COY_ID And GL_GRN_NO = GM_GRN_NO " & _
                    '        "AND GM_PO_INDEX = POM_PO_INDEX AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                    '        "AND GL_PO_LINE = POD_PO_LINE AND GL_LOT_RECEIVED_QTY > GL_LOT_RECEIVED_QTY_ISSUE " & _
                    '        "AND POD_VENDOR_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " & _
                    '        "AND GM_B_COY_ID = '" & strCoyID & "' " & _
                    '        "ORDER BY GM_CREATED_DATE, GL_GRNLOT_INDEX "

                    strSql = "SELECT NULL AS GRN_LOT_INDEX, '" & strCoyID & "' AS COY_ID, 'SYSTEM' AS GRN_NO, CAST(DOL_PO_LINE AS UNSIGNED) AS PO_LINE, " &
                            "DOL_ITEM_CODE AS ITEM_CODE, (SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = DOL_ITEM_CODE AND IM_COY_ID = '" & strCoyID & "') AS ITEM_NAME, " &
                            "DOL_OPN_LOCATION_INDEX AS LOC_INDEX, DOL_LOT_INDEX AS LOT_INDEX, DOL_LOT_QTY AS LOT_RECEIVED_QTY, " &
                            "DOL_OPN_QTY_ISSUE AS LOT_ISSUED_QTY, (DOL_LOT_QTY - DOL_OPN_QTY_ISSUE) AS DIFF_QTY " &
                            "FROM DO_LOT WHERE DOL_COY_ID ='SYSTEM' AND DOL_LOT_QTY > DOL_OPN_QTY_ISSUE " &
                            "AND DOL_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " &
                            "UNION ALL " &
                            "SELECT * FROM " &
                            "(SELECT GL_GRNLOT_INDEX AS GRN_LOT_INDEX, GM_B_COY_ID AS COY_ID, GL_GRN_NO AS GRN_NO, GL_PO_LINE AS PO_LINE, " &
                            "POD_VENDOR_ITEM_CODE AS ITEM_CODE, POD_PRODUCT_DESC AS ITEM_NAME, GL_LOCATION_INDEX AS LOC_INDEX, GL_LOT_INDEX AS LOT_INDEX, " &
                            "GL_LOT_RECEIVED_QTY AS LOT_RECEIVED_QTY, GL_LOT_RECEIVED_QTY_ISSUE AS LOT_ISSUED_QTY, " &
                            "(GL_LOT_RECEIVED_QTY - GL_LOT_RECEIVED_QTY_ISSUE) AS DIFF_QTY " &
                            "FROM GRN_LOT GL, GRN_MSTR GM, PO_MSTR POM, PO_DETAILS POD " &
                            "WHERE GL_B_COY_ID = GM_B_COY_ID AND GL_GRN_NO = GM_GRN_NO " &
                            "AND GM_PO_INDEX = POM_PO_INDEX AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                            "AND GL_PO_LINE = POD_PO_LINE AND GL_LOT_RECEIVED_QTY > GL_LOT_RECEIVED_QTY_ISSUE " &
                            "AND POD_VENDOR_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " &
                            "AND GM_B_COY_ID = '" & strCoyID & "' " &
                            "ORDER BY GM_CREATED_DATE, GL_GRNLOT_INDEX) tb"

                    dsGRNQTY = objDb.FillDs(strSql)
                    If dsGRNQTY.Tables(0).Rows.Count > 0 Then
                        dblTotal = 0
                        For j = 0 To dsGRNQTY.Tables(0).Rows.Count - 1
                            dblTotal = dblTotal + CDec(dsGRNQTY.Tables(0).Rows(j)("DIFF_QTY"))
                        Next

                        If CDec(dsIRSD.Tables(0).Rows(i)("IRSD_QTY")) > dblTotal Then
                            strMsg = "No stock for you to issued this MRS."
                            Return False
                        End If

                        For j = 0 To dsGRNQTY.Tables(0).Rows.Count - 1
                            'strGRNNo = dsGRNQTY.Tables(0).Rows(j)("GL_GRN_NO")
                            'dblIssueQty = dsGRNQTY.Tables(0).Rows(j)("GL_LOT_RECEIVED_QTY_ISSUE")
                            'dblReceiveQty = dsGRNQTY.Tables(0).Rows(j)("GL_LOT_RECEIVED_QTY")
                            'dblDiffQty = dsGRNQTY.Tables(0).Rows(j)("DIFF_QTY")
                            'INVENTORY_INDEX = objDb.GetVal("SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & Common.Parse(dsGRNQTY.Tables(0).Rows(j)("POD_VENDOR_ITEM_CODE")) & "'")
                            'INVENTORY_NAME = dsGRNQTY.Tables(0).Rows(j)("POD_PRODUCT_DESC")
                            'LOT_INDEX = dsGRNQTY.Tables(0).Rows(j)("GL_LOT_INDEX")
                            'LOCATION_INDEX = dsGRNQTY.Tables(0).Rows(j)("GL_LOCATION_INDEX")

                            strGRNNo = dsGRNQTY.Tables(0).Rows(j)("GRN_NO")
                            dblIssueQty = dsGRNQTY.Tables(0).Rows(j)("LOT_ISSUED_QTY")
                            dblReceiveQty = dsGRNQTY.Tables(0).Rows(j)("LOT_RECEIVED_QTY")
                            dblDiffQty = dsGRNQTY.Tables(0).Rows(j)("DIFF_QTY")
                            INVENTORY_INDEX = objDb.GetVal("SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & Common.Parse(dsGRNQTY.Tables(0).Rows(j)("ITEM_CODE")) & "'")
                            INVENTORY_NAME = dsGRNQTY.Tables(0).Rows(j)("ITEM_NAME")
                            LOT_INDEX = dsGRNQTY.Tables(0).Rows(j)("LOT_INDEX")
                            LOCATION_INDEX = dsGRNQTY.Tables(0).Rows(j)("LOC_INDEX")

                            If j = 0 Then
                                dblQty = dblReqQty - dblDiffQty

                                If dblQty > 0 Then
                                    If strGRNNo = "SYSTEM" Then
                                        strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_LOT_QTY WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    Else
                                        strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(j)("GRN_LOT_INDEX")
                                        Common.Insert2Ary(strSqlAry, strSql)

                                        strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblDiffQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(j)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(j)("COY_ID") & "' "
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    End If

                                    strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblDiffQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblDiffQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                            "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblDiffQty & ")"
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblDiffQty, aryTrans)
                                Else
                                    If strGRNNo = "SYSTEM" Then
                                        strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_OPN_QTY_ISSUE + " & dblReqQty & " WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    Else
                                        strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY_ISSUE + " & dblReqQty & " WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(j)("GRN_LOT_INDEX")
                                        Common.Insert2Ary(strSqlAry, strSql)

                                        strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblReqQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(j)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(j)("COY_ID") & "' "
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    End If

                                    strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblReqQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblReqQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                            "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblReqQty & ")"
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblReqQty, aryTrans)
                                    Exit For
                                End If
                            Else
                                If (dblReceiveQty - dblIssueQty) > dblQty Then
                                    If strGRNNo = "SYSTEM" Then
                                        strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_OPN_QTY_ISSUE + " & dblQty & " WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    Else
                                        strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY_ISSUE + " & dblQty & " WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(j)("GRN_LOT_INDEX")
                                        Common.Insert2Ary(strSqlAry, strSql)

                                        strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(j)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(j)("COY_ID") & "' "
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    End If

                                    strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                            "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblQty & ")"
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblQty, aryTrans)
                                    Exit For
                                Else
                                    dblQty = dblQty - (dblReceiveQty - dblIssueQty)

                                    If strGRNNo = "SYSTEM" Then
                                        strSql = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_LOT_QTY WHERE DOL_LOT_INDEX = '" & LOT_INDEX & "'"
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    Else
                                        strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(j)("GRN_LOT_INDEX")
                                        Common.Insert2Ary(strSqlAry, strSql)

                                        strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE + " & dblDiffQty & " WHERE GD_GRN_NO='" & strGRNNo & "' " &
                                                "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(j)("PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(j)("COY_ID") & "' "
                                        Common.Insert2Ary(strSqlAry, strSql)
                                    End If

                                    strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & dblDiffQty & " WHERE ID_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LOCATION_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & dblDiffQty & " WHERE IL_INVENTORY_INDEX = '" & INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & LOT_INDEX & "' "
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    strSql = "INSERT INTO INVENTORY_REQUISITION_SLIP_LOT (IRSL_IRS_COY_ID, IRSL_IRS_NO, IRSL_GRN_NO, IRSL_IRS_LINE, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY) VALUES " &
                                            "('" & strCoyID & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_NO") & "','" & strGRNNo & "','" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "','" & LOCATION_INDEX & "','" & LOT_INDEX & "', " & dblDiffQty & ")"
                                    Common.Insert2Ary(strSqlAry, strSql)

                                    aryTrans = InsertMRStoTrans(INVENTORY_INDEX, INVENTORY_NAME, LOCATION_INDEX, dblDiffQty, aryTrans)
                                End If
                            End If
                        Next

                    Else
                        strMsg = "No stock for you to issued this MRS."
                        Return False
                    End If
                End If
            Next

            'Update Section Code to Product_mstr
            For i = 0 To dsIRSD.Tables(0).Rows.Count - 1
                strSql = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                        "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                        "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(Now) & ", 'II', '" & strMRSNo & "', '" & dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "', " &
                        "'" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_NAME")) & "', '" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_COY_ID") & "', " &
                        "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & dsIRSD.Tables(0).Rows(i)("IRSD_QTY") & ", IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_UPRICE*" & dsIRSD.Tables(0).Rows(i)("IRSD_QTY") & ", " &
                        "IC_COST_CLOSE_QTY-" & dsIRSD.Tables(0).Rows(i)("IRSD_QTY") & "," &
                        "IFNULL((IC_COST_CLOSE_COST-(IC_COST_CLOSE_UPRICE*" & dsIRSD.Tables(0).Rows(i)("IRSD_QTY") & ")) / (IC_COST_CLOSE_QTY-" & dsIRSD.Tables(0).Rows(i)("IRSD_QTY") & "),0)," &
                        "IC_COST_CLOSE_COST-(IC_COST_CLOSE_UPRICE*" & dsIRSD.Tables(0).Rows(i)("IRSD_QTY") & ") " &
                        "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                Common.Insert2Ary(strSqlAry, strSql)

                strSql = "UPDATE PRODUCT_MSTR SET PM_SECTION='" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IRSM_IRS_SECTION")) & "' " &
                        " WHERE PM_S_COY_ID='" & dsIRSD.Tables(0).Rows(i)("IM_COY_ID") & "' AND PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' "
                Common.Insert2Ary(strSqlAry, strSql)

            Next

            'Insert records into Inventory Trans Table
            If aryTrans.Count > 0 Then
                For i = 0 To aryTrans.Count - 1
                    strSql = " INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_ADDITION_INFO, IT_ADDITION_INFO1, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                            " VALUES ('" & strMRSNo & "', " &
                            " '" & aryTrans(i)(0) & "', " &
                            " 'MRS', " & aryTrans(i)(2) & ", " &
                            " " & Common.ConvertDate(Now) & ", " &
                            " '" & aryTrans(i)(1) & "', " &
                            " '" & Common.Parse(tDS.Tables(0).Rows(0)("IRSM_IRS_REF_NO")) & "', '" & Common.Parse(strRemark) & "', " &
                            " '" & tDS.Tables(0).Rows(0)("IRSM_IRS_ISSUE_TO") & "', '" & tDS.Tables(0).Rows(0)("IRSM_IRS_DEPARTMENT") & "', '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', " &
                            " '" & Common.Parse(aryTrans(i)(3)) & "') "
                    Common.Insert2Ary(strSqlAry, strSql)
                Next
            End If

            'Update MRS status to Issued
            strSql = "UPDATE INVENTORY_REQUISITION_SLIP_MSTR SET IRSM_IRS_STATUS=" & MRSStatus_new.Issued &
                    ", IRSM_IRS_APPROVED_DATE = " & Common.ConvertDate(Now) &
                    ", IRSM_IRS_ISSUE_REMARK = '" & Common.Parse(strRemark) & "' " &
                    ", IRSM_BUYER_ID = '" & strLoginUser & "' " &
                    ", IRSM_STATUS_CHANGED_BY = '" & strLoginUser & "', IRSM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                    " WHERE IRSM_IRS_INDEX=" & intMRSIndex
            Common.Insert2Ary(strSqlAry, strSql)

            'Also updated to detail status
            strSql = "UPDATE INVENTORY_REQUISITION_SLIP_DETAILS SET IRSD_IRS_STATUS=" & MRSStatus_new.Issued &
                    " WHERE IRSD_IRS_NO='" & strMRSNo & "' AND IRSD_IRS_COY_ID='" & strCoyID & "' "
            Common.Insert2Ary(strSqlAry, strSql)

            strMsg = "MRS No. " & strMRSNo & " has been issued. "

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.MRSMod, WheelUserActivity.SK_ApproveMRS, strMRSNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                Return False
            Else

                '//only send mail if transaction successfully created
                Dim objMail As New Email


                objMail.sendNotification(EmailType.MRSIssued, strRequestorId, strCoyID, "", strMRSNo, "", strLoginUserName)

                strSql = "SELECT IRA_AO, IRA_A_AO, IRA_APPROVAL_GRP_INDEX FROM ir_approval WHERE IRA_IR_INDEX =" & IRM_IR_INDEX
                dsApp = objDb.FillDs(strSql)

                If dsApp.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsApp.Tables(0).Rows.Count - 1
                        objMail.sendNotification(EmailType.MRSIssued, dsApp.Tables(0).Rows(i)("IRA_AO"), strCoyID, "", strMRSNo, "", strLoginUserName, "HOD")
                    Next

                    strSql = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_GRP_INDEX =" & dsApp.Tables(0).Rows(0)("IRA_APPROVAL_GRP_INDEX")
                    strMgr = objDb.GetVal(strSql)
                    strMgr = Replace(strMgr, " ", "")
                    strStoreMgr = Split(strMgr, ";")

                    For i = 0 To strStoreMgr.Length - 1
                        If System.Text.RegularExpressions.Regex.IsMatch(strStoreMgr(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                            sendMailToStoreMgr(strMRSNo, strStoreMgr(i), strLoginUserName, , "MRS")
                            'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                        End If
                    Next
                End If
            End If

            Return True
        End Function

        'Chee Hong - enhancement on 27/09/2013: To store records into array during Issue MRS
        Function InsertMRStoTrans(ByVal strItemIndex As String, ByVal strItemName As String, ByVal strLocIndex As String, ByVal decQty As Decimal, ByVal aryDetail As ArrayList) As ArrayList
            Dim i As Integer
            Dim blnFound As Boolean

            If aryDetail Is Nothing Then
                aryDetail.Add(New String() {strItemIndex, strLocIndex, decQty, strItemName})
            Else
                If aryDetail.Count > 0 Then
                    blnFound = False
                    For i = 0 To aryDetail.Count - 1
                        If aryDetail(i)(0) = strItemIndex And aryDetail(i)(1) = strLocIndex Then
                            aryDetail(i)(2) = CDec(aryDetail(i)(2)) + decQty
                            blnFound = True
                        End If
                    Next

                    If blnFound = False Then
                        aryDetail.Add(New String() {strItemIndex, strLocIndex, decQty, strItemName})
                    End If
                Else
                    aryDetail.Add(New String() {strItemIndex, strLocIndex, decQty, strItemName})
                End If
            End If

            Return aryDetail
        End Function

        Function RejectMRS(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strLoginUserName As String, ByVal strMRSNo As String, ByVal intMRSIndex As Long, ByVal strRemark As String) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strMsg, strVendor, strRequestorId As String
            Dim intMRSStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname, IRM_IR_INDEX As String
            Dim strMsg1, strMgr As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim dsGRNQTY As New DataSet
            Dim dsApp As New DataSet
            Dim strSqlLast As String
            Dim strStoreMgr() As String
            Dim i, j As Integer
            Dim dblMthStkIssed, dblLast3MthAve As Decimal


            strSql = "SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_DETAILS, INVENTORY_REQUISITION_MSTR WHERE " &
                       "IRD_IR_COY_ID = IRM_IR_COY_ID And IRD_IR_NO = IRM_IR_NO " &
                       "AND IRD_IR_SLIP_INDEX = " & intMRSIndex & " LIMIT 1 "
            IRM_IR_INDEX = objDb.GetVal(strSql)

            strSql = "SELECT IRM_CREATED_BY FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX = " & intMRSIndex
            strRequestorId = objDb.GetVal(strSql)

            strSql = "SELECT IRSM_IRS_STATUS FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_INDEX = " & intMRSIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intMRSStatus = tDS.Tables(0).Rows(0).Item("IRSM_IRS_STATUS")
                If intMRSStatus = MRSStatus_new.Issued Then
                    strMsg = "You have already issued this MRS."
                ElseIf intMRSStatus = MRSStatus_new.Rejected Then
                    strMsg = "You have already rejected this MRS."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            'Update MRS status to Rejected
            strSql = "UPDATE INVENTORY_REQUISITION_SLIP_MSTR SET IRSM_IRS_STATUS=" & MRSStatus_new.Rejected &
                    ", IRSM_IRS_ISSUE_REMARK = '" & Common.Parse(strRemark) & "'" &
                    ", IRSM_BUYER_ID = '" & strLoginUser & "' " &
                    ", IRSM_STATUS_CHANGED_BY = '" & strLoginUser & "', IRSM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                    " WHERE IRSM_IRS_INDEX=" & intMRSIndex
            Common.Insert2Ary(strSqlAry, strSql)

            'Also updated to detail status
            'strSql = "UPDATE INVENTORY_REQUISITION_SLIP_DETAILS SET IRSD_IRS_STATUS=" & MRSStatus_new.Rejected & _
            '        " WHERE IRSD_IRS_NO='" & strMRSNo & "' AND IRSD_IRS_COY_ID='" & strCoyID & "' "
            'Common.Insert2Ary(strSqlAry, strSql)

            strSql = "SELECT * FROM INVENTORY_REQUISITION_SLIP_DETAILS, " &
                    "INVENTORY_REQUISITION_SLIP_MSTR, INVENTORY_MSTR " &
                    "WHERE IRSD_IRS_COY_ID = IRSM_IRS_COY_ID AND IRSD_IRS_NO = IRSM_IRS_NO AND IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "AND IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSD_IRS_NO = '" & strMRSNo & "'"
            Dim dsIRSD As DataSet = objDb.FillDs(strSql)

            For i = 0 To dsIRSD.Tables(0).Rows.Count - 1
                dblMthStkIssed = getMonthStockBalance(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE"))
                dblLast3MthAve = getLast3MthAve(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE"), dsIRSD.Tables(0).Rows(i)("IRSM_IRS_DEPARTMENT"))

                strSql = "UPDATE INVENTORY_REQUISITION_SLIP_DETAILS SET IRSD_IRS_MTHISSUE = " & dblMthStkIssed & ", IRSD_IRS_LAST3MTH = " & dblLast3MthAve & ", IRSD_IRS_STATUS = " & MRSStatus_new.Rejected & " " &
                        "WHERE IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSD_IRS_NO = '" & strMRSNo & "' AND IRSD_IRS_LINE = '" & dsIRSD.Tables(0).Rows(i)("IRSD_IRS_LINE") & "'"
                Common.Insert2Ary(strSqlAry, strSql)
            Next

            strMsg = "MRS No. " & strMRSNo & " has been rejected. "

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.MRSMod, WheelUserActivity.SK_RejectMRS, strMRSNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                Dim objMail As New Email
                Dim strChkEmail As String

                objMail.sendNotification(EmailType.MRSRejected, strRequestorId, strCoyID, "", strMRSNo, "", strLoginUserName)

                strSql = "SELECT IRA_AO, IRA_A_AO, IRA_APPROVAL_GRP_INDEX FROM ir_approval WHERE IRA_IR_INDEX =" & IRM_IR_INDEX
                dsApp = objDb.FillDs(strSql)

                strSql = "SELECT CM_REJECT_STOCK_EMAIL FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "'"
                strChkEmail = objDb.GetVal(strSql)
                If dsApp.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsApp.Tables(0).Rows.Count - 1
                        objMail.sendNotification(EmailType.MRSRejected, dsApp.Tables(0).Rows(i)("IRA_AO"), strCoyID, "", strMRSNo, "", strLoginUserName, "HOD")
                    Next

                    If strChkEmail = "Y" Then
                        strSql = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_GRP_INDEX =" & dsApp.Tables(0).Rows(0)("IRA_APPROVAL_GRP_INDEX")
                        strMgr = objDb.GetVal(strSql)
                        strMgr = Replace(strMgr, " ", "")
                        strStoreMgr = Split(strMgr, ";")

                        For i = 0 To strStoreMgr.Length - 1
                            If System.Text.RegularExpressions.Regex.IsMatch(strStoreMgr(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                                sendMailToStoreMgr(strMRSNo, strStoreMgr(i), strLoginUserName, "R", "MRS")
                                'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                            End If
                        Next
                    End If
                End If
            End If

            Return strMsg
        End Function

        Function MassAcknowledgeMRS(ByVal strLoginUser As String, ByVal strLoginUserName As String, ByVal strAryMRSIndex() As String, ByRef strReturnMsg() As String) As Boolean
            Dim strSql, strSql1, strAryQuery(0), strApprType As String
            Dim strCoyID, strAllIR, strAllMRSIndex As String
            Dim intLoop, i As Integer
            Dim ds As DataSet
            Dim strMsg As String
            Dim dsTemp As DataSet
            Dim dsDetail As DataSet
            Dim dblMthStkIssed, dblLast3MthAve, dblReqQty, dblStkBal, dblSafetyLvl, dblMaxInvQty As Decimal

            strCoyID = HttpContext.Current.Session("CompanyId")

            For intLoop = 0 To strAryMRSIndex.Length - 1
                If intLoop = 0 Then
                    strAllMRSIndex = strAryMRSIndex(intLoop)
                Else
                    strAllMRSIndex = strAllMRSIndex & "," & strAryMRSIndex(intLoop)
                End If
            Next

            strSql = "SELECT IRSM_IRS_INDEX, IRSM_IRS_NO, IRSM_IRS_COY_ID, IRSM_IRS_STATUS, IRSM_STATUS_CHANGED_BY, IRSM_IRS_DEPARTMENT FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_INDEX IN (" & strAllMRSIndex & ") "

            ds = objDb.FillDs(strSql)

            If Not ds Is Nothing Then
                Dim MRSrow, MRSApprrow As DataRow

                For Each MRSrow In ds.Tables(0).Rows
                    If MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.Issued Or MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.PartialIssued Then
                        strMsg = AcknowledgeMRS(strLoginUser, strCoyID, strLoginUserName, MRSrow("IRSM_IRS_NO"), MRSrow("IRSM_IRS_INDEX"), "")
                        Common.Insert2Ary(strReturnMsg, strMsg)
                    Else
                        'IR Cancelled/Rejected
                        If MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.Cancelled Then
                            strMsg = "MRS No. " & MRSrow("IRSM_IRS_NO") & "has already been cancelled."
                        ElseIf MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.Acknowledged Then
                            strMsg = "MRS No. " & MRSrow("IRSM_IRS_NO") & "has already been acknowledged."
                        ElseIf MRSrow("IRSM_IRS_STATUS") = MRSStatus_new.AutoAcknowledged Then
                            strMsg = "MRS No. " & MRSrow("IRSM_IRS_NO") & "has already been auto-acknowledged."
                        End If
                        Common.Insert2Ary(strReturnMsg, strMsg)
                    End If
                Next

            End If
            Return True
        End Function

        Function AcknowledgeMRS(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strLoginUserName As String, ByVal strMRSNo As String, ByVal intMRSIndex As Long, ByVal strRemark As String) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strMsg, strVendor, strRequestorId As String
            Dim intMRSStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname, IRM_IR_INDEX As String
            Dim strMsg1, strMgr As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim dsGRNQTY As New DataSet
            Dim dsApp As New DataSet
            Dim strSqlLast As String
            Dim strStoreMgr() As String
            Dim i, j As Integer

            strSql = "SELECT IRSM_IRS_STATUS FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_INDEX = " & intMRSIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intMRSStatus = tDS.Tables(0).Rows(0).Item("IRSM_IRS_STATUS")
                If intMRSStatus = MRSStatus_new.Acknowledged Then
                    strMsg = "You have already acknowledged this MRS."
                ElseIf intMRSStatus = MRSStatus_new.AutoAcknowledged Then
                    strMsg = "This MRS No has already been auto-acknowledged."
                ElseIf intMRSStatus = MRSStatus_new.Cancelled Then
                    strMsg = "You have already cancelled this MRS."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            'Update MRS status to Rejected
            strSql = "UPDATE INVENTORY_REQUISITION_SLIP_MSTR SET IRSM_IRS_STATUS=" & MRSStatus_new.Acknowledged &
                    ", IRSM_IRS_ACKCANCEL_REMARK = '" & Common.Parse(strRemark) & "'" &
                    ", IRSM_STATUS_CHANGED_BY = '" & strLoginUser & "', IRSM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                    " WHERE IRSM_IRS_INDEX=" & intMRSIndex
            Common.Insert2Ary(strSqlAry, strSql)

            strMsg = "MRS No. " & strMRSNo & " has been acknowledged. "

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.MRSMod, WheelUserActivity.REQ_AckMRS, strMRSNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                'Dim objMail As New Email
                'strSql = "SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_DETAILS, INVENTORY_REQUISITION_MSTR WHERE " & _
                '        "IRD_IR_COY_ID = IRM_IR_COY_ID AND IRD_IR_NO = IRM_IR_NO " & _
                '        "AND IRD_IR_SLIP_INDEX = " & intMRSIndex & " LIMIT 1 "
                'IRM_IR_INDEX = objDb.GetVal(strSql)

                'strSql = "SELECT IRM_CREATED_BY FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX = " & intMRSIndex
                'strRequestorId = objDb.GetVal(strSql)

                'objMail.sendNotification(EmailType.MRSRejected, strRequestorId, strCoyID, "", strMRSNo, "", strLoginUserName)

                'strSql = "SELECT IRA_AO, IRA_A_AO, IRA_APPROVAL_GRP_INDEX FROM ir_approval WHERE IRA_IR_INDEX =" & IRM_IR_INDEX
                'dsApp = objDb.FillDs(strSql)

                'If dsApp.Tables(0).Rows.Count > 0 Then
                '    For i = 0 To dsApp.Tables(0).Rows.Count - 1
                '        objMail.sendNotification(EmailType.MRSRejected, dsApp.Tables(0).Rows(i)("IRA_AO"), strCoyID, "", strMRSNo, "", strLoginUserName, "HOD")
                '    Next

                '    strSql = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_GRP_INDEX =" & dsApp.Tables(0).Rows(0)("IRA_APPROVAL_GRP_INDEX")
                '    strMgr = objDb.GetVal(strSql)
                '    strMgr = Replace(strMgr, " ", "")
                '    strStoreMgr = Split(strMgr, ";")

                '    For i = 0 To strStoreMgr.Length - 1
                '        If System.Text.RegularExpressions.Regex.IsMatch(strStoreMgr(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                '            sendMailToStoreMgr(strMRSNo, strStoreMgr(i), strLoginUserName, "I")
                '            'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                '        End If
                '    Next
                'End If
            End If

            Return strMsg
        End Function

        Function CancelMRS(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strLoginUserName As String, ByVal strMRSNo As String, ByVal intMRSIndex As Long, ByVal strRemark As String) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strIQC, strIQC1, strMsg, strVendor, strRequestorId, strMgr As String
            Dim intMRSStatus As Integer
            Dim INVENTORY_INDEX, LOT_INDEX, LOCATION_INDEX, IRM_IR_INDEX, LM_LOCATION_INDEX As String
            Dim strMsg1 As String
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim dsQTY As New DataSet
            Dim dsGRNQTY As New DataSet
            Dim dsApp As New DataSet
            Dim strSqlLast As String
            Dim strStoreMgr() As String
            Dim i, j, k As Integer
            Dim blnFound As Boolean
            Dim decReqQty, decTotal, decIssueQty, decReceiveQty, decDiffQty, decQty As Decimal

            strSql = "SELECT IRSM_IRS_STATUS FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_INDEX = " & intMRSIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intMRSStatus = tDS.Tables(0).Rows(0).Item("IRSM_IRS_STATUS")
                If intMRSStatus = MRSStatus_new.Acknowledged Then
                    strMsg = "You have already acknowledged this MRS."
                ElseIf intMRSStatus = MRSStatus_new.AutoAcknowledged Then
                    strMsg = "This MRS No has already been auto-acknowledged."
                ElseIf intMRSStatus = MRSStatus_new.Cancelled Then
                    strMsg = "You have already cancelled this MRS."
                End If
            End If

            If strMsg <> "" Then
                Return strMsg
            End If

            strSql = "SELECT IRSD_IRS_COY_ID, IRSD_INVENTORY_INDEX, IRSD_QTY, IM_ITEM_CODE, IRSD_INVENTORY_NAME, IRSL_LOCATION_INDEX, IRSL_LOT_INDEX, IRSL_LOT_QTY, IRSL_GRN_NO " &
                    "FROM INVENTORY_REQUISITION_SLIP_DETAILS, INVENTORY_REQUISITION_SLIP_LOT, INVENTORY_MSTR " &
                    "WHERE IRSD_IRS_COY_ID = IRSL_IRS_COY_ID And IRSD_IRS_NO = IRSL_IRS_NO And IRSD_IRS_LINE = IRSL_IRS_LINE " &
                    "AND IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX AND IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSD_IRS_NO = '" & strMRSNo & "'"
            Dim dsIRSD As DataSet = objDb.FillDs(strSql)

            For i = 0 To dsIRSD.Tables(0).Rows.Count - 1
                decReqQty = dsIRSD.Tables(0).Rows(i)("IRSL_LOT_QTY")

                strSql = "SELECT GL_GRNLOT_INDEX, GM_B_COY_ID, GL_GRN_NO, GL_PO_LINE, POD_VENDOR_ITEM_CODE, POD_UNIT_COST, GL_LOCATION_INDEX, GL_LOT_INDEX, " &
                        "GL_LOT_RECEIVED_QTY, GL_LOT_RECEIVED_QTY_ISSUE, (GL_LOT_RECEIVED_QTY - GL_LOT_RECEIVED_QTY_ISSUE) AS DIFF_QTY " &
                        "FROM GRN_LOT GL, GRN_MSTR GM, PO_MSTR POM, PO_DETAILS POD " &
                        "WHERE GL_B_COY_ID = GM_B_COY_ID And GL_GRN_NO = GM_GRN_NO " &
                        "AND GM_PO_INDEX = POM_PO_INDEX AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                        "AND GL_PO_LINE = POD_PO_LINE " &
                        "AND POD_VENDOR_ITEM_CODE = '" & Common.Parse(dsIRSD.Tables(0).Rows(i)("IM_ITEM_CODE")) & "' " &
                        "AND GL_LOT_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSL_LOT_INDEX") & "' " &
                        "AND GL_LOCATION_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSL_LOCATION_INDEX") & "' " &
                        "AND GL_GRN_NO = '" & dsIRSD.Tables(0).Rows(i)("IRSL_GRN_NO") & "' " &
                        "AND GM_B_COY_ID = '" & strCoyID & "' " &
                        "ORDER BY GM_CREATED_DATE, GL_PO_LINE "

                dsGRNQTY = objDb.FillDs(strSql)
                If dsGRNQTY.Tables(0).Rows.Count > 0 Then
                    For j = 0 To dsGRNQTY.Tables(0).Rows.Count - 1
                        decReceiveQty = dsGRNQTY.Tables(0).Rows(j)("GL_LOT_RECEIVED_QTY_ISSUE")

                        If decReqQty > decReceiveQty Then
                            decReqQty = decReqQty - decReceiveQty

                            strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = 0 WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(j)("GL_GRNLOT_INDEX")
                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = 0 WHERE GD_GRN_NO='" & dsGRNQTY.Tables(0).Rows(j)("GL_GRN_NO") & "' " &
                                    "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(j)("GL_PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(j)("GM_B_COY_ID") & "' "
                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & decReceiveQty & " " &
                                    "WHERE ID_INVENTORY_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "' AND ID_LOCATION_INDEX = '" & dsGRNQTY.Tables(0).Rows(j)("GL_LOCATION_INDEX") & "' "
                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY + " & decReceiveQty & " " &
                                    "WHERE IL_INVENTORY_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "' AND IL_LOCATION_INDEX = '" & dsGRNQTY.Tables(0).Rows(j)("GL_LOCATION_INDEX") & "' " &
                                    "AND IL_LOT_INDEX = '" & dsGRNQTY.Tables(0).Rows(j)("GL_LOT_INDEX") & "' "
                            Common.Insert2Ary(strSqlAry, strSql)
                        Else
                            strSql = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY_ISSUE - " & decReqQty & " WHERE GL_GRNLOT_INDEX=" & dsGRNQTY.Tables(0).Rows(j)("GL_GRNLOT_INDEX")
                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE - " & decReqQty & " WHERE GD_GRN_NO='" & dsGRNQTY.Tables(0).Rows(j)("GL_GRN_NO") & "' " &
                                    "AND GD_PO_LINE =" & dsGRNQTY.Tables(0).Rows(j)("GL_PO_LINE") & " AND GD_B_COY_ID = '" & dsGRNQTY.Tables(0).Rows(j)("GM_B_COY_ID") & "' "
                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & decReqQty & " " &
                                   "WHERE ID_INVENTORY_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "' AND ID_LOCATION_INDEX = '" & dsGRNQTY.Tables(0).Rows(j)("GL_LOCATION_INDEX") & "' "
                            Common.Insert2Ary(strSqlAry, strSql)

                            strSql = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY + " & decReqQty & " " &
                                    "WHERE IL_INVENTORY_INDEX = '" & dsIRSD.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "' AND IL_LOCATION_INDEX = '" & dsGRNQTY.Tables(0).Rows(j)("GL_LOCATION_INDEX") & "' " &
                                    "AND IL_LOT_INDEX = '" & dsGRNQTY.Tables(0).Rows(j)("GL_LOT_INDEX") & "' "
                            Common.Insert2Ary(strSqlAry, strSql)

                            Exit For
                        End If
                    Next
                End If
            Next

            strSql = "SELECT IRSD_IRS_COY_ID, IRSD_INVENTORY_INDEX, IRSD_QTY, IM_ITEM_CODE, IRSD_INVENTORY_NAME " &
                    "FROM INVENTORY_REQUISITION_SLIP_DETAILS, INVENTORY_MSTR " &
                    "WHERE IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX AND IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSD_IRS_NO = '" & strMRSNo & "'"
            Dim dsCost As DataSet = objDb.FillDs(strSql)

            'Update Section Code to Product_mstr
            For i = 0 To dsCost.Tables(0).Rows.Count - 1
                strSql = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                        "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                        "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(Now) & ", 'IIC', '" & strMRSNo & "', '" & dsCost.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "', " &
                        "'" & Common.Parse(dsCost.Tables(0).Rows(i)("IRSD_INVENTORY_NAME")) & "', '" & dsCost.Tables(0).Rows(i)("IRSD_IRS_COY_ID") & "', " &
                        "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & dsCost.Tables(0).Rows(i)("IRSD_QTY") & ", IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_UPRICE*" & dsCost.Tables(0).Rows(i)("IRSD_QTY") & ", " &
                        "IC_COST_CLOSE_QTY+" & dsCost.Tables(0).Rows(i)("IRSD_QTY") & "," &
                        "(IC_COST_CLOSE_COST+(IC_COST_CLOSE_UPRICE*" & dsCost.Tables(0).Rows(i)("IRSD_QTY") & ")) / (IC_COST_CLOSE_QTY+" & dsCost.Tables(0).Rows(i)("IRSD_QTY") & ")," &
                        "IC_COST_CLOSE_COST+(IC_COST_CLOSE_UPRICE*" & dsCost.Tables(0).Rows(i)("IRSD_QTY") & ") " &
                        "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & dsCost.Tables(0).Rows(i)("IRSD_INVENTORY_INDEX") & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                Common.Insert2Ary(strSqlAry, strSql)

            Next

            'Insert into Trans Table
            strSql = "INSERT INTO INVENTORY_TRANS(IT_TRANS_REF_NO, IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_REF_NO, IT_REMARK, IT_ADDITION_INFO, IT_ADDITION_INFO1, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                    "SELECT IT_TRANS_REF_NO, IT_INVENTORY_INDEX, 'MRSC', IT_TRANS_QTY, " & Common.ConvertDate(Now) & ", IT_FRM_LOCATION_INDEX, IT_REF_NO, '" & Common.Parse(strRemark) & "', IT_ADDITION_INFO, IT_ADDITION_INFO1, '" & strLoginUser & "', IT_INVENTORY_NAME " &
                    "FROM INVENTORY_TRANS " &
                    "INNER JOIN INVENTORY_MSTR ON  IT_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "WHERE IT_TRANS_TYPE = 'MRS' AND IT_TRANS_REF_NO = '" & strMRSNo & "' AND IM_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strSqlAry, strSql)

            'Update MRS status to Cancel
            strSql = "UPDATE INVENTORY_REQUISITION_SLIP_MSTR SET IRSM_IRS_STATUS=" & MRSStatus_new.Cancelled &
                    ", IRSM_IRS_APPROVED_DATE = " & Common.ConvertDate(Now) &
                    ", IRSM_IRS_ACKCANCEL_REMARK = '" & Common.Parse(strRemark) & "' " &
                    ", IRSM_STATUS_CHANGED_BY = '" & strLoginUser & "', IRSM_STATUS_CHANGED_ON =" & Common.ConvertDate(Now) &
                    " WHERE IRSM_IRS_INDEX=" & intMRSIndex
            Common.Insert2Ary(strSqlAry, strSql)

            strMsg = "MRS No. " & strMRSNo & " has been cancelled. "


            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.MRSMod, WheelUserActivity.REQ_CancelMRS, strMRSNo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "

            Else

                '//only send mail if transaction successfully created
                '    Dim objMail As New Email
                '    strSql = "SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_DETAILS, INVENTORY_REQUISITION_MSTR WHERE " & _
                '            "IRD_IR_COY_ID = IRM_IR_COY_ID And IRD_IR_NO = IRM_IR_NO " & _
                '            "AND IRD_IR_SLIP_INDEX = " & intMRSIndex & " LIMIT 1 "
                '    IRM_IR_INDEX = objDb.GetVal(strSql)

                '    strSql = "SELECT IRM_CREATED_BY FROM INVENTORY_REQUISITION_MSTR WHERE IRM_IR_INDEX = " & intMRSIndex
                '    strRequestorId = objDb.GetVal(strSql)

                '    objMail.sendNotification(EmailType.MRSIssued, strRequestorId, strCoyID, "", strMRSNo, "", strLoginUserName)

                '    strSql = "SELECT IRA_AO, IRA_A_AO, IRA_APPROVAL_GRP_INDEX FROM ir_approval WHERE IRA_IR_INDEX =" & IRM_IR_INDEX
                '    dsApp = objDb.FillDs(strSql)

                '    If dsApp.Tables(0).Rows.Count > 0 Then
                '        For i = 0 To dsApp.Tables(0).Rows.Count - 1
                '            objMail.sendNotification(EmailType.MRSIssued, dsApp.Tables(0).Rows(i)("IRA_AO"), strCoyID, "", strMRSNo, "", strLoginUserName, "HOD")
                '        Next

                '        strSql = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_GRP_INDEX =" & dsApp.Tables(0).Rows(0)("IRA_APPROVAL_GRP_INDEX")
                '        strMgr = objDb.GetVal(strSql)
                '        strMgr = Replace(strMgr, " ", "")
                '        strStoreMgr = Split(strMgr, ";")

                '        For i = 0 To strStoreMgr.Length - 1
                '            If System.Text.RegularExpressions.Regex.IsMatch(strStoreMgr(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                '                sendMailToStoreMgr(strMRSNo, strStoreMgr(i), strLoginUserName)
                '                'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                '            End If
                '        Next
                '    End If
            End If

            Return strMsg
        End Function

        Public Function getMRSTempAttach(ByVal strDocNo As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'MRS' "
            strsql &= "AND CDA_TYPE = 'I' "

            ds = objDb.FillDs(strsql)
            getMRSTempAttach = ds

        End Function

        Public Function deleteMRSAttachment(ByVal intIndex As Integer)
            Dim strsql As String
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
            objDb.Execute(strsql)
        End Function

        Public Function getMRSLot(ByVal strDocNo As String, ByVal intLine As Integer) As DataSet
            Dim strsql As String
            Dim ds As DataSet
            Dim strCoyID As String
            strCoyID = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT DOL_LOT_NO, LM_LOCATION, IFNULL(LM_SUB_LOCATION,'-') AS LM_SUB_LOCATION, IRSL_LOT_QTY FROM INVENTORY_REQUISITION_SLIP_LOT, " &
                    "INVENTORY_REQUISITION_SLIP_DETAILS, INVENTORY_MSTR, LOCATION_MSTR, DO_LOT " &
                    "WHERE IRSL_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSL_IRS_NO = IRSD_IRS_NO AND IRSL_IRS_LINE = IRSD_IRS_LINE " &
                    "AND IRSD_INVENTORY_INDEX = IM_INVENTORY_INDEX AND IRSL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                    "AND IRSL_LOT_INDEX = DOL_LOT_INDEX " &
                    "AND IRSL_IRS_COY_ID = '" & strCoyID & "' AND IRSL_IRS_NO = '" & strDocNo & "' AND IRSL_IRS_LINE =" & intLine

            ds = objDb.FillDs(strsql)
            getMRSLot = ds
        End Function

        Public Function getROLot(ByVal strDocNo As String, ByVal intLine As Integer) As DataSet
            Dim strsql As String
            Dim ds As DataSet
            Dim strCoyID As String
            strCoyID = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT DOL_LOT_NO, LM_LOCATION, IFNULL(LM_SUB_LOCATION,'-') AS LM_SUB_LOCATION, IROL_LOT_QTY FROM INVENTORY_RETURN_OUTWARD_LOT, " &
                    "INVENTORY_RETURN_OUTWARD_DETAILS, INVENTORY_MSTR, LOCATION_MSTR, DO_LOT " &
                    "WHERE IROL_RO_COY_ID = IROD_RO_COY_ID And IROL_RO_NO = IROD_RO_NO And IROL_RO_LINE = IROD_RO_LINE " &
                    "AND IROD_INVENTORY_INDEX = IM_INVENTORY_INDEX AND IROL_LOCATION_INDEX = LM_LOCATION_INDEX " &
                    "AND IROL_LOT_INDEX = DOL_LOT_INDEX " &
                    "AND IROL_RO_COY_ID = '" & strCoyID & "' AND IROL_RO_NO = '" & strDocNo & "' AND IROL_RO_LINE =" & intLine

            ds = objDb.FillDs(strsql)
            getROLot = ds
        End Function

        Public Function MRSList(ByVal strMRSNo As String, ByVal strIssueTo As String, ByVal strDepartment As String, ByVal strAccCode As String, ByVal strStartDate As String, ByVal strEndDate As String, Optional ByVal strTrx As String = "") As DataSet
            Dim strSql, strCoyID, strLoginUser As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            strSql = "SELECT * FROM " &
                    "(SELECT INVENTORY_REQUISITION_SLIP_MSTR.*, CONCAT(CS_SEC_CODE, ' (', CS_SEC_NAME, ')') AS CS_SEC_NAME, CDM_DEPT_NAME, " &
                    "(SELECT DISTINCT IRD_IR_NO FROM INVENTORY_REQUISITION_DETAILS WHERE IRD_IR_SLIP_INDEX = IRSM_IRS_INDEX) AS IRD_IR_NO " &
                    "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                    "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSD_IRS_NO = IRSM_IRS_NO AND IRSD_IRS_COY_ID = IRSM_IRS_COY_ID " &
                    "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND CS_COY_ID = IRSM_IRS_COY_ID " &
                    "LEFT JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                    "WHERE IRSM_IRS_COY_ID = '" & strCoyID & "' AND (IRSM_IRS_STATUS = 3 OR IRSM_IRS_STATUS = 4) " &
                    "AND (IRSD_QTY - IRSD_RETURN_QTY) > 0) tb " &
                    "INNER JOIN INVENTORY_REQUISITION_MSTR ON tb.IRD_IR_NO = IRM_IR_NO AND IRSM_IRS_COY_ID = IRM_IR_COY_ID " &
                    "WHERE IRM_CREATED_BY = '" & strLoginUser & "' "

            If strMRSNo <> "" Then
                strTemp = Common.BuildWildCard(strMRSNo)
                strSql = strSql & " AND IRSM_IRS_NO" & Common.ParseSQL(strTemp)
            End If

            If strIssueTo <> "" Then
                strTemp = Common.BuildWildCard(strIssueTo)
                strSql = strSql & " AND IRSM_IRS_ISSUE_TO" & Common.ParseSQL(strTemp)
            End If

            If strDepartment <> "" Then
                strTemp = Common.BuildWildCard(strDepartment)
                strSql = strSql & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
            End If

            If strAccCode <> "" Then
                strTemp = Common.BuildWildCard(strAccCode)
                strSql = strSql & " AND IRSM_IRS_ACCOUNT_CODE" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                strSql = strSql & " AND IRSM_IRS_DATE >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                strSql = strSql & " AND IRSM_IRS_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            strSql &= " GROUP BY IRSM_IRS_INDEX "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function MRSInfo(ByVal strMRSNo As String) As DataSet
            Dim strSql, strSqlM, strSqlD, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strTemp = Common.BuildWildCard(strMRSNo)

            strSqlM = "SELECT INVENTORY_REQUISITION_SLIP_MSTR.*, CONCAT(CS_SEC_CODE, ' (', CS_SEC_NAME, ')') AS CS_SEC_NAME, CDM_DEPT_NAME, STATUS_DESC " &
                     "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                     "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND CS_COY_ID = IRSM_IRS_COY_ID " &
                     "LEFT JOIN COMPANY_DEPT_MSTR ON IRSM_IRS_COY_ID = CDM_COY_ID AND IRSM_IRS_DEPARTMENT = CDM_DEPT_CODE " &
                     "LEFT JOIN STATUS_MSTR ON STATUS_TYPE = 'MRS' AND IRSM_IRS_STATUS = STATUS_NO " &
                     "WHERE IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (IRSM_IRS_STATUS = 3 OR IRSM_IRS_STATUS = 4) " &
                     "AND IRSM_IRS_NO " & Common.ParseSQL(strTemp) & ""

            strSqlD = "SELECT INVENTORY_REQUISITION_SLIP_MSTR.*, INVENTORY_REQUISITION_SLIP_DETAILS.*, LOCATION_MSTR.*, INVENTORY_MSTR.*, CONCAT(CS_SEC_CODE, ' (', CS_SEC_NAME, ')') AS CS_SEC_NAME, IRSL_LOT_INDEX, (SELECT DOL_LOT_NO FROM DO_LOT WHERE DOL_LOT_INDEX = IRSL_LOT_INDEX) AS DOL_LOT_NO, IFNULL(IRSL_LOT_RETURN_QTY,0) AS IRSL_LOT_RETURN_QTY, IRSL_LOT_QTY " &
                     "FROM INVENTORY_REQUISITION_SLIP_MSTR " &
                     "INNER JOIN INVENTORY_REQUISITION_SLIP_DETAILS ON IRSD_IRS_NO = IRSM_IRS_NO AND IRSD_IRS_COY_ID = IRSM_IRS_COY_ID " &
                     "INNER JOIN INVENTORY_REQUISITION_SLIP_LOT ON IRSL_IRS_COY_ID = IRSD_IRS_COY_ID AND IRSL_IRS_NO = IRSD_IRS_NO AND IRSL_IRS_LINE = IRSD_IRS_LINE " &
                     "INNER JOIN INVENTORY_MSTR ON IM_COY_ID = IRSD_IRS_COY_ID AND IM_INVENTORY_INDEX = IRSD_INVENTORY_INDEX " &
                     "INNER JOIN INVENTORY_DETAIL ON ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND ID_LOCATION_INDEX = IRSL_LOCATION_INDEX " &
                     "INNER JOIN LOCATION_MSTR ON LM_COY_ID = IRSM_IRS_COY_ID AND LM_LOCATION_INDEX = ID_LOCATION_INDEX " &
                     "INNER JOIN COMPANY_SECTION ON CS_SEC_CODE = IRSM_IRS_SECTION AND CS_COY_ID = IRSM_IRS_COY_ID " &
                     "WHERE IRSM_IRS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (IRSM_IRS_STATUS = 3 OR IRSM_IRS_STATUS = 4) " &
                     "AND IRSM_IRS_NO " & Common.ParseSQL(strTemp) & ""

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "INVENTORY_REQUISITION_SLIP_MSTR"
            ds.Tables(1).TableName = "INVENTORY_REQUISITION_SLIP_DETAILS"
            Return ds
        End Function

        Public Function RISubmit(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strLoginUserName As String, ByVal dsRI As DataSet, ByRef strRINo As String)
            Dim strPrefix, SqlQuery As String
            Dim strAryQuery(0), strNewRINo As String
            Dim intIncrementNo As Integer = 0
            Dim aryItem As New ArrayList()
            Dim dteNow As DateTime = Now()
            Dim dsApp As DataSet
            Dim i, j As Integer
            Dim blnDetail As Boolean
            Dim aryDetails As New ArrayList()
            Dim strStoreMgr() As String


            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'RI' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strRINo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'RI' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO INVENTORY_RETURN_INWARD_MSTR(IRIM_RI_COY_ID, IRIM_RI_NO, IRIM_IR_NO, " &
                       "IRIM_RI_DATE, IRIM_RI_STATUS, IRIM_CREATED_BY, IRIM_CREATED_DATE) " &
                       "VALUES('" & strCoyID & "', " & strRINo & ", '" & dsRI.Tables(0).Rows(0)("IRIM_IR_NO") & "', " &
                       "" & Common.ConvertDate(dteNow) & ", '1', '" & strLoginUser & "', " & Common.ConvertDate(dteNow) & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            Dim intLine As Integer = 0

            For i = 0 To dsRI.Tables(1).Rows.Count - 1
                If aryDetails Is Nothing Then
                    aryDetails.Add(New String() {dsRI.Tables(1).Rows(i)("IRID_INVENTORY_INDEX"), dsRI.Tables(1).Rows(i)("IRID_INVENTORY_NAME"), dsRI.Tables(1).Rows(i)("IRID_QTY"), intLine, dsRI.Tables(1).Rows(i)("IRID_IR_LINE"), dsRI.Tables(1).Rows(i)("IRID_UOM")})
                Else
                    If aryDetails.Count > 0 Then
                        blnDetail = False
                        For j = 0 To aryDetails.Count - 1
                            If aryDetails(j)(0) = dsRI.Tables(1).Rows(i)("IRID_INVENTORY_INDEX") And aryDetails(j)(4) = dsRI.Tables(1).Rows(i)("IRID_IR_LINE") Then
                                aryDetails(j)(2) = CDec(aryDetails(j)(2)) + CDec(dsRI.Tables(1).Rows(i)("IRID_QTY"))
                                blnDetail = True
                                Exit For
                            End If
                        Next

                        If blnDetail = False Then
                            intLine = intLine + 1
                            aryDetails.Add(New String() {dsRI.Tables(1).Rows(i)("IRID_INVENTORY_INDEX"), dsRI.Tables(1).Rows(i)("IRID_INVENTORY_NAME"), dsRI.Tables(1).Rows(i)("IRID_QTY"), intLine, dsRI.Tables(1).Rows(i)("IRID_IR_LINE"), dsRI.Tables(1).Rows(i)("IRID_UOM")})
                        End If
                    Else
                        intLine = intLine + 1
                        aryDetails.Add(New String() {dsRI.Tables(1).Rows(i)("IRID_INVENTORY_INDEX"), dsRI.Tables(1).Rows(i)("IRID_INVENTORY_NAME"), dsRI.Tables(1).Rows(i)("IRID_QTY"), intLine, dsRI.Tables(1).Rows(i)("IRID_IR_LINE"), dsRI.Tables(1).Rows(i)("IRID_UOM")})
                    End If
                End If
            Next

            Dim dtRIDtls As DataTable
            Dim drRIDtl As DataRow

            dtRIDtls = dsRI.Tables(1)

            'Insert record into RI detail table
            For i = 0 To aryDetails.Count - 1
                SqlQuery = "INSERT INTO INVENTORY_RETURN_INWARD_DETAILS(IRID_RI_INDEX, IRID_RI_COY_ID, IRID_RI_NO, IRID_RI_LINE, IRID_IR_LINE, " &
                        "IRID_INVENTORY_INDEX, IRID_INVENTORY_NAME, IRID_QTY, IRID_UOM) " &
                        "VALUES(LAST_INSERT_ID(), '" & strCoyID & "', " & strRINo & ", '" & aryDetails(i)(3) & "', '" & aryDetails(i)(4) & "', " &
                        "'" & aryDetails(i)(0) & "', '" & Common.Parse(aryDetails(i)(1)) & "', " & aryDetails(i)(2) & ", '" & Common.Parse(aryDetails(i)(5)) & "')"
                Common.Insert2Ary(strAryQuery, SqlQuery)

                'Insert record into RI lot table
                For j = 0 To dsRI.Tables(1).Rows.Count - 1
                    If aryDetails(i)(0) = dsRI.Tables(1).Rows(j)("IRID_INVENTORY_INDEX") And aryDetails(i)(4) = dsRI.Tables(1).Rows(j)("IRID_IR_LINE") Then
                        SqlQuery = " INSERT INTO INVENTORY_RETURN_INWARD_LOT(IRIL_RI_COY_ID, IRIL_RI_NO, IRIL_RI_LINE, IRIL_LOCATION_INDEX, IRIL_LOT_INDEX, IRIL_LOT_QTY, IRIL_REMARK) " &
                                " VALUES('" & strCoyID & "', " & strRINo & ", '" & aryDetails(i)(3) & "', '" & dsRI.Tables(1).Rows(j)("IRID_LOCATION_INDEX") & "', '" & dsRI.Tables(1).Rows(j)("IRID_LOT_INDEX") & "', " & dsRI.Tables(1).Rows(j)("IRID_QTY") & ",'" & Common.Parse(dsRI.Tables(1).Rows(j)("IRID_REMARK")) & "') "
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If
                Next
            Next

            'Update return Qty to MRS detail / MRS Lot table
            For i = 0 To dsRI.Tables(1).Rows.Count - 1
                SqlQuery = "UPDATE INVENTORY_REQUISITION_SLIP_MSTR M, INVENTORY_REQUISITION_SLIP_DETAILS D " &
                        "SET D.IRSD_RETURN_QTY = D.IRSD_RETURN_QTY + " & dsRI.Tables(1).Rows(i)("IRID_QTY") & " " &
                        "WHERE M.IRSM_IRS_COY_ID = D.IRSD_IRS_COY_ID AND M.IRSM_IRS_NO = D.IRSD_IRS_NO " &
                        "AND D.IRSD_INVENTORY_INDEX = '" & dsRI.Tables(1).Rows(i)("IRID_INVENTORY_INDEX") & "' " &
                        "AND D.IRSD_IRS_LINE = '" & dsRI.Tables(1).Rows(i)("IRID_IR_LINE") & "' " &
                        "AND D.IRSD_IRS_COY_ID = '" & strCoyID & "' AND M.IRSM_IRS_NO = '" & dsRI.Tables(0).Rows(0)("IRIM_IR_NO") & "' "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = "UPDATE INVENTORY_REQUISITION_SLIP_MSTR M, INVENTORY_REQUISITION_SLIP_DETAILS D, INVENTORY_REQUISITION_SLIP_LOT L " &
                        "SET L.IRSL_LOT_RETURN_QTY = L.IRSL_LOT_RETURN_QTY + " & dsRI.Tables(1).Rows(i)("IRID_QTY") & " " &
                        "WHERE M.IRSM_IRS_COY_ID = D.IRSD_IRS_COY_ID AND M.IRSM_IRS_NO = D.IRSD_IRS_NO " &
                        "AND L.IRSL_IRS_COY_ID = D.IRSD_IRS_COY_ID AND L.IRSL_IRS_NO = D.IRSD_IRS_NO AND L.IRSL_IRS_LINE = D.IRSD_IRS_LINE " &
                        "AND D.IRSD_INVENTORY_INDEX = '" & dsRI.Tables(1).Rows(i)("IRID_INVENTORY_INDEX") & "' " &
                        "AND L.IRSL_IRS_LINE = '" & dsRI.Tables(1).Rows(i)("IRID_IR_LINE") & "' " &
                        "AND L.IRSL_LOCATION_INDEX = '" & dsRI.Tables(1).Rows(i)("IRID_LOCATION_INDEX") & "'" &
                        "AND D.IRSD_IRS_COY_ID = '" & strCoyID & "' AND IRSL_LOT_INDEX = '" & dsRI.Tables(1).Rows(i)("IRID_LOT_INDEX") & "' AND M.IRSM_IRS_NO = '" & dsRI.Tables(0).Rows(0)("IRIM_IR_NO") & "' "
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strCoyID, strLoginUser, strAryQuery, WheelModule.RIMod, WheelUserActivity.REQ_SubmitRI, strRINo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strRINo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'RI' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewRINo, "T_NO") Then
                strRINo = strNewRINo
                RISubmit = True

                '//only send mail if transaction successfully created
                Dim objMail As New Email
                Dim strUserid, strIRIndex, strMRSIndex As String

                SqlQuery = "SELECT IRSM_IRS_INDEX FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = '" & strCoyID & "' AND IRSM_IRS_NO = '" & dsRI.Tables(0).Rows(0)("IRIM_IR_NO") & "'"
                strMRSIndex = objDb.GetVal(SqlQuery)

                SqlQuery = "SELECT IRM_IR_INDEX FROM INVENTORY_REQUISITION_MSTR, INVENTORY_REQUISITION_DETAILS " &
                        "WHERE IRD_IR_COY_ID = IRM_IR_COY_ID And IRD_IR_NO = IRM_IR_NO AND IRD_IR_SLIP_INDEX = '" & strMRSIndex & "'"
                strIRIndex = objDb.GetVal(SqlQuery)

                'Send email to SK
                SqlQuery = "SELECT IRSM_BUYER_ID FROM inventory_requisition_slip_mstr WHERE IRSM_IRS_COY_ID = '" & strCoyID & "' AND IRSM_IRS_NO = '" & dsRI.Tables(0).Rows(0)("IRIM_IR_NO") & "'"
                strUserid = objDb.GetVal(SqlQuery)
                objMail.sendNotification(EmailType.RICreated, strUserid, strCoyID, "", strRINo, "", "sk", strLoginUserName)

                'Send email to HOD
                SqlQuery = "SELECT IRA_AO, IRA_A_AO, IRA_APPROVAL_GRP_INDEX FROM ir_approval WHERE IRA_IR_INDEX =" & strIRIndex
                dsApp = objDb.FillDs(SqlQuery)

                For i = 0 To dsApp.Tables(0).Rows.Count - 1
                    objMail.sendNotification(EmailType.RICreated, dsApp.Tables(0).Rows(i)("IRA_AO"), strCoyID, "", strRINo, "", "hod", strLoginUserName)
                Next

                'Send email to Store Manager
                SqlQuery = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_GRP_INDEX =" & dsApp.Tables(0).Rows(0)("IRA_APPROVAL_GRP_INDEX")
                strUserid = objDb.GetVal(SqlQuery)
                strUserid = Replace(strUserid, " ", "")
                strStoreMgr = Split(strUserid, ";")

                For i = 0 To strStoreMgr.Length - 1
                    If System.Text.RegularExpressions.Regex.IsMatch(strStoreMgr(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                        sendMailToStoreMgr(strRINo, strStoreMgr(i), strLoginUserName, , "RI")
                    End If
                Next
            Else
                RISubmit = False
            End If
        End Function

        Public Function RIList(ByVal strUserId As String, ByVal strCoyID As String, ByVal strRINo As String, ByVal strMRSNo As String, ByVal strStartDate As String, ByVal strEndDate As String, ByVal strStatus As String, Optional ByVal strRole As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Dim strTemp As String

            strSql = "SELECT IRIM_RI_NO, IRIM_RI_DATE, IRID_QTY AS TQTY, IRIM_RI_REJECT_REMARK, " &
                     "(SELECT IRSM_IRS_ACCOUNT_CODE FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = IRIM_RI_COY_ID AND IRSM_IRS_NO = IRIM_IR_NO) AS ACCCODE, IRIM_IR_NO, " &
                     "(SELECT STATUS_REMARK FROM STATUS_MSTR WHERE STATUS_TYPE = 'RI' AND M.IRIM_RI_STATUS = STATUS_NO) AS IRIM_RI_STATUS, D.*, INVENTORY_MSTR.*, INVENTORY_DETAIL.*" &
                     "FROM INVENTORY_RETURN_INWARD_MSTR M INNER JOIN INVENTORY_RETURN_INWARD_DETAILS D " &
                     "INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRIL_RI_COY_ID = D.IRID_RI_COY_ID AND IRIL_RI_NO = D.IRID_RI_NO AND IRIL_RI_LINE = D.IRID_RI_LINE " &
                     "INNER JOIN INVENTORY_MSTR ON IM_COY_ID = M.IRIM_RI_COY_ID AND IM_INVENTORY_INDEX = D.IRID_INVENTORY_INDEX " &
                     "INNER JOIN INVENTORY_DETAIL ON ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND ID_LOCATION_INDEX = IRIL_LOCATION_INDEX " &
                     "WHERE M.IRIM_RI_COY_ID = D.IRID_RI_COY_ID AND M.IRIM_RI_NO = D.IRID_RI_NO AND M.IRIM_RI_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strRole = "req" Then
                strSql &= "AND M.IRIM_CREATED_BY = '" & strUserId & "' "
            ElseIf strRole = "sk" Then
                strSql &= "AND M.IRIM_STATUS_CHANGED_BY = '" & strUserId & "' "
            End If

            ' "INNER JOIN LOCATION_MSTR ON LM_COY_ID = M.IRIM_RI_COY_ID AND LM_LOCATION_INDEX = D.IRID_LOCATION_INDEX " & _

            If strMRSNo <> "" Then
                strTemp = Common.BuildWildCard(strMRSNo)
                strSql = strSql & " AND IRIM_IR_NO" & Common.ParseSQL(strTemp)
            End If

            If strRINo <> "" Then
                strTemp = Common.BuildWildCard(strRINo)
                strSql = strSql & " AND IRIM_RI_NO" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                strSql = strSql & " AND IRIM_RI_DATE >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                strSql = strSql & " AND IRIM_RI_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            If strStatus <> "" Then
                strSql = strSql & " AND IRIM_RI_STATUS IN (" & strStatus & ")"
            End If

            strSql = strSql & "GROUP BY IRIM_RI_NO, IRIM_RI_DATE, IRIM_RI_STATUS, IRIM_IR_NO "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function RIInfo(ByVal strRINo) As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strTemp = Common.BuildWildCard(strRINo)

            strSql = " SELECT IRIM_RI_NO, IRIM_RI_DATE, IRID_UOM, IRID_QTY AS TQTY, IRIM_RI_REJECT_REMARK, " &
                     " (SELECT IRSM_IRS_ACCOUNT_CODE FROM INVENTORY_REQUISITION_SLIP_MSTR WHERE IRSM_IRS_COY_ID = IRIM_RI_COY_ID AND IRSM_IRS_NO = IRIM_IR_NO) AS ACCCODE, IRIM_IR_NO, " &
                     " (SELECT STATUS_REMARK FROM STATUS_MSTR WHERE STATUS_TYPE = 'RI' AND M.IRIM_RI_STATUS = STATUS_NO) AS IRIM_RI_STATUS, D.*, INVENTORY_MSTR.*, INVENTORY_DETAIL.*, LOCATION_MSTR.*, INVENTORY_RETURN_INWARD_LOT.*, DO_LOT.* " &
                     " FROM INVENTORY_RETURN_INWARD_MSTR M INNER JOIN INVENTORY_RETURN_INWARD_DETAILS D " &
                     " INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRIL_RI_COY_ID = D.IRID_RI_COY_ID AND IRIL_RI_NO = D.IRID_RI_NO AND IRIL_RI_LINE = D.IRID_RI_LINE " &
                     " INNER JOIN INVENTORY_MSTR ON IM_COY_ID = M.IRIM_RI_COY_ID AND IM_INVENTORY_INDEX = D.IRID_INVENTORY_INDEX " &
                     " INNER JOIN INVENTORY_DETAIL ON ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND ID_LOCATION_INDEX = IRIL_LOCATION_INDEX " &
                     " INNER JOIN LOCATION_MSTR ON LM_COY_ID = M.IRIM_RI_COY_ID AND LM_LOCATION_INDEX = IRIL_LOCATION_INDEX " &
                     " INNER JOIN DO_LOT ON DOL_LOT_INDEX = IRIL_LOT_INDEX " &
                     " WHERE M.IRIM_RI_COY_ID = D.IRID_RI_COY_ID AND M.IRIM_RI_NO = D.IRID_RI_NO AND M.IRIM_RI_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                     " AND IRIM_RI_NO " & Common.ParseSQL(strTemp) & " "
            ' "GROUP BY IRIM_RI_NO, IRIM_RI_DATE, IRIM_RI_STATUS, IRIM_IR_NO; "

            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "INVENTORY_RETURN_INWARD_MSTR_DETAILS"

            Return ds
        End Function

        Public Function RIAck(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal dsRI As DataSet, ByRef strRINo As String, Optional ByVal strRemarkCR As String = "")
            Dim strPrefix, SqlQuery As String
            Dim strAryQuery(0), strNewRINo, strGRNNo, strPOLine As String
            Dim intIncrementNo As Integer = 0
            Dim aryItem As New ArrayList()
            Dim dteNow As DateTime = Now()
            Dim dsQty As DataSet
            Dim i As Integer

            SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                       " SELECT IRID_INVENTORY_INDEX, 'RI' , IRIL_LOT_QTY, IRIM_RI_DATE, IRIL_LOCATION_INDEX, IRIM_RI_NO, IRIM_CREATED_BY, IRID_INVENTORY_NAME " &
                       " FROM INVENTORY_RETURN_INWARD_MSTR " &
                       " INNER JOIN INVENTORY_RETURN_INWARD_DETAILS ON IRIM_RI_COY_ID = IRID_RI_COY_ID AND " &
                       " IRIM_RI_NO = IRID_RI_NO And IRIM_RI_INDEX = IRID_RI_INDEX " &
                       " INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRIL_RI_COY_ID = IRID_RI_COY_ID AND IRIL_RI_NO = IRID_RI_NO AND IRIL_RI_LINE = IRID_RI_LINE " &
                       " WHERE IRIM_RI_NO = '" & strRINo & "' AND IRIM_RI_COY_ID = '" & strCoyID & "' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "SELECT IRIM_IR_NO, IRIM_RI_COY_ID, IRID_INVENTORY_INDEX, IRID_IR_LINE, " &
                    "IRIL_LOT_QTY, IRIL_LOCATION_INDEX, IRIL_LOT_INDEX " &
                    "FROM INVENTORY_RETURN_INWARD_MSTR " &
                    "INNER JOIN INVENTORY_RETURN_INWARD_DETAILS ON IRIM_RI_COY_ID = IRID_RI_COY_ID AND IRIM_RI_NO = IRID_RI_NO " &
                    "INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRIL_RI_COY_ID = IRID_RI_COY_ID AND IRIL_RI_NO = IRID_RI_NO AND IRID_RI_LINE = IRIL_RI_LINE " &
                    "WHERE IRIM_RI_NO = '" & strRINo & "' AND IRIM_RI_COY_ID = '" & strCoyID & "' "
            dsQty = objDb.FillDs(SqlQuery)

            For i = 0 To dsQty.Tables(0).Rows.Count - 1
                SqlQuery = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & dsQty.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                        "WHERE ID_INVENTORY_INDEX = '" & dsQty.Tables(0).Rows(i)("IRID_INVENTORY_INDEX") & "' " &
                        "AND ID_LOCATION_INDEX =  '" & dsQty.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "' "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY + " & dsQty.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                        "WHERE IL_INVENTORY_INDEX = '" & dsQty.Tables(0).Rows(i)("IRID_INVENTORY_INDEX") & "' " &
                        "AND IL_LOT_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOT_INDEX") & "' " &
                        "AND IL_LOCATION_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "' "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                'Find out GRN number
                SqlQuery = "SELECT IRSL_GRN_NO FROM INVENTORY_REQUISITION_SLIP_LOT WHERE IRSL_IRS_LINE = '" & dsQty.Tables(0).Rows(i)("IRID_IR_LINE") & "' " &
                        "AND IRSL_IRS_NO = '" & dsQty.Tables(0).Rows(i)("IRIM_IR_NO") & "' AND IRSL_IRS_COY_ID = '" & dsQty.Tables(0).Rows(i)("IRIM_RI_COY_ID") & "' " &
                        "AND IRSL_LOT_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOT_INDEX") & "' AND IRSL_LOCATION_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "'"
                strGRNNo = objDb.GetVal(SqlQuery)

                If strGRNNo = "SYSTEM" Then
                    SqlQuery = "UPDATE DO_LOT SET DOL_OPN_QTY_ISSUE = DOL_OPN_QTY_ISSUE - " & dsQty.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                            "WHERE DOL_OPN_LOCATION_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "' " &
                            "AND DOL_LOT_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOT_INDEX") & "'"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Else
                    SqlQuery = "SELECT GL_PO_LINE FROM GRN_LOT " &
                            "WHERE GL_B_COY_ID = '" & dsQty.Tables(0).Rows(i)("IRIM_RI_COY_ID") & "' AND GL_GRN_NO = '" & strGRNNo & "' " &
                            "AND GL_LOCATION_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "' AND GL_LOT_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOT_INDEX") & "' "
                    strPOLine = objDb.GetVal(SqlQuery)

                    SqlQuery = "UPDATE GRN_LOT SET GL_LOT_RECEIVED_QTY_ISSUE = GL_LOT_RECEIVED_QTY_ISSUE - " & dsQty.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                            "WHERE GL_B_COY_ID = '" & dsQty.Tables(0).Rows(i)("IRIM_RI_COY_ID") & "' AND GL_GRN_NO = '" & strGRNNo & "' " &
                            "AND GL_LOCATION_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "' AND GL_LOT_INDEX = '" & dsQty.Tables(0).Rows(i)("IRIL_LOT_INDEX") & "'"
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    SqlQuery = "UPDATE GRN_DETAILS SET GD_RECEIVED_QTY_ISSUE = GD_RECEIVED_QTY_ISSUE - " & dsQty.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                            "WHERE GD_B_COY_ID = '" & dsQty.Tables(0).Rows(i)("IRIM_RI_COY_ID") & "' AND GD_GRN_NO = '" & strGRNNo & "' " &
                            "AND GD_PO_LINE = '" & strPOLine & "'"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
            Next

            SqlQuery = " UPDATE INVENTORY_RETURN_INWARD_MSTR SET " &
                       " IRIM_RI_STATUS = '2', " &
                       " IRIM_RI_APPROVED_DATE = " & Common.ConvertDate(dteNow) & ", " &
                       " IRIM_RI_REJECT_REMARK = '" & strRemarkCR & "', " &
                       " IRIM_STATUS_CHANGED_BY = '" & strLoginUser & "', " &
                       " IRIM_STATUS_CHANGED_ON = " & Common.ConvertDate(dteNow) & " " &
                       " WHERE IRIM_RI_NO = '" & strRINo & "' " &
                       " AND IRIM_RI_COY_ID = '" & strCoyID & "' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            ' Costing
            Dim INV_NAME, INV_CODE, INV_INDEX, MRS_NO, CREATED_BY As String
            Dim str_MRS_UPRICE As String
            Dim MRS_UPRICE, MRS_QTY As Decimal

            SqlQuery = " SELECT INVENTORY_RETURN_INWARD_MSTR.*, INVENTORY_RETURN_INWARD_DETAILS.*, INVENTORY_MSTR.* " &
                       " FROM INVENTORY_RETURN_INWARD_MSTR " &
                       " INNER JOIN INVENTORY_RETURN_INWARD_DETAILS ON IRIM_RI_COY_ID = IRID_RI_COY_ID AND IRIM_RI_NO = IRID_RI_NO " &
                       " INNER JOIN INVENTORY_MSTR ON IM_COY_ID = IRIM_RI_COY_ID AND IM_INVENTORY_INDEX = IRID_INVENTORY_INDEX " &
                       " WHERE IRIM_RI_NO = '" & strRINo & "' AND IRIM_RI_COY_ID = '" & strCoyID & "' "

            dsRI = objDb.FillDs(SqlQuery)

            If dsRI.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsRI.Tables(0).Rows.Count - 1
                    INV_CODE = dsRI.Tables(0).Rows(i).Item("IM_ITEM_CODE")
                    INV_NAME = dsRI.Tables(0).Rows(i).Item("IRID_INVENTORY_NAME")
                    MRS_NO = dsRI.Tables(0).Rows(i).Item("IRIM_IR_NO")
                    MRS_QTY = dsRI.Tables(0).Rows(i).Item("IRID_QTY")
                    CREATED_BY = dsRI.Tables(0).Rows(i).Item("IRIM_CREATED_BY")

                    SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                               " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & INV_CODE & "'"
                    INV_INDEX = objDb.GetVal(SqlQuery)

                    SqlQuery = " SELECT IC_COST_UPRICE FROM INVENTORY_COST " &
                               " WHERE IC_INVENTORY_INDEX = '" & INV_INDEX & "' AND IC_INVENTORY_TYPE = 'II' AND IC_INVENTORY_REF_DOC = '" & MRS_NO & "' AND IC_COY_ID = '" & strCoyID & "' "
                    str_MRS_UPRICE = objDb.GetVal(SqlQuery)

                    If str_MRS_UPRICE = "" Then
                        MRS_UPRICE = 0
                    Else
                        MRS_UPRICE = CDec(str_MRS_UPRICE)
                    End If
                    'SqlQuery = " SELECT IC_COST_QTY FROM INVENTORY_COST " & _
                    '   " WHERE IC_INVENTORY_INDEX = '" & INV_INDEX & "' AND IC_INVENTORY_TYPE = 'II' AND IC_INVENTORY_REF_DOC = '" & MRS_NO & "' AND IC_COY_ID = '" & strCoyID & "' "
                    'MRS_QTY = objDb.GetVal(SqlQuery)

                    'If MRS_UPRICE <> "" Then
                    SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                           "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                           "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(dteNow) & ", 'RI', '" & strRINo & "', '" & INV_INDEX & "', " &
                           "'" & INV_NAME & "', '" & strCoyID & "', " &
                           "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & MRS_QTY & ", " & MRS_UPRICE & ", " & MRS_QTY & "*" & MRS_UPRICE & ", " &
                           "IC_COST_CLOSE_QTY+" & MRS_QTY & "," &
                           "(IC_COST_CLOSE_COST+(" & MRS_QTY & "*" & MRS_UPRICE & ")) / (IC_COST_CLOSE_QTY+" & MRS_QTY & ")," &
                           "IC_COST_CLOSE_COST+(" & MRS_QTY & "*" & MRS_UPRICE & ") " &
                           "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & INV_INDEX & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                    'End If
                Next
            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strAryQuery, WheelModule.RIMod, WheelUserActivity.SK_AckRI, strRINo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                RIAck = True
                Dim objMail As New Email
                objMail.sendNotification(EmailType.RIAcknowledged, strLoginUser, strCoyID, "", strRINo, "", HttpContext.Current.Session("UserName"), CREATED_BY)
                objMail = Nothing
            Else
                RIAck = False
            End If
        End Function

        Public Function RIRej(ByVal dsRI As DataSet, ByRef strRINo As String, ByVal strRemark As String)
            Dim strPrefix, SqlQuery, strCoyID, strLoginUser As String
            Dim strAryQuery(0), strNewRINo As String
            Dim intIncrementNo As Integer = 0
            Dim i As Integer
            Dim aryItem As New ArrayList()
            Dim dteNow As DateTime = Now()
            Dim dsReturn As New DataSet
            Dim CREATED_BY As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            SqlQuery = "SELECT IRIM_RI_COY_ID, IRIM_IR_NO, IRID_IR_LINE, IRID_INVENTORY_INDEX, IRIL_LOCATION_INDEX, IRIL_LOT_INDEX, IRIL_LOT_QTY " &
                    "FROM INVENTORY_RETURN_INWARD_MSTR " &
                    "INNER JOIN INVENTORY_RETURN_INWARD_DETAILS ON IRIM_RI_COY_ID = IRID_RI_COY_ID AND IRIM_RI_NO = IRID_RI_NO " &
                    "INNER JOIN INVENTORY_RETURN_INWARD_LOT ON IRID_RI_COY_ID = IRIL_RI_COY_ID AND IRID_RI_NO = IRIL_RI_NO AND IRID_RI_LINE = IRIL_RI_LINE " &
                    "WHERE IRIM_RI_NO = '" & strRINo & "' AND IRIM_RI_COY_ID = '" & strCoyID & "'"
            dsReturn = objDb.FillDs(SqlQuery)

            For i = 0 To dsReturn.Tables(0).Rows.Count - 1
                SqlQuery = " UPDATE INVENTORY_REQUISITION_SLIP_DETAILS SET " &
                           " IRSD_RETURN_QTY = IRSD_RETURN_QTY - " & dsReturn.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                           " WHERE IRSD_IRS_COY_ID = '" & dsReturn.Tables(0).Rows(i)("IRIM_RI_COY_ID") & "' " &
                           " AND IRSD_IRS_NO = '" & dsReturn.Tables(0).Rows(i)("IRIM_IR_NO") & "' " &
                           " AND IRSD_IRS_LINE = '" & dsReturn.Tables(0).Rows(i)("IRID_IR_LINE") & "'"
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = " UPDATE INVENTORY_REQUISITION_SLIP_LOT SET " &
                           " IRSL_LOT_RETURN_QTY = IRSL_LOT_RETURN_QTY - " & dsReturn.Tables(0).Rows(i)("IRIL_LOT_QTY") & " " &
                           " WHERE IRSL_IRS_COY_ID = '" & dsReturn.Tables(0).Rows(i)("IRIM_RI_COY_ID") & "' " &
                           " AND IRSL_IRS_NO = '" & dsReturn.Tables(0).Rows(i)("IRIM_IR_NO") & "' " &
                           " AND IRSL_IRS_LINE = '" & dsReturn.Tables(0).Rows(i)("IRID_IR_LINE") & "' " &
                           " AND IRSL_LOCATION_INDEX = '" & dsReturn.Tables(0).Rows(i)("IRIL_LOCATION_INDEX") & "' " &
                           " AND IRSL_LOT_INDEX = '" & dsReturn.Tables(0).Rows(i)("IRIL_LOT_INDEX") & "'"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Next

            SqlQuery = " UPDATE INVENTORY_RETURN_INWARD_MSTR SET " &
                       " IRIM_RI_STATUS = '3', " &
                       " IRIM_RI_APPROVED_DATE = " & Common.ConvertDate(dteNow) & ", " &
                       " IRIM_RI_REJECT_REMARK = '" & Common.Parse(strRemark) & "', " &
                       " IRIM_STATUS_CHANGED_BY = '" & strLoginUser & "',  " &
                       " IRIM_STATUS_CHANGED_ON = " & Common.ConvertDate(dteNow) & " " &
                       " WHERE IRIM_RI_NO = '" & strRINo & "' " &
                       " AND IRIM_RI_COY_ID = '" & strCoyID & "' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = " SELECT IRIM_CREATED_BY " &
                       " FROM INVENTORY_RETURN_INWARD_MSTR " &
                       " INNER JOIN INVENTORY_RETURN_INWARD_DETAILS ON IRIM_RI_COY_ID = IRID_RI_COY_ID AND IRIM_RI_NO = IRID_RI_NO AND IRIM_RI_INDEX = IRID_RI_INDEX " &
                       " WHERE IRIM_RI_NO = '" & strRINo & "' AND IRIM_RI_COY_ID = '" & strCoyID & "' "
            CREATED_BY = objDb.GetVal(SqlQuery)

            Dim objUsers As New Users
            objUsers.Log_UserActivity(strLoginUser, strCoyID, strAryQuery, WheelModule.RIMod, WheelUserActivity.SK_RejectRI, strRINo)
            objUsers = Nothing

            If objDb.BatchExecute(strAryQuery) Then
                RIRej = True
                Dim objMail As New Email
                objMail.sendNotification(EmailType.RIRejected, strLoginUser, strCoyID, "", strRINo, "", HttpContext.Current.Session("UserName"), CREATED_BY)
                objMail = Nothing
            Else
                RIRej = False
            End If
        End Function

        Public Function ROList(ByVal strRONo As String, ByVal strVendorName As String, ByVal strItemCode As String, ByVal strStartDate As String, ByVal strEndDate As String) As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT * " &
                     "FROM INVENTORY_RETURN_OUTWARD_MSTR INNER JOIN INVENTORY_RETURN_OUTWARD_DETAILS " &
                     "INNER JOIN INVENTORY_MSTR ON IROD_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                     "INNER JOIN GRN_MSTR ON IROM_GRN_NO = GM_GRN_NO AND IROM_RO_COY_ID = GM_B_COY_ID " &
                     "INNER JOIN COMPANY_MSTR ON GM_S_COY_ID = CM_COY_ID " &
                     "WHERE IROM_RO_COY_ID = IROD_RO_COY_ID AND IROM_RO_NO = IROD_RO_NO AND IROM_RO_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strRONo <> "" Then
                strTemp = Common.BuildWildCard(strRONo)
                strSql = strSql & " AND IROM_RO_NO" & Common.ParseSQL(strTemp)
            End If

            If strVendorName <> "" Then
                strTemp = Common.BuildWildCard(strVendorName)
                strSql = strSql & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                strSql = strSql & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                strSql = strSql & " AND IROM_RO_DATE >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                strSql = strSql & " AND IROM_RO_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            strSql = strSql & " GROUP BY IROM_RO_NO, IROM_RO_DATE "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function WOList(ByVal strWONo As String, ByVal strStartDate As String, ByVal strEndDate As String, ByVal strStatus As String, ByVal strItemCode As String) As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT IWOM_WO_NO, IWOM_WO_DATE, SUM(IWOD_QTY_VAL) AS TQTY, IWOM_WO_REMARK, " &
                     "(SELECT STATUS_REMARK FROM STATUS_MSTR WHERE STATUS_TYPE = 'WO' AND M.IWOM_WO_STATUS = STATUS_NO) AS IWOM_WO_STATUS, D.*, INVENTORY_MSTR.*, INVENTORY_DETAIL.*, LOCATION_MSTR.* " &
                     "FROM INVENTORY_WRITE_OFF_MSTR M INNER JOIN INVENTORY_WRITE_OFF_DETAILS D " &
                     "INNER JOIN INVENTORY_MSTR ON IM_COY_ID = M.IWOM_WO_COY_ID AND IM_INVENTORY_INDEX = D.IWOD_INVENTORY_INDEX " &
                     "INNER JOIN INVENTORY_DETAIL ON ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND ID_LOCATION_INDEX = D.IWOD_FRM_LOCATION_INDEX " &
                     "INNER JOIN LOCATION_MSTR ON LM_COY_ID = M.IWOM_WO_COY_ID AND LM_LOCATION_INDEX = D.IWOD_FRM_LOCATION_INDEX " &
                     "WHERE M.IWOM_WO_COY_ID = D.IWOD_WO_COY_ID AND M.IWOM_WO_NO = D.IWOD_WO_NO AND M.IWOM_WO_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strWONo <> "" Then
                strTemp = Common.BuildWildCard(strWONo)
                strSql = strSql & " AND IWOM_WO_NO" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" Then
                strSql = strSql & " AND IWOM_WO_DATE >= " & Common.ConvertDate(strStartDate)
            End If

            If strEndDate <> "" Then
                strSql = strSql & " AND IWOM_WO_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000")
            End If

            If strStatus <> "" Then
                strSql = strSql & " AND IWOM_WO_STATUS IN (" & strStatus & ")"
            End If

            If strItemCode <> "" Then
                strTemp = Common.BuildWildCard(strItemCode)
                strSql = strSql & " AND IM_ITEM_CODE" & Common.ParseSQL(strTemp)
            End If

            strSql = strSql & " GROUP BY IWOM_WO_NO, IWOM_WO_DATE, IWOM_WO_REMARK "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function ROInfo(ByVal strRONo As String) As DataSet
            Dim strSql, strSqlM, strSqlD, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strTemp = Common.BuildWildCard(strRONo)

            strSqlM = " SELECT GRN_MSTR.*, CM_COY_NAME, POM_PO_NO, POM_CREATED_DATE, DOM_DO_NO, DOM_DO_DATE, INVENTORY_RETURN_OUTWARD_MSTR.* " &
                     " FROM GRN_MSTR " &
                     " INNER JOIN INVENTORY_RETURN_OUTWARD_MSTR ON IROM_GRN_NO = GM_GRN_NO AND IROM_RO_COY_ID = GM_B_COY_ID " &
                     " INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " &
                     " INNER JOIN COMPANY_MSTR ON CM_COY_ID = GM_S_COY_ID " &
                     " INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " &
                     " WHERE GM_B_COY_ID = '" & strCoyID & "' AND IROM_RO_NO = '" & strRONo & "' "

            'strSqlD = " SELECT GRN_MSTR.*, GRN_DETAILS.*, CM_COY_NAME, POM_PO_NO, POM_CREATED_DATE, DOM_DO_NO, DOM_DO_DATE, " & _
            '         " (GD_RECEIVED_QTY-GD_REJECTED_QTY) AS OUTSTANDING, PO_MSTR.*, PO_DETAILS.*, INVENTORY_RETURN_OUTWARD_MSTR.*, INVENTORY_RETURN_OUTWARD_DETAILS.* " & _
            '         " FROM GRN_MSTR " & _
            '         " INNER JOIN GRN_DETAILS ON GD_GRN_NO = GM_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID " & _
            '         " INNER JOIN INVENTORY_RETURN_OUTWARD_MSTR ON IROM_GRN_NO = GM_GRN_NO AND IROM_RO_COY_ID = GD_B_COY_ID " & _
            '         " INNER JOIN INVENTORY_RETURN_OUTWARD_DETAILS ON IROD_RO_NO = IROM_RO_NO AND IROD_RO_COY_ID = IROM_RO_COY_ID AND IROD_RO_LINE = GD_PO_LINE " & _
            '         " INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " & _
            '         " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND GD_PO_LINE = POD_PO_LINE " & _
            '         " INNER JOIN COMPANY_MSTR ON CM_COY_ID = GM_S_COY_ID " & _
            '         " INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " & _
            '         " WHERE GM_B_COY_ID = '" & strCoyID & "' AND IROM_RO_NO = '" & strRONo & "' "

            strSqlD = "SELECT GRN_MSTR.*, GRN_DETAILS.*, CM_COY_NAME, POM_PO_NO, POM_CREATED_DATE, DOM_DO_NO, DOM_DO_DATE, " &
                    "(GD_RECEIVED_QTY-GD_REJECTED_QTY) AS OUTSTANDING, PO_MSTR.*, PO_DETAILS.*, INVENTORY_RETURN_OUTWARD_MSTR.*, " &
                    "INVENTORY_RETURN_OUTWARD_DETAILS.* FROM INVENTORY_RETURN_OUTWARD_MSTR " &
                    "INNER JOIN INVENTORY_RETURN_OUTWARD_DETAILS ON IROD_RO_NO = IROM_RO_NO AND IROD_RO_COY_ID = IROM_RO_COY_ID " &
                    "INNER JOIN GRN_MSTR ON GM_GRN_NO = IROM_GRN_NO AND GM_B_COY_ID = IROM_RO_COY_ID " &
                    "INNER JOIN GRN_DETAILS ON GD_GRN_NO = GM_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID AND GD_PO_LINE = IROD_PO_LINE " &
                    "INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " &
                    "INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " &
                    "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND IROD_PO_LINE = POD_PO_LINE " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = GM_S_COY_ID " &
                    "WHERE IROM_RO_NO = '" & strRONo & "' AND IROM_RO_COY_ID = '" & strCoyID & "' "

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "INVENTORY_RETURN_OUTWARD_MSTR"
            ds.Tables(1).TableName = "INVENTORY_RETURN_OUTWARD_DETAILS"
            Return ds
        End Function

        'Public Function PopLotWItem(ByVal strItem As String) As DataSet
        '    Dim strSql As String
        '    Dim dsLot As DataSet

        '    strSql = " SELECT DISTINCT DOL_LOT_NO FROM INVENTORY_DETAIL, INVENTORY_MSTR, INVENTORY_LOT, DO_LOT " _
        '           & " WHERE  ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND " _
        '           & " ID_INVENTORY_INDEX = IL_INVENTORY_INDEX AND " _
        '           & " DOL_COY_ID = IM_COY_ID AND " _
        '           & " IL_LOT_INDEX = DOL_LOT_INDEX AND " _
        '           & " IM_ITEM_CODE = '" & strItem & "' AND " _
        '           & " IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
        '           & " ORDER BY DOL_LOT_NO "

        '    dsLot = objDb.FillDs(strSql)
        '    PopLotWItem = dsLot

        'End Function

        Public Function PopLotWItemLotLocation(ByVal strItem As String, Optional ByVal strLot As String = "", Optional ByVal strLocation As String = "") As DataSet
            Dim strSql, strDistinct As String
            Dim dsLocation As DataSet

            If strItem <> "" Then
                strDistinct = "DISTINCT DOL_LOT_NO"
            End If

            If strLot <> "" Then
                strDistinct = "DISTINCT LM_LOCATION"
            End If

            If strLocation <> "" Then
                strDistinct = "DISTINCT LM_SUB_LOCATION"
            End If

            strSql = " SELECT " & strDistinct & " FROM INVENTORY_DETAIL, INVENTORY_MSTR, INVENTORY_LOT, DO_LOT, LOCATION_MSTR " _
                   & " WHERE  ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND " _
                   & " ID_INVENTORY_INDEX = IL_INVENTORY_INDEX AND " _
                   & " IL_LOT_INDEX = DOL_LOT_INDEX AND ID_LOCATION_INDEX = IL_LOCATION_INDEX AND " _
                   & " ID_LOCATION_INDEX = LM_LOCATION_INDEX AND " _
                   & " IM_ITEM_CODE = '" & strItem & "' AND " _
                   & " IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " _
                   & " (LM_SUB_LOCATION <> '' OR LM_SUB_LOCATION IS NULL ) " _
                   & " AND (IL_LOT_QTY-IFNULL(IL_IQC_QTY,0)) > 0 "

            '& " DOL_COY_ID = IM_COY_ID AND " _

            If strLot <> "" Then
                strSql = strSql & " AND DOL_LOT_NO = '" & strLot & "' "
            End If

            If strLocation <> "" Then
                strSql = strSql & " AND LM_LOCATION = '" & strLocation & "' "
            End If

            'If strLocation <> "" Then
            '    strSql = strSql & " AND LM_LOCATION = '" & strLocation & "' AND (LM_SUB_LOCATION <> '' AND LM_SUB_LOCATION IS NOT NULL ) "
            'End If

            If strLocation <> "" Then
                strSql = strSql & " ORDER BY LM_SUB_LOCATION "
            Else
                If strLot <> "" Then
                    strSql = strSql & " ORDER BY LM_LOCATION_INDEX "
                Else
                    If strItem <> "" Then
                        strSql = strSql & " ORDER BY DOL_LOT_NO "
                    End If
                End If
            End If

            dsLocation = objDb.FillDs(strSql)
            PopLotWItemLotLocation = dsLocation

        End Function

        Public Function insertWO(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal aryWOItem As ArrayList, ByRef strWONo As String, ByRef Remark As String, ByRef Code As String, ByRef RetLot As String, ByRef RetLoc As String, ByRef RetSLoc As String) As Integer
            Dim strPrefix
            Dim SqlQuery As String
            Dim PM_INV_INDEX, PM_LOT_INDEX, PM_LOC_INDEX, ID_INVENTORY_QTY As String
            Dim strAryQuery(0) As String
            Dim i, k As Integer
            Dim ItemName As String
            Dim intIncrementNo As Integer = 0
            Dim strNewWONo As String = ""
            Dim aryTrans As New ArrayList()
            Dim aryItem As New ArrayList()
            Dim aryCost As New ArrayList()
            Dim blnFound As Boolean
            aryItem = aryWOItem
            Dim strUom As String
            Dim dteNow As DateTime = Now()

            'GetLatestDocNo
            'objGlobal.GetLatestDocNo("WO", strAryQuery, strWONo, strPrefix)
            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'WO' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strWONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                   & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                   & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'WO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            ' Group All Duplicate
            Dim tempItem, tempName, tempLot, tempLoc, tempSLoc As String
            'Dim tempQty As Integer = 0
            Dim tempQty As Decimal = 0
            Dim j As Integer
            For i = 0 To aryItem.Count - 1
                tempItem = aryItem(i)(0)
                tempName = aryItem(i)(1)
                tempLot = aryItem(i)(3)
                tempLoc = aryItem(i)(4)
                tempSLoc = aryItem(i)(5)
                For j = 0 To aryItem.Count - 1
                    If aryItem(j)(6) <> "Done" And tempItem = aryItem(j)(0) And tempLot = aryItem(j)(3) And tempLoc = aryItem(j)(4) And tempSLoc = aryItem(j)(5) Then
                        aryItem(j)(0) = ""
                        aryItem(j)(6) = "Done"
                        tempQty = tempQty + CDec(IIf(aryItem(j)(2) = "", 0, aryItem(j)(2)))
                    End If
                Next
                aryItem.Add(New String() {tempItem, tempName, tempQty, tempLot, tempLoc, tempSLoc, "Done"})
                tempQty = 0
            Next

            SqlQuery = "UPDATE COMPANY_DOC_ATTACHMENT SET CDA_DOC_NO = " & strWONo & " " &
                    "WHERE CDA_DOC_NO = '" & strCoyID & "_" & strLoginUser & "' AND CDA_COY_ID = '" & strCoyID & "' AND CDA_DOC_TYPE = 'WO'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "INSERT INTO INVENTORY_WRITE_OFF_MSTR(IWOM_WO_COY_ID, IWOM_WO_NO, IWOM_WO_DATE, IWOM_WO_REMARK, " &
                       "IWOM_WO_STATUS, IWOM_CREATED_BY, IWOM_CREATED_DATE) " &
                       "VALUES('" & strCoyID & "', " & strWONo & ", " & Common.ConvertDate(dteNow) & ", '" & Remark & "', " &
                       "'1', '" & strLoginUser & "', " & Common.ConvertDate(dteNow) & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            For i = 0 To aryItem.Count - 1
                If aryItem(i)(0) <> "---Select---" And aryItem(i)(0) <> "" And aryItem(i)(2) <> "0" Then
                    Code = aryItem(i)(0)

                    SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                               " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & Code & "'"
                    PM_INV_INDEX = objDb.GetVal(SqlQuery)

                    If Common.parseNull(aryItem(i)(5)) = "" Or Common.parseNull(aryItem(i)(5)) = "---Select---" Then
                        SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & aryItem(i)(4) & "' AND LM_SUB_LOCATION IS NULL "
                        PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                    Else
                        SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & aryItem(i)(4) & "' AND LM_SUB_LOCATION = '" & aryItem(i)(5) & "' "
                        PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                    End If

                    strUom = getInvItemUom(Code)

                    Dim dsWO As DataSet
                    'SqlQuery = " SELECT INVENTORY_LOT.* FROM INVENTORY_LOT " & _
                    '           " INNER JOIN DO_LOT ON DOL_LOT_INDEX = IL_LOT_INDEX AND DOL_LOT_NO = '" & aryItem(i)(3) & "' " & _
                    '           " WHERE IL_LOCATION_INDEX = '" & PM_LOC_INDEX & "' AND IL_INVENTORY_INDEX = '" & PM_INV_INDEX & "' AND IL_LOT_QTY > 0 " & _
                    '           " ORDER BY IL_LOT_INDEX ASC LIMIT 1 "

                    ' This will get FIFO lot from INVENTORY_LOT.
                    SqlQuery = " SELECT SUM(INVENTORY_LOT.IL_LOT_QTY) AS IL_LOT_QTY, DO_LOT.DOL_LOT_NO FROM INVENTORY_LOT " &
                               " INNER JOIN DO_LOT ON DOL_LOT_INDEX = IL_LOT_INDEX AND DOL_LOT_NO = '" & aryItem(i)(3) & "' " &
                               " WHERE IL_LOCATION_INDEX = '" & PM_LOC_INDEX & "' AND IL_INVENTORY_INDEX = '" & PM_INV_INDEX & "' AND (IL_LOT_QTY - IFNULL(IL_IQC_QTY, 0)) > 0 " &
                               " GROUP BY DOL_LOT_NO "
                    dsWO = objDb.FillDs(SqlQuery)

                    If dsWO.Tables(0).Rows.Count > 0 Then
                        For k = 0 To dsWO.Tables(0).Rows.Count - 1
                            If CDec(aryItem(i)(2)) > CDec(dsWO.Tables(0).Rows(k).Item("IL_LOT_QTY")) Then
                                insertWO = 10
                                RetLot = aryItem(i)(3)
                                RetLoc = aryItem(i)(4)
                                RetSLoc = aryItem(i)(5)
                                Exit Function
                                'Else
                                'PM_LOT_INDEX = dsWO.Tables(0).Rows(i).Item("IL_LOT_INDEX")
                            End If
                        Next
                    Else
                        insertWO = 10
                        RetLot = aryItem(i)(3)
                        RetLoc = aryItem(i)(4)
                        RetSLoc = aryItem(i)(5)
                        Exit Function
                    End If

                    'Chee Hong - enhancement on 27/09/2013
                    'Compare & Store transaction record into array before insert into trans table
                    If aryTrans Is Nothing Then
                        aryTrans.Add(New String() {PM_INV_INDEX, PM_LOC_INDEX, aryItem(i)(2), aryItem(i)(1)})
                    Else
                        If aryTrans.Count > 0 Then
                            blnFound = False
                            For j = 0 To aryTrans.Count - 1
                                If aryTrans(j)(0) = PM_INV_INDEX And aryTrans(j)(1) = PM_LOC_INDEX Then
                                    aryTrans(j)(2) = CDec(aryTrans(j)(2)) + CDec(aryItem(i)(2))
                                    blnFound = True
                                End If
                            Next

                            If blnFound = False Then
                                aryTrans.Add(New String() {PM_INV_INDEX, PM_LOC_INDEX, aryItem(i)(2), aryItem(i)(1)})
                            End If
                        Else
                            aryTrans.Add(New String() {PM_INV_INDEX, PM_LOC_INDEX, aryItem(i)(2), aryItem(i)(1)})
                        End If
                    End If

                    'Compare & Store transaction record into array before insert into cost table
                    If aryCost Is Nothing Then
                        aryCost.Add(New String() {PM_INV_INDEX, aryItem(i)(2), aryItem(i)(1)})
                    Else
                        If aryCost.Count > 0 Then
                            blnFound = False
                            For j = 0 To aryCost.Count - 1
                                If aryCost(j)(0) = PM_INV_INDEX Then
                                    aryCost(j)(1) = CDec(aryCost(j)(1)) + CDec(aryItem(i)(2))
                                    blnFound = True
                                End If
                            Next

                            If blnFound = False Then
                                aryCost.Add(New String() {PM_INV_INDEX, aryItem(i)(2), aryItem(i)(1)})
                            End If
                        Else
                            aryCost.Add(New String() {PM_INV_INDEX, aryItem(i)(2), aryItem(i)(1)})
                        End If
                    End If

                    SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & aryItem(i)(2) & " " &
                               " WHERE ID_INVENTORY_INDEX = '" & PM_INV_INDEX & "' " &
                               " AND ID_LOCATION_INDEX = '" & PM_LOC_INDEX & "' "
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    ' This will update using FIFO method from INVENTORY_LOT.
                    SqlQuery = " SELECT INVENTORY_LOT.* FROM INVENTORY_LOT " &
                               " INNER JOIN DO_LOT ON DOL_LOT_INDEX = IL_LOT_INDEX AND DOL_LOT_NO = '" & aryItem(i)(3) & "' " &
                               " WHERE IL_LOCATION_INDEX = '" & PM_LOC_INDEX & "' AND IL_INVENTORY_INDEX = '" & PM_INV_INDEX & "' AND (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0 " &
                               " ORDER BY IL_LOT_INDEX ASC "
                    dsWO = objDb.FillDs(SqlQuery)

                    Dim strWOQty As Decimal = 0.0
                    strWOQty = aryItem(i)(2)
                    If dsWO.Tables(0).Rows.Count > 0 Then
                        For k = 0 To dsWO.Tables(0).Rows.Count - 1
                            If strWOQty = 0 Then
                                Exit For
                            End If

                            PM_LOT_INDEX = dsWO.Tables(0).Rows(k).Item("IL_LOT_INDEX")

                            If CDec(strWOQty) >= CDec(dsWO.Tables(0).Rows(k).Item("IL_LOT_QTY")) Then
                                strWOQty = strWOQty - CDec(dsWO.Tables(0).Rows(k).Item("IL_LOT_QTY"))
                                SqlQuery = " UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & CDec(dsWO.Tables(0).Rows(k).Item("IL_LOT_QTY")) & " " &
                                           " WHERE IL_INVENTORY_INDEX = '" & PM_INV_INDEX & "' " &
                                           " AND IL_LOCATION_INDEX = '" & PM_LOC_INDEX & "' " &
                                           " AND IL_LOT_INDEX = '" & PM_LOT_INDEX & "' "
                                Common.Insert2Ary(strAryQuery, SqlQuery)

                                SqlQuery = " INSERT INTO INVENTORY_WRITE_OFF_DETAILS(IWOD_WO_COY_ID, IWOD_WO_NO, IWOD_WO_LINE, IWOD_WO_LOT_NO, " &
                                           " IWOD_INVENTORY_INDEX, IWOD_INVENTORY_NAME, IWOD_QTY_VAL, IWOD_FRM_LOCATION_INDEX, IWOD_LOT_INDEX, IWOD_UOM) " &
                                           " VALUES('" & strCoyID & "', " & strWONo & ", " & i + 1 & ", '" & aryItem(i)(3) & "', " &
                                           " '" & PM_INV_INDEX & "', '" & aryItem(i)(1) & "', '" & CDec(dsWO.Tables(0).Rows(k).Item("IL_LOT_QTY")) & "', '" & PM_LOC_INDEX & "', '" & PM_LOT_INDEX & "', '" & Common.Parse(strUom) & "') "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            Else
                                'strWOQty = strWOQty - CDec(dsWO.Tables(0).Rows(j).Item("IL_LOT_QTY"))
                                SqlQuery = " UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & CDec(strWOQty) & " " &
                                           " WHERE IL_INVENTORY_INDEX = '" & PM_INV_INDEX & "' " &
                                           " AND IL_LOCATION_INDEX = '" & PM_LOC_INDEX & "' " &
                                           " AND IL_LOT_INDEX = '" & PM_LOT_INDEX & "' "
                                Common.Insert2Ary(strAryQuery, SqlQuery)

                                SqlQuery = " INSERT INTO INVENTORY_WRITE_OFF_DETAILS(IWOD_WO_COY_ID, IWOD_WO_NO, IWOD_WO_LINE, IWOD_WO_LOT_NO, " &
                                           " IWOD_INVENTORY_INDEX, IWOD_INVENTORY_NAME, IWOD_QTY_VAL, IWOD_FRM_LOCATION_INDEX, IWOD_LOT_INDEX, IWOD_UOM) " &
                                           " VALUES('" & strCoyID & "', " & strWONo & ", " & i + 1 & ", '" & aryItem(i)(3) & "', " &
                                           " '" & PM_INV_INDEX & "', '" & aryItem(i)(1) & "', '" & CDec(strWOQty) & "', '" & PM_LOC_INDEX & "', '" & PM_LOT_INDEX & "', '" & Common.Parse(strUom) & "') "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                                Exit For
                            End If


                        Next
                    End If

                    'SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " & _
                    '       "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " & _
                    '       "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(dteNow) & ", 'WO', " & strWONo & ", '" & PM_INV_INDEX & "', " & _
                    '       "'" & Common.Parse(aryItem(i)(1)) & "', '" & strCoyID & "', " & _
                    '       "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & CDec(aryItem(i)(2)) & ", IC_COST_CLOSE_UPRICE, " & CDec(aryItem(i)(2)) & "*IC_COST_CLOSE_UPRICE, " & _
                    '       "IC_COST_CLOSE_QTY-" & CDec(aryItem(i)(2)) & "," & _
                    '       "IFNULL((IC_COST_CLOSE_COST-(" & CDec(aryItem(i)(2)) & "*IC_COST_CLOSE_UPRICE)) / (IC_COST_CLOSE_QTY-" & CDec(aryItem(i)(2)) & "),0)," & _
                    '       "IC_COST_CLOSE_COST-(" & CDec(aryItem(i)(2)) & "*IC_COST_CLOSE_UPRICE) " & _
                    '       "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & PM_INV_INDEX & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                    'Common.Insert2Ary(strAryQuery, SqlQuery)

                End If
            Next


            'Insert into trans table
            If aryTrans.Count > 0 Then
                For i = 0 To aryTrans.Count - 1
                    SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                            " VALUES ('" & aryTrans(i)(0) & "', 'WO', " & aryTrans(i)(2) & ", " & Common.ConvertDate(dteNow) & ", '" & aryTrans(i)(1) & "', " & strWONo & ", '" & strLoginUser & "', '" & Common.Parse(aryTrans(i)(3)) & "') "
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Next
            End If

            'Insert into cost table
            If aryCost.Count > 0 Then
                For i = 0 To aryCost.Count - 1
                    SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                            "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                            "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(dteNow) & ", 'WO', " & strWONo & ", '" & aryCost(i)(0) & "', " &
                            "'" & Common.Parse(aryCost(i)(2)) & "', '" & strCoyID & "', " &
                            "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & CDec(aryCost(i)(1)) & ", IC_COST_CLOSE_UPRICE, " & CDec(aryCost(i)(1)) & "*IC_COST_CLOSE_UPRICE, " &
                            "IC_COST_CLOSE_QTY-" & CDec(aryCost(i)(1)) & "," &
                            "IFNULL((IC_COST_CLOSE_COST-(" & CDec(aryCost(i)(1)) & "*IC_COST_CLOSE_UPRICE)) / (IC_COST_CLOSE_QTY-" & CDec(aryCost(i)(1)) & "),0)," &
                            "IC_COST_CLOSE_COST-(" & CDec(aryCost(i)(1)) & "*IC_COST_CLOSE_UPRICE) " &
                            "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & aryCost(i)(0) & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Next
            End If

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strLoginUser, strCoyID, strAryQuery, WheelModule.WOMod, WheelUserActivity.SK_SubmitWO, strWONo)
            objUsers = Nothing

            SqlQuery = " SET @T_NO = " & strWONo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'WO' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            If objDb.BatchExecuteNew(strAryQuery, , strNewWONo, "T_NO") Then 'objDb.BatchExecute(strAryQuery) Then
                Dim dsEmail As New DataSet
                Dim aryEmail As New ArrayList
                Dim strAllEmail As String
                Dim strEmail() As String
                Dim blnEmailFound As Boolean

                strWONo = strNewWONo
                insertWO = WheelMsgNum.Save

                SqlQuery = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_TYPE = 'MRS' AND AGM_COY_ID = '" & strCoyID & "' AND AGM_MRS_EMAIL2 IS NOT NULL "
                dsEmail = objDb.FillDs(SqlQuery)

                For i = 0 To dsEmail.Tables(0).Rows.Count - 1
                    If dsEmail.Tables(0).Rows(i)("AGM_MRS_EMAIL2") <> "" Then
                        strAllEmail = dsEmail.Tables(0).Rows(i)("AGM_MRS_EMAIL2")
                        strAllEmail = Replace(strAllEmail, " ", "")
                        strEmail = Split(strAllEmail, ";")

                        For j = 0 To strEmail.Length - 1
                            If aryEmail Is Nothing Then
                                aryEmail.Add(strEmail(j))
                            Else
                                If aryEmail.Count > 0 Then
                                    blnEmailFound = False

                                    For k = 0 To aryEmail.Count - 1
                                        If strEmail(j) = aryEmail(k) Then
                                            blnEmailFound = True
                                            Exit For
                                        End If
                                    Next

                                    If blnEmailFound = False Then
                                        aryEmail.Add(strEmail(j))
                                    End If
                                Else
                                    aryEmail.Add(strEmail(j))
                                End If
                            End If
                        Next
                    End If
                Next

                If aryEmail.Count > 0 Then
                    For i = 0 To aryEmail.Count - 1
                        If System.Text.RegularExpressions.Regex.IsMatch(aryEmail(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                            sendMailToStoreMgr(strWONo, aryEmail(i), "", "I", "WO")
                            'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                        End If
                    Next
                End If

            Else
                insertWO = WheelMsgNum.NotSave
            End If
        End Function

        Public Function WOInfo(ByVal strWONo) As DataSet
            Dim strSql, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strTemp = Common.BuildWildCard(strWONo)

            strSql = "SELECT IWOM_WO_NO, IWOM_WO_DATE, IWOM_WO_REMARK, SUM(IWOD_QTY_VAL) AS QTY, IWOM_WO_STATUS, " &
                     "(SELECT STATUS_REMARK FROM STATUS_MSTR WHERE STATUS_TYPE = 'WO' AND M.IWOM_WO_STATUS = STATUS_NO) AS IWOM_WO_STATUS2, " &
                     "LOCATION_MSTR.LM_SUB_LOCATION, LOCATION_MSTR.LM_LOCATION, D.IWOD_WO_LOT_NO, D.IWOD_INVENTORY_NAME, D.IWOD_UOM, INVENTORY_MSTR.IM_ITEM_CODE " &
                     "FROM INVENTORY_WRITE_OFF_MSTR M INNER JOIN INVENTORY_WRITE_OFF_DETAILS D " &
                     "INNER JOIN INVENTORY_MSTR ON IM_COY_ID = M.IWOM_WO_COY_ID AND IM_INVENTORY_INDEX = D.IWOD_INVENTORY_INDEX " &
                     "INNER JOIN LOCATION_MSTR ON LM_COY_ID = M.IWOM_WO_COY_ID AND LM_LOCATION_INDEX = D.IWOD_FRM_LOCATION_INDEX " &
                     "WHERE M.IWOM_WO_COY_ID = D.IWOD_WO_COY_ID AND M.IWOM_WO_NO = D.IWOD_WO_NO AND M.IWOM_WO_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                     "AND IWOM_WO_NO " & Common.ParseSQL(strTemp) & " " &
                     "GROUP BY IWOM_WO_NO, IWOM_WO_DATE, IWOM_WO_STATUS, LOCATION_MSTR.LM_SUB_LOCATION, LOCATION_MSTR.LM_LOCATION, D.IWOD_WO_LOT_NO, D.IWOD_INVENTORY_NAME, INVENTORY_MSTR.IM_ITEM_CODE; "

            '"INNER JOIN INVENTORY_MSTR ON IM_COY_ID = M.IWOM_WO_COY_ID AND IM_INVENTORY_INDEX = D.IWOD_INVENTORY_INDEX " & _
            '"INNER JOIN INVENTORY_DETAIL ON ID_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _

            '"GROUP BY IWOM_WO_NO, IWOM_WO_DATE, IWOM_WO_STATUS; "

            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "INVENTORY_WRITE_OFF_MSTR_DETAILS"

            Return ds
        End Function

        Public Function WOCancel(ByVal dsWO As DataSet, ByRef strWONo As String)
            Dim strPrefix, SqlQuery, SqlUPQuery, strCoyID, strLoginUser, strLoginUserName As String
            Dim strAryQuery(0), strNewRINo As String
            Dim intIncrementNo As Integer = 0
            Dim aryItem As New ArrayList()
            Dim aryCost As New ArrayList()
            Dim dteNow As DateTime = Now()

            strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
            strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))
            strLoginUserName = Common.Parse(HttpContext.Current.Session("UserName"))

            SqlQuery = " UPDATE INVENTORY_WRITE_OFF_MSTR SET " &
                       " IWOM_WO_STATUS = '4', " &
                       " IWOM_STATUS_CHANGED_BY = '" & strLoginUser & "', " &
                       " IWOM_STATUS_CHANGED_ON = " & Common.ConvertDate(dteNow) & " " &
                       " WHERE IWOM_WO_NO = '" & strWONo & "'" &
                       " AND IWOM_WO_COY_ID = '" & strCoyID & "'"
            Common.Insert2Ary(strAryQuery, SqlQuery)

            Dim dsWO2 As DataSet
            Dim k, j As Integer
            Dim totalQty As Decimal
            Dim blnFound As Boolean

            SqlQuery = " SELECT * FROM INVENTORY_WRITE_OFF_MSTR " &
                       " INNER JOIN INVENTORY_WRITE_OFF_DETAILS ON IWOM_WO_COY_ID = IWOD_WO_COY_ID AND IWOM_WO_NO = IWOD_WO_NO " &
                       " WHERE IWOM_WO_NO = '" & strWONo & "' AND IWOM_WO_COY_ID = '" & strCoyID & "' "
            dsWO2 = objDb.FillDs(SqlQuery)

            If dsWO2.Tables(0).Rows.Count > 0 Then
                For k = 0 To dsWO2.Tables(0).Rows.Count - 1
                    SqlQuery = " UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY + " & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & " " &
                               " WHERE IL_INVENTORY_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_INVENTORY_INDEX") & "' " &
                               " AND IL_LOCATION_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_FRM_LOCATION_INDEX") & "' " &
                               " AND IL_LOT_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_LOT_INDEX") & "' "
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & " " &
                           " WHERE ID_INVENTORY_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_INVENTORY_INDEX") & "' " &
                           " AND ID_LOCATION_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_FRM_LOCATION_INDEX") & "' "
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    'Insert record into trans table
                    SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                           " VALUES ('" & dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_INDEX") & "', 'WOC', " & dsWO2.Tables(0).Rows(k).Item("IWOD_QTY_VAL") & ", " & Common.ConvertDate(dteNow) & ", '" & dsWO2.Tables(0).Rows(k).Item("IWOD_FRM_LOCATION_INDEX") & "', '" & strWONo & "', '" & strLoginUser & "', '" & dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_NAME") & "') "
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    'Compare & Store transaction record into array before insert into cost table
                    If aryCost Is Nothing Then
                        aryCost.Add(New String() {dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_INDEX"), dsWO2.Tables(0).Rows(k).Item("IWOD_QTY_VAL"), dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_NAME")})
                    Else
                        If aryCost.Count > 0 Then
                            blnFound = False
                            For j = 0 To aryCost.Count - 1
                                If aryCost(j)(0) = dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_INDEX") Then
                                    aryCost(j)(1) = CDec(aryCost(j)(1)) + CDec(dsWO2.Tables(0).Rows(k).Item("IWOD_QTY_VAL"))
                                    blnFound = True
                                End If
                            Next

                            If blnFound = False Then
                                aryCost.Add(New String() {dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_INDEX"), dsWO2.Tables(0).Rows(k).Item("IWOD_QTY_VAL"), dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_NAME")})
                            End If
                        Else
                            aryCost.Add(New String() {dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_INDEX"), dsWO2.Tables(0).Rows(k).Item("IWOD_QTY_VAL"), dsWO2.Tables(0).Rows(k).Item("IWOD_INVENTORY_NAME")})
                        End If
                    End If

                    'totalQty = totalQty + CDec(dsWO2.Tables(0).Rows(k).Item("IWOD_QTY_VAL"))

                    'SqlUPQuery = "(SELECT IC_COST_UPRICE FROM INVENTORY_COST WHERE IC_INVENTORY_REF_DOC = '" & strWONo & "' " & _
                    '            "AND IC_INVENTORY_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_INVENTORY_INDEX") & "' AND IC_INVENTORY_TYPE = 'WO')"

                    'SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " & _
                    '        "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " & _
                    '        "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(dteNow) & ", 'WOC', '" & strWONo & "', '" & Common.Parse(dsWO2.Tables(0).Rows(k)("IWOD_INVENTORY_INDEX")) & "', " & _
                    '        "'" & Common.Parse(dsWO2.Tables(0).Rows(k)("IWOD_INVENTORY_NAME")) & "', '" & strCoyID & "', " & _
                    '        "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & ", " & SqlUPQuery & ", " & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & "*" & SqlUPQuery & ", " & _
                    '        "IC_COST_CLOSE_QTY+" & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & "," & _
                    '        "(IC_COST_CLOSE_COST+(" & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & "*" & SqlUPQuery & ")) / (IC_COST_CLOSE_QTY+" & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & ")," & _
                    '        "IC_COST_CLOSE_COST+(" & CDec(dsWO2.Tables(0).Rows(k)("IWOD_QTY_VAL")) & "*" & SqlUPQuery & ") " & _
                    '        "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & dsWO2.Tables(0).Rows(k)("IWOD_INVENTORY_INDEX") & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                    'Common.Insert2Ary(strAryQuery, SqlQuery)



                Next

            End If

            If aryCost.Count > 0 Then
                For j = 0 To aryCost.Count - 1
                    SqlUPQuery = "(SELECT IC_COST_UPRICE FROM INVENTORY_COST WHERE IC_INVENTORY_REF_DOC = '" & strWONo & "' " &
                                "AND IC_INVENTORY_INDEX = '" & aryCost(j)(0) & "' AND IC_INVENTORY_TYPE = 'WO')"

                    SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                            "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                            "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(dteNow) & ", 'WOC', '" & strWONo & "', '" & aryCost(j)(0) & "', " &
                            "'" & Common.Parse(aryCost(j)(2)) & "', '" & strCoyID & "', " &
                            "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & CDec(aryCost(j)(1)) & ", " & SqlUPQuery & ", " & CDec(aryCost(j)(1)) & "*" & SqlUPQuery & ", " &
                            "IC_COST_CLOSE_QTY+" & CDec(aryCost(j)(1)) & "," &
                            "(IC_COST_CLOSE_COST+(" & CDec(aryCost(j)(1)) & "*" & SqlUPQuery & ")) / (IC_COST_CLOSE_QTY+" & CDec(aryCost(j)(1)) & ")," &
                            "IC_COST_CLOSE_COST+(" & CDec(aryCost(j)(1)) & "*" & SqlUPQuery & ") " &
                            "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & aryCost(j)(0) & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Next
            End If

            If objDb.BatchExecute(strAryQuery) Then
                Dim dsEmail As New DataSet
                Dim aryEmail As New ArrayList
                Dim strAllEmail As String
                Dim strEmail() As String
                Dim blnEmailFound As Boolean
                Dim i As Integer

                WOCancel = True

                SqlQuery = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_TYPE = 'MRS' AND AGM_COY_ID = '" & strCoyID & "' AND AGM_MRS_EMAIL2 IS NOT NULL "
                dsEmail = objDb.FillDs(SqlQuery)

                For i = 0 To dsEmail.Tables(0).Rows.Count - 1
                    If dsEmail.Tables(0).Rows(i)("AGM_MRS_EMAIL2") <> "" Then
                        strAllEmail = dsEmail.Tables(0).Rows(i)("AGM_MRS_EMAIL2")
                        strAllEmail = Replace(strAllEmail, " ", "")
                        strEmail = Split(strAllEmail, ";")

                        For j = 0 To strEmail.Length - 1
                            If aryEmail Is Nothing Then
                                aryEmail.Add(strEmail(j))
                            Else
                                If aryEmail.Count > 0 Then
                                    blnEmailFound = False

                                    For k = 0 To aryEmail.Count - 1
                                        If strEmail(j) = aryEmail(k) Then
                                            blnEmailFound = True
                                            Exit For
                                        End If
                                    Next

                                    If blnEmailFound = False Then
                                        aryEmail.Add(strEmail(j))
                                    End If
                                Else
                                    aryEmail.Add(strEmail(j))
                                End If
                            End If
                        Next
                    End If
                Next

                If aryEmail.Count > 0 Then
                    For i = 0 To aryEmail.Count - 1
                        If System.Text.RegularExpressions.Regex.IsMatch(aryEmail(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                            sendMailToStoreMgr(strWONo, aryEmail(i), strLoginUserName, "R", "WO")
                            'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                        End If
                    Next
                End If
            Else
                WOCancel = False
            End If
        End Function

        Public Function GRNInfo(ByVal strGRNNo As String) As DataSet
            Dim strSql, strSqlM, strSqlD, strCoyID As String
            Dim ds As DataSet
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strTemp = Common.BuildWildCard(strGRNNo)

            strSqlM = " SELECT GRN_MSTR.*, CM_COY_NAME, POM_PO_NO, POM_CREATED_DATE, DOM_DO_NO, DOM_DO_DATE " &
                      " FROM GRN_MSTR " &
                      " INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " &
                      " INNER JOIN COMPANY_MSTR ON CM_COY_ID = GM_S_COY_ID " &
                      " INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " &
                      " WHERE GM_B_COY_ID = '" & strCoyID & "' AND GM_GRN_NO = '" & strGRNNo & "' "

            strSqlD = " SELECT GRN_MSTR.*, GRN_DETAILS.*, CM_COY_NAME, POM_PO_NO, POM_CREATED_DATE, DOM_DO_NO, DOM_DO_DATE, " &
                      " (GD_RECEIVED_QTY-GD_REJECTED_QTY) AS OUTSTANDING, PO_MSTR.*, PO_DETAILS.* " &
                      " FROM GRN_MSTR " &
                      " INNER JOIN GRN_DETAILS ON GD_GRN_NO = GM_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID " &
                      " INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " &
                      " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND GD_PO_LINE = POD_PO_LINE " &
                      " INNER JOIN COMPANY_MSTR ON CM_COY_ID = GM_S_COY_ID " &
                      " INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " &
                      " WHERE GM_B_COY_ID = '" & strCoyID & "' AND GM_GRN_NO = '" & strGRNNo & "' "

            strSql = strSqlM & ";" & strSqlD & ";"
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "GRN_MSTR"
            ds.Tables(1).TableName = "GRN_DETAILS"
            Return ds
        End Function

        Public Function ROSubmit(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal dsRO As DataSet, ByRef strRONo As String, ByVal aryRO As ArrayList)
            Dim strPrefix, SqlQuery As String
            Dim strAryQuery(0), strNewRONo As String
            Dim intIncrementNo As Integer = 0
            Dim aryItem As New ArrayList()
            Dim dteNow As DateTime = Now()
            Dim i, j, k As Integer
            Dim aryTrans As New ArrayList()
            Dim blnFound As Boolean

            aryItem = aryRO

            SqlQuery = " SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'RO' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            intIncrementNo = 1
            strRONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'RO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            SqlQuery = "INSERT INTO INVENTORY_RETURN_OUTWARD_MSTR(IROM_RO_COY_ID, IROM_RO_NO, " &
                       "IROM_RO_DATE, IROM_GRN_NO, IROM_CREATED_BY, IROM_CREATED_DATE) " &
                       "VALUES('" & strCoyID & "', " & strRONo & ", " &
                       "" & Common.ConvertDate(dteNow) & ", '" & dsRO.Tables(0).Rows(0)("IROM_GRN_NO") & "', '" & strLoginUser & "', " & Common.ConvertDate(dteNow) & ")"
            Common.Insert2Ary(strAryQuery, SqlQuery)


            Dim dtRODtls As DataTable
            Dim drRODtl As DataRow
            Dim dsGRN As DataSet
            Dim PM_INV_INDEX, GRN_UPRICE, LM_LOCATION_INDEX, POD_UNIT_COST, IM_INVENTORY_INDEX As String
            Dim decQty, decAmtF, decAmtM, decFactor, decLandedCost, decCost, decRejIQCQty As Decimal

            dtRODtls = dsRO.Tables(1)
            For Each drRODtl In dtRODtls.Rows
                decQty = 0
                decAmtF = 0
                decAmtM = 0
                decFactor = 0
                decLandedCost = 0
                decCost = 0

                SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                           " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & drRODtl("IROD_ITEM_CODE") & "'"
                PM_INV_INDEX = objDb.GetVal(SqlQuery)

                SqlQuery = "INSERT INTO INVENTORY_RETURN_OUTWARD_DETAILS(IROD_RO_COY_ID, IROD_RO_NO, IROD_RO_LINE, IROD_PO_LINE, " &
                       "IROD_INVENTORY_INDEX, IROD_INVENTORY_NAME, IROD_QTY, IROD_REMARK) " &
                       "VALUES('" & strCoyID & "', " & strRONo & ", '" & drRODtl("IROD_RO_LINE") & "', '" & drRODtl("IROD_PO_LINE") & "', " &
                       "'" & PM_INV_INDEX & "', '" & drRODtl("IROD_INVENTORY_NAME") & "', " & drRODtl("IROD_QTY") & ", '" & Common.Parse(drRODtl("IROD_REMARK")) & "')"
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = " UPDATE GRN_DETAILS SET GD_RETURN_QTY = IFNULL(GD_RETURN_QTY, 0) + " & drRODtl("IROD_QTY") & " " &
                           " WHERE GD_B_COY_ID = '" & strCoyID & "' AND GD_PO_LINE = '" & drRODtl("IROD_PO_LINE") & "' AND GD_GRN_NO = '" & dsRO.Tables(0).Rows(0)("IROM_GRN_NO") & "' "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                'SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
                '               " VALUES ('" & PM_INV_INDEX & "', 'RO', " & drRODtl("IROD_QTY") & ", " & Common.ConvertDate(dteNow) & ", '" & drRODtl("IROD_LOCATION_INDEX") & "', " & strRONo & ", '" & strLoginUser & "', '" & drRODtl("IROD_QTY") & "') "
                'Common.Insert2Ary(strAryQuery, SqlQuery)

                'SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & drRODtl("IROD_QTY") & " " & _
                '           " WHERE ID_INVENTORY_INDEX = '" & PM_INV_INDEX & "' " & _
                '           " AND ID_LOCATION_INDEX = '" & drRODtl("IROD_LOCATION_INDEX") & "' "
                'Common.Insert2Ary(strAryQuery, SqlQuery)

                '' This will update using FIFO method from INVENTORY_LOT.
                'SqlQuery = " UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & drRODtl("IROD_QTY") & " " & _
                '           " WHERE IL_INVENTORY_INDEX = '" & PM_INV_INDEX & "' " & _
                '           " AND IL_LOCATION_INDEX = '" & drRODtl("IROD_LOCATION_INDEX") & "' " & _
                '           " AND IL_LOT_INDEX = '" & drRODtl("IROD_LOT_INDEX") & "' "
                'Common.Insert2Ary(strAryQuery, SqlQuery)

                For i = 0 To aryItem.Count - 1
                    If drRODtl("IROD_ITEM_CODE") = aryItem(i)(4) And drRODtl("IROD_LINE") = aryItem(i)(5) And aryItem(i)(0) <> "---Select---" And aryItem(i)(1) <> "" And aryItem(i)(2) <> "---Select---" And aryItem(i)(3) <> "---Select---" Then
                        SqlQuery = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = '" & Common.Parse(drRODtl("IROD_ITEM_CODE")) & "' AND IM_COY_ID = '" & strCoyID & "' "
                        IM_INVENTORY_INDEX = objDb.GetVal(SqlQuery)

                        If Common.parseNull(aryItem(i)(3)) = "" Then
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & Common.Parse(aryItem(i)(2)) & "' AND LM_SUB_LOCATION IS NULL "
                            LM_LOCATION_INDEX = objDb.GetVal(SqlQuery)
                        Else
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & Common.Parse(aryItem(i)(2)) & "' AND LM_SUB_LOCATION = '" & Common.Parse(aryItem(i)(3)) & "' "
                            LM_LOCATION_INDEX = objDb.GetVal(SqlQuery)
                        End If

                        'Chee Hong = Enhancement on 26/09/2013
                        'Minus Qty from inventory_lot & inventory detail if it's IQC number is rejected with no replacement
                        'Check IQC Qty whether is zero or more than zero
                        decRejIQCQty = 0
                        SqlQuery = "SELECT IFNULL(SUM(IVL_LOT_QTY),0) FROM INVENTORY_VERIFY " &
                                "INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " &
                                "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                                "WHERE IV_GRN_NO = '" & dsRO.Tables(0).Rows(0)("IROM_GRN_NO") & "' AND IM_COY_ID = '" & strCoyID & "' " &
                                "AND IM_ITEM_CODE = '" & drRODtl("IROD_ITEM_CODE") & "' AND IV_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' " &
                                "AND IVL_LOT_INDEX = '" & aryItem(i)(0) & "' AND IVL_STATUS = '4' "
                        decRejIQCQty = objDb.GetVal(SqlQuery)

                        'It's rejected with no replacement if IQC Qty > 0
                        If decRejIQCQty > 0 Then
                            'Minus qty from inventory_lot
                            SqlQuery = "UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & CDec(aryItem(i)(1)) & ", IL_IQC_QTY = IL_IQC_QTY - " & CDec(aryItem(i)(1)) & " " &
                                    "WHERE IL_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "' AND IL_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' AND IL_LOT_INDEX = '" & aryItem(i)(0) & "' "
                            Common.Insert2Ary(strAryQuery, SqlQuery)

                            'Minus qty from inventory_detail
                            SqlQuery = "UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & CDec(aryItem(i)(1)) & ", ID_IQC_QTY = ID_IQC_QTY - " & CDec(aryItem(i)(1)) & " " &
                                    "WHERE ID_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "' AND ID_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "'"
                            Common.Insert2Ary(strAryQuery, SqlQuery)

                        Else
                            SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY - " & CDec(aryItem(i)(1)) & " " &
                                    " WHERE ID_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "' " &
                                    " AND ID_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' "
                            Common.Insert2Ary(strAryQuery, SqlQuery)

                            SqlQuery = " UPDATE INVENTORY_LOT SET IL_LOT_QTY = IL_LOT_QTY - " & CDec(aryItem(i)(1)) & " " &
                                    " WHERE IL_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "' " &
                                    " AND IL_LOCATION_INDEX = '" & LM_LOCATION_INDEX & "' " &
                                    " AND IL_LOT_INDEX = '" & aryItem(i)(0) & "' "
                            Common.Insert2Ary(strAryQuery, SqlQuery)
                        End If

                        'Insert Lot Info into INVENTORY_RETURN_OUTWARD_LOT
                        SqlQuery = " INSERT INTO INVENTORY_RETURN_OUTWARD_LOT(IROL_RO_COY_ID, IROL_RO_NO, IROL_RO_LINE, IROL_LOCATION_INDEX, IROL_LOT_INDEX, IROL_LOT_QTY) " &
                                " VALUES ('" & strCoyID & "'," & strRONo & ", '" & drRODtl("IROD_RO_LINE") & "', '" & LM_LOCATION_INDEX & "', '" & aryItem(i)(0) & "', " & aryItem(i)(1) & ") "
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                        'SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
                        '        " VALUES ('" & PM_INV_INDEX & "', 'RO', " & aryItem(i)(1) & ", " & Common.ConvertDate(dteNow) & ", '" & LM_LOCATION_INDEX & "', " & strRONo & ", '" & strLoginUser & "', '" & Common.Parse(drRODtl("IROD_INVENTORY_NAME")) & "') "
                        'Common.Insert2Ary(strAryQuery, SqlQuery)

                        'Chee Hong - enhancement on 27/09/2013
                        'Compare & Store transaction record into array before insert into trans table
                        If aryTrans Is Nothing Then
                            aryTrans.Add(New String() {IM_INVENTORY_INDEX, LM_LOCATION_INDEX, aryItem(i)(1), drRODtl("IROD_INVENTORY_NAME")})
                        Else
                            If aryTrans.Count > 0 Then
                                blnFound = False
                                For j = 0 To aryTrans.Count - 1
                                    If aryTrans(j)(0) = IM_INVENTORY_INDEX And aryTrans(j)(1) = LM_LOCATION_INDEX Then
                                        aryTrans(j)(2) = CDec(aryTrans(j)(2)) + CDec(aryItem(i)(1))
                                        blnFound = True
                                    End If
                                Next

                                If blnFound = False Then
                                    aryTrans.Add(New String() {IM_INVENTORY_INDEX, LM_LOCATION_INDEX, aryItem(i)(1), drRODtl("IROD_INVENTORY_NAME")})
                                End If
                            Else
                                aryTrans.Add(New String() {IM_INVENTORY_INDEX, LM_LOCATION_INDEX, aryItem(i)(1), drRODtl("IROD_INVENTORY_NAME")})
                            End If
                        End If
                    End If
                Next

                'SqlQuery = " SELECT POD_UNIT_COST FROM PO_DETAILS WHERE POD_COY_ID = '" & strCoyID & "' AND POD_PO_NO = '" & dsRO.Tables(0).Rows(0)("IROM_PO_NO") & "' AND POD_PO_LINE = '" & drRODtl("IROD_PO_LINE") & "'"
                'POD_UNIT_COST = objDb.GetVal(SqlQuery)

                'SqlQuery = " SELECT (GD_FACTOR + GD_INLAND_CHARGE) / GD_RECEIVED_QTY FROM GRN_DETAILS " & _
                '           " WHERE GD_B_COY_ID = '" & strCoyID & "' AND GD_GRN_NO = '" & dsRO.Tables(0).Rows(0)("IROM_GRN_NO") & "' AND GD_PO_LINE = '" & drRODtl("IROD_PO_LINE") & "' "
                'GRN_UPRICE = objDb.GetVal(SqlQuery)

                'Get Unit Price
                SqlQuery = " SELECT GD_RECEIVED_QTY, GD_REJECTED_QTY, IFNULL(GD_OTH_CHARGE, 0) AS GD_OTH_CHARGE, " &
                        " IFNULL(GD_EXCHANGE_RATE, 1) AS GD_EXCHANGE_RATE, IFNULL(GD_FACTOR, 0) AS GD_FACTOR, " &
                        " IFNULL(GD_INLAND_CHARGE, 0) AS GD_INLAND_CHARGE, IFNULL(GD_DUTIES, 0) AS GD_DUTIES, POD_UNIT_COST FROM GRN_DETAILS " &
                        " INNER JOIN GRN_MSTR ON GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " &
                        " INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " &
                        " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID AND GD_PO_LINE = POD_PO_LINE " &
                        " WHERE GD_B_COY_ID = '" & strCoyID & "' AND GD_GRN_NO = '" & dsRO.Tables(0).Rows(0)("IROM_GRN_NO") & "' AND GD_PO_LINE = '" & drRODtl("IROD_PO_LINE") & "'"
                dsGRN = objDb.FillDs(SqlQuery)

                decQty = CDec(dsGRN.Tables(0).Rows(0)("GD_RECEIVED_QTY")) - CDec(dsGRN.Tables(0).Rows(0)("GD_REJECTED_QTY"))

                '1. Calculate Amount F
                decAmtF = (CDec(dsGRN.Tables(0).Rows(0)("POD_UNIT_COST")) * decQty) + CDec(dsGRN.Tables(0).Rows(0)("GD_OTH_CHARGE"))
                '2. Calculate Amount M
                decAmtM = decAmtF * CDec(dsGRN.Tables(0).Rows(0)("GD_EXCHANGE_RATE"))
                '3. Calculate GRN Factor
                decFactor = decAmtM * CDec(dsGRN.Tables(0).Rows(0)("GD_FACTOR")) / 100
                '4. Calculate  Landed Cost
                decLandedCost = decFactor + CDec(dsGRN.Tables(0).Rows(0)("GD_INLAND_CHARGE"))
                decLandedCost = Format(decLandedCost, "###0.00") + CDec(dsGRN.Tables(0).Rows(0)("GD_DUTIES"))
                decCost = decLandedCost / decQty

                If decCost > 0 Then
                    SqlQuery = "INSERT INTO INVENTORY_COST(IC_COST_YEAR, IC_COST_MONTH, IC_COST_DATE, IC_INVENTORY_TYPE, IC_INVENTORY_REF_DOC, IC_INVENTORY_INDEX, IC_INVENTORY_NAME, IC_COY_ID, " &
                           "IC_COST_OPEN_QTY, IC_COST_OPEN_UPRICE, IC_COST_OPEN_COST, IC_COST_QTY, IC_COST_UPRICE, IC_COST_COST, IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST) SELECT " &
                           "YEAR(CURDATE()), DATE_FORMAT(CURDATE(),'%m'), " & Common.ConvertDate(dteNow) & ", 'RO', " & strRONo & ", '" & PM_INV_INDEX & "', " &
                           "'" & drRODtl("IROD_INVENTORY_NAME") & "', '" & strCoyID & "', " &
                           "IC_COST_CLOSE_QTY, IC_COST_CLOSE_UPRICE, IC_COST_CLOSE_COST, " & drRODtl("IROD_QTY") & ", " & decCost & ", " & drRODtl("IROD_QTY") & "*" & decCost & ", " &
                           "IC_COST_CLOSE_QTY-" & drRODtl("IROD_QTY") & "," &
                           "IFNULL((IC_COST_CLOSE_COST-(" & drRODtl("IROD_QTY") & "*" & decCost & ")) / (IC_COST_CLOSE_QTY-" & drRODtl("IROD_QTY") & "),0)," &
                           "IC_COST_CLOSE_COST-(" & drRODtl("IROD_QTY") & "*" & decCost & ") " &
                           "FROM INVENTORY_COST WHERE IC_INVENTORY_INDEX = '" & PM_INV_INDEX & "' ORDER BY IC_COST_INDEX DESC LIMIT 1"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
            Next

            'Insert into trans table
            If aryTrans.Count > 0 Then
                For i = 0 To aryTrans.Count - 1
                    SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_FRM_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                            " VALUES ('" & aryTrans(i)(0) & "', 'RO', " & aryTrans(i)(2) & ", " & Common.ConvertDate(dteNow) & ", '" & aryTrans(i)(1) & "', " & strRONo & ", '" & strLoginUser & "', '" & Common.Parse(aryTrans(i)(3)) & "') "
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Next
            End If

            SqlQuery = " SET @T_NO = " & strRONo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CP_PARAM_TYPE = 'RO' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            Dim objUsers As New Users
            objUsers.Log_UserActivityNew(strCoyID, strLoginUser, strAryQuery, WheelModule.ROMod, WheelUserActivity.SK_SubmitRO, strRONo)
            objUsers = Nothing

            If objDb.BatchExecuteNew(strAryQuery, , strNewRONo, "T_NO") Then
                strRONo = strNewRONo
                ROSubmit = True

                Dim objMail As New Email
                Dim dsEmail As New DataSet
                Dim aryEmail As New ArrayList
                Dim strAllEmail As String
                Dim strEmail() As String
                Dim blnEmailFound As Boolean

                SqlQuery = "SELECT AGM_MRS_EMAIL2 FROM APPROVAL_GRP_MSTR WHERE AGM_TYPE = 'MRS' AND AGM_COY_ID = '" & strCoyID & "' AND AGM_MRS_EMAIL2 IS NOT NULL "
                dsEmail = objDb.FillDs(SqlQuery)

                For i = 0 To dsEmail.Tables(0).Rows.Count - 1
                    If dsEmail.Tables(0).Rows(i)("AGM_MRS_EMAIL2") <> "" Then
                        strAllEmail = dsEmail.Tables(0).Rows(i)("AGM_MRS_EMAIL2")
                        strAllEmail = Replace(strAllEmail, " ", "")
                        strEmail = Split(strAllEmail, ";")

                        For j = 0 To strEmail.Length - 1
                            If aryEmail Is Nothing Then
                                aryEmail.Add(strEmail(j))
                            Else
                                If aryEmail.Count > 0 Then
                                    blnEmailFound = False

                                    For k = 0 To aryEmail.Count - 1
                                        If strEmail(j) = aryEmail(k) Then
                                            blnEmailFound = True
                                            Exit For
                                        End If
                                    Next

                                    If blnEmailFound = False Then
                                        aryEmail.Add(strEmail(j))
                                    End If
                                Else
                                    aryEmail.Add(strEmail(j))
                                End If
                            End If
                        Next
                    End If
                Next

                If aryEmail.Count > 0 Then
                    For i = 0 To aryEmail.Count - 1
                        If System.Text.RegularExpressions.Regex.IsMatch(aryEmail(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                            sendMailToStoreMgr(strRONo, aryEmail(i), HttpContext.Current.Session("UserName"), , "RO")
                            'objMail.sendNotification(EmailType.MRSIssued, strSKId, strCoyID, "", strMRSNo, "", strLoginUserName)
                        End If
                    Next
                End If

                objMail.sendNotification(EmailType.ROCreated, strLoginUser, strCoyID, "", strRONo, "", HttpContext.Current.Session("UserName"))
                'objMail.sendNotification(EmailType.ROCreated, strLoginUser, strCoyID, "", strRONo, "", "", "SM")
                objMail = Nothing
            Else
                ROSubmit = False
            End If
        End Function

        Function GetGRNPartial(ByVal strGRNNo As String, ByVal strPONo As String, ByVal strStartDt As String, ByVal strEndDt As String, ByVal strVendorName As String) As DataSet
            Dim sqlGRN, sqlTemp, strLoginUser, strCompId As String
            Dim dsGRN As DataSet
            Dim strTemp As String
            strLoginUser = HttpContext.Current.Session("UserId")
            strCompId = HttpContext.Current.Session("CompanyId")

            'sqlGRN = "SELECT GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, GM_CREATED_DATE, GM_DATE_RECEIVED, " & _
            '        "GM_GRN_NO, GM_B_COY_ID, DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, POM_S_COY_NAME, GM_GRN_STATUS, POM_S_COY_ID, ACCEPTED_BY, STATUS_DESC FROM " & _
            '        "(SELECT GD_PO_LINE, GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, GM_CREATED_DATE, GM_DATE_RECEIVED, " & _
            '        "GM_GRN_NO, GM_B_COY_ID, DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, POM_S_COY_NAME, GM_GRN_STATUS, POM_S_COY_ID, UM_USER_NAME AS ACCEPTED_BY, " & _
            '        "(SELECT STATUS_DESC FROM STATUS_MSTR WHERE STATUS_NO = GM_GRN_STATUS AND STATUS_TYPE='GRN') AS STATUS_DESC " & _
            '        "FROM GRN_MSTR " & _
            '        "INNER JOIN GRN_DETAILS ON GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " & _
            '        "INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX " & _
            '        "INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " & _
            '        "INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO AND GD_PO_LINE = POD_PO_LINE " & _
            '        "INNER JOIN PRODUCT_MSTR ON POD_COY_ID = PM_S_COY_ID AND POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE " & _
            '        "INNER JOIN USER_MSTR ON UM_USER_ID = GM_CREATED_BY AND UM_COY_ID = GM_B_COY_ID " & _
            '        "WHERE GM_B_COY_ID='" & strCompId & "' AND GD_RECEIVED_QTY > IFNULL(GD_RETURN_QTY,0) " & _
            '        "AND POD_ITEM_TYPE = 'ST' AND PM_IQC_IND = 'N' AND GM_CREATED_BY = '" & strLoginUser & "' "

            'If strStartDt <> "" Then
            '    sqlGRN &= " AND GM_CREATED_DATE >= " & Common.ConvertDate(strStartDt) & " "
            'End If

            'If strEndDt <> "" Then
            '    sqlGRN &= " AND GM_CREATED_DATE <= " & Common.ConvertDate(strEndDt & " 23:59:59.000") & " "
            'End If


            'sqlGRN &= "UNION " & _
            '        "SELECT GD_PO_LINE, GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, GM_CREATED_DATE, GM_DATE_RECEIVED, " & _
            '        "GM_GRN_NO, GM_B_COY_ID, DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, POM_S_COY_NAME, GM_GRN_STATUS, POM_S_COY_ID, UM_USER_NAME AS ACCEPTED_BY, " & _
            '        "(SELECT STATUS_DESC FROM STATUS_MSTR WHERE STATUS_NO = GM_GRN_STATUS AND STATUS_TYPE='GRN') AS STATUS_DESC " & _
            '        "FROM GRN_MSTR " & _
            '        "INNER JOIN GRN_DETAILS ON GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " & _
            '        "INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " & _
            '        "INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX " & _
            '        "INNER JOIN USER_MSTR ON UM_USER_ID = GM_CREATED_BY AND UM_COY_ID = GM_B_COY_ID " & _
            '        "WHERE GM_B_COY_ID='" & strCompId & "' AND GD_RECEIVED_QTY > IFNULL(GD_RETURN_QTY,0) " & _
            '        "AND GM_GRN_NO IN " & _
            '        "(SELECT DISTINCT IV_GRN_NO FROM INVENTORY_VERIFY " & _
            '        "INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " & _
            '        "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " & _
            '        "WHERE IM_COY_ID = '" & strCompId & "' AND GM_CREATED_BY = '" & strLoginUser & "' AND (IVL_STATUS = '1' OR IVL_STATUS = '2' OR IVL_STATUS = '4'))) tb " & _
            '        "INNER JOIN GRN_LOT ON GM_B_COY_ID = GL_B_COY_ID AND GM_GRN_NO = GL_GRN_NO AND GD_PO_LINE = GL_PO_LINE " & _
            '        "INNER JOIN INVENTORY_LOT ON GL_LOCATION_INDEX = IL_LOCATION_INDEX AND GL_LOT_INDEX = IL_LOT_INDEX " & _
            '        "WHERE (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0 "

            sqlGRN = "SELECT GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, GM_CREATED_DATE, " &
                    "GM_DATE_RECEIVED, GM_GRN_NO, GM_B_COY_ID, DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, POM_S_COY_NAME, " &
                    "GM_GRN_STATUS, POM_S_COY_ID, ACCEPTED_BY FROM " &
                    "(SELECT GD_PO_LINE, GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, " &
                    "GM_CREATED_DATE, GM_DATE_RECEIVED, GM_GRN_NO, GM_B_COY_ID, DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, " &
                    "POM_S_COY_NAME, GM_GRN_STATUS, POM_S_COY_ID, UM_USER_NAME AS ACCEPTED_BY " &
                    "FROM GRN_MSTR INNER JOIN GRN_DETAILS ON GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " &
                    "INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX " &
                    "INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " &
                    "INNER JOIN PO_DETAILS ON POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO AND GD_PO_LINE = POD_PO_LINE " &
                    "INNER JOIN INVENTORY_MSTR ON POM_B_COY_ID = IM_COY_ID AND POD_VENDOR_ITEM_CODE = IM_ITEM_CODE " &
                    "INNER JOIN PRODUCT_MSTR ON POD_COY_ID = PM_S_COY_ID AND POD_VENDOR_ITEM_CODE = PM_VENDOR_ITEM_CODE " &
                    "INNER JOIN USER_MSTR ON UM_USER_ID = GM_CREATED_BY AND UM_COY_ID = GM_B_COY_ID " &
                    "INNER JOIN GRN_LOT ON GM_B_COY_ID = GL_B_COY_ID AND GM_GRN_NO = GL_GRN_NO AND GD_PO_LINE = GL_PO_LINE " &
                    "INNER JOIN INVENTORY_LOT ON GL_LOCATION_INDEX = IL_LOCATION_INDEX AND GL_LOT_INDEX = IL_LOT_INDEX AND IM_INVENTORY_INDEX = IL_INVENTORY_INDEX " &
                    "WHERE GM_B_COY_ID='" & strCompId & "' AND GD_RECEIVED_QTY > IFNULL(GD_RETURN_QTY,0) AND POD_ITEM_TYPE = 'ST' AND (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0 " &
                    "AND PM_IQC_IND = 'N' AND GM_CREATED_BY = '" & strLoginUser & "' AND GM_CREATED_DATE >= " & Common.ConvertDate(strStartDt) & " " &
                    "AND GM_CREATED_DATE <= " & Common.ConvertDate(strEndDt & " 23:59:59.000") & " " &
                    "UNION " &
                    "SELECT GD_PO_LINE, GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, " &
                    "POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, GM_CREATED_DATE, GM_DATE_RECEIVED, GM_GRN_NO, GM_B_COY_ID, " &
                    "DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, POM_S_COY_NAME, GM_GRN_STATUS, POM_S_COY_ID, " &
                    "UM_USER_NAME AS ACCEPTED_BY FROM GRN_MSTR " &
                    "INNER JOIN GRN_DETAILS ON GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " &
                    "INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " &
                    "INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX " &
                    "INNER JOIN USER_MSTR ON UM_USER_ID = GM_CREATED_BY AND UM_COY_ID = GM_B_COY_ID " &
                    "WHERE GM_B_COY_ID='" & strCompId & "' AND GD_RECEIVED_QTY > IFNULL(GD_RETURN_QTY,0) AND GM_GRN_NO IN " &
                    "(SELECT DISTINCT IV_GRN_NO FROM INVENTORY_VERIFY " &
                    "INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " &
                    "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "INNER JOIN INVENTORY_LOT ON IV_INVENTORY_INDEX = IL_INVENTORY_INDEX AND IV_LOCATION_INDEX = IL_LOCATION_INDEX " &
                    "AND IVL_LOT_INDEX = IL_LOT_INDEX " &
                    "INNER JOIN GRN_MSTR ON IV_GRN_NO = GM_GRN_NO AND IM_COY_ID = GM_B_COY_ID " &
                    "WHERE GM_CREATED_DATE >= " & Common.ConvertDate(strStartDt) & " AND GM_CREATED_DATE <= " & Common.ConvertDate(strEndDt & " 23:59:59.000") & " " &
                    "AND IM_COY_ID = '" & strCompId & "' AND GM_CREATED_BY = '" & strLoginUser & "' " &
                    "AND (IVL_STATUS = '1' OR IVL_STATUS = '2') AND (IL_LOT_QTY - IFNULL(IL_IQC_QTY,0)) > 0) " &
                    "UNION " &
                    "SELECT GD_PO_LINE, GM_CREATED_BY, GM_GRN_INDEX, POM_PO_NO, " &
                    "POM_PO_DATE, DOM_DO_NO, DOM_DO_DATE, GM_CREATED_DATE, GM_DATE_RECEIVED, GM_GRN_NO, GM_B_COY_ID, " &
                    "DOM_DO_INDEX, DOM_PO_INDEX, GM_LEVEL2_USER, POM_S_COY_NAME, GM_GRN_STATUS, POM_S_COY_ID, " &
                    "UM_USER_NAME AS ACCEPTED_BY FROM GRN_MSTR " &
                    "INNER JOIN GRN_DETAILS ON GD_B_COY_ID = GM_B_COY_ID AND GD_GRN_NO = GM_GRN_NO " &
                    "INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX " &
                    "INNER JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX " &
                    "INNER JOIN USER_MSTR ON UM_USER_ID = GM_CREATED_BY AND UM_COY_ID = GM_B_COY_ID " &
                    "WHERE GM_B_COY_ID='" & strCompId & "' AND GD_RECEIVED_QTY > IFNULL(GD_RETURN_QTY,0) " &
                    "AND GM_GRN_NO IN " &
                    "(SELECT DISTINCT IV_GRN_NO FROM INVENTORY_VERIFY " &
                    "INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " &
                    "INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "INNER JOIN INVENTORY_LOT ON IV_INVENTORY_INDEX = IL_INVENTORY_INDEX AND IV_LOCATION_INDEX = IL_LOCATION_INDEX " &
                    "AND IVL_LOT_INDEX = IL_LOT_INDEX " &
                    "INNER JOIN GRN_MSTR ON IV_GRN_NO = GM_GRN_NO AND IM_COY_ID = GM_B_COY_ID " &
                    "WHERE GM_CREATED_DATE >= " & Common.ConvertDate(strStartDt) & " AND GM_CREATED_DATE <= " & Common.ConvertDate(strEndDt & " 23:59:59.000") & " " &
                    "AND IM_COY_ID = '" & strCompId & "' AND GM_CREATED_BY = '" & strLoginUser & "' AND IVL_STATUS = '4' AND IL_LOT_QTY > 0)) tb "

            sqlTemp = ""

            If strGRNNo <> "" Then
                strTemp = Common.BuildWildCard(strGRNNo)
                If sqlTemp = "" Then
                    sqlTemp &= " WHERE GM_GRN_NO" & Common.ParseSQL(strTemp)
                Else
                    sqlTemp &= " AND GM_GRN_NO" & Common.ParseSQL(strTemp)
                End If
            End If

            If strPONo <> "" Then
                strTemp = Common.BuildWildCard(strPONo)
                If sqlTemp = "" Then
                    sqlTemp &= " WHERE POM_PO_NO" & Common.ParseSQL(strTemp) & " "
                Else
                    sqlTemp &= " AND POM_PO_NO" & Common.ParseSQL(strTemp) & " "
                End If
            End If

            If strVendorName <> "" Then
                strTemp = Common.BuildWildCard(strVendorName)
                If sqlTemp = "" Then
                    sqlTemp &= " WHERE POM_S_COY_NAME" & Common.ParseSQL(strTemp) & " "
                Else
                    sqlTemp &= " AND POM_S_COY_NAME" & Common.ParseSQL(strTemp) & " "
                End If
            End If


            sqlGRN &= sqlTemp


            sqlGRN &= " GROUP BY GM_GRN_INDEX ORDER BY GM_CREATED_DATE "

            dsGRN = objDb.FillDs(sqlGRN)
            Return dsGRN
        End Function

        Function PopLotInfoByRO(ByVal strGRNNo As String, ByVal strPONo As String, ByVal strPOLine As String, ByVal strItemCode As String, Optional ByVal strLotIndex As String = "", Optional ByVal strLoc As String = "", Optional ByVal strSubLoc As String = "", Optional ByVal strGroup As String = "") As DataSet
            Dim strSql, strLoginUser, strCompId As String
            Dim ds As DataSet

            strLoginUser = HttpContext.Current.Session("UserId")
            strCompId = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT GL_LOT_INDEX, DOL_LOT_NO, GL_LOT_RECEIVED_QTY, LM_LOCATION, LM_SUB_LOCATION, " &
                    "POM_PO_NO, POD_VENDOR_ITEM_CODE FROM GRN_MSTR " &
                    "INNER JOIN GRN_DETAILS ON GM_GRN_NO = GD_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID " &
                    "INNER JOIN GRN_LOT ON GL_GRN_NO = GD_GRN_NO AND GL_B_COY_ID = GD_B_COY_ID AND GL_PO_LINE = GD_PO_LINE " &
                    "INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX AND GM_B_COY_ID = POM_B_COY_ID " &
                    "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                    "INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX " &
                    "INNER JOIN DO_DETAILS ON DOD_S_COY_ID = DOM_S_COY_ID AND DOD_DO_NO = DOM_DO_NO AND DOD_PO_LINE = GL_PO_LINE " &
                    "INNER JOIN DO_LOT ON DOL_LOT_INDEX = GL_LOT_INDEX " &
                    "INNER JOIN LOCATION_MSTR ON LM_LOCATION_INDEX = GL_LOCATION_INDEX " &
                    "WHERE GD_GRN_NO = '" & strGRNNo & "' AND POM_PO_NO = '" & strPONo & "' " &
                    "AND POD_VENDOR_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                    "AND GD_B_COY_ID = '" & strCompId & "' AND GD_PO_LINE = '" & strPOLine & "' "

            If strLotIndex <> "" Then
                strSql &= "AND GL_LOT_INDEX = '" & strLotIndex & "' "
            End If

            If strLoc <> "" Then
                strSql &= "AND LM_LOCATION = '" & strLoc & "' "
            End If

            If strSubLoc <> "" Then
                strSql &= "AND LM_SUB_LOCATION = '" & strSubLoc & "' "
            End If

            If strGroup <> "" Then
                strSql &= "GROUP BY " & strGroup
            End If

            ds = objDb.FillDs(strSql)
            Return ds

        End Function

        Function chkROLotRemainingQty(ByVal strGRNNo As String, ByVal strPOLine As String, ByVal strLotIndex As String, ByVal strLoc As String, ByVal strSubLoc As String) As Decimal
            Dim strSql, strCompId, strLocIndex As String
            Dim decQty As Decimal
            strCompId = HttpContext.Current.Session("CompanyId")

            If strSubLoc = "" Then
                strSql = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCompId & "' " &
                        "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' AND LM_SUB_LOCATION = '" & Common.Parse(strSubLoc) & "'"
            Else
                strSql = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCompId & "' " &
                        "AND LM_LOCATION = '" & Common.Parse(strLoc) & "' AND LM_SUB_LOCATION = '" & Common.Parse(strSubLoc) & "'"
            End If
            strLocIndex = objDb.GetVal(strSql)

            strSql = "SELECT IFNULL(SUM(IROL_LOT_QTY),0) AS IROL_LOT_QTY FROM INVENTORY_RETURN_OUTWARD_MSTR " &
                    "INNER JOIN INVENTORY_RETURN_OUTWARD_DETAILS ON IROM_RO_NO = IROD_RO_NO AND IROM_RO_COY_ID = IROD_RO_COY_ID " &
                    "INNER JOIN INVENTORY_RETURN_OUTWARD_LOT ON IROD_RO_NO = IROL_RO_NO AND IROD_RO_COY_ID = IROL_RO_COY_ID AND IROD_RO_LINE = IROL_RO_LINE " &
                    "WHERE IROM_GRN_NO = '" & strGRNNo & "' AND IROM_RO_COY_ID = '" & strCompId & "' AND IROD_PO_LINE = '" & strPOLine & "' " &
                    "AND IROL_LOCATION_INDEX = '" & strLocIndex & "' AND IROL_LOT_INDEX = '" & strLotIndex & "'"

            decQty = objDb.GetVal(strSql)
            chkROLotRemainingQty = decQty
        End Function

        Function chkIQCLotQty(ByVal strGRNNo As String, ByVal strItemCode As String, Optional ByVal strPoLine As String = "", Optional ByVal strLot As String = "", Optional ByVal strLoc As String = "", Optional ByVal strSubLoc As String = "", Optional ByVal strStatus As String = "App", Optional ByVal blnLoc As Boolean = False) As Decimal
            Dim strSql, strCompId, strLocIndex As String
            Dim decQty As Decimal
            Dim strLotIndex As String = ""
            strCompId = HttpContext.Current.Session("CompanyId")

            If strPoLine <> "" Then
                strSql = "SELECT CAST(IFNULL(GROUP_CONCAT(GL_LOT_INDEX),'') AS CHAR(250)) FROM GRN_LOT WHERE GL_B_COY_ID = '" & strCompId & "' AND GL_GRN_NO = '" & strGRNNo & "' AND GL_PO_LINE = '" & strPoLine & "' "
                strLotIndex = objDb.GetVal(strSql)
            End If

            strSql = " SELECT IFNULL(SUM(IVL_LOT_QTY),0) FROM INVENTORY_VERIFY " &
                    " INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " &
                    " INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    " INNER JOIN LOCATION_MSTR ON IV_LOCATION_INDEX = LM_LOCATION_INDEX " &
                    " WHERE IV_GRN_NO = '" & strGRNNo & "' AND IM_COY_ID = '" & strCompId & "' AND IM_ITEM_CODE = '" & strItemCode & "' "

            If strLotIndex <> "" Then
                strSql &= " AND IVL_LOT_INDEX IN (" & strLotIndex & ") "
            End If

            If strStatus = "App" Then
                strSql &= " AND (IVL_STATUS = '1' OR IVL_STATUS = '2') "
            ElseIf strStatus = "Rej" Then
                strSql &= " AND IVL_STATUS = '4' "
            End If


            If blnLoc = True Then
                If strSubLoc = "" Then
                    strSql &= " AND IVL_LOT_INDEX = '" & strLot & "' " &
                        " AND LM_LOCATION = '" & Common.Parse(strLoc) & "' " &
                        " AND LM_SUB_LOCATION IS NULL "
                Else
                    strSql &= " AND IVL_LOT_INDEX = '" & strLot & "' " &
                        " AND LM_LOCATION = '" & Common.Parse(strLoc) & "' " &
                        " AND LM_SUB_LOCATION = '" & Common.Parse(strSubLoc) & "' "
                End If
            End If

            decQty = objDb.GetVal(strSql)
            chkIQCLotQty = decQty
        End Function

        Function chkIQCForGRN(ByVal strGRNNo As String, ByVal strItemCode As String) As String
            Dim strSql, strCompId As String
            Dim ds As DataSet
            strCompId = HttpContext.Current.Session("CompanyId")

            strSql = " SELECT '*' FROM INVENTORY_VERIFY " &
                    " INNER JOIN INVENTORY_VERIFY_LOT ON IV_VERIFY_INDEX = IVL_VERIFY_INDEX " &
                    " INNER JOIN INVENTORY_MSTR ON IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    " WHERE IV_GRN_NO = '" & strGRNNo & "' AND IM_COY_ID = '" & strCompId & "' AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "

            ds = objDb.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                chkIQCForGRN = "Y"
            Else
                chkIQCForGRN = "N"
            End If

        End Function

        Public Function delete_TempAttachment(ByVal strID As String) As String
            Dim strsql As String
            Dim query(0) As String
            strsql = "Delete from COMPANY_DOC_ATTACHMENT" &
                   " where CDA_DOC_NO = '" & strID & "'  AND CDA_DOC_TYPE = 'WO' "
            Common.Insert2Ary(query, strsql)
            objDb.BatchExecute(query)
        End Function

        Public Function getWOAttach(ByVal strDocNo As String, ByVal bcomid As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & bcomid & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'WO' "
            ds = objDb.FillDs(strsql)
            getWOAttach = ds
        End Function

        Public Function deleteWOAttachment(ByVal intIndex As Integer)
            Dim strsql As String
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
            objDb.Execute(strsql)
        End Function

        Public Function getInvItemUom(ByVal strItem As String) As String
            Dim strsql, strCompId As String
            strCompId = HttpContext.Current.Session("CompanyId")

            strsql = "SELECT IFNULL(PM_UOM,'') FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & Common.Parse(strItem) & "' AND PM_S_COY_ID = '" & strCompId & "'"
            getInvItemUom = objDb.GetVal(strsql)
        End Function
    End Class
End Namespace

