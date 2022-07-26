Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraNEO
Imports SSO.Component

Namespace AgoraNEO
    Public Class GRN
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals
        Public GRN_Id As String
        Public Function populateDataIssueGrn(ByVal userID As String, ByVal companyID As String, ByVal strDONo As String, ByVal strPONo As String) As String
            Dim dsDO As DataSet
            Dim strsql As String
            Dim strTemp As String

            'Jules 2019.01.02 - User can only see DOs where they are the requester.
            'Michelle (11/1/2013) - Issue 1832
            'Chee Hong (30 Mar 2015) - 1. Temporary filter the On Going DO before GST cut off - Issue 8317
            'Chee Hong (30 Mar 2015) - 2. Table is on_going_do
            'strsql = "SELECT DOM_DO_INDEX, DOM_DO_NO, POM_PO_NO, POM_PO_DATE,DOM_DO_DATE, CM_COY_NAME " & _
            strsql = "SELECT DOM_DO_INDEX, DOM_DO_NO, POM_PO_NO, POM_PO_DATE,DOM_DO_DATE, CM_COY_NAME, DOM_S_COY_ID, DATEDIFF(NOW(), DOM_DO_DATE) as DO_DAYS " &
                     "FROM do_mstr, PO_MSTR, COMPANY_MSTR " &
                     "WHERE POM_PO_INDEX = DOM_PO_INDEX AND POM_B_COY_ID='" & companyID & "' AND POM_BUYER_ID='" & userID & "' " &
                     "AND POM_S_COY_ID = CM_COY_ID AND POM_BILLING_METHOD <> 'DO' AND DOM_DO_STATUS=2 AND DOM_DO_INDEX NOT IN " &
                     "(SELECT GM_DO_INDEX FROM GRN_MSTR WHERE GM_PO_INDEX=DOM_PO_INDEX " &
                     "UNION ALL " &
                     "SELECT ODO_DO_INDEX FROM ON_GOING_DO) " &
                     "AND DOM_D_ADDR_CODE IN (SELECT UL_ADDR_CODE FROM USERS_LOCATION WHERE UL_COY_ID = '" & companyID & "' " &
                     " AND UL_USER_ID = '" & userID & "' AND UL_LEVEL = 1 "

            If strDONo <> "" Then
                strTemp = Common.BuildWildCard(strDONo)
                strsql = strsql & " AND DOM_DO_NO " & Common.ParseSQL(strTemp)
            End If

            If strPONo <> "" Then
                strTemp = Common.BuildWildCard(strPONo)
                strsql = strsql & " AND POM_PO_NO " & Common.ParseSQL(strTemp)
            End If

            strsql = strsql & ") ORDER BY DOM_DO_DATE DESC "

            dsDO = objDb.FillDs(strsql)
            'Return dsDO
            Return strsql
        End Function
        Public Function getDOAttachment(ByVal strDocNo As String, ByVal addressPath As String, Optional ByVal strCoyID As String = "", Optional ByVal strType As String = "H", Optional ByVal strItemCode As String = "", Optional ByVal strLineNo As String = "", Optional ByVal strPOLine As String = "") As String
            Dim strsql As String
            Dim ds As New DataSet
            strsql = String.Format("SELECT CDDA_ATTACH_INDEX, CDDA_DOC_NO, CDDA_HUB_FILENAME, CDDA_ATTACH_FILENAME, CDDA_FILESIZE, CONCAT('{0}', CDDA_HUB_FILENAME) AS PathRef FROM COMPANY_DO_DOC_ATTACHMENT ", addressPath)
            If strCoyID = "" Then
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID like '%" & strCoyID & "%' "
            Else
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID like '%" & strCoyID & "%' "
            End If

            If strType = "H" Then
                strsql &= " AND CDDA_TYPE = 'H' "
            Else
                strsql &= " AND CDDA_TYPE = '" & strType & "' "
            End If

            If strItemCode <> "" And strLineNo <> "" And strPOLine <> "" Then
                strsql &= " AND CDDA_PO_LINE = '" & strPOLine & "' AND CDDA_ITEM_CODE = '" & strItemCode & "' AND CDDA_LINE_NO = '" & strLineNo & "' "
            End If

            ds = objDb.FillDs(strsql)
            Return strsql
        End Function
        Function GetGRN(ByVal userID As String, ByVal companyID As String, ByVal strDocType As String, ByVal strNo As String, ByVal strCreationDt As String, ByVal strVendorName As String, ByVal strStatus As String, ByVal strGRNType As String) As String
            Dim sqlGRN As String
            Dim dsGRN As DataSet
            Dim strTemp As String

            'sqlGRN = "select GRN_MSTR.GM_GRN_INDEX,PO_MSTR.POM_PO_NO, DO_MSTR.DOM_DO_NO, GRN_MSTR.GM_GRN_NO,DO_MSTR.DOM_DO_Index,DO_MSTR.DOM_PO_Index," & _
            '        " grn_MSTR.GM_LEVEL2_USER,PO_MSTR.POM_S_Coy_Name,GRN_MSTR.GM_GRN_STATUS" & _
            '        " from PO_MSTR, DO_MSTR, GRN_MSTR" & _
            '        " where DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" & _
            '        " AND GRN_MSTR.GM_DO_Index = DO_Mstr.DOM_DO_Index" & _
            '        " AND GRN_MSTR.GM_PO_Index = PO_Mstr.POM_PO_Index AND GM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"           
            sqlGRN = "select GRN_MSTR.GM_GRN_INDEX,PO_MSTR.POM_PO_NO, PO_MSTR.POM_PO_DATE,DO_MSTR.DOM_DO_NO,DO_MSTR.DOM_DO_DATE,GRN_MSTR.GM_CREATED_DATE,GRN_MSTR.GM_DATE_RECEIVED,GRN_MSTR.GM_GRN_NO,DO_MSTR.DOM_DO_Index,DO_MSTR.DOM_PO_Index," &
                   " GRN_MSTR.GM_LEVEL2_USER,PO_MSTR.POM_S_Coy_Name,GRN_MSTR.GM_GRN_STATUS,PO_MSTR.POM_S_Coy_ID,UM_User_Name AS Accepted_By," &
                   " (SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=GRN_MSTR.GM_GRN_STATUS AND STATUS_TYPE='GRN') AS Status_Desc " &
                   " From PO_MSTR, DO_MSTR, GRN_MSTR, USER_MSTR " &
                   " where DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" &
                   " AND GRN_MSTR.GM_DO_Index = DO_Mstr.DOM_DO_Index" &
                   " AND GRN_MSTR.GM_PO_Index = PO_Mstr.POM_PO_Index AND GM_B_COY_ID='" & companyID & "'" &
                   " AND UM_USER_ID=GM_CREATED_BY AND UM_COY_ID='" & companyID & "'"


            If UCase(strGRNType) = "GRNACK" Then
                sqlGRN = sqlGRN & " AND GRN_MSTR.GM_GRN_LEVEL=2 AND GM_LEVEL2_USER='" & userID & "' "
            Else
                sqlGRN = sqlGRN & " AND GM_CREATED_BY='" & userID & "' "
            End If
            If strDocType <> "0" Then
                If strDocType = "DO" Then
                    If strNo <> "" Then
                        strTemp = Common.BuildWildCard(strNo)
                        sqlGRN = sqlGRN & " And DO_Mstr.DOM_DO_NO" & Common.ParseSQL(strTemp)
                    End If
                    If strCreationDt <> "" Then
                        sqlGRN = sqlGRN & " And DO_Mstr.DOM_Created_Date Between " & Common.ConvertDate(strCreationDt & " 00:00:00") & " AND " & Common.ConvertDate(strCreationDt & " 23:59:59")
                    End If
                ElseIf strDocType = "PO" Then
                    If strNo <> "" Then
                        strTemp = Common.BuildWildCard(strNo)
                        sqlGRN = sqlGRN & " And PO_Mstr.POM_PO_No" & Common.ParseSQL(strTemp)
                    End If
                    If strCreationDt <> "" Then
                        sqlGRN = sqlGRN & " And PO_Mstr.POM_PO_Date Between  " & Common.ConvertDate(strCreationDt & " 00:00:00") & " AND " & Common.ConvertDate(strCreationDt & " 23:59:59")
                    End If
                ElseIf strDocType = "GRN" Then
                    If strNo <> "" Then
                        strTemp = Common.BuildWildCard(strNo)
                        sqlGRN = sqlGRN & " And GRN_Mstr.GM_GRN_No" & Common.ParseSQL(strTemp)
                    End If
                    If strCreationDt <> "" Then
                        sqlGRN = sqlGRN & " And GRN_Mstr.GM_CREATED_DATE Between " & Common.ConvertDate(strCreationDt & " 00:00:00") & " AND " & Common.ConvertDate(strCreationDt & " 23:59:59")
                    End If
                End If

                If strStatus <> "" Then
                    sqlGRN = sqlGRN & " And GRN_MSTR.GM_GRN_STATUS in " & strStatus
                End If

                If strVendorName <> "" Then
                    strTemp = Common.BuildWildCard(strVendorName)
                    sqlGRN = sqlGRN & " And POM_S_Coy_Name" & Common.ParseSQL(strTemp)
                End If
            End If
            dsGRN = objDb.FillDs(sqlGRN & " ORDER BY PO_MSTR.POM_PO_NO DESC")
            sqlGRN = sqlGRN & " ORDER BY PO_MSTR.POM_PO_NO DESC"
            Return sqlGRN
        End Function

        Public Function GetGRNDDL(ByRef pDropDownList As UI.WebControls.DropDownList) As DataSet
            Dim SQLquery, strDefaultValue As String
            Dim dsGRN As DataSet

            SQLquery = "SELECT GM_GRN_NO,GM_GRN_INDEX,GM_PO_INDEX,GM_DO_INDEX FROM GRN_MSTR where GM_GRN_Status =" & GRNStatus.PendingACK
            pDropDownList.Items.Clear()
            dsGRN = objDb.FillDs(SQLquery)
            Dim lstItem As New ListItem
            Common.FillDdl(pDropDownList, "GM_GRN_NO", "GM_GRN_INDEX", dsGRN)
            If Not dsGRN Is Nothing Then
                ' Add ---Select---
                lstItem.Value = 0
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            End If
            Return dsGRN
        End Function


        Public Function GetPODetails(ByVal PONo As String, ByVal POIdx As Integer, ByVal DONo As String) As DataSet
            Dim SQLQuery As String
            Dim dsPO As DataSet


            SQLQuery = "SELECT Distinct PO_Mstr.POM_B_Coy_ID,PO_Mstr.POM_PO_Index,PO_Mstr.POM_PO_No,PO_Mstr.POM_Billing_Method, PO_Mstr.POM_PO_Date, PO_Details.POD_Po_Line," &
                   " COMPANY_MSTR.CM_Coy_Name,PO_Details.POD_Vendor_Item_Code, PO_Details.POD_Product_Desc,POD_UOM, PO_Details.POD_ETD, po_Details.POD_Warranty_Terms,PO_Mstr.POM_PO_STATUS," &
                   " PO_Details.POD_Min_Pack_Qty, PO_Details.POD_Min_Order_Qty, PO_Details.POD_Ordered_Qty,PO_Details.POD_Received_Qty,PO_Details.POD_Rejected_Qty, PO_Details.POD_Ordered_Qty as POD_Outstanding, PO_Details.POD_Ordered_Qty as DOD_Ship_Qty, PO_Details.POD_Remark as DOD_Remarks," &
                   " POM_Payment_TERM, POM_Shipment_Term, POM_PAYMENT_METHOD, POM_Shipment_Mode, PO_Details.POD_D_Addr_Code, PO_Details.POD_D_Addr_Line1, " &
                   " PO_Details.POD_D_Addr_Line2, PO_Details.POD_D_Addr_Line3,PO_Details.POD_D_State, DO_Details.DOD_SHIPPED_QTY," &
                   " PO_Details.POD_D_Country, PO_Details.POD_D_PostCode,PO_Details.POD_D_City,PO_Details.POD_REJECTED_QTY, DO_Mstr.DOM_DO_INDEX" &
                   " FROM PO_Details ,PO_Mstr, COMPANY_MSTR , DO_Mstr ,users_location UL,DO_Details" &
                   " WHERE PO_Mstr.POM_PO_Index = '" & POIdx & "'" &
                   " And PO_Mstr.POM_PO_No = '" & PONo & "'" &
                   " And PO_Mstr.POM_B_Coy_ID  = '" & HttpContext.Current.Session("CompanyID") & "'" &
                   " and PO_Details.POD_Po_No = PO_Mstr.POM_PO_No " &
                   " and PO_Details.POD_Coy_ID = PO_Mstr.POM_b_Coy_ID" &
                   " And PO_Mstr.POM_S_Coy_ID = COMPANY_MSTR.CM_Coy_ID" &
                   " And PO_MSTR.POM_PO_Index = DO_Mstr.DOM_PO_Index" &
                   " And DO_MSTR.DOM_DO_NO = '" & DONo & "'" &
                   " And DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" &
                   " And DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO" &
                   " And DO_Details.DOD_DO_Line = PO_Details.POD_Po_Line" &
                   " And UL.UL_COY_ID = PO_Mstr.POM_B_Coy_ID" &
                   " And UL.UL_ADDR_CODE = PO_Details.POD_D_Addr_Code" &
                   " And UL.UL_USER_ID = '" & HttpContext.Current.Session("UserId") & "'"
            dsPO = objDb.FillDs(SQLQuery)
            Return dsPO

        End Function
        Public Function GetGRNSumm(ByVal companyID As String, ByVal POIndex As Integer, ByVal strGrnNo As String) As String
            Dim SqlQuery As String
            Dim dsGRNSumm As DataSet
            SqlQuery = "SELECT distinct GRN_Mstr.GM_CREATED_BY,GRN_Mstr.GM_Date_Received," &
                    " GRN_Mstr.GM_CREATED_DATE,GRN_Mstr.GM_GRN_No as Grn,DOM_DO_INDEX,GM_DO_INDEX,UM_USER_NAME, " &
                    " GRN_Mstr.GM_GRN_No,B.DOM_DO_NO FROM GRN_Mstr ,DO_MSTR B,USER_MSTR C " &
                    " WHERE GRN_Mstr.GM_DO_INDEX=B.DOM_DO_INDEX AND GRN_Mstr.GM_PO_INDEX=B.DOM_PO_INDEX " &
                    " AND GM_CREATED_BY=C.UM_USER_ID AND UM_COY_ID='" & companyID & "'" &
                    " AND GRN_Mstr.GM_PO_Index = " & POIndex & " AND GM_GRN_STATUS<>" & GRNStatus.PendingACK 'Michelle (22/9/2010) - Add a space before GM_GRN_STATUS, to cater for MYSQL " AND GRN_Mstr.GM_PO_Index = " & POIndex & "AND GM_GRN_STATUS<>" & GRNStatus.PendingACK

            If strGrnNo <> "" Then
                SqlQuery = SqlQuery & " And GRN_Mstr.GM_GRN_NO = '" & strGrnNo & "'"
            End If
            SqlQuery = SqlQuery & " order by GRN_Mstr.GM_Date_Received desc, GM_GRN_No desc"

            dsGRNSumm = objDb.FillDs(SqlQuery)
            Return SqlQuery
            '    SQLQuery1 = "INSERT INTO M_GRN (B_Company_Id,V_Company_Id,GRN_No,PO_Number,Date_Received,Created_by," & _
            '" Date_Created,NO_of_Copy_Printed,DO_No,Actual_GRN_No,GRN_Prefix)" & _
            '   " Values ('" & B_Company_Id & "','" & V_Company_Id & "','" & GRN_Number & "','" & PO_Number & "'" & _
            '",'" & ReceivedDate & "','" & Created_By & "','" & Now() & "'," & NO_of_Copy_Printed & ",'" & DO_No & "','" & Actual_GRN_Number & "','" & GRN_Prefix & "')"

            ' SQLQuery = "INSERT INTO M_DO (V_Company_Id,DO_No,DO_Date,PO_Number,Location_Id," &_
            '" Do_Status,Created_By,Date_Created,NO_of_Copy_Printed,Grn_No,DO_Prefix," & _
            '" Values ('" & V_Company_Id & "','" & DO_No & "','" & DO_Date & "'" & _
            '",'" & PO_Number & "','" & Location_Id &"','"& Do_Status & "'" & _
            '",'" & Created_By & "','" & DO_Date & "'," & NO_of_Copy_Printed & ",'" & GRN_Number &"','" & DO_Prefix & "'" &_											

        End Function
        Public Function GetGRNSumms(ByVal companyID As String, ByVal POIndex As Integer, ByVal strGrnNo As String) As DataSet
            Dim SqlQuery As String
            Dim dsGRNSumm As DataSet
            SqlQuery = "SELECT distinct GRN_Mstr.GM_CREATED_BY,GRN_Mstr.GM_Date_Received," &
                    " GRN_Mstr.GM_CREATED_DATE,GRN_Mstr.GM_GRN_No as Grn,DOM_DO_INDEX,GM_DO_INDEX,UM_USER_NAME, " &
                    " GRN_Mstr.GM_GRN_No,B.DOM_DO_NO FROM GRN_Mstr ,DO_MSTR B,USER_MSTR C " &
                    " WHERE GRN_Mstr.GM_DO_INDEX=B.DOM_DO_INDEX AND GRN_Mstr.GM_PO_INDEX=B.DOM_PO_INDEX " &
                    " AND GM_CREATED_BY=C.UM_USER_ID AND UM_COY_ID='" & companyID & "'" &
                    " AND GRN_Mstr.GM_PO_Index = " & POIndex & " AND GM_GRN_STATUS<>" & GRNStatus.PendingACK 'Michelle (22/9/2010) - Add a space before GM_GRN_STATUS, to cater for MYSQL " AND GRN_Mstr.GM_PO_Index = " & POIndex & "AND GM_GRN_STATUS<>" & GRNStatus.PendingACK

            If strGrnNo <> "" Then
                SqlQuery = SqlQuery & " And GRN_Mstr.GM_GRN_NO = '" & strGrnNo & "'"
            End If
            SqlQuery = SqlQuery & " order by GRN_Mstr.GM_Date_Received desc, GM_GRN_No desc"

            dsGRNSumm = objDb.FillDs(SqlQuery)
            Return dsGRNSumm
            '    SQLQuery1 = "INSERT INTO M_GRN (B_Company_Id,V_Company_Id,GRN_No,PO_Number,Date_Received,Created_by," & _
            '" Date_Created,NO_of_Copy_Printed,DO_No,Actual_GRN_No,GRN_Prefix)" & _
            '   " Values ('" & B_Company_Id & "','" & V_Company_Id & "','" & GRN_Number & "','" & PO_Number & "'" & _
            '",'" & ReceivedDate & "','" & Created_By & "','" & Now() & "'," & NO_of_Copy_Printed & ",'" & DO_No & "','" & Actual_GRN_Number & "','" & GRN_Prefix & "')"

            ' SQLQuery = "INSERT INTO M_DO (V_Company_Id,DO_No,DO_Date,PO_Number,Location_Id," &_
            '" Do_Status,Created_By,Date_Created,NO_of_Copy_Printed,Grn_No,DO_Prefix," & _
            '" Values ('" & V_Company_Id & "','" & DO_No & "','" & DO_Date & "'" & _
            '",'" & PO_Number & "','" & Location_Id &"','"& Do_Status & "'" & _
            '",'" & Created_By & "','" & DO_Date & "'," & NO_of_Copy_Printed & ",'" & GRN_Number &"','" & DO_Prefix & "'" &_											

        End Function

        Public Function IsGRNCreated(ByVal strDONo As String, ByVal intDOIdx As Integer) As Boolean
            Dim SqlQuery As String
            SqlQuery = "Select 'X' from GRN_MSTR WHERE GM_DO_INDEX = " & intDOIdx
            If objDb.Exist(SqlQuery) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GRNSubmit(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal dsGRN As DataSet, ByRef strNewGRNNo As String, ByVal strGRNType As String, ByVal blnRejectAll As Boolean, ByVal aryLoc As ArrayList) As Boolean
            Dim strAryQuery(0) As String
            Dim SqlQuery As String
            Dim strGRNPrefix As String
            Dim OutStanding, Ordered, TotalDo_Qty, Do_Quantity As Decimal 'Integer
            Dim dteNow As DateTime = Now()
            Dim ds As DataSet
            Dim intTotRecord, intGRNStatus, intGRNLevel, intDOStatus As Integer
            Dim intIncrementNo As Integer = 0

            ' Yap: Inventory Module 15/Apr/2011
            Dim arySetLocation As New ArrayList()
            arySetLocation = aryLoc
            Dim i As Integer = 0
            Dim PM_LOC_INDEX As String
            Dim PM_PRODUCT_INDEX As String
            Dim IM_PRODUCT_INDEX, PM_PRODUCT As String
            Dim QC As String

            ' Insert Default Location
            Dim storItem, storItem_temp, LocDesc, SubLocDesc As String
            Dim iTotal As Decimal = 0 'Integer = 0
            Dim objINV As New Inventory


            '//ADD BY MOO
            Dim strSCoyID, strPONo, strDONo, strGRNNo, strFlag As String
            Dim intPOIdx, intDOIdx, intGRNIdx As Integer
            strSCoyID = Common.Parse(dsGRN.Tables(0).Rows(0)("SCoyID"))
            strPONo = dsGRN.Tables(0).Rows(0)("PONo")
            strDONo = dsGRN.Tables(0).Rows(0)("DONo")
            intPOIdx = dsGRN.Tables(0).Rows(0)("POIndex")
            intDOIdx = dsGRN.Tables(0).Rows(0)("DOIndex")

            If UCase(strGRNType) <> "GRNACK" Then
                'objGlobal.GetLatestDocNo("GRN", strAryQuery, strNewGRNNo, strGRNPrefix)
                'SqlQuery = "SELECT '*' FROM GRN_MSTR WHERE GM_GRN_NO='" & strNewGRNNo & "' AND GM_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
                'If objDb.Exist(SqlQuery) Then
                '    strNewGRNNo = "dup"
                '    Return False
                'End If
                SqlQuery = " SET @DUPLICATE_CHK =''; SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' "
                Common.Insert2Ary(strAryQuery, SqlQuery)

                intIncrementNo = 1
                strNewGRNNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                strGRNPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN') "

                'strNewGRNNo = Common.Parse(strNewGRNNo)
                strGRNNo = strNewGRNNo

                '2 Level Receiving , CS_COY_ID , CS_FLAG_VALUE
                SqlQuery = "SELECT ISNULL(CS_FLAG_VALUE,0) from COMPANY_SETTING WHERE CS_COY_ID = '" & strCoyID &
                "' AND CS_FLAG_NAME = '2 Level Receiving'"
                strFlag = objDb.GetVal(SqlQuery)
                If strFlag = "0" Then
                    intGRNLevel = 1 '0 CHANGE BY MOO
                    intGRNStatus = GRNStatus.Uninvoice
                Else
                    If blnRejectAll Then
                        intGRNLevel = 1 '0 CHANGE BY MOO
                        intGRNStatus = GRNStatus.Uninvoice
                    Else
                        intGRNLevel = 2 '1 CHANGE BY MOO
                        intGRNStatus = GRNStatus.PendingACK
                    End If
                End If

                SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IFNULL(GM_GRN_NO,'') AS CHAR(1000)) FROM GRN_MSTR WHERE GM_B_COY_ID='" & Common.Parse(strCoyID) & "' AND GM_DO_INDEX = " & intDOIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)

                If IsGRNCreated(strDONo, intDOIdx) Then
                    strNewGRNNo = "exist1" 'GRN Created 
                    Return False
                End If

                SqlQuery = "Insert into GRN_Mstr(GM_GRN_NO,GM_B_COY_ID,GM_PO_INDEX,GM_DATE_RECEIVED," &
                    " GM_NOOFCOPY_PRINTED, GM_DO_INDEX, GM_GRN_PREFIX, GM_S_COY_ID," &
                    " GM_GRN_STATUS, GM_GRN_LEVEL, GM_CREATED_BY, GM_CREATED_DATE)" &
                    " VALUES(" & strNewGRNNo & ",'" & strCoyID & "'," & intPOIdx & "," & Common.ConvertDate(dsGRN.Tables(0).Rows(0)("GRNReceivedDt")) &
                    ", 0," & intDOIdx & "," & strGRNPrefix & ",'" & strSCoyID & "'," &
                    intGRNStatus & "," & intGRNLevel & ",'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ")"
                Common.Insert2Ary(strAryQuery, SqlQuery)
            Else
                SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF(ISNULL(GM_GRN_STATUS,1)= " & GRNStatus.Uninvoice & ",'ACK', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM GRN_MSTR WHERE GM_GRN_INDEX = " & intGRNIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)

                SqlQuery = "SELECT ISNULL(GM_GRN_STATUS,1) FROM GRN_MSTR WHERE GM_GRN_INDEX=" & intGRNIdx
                If objDb.GetVal(SqlQuery) = GRNStatus.Uninvoice Then
                    strNewGRNNo = "exist2" 'GRN already ack
                    Return False
                End If

                intGRNStatus = GRNStatus.Uninvoice
                strGRNNo = dsGRN.Tables(0).Rows(0)("GRNNo")
                intGRNIdx = dsGRN.Tables(0).Rows(0)("GRNINDEX")
                SqlQuery = "UPDATE GRN_Mstr set GM_LEVEL2_USER = '" & strLoginUser & "',GM_GRN_STATUS=" &
                intGRNStatus & " Where GM_GRN_INDEX=" & intGRNIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)
            End If

            ''//DOBULE CHECK
            'If UCase(strGRNType) <> "GRNACK" Then
            '    SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IFNULL(GM_GRN_NO,'') AS CHAR(1000)) FROM GRN_MSTR WHERE GM_B_COY_ID='" & Common.Parse(strCoyID) & "' AND GM_DO_INDEX = " & intDOIdx
            '    Common.Insert2Ary(strAryQuery, SqlQuery)

            '    If IsGRNCreated(strDONo, intDOIdx) Then
            '        strNewGRNNo = "exist1" 'GRN Created 
            '        Return False
            '    End If

            'Else
            '    SqlQuery = " SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF(ISNULL(GM_GRN_STATUS,1)= " & GRNStatus.Uninvoice & ",'ACK', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM GRN_MSTR WHERE GM_GRN_INDEX = " & intGRNIdx
            '    Common.Insert2Ary(strAryQuery, SqlQuery)

            '    SqlQuery = "SELECT ISNULL(GM_GRN_STATUS,1) FROM GRN_MSTR WHERE GM_GRN_INDEX=" & intGRNIdx
            '    If objDb.GetVal(SqlQuery) = GRNStatus.Uninvoice Then
            '        strNewGRNNo = "exist2" 'GRN already ack
            '        Return False
            '    End If
            'End If

            Dim dtGRNDtls As DataTable
            Dim drGRNDtl As DataRow
            Dim GRNLevel As Integer
            Dim dsGRNIdx As DataSet
            Dim intItemRow As Integer = 1

            Dim Received, Rejected As Decimal 'Integer
            dtGRNDtls = dsGRN.Tables(1) ' fr dtGRNDtls
            For Each drGRNDtl In dtGRNDtls.Rows
                If UCase(strGRNType) <> "GRNACK" Then
                    GRNLevel = 1
                    SqlQuery = "INSERT into GRN_Details(GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE," &
                           " GD_RECEIVED_QTY, GD_REJECTED_QTY,  GD_REMARKS)" &
                           "VALUES('" & strCoyID & "'," & strNewGRNNo & "," & drGRNDtl("PO_LINE") &
                           "," & drGRNDtl("Received_Qty") & "," & drGRNDtl("Rejected_Qty") &
                           ",'" & Common.Parse(drGRNDtl("REMARKS")) & "')"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                Else
                    GRNLevel = 2
                    SqlQuery = " UPDATE GRN_Details SET GD_REJECTED_QTY = " & drGRNDtl("Rejected_Qty") &
                            ",GD_REMARKS='" & Common.Parse(drGRNDtl("REMARKS")) & "' " &
                            " where GD_B_COY_ID ='" & strCoyID &
                            "' and GD_GRN_NO = '" & strGRNNo & "'" &
                            " and GD_PO_LINE =" & drGRNDtl("PO_LINE")
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
                'intGRNLevel
                If UCase(strGRNType) <> "GRNACK" Then
                    If intGRNLevel = 2 Then
                        'Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY, GL_REJECTED_QTY,GL_REMARKS, 
                        'GL_ACTION_BY,GL_ACTION_DT) SELECT (Select max(GM_GRN_INDEX) from GRN_Mstr)+1,1,1,1,0,'','myongnc',
                        'Convert(DateTime,'03/01/2005 15:09:55',103)
                        SqlQuery = "  Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY," &
                        " GL_REJECTED_QTY,GL_REMARKS," &
                        " GL_ACTION_BY,GL_ACTION_DT) SELECT (SELECT ISNULL(MAX(GM_GRN_INDEX),1) From GRN_Mstr)" &
                        "," & drGRNDtl("PO_LINE") & "," & GRNLevel & "," & drGRNDtl("Received_Qty") &
                        "," & drGRNDtl("Rejected_Qty") & ",'" & Common.Parse(drGRNDtl("REMARKS")) & "','" & strLoginUser &
                        "'," & Common.ConvertDate(Now())
                        Common.Insert2Ary(strAryQuery, SqlQuery)
                    End If
                Else
                    intGRNIdx = dsGRN.Tables(0).Rows(0)("GRNINDEX")
                    SqlQuery = "  Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY," &
                    " GL_REJECTED_QTY,GL_REMARKS," &
                    " GL_ACTION_BY,GL_ACTION_DT) VALUES(" &
                    intGRNIdx & "," & drGRNDtl("PO_LINE") & "," & GRNLevel & "," & drGRNDtl("Received_Qty") &
                    "," & drGRNDtl("Rejected_Qty") & ",'" & Common.Parse(drGRNDtl("REMARKS")) & "','" & strLoginUser &
                    "'," & Common.ConvertDate(Now()) & ")"
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If

                If intGRNStatus = GRNStatus.Uninvoice Then
                    'Update PO_Details
                    'POD_DELIVERED_QTY
                    SqlQuery = "UPDATE PO_DETAILS SET" &
                    " POD_RECEIVED_QTY = POD_RECEIVED_QTY + " & drGRNDtl("Received_Qty") &
                    " , POD_REJECTED_QTY = POD_REJECTED_QTY + " & drGRNDtl("Rejected_Qty") &
                    " , POD_DELIVERED_QTY = POD_DELIVERED_QTY - " & drGRNDtl("Rejected_Qty") &
                    " WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID='" & strCoyID & "' " &
                    " And POD_PO_LINE = " & drGRNDtl("PO_LINE")
                    Common.Insert2Ary(strAryQuery, SqlQuery)

                    If drGRNDtl("Rejected_Qty") <> 0 Then
                        'Update DO_Details
                        SqlQuery = "UPDATE DO_DETAILS SET DOD_DO_QTY = DOD_DO_QTY - " & drGRNDtl("Rejected_Qty") &
                        " WHERE DOD_S_COY_ID ='" & strSCoyID & "'" &
                        " and DOD_DO_NO = '" & strDONo & "'" &
                        " And DOD_PO_LINE = " & drGRNDtl("PO_LINE") & ""
                        Common.Insert2Ary(strAryQuery, SqlQuery)

                    End If
                    Received = Received + drGRNDtl("Received_Qty")
                    Rejected = Rejected + drGRNDtl("Rejected_Qty")

                    ' Yap: Inventory Module 15/Apr/2011
                    SqlQuery = " SELECT POD_VENDOR_ITEM_CODE FROM PO_DETAILS " &
                               " WHERE POD_PO_LINE = " & drGRNDtl("PO_LINE") & " AND POD_COY_ID = '" & strCoyID & "' AND POD_PO_NO = '" & strPONo & "'"
                    PM_PRODUCT = objDb.GetVal(SqlQuery)

                    iTotal = 0
                    '' ''For i = 0 To arySetLocation.Count - 1
                    '' ''    If PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;" Then
                    '' ''        'If i = 0 Then
                    '' ''        '    storItem = arySetLocation(i)(3)
                    '' ''        '    storItem_temp = arySetLocation(i)(3)
                    '' ''        'End If
                    '' ''        'storItem_temp = arySetLocation(i)(3)
                    '' ''        'iTotal = iTotal + CInt(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))

                    '' ''        'If (storItem <> storItem_temp) Or (i = arySetLocation.Count - 1) Then
                    '' ''        '    storItem = arySetLocation(i)(3)
                    '' ''        '    If iTotal < (Received + Rejected) Then
                    '' ''        '        objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
                    '' ''        '        arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((Received + Rejected) - iTotal), arySetLocation(i)(3), ""})
                    '' ''        '    Else
                    '' ''        '        strNewGRNNo = "LocationError"
                    '' ''        '        Return False
                    '' ''        '    End If
                    '' ''        '    iTotal = 0
                    '' ''        'End If
                    '' ''        iTotal = iTotal + CInt(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))

                    '' ''    End If
                    '' ''Next

                    objINV.GetDefaultLocationDesc(strLoginUser, strCoyID, LocDesc, SubLocDesc)
                    ' yAP: This to check each item total quantity inside the SET.
                    For i = 0 To arySetLocation.Count - 1
                        If (PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;") _
                            And arySetLocation(i)(2) <> "" And (CDec(IIf(arySetLocation(i)(8) = "", 0, arySetLocation(i)(8))) = intItemRow) Then 'And arySetLocation(i)(2) <> "" And (CInt(IIf(arySetLocation(i)(8) = "", 0, arySetLocation(i)(8))) = intItemRow) Then
                            iTotal = iTotal + arySetLocation(i)(2)
                        End If
                    Next

                    ' yAP: This to put the balance quantity to default location.
                    If iTotal < (drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) Then
                        'objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
                        arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) - iTotal), PM_PRODUCT, "", "", "", "", intItemRow})

                    ElseIf iTotal > (drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) Then
                        'strNewGRNNo = "LocationError"
                        strNewGRNNo = "RejectError"
                        Return False
                    End If

                    'Michelle (10/1/2013) - Issue 1835
                    'intItemRow += intItemRow
                    intItemRow += 1

                End If
            Next

            If intGRNStatus = GRNStatus.Uninvoice Then
                If Rejected = 0 Then
                    intDOStatus = DOStatus.FullyAccepted
                ElseIf Received <> Rejected Then
                    intDOStatus = DOStatus.PartiallyAccepted
                ElseIf Received = Rejected Then
                    intDOStatus = DOStatus.Rejected
                End If

                '//pending. if there is rejection. need to update fulfilment or not
                If intDOStatus = DOStatus.PartiallyAccepted Or intDOStatus = DOStatus.Rejected Then
                    Dim objDO As New DeliveryOrder
                    SqlQuery = objDO.SetPOFulFilment(strPONo, strCoyID)
                    Common.Insert2Ary(strAryQuery, SqlQuery)
                End If
                'Updata DO_Mstr
                'remark by Moo, DOM_GRN_Index is taken out
                SqlQuery = "UPDATE DO_MSTR SET DOM_DO_Status =" & intDOStatus &
                " WHERE DOM_PO_Index = " & intPOIdx &
                " AND DOM_DO_INDEX = " & intDOIdx
                Common.Insert2Ary(strAryQuery, SqlQuery)
            End If

            Dim objTrans As New Tracking
            If UCase(strGRNType) <> "GRNACK" Then
                SqlQuery = objTrans.updateDocMatchingNew(strPONo, strNewGRNNo, strDONo, "GRN", strSCoyID, strCoyID)
            Else
                SqlQuery = objTrans.updateDocMatching(strPONo, strNewGRNNo, strDONo, "GRN", strSCoyID, strCoyID)
            End If

            Common.Insert2Ary(strAryQuery, SqlQuery)
            objTrans = Nothing

            Dim objUsers As New Users
            If UCase(strGRNType) <> "GRNACK" Then
                objUsers.Log_UserActivityNew(strCoyID, strLoginUser, strAryQuery, WheelModule.Fulfillment, WheelUserActivity.B_GRN, strNewGRNNo, strDONo)
            Else
                objUsers.Log_UserActivity(strCoyID, strLoginUser, strAryQuery, WheelModule.Fulfillment, WheelUserActivity.B_GRNACK, strNewGRNNo, strDONo)
            End If
            objUsers = Nothing

            ' Yap: Inventory Module 15/Apr/2011 
            ' Check Duplicate
            Dim tempItem, tempLoc, tempSLoc As String
            Dim tempQty As Decimal = 0 'Integer = 0
            Dim j As Integer
            For i = 0 To arySetLocation.Count - 1
                tempItem = arySetLocation(i)(3)
                tempLoc = arySetLocation(i)(0)
                tempSLoc = arySetLocation(i)(1)
                For j = 0 To arySetLocation.Count - 1
                    If (arySetLocation(j)(4) <> "Done") And tempItem = arySetLocation(j)(3) And tempLoc = arySetLocation(j)(0) And tempSLoc = arySetLocation(j)(1) Then
                        arySetLocation(j)(0) = "---Select---"
                        arySetLocation(j)(4) = "Done"
                        tempQty = tempQty + IIf(arySetLocation(j)(2) = "", 0, arySetLocation(j)(2))
                    End If
                Next
                arySetLocation.Add(New String() {tempLoc, tempSLoc, tempQty, tempItem, "Done"})
                tempQty = 0
            Next

            ' Yap: Inventory Module 15/Apr/2011
            For i = 0 To arySetLocation.Count - 1
                SqlQuery = " SELECT * FROM INVENTORY_MSTR " &
                               " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"

                If objDb.Exist(SqlQuery) Then
                    If arySetLocation(i)(0) <> "---Select---" And arySetLocation(i)(0) <> "" And arySetLocation(i)(2) <> "0" Then

                        SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                   " WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                        PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                        If Common.parseNull(arySetLocation(i)(1)) = "" Then
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        Else
                            SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' AND LM_SUB_LOCATION = '" & arySetLocation(i)(1) & "' "
                            PM_LOC_INDEX = objDb.GetVal(SqlQuery)
                        End If

                        Dim ItemName As String
                        SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " &
                                   "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                        ItemName = objDb.GetVal(SqlQuery)

                        If UCase(strGRNType) <> "GRNACK" Then
                            SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                    " VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLocation(i)(2) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", " & strGRNNo & ", '" & strLoginUser & "','" & Common.Parse(ItemName) & "') "
                            Common.Insert2Ary(strAryQuery, SqlQuery)

                        Else
                            SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " &
                                    " VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLocation(i)(2) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", '" & strGRNNo & "', '" & strLoginUser & "','" & Common.Parse(ItemName) & "') "
                            Common.Insert2Ary(strAryQuery, SqlQuery)
                        End If

                        SqlQuery = " SELECT IM_IQC_IND FROM INVENTORY_MSTR " &
                                   " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                        QC = objDb.GetVal(SqlQuery)

                        If QC = "Y" Then

                            SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " &
                                   " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

                            If objDb.Exist(SqlQuery) Then

                            Else
                                SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " &
                                           " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", 0) "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If

                            SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " &
                                   " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
                            IM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

                            If UCase(strGRNType) <> "GRNACK" Then
                                SqlQuery = " INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " &
                                        " VALUES(" & strGRNNo & ", '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLocation(i)(2) & ")"
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            Else
                                SqlQuery = " INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " &
                                        " VALUES('" & strGRNNo & "', '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLocation(i)(2) & ")"
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If

                        Else
                            SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " &
                                   " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

                            If objDb.Exist(SqlQuery) Then
                                SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & arySetLocation(i)(2) &
                                           " WHERE ID_INVENTORY_INDEX = " & PM_PRODUCT_INDEX &
                                           " AND ID_LOCATION_INDEX = " & PM_LOC_INDEX
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            Else
                                SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " &
                                           " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & arySetLocation(i)(2) & ") "
                                Common.Insert2Ary(strAryQuery, SqlQuery)
                            End If
                        End If
                    End If
                End If
            Next

            SqlQuery = " SET @T_NO = " & strNewGRNNo & "; "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            SqlQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' "
            Common.Insert2Ary(strAryQuery, SqlQuery)

            Dim strNewGRNNum As String = ""
            If UCase(strGRNType) <> "GRNACK" Then
                If objDb.BatchExecuteForGRNDup(strAryQuery, , strNewGRNNum, "T_NO") Then
                    strNewGRNNo = strNewGRNNum
                    If strNewGRNNum <> "Generated" Then
                        If strNewGRNNum <> "exist2" Then
                            Dim objMail As New Email
                            If intGRNLevel = "1" Then
                                If intDOStatus = DOStatus.Rejected Then
                                    'objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                                Else
                                    'objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                                End If
                            Else
                                'objMail.sendNotification(EmailType.AckGRN, strLoginUser, strCoyID, strSCoyID, strNewGRNNo, "")
                            End If
                            objMail = Nothing
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    Return False
                End If

            Else
                If objDb.BatchExecute(strAryQuery) Then
                    Dim objMail As New Email
                    If intDOStatus = DOStatus.Rejected Then
                        'objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                    Else
                        'objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
                    End If
                    objMail = Nothing
                    Return True
                Else
                    Return False
                End If

            End If
            'If objDb.BatchExecute(strAryQuery) Then
            '    strNewGRNNum = strNewGRNNo
            '    Dim objMail As New Email
            '    If UCase(strGRNType) <> "GRNACK" Then
            '        If intGRNLevel = "1" Then
            '            If intDOStatus = DOStatus.Rejected Then
            '                objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNum)
            '            Else
            '                objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNum)
            '            End If
            '        Else
            '            objMail.sendNotification(EmailType.AckGRN, strLoginUser, strCoyID, strSCoyID, strNewGRNNum, "")
            '        End If
            '    Else
            '        If intDOStatus = DOStatus.Rejected Then
            '            objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
            '        Else
            '            objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
            '        End If
            '    End If
            '    objMail = Nothing
            '    Return True
            'Else
            '    Return False
            'End If
        End Function

        'Public Function GRNSubmit(ByVal dsGRN As DataSet, ByRef strNewGRNNo As String, ByVal strGRNType As String, ByVal blnRejectAll As Boolean, ByVal aryLoc As ArrayList) As Boolean
        '    Dim strAryQuery(0) As String
        '    Dim SqlQuery As String
        '    Dim strGRNPrefix, strCoyID, strLoginUser As String
        '    Dim OutStanding, Ordered, TotalDo_Qty, Do_Quantity As Integer
        '    Dim dteNow As DateTime = Now()
        '    Dim ds As DataSet
        '    Dim intTotRecord, intGRNStatus, intGRNLevel, intDOStatus As Integer
        '    Dim intIncrementNo As Integer = 0

        '    ' Yap: Inventory Module 15/Apr/2011
        '    Dim arySetLocation As New ArrayList()
        '    arySetLocation = aryLoc
        '    Dim i As Integer = 0
        '    Dim PM_LOC_INDEX As String
        '    Dim PM_PRODUCT_INDEX As String
        '    Dim IM_PRODUCT_INDEX, PM_PRODUCT As String
        '    Dim QC As String

        '    ' Insert Default Location
        '    Dim storItem, storItem_temp, LocDesc, SubLocDesc As String
        '    Dim iTotal As Integer = 0
        '    Dim objINV As New Inventory

        '    strCoyID = Common.Parse(HttpContext.Current.Session("CompanyId"))
        '    strLoginUser = Common.Parse(HttpContext.Current.Session("UserId"))

        '    '//ADD BY MOO
        '    Dim strSCoyID, strPONo, strDONo, strGRNNo, strFlag As String
        '    Dim intPOIdx, intDOIdx, intGRNIdx As Integer
        '    strSCoyID = Common.Parse(dsGRN.Tables(0).Rows(0)("SCoyID"))
        '    strPONo = dsGRN.Tables(0).Rows(0)("PONo")
        '    strDONo = dsGRN.Tables(0).Rows(0)("DONo")
        '    intPOIdx = dsGRN.Tables(0).Rows(0)("POIndex")
        '    intDOIdx = dsGRN.Tables(0).Rows(0)("DOIndex")

        '    If UCase(strGRNType) <> "GRNACK" Then
        '        objGlobal.GetLatestDocNo("GRN", strAryQuery, strNewGRNNo, strGRNPrefix)
        '        SqlQuery = "SELECT '*' FROM GRN_MSTR WHERE GM_GRN_NO='" & strNewGRNNo & "' AND GM_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        '        If objDb.Exist(SqlQuery) Then
        '            strNewGRNNo = "dup"
        '            Return False
        '        End If
        '        'SqlQuery = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN' "
        '        'Common.Insert2Ary(strAryQuery, SqlQuery)

        '        'intIncrementNo = 1
        '        'strNewGRNNo = " (SELECT GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
        '        '       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
        '        '       & " SEPARATOR '') AS cp_param_value FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'GRN') "

        '        'strNewGRNNo = Common.Parse(strNewGRNNo)
        '        strGRNNo = strNewGRNNo
        '        strGRNPrefix = Common.Parse(strGRNPrefix)
        '        '2 Level Receiving , CS_COY_ID , CS_FLAG_VALUE
        '        SqlQuery = "SELECT ISNULL(CS_FLAG_VALUE,0) from COMPANY_SETTING WHERE CS_COY_ID = '" & strCoyID & _
        '        "' AND CS_FLAG_NAME = '2 Level Receiving'"
        '        strFlag = objDb.GetVal(SqlQuery)
        '        If strFlag = "0" Then
        '            intGRNLevel = 1 '0 CHANGE BY MOO
        '            intGRNStatus = GRNStatus.Uninvoice
        '        Else
        '            If blnRejectAll Then
        '                intGRNLevel = 1 '0 CHANGE BY MOO
        '                intGRNStatus = GRNStatus.Uninvoice
        '            Else
        '                intGRNLevel = 2 '1 CHANGE BY MOO
        '                intGRNStatus = GRNStatus.PendingACK
        '            End If
        '        End If

        '        SqlQuery = "Insert into GRN_Mstr(GM_GRN_NO,GM_B_COY_ID,GM_PO_INDEX,GM_DATE_RECEIVED," & _
        '        " GM_NOOFCOPY_PRINTED, GM_DO_INDEX, GM_GRN_PREFIX, GM_S_COY_ID," & _
        '        " GM_GRN_STATUS, GM_GRN_LEVEL, GM_CREATED_BY, GM_CREATED_DATE)" & _
        '        " VALUES('" & Common.Parse(strNewGRNNo) & "','" & strCoyID & "'," & intPOIdx & "," & Common.ConvertDate(dsGRN.Tables(0).Rows(0)("GRNReceivedDt")) & _
        '        ", 0," & intDOIdx & ",'" & strGRNPrefix & "','" & strSCoyID & "'," & _
        '        intGRNStatus & "," & intGRNLevel & ",'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ")"
        '        Common.Insert2Ary(strAryQuery, SqlQuery)
        '    Else
        '        intGRNStatus = GRNStatus.Uninvoice
        '        strGRNNo = dsGRN.Tables(0).Rows(0)("GRNNo")
        '        intGRNIdx = dsGRN.Tables(0).Rows(0)("GRNINDEX")
        '        SqlQuery = "UPDATE GRN_Mstr set GM_LEVEL2_USER = '" & strLoginUser & "',GM_GRN_STATUS=" & _
        '        intGRNStatus & " Where GM_GRN_INDEX=" & intGRNIdx
        '        Common.Insert2Ary(strAryQuery, SqlQuery)
        '    End If

        '    '//DOBULE CHECK
        '    If UCase(strGRNType) <> "GRNACK" Then
        '        If IsGRNCreated(strDONo, intDOIdx) Then
        '            strNewGRNNo = "exist1" 'GRN Created 
        '            Return False
        '        End If
        '    Else
        '        SqlQuery = "SELECT ISNULL(GM_GRN_STATUS,1) FROM GRN_MSTR WHERE GM_GRN_INDEX=" & intGRNIdx
        '        If objDb.GetVal(SqlQuery) = GRNStatus.Uninvoice Then
        '            strNewGRNNo = "exist2" 'GRN already ack
        '            Return False
        '        End If
        '    End If

        '    Dim dtGRNDtls As DataTable
        '    Dim drGRNDtl As DataRow
        '    Dim GRNLevel As Integer
        '    Dim dsGRNIdx As DataSet

        '    Dim Received, Rejected As Integer
        '    dtGRNDtls = dsGRN.Tables(1) ' fr dtGRNDtls
        '    For Each drGRNDtl In dtGRNDtls.Rows
        '        If UCase(strGRNType) <> "GRNACK" Then
        '            GRNLevel = 1
        '            SqlQuery = "INSERT into GRN_Details(GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE," & _
        '                   " GD_RECEIVED_QTY, GD_REJECTED_QTY,  GD_REMARKS)" & _
        '                   "VALUES('" & strCoyID & "','" & Common.Parse(strNewGRNNo) & "'," & drGRNDtl("PO_LINE") & _
        '                   "," & drGRNDtl("Received_Qty") & "," & drGRNDtl("Rejected_Qty") & _
        '                   ",'" & Common.Parse(drGRNDtl("REMARKS")) & "')"
        '            Common.Insert2Ary(strAryQuery, SqlQuery)
        '        Else
        '            GRNLevel = 2
        '            SqlQuery = " UPDATE GRN_Details SET GD_REJECTED_QTY = " & drGRNDtl("Rejected_Qty") & _
        '                    ",GD_REMARKS='" & Common.Parse(drGRNDtl("REMARKS")) & "' " & _
        '                    " where GD_B_COY_ID ='" & strCoyID & _
        '                    "' and GD_GRN_NO = '" & strGRNNo & "'" & _
        '                    " and GD_PO_LINE =" & drGRNDtl("PO_LINE")
        '            Common.Insert2Ary(strAryQuery, SqlQuery)
        '        End If
        '        'intGRNLevel
        '        If UCase(strGRNType) <> "GRNACK" Then
        '            If intGRNLevel = 2 Then
        '                'Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY, GL_REJECTED_QTY,GL_REMARKS, 
        '                'GL_ACTION_BY,GL_ACTION_DT) SELECT (Select max(GM_GRN_INDEX) from GRN_Mstr)+1,1,1,1,0,'','myongnc',
        '                'Convert(DateTime,'03/01/2005 15:09:55',103)
        '                SqlQuery = "  Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY," & _
        '                " GL_REJECTED_QTY,GL_REMARKS," & _
        '                " GL_ACTION_BY,GL_ACTION_DT) SELECT (SELECT ISNULL(MAX(GM_GRN_INDEX),1) From GRN_Mstr)" & _
        '                "," & drGRNDtl("PO_LINE") & "," & GRNLevel & "," & drGRNDtl("Received_Qty") & _
        '                "," & drGRNDtl("Rejected_Qty") & ",'" & Common.Parse(drGRNDtl("REMARKS")) & "','" & strLoginUser & _
        '                "'," & Common.ConvertDate(Now())
        '                Common.Insert2Ary(strAryQuery, SqlQuery)
        '            End If
        '        Else
        '            intGRNIdx = dsGRN.Tables(0).Rows(0)("GRNINDEX")
        '            SqlQuery = "  Insert into GRN_DETAILS_ACK (GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY," & _
        '            " GL_REJECTED_QTY,GL_REMARKS," & _
        '            " GL_ACTION_BY,GL_ACTION_DT) VALUES(" & _
        '            intGRNIdx & "," & drGRNDtl("PO_LINE") & "," & GRNLevel & "," & drGRNDtl("Received_Qty") & _
        '            "," & drGRNDtl("Rejected_Qty") & ",'" & Common.Parse(drGRNDtl("REMARKS")) & "','" & strLoginUser & _
        '            "'," & Common.ConvertDate(Now()) & ")"
        '            Common.Insert2Ary(strAryQuery, SqlQuery)
        '        End If

        '        If intGRNStatus = GRNStatus.Uninvoice Then
        '            'Update PO_Details
        '            'POD_DELIVERED_QTY
        '            SqlQuery = "UPDATE PO_DETAILS SET" & _
        '            " POD_RECEIVED_QTY = POD_RECEIVED_QTY + " & drGRNDtl("Received_Qty") & _
        '            " , POD_REJECTED_QTY = POD_REJECTED_QTY + " & drGRNDtl("Rejected_Qty") & _
        '            " , POD_DELIVERED_QTY = POD_DELIVERED_QTY - " & drGRNDtl("Rejected_Qty") & _
        '            " WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID='" & strCoyID & "' " & _
        '            " And POD_PO_LINE = " & drGRNDtl("PO_LINE")
        '            Common.Insert2Ary(strAryQuery, SqlQuery)

        '            If drGRNDtl("Rejected_Qty") <> 0 Then
        '                'Update DO_Details
        '                SqlQuery = "UPDATE DO_DETAILS SET DOD_DO_QTY = DOD_DO_QTY - " & drGRNDtl("Rejected_Qty") & _
        '                " WHERE DOD_S_COY_ID ='" & strSCoyID & "'" & _
        '                " and DOD_DO_NO = '" & strDONo & "'" & _
        '                " And DOD_PO_LINE = " & drGRNDtl("PO_LINE") & ""
        '                Common.Insert2Ary(strAryQuery, SqlQuery)

        '            End If
        '            Received = Received + drGRNDtl("Received_Qty")
        '            Rejected = Rejected + drGRNDtl("Rejected_Qty")

        '            ' Yap: Inventory Module 15/Apr/2011
        '            SqlQuery = " SELECT POD_VENDOR_ITEM_CODE FROM PO_DETAILS " & _
        '                       " WHERE POD_PO_LINE = " & drGRNDtl("PO_LINE") & " AND POD_COY_ID = '" & strCoyID & "' AND POD_PO_NO = '" & strPONo & "'"
        '            PM_PRODUCT = objDb.GetVal(SqlQuery)

        '            iTotal = 0
        '            For i = 0 To arySetLocation.Count - 1
        '                If PM_PRODUCT = arySetLocation(i)(3) And PM_PRODUCT <> "&nbsp;" Then
        '                    'If i = 0 Then
        '                    '    storItem = arySetLocation(i)(3)
        '                    '    storItem_temp = arySetLocation(i)(3)
        '                    'End If
        '                    'storItem_temp = arySetLocation(i)(3)
        '                    'iTotal = iTotal + CInt(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))

        '                    'If (storItem <> storItem_temp) Or (i = arySetLocation.Count - 1) Then
        '                    '    storItem = arySetLocation(i)(3)
        '                    '    If iTotal < (Received + Rejected) Then
        '                    '        objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
        '                    '        arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((Received + Rejected) - iTotal), arySetLocation(i)(3), ""})
        '                    '    Else
        '                    '        strNewGRNNo = "LocationError"
        '                    '        Return False
        '                    '    End If
        '                    '    iTotal = 0
        '                    'End If
        '                    iTotal = iTotal + CInt(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))

        '                End If
        '            Next
        '            If iTotal < (drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) Then
        '                objINV.GetDefaultLocationDesc(LocDesc, SubLocDesc)
        '                arySetLocation.Add(New String() {LocDesc, SubLocDesc, ((drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) - iTotal), PM_PRODUCT, ""})
        '            ElseIf iTotal > (drGRNDtl("Received_Qty") - drGRNDtl("Rejected_Qty")) Then
        '                strNewGRNNo = "LocationError"
        '                Return False
        '            End If

        '        End If
        '    Next

        '    If intGRNStatus = GRNStatus.Uninvoice Then
        '        If Rejected = 0 Then
        '            intDOStatus = DOStatus.FullyAccepted
        '        ElseIf Received <> Rejected Then
        '            intDOStatus = DOStatus.PartiallyAccepted
        '        ElseIf Received = Rejected Then
        '            intDOStatus = DOStatus.Rejected
        '        End If

        '        '//pending. if there is rejection. need to update fulfilment or not
        '        If intDOStatus = DOStatus.PartiallyAccepted Or intDOStatus = DOStatus.Rejected Then
        '            Dim objDO As New DeliveryOrder
        '            SqlQuery = objDO.SetPOFulFilment(strPONo, strCoyID)
        '            Common.Insert2Ary(strAryQuery, SqlQuery)
        '        End If
        '        'Updata DO_Mstr
        '        'remark by Moo, DOM_GRN_Index is taken out
        '        SqlQuery = "UPDATE DO_MSTR SET DOM_DO_Status =" & intDOStatus & _
        '        " WHERE DOM_PO_Index = " & intPOIdx & _
        '        " AND DOM_DO_INDEX = " & intDOIdx
        '        Common.Insert2Ary(strAryQuery, SqlQuery)
        '    End If

        '    Dim objTrans As New Tracking
        '    SqlQuery = objTrans.updateDocMatching(strPONo, strNewGRNNo, strDONo, "GRN", strSCoyID, strCoyID)
        '    Common.Insert2Ary(strAryQuery, SqlQuery)
        '    objTrans = Nothing

        '    Dim objUsers As New Users
        '    If UCase(strGRNType) <> "GRNACK" Then
        '        objUsers.Log_UserActivity(strAryQuery, WheelModule.Fulfillment, WheelUserActivity.B_GRN, strNewGRNNo, strDONo)
        '    Else
        '        objUsers.Log_UserActivity(strAryQuery, WheelModule.Fulfillment, WheelUserActivity.B_GRNACK, strNewGRNNo, strDONo)
        '    End If
        '    objUsers = Nothing

        '    ' Yap: Inventory Module 15/Apr/2011 
        '    ' Check Duplicate
        '    Dim tempItem, tempLoc, tempSLoc As String
        '    Dim tempQty As Integer = 0
        '    Dim j As Integer
        '    For i = 0 To arySetLocation.Count - 1
        '        tempItem = arySetLocation(i)(3)
        '        tempLoc = arySetLocation(i)(0)
        '        tempSLoc = arySetLocation(i)(1)
        '        For j = 0 To arySetLocation.Count - 1
        '            If arySetLocation(j)(4) <> "Done" And tempItem = arySetLocation(j)(3) And tempLoc = arySetLocation(j)(0) And tempSLoc = arySetLocation(j)(1) Then
        '                arySetLocation(j)(0) = "---Select---"
        '                arySetLocation(j)(4) = "Done"
        '                tempQty = tempQty + IIf(arySetLocation(j)(2) = "", 0, arySetLocation(j)(2))
        '            End If
        '        Next
        '        arySetLocation.Add(New String() {tempLoc, tempSLoc, tempQty, tempItem, "Done"})
        '        tempQty = 0
        '    Next


        '    ' Yap: Inventory Module 15/Apr/2011
        '    For i = 0 To arySetLocation.Count - 1
        '        SqlQuery = " SELECT * FROM INVENTORY_MSTR " & _
        '                       " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"

        '        If objDb.Exist(SqlQuery) Then
        '            If arySetLocation(i)(0) <> "---Select---" And arySetLocation(i)(0) <> "" And arySetLocation(i)(2) <> "0" Then

        '                SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " & _
        '                           " WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
        '                PM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

        '                If Common.parseNull(arySetLocation(i)(1)) = "" Then
        '                    SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' "
        '                    PM_LOC_INDEX = objDb.GetVal(SqlQuery)
        '                Else
        '                    SqlQuery = "SELECT LM_LOCATION_INDEX FROM LOCATION_MSTR WHERE LM_COY_ID = '" & strCoyID & "' AND LM_LOCATION = '" & arySetLocation(i)(0) & "' AND LM_SUB_LOCATION = '" & arySetLocation(i)(1) & "' "
        '                    PM_LOC_INDEX = objDb.GetVal(SqlQuery)
        '                End If

        '                Dim ItemName As String
        '                SqlQuery = " SELECT IM_INVENTORY_NAME FROM INVENTORY_MSTR " & _
        '                           "WHERE IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
        '                ItemName = objDb.GetVal(SqlQuery)
        '                SqlQuery = " INSERT INTO INVENTORY_TRANS(IT_INVENTORY_INDEX, IT_TRANS_TYPE, IT_TRANS_QTY, IT_TRANS_DATE, IT_TO_LOCATION_INDEX, IT_TRANS_REF_NO, IT_TRANS_USER_ID, IT_INVENTORY_NAME) " & _
        '                           " VALUES (" & PM_PRODUCT_INDEX & ", 'II', " & arySetLocation(i)(2) & ", " & Common.ConvertDate(Now()) & ", " & PM_LOC_INDEX & ", '" & strGRNNo & "', '" & strLoginUser & "','" & Common.Parse(ItemName) & "') "
        '                Common.Insert2Ary(strAryQuery, SqlQuery)

        '                SqlQuery = " SELECT IM_IQC_IND FROM INVENTORY_MSTR " & _
        '                           " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
        '                QC = objDb.GetVal(SqlQuery)

        '                If QC = "Y" Then

        '                    SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " & _
        '                           " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

        '                    If objDb.Exist(SqlQuery) Then

        '                    Else
        '                        SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " & _
        '                                   " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", 0) "
        '                        Common.Insert2Ary(strAryQuery, SqlQuery)
        '                    End If

        '                    SqlQuery = " SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR " & _
        '                           " WHERE  IM_COY_ID = '" & strCoyID & "' AND IM_ITEM_CODE = '" & arySetLocation(i)(3) & "'"
        '                    IM_PRODUCT_INDEX = objDb.GetVal(SqlQuery)

        '                    SqlQuery = " INSERT INTO INVENTORY_VERIFY (IV_GRN_NO, IV_INVENTORY_INDEX, IV_LOCATION_INDEX, IV_VERIFY_STATUS, IV_RECEIVE_QTY) " & _
        '                                   " VALUES('" & strGRNNo & "', '" & IM_PRODUCT_INDEX & "', " & PM_LOC_INDEX & ", 'P', " & arySetLocation(i)(2) & ")"
        '                    Common.Insert2Ary(strAryQuery, SqlQuery)
        '                Else
        '                    SqlQuery = " SELECT '*' FROM INVENTORY_DETAIL " & _
        '                           " WHERE ID_INVENTORY_INDEX = '" & PM_PRODUCT_INDEX & "' AND ID_LOCATION_INDEX='" & PM_LOC_INDEX & "' "

        '                    If objDb.Exist(SqlQuery) Then
        '                        SqlQuery = " UPDATE INVENTORY_DETAIL SET ID_INVENTORY_QTY = ID_INVENTORY_QTY + " & arySetLocation(i)(2) & _
        '                                   " WHERE ID_INVENTORY_INDEX = " & PM_PRODUCT_INDEX & _
        '                                   " AND ID_LOCATION_INDEX = " & PM_LOC_INDEX
        '                        Common.Insert2Ary(strAryQuery, SqlQuery)
        '                    Else
        '                        SqlQuery = " INSERT INTO INVENTORY_DETAIL (ID_INVENTORY_INDEX, ID_LOCATION_INDEX, ID_INVENTORY_QTY) " & _
        '                                   " VALUES (" & PM_PRODUCT_INDEX & ", " & PM_LOC_INDEX & ", " & arySetLocation(i)(2) & ") "
        '                        Common.Insert2Ary(strAryQuery, SqlQuery)
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Next

        '    If objDb.BatchExecute(strAryQuery) Then
        '        Dim objMail As New Email
        '        If UCase(strGRNType) <> "GRNACK" Then
        '            If intGRNLevel = "1" Then
        '                If intDOStatus = DOStatus.Rejected Then
        '                    objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
        '                Else
        '                    objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
        '                End If
        '            Else
        '                objMail.sendNotification(EmailType.AckGRN, strLoginUser, strCoyID, strSCoyID, strNewGRNNo, "")
        '            End If
        '        Else
        '            If intDOStatus = DOStatus.Rejected Then
        '                objMail.sendNotification(EmailType.GoodsReceiptNoteReject, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
        '            Else
        '                objMail.sendNotification(EmailType.GoodsReceiptNoteCreated, strLoginUser, strCoyID, strSCoyID, strDONo, strPONo, strNewGRNNo)
        '            End If
        '        End If
        '        objMail = Nothing
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        Function IsTieToLocation(ByVal intDOIdx As Integer, ByVal intLevel As Integer)
            Dim lsSql, strCoyID, strLoginUser As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")
            lsSql = "SELECT '*' FROM DO_MSTR A, USERS_LOCATION B WHERE A.DOM_D_ADDR_CODE=UL_ADDR_CODE " _
            & "AND UL_COY_ID='" & strCoyID & "' AND UL_USER_ID='" & strLoginUser &
            "' AND UL_LEVEL=" & intLevel & " AND A.DOM_DO_INDEX=" & intDOIdx
            If objDb.Exist(lsSql) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function ShowDOdetails(ByVal companyID As String, ByVal strDONo As String, ByVal intDOIdx As Integer, ByVal IntPOIdx As Integer) As String
            Dim SQLQuery As String
            Dim dsDODtl As DataSet
            'DO_Mstr.DOM_GRN_INDEX ," & _
            SQLQuery = " SELECT Distinct PO_Details.POD_CANCELLED_QTY,PO_MSTR.POM_PO_Index,PO_Mstr.POM_PO_No,PO_Mstr.POM_PO_Status," &
                " PO_Details.POD_Product_Code, PO_Details.POD_Vendor_Item_Code, PO_Details.POD_B_Item_Code, PO_Details.POD_Product_Desc, " &
                " PO_Details.POD_UOM,PO_Details.POD_Po_Line,COMPANY_MSTR.CM_Coy_Name," &
                " PO_Details.POD_Product_Code as POD_Product_Code_D, PO_Mstr.POM_S_Coy_ID, PO_Details.POD_Delivered_Qty," &
                " PO_Details.POD_Ordered_Qty, PO_Details.POD_Received_Qty,PO_Details.POD_Rejected_Qty, " &
                " PO_Details.POD_ETD,PO_Details.POD_Min_Pack_Qty,PO_Details.POD_Min_Order_Qty,PO_Details.POD_Warranty_Terms," &
                " DO_Mstr.*,DO_Details.*,0 as POD_Outstanding,0 as GD_REJECTED_QTY " &
                " FROM PO_Mstr, PO_Details ,COMPANY_MSTR , DO_Mstr, DO_Details " &
                " WHERE DO_Mstr.DOM_DO_Index  = '" & intDOIdx & "'" &
                " And PO_Mstr.POM_PO_Index = " & IntPOIdx &
                " And PO_Mstr.POM_B_Coy_ID  = '" & companyID & "'" &
                " And DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" &
                " And DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO" &
                " And DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" &
                " And PO_Mstr.POM_PO_No = PO_Details.POD_Po_No" &
                " And PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID" &
                " And DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line" &
                " And COMPANY_MSTR.CM_Coy_ID = DO_Mstr.DOM_S_Coy_ID ORDER BY POD_Po_Line"

            dsDODtl = objDb.FillDs(SQLQuery)
            Return SQLQuery

        End Function
        Public Function ShowDOdetailsSet(ByVal companyID As String, ByVal strDONo As String, ByVal intDOIdx As Integer, ByVal IntPOIdx As Integer) As DataSet
            Dim SQLQuery As String
            Dim dsDODtl As DataSet
            'DO_Mstr.DOM_GRN_INDEX ," & _
            SQLQuery = " SELECT Distinct PO_Details.POD_CANCELLED_QTY,PO_MSTR.POM_PO_Index,PO_Mstr.POM_PO_No,PO_Mstr.POM_PO_Status," &
                " PO_Details.POD_Product_Code, PO_Details.POD_Vendor_Item_Code, PO_Details.POD_B_Item_Code, PO_Details.POD_Product_Desc, " &
                " PO_Details.POD_UOM,PO_Details.POD_Po_Line,COMPANY_MSTR.CM_Coy_Name," &
                " PO_Details.POD_Product_Code as POD_Product_Code_D, PO_Mstr.POM_S_Coy_ID, PO_Details.POD_Delivered_Qty," &
                " PO_Details.POD_Ordered_Qty, PO_Details.POD_Received_Qty,PO_Details.POD_Rejected_Qty, " &
                " PO_Details.POD_ETD,PO_Details.POD_Min_Pack_Qty,PO_Details.POD_Min_Order_Qty,PO_Details.POD_Warranty_Terms," &
                " DO_Mstr.*,DO_Details.*,0 as POD_Outstanding,0 as GD_REJECTED_QTY " &
                " FROM PO_Mstr, PO_Details ,COMPANY_MSTR , DO_Mstr, DO_Details " &
                " WHERE DO_Mstr.DOM_DO_Index  = '" & intDOIdx & "'" &
                " And PO_Mstr.POM_PO_Index = " & IntPOIdx &
                " And PO_Mstr.POM_B_Coy_ID  = '" & companyID & "'" &
                " And DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" &
                " And DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO" &
                " And DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" &
                " And PO_Mstr.POM_PO_No = PO_Details.POD_Po_No" &
                " And PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID" &
                " And DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line" &
                " And COMPANY_MSTR.CM_Coy_ID = DO_Mstr.DOM_S_Coy_ID ORDER BY POD_Po_Line"

            dsDODtl = objDb.FillDs(SQLQuery)
            Return dsDODtl

        End Function

        Public Function GetAckDetails(ByVal GRNIdx As Integer, ByVal GrnPoLine As Integer) As DataSet
            Dim dsAck As DataSet
            Dim SqlQuery As String
            SqlQuery = "select GL_GRN_INDEX,GL_PO_LINE,GL_LEVEL,GL_RECEIVED_QTY,GL_REJECTED_QTY," &
                        " GL_REMARKS, GL_ACTION_BY, GL_ACTION_DT,UM_USER_NAME AS ACTION_NAME " &
                        " FROM GRN_DETAILS_ACK A, USER_MSTR B" &
                        " WHERE A.GL_ACTION_BY=B.UM_USER_ID AND B.UM_COY_ID='" &
                        HttpContext.Current.Session("CompanyID") & "' AND GL_GRN_INDEX = " & GRNIdx &
                        " AND GL_PO_LINE = " & GrnPoLine & " order BY GL_LEVEL"
            dsAck = objDb.FillDs(SqlQuery)
            Return dsAck
        End Function
        '//this Function also called during GRN ACK, so hv to pass in DO Index because Dropdownlist can only store 2 values

        Public Function GetGRNDetails(ByVal intDOIdx As Integer) As DataSet
            Dim dsGRN As DataSet
            Dim SqlQuery As String
            SqlQuery = "select PO_Mstr.POM_PO_Index,POM_BUYER_NAME,PO_Mstr.POM_PO_NO,PO_Details.POD_VENDOR_ITEM_CODE,PO_Details.POD_PRODUCT_DESC," &
                        "PO_Details.POD_UOM,DOM_DO_NO,DOM_D_ADDR_LINE1,DOM_D_ADDR_LINE2,DOM_D_ADDR_LINE3,DOM_D_POSTCODE,DOM_D_CITY,DOM_D_STATE,DOM_D_COUNTRY,POD_MIN_PACK_QTY,PO_Details.POD_ORDERED_QTY,GM_GRN_LEVEL," &
                        "GM_PO_INDEX,GM_LEVEL2_USER,GM_GRN_INDEX,GM_GRN_NO,GM_DATE_RECEIVED,GM_CREATED_BY,GM_CREATED_DATE,UM_TEL_NO,UM_EMAIL,UM_USER_NAME AS GRN_Created_Name," &
                        "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,GRN_Mstr.GM_GRN_STATUS," &
                        "PO_Details.POD_Po_Line,PO_Details.POD_PR_INDEX,PO_Details.POD_B_ITEM_CODE,DOM_DO_INDEX,GM_GRN_LEVEL,0 as POD_Outstanding, " &
                        "DO_Details.DOD_SHIPPED_QTY,PO_Details.POD_DELIVERED_QTY,PO_Details.POD_CANCELLED_QTY,DO_DEtails.DOD_S_COY_ID,PO_Mstr.POM_B_Coy_ID,POM_S_Coy_ID " &
                        " from GRN_DEtails, GRN_Mstr, PO_Mstr, PO_Details,DO_Mstr,DO_Details,User_Mstr " &
                        " where GRN_Details.GD_GRN_NO = GRN_Mstr.GM_GRN_NO" &
                        " And GRN_Mstr.GM_B_COY_ID = GRN_Details.GD_B_COY_ID" &
                        " And GRN_Mstr.GM_PO_INDEX = PO_Mstr.POM_PO_INDEX" &
                        " And PO_Mstr.POM_PO_No = PO_Details.POD_Po_No" &
                        " And PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID" &
                        " And GRN_DEtails.GD_PO_LINE = PO_Details.POD_Po_Line" &
                        " And DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" &
                        " And DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO" &
                        " And DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" &
                        " And DO_Mstr.DOM_DO_INDEX = GRN_Mstr.GM_DO_INDEX" &
                        " And DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line" &
                        " And GRN_MSTR.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID='" &
                        HttpContext.Current.Session("CompanyId") & "'" &
                        " And DO_Mstr.DOM_DO_INDEX = " & intDOIdx & " ORDER BY GD_PO_LINE"
            dsGRN = objDb.FillDs(SqlQuery)
            Return dsGRN
        End Function
        Public Function GetGRNHistory(ByVal strGRNNo As String, ByVal strBCoyID As String) As DataSet
            Dim dsGRN As DataSet
            Dim SqlQuery As String
            SqlQuery = "select C.POM_PO_NO,D.POD_VENDOR_ITEM_CODE,D.POD_PRODUCT_DESC," &
                        "D.POD_UOM,POD_MIN_PACK_QTY,D.POD_ORDERED_QTY,B.GM_GRN_LEVEL," &
                        "GM_PO_INDEX,GM_GRN_INDEX,GM_GRN_NO,GM_CREATED_DATE,GM_DATE_RECEIVED,GM_CREATED_BY,UM_USER_NAME AS GRN_Created_Name," &
                        "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,B.GM_GRN_STATUS," &
                        "D.POD_Po_Line,D.POD_B_ITEM_CODE,GM_GRN_LEVEL,0 as POD_Outstanding,E.DOM_DO_NO,DOD_SHIPPED_QTY  " &
                        " From GRN_Details A, GRN_Mstr B, PO_Mstr C, PO_Details D,DO_Mstr E,User_Mstr,DO_DETAILS DD " &
                        " where A.GD_GRN_NO = B.GM_GRN_NO" &
                        " And B.GM_B_COY_ID = A.GD_B_COY_ID" &
                        " And B.GM_PO_INDEX = C.POM_PO_INDEX" &
                        " And C.POM_PO_No = D.POD_Po_No" &
                        " And C.POM_B_Coy_ID = D.POD_Coy_ID" &
                        " And A.GD_PO_LINE = D.POD_Po_Line" &
                        " And E.DOM_PO_Index = C.POM_PO_Index" &
                        " And E.DOM_DO_INDEX = B.GM_DO_INDEX" &
                        " AND E.DOM_DO_NO = DD.DOD_DO_NO AND  E.DOM_S_COY_ID = DD.DOD_S_COY_ID AND  D.POD_PO_LINE = DD.DOD_PO_LINE" &
                        " And B.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID='" &
                         strBCoyID & "'" &
                        " And B.GM_GRN_NO='" & strGRNNo & "' And A.GD_B_COY_ID='" & strBCoyID & "' ORDER BY GD_PO_LINE"
            dsGRN = objDb.FillDs(SqlQuery)
            Return dsGRN
        End Function
        Public Function GetGRNHistoryString(ByVal strGRNNo As String, ByVal strBCoyID As String) As String
            Dim dsGRN As DataSet
            Dim SqlQuery As String
            SqlQuery = "select C.POM_PO_NO,D.POD_VENDOR_ITEM_CODE,D.POD_PRODUCT_DESC," &
                        "D.POD_UOM,POD_MIN_PACK_QTY,D.POD_ORDERED_QTY,B.GM_GRN_LEVEL," &
                        "GM_PO_INDEX,GM_GRN_INDEX,GM_GRN_NO,GM_CREATED_DATE,GM_DATE_RECEIVED,GM_CREATED_BY,UM_USER_NAME AS GRN_Created_Name," &
                        "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,B.GM_GRN_STATUS," &
                        "D.POD_Po_Line,D.POD_B_ITEM_CODE,GM_GRN_LEVEL as GM_GRN_LVL,0 as POD_Outstanding,E.DOM_DO_NO,DOD_SHIPPED_QTY  " &
                        " From GRN_Details A, GRN_Mstr B, PO_Mstr C, PO_Details D,DO_Mstr E,User_Mstr,DO_DETAILS DD " &
                        " where A.GD_GRN_NO = B.GM_GRN_NO" &
                        " And B.GM_B_COY_ID = A.GD_B_COY_ID" &
                        " And B.GM_PO_INDEX = C.POM_PO_INDEX" &
                        " And C.POM_PO_No = D.POD_Po_No" &
                        " And C.POM_B_Coy_ID = D.POD_Coy_ID" &
                        " And A.GD_PO_LINE = D.POD_Po_Line" &
                        " And E.DOM_PO_Index = C.POM_PO_Index" &
                        " And E.DOM_DO_INDEX = B.GM_DO_INDEX" &
                        " AND E.DOM_DO_NO = DD.DOD_DO_NO AND  E.DOM_S_COY_ID = DD.DOD_S_COY_ID AND  D.POD_PO_LINE = DD.DOD_PO_LINE" &
                        " And B.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID='" &
                         strBCoyID & "'" &
                        " And B.GM_GRN_NO='" & strGRNNo & "' And A.GD_B_COY_ID='" & strBCoyID & "' ORDER BY GD_PO_LINE"
            dsGRN = objDb.FillDs(SqlQuery)
            Return SqlQuery
        End Function
        '*******************meilai 2/3/05******************************************
        'Public Function GetGRNDetails1(ByVal strGRNNo As String, ByVal strBCoyID As String) As DataSet
        '    Dim dsGRN As DataSet
        '    Dim SqlQuery As String
        '    'SqlQuery = "select C.POM_PO_NO,C.POM_REFERENCE_NO,D.POD_VENDOR_ITEM_CODE,D.POD_PRODUCT_DESC," & _
        '    '           "D.POD_UOM,D.POD_DELIVERED_QTY,POD_MIN_PACK_QTY,D.POD_ORDERED_QTY,B.GM_GRN_LEVEL,GM_PO_INDEX," & _
        '    '           "GM_GRN_INDEX(, GM_GRN_NO, GM_DATE_RECEIVED, GM_CREATED_BY, UM_USER_NAME)" & _
        '    '           "AS GRN_Created_Name,GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY," & _
        '    '           "GD_REJECTED_QTY, GD_REMARKS,B.GM_GRN_STATUS,D.POD_Po_Line," & _
        '    '           "D.POD_B_ITEM_CODE,GM_GRN_LEVEL,0 as POD_Outstanding,E.DOM_DO_NO" & _
        '    '           "From GRN_Details A, GRN_Mstr B, PO_Mstr C, PO_Details D,DO_Mstr E,User_Mstr" & _
        '    '           "where(A.GD_GRN_NO = B.GM_GRN_NO And B.GM_B_COY_ID = A.GD_B_COY_ID)" & _
        '    '           "And B.GM_PO_INDEX = C.POM_PO_INDEX And C.POM_PO_No = D.POD_Po_No" & _
        '    '           "And C.POM_B_Coy_ID = D.POD_Coy_ID And A.GD_PO_LINE = D.POD_Po_Line" & _
        '    '           "And E.DOM_PO_Index = C.POM_PO_Index And E.DOM_DO_INDEX = B.GM_DO_INDEX" & _
        '    '           "And B.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID='demo'" & _
        '    '           "And B.GM_GRN_NO='" & strGRNNo & "' ORDER BY GD_PO_LINE"
        '    SqlQuery = "select C.POM_PO_NO,C.POM_REFERENCE_NO,B.GM_B_COY_ID,B.GM_CREATED_DATE,B.GM_CREATED_BY,B.GM_DATE_RECEIVED,D.POD_VENDOR_ITEM_CODE,D.POD_DELIVERED_QTY,D.POD_PRODUCT_DESC," & _
        '                "D.POD_UOM,POD_MIN_PACK_QTY,D.POD_ORDERED_QTY,B.GM_GRN_LEVEL," & _
        '                "GM_PO_INDEX,GM_GRN_INDEX,GM_GRN_NO,GM_DATE_RECEIVED,GM_CREATED_BY,UM_USER_NAME AS GRN_Created_Name," & _
        '                "GD_B_COY_ID,GD_GRN_NO,GD_PO_LINE, GD_RECEIVED_QTY, GD_REJECTED_QTY, GD_REMARKS,B.GM_GRN_STATUS," & _
        '                "D.POD_Po_Line,D.POD_B_ITEM_CODE,GM_GRN_LEVEL,0 as POD_Outstanding,E.DOM_DO_NO " & _
        '                " From GRN_Details A, GRN_Mstr B, PO_Mstr C, PO_Details D,DO_Mstr E,User_Mstr " & _
        '                " where A.GD_GRN_NO = B.GM_GRN_NO" & _
        '                " And B.GM_B_COY_ID = A.GD_B_COY_ID" & _
        '                " And B.GM_PO_INDEX = C.POM_PO_INDEX" & _
        '                " And C.POM_PO_No = D.POD_Po_No" & _
        '                " And C.POM_B_Coy_ID = D.POD_Coy_ID" & _
        '                " And A.GD_PO_LINE = D.POD_Po_Line" & _
        '                " And E.DOM_PO_Index = C.POM_PO_Index" & _
        '                " And E.DOM_DO_INDEX = B.GM_DO_INDEX" & _
        '                " And B.GM_CREATED_BY = User_Mstr.UM_User_ID And UM_Coy_ID=" & _
        '                 strBCoyID & " " & _
        '                " And B.GM_GRN_NO=" & strGRNNo & " ORDER BY GD_PO_LINE"
        '    dsGRN = objDb.FillDs(SqlQuery)
        '    Return dsGRN
        'End Function
