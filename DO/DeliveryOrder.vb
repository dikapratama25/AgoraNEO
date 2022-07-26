Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component


Namespace AgoraNEO
    Public Class DeliveryOrder
        Dim objDb As New EAD.DBCom
        Dim objUsr As New Users
        Dim objGlobal As New AgoraNEO.AppGlobals
        Public dtDOMstr As New DataTable
        Public dtDODtls As New DataTable

        Public Sub Init()
            dtDOMstr.Columns.Add("DOM_DO_Date", Type.GetType("System.DateTime"))
            dtDOMstr.Columns.Add("DOM_S_Ref_No", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_S_REF_DATE", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_PO_INDEX", Type.GetType("System.Double"))
            dtDOMstr.Columns.Add("DOM_D_Addr_Code", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_WAYBILL_NO", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_FREIGHT_CARRIER", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_FREIGHT_AMT", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_DO_REMARKS", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("DOM_CREATED_DATE", Type.GetType("System.DateTime"))
            dtDOMstr.Columns.Add("POD_PO_NO", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_Addr_Line1", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_Addr_Line2", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_Addr_Line3", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_State", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_Country", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_PostCode", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_D_City", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("POD_B_COY_ID", Type.GetType("System.String"))
            dtDOMstr.Columns.Add("CDDA_DOC_NO", Type.GetType("System.String"))

            dtDODtls.Columns.Add("DOD_DO_LINE", Type.GetType("System.String"))
            dtDODtls.Columns.Add("DOD_PO_LINE", Type.GetType("System.String"))
            dtDODtls.Columns.Add("DOD_DO_QTY", Type.GetType("System.String"))
            dtDODtls.Columns.Add("DOD_SHIPPED_QTY", Type.GetType("System.String"))
            dtDODtls.Columns.Add("DOD_REMARKS", Type.GetType("System.String"))
            dtDODtls.Columns.Add("DOD_Outstanding", Type.GetType("System.String"))
            dtDODtls.Columns.Add("DOD_LotNo", Type.GetType("System.String"))
        End Sub

        Function getVendorItemCode(ByVal companyid As String, ByVal strPONo As String, ByVal strBCoyID As String, ByVal strDONo As String, ByVal strVendorCode As String) As String
            Dim strSql, strTemp, strTemp2 As String
            'strSql = "Select POD_VENDOR_ITEM_CODE From PO_Details where POD_COY_ID='" & strBCoyID & "' and POD_PO_NO='" & strPONo & "'"
            strSql = "SELECT distinct POD_VENDOR_ITEM_CODE " &
                   " FROM DO_Mstr, DO_Details, PO_Details, PO_Mstr " &
                   " WHERE DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO and " &
                   " DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID and " &
                   " DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index and " &
                   " PO_Mstr.POM_PO_No = PO_Details.POD_Po_No and " &
                   " PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID and " &
                   " DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line and " &
                   " DO_Mstr.DOM_S_Coy_ID = '" & companyid & "' and " &
                   " POD_COY_ID='" & strBCoyID & "' AND PO_Mstr.POM_PO_NO='" & strPONo & "' AND " &
                   " DO_Mstr.DOM_DO_NO='" & strDONo & "'"

            If strVendorCode <> "" Then
                strTemp2 = Common.BuildWildCard(strVendorCode)
                strSql = strSql & " AND PO_Details.POD_VENDOR_ITEM_CODE" & Common.ParseSQL(strTemp2)
            End If

            Dim tDS As DataSet = objDb.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If strTemp = "" Then
                    strTemp = Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_VENDOR_ITEM_CODE"))
                Else
                    strTemp = strTemp & "," & Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_VENDOR_ITEM_CODE"))
                End If
            Next


            Dim iLoop As Integer
            Dim strTemp1 As String
            If Len(strTemp) > 0 Then
                For iLoop = 0 To Len(strTemp)
                    If Len(strTemp) > 0 Then
                        If iLoop = 0 Then
                            strTemp1 = Left(strTemp, 25)
                        Else
                            strTemp1 = strTemp1 & "<BR>" & Left(strTemp, 25)
                        End If
                        strTemp = Mid(strTemp, 26)
                    End If
                Next
                strTemp = strTemp1
            End If
            Return strTemp
        End Function
        Public Function GetDO(ByVal userID As String, ByVal strDocType As String, ByVal strDocNo As String, ByVal strCreationDt As String, ByVal strSubmittedDt As String, ByVal strOurRef As String, ByVal strBuyerComp As String, ByVal strVenItem As String, ByVal strStatus As String) As String
            Dim dsDO As DataSet
            Dim strsqlDO As String
            Dim strTemp As String
            Dim companyID As String = objUsr.getCompanyID(userID)

            'strsqlDO = "SELECT distinct PO_Details.POD_Vendor_Item_Code, PO_Mstr.POM_PO_Index,PO_MSTR.POM_S_Coy_Name,DO_Mstr.DOM_DO_NO," & _
            strsqlDO = "SELECT distinct PO_Mstr.POM_PO_Index,PO_MSTR.POM_S_Coy_Name,DO_Mstr.DOM_DO_NO," &
                    " DO_Mstr.DOM_S_Ref_No,DO_Mstr.DOM_S_Coy_ID, DO_Mstr.DOM_DO_Index, DO_Mstr.DOM_PO_Index, " &
                    " PO_Mstr.POM_B_Coy_ID,DO_Mstr.DOM_Created_Date,DO_Mstr.DOM_Created_By,DATE_FORMAT(DO_Mstr.DOM_DO_Date, ""%d/%m/%Y %H:%i"") AS DOM_DO_Date, " &
                    " (SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=DO_Mstr.DOM_DO_Status AND STATUS_TYPE='DO') AS Status_Desc,DO_Mstr.DOM_DO_Status, " &
                    " DATE_FORMAT(PO_Mstr.POM_PO_Date, ""%d/%m/%Y %H:%i"") AS POM_PO_Date, PO_Mstr.POM_PO_No,  DO_Mstr.DOM_D_ADDR_CODE," &
                    " COMPANY_MSTR.CM_Coy_Name, " & 'DO_Details.DOD_SHIPPED_QTY AS DOD_SHIPPED_QTY_Ship, " &
                    "(SELECT DISTINCT" & '20210614: add ItemCode directly on DO listing
                    "    GROUP_CONCAT(DISTINCT POD_VENDOR_ITEM_CODE SEPARATOR ', ')" &
                    "FROM" &
                    "   DO_Mstr DOM," &
                    "   DO_Details DOD," &
                    "   PO_Details POD," &
                    "   PO_Mstr POM" &
                    " WHERE" &
                    "   DOM.DOM_DO_NO = DOD.DOD_DO_NO" &
                    "       AND DOM.DOM_S_Coy_ID = DOD.DOD_S_COY_ID" &
                    "       And DOM.DOM_PO_Index = POM.POM_PO_Index" &
                    "       AND POM.POM_PO_No = POD.POD_Po_No" &
                    "       And POM.POM_B_Coy_ID = POD.POD_Coy_ID" &
                    "       AND DOD.DOD_PO_Line = POD.POD_Po_Line" &
                    "       And DOM.DOM_S_Coy_ID = '" & companyID & "'" &
                    "       AND POM.POM_PO_NO = PO_Mstr.POM_PO_NO" &
                    "       And DOM.DOM_DO_NO = DO_Mstr.DOM_DO_NO" &
                    "       ) AS Item_Code" &
                    " FROM DO_Mstr, DO_Details, PO_Details, PO_Mstr, COMPANY_MSTR " &
                    " WHERE DO_Mstr.DOM_DO_NO = DO_Details.DOD_DO_NO and " &
                    " DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID and" &
                    " DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index and" &
                    " PO_Mstr.POM_PO_No = PO_Details.POD_Po_No and" &
                    " PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID and" &
                    " DO_Details.DOD_PO_Line = PO_Details.POD_Po_Line and" &
                    " COMPANY_MSTR.CM_Coy_ID = PO_Mstr.POM_B_COY_ID and" &
                    " DO_Mstr.DOM_S_Coy_ID = '" & companyID & "'"
            '(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR'

            If strDocType <> "0" Then
                If strDocType = "DO" Then
                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strsqlDO = strsqlDO & " And DO_Mstr.DOM_DO_NO" & Common.ParseSQL(strTemp)
                    End If
                    If strCreationDt <> "" Then
                        strsqlDO = strsqlDO & " And DO_Mstr.DOM_CREATED_DATE BETWEEN " & Common.ConvertDate(strCreationDt & " 00:00:00.000") &
                       " AND " & Common.ConvertDate(strCreationDt & " 23:59:59.000")
                    End If
                    If strSubmittedDt <> "" Then
                        strsqlDO = strsqlDO & " And DO_Mstr.DOM_DO_Date BETWEEN " & Common.ConvertDate(strSubmittedDt & " 00:00:00.000") &
                       " AND " & Common.ConvertDate(strSubmittedDt & " 23:59:59.000")
                    End If

                ElseIf strDocType = "PO" Then
                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strsqlDO = strsqlDO & " And PO_Mstr.POM_PO_No" & Common.ParseSQL(strTemp)
                    End If
                    If strCreationDt <> "" Then
                        'Common.ConvertDate(strdatetime & " 00:00:00.000") & " AND " & Common.ConvertDate(strdatetime & " 23:59:59.000") & ""
                        strsqlDO = strsqlDO & " And PO_Mstr.POM_PO_Date BETWEEN " & Common.ConvertDate(strCreationDt & " 00:00:00.000") &
                        " AND " & Common.ConvertDate(strCreationDt & " 23:59:59.000")
                    End If

                End If
            End If

            If strOurRef <> "" Then
                strTemp = Common.BuildWildCard(strOurRef)
                strsqlDO = strsqlDO & " And DO_Mstr.DOM_S_Ref_No" & Common.ParseSQL(strTemp)
            End If
            If strBuyerComp <> "" Then
                strTemp = Common.BuildWildCard(strBuyerComp)
                strsqlDO = strsqlDO & " And COMPANY_MSTR.CM_Coy_Name" & Common.ParseSQL(strTemp)
            End If
            If strVenItem <> "" Then
                strTemp = Common.BuildWildCard(strVenItem)
                strsqlDO = strsqlDO & " And PO_Details.POD_Vendor_Item_Code" & Common.ParseSQL(strTemp)
            End If
            If strStatus <> "" Then
                strsqlDO = strsqlDO & " And DO_Mstr.DOM_DO_Status in " & strStatus
            End If

            strsqlDO &= " order by DO_Mstr.DOM_DO_Index desc"

            'dsDO = objDb.FillDs(strsqlDO)
            Return strsqlDO

        End Function

        Public Function GetDO2(ByVal userID As String, ByVal companyID As String, ByVal strDONo As String, ByVal strPONo As String) As DataSet
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
                     "WHERE POM_PO_INDEX = DOM_PO_INDEX AND POM_B_COY_ID='" & companyID & "' AND POM_BUYER_ID='" & companyID & "' " &
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
            Return dsDO
            'Return strsql
        End Function
        'Michelle (11/1/2013) - Issue 1832
        Public Function ChkOustdGRN(ByVal strVendor As String, ByVal intDODays As Integer) As Boolean
            Dim dsDO As DataSet
            Dim strsql, strCtrlDay As String
            Dim objDb As New EAD.DBCom

            strsql = "SELECT cv_grn_ctrl_term FROM company_vendor WHERE cv_b_coy_id ='" & HttpContext.Current.Session("CompanyID") & "' AND cv_s_coy_id ='" & strVendor & "'"
            strCtrlDay = objDb.GetVal(strsql)
            If strCtrlDay <> "" Then
                If intDODays > Convert.ToInt32(strCtrlDay) Then Return True
            End If
            Return False
        End Function

        'Public Function GetPODDL(ByRef pDropDownList As UI.WebControls.DropDownList) As DataSet
        Public Sub GetOutStandingPO(ByRef pDropDownList As UI.WebControls.DropDownList)
            Dim strDefaultValue As String
            Dim SQLQuery As String
            Dim dsPO As DataSet
            'Dim drw As DataView

            '//Query modified by Moo
            '//POD_Received_Qty And POD_Rejected_Qty only updated when generating GRN
            '//check of (PO_Details.POD_Received_Qty - PO_Details. POD_Rejected_Qty) will cause PO still
            '//remail in dropdownlist although item is fully delivered(with or without GRN)
            '//check of (POD_Ordered_Qty > POD_DELIVERED_QTY) is more than enough
            '//POD_DELIVERED_QTY = all delivered qty include those without GRN
            '//support partially cancelled,so need to check POD_CANCELLED_QTY
            SQLQuery = " SELECT PM.POM_PO_No,PM.POM_PO_Index,POM_PO_Date " &
                        " From PO_MSTR PM where PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND " &
                        "PM.POM_PO_STATUS IN ('" & POStatus_new.Accepted & "') AND EXISTS (Select '*' From PO_Details where PM.POM_PO_No=POD_PO_NO and " _
                        & "PM.POM_B_Coy_ID=POD_Coy_ID GROUP BY POD_Coy_ID,POD_PO_NO HAVING SUM(POD_Ordered_Qty)- SUM(POD_CANCELLED_QTY) - SUM(POD_DELIVERED_QTY) > 0)" &
                        " Order by POM_PO_Date Desc, POM_PO_Index Desc "
            dsPO = objDb.FillDs(SQLQuery)

            '//take out fulfilment because GRN may got rejection, but wouldnt update fulfilment
            '//so may got case like fulfilment=fully delivered but still got outstanding item
            '" And PM.POM_FulFilment in ('" & Fulfilment.Open & "','" & Fulfilment.Part_Delivered & "') AND " _
            ' drw = objDB.GetView(SQLQuery)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            Common.FillDdl(pDropDownList, "POM_PO_No", "POM_PO_Index", dsPO)
            If Not dsPO Is Nothing Then
                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            End If
            'Return dsPO

        End Sub
        'Michelle (9/9/2010) - To get the PO details for the outstanding PO
        Public Function GetOutStandingPO() As DataSet
            Dim SQLQuery As String
            Dim dsPO As DataSet
            SQLQuery = " SELECT PM.POM_PO_No,PM.POM_PO_Index,POM_PO_Date, CM_COY_NAME, SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) as Outs, POM_B_COY_ID, SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) as Tot, DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) AS 'DUE_DATE' " &
                        " From PO_MSTR PM, PO_DETAILS, COMPANY_MSTR where POM_B_COY_ID = CM_COY_ID AND PM.POM_S_Coy_ID =  '" & HttpContext.Current.Session("CompanyID") & "' AND " &
                        " POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO AND " &
                        "PM.POM_PO_STATUS IN ('" & POStatus_new.Accepted & "') AND EXISTS (Select '*' From PO_Details where PM.POM_PO_No=POD_PO_NO and " _
                        & "PM.POM_B_Coy_ID=POD_Coy_ID GROUP BY POD_Coy_ID,POD_PO_NO HAVING SUM(POD_Ordered_Qty)- SUM(POD_CANCELLED_QTY) - SUM(POD_DELIVERED_QTY) > 0)" &
                        " GROUP BY PM.POM_PO_No,PM.POM_PO_Index,POM_PO_Date, CM_COY_NAME, POM_B_COY_ID" &
                        " Order by 'DUE_DATE' ASC, POM_PO_Index ASC "
            dsPO = objDb.FillDs(SQLQuery)

            Return dsPO

        End Function

        Public Function GetOutStandingPOWithDAddress(ByVal userID As String, ByVal strPONo As String) As String
            Dim SQLQuery As String
            Dim dsPO As DataSet
            Dim companyID = objUsr.getCompanyID(userID)

            SQLQuery = " SELECT POD_D_ADDR_CODE,PM.POM_PO_No,PM.POM_PO_Index,POM_PO_Date, CM_COY_NAME, SUM(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) as Outs, POM_B_COY_ID, SUM(POD_ORDERED_QTY - POD_CANCELLED_QTY) as Tot, DATE_ADD(PM.POM_PO_DATE, INTERVAL POD_ETD DAY) AS 'DUE_DATE' " &
                        " From PO_MSTR PM, PO_DETAILS, COMPANY_MSTR where POM_B_COY_ID = CM_COY_ID AND PM.POM_S_Coy_ID =  '" & companyID & "' AND " &
                        " POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO AND " &
                        "PM.POM_PO_STATUS IN ('" & POStatus_new.Accepted & "') AND EXISTS (Select '*' From PO_Details where PM.POM_PO_No=POD_PO_NO and " & If(Not String.IsNullOrEmpty(strPONo), " POD_PO_NO = '" & strPONo & "' AND ", String.Empty) &
                        "PM.POM_B_Coy_ID=POD_Coy_ID GROUP BY POD_Coy_ID,POD_PO_NO HAVING (POD_Ordered_Qty - POD_CANCELLED_QTY - POD_DELIVERED_QTY) > 0)" &
                        " GROUP BY PM.POM_PO_No,PM.POM_PO_Index,POM_PO_Date, CM_COY_NAME, POM_B_COY_ID, POD_D_ADDR_CODE" &
                        " Order by 'DUE_DATE' DESC, POM_PO_Index DESC "

            'dsPO = objDb.FillDs(SQLQuery)

            Return SQLQuery

        End Function

        Public Function filterDevlAdd(ByRef pDropDownList As UI.WebControls.DropDownList, ByVal PONo As String, ByVal POIndex As Integer, Optional ByVal DA As String = "") As DataSet
            Dim SQLQuery As String
            Dim strDefaultValue As String
            Dim dsDevlAdd As DataSet
            SQLQuery = " SELECT Distinct PO_Mstr.POM_B_Coy_ID,PO_Details.POD_D_Addr_Code,PO_Details.POD_D_Addr_Line1,PO_Details.POD_D_Addr_Line2," &
                    " PO_Details.POD_Coy_ID,PO_Details.POD_D_Addr_Line3,(SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Details.POD_D_State AND CODE_CATEGORY='S' AND CODE_VALUE=POD_D_Country)as POD_D_State_desc,POD_D_State," &
                    " (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Details.POD_D_Country AND CODE_CATEGORY='CT') as POD_D_Country_desc,POD_D_Country, " &
                    " PO_Details.POD_D_PostCode,PO_Details.POD_D_City" &
                    " FROM PO_Details ,PO_Mstr" &
                    " WHERE po_Mstr.POM_PO_No = '" & PONo & "'" &
                    " AND PO_Mstr.POM_PO_Index = " & POIndex &
                    " AND PO_Mstr.POM_S_Coy_ID = '" & HttpContext.Current.Session("CompanyID") & "'" &
                    " AND PO_Details.POD_Coy_ID = PO_Mstr.POM_B_Coy_ID" &
                    " AND PO_Details.POD_Po_No = po_Mstr.POM_PO_No AND (POD_Ordered_Qty-POD_CANCELLED_QTY > POD_DELIVERED_QTY)" '& _

            If DA <> "" Then
                SQLQuery &= " AND PO_Details.POD_D_ADDR_CODE = '" & DA & "' "
            End If
            '" AND NOT EXISTS " & _
            '"(SELECT '*' FROM DO_MSTR WHERE DO_MSTR.DOM_PO_INDEX=PO_Mstr.POM_PO_INDEX AND DOM_D_ADDR_CODE=POD_D_Addr_Code AND DOM_DO_STATUS=" & DOStatus.Draft & ")"


            dsDevlAdd = objDb.FillDs(SQLQuery)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem

            Common.FillDdl(pDropDownList, "POD_D_Addr_Code", "POD_D_Addr_Code", dsDevlAdd)
            If dsDevlAdd.Tables(0).Rows.Count > 1 Then
                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            End If
            Return dsDevlAdd
        End Function
        'Display for new DO
        Public Function GetPODetails(ByVal userID As String, ByVal PONo As String, ByVal POIndex As Integer, ByVal AddrCode As String, ByVal strBCoyID As String, ByVal strType As String) As Result
            Dim SQLQuery, sql, sql2 As String
            Dim dsPO As DataSet
            Dim result As Result = New Result
            Dim companyID As String = objUsr.getCompanyID(userID)

            Select Case strType
                Case "POHeader"
                    sql = "SELECT POM_B_COY_ID,POM_PO_Index,POM_Billing_Method,POM_PO_Date,POM_CREATED_DATE, POM_PAYMENT_TERM,POM_PAYMENT_METHOD, POM_SHIPMENT_MODE, POM_SHIPMENT_TERM,POM_BUYER_NAME,POM_DEL_CODE " &
                    " From PO_Mstr" &
                    " WHERE PO_Mstr.POM_PO_Index = '" & POIndex & "'" &
                    " And PO_Mstr.POM_PO_No = '" & PONo & "'" &
                    " And PO_Mstr.POM_S_Coy_ID  = '" & companyID & "'"

                    SQLQuery = sql & ";"
                    dsPO = objDb.FillDs(SQLQuery)
                    dsPO.Tables(0).TableName = "PO_MSTR"
                    'dsPO.Tables(1).TableName = "PO_DETAILS"
                    result.strSql = SQLQuery
                    result.ds = dsPO

                    GetPODetails = result
                Case "PODetails"
                    sql2 = " Select POD_ORDERED_QTY AS POD_Outstanding,(SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = POD_D_State AND CODE_CATEGORY='S' AND CODE_VALUE='MY')as POD_D_State_desc," &
                            " (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = POD_D_Country AND CODE_CATEGORY='CT') as POD_D_Country_desc, " &
                    " POD_D_Addr_Line1, POD_D_Addr_Line2, POD_D_Addr_Line3, POD_D_PostCode, POD_D_City, POD_D_State, POD_D_Country, POD_COY_ID , POD_PO_Line," &
                    " POD_ETD, POD_ORDERED_QTY, POD_DELIVERED_QTY, POD_CANCELLED_QTY, POD_Vendor_Item_Code," &
                    "POD_Product_Desc, POD_UOM, POD_Warranty_Terms, POD_Min_Pack_Qty, POD_Min_Order_Qty, POD_ITEM_TYPE, POD_PRODUCT_CODE, POD_ORDERED_QTY AS DOD_SHIPPED_QTY_Ship " &
                    " from PO_DETAILS" &
                    " where POD_Po_No = '" & PONo & "'" &
                    " and POD_Coy_ID =  '" & strBCoyID & "'" &
                    " and POD_D_ADDR_CODE = '" & Common.Parse(AddrCode) & "'"

                    result.strSql = sql2
                    GetPODetails = result
            End Select

        End Function
        'Display for edit DO
        Public Function ShowDOdetails(ByVal userID As String, ByVal strDONo As String, ByVal PONo As String, ByVal IntPOIdx As Integer, ByVal strLocID As String, ByVal strBCoyID As String, ByVal strType As String) As Result
            Dim SQLQuery, sql, sql2, sql3 As String
            Dim dsDODtl As DataSet
            Dim result As Result = New Result
            Dim companyID As String = objUsr.getCompanyID(userID)
            Select Case strType
                Case "POHeader"
                    sql = "SELECT PO_Mstr.POM_PO_Index,PO_Mstr.POM_Billing_Method,PO_Mstr.POM_PO_Date,PO_Mstr.POM_CREATED_DATE,PO_Mstr.POM_PAYMENT_TERM,PO_Mstr.POM_PAYMENT_METHOD, PO_Mstr.POM_SHIPMENT_MODE, PO_Mstr.POM_SHIPMENT_TERM,PO_Mstr.POM_BUYER_NAME,PO_Mstr.POM_DEL_CODE,PO_Mstr.POM_B_COY_ID," &
                    " Do_Mstr.DOM_Waybill_No,Do_Mstr.DOM_Freight_Carrier,Do_Mstr.DOM_DO_Remarks,Do_Mstr.DOM_S_Ref_No,Do_Mstr.DOM_S_Ref_Date,Do_Mstr.DOM_Freight_Amt" &
                    " From PO_Mstr INNER JOIN Do_Mstr ON POM_PO_Index = " & IntPOIdx &
                    " and DO_mstr.DOM_PO_Index = PO_mstr.POM_PO_Index AND DOM_DO_NO='" & strDONo & "' AND DOM_S_COY_ID='" & companyID & "'"
                    SQLQuery = sql & ";"
                    dsDODtl = objDb.FillDs(SQLQuery)
                    dsDODtl.Tables(0).TableName = "PO_MSTR"
                    'dsDODtl.Tables(1).TableName = "PO_DETAILS"
                    'dsDODtl.Tables(2).TableName = "DO_DETAILS"
                    result.strSql = SQLQuery
                    result.ds = dsDODtl
                    ShowDOdetails = result
                Case "PODetails"
                    sql2 = "Select POD_ORDERED_QTY AS POD_Outstanding,(SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = POD_D_State AND CODE_CATEGORY='S' AND CODE_VALUE='MY')as POD_D_State_Desc," &
                    " (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = POD_D_Country AND CODE_CATEGORY='CT') as POD_D_Country_Desc, " &
                    " POD_D_Addr_Line1, POD_D_Addr_Line2, POD_D_Addr_Line3, POD_D_PostCode, POD_D_City, POD_D_State, POD_D_Country, POD_COY_ID, POD_Po_Line, " &
                    " POD_ETD, POD_ORDERED_QTY, POD_DELIVERED_QTY, POD_CANCELLED_QTY, POD_Vendor_Item_Code," &
                    "POD_Product_Desc, POD_UOM, POD_Warranty_Terms, POD_Min_Pack_Qty, POD_Min_Order_Qty, POD_Item_Type, POD_Product_Code" &
                    " from PO_details" &
                    " where POD_PO_NO = '" & PONo & "'" &
                    " and POD_Coy_ID =  '" & strBCoyID & "'" &
                    " and POD_D_ADDR_CODE = '" & Common.Parse(strLocID) & "'"

                    result.strSql = sql2
                    ShowDOdetails = result
                Case "DODetails"
                    sql3 = "Select * from DO_details where DOD_DO_NO = '" & strDONo & "' AND DOD_S_COY_ID='" & companyID & "'"
                    result.strSql = sql3
                    ShowDOdetails = result
            End Select
        End Function
        Function ISDraftDOSubmitted(ByVal companyID As String, ByVal strDoNo As String) As Boolean
            Dim SQLQuery, SQLSelect As String
            Dim blnStatus As Boolean = False
            SQLQuery = "SELECT DOM_DO_STATUS FROM DO_MSTR WHERE DOM_DO_NO='" & strDoNo & "' AND DOM_S_COY_ID='" & companyID & "'"
            Dim tDS As DataSet = objDb.FillDs(SQLQuery)
            If tDS.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(tDS.Tables(0).Rows(0).Item("DOM_DO_STATUS")) Then
                    If tDS.Tables(0).Rows(0).Item("DOM_DO_STATUS") = DOStatus.Draft Then
                        blnStatus = False
                    Else
                        blnStatus = True
                    End If
                End If
            End If

            Return blnStatus
        End Function
        Public Function DOEdit(ByVal userID As String, ByVal ds As DataSet, ByVal btn As String, ByVal strDoNo As String, ByVal strBCoyID As String, ByRef strMsg As String, Optional ByVal LotBln As Boolean = True, Optional ByVal aryLotNo As ArrayList = Nothing) As Boolean
            Dim SQLQuery, SQLSelect As String
            Dim strAryQuery(0) As String
            Dim dtMstr, dtDtls, dtQutS As DataTable
            Dim dr, drDtl As DataRow
            Dim dsShip As DataSet
            Dim intShipped, intShipQty As Integer
            Dim strStatus, strPONo As String
            Dim companyID As String = objUsr.getCompanyID(userID)
            If btn = "Save" Then
                strStatus = DOStatus.Draft
            Else
                strStatus = DOStatus.Submitted
            End If
            'Update DO Master
            dtMstr = ds.Tables(0) 'fr dtDOMstr
            dr = dtMstr.Rows(0)
            dtDtls = ds.Tables(1) ' fr dtDODtls
            If (btn = "Submit") Then
                If ISDraftDOSubmitted(companyID, strDoNo) Then
                    strMsg = "99"
                    Return False
                End If

                Dim strPOLine As String
                Dim intItemCnt As Integer
                intItemCnt = dtDtls.Rows.Count
                For Each drDtl In dtDtls.Rows
                    If strPOLine = "" Then
                        strPOLine = drDtl("DOD_PO_LINE")
                    Else
                        strPOLine += "," & drDtl("DOD_PO_LINE")
                    End If
                Next
                dtQutS = GetPOLineOutstanding(dr("POD_B_COY_ID"), dr("POD_PO_NO"), strPOLine)
                Dim drOutS As DataRow()
                For Each drDtl In dtDtls.Rows
                    drOutS = dtQutS.Select("POD_PO_LINE=" & drDtl("DOD_PO_LINE"))
                    If drOutS.Length > 0 Then
                        If drOutS(0)("Outs") < drDtl("DOD_SHIPPED_QTY") Then
                            Return False
                        End If
                    End If
                Next
            End If

            SQLQuery = "Update DO_Mstr set " &
                    " DOM_S_REF_NO='" & Common.Parse(dr("DOM_S_Ref_No")) & "' " &
                    ", DOM_WAYBILL_NO ='" & Common.Parse(dr("DOM_WAYBILL_NO")) & "'" &
                    ", DOM_FREIGHT_AMT = " & IIf(dr("DOM_FREIGHT_AMT") = "", "NULL", dr("DOM_FREIGHT_AMT")) &
                    ", DOM_S_REF_DATE =" & ParseDate(dr("DOM_S_REF_DATE")) &
                    ", DOM_FREIGHT_CARRIER  = '" & Common.Parse(dr("DOM_FREIGHT_CARRIER")) & "'" &
                    ", DOM_DO_REMARKS = '" & Common.Parse(dr("DOM_DO_REMARKS")) & "'" &
                    ", DOM_DO_Date = Now()"   'Michelle (11/1/2013) to capture the time '& ParseDate(dr("DOM_DO_DATE")) & _
            SQLQuery &= ", DOM_DO_STATUS = " & Common.Parse(strStatus) &
                    " Where DOM_DO_NO = '" & strDoNo & "'" &
                    " and DOM_S_COY_ID = '" & companyID & "'"
            ' Shipped Quantity Should not be greater than the Outstanding Quantity
            'objDb.Execute(SQLQuery)
            Common.Insert2Ary(strAryQuery, SQLQuery)

            SQLQuery = "Delete from DO_Details WHERE DO_Details.DOD_DO_NO = '" & strDoNo & "'" &
                       " And DO_Details.DOD_S_COY_ID = '" & companyID & "'"
            'objDb.Execute(SQLQuery)
            Common.Insert2Ary(strAryQuery, SQLQuery)


            For Each drDtl In dtDtls.Rows
                If btn = "Submit" Then
                    SQLQuery = " UPDATE PO_Details Set POD_Delivered_Qty = POD_Delivered_Qty + " &
                       IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) &
                       " WHERE PO_Details.POD_PO_NO = '" & dr("POD_PO_NO") & "' AND POD_COY_ID='" & dr("POD_B_COY_ID") &
                       "' And PO_Details.POD_PO_LINE = " & drDtl("DOD_PO_LINE")
                    Common.Insert2Ary(strAryQuery, SQLQuery)

                    SQLQuery = SetPOFulFilment(dr("POD_PO_NO"), dr("POD_B_COY_ID"))
                    Common.Insert2Ary(strAryQuery, SQLQuery)
                End If

                strPONo = dr("POD_PO_NO")

                SQLQuery = "INSERT INTO DO_Details (DOD_S_COY_ID,DOD_DO_NO,DOD_DO_LINE,DOD_PO_LINE,DOD_DO_QTY,"

                If LotBln = True Then
                    SQLQuery &= "DOD_DO_LOT_NO,"
                End If

                SQLQuery &= "DOD_SHIPPED_QTY,DOD_REMARKS)" &
                          " Values ('" & companyID &
                          "','" & Common.Parse(strDoNo) & "'" &
                          "," & drDtl("DOD_DO_LINE") &
                          "," & drDtl("DOD_PO_LINE") &
                          "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY"))

                If LotBln = True Then
                    SQLQuery &= "," & drDtl("DOD_LotNo")
                End If

                SQLQuery &= "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) &
                          ",'" & Common.Parse(drDtl("DOD_REMARKS")) & "')"
                Common.Insert2Ary(strAryQuery, SQLQuery)
                'SQLQuery = " UPDATE DO_Details Set POD_Delivered_Qty =" & intShipped & "" & _
                '" ,DOD_Remarks = '" & drDtl("DOD_REMARKS") & "'" & _
                '        " WHERE DO_Details.DOD_DO_NO = '" & dr("DOD_DO_NO") & "'" & _
                '        " And DO_Details.DOD_DO_Line = " & drDtl("DOD_DO_LINE") & ""
            Next

            'Michelle (21/1/2013) - Issue 1727
            'Delete those attachments that are marked for deletion
            SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT WHERE CDDA_COY_ID ='" & companyID & "' "
            SQLQuery &= "AND CDDA_DOC_NO ='" & Common.Parse(strDoNo) & "' AND CDDA_ATTACH_INDEX IN "
            SQLQuery &= "(SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID ='"
            SQLQuery &= companyID & "' AND CDDA_DOC_NO ='" & Common.Parse(strDoNo) & "' AND CDDA_TYPE = 'H' AND CDDA_STATUS = 'U') "
            Common.Insert2Ary(strAryQuery, SQLQuery)

            'Insert those new attachments 
            SQLQuery = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT"
            SQLQuery &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE) "
            SQLQuery &= "SELECT CDDA_COY_ID, '" & strDoNo & "', CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE "
            SQLQuery &= "FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
            SQLQuery &= "AND CDDA_DOC_NO = '" & strDoNo & "' AND CDDA_TYPE = 'H' AND CDDA_STATUS = 'N' "

            SQLQuery &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
            SQLQuery &= "WHERE CDDA_TYPE = 'H' AND CDDA_STATUS = 'U' )"
            Common.Insert2Ary(strAryQuery, SQLQuery)

            SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
            SQLQuery &= "AND (CDDA_DOC_NO = '" & strDoNo & "' OR CDDA_DOC_NO = '" & strDoNo & "') AND CDDA_TYPE = 'H'"
            Common.Insert2Ary(strAryQuery, SQLQuery)

            If Not aryLotNo Is Nothing Then
                Dim i As Integer
                Dim strIndex As String

                For i = 0 To aryLotNo.Count - 1
                    SQLQuery = "SELECT DOL_LOT_INDEX FROM DO_LOT WHERE DOL_COY_ID='" & companyID & "' " &
                            "AND DOL_ITEM_CODE = '" & Common.Parse(aryLotNo(i)(6)) & "' AND DOL_DO_NO = '" & strDoNo & "' AND DOL_ITEM_LINE = " & aryLotNo(i)(7) & " " &
                            "AND DOL_PO_LINE = '" & aryLotNo(i)(10) & "' "
                    strIndex = objDb.GetVal(SQLQuery)

                    If aryLotNo(i)(0) <> "" And aryLotNo(i)(1) <> "" And aryLotNo(i)(2) <> "" And aryLotNo(i)(3) <> "" Then
                        If strIndex <> "" Then
                            SQLQuery = "UPDATE DO_LOT SET DOL_LOT_QTY=" & aryLotNo(i)(0) & ", DOL_LOT_NO='" & aryLotNo(i)(1) & "' " &
                                    ",DOL_DO_MANU_DT='" & Format(CDate(aryLotNo(i)(2)), "yyyy-MM-dd") & "' ,DOL_DO_EXP_DT='" & Format(CDate(aryLotNo(i)(3)), "yyyy-MM-dd") &
                                    "',DOL_IQC_MANU_DT='" & Format(CDate(aryLotNo(i)(2)), "yyyy-MM-dd") & "' ,DOL_IQC_EXP_DT='" & Format(CDate(aryLotNo(i)(3)), "yyyy-MM-dd") &
                                    "',DOL_DO_MANUFACTURER='" & Common.Parse(aryLotNo(i)(4)) & "' " &
                                    "WHERE DOL_LOT_INDEX=" & strIndex & ""
                        Else
                            SQLQuery = "INSERT INTO DO_LOT (DOL_COY_ID, DOL_DO_NO, DOL_ITEM_CODE, DOL_LOT_QTY, DOL_LOT_NO, " &
                                     "DOL_DO_MANU_DT, DOL_DO_EXP_DT, DOL_IQC_MANU_DT, DOL_IQC_EXP_DT, DOL_DO_MANUFACTURER, DOL_ITEM_LINE, DOL_PO_LINE) Values (" &
                                     "'" & Common.Parse(companyID) &
                                     "','" & strDoNo &
                                     "','" & Common.Parse(aryLotNo(i)(6)) &
                                     "'," & aryLotNo(i)(0) &
                                     ",'" & Common.Parse(aryLotNo(i)(1)) &
                                     "','" & Format(CDate(aryLotNo(i)(2)), "yyyy-MM-dd") &
                                     "','" & Format(CDate(aryLotNo(i)(3)), "yyyy-MM-dd") &
                                     "','" & Format(CDate(aryLotNo(i)(2)), "yyyy-MM-dd") &
                                     "','" & Format(CDate(aryLotNo(i)(3)), "yyyy-MM-dd") &
                                     "','" & Common.Parse(aryLotNo(i)(4)) &
                                     "'," & aryLotNo(i)(7) &
                                     ",'" & aryLotNo(i)(10) & "')"
                        End If
                        Common.Insert2Ary(strAryQuery, SQLQuery)
                    Else
                        If strIndex <> "" Then
                            SQLQuery = "DELETE FROM DO_LOT WHERE DOL_LOT_INDEX =" & strIndex
                            Common.Insert2Ary(strAryQuery, SQLQuery)
                        End If

                    End If

                Next

                Dim strTemp As String = "(SELECT DOL_LOT_INDEX FROM DO_LOT WHERE DOL_COY_ID=CDDA_COY_ID AND DOL_ITEM_CODE=CDDA_ITEM_CODE AND DOL_PO_LINE=CDDA_PO_LINE AND DOL_ITEM_LINE=CDDA_LINE_NO AND DOL_DO_NO='" & strDoNo & "') as CDDA_LOT_INDEX"

                'CHee Hong (19/3/2013) - SEH eProcurement Enhancement 
                'Delete those attachments that are marked for deletion
                SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT WHERE CDDA_COY_ID ='" & companyID & "' "
                SQLQuery &= "AND CDDA_DOC_NO ='" & Common.Parse(strDoNo) & "' AND CDDA_ATTACH_INDEX IN "
                SQLQuery &= "(SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID ='"
                SQLQuery &= companyID & "' AND CDDA_DOC_NO ='" & Common.Parse(strDoNo) & "' AND CDDA_TYPE = 'D' AND CDDA_STATUS = 'U') "
                Common.Insert2Ary(strAryQuery, SQLQuery)

                'Insert those new attachments 
                SQLQuery = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT"
                SQLQuery &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ITEM_CODE,CDDA_PO_LINE,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_LOT_INDEX,CDDA_LINE_NO,CDDA_TYPE) "
                SQLQuery &= "SELECT CDDA_COY_ID, '" & strDoNo & "', CDDA_HUB_FILENAME,CDDA_ITEM_CODE,CDDA_PO_LINE,CDDA_ATTACH_FILENAME,CDDA_FILESIZE," & strTemp & ",CDDA_LINE_NO,CDDA_TYPE "
                SQLQuery &= "FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
                SQLQuery &= "AND CDDA_DOC_NO = '" & strDoNo & "' AND CDDA_TYPE = 'D' AND CDDA_STATUS = 'N' "
                'Exclude those new attachment where user later delete it
                SQLQuery &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                SQLQuery &= "WHERE CDDA_TYPE = 'D' AND CDDA_STATUS = 'U' )"
                Common.Insert2Ary(strAryQuery, SQLQuery)
                'Clear the temp table
                'SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
                'SQLQuery &= "AND (CDDA_DOC_NO = '" & strDoNo & "' OR CDDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "') AND CDDA_TYPE = 'D'"
                Common.Insert2Ary(strAryQuery, SQLQuery)

            End If




            If btn <> "Save" Then
                Dim objTrack As New Tracking
                Dim objUsers As New Users
                SQLQuery = objTrack.updateDocMatching(dr("POD_PO_NO"), Common.Parse(strDoNo), dr("POD_PO_NO"), "DO", companyID, dr("POD_B_COY_ID"))
                Common.Insert2Ary(strAryQuery, SQLQuery)
                objUsers.Log_UserActivity(userID, companyID, strAryQuery, WheelModule.Fulfillment, WheelUserActivity.V_SubmitDO, Common.Parse(strDoNo), strPONo)
                objTrack = Nothing
                objUsers = Nothing
            End If

            If objDb.BatchExecute(strAryQuery) Then
                If btn <> "Save" Then
                    Dim objMail As New Email
                    'objMail.sendNotification(EmailType.DOCreated, HttpContext.Current.Session("UserID"), strBCoyID, companyID, strDoNo, strPONo)
                    'Michelle (23/1/2010) - To remove sending email to storekeeper
                    'objMail.sendNotification(EmailType.DOCreatedToSK, HttpContext.Current.Session("UserID"), strBCoyID, companyID, strDoNo, strPONo)
                    objMail = Nothing
                End If
                Return True
            Else
                Return False
            End If
        End Function

        'Public Function DONew(ByVal ds As DataSet, ByVal btn As String, ByRef strDONo As String, ByRef strMsg As String) As Boolean
        '    Dim blnGetNewNo As Boolean = True
        '    Dim SQLQuery, SQLSelect As String
        '    Dim dr, drDtl As DataRow
        '    'Dim strDONo As String
        '    Dim strDOPrefix, strNewNo As String
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim strStatus As String
        '    Dim dtMstr, dtDtls, dtQutS As DataTable
        '    Dim intShipped, intShipQty, intOutStnd As Integer
        '    Dim dsShip As DataSet

        '    Dim strPONo, strBCoyID As String

        '    Do While blnGetNewNo
        '        If (btn = "Save" Or btn = "Submit") Then
        '            If btn = "Save" Then
        '                strStatus = DOStatus.Draft
        '            Else
        '                strStatus = DOStatus.Submitted
        '            End If
        '            'GetLatestDocNo
        '            objGlobal.GetLatestDocNo("DO", strAryQuery, strDONo, strDOPrefix, , , strNewNo)
        '            SQLQuery = "SELECT '*' FROM DO_MSTR WHERE DOM_DO_NO='" & strDONo & "' AND DOM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        '            If objDb.Exist(SQLQuery) Then
        '                strMsg = "dup"
        '                Return False
        '            End If
        '            'Insert into DO Master 
        '            dtMstr = ds.Tables(0) 'fr dtDOMstr
        '            dr = dtMstr.Rows(0)

        '            dtDtls = ds.Tables(1) ' fr dtDODtls

        '            '//Get PO Line outstanding qty from Database and compare with user entered shipped
        '            '//if any of outstanding < shipped, disallow DO creation
        '            If (btn = "Submit") Then
        '                Dim strPOLine As String
        '                Dim intItemCnt As Integer
        '                intItemCnt = dtDtls.Rows.Count
        '                For Each drDtl In dtDtls.Rows
        '                    If strPOLine = "" Then
        '                        strPOLine = drDtl("DOD_PO_LINE")
        '                    Else
        '                        strPOLine += "," & drDtl("DOD_PO_LINE")
        '                    End If
        '                Next
        '                dtQutS = GetPOLineOutstanding(dr("POD_B_COY_ID"), dr("POD_PO_NO"), strPOLine)
        '                Dim drOutS As DataRow()
        '                For Each drDtl In dtDtls.Rows
        '                    drOutS = dtQutS.Select("POD_PO_LINE=" & drDtl("DOD_PO_LINE"))
        '                    If drOutS.Length > 0 Then
        '                        If drOutS(0)("Outs") < drDtl("DOD_SHIPPED_QTY") Then
        '                            strMsg = "outs" 'out < ship
        '                            Return False
        '                        End If
        '                    End If
        '                Next
        '            End If
        '            '//Get PO Line outstanding qty from Database and compare with user entered shipped
        '            '//if any of outstanding < shipped, disallow DO creation

        '            SQLQuery = "INSERT INTO DO_Mstr (DOM_S_COY_ID,DOM_DO_NO,DOM_DO_Date,DOM_S_Ref_No,DOM_S_REF_DATE,DOM_PO_INDEX," & _
        '            " DOM_D_Addr_Code,DOM_WAYBILL_NO,DOM_FREIGHT_CARRIER,DOM_FREIGHT_AMT,DOM_DO_REMARKS,DOM_DO_STATUS,DOM_CREATED_BY, " & _
        '            " DOM_CREATED_DATE,DOM_NOOFCOPY_PRINTED,DOM_DO_PREFIX,DOM_D_ADDR_LINE1,DOM_D_ADDR_LINE2, " & _
        '            " DOM_D_ADDR_LINE3,DOM_D_POSTCODE,DOM_D_CITY,DOM_D_STATE,DOM_D_COUNTRY)" & _
        '            " Values ('" & HttpContext.Current.Session("CompanyId") & "','" & strDONo & _
        '            "'," & Common.ConvertDate(Now()) & _
        '            ",'" & Common.Parse(dr("DOM_S_Ref_No")) & "'" & _
        '            "," & ParseDate(dr("DOM_S_REF_DATE")) & _
        '            "," & Common.Parse(dr("DOM_PO_INDEX")) & _
        '            ",'" & Common.Parse(dr("DOM_D_Addr_Code")) & _
        '            "','" & Common.Parse(dr("DOM_WAYBILL_NO")) & "" & _
        '            "','" & Common.Parse(dr("DOM_FREIGHT_CARRIER")) & _
        '            "'," & IIf(dr("DOM_FREIGHT_AMT") = "", "NULL", dr("DOM_FREIGHT_AMT")) & _
        '            ",'" & Common.Parse(dr("DOM_DO_REMARKS")) & _
        '            "','" & Common.Parse(strStatus) & _
        '            "','" & HttpContext.Current.Session("UserId") & _
        '            "'," & ParseDate(Now()) & _
        '            ", 0 ,'" & strDOPrefix & _
        '            "','" & Common.Parse(dr("POD_D_Addr_Line1")) & _
        '            "','" & Common.Parse(dr("POD_D_Addr_Line2")) & _
        '            "','" & Common.Parse(dr("POD_D_Addr_Line3")) & _
        '            "','" & Common.Parse(dr("POD_D_PostCode")) & _
        '            "','" & Common.Parse(dr("POD_D_City")) & _
        '            "','" & Common.Parse(dr("POD_D_State")) & _
        '            "','" & Common.Parse(dr("POD_D_Country")) & "')"
        '            Common.Insert2Ary(strAryQuery, SQLQuery)
        '            'Insert into DO Details

        '            For Each drDtl In dtDtls.Rows
        '                intShipQty = CInt(IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")))

        '                SQLQuery = "INSERT INTO DO_Details (DOD_S_COY_ID,DOD_DO_NO,DOD_DO_LINE,DOD_PO_LINE,DOD_DO_QTY,DOD_SHIPPED_QTY,DOD_REMARKS)" & _
        '                        " Values ('" & HttpContext.Current.Session("CompanyID") & _
        '                        "','" & Common.Parse(strDONo) & "'" & _
        '                        "," & Common.Parse(drDtl("DOD_DO_LINE")) & _
        '                        "," & Common.Parse(drDtl("DOD_PO_LINE")) & _
        '                        "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) & _
        '                        "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) & _
        '                        ",'" & Common.Parse(drDtl("DOD_REMARKS")) & "')"
        '                Common.Insert2Ary(strAryQuery, SQLQuery)

        '                If (btn = "Submit") Then
        '                    SQLQuery = " UPDATE PO_Details Set POD_Delivered_Qty = POD_Delivered_Qty + " & _
        '                    IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) & _
        '                    " WHERE PO_Details.POD_PO_NO = '" & dr("POD_PO_NO") & "' AND POD_COY_ID='" & dr("POD_B_COY_ID") & _
        '                    "' And PO_Details.POD_PO_LINE = " & drDtl("DOD_PO_LINE")
        '                    Common.Insert2Ary(strAryQuery, SQLQuery)

        '                    SQLQuery = SetPOFulFilment(dr("POD_PO_NO"), dr("POD_B_COY_ID"))
        '                    Common.Insert2Ary(strAryQuery, SQLQuery)

        '                    strPONo = dr("POD_PO_NO")
        '                    strBCoyID = dr("POD_B_COY_ID")
        '                End If
        '            Next
        '        End If

        '        Dim objUsers As New Users
        '        If (btn = "Submit") Then
        '            Dim objTrack As New Tracking
        '            strsql = objTrack.updateDocMatching(strPONo, Common.Parse(strDONo), strPONo, "DO", HttpContext.Current.Session("CompanyID"), strBCoyID)
        '            Common.Insert2Ary(strAryQuery, strsql)
        '            objUsers.Log_UserActivity(strAryQuery, WheelModule.Fulfillment, WheelUserActivity.V_SubmitDO, Common.Parse(strDONo), dr("POD_PO_NO"))
        '            objTrack = Nothing
        '        Else
        '            objUsers.Log_UserActivity(strAryQuery, WheelModule.Fulfillment, WheelUserActivity.V_SaveDO, Common.Parse(strDONo), dr("POD_PO_NO"))
        '        End If
        '        objUsers = Nothing


        '        'Michelle (2/11/2011) - check whether the new document no. has been taken
        '        Dim strLastNo As String
        '        strLastNo = objDb.GetVal("SELECT CP_PARAM_VALUE FROM company_param WHERE cp_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND cp_param_name = 'Last Used No' AND cp_param_type = 'DO'")
        '        If Convert.ToInt64(strNewNo) <= Convert.ToInt64(strLastNo) Then
        '            strNewNo = ""
        '            blnGetNewNo = True
        '        Else
        '            blnGetNewNo = False
        '        End If
        '    Loop

        '    If objDb.BatchExecute(strAryQuery) Then
        '        If btn <> "Save" Then
        '            Dim objMail As New Email
        '            objMail.sendNotification(EmailType.DOCreated, HttpContext.Current.Session("UserID"), strBCoyID, HttpContext.Current.Session("CompanyID"), strDONo, strPONo)
        '            'Michelle (23/1/2010) - To remove sending email to storekeeper
        '            objMail.sendNotification(EmailType.DOCreatedToSK, HttpContext.Current.Session("UserID"), strBCoyID, HttpContext.Current.Session("CompanyID"), strDONo, strPONo)
        '            objMail = Nothing
        '        End If
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        Public Function DONew(ByVal userID As String, ByVal ds As DataSet, ByVal btn As String, ByRef strDONo As String, ByRef strMsg As String, Optional ByVal LotBln As Boolean = True, Optional ByVal aryLotNo As ArrayList = Nothing) As Boolean
            Dim SQLQuery, SQLSelect As String
            Dim dr, drDtl As DataRow
            'Dim strDONo As String
            Dim strDOPrefix As String
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim strStatus As String
            Dim dtMstr, dtDtls, dtQutS As DataTable
            Dim intShipped, intShipQty, intOutStnd As Integer
            Dim dsShip As DataSet
            Dim intIncrementNo As Integer = 0
            Dim strNewDONo As String = ""

            Try

                Dim companyID As String = objUsr.getCompanyID(userID)

                Dim strPONo, strBCoyID As String

                If (btn = "Save" Or btn = "Submit") Then
                    If btn = "Save" Then
                        strStatus = DOStatus.Draft
                    Else
                        strStatus = DOStatus.Submitted
                    End If
                    'GetLatestDocNo
                    'objGlobal.GetLatestDocNo("DO", strAryQuery, strDONo, strDOPrefix)
                    'SQLQuery = "SELECT '*' FROM DO_MSTR WHERE DOM_DO_NO='" & strDONo & "' AND DOM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
                    'If objDb.Exist(SQLQuery) Then
                    '    strMsg = "dup"
                    '    Return False
                    'End If
                    strsql = " SET @DUPLICATE_CHK =''; SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(companyID) & "' AND CP_PARAM_TYPE = 'DO' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    intIncrementNo = 1
                    strDONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                           & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                           & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(companyID) & "' AND CP_PARAM_TYPE = 'DO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                    strDOPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & Common.Parse(companyID) & "' AND CP_PARAM_TYPE = 'DO') "

                    'Insert into DO Master 
                    dtMstr = ds.Tables(0) 'fr dtDOMstr
                    dr = dtMstr.Rows(0)

                    dtDtls = ds.Tables(1) ' fr dtDODtls

                    '//Get PO Line outstanding qty from Database and compare with user entered shipped
                    '//if any of outstanding < shipped, disallow DO creation
                    If (btn = "Submit") Then
                        Dim strPOLine As String
                        Dim intItemCnt As Integer
                        intItemCnt = dtDtls.Rows.Count
                        For Each drDtl In dtDtls.Rows
                            If strPOLine = "" Then
                                strPOLine = drDtl("DOD_PO_LINE")
                            Else
                                strPOLine += "," & drDtl("DOD_PO_LINE")
                            End If
                        Next
                        dtQutS = GetPOLineOutstanding(dr("POD_B_COY_ID"), dr("POD_PO_NO"), strPOLine)
                        Dim drOutS As DataRow()
                        For Each drDtl In dtDtls.Rows
                            drOutS = dtQutS.Select("POD_PO_LINE=" & drDtl("DOD_PO_LINE"))
                            SQLQuery = "SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF((POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY)< " & drDtl("DOD_SHIPPED_QTY") & ",'outs', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM PO_Details WHERE POD_PO_NO = '" & dr("POD_PO_NO") & "' AND POD_COY_ID='" & dr("POD_B_COY_ID") & "' AND POD_PO_LINE = '" & drDtl("DOD_PO_LINE") & "'"
                            Common.Insert2Ary(strAryQuery, SQLQuery)

                            If drOutS.Length > 0 Then
                                If drOutS(0)("Outs") < drDtl("DOD_SHIPPED_QTY") Then
                                    strMsg = "outs" 'out < ship
                                    Return False
                                End If
                            End If
                        Next
                    End If
                    '//Get PO Line outstanding qty from Database and compare with user entered shipped
                    '//if any of outstanding < shipped, disallow DO creation

                    SQLQuery = "INSERT INTO DO_Mstr (DOM_S_COY_ID,DOM_DO_NO,DOM_DO_Date,DOM_S_Ref_No,DOM_S_REF_DATE,DOM_PO_INDEX," &
                            " DOM_D_Addr_Code,DOM_WAYBILL_NO,DOM_FREIGHT_CARRIER,DOM_FREIGHT_AMT,DOM_DO_REMARKS,DOM_DO_STATUS,DOM_CREATED_BY, " &
                            " DOM_CREATED_DATE,DOM_NOOFCOPY_PRINTED,DOM_DO_PREFIX,DOM_D_ADDR_LINE1,DOM_D_ADDR_LINE2, " &
                            " DOM_D_ADDR_LINE3,DOM_D_POSTCODE,DOM_D_CITY,DOM_D_STATE,DOM_D_COUNTRY)" &
                            " Values ('" & Common.Parse(companyID) & "'," & strDONo &
                            "," & Common.ConvertDate(Now()) &
                            ",'" & Common.Parse(dr("DOM_S_Ref_No")) & "'" &
                            "," & ParseDate(dr("DOM_S_REF_DATE")) &
                            "," & Common.Parse(dr("DOM_PO_INDEX")) &
                            ",'" & Common.Parse(dr("DOM_D_Addr_Code")) &
                            "','" & Common.Parse(dr("DOM_WAYBILL_NO")) & "" &
                            "','" & Common.Parse(dr("DOM_FREIGHT_CARRIER")) &
                            "'," & IIf(dr("DOM_FREIGHT_AMT") = "", "NULL", dr("DOM_FREIGHT_AMT")) &
                            ",'" & Common.Parse(dr("DOM_DO_REMARKS")) &
                            "','" & Common.Parse(strStatus) &
                            "','" & userID &
                            "'," & ParseDate(Now()) &
                            ", 0 ," & strDOPrefix &
                            ",'" & Common.Parse(dr("POD_D_Addr_Line1")) &
                            "','" & Common.Parse(dr("POD_D_Addr_Line2")) &
                            "','" & Common.Parse(dr("POD_D_Addr_Line3")) &
                            "','" & Common.Parse(dr("POD_D_PostCode")) &
                            "','" & Common.Parse(dr("POD_D_City")) &
                            "','" & Common.Parse(dr("POD_D_State")) &
                            "','" & Common.Parse(dr("POD_D_Country")) & "')"
                    Common.Insert2Ary(strAryQuery, SQLQuery)

                    ''Michelle (21/1/2013) - Issue 1727
                    'SQLQuery = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT"
                    'SQLQuery &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE) "
                    'SQLQuery &= "SELECT CDDA_COY_ID," & strDONo & ", CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE "
                    'SQLQuery &= "FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'SQLQuery &= "AND CDDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDDA_TYPE = 'H' AND CDDA_STATUS = 'N' "
                    ''Exclude those new attachment where user later delete it
                    'SQLQuery &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                    'SQLQuery &= "WHERE CDDA_TYPE = 'H' AND CDDA_STATUS = 'D' )"
                    'Common.Insert2Ary(strAryQuery, SQLQuery)
                    ''Clear the temp table
                    'SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'SQLQuery &= "AND CDDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDDA_TYPE = 'H'"
                    'Common.Insert2Ary(strAryQuery, SQLQuery)

                    'Insert into DO Details

                    For Each drDtl In dtDtls.Rows
                        intShipQty = CInt(IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")))

                        'SQLQuery = "INSERT INTO DO_Details (DOD_S_COY_ID,DOD_DO_NO,DOD_DO_LINE,DOD_PO_LINE,DOD_DO_QTY,DOD_SHIPPED_QTY,DOD_REMARKS)" & _
                        '        " Values ('" & Common.Parse(HttpContext.Current.Session("CompanyId")) & _
                        '        "'," & strDONo & _
                        '        "," & Common.Parse(drDtl("DOD_DO_LINE")) & _
                        '        "," & Common.Parse(drDtl("DOD_PO_LINE")) & _
                        '        "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) & _
                        '        "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) & _
                        '        ",'" & Common.Parse(drDtl("DOD_REMARKS")) & "')"

                        SQLQuery = "INSERT INTO DO_Details (DOD_S_COY_ID,DOD_DO_NO,DOD_DO_LINE,DOD_PO_LINE,DOD_DO_QTY,"

                        If LotBln = True Then
                            SQLQuery &= "DOD_DO_LOT_NO,"
                        End If

                        SQLQuery &= "DOD_SHIPPED_QTY,DOD_REMARKS)"
                        SQLQuery &= " Values ('" & Common.Parse(companyID)
                        SQLQuery &= "'," & strDONo
                        SQLQuery &= "," & Common.Parse(drDtl("DOD_DO_LINE"))
                        SQLQuery &= "," & Common.Parse(drDtl("DOD_PO_LINE"))
                        SQLQuery &= "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY"))

                        If LotBln = True Then
                            SQLQuery &= "," & drDtl("DOD_LotNo")
                        End If

                        SQLQuery &= "," & IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY"))
                        SQLQuery &= ",'" & Common.Parse(drDtl("DOD_REMARKS")) & "')"
                        Common.Insert2Ary(strAryQuery, SQLQuery)

                        If (btn = "Submit") Then
                            SQLQuery = " UPDATE PO_Details Set POD_Delivered_Qty = POD_Delivered_Qty + " &
                            IIf(drDtl("DOD_SHIPPED_QTY") = "", 0, drDtl("DOD_SHIPPED_QTY")) &
                            " WHERE PO_Details.POD_PO_NO = '" & dr("POD_PO_NO") & "' AND POD_COY_ID='" & dr("POD_B_COY_ID") &
                            "' And PO_Details.POD_PO_LINE = " & drDtl("DOD_PO_LINE")
                            Common.Insert2Ary(strAryQuery, SQLQuery)

                            SQLQuery = SetPOFulFilment(dr("POD_PO_NO"), dr("POD_B_COY_ID"))
                            Common.Insert2Ary(strAryQuery, SQLQuery)

                            strPONo = dr("POD_PO_NO")
                            strBCoyID = dr("POD_B_COY_ID")
                        End If
                    Next

                    If Not aryLotNo Is Nothing Then
                        Dim i As Integer

                        For i = 0 To aryLotNo.Count - 1
                            If aryLotNo(i)(0) <> "" And aryLotNo(i)(1) <> "" And aryLotNo(i)(2) <> "" And aryLotNo(i)(3) <> "" Then
                                SQLQuery = "INSERT INTO DO_LOT (DOL_COY_ID, DOL_DO_NO, DOL_ITEM_CODE, DOL_LOT_QTY, DOL_LOT_NO, " &
                                        "DOL_DO_MANU_DT, DOL_DO_EXP_DT, DOL_IQC_MANU_DT, DOL_IQC_EXP_DT, DOL_DO_MANUFACTURER, DOL_ITEM_LINE, DOL_PO_LINE) Values (" &
                                        "'" & Common.Parse(companyID) &
                                        "'," & strDONo &
                                        ",'" & Common.Parse(aryLotNo(i)(6)) &
                                        "'," & aryLotNo(i)(0) &
                                        ",'" & Common.Parse(aryLotNo(i)(1)) &
                                        "','" & Format(CDate(aryLotNo(i)(2)), "yyyy-MM-dd") &
                                        "','" & Format(CDate(aryLotNo(i)(3)), "yyyy-MM-dd") &
                                        "','" & Format(CDate(aryLotNo(i)(2)), "yyyy-MM-dd") &
                                        "','" & Format(CDate(aryLotNo(i)(3)), "yyyy-MM-dd") &
                                        "','" & Common.Parse(aryLotNo(i)(4)) &
                                        "'," & aryLotNo(i)(7) &
                                        ",'" & aryLotNo(i)(10) & "')"


                                Common.Insert2Ary(strAryQuery, SQLQuery)
                            End If
                        Next

                        Dim strTemp As String = "(SELECT DOL_LOT_INDEX FROM DO_LOT WHERE DOL_COY_ID=CDDA_COY_ID AND DOL_ITEM_CODE=CDDA_ITEM_CODE AND DOL_PO_LINE=CDDA_PO_LINE AND DOL_ITEM_LINE=CDDA_LINE_NO AND DOL_DO_NO=" & strDONo & ") as CDDA_LOT_INDEX"

                        'Michelle (21/1/2013) - Issue 1727
                        SQLQuery = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT"
                        SQLQuery &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ITEM_CODE,CDDA_PO_LINE,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_LOT_INDEX,CDDA_LINE_NO,CDDA_TYPE) "
                        SQLQuery &= "SELECT CDDA_COY_ID," & strDONo & ", CDDA_HUB_FILENAME,CDDA_ITEM_CODE,CDDA_PO_LINE,CDDA_ATTACH_FILENAME,CDDA_FILESIZE," & strTemp & ",CDDA_LINE_NO,CDDA_TYPE "
                        SQLQuery &= "FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
                        SQLQuery &= "AND CDDA_DOC_NO IN (" & dr("CDDA_DOC_NO") & ") AND (CDDA_TYPE = 'H' OR CDDA_TYPE = 'D') "
                        'Exclude those new attachment where user later delete it
                        SQLQuery &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                        SQLQuery &= "WHERE (CDDA_TYPE = 'H' OR CDDA_TYPE = 'D') AND CDDA_STATUS = 'D' )"
                        Common.Insert2Ary(strAryQuery, SQLQuery)
                        'Clear the temp table
                        'SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
                        'SQLQuery &= "AND CDDA_DOC_NO = '" & "HttpContext.Current.Session.SessionID" & "' AND (CDDA_TYPE = 'H' OR CDDA_TYPE = 'D')"
                        'Common.Insert2Ary(strAryQuery, SQLQuery)

                    Else
                        'Michelle (21/1/2013) - Issue 1727
                        SQLQuery = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT"
                        SQLQuery &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE) "
                        SQLQuery &= "SELECT CDDA_COY_ID," & strDONo & ", CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE "
                        SQLQuery &= "FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
                        SQLQuery &= "AND CDDA_DOC_NO IN (" & dr("CDDA_DOC_NO") & ") AND CDDA_TYPE = 'H' AND CDDA_STATUS = 'N' "
                        'Exclude those new attachment where user later delete it
                        SQLQuery &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                        SQLQuery &= "WHERE CDDA_TYPE = 'H' AND CDDA_STATUS = 'D' )"
                        Common.Insert2Ary(strAryQuery, SQLQuery)
                        'Clear the temp table
                        'SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' "
                        'SQLQuery &= "AND CDDA_DOC_NO = '" & "HttpContext.Current.Session.SessionID" & "' AND CDDA_TYPE = 'H'"
                        'Common.Insert2Ary(strAryQuery, SQLQuery)

                        ''Chee Hong (18/3/2013) - SEH eProcurement Enhancement (Included CDDA_TYPE = 'D')
                        'SQLQuery = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT"
                        'SQLQuery &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ITEM_CODE,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_LOT_INDEX,CDDA_LINE_NO,CDDA_TYPE) "
                        'SQLQuery &= "SELECT CDDA_COY_ID," & strDONo & ", CDDA_HUB_FILENAME,CDDA_ITEM_CODE,CDDA_ATTACH_FILENAME,CDDA_FILESIZE," & strTemp & ",CDDA_LINE_NO,CDDA_TYPE "
                        'SQLQuery &= "FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        'SQLQuery &= "AND CDDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDDA_TYPE = 'D' AND CDDA_STATUS = 'N' "
                        ''Exclude those new attachment where user later delete it
                        'SQLQuery &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                        'SQLQuery &= "WHERE CDDA_TYPE = 'D' AND CDDA_STATUS = 'D' )"
                        'Common.Insert2Ary(strAryQuery, SQLQuery)
                        ''Clear the temp table
                        'SQLQuery = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        'SQLQuery &= "AND CDDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' AND CDDA_TYPE = 'D'"
                        'Common.Insert2Ary(strAryQuery, SQLQuery)

                    End If

                End If

                Dim objUsers As New Users
                If (btn = "Submit") Then
                    Dim objTrack As New Tracking
                    strsql = objTrack.updateDocMatchingNew(strPONo, strDONo, strPONo, "DO", companyID, strBCoyID)
                    Common.Insert2Ary(strAryQuery, strsql)

                    'objUsers.Log_UserActivityNew(strBCoyID, HttpContext.Current.Session.SessionID, strAryQuery, WheelModule.Fulfillment, WheelUserActivity.V_SubmitDO, strDONo, dr("POD_PO_NO"))
                    objTrack = Nothing
                Else
                    'objUsers.Log_UserActivityNew(strBCoyID, HttpContext.Current.Session.SessionID, strAryQuery, WheelModule.Fulfillment, WheelUserActivity.V_SaveDO, strDONo, dr("POD_PO_NO"))
                End If
                objUsers = Nothing

                SQLQuery = " SET @T_NO = " & strDONo & "; "
                Common.Insert2Ary(strAryQuery, SQLQuery)

                SQLQuery = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(companyID) & "' AND CP_PARAM_TYPE = 'DO' "
                Common.Insert2Ary(strAryQuery, SQLQuery)

                'If objDb.BatchExecuteNew(strAryQuery, , strNewDONo, "T_NO") Then 'objDb.BatchExecute(strAryQuery) Then
                If objDb.BatchExecuteForDup(strAryQuery, , strNewDONo, "T_NO") Then 'objDb.BatchExecute(strAryQuery) Then
                    strDONo = strNewDONo
                    If btn <> "Save" Then
                        If strNewDONo <> "Generated" Then
                            Dim objMail As New Email
                            'objMail.sendNotification(EmailType.DOCreated, HttpContext.Current.Session("UserID"), strBCoyID, HttpContext.Current.Session("CompanyID"), strDONo, strPONo)
                            'Michelle (23/1/2010) - To remove sending email to storekeeper
                            'objMail.sendNotification(EmailType.DOCreatedToSK, HttpContext.Current.Session("UserID"), strBCoyID, HttpContext.Current.Session("CompanyID"), strDONo, strPONo)
                            objMail = Nothing
                        Else
                            strMsg = "outs"
                            Return False
                        End If

                    End If
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function GetPOLineOutstanding(ByVal strBCoyID As String, ByVal strPONo As String, ByVal strPOLine As String) As DataTable
            Dim SQLQuery As String
            'POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY,POD_CANCELLED_QTY
            SQLQuery = " Select POD_PO_LINE,POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY as Outs " _
            & " From PO_Details WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID='" & strBCoyID & "'" _
            & " AND POD_PO_LINE IN (" & strPOLine & " )"
            Return objDb.FillDs(SQLQuery).Tables(0)
        End Function

        Public Function GetDOSumm(ByVal userID As String, ByVal POIndex As Integer, Optional ByVal strSCoyID As String = "") As String
            Dim SqlQuery As String
            'Dim dsDOSumm As DataSet

            If strSCoyID = "" Then
                strSCoyID = objUsr.getCompanyID(userID)
            End If

            SqlQuery = " SELECT Distinct DO_Mstr.DOM_DO_INDEX,DO_Mstr.DOM_Created_Date as date_created, " &
            " DO_Mstr.DOM_PO_Index,DO_Mstr.DOM_Created_By,USER_MSTR.UM_USER_NAME,DO_Mstr.DOM_DO_NO,DO_Mstr.DOM_D_Addr_Code, DO_Mstr.DOM_S_Coy_ID " &
            " From DO_Mstr,USER_MSTR " &
            " WHERE DO_Mstr.DOM_S_Coy_ID = '" & strSCoyID & "'" &
            " and DO_Mstr.DOM_PO_Index = " & POIndex &
            " and DO_Mstr.DOM_Created_By =  USER_MSTR.UM_USER_ID" &
            " and DO_Mstr.DOM_S_Coy_ID =  USER_MSTR.UM_COY_ID AND DO_MSTR.DOM_DO_STATUS <> " & DOStatus.Draft
            'If AddrCode <> "" Then
            '    SqlQuery = SqlQuery & " and  DO_Mstr.DOM_D_Addr_Code ='" & AddrCode & "'"
            'End If
            'dsDOSumm = objDb.FillDs(SqlQuery)
            Return SqlQuery
        End Function

        Public Function GetMfgNameForLot(ByVal strItemCode As String, ByVal strBCoyID As String) As DataSet
            Dim strSql As String
            Dim dsDel As New DataSet

            strSql = "SELECT PM_MANUFACTURER, PM_MANUFACTURER2, PM_MANUFACTURER3 FROM PRODUCT_MSTR " &
                    "WHERE PM_S_COY_ID = '" & Common.Parse(strBCoyID) & "' AND PM_VENDOR_ITEM_CODE = '" & Common.Parse(strItemCode) & "'"

            dsDel = objDb.FillDs(strSql)
            GetMfgNameForLot = dsDel

        End Function

        Public Function GetSelectedMfgFromPO(ByVal strPOLine As String, ByVal strPONo As String, ByVal strBCoyID As String) As String
            Dim strSql, strMfg As String

            strSql = "SELECT IFNULL(POD_MANUFACTURER,'') FROM PO_DETAILS " &
                    "WHERE POD_PO_LINE = '" & strPOLine & "' AND POD_COY_ID = '" & strBCoyID & "' AND POD_PO_NO = '" & strPONo & "'"

            strMfg = objDb.GetVal(strSql)
            GetSelectedMfgFromPO = strMfg

        End Function

        '//change by Moo
        '//need to pass in company id because this function may be when buyer click on DO link
        Public Function DOReport(ByVal DO_Number As String, ByVal strCoyID As String) As DataSet
            Dim dsDOReport As DataSet
            Dim SqlQuery, SqlQuery1 As String


            'PO_Mstr.Delivery_Addr_1,PO_Mstr.Delivery_Addr_2,PO_Mstr.Delivery_Addr_3,PO_Mstr.Delivery_Addr_4,
            '" DO_Mstr.Billing_Addr_Code,DO_Mstr.Billing_Addr_Line_1,DO_Mstr.Billing_Addr_Line_2,DO_Mstr.Billing_Addr_Line_3," & _
            '            " DO_Mstr.Billing_Addr_Line_4, " & _
            SqlQuery = " SELECT Distinct PO_Details.POD_Delivered_Qty,PO_Mstr.POM_External_Remark,PO_Mstr.POM_PO_Date,PO_Mstr.POM_CREATED_DATE," _
                    & "DO_Mstr.*, DO_Details.*, " &
                    "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=DO_Mstr.DOM_DO_Status AND STATUS_TYPE='DO') AS Status_Desc, " &
                    " PO_Mstr.POM_S_Coy_ID,PO_Mstr.POM_PO_Index,PO_Mstr.POM_Payment_TERM,PO_Mstr.POM_PAYMENT_METHOD," _
                    & "PO_Mstr.POM_Shipment_Term,PO_Mstr.POM_Shipment_Mode," &
                    " PO_Mstr.POM_PO_No,PO_Details.POD_D_Addr_Line1,PO_Details.POD_D_Addr_Line2,PO_Details.POD_D_Addr_Line3, " &
                    " (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Details.POD_D_State AND CODE_VALUE = PO_DETAILS.POD_D_COUNTRY AND CODE_CATEGORY='S') as POD_D_State ,(SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Details.POD_D_Country AND CODE_CATEGORY='CT') as POD_D_Country ,PO_Details.POD_D_PostCode,PO_Details.POD_D_City," &
                    " PO_Mstr.POM_B_Coy_ID, PO_Mstr.POM_PO_No, MC1.CM_Coy_Logo,MC1.CM_TAX_Reg_No,MC1.CM_BUSINESS_REG_NO,MC1.CM_PHONE,MC1.CM_EMAIL, " &
                    " PO_Mstr.POM_B_Addr_Line1,PO_Mstr.POM_B_Addr_Line2,PO_Mstr.POM_B_Addr_Line3, " &
                    " PO_Mstr.POM_B_POSTCODE, PO_Mstr.POM_B_CITY, (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Mstr.POM_B_STATE AND CODE_VALUE = PO_DETAILS.POD_D_COUNTRY AND CODE_CATEGORY='S') as POM_B_STATE, (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Mstr.POM_B_COUNTRY AND CODE_CATEGORY='CT' ) AS POM_B_COUNTRY,PO_Mstr.POM_BUYER_NAME," &
                    " PO_Mstr.POM_S_COY_NAME, PO_Mstr.POM_S_ATTN,PO_Mstr.POM_S_ADDR_LINE1, PO_Mstr.POM_S_ADDR_LINE2, PO_Mstr.POM_DEL_CODE, " &
                    " PO_Mstr.POM_S_ADDR_LINE3, PO_Mstr.POM_S_POSTCODE, PO_Mstr.POM_S_CITY, (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Mstr.POM_S_STATE AND CODE_VALUE=POM_S_COUNTRY AND CODE_CATEGORY='S') as POM_S_STATE , (SELECT CODE_DESC FROM CODE_MSTR where Code_ABBR = PO_Mstr.POM_S_COUNTRY) as POM_S_COUNTRY, " &
                    " PO_Details.POD_Ordered_Qty,PO_Details.POD_B_ITEM_CODE,PO_Details.POD_Vendor_Item_Code,PO_Details.POD_Product_Desc,PO_Details.POD_UOM,PO_Details.POD_Po_Line, " &
                    " PO_Details.POD_ETD,PO_Details.POD_Min_Pack_Qty,PO_Details.POD_Min_Order_Qty,PO_Details.POD_Warranty_Terms,PO_Details.POD_ITEM_TYPE,MC.CM_Coy_Name" &
                    " FROM DO_Mstr, DO_Details, PO_Mstr, PO_Details, Company_Mstr MC,Company_Mstr MC1 " &
                    " WHERE DO_Mstr.DOM_DO_NO = '" & DO_Number & "' " &
                    " AND DO_Mstr.DOM_DO_NO = DO_Details.DOD_Do_No" &
                    " AND DO_Mstr.DOM_S_Coy_ID = DO_Details.DOD_S_COY_ID" &
                    " And PO_Details.POD_Po_Line = DO_Details.DOD_PO_Line" &
                    " And DO_Mstr.DOM_PO_Index = PO_Mstr.POM_PO_Index" &
                    " and PO_Mstr.POM_PO_No = PO_Details.POD_Po_No" &
                    " and PO_Mstr.POM_B_Coy_ID = PO_Details.POD_Coy_ID " &
                    " and PO_Mstr.POM_B_COY_ID = MC.CM_Coy_ID" &
                    " and PO_Mstr.POM_S_COY_ID = MC1.CM_Coy_ID" &
                    " AND DO_Mstr.DOM_S_Coy_ID = '" & strCoyID & "' " &
                    " Order by DO_Details.DOD_DO_Line "

            Dim strPOIndex As String
            SqlQuery1 = "SELECT ISNULL(DOM_PO_INDEX,'') FROM DO_MSTR WHERE DOM_DO_NO='" & DO_Number & "' AND DOM_S_Coy_ID='" & strCoyID & "'"
            strPOIndex = objDb.GetVal(SqlQuery1)
            SqlQuery1 = "SELECT PRM_S_ATTN,PRM_REQ_NAME FROM PR_MSTR WHERE PRM_PO_INDEX='" & strPOIndex & "'"
            dsDOReport = objDb.FillDs(SqlQuery & ";" & SqlQuery1)

            Return dsDOReport
        End Function

        Public Function GetDOLotList(ByVal DO_Number As String, ByVal strCoyID As String, ByVal strItemCode As String, ByVal strPOLine As String) As DataSet
            Dim dsLot As New DataSet
            Dim strSql As String

            strSql = "SELECT DOL_LOT_QTY, DOL_LOT_NO, DOL_DO_MANU_DT, DOL_DO_EXP_DT, DOL_DO_MANUFACTURER, DOL_ITEM_LINE, DOL_LOT_INDEX " &
                    "FROM DO_LOT WHERE DOL_COY_ID = '" & Common.Parse(strCoyID) & "' AND DOL_DO_NO = '" & Common.Parse(DO_Number) & "' AND DOL_ITEM_CODE = '" & Common.Parse(strItemCode) & "' " &
                    "AND DOL_PO_LINE = '" & strPOLine & "' ORDER BY DOL_ITEM_LINE"

            dsLot = objDb.FillDs(strSql)
            GetDOLotList = dsLot
        End Function

        Private Function ParseDate(ByVal ctrl As Object) As Object
            If CStr(ctrl) <> "" Then
                Return Common.ConvertDate(ctrl)
            Else
                Return "null"
            End If
        End Function

        Public Function DeleteDO(ByVal strDONo As String) As String
            Dim strSql, strSQLAry(0), strMsg As String
            'Delete from DO_DETAILS
            strSql = "Delete From DO_Details Where DOD_DO_No='" & strDONo & "' AND DOD_S_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strSQLAry, strSql)
            'Delete from DO_MSTR
            strSql = "Delete From DO_Mstr Where DOM_DO_No='" & strDONo & "' AND DOM_S_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strSQLAry, strSql)
            'Delete from DO_LOT
            strSql = "Delete From DO_LOT Where DOL_DO_No='" & strDONo & "' AND DOL_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strSQLAry, strSql)
            'Delete from COMPANY_DO_DOC_ATTACHMENT
            strSql = "Delete From COMPANY_DO_DOC_ATTACHMENT Where CDDA_DOC_NO='" & strDONo & "' AND CDDA_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strSQLAry, strSql)
            If objDb.BatchExecute(strSQLAry) Then
                strMsg = "Delivery Order " & strDONo & " deleted."
            End If
            Return strMsg
        End Function

        Public Function IsLocHasDraftDo(ByVal strLocID As String, ByVal intPOIdx As Integer) As Boolean
            Dim strSql As String
            strSql = "SELECT '*' FROM DO_MSTR WHERE DOM_PO_INDEX=" & intPOIdx & " AND DOM_D_ADDR_CODE='" & strLocID & "' AND DOM_DO_STATUS=" & DOStatus.Draft
            If objDb.Exist(strSql) Then
                IsLocHasDraftDo = True
            Else
                IsLocHasDraftDo = False
            End If
        End Function

        Public Function SetPOFulFilment(ByVal strPONo As String, ByVal strBCoyID As String) As String
            Dim SQLQuery As String
            'Michelle (14/9/2010) - To change syntax to cater for MYSAL
            'SQLQuery = "UPDATE PO_MSTR SET POM_FULFILMENT=FULFIL " _
            '     & " FROM (SELECT POD_COY_ID,POD_PO_NO,CASE WHEN ORDER1=DEL+CANCEL AND DEL <>0 THEN 3 " _
            '     & " WHEN ORDER1 > DEL+CANCEL AND DEL <> 0 THEN 2 " _
            '     & " WHEN DEL=0 AND CANCEL=0 THEN 1 WHEN ORDER1=CANCEL THEN 5 " _
            '     & " ELSE 1 END AS FULFIL FROM( " _
            '     & " SELECT POD_COY_ID,POD_PO_NO,SUM(POD_ORDERED_QTY) AS ORDER1, " _
            '     & " SUM(POD_DELIVERED_QTY) AS DEL,SUM(POD_CANCELLED_QTY) AS CANCEL " _
            '     & " FROM PO_DETAILS WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID='" & strBCoyID & _
            '     "' GROUP BY POD_COY_ID,POD_PO_NO) A, PO_MSTR B " _
            '     & " WHERE A.POD_COY_ID=B.POM_B_COY_ID AND A.POD_PO_NO=B.POM_PO_NO) C, PO_MSTR D " _
            '     & " WHERE C.POD_COY_ID=D.POM_B_COY_ID AND C.POD_PO_NO=D.POM_PO_NO"
            SQLQuery = "UPDATE PO_MSTR D," _
                 & " (SELECT POD_COY_ID,POD_PO_NO,CASE WHEN ORDER1=DEL+CANCEL AND DEL <>0 THEN 3 " _
                 & " WHEN ORDER1 > DEL+CANCEL AND DEL <> 0 THEN 2 " _
                 & " WHEN DEL=0 AND CANCEL=0 THEN 1 WHEN ORDER1=CANCEL THEN 5 " _
                 & " ELSE 1 END AS FULFIL FROM( " _
                 & " SELECT POD_COY_ID,POD_PO_NO,SUM(POD_ORDERED_QTY) AS ORDER1, " _
                 & " SUM(POD_DELIVERED_QTY) AS DEL,SUM(POD_CANCELLED_QTY) AS CANCEL " _
                 & " FROM PO_DETAILS WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID='" & strBCoyID &
                 "' GROUP BY POD_COY_ID,POD_PO_NO) A, PO_MSTR B " _
                 & " WHERE A.POD_COY_ID=B.POM_B_COY_ID AND A.POD_PO_NO=B.POM_PO_NO) C " _
                 & " SET D.POM_FULFILMENT=FULFIL" _
                 & " WHERE C.POD_COY_ID=D.POM_B_COY_ID AND C.POD_PO_NO=D.POM_PO_NO"
            Return SQLQuery
        End Function

        Public Function DONum(ByVal poid As String) As String
            Dim strsql As String = "SELECT DOM_DO_NO FROM do_mstr WHERE DOM_DO_STATUS = 1 AND DOM_PO_INDEX=" & poid
            DONum = objDb.GetVal(strsql)
        End Function
        Public Function getTempDOAttachment(ByVal userID As String, ByVal strDocNo As String, ByVal addressPath As String, Optional ByVal strType As String = "H", Optional ByVal strItemCode As String = "", Optional ByVal strLineNo As String = "", Optional ByVal strPOLine As String = "") As String
            Dim companyID As String = objUsr.getCompanyID(userID)
            Dim strsql As String
            Dim ds As New DataSet
            strsql = String.Format("SELECT CDDA_ATTACH_FILENAME, CDDA_HUB_FILENAME, CDDA_ATTACH_INDEX, CDDA_FILESIZE, CONCAT('{0}', CDDA_HUB_FILENAME) AS PathRef FROM COMPANY_DO_DOC_ATTACHMENT_TEMP ", addressPath)
            strsql &= "WHERE CDDA_DOC_NO IN (" & strDocNo & ") AND CDDA_COY_ID ='" & companyID & "' AND CDDA_STATUS = 'N' "

            If strItemCode <> "" Then
                strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
            End If

            If strLineNo <> "" Then
                strsql &= "AND CDDA_LINE_NO = '" & strLineNo & "' "
            End If

            If strPOLine <> "" Then
                strsql &= "AND CDDA_PO_LINE = '" & strPOLine & "' "
            End If

            strsql &= "AND CDDA_TYPE = '" & strType & "' " 'Get those new attachments
            'Excluding those new attachments which user decide to deletedelete 
            strsql &= "AND CDDA_ATTACH_INDEX  NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDDA_DOC_NO IN (" & strDocNo & ") AND CDDA_COY_ID ='" & companyID & "' AND (CDDA_STATUS = 'D' OR CDDA_STATUS = 'U') "
            strsql &= "AND CDDA_TYPE = '" & strType & "' )"
            strsql &= "UNION " 'Get the attachments of the Draft DO 
            strsql &= String.Format("SELECT CDDA_ATTACH_FILENAME, CDDA_HUB_FILENAME, CDDA_ATTACH_INDEX, CDDA_FILESIZE, CONCAT('{0}', CDDA_HUB_FILENAME) AS PathRef FROM COMPANY_DO_DOC_ATTACHMENT ", addressPath)
            strsql &= "WHERE CDDA_DOC_NO IN (" & strDocNo & ") AND CDDA_COY_ID ='" & companyID & "' "

            If strItemCode <> "" Then
                strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
            End If

            If strLineNo <> "" Then
                strsql &= "AND CDDA_LINE_NO = '" & strLineNo & "' "
            End If

            If strPOLine <> "" Then
                strsql &= "AND CDDA_PO_LINE = '" & strPOLine & "' "
            End If

            strsql &= "AND CDDA_TYPE = '" & strType & "' "
            'Excluding those attachments that are marked for deletion 
            strsql &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDDA_DOC_NO IN (" & strDocNo & ") AND CDDA_COY_ID ='" & companyID & "' AND CDDA_STATUS = 'U' "
            strsql &= "AND CDDA_TYPE = '" & strType & "') "
            'ds = objDb.FillDs(strsql)
            getTempDOAttachment = strsql
        End Function
        Public Function chkDupDOAttach(ByVal strDocNo As String, ByVal strFileName As String, Optional ByVal strType As String = "H", Optional ByVal strLineNo As String = "", Optional ByVal strItemCode As String = "", Optional ByVal strPOLine As String = "") As Boolean
            Dim strsql As String
            Dim intCnt As Integer
            strsql = "(SELECT CDDA_DOC_NO FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDDA_STATUS = 'N' "
            strsql &= "AND CDDA_TYPE = '" & strType & "' AND CDDA_ATTACH_FILENAME ='" & strFileName & "' " 'Get those new attachments

            If strLineNo <> "" Then
                strsql &= "AND CDDA_LINE_NO = '" & strLineNo & "' "
            End If

            If strItemCode <> "" Then
                strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
            End If

            If strPOLine <> "" Then
                strsql &= "AND CDDA_PO_LINE = '" & strPOLine & "' "
            End If

            'Excluding those new attachments which user decide to deletedelete 
            strsql &= "AND CDDA_ATTACH_INDEX  NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND (CDDA_STATUS = 'D' OR CDDA_STATUS = 'U') "

            'If strLineNo <> "" Then
            '    strsql &= "AND CDDA_LINE_NO = '" & strLineNo & "' "
            'End If

            'If strItemCode <> "" Then
            '    strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
            'End If

            strsql &= "AND CDDA_TYPE = '" & strType & "') "
            strsql &= "UNION " 'Get the attachments of the Draft DO 
            strsql &= "SELECT '1' FROM COMPANY_DO_DOC_ATTACHMENT "
            strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDDA_TYPE = '" & strType & "' AND CDDA_ATTACH_FILENAME ='" & strFileName & "' "

            If strLineNo <> "" Then
                strsql &= "AND CDDA_LINE_NO = '" & strLineNo & "' "
            End If

            If strItemCode <> "" Then
                strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
            End If

            If strPOLine <> "" Then
                strsql &= "AND CDDA_PO_LINE = '" & strPOLine & "' "
            End If

            'Excluding those attachments that are marked for deletion 
            strsql &= "AND CDDA_ATTACH_INDEX NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CDDA_STATUS = 'U' "

            'If strLineNo <> "" Then
            '    strsql &= "AND CDDA_LINE_NO = '" & strLineNo & "' "
            'End If

            'If strItemCode <> "" Then
            '    strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
            'End If

            strsql &= "AND CDDA_TYPE = '" & strType & "')) AS TEMP"
            intCnt = objDb.GetCount(strsql)
            If intCnt > 0 Then Return True
            Return False
        End Function
        Public Function withAttach(ByVal strDocNo As String, Optional ByVal strCoyID As String = "") As Boolean
            Dim strsql As String
            Dim intCnt As Integer
            strsql = "(SELECT CDDA_DOC_NO FROM COMPANY_DO_DOC_ATTACHMENT "
            If strCoyID = "" Then
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "
            Else
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & strCoyID & "' "
            End If
            strsql &= "AND CDDA_TYPE = 'H') AS TEMP"
            intCnt = objDb.GetCount(strsql)
            If intCnt > 0 Then Return True
            Return False
        End Function
        Public Function getDOAttachment(ByVal strDocNo As String, Optional ByVal strCoyID As String = "", Optional ByVal strType As String = "H", Optional ByVal strItemCode As String = "", Optional ByVal strLineNo As String = "", Optional ByVal strPOLine As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT CDDA_ATTACH_INDEX, CDDA_DOC_NO, CDDA_HUB_FILENAME, CDDA_ATTACH_FILENAME, CDDA_FILESIZE FROM COMPANY_DO_DOC_ATTACHMENT "
            If strCoyID = "" Then
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & strCoyID & "' "
            Else
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & strCoyID & "' "
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
            getDOAttachment = ds
        End Function

        Public Function getLotAttachment(ByVal strDocNo As String, ByVal strIndex As String, Optional ByVal strCoyID As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT CDDA_DOC_NO, CDDA_HUB_FILENAME, CDDA_ATTACH_FILENAME, CDDA_FILESIZE FROM COMPANY_DO_DOC_ATTACHMENT "
            If strCoyID = "" Then
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_LOT_INDEX = '" & strIndex & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "
            Else
                strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_LOT_INDEX = '" & strIndex & "' AND CDDA_COY_ID ='" & strCoyID & "' "
            End If
            strsql &= "AND CDDA_TYPE = 'D'"
            ds = objDb.FillDs(strsql)
            getLotAttachment = ds
        End Function

        Public Function deleteTempDOAttachment(ByVal userID As String, ByVal intIndex As Integer, ByVal strDocNo As String, ByVal strType As String, ByVal strStatus As String, Optional ByVal blnDelTemp As Boolean = False, Optional ByVal strItemCode As String = "", Optional ByVal strPOLine As String = "")
            Dim strsql As String
            Dim companyID As String = objUsr.getCompanyID(userID)

            If blnDelTemp Then
                'Clear the temp table 
                strsql = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_COY_ID = '" & companyID & "' AND CDDA_DOC_NO ='" & strDocNo & "' "

                If strItemCode <> "" Then
                    strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' "
                End If

                If strPOLine <> "" Then
                    strsql &= "AND CDDA_PO_LINE = '" & strPOLine & "' "
                End If
            Else
                If strStatus = "U" Then 'ie. delete those attachment that is already in the database or delete those new attachment from the draft DO, HUB_FILENAME will store the cdda_attach_index of the actual table
                    strsql = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT_TEMP "
                    strsql &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_TYPE,CDDA_STATUS,CDDA_DATE) "
                    strsql &= "VALUES('" & companyID & "', '" & strDocNo & "','" & intIndex & "','" & strType & "','" & strStatus & "', NOW())"
                Else 'ie. delete those attachment that have just added (not yet save into database)
                    strsql = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT_TEMP "
                    strsql &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_TYPE,CDDA_STATUS,CDDA_DATE) "
                    strsql &= "VALUES('" & companyID & "', '" & strDocNo & "','" & intIndex & "','" & strType & "','D', NOW())"
                End If
            End If
            objDb.Execute(strsql)
        End Function

        Public Function deleteTempDOAttachment2(ByVal intIndex As Integer, ByVal strDocNo As String, ByVal strType As String, ByVal strStatus As String, Optional ByVal blnDelTemp As Boolean = False, Optional ByVal strItemCode As String = "", Optional ByVal strClean As String = "", Optional ByVal strPOLine As String = "")
            Dim strsql As String

            If strClean <> "" Then

                'strsql = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT_TEMP "
                'strsql &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_TYPE,CDDA_STATUS,CDDA_DATE) "
                'strsql &= "VALUES('" & HttpContext.Current.Session("CompanyId") & "', '" & strDocNo & "','" & intIndex & "','" & strType & "','U', NOW()); "


                strsql = "DELETE FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_LINE_NO = " & strClean & " AND CDDA_LOT_INDEX IS NULL AND CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDDA_TYPE = 'D' AND CDDA_DOC_NO ='" & strDocNo & "' "

                If strItemCode <> "" And strPOLine <> "" Then
                    strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND CDDA_PO_LINE = '" & strPOLine & "'; "
                End If

                'strsql &= "AND CDDA_ATTACH_INDEX  NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                'strsql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND (CDDA_STATUS = 'D' OR CDDA_STATUS = 'U')); "

                'strsql &= "DELETE FROM COMPANY_DO_DOC_ATTACHMENT WHERE CDDA_LINE_NO = " & strClean & " AND CDDA_LOT_INDEX IS NULL AND CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDDA_TYPE = 'D' AND CDDA_DOC_NO ='" & strDocNo & "' "
                strsql &= "DELETE FROM COMPANY_DO_DOC_ATTACHMENT WHERE CDDA_LINE_NO = " & strClean & " AND CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDDA_TYPE = 'D' AND CDDA_DOC_NO ='" & strDocNo & "' "

                If strItemCode <> "" And strPOLine <> "" Then
                    strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND CDDA_PO_LINE = '" & strPOLine & "'; "
                End If

            End If
            objDb.Execute(strsql)
        End Function

        Public Function UpdateAttachmentLine(ByVal strDocNo As String, ByVal strItemCode As String, ByVal strOldLine As String, ByVal strNewLine As String, ByVal strPOLine As String)
            Dim strsql As String

            strsql = "SELECT '*' FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_DOC_NO = '" & strDocNo & "' AND CDDA_TYPE = 'D' AND CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND CDDA_LINE_NO = '" & strOldLine & "' AND CDDA_PO_LINE = '" & strPOLine & "'"

            If objDb.Exist(strsql) Then
                strsql = "UPDATE COMPANY_DO_DOC_ATTACHMENT_TEMP SET CDDA_LINE_NO = '" & strNewLine & "' "
                strsql &= "WHERE CDDA_TYPE = 'D' AND CDDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql &= "AND CDDA_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND CDDA_LINE_NO = '" & strOldLine & "' AND CDDA_PO_LINE = '" & strPOLine & "'"
                objDb.Execute(strsql)
            End If
        End Function

        Public Function getDOLot(ByVal DO_Number As String, ByVal strItemCode As String, ByVal strPOLine As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT DOL_LOT_QTY, DOL_LOT_NO, DOL_DO_MANU_DT, DOL_DO_EXP_DT, DOL_DO_MANUFACTURER, DOL_ITEM_LINE FROM DO_LOT "
            strsql &= "WHERE DOL_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND DOL_DO_NO = '" & DO_Number & "' AND DOL_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND DOL_PO_LINE ='" & strPOLine & "' "

            ds = objDb.FillDs(strsql)
            getDOLot = ds
        End Function

        Public Function chkIQCWithAttachment(ByVal ItemCode As String, ByVal ProdCode As String, ByVal POLine As String, ByVal DONo As String, Optional ByVal aryLot As ArrayList = Nothing, Optional ByVal strMode As String = "") As Boolean
            Dim strSql, strAttach As String
            Dim ds As New DataSet
            Dim ds2 As New DataSet
            Dim i As Integer

            strSql = "SELECT PM_S_COY_ID, PM_IQC_IND, IFNULL(PM_IQC_TYPE,'') AS PM_IQC_TYPE " &
                    "FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & ProdCode & "'"
            ds = objDb.FillDs(strSql)

            ''Need QC
            'strSql = "SELECT PM_IQC_IND FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & ProdCode & "'"
            'strNeedQC = objDb.GetVal(strSql)

            ''IQC Type
            'strSql = "SELECT IFNULL(PM_IQC_TYPE,'') AS PM_IQC_TYPE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & ProdCode & "'"
            'strIQCType = objDb.GetVal(strSql)

            'If strNeedQC = "Y" And strIQCType <> "" Then

            'End If

            If ds.Tables(0).Rows(0)("PM_IQC_IND") = "Y" And ds.Tables(0).Rows(0)("PM_IQC_TYPE") <> "" Then
                strSql = "SELECT CPA_PARAM_ATTACHMENT FROM COMPANY_PARAM_ADDITIONAL WHERE " &
                        "CPA_PARAM_LABEL = '" & Common.Parse(ds.Tables(0).Rows(0)("PM_IQC_TYPE")) & "' " &
                        "AND CPA_COY_ID = '" & ds.Tables(0).Rows(0)("PM_S_COY_ID") & "' AND CPA_PARAM_TYPE = 'IQC' "
                strAttach = objDb.GetVal(strSql)

                If strAttach = "Y" Then
                    If Not aryLot Is Nothing Then
                        For i = 0 To aryLot.Count - 1
                            If ItemCode = aryLot(i)(6) And POLine = aryLot(i)(10) And aryLot(i)(0) <> "" And aryLot(i)(1) <> "" Then
                                If strMode = "New" Then
                                    strSql = "SELECT '*' FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_TYPE='D' AND CDDA_LINE_NO = '" & aryLot(i)(7) & "' " &
                                             "AND CDDA_DOC_NO = '" & DONo & "' AND CDDA_ITEM_CODE = '" & Common.Parse(aryLot(i)(6)) & "' AND CDDA_PO_LINE = '" & POLine & "' "

                                    strSql &= "AND CDDA_ATTACH_INDEX  NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                                    strSql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(DONo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND (CDDA_STATUS = 'D' OR CDDA_STATUS = 'U')) "
                                Else
                                    strSql = "SELECT '*' FROM COMPANY_DO_DOC_ATTACHMENT WHERE CDDA_TYPE='D' AND CDDA_LINE_NO = '" & aryLot(i)(7) & "' " &
                                             "AND CDDA_DOC_NO = '" & DONo & "' AND CDDA_ITEM_CODE = '" & Common.Parse(aryLot(i)(6)) & "' AND CDDA_PO_LINE = '" & POLine & "' "

                                    strSql &= "AND CDDA_ATTACH_INDEX  NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                                    strSql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(DONo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND (CDDA_STATUS = 'D' OR CDDA_STATUS = 'U')) "

                                    strSql &= "UNION "

                                    strSql &= "SELECT '*' FROM COMPANY_DO_DOC_ATTACHMENT_TEMP WHERE CDDA_TYPE='D' AND CDDA_LINE_NO = '" & aryLot(i)(7) & "' " &
                                              "AND CDDA_DOC_NO = '" & DONo & "' AND CDDA_ITEM_CODE = '" & Common.Parse(aryLot(i)(6)) & "' AND CDDA_PO_LINE = '" & POLine & "' "

                                    strSql &= "AND CDDA_ATTACH_INDEX  NOT IN (SELECT CDDA_HUB_FILENAME FROM COMPANY_DO_DOC_ATTACHMENT_TEMP "
                                    strSql &= "WHERE CDDA_DOC_NO = '" & Common.Parse(DONo) & "' AND CDDA_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND (CDDA_STATUS = 'D' OR CDDA_STATUS = 'U')) "
                                End If
                                ds2 = objDb.FillDs(strSql)
                                If Not ds2.Tables(0).Rows.Count > 0 Then
                                    Return False
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            Return True
        End Function

        Public Function chkLotWithAO(ByVal strItemCode As String) As Boolean
            Dim strSql, strUserId, strCoyId As String
            Dim blnLot As Boolean

            strUserId = HttpContext.Current.Session("UserId")
            strCoyId = HttpContext.Current.Session("CompanyId")
            'strSql = "SELECT '*' FROM IQC_APPROVAL IQCA, INVENTORY_VERIFY_LOT IVL " & _
            '        "WHERE IQCA.IQCA_IQC_INDEX = IVL.IVL_VERIFY_LOT_INDEX " & _
            '        "AND (IQCA_AO = '" & strUserId & "' OR IQCA_A_AO = '" & strUserId & "') " & _
            '        "AND IVL_LOT_INDEX IN " & _
            '        "(SELECT DOL_LOT_INDEX FROM DO_LOT WHERE DOL_DO_NO = '" & strDoNo & "' AND DOL_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND DOL_COY_ID = '" & strVCompID & "') "

            strSql = "SELECT '*' FROM IQC_APPROVAL IQCA, INVENTORY_VERIFY_LOT IVL, INVENTORY_VERIFY, INVENTORY_MSTR " &
                    "WHERE IQCA.IQCA_IQC_INDEX = IVL.IVL_VERIFY_LOT_INDEX AND (IQCA_AO = '" & strUserId & "' OR IQCA_A_AO = '" & strUserId & "') " &
                    "AND IVL_VERIFY_INDEX = IV_VERIFY_INDEX AND IV_INVENTORY_INDEX = IM_INVENTORY_INDEX " &
                    "AND IM_ITEM_CODE = '" & Common.Parse(strItemCode) & "' AND IM_COY_ID = '" & strCoyId & "' "

            If objDb.Exist(strSql) Then
                blnLot = True
            Else
                blnLot = False
            End If

            Return blnLot
        End Function

        Public Function getDODate(ByVal UserID As String, ByVal strDONo As String) As String
            Dim strSql, strCoyId As String
            strCoyId = objUsr.getCompanyID(UserID)
            strSql = "SELECT DATE_FORMAT(DOM_DO_DATE, '%d/%m/%Y') FROM DO_MSTR WHERE DOM_DO_NO = '" & Common.Parse(strDONo) & "' " &
                    "AND DOM_S_COY_ID = '" & strCoyId & "'"

            getDODate = objDb.GetVal(strSql)
        End Function
    End Class
End Namespace
