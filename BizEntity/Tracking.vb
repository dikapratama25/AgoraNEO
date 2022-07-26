Imports System
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.IO
Imports SSO.Component


Namespace AgoraNEO
    Public Class Tracking
        Dim objUsers As New Users
        Dim objDb As New EAD.DBCom
        'Name       : updateDocMatching
        'Author     : Moo
        'Descption  : update COMPANY_DOC_MATCH for transaction tracking purpose 
        'Remark     : 
        'ReturnValue:        
        'LastUpadte : 13 Nov 2004
        'Version    : 1.00
        Function updateDocMatching(ByVal strPONo As String, ByVal strDocNo As String, ByVal strRefDocNo As String, ByVal strDocType As String, ByVal strVendor As String, ByVal strBuyerCoy As String, Optional ByVal strBillMethod As String = "FPO") As String
            Dim strSql As String
            Dim objDB As New EAD.DBCom
            '//PO - DO - GRN - INVOICE
            Select Case strDocType
                Case "PO"
                    '//updateDocMatching(PO_No,"","","PO".....
                    '//insert
                    strSql = "INSERT INTO COMPANY_DOC_MATCH(CDM_B_COY_ID,CDM_S_COY_ID,CDM_PO_NO) VALUES('" & strBuyerCoy & "','" & strVendor & "','" & strPONo & "')"
                Case "DO"
                    '//updateDocMatching(PO_No,DO_NO,PO_NO,"DO".....
                    '//insert or update because 1 PO-many DO
                    strSql = "SELECT '*' FROM COMPANY_DOC_MATCH WHERE CDM_PO_NO='" & strPONo &
                    "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_DO_NO IS NULL"
                    If objDB.Exist(strSql) Then
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_DO_NO='" & strDocNo & "' WHERE CDM_PO_NO='" & strPONo &
                        "' AND CDM_B_COY_ID='" & strBuyerCoy & "' "
                    Else
                        strSql = "INSERT INTO COMPANY_DOC_MATCH(CDM_B_COY_ID,CDM_S_COY_ID,CDM_PO_NO,CDM_DO_NO) VALUES('" & strBuyerCoy & "','" & strVendor & "','" & strPONo & "','" & strDocNo & "')"
                    End If
                Case "GRN"
                    '//updateDocMatching(PO_No,GRN_NO,DO_NO,"GRN".....
                    '//UPdate - 1 DO - 1 GRN
                    strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_GRN_NO='" & strDocNo & "' WHERE CDM_PO_NO='" & strPONo &
                    "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_DO_NO='" & strRefDocNo & "' AND CDM_S_COY_ID='" & strVendor & "'"
                Case "INV"
                    '//updateDocMatching(PO_No,GRN_NO,DO_NO,"GRN".....
                    '//insert or update
                    'strSql = "SELECT ISNULL(CV_BILLING_METHOD,'FPO') FROM COMPANY_VENDOR WHERE CV_B_COY_ID='" & strBuyerCoy & _
                    '"' AND CV_S_COY_ID='" & strVendor & "'"
                    'strBillMethod = objDB.GetVal(strSql)
                    If strBillMethod = "FPO" Then
                        '//updateDocMatching(PO_No,INV_NO,PO_NO,"INV".....
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_INVOICE_NO='" & strDocNo & "' WHERE CDM_PO_NO='" & strPONo &
                        "' AND CDM_B_COY_ID='" & strBuyerCoy & "'"
                    ElseIf strBillMethod = "DO" Then
                        '//updateDocMatching(PO_No,INV_NO,DO_NO,"INV".....
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_INVOICE_NO='" & strDocNo & "' WHERE CDM_PO_NO='" & strPONo &
                         "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_DO_NO='" & strRefDocNo & "' AND CDM_S_COY_ID='" & strVendor & "'"
                    ElseIf strBillMethod = "GRN" Then
                        '//updateDocMatching(PO_No,INV_NO,GRN_NO,"INV".....
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_INVOICE_NO='" & strDocNo & "' WHERE CDM_PO_NO='" & strPONo &
                        "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_GRN_NO='" & strRefDocNo & "'"
                    End If
            End Select
            Return strSql
        End Function

        Function updateDocMatchingNew(ByVal strPONo As String, ByVal strDocNo As String, ByVal strRefDocNo As String, ByVal strDocType As String, ByVal strVendor As String, ByVal strBuyerCoy As String, Optional ByVal strBillMethod As String = "FPO") As String
            Dim strSql As String
            Dim objDB As New EAD.DBCom
            '//PO - DO - GRN - INVOICE
            Select Case strDocType
                Case "PO"
                    '//updateDocMatching(PO_No,"","","PO".....
                    '//insert
                    strSql = "INSERT INTO COMPANY_DOC_MATCH(CDM_B_COY_ID,CDM_S_COY_ID,CDM_PO_NO) VALUES('" & strBuyerCoy & "','" & strVendor & "'," & strPONo & ")"
                Case "DO"
                    '//updateDocMatching(PO_No,DO_NO,PO_NO,"DO".....
                    '//insert or update because 1 PO-many DO
                    strSql = "SELECT '*' FROM COMPANY_DOC_MATCH WHERE CDM_PO_NO='" & strPONo &
                    "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_DO_NO IS NULL"
                    If objDB.Exist(strSql) Then
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_DO_NO=" & strDocNo & " WHERE CDM_PO_NO='" & strPONo &
                        "' AND CDM_B_COY_ID='" & strBuyerCoy & "' "
                    Else
                        strSql = "INSERT INTO COMPANY_DOC_MATCH(CDM_B_COY_ID,CDM_S_COY_ID,CDM_PO_NO,CDM_DO_NO) VALUES('" & strBuyerCoy & "','" & strVendor & "','" & strPONo & "'," & strDocNo & ")"
                    End If
                Case "GRN"
                    '//updateDocMatching(PO_No,GRN_NO,DO_NO,"GRN".....
                    '//UPdate - 1 DO - 1 GRN
                    strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_GRN_NO=" & strDocNo & " WHERE CDM_PO_NO='" & strPONo &
                    "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_DO_NO='" & strRefDocNo & "' AND CDM_S_COY_ID='" & strVendor & "'"
                Case "INV"
                    '//updateDocMatching(PO_No,GRN_NO,DO_NO,"GRN".....
                    '//insert or update
                    'strSql = "SELECT ISNULL(CV_BILLING_METHOD,'FPO') FROM COMPANY_VENDOR WHERE CV_B_COY_ID='" & strBuyerCoy & _
                    '"' AND CV_S_COY_ID='" & strVendor & "'"
                    'strBillMethod = objDB.GetVal(strSql)
                    If strBillMethod = "FPO" Then
                        '//updateDocMatching(PO_No,INV_NO,PO_NO,"INV".....
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_INVOICE_NO=" & strDocNo & " WHERE CDM_PO_NO='" & strPONo &
                        "' AND CDM_B_COY_ID='" & strBuyerCoy & "'"
                    ElseIf strBillMethod = "DO" Then
                        '//updateDocMatching(PO_No,INV_NO,DO_NO,"INV".....
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_INVOICE_NO=" & strDocNo & " WHERE CDM_PO_NO='" & strPONo &
                         "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_DO_NO='" & strRefDocNo & "' AND CDM_S_COY_ID='" & strVendor & "'"
                    ElseIf strBillMethod = "GRN" Then
                        '//updateDocMatching(PO_No,INV_NO,GRN_NO,"INV".....
                        strSql = "UPDATE COMPANY_DOC_MATCH SET CDM_INVOICE_NO=" & strDocNo & " WHERE CDM_PO_NO='" & strPONo &
                        "' AND CDM_B_COY_ID='" & strBuyerCoy & "' AND CDM_GRN_NO='" & strRefDocNo & "'"
                    End If
            End Select
            Return strSql
        End Function

        Public Function getRelatedPR(ByVal strDOCNo As String, ByRef PRNo() As String, ByRef PRNoBuyer() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = " SELECT DISTINCT PRD_PR_NO, (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PR_MSTR.PRM_BUYER_ID AND " _
                    & " UM_COY_ID = PR_MSTR.PRM_COY_ID) AS PRM_BUYER_ID " _
                    & " FROM PR_DETAILS, PR_MSTR " _
                    & " WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_CONVERT_TO_DOC = (SELECT RM_RFQ_NO FROM RFQ_MSTR WHERE RM_RFQ_ID = '" & Common.Parse(strDOCNo) & "' AND RM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') " _
                    & " AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_PR_NO")
                PRNoBuyer(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_BUYER_ID")
                COUNT = COUNT + 1
            Next
        End Function

        Public Function getRelatedPRAndOld(ByVal strDOCNo As String, ByRef PRNo() As String, ByRef PRNoBuyer() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = " SELECT DISTINCT PRD_PR_NO, (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PR_MSTR.PRM_BUYER_ID AND " _
                    & " UM_COY_ID = PR_MSTR.PRM_COY_ID) AS PRM_BUYER_ID " _
                    & " FROM PR_DETAILS, PR_MSTR " _
                    & " WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_CONVERT_TO_DOC = (SELECT RM_RFQ_NO FROM RFQ_MSTR WHERE RM_RFQ_ID = '" & Common.Parse(strDOCNo) & "' AND RM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') " _
                    & " AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            strSQL = strSQL + " UNION SELECT DISTINCT PRD_PR_NO, (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PR_MSTR.PRM_BUYER_ID AND " _
                    & " UM_COY_ID = PR_MSTR.PRM_COY_ID) AS PRM_BUYER_ID " _
                    & " FROM PR_DETAILS, PR_MSTR " _
                    & " WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRM_RFQ_INDEX = '" & Common.Parse(strDOCNo) & "' " _
                    & " AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_PR_NO")
                PRNoBuyer(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_BUYER_ID")
                COUNT = COUNT + 1
            Next
        End Function

        Public Function getRelatedPR_PO(ByVal strDOCNo As String, ByRef PRNo() As String, ByRef PRNoBuyer() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = " SELECT DISTINCT PRD_PR_NO, (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PR_MSTR.PRM_BUYER_ID AND " _
                    & " UM_COY_ID = PR_MSTR.PRM_COY_ID) AS PRM_BUYER_ID " _
                    & " FROM PR_DETAILS, PR_MSTR " _
                    & " WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_CONVERT_TO_DOC = '" & Common.Parse(strDOCNo) & "' " _
                    & " AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_PR_NO")
                PRNoBuyer(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_BUYER_ID")
                COUNT = COUNT + 1
            Next
        End Function

        Function getRelatedPR(ByVal strPOIndex As String) As DataTable
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom
            strSql = "SELECT PRM_PR_INDEX,PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX=" & strPOIndex
            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function getRelatedPR_PO(ByVal strPONo As String, ByVal strBCoyID As String) As DataTable 'Michelle (1/8/2007) - To cater for 1 PR with multi POs
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom
            strSql = "SELECT PRM_PR_INDEX,PRM_PR_NO FROM PR_MSTR WHERE PRM_COY_ID = '" & strBCoyID & "' AND PRM_PR_INDEX IN (SELECT DISTINCT(POD_PR_INDEX) FROM PO_DETAILS WHERE POD_PO_NO = '" & strPONo & "' AND POD_COY_ID = '" & strBCoyID & "')"
            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function getRelated_DocMatchForPO(ByVal strPONo As String, ByVal strSCoyID As String, Optional ByVal strBCoyID As String = "", Optional ByVal strType As String = "", Optional ByVal strGRNNo As String = "", Optional ByVal strDONo As String = "") As DataTable 'Michelle (1/8/2007) - To cater for 1 PR with multi POs
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom

            If strType = "GRN" Then
                strSql = " SELECT DISTINCT PO1.CDM_GRN_NO AS CDM_NO, PO1.CDM_DO_NO AS CDM_DO_NO, CDM_S_COY_ID " _
                        & "FROM COMPANY_DOC_MATCH PO1 WHERE PO1.CDM_S_COY_ID = '" & Common.Parse(strSCoyID) & "' AND PO1.CDM_PO_NO = '" & Common.Parse(strPONo) & "' AND (PO1.CDM_GRN_NO IS NOT NULL OR PO1.CDM_DO_NO IS NOT NULL) AND PO1.CDM_B_COY_ID = '" & Common.Parse(strBCoyID) & "' " _
                        & "AND CDM_GRN_NO='" & Common.Parse(strGRNNo) & "' AND CDM_DO_NO='" & Common.Parse((strDONo)) & "'"

            ElseIf strType = "DO" Then
                strSql = " SELECT DISTINCT PO1.CDM_GRN_NO AS CDM_NO, PO1.CDM_DO_NO AS CDM_DO_NO, CDM_S_COY_ID " _
                        & "FROM COMPANY_DOC_MATCH PO1 WHERE PO1.CDM_S_COY_ID = '" & Common.Parse(strSCoyID) & "' AND PO1.CDM_PO_NO = '" & Common.Parse(strPONo) & "' AND (PO1.CDM_GRN_NO IS NOT NULL OR PO1.CDM_DO_NO IS NOT NULL) AND PO1.CDM_B_COY_ID = '" & Common.Parse(strBCoyID) & "' " _
                        & "AND CDM_DO_NO='" & Common.Parse((strDONo)) & "'"

            Else
                strSql = " SELECT DISTINCT PO1.CDM_GRN_NO AS CDM_NO, PO1.CDM_DO_NO AS CDM_DO_NO, CDM_S_COY_ID FROM COMPANY_DOC_MATCH PO1 WHERE PO1.CDM_S_COY_ID = '" & Common.Parse(strSCoyID) & "' AND PO1.CDM_PO_NO = '" & Common.Parse(strPONo) & "' AND (PO1.CDM_GRN_NO IS NOT NULL OR PO1.CDM_DO_NO IS NOT NULL) AND PO1.CDM_B_COY_ID = '" & Common.Parse(strBCoyID) & "' "

            End If

            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function getRelated_DocMatch(ByVal strInvNo As String, ByVal strBCoyID As String, ByVal strCoy As String) As DataTable 'Michelle (1/8/2007) - To cater for 1 PR with multi POs
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom

            strSql = " SELECT DISTINCT DO1.CDM_DO_NO AS CDM_NO, 'DO' AS TYPE, CDM_S_COY_ID FROM COMPANY_DOC_MATCH DO1 WHERE DO1.CDM_B_COY_ID = '" & strCoy & "' AND DO1.CDM_S_COY_ID = '" & strBCoyID & "' AND DO1.CDM_INVOICE_NO = '" & strInvNo & "' "

            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function getRelated_DocMatchWithDO(ByVal strInvNo As String, ByVal strBCoyID As String, ByVal strDo As String, ByVal strCoy As String) As DataTable 'Michelle (1/8/2007) - To cater for 1 PR with multi POs
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom

            strSql = " SELECT DISTINCT GRN.CDM_GRN_NO AS CDM_NO, 'GRN' AS TYPE, CDM_S_COY_ID FROM COMPANY_DOC_MATCH GRN WHERE GRN.CDM_B_COY_ID = '" & strCoy & "' AND GRN.CDM_S_COY_ID = '" & strBCoyID & "' AND GRN.CDM_DO_NO = '" & strDo & "' AND GRN.CDM_INVOICE_NO = '" & strInvNo & "' "

            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function getRelatedCR(ByVal strPOIndex As String) As DataTable
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom
            strSql = "SELECT * FROM PO_CR_MSTR WHERE PCM_PO_INDEX=" & strPOIndex
            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function getRelatedCR_PO(ByVal strPONo As String, ByVal strBCoyID As String) As DataTable 'Michelle (1/8/2007) - To cater for 1 PR with multi POs
            Dim strSql As String
            Dim ds As DataSet
            Dim objDB As New EAD.DBCom
            strSql = "SELECT * FROM PO_CR_MSTR WHERE PCM_PO_INDEX= (SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' AND POM_B_COY_ID = '" & strBCoyID & "')"
            ds = objDB.FillDs(strSql)
            Return ds.Tables(0)
        End Function

        Function TransTracking(ByVal strDocType As String, ByVal strDocNo As String, ByVal strVendor As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strBuyer As String, ByVal strDept As String) As DataSet
            '//Use Data Shaping because it is difficut to filter second table if we use DataRelation of ADO.Net
            Dim strSql, strSql_1, strSql_2, strCoyID As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            Dim strTemp As String
            Select Case strDocType
                Case "PR"
                    '//join table to get PO Number

                    '-----------New code Adding to get the multiple POS by praveen 
                    'Michelle (CR0046) - To take the PO index from the PO mstr table, instead of the PR mstr for multiple POs

                    'strSql_1 = "SHAPE {select distinct PRM_PR_INDEX AS DOC_INDEX,PRM_PR_NO AS DOC_NO,PRM_PR_DATE as DOC_DATE," & _
                    '           " PRM_S_COY_ID AS VEN_ID, PRM_S_COY_NAME AS COY_NAME,PRM_PR_COST as COST," & _
                    '           " PRM_REQ_NAME AS BUYER,POD_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID,CDM_DEPT_INDEX from " & _
                    '           " PO_DETAILS,PR_MSTR,PO_MSTR inner join COMPANY_DePT_MSTR on " & _
                    '           "  PO_MSTR.POM_DEPT_INDEX = company_dept_mstr.CDM_DEPT_INDEX " & _
                    '           " where PR_MSTR.PRM_PO_INDEX=PO_MSTR.POM_PO_INDEX and  PR_MSTR.PRM_PR_INDEX = PO_DETAILS.POD_PR_INDEX  and PRM_COY_ID='" & strCoyID & "' " & _
                    '           "  and PRM_PO_INDEX IS NOT NULL "
                    'strSql_1 = "select distinct PRM_PR_INDEX AS DOC_INDEX,PRM_PR_NO AS DOC_NO,PRM_PR_DATE as DOC_DATE," & _
                    '           " PRM_S_COY_ID AS VEN_ID, PRM_S_COY_NAME AS COY_NAME,PRM_PR_COST as COST," & _
                    '           " PRM_REQ_NAME AS BUYER,POD_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID,CDM_DEPT_INDEX from " & _
                    '           " PO_DETAILS,PR_MSTR,PO_MSTR inner join COMPANY_DePT_MSTR on " & _
                    '           "  PO_MSTR.POM_DEPT_INDEX = company_dept_mstr.CDM_DEPT_INDEX " & _
                    '           " where PO_DETAILS.POD_PO_NO=PO_MSTR.POM_PO_NO and  PR_MSTR.PRM_PR_INDEX = PO_DETAILS.POD_PR_INDEX  and PRM_COY_ID='" & strCoyID & "' " & _
                    '           "  and PRM_PO_INDEX IS NOT NULL "
                    '" and PRM_PR_NO = 'PR010000532' and PRM_PR_NO = 'PR010000532' AND (PRM_PR_NO = 'PR010000532' OR POM_PO_NO = 'PR010000532' " & _
                    '" OR PO_MSTR.POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO = 'PR010000532') " & _
                    '" OR  PO_MSTR.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO = 'PR010000532') " & _
                    '" OR PO_MSTR.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO = 'PR010000532')} "
                    'If strDocNo <> "" Then
                    '    strTemp = Common.BuildWildCard(strDocNo)
                    '    strSql_1 = strSql_1 & " AND (PRM_PR_NO" & Common.ParseSQL(strTemp) & " OR PRM_PR_NO" & _
                    '    Common.ParseSQL(strTemp) & " OR PO_MSTR.POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR  PO_MSTR.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR PO_MSTR.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" & _
                    '    Common.ParseSQL(strTemp) & ")) "
                    'End If
                    '------------End the code 
                    '--------------The following query is commented by praveen on 25/07/07 to  
                    'strSql_1 = "SHAPE {SELECT PRM_PR_Index AS DOC_INDEX,PRM_PR_NO AS DOC_NO,PRM_PR_DATE AS DOC_DATE ," _
                    '& "PRM_S_COY_ID AS VEN_ID,PRM_S_COY_NAME AS COY_NAME,PRM_PR_COST AS COST,PRM_REQ_NAME AS BUYER," _
                    '& "POM_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID " _
                    '& "FROM PR_MSTR A, PO_MSTR B  LEFT OUTER JOIN COMPANY_DEPT_MSTR " _
                    '& "ON POM_DEPT_INDEX=CDM_DEPT_INDEX WHERE A.PRM_PO_INDEX=B.POM_PO_INDEX AND PRM_COY_ID='" & strCoyID & _
                    '"' AND PRM_PO_INDEX IS NOT NULL"
                    'If strDocNo <> "" Then
                    '    strSql_1 = strSql_1 & " AND (PRM_PR_NO" & Common.ParseSQL(strDocNo) & " OR POM_PO_NO" & _
                    '    Common.ParseSQL(strDocNo) & " OR B.POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & _
                    '    Common.ParseSQL(strDocNo) & ") OR  B.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    '    Common.ParseSQL(strDocNo) & ") OR B.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" & _
                    '    Common.ParseSQL(strDocNo) & "))"
                    'End If
                    '------------end comenting  the code by praveen 
                    'Change PR query by craven 26-07-2011
                    'strSql_1 = "SELECT POM_B_COY_ID, A.PRM_PR_INDEX AS DOC_INDEX,A.PRM_S_COY_NAME AS COY_NAME, A.PRM_PR_NO AS DOC_NO,A.PRM_PR_COST AS COST,A.PRM_CREATED_DATE AS DOC_DATE,A.PRM_S_COY_ID AS VEN_ID, A.PRM_REQ_NAME AS BUYER,A.CDM_DEPT_NAME AS DEPT, " & _
                    '"POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM (SELECT * FROM pr_mstr INNER JOIN company_dept_mstr ON prm_dept_index = cdm_dept_index) A INNER JOIN po_mstr ON A.prm_po_index = pom_po_index INNER JOIN PR_DETAILS ON PRM_PR_INDEX = PRD_PR_LINE_INDEX WHERE A.PRM_COY_ID = '" & strCoyID & "' AND POM_FULFILMENT != 0"

                    'Michelle (29/9/2011) - Missing record
                    'Zulham 30112018
                    strSql_1 = "SELECT DISTINCT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME, PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID, PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT, " &
                    "POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM PR_MSTR INNER JOIN po_details ON prm_pr_index = pod_pr_index INNER JOIN po_mstr ON POM_B_COY_ID = POD_COY_ID AND pom_po_no = pod_po_no LEFT OUTER JOIN company_dept_mstr ON prm_dept_index = cdm_dept_index " &
                    " WHERE PRM_COY_ID = '" & strCoyID & "' AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    'Michelle (30/1/2012) - Issue 1458 
                    strSql_1 = strSql_1 & " AND PRM_PR_TYPE = 'CC' "

                    'If strVendor <> "" Then
                    '    strTemp = Common.BuildWildCard(strVendor)
                    '    strSql_1 = strSql_1 & " AND PRM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    'End If
                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND PRM_PR_NO" & Common.ParseSQL(strTemp)
                    End If
                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If
                    'Query Old data have to ignore the convert PO data
                    'strSql_1 = strSql_1 & " AND PRD_CONVERT_TO_IND = (NULL)"

                    'For New Data
                    'Michelle (15/12/2011) - Issue 1385
                    'strSql_1 = strSql_1 & " UNION SELECT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," & _
                    '" PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID,POM_BUYER_NAME AS BUYER," & _
                    '" CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN (SELECT DISTINCT PRD_PR_NO,prd_convert_to_ind,POM_B_COY_ID,POM_S_COY_NAME," & _
                    '" POM_BUYER_NAME,CDM_DEPT_NAME,POM_PO_NO,POM_PO_INDEX FROM pr_details INNER JOIN po_mstr ON prd_convert_to_doc=pom_po_no" & _
                    '" LEFT JOIN company_dept_mstr ON POM_DEPT_INDEX = CDM_DEPT_INDEX WHERE prd_convert_to_ind ='PO' " & _
                    '" AND PRD_COY_ID ='" & strCoyID & "' AND pom_fulfilment != 0 AND POM_B_COY_ID='" & strCoyID & "'"

                    'Michelle (10/2/2012) - To get the department based on the PR
                    ' strSql_1 = strSql_1 & " UNION SELECT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," & _
                    '" PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID,PRM_REQ_NAME AS BUYER," & _
                    '" CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN (SELECT DISTINCT PRD_PR_NO,prd_convert_to_ind,POM_B_COY_ID,POM_S_COY_NAME," & _
                    '" POM_BUYER_NAME,CDM_DEPT_NAME,POM_PO_NO,POM_PO_INDEX FROM pr_details INNER JOIN po_mstr ON prd_convert_to_doc=pom_po_no" & _
                    '" LEFT JOIN company_dept_mstr ON POM_DEPT_INDEX = CDM_DEPT_INDEX WHERE prd_convert_to_ind ='PO' " & _
                    '" AND PRD_COY_ID ='" & strCoyID & "' AND pom_fulfilment != 0 AND POM_B_COY_ID='" & strCoyID & "'"
                    'Zulham 30112018
                    strSql_1 = strSql_1 & " UNION SELECT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," &
                   " PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID,PRM_REQ_NAME AS BUYER," &
                   " CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN (SELECT DISTINCT PRD_PR_NO,prd_convert_to_ind,POM_B_COY_ID,POM_S_COY_NAME," &
                   " POM_BUYER_NAME,POM_PO_NO,POM_PO_INDEX FROM pr_details INNER JOIN po_mstr ON prd_convert_to_doc=pom_po_no" &
                   " WHERE prd_convert_to_ind ='PO' " &
                   " AND PRD_COY_ID ='" & strCoyID & "' AND pom_fulfilment != 0 AND POM_B_COY_ID='" & strCoyID & "' AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    strSql_1 = strSql_1 & ") a ON a.PRD_PR_NO = PRM_PR_NO LEFT JOIN company_dept_mstr ON PRM_DEPT_INDEX = CDM_DEPT_INDEX WHERE prd_convert_to_ind = 'PO' AND PRM_COY_ID='" & strCoyID & "' "

                    'Michelle (30/1/2012) - Issue 1458 
                    strSql_1 = strSql_1 & " AND PRM_PR_TYPE = 'CC' "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND PRD_PR_NO" & Common.ParseSQL(strTemp)
                    End If
                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If

                    '   strSql_1 = strSql_1 & ") a ON a.PRD_PR_NO = PRM_PR_NO WHERE prd_convert_to_ind = 'PO' AND PRM_COY_ID='" & strCoyID & "'"


                    'FOR PR convert to RFQ
                    'Michelle (15/12/2011) - Issue 1391
                    'strSql_1 = strSql_1 & " UNION SELECT DISTINCT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," & _
                    '"PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID," & _
                    '"PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN" & _
                    '"(SELECT DISTINCT POM_B_COY_ID,POM_S_COY_NAME,POM_BUYER_NAME,POM_PO_NO,POM_PO_INDEX,CDM_DEPT_NAME,prd_pr_no" & _
                    '" FROM pr_details INNER JOIN rfq_mstr ON prd_convert_to_doc=rm_rfq_no INNER JOIN rfq_replies_mstr ON rm_rfq_id = rrm_rfq_id" & _
                    '" INNER JOIN po_mstr ON pom_quotation_no = rrm_actual_quot_num LEFT JOIN company_dept_mstr ON POM_DEPT_INDEX = CDM_DEPT_INDEX" & _
                    '" WHERE prd_coy_id='" & strCoyID & "' AND rm_coy_id='" & strCoyID & "' AND pom_b_coy_id='" & strCoyID & "' AND prd_convert_to_ind = 'RFQ'"

                    'Michelle (10/2/2012) - To get the department based on the PR
                    'strSql_1 = strSql_1 & " UNION SELECT DISTINCT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," & _
                    '"PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID," & _
                    '"PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN" & _
                    '"(SELECT DISTINCT POM_B_COY_ID,POM_S_COY_NAME,POM_BUYER_NAME,POM_PO_NO,POM_PO_INDEX,CDM_DEPT_NAME,prd_pr_no" & _
                    '" FROM pr_details INNER JOIN rfq_mstr ON prd_convert_to_doc=rm_rfq_no " & _
                    '" INNER JOIN po_mstr FORCE INDEX (idx_po_mstr2) ON pom_rfq_index = rm_rfq_id LEFT JOIN company_dept_mstr ON POM_DEPT_INDEX = CDM_DEPT_INDEX" & _
                    '" WHERE prd_coy_id='" & strCoyID & "' AND rm_coy_id='" & strCoyID & "' AND pom_b_coy_id='" & strCoyID & "' AND prd_convert_to_ind = 'RFQ'"
                    'Zulham 30112018
                    strSql_1 = strSql_1 & " UNION SELECT DISTINCT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," &
                    "PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID," &
                    "PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN" &
                    "(SELECT DISTINCT POM_B_COY_ID,POM_S_COY_NAME,POM_BUYER_NAME,POM_PO_NO,POM_PO_INDEX,prd_pr_no" &
                    " FROM pr_details INNER JOIN rfq_mstr ON prd_convert_to_doc=rm_rfq_no " &
                    " INNER JOIN po_mstr FORCE INDEX (idx_po_mstr2) ON pom_rfq_index = rm_rfq_id " &
                    " WHERE prd_coy_id='" & strCoyID & "' AND rm_coy_id='" & strCoyID & "' AND pom_b_coy_id='" & strCoyID & "' AND prd_convert_to_ind = 'RFQ' AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    strSql_1 = strSql_1 & ") a ON a.PRD_PR_NO = PRM_PR_NO LEFT JOIN company_dept_mstr ON PRM_DEPT_INDEX = CDM_DEPT_INDEX WHERE PRM_COY_ID='" & strCoyID & "' "

                    'Michelle (30/1/2012) - Issue 1458 
                    strSql_1 = strSql_1 & " AND PRM_PR_TYPE = 'CC' "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND PRD_PR_NO" & Common.ParseSQL(strTemp)
                    End If
                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If

                    '          strSql_1 = strSql_1 & ") a ON a.PRD_PR_NO = PRM_PR_NO WHERE PRM_COY_ID='" & strCoyID & "'"

                    'strSql_1 = strSql_1 & ") A inner join "

                    'strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "') B ON A.PO_NO = B.CDM_PO_NO AND A.POM_B_COY_ID = B.CDM_B_COY_ID"
                    'strSql_2 = " GROUP BY POM_B_COY_ID, DOC_INDEX,COY_NAME, DOC_NO,COST,DOC_DATE,VEN_ID,BUYER, DEPT, PO_NUMBER, POM_PO_INDEX"

                    strSql_2 = ""
                    'strParentCol = "POM_PO_NO"
                    'strChildCol = "CDM_PO_NO"

                    'Michelle (30/1/2012) - Issue 1458
                Case "NPR" 'Non Contract PR
                    'For PR converted to PO directly
                    'strSql_1 = "SELECT DISTINCT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME, PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID, PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT, " & _
                    '"CAST(POM_PO_NO AS CHAR) AS PO_NUMBER,CAST(POM_PO_INDEX AS CHAR) AS POM_PO_INDEX, '' AS RFQ_NO, '' AS RFQ_ID, CAST(POM_PO_STATUS AS CHAR) AS PO_STATUS, '' AS RM_STATUS FROM PR_MSTR INNER JOIN po_details ON prm_pr_index = pod_pr_index INNER JOIN po_mstr ON POM_B_COY_ID = POD_COY_ID AND pom_po_no = pod_po_no LEFT OUTER JOIN company_dept_mstr ON prm_dept_index = cdm_dept_index " & _
                    '" WHERE PRM_COY_ID = '" & strCoyID & "' AND PRM_PR_TYPE <> 'CC' "
                    'Zulham 30112018
                    strSql_1 = "SELECT DISTINCT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME, PRM_PR_NO AS DOC_NO, PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID, PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT, " &
                    "CAST(POM_PO_NO AS CHAR) AS PO_NUMBER,CAST(POM_PO_INDEX AS CHAR) AS POM_PO_INDEX, '' AS RFQ_NO, '' AS RFQ_ID, CAST(POM_PO_STATUS AS CHAR) AS PO_STATUS, '' AS RM_STATUS FROM PR_MSTR INNER JOIN po_mstr ON POM_B_COY_ID = PRM_COY_ID AND POM_PO_INDEX = PRM_PO_INDEX INNER JOIN po_details ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID LEFT OUTER JOIN company_dept_mstr ON prm_dept_index = cdm_dept_index " &
                    "WHERE PRM_COY_ID = '" & strCoyID & "' AND PRM_PR_TYPE <> 'CC' AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND PRM_PR_NO" & Common.ParseSQL(strTemp)
                    End If
                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If

                    ' strSql_1 = strSql_1 & " UNION SELECT PRM_CREATED_DATE, POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," & _
                    '" PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID,PRM_REQ_NAME AS BUYER," & _
                    '" CDM_DEPT_NAME AS DEPT,POM_PO_NO AS PO_NUMBER,POM_PO_INDEX FROM pr_mstr INNER JOIN (SELECT DISTINCT PRD_PR_NO,prd_convert_to_ind,POM_B_COY_ID,POM_S_COY_NAME," & _
                    '" POM_BUYER_NAME,CDM_DEPT_NAME,POM_PO_NO,POM_PO_INDEX FROM pr_details INNER JOIN po_mstr ON prd_convert_to_doc=pom_po_no" & _
                    '" LEFT JOIN company_dept_mstr ON POM_DEPT_INDEX = CDM_DEPT_INDEX WHERE prd_convert_to_ind ='PO' " & _
                    '" AND PRD_COY_ID ='" & strCoyID & "' AND pom_fulfilment != 0 AND POM_B_COY_ID='" & strCoyID & "'"

                    ' strSql_1 = strSql_1 & ") a ON a.PRD_PR_NO = PRM_PR_NO WHERE prd_convert_to_ind = 'PO' AND PRM_COY_ID='" & strCoyID & "' AND PRM_PR_TYPE <> 'CC' "

                    ' If strDocNo <> "" Then
                    '     strTemp = Common.BuildWildCard(strDocNo)
                    '     strSql_1 = strSql_1 & " AND PRD_PR_NO" & Common.ParseSQL(strTemp)
                    ' End If
                    ' If dteDateFr <> "" And dteDateTo <> "" Then
                    '     strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") & _
                    '     " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    ' End If

                    ' If strBuyer <> "" Then
                    '     strTemp = Common.BuildWildCard(strBuyer)
                    '     strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    ' End If

                    ' If strDept <> "" Then
                    '     strTemp = Common.BuildWildCard(strDept)
                    '     strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    ' End If
                    'Zulham 30112018
                    'FOR PR convert to RFQ
                    strSql_1 = strSql_1 & " UNION SELECT DISTINCT PRM_CREATED_DATE, A.POM_B_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," &
                    "PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID," &
                    "PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT,CAST(A.POM_PO_NO AS CHAR)AS PO_NUMBER,CAST(A.POM_PO_INDEX AS CHAR) AS POM_PO_INDEX, A.RM_RFQ_NO AS RFQ_NO, CAST(A.RM_RFQ_ID AS CHAR) AS RFQ_ID, CAST(A.POM_PO_STATUS AS CHAR), CAST(A.RM_STATUS AS CHAR) FROM pr_mstr " &
                    "INNER JOIN (SELECT DISTINCT PRD_PR_NO, RM_RFQ_NO, POM_B_COY_ID, POM_PO_NO, POM_PO_INDEX, PRD_COY_ID, RM_RFQ_ID, POM_PO_STATUS, RM_STATUS  FROM PR_DETAILS " &
                    "INNER JOIN RFQ_MSTR ON PRD_CONVERT_TO_DOC = RM_RFQ_NO " &
                    " LEFT JOIN PO_MSTR FORCE INDEX (idx_po_mstr2) ON POM_RFQ_INDEX = RM_RFQ_ID AND pom_b_coy_id='" & strCoyID & "' " &
                    " WHERE prd_coy_id='" & strCoyID & "' AND rm_coy_id='" & strCoyID & "' AND PRD_CONVERT_TO_IND = 'RFQ' AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    strSql_1 = strSql_1 & ") a ON a.PRD_PR_NO = PRM_PR_NO AND A.PRD_COY_ID = PRM_COY_ID LEFT JOIN company_dept_mstr ON PRM_DEPT_INDEX = CDM_DEPT_INDEX WHERE PRM_PR_TYPE <> 'CC' "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND PRD_PR_NO" & Common.ParseSQL(strTemp)
                    End If
                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If

                    'FOR Approved PR withoutout conversion yet
                    strSql_1 = strSql_1 & " UNION SELECT DISTINCT PRM_CREATED_DATE, PRM_COY_ID, PRM_PR_INDEX AS DOC_INDEX,'' AS COY_NAME," &
                    "PRM_PR_NO AS DOC_NO,PRM_PR_COST AS COST,PRM_CREATED_DATE AS DOC_DATE,PRM_S_COY_ID AS VEN_ID," &
                    "PRM_REQ_NAME AS BUYER,CDM_DEPT_NAME AS DEPT,'' AS PO_NUMBER,null AS POM_PO_INDEX, '' AS RFQ_NO, null AS RFQ_ID, '', '' FROM pr_mstr " &
                    "INNER JOIN PR_DETAILS ON PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID LEFT JOIN company_dept_mstr ON PrM_DEPT_INDEX = CDM_DEPT_INDEX" &
                    " WHERE PRM_COY_ID='" & strCoyID & "' AND PRM_PR_STATUS = '4' AND PRD_CONVERT_TO_IND IS NULL AND PRM_PR_TYPE <> 'CC'  AND PRM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND PRD_PR_NO" & Common.ParseSQL(strTemp)
                    End If
                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND PRM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND PRM_REQ_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If


                    '  strSql_2 = " GROUP BY POM_B_COY_ID, DOC_INDEX,COY_NAME, DOC_NO,COST,DOC_DATE,VEN_ID,BUYER, DEPT, PO_NUMBER, POM_PO_INDEX"
                    strSql_2 = ""

                Case "PO"
                    'If strDocNo <> "" Then
                    strTemp = Common.BuildWildCard(strDocNo)
                    'Michelle (29/9/2011)
                    'Zulham 30112018
                    strSql_1 = "select IFNULL(CDM_PO_NO,'') AS CDM_PO_NO,IFNULL(CDM_DO_NO,'') AS CDM_DO_NO, IFNULL(CDM_GRN_NO,'') AS CDM_GRN_NO,IFNULL(CDM_INVOICE_NO,'') AS CDM_INVOICE_NO, PCM_CR_NO,CDM_DEPT_NAME AS DEPT, POM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_NO AS PO_NUMBER, " _
                            & "POM_PO_DATE AS DOC_DATE, POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME," _
                            & "POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER,POM_PO_NO AS PO_NO,POM_B_COY_ID FROM PO_MSTR " _
                            & "LEFT JOIN  COMPANY_DOC_MATCH ON POM_B_COY_ID = CDM_B_COY_ID AND CDM_PO_NO = POM_PO_NO " _
                            & "LEFT OUTER JOIN COMPANY_DEPT_MSTR A ON A.CDM_DEPT_INDEX=POM_DEPT_INDEX AND A.CDM_COY_ID = '" & strCoyID & "' " _
                            & "LEFT JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM_PO_INDEX  AND PCM_B_COY_ID = '" & strCoyID & "' " _
                            & " WHERE POM_B_COY_ID='" & strCoyID & "' AND POM_PO_DATE IS NOT NULL AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    ' '' strSql_1 = "select PCM_CR_NO, A.*, B.* from (SELECT DISTINCT B.POM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_NO AS PO_NUMBER,POM_PO_DATE AS DOC_DATE, " _
                    ' ''& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME,POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER," _
                    ' ''& "POM_PO_NO AS PO_NO,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID FROM PO_MSTR B LEFT OUTER JOIN COMPANY_DEPT_MSTR " _
                    ' ''& "ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    ' ''& "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " _
                    ' ''& "LEFT OUTER JOIN PR_MSTR ON POD_PR_INDEX = PRM_PR_INDEX " _
                    ' ''& "WHERE POM_B_COY_ID='" & strCoyID & "' AND POD_COY_ID = '" & strCoyID & "'"

                    ' '' 'strSql_1 = "SHAPE {SELECT DISTINCT A.PRM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_DATE AS DOC_DATE, " _
                    ' '' '& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME,POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER," _
                    ' '' '& "POM_PO_NO AS PO_NO,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID FROM PO_DETAILS C, PR_MSTR A,PO_MSTR B LEFT OUTER JOIN COMPANY_DEPT_MSTR " _
                    ' '' '& "ON POM_DEPT_INDEX=CDM_DEPT_INDEX WHERE POM_B_COY_ID='" & strCoyID & "' AND B.POM_PO_NO = C.POD_PO_NO AND C.POD_COY_ID = '" & strCoyID & "'" & " AND C.POD_PR_INDEX = A.PRM_PR_INDEX"

                    ' '' strSql_1 = strSql_1 & " AND (POM_PO_NO" & Common.ParseSQL(strTemp) & " OR PRM_PR_NO" & _
                    ' '' Common.ParseSQL(strTemp) & " OR B.POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & _
                    ' '' Common.ParseSQL(strTemp) & ") OR B.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    ' '' Common.ParseSQL(strTemp) & ") OR B.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" & _
                    ' '' Common.ParseSQL(strTemp) & ")) "
                    ' Else
                    ' strSql_1 = "select A.*, B.* from (SELECT DISTINCT B.POM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_DATE AS DOC_DATE, " _
                    ' & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME,POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER," _
                    ' & "POM_PO_NO AS PO_NO,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID  FROM PO_MSTR B LEFT OUTER JOIN COMPANY_DEPT_MSTR " _
                    ' & "ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    '& "INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " _
                    '& "LEFT OUTER JOIN PR_MSTR ON POD_PR_INDEX = PRM_PR_INDEX " _
                    ' & "WHERE POM_B_COY_ID='" & strCoyID & "' "
                    ' '& "WHERE POM_B_COY_ID='" & strCoyID & "' and PRM_PO_INDEX IS NOT NULL"

                    ' 'strSql_1 = "SHAPE{SELECT DISTINCT A.PRM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_DATE AS DOC_DATE, " _
                    ' '& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME,POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER," _
                    ' '& "POM_PO_NO AS PO_NO,CDM_DEPT_NAME AS DEPT,POM_B_COY_ID  FROM PR_MSTR A,PO_MSTR B LEFT OUTER JOIN COMPANY_DEPT_MSTR " _
                    ' '& "ON POM_DEPT_INDEX=CDM_DEPT_INDEX WHERE  A.PRM_PO_INDEX=B.POM_PO_INDEX and POM_B_COY_ID='" & strCoyID & "' and PRM_PO_INDEX IS NOT NULL"

                    ' End If

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND (POM_PO_NO" & Common.ParseSQL(strTemp)
                        strSql_1 = strSql_1 & " OR CDM_DO_NO" & Common.ParseSQL(strTemp)
                        strSql_1 = strSql_1 & " OR CDM_GRN_NO" & Common.ParseSQL(strTemp)
                        strSql_1 = strSql_1 & " OR CDM_INVOICE_NO" & Common.ParseSQL(strTemp) & ")"
                    End If

                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND POM_CREATED_Date BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strVendor <> "" Then
                        strTemp = Common.BuildWildCard(strVendor)
                        strSql_1 = strSql_1 & " AND POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If
                    'strSql_1 = strSql_1 & ") A inner join "
                    'strSql_2 = " COMPANY_DOC_MATCH ON POM_B_COY_ID = CDM_B_COY_ID AND CDM_PO_NO = DOC_NO AND CDM_DO_NO <> '' LEFT OUTER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_INDEX=POM_DEPT_INDEX LEFT JOIN PO_CR_MSTR ON PCM_PO_INDEX = DOC_INDEX WHERE CDM_B_COY_ID ='" & strCoyID & "' "

                    strSql_2 = " "
                    'strSql_1 = strSql_1 & "} "
                    'strSql_2 = "APPEND ({SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "'} RELATE DOC_NO TO CDM_PO_NO,POM_B_COY_ID TO CDM_B_COY_ID)"


                Case "GRN"
                    ' Michelle (28/8/2007) - To cater for multiple POs & "AND POM.POM_PO_INDEX=PRM.PRM_PO_INDEX " _
                    'strSql_1 = "SHAPE {SELECT DISTINCT GM.GM_GRN_INDEX AS DOC_INDEX,GM.GM_GRN_NO AS DOC_NO, GM.GM_DATE_RECEIVED AS DOC_DATE, " _
                    '& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME," _
                    '& "Sum(PD.POD_UNIT_COST * (GD.GD_RECEIVED_QTY - GD.GD_REJECTED_QTY)) AS COST, GM.GM_DO_INDEX AS DO_INDEX, " _
                    '& "POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,GM_B_COY_ID " _
                    '& "FROM GRN_MSTR GM, PO_DETAILS PD, GRN_DETAILS GD, PR_MSTR PRM, PO_MSTR POM " _
                    '& "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    '& "WHERE GM.GM_B_COY_ID = '" & strCoyID & "' AND GD.GD_GRN_NO = GM.GM_GRN_NO " _
                    '& "AND GD.GD_B_COY_ID = GM.GM_B_COY_ID " _
                    '& "AND GM.GM_PO_INDEX=POM.POM_PO_INDEX " _
                    '& "AND PD.POD_PO_LINE = GD.GD_PO_LINE " _
                    '& "AND POM.POM_PO_INDEX=PRM.PRM_PO_INDEX " _
                    '& "AND POM.POM_PO_NO=PD.POD_PO_NO " _
                    '& "AND POM.POM_B_COY_ID=PD.POD_COY_ID "

                    'Michelle (22/4/2011) - To cater for FTN
                    'strSql_1 = "select A.*, B.* from (SELECT DISTINCT GM.GM_GRN_INDEX AS DOC_INDEX,GM.GM_GRN_NO AS DOC_NO, GM.GM_DATE_RECEIVED AS DOC_DATE, " _
                    '& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME," _
                    '& "Sum(PD.POD_UNIT_COST * (GD.GD_RECEIVED_QTY - GD.GD_REJECTED_QTY)) AS COST, GM.GM_DO_INDEX AS DO_INDEX, " _
                    '& "POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,GM_B_COY_ID " _
                    '& "FROM GRN_MSTR GM, PO_DETAILS PD, GRN_DETAILS GD, PR_MSTR PRM, PO_MSTR POM " _
                    '& "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    '& "WHERE GM.GM_B_COY_ID = '" & strCoyID & "' AND GD.GD_GRN_NO = GM.GM_GRN_NO " _
                    '& "AND GD.GD_B_COY_ID = GM.GM_B_COY_ID " _
                    '& "AND GM.GM_PO_INDEX=POM.POM_PO_INDEX " _
                    '& "AND PD.POD_PO_LINE = GD.GD_PO_LINE " _
                    '& "AND PD.POD_PR_INDEX=PRM.PRM_PR_INDEX " _
                    '& "AND POM.POM_PO_NO=PD.POD_PO_NO " _
                    '& "AND POM.POM_B_COY_ID=PD.POD_COY_ID "

                    'Michelle (30/9/2011)
                    'strSql_1 = "select A.*, B.* from (SELECT DISTINCT GM.GM_GRN_INDEX AS DOC_INDEX,GM.GM_GRN_NO AS DOC_NO,GM.GM_GRN_NO AS PO_NUMBER,GM.GM_DATE_RECEIVED AS DOC_DATE, " _
                    '            & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME," _
                    '            & "Sum(PD.POD_UNIT_COST * (GD.GD_RECEIVED_QTY - GD.GD_REJECTED_QTY)) AS COST, GM.GM_DO_INDEX AS DO_INDEX, " _
                    '            & "POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,GM_B_COY_ID " _
                    '            & "FROM GRN_MSTR GM, GRN_DETAILS GD, PO_MSTR POM " _
                    '            & "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    '            & ", PO_DETAILS PD LEFT OUTER JOIN PR_MSTR PRM ON PD.POD_PR_INDEX=PRM.PRM_PR_INDEX " _
                    '            & "WHERE GM.GM_B_COY_ID = '" & strCoyID & "' AND GD.GD_GRN_NO = GM.GM_GRN_NO " _
                    '            & "AND GD.GD_B_COY_ID = GM.GM_B_COY_ID " _
                    '            & "AND GM.GM_PO_INDEX=POM.POM_PO_INDEX " _
                    '            & "AND PD.POD_PO_LINE = GD.GD_PO_LINE " _
                    '            & "AND POM.POM_PO_NO=PD.POD_PO_NO " _
                    '            & "AND POM.POM_B_COY_ID=PD.POD_COY_ID "

                    'If strDocNo <> "" Then
                    '    strTemp = Common.BuildWildCard(strDocNo)
                    '    strSql_1 = strSql_1 & " AND (GM_GRN_NO" & Common.ParseSQL(strTemp) & " OR PRM_PR_NO" & _
                    '    Common.ParseSQL(strTemp) & " OR POM_PO_NO" & Common.ParseSQL(strTemp) & " OR POM.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR POM.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" & _
                    '    Common.ParseSQL(strTemp) & "))"
                    'End If

                    strSql_1 = "SELECT CDM_S_COY_ID,CDM_PO_NO, CDM_DO_NO, CDM_GRN_NO, CDM_INVOICE_NO, GM.GM_GRN_INDEX AS DOC_INDEX,GM.GM_GRN_NO AS DOC_NO,GM.GM_GRN_NO AS PO_NUMBER,GM.GM_DATE_RECEIVED AS DOC_DATE, " _
            & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS COY_NAME,"
                    '& "Sum(PD.POD_UNIT_COST * (GD.GD_RECEIVED_QTY - GD.GD_REJECTED_QTY) AS COST, GM.GM_DO_INDEX AS DO_INDEX, " _
                    strSql_1 = strSql_1 & " 0 AS COST, GM.GM_DO_INDEX AS DO_INDEX, POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,GM_B_COY_ID,'' AS PCM_CR_NO, POM_B_COY_ID " _
            & "FROM GRN_MSTR GM  " _
            & "INNER JOIN COMPANY_DOC_MATCH ON GM.GM_GRN_NO = CDM_GRN_NO AND CDM_B_COY_ID='" & strCoyID & "', " _
            & "PO_MSTR POM LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
            & "WHERE GM.GM_B_COY_ID = '" & strCoyID & "' " _
            & "AND GM.GM_PO_INDEX=POM.POM_PO_INDEX AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

                    'If strDocNo <> "" Then
                    '    strTemp = Common.BuildWildCard(strDocNo)
                    '    strSql_1 = strSql_1 & " AND (GM_GRN_NO" & Common.ParseSQL(strTemp) & "OR POM_PO_NO" & Common.ParseSQL(strTemp) & " OR POM.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR POM.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" & _
                    '    Common.ParseSQL(strTemp) & "))"
                    'End If

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND (GM_GRN_NO" & Common.ParseSQL(strTemp) & " OR POM_PO_NO" & Common.ParseSQL(strTemp) &
                                    " OR CDM_DO_NO " & Common.ParseSQL(strTemp) & "OR CDM_INVOICE_NO" & Common.ParseSQL(strTemp) & ")"
                    End If

                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND GM_CREATED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strVendor <> "" Then
                        strTemp = Common.BuildWildCard(strVendor)
                        strSql_1 = strSql_1 & " AND POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strDept <> "" Then
                        strTemp = Common.BuildWildCard(strDept)
                        strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
                    End If

                    'strSql_1 = strSql_1 & " GROUP BY GM.GM_GRN_NO, GM.GM_DATE_RECEIVED," _
                    '& "POM.POM_PO_NO, POM.POM_S_COY_ID,GM_GRN_INDEX ,POM_S_COY_NAME," _
                    '& "POM_BUYER_NAME,POM_PO_INDEX,CDM_DEPT_NAME,GM_B_COY_ID, GM_DO_INDEX "
                    'strSql_1 = strSql_1 & ") A inner join "

                    'strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "') B ON A.DOC_NO = B.CDM_GRN_NO AND A.GM_B_COY_ID = B.CDM_B_COY_ID"
                    strSql_2 = ""
            End Select

            'strSql = strSql_1 '& ";" & strSql_2
            strSql = strSql_1 & strSql_2
            'Michelle (24/2/2011) - To resolve the 'shape' error
            'Dim objDB As New EAD.DBCom(ConfigurationSettings.AppSettings("shape"))
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            ds = objDB.FillDs(strSql)



            '//cannot use relation because strsql_1 may return multiple record with same PO_NO
            'Dim parentCol, childCol As DataColumn

            'parentCol = ds.Tables(0).Columns(strParentCol)
            'childCol = ds.Tables(1).Columns(strChildCol)

            ' Create DataRelation.
            'Dim relDoc As DataRelation
            'relDoc = New DataRelation("match", parentCol, childCol)
            ' Add the relation to the DataSet.
            'ds.Relations.Add(relDoc)

            objDB = Nothing
            Return ds
        End Function
        Function TransTracking_Vendor(ByVal strDocType As String, ByVal strDocNo As String, ByVal strBuyerCoy As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strBuyer As String) As DataSet
            '//Use Data Shaping because it is difficut to filter second table if we use DataRelation of ADO.Net
            Dim strSql, strSql_1, strSql_2, strSql_3, strCoyID As String
            Dim strParentCol, strChildCol As String
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            Select Case strDocType
                Case "PO"
                    '//for vendor side, no need to display PR                  
                    'If strDocNo <> "" Then
                    strTemp = Common.BuildWildCard(strDocNo)
                    strSql_1 = "select PCM_CR_NO,X.*, Y.* from (SELECT DISTINCT POM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_DATE AS DOC_DATE, " _
                    & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME,POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER," _
                    & "POM_PO_NO AS PO_NO,POM_B_COY_ID AS BUYER_COY,CM_COY_NAME AS COY_NAME,'' AS Dept,POM_B_COY_ID FROM PO_MSTR LEFT JOIN COMPANY_MSTR ON POM_B_COY_ID=CM_COY_ID WHERE POM_S_COY_ID='" & strCoyID & "'"

                    strSql_1 = strSql_1 & " AND (POM_PO_NO" & Common.ParseSQL(strTemp) & " OR POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" &
                    Common.ParseSQL(strTemp) & ") OR POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" &
                    Common.ParseSQL(strTemp) & ") OR POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" &
                    Common.ParseSQL(strTemp) & "))"
                    'Else
                    '    strSql_1 = "select PCM_CR_NO,X.*, Y.* from (SELECT DISTINCT POM_PO_INDEX AS DOC_INDEX,POM_PO_NO AS DOC_NO,POM_PO_DATE AS DOC_DATE, " _
                    '    & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME,POM_PO_COST AS COST,POM_BUYER_NAME AS BUYER," _
                    '    & "POM_PO_NO AS PO_NO,POM_B_COY_ID AS BUYER_COY,CM_COY_NAME AS COY_NAME,'' AS Dept,POM_B_COY_ID  " _
                    '    & "FROM PO_MSTR LEFT JOIN COMPANY_MSTR  " _
                    '    & "ON POM_B_COY_ID=CM_COY_ID WHERE POM_S_COY_ID='" & strCoyID & "'"
                    'End If

                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND POM_CREATED_Date BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyerCoy <> "" Then
                        strTemp = Common.BuildWildCard(strBuyerCoy)
                        strSql_1 = strSql_1 & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    End If

                    'If strDept <> "" Then
                    '    strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDept)
                    'End If
                    strSql_1 = strSql_1 & ") X inner join "
                    strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_S_COY_ID='" & strCoyID & "') Y ON X.DOC_NO = Y.CDM_PO_NO AND X.POM_B_COY_ID = Y.CDM_B_COY_ID LEFT JOIN PO_CR_MSTR ON PCM_PO_INDEX = DOC_INDEX"
                    'strParentCol = "DOC_NO"
                    'strChildCol = "CDM_PO_NO"
                Case "GRN"
                    'Michelle (30/9/2011)
                    strSql_1 = "SELECT CDM_S_COY_ID,CDM_PO_NO, CDM_DO_NO, CDM_GRN_NO, CDM_INVOICE_NO, GM.GM_GRN_INDEX AS DOC_INDEX,GM.GM_GRN_NO AS DOC_NO,GM.GM_GRN_NO AS PO_NUMBER,GM.GM_DATE_RECEIVED AS DOC_DATE, " _
                    & "POM_S_COY_ID AS VEN_ID,CM_COY_NAME AS COY_NAME,"
                    '& "Sum(PD.POD_UNIT_COST * (GD.GD_RECEIVED_QTY - GD.GD_REJECTED_QTY) AS COST, GM.GM_DO_INDEX AS DO_INDEX, " _
                    strSql_1 = strSql_1 & " 0 AS COST, GM.GM_DO_INDEX AS DO_INDEX, POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,CDM_DEPT_NAME AS DEPT,GM_B_COY_ID,'' AS PCM_CR_NO, POM_B_COY_ID, GM_B_COY_ID AS BUYER_COY " _
                    & "FROM GRN_MSTR GM  " _
                    & "INNER JOIN COMPANY_DOC_MATCH ON GM.GM_GRN_NO = CDM_GRN_NO AND CDM_S_COY_ID='" & strCoyID & "', " _
                    & "PO_MSTR POM INNER JOIN COMPANY_MSTR CM ON CM.CM_COY_ID=POM.POM_B_COY_ID     LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    & "WHERE GM.GM_S_COY_ID = '" & strCoyID & "' " _
                    & "AND GM.GM_PO_INDEX=POM.POM_PO_INDEX "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND (GM_GRN_NO" & Common.ParseSQL(strTemp) & " OR POM_PO_NO" & Common.ParseSQL(strTemp) &
                                    " OR CDM_DO_NO " & Common.ParseSQL(strTemp) & "OR CDM_INVOICE_NO" & Common.ParseSQL(strTemp) & ")"
                    End If
                    'strSql_1 = "select X.*, Y.* from (SELECT DISTINCT GM.GM_GRN_INDEX AS DOC_INDEX,GM.GM_GRN_NO AS DOC_NO, GM.GM_DATE_RECEIVED AS DOC_DATE, " _
                    '& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME," _
                    '& "Sum(PD.POD_UNIT_COST * (GD.GD_RECEIVED_QTY - GD.GD_REJECTED_QTY)) AS COST, GM.GM_DO_INDEX AS DO_INDEX, " _
                    '& "POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,GM_B_COY_ID AS BUYER_COY,CM_COY_NAME AS COY_NAME,'' AS Dept,GM_B_COY_ID " _
                    '& "FROM GRN_MSTR GM, PO_DETAILS PD, GRN_DETAILS GD, PO_MSTR POM,COMPANY_MSTR CM " _
                    '& "WHERE GM.GM_S_COY_ID = '" & strCoyID & "' AND GD.GD_GRN_NO = GM.GM_GRN_NO " _
                    '& "AND GD.GD_B_COY_ID = GM.GM_B_COY_ID " _
                    '& "AND GM.GM_PO_INDEX=POM.POM_PO_INDEX " _
                    '& "AND PD.POD_PO_LINE = GD.GD_PO_LINE " _
                    '& "AND POM.POM_PO_NO=PD.POD_PO_NO " _
                    '& "AND POM.POM_B_COY_ID=PD.POD_COY_ID " _
                    '& "AND CM.CM_COY_ID=GM.GM_B_COY_ID "
                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND (GM_GRN_NO" & Common.ParseSQL(strTemp) & " OR POM_PO_NO" & Common.ParseSQL(strTemp) & " OR POM.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" &
                        Common.ParseSQL(strTemp) & ") OR POM.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" &
                        Common.ParseSQL(strTemp) & "))"
                    End If

                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND GM_DATE_RECEIVED BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyerCoy <> "" Then
                        strTemp = Common.BuildWildCard(strBuyerCoy)
                        strSql_1 = strSql_1 & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    End If

                    'If strDept <> "" Then
                    '    strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDept)
                    'End If

                    'strSql_1 = strSql_1 & " GROUP BY GM.GM_GRN_NO, GM.GM_DATE_RECEIVED," _
                    '& "POM.POM_PO_NO, POM.POM_S_COY_ID,GM_GRN_INDEX ,POM_S_COY_NAME," _
                    '& "POM_BUYER_NAME,POM_PO_INDEX,GM_B_COY_ID,CM_COY_NAME,GM_DO_INDEX  "
                    'strSql_1 = strSql_1 & ") X inner join  "

                    'strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_S_COY_ID='" & strCoyID & "') Y ON X.DOC_NO = Y.CDM_GRN_NO AND X.GM_B_COY_ID = Y.CDM_B_COY_ID"
                    strSql_2 = ""

                Case "DO"
                    'Michelle (30/9/2011)
                    strSql_1 = "SELECT CDM_S_COY_ID,CDM_PO_NO, CDM_GRN_NO, CDM_INVOICE_NO, DOM.DOM_DO_INDEX AS DOC_INDEX,DOM.DOM_DO_NO AS DOC_NO,DOM.DOM_DO_DATE AS DOC_DATE, " _
                    & "POM_S_COY_ID AS VEN_ID," _
                    & "Sum(PD.POD_UNIT_COST * (DOD.DOD_SHIPPED_QTY)) AS COST, DOM.DOM_DO_INDEX AS DO_INDEX, " _
                    & " POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,CM_COY_NAME AS COY_NAME, CDM_DEPT_NAME AS DEPT,'' AS PCM_CR_NO, POM_B_COY_ID, CDM_B_COY_ID AS BUYER_COY, CDM_DO_NO " _
                    & "FROM DO_MSTR DOM INNER JOIN DO_DETAILS DOD ON DOM.DOM_S_COY_ID = DOD_S_COY_ID AND " _
                    & "DOM.DOM_DO_NO = DOD.DOD_DO_NO INNER JOIN PO_MSTR POM ON DOM.DOM_PO_INDEX=POM.POM_PO_INDEX " _
                    & "INNER JOIN PO_DETAILS PD ON POM.POM_PO_NO=PD.POD_PO_NO AND POM.POM_B_COY_ID=PD.POD_COY_ID " _
                    & "INNER JOIN COMPANY_DOC_MATCH ON POM.POM_PO_NO = CDM_PO_NO AND DOM.DOM_DO_NO = CDM_DO_NO AND CDM_S_COY_ID='" & strCoyID & "' " _
                    & "INNER JOIN COMPANY_MSTR CM ON CM.CM_COY_ID=POM.POM_B_COY_ID LEFT OUTER JOIN `sso`.`COMPANY_DEPT_MSTR` ON POM_DEPT_INDEX=CDM_DEPT_INDEX " _
                    & "WHERE DOM.DOM_S_COY_ID = '" & strCoyID & "' AND PD.POD_PO_LINE = DOD.DOD_PO_LINE "

                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND (DOM.DOM_DO_NO" & Common.ParseSQL(strTemp) & " OR POM_PO_NO" & Common.ParseSQL(strTemp) &
                                    " OR CDM_GRN_NO " & Common.ParseSQL(strTemp) & "OR CDM_INVOICE_NO" & Common.ParseSQL(strTemp) & ")"
                    End If


                    'strSql_1 = "select X.*, Y.* from (SELECT DISTINCT DOM.DOM_DO_INDEX AS DOC_INDEX,DOM.DOM_DO_NO AS DOC_NO, DOM.DOM_DO_DATE AS DOC_DATE, " _
                    '& "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME,DOM_S_COY_ID," _
                    '& "Sum(PD.POD_UNIT_COST * (DOD.DOD_SHIPPED_QTY)) AS COST, DOM.DOM_DO_INDEX AS DO_INDEX, " _
                    '& "POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,POM_B_COY_ID AS BUYER_COY,CM_COY_NAME AS COY_NAME,'' AS Dept " _
                    '& "FROM DO_MSTR DOM, PO_DETAILS PD, DO_DETAILS DOD, PO_MSTR POM,COMPANY_MSTR CM " _
                    '& "WHERE DOM.DOM_S_COY_ID = '" & strCoyID & "' AND DOD.DOD_DO_NO = DOM.DOM_DO_NO " _
                    '& "AND DOD.DOD_S_COY_ID = DOM.DOM_S_COY_ID " _
                    '& "AND DOM.DOM_PO_INDEX=POM.POM_PO_INDEX " _
                    '& "AND PD.POD_PO_LINE = DOD.DOD_PO_LINE " _
                    '& "AND POM.POM_PO_NO=PD.POD_PO_NO " _
                    '& "AND POM.POM_B_COY_ID=PD.POD_COY_ID " _
                    '& "AND CM.CM_COY_ID=POM.POM_B_COY_ID "
                    'If strDocNo <> "" Then
                    '    strTemp = Common.BuildWildCard(strDocNo)
                    '    strSql_1 = strSql_1 & " AND (DOM_DO_NO" & Common.ParseSQL(strTemp) & " OR POM_PO_NO" & Common.ParseSQL(strTemp) & _
                    '    " OR POM.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR POM.POM_PO_INDEX IN (SELECT IM_PO_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & _
                    '    Common.ParseSQL(strTemp) & "))"
                    'End If

                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND DOM.DOM_DO_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyerCoy <> "" Then
                        strTemp = Common.BuildWildCard(strBuyerCoy)
                        strSql_1 = strSql_1 & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    End If

                    'If strDept <> "" Then
                    '    strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDept)
                    'End If

                    'strSql_1 = strSql_1 & " GROUP BY DOM.DOM_DO_NO, DOM.DOM_DO_DATE," _
                    '& "POM.POM_PO_NO, POM.POM_S_COY_ID,DOM_DO_INDEX ,POM_S_COY_NAME," _
                    '& "POM_BUYER_NAME,POM_PO_INDEX,DOM_S_COY_ID,POM_PO_INDEX,CM_COY_NAME,POM_B_COY_ID "
                    'strSql_1 = strSql_1 & ") X inner join  "

                    'strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_S_COY_ID='" & strCoyID & "') Y ON X.DOC_NO = Y.CDM_DO_NO AND X.DOM_S_COY_ID = Y.CDM_S_COY_ID"
                    strSql_2 = " GROUP BY DOM.DOM_DO_NO"
                Case "INV"
                    'strSql_1 = "select X.*, Y.* from (SELECT DISTINCT IM.IM_INVOICE_INDEX AS DOC_INDEX,IM.IM_INVOICE_NO AS DOC_NO, IM.IM_CREATED_ON AS DOC_DATE, " _
                    ' & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME,IM_S_COY_ID," _
                    '& "Sum(ID.ID_UNIT_COST * (ID.ID_RECEIVED_QTY)) AS COST, " _

                    'Michelle (1/10/2011)
                    'strSql_1 = "select X.*, Y.* from (SELECT DISTINCT IM.IM_INVOICE_INDEX AS DOC_INDEX,IM.IM_INVOICE_NO AS DOC_NO, IM.IM_CREATED_ON AS DOC_DATE, " _
                    ' & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME,IM_S_COY_ID," _
                    ' & "IM.IM_INVOICE_TOTAL AS COST, " _
                    ' & "POM_BUYER_NAME AS BUYER,POM.POM_PO_NO AS PO_NO,POM_PO_INDEX,POM_B_COY_ID AS BUYER_COY,CM_COY_NAME AS COY_NAME,'' AS Dept " _
                    ' & "FROM INVOICE_MSTR IM, PO_DETAILS PD, INVOICE_DETAILS ID, PO_MSTR POM,COMPANY_MSTR CM " _
                    ' & "WHERE IM.IM_S_COY_ID = '" & strCoyID & "' AND ID.ID_INVOICE_NO = IM.IM_INVOICE_NO " _
                    ' & "AND ID.ID_S_COY_ID = IM.IM_S_COY_ID " _
                    ' & "AND IM.IM_PO_INDEX=POM.POM_PO_INDEX " _
                    ' & "AND PD.POD_PO_LINE = ID.ID_PO_LINE " _
                    ' & "AND POM.POM_PO_NO=PD.POD_PO_NO " _
                    ' & "AND POM.POM_B_COY_ID=PD.POD_COY_ID " _
                    ' & "AND CM.CM_COY_ID=POM.POM_B_COY_ID "
                    'If strDocNo <> "" Then
                    '    strTemp = Common.BuildWildCard(strDocNo)
                    '    strSql_1 = strSql_1 & " AND (IM_INVOICE_NO" & _
                    '    Common.ParseSQL(strTemp) & " OR POM_PO_NO" & _
                    '    Common.ParseSQL(strTemp) & " OR POM.POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & _
                    '    Common.ParseSQL(strTemp) & ") OR POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & _
                    '    Common.ParseSQL(strTemp) & "))"
                    'End If

                    strSql_1 = "SELECT IM.IM_INVOICE_INDEX AS DOC_INDEX,IM.IM_INVOICE_NO AS DOC_NO, IM.IM_CREATED_ON AS DOC_DATE, " _
                             & "POM_S_COY_ID AS VEN_ID,POM_S_COY_NAME AS VEN_NAME,IM_S_COY_ID, POM_B_COY_ID,CDM_PO_NO," _
                             & "IM.IM_INVOICE_TOTAL AS COST, '' AS PCM_CR_NO,CDM_DO_NO, CDM_GRN_NO, CDM_INVOICE_NO," _
                             & "POM_BUYER_NAME AS BUYER,POM_PO_NO AS PO_NO,POM_PO_INDEX,POM_B_COY_ID AS BUYER_COY,CM_COY_NAME AS COY_NAME,'' AS Dept " _
                             & "FROM INVOICE_MSTR IM " _
                             & "INNER JOIN COMPANY_DOC_MATCH ON IM.IM_INVOICE_NO = CDM_INVOICE_NO AND CDM_S_COY_ID= '" & strCoyID & "' " _
                            & ", PO_MSTR POM,COMPANY_MSTR CM  WHERE IM.IM_S_COY_ID = '" & strCoyID & "' " _
                             & "AND IM.IM_PO_INDEX=POM_PO_INDEX " _
                             & "AND CM.CM_COY_ID=POM_B_COY_ID "
                    If strDocNo <> "" Then
                        strTemp = Common.BuildWildCard(strDocNo)
                        strSql_1 = strSql_1 & " AND (IM.IM_INVOICE_NO" &
                        Common.ParseSQL(strTemp) & " OR POM_PO_NO" &
                        Common.ParseSQL(strTemp) & " OR CDM_DO_NO" &
                        Common.ParseSQL(strTemp) & " OR CDM_GRN_NO" &
                       Common.ParseSQL(strTemp) & ")"
                    End If

                    If dteDateFr <> "" And dteDateTo <> "" Then
                        strSql_1 = strSql_1 & " AND IM.IM_CREATED_ON BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                        " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
                    End If

                    If strBuyerCoy <> "" Then
                        strTemp = Common.BuildWildCard(strBuyerCoy)
                        strSql_1 = strSql_1 & " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
                    End If

                    If strBuyer <> "" Then
                        strTemp = Common.BuildWildCard(strBuyer)
                        strSql_1 = strSql_1 & " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    End If

                    'If strDept <> "" Then
                    '    strSql_1 = strSql_1 & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDept)
                    'End If

                    'strSql_1 = strSql_1 & " GROUP BY IM.IM_INVOICE_NO, IM.IM_CREATED_ON," _
                    '& "POM.POM_PO_NO, POM.POM_S_COY_ID,IM_INVOICE_INDEX ,POM_S_COY_NAME," _
                    '& "POM_BUYER_NAME,POM_PO_INDEX,IM_S_COY_ID,POM_PO_INDEX,CM_COY_NAME,POM_B_COY_ID "
                    'strSql_1 = strSql_1 & ") X inner join  "


                    'strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_S_COY_ID='" & strCoyID & "') Y ON X.DOC_NO = Y.CDM_INVOICE_NO AND X.IM_S_COY_ID = Y.CDM_S_COY_ID"
                    strSql_2 = ""
            End Select

            'strSql = strSql_1 '& ";" & strSql_2
            strSql = strSql_1 & strSql_2
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            ds = objDB.FillDs(strSql)

            '//cannot use relation because strsql_1 may return multiple record with same PO_NO
            'Dim parentCol, childCol As DataColumn

            'parentCol = ds.Tables(0).Columns(strParentCol)
            'childCol = ds.Tables(1).Columns(strChildCol)

            ' Create DataRelation.
            'Dim relDoc As DataRelation
            'relDoc = New DataRelation("match", parentCol, childCol)
            ' Add the relation to the DataSet.
            'ds.Relations.Add(relDoc)

            objDB = Nothing
            Return ds
        End Function

        Function RejectInvoice(ByVal UserId As String, ByVal strInvoiceNo As String, ByVal strVendor As String, ByVal strRemarks As String) As String
            Dim CompanyId As String = objUsers.getCompanyID(UserId)

            Dim objDB As New EAD.DBCom

            Dim strsql As String
            Dim strAryQuery(0) As String

            Dim intInvoiceIndex, intInvoiceStatus As Integer
            Dim strMsg As String

            strsql = "SELECT IM_INVOICE_INDEX, IM_INVOICE_STATUS, IM_INVOICE_TOTAL FROM INVOICE_MSTR WHERE IM_Invoice_No= '" & strInvoiceNo & "'"
            strsql &= " AND IM_S_COY_ID='" & strVendor & "'"
            Dim tDS As DataSet = objDB.FillDs(strsql)


            If tDS.Tables(0).Rows.Count > 0 Then
                intInvoiceIndex = tDS.Tables(0).Rows(0).Item("IM_INVOICE_INDEX")
                intInvoiceStatus = tDS.Tables(0).Rows(0).Item("IM_INVOICE_STATUS")

                If intInvoiceStatus = invStatus.Approved Then
                    strMsg = "The invoice has already been approved. Rejecting of this invoice is not allowed. "
                ElseIf intInvoiceStatus = invStatus.Paid Then
                    strMsg = "The invoice has already been paid. Rejecting of this invoice is not allowed. "
                ElseIf intInvoiceStatus = invStatus.PendingAppr Then
                    If isApproved(intInvoiceIndex, "FO", UserId) Then
                        strMsg = "You have already approved this invoice. Rejecting of this invoice is not allowed."
                    End If
                End If
            End If


            '//update INVIOCE status, status_changed_by,status_changed_date
            strsql = "UPDATE INVOICE_MSTR SET IM_INVOICE_STATUS=" & invStatus.NewInv & "," &
            "IM_INVOICE_STATUS = NULL," &
            "IM_SUBMITTEDBY_FO = NULL," &
            "IM_FM_APPROVED_DATE = NULL," &
            "IM_PAYMENT_TERM = NULL," &
            "IM_FINANCE_REMARKS = '" & strRemarks & "'," &
            "IM_STATUS_CHANGED_BY='" & UserId & "'," &
            "IM_STATUS_CHANGED_ON = " & Common.ConvertDate(Now) &
            " WHERE IM_INVOICE_Index=" & intInvoiceIndex

            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "UPDATE FINANCE_APPROVAL SET FA_ACTIVE_AO = NULL, FA_ACTION_DATE = NULL, FA_AO_ACTION = 0, FA_AO_REMARK = NULL, FA_APPROVAL_TYPE = NULL WHERE FA_INVOICE_INDEX=" & intInvoiceIndex
            Common.Insert2Ary(strAryQuery, strsql)

            If objDB.BatchExecute(strAryQuery) Then
                Dim objMail As New Email

                '//next ao
                sendMailToApproval(CompanyId, strInvoiceNo, intInvoiceIndex, "FO")
            End If

            Return strMsg

        End Function

        Function HoldInvoice(ByVal strInvoiceNo As String, ByVal strVendor As String, ByVal strRemarks As String)
            Dim strCoyId, strLoginUser As String

            Dim objDB As New EAD.DBCom

            Dim strsql As String
            Dim strAryQuery(0) As String

            Dim intInvoiceIndex, intInvoiceStatus As Integer
            Dim strMsg As String

            strCoyId = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            strsql = "SELECT IM_INVOICE_INDEX, IM_INVOICE_STATUS, IM_INVOICE_TOTAL FROM INVOICE_MSTR WHERE IM_Invoice_No= '" & strInvoiceNo & "'"
            strsql &= " AND IM_S_COY_ID='" & strVendor & "'"
            Dim tDS As DataSet = objDB.FillDs(strsql)


            If tDS.Tables(0).Rows.Count > 0 Then
                intInvoiceIndex = tDS.Tables(0).Rows(0).Item("IM_INVOICE_INDEX")
                intInvoiceStatus = tDS.Tables(0).Rows(0).Item("IM_INVOICE_STATUS")

                If intInvoiceStatus = invStatus.Approved Then
                    strMsg = "The invoice has already been approved. Holding of this invoice is not allowed. "
                ElseIf intInvoiceStatus = invStatus.Paid Then
                    strMsg = "The invoice has already been paid. Holding of this invoice is not allowed. "
                ElseIf intInvoiceStatus = invStatus.Hold Then
                    strMsg = "You have already hold this invoice. Holding of this invoice is not allowed."
                End If
            End If


            If strMsg <> "" Then Return strMsg

            '//update INVIOCE status, status_changed_by,status_changed_date
            strsql = "UPDATE INVOICE_MSTR SET IM_INVOICE_STATUS=" & invStatus.Hold & "," &
            "IM_STATUS_CHANGED_BY='" & strLoginUser & "'," &
            "IM_STATUS_CHANGED_ON = " & Common.ConvertDate(Now) &
            " WHERE IM_INVOICE_Index=" & intInvoiceIndex

            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemarks) & "' WHERE FA_INVOICE_INDEX=" & intInvoiceIndex & " AND FA_SEQ - 1 = FA_AO_ACTION "
            Common.Insert2Ary(strAryQuery, strsql)

            objDB.BatchExecute(strAryQuery)
            Return "Invoice hold"
        End Function

        Function updateInvoice(ByVal userID As String, ByVal companyID As String, ByVal dtInv As DataTable, ByVal blnSend As Boolean)
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0), strAryQuery2(0) As String
            Dim objDb As New EAD.DBCom
            Dim strDoc As String = ""
            Dim blnUpdatePO As Boolean = True

            If dtInv.Rows.Count > 0 Then
                For i = 0 To dtInv.Rows.Count - 1
                    strDoc &= dtInv.Rows(i)("InvNo") & ","
                    strsql = "UPDATE INVOICE_MSTR SET "
                    If dtInv.Rows(i)("InvStatus") <> 0 Then ' IM_INVOICE_STATUS
                        strsql &= "IM_INVOICE_STATUS = '" & Common.Parse(dtInv.Rows(i)("InvStatus")) & "', "
                    End If

                    If dtInv.Rows(i)("Submitted") <> "" Then ' IM_SUBMITTEDBY_FO
                        strsql &= "IM_SUBMITTEDBY_FO = '" & Common.Parse(dtInv.Rows(i)("Submitted")) & "', "
                    End If

                    If dtInv.Rows(i)("AppDate") <> "" Then ' IM_FM_APPROVED_DATE
                        strsql &= "IM_FM_APPROVED_DATE = GETDATE(), "
                    End If

                    If dtInv.Rows(i)("PayTerm") <> "" Then ' IM_PAYMENT_TERM
                        strsql &= "IM_PAYMENT_TERM = '" & Common.Parse(dtInv.Rows(i)("PayTerm")) & "', "
                        'strsql &= "IM_PAYMENT_DATE = GETDATE(), "
                    End If

                    If dtInv.Rows(i)("FinRemark") <> "" Then ' IM_FINANCE_REMARKS
                        strsql &= "IM_FINANCE_REMARKS = '" & Common.Parse(dtInv.Rows(i)("FinRemark")) & "' "
                    Else
                        strsql &= "IM_FINANCE_REMARKS = IM_FINANCE_REMARKS "
                    End If

                    strsql &= "WHERE IM_INVOICE_NO = '" & Common.Parse(dtInv.Rows(i)("InvNo")) & "' "
                    strsql &= "AND IM_S_COY_ID = '" & Common.Parse(dtInv.Rows(i)("Vendor")) & "' "

                    Common.Insert2Ary(strAryQuery, strsql)

                    ' Ai Chu Remark for SRU30011 19/08/2005
                    ''If dtInv.Rows(i)("InvStatus") = invStatus.Paid Then ' marked as paid
                    ''    ' check whether all invoices been marked as paid
                    ''    strsql = "SELECT '*' FROM INVOICE_MSTR "
                    ''    strsql &= "WHERE IM_PO_INDEX = '" & Common.Parse(dtInv.Rows(i)("PoIndex")) & "' "
                    ''    strsql &= "AND IM_INVOICE_STATUS <> " & invStatus.Paid & " "
                    ''    strsql &= "AND IM_INVOICE_NO <> '" & Common.Parse(dtInv.Rows(i)("InvNo")) & "' "
                    ''    strsql &= "AND IM_S_COY_ID = '" & Common.Parse(dtInv.Rows(i)("Vendor")) & "' "

                    ''    If objDb.Exist(strsql) <= 0 Then
                    ''        If dtInv.Rows(i)("BillMethod") <> "FPO" Then ' DO or GRN
                    ''            strsql = "SELECT DOM_DO_INDEX FROM DO_MSTR "
                    ''            strsql &= "RIGHT JOIN PO_MSTR ON DOM_PO_INDEX = POM_PO_INDEX "
                    ''            strsql &= "WHERE POM_PO_INDEX = '" & Common.Parse(dtInv.Rows(i)("PoIndex")) & "' "
                    ''            strsql &= "AND DOM_DO_STATUS <> '" & DOStatus.Invoiced & "' "
                    ''            strsql &= "AND DOM_DO_STATUS <> '" & DOStatus.Rejected & "' "

                    ''            If objDb.Exist(strsql) <= 0 Then ' all DO status = paid OR rejected
                    ''                strsql = "SELECT '*' FROM DO_MSTR RIGHT JOIN PO_MSTR ON DOM_PO_INDEX = POM_PO_INDEX "
                    ''                strsql &= "LEFT JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO "
                    ''                strsql &= "WHERE POM_PO_INDEX = '" & Common.Parse(dtInv.Rows(i)("PoIndex")) & "' "
                    ''                strsql &= "GROUP BY DOM_PO_INDEX "
                    ''                strsql &= "HAVING (SUM(POD_Ordered_Qty)- SUM(POD_CANCELLED_QTY) - SUM(POD_DELIVERED_QTY))> 0"

                    ''                If objDb.Exist(strsql) > 0 Then
                    ''                    blnUpdatePO = False
                    ''                End If
                    ''            End If
                    ''        End If

                    ''        If blnUpdatePO Then
                    ''            strsql = "UPDATE PO_MSTR SET POM_PO_STATUS = '" & POStatus_new.Close & "' "
                    ''            strsql &= "WHERE POM_PO_INDEX = '" & Common.Parse(dtInv.Rows(i)("PoIndex")) & "' "
                    ''            Common.Insert2Ary(strAryQuery, strsql)
                    ''        End If
                    ''    End If
                    ''End If
                Next

                If objDb.BatchExecute(strAryQuery) Then
                    Dim dt As New DataTable

                    ' declare fieldname in the new datatable
                    Dim strField(1) As String

                    strField(0) = "PoIndex"
                    strField(1) = "Vendor"
                    dt = Common.SelectDistinct(dtInv, strField)

                    For i = 0 To dt.Rows.Count - 1
                        'strsql = "SELECT '*' FROM DO_MSTR RIGHT JOIN PO_MSTR ON DOM_PO_INDEX = POM_PO_INDEX "
                        'strsql &= "LEFT JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO "
                        'strsql &= "WHERE POM_PO_INDEX = '" & Common.Parse(dt.Rows(i)("PoIndex")) & "' "
                        'strsql &= "GROUP BY DOM_PO_INDEX "
                        'strsql &= "HAVING (SUM(POD_Ordered_Qty)- SUM(POD_CANCELLED_QTY) - SUM(POD_DELIVERED_QTY)) = 0"

                        '//Add By Moo (29/08/2005)
                        '//Check whether Qty To Be Delivered=Qty Already Invoiced
                        '//If yes , Set Status to 'Close'
                        'strsql = "SELECT '*' FROM TotalQtyInvPaidByPO A,TotalQtyToBeDeliveredByPO B "
                        'strsql &= " WHERE A.IM_PO_INDEX = B.POM_PO_INDEX And A.Qty_Inv = B.QtyToBeDelivered And B.POM_PO_INDEX =" & Common.Parse(dt.Rows(i)("PoIndex"))

                        strsql = " SELECT * FROM ( "
                        strsql &= " SELECT c.IM_PO_INDEX AS IM_PO_INDEX, SUM(d.ID_RECEIVED_QTY) AS Qty_Inv "
                        strsql &= " FROM invoice_mstr c JOIN invoice_details d ON c.IM_INVOICE_NO = d.ID_INVOICE_NO AND c.IM_S_COY_ID = d.ID_S_COY_ID "
                        strsql &= " WHERE c.IM_INVOICE_STATUS = 4 AND c.IM_PO_INDEX =" & Common.Parse(dt.Rows(i)("PoIndex"))
                        strsql &= " GROUP BY c.IM_PO_INDEX "
                        strsql &= " ) A INNER JOIN ( "
                        strsql &= " SELECT "
                        strsql &= " a.POM_PO_INDEX AS POM_PO_INDEX, "
                        strsql &= " a.POM_PO_NO    AS POM_PO_NO, "
                        strsql &= " a.POM_B_COY_ID AS POM_B_COY_ID, "
                        strsql &= " SUM(b.POD_ORDERED_QTY - b.POD_CANCELLED_QTY) AS QtyToBeDelivered "
                        strsql &= " FROM po_mstr a JOIN po_details b ON a.POM_PO_NO = b.POD_PO_NO AND a.POM_B_COY_ID = b.POD_COY_ID "
                        strsql &= " WHERE a.POM_FULFILMENT = 3 AND a.POM_PO_INDEX =" & Common.Parse(dt.Rows(i)("PoIndex"))
                        strsql &= " GROUP BY a.POM_PO_INDEX,a.POM_PO_NO,a.POM_B_COY_ID "
                        strsql &= " ) B ON A.IM_PO_INDEX = B.POM_PO_INDEX AND A.Qty_Inv = B.QtyToBeDelivered AND "
                        strsql &= " B.POM_PO_INDEX =" & Common.Parse(dt.Rows(i)("PoIndex"))


                        If objDb.Exist(strsql) > 0 Then
                            strsql = "UPDATE PO_MSTR SET POM_PO_STATUS = '" & POStatus_new.Close & "' "
                            strsql &= "WHERE POM_PO_INDEX = '" & Common.Parse(dt.Rows(i)("PoIndex")) & "' "
                            Common.Insert2Ary(strAryQuery2, strsql)
                        End If
                    Next

                    If strAryQuery2(0) <> String.Empty Then
                        objDb.BatchExecute(strAryQuery2)
                    End If

                    If blnSend Then
                        Dim objMail As New Email
                        strDoc = strDoc.Substring(0, strDoc.Length - 1)
                        objMail.sendNotification(EmailType.InvoiceApproval, userID, companyID, "", strDoc, "")
                        objMail = Nothing
                    End If
                End If
            End If
        End Function
        Function getIPPINVTracking(ByVal strDocNo As String, ByVal strVendor As String, ByVal status As String, ByVal invfrom As String, ByVal doctype As String) As String
            Dim sql, strCoyID, strUserID As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            sql = " UNION SELECT '' AS CDM_DO_NO, '' AS CDM_GRN_NO, '' AS CDM_PO_NO, '' AS CDM_INVOICE_NO, '' AS CDM_S_COY_ID,IM_INVOICE_INDEX,'IPP' AS DOC_TYPE, IM_INVOICE_NO,IM_INVOICE_TYPE, IM_DOC_DATE AS POM_PO_Date, IM_DUE_DATE AS IM_PAYMENT_DATE, IM_INVOICE_TOTAL, " &
            "IM_CURRENCY_CODE AS POM_CURRENCY_CODE, IM_PRINTED, IM_PAYMENT_TERM, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, " &
            "IM_PO_INDEX, IM_S_COY_ID, '' AS POM_BILLING_METHOD, IM_INVOICE_STATUS, UM_USER_NAME AS POM_BUYER_NAME, " &
            "IM_S_COY_NAME AS POM_S_COY_NAME, '' AS STATUS_DESC, '' AS STATUS_REMARK, '' AS POM_PO_NO" &
            ", CDM_DEPT_NAME AS DEPT, IM_PAYMENT_TERM AS POM_PAYMENT_METHOD, '' AS POM_PO_INDEX " &
            "FROM INVOICE_MSTR " &
            "LEFT JOIN COMPANY_DEPT_MSTR ON IM_DEPT_INDEX = CDM_DEPT_INDEX " &
            "LEFT JOIN USER_MSTR ON UM_USER_ID = IM_CREATED_BY " &
            "LEFT JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX " &
            "WHERE IM_B_COY_ID = '" & strCoyID & "' AND IM_INVOICE_STATUS IN (" & status & ") AND UM_COY_ID = '" & strCoyID & "' " &
            "AND (FA_AO = '" & strUserID & "' OR FA_A_AO='" & strUserID & "' OR FA_A_AO_2='" & strUserID & "' OR FA_A_AO_3='" & strUserID & "' OR FA_A_AO_4='" & strUserID & "') "
            If invfrom = "new" Then
                sql &= "AND FA_AO_ACTION = (FA_SEQ - 1)"
            End If
            If strDocNo <> "" Then
                sql &= "AND IM_INVOICE_NO LIKE '%" & strDocNo & "%'"
            End If
            If strVendor <> "" Then
                sql &= "AND IM_S_COY_NAME LIKE '%" & strVendor & "%'"
            End If
            If doctype <> "" Then
                sql &= " AND IM_INVOICE_TYPE = '" & doctype & "' "
            End If
            Return sql
        End Function

        'mimi 2018-06-05 : invoice processing
        Function getIPPINVTracking_NoDocMatch(ByVal strCoyID As String, ByVal strUserID As String, ByVal strDocNo As String, ByVal strVendor As String, ByVal status As String, ByVal invfrom As String, ByVal doctype As String, Optional ByVal dteDateFr As String = "", Optional ByVal dteDateTo As String = "", Optional ByVal strCurr As String = "", Optional ByVal strFundType As String = "", Optional ByVal strResident As String = "", Optional ByVal strAmtFrom As String = "", Optional ByVal strAmtTo As String = "", Optional ByVal strDueDate As String = "", Optional ByVal strPayMode As String = "") As String
            Dim sql As String

            '' ''sql = " UNION SELECT IM_INVOICE_INDEX,'IPP' AS DOC_TYPE, IM_INVOICE_NO,IM_INVOICE_TYPE, IM_DOC_DATE AS POM_PO_Date, IM_DOC_DATE AS IM_PAYMENT_DATE, IM_INVOICE_TOTAL, " & _
            '' ''"IM_CURRENCY_CODE AS POM_CURRENCY_CODE, IM_PRINTED, IM_PAYMENT_TERM, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, " & _
            sql = " UNION SELECT IM_INVOICE_INDEX, '' AS CDM_DO_NO, '' AS CDM_GRN_NO, '' AS CDM_PO_NO, '' AS CDM_S_COY_ID,'' AS PRM_PR_INDEX,'' AS PRM_PR_NO,'IPP' AS DOC_TYPE, IM_INVOICE_NO,IM_GST_INVOICE,IM_INVOICE_TYPE, IM_DOC_DATE AS POM_PO_Date, IM_DUE_DATE AS IM_PAYMENT_DATE, IM_INVOICE_TOTAL, " &
            "IM_CURRENCY_CODE AS POM_CURRENCY_CODE, IM_PRINTED, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, " &
            "IM_PO_INDEX, IM_S_COY_ID, '' AS POM_BILLING_METHOD, IM_INVOICE_STATUS, UM_USER_NAME AS POM_BUYER_NAME, " &
            "IM_S_COY_NAME AS POM_S_COY_NAME, '' AS STATUS_DESC, '' AS STATUS_REMARK, '' AS POM_PO_NO" &
            ", CDM_DEPT_NAME AS DEPT, IM_PAYMENT_TERM AS POM_PAYMENT_METHOD, '' AS POM_PO_INDEX, IM_RESIDENT_TYPE, IM_PAYMENT_TERM, ID_ANALYSIS_CODE1 " &
            ",IF(IM_CURRENCY_CODE = 'MYR',IM_INVOICE_TOTAL,IM_INVOICE_TOTAL * (SELECT CE_RATE FROM company_exchangerate WHERE CE_COY_ID = '" & strCoyID & "' AND CE_CURRENCY_CODE = IM_CURRENCY_CODE " &
            "AND CE_DELETED='N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE())) INVAMT_INMYR " &
            "FROM INVOICE_MSTR " &
            "LEFT JOIN COMPANY_DEPT_MSTR ON IM_DEPT_INDEX = CDM_DEPT_INDEX " &
            "LEFT JOIN USER_MSTR ON UM_USER_ID = IM_CREATED_BY " &
            "LEFT JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX " &
            "LEFT JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO " &
            "WHERE IM_B_COY_ID = '" & strCoyID & "' AND IM_INVOICE_STATUS IN (" & status & ") AND UM_COY_ID = '" & strCoyID & "' " &
            "AND (FA_AO = '" & strUserID & "' OR FA_A_AO='" & strUserID & "' OR FA_A_AO_2='" & strUserID & "' OR FA_A_AO_3='" & strUserID & "' OR FA_A_AO_4='" & strUserID & "') "
            If invfrom = "new" Then
                sql &= " AND FA_AO_ACTION = (FA_SEQ - 1)"
            End If
            If strDocNo <> "" Then
                sql &= " AND IM_INVOICE_NO LIKE '%" & strDocNo & "%'"
            End If
            If strCurr <> "" Then
                sql &= " AND IM_CURRENCY_CODE = '" & strCurr & "'"
            End If
            If strVendor <> "" Then
                sql &= " AND IM_S_COY_NAME LIKE '%" & strVendor & "%'"
            End If
            If doctype <> "" Then
                sql &= " AND IM_INVOICE_TYPE = '" & doctype & "' "
            End If
            If dteDateFr <> "" And dteDateTo <> "" Then
                sql &= " AND IM_FM_APPROVED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00")
                sql &= " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            'Jules 2018.10.15
            If strFundType <> "" Then
                If InStr(strFundType, ":") Then
                    sql &= " AND ID_ANALYSIS_CODE1 = '" & strFundType.Substring(0, InStr(strFundType, ":") - 1) & "' "
                Else
                    sql &= " AND ID_ANALYSIS_CODE1 = '" & Common.Parse(strFundType) & "' "
                End If
            End If

            If strResident <> "" Then
                sql &= " AND IM_RESIDENT_TYPE = '" & strResident & "' "
            End If

            If strAmtFrom <> "" And strAmtTo <> "" Then
                sql &= " AND IM_INVOICE_TOTAL BETWEEN '" & strAmtFrom & "' "
                sql &= " AND '" & strAmtTo & "' "
            ElseIf strAmtFrom <> "" AndAlso strAmtTo = "" Then
                sql &= " AND IM_INVOICE_TOTAL >= '" & strAmtFrom & "' "
            ElseIf strAmtFrom = "" AndAlso strAmtTo <> "" Then
                sql &= " AND IM_INVOICE_TOTAL <= '" & strAmtTo & "' "
            End If

            If strDueDate <> "" Then
                sql &= " AND DATE(IM_PAYMENT_DATE) = " & Common.ConvertDate(strDueDate) & " "
            End If

            If strPayMode <> "" Then
                sql &= " And IM_PAYMENT_TERM = '" & strPayMode & "' "
            End If
            'End modification.
            Return sql
        End Function

        Function getInvoiceTracking(ByVal strDocNo As String, ByVal strVendor As String, ByVal strBuyer As String, ByVal strDept As String, ByVal strStatus As String, Optional ByVal IPPstatus As String = "", Optional ByVal invfrom As String = "", Optional ByVal doctype As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True) As DataSet
            '//Use Data Shaping because it is difficut to filter second table if we use DataRelation of ADO.Net
            Dim objDb1 As New EAD.DBCom
            Dim strSql, strSql_1, strSql_2, strSql_3, strCoyID, strUserID, strDeptAllowed As String
            Dim strParentCol, strChildCol As String
            Dim blnFinDept As Boolean = False
            Dim strTemp As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            strSql = "SELECT CM_FINDEPT_MODE, FUD_DEPT_CODE, FUD_VIEWOPTION, CDM_DEPT_INDEX "
            strSql &= "FROM COMPANY_MSTR "
            strSql &= "LEFT JOIN FINANCE_USER_DEPARTMENT ON CM_COY_ID = FUD_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
            strSql &= "LEFT JOIN COMPANY_DEPT_MSTR ON CDM_COY_ID = FUD_COY_ID AND CDM_DEPT_CODE = FUD_DEPT_CODE "
            strSql &= "WHERE FUD_COY_ID = '" & strCoyID & "' "
            strSql &= "AND FUD_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            strSql &= "AND (CDM_DEPT_INDEX IS NOT NULL OR CDM_DEPT_INDEX IS NULL AND FUD_VIEWOPTION = 1) "
            Dim tDS As DataSet = objDb1.FillDs(strSql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("CM_FINDEPT_MODE") = "N" Then ' Finance department mode set to off, so can view all dept
                    Exit For
                ElseIf tDS.Tables(0).Rows(j).Item("FUD_VIEWOPTION") = "1" Then ' view option = 1, so can view all dept
                    Exit For
                Else
                    blnFinDept = True
                    strDeptAllowed &= tDS.Tables(0).Rows(j).Item("CDM_DEPT_INDEX").ToString.Trim & ","
                End If
            Next

            '//join table to get PO Number
            'strSql_1 = "select X.*, Y.* from ( SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, POM_PO_Date, IM_PAYMENT_DATE, IM_INVOICE_TOTAL, POM_CURRENCY_CODE, IM_PRINTED, "
            strSql_1 = "SELECT CDM_DO_NO, CDM_GRN_NO, CDM_PO_NO, CDM_INVOICE_NO, CDM_S_COY_ID,IM_INVOICE_INDEX,'EPROC' AS DOC_TYPE, IM_INVOICE_NO,IM_INVOICE_TYPE, POM_PO_Date, IM_PAYMENT_DATE, IM_INVOICE_TOTAL, POM_CURRENCY_CODE, IM_PRINTED, "
            strSql_1 &= "IM_PAYMENT_TERM, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, IM_PO_INDEX, IM_S_COY_ID, POM_BILLING_METHOD, IM_INVOICE_STATUS, IM_GST_INVOICE, "
            strSql_1 &= "POM_BUYER_NAME, POM_S_COY_NAME, STATUS_DESC, STATUS_REMARK, POM_PO_NO, CDM_DEPT_NAME AS DEPT, POM_PAYMENT_METHOD, POM_PO_INDEX "
            strSql_1 &= "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX "
            strSql_1 &= "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' "
            strSql_1 &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX "

            If strDocNo <> "" Then
                strTemp = Common.BuildWildCard(strDocNo)
                strSql_1 &= "LEFT JOIN COMPANY_DOC_MATCH ON CDM_B_COY_ID='" & strCoyID & "' "
                strSql_1 &= "AND (CDM_PO_NO " & Common.ParseSQL(strTemp) & " OR CDM_INVOICE_NO " & Common.ParseSQL(strTemp) & " OR CDM_GRN_NO " & Common.ParseSQL(strTemp) & " OR CDM_DO_NO " & Common.ParseSQL(strTemp) & ") AND "
                strSql_1 &= "POM_PO_NO = CDM_PO_NO And IM_INVOICE_NO = CDM_INVOICE_NO And IM_S_COY_ID = CDM_S_COY_ID "
            Else
                strSql_1 &= "INNER JOIN COMPANY_DOC_MATCH ON CDM_B_COY_ID='" & strCoyID & "' AND "
                strSql_1 &= "POM_PO_NO = CDM_PO_NO And IM_INVOICE_NO = CDM_INVOICE_NO And IM_S_COY_ID = CDM_S_COY_ID "
            End If

            'If strDocNo <> "" Then
            '    strTemp = Common.BuildWildCard(strDocNo)
            '    strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "' " _
            '                & "AND (CDM_PO_NO " & Common.ParseSQL(strTemp) & " OR CDM_INVOICE_NO " & Common.ParseSQL(strTemp) & " OR CDM_GRN_NO " & Common.ParseSQL(strTemp) & " OR CDM_DO_NO " & Common.ParseSQL(strTemp) & ")) Y ON "

            'Else
            '    strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "' ) Y ON "

            'End If
            'strSql_2 &= " X.POM_PO_NO = Y.CDM_PO_NO AND X.IM_INVOICE_NO = Y.CDM_INVOICE_NO AND X.IM_S_COY_ID = Y.CDM_S_COY_ID"



            strSql_1 &= "WHERE POM_B_COY_ID = '" & strCoyID & "' "

            Dim objCompany As New Companies
            Dim strInvAppr As String = objCompany.GetInvApprMode(strCoyID)

            If strInvAppr <> "Y" Then
                If strStatus = "" Then ' trash
                    strSql_1 &= "AND IM_FOLDER = 1 "
                Else
                    strSql_1 &= "AND IM_INVOICE_STATUS IN (" & strStatus & ",5) AND IM_FOLDER = 0 "
                End If
            Else
                If strStatus = "" Then ' trash
                    strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                    strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE "
                    strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                    strSql_1 &= ") "

                    strSql_1 &= "AND IM_FOLDER = 1 "
                Else
                    If strStatus = "3,4" Then ' payment
                        'strSql_1 &= "AND ((IM_INVOICE_STATUS = " & invStatus.Paid
                        'strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                        'strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ = FA_AO_ACTION AND "
                        'strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                        'strSql_1 &= ")) OR ( "
                        'strSql_1 &= "IM_INVOICE_STATUS = " & invStatus.Approved
                        'strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                        'strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ <= FA_AO_ACTION AND "
                        'strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                        'strSql_1 &= "))"

                        '' Current fm approval
                        'strSql_1 &= "AND ((IM_INVOICE_STATUS IN (2,5) " ' pending, hold
                        'strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                        'strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ - 1 = FA_AO_ACTION AND "
                        'strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                        'strSql_1 &= ")) OR ( "
                        strSql_1 &= "AND ("
                        ' Previous fm approval
                        strSql_1 &= "IM_INVOICE_STATUS IN (3) " ' approved
                        strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                        ' Michelle (17/9/2007) - To solve the problem where fm cannot see those invoices that have been
                        '                        approved because of 'FA_SEQ <= FA_AO_ACTION'
                        ' strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ <= FA_AO_ACTION AND "
                        strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE (FA_SEQ <= FA_AO_ACTION OR IM_INVOICE_STATUS IN ('3','4')) AND "
                        strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                        strSql_1 &= ")"


                        strSql_1 &= ") AND IM_FOLDER = 0 "

                        'strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                        'strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ <= FA_AO_ACTION AND "
                        'strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                        'strSql_1 &= ")"
                    Else
                        strSql_1 &= "AND IM_INVOICE_STATUS IN (" & IIf(strStatus = "1", "1,2", strStatus) & ",5) AND IM_FOLDER = 0 "

                        If strStatus = invStatus.NewInv Or strStatus = invStatus.PendingAppr Then
                            strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                            strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ - 1 = FA_AO_ACTION AND "
                            strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"

                            If strStatus = invStatus.PendingAppr Then
                                strSql_1 &= " AND (FA_AGA_TYPE = 'FM' OR (FA_AGA_TYPE = 'FO' AND FA_SEQ > FA_AO_ACTION))"
                            End If

                            strSql_1 &= ")"
                        Else
                            strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                            strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ <= FA_AO_ACTION AND "
                            strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"

                            strSql_1 &= ")"
                        End If
                    End If
                End If
            End If

            If blnFinDept Then
                strSql_1 &= "AND POM_DEPT_INDEX IN (" & strDeptAllowed.Substring(0, strDeptAllowed.Length - 1) & ") "
            End If

            If strDocNo <> "" Then
                strTemp = Common.BuildWildCard(strDocNo)
                strSql_1 &= " AND (POM_PO_NO" & Common.ParseSQL(strTemp)
                strSql_1 &= " OR IM_INVOICE_NO" & Common.ParseSQL(strTemp)
                strSql_1 &= " OR POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & Common.ParseSQL(strTemp) & ") "
                strSql_1 &= " OR POM_PO_INDEX IN (SELECT PRM_PO_INDEX FROM PR_MSTR WHERE PRM_PR_NO" & Common.ParseSQL(strTemp) & ") "
                strSql_1 &= " OR POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & Common.ParseSQL(strTemp) & ")) "
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql_1 &= " AND POM_S_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            'If dteDateFr <> "" And dteDateTo <> "" Then
            '    strSql_1 &= " AND POM_PO_Date BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00")
            '    strSql_1 &= " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            'End If

            If strBuyer <> "" Then
                strTemp = Common.BuildWildCard(strBuyer)
                strSql_1 &= " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strSql_1 &= " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
            End If

            If doctype <> "" Then
                If doctype = "INV" Then
                    strSql_1 &= " AND IM_INVOICE_TYPE IS NULL "
                Else
                    strSql_1 &= " AND IM_INVOICE_TYPE = '" & doctype & "' "
                End If
            End If

            'strSql_1 &= ") X inner join "

            'If strDocNo <> "" Then
            '    strTemp = Common.BuildWildCard(strDocNo)
            '    strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "' " _
            '                & "AND (CDM_PO_NO " & Common.ParseSQL(strTemp) & " OR CDM_INVOICE_NO " & Common.ParseSQL(strTemp) & " OR CDM_GRN_NO " & Common.ParseSQL(strTemp) & " OR CDM_DO_NO " & Common.ParseSQL(strTemp) & ")) Y ON "

            'Else
            '    strSql_2 = " (SELECT * FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & strCoyID & "' ) Y ON "

            'End If
            'strSql_2 &= " X.POM_PO_NO = Y.CDM_PO_NO AND X.IM_INVOICE_NO = Y.CDM_INVOICE_NO AND X.IM_S_COY_ID = Y.CDM_S_COY_ID"
            strSql = strSql_1 & strSql_2
            'If ConfigurationManager.AppSettings("Env") <> "FTN" Then
            If blnEnterpriseVersion = True Then
                strSql &= getIPPINVTracking(strDocNo, strVendor, IPPstatus, invfrom, doctype)
            End If
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            ds = objDB.FillDs(strSql)

            'Dim dtLevel1 As DataTable
            'dtLevel1 = objDB.FillDs(strSql).Tables(0)
            '//cannot use relation because strsql_1 may return multiple record with same PO_NO
            'Dim parentCol, childCol As DataColumn

            'parentCol = ds.Tables(0).Columns(strParentCol)
            'childCol = ds.Tables(1).Columns(strChildCol)

            ' Create DataRelation.
            'Dim relDoc As DataRelation
            'relDoc = New DataRelation("match", parentCol, childCol)
            ' Add the relation to the DataSet.
            'ds.Relations.Add(relDoc)

            objDB = Nothing
            Return ds
        End Function
        'mimi 2018-06-06 : inv processing
        Function getInvoiceTracking_NoDocMatch(ByVal strUserID As String, ByVal strCoyID As String, ByVal strDocNo As String, ByVal strVendor As String, ByVal strBuyer As String, ByVal strDept As String, ByVal strStatus As String, Optional ByVal IPPstatus As String = "", Optional ByVal invfrom As String = "", Optional ByVal doctype As String = "", Optional ByVal dteDateFr As String = "", Optional ByVal dteDateTo As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal strCurr As String = "", Optional ByVal strFundType As String = "", Optional ByVal strResident As String = "", Optional ByVal strAmtFrom As String = "", Optional ByVal strAmtTo As String = "", Optional ByVal strDueDate As String = "", Optional ByVal strPayMode As String = "") As String

            '//Use Data Shaping because it is difficut to filter second table if we use DataRelation of ADO.Net
            Dim objDb1 As New EAD.DBCom
            Dim strSql, strSql_1, strSql_2, strSql_3, strDeptAllowed As String
            Dim strParentCol, strChildCol As String
            Dim blnFinDept As Boolean = False
            Dim strTemp As String

            strSql = "SELECT CM_FINDEPT_MODE, FUD_DEPT_CODE, FUD_VIEWOPTION, CDM_DEPT_INDEX "
            strSql &= "FROM COMPANY_MSTR "
            strSql &= "LEFT JOIN FINANCE_USER_DEPARTMENT ON CM_COY_ID = FUD_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
            strSql &= "LEFT JOIN COMPANY_DEPT_MSTR ON CDM_COY_ID = FUD_COY_ID AND CDM_DEPT_CODE = FUD_DEPT_CODE "
            strSql &= "WHERE FUD_COY_ID = '" & strCoyID & "' "
            strSql &= "AND FUD_USER_ID = '" & strUserID & "' "
            strSql &= "AND (CDM_DEPT_INDEX IS NOT NULL OR CDM_DEPT_INDEX IS NULL AND FUD_VIEWOPTION = 1) "
            Dim tDS As DataSet = objDb1.FillDs(strSql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If tDS.Tables(0).Rows(j).Item("CM_FINDEPT_MODE") = "N" Then ' Finance department mode set to off, so can view all dept
                    Exit For
                ElseIf tDS.Tables(0).Rows(j).Item("FUD_VIEWOPTION") = "1" Then ' view option = 1, so can view all dept
                    Exit For
                Else
                    blnFinDept = True
                    strDeptAllowed &= tDS.Tables(0).Rows(j).Item("CDM_DEPT_INDEX").ToString.Trim & ","
                End If
            Next

            'Jules 2018.07.30 - Added 'DISTINCT' to avoid duplicates of CDM_DO_NO,CDM_GRN_NO, CDM_PO_NO
            '//join table to get PO Number
            'strSql_1 = "select X.*, Y.* from ( SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, POM_PO_Date, IM_PAYMENT_DATE, IM_INVOICE_TOTAL, POM_CURRENCY_CODE, IM_PRINTED, "
            strSql_1 = "SELECT IM_INVOICE_INDEX, "
            'strSql_1 &= "CAST(GROUP_CONCAT(CDM_DO_NO) AS CHAR(20000)) AS CDM_DO_NO, CAST(GROUP_CONCAT(CDM_GRN_NO) AS CHAR(20000)) AS CDM_GRN_NO, CAST(GROUP_CONCAT(CDM_PO_NO) AS CHAR(20000)) AS CDM_PO_NO, CDM_S_COY_ID, "
            strSql_1 &= "CAST(GROUP_CONCAT(DISTINCT(CDM_DO_NO)) AS CHAR(20000)) AS CDM_DO_NO, CAST(GROUP_CONCAT(DISTINCT(CDM_GRN_NO)) AS CHAR(20000)) AS CDM_GRN_NO, CAST(GROUP_CONCAT(DISTINCT(CDM_PO_NO)) AS CHAR(20000)) AS CDM_PO_NO, CDM_S_COY_ID, "

            'strSql_1 &= "CAST((SELECT GROUP_CONCAT(PRM_PR_INDEX) AS PRM_PR_INDEX "
            'strSql_1 &= "FROM PR_MSTR WHERE PRM_COY_ID = 'hlb' AND "
            'strSql_1 &= "PRM_PR_INDEX IN (SELECT DISTINCT(POD_PR_INDEX) FROM PO_DETAILS WHERE POD_PO_NO = PO_MSTR.POM_PO_NO AND POD_COY_ID = '" & strCoyID & "') "
            'strSql_1 &= ") AS CHAR(20000)) AS PRM_PR_INDEX, "

            strSql_1 &= " CAST((SELECT GROUP_CONCAT(DISTINCT(POD_PR_INDEX)) FROM PO_DETAILS WHERE POD_PO_NO = PO_MSTR.POM_PO_NO AND POD_COY_ID = '" & strCoyID & "' "
            strSql_1 &= " ) AS CHAR(20000)) AS PRM_PR_INDEX, "

            'strSql_1 &= "CAST((SELECT GROUP_CONCAT(PRM_PR_NO) AS PRM_PR_NO "
            'strSql_1 &= "FROM PR_MSTR WHERE PRM_COY_ID = 'hlb' AND "
            'strSql_1 &= "PRM_PR_INDEX IN (SELECT DISTINCT(POD_PR_INDEX) FROM PO_DETAILS WHERE POD_PO_NO = PO_MSTR.POM_PO_NO AND POD_COY_ID = '" & strCoyID & "') "
            'strSql_1 &= ")AS CHAR(20000)) AS PRM_PR_NO, "

            strSql_1 &= " '' AS PRM_PR_NO, "

            strSql_1 &= "'EPROC' AS DOC_TYPE, IM_INVOICE_NO, IM_GST_INVOICE, IM_INVOICE_TYPE,POM_PO_Date, IM_PAYMENT_DATE, IM_INVOICE_TOTAL, POM_CURRENCY_CODE, IM_PRINTED, "
            strSql_1 &= "IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, IM_PO_INDEX, IM_S_COY_ID, POM_BILLING_METHOD, IM_INVOICE_STATUS, "
            strSql_1 &= "POM_BUYER_NAME, POM_S_COY_NAME, STATUS_DESC, STATUS_REMARK, POM_PO_NO, CDM_DEPT_NAME AS DEPT, POM_PAYMENT_METHOD, POM_PO_INDEX, "

            'Jules 2018.10.17
            'strSql_1 &= "IM_RESIDENT_TYPE, IM_PAYMENT_TERM, ID_ANALYSIS_CODE1  " 'mimi 2018-06-05 : inv processing
            strSql_1 &= "CM_RESIDENT AS IM_RESIDENT_TYPE, POM_PAYMENT_METHOD AS IM_PAYMENT_TERM, CAST(GROUP_CONCAT(DISTINCT(ID_ANALYSIS_CODE1)) AS CHAR(20000)) AS ID_ANALYSIS_CODE1, "
            strSql_1 &= "IF(POM_CURRENCY_CODE = 'MYR',IM_INVOICE_TOTAL,IM_INVOICE_TOTAL * (SELECT CE_RATE FROM COMPANY_EXCHANGERATE WHERE CE_COY_ID = '" & strCoyID & "' AND CE_CURRENCY_CODE = POM_CURRENCY_CODE " &
                        "AND CE_DELETED='N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE())) INVAMT_INMYR "
            'End modification.

            strSql_1 &= "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX "
            strSql_1 &= "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' "
            strSql_1 &= "LEFT JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO " 'mimi 2018-06-06 : inv processing
            strSql_1 &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX "

            If strDocNo <> "" Then
                strTemp = Common.BuildWildCard(strDocNo)
                strSql_1 &= "LEFT JOIN COMPANY_DOC_MATCH ON CDM_B_COY_ID='" & strCoyID & "' "
                strSql_1 &= "AND (CDM_PO_NO " & Common.ParseSQL(strTemp) & " OR CDM_INVOICE_NO " & Common.ParseSQL(strTemp) & " OR CDM_GRN_NO " & Common.ParseSQL(strTemp) & " OR CDM_DO_NO " & Common.ParseSQL(strTemp) & ") AND "
                strSql_1 &= "POM_PO_NO = CDM_PO_NO And IM_INVOICE_NO = CDM_INVOICE_NO And IM_S_COY_ID = CDM_S_COY_ID "
            Else
                strSql_1 &= "INNER JOIN COMPANY_DOC_MATCH ON CDM_B_COY_ID='" & strCoyID & "' AND "
                strSql_1 &= "POM_PO_NO = CDM_PO_NO And IM_INVOICE_NO = CDM_INVOICE_NO And IM_S_COY_ID = CDM_S_COY_ID "
            End If

            'Jules 2018.10.17
            strSql_1 &= "INNER JOIN COMPANY_MSTR ON CM_COY_ID=CDM_S_COY_ID AND CM_COY_ID=POM_S_COY_ID "
            'End modification.

            strSql_1 &= "WHERE POM_B_COY_ID = '" & strCoyID & "' "

            'Jules 2018.07.20 - Commented out because (P2P) users tied to both Resident and Non-Resident workflows should be able to see invoices for both.
            ''Zulham 07072018 - PAMB
            ''Added a condition for resident type
            'Dim isResident = objDb1.GetVal("SELECT DISTINCT IFNULL(agm_resident,'') 'agm_resident' " &
            '                                "From approval_grp_mstr, approval_grp_fo " &
            '                                "Where agfo_grp_index = agm_grp_index " &
            '                                "And (agfo_fo = '" & strUserID & "' OR agfo_a_fo = '" & strUserID & "' OR agfo_a_fo_2 = '" & strUserID & "' OR agfo_a_fo_3 = '" & strUserID & "' OR agfo_a_fo_4 = '" & strUserID & "') " &
            '                                "LIMIT 1 ")
            'Select Case isResident
            '    Case "Y", "N"
            '        strSql_1 &= " AND IM_RESIDENT_TYPE = '" & isResident & "' "
            'End Select
            ''End

            Dim objCompany As New Companies
            Dim strInvAppr As String = objCompany.GetInvApprMode(strCoyID)

            If strInvAppr <> "Y" Then
                If strStatus = "" Then ' trash
                    strSql_1 &= "AND IM_FOLDER = 1 "
                Else
                    strSql_1 &= "AND IM_INVOICE_STATUS IN (" & strStatus & ",5) And IM_FOLDER = 0 "
                End If
            Else
                If strStatus = "" Then ' trash
                    strSql_1 &= " And IM_INVOICE_INDEX In ("
                    strSql_1 &= " Select FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE "
                    strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                    strSql_1 &= ") "

                    strSql_1 &= "AND IM_FOLDER = 1 "
                Else
                    If strStatus = "3,4" Then ' payment
                        strSql_1 &= "AND ("
                        ' Previous fm approval
                        strSql_1 &= "IM_INVOICE_STATUS IN (3) " ' approved
                        strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                        ' Michelle (17/9/2007) - To solve the problem where fm cannot see those invoices that have been
                        '                        approved because of 'FA_SEQ <= FA_AO_ACTION'
                        strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE (FA_SEQ <= FA_AO_ACTION OR IM_INVOICE_STATUS IN ('3','4')) AND "
                        strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                        strSql_1 &= ")"

                        strSql_1 &= ") AND IM_FOLDER = 0 "

                    Else
                        strSql_1 &= "AND IM_INVOICE_STATUS IN (" & IIf(strStatus = "1", "1,2", strStatus) & ",5) AND IM_FOLDER = 0 "

                        If strStatus = invStatus.NewInv Or strStatus = invStatus.PendingAppr Then
                            strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                            strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ - 1 = FA_AO_ACTION AND "
                            strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"

                            If strStatus = invStatus.PendingAppr Then
                                strSql_1 &= " AND (FA_AGA_TYPE = 'FM' OR (FA_AGA_TYPE = 'FO' AND FA_SEQ > FA_AO_ACTION))"
                            End If

                            strSql_1 &= ")"
                        Else
                            strSql_1 &= " AND IM_INVOICE_INDEX IN ("
                            strSql_1 &= " SELECT FA_INVOICE_INDEX FROM FINANCE_APPROVAL WHERE FA_SEQ <= FA_AO_ACTION AND "
                            strSql_1 &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"

                            strSql_1 &= ")"
                        End If
                    End If
                End If
            End If

            If blnFinDept Then
                strSql_1 &= "AND POM_DEPT_INDEX IN (" & strDeptAllowed.Substring(0, strDeptAllowed.Length - 1) & ") "
            End If

            If strDocNo <> "" Then
                strTemp = Common.BuildWildCard(strDocNo)
                strSql_1 &= " AND (POM_PO_NO" & Common.ParseSQL(strTemp)
                strSql_1 &= " OR IM_INVOICE_NO" & Common.ParseSQL(strTemp)
                strSql_1 &= " OR POM_PO_INDEX IN (SELECT GM_PO_INDEX FROM GRN_MSTR WHERE GM_GRN_NO" & Common.ParseSQL(strTemp) & ") "
                strSql_1 &= " OR POM_PO_INDEX IN (SELECT PRM_PO_INDEX FROM PR_MSTR WHERE PRM_PR_NO" & Common.ParseSQL(strTemp) & ") "
                strSql_1 &= " OR POM_PO_INDEX IN (SELECT DOM_PO_INDEX FROM DO_MSTR WHERE DOM_DO_NO" & Common.ParseSQL(strTemp) & ")) "
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql_1 &= " AND POM_S_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strCurr <> "" Then
                strSql_1 &= " AND POM_CURRENCY_CODE = '" & strCurr & "' "
            End If

            If dteDateFr <> "" And dteDateTo <> "" Then
                strSql_1 &= " AND IM_FM_APPROVED_DATE BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00")
                strSql_1 &= " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If strBuyer <> "" Then
                strTemp = Common.BuildWildCard(strBuyer)
                strSql_1 &= " AND POM_BUYER_NAME" & Common.ParseSQL(strTemp)
            End If

            If strDept <> "" Then
                strTemp = Common.BuildWildCard(strDept)
                strSql_1 &= " AND CDM_DEPT_NAME" & Common.ParseSQL(strTemp)
            End If

            If doctype <> "" Then
                If doctype = "INV" Then
                    strSql_1 &= " AND IM_INVOICE_TYPE IS NULL "
                Else
                    strSql_1 &= " AND IM_INVOICE_TYPE = '" & doctype & "' "
                End If
            End If

            'mimi 2018-06-27 : inv processing
            If strFundType <> "" Then
                If InStr(strFundType, ":") Then
                    strSql_1 &= " AND ID_ANALYSIS_CODE1 = '" & strFundType.Substring(0, InStr(strFundType, ":") - 1) & "' "
                Else
                    strSql_1 &= " AND ID_ANALYSIS_CODE1 = '" & Common.Parse(strFundType) & "' "
                End If
            End If
            ''mimi 2018-06-06 : inv processing
            'If strFundType <> "" Then
            '    strSql_1 &= " AND ID_ANALYSIS_CODE1 = '" & strFundType & "' "
            'End If

            If strResident <> "" Then
                'Jules 2018.10.17
                'strSql_1 &= " AND IM_RESIDENT_TYPE = '" & strResident & "' "
                strSql_1 &= " AND CM_RESIDENT = '" & strResident & "' "
                'End modification.
            End If

            If strAmtFrom <> "" And strAmtTo <> "" Then
                strSql_1 &= " AND IM_INVOICE_TOTAL BETWEEN '" & strAmtFrom & "' "
                strSql_1 &= " AND '" & strAmtTo & "' "
            ElseIf strAmtFrom <> "" AndAlso strAmtTo = "" Then
                strSql_1 &= " AND IM_INVOICE_TOTAL >= '" & strAmtFrom & "' "
            ElseIf strAmtFrom = "" AndAlso strAmtTo <> "" Then
                strSql_1 &= " AND IM_INVOICE_TOTAL <= '" & strAmtTo & "' "
            End If

            If strDueDate <> "" Then
                strSql_1 &= " AND DATE(IM_PAYMENT_DATE) = " & Common.ConvertDate(strDueDate) & " "
            End If

            If strPayMode <> "" Then
                'Jules 2018.10.17
                'strSql_1 &= " And IM_PAYMENT_TERM = '" & strPayMode & "' "
                strSql_1 &= " And POM_PAYMENT_METHOD = '" & strPayMode & "' "
                'End modification.
            End If
            'end

            strSql_1 &= " GROUP BY IM_INVOICE_INDEX,  CDM_INVOICE_NO, CDM_S_COY_ID,  IM_INVOICE_NO, POM_PO_Date, IM_PAYMENT_DATE, "

            'Jules 2018.10.17
            'strSql_1 &= " IM_INVOICE_TOTAL, POM_CURRENCY_CODE, IM_PRINTED,  IM_PAYMENT_TERM, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, "
            strSql_1 &= " IM_INVOICE_TOTAL, POM_CURRENCY_CODE, IM_PRINTED,  POM_PAYMENT_METHOD, IM_FINANCE_REMARKS, IM_FM_APPROVED_DATE, "
            'End modification.

            strSql_1 &= " IM_PO_INDEX, IM_S_COY_ID,  POM_BILLING_METHOD, IM_INVOICE_STATUS, POM_BUYER_NAME, POM_S_COY_NAME, STATUS_DESC,  "
            strSql_1 &= " STATUS_REMARK, POM_PO_NO, CDM_DEPT_NAME, POM_PAYMENT_METHOD, POM_PO_INDEX "

            strSql = strSql_1 & strSql_2
            'If ConfigurationManager.AppSettings("Env") <> "FTN" Then
            If blnEnterpriseVersion = True Then
                'Jules 2018.10.15 - Added Fund Type.
                strSql &= getIPPINVTracking_NoDocMatch(strCoyID, strUserID, strDocNo, strVendor, IPPstatus, invfrom, doctype, dteDateFr, dteDateTo, strCurr, strFundType, strResident, strAmtFrom, strAmtTo, strDueDate, strPayMode)
            End If
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            ds = objDB.FillDs(strSql)

            objDB = Nothing
            'Return ds
            Return strSql
        End Function

        Function trashInv(ByVal dtInv As DataTable)
            Dim strsql As String
            Dim objDb As New EAD.DBCom
            Dim strAryQuery(0) As String
            Dim i As Integer
            For i = 0 To dtInv.Rows.Count - 1
                strsql = "UPDATE INVOICE_MSTR SET IM_FOLDER = 1, "
                strsql &= "IM_FINANCE_REMARKS = '" & Common.Parse(dtInv.Rows(i)("FinRemark")) & "' "
                strsql &= "WHERE IM_INVOICE_NO = '" & Common.Parse(dtInv.Rows(i)("InvNo")) & "' "
                strsql &= "AND IM_S_COY_ID = '" & Common.Parse(dtInv.Rows(i)("Vendor")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next
            objDb.BatchExecute(strAryQuery)
        End Function

        Function printInv(ByVal dtInv As DataTable)
            Dim strsql As String
            Dim objDb As New EAD.DBCom
            Dim strAryQuery(0) As String
            Dim i As Integer
            For i = 0 To dtInv.Rows.Count - 1
                strsql = "UPDATE INVOICE_MSTR SET IM_PRINTED = '1' "
                strsql &= "WHERE IM_INVOICE_NO = '" & Common.Parse(dtInv.Rows(i)("INVNO")) & "' "
                strsql &= "AND IM_S_COY_ID = '" & Common.Parse(dtInv.Rows(i)("vcomid")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next
            objDb.BatchExecute(strAryQuery)
        End Function

        Function TrackRFQ(ByVal strDocNo As String, ByVal strVendor As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strBuyer As String, ByVal strDept As String) As DataSet
            Dim strSql, strCoyID As String
            Dim strTemp As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            'Michelle (22/4/2011) - To cater for FTN, remove the PR
            'strSql = "SELECT RM_RFQ_ID,RM_RFQ_NO,RM_RFQ_NAME,RM_CREATED_ON,RM_EXPIRY_DATE,RRM_V_Company_ID,RRM_ACTUAL_QUOT_NUM," _
            '& "PRM_PR_INDEX,PRM_PR_NO,PRM_REQ_NAME,PRM_S_COY_ID,PRM_S_COY_NAME,CDM_DEPT_NAME FROM RFQ_MSTR A, RFQ_REPLIES_MSTR B,PR_MSTR C " _
            '& "LEFT OUTER JOIN COMPANY_DEPT_MSTR D ON C.PRM_DEPT_INDEX=D.CDM_DEPT_INDEX " _
            '& "WHERE A.RM_RFQ_ID = C.PRM_RFQ_INDEX AND B.RRM_V_Company_ID = C.PRM_S_COY_ID " _
            '& "AND A.RM_RFQ_ID=B.RRM_RFQ_ID AND RM_Coy_ID='" & strCoyID & "'"
            strSql = "SELECT RM_Coy_ID,RM_RFQ_ID,RM_RFQ_NO,RM_RFQ_NAME,RM_CREATED_ON,RM_EXPIRY_DATE,RRM_V_Company_ID,RRM_ACTUAL_QUOT_NUM," _
            & "POM_PO_INDEX,POM_PO_NO,POM_BUYER_NAME,POM_S_COY_ID,POM_S_COY_NAME,CDM_DEPT_NAME, IFNULL(CAST(C.POM_RFQ_INDEX AS CHAR), '') AS POM_RFQ_INDEX FROM RFQ_MSTR A, RFQ_REPLIES_MSTR B,PO_MSTR C " _
            & "LEFT OUTER JOIN COMPANY_DEPT_MSTR D ON C.POM_DEPT_INDEX=D.CDM_DEPT_INDEX " _
            & "WHERE A.RM_B_Display_Status='0' AND A.RM_RFQ_ID = C.POM_RFQ_INDEX AND B.RRM_V_Company_ID = C.POM_S_COY_ID " _
            & "AND A.RM_RFQ_ID=B.RRM_RFQ_ID AND RM_Coy_ID='" & strCoyID & "' AND POM_FULFILMENT != 0  AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

            If strDocNo <> "" Then
                strSql = strSql & " AND RM_RFQ_NO" & Common.ParseSQL(strDocNo)
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql = strSql & " AND POM_S_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If dteDateFr <> "" And dteDateTo <> "" Then
                strSql = strSql & " AND RM_CREATED_ON BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If strBuyer <> "" Then
                strSql = strSql & " AND POM_Buyer_NAME" & Common.ParseSQL(strBuyer)
            End If

            If strDept <> "" Then
                strSql = strSql & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDept)
            End If

            'Union with old data
            strSql = strSql & " UNION "
            strSql = strSql & "SELECT RM_Coy_ID,RM_RFQ_ID,RM_RFQ_NO,RM_RFQ_NAME,RM_CREATED_ON,RM_EXPIRY_DATE,RRM_V_Company_ID,RRM_ACTUAL_QUOT_NUM,PRM_PR_INDEX,POM_PO_NO,PRM_REQ_NAME,PRM_S_COY_ID,PRM_S_COY_NAME, " _
                        & "CDM_DEPT_NAME, IFNULL(CAST(PO_MSTR.POM_RFQ_INDEX AS CHAR), CAST(C.PRM_RFQ_INDEX AS CHAR)) POM_RFQ_INDEX FROM RFQ_MSTR A, RFQ_REPLIES_MSTR B,PR_MSTR C LEFT OUTER JOIN COMPANY_DEPT_MSTR D ON C.PRM_DEPT_INDEX=D.CDM_DEPT_INDEX " _
                        & "INNER JOIN po_mstr ON prm_po_index = pom_po_index WHERE " _
                        & "A.RM_B_Display_Status='0' AND A.RM_RFQ_ID = C.PRM_RFQ_INDEX AND B.RRM_V_Company_ID = C.PRM_S_COY_ID " _
                        & "AND A.RM_RFQ_ID=B.RRM_RFQ_ID AND RM_Coy_ID='" & strCoyID & "'  AND POM_CREATED_BY = '" & HttpContext.Current.Session("UserId") & "' AND POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            If strDocNo <> "" Then
                strSql = strSql & " AND RM_RFQ_NO" & Common.ParseSQL(strDocNo)
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql = strSql & " AND PRM_S_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If dteDateFr <> "" And dteDateTo <> "" Then
                strSql = strSql & " AND RM_CREATED_ON BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") &
                " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If strBuyer <> "" Then
                strSql = strSql & " AND PRM_REQ_NAME" & Common.ParseSQL(strBuyer)
            End If

            If strDept <> "" Then
                strSql = strSql & " AND CDM_DEPT_NAME" & Common.ParseSQL(strDept)
            End If

            'strSql = strSql & " GROUP BY RM_RFQ_ID"
            Dim objDB As New EAD.DBCom
            Dim ds As DataSet
            ds = objDB.FillDs(strSql)

            objDB = Nothing
            Return ds
        End Function

        Public Function getInvoiceSave(ByVal strIndex As String) As DataSet
            Dim strsql As String
            Dim objDB As New EAD.DBCom
            Dim ds As New DataSet
            strsql = "SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, IM_CREATED_ON, IM_PAYMENT_DATE, ID_INVOICE_LINE, "
            strsql &= "ID_B_ITEM_CODE, POD_VENDOR_ITEM_CODE, ID_PRODUCT_DESC, ID_RECEIVED_QTY, POM_CURRENCY_CODE, "
            strsql &= "ID_UNIT_COST, IM_EXCHANGE_RATE, POM_S_REMARK, POM_BUYER_NAME, CDM_DEPT_CODE, PRM_PR_NO, "
            strsql &= "POM_PO_NO, GM_GRN_NO, ID_PO_LINE "
            strsql &= "FROM INVOICE_MSTR INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX "
            strsql &= "LEFT JOIN COMPANY_DEPT_MSTR ON POM_DEPT_INDEX = CDM_DEPT_INDEX "
            strsql &= "LEFT JOIN INVOICE_DETAILS ON IM_S_COY_ID = ID_S_COY_ID AND IM_INVOICE_NO = ID_INVOICE_NO "
            strsql &= "LEFT JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID AND POD_PO_LINE = ID_PO_LINE "
            strsql &= "LEFT JOIN PR_MSTR ON PRM_PO_INDEX = POM_PO_INDEX "
            strsql &= "LEFT JOIN GRN_MSTR ON GM_PO_INDEX = IM_PO_INDEX "
            strsql &= "WHERE IM_INVOICE_INDEX IN (" & strIndex & ") "
            ds = objDB.FillDs(strsql)
            getInvoiceSave = ds
        End Function



        Function ApproveInvoice(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strInvoiceNo As String, ByVal strVendor As String, ByVal strRole As String, ByVal strAORemark As String,
            ByVal blnRelief As Boolean, Optional ByRef blnHighestLevel As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal blnMass As Boolean = False, Optional ByVal blnAckCN As Boolean = False) As String

            Dim objDB As New EAD.DBCom
            Dim strSql As String
            Dim intInvoiceIndex, intInvoiceStatus As Integer
            Dim dblInvoiceAmount As Double = 0

            Dim strSqlAry(0) As String
            Dim strMsg As String


            'PRM_STATUS_CHANGED_BY,PRM_S_COY_ID

            strSql = "SELECT IM_INVOICE_INDEX, IM_INVOICE_STATUS, IM_INVOICE_TOTAL FROM INVOICE_MSTR WHERE IM_Invoice_No= '" & strInvoiceNo & "'"
            strSql &= " AND IM_S_COY_ID='" & strVendor & "'"

            Dim tDS As DataSet = objDB.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intInvoiceIndex = tDS.Tables(0).Rows(0).Item("IM_INVOICE_INDEX")
                intInvoiceStatus = tDS.Tables(0).Rows(0).Item("IM_INVOICE_STATUS")
                dblInvoiceAmount = tDS.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL")

                If intInvoiceStatus = invStatus.Approved Then
                    strMsg = "The invoice has already been approved. Approving of this invoice is not allowed. "
                ElseIf intInvoiceStatus = invStatus.Paid Then
                    strMsg = "The invoice has already been paid. Approving of this invoice is not allowed. "
                ElseIf intInvoiceStatus = invStatus.PendingAppr Then
                    If objDB.Get1ColumnCheckNull("company_mstr", "cm_inv_appr", " WHERE cm_coy_id='" & strCoyID & "'") = "Y" Then
                        If isApproved(intInvoiceIndex, strRole, strLoginUser) Then 'Michelle 24/68/2007 - To remove the error message
                            strMsg = "You have already approved this invoice. Approving of this invoice is not allowed."
                        End If
                    End If
                ElseIf intInvoiceStatus = invStatus.NewInv Then
                    If objDB.Get1ColumnCheckNull("company_mstr", "cm_inv_appr", " WHERE cm_coy_id='" & strCoyID & "'") = "Y" Then
                        If objDB.Get1ColumnCheckNull("company_mstr", "cm_findept_mode", " WHERE cm_coy_id='" & strCoyID & "'") = "Y" Then
                            If isApproved(intInvoiceIndex, strRole, strLoginUser) Then
                                strMsg = "You have already approved this invoice. Approving of this invoice is not allowed."
                            End If
                        End If
                    End If
                End If
            End If

            If strMsg <> "" Then Return strMsg

            If strRole <> "FO" Then
                blnHighestLevel = isHighestLevel(intInvoiceIndex, strRole)
            End If

            '//update INVIOCE status, status_changed_by,status_changed_date
            strSql = "UPDATE INVOICE_MSTR SET IM_INVOICE_STATUS=" & invStatus.PendingAppr &
            ",IM_STATUS_CHANGED_BY='" & strLoginUser & "',IM_STATUS_CHANGED_ON=" &
            Common.ConvertDate(Now) & " WHERE IM_INVOICE_Index=" & intInvoiceIndex

            Common.Insert2Ary(strSqlAry, strSql)

            Dim blnSkipNextLevel As Boolean = False

            If strRole = "FO" And getInvAppLimit(strLoginUser, strCoyID) >= dblInvoiceAmount Then
                updateAppAction(strLoginUser, strSqlAry, intInvoiceIndex, strAORemark, blnRelief, True, blnEnterpriseVersion)
                blnSkipNextLevel = True
            Else
                'updateAppAction(strSqlAry, intInvoiceIndex, strAORemark, blnRelief, strRole = "FM" And blnHighestLevel)
                updateAppAction(strLoginUser, strSqlAry, intInvoiceIndex, strAORemark, blnRelief, False, blnEnterpriseVersion)
            End If

            'If Invoice raised from PO with contract item, run it. (For Enterprise Ver/ FTN Ver)
            If blnMass = True Then
                strSql = "SELECT '*' FROM INVOICE_MSTR " &
                        "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " &
                        "WHERE IM_INVOICE_INDEX = " & intInvoiceIndex & " " &
                        "AND (ID_GST_INPUT_TAX_CODE IS NULL OR ID_GST_INPUT_TAX_CODE = '')"

                If objDB.Exist(strSql) > 0 Then
                    updateInvTaxCode(intInvoiceIndex, strSqlAry)
                End If
            End If

            'Modified for Agora GST Stage 2 - CH - 26/02/2015
            If blnAckCN = True Then
                strSql = "UPDATE CREDIT_NOTE_MSTR SET CNM_CN_STATUS = '2', CNM_STATUS_CHANGED_BY = '" & strLoginUser & "', CNM_STATUS_CHANGED_ON = NOW() " &
                        "WHERE CNM_INV_NO = '" & Common.Parse(strInvoiceNo) & "' AND CNM_CN_S_COY_ID = '" & strVendor & "' AND CNM_CN_STATUS = '1'"
                Common.Insert2Ary(strSqlAry, strSql)
            End If
            '---------------------------------------------------

            Dim objUsers As New Users

            If strRole = "FO" Then
                objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.InvoiceMod, WheelUserActivity.FO_ApproveInvoice, strInvoiceNo)
            ElseIf strRole = "FM" Then
                objUsers.Log_UserActivity(strLoginUser, strCoyID, strSqlAry, WheelModule.InvoiceMod, WheelUserActivity.FM_ApprovePayment, strInvoiceNo)
            End If

            objUsers = Nothing

            If Not objDB.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                If Not blnHighestLevel Or strRole = "FO" Then
                    '//only send mail if transaction successfully created
                    Dim objMail As New Email

                    '//next ao
                    If blnSkipNextLevel Then strRole = "FM"

                    Dim objCompany As New Companies
                    Dim strInvAppr As String = objCompany.GetInvApprMode(strCoyID)

                    If strInvAppr = "Y" Then
                        sendMailToApproval(strCoyID, strInvoiceNo, intInvoiceIndex, strRole)
                    Else
                        'Michelle (CR0031) - To send email to all the FMs for non-invoice approval flow
                        objMail.sendNotification(EmailType.InvoiceApproval, strLoginUser, strCoyID, "", strInvoiceNo, "")
                    End If
                End If
            End If
            Return strMsg
        End Function

        Function isApproved(ByVal intInvoiceIndex As Long, ByVal strRole As String, ByVal strUser As String) As Boolean
            Dim objDB As New EAD.DBCom
            Dim strSql As String


            isApproved = True

            strSql = "SELECT 1 FROM FINANCE_APPROVAL "
            strSql &= "WHERE FA_INVOICE_INDEX = '" & intInvoiceIndex & "' "
            strSql &= "AND (FA_AO = '" & strUser & "' OR fA_A_AO = '" & strUser & "' OR fA_A_AO_2 = '" & strUser & "' OR fA_A_AO_3 = '" & strUser & "' OR fA_A_AO_4 = '" & strUser & "') "
            strSql &= "AND FA_SEQ = FA_AO_ACTION + 1 "
            strSql &= "AND FA_AGA_TYPE = '" & strRole & "' "
            strSql &= "ORDER BY FA_SEQ"
            Dim tDS As DataSet = objDB.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                isApproved = False
            End If

        End Function

        Function isHighestLevel(ByVal intInvoiceIndex As Long, ByVal strRole As String) As Boolean

            isHighestLevel = False

            Dim objDB As New EAD.DBCom
            Dim strSql As String


            strSql = "SELECT MAX(FA_SEQ) AS HIGHEST_SEQ, FA_AO_ACTION FROM FINANCE_APPROVAL "
            strSql &= "WHERE FA_INVOICE_INDEX = '" & intInvoiceIndex & "' "
            strSql &= "AND FA_AGA_TYPE = '" & strRole & "' "
            strSql &= "GROUP BY FA_AO_ACTION"
            Dim tDS As DataSet = objDB.FillDs(strSql)

            If tDS.Tables(0).Rows.Count > 0 Then
                If Common.parseNull(tDS.Tables(0).Rows(0).Item("HIGHEST_SEQ")) = Common.parseNull(tDS.Tables(0).Rows(0).Item("FA_AO_ACTION"), 0) + 1 Then
                    isHighestLevel = True
                End If
            End If

        End Function

        Function getInvAppLimit(ByVal strLoginUser As String, ByVal strCompanyId As String) As Double
            Dim objDB As New EAD.DBCom
            Dim strSql As String


            Dim dblAppLimit As Double = 0


            strSql = "SELECT UM_INVOICE_APP_LIMIT FROM USER_MSTR " &
                "WHERE UM_USER_ID = '" & strLoginUser & "' AND UM_COY_ID = '" & strCompanyId & "'"

            Dim tDS As DataSet = objDB.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                dblAppLimit = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_INVOICE_APP_LIMIT"))
            End If


            Return dblAppLimit
        End Function

        Sub updateAppAction(ByVal strLoginUser As String, ByRef pQuery() As String, ByVal intInvoiceIndex As Long, ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal blnHighestLevel As Boolean, Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim objDB As New EAD.DBCom
            Dim strSql As String

            If blnRelief Then
                strSql = "UPDATE FINANCE_APPROVAL SET FA_AO_REMARK='" & Common.Parse(strAORemark) & "',FA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",FA_AO='" & strLoginUser & "',FA_ON_BEHALFOF = FA_AO, FA_ACTIVE_AO='" &
                strLoginUser & "' WHERE FA_INVOICE_INDEX=" & intInvoiceIndex & " AND FA_SEQ = FA_AO_ACTION + 1"
            Else
                strSql = "UPDATE FINANCE_APPROVAL SET FA_AO_REMARK='" & Common.Parse(strAORemark) & "',FA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",FA_ACTIVE_AO='" & strLoginUser & "' WHERE FA_INVOICE_INDEX=" & intInvoiceIndex & " AND FA_SEQ = FA_AO_ACTION + 1"
            End If

            Common.Insert2Ary(pQuery, strSql)

            'If blnHighestLevel And HttpContext.Current.Session("Env") <> "FTN" Then
            If blnHighestLevel And blnEnterpriseVersion = True Then
                Dim minFA_SEQ As String
                minFA_SEQ = objDB.Get1Column("FINANCE_APPROVAL", "MIN(FA_SEQ) - 1", " WHERE FA_AGA_TYPE = 'FM' AND FA_INVOICE_INDEX=" & intInvoiceIndex & "")
                strSql = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION = ('" & minFA_SEQ & "') WHERE FA_INVOICE_INDEX=" & intInvoiceIndex
                Common.Insert2Ary(pQuery, strSql)
            Else
                strSql = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION = FA_AO_ACTION + 1 WHERE FA_INVOICE_INDEX=" & intInvoiceIndex
                Common.Insert2Ary(pQuery, strSql)
            End If

        End Sub

        'Jules 2018.05.16 - PAMB Scrum 3
        'Sub updateAppRemark(ByVal intInvoiceIndex As Long, ByVal strAORemark As String, Optional ByVal intSeq As Integer = Nothing, Optional ByVal strPT As String = "", Optional ByVal aryTemp As ArrayList = Nothing, Optional ByVal blnAck As Boolean = False)
        Sub updateAppRemark(ByVal intInvoiceIndex As Long, ByVal strAORemark As String, Optional ByVal intSeq As Integer = Nothing, Optional ByVal strPT As String = "", Optional ByVal aryTemp As ArrayList = Nothing, Optional ByVal blnAck As Boolean = False, Optional ByVal aryFundType As ArrayList = Nothing)

            Dim objDB As New EAD.DBCom
            Dim strSql, strLoginUser As String
            Dim i As Integer

            strSql = "UPDATE FINANCE_APPROVAL SET FA_AO_REMARK='" & Common.Parse(strAORemark) & "' " &
                "WHERE FA_INVOICE_INDEX=" & intInvoiceIndex

            If intSeq = Nothing Then
                strSql &= " AND FA_SEQ = FA_AO_ACTION + 1"
            Else
                strSql &= " AND FA_SEQ = " & intSeq
            End If

            objDB.Execute(strSql)

            If strPT <> "" Then
                strSql = "UPDATE INVOICE_MSTR SET IM_PAYMENT_TERM = '" & Common.Parse(strPT) & "' " &
                "WHERE IM_INVOICE_INDEX=" & intInvoiceIndex
                objDB.Execute(strSql)
            End If

            If Not aryTemp Is Nothing Then
                For i = 0 To aryTemp.Count - 1
                    'Jules 2018.05.16 - PAMB Scrum 3
                    If aryFundType IsNot Nothing AndAlso aryTemp(i).TAX = aryFundType(i).FUNDTYPE Then
                        strSql = "UPDATE INVOICE_DETAILS " &
                            "INNER JOIN INVOICE_MSTR ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                            "SET ID_GST_INPUT_TAX_CODE = '" & Common.Parse(aryTemp(i).TAXCODE) & "', " &
                            "ID_ANALYSIS_CODE1 = '" & Common.Parse(aryFundType(i).FUNDTYPECODE) & "' " &
                            "WHERE IM_INVOICE_INDEX = " & intInvoiceIndex & " " &
                            "AND ID_PO_LINE = '" & aryTemp(i).TAX & "'"
                    Else 'Original code
                        strSql = "UPDATE INVOICE_DETAILS " &
                            "INNER JOIN INVOICE_MSTR ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                            "SET ID_GST_INPUT_TAX_CODE = '" & Common.Parse(aryTemp(i).TAXCODE) & "' " &
                            "WHERE IM_INVOICE_INDEX = " & intInvoiceIndex & " " &
                            "AND ID_PO_LINE = '" & aryTemp(i).TAX & "'"
                    End If
                    'End modification.

                    objDB.Execute(strSql)

                    'CH - 22 Apr: Update Tax Code link to Credit Note
                    If blnAck = True Then
                        strSql = "UPDATE CREDIT_NOTE_DETAILS " &
                               "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                               "INNER JOIN INVOICE_MSTR ON IM_INVOICE_NO = CNM_INV_NO AND IM_S_COY_ID = CNM_CN_S_COY_ID " &
                               "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND ID_INVOICE_LINE = CND_INV_LINE " &
                               "SET CND_GST_INPUT_TAX_CODE = '" & Common.Parse(aryTemp(i).TAXCODE) & "' " &
                               "WHERE IM_INVOICE_INDEX = " & intInvoiceIndex & " AND ID_PO_LINE = " & aryTemp(i).TAX & " " &
                               "AND (CND_QTY > 0 AND CND_UNIT_COST > 0) " &
                               "AND (CND_GST_INPUT_TAX_CODE = '' OR CND_GST_INPUT_TAX_CODE IS NULL) " &
                               "AND CNM_CN_STATUS = 1 "
                        objDB.Execute(strSql)
                    End If
                Next

            Else 'Jules 2018.05.16 - PAMB Scrum 3.
                If Not aryFundType Is Nothing Then
                    For i = 0 To aryFundType.Count - 1
                        strSql = "UPDATE INVOICE_DETAILS " &
                           "INNER JOIN INVOICE_MSTR ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                           "SET ID_ANALYSIS_CODE1 = '" & Common.Parse(aryFundType(i).FUNDTYPECODE) & "' " &
                           "WHERE IM_INVOICE_INDEX = " & intInvoiceIndex & " " &
                           "AND ID_PO_LINE = '" & aryFundType(i).FUNDTYPE & "'"
                    Next
                End If
            End If


        End Sub

        Public Function sendMailToApproval(ByVal companyID As String, ByVal strDocNo As String, ByVal intIndex As Long, ByVal strRole As String)
            Dim objDB As New EAD.DBCom
            Dim strsql As String
            Dim ds As New DataSet

            Dim blnRelief As Integer
            Dim strBody As String
            Dim objCommon As New Common

            If strRole = "FO" Then
                strBody &= "<P>You have an outstanding Invoice (" & strDocNo & ") waiting for approval. <BR>"
            Else
                strBody &= "<P>You have an outstanding Payment (" & strDocNo & ") waiting for approval. <BR>"
            End If

            'strBody = "Dear Approving Officer, <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
            '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
            '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooterENT & Common.EmailCompGen
            'End If
            strsql = "SELECT RAM_USER_ID FROM RELIEF_ASSIGNMENT_MSTR "
            strsql &= "WHERE RAM_USER_ROLE = 'Approving Officer' "
            strsql &= "AND RAM_COY_ID = '" & companyID & "' "
            strsql &= "AND NOW() BETWEEN RAM_START_DATE AND RAM_END_DATE + 1 "
            strsql &= "AND RAM_USER_ID = "
            strsql &= "(SELECT FA_AO FROM FINANCE_APPROVAL "
            strsql &= "WHERE FA_INVOICE_INDEX = " & intIndex & " "
            strsql &= "AND FA_SEQ = FA_AO_ACTION + 1 "
            strsql &= "ORDER BY FA_SEQ LIMIT 0,1)"

            If objDB.Exist(strsql) > 0 Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            strsql = "SELECT FA_AO, ISNULL(FA_A_AO, '') AS FA_A_AO, B.UM_EMAIL AS AO_EMAIL, ISNULL(C.UM_EMAIL, '') AS AAO_EMAIL, "
            strsql &= "B.UM_USER_NAME AS AO_NAME, ISNULL(C.UM_USER_NAME, '') AS AAO_NAME "
            strsql &= "FROM FINANCE_APPROVAL "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = FA_AO AND B.UM_DELETED <> 'Y' AND B.UM_COY_ID = '" & companyID & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = FA_A_AO AND C.UM_DELETED <> 'Y' AND C.UM_COY_ID = '" & companyID & "' "
            strsql &= "WHERE FA_INVOICE_INDEX = " & intIndex & " AND FA_SEQ = FA_AO_ACTION + 1 "
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objMail As New AppMail
                Dim strRoleDesc As String

                'Michelle (CR0016) - To get the correct role description
                If strRole = "FO" Then
                    strRoleDesc = " (Approving Officer)"
                Else
                    strRoleDesc = " (Finance Manager)"
                End If

                If blnRelief Then
                    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        ' objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & strRoleDesc & ", <BR>" & strBody
                    Else
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        ' objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (Approving Officer), <BR>" & strBody
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & strRoleDesc & ", <BR>" & strBody
                    End If
                Else
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    ' objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & strRoleDesc & ", <BR>" & strBody
                End If

                If strRole = "FO" Then
                    objMail.Subject = "Agora : Invoice Approval"
                Else
                    objMail.Subject = "Agora : Payment Approval"
                End If

                objMail.SendMail()
            End If
            objCommon = Nothing
            Return "success"
        End Function

        Public Function HighestFMApprLevel(ByVal pInvIndex As Integer)
            Dim objDb1 As New EAD.DBCom
            Dim strSql As String

            Dim strUserID As String = HttpContext.Current.Session("UserId")

            'strSql = "SELECT TOP 1 1 FROM FINANCE_APPROVAL AS FA1 WHERE  FA1.FA_AGA_TYPE = 'FM' "
            'strSql &= "AND FA1.FA_SEQ = "
            'strSql &= "(SELECT MAX(FA2.FA_SEQ) FROM FINANCE_APPROVAL AS FA2 WHERE FA2.FA_AGA_TYPE = 'FM' "
            'strSql &= "AND FA2.FA_INVOICE_INDEX = FA1.FA_INVOICE_INDEX "
            'strSql &= " AND FA1.FA_AO = '" & strUserID & "' OR FA1.FA_A_AO = '" & strUserID & "')"
            'strSql &= " AND FA1.FA_INVOICE_INDEX = " & pInvIndex
            strSql = "SELECT * FROM FINANCE_APPROVAL AS FA1 WHERE  FA1.FA_AGA_TYPE = 'FM' "
            strSql &= "AND FA1.FA_SEQ = "
            strSql &= "(SELECT MAX(FA2.FA_SEQ) FROM FINANCE_APPROVAL AS FA2 WHERE FA2.FA_AGA_TYPE = 'FM' "
            strSql &= "AND FA2.FA_INVOICE_INDEX = FA1.FA_INVOICE_INDEX "
            strSql &= " AND FA1.FA_AO = '" & strUserID & "' OR FA1.FA_A_AO = '" & strUserID & "')"
            strSql &= " AND FA1.FA_INVOICE_INDEX = " & pInvIndex & " LIMIT 1"

            If objDb1.Exist(strSql) = 1 Then
                Return True
            Else
                Return False
            End If

        End Function

        Function getFinanceApprFlow(ByVal strCoyId As String, ByVal intInvoiceNo As String, ByVal strSupCompanyId As String) As String
            Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim ds As DataSet

            strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME,UMB.UM_USER_NAME AS AAO_NAME," _
            & "UMA.UM_INVOICE_APP_LIMIT AS AO_LIMIT,UMB.UM_INVOICE_APP_LIMIT AS AAO_LIMIT FROM " _
            & "FINANCE_APPROVAL FA LEFT OUTER JOIN USER_MSTR UMA ON " _
            & "FA.FA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & strCoyId & "' LEFT OUTER JOIN USER_MSTR UMB ON " _
            & "FA.FA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID='" & strCoyId & "', INVOICE_MSTR " _
            & "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND IM_INVOICE_NO = '" & intInvoiceNo _
            & "' AND IM_S_COY_ID = '" & strSupCompanyId & "' " _
            & " ORDER BY FA.FA_SEQ"

            ds = objDb.FillDs(strSql)
            Return strSql
        End Function

        Function getFinanceApprRemarks(ByVal intInvoiceIndex As Integer) As String
            Dim objDb As New EAD.DBCom
            Dim strSql As String
            Dim ds As DataSet

            strSql = "SELECT FA_SEQ, FA_AO_ACTION, FA_AO_REMARK " &
                " FROM FINANCE_APPROVAL " &
                " WHERE FA_INVOICE_INDEX = " & intInvoiceIndex &
                " AND FA_SEQ - 1 <= FA_AO_ACTION"

            ds = objDb.FillDs(strSql)
            Return strSql
        End Function

        Function updateHLISBCurrency(ByVal invoiceNo As String, ByVal lineNo As Integer, ByVal SCoyId As Integer)

            Dim strsql As String
            Dim objDb As New EAD.DBCom
            Dim strAryQuery(0) As String
            Dim i As Integer
            strsql = "UPDATE INVOICE_DETAILS SET ID_DR_CURRENCY = 'MYR', ID_DR_EXCHANGE_RATE = 1 "
            strsql &= "WHERE ID_INVOICE_NO = '" & Common.Parse(invoiceNo) & "' "
            strsql &= "AND ID_S_COY_ID = '" & Common.Parse(SCoyId) & "' and ID_invoice_line = " & lineNo
            Common.Insert2Ary(strAryQuery, strsql)
            objDb.BatchExecute(strAryQuery)

        End Function

        Function chkInvContract(ByVal intPOIndex As Long) As Boolean
            Dim strSql, strPONo As String

            strPONo = objDb.GetVal("SELECT POM_PO_NO FROM PO_MSTR WHERE POM_PO_INDEX = " & intPOIndex)
            strSql = "SELECT '*' FROM PO_DETAILS " &
                    "WHERE POD_SOURCE = 'CP' " &
                    "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POD_PO_NO = '" & Common.Parse(strPONo) & "'"

            If objDb.Exist(strSql) > 0 Then
                chkInvContract = True
            Else
                chkInvContract = False
            End If

        End Function

        Sub updateInvTaxCode(ByVal intInvIndex As Long, ByRef pQuery() As String, Optional ByVal blnDefault As Boolean = True)
            Dim strSql, strGSTRegNo, strTaxCode, strCCGROUP_INDEX As String
            Dim intPOIndex As Long
            Dim i As Integer
            Dim dsInv As New DataSet
            'Dim objGst As New GST

            If blnDefault = True Then
                strSql = "SELECT * FROM INVOICE_MSTR " &
                        "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " &
                        "INNER JOIN PO_MSTR ON POM_PO_INDEX = IM_PO_INDEX " &
                        "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID AND ID_PO_LINE = POD_PO_LINE " &
                        "WHERE IM_INVOICE_INDEX = " & intInvIndex
                dsInv = objDb.FillDs(strSql)

                If dsInv.Tables(0).Rows.Count > 0 Then
                    'strGSTRegNo = objGst.chkGST(dsInv.Tables(0).Rows(0)("IM_S_COY_ID"))

                    If Common.parseNull(dsInv.Tables(0).Rows(0)("IM_GST_INVOICE")) = "Y" Then
                        For i = 0 To dsInv.Tables(0).Rows.Count - 1
                            If Common.parseNull(dsInv.Tables(0).Rows(i)("ID_GST_INPUT_TAX_CODE")) = "" Then
                                'strSql = "SELECT PRD_CD_GROUP_INDEX FROM PR_MSTR " & _
                                '                    "INNER JOIN PR_DETAILS ON PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO " & _
                                '                    "WHERE PRD_CONVERT_TO_DOC = (SELECT POM_PO_NO FROM PO_MSTR WHERE POM_PO_INDEX = '" & dsInv.Tables(0).Rows(i)("IM_PO_INDEX") & "') "
                                strSql = "SELECT PRD_CD_GROUP_INDEX FROM PR_MSTR " &
                                        "INNER JOIN PR_DETAILS ON PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO " &
                                        "WHERE PRD_CONVERT_TO_DOC = '" & Common.Parse(dsInv.Tables(0).Rows(i)("POD_PO_NO")) & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                                        "AND PRD_PR_LINE = " & dsInv.Tables(0).Rows(i)("POD_PR_LINE")

                                strCCGROUP_INDEX = objDb.GetVal(strSql)

                                If strCCGROUP_INDEX <> "" Then
                                    strSql = "SELECT IFNULL(CDI_GST_TAX_CODE,'') FROM CONTRACT_DIST_MSTR " &
                                        "INNER JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
                                        "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " &
                                        "WHERE CDM_GROUP_INDEX = " & strCCGROUP_INDEX & " " &
                                        "AND CDI_PRODUCT_CODE = '" & dsInv.Tables(0).Rows(i)("POD_PRODUCT_CODE") & "'"
                                    strTaxCode = objDb.GetVal(strSql)
                                Else
                                    strSql = "SELECT IFNULL(CDI_GST_TAX_CODE,'') FROM CONTRACT_DIST_MSTR " &
                                        "INNER JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
                                        "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " &
                                        "WHERE CDM_GROUP_INDEX = " & dsInv.Tables(0).Rows(i)("POD_CD_GROUP_INDEX") & " " &
                                        "AND CDI_PRODUCT_CODE = '" & dsInv.Tables(0).Rows(i)("POD_PRODUCT_CODE") & "'"
                                    strTaxCode = objDb.GetVal(strSql)
                                End If

                                If strTaxCode <> "" Then
                                    strSql = "UPDATE INVOICE_DETAILS SET ID_GST_INPUT_TAX_CODE = '" & Common.Parse(strTaxCode) & "' " &
                                            "WHERE ID_INVOICE_NO = '" & dsInv.Tables(0).Rows(i)("ID_INVOICE_NO") & "' " &
                                            "AND ID_S_COY_ID = '" & dsInv.Tables(0).Rows(i)("ID_S_COY_ID") & "' " &
                                            "AND ID_INVOICE_LINE = '" & dsInv.Tables(0).Rows(i)("ID_INVOICE_LINE") & "'"
                                    Common.Insert2Ary(pQuery, strSql)
                                End If
                            End If
                        Next
                    Else
                        strSql = "UPDATE INVOICE_DETAILS SET ID_GST_INPUT_TAX_CODE = 'NR' " &
                                "WHERE ID_INVOICE_NO = '" & dsInv.Tables(0).Rows(i)("ID_INVOICE_NO") & "' " &
                                "AND ID_S_COY_ID = '" & dsInv.Tables(0).Rows(i)("ID_S_COY_ID") & "' "
                        Common.Insert2Ary(pQuery, strSql)
                    End If
                End If
            End If

        End Sub

        Function DisplayDefaultTaxCode(ByVal intInvIndex As Integer, ByVal intPoLine As Integer) As String
            Dim strSql, strGSTRegNo, strTaxCode As String
            Dim ds As New DataSet

            strTaxCode = ""
            'strSql = "SELECT POD_CD_GROUP_INDEX, POD_PRODUCT_CODE FROM INVOICE_MSTR " & _
            '        "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " & _
            '        "INNER JOIN PO_MSTR ON POM_PO_INDEX = IM_PO_INDEX " & _
            '        "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID AND ID_PO_LINE = POD_PO_LINE " & _
            '        "WHERE IM_INVOICE_INDEX = " & intInvIndex & " AND ID_PO_LINE = " & intPoLine
            strSql = "SELECT CASE WHEN POD_CD_GROUP_INDEX <> '' THEN POD_CD_GROUP_INDEX ELSE PRD_CD_GROUP_INDEX END AS CD_GROUP_INDEX, POD_PRODUCT_CODE " &
                    "FROM INVOICE_MSTR " &
                    "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " &
                    "INNER JOIN PO_MSTR ON POM_PO_INDEX = IM_PO_INDEX " &
                    "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID AND ID_PO_LINE = POD_PO_LINE " &
                    "LEFT JOIN PR_MSTR ON POD_PR_INDEX = PRM_PR_INDEX " &
                    "LEFT JOIN PR_DETAILS ON PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND POD_PR_LINE = PRD_PR_LINE " &
                    "WHERE IM_INVOICE_INDEX = " & intInvIndex & " AND ID_PO_LINE = " & intPoLine
            ds = objDb.FillDs(strSql)

            If ds.Tables(0).Rows.Count > 0 Then
                strSql = "SELECT IFNULL(CDI_GST_TAX_CODE,'') FROM CONTRACT_DIST_MSTR " &
                        "INNER JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
                        "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " &
                        "WHERE CDM_GROUP_INDEX = " & ds.Tables(0).Rows(0)("CD_GROUP_INDEX") & " " &
                        "AND CDI_PRODUCT_CODE = '" & ds.Tables(0).Rows(0)("POD_PRODUCT_CODE") & "'"
                strTaxCode = objDb.GetVal(strSql)
            End If

            Return strTaxCode
        End Function

        Function chkGstInvoice(ByVal intInvIndex As Integer) As Boolean
            Dim strSql As String
            strSql = "SELECT IM_GST_INVOICE FROM INVOICE_MSTR WHERE IM_INVOICE_INDEX = " & intInvIndex

            If (objDb.GetVal(strSql) = "Y") Then
                chkGstInvoice = True
            Else
                chkGstInvoice = False
            End If
        End Function

        Function chkInvTaxCode(ByVal intInvIndex As Integer, Optional ByVal blnContract As Boolean = False) As Boolean
            Dim strSql, strGSTRegNo As String
            Dim ds As DataSet
            Dim objGst As New GST

            If blnContract = False Then
                strSql = "SELECT '*' FROM INVOICE_MSTR " &
                    "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                    "WHERE IM_INVOICE_INDEX = " & intInvIndex & " AND (ID_GST_INPUT_TAX_CODE = '' OR ID_GST_INPUT_TAX_CODE IS NULL)"

                If objDb.Exist(strSql) > 0 Then
                    chkInvTaxCode = True
                Else
                    chkInvTaxCode = False
                End If
            Else
                strSql = "SELECT '*' FROM INVOICE_MSTR " &
                   "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                   "WHERE IM_INVOICE_INDEX = " & intInvIndex & " AND (ID_GST_INPUT_TAX_CODE = '' OR ID_GST_INPUT_TAX_CODE IS NULL)"

                If objDb.Exist(strSql) > 0 Then
                    strSql = "SELECT * FROM INVOICE_MSTR " &
                        "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " &
                        "INNER JOIN PO_MSTR ON POM_PO_INDEX = IM_PO_INDEX " &
                        "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID AND ID_PO_LINE = POD_PO_LINE " &
                        "INNER JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = POD_CD_GROUP_INDEX " &
                        "INNER JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
                        "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND CDI_PRODUCT_CODE = POD_PRODUCT_CODE " &
                        "WHERE IM_INVOICE_INDEX = " & intInvIndex & " AND (CDI_GST_TAX_CODE = '' OR CDI_GST_TAX_CODE IS NULL)"

                    ds = objDb.FillDs(strSql)

                    If ds.Tables(0).Rows.Count > 0 Then
                        strGSTRegNo = objGst.chkGST(ds.Tables(0).Rows(0)("IM_S_COY_ID"))
                        If strGSTRegNo <> "" Then
                            chkInvTaxCode = True
                        Else
                            chkInvTaxCode = False
                        End If
                    Else
                        chkInvTaxCode = False
                    End If
                Else
                    chkInvTaxCode = False
                End If


            End If

        End Function
    End Class
End Namespace