#Region " FOR GRN ACK - BY MOO"
        Function getPOListForGRNAck(ByVal strPO As String, ByRef blnValid As Boolean, ByRef strMsg As String) As DataSet
            Dim dsGRN, dsAddr As DataSet
            Dim SqlQuery, strsql As String
            strsql = "SELECT '*' FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(strPO) & "' "
            strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If objDb.Exist(strsql) > 0 Then
                blnValid = True
                ' ai chu modified on 25/11/2005
                ' user request to split the error message to clearer message when acknowledging GRN
                SqlQuery = "SELECT DISTINCT POM_B_COY_ID,POM_PO_NO,POM_PO_INDEX, 1 as DoExists FROM DO_MSTR B, GRN_MSTR C, PO_MSTR A " &
                            " LEFT JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                            "WHERE A.POM_PO_INDEX = B.DOM_PO_INDEX AND A.POM_PO_INDEX=C.GM_PO_INDEX " &
                            "AND B.DOM_DO_INDEX=C.GM_DO_INDEX AND C.GM_GRN_STATUS=" & GRNStatus.PendingACK &
                            " AND POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " &
                            " AND POM_PO_NO = '" & Common.Parse(strPO) & "' "
                dsGRN = objDb.FillDs(SqlQuery)

                SqlQuery = "SELECT DISTINCT POM_B_COY_ID,POM_PO_NO,POM_PO_INDEX, 1 as DoExists " &
                            "FROM PO_MSTR A LEFT JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                            "WHERE POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " &
                            "AND POM_PO_NO = '" & Common.Parse(strPO) & "' " &
                            "AND POD_D_ADDR_CODE IN (SELECT UL_ADDR_CODE FROM USERS_LOCATION " &
                            "WHERE UL_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
                            "AND UL_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND UL_LEVEL = 2 ) "
                dsAddr = objDb.FillDs(SqlQuery)

                If dsGRN.Tables(0).Rows.Count = 0 Or dsAddr.Tables(0).Rows.Count = 0 Then
                    If dsGRN.Tables(0).Rows.Count = 0 Then
                        strMsg = "No GRN has been created for this PO"
                    Else
                        strMsg = "No Access to the PO Item's Delivery Address"
                    End If
                    SqlQuery = "SELECT POM_PO_No, POM_PO_Index, 0 as DoExists FROM PO_MSTR "
                    SqlQuery &= "WHERE POM_PO_NO = '" & Common.Parse(strPO) & "' "
                    SqlQuery &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "
                    dsGRN = objDb.FillDs(SqlQuery)
                End If
            Else
                blnValid = False
            End If

            Return dsGRN
        End Function

        Function getDOListForGRNAck(ByVal intPOIdx As Integer) As DataSet
            Dim dsGRN As DataSet
            Dim SqlQuery As String
            SqlQuery = "SELECT DOM_DO_NO,DOM_DO_INDEX FROM PO_MSTR A, DO_MSTR B, GRN_MSTR C " &
            "WHERE A.POM_PO_INDEX=B.DOM_PO_INDEX AND A.POM_PO_INDEX=C.GM_PO_INDEX " &
            "AND B.DOM_DO_INDEX=C.GM_DO_INDEX AND C.GM_GRN_STATUS=" & GRNStatus.PendingACK &
            " AND POM_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' AND POM_PO_INDEX=" & intPOIdx &
            " AND DOM_D_ADDR_CODE IN (SELECT UL_ADDR_CODE FROM USERS_LOCATION " &
            " WHERE UL_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
            " AND UL_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND UL_LEVEL = 2)"


            dsGRN = objDb.FillDs(SqlQuery)
            Return dsGRN
        End Function
