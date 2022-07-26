Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports SSO.Component
Imports System.IO
Imports AgoraNEO.AgoraNEO

Namespace AgoraNEO
    Public Class PurchaseOrder_Buyer

        Dim objDb As New EAD.DBCom

        Dim userid As String = userid


        '//PO List - for PO Cancellation
        'Public Function VIEW_POList2(ByVal PO_Status As String, ByVal fulfilment As String, ByVal side As String, Optional ByVal ven_name As String = "", Optional ByVal po_no As String = "", Optional ByVal startdate As String = "", Optional ByVal enddate As String = "", Optional ByVal Buyer_Status As String = "", Optional ByVal PO_Type As String = "") As DataSet
        '    Dim ds As DataSet
        '    Dim strTemp As String
        '    Dim strsql As String

        '    If Buyer_Status = "ByBuyerAdmin" Then
        '        strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," &
        '                   "POM.POM_PO_STATUS,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " &
        '                   "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
        '                   "AS REMARK1," &
        '                   "CASE WHEN (PCM.PCM_REQ_BY) ='" & Common.Parse(userid) & "'" &
        '                   "THEN 'CANCELED BY BA' ELSE (SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND " &
        '                   "C.STATUS_NO=POM.POM_PO_STATUS) END AS STATUS_DESC,PCM.PCM_REQ_BY," &
        '                   "(SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX = POM_PO_INDEX AND PRM_COY_ID = POM_B_COY_ID AND prm_pr_type='CC') PR_NO, POM.POM_URGENT FROM PO_MSTR POM" &
        '                   " LEFT JOIN PO_CR_MSTR PCM ON PCM.PCM_PO_INDEX = POM.POM_PO_INDEX " &
        '                   " WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" &
        '                   " And POM.POM_PO_STATUS IN(" & PO_Status & ") " &
        '                   " and " &
        '                   " ( (POM_BILLING_METHOD='DO' )" &
        '                   " or ( POM_BILLING_METHOD in('FPO','GRN') AND NOT EXISTS (SELECT '*' FROM DO_MSTR DOM WHERE POM.POM_PO_INDEX=DOM.DOM_PO_INDEX " &
        '                   " AND DOM.DOM_DO_STATUS=2)))"
        '    Else

        '        strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," &
        '                 "POM.POM_PO_STATUS,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " &
        '                 "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
        '                 "AS REMARK1," &
        '                 "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " &
        '                 "AS STATUS_DESC,(SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX = POM_PO_INDEX AND PRM_COY_ID = POM_B_COY_ID AND prm_pr_type='CC') PR_NO, POM.POM_URGENT FROM PO_MSTR POM " &
        '                 " WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" &
        '                 " And POM.POM_PO_STATUS IN(" & PO_Status & ") " &
        '                 " and " &
        '                 " ( (POM_BILLING_METHOD='DO' )" &
        '                 " or ( POM_BILLING_METHOD in('FPO','GRN') AND NOT EXISTS (SELECT '*' FROM DO_MSTR DOM WHERE POM.POM_PO_INDEX=DOM.DOM_PO_INDEX " &
        '                 " AND DOM.DOM_DO_STATUS=2)))"
        '    End If

        '    If side = "u" Then
        '        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and POM_BUYER_ID = '" & Common.Parse(userid) & "'"
        '        If ven_name <> "" Then
        '            strTemp = Common.BuildWildCard(ven_name)
        '            strsql = strsql & " AND POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
        '        End If
        '    End If

        '    If side = "b" Then
        '        'Michelle (6/2/2010) - For BA, retrieve all POs
        '        Dim objUser As New Users
        '        Dim IsBoolean As Boolean = objUser.BAdminRole(Common.Parse(userid), Common.Parse(com_id))

        '        If PO_Type = "MyPO" Then
        '            IsBoolean = False
        '        End If

        '        If IsBoolean Then
        '            If Buyer_Status = "ByBuyerAdmin" Then

        '                strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and PCM_REQ_BY = '" & Common.Parse(userid) & "' "

        '                'ElseIf Buyer_Status = "ByBuyer" Then

        '                '    strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and PCM_REQ_BY <> '" & Common.Parse(userid) & "' "
        '            Else
        '                strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '            End If

        '        Else
        '            strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and POM_BUYER_ID = '" & Common.Parse(userid) & "' "
        '        End If



        '        If ven_name <> "" Then
        '            strTemp = Common.BuildWildCard(ven_name)
        '            strsql = strsql & " AND POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
        '        End If
        '    ElseIf side = "v" Then
        '        strsql = strsql & " AND POM.POM_S_COY_ID= '" & Common.Parse(v_com_id) & "'"
        '        '//add by Moo, to filter buyer name
        '        If ven_name <> "" Then
        '            strTemp = Common.BuildWildCard(ven_name)
        '            strsql = strsql & " AND POM.POM_BUYER_NAME" & Common.ParseSQL(strTemp)
        '        End If
        '    End If

        '    If po_no <> "" Then
        '        strTemp = Common.BuildWildCard(po_no)
        '        strsql = strsql & " and POM.POM_PO_NO " & Common.ParseSQL(strTemp) & " "
        '    End If
        '    If startdate <> "" Then
        '        strsql &= "AND POM.POM_PO_DATE >= " & Common.ConvertDate(startdate) & " "
        '    End If

        '    If enddate <> "" Then
        '        strsql &= "AND POM.POM_PO_DATE <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
        '    End If
        '    ds = objDb.FillDs(strsql)
        '    Return ds

        'End Function

        'Public Function get_poDetail2(ByVal PO_No As String, ByVal v_comid As String) As DataSet
        '    Dim strsql As String
        '    Dim DS As DataSet

        '    strsql = "select PO_MSTR.*, PO_DETAILS.*, CONCAT(PO_DETAILS.POD_ASSET_GROUP, CONCAT(' ', PO_DETAILS.POD_ASSET_NO)) AS ASSET_CODE from PO_MSTR,PO_DETAILS Where POM_PO_NO= '" & Common.Parse(PO_No) & "'" &
        '                      " AND POM_B_COY_ID= '" & Common.Parse(com_id) & "' AND POM_B_COY_ID=POD_COY_ID " &
        '                      " AND POD_PO_NO=POM_PO_NO"

        '    strsql = strsql & " order by POD_PO_LINE"
        '    DS = objDb.FillDs(strsql)
        '    get_poDetail2 = DS
        'End Function

        'Public Function update_cancellation(ByVal ds As DataSet, ByRef CR_num As String, ByRef CancelBfToVendor As String) As Boolean
        '    Dim OBJGLB As New AppGlobals
        '    Dim strArray(0), strArray1(0) As String
        '    Dim strsql As String
        '    Dim prefix As String
        '    Dim qty As Integer
        '    Dim crstatus1 As Integer = CRStatus.newCR

        '    OBJGLB.GetLatestDocNo("CR", strArray, CR_num, prefix)
        '    strsql = "SELECT '*' FROM PO_CR_MSTR WHERE PCM_CR_NO= '" & Common.Parse(CR_num) & "' AND PCM_B_COY_ID= '" & Common.Parse(com_id) & "'"
        '    If objDb.Exist(strsql) Then
        '        Return False
        '    End If
        '    strsql = "insert into PO_CR_MSTR (PCM_CR_NO,PCM_B_COY_ID,PCM_S_COY_ID,PCM_PO_INDEX,PCM_CR_STATUS,PCM_REQ_BY,PCM_REQ_DATE,PCM_CR_REMARKS) values " & _
        '                "('" & Common.Parse(CR_num) & "','" & Common.Parse(com_id) & "','" & Common.Parse(ds.Tables(0).Rows(0)("vendor")) & "','" & Common.Parse(ds.Tables(0).Rows(0)("INDEX")) & "','" & Common.Parse(crstatus1) & "','" & Common.Parse(userid) & "'," & Common.ConvertDate(Date.Today) & ",'" & Common.Parse(ds.Tables(0).Rows(0)("REMARK")) & "') "

        '    Common.Insert2Ary(strArray, strsql)

        '    '-- str(2)' update po status 
        '    strsql = "update PO_MSTR SET POM_FULFILMENT='" & Common.parseNull(ds.Tables(1).Rows(0)("status")) & "' "

        '    If Common.parseNull(ds.Tables(1).Rows(0)("Cancelled")) <> "" Then
        '        strsql = strsql & " , POM_PO_STATUS= '" & Common.Parse(ds.Tables(1).Rows(0)("Cancelled")) & "'"
        '    End If

        '    'Michelle (6/2/2010) - POM_BUYER_ID might not be the login user
        '    strsql = strsql & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '    Common.Insert2Ary(strArray, strsql)


        '    '-- str (3) ' insert CR detail 
        '    Dim i As Integer
        '    For i = 0 To ds.Tables(2).Rows.Count - 1

        '        Dim line1 As Integer = ds.Tables(2).Rows(i)("lineno")
        '        If check_qty(ds.Tables(2).Rows(i)("qty_cancel"), ds.Tables(1).Rows(0)("po_no"), line1) Then
        '            ' check line QTY if QTY < 0 
        '            HttpContext.Current.Response.Redirect("errorpage.aspx?line=line1&PO_NO=" & Common.Parse(ds.Tables(1).Rows(0)("po_no")))
        '        End If

        '        strsql = "Insert into PO_CR_DETAILS (PCD_CR_NO,PCD_COY_ID,PCD_PO_LINE,PCD_CANCELLED_QTY,PCD_REMARKS) values" & _
        '                " ('" & Common.Parse(CR_num) & "','" & Common.Parse(com_id) & "','" & Common.Parse(ds.Tables(2).Rows(i)("lineno")) & "','" & Common.Parse(ds.Tables(2).Rows(i)("qty_cancel")) & "','" & Common.Parse(ds.Tables(2).Rows(i)("remarks")) & "') "
        '        Common.Insert2Ary(strArray, strsql)
        '        strsql = "  update PO_DETAILS SET POD_CANCELLED_QTY= POD_CANCELLED_QTY+" & Common.Parse(ds.Tables(2).Rows(i)("qty_cancel")) & " WHERE POD_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' AND POD_PO_LINE = '" & Common.Parse(ds.Tables(2).Rows(i)("lineno")) & "' And POD_COY_ID = '" & Common.Parse(com_id) & "'"
        '        Common.Insert2Ary(strArray, strsql)
        '    Next

        '    If objDb.BatchExecute(strArray) Then
        '        '//Send Mail
        '        Dim objMail As New Email
        '        Dim objMail1 As New Email
        '        Dim objMail2 As New Email

        '        If CancelBfToVendor = "" Then 'ie cancel after sending to Vendor
        '            objMail.sendNotification(EmailType.POCancellationRequest, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "")
        '            objMail = Nothing
        '            'Michelle (eBiz/134/08) - Send email to all AOs upon PO cancellation
        '            objMail1.sendNotification(EmailType.POCancellationRequestToAOBuyer, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "AO")
        '            objMail1 = Nothing
        '        Else
        '            objMail1.sendNotification(EmailType.POCancellationRequestToAO, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "AO")
        '            objMail1 = Nothing

        '        End If
        '        'Michelle (6/2/2010) - Check whether need to send email to Buyer (ie. PO cancel by BA)
        '        Dim objUser As New Users
        '        Dim IsBA As String = objUser.BAdminRole(userid, com_id)

        '        If HttpContext.Current.Session("Env") <> "FTN" Then
        '            IsBA = objUser.BAdminRole(userid, com_id)
        '        Else
        '            IsBA = False
        '        End If
        '        If IsBA Then
        '            objMail2.sendNotification(EmailType.POCancellationRequestToAOBuyer, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "BUYER")
        '            objMail2 = Nothing
        '        End If

        '        Dim objBCM As New BudgetControl
        '        If Common.parseNull(ds.Tables(1).Rows(0)("Cancelled")) = "" Then
        '            strsql = "Select COUNT('*') from PO_Details where POD_ORDERED_QTY > POD_CANCELLED_QTY AND POD_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' And POD_COY_ID = '" & Common.Parse(com_id) & "'"
        '            If objDb.GetVal(strsql) = 0 Then 'ALL ITEM US CANCELLE
        '                strsql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & Common.Parse(POStatus_new.Cancelled) & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '                Common.Insert2Ary(strArray1, strsql)
        '            End If

        '            strsql = "SELECT '*' FROM TotalQtyInvPaidByPO A,TotalQtyToBeDeliveredByPO B "
        '            strsql &= " WHERE A.IM_PO_INDEX = B.POM_PO_INDEX And A.Qty_Inv = B.QtyToBeDelivered And B.POM_PO_NO='" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "'"
        '            strsql &= " AND POM_B_COY_ID='" & Common.Parse(com_id) & "'"
        '            If objDb.Exist(strsql) = 1 Then
        '                strsql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & Common.Parse(POStatus_new.Close) & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '                Common.Insert2Ary(strArray1, strsql)
        '            End If
        '            If HttpContext.Current.Session("Env") <> "FTN" Then
        '                objBCM.BCMCalc("CR", CR_num, EnumBCMAction.CancelPO_AF, strArray1)
        '            End If
        '        Else
        '            If HttpContext.Current.Session("Env") <> "FTN" Then
        '                objBCM.BCMCalc("CR", CR_num, EnumBCMAction.CancelPO_BF, strArray1)
        '            End If
        '        End If
        '        If strArray1(0) <> "" Then
        '            objDb.BatchExecute(strArray1)
        '        End If
        '        objMail = Nothing
        '        objBCM = Nothing
        '        Return True
        '    End If
        'End Function

        'Public Function update_cancellation(ByVal ds As DataSet, ByRef CR_num As String, ByRef CancelBfToVendor As String, ByRef strMsg As String, Optional ByVal blnEnterpriseVersion As Boolean = True) As Boolean
        '    'Dim OBJGLB As New AppGlobals
        '    Dim strArray(0), strArray1(0) As String
        '    Dim strsql As String
        '    Dim prefix As String
        '    Dim qty As Integer
        '    Dim crstatus1 As Integer = CRStatus.newCR
        '    Dim intIncrementNo As Integer = 0
        '    Dim strNewCRNum As String = ""
        '    Dim blnProceed As Boolean
        '    'OBJGLB.GetLatestDocNo("CR", strArray, CR_num, prefix)
        '    'strsql = "SELECT '*' FROM PO_CR_MSTR WHERE PCM_CR_NO= '" & Common.Parse(CR_num) & "' AND PCM_B_COY_ID= '" & Common.Parse(com_id) & "'"
        '    'If objDb.Exist(strsql) Then
        '    '    Return False
        '    'End If
        '    strsql = " SET @DUPLICATE_CHK =''; SET @T_NO ='';UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(com_id) & "' AND CP_PARAM_TYPE = 'CR' "
        '    Common.Insert2Ary(strArray, strsql)

        '    intIncrementNo = 1
        '    CR_num = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
        '           & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
        '           & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & Common.Parse(com_id) & "' AND CP_PARAM_TYPE = 'CR' ORDER BY CP_PARAM_NAME DESC) ZZZ )  "

        '    strsql = "insert into PO_CR_MSTR (PCM_CR_NO,PCM_B_COY_ID,PCM_S_COY_ID,PCM_PO_INDEX,PCM_CR_STATUS,PCM_REQ_BY,PCM_REQ_DATE,PCM_CR_REMARKS) values " &
        '                "(" & CR_num & ",'" & Common.Parse(com_id) & "','" & Common.Parse(ds.Tables(0).Rows(0)("vendor")) & "','" & Common.Parse(ds.Tables(0).Rows(0)("INDEX")) & "','" & Common.Parse(crstatus1) & "','" & Common.Parse(userid) & "'," & Common.ConvertDate(Date.Today) & ",'" & Common.Parse(ds.Tables(0).Rows(0)("REMARK")) & "') "

        '    Common.Insert2Ary(strArray, strsql)

        '    '-- str(2)' update po status 
        '    strsql = "update PO_MSTR SET POM_FULFILMENT='" & Common.parseNull(ds.Tables(1).Rows(0)("status")) & "' "

        '    If Common.parseNull(ds.Tables(1).Rows(0)("Cancelled")) <> "" Then
        '        strsql = strsql & " , POM_PO_STATUS= '" & Common.Parse(ds.Tables(1).Rows(0)("Cancelled")) & "'"
        '    End If

        '    'Michelle (6/2/2010) - POM_BUYER_ID might not be the login user
        '    strsql = strsql & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '    Common.Insert2Ary(strArray, strsql)

        '    '-- str (3) ' insert CR detail 
        '    Dim i As Integer
        '    Dim dtQutS As DataTable

        '    For i = 0 To ds.Tables(2).Rows.Count - 1
        '        Dim line1 As Integer = ds.Tables(2).Rows(i)("lineno")
        '        dtQutS = check_qty(ds.Tables(2).Rows(i)("qty_cancel"), ds.Tables(1).Rows(0)("po_no"), line1)

        '        strsql = "SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF((POD_ORDERED_QTY - POD_CANCELLED_QTY - (POD_RECEIVED_QTY-POD_REJECTED_QTY))< " & ds.Tables(2).Rows(i)("qty_cancel") & ",'outs', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM PO_Details WHERE POD_PO_NO = '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' AND POD_COY_ID='" & Common.Parse(com_id) & "' AND POD_PO_LINE = '" & line1 & "'"
        '        Common.Insert2Ary(strArray, strsql)

        '        If dtQutS.Rows.Count > 0 Then
        '            If dtQutS.Rows(0)("Outs") < ds.Tables(2).Rows(i)("qty_cancel") Then
        '                strMsg = "This PO has already been cancelled."
        '                CR_num = ""
        '                Return False
        '            End If
        '        End If

        '        'If check_qty(ds.Tables(2).Rows(i)("qty_cancel"), ds.Tables(1).Rows(0)("po_no"), line1) Then 'Cancel Qty> Outstanding Qty
        '        '    ' check line QTY if QTY < 0 
        '        '    'HttpContext.Current.Response.Redirect("errorpage.aspx?line=line1&PO_NO=" & Common.Parse(ds.Tables(1).Rows(0)("po_no")))
        '        '    strMsg = "The Qty To Cancel is greater than the Outstd quantity."
        '        '    CR_num = ""
        '        '    Return False
        '        'End If

        '        strsql = "Insert into PO_CR_DETAILS (PCD_CR_NO,PCD_COY_ID,PCD_PO_LINE,PCD_CANCELLED_QTY,PCD_REMARKS) values" &
        '                " (" & CR_num & ",'" & Common.Parse(com_id) & "','" & Common.Parse(ds.Tables(2).Rows(i)("lineno")) & "','" & Common.Parse(ds.Tables(2).Rows(i)("qty_cancel")) & "','" & Common.Parse(ds.Tables(2).Rows(i)("remarks")) & "') "
        '        Common.Insert2Ary(strArray, strsql)
        '        strsql = "  update PO_DETAILS SET POD_CANCELLED_QTY= POD_CANCELLED_QTY+" & Common.Parse(ds.Tables(2).Rows(i)("qty_cancel")) & " WHERE POD_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' AND POD_PO_LINE = '" & Common.Parse(ds.Tables(2).Rows(i)("lineno")) & "' And POD_COY_ID = '" & Common.Parse(com_id) & "'"
        '        Common.Insert2Ary(strArray, strsql)
        '    Next
        '    strsql = " SET @T_NO = " & CR_num & "; "
        '    Common.Insert2Ary(strArray, strsql)

        '    strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & Common.Parse(com_id) & "' AND CP_PARAM_TYPE = 'CR' "
        '    Common.Insert2Ary(strArray, strsql)

        '    If objDb.BatchExecuteForDup(strArray, , strNewCRNum, "T_NO") Then
        '        CR_num = strNewCRNum
        '        If strNewCRNum <> "Generated" Then
        '            '//Send Mail
        '            Dim objMail As New Email
        '            Dim objMail1 As New Email
        '            Dim objMail2 As New Email

        '            If CancelBfToVendor = "" Then 'ie cancel after sending to Vendor
        '                objMail.sendNotification(EmailType.POCancellationRequest, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", , , blnEnterpriseVersion)
        '                objMail = Nothing
        '                'Michelle (eBiz/134/08) - Send email to all AOs upon PO cancellation
        '                objMail1.sendNotification(EmailType.POCancellationRequestToAOBuyer, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "AO", blnEnterpriseVersion)
        '                objMail1 = Nothing
        '            Else
        '                objMail1.sendNotification(EmailType.POCancellationRequestToAO, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "AO", blnEnterpriseVersion)
        '                objMail1 = Nothing

        '            End If
        '            'Michelle (6/2/2010) - Check whether need to send email to Buyer (ie. PO cancel by BA)
        '            Dim objUser As New Users
        '            Dim IsBA As String = objUser.BAdminRole(userid, com_id)

        '            'If HttpContext.Current.Session("Env") <> "FTN" Then
        '            If blnEnterpriseVersion = True Then
        '                IsBA = objUser.BAdminRole(userid, com_id)
        '            Else
        '                IsBA = False
        '            End If
        '            If IsBA Then
        '                objMail2.sendNotification(EmailType.POCancellationRequestToAOBuyer, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "BUYER", blnEnterpriseVersion)
        '                objMail2 = Nothing
        '            End If

        '            Dim objBCM As New BudgetControl
        '            Dim objPR As New PR
        '            If Common.parseNull(ds.Tables(1).Rows(0)("Cancelled")) = "" Then
        '                strsql = "Select COUNT('*') from PO_Details where POD_ORDERED_QTY > POD_CANCELLED_QTY AND POD_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' And POD_COY_ID = '" & Common.Parse(com_id) & "'"
        '                If objDb.GetVal(strsql) = 0 Then 'ALL ITEM US CANCELLE
        '                    strsql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & Common.Parse(POStatus_new.Cancelled) & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '                    Common.Insert2Ary(strArray1, strsql)
        '                End If

        '                'Michelle (13/1/2014) - Shouldn't set the PO status to 'Closed' as still need Vendor to acknowledge the cancellation, furthermore the following sql statement will have time out issue
        '                'strsql = "SELECT '*' FROM TotalQtyInvPaidByPO A,TotalQtyToBeDeliveredByPO B "
        '                'strsql &= " WHERE A.IM_PO_INDEX = B.POM_PO_INDEX And A.Qty_Inv = B.QtyToBeDelivered And B.POM_PO_NO='" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "'"
        '                'strsql &= " AND POM_B_COY_ID='" & Common.Parse(com_id) & "'"
        '                'If objDb.Exist(strsql) = 1 Then
        '                '    strsql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & Common.Parse(POStatus_new.Close) & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '                '    Common.Insert2Ary(strArray1, strsql)
        '                'End If

        '                'If HttpContext.Current.Session("Env") <> "FTN" Then
        '                If blnEnterpriseVersion = True Then
        '                    If objPR.checkBCM() > 0 Then
        '                        objBCM.BCMCalc("CR", strNewCRNum, EnumBCMAction.CancelPO_AF, strArray1)
        '                    End If
        '                End If
        '            Else
        '                'If HttpContext.Current.Session("Env") <> "FTN" Then
        '                If blnEnterpriseVersion = True Then
        '                    If objPR.checkBCM() > 0 Then
        '                        objBCM.BCMCalc("CR", strNewCRNum, EnumBCMAction.CancelPO_BF, strArray1)
        '                    End If
        '                End If
        '            End If
        '            If strArray1(0) <> "" Then
        '                objDb.BatchExecute(strArray1)
        '            End If
        '            objMail = Nothing
        '            objBCM = Nothing
        '            strMsg = CR_num & " has been successfully cancelled."
        '            Return True
        '        Else
        '            strMsg = "This PO has already been cancelled."
        '            CR_num = ""
        '            Return False
        '        End If

        '    Else
        '        strMsg = "Error in cancellation"
        '        CR_num = ""
        '        Return False
        '    End If

        'End Function
        'Public Function getVendorList(ByVal aryVendor As ArrayList, Optional ByVal sPOMode As String = "") As DataView 'Michelle (14/11/2010) 
        '    Dim drw As DataView
        '    'Dim aryVendorList As New ArrayList
        '    Dim strVendList As String = ""
        '    Dim i As Integer
        '    Dim objDB As New EAD.DBCom

        '    For i = 0 To aryVendor.Count - 1
        '        If strVendList = "" Then
        '            strVendList = "'" & aryVendor(i) & "'"
        '        Else
        '            strVendList &= ", '" & aryVendor(i) & "'"
        '        End If
        '    Next
        '    Dim strSQL As String
        '    If sPOMode <> "cc" Then
        '        strSQL = "SELECT CM_COY_ID, CM_COY_NAME " &
        '                             "FROM COMPANY_MSTR " &
        '                             "WHERE CM_COY_ID IN (" & strVendList & ")"
        '    Else
        '        strSQL = "SELECT DISTINCT CDM_S_COY_ID AS CM_COY_ID, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = CDM_S_COY_ID) AS CM_COY_NAME FROM CONTRACT_DIST_MSTR, CONTRACT_DIST_ITEMS "
        '        strSQL &= "WHERE CDM_GROUP_INDEX = CDI_GROUP_INDEX AND CDM_S_COY_ID IN (" & strVendList & ") "
        '    End If

        '    drw = objDB.GetView(strSQL)
        '    Return drw
        'End Function

        'Function AssetGroupMstr() As Boolean
        '    Dim strsql = " SELECT IFNULL(AG_GROUP, '') AS AG_GROUP FROM ASSET_GROUP WHERE AG_GROUP_TYPE = 'A' AND AG_STATUS = 'A' " _
        '            & " AND AG_COY_ID = '" & companyID & "' "

        '    Dim AssetGroup As String = objDb.GetVal(strsql)
        '    If AssetGroup = "" Then
        '        AssetGroupMstr = False
        '    Else
        '        AssetGroupMstr = True
        '    End If

        'End Function

        '' ''Public Function getcboAsset(Optional ByVal strWhere As String = Nothing) As DataView

        '' ''    Dim strSql As String
        '' ''    Dim drw As DataView
        '' ''    Dim objDB As New EAD.DBCom
        '' ''    strSql = "SELECT IFNULL(AG_GROUP, '') AS AG_GROUP, CONCAT(CONCAT(IFNULL(AG_GROUP, ''),' : '), IFNULL(AG_GROUP_DESC, '')) AS AG_GROUP_DESC " & _
        '' ''             "FROM ASSET_GROUP " & _
        '' ''             "WHERE AG_COY_ID ='" & companyID & "'"

        '' ''    If strWhere <> "" Then
        '' ''        strSql = strSql & " AND AG_GROUP = '" & strWhere & "' "
        '' ''    End If

        '' ''    strSql = strSql & " ORDER BY AG_GROUP"
        '' ''    drw = objDB.GetView(strSql)
        '' ''    Return drw
        '' ''End Function

        'Function check_qty(ByVal cancelitem As Integer, ByVal po_no As String, ByVal line As Integer) As DataTable

        '    'Dim strsql As String = "select count(*)AS counter from PO_DETAILS WHERE (POD_ORDERED_QTY - POD_CANCELLED_QTY - (POD_RECEIVED_QTY-POD_REJECTED_QTY))>=" & Common.Parse(cancelitem) & " " & _
        '    '                        " and POD_PO_LINE = '" & Common.Parse(line) & "' and POD_PO_NO = '" & Common.Parse(po_no) & "' AND POD_COY_ID= '" & Common.Parse(com_id) & "' "
        '    Dim strsql = "SELECT POD_ORDERED_QTY - POD_CANCELLED_QTY - (POD_RECEIVED_QTY-POD_REJECTED_QTY) AS Outs " _
        '            & "FROM PO_DETAILS " _
        '            & "WHERE POD_PO_NO = '" & Common.Parse(po_no) & "' AND POD_PO_LINE = " & Common.Parse(line) & " AND POD_COY_ID='" & Common.Parse(com_id) & "' "
        '    Return objDb.FillDs(strsql).Tables(0)
        '    'Dim tDS As DataSet = objDb.FillDs(strsql)
        '    'If tDS.Tables(0).Rows.Count > 0 Then
        '    '    If tDS.Tables(0).Rows(0).Item("counter").ToString.Trim > 0 Then
        '    '        Return 0 ' cancel QTY < Outstading QTY  
        '    '    Else
        '    '        Return 1 ' Camcel QTY > Outstanding QTY 
        '    '    End If
        '    'End If


        'End Function

        'Public Function delete_Attachment(ByVal strDocNo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
        '    strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "

        '    ds = objDb.FillDs(strsql)

        '    ds = Nothing
        'End Function

        'Public Function delete_Attachment_Temp(ByVal intIndex As Integer, Optional ByVal strConnStr As String = Nothing)
        '    Dim strsql As String
        '    If strConnStr Is Nothing Then
        '        objDb = New EAD.DBCom
        '    Else
        '        objDb = New EAD.DBCom(strConnStr)
        '    End If

        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
        '    strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
        '    objDb.Execute(strsql)
        'End Function

        'Public Function insert_Attachment(ByVal strDocNo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    ' insert COMPANY_DOC_ATTACHMENT table
        '    strsql = "INSERT INTO company_doc_attachment_temp(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
        '    strsql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment "
        '    strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "
        '    ds = objDb.FillDs(strsql)

        '    ds = Nothing
        'End Function

        'Public Function getPOTempAttach(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E") As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT_TEMP "
        '    strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "
        '    strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "
        '    ds = objDb.FillDs(strsql)
        '    getPOTempAttach = ds
        'End Function

        ''Jules 2019.03.21
        'Public Function getPOAttachment(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E") As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "
        '    strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "
        '    ds = objDb.FillDs(strsql)
        '    getPOAttachment = ds
        'End Function

        ''Jules 2019.03.21
        'Public Function delete_Attachment2(ByVal strDocNo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "

        '    ds = objDb.FillDs(strsql)

        '    ds = Nothing
        'End Function

        ''Jules 2019.03.26
        'Public Function delete_Attachment_byIndex(ByVal intIndex As Integer, Optional ByVal strConnStr As String = Nothing)
        '    Dim strsql As String
        '    If strConnStr Is Nothing Then
        '        objDb = New EAD.DBCom
        '    Else
        '        objDb = New EAD.DBCom(strConnStr)
        '    End If

        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
        '    objDb.Execute(strsql)
        'End Function

        'Public Function GetQuotationPrice(ByVal sRFQ As String, ByVal sQuo As String, ByVal pComId As String, ByVal pItemNo As String, Optional ByVal vCompanyid As String = "", Optional ByVal vCode As String = "", Optional ByVal vCode2 As String = "") As DataSet
        '    Dim strSql As String
        '    Dim objDB As New EAD.DBCom

        '    If sRFQ Is Nothing Then
        '        sRFQ = ""
        '    End If

        '    strSql = "SELECT rm_rfq_id, rm_rfq_no, rrd_line_no, rrd_unit_price "
        '    strSql &= "FROM rfq_mstr "
        '    strSql &= "INNER JOIN rfq_replies_mstr ON rm_rfq_id = rrm_rfq_id "
        '    strSql &= "INNER JOIN rfq_replies_detail ON rrd_rfq_id = rrm_rfq_id  "
        '    strSql &= "WHERE rm_coy_id = '" & pComId & "' "
        '    strSql &= "AND rrm_actual_quot_num = '" & sQuo & "' "
        '    ' Modified by craven to get the selected vendor company


        '    If vCode = "&nbsp;" Then
        '        strSql &= " AND rrd_product_desc='" & Common.Parse(vCode2) & "' "
        '    ElseIf vCode <> "" Then
        '        strSql &= " AND rrd_vendor_item_code='" & vCode & "' "
        '    End If

        '    If sRFQ <> "" Then
        '        strSql &= " AND rm_rfq_no = '" & sRFQ & "' "
        '    End If

        '    If vCompanyid <> "" Then
        '        strSql &= " AND RRD_V_COY_ID='" & vCompanyid & "'"
        '    End If

        '    Return objDB.FillDs(strSql)
        'End Function

        Public Function insertPO(ByVal userid As String, ByVal companyid As String, ByVal dsPO As DataSet, ByRef strPONo As String, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal FreeForm As Boolean = False) As Integer

            Dim strPrefix, strName, strPhone, strFax As String
            Dim strsql, strTermFile As String
            Dim strAryQuery(0) As String
            Dim strAryAdd(8) As String
            Dim i, j As Integer
            Dim blnAdd As Boolean = False
            Dim strDeptIndex As String = ""
            Dim objGlobal As New AppGlobals
            Dim intIncrementNo As Integer = 0

            strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyid & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(strAryQuery, strsql)


            intIncrementNo = 1

            strPONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & companyid & "' AND CP_PARAM_TYPE = 'PO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            strPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & companyid & "' AND CP_PARAM_TYPE = 'PO') "

            strsql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = " & strPONo & " "
            strsql &= "AND POM_B_COY_ID = '" & companyid & "' "

            If objDb.Exist(strsql) > 0 Then
                insertPO = WheelMsgNum.Duplicate
                Exit Function
            End If

            ' to check whether vendor company is inactive
            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
            strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
            If objDb.Exist(strsql) > 0 Then
                insertPO = -1
                Exit Function
            End If

            ' to check whether vendor company is being deleted
            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
            strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
            If objDb.Exist(strsql) > 0 Then
                insertPO = -2
                Exit Function
            End If

            ' get dept index
            'Michelle (26/4/2011) - To cater for those without department
            strsql = "SELECT CDM_DEPT_INDEX, UM_USER_NAME, UM_FAX_NO, UM_TEL_NO FROM USER_MSTR "
            strsql &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
            strsql &= "AND UM_COY_ID = CDM_COY_ID "
            strsql &= "WHERE UM_USER_ID = '" & userid & "' "
            strsql &= "AND UM_COY_ID = '" & companyid & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                strDeptIndex = Common.parseNull(tDS.Tables(0).Rows(q).Item("CDM_DEPT_INDEX"))
                strName = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_USER_NAME"))
                strPhone = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_TEL_NO"))
                strFax = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_FAX_NO"))
            Next

            'Michelle (27/12/2010) - To store the Term & Condition
            Dim objWheelFile As New FileManagement
            strTermFile = objWheelFile.copyTermCondToPO(companyid, strPONo)

            ' PO_MSTR table
            strsql = "INSERT INTO PO_MSTR (POM_PO_NO, POM_B_COY_ID, POM_BUYER_ID, POM_BUYER_NAME, POM_BUYER_PHONE, POM_BUYER_FAX, "
            strsql &= "POM_CREATED_DATE, POM_CREATED_BY, POM_STATUS_CHANGED_BY, POM_STATUS_CHANGED_ON, "
            strsql &= "POM_S_COY_ID, POM_S_ATTN, POM_S_COY_NAME, POM_S_ADDR_LINE1, "
            strsql &= "POM_S_ADDR_LINE2, POM_S_ADDR_LINE3, POM_S_POSTCODE, POM_S_CITY, POM_S_STATE, "
            strsql &= "POM_S_COUNTRY, POM_S_PHONE, POM_S_FAX, POM_S_EMAIL, "
            strsql &= "POM_PAYMENT_METHOD, POM_SHIPMENT_TERM, POM_SHIPMENT_MODE, POM_CURRENCY_CODE, "
            strsql &= "POM_EXCHANGE_RATE, POM_PAYMENT_TERM, POM_SHIP_VIA, POM_BILLING_METHOD, POM_PO_TYPE, POM_INTERNAL_REMARK, "
            strsql &= "POM_EXTERNAL_REMARK, POM_PO_STATUS, POM_FULFILMENT, POM_PO_INDEX, POM_ARCHIVE_IND, "
            strsql &= "POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SHIP_AMT, POM_PO_PREFIX, POM_B_ADDR_CODE, "
            strsql &= "POM_B_ADDR_LINE1, POM_B_ADDR_LINE2, POM_B_ADDR_LINE3, POM_B_POSTCODE, "
            strsql &= "POM_B_STATE, POM_B_CITY, POM_B_COUNTRY, "
            strsql &= "POM_DUP_FROM, POM_EXTERNAL_IND, POM_REFERENCE_NO, "
            strsql &= "POM_PO_COST, POM_RFQ_INDEX, POM_DEPT_INDEX, POM_QUOTATION_NO, POM_TERMANDCOND, POM_URGENT) SELECT "
            strsql &= "" & strPONo & ", "
            strsql &= "'" & companyid & "', "
            strsql &= "'" & userid & "', "
            strsql &= "'" & Common.Parse(strName) & "', "
            strsql &= "'" & Common.Parse(strPhone) & "', "
            strsql &= "'" & Common.Parse(strFax) & "', " 'BUYER_FAX
            strsql &= Common.ConvertDate(Now) & ", "  'CREATED_DATE
            strsql &= "'" & userid & "', "
            strsql &= "'" & userid & "', " 'STATUS_CHANGED_BY
            strsql &= Common.ConvertDate(Now) & ", " 'STATUS_CHANGED_ON
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "', " 'S_COY_ID
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("Attn")) & "', " 'S_ATTN
            strsql &= "CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, "
            strsql &= "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "
            'strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', " ' FREIGHT_TERMS
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentType")) & "', " ' PAYMENT_TYPE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentTerm")) & "', " ' SHIPMENT_TERM
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentMode")) & "', " ' SHIPMENT_MODE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("CurrencyCode")) & "', " ' CURRENCY_CODE
            strsql &= Common.Parse(dsPO.Tables(0).Rows(0)("ExchangeRate")) & ", "  ' EXCHANGE_RATE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentTerm")) & "', " ' PAYMENT_TERM
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipVia")) & "', " ' SHIP_VIA
            'zulham Aug 23, 2013
            If Not FreeForm Then
                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillingMethod")) & "', " ' Billing_Method
                strsql &= "' ', "
            Else
                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillingMethod")) & "', " ' Billing_Method
                strsql &= "'Y', "
            End If
            'End
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("InternalRemark")) & "', " ' INTERNAL_REMARK
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ExternalRemark")) & "', " 'EXTERNAL_REMARK
            strsql &= "'" & POStatus_new.Draft & "', " ' PO_STATUS
            strsql &= "'" & Fulfilment.null & "', "
            strsql &= "NULL, '', "   ' PO_INDEX, ARCHIVE_IND
            strsql &= "'" & Common.Parse(Common.parseNull(dsPO.Tables(0).Rows(0)("PrintCustom"))) & "', " 'PRINT_CUSTOM_FIELDS
            strsql &= "'" & Common.Parse(Common.parseNull(dsPO.Tables(0).Rows(0)("PrintRemark"))) & "', " 'PRINT_REMARK
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipAmt")) & "', " 'POM_SHIP_AMT
            '' ''strsql &= "'" & Common.Parse(strPrefix) & "', "
            strsql &= "" & strPrefix & ", "
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCode")) & "', " ' B_ADDR_CODE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine1")) & "', " ' B_ADDR_LINE1
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine2")) & "', " ' B_ADDR_LINE2
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine3")) & "', " ' B_ADDR_LINE3
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrPostCode")) & "', " ' B_POSTCODE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrState")) & "', " ' B_STATE
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCity")) & "', " ' B_CITY
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCountry")) & "', " ' B_COUNTRY
            strsql &= "'', '', '', " ' DUP_FROM, EXTERNAL_IND, REFERENCE_NO
            strsql &= dsPO.Tables(0).Rows(0)("POCost") & ", " ' PO_COST
            strsql &= dsPO.Tables(0).Rows(0)("RfqIndex") & ", " ' RFQ_INDEX

            If strDeptIndex = "" Then ' PRM_DEPT_INDEX
                strsql &= "NULL, "
            Else
                strsql &= strDeptIndex & ", "
            End If
            strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("QuoNo")) & "' " ' QUOTATION_NO
            strsql &= ", '" & strTermFile & "', '" & Common.Parse(dsPO.Tables(0).Rows(0)("Urgent")) & "' " 'Term & Condition
            strsql &= "FROM COMPANY_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
            strsql &= " AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "

            Common.Insert2Ary(strAryQuery, strsql)

            ' PO_DETAILS table

            Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            For i = 0 To dsPO.Tables(1).Rows.Count - 1
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - Include Gst Tax Code
                strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
                strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
                strsql &= "POD_ETD, POD_REMARK, POD_GST,POD_TAX_VALUE, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
                strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
                strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, POD_GST_RATE, POD_GST_INPUT_TAX_CODE "

                'Jules 2018.05.07 - PAMB Scrum 2
                If companyid.ToString.ToUpper = "PAMB" Then
                    If blnEnterpriseVersion = True Then
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO, POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE) SELECT "
                    Else
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE) SELECT "
                    End If
                Else
                    If blnEnterpriseVersion = True Then
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO ) SELECT "
                    Else
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE) SELECT "
                    End If
                End If
                'End modification.

                strsql &= "" & strPONo & ", " ' PR_No
                strsql &= "'" & companyid & "', " ' COY_ID
                strsql &= Common.Parse(dsPO.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPO.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPO.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPO.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Remark")) & "', " ' REMARK

                If Not IsDBNull(dsPO.Tables(1).Rows(i)("GST")) Then strsql &= dsPO.Tables(1).Rows(i)("GST") & ", " Else strsql &= "'0.00', " 'Tax
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GSTTaxAmount")) & "', " 'GST Tax Value

                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Source")) & "', " ' SOURCE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SelectedGST")) & "', " ' POD_GST_RATE
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GSTTaxCode")) & "', " ' POD_GST_INPUT_TAX_CODE

                strsql &= IIf(dsPO.Tables(1).Rows(i)("MOQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MOQ")) & ", "
                strsql &= IIf(dsPO.Tables(1).Rows(i)("MPQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MPQ")) & ", "

                If Not IsDBNull(dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE")) Then
                    strsql &= dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE") & ", "
                Else
                    strsql &= "NULL, "
                End If

                If Common.parseNull(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & " " ' PRD_CD_GROUP_INDEX
                End If
                ' _Yap: For Interface
                strsql &= ",'" & Common.parseNull(dsPO.Tables(1).Rows(i)("ItemCode")) & "' " ' PRD_B_ITEM_CODE

                If blnEnterpriseVersion = True Then
                    strsql &= ", " & dsPO.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                    ' _Yap: For Interface
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                    If Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) <> "" Then
                        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                    Else
                        strsql &= "null, " ' ACCT_INDEX
                    End If
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AssetGroup")) & "', "
                    strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AssetGroupNo")) & "' "
                End If

                'Jules 2018.05.07 - PAMB Scrum 2 & 3
                If companyid.ToString.ToUpper = "PAMB" Then
                    strsql &= ",'" & Common.Parse(dsPO.Tables(1).Rows(i)("Gift")) & "'"
                    strsql &= ",'" & Common.Parse(dsPO.Tables(1).Rows(i)("FundType")) & "'"
                    strsql &= ",'" & Common.Parse(dsPO.Tables(1).Rows(i)("PersonCode")) & "'"
                    strsql &= ",'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProjectCode")) & "' "
                End If
                'End modification.

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & companyid & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPO.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

            Next

            If modePR = True Then
                ' PR_CUSTOM_FIELD_MSTR table
                For i = 0 To dsPO.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    'Michelle (16/11/2010) - To cater for MYSQL
                    strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                    strsql &= dsPO.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPO.Tables(2).Rows(i)("FieldName")) & "', 'PO'" ' FIELD_NAME
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' PR_CUSTOM_FIELD_DETAILS table
                For i = 0 To dsPO.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    'Michelle (16/11/2010) - To cater for MYSQL
                    strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                    strsql &= dsPO.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPO.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPO.Tables(3).Rows(i)("FieldValue")) & "', 'PO' " ' FIELD_VALUE
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If

            If dsPO.Tables(0).Rows(0)("RfqIndex") = "NULL" Then
            Else
                strsql = "UPDATE RFQ_MSTR SET RM_Status = '2' "
                strsql &= "WHERE RM_RFQ_ID = " & dsPO.Tables(0).Rows(0)("RfqIndex")
                Common.Insert2Ary(strAryQuery, strsql)
            End If

            'Jules 2019.03.21 - To avoid missing attachment issue.
            ' delete COMPANY_DOC_ATTACHMENT table
            'strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            'strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = " & strPONo & " "
            'Common.Insert2Ary(strAryQuery, strsql)

            '' update COMPANY_DOC_ATTACHMENT_TEMP table
            'strsql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
            'strsql &= "CDA_DOC_NO = " & strPONo & " "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            '' insert COMPANY_DOC_ATTACHMENT table
            'strsql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
            'strsql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = " & strPONo & " "
            'Common.Insert2Ary(strAryQuery, strsql)

            '' delete COMPANY_DOC_ATTACHMENT_TEMP table
            'strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = " & strPONo & " "
            'Common.Insert2Ary(strAryQuery, strsql)

            ' update COMPANY_DOC_ATTACHMENT table
            strsql = "UPDATE COMPANY_DOC_ATTACHMENT SET "
            strsql &= "CDA_DOC_NO = " & strPONo & " "
            strsql &= "WHERE CDA_COY_ID = '" & companyid & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            Common.Insert2Ary(strAryQuery, strsql)
            'End modification.

            strsql = " SET @T_NO = " & strPONo & "; "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyid & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(strAryQuery, strsql)

            'If objDb.BatchExecute(strAryQuery) Then
            Dim strTPONo As String = ""
            If objDb.BatchExecuteNew(strAryQuery, , strTPONo, "T_NO") Then
                strPONo = strTPONo
                insertPO = WheelMsgNum.Save
            Else
                insertPO = WheelMsgNum.NotSave
            End If
        End Function

        Public Function getPOApprFlow(ByVal userID As String, ByVal companyID As String, ByVal isFTN As Boolean, Optional ByRef strDept As String = "") As DataTable
            Dim strsql As String
            Dim dt As DataTable

            If isFTN Then
                strsql = "SELECT AGM_GRP_INDEX AS GrpIndex, ISNULL(AGA_AO,'') AS AO, ISNULL(AGA_A_AO,'') AS AAO, CAST(ISNULL(AGA_SEQ, 1) AS CHAR) AS SEQ, ISNULL(AGM_TYPE, '') AS Type, ISNULL(AGA_RELIEF_IND, '') AS Relief  FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
                strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGB_BUYER = '" & userID & "' AND AGM_TYPE = 'PO'"
            Else
                strsql = "SELECT AGM_GRP_INDEX, AGA_AO, AGA_A_AO FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
                strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGB_BUYER = '" & userID & "' "
            End If

            'Jules 2018.11.13 - Disregard department checking
            'If strDept <> "" Then
            '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
            'End If
            'End modification.

            dt = objDb.FillDt(strsql)
            getPOApprFlow = dt
        End Function

        'Public Function get_TaxPerc(ByVal pProdCode As String, ByVal pCompID As String, ByRef pTaxId As String) As String
        '    Dim strsql As String, strPerc As String = ""
        '    Dim ds As New DataSet

        '    'Chk whether to get the tax from from the Prefer Vendor
        '    strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '    strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '    strsql &= "PM_PREFER_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '    strsql &= "PM_PREFER_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '    ds = objDb.FillDs(strsql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '        pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '    End If


        '    If strPerc = "" Then 'Chk whether to get the tax from from the 1st Vendor
        '        strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '        strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '        strsql &= "PM_1ST_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '        strsql &= "PM_1ST_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '            pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '        End If
        '    End If

        '    If strPerc = "" Then 'Chk whether to get the tax from from the 2nd Vendor
        '        strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '        strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '        strsql &= "PM_2ND_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '        strsql &= "PM_2ND_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '            pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '        End If
        '    End If

        '    If strPerc = "" Then 'Chk whether to get the tax from from the 3rd Vendor
        '        strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '        strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '        strsql &= "PM_3RD_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '        strsql &= "PM_3RD_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '            pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '        End If
        '    End If

        '    If strPerc = "" Then
        '        Return "0"
        '    Else
        '        Return strPerc
        '    End If
        'End Function

        'Public Function get_EDDPerc(ByVal pProdCode As String, ByVal pCompID As String) As String
        '    Dim strsql As String, iEDD As String = ""
        '    Dim ds As New DataSet

        '    Dim INDEX As String
        '    strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
        '    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.parseNull(pProdCode) & "'"

        '    INDEX = objDb.GetVal(strsql)

        '    'Chk whether to get the tax from from the Prefer Vendor
        '    strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '    strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '    strsql &= "PM_PREFER_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '    strsql &= "PV_VENDOR_TYPE = 'P' AND "
        '    strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '    ds = objDb.FillDs(strsql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '    End If


        '    If iEDD = "" Then 'Chk whether to get the tax from from the 1st Vendor
        '        strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '        strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '        strsql &= "PM_1ST_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '        strsql &= "PV_VENDOR_TYPE = '1' AND "
        '        strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '        End If

        '    End If

        '    If iEDD = "" Then 'Chk whether to get the tax from from the 2nd Vendor
        '        strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '        strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '        strsql &= "PM_2ND_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '        strsql &= "PV_VENDOR_TYPE = '2' AND "
        '        strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '        End If
        '    End If

        '    If iEDD = "" Then 'Chk whether to get the tax from from the 3rd Vendor
        '        strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '        strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '        strsql &= "PM_3RD_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '        strsql &= "PV_VENDOR_TYPE = '3' AND "
        '        strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '        End If
        '    End If

        '    If iEDD = "" Then
        '        Return "0"
        '    Else
        '        Return iEDD
        '    End If
        'End Function

        'Public Function updatePODIndex(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer, Optional ByVal FFPO As Boolean = False, Optional ByVal itemDesc As String = "") As String
        '    Dim strsql As String

        '    If Not FFPO Then
        '        strsql = "UPDATE PO_DETAILS "
        '        strsql &= "SET POD_PO_LINE = '" & sProductIndex & "' "
        '        strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND POD_PO_LINE >= '" & sProductIndex & "' "
        '        strsql &= "AND POD_PRODUCT_CODE = '" & sProductCode & "' "
        '        strsql &= "ORDER BY POD_PO_LINE "
        '        strsql &= "LIMIT 1 ; "
        '    Else
        '        strsql = "UPDATE PO_DETAILS "
        '        strsql &= "SET POD_PO_LINE = '" & sProductIndex & "' "
        '        strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND POD_PO_LINE >= '" & sProductIndex & "' "
        '        'strsql &= "AND POD_PRODUCT_Desc = '" & itemDesc & "' "
        '        strsql &= "ORDER BY POD_PO_LINE "
        '        strsql &= "LIMIT 1 ; "
        '    End If

        '    updatePODIndex = strsql
        'End Function

        'Public Function deletePOItemSQL(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer, Optional ByVal FFPO As Boolean = False) As String
        '    Dim strsql As String
        '    ' delete from PR_DETAILS table
        '    If FFPO = False Then
        '        strsql = "DELETE FROM PO_DETAILS "
        '        strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND POD_PO_LINE = '" & sProductIndex & "' "
        '        strsql &= "AND POD_PRODUCT_CODE = '" & sProductCode & "'; "
        '    Else
        '        strsql = "DELETE FROM PO_DETAILS "
        '        strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND POD_PO_LINE = '" & sProductIndex & "' "
        '    End If
        '    '' delete from PR_CUSTOM_FIELD_DETAILS table
        '    'strsql &= "DELETE FROM PR_CUSTOM_FIELD_DETAILS "
        '    'strsql &= "WHERE PCD_PR_LINE = " & sProductCode & " "
        '    'strsql &= "AND PCD_PR_INDEX IN "
        '    'strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
        '    'strsql &= "WHERE POM_PO_NO = '" & strPO & "'); "

        '    deletePOItemSQL = strsql
        'End Function

        'Public Function updatePOHeaderSQL(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer, Optional ByVal FFPO As Boolean = False) As String
        '    Dim strsql As String
        '    If FFPO = False Then
        '        strsql = "UPDATE PO_MSTR A, PO_DETAILS B "
        '        strsql &= "SET A.POM_PO_COST = A.POM_PO_COST - (B.POD_ORDERED_QTY * B.POD_UNIT_COST) "
        '        strsql &= "WHERE A.POM_PO_NO = B.POD_PO_NO AND A.POM_B_COY_ID = B.POD_COY_ID "
        '        strsql &= "AND B.POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND B.POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND B.POD_PO_LINE = '" & sProductIndex & "' "
        '        strsql &= "AND B.POD_PRODUCT_CODE = '" & sProductCode & "'; "
        '    Else

        '    End If
        '    updatePOHeaderSQL = strsql
        'End Function

        'Function DupPOItem(ByVal strNewPO As String, ByVal intPOLine As Int32) As String
        '    Dim strSQL, strMsg As String
        '    Dim strAryQuery(0) As String

        '    strSQL = "UPDATE PO_DETAILS SET POD_PO_LINE=POD_PO_LINE+1 WHERE POD_PO_NO='" &
        '    strNewPO & "' AND POD_COY_ID='" & companyID & "' AND POD_PO_LINE>=" & intPOLine + 1 & " ORDER BY POD_PO_LINE DESC "
        '    Common.Insert2Ary(strAryQuery, strSQL)

        '    strSQL = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
        '    strSQL &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
        '    strSQL &= "POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
        '    strSQL &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
        '    strSQL &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, "
        '    strSQL &= "POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, "
        '    strSQL &= "POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX ) SELECT "

        '    strSQL &= " POD_PO_NO, POD_COY_ID, " & intPOLine & "+1, POD_PRODUCT_CODE, " &
        '    " POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, " &
        '    " POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, " &
        '    " POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, " &
        '    " POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, " &
        '    " POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, " &
        '    " POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX " &
        '    " FROM PO_DETAILS WHERE POD_PO_NO= '" & strNewPO & "' " &
        '    " AND POD_COY_ID='" & companyID & "' AND POD_PO_LINE=" & intPOLine
        '    Common.Insert2Ary(strAryQuery, strSQL)

        '    If objDb.BatchExecute(strAryQuery) Then
        '        DupPOItem = "1"
        '    Else
        '        DupPOItem = "2"
        '    End If
        'End Function

        'Public Function get_PO(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer, Optional ByVal FFPO As Boolean = False) As Boolean

        '    Dim strsql As String

        '    If FFPO = False Then
        '        strsql = "select * from PO_DETAILS "
        '        strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND POD_PO_LINE = '" & sProductIndex & "' "
        '        strsql &= "AND POD_PRODUCT_CODE = '" & sProductCode & "'; "
        '    Else
        '        strsql = "select * from PO_DETAILS "
        '        strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '        strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '        strsql &= "AND POD_PO_LINE = '" & sProductIndex & "' "
        '    End If

        '    Dim tDS As DataSet = objDb.FillDs(strsql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        Return True
        '    End If

        'End Function

        'Public Sub getVendorViaPO(ByVal sPONo As String, ByRef sCoyId As String)
        '    Dim tSQL As String, tDS As DataSet
        '    tSQL = "SELECT POM_S_COY_ID FROM po_mstr WHERE POM_PO_NO = '" & sPONo & "'"
        '    tDS = objDb.FillDs(tSQL)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        sCoyId = tDS.Tables(0).Rows(0).Item("POM_S_COY_ID")
        '    End If
        '    tDS = Nothing
        'End Sub

        'Public Sub getVendorViaProductCode(ByVal sProductCode As String, ByRef sP As String, ByRef s1 As String, ByRef s2 As String, ByRef s3 As String)
        '    Dim tSQL As String, tDS As DataSet
        '    tSQL = "SELECT IFNULL(PM_PREFER_S_COY_ID,'') AS PM_PREFER_S_COY_ID, IFNULL(PM_1ST_S_COY_ID,'') AS PM_1ST_S_COY_ID, IFNULL(PM_2ND_S_COY_ID,'') AS PM_2ND_S_COY_ID, IFNULL(PM_3RD_S_COY_ID,'') AS PM_3RD_S_COY_ID FROM product_mstr WHERE PM_PRODUCT_CODE = '" & sProductCode & "'"
        '    tDS = objDb.FillDs(tSQL)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        sP = tDS.Tables(0).Rows(0).Item("PM_PREFER_S_COY_ID")
        '        s1 = tDS.Tables(0).Rows(0).Item("PM_1ST_S_COY_ID")
        '        s2 = tDS.Tables(0).Rows(0).Item("PM_2ND_S_COY_ID")
        '        s3 = tDS.Tables(0).Rows(0).Item("PM_3RD_S_COY_ID")
        '    End If
        '    tDS = Nothing
        'End Sub

        'Public Function deletePO(ByVal strPO As String)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    '' delete from PR_DETAILS table
        '    'strsql = "DELETE FROM PO_DETAILS "
        '    'strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '    'strsql &= "AND POD_COY_ID = '" & companyID & "' "
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    ' delete from PR_MST table
        '    strsql = "UPDATE PO_MSTR SET POM_PO_STATUS = " & POStatus_new.Void & ", POM_STATUS_CHANGED_BY = '" & userid & "', POM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now)
        '    strsql &= " WHERE POM_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & companyID & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function get_PO_Quo(ByVal strPO As String) As DataSet

        '    Dim strsql As String

        '    strsql = "select * from PO_MSTR "
        '    strsql &= "WHERE POM_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & companyID & "'; "

        '    Return objDb.FillDs(strsql)

        'End Function

        Public Sub updatePO(ByVal userID As String, ByVal companyID As String, ByVal dsPO As DataSet, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            strsql = "UPDATE PO_MSTR SET "
            strsql &= "POM_S_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "', "
            strsql &= "POM_S_ATTN = '" & Common.Parse(dsPO.Tables(0).Rows(0)("Attn")) & "', "
            'strsql &= "POM_FREIGHT_TERMS = '" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', "
            strsql &= "POM_PAYMENT_METHOD = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentType")) & "', "
            strsql &= "POM_SHIPMENT_TERM = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentTerm")) & "', "
            strsql &= "POM_SHIPMENT_MODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentMode")) & "', "
            strsql &= "POM_EXCHANGE_RATE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ExchangeRate")) & "', "
            strsql &= "POM_PAYMENT_TERM = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentTerm")) & "', "
            strsql &= "POM_SHIP_VIA = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipVia")) & "', "
            strsql &= "POM_INTERNAL_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("InternalRemark")) & "', "
            strsql &= "POM_EXTERNAL_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ExternalRemark")) & "', "
            strsql &= "POM_PRINT_CUSTOM_FIELDS = '" & Common.Parse(Common.parseNull(dsPO.Tables(0).Rows(0)("PrintCustom"))) & "', "
            strsql &= "POM_PRINT_REMARK = '" & Common.Parse(Common.parseNull(dsPO.Tables(0).Rows(0)("PrintRemark"))) & "', "
            strsql &= "POM_SHIP_AMT = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipAmt")) & "', "
            strsql &= "POM_B_ADDR_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCode")) & "', "
            strsql &= "POM_B_ADDR_LINE1 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine1")) & "', "
            strsql &= "POM_B_ADDR_LINE2 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine2")) & "', "
            strsql &= "POM_B_ADDR_LINE3 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine3")) & "', "
            strsql &= "POM_B_POSTCODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrPostCode")) & "', "
            strsql &= "POM_B_STATE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrState")) & "', "
            strsql &= "POM_B_CITY = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCity")) & "', "
            strsql &= "POM_B_COUNTRY = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCountry")) & "', "
            strsql &= "POM_URGENT = '" & Common.Parse(dsPO.Tables(0).Rows(0)("Urgent")) & "', "
            'Zulham Dec 13, 2013 - Comp's Details doesnt get updated
            Dim compName = objDb.GetVal("SELECT cm_coy_name FROM company_mstr WHERE cm_coy_id = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "'")
            strsql &= "POM_S_COY_NAME = '" & compName & "', "
            strsql &= "POM_CURRENCY_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("CurrencyCode")) & "', "
            Dim ds As New DataSet : ds = objDb.FillDs("select * from company_MSTR where CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' ")
            strsql &= "POM_S_ADDR_LINE1 = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_ADDR_LINE1")) & "', "
            strsql &= "POM_S_ADDR_LINE2 = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_ADDR_LINE2")) & "', "
            strsql &= "POM_S_ADDR_LINE3 = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_ADDR_LINE3")) & "', "
            strsql &= "POM_S_POSTCODE = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_POSTCODE")) & "', "
            strsql &= "POM_S_CITY = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_CITY")) & "', "
            strsql &= "POM_S_STATE = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_STATE")) & "', "
            strsql &= "POM_S_COUNTRY = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_COUNTRY")) & "', "
            strsql &= "POM_S_PHONE = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_PHONE")) & "', "
            strsql &= "POM_S_EMAIL = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_EMAIL")) & "', "
            strsql &= "POM_S_FAX = '" & Common.Parse(ds.Tables(0).Rows(0)("CM_FAX")) & "', "
            'End
            strsql &= "POM_PO_COST = " & Common.Parse(dsPO.Tables(0).Rows(0)("POCost")) & " "
            'strsql &= "POM_GST = " & Common.Parse(dsPO.Tables(0).Rows(0)("GST")) & " "
            strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            strsql &= "AND POM_B_COY_ID = '" & companyID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' delete from PR_DETAILS table
            strsql = "DELETE FROM PO_DETAILS "
            strsql &= "WHERE POD_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            strsql &= "AND POD_COY_ID = '" & companyID & "' "
            Common.Insert2Ary(strAryQuery, strsql)


            ' PO_DETAILS table
            ' Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            For i = 0 To dsPO.Tables(1).Rows.Count - 1
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - Include GST Tax Code
                strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
                strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
                strsql &= "POD_ETD, POD_REMARK, POD_GST, POD_GST_RATE, POD_GST_INPUT_TAX_CODE, POD_TAX_VALUE, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
                strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
                strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE "
                'If HttpContext.Current.Session("Env") <> "FTN" Then

                'Jules 2018.05.07 - PAMB Scrum 2
                If companyID.ToString.ToUpper = "PAMB" Then
                    If blnEnterpriseVersion = True Then
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_WARRANTY_TERMS, POD_B_ITEM_CODE, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO, POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE) SELECT "
                    Else
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE) SELECT "
                    End If
                Else
                    If blnEnterpriseVersion = True Then
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_WARRANTY_TERMS, POD_B_ITEM_CODE, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO ) SELECT "
                    Else
                        strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX) SELECT "
                    End If
                End If
                'End modification.

                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "', " ' PR_No
                strsql &= "'" & companyID & "', " ' COY_ID
                strsql &= Common.Parse(dsPO.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("ProductCode")), dsPO.Tables(1).Rows(i)("ProductCode"), "")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("VendorItemCode")), dsPO.Tables(1).Rows(i)("VendorItemCode"), "")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("ProductDesc")), dsPO.Tables(1).Rows(i)("ProductDesc"), "")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= "'" & dsPO.Tables(1).Rows(i)("Qty") & "', " ' ORDERED_QTY
                strsql &= "'" & dsPO.Tables(1).Rows(i)("UnitCost") & "', " ' UNIT_COST
                strsql &= IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("ETD")), dsPO.Tables(1).Rows(i)("ETD"), 0) & ", " ' ETD
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("Remark")), dsPO.Tables(1).Rows(i)("Remark"), "")) & "', " ' REMARK
                strsql &= IIf(IsDBNull(dsPO.Tables(1).Rows(i)("GST")), dsPO.Tables(1).Rows(i)("GST"), 0.00) & ", "
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("SelectedGST")), dsPO.Tables(1).Rows(i)("SelectedGST"), "")) & "', " 'POD_GST_RATE
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("GSTTaxCode")), dsPO.Tables(1).Rows(i)("GSTTaxCode"), "")) & "', " 'POD_GST_INPUT_TAX_CODE
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("GSTTaxAmount")), dsPO.Tables(1).Rows(i)("GSTTaxAmount"), "")) & "', " 'POD_GST_TAX Amount
                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("ProductType")), dsPO.Tables(1).Rows(i)("ProductType"), "")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("Source")), dsPO.Tables(1).Rows(i)("Source"), "")) & "', " ' SOURCE
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= "'" & IIf(dsPO.Tables(1).Rows(i)("MOQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MOQ")) & "', "
                strsql &= "'" & IIf(dsPO.Tables(1).Rows(i)("MPQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MPQ")) & "', "

                If Common.parseNull(dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE")) = "" Then
                    strsql &= "NULL, "
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("POD_RFQ_ITEM_LINE") & ", "
                End If

                If Common.parseNull(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & " " ' PRD_CD_GROUP_INDEX
                End If

                'If HttpContext.Current.Session("Env") <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= ", " & dsPO.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                    'If Common.Parse(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
                    '    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                    'Else
                    '    strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                    'End If
                    strsql &= "'" & Common.parseNull(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("ItemCode")), dsPO.Tables(1).Rows(i)("ItemCode"), "")) & "', " ' PRD_B_ITEM_CODE
                    strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("CategoryCode")), dsPO.Tables(1).Rows(i)("CategoryCode"), "")) & "', " ' PRD_B_CATEGORY_CODE
                    strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("GLCode")), dsPO.Tables(1).Rows(i)("GLCode"), "")) & "', " ' PRD_B_GL_CODE
                    If Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("AcctIndex")), dsPO.Tables(1).Rows(i)("AcctIndex"), "")) <> "" Then
                        strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("AcctIndex")), dsPO.Tables(1).Rows(i)("AcctIndex"), "")) & "', " ' ACCT_INDEX
                    Else
                        strsql &= "null, " ' ACCT_INDEX
                    End If
                    strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("AssetGroup")), dsPO.Tables(1).Rows(i)("AssetGroup"), "")) & "', "
                    strsql &= "'" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("AssetGroupNo")), dsPO.Tables(1).Rows(i)("AssetGroupNo"), "")) & "' "
                End If

                'Jules 2018.05.07 - PAMB Scrum 2
                If companyID.ToString.ToUpper = "PAMB" Then
                    strsql &= ", '" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("Gift")), dsPO.Tables(1).Rows(i)("Gift"), "")) & "'"
                    strsql &= ", '" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("pod_fund_type")), dsPO.Tables(1).Rows(i)("pod_fund_type"), "")) & "'"
                    strsql &= ", '" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("pod_person_code")), dsPO.Tables(1).Rows(i)("pod_person_code"), "")) & "'"
                    strsql &= ", '" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("pod_project_code")), dsPO.Tables(1).Rows(i)("pod_project_code"), "")) & "'"
                End If
                'End modification.

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & companyID & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(IIf(Not IsDBNull(dsPO.Tables(1).Rows(i)("DeliveryAddr")), dsPO.Tables(1).Rows(i)("DeliveryAddr"), "")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

            Next

            'Jules 2019.03.21 - To avoid missing attachment issue.
            '' delete COMPANY_DOC_ATTACHMENT table
            'strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            'strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            '' update COMPANY_DOC_ATTACHMENT_TEMP table
            'strsql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
            'strsql &= "CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            '' insert COMPANY_DOC_ATTACHMENT table
            'strsql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
            'strsql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            '' delete COMPANY_DOC_ATTACHMENT_TEMP table
            'strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
            'strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            'strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            'Common.Insert2Ary(strAryQuery, strsql)

            'Update COMPANY_DOC_ATTACHMENT table although there shouldn't be any tagged with sessionID.
            strsql = "UPDATE COMPANY_DOC_ATTACHMENT SET "
            strsql &= "CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
            strsql &= "AND CDA_DOC_TYPE = 'PO' "
            'strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
            Common.Insert2Ary(strAryQuery, strsql)
            'End modification.

            If modePR = True Then
                ' delete from PR_CUSTOM_FIELD_MSTR
                'strsql = "DELETE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX IN "
                'strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
                'strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                'strsql &= "AND POM_B_COY_ID = '" & companyID & "') "
                'Common.Insert2Ary(strAryQuery, strsql)


                '' PR_CUSTOM_FIELD_MSTR table
                'For i = 0 To dsPO.Tables(2).Rows.Count - 1
                '    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                '    'Michelle (16/11/2010) - To cater for MYSQL
                '    'strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                '    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                '    strsql &= "(SELECT POM_PO_Index FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                '    strsql &= "AND POM_B_COY_ID = '" & companyID & "'), "
                '    strsql &= dsPO.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                '    strsql &= "'" & Common.Parse(dsPO.Tables(2).Rows(i)("FieldName")) & "', 'PO'" ' FIELD_NAME
                '    Common.Insert2Ary(strAryQuery, strsql)
                'Next

                '' delete from PR_CUSTOM_FIELD_DETAILS
                'strsql = "DELETE FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX IN "
                'strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
                'strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                'strsql &= "AND POM_B_COY_ID = '" & companyID & "') "
                'Common.Insert2Ary(strAryQuery, strsql)

                '' PR_CUSTOM_FIELD_DETAILS table
                'For i = 0 To dsPO.Tables(3).Rows.Count - 1
                '    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                '    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                '    'Michelle (16/11/2010) - To cater for MYSQL
                '    'strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
                '    'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
                '    strsql &= "(SELECT POM_PO_Index FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
                '    strsql &= "AND POM_B_COY_ID = '" & companyID & "'), "
                '    strsql &= dsPO.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                '    strsql &= dsPO.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                '    strsql &= "'" & Common.Parse(dsPO.Tables(3).Rows(i)("FieldValue")) & "', 'PO' " ' FIELD_VALUE
                '    Common.Insert2Ary(strAryQuery, strsql)
                'Next
            End If

            objDb.BatchExecute(strAryQuery)
        End Sub

        'Public Function getApprovedVendorGSTRate(ByVal strVCoyId As String) As String
        '    Dim strSQL As String

        '    strSQL = "SELECT IFNULL(CV_GST_RATE, '') FROM COMPANY_VENDOR WHERE CV_S_COY_ID = '" & strVCoyId & "' AND CV_B_COY_ID = '" & companyID & "'"
        '    getApprovedVendorGSTRate = objDb.GetVal(strSQL)

        'End Function
        'Public Function deleteRFQItemSQL(ByVal strRFQ As String, ByVal sProductCode As Integer) As String
        '    Dim strsql As String
        '    ' delete from PR_DETAILS table
        '    strsql = "DELETE FROM RFQ_REPLIES_DETAIL "
        '    strsql &= "WHERE RRD_RFQ_ID = '" & strRFQ & "' "
        '    strsql &= "AND RRD_Product_Code = '" & sProductCode & "'; "

        '    deleteRFQItemSQL = strsql
        'End Function

        Public Function updatePRStatus(ByVal userID As String, ByVal companyID As String, ByVal strPR_Index As String, ByVal strType As String) As Integer
            Dim strsql, strPOM_PO_NO As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            Dim strIndex As String
            Dim ds, ds2, ds3 As New DataSet
            Dim blnUpdate As Boolean = False

            If strPR_Index <> "" Then
                strsql = " SELECT PRM_PR_NO FROM PR_MSTR, PR_DETAILS "
                strsql &= " WHERE PRM_PR_NO = PRD_PR_NO And PRM_COY_ID = PRD_COY_ID "
                strsql &= " AND PRM_COY_ID = '" & companyID & "' AND PRM_PR_INDEX IN (" & strPR_Index & ") "
                strsql &= " AND PRD_CONVERT_TO_DOC IS NULL "
                ds = objDb.FillDs(strsql)

                If ds.Tables(0).Rows.Count <= 0 Then
                    strsql = " SELECT PRD_CONVERT_TO_DOC FROM PR_MSTR, PR_DETAILS "
                    strsql &= " WHERE PRM_PR_NO = PRD_PR_NO And PRM_COY_ID = PRD_COY_ID "
                    strsql &= " AND PRM_COY_ID = '" & companyID & "' AND PRM_PR_INDEX IN (" & strPR_Index & ") "
                    strsql &= " AND PRD_CONVERT_TO_IND = 'RFQ' "
                    ds2 = objDb.FillDs(strsql)

                    If ds2.Tables(0).Rows.Count > 0 Then
                        For i = 0 To ds2.Tables(0).Rows.Count - 1

                            strsql = " SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL "
                            strsql &= " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & companyID & "' AND "
                            strsql &= " RM_RFQ_NO = '" & ds2.Tables(0).Rows(0)("PRD_CONVERT_TO_DOC") & "' AND "
                            strsql &= " (RD_PRODUCT_CODE NOT IN ( "
                            strsql &= " SELECT POD_PRODUCT_CODE FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND "
                            strsql &= " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & companyID & "' AND POM_RFQ_INDEX = ( "
                            strsql &= " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & companyID & "' AND "
                            strsql &= " RM_RFQ_NO = '" & ds2.Tables(0).Rows(0)("PRD_CONVERT_TO_DOC") & "') "
                            strsql &= " ) OR "
                            strsql &= " RD_PRODUCT_DESC NOT IN ( "
                            strsql &= " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND "
                            strsql &= " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & companyID & "' AND POM_RFQ_INDEX = ( "
                            strsql &= " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & companyID & "' AND "
                            strsql &= " RM_RFQ_NO = '" & ds2.Tables(0).Rows(0)("PRD_CONVERT_TO_DOC") & "') "
                            strsql &= " ) ) "

                            ds3 = objDb.FillDs(strsql)

                            If ds3.Tables(0).Rows.Count > 0 Then
                                blnUpdate = False
                                Exit Function
                            Else
                                blnUpdate = True
                            End If

                            'strPOM_PO_NO = objDb.GetVal(" SELECT POM_PO_NO FROM PO_MSTR WHERE POM_B_COY_ID = '" & companyID & "' AND POM_RFQ_INDEX = (SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & companyID & "' AND RM_RFQ_NO = '" & ds2.Tables(0).Rows(0)("PRD_CONVERT_TO_DOC") & "') ")
                            'If strPOM_PO_NO <> "" Then ' Got value = Converted
                            '    blnUpdate = True
                            'Else
                            '    blnUpdate = False
                            '    Exit Function
                            'End If
                        Next
                    End If
                End If
            End If

            If blnUpdate = True Then
                strsql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_COY_ID = '" & companyID & "' AND PRM_PR_INDEX IN (" & strPR_Index & ") "
                Common.Insert2Ary(strAryQuery, strsql)
                objDb.BatchExecute(strAryQuery)
            End If

        End Function

        Public Function submitPO(ByVal userID As String, ByVal companyID As String, ByVal strPO As String, ByVal dtAO As DataTable, Optional ByVal strType As String = "", Optional ByVal frmApprSetup As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal blnFrPR As Boolean = False) As Integer
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            Dim pQuery(0) As String
            Dim strIndex As String
            Dim ds As New DataSet
            Dim objPR As New PR

            Dim blnPR As Boolean = False
            Dim objDB As New EAD.DBCom
            Dim strBuyer As String
            Dim Owner_Type As String = objDB.GetVal("SELECT IFNULL(COMPANY_MSTR.CM_NCONTR_PR_SETTING,'') AS CM_NCONTR_PR_SETTING FROM COMPANY_MSTR WHERE CM_COY_ID = '" & companyID & "' ")
            'If Owner_Type = "NP" Then
            '    Dim Owner_ID As String = objDB.GetVal("SELECT IFNULL(COMPANY_MSTR.CM_NCONTR_PR_PO_OWNER_ID,'') AS CM_NCONTR_PR_PO_OWNER_ID FROM COMPANY_MSTR WHERE CM_COY_ID = '" & companyID & "' ")
            '    strBuyer = Owner_ID
            'End If

            Dim objUser As New User
            Dim objUsers As New Users
            Dim strUserName As String, strPhone As String, strFax As String
            Dim strPO_Dept_Index As String = ""
            If objUsers.GetUserDetail(strBuyer, "" & companyID & "", objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
            End If

            Dim strUser As String
            Dim objUser2 As New User
            Dim objUsers2 As New Users
            Dim strUserName2 As String, strPhone2 As String, strFax2 As String
            Dim strPO_No2 As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strPO & "'")
            If strPO_No2 <> "" Then
                strUser = objDB.GetVal("SELECT PRM_BUYER_ID FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strPO & "' LIMIT 1")
                strPO_Dept_Index = objDB.GetVal("SELECT IFNULL(PRM_DEPT_INDEX,'') AS PRM_DEPT_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strPO & "' LIMIT 1")
                If objUsers2.GetUserDetail(strUser, "" & companyID & "", objUser2) Then
                    strUserName2 = objUser2.Name
                    strPhone2 = objUser2.PhoneNo
                    strFax2 = objUser2.FaxNo
                Else
                    strUserName2 = ""
                    strPhone2 = ""
                    strFax2 = ""
                End If

                blnPR = True
            End If


            Dim strRFQ_No As String = objDB.GetVal("SELECT RM_RFQ_NO FROM PO_MSTR, RFQ_MSTR WHERE POM_RFQ_INDEX = RM_RFQ_ID AND POM_PO_NO = '" & strPO & "'")
            If strRFQ_No <> "" Then
                strRFQ_No = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strRFQ_No & "'")
                If strRFQ_No <> "" Then
                    strUser = objDB.GetVal("SELECT PRM_BUYER_ID FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strRFQ_No & "' LIMIT 1")
                    strPO_Dept_Index = objDB.GetVal("SELECT IFNULL(CDM_DEPT_INDEX,'') AS CDM_DEPT_INDEX FROM USER_MSTR LEFT OUTER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID AND UM_COY_ID = CDM_COY_ID WHERE UM_USER_ID = '" & strUser & "' AND UM_COY_ID = '" & companyID & "' ")

                    If objUsers2.GetUserDetail(strUser, "" & companyID & "", objUser2) Then
                        strUserName2 = objUser2.Name
                        strPhone2 = objUser2.PhoneNo
                        strFax2 = objUser2.FaxNo
                    Else
                        strUserName2 = ""
                        strPhone2 = ""
                        strFax2 = ""
                    End If

                    blnPR = True
                End If
            End If

            strsql = "SELECT POM_PO_INDEX, POM_PO_STATUS FROM PO_MSTR WHERE POM_PO_NO = '" & strPO & "' "
            strsql &= "AND POM_B_COY_ID = '" & companyID & "' "
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                strIndex = ds.Tables(0).Rows(0)("POM_PO_INDEX")
                ' check PO status still is draft
                If Common.parseNull(ds.Tables(0).Rows(0)("POM_PO_STATUS")) = "0" Then
                    strsql = "UPDATE PO_MSTR SET "
                    strsql &= "POM_STATUS_CHANGED_ON = " & Common.ConvertDate(Now) & ", "
                    strsql &= "POM_SUBMIT_DATE = " & Common.ConvertDate(Now) & ", "
                    strsql &= "POM_STATUS_CHANGED_BY = '" & userID & "', "
                    strsql &= "POM_PO_STATUS = '" & POStatus_new.Submitted & "' "
                    strsql &= "WHERE POM_PO_INDEX = '" & strIndex & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'If Owner_Type = "NP" And blnPR = True Then
                    '    strsql = "UPDATE PO_MSTR SET "
                    '    strsql &= "POM_BUYER_ID = '" & strBuyer & "', "
                    '    strsql &= "POM_BUYER_NAME = '" & strUserName & "', "
                    '    strsql &= "POM_BUYER_PHONE = '" & strPhone & "', "
                    '    strsql &= "POM_DEPT_INDEX = '" & strPO_Dept_Index & "', "
                    '    strsql &= "POM_BUYER_FAX = '" & strFax & "' "
                    '    strsql &= "WHERE POM_PO_INDEX = '" & strIndex & "' "
                    '    Common.Insert2Ary(strAryQuery, strsql)
                    If Owner_Type = "NB" And blnPR = True Then
                        strsql = "UPDATE PO_MSTR SET "
                        strsql &= "POM_BUYER_ID = '" & strUser & "', "
                        strsql &= "POM_BUYER_NAME = '" & strUserName2 & "', "
                        strsql &= "POM_BUYER_PHONE = '" & strPhone2 & "', "

                        If strPO_Dept_Index <> "" Then
                            strsql &= "POM_DEPT_INDEX = '" & strPO_Dept_Index & "', "
                        End If

                        strsql &= "POM_BUYER_FAX = '" & strFax2 & "' "
                        strsql &= "WHERE POM_PO_INDEX = '" & strIndex & "' "
                        Common.Insert2Ary(strAryQuery, strsql)
                    End If

                    ' insert into PR_APPROVAL
                    For i = 0 To 0
                        strsql = "INSERT INTO PR_APPROVAL (PRA_PR_INDEX, PRA_AO, PRA_A_AO, PRA_SEQ, PRA_AO_ACTION, "
                        strsql &= "PRA_APPROVAL_TYPE, PRA_APPROVAL_GRP_INDEX, PRA_RELIEF_IND, PRA_FOR ) VALUES ("
                        strsql &= strIndex & ", "                        ' PRA_PR_INDEX
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("AO")) & "', "                         ' PRA_AO
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("AAO")) & "', "                        ' PRA_A_AO
                        strsql &= dtAO.Rows(i)("Seq") & ", 0, "                      ' PRA_SEQ, PRA_AO_ACTION
                        If frmApprSetup Then
                            strsql &= "'" & Common.Parse(dtAO.Rows(i)("Type")) & "', "   ' PRA_APPROVAL_TYPE
                        Else
                            strsql &= "'1', "
                        End If
                        strsql &= dtAO.Rows(i)("GrpIndex") & ", "                        ' PRA_APPROVAL_GRP_INDEX
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("Relief")) & "', 'PO' ) "                         ' PRA_RELIEF_IND
                        Common.Insert2Ary(strAryQuery, strsql)
                    Next

                    Dim objBudget As New BudgetControl
                    Dim strBCM, strCurrency As String
                    Dim blnExceed As Boolean
                    Dim dtBCM As New DataTable

                    Dim strPO_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strPO & "'")
                    If strPO_No <> "" Then
                        strBCM = "PR"
                    End If

                    'If HttpContext.Current.Session("Env") <> "FTN" And strBCM <> "PR" Then
                    If blnEnterpriseVersion = True And strBCM <> "PR" Then
                        objBudget.BCMCalc("PO", companyID, strPO, EnumBCMAction.SubmitPO, strAryQuery)
                        blnExceed = objBudget.checkBCM(strPO, companyID, dtBCM, strBCM)
                        strsql = "SELECT POM_CURRENCY_CODE FROM PO_MSTR "
                        strsql &= "WHERE POM_PO_NO = '" & Common.Parse(strPO) & "' AND POM_B_COY_ID = '" & companyID & "' "
                        Dim tDS As DataSet = objDB.FillDs(strsql)
                        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                            strCurrency = tDS.Tables(0).Rows(j).Item("POM_CURRENCY_CODE")
                        Next

                        'Dim objUsers As New Users
                        objUsers.Log_UserActivity(userID, companyID, strAryQuery, WheelModule.PRMod, WheelUserActivity.B_SubmitPR, strPO)
                        objUsers = Nothing
                    End If

                    If blnEnterpriseVersion = True And strBCM <> "PR" And blnFrPR = False Then
                        strsql = "SELECT POD_PO_NO, POD_PO_LINE, POD_ASSET_GROUP FROM PO_DETAILS WHERE POD_PO_NO = '" & strPO & "' "
                        strsql &= "AND (POD_ASSET_GROUP IS NOT NULL AND POD_ASSET_GROUP <> '') AND POD_COY_ID = '" & companyID & "' "
                        ds = objDB.FillDs(strsql)

                        Dim strAssetNo As String
                        Dim intAssetIncrementNo As Integer
                        intAssetIncrementNo = 1
                        For i = 0 To 0
                            Common.parseNull(ds.Tables(0).Rows(i)("POD_ASSET_GROUP"))

                            Dim strParam As String = objDB.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "' AND CP_COY_ID = '" & companyID & "'")
                            If strParam = "" Then
                                strsql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                                strsql &= "'" & companyID & "', "
                                strsql &= "'Prefix', "
                                strsql &= "'" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "', "
                                strsql &= "'" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "', "
                                strsql &= "'eProcure' ) "
                                Common.Insert2Ary(strAryQuery, strsql)

                                strsql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                                strsql &= "'" & companyID & "', "
                                strsql &= "'Last Used No', "
                                strsql &= "'00000000', "
                                strsql &= "'" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "', "
                                strsql &= "'eProcure' ) "
                                Common.Insert2Ary(strAryQuery, strsql)
                            End If

                            strAssetNo = " (SELECT CAST( CONCAT(RIGHT(YEAR(CURRENT_DATE()),2), " _
                                        & " CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), " _
                                        & " (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')))  AS CHAR(1000)) AS CP_PARAM_VALUE FROM (SELECT * FROM COMPANY_PARAM " _
                                        & " WHERE CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "' AND CP_PARAM_NAME = 'Last Used No') ZZZ )"

                            strsql = "UPDATE PO_DETAILS SET "
                            strsql &= "POD_ASSET_NO = " & strAssetNo & " "
                            strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
                            strsql &= "AND POD_COY_ID = '" & companyID & "' "
                            strsql &= "AND POD_PO_LINE = '" & ds.Tables(0).Rows(i)("POD_PO_LINE") & "' "
                            Common.Insert2Ary(strAryQuery, strsql)

                            strsql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "' "
                            Common.Insert2Ary(strAryQuery, strsql)

                            'intAssetIncrementNo += 1
                        Next
                    End If

                    If objDB.BatchExecute(strAryQuery) Then

                        'If HttpContext.Current.Session("Env") <> "FTN" Then
                        '' ''If blnEnterpriseVersion = True Then
                        'Michelle (19/3/2008) - To allow mail to send to AO evenif budget has been bust when the budget mode is 'Advisory'
                        If strType = "0" Or strType = "1" Or strType = "" Then
                            objPR.sendMailToAO(userID, companyID, strPO, CLng(strIndex), 1, "PO")
                        End If
                        If strType <> "0" Then
                            ' PR value more than operating budget
                            objPR.RequestBudgetTopup(userID, companyID, strBCM, "", "", strCurrency, dtBCM)

                        End If
                        '' ''End If
                        submitPO = WheelMsgNum.Save
                        '' ''objBudget = Nothing
                        '' ''Else
                        '' ''    objPR.sendMailToAO(strPO, CLng(strIndex), 1, "PO")
                    End If
                    objBudget = Nothing
                Else
                    submitPO = WheelMsgNum.NotSave
                End If
            Else
                submitPO = WheelMsgNum.Delete
            End If

        End Function

        '        Public Function getCompSetting() As String
        '            Dim strsql As String = "SELECT CM_BA_CANCEL FROM COMPANY_MSTR WHERE CM_COY_ID = '" & companyID & "'"
        '            getCompSetting = objDb.GetVal(strsql)

        '        End Function

        '#Region "Zulham"
        '        Public Function insertPODetails(ByVal dsPO As DataSet, ByRef strPONo As String, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal dictCustoms As System.Collections.Generic.Dictionary(Of String, String) = Nothing, Optional ByVal strAryQuery As Array = Nothing, Optional ByVal dtHeader As DataTable = Nothing, Optional ByVal rowCount As Integer = 0) As Integer

        '            Dim strPrefix, strName, strPhone, strFax As String
        '            Dim strsql, strTermFile As String
        '            'Dim strAryQuery(0) As String
        '            Dim strAryAdd(8) As String
        '            Dim i, j As Integer
        '            Dim blnAdd As Boolean = False
        '            Dim strDeptIndex As String = ""
        '            Dim objGlobal As New AppGlobals
        '            Dim intIncrementNo As Integer = 0
        '            Dim objAdmin As New Admin
        '            Dim dsBillTo As New DataSet
        '            Dim dtBillTo As New DataTable
        '            Dim strVendor As String = ""
        '            Dim _strNewPONo As String = ""
        '            Dim dsBCM As New DataSet
        '            Dim objBudget As New BudgetControl

        '            'Michelle (27/12/2010) - To store the Term & Condition
        '            'Dim objWheelFile As New FileManagement
        '            'strTermFile = objWheelFile.copyTermCondToPO(strPONo)

        '            'save PO_Mstr if it hasnt been saved to get the PONumber
        '            If strPONo.Trim = "To Be Allocated By System" Then
        '                HttpContext.Current.Session("NewPoInfo") = Nothing
        '                dsBillTo = objAdmin.PopulateAddr("B", "", "", "")
        '                If Not dsBillTo.Tables(0).Rows.Count = 0 Then
        '                    For i = 0 To dsBillTo.Tables(0).Rows.Count - 1
        '                        If dtHeader.Rows(0).Item(1).ToString.Contains(dsBillTo.Tables(0).Rows(i).Item("Address")) Then
        '                            dtBillTo.Columns.Add("BillAddrCode") 'B_ADDR_CODE
        '                            dtBillTo.Columns.Add("BillAddrLine1") ' B_ADDR_LINE1
        '                            dtBillTo.Columns.Add("BillAddrLine2") ' B_ADDR_LINE2
        '                            dtBillTo.Columns.Add("BillAddrLine3") ' B_ADDR_LINE3
        '                            dtBillTo.Columns.Add("BillAddrPostCode") ' B_POSTCODE
        '                            dtBillTo.Columns.Add("BillAddrState") ' B_STATE
        '                            dtBillTo.Columns.Add("BillAddrCity") ' B_CITY
        '                            dtBillTo.Columns.Add("BillAddrCountry") ' B_COUNTRY

        '                            Dim row As DataRow = dtBillTo.NewRow
        '                            row("BillAddrCode") = dsBillTo.Tables(0).Rows(i).Item("AM_ADDR_CODE") 'B_ADDR_CODE
        '                            row("BillAddrLine1") = dsBillTo.Tables(0).Rows(i).Item("AM_ADDR_LINE1") ' B_ADDR_LINE1
        '                            row("BillAddrLine2") = dsBillTo.Tables(0).Rows(i).Item("AM_ADDR_LINE2") ' B_ADDR_LINE2
        '                            row("BillAddrLine3") = dsBillTo.Tables(0).Rows(i).Item("AM_ADDR_LINE3") ' B_ADDR_LINE3
        '                            row("BillAddrPostCode") = dsBillTo.Tables(0).Rows(i).Item("AM_POSTCODE") ' B_POSTCODE
        '                            row("BillAddrState") = dsBillTo.Tables(0).Rows(i).Item("AM_STATE") ' B_STATE
        '                            row("BillAddrCity") = dsBillTo.Tables(0).Rows(i).Item("AM_CITY") ' B_CITY
        '                            row("BillAddrCountry") = dsBillTo.Tables(0).Rows(i).Item("AM_COUNTRY") ' B_COUNTRY
        '                            dtBillTo.Rows.Add(row)
        '                            Exit For
        '                        End If
        '                    Next
        '                    dtBillTo.AcceptChanges()
        '                End If

        '                strVendor = objDb.GetVal("SELECT cm_coy_id FROM company_mstr WHERE cm_coy_name = '" & dtHeader.Rows(0).Item(0).ToString.Trim & "'")

        '                strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PO' "
        '                Common.Insert2Ary(strAryQuery, strsql)

        '                'GetLatestDocNo
        '                '' ''objGlobal.GetLatestDocNo("PO", strAryQuery, strPONo, strPrefix)

        '                intIncrementNo = 1

        '                _strNewPONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
        '                           & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
        '                           & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

        '                HttpContext.Current.Session("NewPoInfo") = objDb.GetVal("(SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
        '                                    & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
        '                                    & " SEPARATOR '') AS CHAR(1000)) AS cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) ")
        '                HttpContext.Current.Session("NewPoInfo") &= "|mod|BatchUpload"
        '                strPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PO') "

        '                strsql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = " & _strNewPONo & " "
        '                strsql &= "AND POM_B_COY_ID = '" & companyID & "' "

        '                If objDb.Exist(strsql) > 0 Then
        '                    insertPODetails = WheelMsgNum.Duplicate
        '                    Exit Function
        '                End If

        '                ' to check whether vendor company is inactive
        '                strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
        '                strsql &= "AND CM_COY_ID = '" & strVendor & "' "
        '                If objDb.Exist(strsql) > 0 Then
        '                    insertPODetails = -1
        '                    Exit Function
        '                End If

        '                ' to check whether vendor company is being deleted
        '                strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
        '                strsql &= "AND CM_COY_ID = '" & strVendor & "' "
        '                If objDb.Exist(strsql) > 0 Then
        '                    insertPODetails = -2
        '                    Exit Function
        '                End If

        '                ' get dept index
        '                'Michelle (26/4/2011) - To cater for those without department
        '                strsql = "SELECT CDM_DEPT_INDEX, UM_USER_NAME, UM_FAX_NO, UM_TEL_NO FROM USER_MSTR "
        '                strsql &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
        '                strsql &= "AND UM_COY_ID = CDM_COY_ID "
        '                strsql &= "WHERE UM_USER_ID = '" & userID & "' "
        '                strsql &= "AND UM_COY_ID = '" & companyID & "' "
        '                Dim tDS As DataSet = objDb.FillDs(strsql)
        '                For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '                    strDeptIndex = Common.parseNull(tDS.Tables(0).Rows(q).Item("CDM_DEPT_INDEX"))
        '                    strName = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_USER_NAME"))
        '                    strPhone = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_TEL_NO"))
        '                    strFax = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_FAX_NO"))
        '                Next

        '                'Michelle (27/12/2010) - To store the Term & Condition
        '                Dim objWheelFile As New FileManagement
        '                strTermFile = objWheelFile.copyTermCondToPO(_strNewPONo)

        '                ' PO_MSTR table
        '                strsql = "INSERT INTO PO_MSTR (POM_PO_NO, POM_B_COY_ID, POM_BUYER_ID, POM_BUYER_NAME, POM_BUYER_PHONE, POM_BUYER_FAX, "
        '                strsql &= "POM_CREATED_DATE, POM_CREATED_BY, POM_STATUS_CHANGED_BY, POM_STATUS_CHANGED_ON, "
        '                strsql &= "POM_S_COY_ID, POM_S_ATTN, POM_S_COY_NAME, POM_S_ADDR_LINE1, "
        '                strsql &= "POM_S_ADDR_LINE2, POM_S_ADDR_LINE3, POM_S_POSTCODE, POM_S_CITY, POM_S_STATE, "
        '                strsql &= "POM_S_COUNTRY, POM_S_PHONE, POM_S_FAX, POM_S_EMAIL, "
        '                strsql &= "POM_PAYMENT_METHOD, POM_SHIPMENT_TERM, POM_SHIPMENT_MODE, POM_CURRENCY_CODE, "
        '                strsql &= "POM_EXCHANGE_RATE, POM_PAYMENT_TERM, POM_SHIP_VIA, POM_BILLING_METHOD, POM_PO_TYPE, POM_INTERNAL_REMARK, "
        '                strsql &= "POM_EXTERNAL_REMARK, POM_PO_STATUS, POM_FULFILMENT, POM_PO_INDEX, POM_ARCHIVE_IND, "
        '                strsql &= "POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SHIP_AMT, POM_PO_PREFIX, POM_B_ADDR_CODE, "
        '                strsql &= "POM_B_ADDR_LINE1, POM_B_ADDR_LINE2, POM_B_ADDR_LINE3, POM_B_POSTCODE, "
        '                strsql &= "POM_B_STATE, POM_B_CITY, POM_B_COUNTRY, "
        '                strsql &= "POM_DUP_FROM, POM_EXTERNAL_IND, POM_REFERENCE_NO, "
        '                strsql &= "POM_PO_COST, POM_RFQ_INDEX, POM_DEPT_INDEX, POM_QUOTATION_NO, POM_TERMANDCOND, POM_URGENT) SELECT "
        '                strsql &= "" & _strNewPONo & ", "
        '                strsql &= "'" & companyID & "', "
        '                strsql &= "'" & userID & "', "
        '                strsql &= "'" & Common.Parse(strName) & "', "
        '                strsql &= "'" & Common.Parse(strPhone) & "', "
        '                strsql &= "'" & Common.Parse(strFax) & "', " 'BUYER_FAX
        '                strsql &= Common.ConvertDate(Now) & ", "  'CREATED_DATE
        '                strsql &= "'" & userID & "', "
        '                strsql &= "'" & userID & "', " 'STATUS_CHANGED_BY
        '                strsql &= Common.ConvertDate(Now) & ", " 'STATUS_CHANGED_ON
        '                strsql &= "'" & Common.Parse(strVendor) & "', " 'S_COY_ID
        '                strsql &= "'" & "" & "', " 'POM_S_ATTN
        '                strsql &= "CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, "
        '                strsql &= "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "
        '                ' = CStr(Common.parseNull(dsVendor.Tables(0).Rows(0)("CV_Payment_Term")))
        '                '
        '                'strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', " ' FREIGHT_TERMS
        '                strsql &= "'" & HttpContext.Current.Session("PaymentMethod") & "', " ' PAYMENT_TYPE
        '                strsql &= "'" & HttpContext.Current.Session("ShipmentTerm") & "', " ' SHIPMENT_TERM
        '                strsql &= "'" & HttpContext.Current.Session("ShipmentMethod") & "', " ' SHIPMENT_MODE
        '                strsql &= "'" & HttpContext.Current.Session("CurrencyCode") & "', " ' CURRENCY_CODE
        '                strsql &= 0.0 & ", "  ' EXCHANGE_RATE
        '                strsql &= "'" & HttpContext.Current.Session("PaymentTerm") & "', " ' PAYMENT_TERM
        '                strsql &= "'" & "" & "', " ' SHIP_VIA
        '                strsql &= "'" & HttpContext.Current.Session("BillingMethod") & "', " ' Empty string Billing_Method
        '                strsql &= "'Y', "
        '                strsql &= "'" & "" & "', " ' INTERNAL_REMARK
        '                strsql &= "'" & "" & "', " 'EXTERNAL_REMARK
        '                strsql &= "'" & POStatus_new.Draft & "', " ' PO_STATUS
        '                strsql &= "'" & Fulfilment.null & "', "
        '                strsql &= "NULL, '', "   ' PO_INDEX, ARCHIVE_IND
        '                strsql &= "'" & "1" & "', " 'PRINT_CUSTOM_FIELDS
        '                strsql &= "'" & "1" & "', " 'PRINT_REMARK
        '                strsql &= "'" & 0.0 & "', " 'POM_SHIP_AMT
        '                strsql &= "" & strPrefix & ", "
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrCode")) & "', " ' B_ADDR_CODE
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrLine1")) & "', " ' B_ADDR_LINE1
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrLine2")) & "', " ' B_ADDR_LINE2
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrLine3")) & "', " ' B_ADDR_LINE3
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrPostCode")) & "', " ' B_POSTCODE
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrState")) & "', " ' B_STATE
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrCity")) & "', " ' B_CITY
        '                strsql &= "'" & Common.Parse(dtBillTo.Rows(0).Item("BillAddrCountry")) & "', " ' B_COUNTRY
        '                strsql &= "'', '', '', " ' DUP_FROM, EXTERNAL_IND, REFERENCE_NO
        '                strsql &= 0.0 & ", " ' PO_COST
        '                strsql &= "NULL, " ' RFQ_INDEX

        '                If strDeptIndex = "" Then ' PRM_DEPT_INDEX
        '                    strsql &= "NULL, "
        '                Else
        '                    strsql &= strDeptIndex & ", "
        '                End If
        '                strsql &= "'" & "" & "' " ' QUOTATION_NO
        '                strsql &= ", '" & strTermFile & "', '" & "" & "' " 'Term & Condition
        '                strsql &= "FROM COMPANY_MSTR "
        '                strsql &= "WHERE CM_COY_ID = '" & Common.Parse(strVendor) & "' "
        '                strsql &= " AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "

        '                Common.Insert2Ary(strAryQuery, strsql)

        '                'Zulham Oct 24, 2013
        '                'For CustomFields
        '                If Not dictCustoms.Count = 0 Then
        '                    Dim dtCustomMaster As New DataTable
        '                    Dim dvwCus As New DataView
        '                    dvwCus = objAdmin.getCustomField("", "PO")

        '                    dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        '                    dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))
        '                    If Not dvwCus Is Nothing Then
        '                        For i = 0 To dvwCus.Count - 1
        '                            strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
        '                            strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
        '                            strsql &= dvwCus.Table.Rows(i)("CF_FIELD_NO") & ", " ' FIELD_NO
        '                            HttpContext.Current.Session("CF_FIELD_NO") = dvwCus.Table.Rows(i)("CF_FIELD_NO")
        '                            strsql &= "'" & Common.Parse(dvwCus.Table.Rows(i)("CF_FIELD_NAME")) & "', 'PO'" ' FIELD_NAME
        '                            Common.Insert2Ary(strAryQuery, strsql)
        '                        Next
        '                    End If
        '                End If
        '                'End
        '                'End
        '            Else
        '                HttpContext.Current.Session("NewPoInfo") = strPONo & "|mod"
        '            End If
        '            'Get MAxLine
        '            Dim _count = 0

        '            If Not strPONo = "To Be Allocated By System" Then
        '                Dim _sql = "select count(pod_po_line) from po_details where POD_PO_NO = '" & strPONo & "' and pod_coy_id = '" & companyID & "' "
        '                _count = objDb.GetVal(_sql)
        '            End If

        '            'Jules 2018.07.17 - PAMB
        '            ' PO_DETAILS table
        '            Dim dd As New System.Web.UI.WebControls.DropDownList
        '            Dim dds As DataTable
        '            For i = 0 To dsPO.Tables(0).Rows.Count - 1
        '                strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
        '                strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
        '                strsql &= "POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
        '                strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
        '                strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, pod_gst_rate, POD_TAX_VALUE "
        '                If blnEnterpriseVersion = True Then
        '                    strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, POD_ASSET_GROUP, POD_ASSET_NO, POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE, POD_GST_INPUT_TAX_CODE ) SELECT "
        '                Else
        '                    strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_RFQ_ITEM_LINE, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE, POD_GST_INPUT_TAX_CODE) SELECT "
        '                End If
        '                If Not strPONo = "To Be Allocated By System" Then
        '                    strsql &= "'" & strPONo & "', " ' PR_No
        '                Else
        '                    strsql &= "" & _strNewPONo & ", "
        '                End If
        '                strsql &= "'" & companyID & "', " ' COY_ID
        '                strsql &= _count + i + 1 & ", " ' PR_LINE
        '                strsql &= "'', " ' PRODUCT_CODE
        '                strsql &= "'', " ' VENDOR_ITEM_CODE
        '                Dim Desc As String = ""
        '                If dsPO.Tables(0).Rows(i)("Description") IsNot DBNull.Value Then
        '                    Desc = Common.Parse(dsPO.Tables(0).Rows(i)("Description"))
        '                End If
        '                strsql &= "'" & Desc & "', " ' PRODUCT_DESC
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("UOM")) & "', " ' UOM
        '                strsql &= dsPO.Tables(0).Rows(i)("Quantity") & ", " ' ORDERED_QTY
        '                strsql &= dsPO.Tables(0).Rows(i)("UnitPrice") & ", " ' UNIT_COST
        '                Dim _int = DateDiff(DateInterval.Day, Date.Now, dsPO.Tables(0).Rows(i)("DeliveryDate")) + 1
        '                strsql &= _int & ", " ' ETD
        '                Dim remarks As String = ""
        '                If dsPO.Tables(0).Rows(i)("Remarks") IsNot DBNull.Value Then
        '                    remarks = Common.Parse(dsPO.Tables(0).Rows(i)("Remarks"))
        '                End If
        '                strsql &= "'" & remarks & "', " ' REMARK
        '                strsql &= "'" & dsPO.Tables(0).Rows(i)("Tax") & "', " ' GST
        '                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
        '                strsql &= "AM_STATE, AM_COUNTRY, "
        '                strsql &= "''," ' PRODUCT_TYPE
        '                strsql &= "'FF',"  ' SOURCE
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("GSTRate")) & "', " 'POD_GST_RATE
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("GSTTaxValue")) & "', " 'POD_TAx_VALUE
        '                strsql &= "NULL,"
        '                strsql &= "NULL,"
        '                strsql &= "NULL, "
        '                strsql &= "NULL " ' PRD_CD_GROUP_INDEX
        '                ' _Yap: For Interface
        '                strsql &= ",' '" ' PRD_B_ITEM_CODE

        '                If blnEnterpriseVersion = True Then
        '                    Dim WarrantyTerms As Integer = 0
        '                    If dsPO.Tables(0).Rows(i)("WarrantyTerms") IsNot DBNull.Value Then
        '                        WarrantyTerms = Common.Parse(dsPO.Tables(0).Rows(i)("WarrantyTerms"))
        '                    End If
        '                    strsql &= ", '" & WarrantyTerms & "', " ' WARRANTY_TERMS
        '                    ' _Yap: For Interface
        '                    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
        '                    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
        '                    strsql &= Common.Parse(dsPO.Tables(0).Rows(i)("BudgetAccount")) & ", " ' ACCT_INDEX
        '                    'End If
        '                    Dim AssetGroup As String = ""
        '                    If dsPO.Tables(0).Rows(i)("AssetGroupNo") IsNot DBNull.Value Then
        '                        AssetGroup = Common.Parse(dsPO.Tables(0).Rows(i)("AssetGroupNo"))
        '                    End If
        '                    strsql &= "'" & AssetGroup & "', " 'Common.Parse(dsPO.Tables(0).Rows(i)("AssetGroupNo")) & "', "
        '                    strsql &= "'" & AssetGroup & "' " 'Common.Parse(dsPO.Tables(0).Rows(i)("AssetGroupNo")) & "' "
        '                End If

        '                'Jules 2018.07.17 - PAMB                
        '                strsql &= ",'" & Common.Parse(dsPO.Tables(0).Rows(i)("Gift")) & "',"
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("FundType")) & "',"
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("PersonCode")) & "',"
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("ProjectCode")) & "',"
        '                strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(i)("TaxCode")) & "' "
        '                'End modification.

        '                strsql &= "FROM ADDRESS_MSTR "
        '                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
        '                strsql &= "AND AM_COY_ID = '" & companyID & "' "

        '                'Get the Address Code
        '                Dim _addrCode As String = ""
        '                Dim _strAdd() As String
        '                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(i)("DeliveryAddress").ToString.Trim) & "' "
        '                Common.Insert2Ary(strAryQuery, strsql)

        '            Next

        '            'Zulham Oct 24, 2013
        '            'For CustomFields
        '            If Not dictCustoms.Count = 0 Then
        '                Dim _countLn = 0
        '                Dim pair As New System.Collections.Generic.KeyValuePair(Of String, String)
        '                If strPONo.Trim = "To Be Allocated By System" Then
        '                    For Each pair In dictCustoms
        '                        strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
        '                        strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
        '                        strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
        '                        strsql &= rowCount & ", "  ' PR_LINE
        '                        strsql &= _countLn + 1 & ", " ' FIELD_NO
        '                        strsql &= "'" & Common.Parse(pair.Value.ToString.Trim) & "', 'PO' " ' FIELD_VALUE
        '                        Common.Insert2Ary(strAryQuery, strsql)
        '                        _countLn += 1
        '                    Next
        '                Else
        '                    For Each pair In dictCustoms
        '                        strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
        '                        strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
        '                        strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo.ToString.Trim & "' AND POM_B_COY_ID = '" & companyID & "' ), "
        '                        strsql &= rowCount & ", "  ' PR_LINE
        '                        strsql &= _countLn + 1 & ", " ' FIELD_NO
        '                        strsql &= "'" & Common.Parse(pair.Value.ToString.Trim) & "', 'PO' " ' FIELD_VALUE
        '                        Common.Insert2Ary(strAryQuery, strsql)
        '                        _countLn += 1
        '                    Next
        '                End If
        '            End If
        '            HttpContext.Current.Session("Test") = strAryQuery
        '            'End
        '            HttpContext.Current.Session("Test") = strAryQuery
        '            If strPONo = "To Be Allocated By System" Then
        '                strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PO' "
        '                Common.Insert2Ary(strAryQuery, strsql)

        '                If objDb.BatchExecute(strAryQuery) Then
        '                    insertPODetails = WheelMsgNum.Save
        '                Else
        '                    insertPODetails = WheelMsgNum.NotSave
        '                End If

        '            End If

        '        End Function
        '        'Stage 3 (Enhancement) (GST-0018) - 09/07/2015 - CH
        '        'Function to duplicate selected PO number
        '        Function DuplicatePO(ByVal strPONo As String, ByVal intPOIndex As Long) As String

        '            Dim strSql, strArySql(0), strLoginUser, strVenId As String
        '            Dim strNewPONo, strPOPrefix, strCoyID As String
        '            Dim objGlobal As New AppGlobals
        '            Dim dblTotalCost As Double
        '            Dim dteNow As DateTime = Now()

        '            strCoyID = companyID
        '            strLoginUser = userID

        '            'GetLatestDocNo
        '            objGlobal.GetLatestDocNo("PO", strArySql, strNewPONo, strPOPrefix)

        '            ' user may change last used no in company param - may cause duplication of PO NO
        '            strSql = "SELECT '*' FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(strNewPONo) & "' "
        '            strSql &= "AND POM_B_COY_ID = '" & companyID & "' "

        '            If objDb.Exist(strSql) > 0 Then
        '                ' msg retrieve eProcure modWheel
        '                Return "Duplicate transaction number found.""&vbCrLf&""Please contact your Administrator to rectify the problem."
        '            End If

        '            ' ------- check item not deleted from List Price Catalogue ----------
        '            strSql = "SELECT '*' FROM PO_DETAILS "
        '            strSql &= "WHERE POD_PO_NO = '" & Common.Parse(strPONo) & "' "
        '            strSql &= "AND POD_COY_ID = '" & strCoyID & "' "
        '            strSql &= "AND POD_PRODUCT_CODE IN ("
        '            strSql &= "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR "
        '            strSql &= "WHERE PM_DELETED = 'Y')"

        '            If objDb.Exist(strSql) > 0 Then
        '                Return "This PO cannot be duplicated because some of the items have been deleted from the List Price Catalogue."
        '            End If


        '            strVenId = objDb.GetVal("SELECT POM_S_COY_ID FROM PO_MSTR WHERE POM_PO_INDEX = " & intPOIndex)

        '            ' to check whether vendor company is inactive
        '            strSql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
        '            strSql &= "AND CM_COY_ID = '" & Common.Parse(strVenId) & "' "
        '            If objDb.Exist(strSql) > 0 Then
        '                Return "Vendor Company is currently inactive."
        '            End If

        '            ' to check whether vendor company is being deleted
        '            strSql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
        '            strSql &= "AND CM_COY_ID = '" & Common.Parse(strVenId) & "' "
        '            If objDb.Exist(strSql) > 0 Then
        '                Return "Vendor Company is being deleted."
        '            End If
        '            ' -------------------------------------------------------------------

        '            strNewPONo = Common.Parse(strNewPONo)
        '            strPOPrefix = Common.Parse(strPOPrefix)

        '            'PO_MSTR
        '            strSql = "INSERT INTO PO_MSTR (POM_PO_NO, POM_B_COY_ID, POM_BUYER_ID, POM_BUYER_NAME, POM_BUYER_PHONE, POM_BUYER_FAX, " &
        '                    "POM_CREATED_DATE, POM_CREATED_BY, POM_STATUS_CHANGED_BY, POM_STATUS_CHANGED_ON, " &
        '                    "POM_S_COY_ID, POM_S_ATTN, POM_S_COY_NAME, POM_S_ADDR_LINE1, " &
        '                    "POM_S_ADDR_LINE2, POM_S_ADDR_LINE3, POM_S_POSTCODE, POM_S_CITY, POM_S_STATE, " &
        '                    "POM_S_COUNTRY, POM_S_PHONE, POM_S_FAX, POM_S_EMAIL, " &
        '                    "POM_PAYMENT_METHOD, POM_SHIPMENT_TERM, POM_SHIPMENT_MODE, POM_CURRENCY_CODE, " &
        '                    "POM_EXCHANGE_RATE, POM_PAYMENT_TERM, POM_SHIP_VIA, POM_BILLING_METHOD, POM_PO_TYPE, POM_INTERNAL_REMARK, " &
        '                    "POM_EXTERNAL_REMARK, POM_PO_STATUS, POM_FULFILMENT, POM_PO_INDEX, POM_ARCHIVE_IND, " &
        '                    "POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SHIP_AMT, POM_PO_PREFIX, POM_B_ADDR_CODE, " &
        '                    "POM_B_ADDR_LINE1, POM_B_ADDR_LINE2, POM_B_ADDR_LINE3, POM_B_POSTCODE, " &
        '                    "POM_B_STATE, POM_B_CITY, POM_B_COUNTRY, " &
        '                    "POM_DUP_FROM, POM_EXTERNAL_IND, POM_REFERENCE_NO, " &
        '                    "POM_PO_COST, POM_RFQ_INDEX, POM_DEPT_INDEX, POM_QUOTATION_NO, POM_TERMANDCOND, POM_URGENT) " &
        '                    "SELECT '" & strNewPONo & "', POM_B_COY_ID, POM_BUYER_ID, POM_BUYER_NAME, POM_BUYER_PHONE, POM_BUYER_FAX, " &
        '                    Common.ConvertDate(dteNow) & ", '" & strLoginUser & "', '" & strLoginUser & "', " & Common.ConvertDate(dteNow) & ", " &
        '                    "POM_S_COY_ID, POM_S_ATTN, CM_COY_NAME, CM_ADDR_LINE1, " &
        '                    "CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, CM_CITY, CM_STATE, " &
        '                    "CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, " &
        '                    "POM_PAYMENT_METHOD, POM_SHIPMENT_TERM, POM_SHIPMENT_MODE, POM_CURRENCY_CODE, " &
        '                    "POM_EXCHANGE_RATE, POM_PAYMENT_TERM, POM_SHIP_VIA, POM_BILLING_METHOD, 'Y', POM_INTERNAL_REMARK, " &
        '                    "CASE POM_PO_STATUS WHEN '13' Then '' ELSE POM_EXTERNAL_REMARK END, '" & POStatus_new.Draft & "', '" & Fulfilment.null & "', NULL, '', " &
        '                    "POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SHIP_AMT, '" & strPOPrefix & "', POM_B_ADDR_CODE, " &
        '                    "POM_B_ADDR_LINE1, POM_B_ADDR_LINE2, POM_B_ADDR_LINE3, POM_B_POSTCODE, " &
        '                    "POM_B_STATE, POM_B_CITY, POM_B_COUNTRY, " &
        '                    "'', '', '', " &
        '                    "POM_PO_COST, POM_RFQ_INDEX, POM_DEPT_INDEX, POM_QUOTATION_NO, POM_TERMANDCOND, POM_URGENT " &
        '                    "FROM PO_MSTR " &
        '                    "LEFT JOIN COMPANY_MSTR ON POM_S_COY_ID = CM_COY_ID WHERE POM_PO_INDEX = " & intPOIndex
        '            Common.Insert2Ary(strArySql, strSql)

        '            'Jules 2018.07.16 - PAMB
        '            'PO DETAILS
        '            strSql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, " &
        '                    "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, POD_GST_RATE, POD_GST_INPUT_TAX_CODE, POD_TAX_VALUE, " &
        '                    "POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, " &
        '                    "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, " &
        '                    "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, " &
        '                    "POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, " &
        '                    "POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, " &
        '                    "POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE) " &
        '                    "SELECT '" & Common.Parse(strNewPONo) & "', POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, " &
        '                    "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, POD_GST_RATE, POD_GST_INPUT_TAX_CODE, POD_TAX_VALUE, " &
        '                    "POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, " &
        '                    "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, " &
        '                    "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE, " &
        '                    "POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY, POD_CD_GROUP_INDEX, POD_B_ITEM_CODE, " &
        '                    "POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_ACCT_INDEX, " &
        '                    "POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE " &
        '                    "FROM PO_DETAILS WHERE POD_PO_NO= '" & Common.Parse(strPONo) & "' " &
        '                    "AND POD_COY_ID='" & companyID & "'"
        '            Common.Insert2Ary(strArySql, strSql)


        '            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR(PCM_PR_INDEX,PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE) SELECT " _
        '            & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR),PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & intPOIndex & " AND PCM_TYPE='PO'"
        '            Common.Insert2Ary(strArySql, strSql)

        '            strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS(PCD_PR_INDEX,PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE)  SELECT " _
        '            & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR),PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE FROM PR_CUSTOM_FIELD_DETAILS " _
        '            & "WHERE PCD_PR_INDEX=" & intPOIndex & " AND PCD_TYPE='PO'"
        '            Common.Insert2Ary(strArySql, strSql)

        '            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPONo & "' AND CDA_DOC_TYPE='PO'"
        '            Dim dtAttach As DataTable
        '            Dim dr As DataRow
        '            Dim objFile As New FileManagement
        '            Dim strHubFile, strActualFile As String
        '            Dim strBasePath, strPOPath As String
        '            Dim strSourceFile, strDestFile As String

        '            strBasePath = objFile.getBasePath(EnumUploadFrom.FrontOff)
        '            strPOPath = objFile.getSystemParam("DocAttachPath", "PO")
        '            objFile.checkDirectory(strBasePath & strPOPath)
        '            dtAttach = objDb.FillDs(strSql).Tables(0)
        '            For Each dr In dtAttach.Rows
        '                If Not IsDBNull(dr("CDA_HUB_FILENAME")) Then
        '                    strSourceFile = strBasePath & strPOPath & dr("CDA_HUB_FILENAME")
        '                    strHubFile = objFile.getLastDocAttachFileName(dr("CDA_HUB_FILENAME"))
        '                    strDestFile = strBasePath & strPOPath & strHubFile
        '                    'Michelle (17/6/2010) - To solve the problem where attachment is not create when duplicating a PR
        '                    'If System.IO.File.Exists(strDestFile) Then
        '                    If Not System.IO.File.Exists(strDestFile) Then
        '                        System.IO.File.Copy(strSourceFile, strDestFile)
        '                    End If
        '                    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," _
        '                    & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) VALUES('" & strCoyID & "','" &
        '                    strNewPONo & "','PO','" & strHubFile & "','" & Common.Parse(dr("CDA_ATTACH_FILENAME")) &
        '                    "'," & dr("CDA_FILESIZE") & ",'" & Common.Parse(dr("CDA_TYPE")) & "')"
        '                    Common.Insert2Ary(strArySql, strSql)
        '                End If
        '            Next

        '            objGlobal = Nothing
        '            objFile = Nothing
        '            If objDb.BatchExecute(strArySql) Then
        '                Return "Purchase Order Number " & strNewPONo & " created."
        '            Else
        '                Return "Error occurs. Kindly contact the Administrator to resolve this. "
        '            End If
        '        End Function
        '#End Region
    End Class
End Namespace