#End Region
#Region " FOR GRN LEVEL 1"
        Public Function getPOListForOutsDO(ByVal userID As String, ByVal companyID As String, ByVal strPO As String, ByRef blnValid As Boolean, ByRef strMsg As String) As DataSet
            '//PLAN TO RENAME TO getPOListForOutsDO
            Dim strDefaultValue As String
            Dim SQLQuery, strsql As String
            Dim dsPO, dsAddr As DataSet
            strsql = "SELECT '*' FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(strPO) & "' "
            strsql &= "AND POM_B_COY_ID = '" & companyID & "' "
            If objDb.Exist(strsql) > 0 Then
                blnValid = True
                ' ai chu modified on 25/11/2005
                ' user request to split the error message to clearer message when creating GRN
                SQLQuery = "SELECT distinct PO_MSTR.POM_PO_No,PO_MSTR.POM_PO_Index, 1 as DoExists " &
                            " From PO_MSTR " &
                            " LEFT JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID" &
                            " Where POM_B_COY_ID='" & companyID &
                            "' AND PO_MSTR.POM_BILLING_METHOD <> 'DO' AND " &
                            " POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR A WHERE DOM_DO_STATUS=" &
                            DOStatus.Submitted & " AND DOM_DO_INDEX NOT IN (SELECT GM_DO_INDEX FROM GRN_MSTR B WHERE B.GM_PO_INDEX=A.DOM_PO_INDEX))" &
                            " AND POM_PO_NO = '" & Common.Parse(strPO) & "' "
                dsPO = objDb.FillDs(SQLQuery)

                'SQLQuery = "SELECT DISTINCT PO_MSTR.POM_PO_No,PO_MSTR.POM_PO_Index, 1 as DoExists " & _
                '            "From PO_MSTR LEFT JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                '            "Where POM_B_COY_ID='" & companyID & "' " & _
                '            "AND PO_MSTR.POM_BILLING_METHOD <> 'DO' AND POM_PO_NO = '" & Common.Parse(strPO) & "' " & _
                '            "AND POD_D_ADDR_CODE IN (SELECT UL_ADDR_CODE FROM USERS_LOCATION " & _
                '            "WHERE UL_COY_ID = '" & companyID & "' " & _
                '            "AND UL_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND UL_LEVEL = 1)"
                SQLQuery = "SELECT DISTINCT DOM_Do_No,DOM_DO_INDEX, DOM_D_ADDR_CODE From DO_MSTR " &
                            "INNER JOIN PO_MSTR ON POM_PO_INDEX = DOM_PO_INDEX Where POM_PO_NO = '" & Common.Parse(strPO) & "' " &
                            "And DOM_DO_Status in(2) AND DOM_DO_INDEX NOT IN " &
                            "(SELECT GM_DO_INDEX FROM GRN_MSTR INNER JOIN PO_MSTR ON POM_PO_INDEX = GM_PO_INDEX " &
                            "WHERE POM_PO_NO = '" & Common.Parse(strPO) & "')  AND DOM_D_ADDR_CODE IN " &
                            "(SELECT UL_ADDR_CODE FROM USERS_LOCATION  WHERE UL_COY_ID = '" & companyID & "' " &
                            "AND UL_USER_ID = '" & userID & "' AND UL_LEVEL = 1)"
                dsAddr = objDb.FillDs(SQLQuery)

                If dsPO.Tables(0).Rows.Count = 0 Or dsAddr.Tables(0).Rows.Count = 0 Then
                    If dsPO.Tables(0).Rows.Count = 0 Then
                        strMsg = "No DO has been created for this PO"
                    Else
                        strMsg = "No Access to the PO Item's Delivery Address"
                    End If
                    SQLQuery = "SELECT POM_PO_No, POM_PO_Index, 0 as DoExists FROM PO_MSTR "
                    SQLQuery &= "WHERE POM_PO_NO = '" & Common.Parse(strPO) & "' "
                    SQLQuery &= "AND POM_B_COY_ID = '" & companyID & "' "
                    dsPO = objDb.FillDs(SQLQuery)
                End If
            Else
                blnValid = False
            End If

            Return dsPO
        End Function
        '//rename to getDOListForGRNLevel1
        Public Function getDOListForGRNLevel1(ByVal PoIdx As Integer) As DataSet

            Dim strDefaultValue As String
            Dim SQLQuery As String
            Dim dsDO As DataSet
            'Dim drw As DataView

            'SQLQuery = " Select distinct DOM_Do_No,DOM_DO_INDEX " & _
            '            " From DO_MSTR " & _
            '            " Where DOM_PO_INDEX ='" & PoIdx & "'" & _
            '            " And DOM_DO_Status in(" & DOStatus.Submitted & ")"

            '//modified by Moo, to filter OUT DO that has GRN (because of GRN Ack)

            SQLQuery = " Select distinct DOM_Do_No,DOM_DO_INDEX " &
            " From DO_MSTR Where DOM_PO_INDEX =" & PoIdx & " And DOM_DO_Status in(" &
            DOStatus.Submitted & ") AND DOM_DO_INDEX NOT IN (SELECT GM_DO_INDEX FROM GRN_MSTR WHERE GM_PO_INDEX=" & PoIdx & ") " &
            " AND DOM_D_ADDR_CODE IN (SELECT UL_ADDR_CODE FROM USERS_LOCATION " &
            " WHERE UL_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " &
            " AND UL_USER_ID = '" & HttpContext.Current.Session("UserID") & "' AND UL_LEVEL = 1)"

            dsDO = objDb.FillDs(SQLQuery)

            ' drw = objDB.GetView(SQLQuery)          
            Return dsDO

        End Function
#End Region
    End Class
End Namespace

