Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web.UI.WebControls
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraNEO
    Public Class PurchaseReq2
        'Inherits AppBaseClass
        Dim objDb As New EAD.DBCom

        Function isApproved(ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strUser As String, Optional ByVal strFor As String = "") As Boolean
            Dim strSql, strAO, strAAO, strActiveAO As String
            Dim intLastSeq As Integer
            intLastSeq = intCurrentSeq '- 1
            If strFor <> "" Then
                strSql = "SELECT PRA_AO,PRA_A_AO,PRA_ACTIVE_AO FROM PR_APPROVAL WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intLastSeq & " AND PRA_FOR = 'PO' "
            Else
                strSql = "SELECT PRA_AO,PRA_A_AO,PRA_ACTIVE_AO FROM PR_APPROVAL WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intLastSeq & " AND PRA_FOR <> 'PO' "
            End If
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRA_AO"))
                strAAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRA_A_AO"))
                strActiveAO = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRA_ACTIVE_AO"))
            End If

            If strActiveAO <> "" Then
                '//already approved
                isApproved = True
            Else
                '//not approved yet
                '//check whether this is a valid user
                If UCase(strAO) = UCase(strUser) Or UCase(strAAO) = UCase(strUser) Then
                    '//valid user
                    isApproved = False
                Else
                    '//not a valid user
                    isApproved = True
                End If
                'isApproved = False
            End If
        End Function
        'Function checkPR_Ready(ByVal strPRNO As String, ByVal strAction As String, ByRef strMsg1() As String) As Boolean
        '    Dim strSql, strCoyId, strMsg As String
        '    Dim blnReady As Boolean = True
        '    strCoyId = HttpContext.Current.Session("CompanyId")
        '    'Conversion from PR to PO unsuccessful.

        '    'You have already converted the following PRs to POs:
        '    '//Check current PR status
        '    strSql = "SELECT PRM_PR_STATUS,PRM_PO_INDEX,POM_PO_NO,PRM_PR_NO FROM PR_MSTR A LEFT OUTER JOIN PO_MSTR B " _
        '    & "ON A.PRM_PO_INDEX=POM_PO_INDEX WHERE PRM_COY_ID= '" & strCoyId & "' AND PRM_PR_NO IN(" &
        '    strPRNO & ")"

        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        Select Case LCase(strAction)
        '            Case "approve"
        '            Case "reject"
        '            Case "hold"
        '            Case "cancel"
        '            Case "createpo", "consolidate" '//if any of PR has been converted to PO,cannot consolidate
        '                'You have already converted PR No. PR0209118 to PO No
        '                If tDS.Tables(0).Rows(j).Item("PRM_PR_STATUS") = PRStatus.ConvertedToPO And Not IsDBNull(tDS.Tables(0).Rows(j).Item("POM_PO_NO")) Then
        '                    strMsg = "You have already convert PR No. " & tDS.Tables(0).Rows(j).Item("PRM_PR_NO") & " to PO No. " & tDS.Tables(0).Rows(j).Item("POM_PO_NO")
        '                    Common.Insert2Ary(strMsg1, strMsg)
        '                    blnReady = False
        '                End If
        '        End Select
        '        Return blnReady
        '    Next
        'End Function

        'Function getPRConsolidation(ByVal strUserID As String) As DataSet
        '    Dim strSql, strCoyID As String
        '    Dim ds As DataSet
        '    ' DS.Tables(0).Select(
        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strSql = "SELECT PRM_PR_Index,PRM_PR_NO,PRM_EXTERNAL_REMARK,PRM_CURRENCY_CODE,PRM_PR_COST,PRM_S_COY_ID,PRM_S_COY_NAME, " _
        '    & "PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE,PRM_PAYMENT_TERM,PRM_REQ_NAME," _
        '    & "PRM_SHIP_VIA,PRM_B_ADDR_CODE,PRM_CREATED_DATE,PRM_PR_Date FROM PR_MSTR WHERE PRM_PR_INDEX IN( " _
        '    & "SELECT DISTINCT PRA_PR_INDEX FROM PR_APPROVAL A WHERE PRA_AO_ACTION= " _
        '    & "(SELECT MAX(PRA_SEQ) FROM PR_APPROVAL B WHERE A.PRA_PR_INDEX=B.PRA_PR_INDEX)) " _
        '    & " AND PRM_COY_ID='" & strCoyID & "' AND PRM_CONSOLIDATOR='" & strUserID & "' AND PRM_PR_STATUS=" & PRStatus.Approved
        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function
        'Function getPRVendor(ByVal strPRNo As String) As String ' Michelle 
        '    Dim strSql, strVendor As String


        '    strSql = "SELECT PRM_S_COY_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strPRNo & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        strVendor = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRM_S_COY_ID"))
        '    End If

        '    Return strVendor
        'End Function
        Function getPO_CR_LineNo(ByVal strBCoyID As String, ByVal strPONo As String, ByVal strPOLine As String) As String ' Michelle (20/7/2007) - To get the re-sequenced PO Line no that has been cancelled
            Dim strSql, strPO_CR_LineNo As String
            strSql = "SELECT COUNT(*) as LINE_NO FROM PO_DETAILS WHERE POD_PO_NO = '" & strPONo & "' AND POD_PO_LINE <= '" & strPOLine & "' AND POD_COY_ID = '" & strBCoyID & "'"
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strPO_CR_LineNo = Common.parseNull(tDS.Tables(0).Rows(0).Item("LINE_NO"))
            End If
            Return strPO_CR_LineNo
        End Function

        'Function getPR(ByVal strPRNo As String, ByVal strPRIndex As String) As DataSet
        '    Dim strSql, strSqlPRM, strSqlPRD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
        '    Dim strPRField As String
        '    Dim ds, ds1 As DataSet
        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    strPRField = "PM.PRM_PR_NO, PM.PRM_REQ_NAME,PM.PRM_REQ_PHONE, PM.PRM_PR_DATE, PM.PRM_B_ADDR_LINE1, PM.PRM_B_ADDR_LINE2, PM.PRM_B_ADDR_LINE3, " _
        '    & "PM.PRM_B_POSTCODE, PM.PRM_B_CITY, PM.PRM_INTERNAL_REMARK, PM.PRM_S_COY_ID, PM.PRM_S_COY_NAME, PM.PRM_CURRENCY_CODE,PM.PRM_BUYER_ID, " _
        '    & "PM.PRM_S_ADDR_LINE1,PM.PRM_S_ADDR_LINE2,PM.PRM_S_ADDR_LINE3,PM.PRM_S_POSTCODE,PM.PRM_S_CITY,PM.PRM_S_STATE,PM.PRM_S_COUNTRY,PM.PRM_S_PHONE,PM.PRM_S_FAX,PM.PRM_S_EMAIL,PM.PRM_SHIP_VIA,PM.PRM_FREIGHT_TERMS,PM.PRM_S_ATTN,PM.PRM_CREATED_DATE, PM.PRM_SUBMIT_DATE, " _
        '    & "PM.PRM_EXCHANGE_RATE, PM.PRM_External_Remark, PM.PRM_CONSOLIDATOR, PM.PRM_GST, PM.PRM_PR_Index, PM.PRM_PR_Status,PM.PRM_RFQ_INDEX, PM.PRM_PR_Type, PM.PRM_URGENT "

        '    strSqlPRM = "SELECT " & strPRField & ", CM.CM_TAX_CALC_BY," _
        '    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.PRM_B_COUNTRY) AS CT, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.PRM_B_STATE AND CODE_VALUE=PM.PRM_B_COUNTRY) AS STATE, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PT' AND CODE_ABBR=PM.PRM_PAYMENT_TERM) AS PT," _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PM' AND CODE_ABBR=PM.PRM_PAYMENT_TYPE) AS PM," _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='ST' AND CODE_ABBR=PM.PRM_SHIPMENT_TERM) AS ST," _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='SM' AND CODE_ABBR=PM.PRM_SHIPMENT_MODE) AS SC, " _
        '    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyID & "') AS NAME " _
        '    & "FROM PR_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.PRM_S_COY_ID=CM.CM_COY_ID " _
        '    & "WHERE " _
        '    & "PRM_COY_ID='" & strCoyID & "' AND PRM_PR_No='" & strPRNo & "'"

        '    'strSqlPRD = "SELECT PD.*, " _
        '    '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
        '    '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE " _
        '    '& "FROM PR_DETAILS PD " _
        '    '& "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"

        '    'Jules 2018.05.07 - PAMB Scrum 2 - Added Gift and Analysis Codes.
        '    'Yik Foong
        '    '15 Oct 2009
        '    'Left join to return all rows from first table
        '    strSqlPRD = "SELECT PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE, PD.PRD_ORDERED_QTY * PD.PRD_UNIT_COST AS AMOUNT,CM.CM_COY_NAME, " _
        '    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyID & "') AS NAME, CONCAT(PD.PRD_ASSET_GROUP, CONCAT(' ', PD.PRD_ASSET_NO)) AS ASSET_CODE, PD.PRD_GST_RATE AS GSTRATE, " _
        '    & "CASE WHEN PRD_GST_RATE = 'N/A' THEN PRD_GST_RATE ELSE " _
        '    & "IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, CASE WHEN IFNULL(PRD_GIFT,'N') = 'N' THEN 'No' ELSE 'Yes' END AS GIFT, "

        '    'Jules 2018.10.26 - Swap Analysis Code with Analysis Code Desc.
        '    '& "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PD.PRD_FUND_TYPE) AS FUNDTYPE, " _
        '    '& "CASE WHEN PRD_PERSON_CODE = 'N/A' THEN PRD_PERSON_CODE ELSE (SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PD.PRD_PERSON_CODE) END PERSONCODE, " _
        '    '& "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PD.PRD_PROJECT_CODE) AS PROJECTCODE " _
        '    strSqlPRD &= "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PD.PRD_FUND_TYPE) AS FUNDTYPE, " _
        '    & "CASE WHEN PRD_PERSON_CODE = 'N/A' THEN PRD_PERSON_CODE ELSE (SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PD.PRD_PERSON_CODE) END PERSONCODE, " _
        '    & "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PD.PRD_PROJECT_CODE) AS PROJECTCODE "
        '    'End modification.

        '    strSqlPRD &= "FROM PR_DETAILS PD " _
        '    & "INNER JOIN PR_MSTR PM ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
        '    & "LEFT JOIN COMPANY_B_GL_CODE " _
        '    & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.PRD_B_GL_CODE AND CBG_B_COY_ID = PRM_COY_ID " _
        '    & "LEFT JOIN CONTRACT_DIST_MSTR CDM ON PRD_CD_GROUP_INDEX = CDM_GROUP_INDEX " _
        '    & "LEFT JOIN COMPANY_MSTR CM ON CDM_S_COY_ID = CM_COY_ID " _
        '    & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = PRD_GST_RATE " _
        '    & "LEFT JOIN TAX ON TAX_CODE = PRD_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
        '    & "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"



        '    'strSqlCustomF = "SELECT * FROM PR_CUSTOM_FIELD_MSTR A,PR_CUSTOM_FIELD_DETAILS B " _
        '    '& "WHERE B.PCD_PR_INDEX=A.PCM_PR_INDEX AND PCD_PR_INDEX=" & strPOIndex

        '    strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_PR_INDEX=" & strPRIndex & " AND PCD_TYPE = 'PR' "
        '    strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & strPRIndex & " AND PCM_TYPE = 'PR' "
        '    strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"


        '    strSql = strSqlPRM & ";" & strSqlPRD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
        '    ds = objDb.FillDs(strSql)
        '    'ds1 = getApprFlow(strPOIndex)

        '    ds.Tables(0).TableName = "PR_MSTR"
        '    ds.Tables(1).TableName = "PR_DETAILS"
        '    ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
        '    ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
        '    ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
        '    'Dim dt As New DataTable()
        '    'dt = ds1.Tables(0)
        '    'ds.Tables.Add(ds1.Tables(0))
        '    'ds.Tables(4).TableName = "PR_APPROVAL"
        '    Return ds
        'End Function

        'Function getPRInfo(ByVal strPRNo As String, ByVal strPRIndex As String) As DataSet
        '    Dim strSql, strSqlPRM, strSqlPRD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
        '    Dim strPRField As String
        '    Dim ds, ds1 As DataSet
        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    strPRField = "PM.PRM_PR_NO, PM.PRM_REQ_NAME,PM.PRM_REQ_PHONE, PM.PRM_PR_DATE, PM.PRM_SUBMIT_DATE, PM.PRM_B_ADDR_LINE1, PM.PRM_B_ADDR_LINE2, PM.PRM_B_ADDR_LINE3, " _
        '    & "PM.PRM_B_POSTCODE, PM.PRM_B_CITY, PM.PRM_INTERNAL_REMARK, PM.PRM_S_COY_ID, PM.PRM_S_COY_NAME, PM.PRM_CURRENCY_CODE,PM.PRM_BUYER_ID, " _
        '    & "PM.PRM_S_ADDR_LINE1,PM.PRM_S_ADDR_LINE2,PM.PRM_S_ADDR_LINE3,PM.PRM_S_POSTCODE,PM.PRM_S_CITY,PM.PRM_S_STATE,PM.PRM_S_COUNTRY,PM.PRM_S_PHONE,PM.PRM_S_FAX,PM.PRM_S_EMAIL,PM.PRM_SHIP_VIA,PM.PRM_FREIGHT_TERMS,PM.PRM_S_ATTN,PM.PRM_CREATED_DATE, " _
        '    & "PM.PRM_EXCHANGE_RATE, PM.PRM_External_Remark, PM.PRM_CONSOLIDATOR, PM.PRM_GST, PM.PRM_PR_Index, PM.PRM_PR_Status,PM.PRM_RFQ_INDEX,PM.PRM_PR_TYPE, PM.PRM_URGENT, PM.PRM_COY_ID, PM.PRM_STATUS_CHANGED_BY "

        '    strSqlPRM = "SELECT " & strPRField & ", CM.CM_TAX_CALC_BY," _
        '    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.PRM_B_COUNTRY) AS CT, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.PRM_B_STATE AND CODE_VALUE=PM.PRM_B_COUNTRY) AS STATE, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PT' AND CODE_ABBR=PM.PRM_PAYMENT_TERM) AS PT," _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PM' AND CODE_ABBR=PM.PRM_PAYMENT_TYPE) AS PM," _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='ST' AND CODE_ABBR=PM.PRM_SHIPMENT_TERM) AS ST," _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='SM' AND CODE_ABBR=PM.PRM_SHIPMENT_MODE) AS SC, " _
        '    & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(strCoyID) & "') AS NAME " _
        '    & "FROM PR_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.PRM_S_COY_ID=CM.CM_COY_ID " _
        '    & "WHERE " _
        '    & "PRM_COY_ID='" & strCoyID & "' AND PRM_PR_No='" & strPRNo & "'"

        '    'strSqlPRD = "SELECT PD.*, " _
        '    '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
        '    '& "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE " _
        '    '& "FROM PR_DETAILS PD " _
        '    '& "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"

        '    'Jules 2018.05.07 - PAMB Scrum 2 - Added Gift and Analysis Codes.
        '    'Yik Foong
        '    '15 Oct 2009
        '    'Left join to return all rows from first table
        '    strSqlPRD = "SELECT PM.*, PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.PRD_D_COUNTRY) AS CT, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.PRD_D_STATE AND CODE_VALUE=PD.PRD_D_COUNTRY) AS STATE, PD.PRD_ORDERED_QTY * PD.PRD_UNIT_COST AS AMOUNT, CONCAT(PD.PRD_ASSET_GROUP, CONCAT(' ', IFNULL(PD.PRD_ASSET_NO,''))) AS ASSET_CODE, " _
        '    & "CASE WHEN PRD_GST_RATE = 'N/A' THEN PRD_GST_RATE ELSE " _
        '    & "IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE " _
        '    & ", CASE WHEN IFNULL(PRD_GIFT,'N') = 'N' THEN 'No' ELSE 'Yes' END AS GIFT, " _
        '    & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE =PD.PRD_FUND_TYPE) AS FUNDTYPE, " _
        '    & "CASE WHEN PRD_PERSON_CODE = 'N/A' THEN PRD_PERSON_CODE ELSE (SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE =PD.PRD_PERSON_CODE) END PERSONCODE, " _
        '    & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE =PD.PRD_PROJECT_CODE) AS PROJECTCODE " _
        '    & "FROM PR_DETAILS PD " _
        '    & "INNER JOIN PR_MSTR PM ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
        '    & "LEFT JOIN COMPANY_B_GL_CODE " _
        '    & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.PRD_B_GL_CODE AND CBG_B_COY_ID = PRM_COY_ID " _
        '    & "LEFT JOIN COMPANY_MSTR ON PRM_COY_ID = CM_COY_ID " _
        '    & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = PRD_GST_RATE " _
        '    & "LEFT JOIN TAX ON TAX_CODE = PRD_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
        '    & "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strPRNo & "' ORDER BY PRD_PR_LINE"



        '    'strSqlCustomF = "SELECT * FROM PR_CUSTOM_FIELD_MSTR A,PR_CUSTOM_FIELD_DETAILS B " _
        '    '& "WHERE B.PCD_PR_INDEX=A.PCM_PR_INDEX AND PCD_PR_INDEX=" & strPOIndex

        '    strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_PR_INDEX=" & strPRIndex & " AND PCD_TYPE='PR'"
        '    strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & strPRIndex & " AND PCM_TYPE='PR'"
        '    strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"


        '    strSql = strSqlPRM & ";" & strSqlPRD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
        '    ds = objDb.FillDs(strSql)
        '    'ds1 = getApprFlow(strPOIndex)

        '    ds.Tables(0).TableName = "PR_MSTR"
        '    ds.Tables(1).TableName = "PR_DETAILS"
        '    ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
        '    ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
        '    ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
        '    'Dim dt As New DataTable()
        '    'dt = ds1.Tables(0)
        '    'ds.Tables.Add(ds1.Tables(0))
        '    'ds.Tables(4).TableName = "PR_APPROVAL"
        '    Return ds
        'End Function

        '//caller= View Requisition by buyer or buyer admin
        Function SearchPRList(ByVal userID As String, ByVal companyID As String, ByVal strPRNo As String, ByVal strItemCode As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strRole As String, Optional ByVal strBuyer As String = "", Optional ByVal strStatus As String = "", Optional ByVal strPRType As String = "", Optional ByVal strStatus2 As String = "") As String
            'Modified by Joon on 24th June 2011
            Dim strSql, strCondition As String
            Dim strUser As String = userID
            Dim strCoyId As String = companyID
            Dim ds As DataSet

            'strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, " _
            '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PM.PRM_S_COY_NAME,PM.PRM_REQ_NAME,PRM_PR_STATUS," _
            '& "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
            '& "(SELECT SUM(PRD_UNIT_COST*PRD_ORDERED_QTY) FROM PR_DETAILS PD WHERE " _
            '& "PD.PRD_PR_NO=PM.PRM_PR_NO AND PD.PRD_COY_ID=PM.PRM_COY_ID) AS PR_AMT,PM.PRM_RFQ_INDEX " _
            '& ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO,PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME " _
            '& "FROM PR_MSTR PM LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
            '& "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _           
            '& "AND UM.UM_COY_ID='" & strCoyId & "' WHERE PM.PRM_COY_ID='" & strCoyId & "'"

            'Michelle (21/9/2010) - To change the syntax to cater for MSSQL & MYSQL
            'strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, " _
            '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PRM_S_COY_NAME = CASE isnull(PM.PRM_S_COY_NAME,'NULL')  " _
            '& "WHEN '' THEN 'Multiple Vendors' WHEN 'NULL' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END ,PM.PRM_REQ_NAME,PRM_PR_STATUS," _

            'strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, " _
            '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, (CASE WHEN PRM_S_COY_NAME IS NULL THEN 'MULTIPLE VENDORS'  " _
            '& "WHEN PM.PRM_S_COY_NAME = '' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END) as PRM_S_COY_NAME,PM.PRM_REQ_NAME,PRM_PR_STATUS," _
            '& "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
            '& " PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, UM1.UM_USER_NAME " _
            '& ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO,PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME " _
            '& "FROM PR_MSTR PM LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
            '& "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID=PM.PRM_BUYER_ID " _
            '& "AND UM1.UM_COY_ID='" & strCoyId & "' " _
            '& "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
            '& "AND UM.UM_COY_ID='" & strCoyId & "' WHERE PM.PRM_COY_ID='" & strCoyId & "'"


            strSql = " SELECT * FROM (SELECT DISTINCT PRM_PR_Index, PRM_PR_No, PRM_S_Coy_ID, DATE_FORMAT(PRM_PR_Date, ""%d/%m/%Y %H:%i"") AS PRM_PR_Date, DATE_FORMAT(PRM_SUBMIT_DATE, ""%d/%m/%Y %H:%i"") AS PRM_SUBMIT_DATE, " _
            & " PRM_CURRENCY_CODE, PRM_S_COY_NAME, PRM_REQ_NAME, MAX(PRM_PR_STATUS) AS PRM_PR_STATUS, STATUS_DESC, " _
            & " MAX(PR_AMT) AS PR_AMT, PRM_RFQ_INDEX, PRM_PO_INDEX, PRM_STATUS_CHANGED_BY, (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(strCoyId) & "') AS NAME, DATE_FORMAT(PRM_CREATED_DATE, ""%d/%m/%Y %H:%i"") AS CreatedDate, PRM_PR_TYPE, PO_NO, PRM_URGENT FROM ( " _
            & "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PRM_SUBMIT_DATE, " _
            & "PM.PRM_CURRENCY_CODE,PRM_S_COY_NAME,PM.PRM_REQ_NAME, " _
            & " (CASE WHEN PR_DETAILS.PRD_CONVERT_TO_IND = 'PO' OR PR_DETAILS.PRD_CONVERT_TO_IND IS NULL THEN PM.PRM_PR_STATUS " _
            & " ELSE (CASE WHEN " _
            & " (SELECT PRM_PR_Index FROM (SELECT PRM_PR_Index, prd_convert_to_doc FROM (SELECT PRM_PR_Index, prd_convert_to_doc " _
            & " FROM pr_mstr INNER JOIN pr_details ON PRD_PR_NO = PRM_PR_NO AND PRM_COY_ID = PRD_COY_ID AND prd_coy_id  = '" & Common.Parse(strCoyId) & "' " _
            & " WHERE PRM_PR_STATUS = '4' AND PRD_CONVERT_TO_IND = 'RFQ' GROUP BY PRM_PR_Index, prd_convert_to_doc) AS tt) AS aa " _
            & " INNER JOIN rfq_mstr ON rm_rfq_no = prd_convert_to_doc AND rm_coy_id = '" & Common.Parse(strCoyId) & "' " _
            & " AND rm_rfq_id NOT IN (SELECT IFNULL(pom_rfq_index,'') FROM po_mstr) WHERE  prm_pr_index = pm.prm_pr_index " _
            & " GROUP BY PRM_PR_Index) <> '' " _
            & " THEN 99 ELSE (CASE WHEN PRM_PR_STATUS = 4 THEN 99 ELSE PRM_PR_STATUS END) END) END) AS PRM_PR_STATUS, " _
            & " SM.STATUS_DESC," _
            & " PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, " _
            & "PRM_PO_INDEX, PRM_STATUS_CHANGED_BY, " _
            & "PM.PRM_CREATED_DATE AS PRM_CREATED_DATE,PRM_PR_TYPE, " _
            & "(SELECT POM_PO_NO FROM PO_MSTR WHERE POM_PO_INDEX = PRM_PO_INDEX) AS PO_NO, PM.PRM_URGENT AS PRM_URGENT " _
            & "FROM PR_MSTR PM JOIN STATUS_MSTR SM ON SM.STATUS_NO = PM.PRM_PR_STATUS AND SM.STATUS_TYPE='PR'"

            If strItemCode <> "" Then
                strSql = strSql & "LEFT JOIN PR_DETAILS ON PRM_PR_NO=PRD_PR_NO AND PRM_COY_ID=PRD_COY_ID AND PRD_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strItemCode) & "%' "
            Else
                strSql = strSql & "LEFT JOIN PR_DETAILS ON PRM_PR_NO=PRD_PR_NO AND PRM_COY_ID=PRD_COY_ID "
            End If

            strSql = strSql & " WHERE PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "'"

            '//REMARK BY MOO
            '//No need to filter the status because only need display name (not finding active user)
            'AND UM.UM_DELETED='N' AND UM.UM_STATUS='A'" _

            If strRole = "BUYER" Then
                strSql = strSql & " AND PRM_BUYER_ID='" & Common.Parse(strUser) & "'"
            End If

            '//nobody except buyer himself can view his draft PR
            If strRole = "ADMIN" Then
                strSql = strSql & " AND PM.PRM_PR_Status <> " & PRStatus.Draft
            End If

            If strPRNo <> "" Then
                strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
            End If

            If strPRType = "CC" Then
                strSql = strSql & " AND PM.PRM_PR_TYPE = '" & Common.Parse(strPRType) & "'"
            ElseIf strPRType = "NonCont" Then
                strSql = strSql & " AND (PM.PRM_PR_TYPE IS NULL OR PM.PRM_PR_TYPE = '')"
            End If
            'If strItemCode <> "" Then
            '    strSql = strSql & " AND PM.PRM_S_COY_NAME" & Common.ParseSQL(strVendor)
            'End If

            If dteDateFr <> "" Then
                'strSql = strSql & " AND PRM_PR_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
                strSql = strSql & " AND (PRM_CREATED_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & " OR PRM_SUBMIT_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & " OR PRM_PR_Date>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & ")"
            End If

            'If dteDateFr = "" And dteDateTo <> "" Then
            If dteDateTo <> "" Then
                'strSql = strSql & " AND PRM_PR_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
                strSql = strSql & " AND (PRM_CREATED_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & " OR PRM_SUBMIT_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & " OR PRM_PR_Date<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & ")"

            End If

            If strStatus <> "" Then
                strSql = strSql & " AND PM.PRM_PR_Status IN (" & strStatus & ")"
            End If

            strSql = strSql & " ) ZZZZ " _
            & " GROUP BY PRM_PR_Index, PRM_PR_No, PRM_S_Coy_ID, PRM_PR_Date, PRM_SUBMIT_DATE, " _
            & " PRM_CURRENCY_CODE, PRM_S_COY_NAME, PRM_REQ_NAME,  STATUS_DESC, PRM_RFQ_INDEX, " _
            & " PRM_PO_INDEX, PRM_STATUS_CHANGED_BY, NAME, PRM_CREATED_DATE, PRM_PR_TYPE, PO_NO ORDER BY PRM_CREATED_DATE DESC ) XXX "

            If strStatus2 <> "" Then
                strSql = strSql & " WHERE XXX.PRM_PR_STATUS IN (" & strStatus2 & ") "
            End If



            'If strBuyer <> "" Then
            '    strSql = strSql & " AND PM.PRM_REQ_NAME" & Common.ParseSQL(strBuyer)
            'End If

            'ds = objDb.FillDs(strSql)
            Return strSql
        End Function

        Function PRListForConvertPO(ByVal strPRNo As String, ByVal strUser As String, ByVal strCoyId As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strRole As String, Optional ByVal strBuyer As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pComType As String = "") As String
            Dim strSql, strCondition As String
            Dim ds As DataSet
            Dim strType As String

            strSql = "SELECT PM.*, PD.*, PMM.*,PRD_GST as SST_AMMOUNT, " _
            & " (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PMM.PM_LAST_TXN_S_COY_ID) AS PM_LAST_TXN_S_COY_NAME, " _
            & " (SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_COY_ID = PM.PRM_COY_ID AND UM_USER_ID = PM.PRM_BUYER_ID) AS UM_USER_NAME " _
            & " FROM PR_MSTR PM " _
            & " INNER JOIN PR_DETAILS PD ON PM.PRM_PR_NO=PD.PRD_PR_NO AND PM.PRM_COY_ID=PD.PRD_COY_ID " _
            & " INNER JOIN COMMODITY_TYPE CM ON PD.PRD_CT_ID=CM.CT_ID " _
            & " INNER JOIN USER_ASSIGN UA ON PM.PRM_COY_ID=UA.UA_COY_ID AND CM.CT_ROOT_PREFIX=UA.UA_ASSIGN_VALUE  " _
            & " LEFT JOIN PRODUCT_MSTR PMM ON PD.PRD_PRODUCT_CODE=PMM.PM_PRODUCT_CODE AND PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
            strSql = strSql & " WHERE PM.PRM_PR_TYPE <> 'CC' AND PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "'"
            strSql = strSql & " AND PD.PRD_CONVERT_TO_IND IS NULL AND PM.PRM_PR_Status = " & PRStatus.Approved
            strSql = strSql & " AND UA.UA_TYPE = 'CT' AND UA.UA_USER_ID = '" & Common.Parse(strUser) & "' "

            'If strRole = "BUYER" Then
            '    strSql = strSql & " AND PRM_BUYER_ID='" & Common.Parse(strUser) & "'"
            'End If

            ' Nobody except buyer himself can view his draft PR
            'If strRole = "ADMIN" Then
            '    strSql = strSql & " AND PM.PRM_PR_Status <> " & PRStatus.Draft
            'End If

            If strPRNo <> "" Then
                strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
            End If

            'strSql = strSql & " AND PRM_PR_Date >= " & Common.ConvertDate("01/07/2011" & " 00:00:00")
            If dteDateFr <> "" Then
                strSql = strSql & " AND PRM_PR_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
            End If

            If dteDateTo <> "" Then
                strSql = strSql & " AND PRM_PR_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If pComType <> "" Then
                strSql &= "AND PD.PRD_CT_ID =" & pComType & " "
            End If

            If pItemType IsNot Nothing Then
                For i As Integer = 0 To pItemType.Count - 1
                    If strType = "" Then
                        strType = "'" & pItemType(i) & "'"
                    Else
                        strType = strType & "," & "'" & pItemType(i) & "'"
                    End If
                Next
                strType = "(" & strType & ")"
                If strType <> "" Then
                    strSql &= " AND PMM.PM_ITEM_TYPE IN " & strType
                End If
            End If

            ds = objDb.FillDs(strSql)
            Return strSql
        End Function

        'Function PRListForPORFQ(ByVal strPRNo As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strRole As String, Optional ByVal strBuyer As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pDoc As String = "") As DataSet
        '    Dim strSql, strCondition As String
        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    Dim ds As DataSet
        '    Dim strType As String

        '    strSql = "SELECT PM.*, PD.*, PMM.*, " _
        '    & " (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PMM.PM_LAST_TXN_S_COY_ID) AS PM_LAST_TXN_S_COY_NAME " _
        '    & " FROM PR_MSTR PM " _
        '    & " INNER JOIN PR_DETAILS PD ON PM.PRM_PR_NO=PD.PRD_PR_NO AND PM.PRM_COY_ID=PD.PRD_COY_ID " _
        '    & " INNER JOIN USER_ASSIGN UA ON PM.PRM_COY_ID=UA.UA_COY_ID " _
        '    & " INNER JOIN COMMODITY_TYPE CM ON CM.CT_ID = PD.PRD_CT_ID AND CM.CT_ROOT_PREFIX=UA.UA_ASSIGN_VALUE " _
        '    & " LEFT JOIN PRODUCT_MSTR PMM ON PD.PRD_PRODUCT_CODE=PMM.PM_PRODUCT_CODE "
        '    strSql = strSql & " WHERE PM.PRM_PR_TYPE <> 'CC' AND PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "'"
        '    strSql = strSql & " AND PD.PRD_CONVERT_TO_IND IS NOT NULL "
        '    strSql = strSql & " AND UA.UA_TYPE = 'CT' AND UA.UA_USER_ID = '" & Common.Parse(strUser) & "' "

        '    'If strRole = "BUYER" Then
        '    '    strSql = strSql & " AND PRM_BUYER_ID='" & Common.Parse(strUser) & "'"
        '    'End If

        '    ' Nobody except buyer himself can view his draft PR
        '    'If strRole = "ADMIN" Then
        '    '    strSql = strSql & " AND PM.PRM_PR_Status <> " & PRStatus.Draft
        '    'End If

        '    If strPRNo <> "" Then
        '        strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
        '    End If

        '    'strSql = strSql & " AND PRM_PR_Date >= " & Common.ConvertDate("01/07/2011" & " 00:00:00")
        '    If dteDateFr <> "" Then
        '        strSql = strSql & " AND PRM_PR_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
        '    End If

        '    If dteDateTo <> "" Then
        '        strSql = strSql & " AND PRM_PR_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '    End If

        '    'If pComType <> "" Then
        '    '    strSql &= "AND PD.PRD_CT_ID =" & pComType & " "
        '    'End If

        '    If pDoc <> "" Then
        '        strSql &= "AND PD.PRD_CONVERT_TO_DOC ='" & pDoc & "' "
        '    End If

        '    If pItemType.Count > 0 Then
        '        For i As Integer = 0 To pItemType.Count - 1
        '            If strType = "" Then
        '                strType = "'" & pItemType(i) & "'"
        '            Else
        '                strType = strType & "," & "'" & pItemType(i) & "'"
        '            End If
        '        Next
        '        strType = "(" & strType & ")"
        '        If strType <> "" Then
        '            strSql &= " AND PD.PRD_CONVERT_TO_IND IN " & strType
        '        End If
        '    End If

        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function

        'Public Function GetPONo(ByVal strPRNo As String, ByRef PONo() As String, ByRef COUNT As Integer)
        '    COUNT = 0
        '    Dim strSQL As String

        '    strSQL = "SELECT DISTINCT PRD_CONVERT_TO_DOC " _
        '        & "FROM pr_details " _
        '        & "INNER JOIN pr_mstr ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
        '        & "WHERE PRM_PR_TYPE='CC' AND prd_pr_no='" & Common.Parse(strPRNo) & "' " _
        '        & "AND PRD_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
        '        & "AND PRD_CONVERT_TO_DOC IS NOT NULL"
        '    ' & "AND PRM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        PONo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_CONVERT_TO_DOC")
        '        COUNT = COUNT + 1
        '    Next
        'End Function

        'Public Function GetPONoCC(ByVal strPRNo As String, ByRef PONo() As String, ByRef COUNT As Integer)
        '    COUNT = 0
        '    Dim strSQL As String

        '    strSQL = "SELECT DISTINCT PRD_CONVERT_TO_DOC " _
        '        & "FROM pr_details " _
        '        & "INNER JOIN pr_mstr ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
        '        & "WHERE PRM_PR_TYPE = 'CC' AND PRD_CONVERT_TO_IND = 'PO' AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' " _
        '        & "AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
        '        & "AND PRD_CONVERT_TO_DOC IS NOT NULL"
        '    ' & "AND PRM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        PONo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_CONVERT_TO_DOC")
        '        COUNT = COUNT + 1
        '    Next
        'End Function

        'Public Function GetPONoNonCC(ByVal strPRNo As String, ByRef PONo() As String, ByRef COUNT As Integer)
        '    COUNT = 0
        '    Dim strSQL As String

        '    strSQL = "SELECT DISTINCT PRD_CONVERT_TO_DOC " _
        '        & "FROM pr_details " _
        '        & "INNER JOIN pr_mstr ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
        '        & "WHERE PRM_PR_TYPE <> 'CC' AND PRM_PR_STATUS = '" & PRStatus.ConvertedToPO & "' AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' " _
        '        & "AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
        '        & "AND PRD_CONVERT_TO_DOC IS NOT NULL"
        '    ' & "AND PRM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        PONo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_CONVERT_TO_DOC")
        '        COUNT = COUNT + 1
        '    Next
        'End Function

        'Public Function GetPONoNonCC2(ByVal strDocNo As String, ByRef DocNo() As String, ByRef COUNT As Integer)
        '    COUNT = 0
        '    Dim strSQL As String

        '    strSQL = "SELECT IFNULL(POM_PO_NO,'') AS POM_PO_NO FROM PO_MSTR WHERE POM_RFQ_INDEX = '" & strDocNo & "' AND POM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        DocNo(COUNT) = tDS.Tables(0).Rows(j).Item("POM_PO_NO")
        '        COUNT = COUNT + 1
        '    Next
        'End Function

        'Public Function GetPONoALL(ByVal strPRNo As String, ByRef PONo() As String, ByRef COUNT As Integer)
        '    COUNT = 0
        '    Dim strSQL As String

        '    strSQL = "SELECT DISTINCT PRD_CONVERT_TO_DOC " _
        '        & "FROM pr_details " _
        '        & "INNER JOIN pr_mstr ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
        '        & "WHERE prd_pr_no='" & Common.Parse(strPRNo) & "' " _
        '        & "AND PRD_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
        '        & "AND PRD_CONVERT_TO_DOC IS NOT NULL"
        '    ' & "AND PRM_BUYER_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        PONo(COUNT) = tDS.Tables(0).Rows(j).Item("PRD_CONVERT_TO_DOC")
        '        COUNT = COUNT + 1
        '    Next
        'End Function

        'Function SearchPRCancelList(ByVal strPRNo As String, ByVal dteDateFr As String, ByVal dteDateTo As String, Optional ByVal strPRType As String = "") As DataSet
        '    Dim strSql, strCondition As String
        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    Dim ds As DataSet
        '    strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_CURRENCY_CODE," _
        '        & "PRM_S_COY_NAME,PM.PRM_REQ_NAME,PRM_PR_STATUS," _
        '        & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') AS STATUS_DESC, " _
        '        & "PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, PRM_PO_INDEX, " _
        '        & "(SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) AS PO_NO," _
        '        & "PRM_STATUS_CHANGED_BY, PM.PRM_CREATED_DATE AS PRM_CREATED_DATE,PRM_SUBMIT_DATE,PRM_PR_TYPE, PM.PRM_URGENT " _
        '        & "FROM PR_MSTR PM " _
        '        & "WHERE PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "' " _
        '        & "AND PRM_BUYER_ID='" & Common.Parse(strUser) & "' " _
        '        & "AND PM.PRM_PR_Status =2 "

        '    If strPRNo <> "" Then
        '        strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
        '    End If

        '    If strPRType = "CC" Then
        '        strSql = strSql & " AND PM.PRM_PR_TYPE = '" & Common.Parse(strPRType) & "'"
        '    ElseIf strPRType = "NonCont" Then
        '        strSql = strSql & " AND (PM.PRM_PR_TYPE IS NULL OR PM.PRM_PR_TYPE = '')"
        '    End If

        '    If dteDateFr <> "" Then
        '        strSql = strSql & " AND (PRM_CREATED_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & " OR PRM_SUBMIT_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & ")"
        '    End If

        '    If dteDateTo <> "" Then
        '        strSql = strSql & " AND (PRM_CREATED_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & " OR PRM_SUBMIT_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & ")"
        '    End If

        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function

        'Function SearchPRCancelListByPO(ByVal strPRNo As String, ByVal dteDateFr As String, ByVal dteDateTo As String, Optional ByVal strPRType As String = "") As DataSet
        '    Dim strSql, strCondition As String
        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    Dim ds As DataSet
        '    strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_CURRENCY_CODE," _
        '        & "PRM_S_COY_NAME,PM.PRM_REQ_NAME,PRM_PR_STATUS," _
        '        & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') AS STATUS_DESC, " _
        '        & "PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, PRM_PO_INDEX, " _
        '        & "(SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) AS PO_NO," _
        '        & "PRM_STATUS_CHANGED_BY, PM.PRM_CREATED_DATE AS PRM_CREATED_DATE,PRM_SUBMIT_DATE,PRM_PR_TYPE, PM.PRM_URGENT " _
        '        & "FROM PR_MSTR PM " _
        '        & "WHERE PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "' " _
        '        & "AND PRM_STATUS_CHANGED_BY='" & Common.Parse(strUser) & "' " _
        '        & "AND PM.PRM_PR_Status =6 "

        '    If strPRNo <> "" Then
        '        strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
        '    End If

        '    If strPRType = "CC" Then
        '        strSql = strSql & " AND PM.PRM_PR_TYPE = '" & Common.Parse(strPRType) & "'"
        '    ElseIf strPRType = "NonCont" Then
        '        strSql = strSql & " AND (PM.PRM_PR_TYPE IS NULL OR PM.PRM_PR_TYPE = '') AND PM.PRM_PR_DATE IS NOT NULL "
        '    End If

        '    If dteDateFr <> "" Then
        '        strSql = strSql & " AND (PRM_CREATED_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & " OR PRM_SUBMIT_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & ")"
        '    End If

        '    If dteDateTo <> "" Then
        '        strSql = strSql & " AND (PRM_CREATED_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & " OR PRM_SUBMIT_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & ")"
        '    End If

        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function

        Function getReliefList(ByVal UserID As String, ByVal CompanyID As String, Optional ByVal strType As String = "") As DataView
            Dim strSql As String
            Dim dvRelief As DataView
            'SELECT AGA_A_AO,B.UM_USER_NAME as AAO_NAME
            'FROM APPROVAL_GRP_AO A 
            'left join USER_MSTR B ON A.AGA_A_AO = B.UM_USER_ID AND B.UM_COY_ID='demo'
            'left JOIN APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX =M.AGM_GRP_INDEX 
            'AND M.AGM_COY_ID = B.UM_COY_ID 
            'where M.AGM_COY_ID ='demo' 
            'AND (AGA_A_AO <>'' AND AGA_A_AO IS NOT NULL) 
            'AND AGA_AO in (
            'SELECT RAM_USER_ID  FROM RELIEF_ASSIGNMENT_MSTR
            'where RAM_COY_ID ='demo' AND RAM_USER_ROLE='Approving Officer' AND getdate()
            'BETWEEN RAM_START_DATE AND RAM_END_DATE+1)
            'AND AGA_A_AO='AO1'
            'AND AGA_RELIEF_IND='C'

            Dim strUser As String = UserID
            Dim strCoyId As String = CompanyID
            '//For Relief Ind=Controlled

            '//AGA_A_AO=assign_to_user
            strSql = "SELECT DISTINCT AGA_AO as RAM_USER_ID," &
                objDb.Concat(" : ", "", "AGA_AO", "B.UM_USER_NAME") & " as NAME FROM APPROVAL_GRP_AO A " &
            " Left Join USER_MSTR B ON A.AGA_AO = B.UM_USER_ID AND B.UM_COY_ID='" & strCoyId &
            "' Left JOIN APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX =M.AGM_GRP_INDEX AND M.AGM_COY_ID = B.UM_COY_ID AND M.AGM_TYPE<>'INV' "
            'Michelle (23/11/2010) - To cater for PO
            If strType <> "" Then
                strSql &= " AND  M.AGM_TYPE = '" & strType & "' "
            End If
            strSql &= " Where M.AGM_COY_ID ='" & strCoyId & "' AND AGA_A_AO='" & strUser & "' AND AGA_RELIEF_IND='C' " &
            " AND AGA_AO in (SELECT RAM_USER_ID  FROM RELIEF_ASSIGNMENT_MSTR " &
            " Where RAM_COY_ID ='" & strCoyId & "' AND RAM_USER_ROLE='Approving Officer' AND " &
            Common.ConvertDate(Now.Today) & " BETWEEN RAM_START_DATE AND RAM_END_DATE)"

            'strSql = "SELECT RAM_USER_ID,UM_USER_NAME AS NAME FROM RELIEF_ASSIGNMENT_MSTR A,RELIEF_ASSIGNMENT_USER B,USER_MSTR UM " _
            '& "WHERE A.RAM_ASSIGN_INDEX = B.RAU_ASSIGN_INDEX AND A.RAM_USER_ID=UM.UM_USER_ID AND " & Common.ConvertDate(Now.Today) & _
            '" BETWEEN RAM_START_DATE AND RAM_END_DATE+1 AND RAM_COY_ID='" & strCoyId & _
            '"' AND RAU_ASSIGN_TO_USER='" & strUser & "'"
            dvRelief = objDb.GetView(strSql)
            Return dvRelief
        End Function
        'Function getReliefConsolidator()
        '    Dim strSql As String
        '    Dim dvRelief As DataView

        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    '//No need to check for deactivate User
        '    '//Relief assignment user should be active user as assignment period is short
        '    strSql = "SELECT RAM_USER_ID," &
        '    objDb.Concat(" : ", "", "RAM_USER_ID", "UM_USER_NAME") & " AS NAME FROM RELIEF_ASSIGNMENT_MSTR A,RELIEF_ASSIGNMENT_USER B,USER_MSTR UM " _
        '    & "WHERE A.RAM_ASSIGN_INDEX = B.RAU_ASSIGN_INDEX AND A.RAM_USER_ID=UM.UM_USER_ID AND UM.UM_COY_ID='" & strCoyId &
        '    " 'AND " & Common.ConvertDate(Now.Today) & " BETWEEN RAM_START_DATE AND RAM_END_DATE AND RAM_COY_ID='" & strCoyId &
        '    "' AND RAU_ASSIGN_TO_USER='" & strUser & "' AND RAM_USER_ROLE='Consolidator'"
        '    dvRelief = objDb.GetView(strSql)
        '    Return dvRelief
        'End Function

        '//To be called by PR Approval Page
        '//Date From and Date To are passed by String, so that we can pass in empty string
        'Function getPRListForApproval(ByVal strPRNo As String, ByVal strVendor As String,
        'ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strReliefOn As String,
        'Optional ByVal strAction As String = "new", Optional ByVal strStatus As String = "", Optional ByVal strAOAction As String = "", Optional ByVal strPRType As String = "", Optional ByVal strInclude As String = "", Optional ByVal strIncludeHold As String = "") As DataSet
        '    '//strAction = new (New Requisitions), app (approved list)
        '    Dim strSql, strSqlReliefO, strSqlReliefC, strCondition, strCondition1, strSqlAttached As String
        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    Dim ds As DataSet
        '    Dim strTemp As String
        '    '//??? Old system only filter Out PR with status=Rejected,AO still can see cancelled PR???
        '    '//may be no need to filter out Open PR, because no App grp selected yet

        '    If strAction = "new" Then
        '        '//current approval AO of the PR is login user and PR not rejected by previous AO
        '        strCondition = " AND PA.PRA_FOR = 'PR' AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.PRM_PR_Status NOT IN(" & PRStatus.RejectedBy & "," & PRStatus.CancelledBy & "))"
        '        strCondition1 = "(PA.PRA_AO = '" & strUser & "' " _
        '        & "OR (PA.PRA_A_AO = '" & strUser & "' AND PA.PRA_Relief_Ind='O'))"
        '        ' ''ElseIf strAction = "app" Then
        '        ' ''    strCondition = " AND (PA.PRA_Seq <= PA.PRA_AO_Action)"
        '        ' ''    '//ownership already taken by AAO, no need to check for relief ind
        '        ' ''    strCondition1 = "(((PA.PRA_AO = '" & strUser & "' " _
        '        ' ''    & "OR PA.PRA_A_AO = '" & strUser & "') AND PRA_RELIEF_IND='O') OR ((PA.PRA_AO = '" & strUser & "' " _
        '        ' ''    & "OR PA.PRA_ON_BEHALFOF = '" & strUser & "') AND PRA_RELIEF_IND='C'))"
        '        ' ''    '//set to empty string to prevent mistake during parameter passing
        '        ' ''    strReliefOn = ""
        '    ElseIf strAction = "app" Then
        '        If strAOAction = "" Then
        '            strCondition = " AND PA.PRA_FOR = 'PR' AND (PA.PRA_Seq <= PA.PRA_AO_Action)"
        '        Else
        '            If strAOAction = "Approved" Then
        '                strCondition = " AND PA.PRA_FOR = 'PR' AND (PA.PRA_Seq <= PA.PRA_AO_Action) AND (SUBSTRING(PRA_AO_REMARK,1,8) = '" & strAOAction & "' OR SUBSTRING(PRA_AO_REMARK,1,8) = 'Endorsed') "
        '            Else
        '                strCondition = " AND PA.PRA_FOR = 'PR' AND (PA.PRA_Seq <= PA.PRA_AO_Action) AND SUBSTRING(PRA_AO_REMARK,1,8) = '" & strAOAction & "' "
        '            End If

        '        End If
        '        '//ownership already taken by AAO, no need to check for relief ind
        '        strCondition1 = "(((PA.PRA_AO = '" & strUser & "' " _
        '        & "OR PA.PRA_A_AO = '" & strUser & "') AND PRA_RELIEF_IND='O') OR ((PA.PRA_AO = '" & strUser & "' " _
        '        & "OR PA.PRA_ON_BEHALFOF = '" & strUser & "') AND PRA_RELIEF_IND<>'O'))"
        '        '//set to empty string to prevent mistake during parameter passing
        '        strReliefOn = ""
        '    End If

        '    '// 1 PR MANY ATTACHMENT
        '    '//MAYBE USE ATTCHED IND
        '    strSqlAttached = "SELECT CDA_DOC_NO FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyId & "' AND CDA_DOC_TYPE='PR'"

        '    '//For Relief Ind=Open
        '    'strSqlReliefO = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_BUYER_ID, " _
        '    '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PM.PRM_S_COY_NAME,PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '    '& "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
        '    '& "(SELECT SUM(PRD_UNIT_COST*PRD_ORDERED_QTY) FROM PR_DETAILS PD WHERE " _
        '    '& "PD.PRD_PR_NO=PM.PRM_PR_NO AND PD.PRD_COY_ID=PM.PRM_COY_ID) AS PR_AMT,PM.PRM_RFQ_INDEX " _
        '    '& ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO,PRM_RFQ_INDEX " _
        '    '& "FROM PR_Approval PA INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
        '    '& "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
        '    '& "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
        '    '& "AND UM.UM_COY_ID='" & strCoyId & "' WHERE " & strCondition1 & _
        '    '" AND PM.PRM_COY_ID='" & strCoyId & "'" & strCondition

        '    'Michelle (16/7/2007) - To cater for multiple vendors
        '    'strSqlReliefO = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_BUYER_ID, " _
        '    '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PM.PRM_S_COY_NAME,PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '    '& "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
        '    '& "PM.PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, UM1.UM_USER_NAME " _
        '    '& ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO,PRM_RFQ_INDEX " _
        '    '& "FROM PR_Approval PA INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
        '    '& "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
        '    '& "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.PRM_BUYER_ID " _
        '    '& "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '    '& "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
        '    '& "AND UM.UM_COY_ID='" & strCoyId & "' WHERE " & strCondition1 & _
        '    '" AND PM.PRM_COY_ID='" & strCoyId & "'" & strCondition

        '    'Michelle (21/9/2010) - To change the syntax to cater for MSSQL & MYSQL
        '    'strSqlReliefO = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_BUYER_ID, " _
        '    '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PRM_S_COY_NAME = CASE isnull(PM.PRM_S_COY_NAME,'NULL')  " _
        '    '& "WHEN '' THEN 'Multiple Vendors' WHEN 'NULL' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END ,PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '    strSqlReliefO = "SELECT * FROM (SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_SUBMIT_DATE, PM.PRM_BUYER_ID, " _
        '   & "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, (CASE WHEN PRM_S_COY_NAME IS NULL THEN 'MULTIPLE VENDORS' " _
        '   & "WHEN PM.PRM_S_COY_NAME = '' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END) AS PRM_S_COY_NAME , " _
        '   & "CAST(GROUP_CONCAT(CM.CM_COY_NAME) AS CHAR(1000)) AS ALLV, " _
        '   & "(CASE WHEN COUNT(DISTINCT PD.PRD_S_COY_ID) > 1 THEN (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' HAVING COUNT(DISTINCT IFNULL(R.PRD_VENDOR_ITEM_CODE, '')) = 1) <> '' THEN CAST(CM.CM_COY_NAME AS CHAR(1000)) ELSE (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' AND R.PRD_S_COY_ID <> '&nbsp;'	HAVING COUNT(DISTINCT IFNULL(R.PRD_S_COY_ID, '')) = 1) <> '' THEN CAST(CM.CM_COY_NAME AS CHAR(1000)) ELSE 'Multiple Vendors' END ) END) ELSE CM.CM_COY_NAME END) AS SNAME, " _
        '   & "(CASE WHEN COUNT(DISTINCT PD.PRD_S_COY_ID) > 1 THEN (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' HAVING COUNT(DISTINCT IFNULL(R.PRD_VENDOR_ITEM_CODE, '')) = 1) <> '' THEN CAST(PD.PRD_S_COY_ID AS CHAR(100)) ELSE (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' AND R.PRD_S_COY_ID <> '&nbsp;'	HAVING COUNT(DISTINCT IFNULL(R.PRD_S_COY_ID, '')) = 1) <> '' THEN CAST(CM.CM_COY_ID AS CHAR(1000)) ELSE 'Multiple Vendors' END ) END) ELSE CM.CM_COY_ID END) AS SNAMEID, " _
        '   & "PM.PRM_REQ_NAME,PM.PRM_PR_STATUS, (CASE WHEN PM.PRM_PR_STATUS = '8' THEN 'Rejected' WHEN PM.PRM_PR_STATUS = '7' THEN 'On Hold' ELSE 'Approved' END) AS STAT, PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '   & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
        '   & "PM.PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, UM1.UM_USER_NAME " _
        '   & ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO, (SELECT POM_PO_INDEX FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as POM_PO_INDEX, (SELECT POM_B_COY_ID FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as POM_B_COY_ID, (SELECT POM_PO_STATUS FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as POM_PO_STATUS, PM.PRM_PR_TYPE, PM.PRM_URGENT, " _
        '   & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyId & "') AS NAME " _
        '   & "FROM PR_Approval PA INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
        '   & "INNER JOIN PR_DETAILS PD ON PD.PRD_PR_NO = PM.PRM_PR_NO AND PD.PRD_COY_ID = PM.PRM_COY_ID " _
        '   & "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
        '   & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.PRM_BUYER_ID " _
        '   & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '   & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
        '   & "AND UM.UM_COY_ID='" & strCoyId & "' " _
        '   & "LEFT OUTER JOIN COMPANY_MSTR CM ON CM.CM_COY_ID = PD.PRD_S_COY_ID " _
        '   & "WHERE " & strCondition1 &
        '   " AND PM.PRM_COY_ID='" & strCoyId & "'" & strCondition


        '    ' & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY AND UM.UM_DELETED='N' AND UM.UM_STATUS='A'" _
        '    '& "AND UM.UM_COY_ID='" & strCoyId & "' WHERE PM.PRM_COY_ID='" & strCoyId & "'"

        '    If strPRNo <> "" Then
        '        strTemp = Common.BuildWildCard(strPRNo)
        '        strSqlReliefO = strSqlReliefO & " AND PM.PRM_PR_No" & Common.ParseSQL(strTemp)
        '    End If

        '    If strPRType = "CC" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.PRM_PR_TYPE = '" & Common.Parse(strPRType) & "'"
        '    ElseIf strPRType = "NonCont" Then
        '        strSqlReliefO = strSqlReliefO & " AND (PM.PRM_PR_TYPE IS NULL OR PM.PRM_PR_TYPE = '')"
        '    End If

        '    'If strVendor <> "" Then
        '    '    strSqlReliefO = strSqlReliefO & " AND PM.PRM_S_COY_NAME" & Common.ParseSQL(strVendor)
        '    'End If

        '    If dteDateFr <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PRM_SUBMIT_DATE >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
        '    End If

        '    'If dteDateFr = "" And dteDateTo <> "" Then
        '    If dteDateTo <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PRM_SUBMIT_DATE <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '    End If

        '    If strAction = "app" And strStatus <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.PRM_PR_Status IN(" & strStatus & ")"
        '    End If

        '    If (strAOAction = "Approved" Or strAOAction = "") And strInclude = "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.PRM_PR_Status NOT IN(8)"
        '    End If

        '    If (strAOAction = "Approved" Or strAOAction = "") And strIncludeHold = "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.PRM_PR_Status NOT IN(7)"
        '    End If
        '    '//For Relief Ind=Open

        '    strSqlReliefO = strSqlReliefO & " GROUP BY PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_SUBMIT_DATE, " _
        '                    & " PM.PRM_BUYER_ID, PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, " _
        '                    & " PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME, " _
        '                    & " PM.PRM_PR_COST,PM.PRM_RFQ_INDEX, UM1.UM_USER_NAME ,PRM_PO_INDEX, " _
        '                    & " PM.PRM_PR_TYPE, PM.PRM_URGENT) XXX "

        '    If strVendor <> "" Then
        '        strSqlReliefO = strSqlReliefO & " WHERE INSTR(XXX.ALLV,'" & strVendor & "') > 0 "
        '    End If

        '    Dim strSqlCheck, strReliefList As String

        '    If UCase(strReliefOn) = "WHEELALL" Then '//in case need to show all PR
        '        Dim dvCheck As DataView
        '        '//For Relief Ind=Controlled             
        '        dvCheck = getReliefList()
        '        Dim intCnt, intLoop As Integer
        '        If Not dvCheck Is Nothing Then
        '            intCnt = dvCheck.Count
        '            For intLoop = 0 To intCnt - 1
        '                If Not IsDBNull(dvCheck.Item(intLoop)("RAM_USER_ID")) Then
        '                    If strReliefList = "" Then
        '                        strReliefList = "'" & dvCheck.Item(intLoop)("RAM_USER_ID") & "'"
        '                    Else
        '                        strReliefList = strReliefList & ",'" & dvCheck(intLoop)("RAM_USER_ID") & "'"
        '                    End If
        '                End If
        '            Next
        '        End If
        '    ElseIf strReliefOn <> "" Then
        '        strReliefList = "'" & strReliefOn & "'"
        '    Else
        '        strReliefList = ""
        '    End If

        '    If strReliefList <> "" Then
        '        ' Michelle (16/7/2007) - To cater for multiple vendors
        '        'strSqlReliefC = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_BUYER_ID, " _
        '        '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PM.PRM_S_COY_NAME,PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '        '& "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
        '        '& "(SELECT SUM(PRD_UNIT_COST*PRD_ORDERED_QTY) FROM PR_DETAILS PD WHERE " _
        '        '& "PD.PRD_PR_NO=PM.PRM_PR_NO AND PD.PRD_COY_ID=PM.PRM_COY_ID) AS PR_AMT,PM.PRM_RFQ_INDEX,UM1.UM_USER_NAME " _
        '        '& ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO,PRM_RFQ_INDEX " _
        '        '& "FROM PR_Approval PA INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
        '        '& "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
        '        '& "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.PRM_BUYER_ID " _
        '        '& "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '        '& "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
        '        '& "AND UM.UM_COY_ID='" & strCoyId & "' " _
        '        '& "WHERE PA.PRA_AO IN(" & strReliefList & ") " _
        '        '& "AND PA.PRA_A_AO = '" & strUser & "' " _
        '        '& "And PA.PRA_Relief_Ind='C' AND PM.PRM_COY_ID='" & strCoyId & "'" & strCondition

        '        'Michelle (21/9/2010) - To change the syntax to cater for MSSQL & MYSQL
        '        '    strSqlReliefC = "SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date,PM.PRM_BUYER_ID, " _
        '        '    & "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, PRM_S_COY_NAME = CASE isnull(PM.PRM_S_COY_NAME,'NULL')  " _
        '        '& "WHEN '' THEN 'Multiple Vendors' WHEN 'NULL' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END ,PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '        strSqlReliefC = "SELECT * FROM (SELECT DISTINCT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_SUBMIT_DATE,PM.PRM_BUYER_ID, " _
        '        & "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, (CASE WHEN PRM_S_COY_NAME IS NULL THEN 'Multiple Vendors'  " _
        '        & "WHEN PM.PRM_S_COY_NAME = '' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END) as PRM_S_COY_NAME, " _
        '        & "CAST(GROUP_CONCAT(CM.CM_COY_NAME) AS CHAR(1000)) AS ALLV, " _
        '        & "(CASE WHEN COUNT(DISTINCT PD.PRD_S_COY_ID) > 1 THEN (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' HAVING COUNT(DISTINCT IFNULL(R.PRD_VENDOR_ITEM_CODE, '')) = 1) <> '' THEN CAST(CM.CM_COY_NAME AS CHAR(1000)) ELSE (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' AND R.PRD_S_COY_ID <> '&nbsp;'	HAVING COUNT(DISTINCT IFNULL(R.PRD_S_COY_ID, '')) = 1) <> '' THEN CAST(CM.CM_COY_NAME AS CHAR(1000)) ELSE 'Multiple Vendors' END ) END) ELSE CM.CM_COY_NAME END) AS SNAME, " _
        '        & "(CASE WHEN COUNT(DISTINCT PD.PRD_S_COY_ID) > 1 THEN (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' HAVING COUNT(DISTINCT IFNULL(R.PRD_VENDOR_ITEM_CODE, '')) = 1) <> '' THEN CAST(PD.PRD_S_COY_ID AS CHAR(100)) ELSE (CASE WHEN (SELECT IFNULL(R.PRD_S_COY_ID, '') PRD_S_COY_ID FROM PR_DETAILS R WHERE R.PRD_COY_ID = '" & strCoyId & "' AND R.PRD_PR_NO = PM.PRM_PR_NO AND R.PRD_VENDOR_ITEM_CODE <> '' AND R.PRD_S_COY_ID <> '&nbsp;'	HAVING COUNT(DISTINCT IFNULL(R.PRD_S_COY_ID, '')) = 1) <> '' THEN CAST(CM.CM_COY_ID AS CHAR(1000)) ELSE 'Multiple Vendors' END ) END) ELSE CM.CM_COY_ID END) AS SNAMEID, " _
        '        & "PM.PRM_REQ_NAME,PM.PRM_PR_STATUS, (CASE WHEN PM.PRM_PR_STATUS = '8' THEN 'Rejected' WHEN PM.PRM_PR_STATUS = '7' THEN 'On Hold' ELSE 'Approved' END) AS STAT, PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '        & "WHEN PM.PRM_S_COY_NAME = '' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END) as PRM_S_COY_NAME,PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,(CASE WHEN PM.PRM_PR_STATUS = '8' THEN 'Rejected' WHEN PM.PRM_PR_STATUS = '7' THEN 'On Hold' ELSE 'Approved' END) AS STAT, PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '        & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
        '        & "(SELECT SUM(PRD_UNIT_COST*PRD_ORDERED_QTY) FROM PR_DETAILS PD WHERE " _
        '        & "PD.PRD_PR_NO=PM.PRM_PR_NO AND PD.PRD_COY_ID=PM.PRM_COY_ID) AS PR_AMT,PM.PRM_RFQ_INDEX,UM1.UM_USER_NAME " _
        '        & ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO, (SELECT POM_PO_INDEX FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as POM_PO_INDEX, (SELECT POM_B_COY_ID FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as POM_B_COY_ID, (SELECT POM_PO_STATUS FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as POM_PO_STATUS, PM.PRM_PR_TYPE, PM.PRM_URGENT, " _
        '        & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.PRM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyId & "') AS NAME " _
        '        & "FROM PR_Approval PA INNER JOIN PR_MSTR PM ON PA.PRA_PR_INDEX = PM.PRM_PR_INDEX " _
        '        & "INNER JOIN PR_DETAILS PD ON PD.PRD_PR_NO = PM.PRM_PR_NO AND PD.PRD_COY_ID = PM.PRM_COY_ID " _
        '        & "LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
        '        & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.PRM_BUYER_ID " _
        '        & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '        & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
        '        & "AND UM.UM_COY_ID='" & strCoyId & "' " _
        '        & "LEFT OUTER JOIN COMPANY_MSTR CM ON CM.CM_COY_ID = PD.PRD_S_COY_ID " _
        '        & "WHERE PA.PRA_AO IN(" & strReliefList & ") " _
        '        & "AND PA.PRA_A_AO = '" & strUser & "' " _
        '        & "And PA.PRA_Relief_Ind='C' AND PM.PRM_COY_ID='" & strCoyId & "'" & strCondition


        '        If strPRNo <> "" Then
        '            strTemp = Common.BuildWildCard(strPRNo)
        '            strSqlReliefC = strSqlReliefC & " AND PM.PRM_PR_No" & Common.ParseSQL(strTemp)
        '        End If

        '        'If strVendor <> "" Then
        '        '    strSqlReliefC = strSqlReliefC & " AND PM.PRM_S_COY_NAME" & Common.ParseSQL(strVendor)
        '        'End If

        '        'If dteDateFr <> "" And dteDateTo <> "" Then
        '        '    strSqlReliefC = strSqlReliefC & " AND PRM_PR_Date BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00") & _
        '        '    " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '        'ElseIf dteDateFr <> "" And dteDateTo = "" Then
        '        '    strSqlReliefC = strSqlReliefC & " AND PRM_PR_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
        '        'ElseIf dteDateFr = "" And dteDateTo <> "" Then
        '        '    strSqlReliefC = strSqlReliefC & " AND PRM_PR_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '        'End If

        '        If dteDateFr <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PRM_SUBMIT_DATE >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
        '        End If

        '        'If dteDateFr = "" And dteDateTo <> "" Then
        '        If dteDateTo <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PRM_SUBMIT_DATE <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '        End If


        '        If strAction = "app" And strStatus <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PM.PRM_PR_Status IN(" & strStatus & ")"
        '        End If

        '        If (strAOAction = "Approved" Or strAOAction = "") And strInclude = "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PM.PRM_PR_Status NOT IN(8)"
        '        End If

        '        If (strAOAction = "Approved" Or strAOAction = "") And strIncludeHold = "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PM.PRM_PR_Status NOT IN(7)"
        '        End If

        '        strSqlReliefC = strSqlReliefC & " GROUP BY PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_SUBMIT_DATE, " _
        '                    & " PM.PRM_BUYER_ID, PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, " _
        '                    & " PM.PRM_REQ_NAME,PM.PRM_PR_STATUS,PM.PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME, " _
        '                    & " PM.PRM_PR_COST,PM.PRM_RFQ_INDEX, UM1.UM_USER_NAME ,PRM_PO_INDEX, " _
        '                    & " PM.PRM_PR_TYPE, PM.PRM_URGENT) ZZZ "

        '        If strVendor <> "" Then
        '            strSqlReliefC = strSqlReliefC & " WHERE INSTR(ZZZ.ALLV,'" & strVendor & "') > 0 "
        '        End If
        '    End If
        '    '//For Relief Ind=Controlled


        '    If UCase(strReliefOn) = "WHEELALL" Then '//show all
        '        If strSqlReliefC <> "" Then
        '            strSql = strSqlReliefO & " UNION " & strSqlReliefC
        '        Else
        '            strSql = strSqlReliefO
        '        End If
        '    ElseIf strReliefOn <> "" Then '//show other ao's
        '        strSql = strSqlReliefC
        '    Else '//show own pr
        '        strSql = strSqlReliefO
        '    End If

        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function

        Function getApprFlowPO(ByVal intPRIndex As Long, ByVal CompanyID As String, Optional ByVal strFor As String = "") As String
            Dim strSql As String
            Dim ds As DataSet
            strSql = "SELECT PA.*,UMA.UM_USER_NAME AS AO_NAME,UMB.UM_USER_NAME AS AAO_NAME," _
            & "CASE WHEN PRA_APPROVAL_TYPE = 1 THEN 'Approval' ELSE 'None' END AS Type,  " _
            & "UMA.UM_APP_LIMIT AS AO_LIMIT,UMB.UM_APP_LIMIT AS AAO_LIMIT FROM " _
            & "PR_APPROVAL PA LEFT OUTER JOIN USER_MSTR UMA ON " _
            & "PA.PRA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & CompanyID & "' LEFT OUTER JOIN USER_MSTR UMB ON " _
            & "PA.PRA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID='" & CompanyID & "' " _
            & "WHERE PRA_PR_INDEX=" & intPRIndex
            If strFor <> "" Then
                strSql &= " AND PRA_FOR = '" & Common.Parse(strFor) & "' ORDER BY PA.PRA_SEQ"
            Else
                strSql &= " ORDER BY PA.PRA_SEQ"
            End If
            ds = objDb.FillDs(strSql)
            Return strSql
        End Function


        Function getApprFlow(ByVal intPRIndex As Long, ByVal CompanyID As String, Optional ByVal strFor As String = "") As String
            Dim strSql As String
            Dim ds As DataSet
            strSql = "SELECT PA.*,UMA.UM_USER_NAME AS AO_NAME,UMB.UM_USER_NAME AS AAO_NAME," _
            & "CASE WHEN PRA_APPROVAL_TYPE = 2 THEN 'Approval' ELSE 'None' END AS Type,  " _
            & "UMA.UM_APP_LIMIT AS AO_LIMIT,UMB.UM_APP_LIMIT AS AAO_LIMIT FROM " _
            & "PR_APPROVAL PA LEFT OUTER JOIN USER_MSTR UMA ON " _
            & "PA.PRA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & CompanyID & "' LEFT OUTER JOIN USER_MSTR UMB ON " _
            & "PA.PRA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID='" & CompanyID & "' " _
            & "WHERE PRA_PR_INDEX=" & intPRIndex
            If strFor <> "" Then
                strSql &= " AND PRA_FOR = '" & Common.Parse(strFor) & "' ORDER BY PA.PRA_SEQ"
            Else
                strSql &= " ORDER BY PA.PRA_SEQ"
            End If
            ds = objDb.FillDs(strSql)
            Return strSql
        End Function

        Function RejectPR(ByVal companyID As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal strUserID As String, ByVal blnRelief As Boolean) As String
            Dim strSql, strSqlAry(0) As String
            Dim strCoyID As String
            Dim intPRStatus As Integer
            Dim strMsg As String
            Dim result As Boolean = False
            strCoyID = companyID
            strSql = "SELECT PRM_PR_STATUS,PRM_STATUS_CHANGED_BY FROM PR_MSTR WHERE PRM_PR_Index=" & intPRIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intPRStatus = tDS.Tables(0).Rows(0).Item("PRM_PR_STATUS")
                If intPRStatus = PRStatus.CancelledBy Then
                    strMsg = "The PR has already been cancelled. Rejecting of this PR is not allowed. "
                ElseIf intPRStatus = PRStatus.RejectedBy And tDS.Tables(0).Rows(0).Item("PRM_STATUS_CHANGED_BY") = strUserID Then
                    strMsg = "You have already rejected this PR."
                ElseIf intPRStatus = PRStatus.Approved Then '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved/endorsed this PR. Rejecting of this PR is not allowed."
                ElseIf intPRStatus = PRStatus.ConvertedToPO Then
                    strMsg = "You have already converted this PR to PO. Rejecting of this PR is not allowed."
                End If
            End If

            If intPRStatus = PRStatus.PendingApproval And isApproved(intPRIndex, intCurrentSeq, strUserID) Then
                strMsg = "You have already approved/endorsed this PR. Rejecting of this PR is not allowed."
            End If

            If strMsg = "" Then
                'Dim objUsers As New Users
                'objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_RejectPR, strPRNo)
                'objUsers = Nothing
                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.RejectedBy &
                ",PRM_STATUS_CHANGED_BY='" & strUserID & "',PRM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & " WHERE PRM_PR_Index=" & intPRIndex
                Common.Insert2Ary(strSqlAry, strSql)
                updateAOAction(strUserID, intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)

                Dim objPR As New PR
                Dim BCM As Integer = 0
                BCM = CInt(objPR.checkBCM(companyID))
                'ViewState("BCM") = 1
                If BCM > 0 Then
                    Dim objBCM As New BudgetControl
                    objBCM.BCMCalc("PR", strCoyID, strPRNo, EnumBCMAction.RejectPR, strSqlAry)
                    objBCM = Nothing
                End If



                If objDb.BatchExecute(strSqlAry) Then
                    Dim objMail As New Email
                    'Dim objPR As New PR
                    Dim strName As String
                    strName = objPR.getRequestorName("PR", strPRNo, strCoyID)
                    'objMail.sendNotification(EmailType.PRRejected, strUserID, strCoyID, "", strPRNo, strName)
                    ' sendMailToPrevAO(strPRNo, intPRIndex, intCurrentSeq, strName)
                    objMail = Nothing
                    objPR = Nothing
                    strMsg = "Purchase Requisition Number " & strPRNo & " has been rejected."
                    '//SEND MAIL TO BUYER
                    'result = True
                Else
                    strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                    'result = False
                End If
                Return strMsg

            Else
                Return strMsg
            End If
        End Function

        'Public Function sendMailToPrevAO(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, ByVal strRequestor As String)
        '    Dim strsql, strRole As String
        '    Dim i As Integer
        '    Dim ds As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common

        '    strRole = "Approving Officer"
        '    strBody &= "<P>PR (" & strDocNo & ") has been rejected by your Approving Officer. <BR>"
        '    strBody &= "<P>Requestor Name : " & strRequestor & "<BR>"
        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
        '    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
        '    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
        '    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
        '    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
        '    '    strBody &= "<P>" & objCommon.EmailFooterENT & Common.EmailCompGen
        '    'End If

        '    strsql = "SELECT PRA_ACTIVE_AO, UM_EMAIL, UM_USER_NAME FROM PR_APPROVAL "
        '    strsql &= "LEFT JOIN USER_MSTR ON PRA_ACTIVE_AO = UM_USER_ID AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_FOR = 'PR' AND PRA_SEQ < " & intSeq
        '    ds = objDb.FillDs(strsql)

        '    Dim objMail As New AppMail
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))
        '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Approving Officer), <BR>" & strBody

        '        objMail.Subject = "Agora : PR Rejected"
        '        objMail.SendMail()
        '    Next

        '    objMail = Nothing
        '    objCommon = Nothing
        'End Function

        'Public Function sendMailToPrevAOHold(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, ByVal strRequestor As String)
        '    Dim strsql, strRole As String
        '    Dim i As Integer
        '    Dim ds As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common

        '    strRole = "Approving Officer"
        '    strBody &= "<P>PR (" & strDocNo & ") has been held by your Approving Officer. <BR>"
        '    strBody &= "<P>Requestor Name : " & strRequestor & "<BR>"
        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

        '    strsql = "SELECT PRA_ACTIVE_AO, UM_EMAIL, UM_USER_NAME FROM PR_APPROVAL "
        '    strsql &= "LEFT JOIN USER_MSTR ON PRA_ACTIVE_AO = UM_USER_ID AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_FOR = 'PR' AND PRA_SEQ < " & intSeq
        '    ds = objDb.FillDs(strsql)

        '    Dim objMail As New AppMail
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))
        '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Approving Officer), <BR>" & strBody

        '        objMail.Subject = "Agora : PR Held"
        '        objMail.SendMail()
        '    Next

        '    objMail = Nothing
        '    objCommon = Nothing
        'End Function

        'Public Function sendMailToBuyer(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, ByVal strRequestor As String)
        '    Dim strsql, strRole As String
        '    Dim i As Integer
        '    Dim ds As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common

        '    strRole = "Buyer"
        '    strBody &= "<P>PR (" & strDocNo & ") has been cancelled by your Purchasing Officer. <BR>"
        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

        '    strsql = "SELECT PRA_ACTIVE_AO, UM_EMAIL, UM_USER_NAME FROM PR_APPROVAL "
        '    strsql &= "LEFT JOIN USER_MSTR ON PRA_ACTIVE_AO = UM_USER_ID AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_FOR = 'PR' AND PRA_SEQ < " & intSeq
        '    ds = objDB.FillDs(strsql)

        '    Dim objMail As New AppMail
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))
        '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Buyer), <BR>" & strBody

        '        objMail.Subject = "eProcure : PR Cancelled"
        '        objMail.SendMail()
        '    Next

        '    objMail = Nothing
        '    objCommon = Nothing
        'End Function

        'Function HoldPR(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal strUserID As String, ByVal blnRelief As Boolean) As String
        '    Dim strSql, strSqlAry(0) As String
        '    Dim strCoyID As String
        '    Dim intPRStatus As Integer
        '    Dim strMsg As String

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strSql = "SELECT PRM_PR_STATUS,PRM_STATUS_CHANGED_BY FROM PR_MSTR WHERE PRM_PR_Index=" & intPRIndex
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        intPRStatus = tDS.Tables(0).Rows(0).Item("PRM_PR_STATUS")
        '        If intPRStatus = PRStatus.CancelledBy Then
        '            strMsg = "The PR has already been cancelled. Holding of this PR is not allowed. "
        '        ElseIf intPRStatus = PRStatus.RejectedBy And tDS.Tables(0).Rows(0).Item("PRM_STATUS_CHANGED_BY") = strUserID Then
        '            strMsg = "You have already rejected this PR. Holding of this PR is not allowed."
        '        ElseIf intPRStatus = PRStatus.Approved Then '//approved '//NEED TO PR-APPROVAL
        '            strMsg = "You have already approved/endorsed this PR. Holding of this PR is not allowed."
        '        ElseIf intPRStatus = PRStatus.ConvertedToPO Then
        '            strMsg = "You have already converted this PR to PO. Holding of this PR is not allowed."
        '        End If
        '    End If


        '    If intPRStatus = PRStatus.PendingApproval And isApproved(intPRIndex, intCurrentSeq, strUserID) Then
        '        strMsg = "You have already approved/endorsed this PR. Holding of this PR is not allowed."
        '    End If

        '    If strMsg = "" Then
        '        Dim objUsers As New Users
        '        objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_HoldPR, strPRNo)
        '        objUsers = Nothing
        '        strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.HeldBy &
        '        ",PRM_STATUS_CHANGED_BY='" & strUserID & "',PRM_STATUS_CHANGED_ON=" &
        '        Common.ConvertDate(Now) & " WHERE PRM_PR_Index=" & intPRIndex
        '        Common.Insert2Ary(strSqlAry, strSql)
        '        strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" &
        '        Common.ConvertDate(Now) & " WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
        '        Common.Insert2Ary(strSqlAry, strSql)
        '        If objDb.BatchExecute(strSqlAry) Then
        '            Dim objMail As New Email
        '            Dim objPR As New PR
        '            Dim strName As String
        '            strName = objPR.getRequestorName("PR", strPRNo, strCoyID)
        '            objMail.sendNotification(EmailType.PRHeld, strUserID, strCoyID, "", strPRNo, strName)
        '            sendMailToPrevAOHold(strPRNo, intPRIndex, intCurrentSeq, strName)
        '            objMail = Nothing
        '            objPR = Nothing
        '            strMsg = "Purchase Requisition Number " & strPRNo & " has been put on hold. "
        '            '//SEND MAIL TO BUYER
        '        Else
        '            strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        '        End If
        '    End If

        '    Return strMsg
        'End Function
        Function CancelPR(ByVal userID As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strRemark As String, Optional ByVal strPOfficer As Boolean = False) As String
            Dim strSql, strSqlAry(0), strUserID As String
            Dim strCoyID As String
            Dim intPRStatus As Integer
            Dim strMsg As String
            Dim objUsers As New Users

            strUserID = userID
            strCoyID = objUsers.getCompanyID(strUserID)

            strSql = "SELECT PRM_PR_STATUS,PRM_STATUS_CHANGED_BY FROM PR_MSTR WHERE PRM_PR_Index=" & intPRIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intPRStatus = tDS.Tables(0).Rows(0).Item("PRM_PR_STATUS")
                If intPRStatus = PRStatus.CancelledBy Then
                    strMsg = "The PR has already been cancelled. "
                ElseIf intPRStatus = PRStatus.RejectedBy And tDS.Tables(0).Rows(0).Item("PRM_STATUS_CHANGED_BY") = strUserID Then
                    strMsg = "The PR has already been rejected."
                ElseIf (intPRStatus = PRStatus.Approved Or intPRStatus = PRStatus.PendingApproval) And strPOfficer = False Then '//approved '//NEED TO check PR-APPROVAL
                    strMsg = "The PR has already been approved/endorsed. "
                ElseIf intPRStatus = PRStatus.ConvertedToPO Then
                    strMsg = "The PR has already been converted to PO."
                End If
            End If

            'Michelle (CR0010) - To update Remarks
            If strMsg = "" Then
                objUsers.Log_UserActivity(strUserID, strCoyID, strSqlAry, WheelModule.PRMod, WheelUserActivity.B_CancelPR, strPRNo)
                objUsers = Nothing
                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.CancelledBy &
                ",PRM_STATUS_CHANGED_BY='" & strUserID & "',PRM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & ", PRM_INTERNAL_REMARK=CONCAT(CONCAT(PRM_INTERNAL_REMARK, CAST('\nCancelled :' AS CHAR(100))), CAST('" & Common.Parse(strRemark) & "' AS CHAR(2500))) WHERE PRM_PR_Index=" & intPRIndex
                Common.Insert2Ary(strSqlAry, strSql)

                Dim objPR As New PR
                Dim BCM As Integer = CInt(objPR.checkBCM(strCoyID))
                'ViewState("BCM") = 1
                If BCM > 0 Then
                    Dim objBCM As New BudgetControl
                    objBCM.BCMCalc("PR", strCoyID, strPRNo, EnumBCMAction.CancelPR, strSqlAry)
                    objBCM = Nothing
                End If

                If objDb.BatchExecute(strSqlAry) Then
                    'ghesa 20210524 - disable email notif
                    'Dim objMail As New Email
                    'If strPOfficer = False Then
                    '    objMail.sendNotification(EmailType.PRCancelled, strUserID, strCoyID, "", strPRNo, "")
                    'Else
                    '    objMail.sendNotification(EmailType.PRCancelled, strUserID, strCoyID, "", strPRNo, "", "All")
                    '    objMail.sendNotification(EmailType.PRCancelledToBuyer, strUserID, strCoyID, "", strPRNo, "")
                    '    'sendMailToBuyer(strPRNo, intPRIndex, intCurrentSeq, strUserID)
                    'End If
                    'objMail = Nothing
                    strMsg = "Purchase Requisition Number " & strPRNo & " has been cancelled."
                    '//SEND MAIL TO first level AO
                    '//old system no mail
                Else
                    strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                End If
                Return strMsg

            Else
                Return strMsg
            End If

        End Function
        Function DuplicatePR(ByVal userID As String, ByVal strPRNo As String, ByVal intPRIndex As Long) As String
            Dim strSql, strArySql(0), strLoginUser As String
            Dim strNewPRNo, strPRPrefix, strCoyID As String
            Dim objGlobal As New AppGlobals
            Dim objUsers As New Users
            Dim dblTotalCost As Double
            Dim dteNow As DateTime = Now()

            strLoginUser = userID
            strCoyID = objUsers.getCompanyID(strLoginUser)

            'GetLatestDocNo
            objGlobal.GetLatestDocNo(strCoyID, "PR", strArySql, strNewPRNo, strPRPrefix)

            ' Ai Chu Add 
            ' user may change last used no in company param - may cause duplication of PR NO
            strSql = "SELECT '*' FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(strNewPRNo) & "' "
            strSql &= "AND PRM_COY_ID = '" & strCoyID & "' "

            If objDb.Exist(strSql) > 0 Then
                ' msg retrieve eProcure modWheel
                Return "Duplicate transaction number found.""&vbCrLf&""Please contact your Administrator to rectify the problem."
            End If

            ' ------- check item not deleted from List Price Catalogue ----------
            strSql = "SELECT '*' FROM PR_DETAILS "
            strSql &= "WHERE PRD_PR_NO = '" & Common.Parse(strPRNo) & "' "
            strSql &= "AND PRD_COY_ID = '" & strCoyID & "' "
            strSql &= "AND PRD_PRODUCT_CODE IN ("
            strSql &= "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR "
            strSql &= "WHERE PM_DELETED = 'Y')"

            If objDb.Exist(strSql) > 0 Then
                Return "This PR cannot be duplicated because some of the items have been deleted from the List Price Catalogue."
            End If
            ' -------------------------------------------------------------------

            strNewPRNo = Common.Parse(strNewPRNo)
            strPRPrefix = Common.Parse(strPRPrefix)

            '//reset PRM_PR_NO,PRM_PR_DATE,PRM_CREATED_DATE,PRM_STATUS_CHANGED_BY,PRM_STATUS_CHANGED_ON,
            '//PRM_PR_STATUS,PRM_PO_INDEX,PRM_ARCHIVE_IND,PRM_CONSOLIDATOR,PRM_RFQ_INDEX,PRM_PR_PREFIX
            '//get updated Vendor info from Company Mstr

            'Michelle (11/7/2007) - Vendor info will not be in the PR_MSTR as 1 PR can have multiple Vendor info
            'strSql = "INSERT INTO PR_MSTR(PRM_PR_NO,PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE,PRM_PR_DATE,PRM_CREATED_DATE," _
            '& "PRM_STATUS_CHANGED_BY,PRM_STATUS_CHANGED_ON,PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,PRM_S_COY_NAME," _
            '& "PRM_S_ADDR_LINE1,PRM_S_ADDR_LINE2,PRM_S_ADDR_LINE3,PRM_S_POSTCODE,PRM_S_CITY,PRM_S_STATE,PRM_S_COUNTRY," _
            '& "PRM_S_PHONE,PRM_S_FAX,PRM_S_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
            '& "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,PRM_EXTERNAL_REMARK," _
            '& "PRM_PR_STATUS,PRM_PO_INDEX,PRM_ARCHIVE_IND,PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,PRM_PR_PREFIX,PRM_B_ADDR_CODE," _
            '& "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,PRM_RFQ_INDEX," _
            '& "PRM_DEPT_INDEX,PRM_DUP_FROM,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,PRM_CONSOLIDATOR,PRM_PR_COST,PRM_GST) SELECT '" & strNewPRNo & "'," _
            '& "PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE," & Common.ConvertDate(dteNow) & "," & Common.ConvertDate(dteNow) & _
            '",'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ",PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,CM_COY_NAME," _
            '& "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY," _
            '& "CM_PHONE,CM_FAX,CM_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
            '& "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,PRM_EXTERNAL_REMARK," _
            '& PRStatus.Draft & ",NULL,'N',PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,'" & strPRPrefix & "',PRM_B_ADDR_CODE," _
            '& "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,NULL,PRM_DEPT_INDEX," _
            '& "PRM_PR_NO,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,NULL,PRM_PR_COST,PRM_GST FROM PR_MSTR A,COMPANY_MSTR B " _
            '& "WHERE PRM_PR_Index=" & intPRIndex & " AND A.PRM_S_COY_ID=B.CM_COY_ID"

            'Michelle (CR0010) - Blank off the remarks if duplication is from a canceled PR
            strSql = "INSERT INTO PR_MSTR(PRM_PR_NO,PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE,PRM_PR_DATE,PRM_CREATED_DATE," _
            & "PRM_STATUS_CHANGED_BY,PRM_STATUS_CHANGED_ON,PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,PRM_S_COY_NAME," _
            & "PRM_S_ADDR_LINE1,PRM_S_ADDR_LINE2,PRM_S_ADDR_LINE3,PRM_S_POSTCODE,PRM_S_CITY,PRM_S_STATE,PRM_S_COUNTRY," _
            & "PRM_S_PHONE,PRM_S_FAX,PRM_S_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
            & "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,PRM_EXTERNAL_REMARK," _
            & "PRM_PR_STATUS,PRM_PO_INDEX,PRM_ARCHIVE_IND,PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,PRM_PR_PREFIX,PRM_B_ADDR_CODE," _
            & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,PRM_RFQ_INDEX," _
            & "PRM_DEPT_INDEX,PRM_DUP_FROM,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,PRM_CONSOLIDATOR,PRM_PR_COST,PRM_GST) SELECT '" & strNewPRNo & "'," _
            & "PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE," & Common.ConvertDate(dteNow) & "," & Common.ConvertDate(dteNow) &
            ",'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ",PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,CM_COY_NAME," _
            & "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY," _
            & "CM_PHONE,CM_FAX,CM_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
            & "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,case PRM_PR_STATUS when '6' Then '' else PRM_EXTERNAL_REMARK end," _
            & PRStatus.Draft & ",NULL,'N',PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,'" & strPRPrefix & "',PRM_B_ADDR_CODE," _
            & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,NULL,PRM_DEPT_INDEX," _
            & "PRM_PR_NO,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,NULL,PRM_PR_COST,PRM_GST FROM PR_MSTR A LEFT OUTER JOIN COMPANY_MSTR B " _
            & "ON A.PRM_S_COY_ID=B.CM_COY_ID WHERE PRM_PR_Index=" & intPRIndex



            Common.Insert2Ary(strArySql, strSql)

            '//need to check status of the product??
            '//need to get latest product info??
            'Michelle (16/6/2010) - To include Tax Code
            strSql = "INSERT INTO PR_DETAILS(PRD_PR_NO,PRD_COY_ID,PRD_PR_LINE,PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE," _
            & "PRD_PRODUCT_DESC,PRD_UOM,PRD_ORDERED_QTY,PRD_UNIT_COST,PRD_ETD,PRD_WARRANTY_TERMS,PRD_MIN_ORDER_QTY," _
            & "PRD_MIN_PACK_QTY,PRD_REMARK,PRD_GST,PRD_D_ADDR_CODE,PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3," _
            & "PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY,PRD_ACCT_INDEX,PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_SOURCE,PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE)SELECT '" & strNewPRNo & "'," _
            & "PRD_COY_ID,PRD_PR_LINE,PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE," _
            & "PRD_PRODUCT_DESC,PRD_UOM,PRD_ORDERED_QTY,PRD_UNIT_COST,PRD_ETD,PRD_WARRANTY_TERMS,PRD_MIN_ORDER_QTY," _
            & "PRD_MIN_PACK_QTY,PRD_REMARK,PRD_GST,PRD_D_ADDR_CODE,PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3," _
            & "PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY,PRD_ACCT_INDEX,PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_SOURCE,PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE " _
            & "FROM PR_DETAILS WHERE PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID='" & strCoyID & "'"
            Common.Insert2Ary(strArySql, strSql)

            '//need to check status of the custom field???
            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR(PCM_PR_INDEX,PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE) SELECT " _
            & "(SELECT MAX(PRM_PR_INDEX) FROM PR_MSTR),PCM_FIELD_NO,PCM_FIELD_NAME,'PR' FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & intPRIndex
            Common.Insert2Ary(strArySql, strSql)

            strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS(PCD_PR_INDEX,PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE)  SELECT " _
            & "(SELECT MAX(PRM_PR_INDEX) FROM PR_MSTR),PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,'PR' FROM PR_CUSTOM_FIELD_DETAILS " _
            & "WHERE PCD_PR_INDEX=" & intPRIndex
            Common.Insert2Ary(strArySql, strSql)

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"
            Dim dtAttach As DataTable
            Dim dr As DataRow
            Dim objFile As New FileManagement
            Dim strHubFile, strActualFile As String
            Dim strBasePath, strPRPath As String
            Dim strSourceFile, strDestFile As String

            strBasePath = objFile.getBasePath(EnumUploadFrom.FrontOff)
            strPRPath = objFile.getSystemParam("DocAttachPath", "PR")
            objFile.checkDirectory(strBasePath & strPRPath)
            dtAttach = objDb.FillDs(strSql).Tables(0)
            For Each dr In dtAttach.Rows
                If Not IsDBNull(dr("CDA_HUB_FILENAME")) Then
                    strSourceFile = strBasePath & strPRPath & dr("CDA_HUB_FILENAME")
                    strHubFile = objFile.getLastDocAttachFileName(dr("CDA_HUB_FILENAME"))
                    strDestFile = strBasePath & strPRPath & strHubFile
                    'Michelle (17/6/2010) - To solve the problem where attachment is not create when duplicating a PR
                    'If System.IO.File.Exists(strDestFile) Then
                    If Not System.IO.File.Exists(strDestFile) Then
                        System.IO.File.Copy(strSourceFile, strDestFile)
                    End If
                    strSql = "INSERT INTO Company_Doc_Attachment(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," _
                    & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) VALUES('" & strCoyID & "','" &
                    strNewPRNo & "','PR','" & strHubFile & "','" & Common.Parse(dr("CDA_ATTACH_FILENAME")) &
                    "'," & dr("CDA_FILESIZE") & ")"
                    Common.Insert2Ary(strArySql, strSql)
                End If
            Next

            objGlobal = Nothing
            objFile = Nothing
            If objDb.BatchExecute(strArySql) Then
                Return "Purchase Requisition Number " & strNewPRNo & " created."
            Else
                Return "Error occurs. Kindly contact the Administrator to resolve this. "
            End If

            ' strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"
            '//return relevant parameter and redirect to Raise PR page
        End Function

        'Function DuplicatePR1(ByVal strPRNo As String, ByVal intPRIndex As Long) As String
        '    'Modified by Joon on 30th June 2011
        '    Dim strSql, strArySql(0), strLoginUser As String
        '    Dim strNewPRNo, strPRPrefix, strCoyID As String
        '    Dim objGlobal As New AppGlobals
        '    Dim dblTotalCost As Double
        '    Dim dteNow As DateTime = Now()

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strLoginUser = HttpContext.Current.Session("UserId")

        '    'GetLatestDocNo
        '    objGlobal.GetLatestDocNo("PR", strArySql, strNewPRNo, strPRPrefix)

        '    ' Ai Chu Add 
        '    ' user may change last used no in company param - may cause duplication of PR NO
        '    strSql = "SELECT '*' FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(strNewPRNo) & "' "
        '    strSql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

        '    If objDb.Exist(strSql) > 0 Then
        '        ' msg retrieve eProcure modWheel
        '        Return "Duplicate transaction number found.""&vbCrLf&""Please contact your Administrator to rectify the problem."
        '    End If

        '    ' ------- check item not deleted from List Price Catalogue ----------
        '    strSql = "SELECT '*' FROM PR_DETAILS "
        '    strSql &= "WHERE PRD_PR_NO = '" & Common.Parse(strPRNo) & "' "
        '    strSql &= "AND PRD_COY_ID = '" & strCoyID & "' "
        '    strSql &= "AND PRD_PRODUCT_CODE IN ("
        '    strSql &= "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR "
        '    strSql &= "WHERE PM_DELETED = 'Y')"

        '    If objDb.Exist(strSql) > 0 Then
        '        Return "This PR cannot be duplicated because some of the items have been deleted from the List Price Catalogue."
        '    End If
        '    ' -------------------------------------------------------------------

        '    strNewPRNo = Common.Parse(strNewPRNo)
        '    strPRPrefix = Common.Parse(strPRPrefix)

        '    'Michelle (CR0010) - Blank off the remarks if duplication is from a canceled PR
        '    strSql = "INSERT INTO PR_MSTR(PRM_PR_NO,PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE,PRM_PR_DATE,PRM_CREATED_DATE,PRM_SUBMIT_DATE," _
        '    & "PRM_STATUS_CHANGED_BY,PRM_STATUS_CHANGED_ON,PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,PRM_S_COY_NAME," _
        '    & "PRM_S_ADDR_LINE1,PRM_S_ADDR_LINE2,PRM_S_ADDR_LINE3,PRM_S_POSTCODE,PRM_S_CITY,PRM_S_STATE,PRM_S_COUNTRY," _
        '    & "PRM_S_PHONE,PRM_S_FAX,PRM_S_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
        '    & "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,PRM_EXTERNAL_REMARK," _
        '    & "PRM_PR_STATUS,PRM_PO_INDEX,PRM_ARCHIVE_IND,PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,PRM_PR_PREFIX,PRM_B_ADDR_CODE," _
        '    & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,PRM_RFQ_INDEX," _
        '    & "PRM_DEPT_INDEX,PRM_DUP_FROM,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,PRM_CONSOLIDATOR,PRM_PR_COST,PRM_GST,PRM_PR_TYPE) " _
        '    & "SELECT '" & strNewPRNo & "'," _
        '    & "PRM_COY_ID,PRM_REQ_NAME,PRM_REQ_PHONE,NULL," & Common.ConvertDate(dteNow) & ",NULL," _
        '    & "'" & strLoginUser & "'," & Common.ConvertDate(dteNow) & ",PRM_BUYER_ID,PRM_S_COY_ID,PRM_S_ATTN,CM_COY_NAME," _
        '    & "CM_ADDR_LINE1,CM_ADDR_LINE2,CM_ADDR_LINE3,CM_POSTCODE,CM_CITY,CM_STATE,CM_COUNTRY," _
        '    & "CM_PHONE,CM_FAX,CM_EMAIL,PRM_FREIGHT_TERMS,PRM_PAYMENT_TYPE,PRM_SHIPMENT_TERM,PRM_SHIPMENT_MODE," _
        '    & "PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_PAYMENT_TERM,PRM_SHIP_VIA,PRM_INTERNAL_REMARK,case PRM_PR_STATUS when '6' Then '' else PRM_EXTERNAL_REMARK end," _
        '    & PRStatus.Draft & ",NULL,'N',PRM_PRINT_CUSTOM_FIELDS,PRM_PRINT_REMARK,'" & strPRPrefix & "',PRM_B_ADDR_CODE," _
        '    & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_STATE,PRM_B_CITY,PRM_B_COUNTRY,NULL,PRM_DEPT_INDEX," _
        '    & "PRM_PR_NO,PRM_EXTERNAL_IND,PRM_REFERENCE_NO,NULL,PRM_PR_COST,PRM_GST,PRM_PR_TYPE FROM PR_MSTR A LEFT OUTER JOIN COMPANY_MSTR B " _
        '    & "ON A.PRM_S_COY_ID=B.CM_COY_ID WHERE PRM_PR_Index=" & intPRIndex
        '    Common.Insert2Ary(strArySql, strSql)

        '    'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift & Analysis Codes.
        '    '//need to check status of the product??
        '    '//need to get latest product info??
        '    'Michelle (16/6/2010) - To include Tax Code
        '    'Stage 3 (Enhancement) (GST-0010) - 13/07/2015 - CH - To include GST Tax Code
        '    strSql = "INSERT INTO PR_DETAILS(PRD_PR_NO,PRD_COY_ID,PRD_PR_LINE,PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE," _
        '    & "PRD_PRODUCT_DESC,PRD_UOM,PRD_ORDERED_QTY,PRD_UNIT_COST,PRD_ETD,PRD_WARRANTY_TERMS,PRD_MIN_ORDER_QTY," _
        '    & "PRD_MIN_PACK_QTY,PRD_REMARK,PRD_GST,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,PRD_D_ADDR_CODE,PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3," _
        '    & "PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY,PRD_ACCT_INDEX,PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_SOURCE,PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE," _
        '    & "PRD_CURRENCY_CODE,PRD_CT_ID,PRD_ASSET_GROUP,PRD_GIFT,PRD_FUND_TYPE,PRD_PERSON_CODE,PRD_PROJECT_CODE) " _
        '    & "SELECT '" & strNewPRNo & "'," _
        '    & "PRD_COY_ID,PRD_PR_LINE,PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE," _
        '    & "PRD_PRODUCT_DESC,PRD_UOM,PRD_ORDERED_QTY,PRD_UNIT_COST,PRD_ETD,PRD_WARRANTY_TERMS,PRD_MIN_ORDER_QTY," _
        '    & "PRD_MIN_PACK_QTY,PRD_REMARK,PRD_GST,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,PRD_D_ADDR_CODE,PRD_D_ADDR_LINE1,PRD_D_ADDR_LINE2,PRD_D_ADDR_LINE3," _
        '    & "PRD_D_POSTCODE,PRD_D_CITY,PRD_D_STATE,PRD_D_COUNTRY,PRD_ACCT_INDEX,PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_SOURCE,PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE, " _
        '    & "PRD_CURRENCY_CODE,PRD_CT_ID,PRD_ASSET_GROUP,PRD_GIFT,PRD_FUND_TYPE,PRD_PERSON_CODE,PRD_PROJECT_CODE " _
        '    & "FROM PR_DETAILS WHERE PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID='" & strCoyID & "'"
        '    Common.Insert2Ary(strArySql, strSql)

        '    '//need to check status of the custom field???
        '    strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR(PCM_PR_INDEX,PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE) SELECT " _
        '    & "(SELECT MAX(PRM_PR_INDEX) FROM PR_MSTR),PCM_FIELD_NO,PCM_FIELD_NAME,PCM_TYPE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX=" & intPRIndex & " AND PCM_TYPE='PR'"
        '    Common.Insert2Ary(strArySql, strSql)

        '    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS(PCD_PR_INDEX,PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE)  SELECT " _
        '    & "(SELECT MAX(PRM_PR_INDEX) FROM PR_MSTR),PCD_PR_LINE,PCD_FIELD_NO,PCD_FIELD_VALUE,PCD_TYPE FROM PR_CUSTOM_FIELD_DETAILS " _
        '    & "WHERE PCD_PR_INDEX=" & intPRIndex & " AND PCD_TYPE='PR'"
        '    Common.Insert2Ary(strArySql, strSql)

        '    strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"
        '    Dim dtAttach As DataTable
        '    Dim dr As DataRow
        '    Dim objFile As New FileManagement
        '    Dim strHubFile, strActualFile As String
        '    Dim strBasePath, strPRPath As String
        '    Dim strSourceFile, strDestFile As String

        '    strBasePath = objFile.getBasePath(EnumUploadFrom.FrontOff)
        '    strPRPath = objFile.getSystemParam("DocAttachPath", "PR")
        '    objFile.checkDirectory(strBasePath & strPRPath)
        '    dtAttach = objDb.FillDs(strSql).Tables(0)
        '    For Each dr In dtAttach.Rows
        '        If Not IsDBNull(dr("CDA_HUB_FILENAME")) Then
        '            strSourceFile = strBasePath & strPRPath & dr("CDA_HUB_FILENAME")
        '            strHubFile = objFile.getLastDocAttachFileName(dr("CDA_HUB_FILENAME"))
        '            strDestFile = strBasePath & strPRPath & strHubFile
        '            'Michelle (17/6/2010) - To solve the problem where attachment is not create when duplicating a PR
        '            'If System.IO.File.Exists(strDestFile) Then
        '            If Not System.IO.File.Exists(strDestFile) Then
        '                System.IO.File.Copy(strSourceFile, strDestFile)
        '            End If
        '            strSql = "INSERT INTO Company_Doc_Attachment(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," _
        '            & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) VALUES('" & strCoyID & "','" &
        '            strNewPRNo & "','PR','" & strHubFile & "','" & Common.Parse(dr("CDA_ATTACH_FILENAME")) &
        '            "'," & dr("CDA_FILESIZE") & ",'" & Common.Parse(dr("CDA_TYPE")) & "')"
        '            Common.Insert2Ary(strArySql, strSql)
        '        End If
        '    Next

        '    objGlobal = Nothing
        '    objFile = Nothing
        '    If objDb.BatchExecute(strArySql) Then
        '        Return "Purchase Requisition Number " & strNewPRNo & " created."
        '    Else
        '        Return "Error occurs. Kindly contact the Administrator to resolve this. "
        '    End If

        '    ' strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR'"
        '    '//return relevant parameter and redirect to Raise PR page
        'End Function

        Function ApprovePR(ByVal companyID As String, ByVal UserID As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer,
        ByVal blnHighestLevel As Boolean, ByVal strConsolidator As String, ByVal strAORemark As String,
        ByVal strBuyer As String, ByVal blnRelief As Boolean, ByVal strApprType As String, Optional ByVal strBC As String = "", Optional ByVal strAPP As String = "") As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strSqlAryLast(0) As String
            Dim strCoyID, strMsg, strPO, strPO1, strVendor, strLoginUser As String
            Dim intPRStatus As Integer
            'Dim strMsg2(0) As String
            Dim strvendorname As String
            Dim strMsg1 As String
            Dim strVendorNameList As String
            Dim arrPOInfo, arrPO As Array
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strPoNo, strVen, strVenList As String
            Dim strPR_Type As String
            Dim strSqlLast As String

            If strApprType = "1" Then
                strApprType = "approved"
            Else
                strApprType = "endorsed"
            End If
            strCoyID = companyID
            strLoginUser = UserID 'is a AO
            '    strSql1 = "SELECT POM_S_COY_NAME FROM PO_MSTR WHERE  POM_B_COY_ID='" & strCoyID & "'"
            '   drdPR1 = objDb.GetReader(strSql1)
            '   If drdPR1.Read Then
            '   strvendorname = drdPR1("POM_S_COY_NAME")
            '    End If
            strSql = "SELECT PRM_PR_STATUS,PRM_STATUS_CHANGED_BY,PRM_S_COY_ID,PRM_S_COY_NAME, PRM_PR_TYPE FROM PR_MSTR WHERE PRM_PR_Index=" & intPRIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intPRStatus = tDS.Tables(0).Rows(0).Item("PRM_PR_STATUS")
                strVendor = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRM_S_COY_ID"))
                If intPRStatus = PRStatus.CancelledBy Then
                    strMsg = "The PR has already been cancelled. Approving of this PR is not allowed. "
                ElseIf intPRStatus = PRStatus.RejectedBy Then 'And Common.parseNull(drdPR("PRM_STATUS_CHANGED_BY")) = strUserID Then
                    strMsg = "You have already rejected this PR. Approving of this PR is not allowed. "
                ElseIf intPRStatus = PRStatus.Approved Then  '//approved '//NEED TO PR-APPROVAL
                    strMsg = "You have already approved/endorsed this PR."
                ElseIf intPRStatus = PRStatus.ConvertedToPO Then
                    strMsg = "You have already converted this PR to PO. Approving of this PR is not allowed."
                End If
            End If
            strCoyID = companyID
            strPR_Type = Common.parseNull(tDS.Tables(0).Rows(0).Item("PRM_PR_TYPE"))
            'For intLoop = 0 To strAryPRNo.GetUpperBound(0)
            '    If intLoop = 0 Then
            '        strAllPR = "'" & strAryPRNo(intLoop) & "'"
            '        strAllPRIndex = strAryPRIndex(intLoop)
            '    Else
            '        strAllPR = strAllPR & ",'" & strAryPRNo(intLoop) & "'"
            '        strAllPRIndex = strAllPRIndex & "," & strAryPRIndex(intLoop)
            '    End If
            'Next

            If (intPRStatus = PRStatus.PendingApproval Or intPRStatus = PRStatus.Submitted) And isApproved(intPRIndex, intCurrentSeq, strLoginUser) Then
                strMsg = "You have already approved/endorsed this PR. Approving of this PR is not allowed."
                Return False
            End If

            If strMsg <> "" Then
                Return strMsg
            End If
            If blnHighestLevel Then
                If strConsolidator = String.Empty And (strPR_Type = "CC") Then
                    '//update PR status to PO created, status_changed_by,status_changed_date
                    '//update PR_Approval
                    '//UPDATE PR_APPROVAL SET AO_REMARK=STRREMARK, ACTION_DATE=TODAY,AO_ACTION=CURRENT SEQ WHERE SEQ=CURRENT SEQ
                    '//UPDATE PR_APPROVAL SET AO_ACTION=CURRENT SEQ WHERE SEQ<>CURRENT SEQ
                    '//Create PO
                    '//send mail to Vendor, requestor   

                    Dim objDB As New EAD.DBCom
                    Dim Owner_Type As String = objDB.GetVal("SELECT IFNULL(COMPANY_MSTR.CM_CONTR_PR_SETTING,'') AS CM_CONTR_PR_SETTING FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "' ")
                    If Owner_Type = "P" Then
                        Dim Owner_ID As String = objDB.GetVal("SELECT IFNULL(COMPANY_MSTR.CM_CONTR_PR_PO_OWNER_ID,'') AS CM_CONTR_PR_PO_OWNER_ID FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "' ")
                        strBuyer = Owner_ID
                    End If

                    'Michelle (19/2/2014) - Issue 2694
                    Dim strInvAppr As String = ""
                    strInvAppr = objDB.GetVal("SELECT CM_INV_APPR FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "'")
                    If strInvAppr = "Y" Then 'Check whether the department is tied with approval flow
                        If objDB.Exist("SELECT '*' FROM USER_MSTR " &
                                    "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID AND CDM_COY_ID = UM_COY_ID " &
                                    "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE UM_USER_ID ='" & strBuyer & "' " &
                                    "AND UM_COY_ID = '" & strCoyID & "'") = 0 Then
                            strMsg = "The Department Invoice Approval List for " & strPRNo & " has not been set up. Please contact the administrator."
                            Return strMsg
                        End If
                    End If

                    updateAOAction(UserID, intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PR")
                    strPO = CreatePO(companyID, UserID, strPRNo, intPRIndex, strVendor, strSqlAry, strBuyer, "", False, Nothing, strAPP, strSqlAryLast)
                    If strPO = "error" Then
                        ' strMsg = Common.MsgTransDup
                        ' Return strMsg
                        Return False
                    End If

                    ' _Yap move down to after batch run
                    ' '' ''--Michelle Start
                    '' ''arrPOInfo = Split(strPO, ",")
                    '' ''intLowBound = LBound(arrPOInfo)
                    '' ''intUpBound = UBound(arrPOInfo)

                    ' '' ''    If UBound(arrPoNo) = 0 Then
                    ' '' ''   strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    ' '' ''      Else
                    '' ''For intLoop = 0 To intUpBound
                    '' ''    'To get the Vendor name
                    '' ''    arrPO = Split(arrPOInfo(intLoop), "!")
                    '' ''    strPoNo = arrPO(0) 'Capture the PO No.
                    '' ''    strVen = arrPO(1)  'Caputre the Vendor code

                    '' ''    strSql1 = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE  CM_COY_ID='" & strVen & "'"
                    '' ''    tDS = objDB.FillDs(strSql1)
                    '' ''    If tDS.Tables(0).Rows.Count > 0 Then
                    '' ''        strvendorname = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                    '' ''    End If
                    '' ''    If intLoop = 0 Then
                    '' ''        strMsg = "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                    '' ''        strVendorNameList = strvendorname
                    '' ''    Else
                    '' ''        strMsg = strMsg & """& vbCrLf & """ & "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                    '' ''        strVendorNameList = strVendorNameList & ", " & strvendorname
                    '' ''    End If

                    '' ''Next
                    '  End If
                    'strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    'strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    'If strMsg.Length > 0 Then
                    '    Dim intLoop, intCnt As Integer
                    '    intCnt = strMsg.Length
                    '    For intLoop = 0 To intCnt - 1
                    '        If intLoop = 0 Then
                    '            strMsg1 = strMsg
                    '            ' strMsg(0) = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                    '        Else
                    '            strMsg1 = strMsg1 & """& vbCrLf & """ & strMsg
                    '        End If
                    '    Next
                    ''''Common.NetMsgbox(Me, strMsg)
                    'End If

                    '//return message = PO created
                Else
                    strSql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
                    Common.Insert2Ary(strSqlAry, strSql)

                    '//update PR status to Approved/Endorsed, status_changed_by,status_changed_date
                    strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.Approved &
                    ", PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_BY='" & strLoginUser & "',PRM_STATUS_CHANGED_ON=" &
                    Common.ConvertDate(Now) & " WHERE PRM_PR_Index=" & intPRIndex

                    Common.Insert2Ary(strSqlAry, strSql)
                    updateAOAction(UserID, intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)

                    strSqlLast = "SELECT 1"
                    Common.Insert2Ary(strSqlAryLast, strSqlLast)

                    '//send mail to consolidator
                    '//return message = PR Approved/endorsed
                    strMsg = "PR No. " & strPRNo & " has been " & strApprType & ". "
                    'Return True
                End If
            Else
                strSql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
                Common.Insert2Ary(strSqlAry, strSql)

                '//update PR status, status_changed_by,status_changed_date
                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS=" & PRStatus.PendingApproval &
                ",PRM_STATUS_CHANGED_BY='" & strLoginUser & "',PRM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & " WHERE PRM_PR_Index=" & intPRIndex

                Common.Insert2Ary(strSqlAry, strSql)
                updateAOAction(UserID, intPRIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief)

                strSqlLast = "SELECT 1"
                Common.Insert2Ary(strSqlAryLast, strSqlLast)
                Return True

                '//notify next AO
                '//return message = PR Approved/endorsed
                strMsg = "PR No. " & strPRNo & " has been " & strApprType & ". "
            End If
            ' Dim objUsers As New Users
            'objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_ApprovePR, strPRNo)
            'objUsers = Nothing

            'If Not objDb.BatchExecute(strSqlAry) Then
            If Not objDb.BatchExecuteNew(strSqlAry, , strPO, "T_NO") Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                Return False
            Else
                If strSqlAryLast(0) <> String.Empty Then
                    If Not objDb.BatchExecute(strSqlAryLast) Then
                        strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
                        Return False
                        'Else
                        ' _Yap: Move to here
                        '--Michelle Start
                        'If strPO <> "" Then
                        '    arrPOInfo = Split(strPO, ",")
                        '    intLowBound = LBound(arrPOInfo)
                        '    intUpBound = UBound(arrPOInfo)

                        '    '    If UBound(arrPoNo) = 0 Then
                        '    '   strMsg = "Purchase Order Number " & strPO & " has been created for " & strvendorname & "."
                        '    '      Else
                        '    For intLoop = 0 To intUpBound
                        '        'To get the Vendor name
                        '        arrPO = Split(arrPOInfo(intLoop), "!")
                        '        strPoNo = arrPO(0) 'Capture the PO No.
                        '        strVen = arrPO(1)  'Caputre the Vendor code

                        '        strSql1 = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE  CM_COY_ID='" & strVen & "'"
                        '        tDS = objDb.FillDs(strSql1)
                        '        If tDS.Tables(0).Rows.Count > 0 Then
                        '            strvendorname = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                        '        End If
                        '        If intLoop = 0 Then
                        '            strMsg = "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                        '            strVendorNameList = strvendorname
                        '        Else
                        '            strMsg = strMsg & """& vbCrLf & """ & "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                        '            strVendorNameList = strVendorNameList & ", " & strvendorname
                        '        End If
                        '    Next
                        'End If
                        'Common.NetMsgbox(Me, strMsg)

                        '//only send mail if transaction successfully created
                    End If
                End If
            End If
            'Return True
            Return strMsg
        End Function
        Sub updateAOAction(ByVal UserID As String, ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByRef pQuery() As String, ByVal blnRelief As Boolean, Optional ByVal pFor As String = "")
            Dim strSql, strLoginUser As String
            strLoginUser = UserID
            'PRA_RELIEF_AO
            If blnRelief Then
                strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",PRA_AO='" & strLoginUser & "',PRA_ON_BEHALFOF=PRA_AO,PRA_ACTIVE_AO='" &
                strLoginUser & "' WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
                If pFor = "PO" Then
                    strSql &= " AND PRA_FOR = 'PO'"
                Else
                    strSql &= " AND PRA_FOR <> 'PO'"
                End If
            Else
                strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",PRA_ACTIVE_AO='" & strLoginUser & "' WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
                If pFor = "PO" Then
                    strSql &= " AND PRA_FOR = 'PO'"
                Else
                    strSql &= " AND PRA_FOR <> 'PO'"
                End If
            End If

            Common.Insert2Ary(pQuery, strSql)
            strSql = "UPDATE PR_APPROVAL SET PRA_AO_ACTION = " & intCurrentSeq & " WHERE PRA_PR_INDEX=" & intPRIndex '& " AND PRA_SEQ=" & intCurrentSeq
            If pFor = "PO" Then
                strSql &= " AND PRA_FOR = 'PO'"
            Else
                strSql &= " AND PRA_FOR <> 'PO'"
            End If
            Common.Insert2Ary(pQuery, strSql)
        End Sub
        'Sub updateAOAction(ByVal intPRIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByRef pQuery() As String, ByVal blnRelief As Boolean, Optional ByVal pFor As String = "")
        '    Dim strSql, strLoginUser As String
        '    strLoginUser = HttpContext.Current.Session("UserId")
        '    'PRA_RELIEF_AO
        '    If blnRelief Then
        '        strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" &
        '        Common.ConvertDate(Now) & ",PRA_AO='" & strLoginUser & "',PRA_ON_BEHALFOF=PRA_AO,PRA_ACTIVE_AO='" &
        '        strLoginUser & "' WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
        '        If pFor = "PO" Then
        '            strSql &= " AND PRA_FOR = 'PO'"
        '        Else
        '            strSql &= " AND PRA_FOR <> 'PO'"
        '        End If
        '    Else
        '        strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" &
        '        Common.ConvertDate(Now) & ",PRA_ACTIVE_AO='" & strLoginUser & "' WHERE PRA_PR_INDEX=" & intPRIndex & " AND PRA_SEQ=" & intCurrentSeq
        '        If pFor = "PO" Then
        '            strSql &= " AND PRA_FOR = 'PO'"
        '        Else
        '            strSql &= " AND PRA_FOR <> 'PO'"
        '        End If
        '    End If

        '    Common.Insert2Ary(pQuery, strSql)
        '    strSql = "UPDATE PR_APPROVAL SET PRA_AO_ACTION = " & intCurrentSeq & " WHERE PRA_PR_INDEX=" & intPRIndex '& " AND PRA_SEQ=" & intCurrentSeq
        '    If pFor = "PO" Then
        '        strSql &= " AND PRA_FOR = 'PO'"
        '    Else
        '        strSql &= " AND PRA_FOR <> 'PO'"
        '    End If
        '    Common.Insert2Ary(pQuery, strSql)
        'End Sub

        Function CreatePOHeader(ByVal strCoyID As String, ByVal strLoginUser As String, ByVal strPRNo As String, ByVal strVendor As String, ByVal strNewPONo As String,
        ByVal strPOPrefix As String, ByRef pQuery() As String, ByVal dblPOCost As Double,
        ByVal strUserID As String, ByVal blnPreviewPO As Boolean, ByVal strTermFile As String,
        Optional ByVal blnConsolidate As Boolean = False, Optional ByVal strConsoRemark As String = "", Optional ByVal strCurr As String = "", Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal strAPP As String = "", Optional ByVal strIntR As String = "", Optional ByVal strExtR As String = "")
            Dim strSql, strAryQuery(0) As String
            Dim strUserName As String, strPhone As String, strFax As String
            Dim strBillMethod, strTable, strCondition, strPayTerm, strPayMethod As String
            Dim dteNow As DateTime = Now()

            '//If got consolidation, Requestor become AO
            Dim objUser As New User
            Dim objUsers As New Users

            If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
            End If

            ' Michelle (CR0012) - To check whether Payment Terms and Payment Methods have been selected
            'strSql = "SELECT ISNULL(PRM_PAYMENT_TERM,'') FROM PR_MSTR WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO ='" & strPRNo & "'"
            'strPayTerm = objDb.GetVal(strSql)

            'strSql = "SELECT ISNULL(PRM_PAYMENT_TYPE,'') FROM PR_MSTR" & _
            '" WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO ='" & strPRNo & "'"
            'strPayMethod = objDb.GetVal(strSql)

            ''Get the default payment term and method
            'If strPayTerm = "" Then
            '    strPayTerm = objDb.GetVal("SELECT CV_PAYMENT_TERM FROM COMPANY_VENDOR" _
            '    & " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")
            'End If

            'If strPayMethod = "" Then
            '    strPayMethod = objDb.GetVal("SELECT CV_PAYMENT_METHOD FROM COMPANY_VENDOR" _
            '& " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")
            'End If

            strPayTerm = objDb.GetVal("SELECT CV_PAYMENT_TERM FROM COMPANY_VENDOR" _
                & " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")

            strPayMethod = objDb.GetVal("SELECT CV_PAYMENT_METHOD FROM COMPANY_VENDOR" _
                        & " WHERE CV_B_COY_ID= '" & strCoyID & "' AND CV_S_COY_ID ='" & strVendor & "'")

            '//If got consolidation, Requestor become AO

            '//Get Billing Method
            '//GetVal = return one skalar value, value returned must not be null
            strSql = "SELECT ISNULL(CV_BILLING_METHOD,'FPO') FROM COMPANY_VENDOR WHERE CV_B_COY_ID='" & strCoyID &
            "' AND CV_S_COY_ID='" & strVendor & "'"
            strBillMethod = objDb.GetVal(strSql)
            If Trim(strBillMethod) = "" Then strBillMethod = "FPO"
            '//Get Billing Method

            '//preview of PR consolidation           
            If blnPreviewPO Then
                strTable = "PO_MSTR_PREVIEW"
            Else
                strTable = "PO_MSTR"
            End If

            If blnConsolidate Then
                strCondition = "'" & Common.Parse(strIntR) & "', '" & Common.Parse(strConsoRemark) & "'"
            Else
                strCondition = "'" & Common.Parse(strIntR) & "', '" & Common.Parse(strExtR) & "'"
            End If

            strSql = "INSERT INTO " & strTable & "(" _
            & "POM_PO_NO,POM_B_COY_ID,POM_BUYER_ID,POM_BUYER_NAME,POM_BUYER_PHONE,POM_BUYER_FAX," _
            & "POM_S_COY_ID,POM_S_COY_NAME,POM_S_ATTN,POM_S_ADDR_LINE1," _
            & "POM_S_ADDR_LINE2,POM_S_ADDR_LINE3,POM_S_POSTCODE,POM_S_CITY," _
            & "POM_S_STATE,POM_S_COUNTRY,POM_S_PHONE,POM_S_FAX,POM_S_EMAIL,POM_PO_DATE,POM_CREATED_DATE," _
            & "POM_FREIGHT_TERMS,POM_PAYMENT_TERM,POM_PAYMENT_METHOD,POM_SHIPMENT_MODE," _
            & "POM_SHIPMENT_TERM,POM_CURRENCY_CODE,POM_EXCHANGE_RATE," _
            & "POM_SHIP_VIA,POM_PO_STATUS,POM_STATUS_CHANGED_BY," _
            & "POM_STATUS_CHANGED_ON,POM_INTERNAL_REMARK, POM_EXTERNAL_REMARK, POM_CREATED_BY, POM_PO_COST," _
            & "POM_BILLING_METHOD,POM_PO_PREFIX,POM_B_ADDR_CODE,POM_B_ADDR_LINE1,POM_B_ADDR_LINE2," _
            & "POM_B_ADDR_LINE3,POM_B_POSTCODE,POM_B_CITY,POM_B_STATE,POM_B_COUNTRY,POM_FULFILMENT," _
            & "POM_DEPT_INDEX,POM_TERMANDCOND, POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SUBMIT_DATE, POM_URGENT) SELECT " & strNewPONo & ",'" & strCoyID & "','" & strUserID & "','" &
            Common.Parse(strUserName) & "','" & strPhone & "','" & strFax & "'," _
            & "'" & strVendor & "',CM_COY_NAME, PRM_S_ATTN, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, " _
            & "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "

            If strAPP = "APP" Then
                strSql &= "" & Common.ConvertDate(dteNow) & ", "
            Else
                strSql &= " NULL, "
            End If


            strSql &= "" & Common.ConvertDate(dteNow) & ", PRM_FREIGHT_TERMS," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PT' AND CODE_ABBR='" & strPayTerm & "'), " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='PM' AND CODE_ABBR='" & strPayMethod & "'), " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='SM' AND CODE_ABBR=PM.PRM_SHIPMENT_MODE)," _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='ST' AND CODE_ABBR=PM.PRM_SHIPMENT_TERM)"

            Dim POStatus As String
            If strItemLine = "" Then
                POStatus = POStatus_new.NewPO
                ' strSql &= ",PRM_CURRENCY_CODE,PRM_EXCHANGE_RATE,PRM_SHIP_VIA,'" & _
                strSql &= ",'" & strCurr & "',PRM_EXCHANGE_RATE,PRM_SHIP_VIA,'" &
                        POStatus & "','" & strLoginUser & "'," & Common.ConvertDate(dteNow)
            Else
                POStatus = POStatus_new.Draft
                strSql &= ",'" & strCurr & "',PRM_EXCHANGE_RATE,PRM_SHIP_VIA,'" &
                        POStatus & "','" & strLoginUser & "'," & Common.ConvertDate(dteNow)
            End If

            strSql &= "," & strCondition & ",'" & strLoginUser & "'," & dblPOCost & ",'" & strBillMethod & "'," & strPOPrefix & ",PRM_B_ADDR_CODE, " _
            & "PRM_B_ADDR_LINE1,PRM_B_ADDR_LINE2,PRM_B_ADDR_LINE3,PRM_B_POSTCODE,PRM_B_CITY," _
            & "PRM_B_STATE,PRM_B_COUNTRY," & Fulfilment.null & ",PRM_DEPT_INDEX,'" & strTermFile & "', PRM_PRINT_CUSTOM_FIELDS, PRM_PRINT_REMARK," & Common.ConvertDate(dteNow) & ", PRM_URGENT FROM PR_MSTR PM, (SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Common.Parse(strVendor) & "') AS CM WHERE PRM_COY_ID='" & strCoyID &
            "' AND PRM_PR_No='" & strPRNo & "'"
            Common.Insert2Ary(pQuery, strSql)

            If Not blnPreviewPO Then
                Dim objTrack As New Tracking
                strSql = objTrack.updateDocMatchingNew(strNewPONo, "", "", "PO", strVendor, strCoyID)
                Common.Insert2Ary(pQuery, strSql)
                objTrack = Nothing
            End If

            objUsers = Nothing
            objUser = Nothing
            '//update RFQ_Replies
        End Function

        Function CreatePODetail(ByVal strCoyID As String, ByVal strLoginUser As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByVal strNewPONo As String,
        ByRef pQuery() As String, ByRef intPOLine As Integer, ByVal strUserID As String, Optional ByVal blnPreviewPO As Boolean = False, Optional ByVal strCurr As String = "", Optional ByVal strAddr As String = "", Optional ByVal strItemLine As String = "", Optional ByVal strAPP As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strSql, strTable As String


            Dim objUsers As New Users
            Dim i As Integer

            If blnPreviewPO Then
                strTable = "PO_DETAILS_PREVIEW"
            Else
                strTable = "PO_DETAILS"
            End If

            '//preview of PR consolidation
            '//not suppose to update PR because no actual PO created
            If Not blnPreviewPO Then
                'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                '& "PRM_PR_STATUS = " & PRStatus.ConvertedToPO & ",PRM_STATUS_CHANGED_BY='" & strLoginUser & _
                '"', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
                'Common.Insert2Ary(pQuery, strSql)

                strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                        & "PRM_STATUS_CHANGED_BY='" & strLoginUser &
                        "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
                Common.Insert2Ary(pQuery, strSql)


                'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
                'Common.Insert2Ary(pQuery, strSql)
            End If

            If strItemLine = "" Then
                'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift and Analysis Codes.
                'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH - Include Gst Tax Code
                strSql = "set @num  = 0; INSERT INTO " & strTable & "(" _
                    & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                    & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                    & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                    & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_GST_RATE,POD_GST_INPUT_TAX_CODE,POD_TAX_VALUE,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                    & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                    & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_GIFT,POD_FUND_TYPE,POD_PERSON_CODE,POD_PROJECT_CODE) SELECT " &
                    strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                    & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PRD_UNIT_COST,PRD_REMARK,PRD_GST,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,(ROUND(ROUND(PRD_ORDERED_QTY*PRD_UNIT_COST,2)*PRD_GST/100,2))," &
                    intPRIndex & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                    & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                    & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO,PRD_GIFT,PRD_FUND_TYPE,PRD_PERSON_CODE,PRD_PROJECT_CODE FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID &
                    "' AND PRD_PR_NO='" & strPRNo & "' "
            Else
                'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift and Analysis Codes.
                'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH - Include Gst Tax Code
                strSql = "set @num  = 0; INSERT INTO " & strTable & "(" _
                    & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                    & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                    & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                    & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_GST_RATE,POD_GST_INPUT_TAX_CODE,POD_TAX_VALUE,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                    & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                    & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_GIFT,POD_FUND_TYPE,POD_PERSON_CODE,POD_PROJECT_CODE) SELECT " &
                    strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                    & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PM_LAST_TXN_PRICE,PRD_REMARK,PM_LAST_TXN_TAX,PRD_GST_RATE,PRD_GST_INPUT_TAX_CODE,IF(PRD_GST IS NULL,0,(ROUND(ROUND(PRD_ORDERED_QTY*PRD_UNIT_COST,2)*PRD_GST/100,2))) AS PRD_GST," &
                    intPRIndex & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                    & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                    & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO,PRD_GIFT,PRD_FUND_TYPE,PRD_PERSON_CODE,PRD_PROJECT_CODE " _
                    & "FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID &
                    "' AND PRD_PR_NO='" & strPRNo & "' "
            End If

            If strVendor <> "" Then
                strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
            End If

            If strCurr <> "" Then
                strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
            End If

            If strAddr <> "" Then
                strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
            End If

            strSql &= " ORDER BY PRD_PR_LINE; set @num  = 0;"

            Common.Insert2Ary(pQuery, strSql)

            Dim ds As New DataSet
            'If blnEnterpriseVersion = True Then
            '    strSql = "SELECT PRD_PR_NO, PRD_PR_LINE, PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_PR_NO = '" & strPRNo & "' "
            '    strSql &= "AND (PRD_ASSET_GROUP IS NOT NULL AND PRD_ASSET_GROUP <> '') AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '    ds = objDb.FillDs(strSql)

            '    Dim strAssetNo As String
            '    Dim intAssetIncrementNo As Integer
            '    intAssetIncrementNo = 1
            '    For i = 0 To ds.Tables(0).Rows.Count - 1
            '        Common.parseNull(ds.Tables(0).Rows(i)("PRD_ASSET_GROUP"))

            '        Dim strParam As String = objDb.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            '        If strParam = "" Then
            '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
            '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '            strSql &= "'Prefix', "
            '            strSql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
            '            strSql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
            '            strSql &= "'eProcure' ) "
            '            Common.Insert2Ary(pQuery, strSql)

            '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
            '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
            '            strSql &= "'Last Used No', "
            '            strSql &= "'00000000', "
            '            strSql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
            '            strSql &= "'eProcure' ) "
            '            Common.Insert2Ary(pQuery, strSql)
            '        End If

            '        strAssetNo = " (SELECT CAST( CONCAT(RIGHT(YEAR(CURRENT_DATE()),2), " _
            '                    & " CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), " _
            '                    & " (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')))  AS CHAR(1000)) AS CP_PARAM_VALUE FROM (SELECT * FROM COMPANY_PARAM " _
            '                    & " WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' AND CP_PARAM_NAME = 'Last Used No') ZZZ )"

            '        strSql = "UPDATE PO_DETAILS SET "
            '        strSql &= "POD_ASSET_NO = " & strAssetNo & " "
            '        strSql &= "WHERE POD_PO_NO = " & strNewPONo & " "
            '        strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            '        strSql &= "AND POD_PO_LINE = '" & ds.Tables(0).Rows(i)("PRD_PR_LINE") & "' "
            '        Common.Insert2Ary(pQuery, strSql)

            '        strSql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' "
            '        Common.Insert2Ary(pQuery, strSql)

            '        'intAssetIncrementNo += 1
            '    Next
            'End If

            If strItemLine = "" Then
                strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser &
                     "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_NO = '" & strPRNo & "'"
                Common.Insert2Ary(pQuery, strSql)

                'strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                '    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                '    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                '    "PRD_GST = PM_LAST_TXN_TAX, " & _
                '    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                '    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & strPRNo & "' " & _
                '    "AND PM_S_COY_ID = PRD_COY_ID " & _
                '    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                'Common.Insert2Ary(pQuery, strSql)
            Else
                strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser &
                     "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "'"
                Common.Insert2Ary(pQuery, strSql)

                'strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " & _
                '    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " & _
                '    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " & _
                '    "PRD_GST = PM_LAST_TXN_TAX, " & _
                '    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " & _
                '    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " & _
                '    "AND PM_S_COY_ID = PRD_COY_ID " & _
                '    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                'Common.Insert2Ary(pQuery, strSql)
            End If

            'If strItemLine = "" Then
            '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
            '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & strPRNo & "'"
            '    Common.Insert2Ary(pQuery, strSql)
            'Else
            '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
            '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "'"
            '    Common.Insert2Ary(pQuery, strSql)
            'End If

            'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID='" & Common.Parse(strCoyID) & "') = 0"
            'Common.Insert2Ary(pQuery, strSql)

            'Update last transacted price
            If strAPP = "APP" Then
                strSql = "UPDATE PRODUCT_MSTR, PO_DETAILS, PO_MSTR " &
                                    "SET PM_LAST_TXN_PRICE = POD_UNIT_COST, " &
                                    "PM_LAST_TXN_PRICE_CURR = POM_CURRENCY_CODE, " &
                                    "PM_LAST_TXN_TAX = POD_GST, " &
                                    "PM_LAST_TXN_S_COY_ID = '" & strVendor & "' " &
                                    "WHERE POM_PO_NO = " & strNewPONo & " AND POM_B_COY_ID = POD_COY_ID " &
                                    "AND POM_PO_NO = POD_PO_NO AND PM_S_COY_ID = POM_B_COY_ID " &
                                    "AND POD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(pQuery, strSql)
            End If

            ' Here new enhancement on PO Custom will be create from the PR Custom Field.
            ' 29/Aug/2011
            ' Create PO Custom Field from PR Custom Field
            ' PR_CUSTOM_FIELD_DETAILS table
            ' _Yap Backup this portion
            'If strItemLine = "" Then
            '    strSql = "set @num  = 1; SET @PCD_FIELD_NO = 0;INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
            '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
            '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
            '        & "@num := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num, @num+1) AS d4, " _
            '        & "@num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num, 1) AS LINE_NO, " _
            '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
            '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
            '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
            '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
            '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
            '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
            '        "' AND PRD_PR_NO='" & strPRNo & "' "
            'Else
            '    strSql = "set @num  = 1; SET @PCD_FIELD_NO = 0;INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
            '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
            '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
            '        & "@num := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num, @num+1) AS d4, " _
            '        & "@num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num, 1) AS LINE_NO, " _
            '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
            '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
            '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
            '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
            '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
            '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & _
            '        "' AND PRD_PR_NO='" & strPRNo & "' "
            'End If

            If strItemLine = "" Then
                strSql = "set @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                    & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                    & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                    & "@num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, " _
                    & "@num AS LINE_NO, " _
                    & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                    & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                    & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                    & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                    & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                    & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID &
                    "' AND PRD_PR_NO='" & strPRNo & "' "

                '& "IF(@PCD_FIELD_NO=1,@num,PCD_PR_LINE - IF(@PCD_FIELD_NO=0,(PCD_PR_LINE-1),0)) AS LINE_NO, " _

            Else
                strSql = "set @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                    & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                    & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                    & "@num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, " _
                    & "@num AS LINE_NO, " _
                    & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                    & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                    & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                    & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                    & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                    & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID &
                    "' AND PRD_PR_NO='" & strPRNo & "' "

                '& "IF(@PCD_FIELD_NO=1,@num,PCD_PR_LINE - IF(@PCD_FIELD_NO=0,(PCD_PR_LINE-1),0)) AS LINE_NO, " _

            End If

            If strVendor <> "" Then
                strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
            End If
            If strCurr <> "" Then
                strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
            End If
            If strAddr <> "" Then
                strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
            End If
            strSql &= " ORDER BY PRD_PR_LINE, PCD_FIELD_NO) XXX) ZZZ; set @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
            Common.Insert2Ary(pQuery, strSql)

            ' Here new enhancement on PO Custom will be create from the PR Custom Field.
            ' 29/Aug/2011
            ' Create PO Custom Field from PR Custom Field
            ' PR_CUSTOM_FIELD_MSTR table
            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (" _
            & "PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT " _
            & "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), PCM_FIELD_NO, PCM_FIELD_NAME, 'PO' " _
            & "FROM PR_MSTR PM, PR_CUSTOM_FIELD_MSTR WHERE PRM_PR_INDEX = PCM_PR_INDEX AND PRM_COY_ID='" & strCoyID &
            "' AND PRM_PR_No='" & strPRNo & "' AND PCM_TYPE = 'PR'"
            Common.Insert2Ary(pQuery, strSql)

            '//for PR Consolidation
            Dim intCnt As Integer = 0
            If strVendor <> "" Then
                intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID &
                    "' AND PRD_PR_NO='" & strPRNo & "' AND PRD_S_COY_ID = '" & strVendor & "'")
            Else
                intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID &
                    "' AND PRD_PR_NO='" & strPRNo & "' ")
            End If

            intPOLine += intCnt

            If Not blnPreviewPO Then
                objUsers.Log_UserActivityNew(strCoyID, strUserID, pQuery, WheelModule.PRMod, WheelUserActivity.AO_CreatePO, strNewPONo, strPRNo)
            End If
            objUsers = Nothing
        End Function

        Function CreatePODetailMulti(ByVal strLoginUser As String, ByVal strCoyID As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByVal strNewPONo As String,
        ByRef pQuery() As String, ByRef intPOLine As Integer, ByVal strUserID As String, Optional ByVal blnPreviewPO As Boolean = False, Optional ByVal strCurr As String = "", Optional ByVal strAddr As String = "", Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal strAPP As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strSql, strTable As String

            Dim objUsers As New Users
            Dim i, j As Integer

            If blnPreviewPO Then
                strTable = "PO_DETAILS_PREVIEW"
            Else
                strTable = "PO_DETAILS"
            End If

            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)
            strSql = " SET @num2  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
            Common.Insert2Ary(pQuery, strSql)

            For i = 0 To PRList.Count - 1
                '//preview of PR consolidation
                '//not suppose to update PR because no actual PO created
                If Not blnPreviewPO Then
                    'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                    '& "PRM_PR_STATUS = " & PRStatus.ConvertedToPO & ",PRM_STATUS_CHANGED_BY='" & strLoginUser & _
                    '"', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
                    'Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
                            & "PRM_STATUS_CHANGED_BY='" & strLoginUser &
                            "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & PRList(i)(1)
                    Common.Insert2Ary(pQuery, strSql)



                    'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
                    'Common.Insert2Ary(pQuery, strSql)
                End If

                If strItemLine = "" Then
                    'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift & Analysis Codes.
                    strSql = " INSERT INTO " & strTable & "(" _
                        & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                        & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                        & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                        & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                        & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                        & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_GIFT,POD_FUND_TYPE,POD_PERSON_CODE,POD_PROJECT_CODE) SELECT " &
                        strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                        & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PRD_UNIT_COST,PRD_REMARK,PRD_GST," &
                        PRList(i)(1) & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                        & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                        & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO,PRD_GIFT,PRD_FUND_TYPE,PRD_PERSON_CODE,PRD_PROJECT_CODE FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID &
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "

                Else
                    'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift & Analysis Codes.
                    strSql = " INSERT INTO " & strTable & "(" _
                        & "POD_PO_NO,POD_COY_ID,POD_PO_LINE,POD_PRODUCT_CODE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_DESC," _
                        & "POD_UOM,POD_ORDERED_QTY,POD_RECEIVED_QTY,POD_REJECTED_QTY,POD_DELIVERED_QTY," _
                        & "POD_CANCELLED_QTY,POD_MIN_PACK_QTY,POD_MIN_ORDER_QTY,POD_ETD,POD_WARRANTY_TERMS," _
                        & "POD_UNIT_COST,POD_REMARK,POD_GST,POD_PR_INDEX,POD_PR_LINE,POD_ACCT_INDEX,POD_PRODUCT_TYPE, " _
                        & "POD_B_ITEM_CODE,POD_SOURCE,POD_D_ADDR_CODE,POD_D_ADDR_LINE1,POD_D_ADDR_LINE2," _
                        & "POD_D_ADDR_LINE3,POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_ASSET_GROUP,POD_ASSET_NO,POD_GIFT,POD_FUND_TYPE,POD_PERSON_CODE,POD_PROJECT_CODE) SELECT " &
                        strNewPONo & ",'" & strCoyID & "', @num := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num + 1, 1), PRD_PRODUCT_CODE,PRD_VENDOR_ITEM_CODE,PRD_PRODUCT_DESC,PRD_UOM," _
                        & "PRD_ORDERED_QTY,0,0,0,0,1,1,PRD_ETD,PRD_WARRANTY_TERMS,PM_LAST_TXN_PRICE,PRD_REMARK,PM_LAST_TXN_TAX," &
                        PRList(i)(1) & ",PRD_PR_LINE,PRD_ACCT_INDEX, PRD_PRODUCT_TYPE,PRD_B_ITEM_CODE,PRD_SOURCE,PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1," _
                        & "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, " _
                        & "PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY,PRD_B_CATEGORY_CODE,PRD_B_GL_CODE,PRD_ASSET_GROUP,PRD_ASSET_NO,PRD_GIFT,PRD_FUND_TYPE,PRD_PERSON_CODE,PRD_PROJECT_CODE " _
                        & "FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID &
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "
                End If

                If strVendor <> "" Then
                    strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
                End If

                If strCurr <> "" Then
                    strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
                End If

                If strAddr <> "" Then
                    strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
                End If

                strSql &= " ORDER BY PRD_PR_LINE;"
                Common.Insert2Ary(pQuery, strSql)

                Dim ds As New DataSet
                'If blnEnterpriseVersion = True Then
                '    strSql = "SELECT PRD_PR_NO, PRD_PR_LINE, PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_PR_NO = '" & strPRNo & "' "
                '    strSql &= "AND (PRD_ASSET_GROUP IS NOT NULL AND PRD_ASSET_GROUP <> '') AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                '    ds = objDb.FillDs(strSql)

                '    Dim strAssetNo As String
                '    Dim intAssetIncrementNo As Integer
                '    intAssetIncrementNo = 1
                '    For j = 0 To ds.Tables(0).Rows.Count - 1
                '        Common.parseNull(ds.Tables(0).Rows(j)("PRD_ASSET_GROUP"))

                '        Dim strParam As String = objDb.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                '        If strParam = "" Then
                '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                '            strSql &= "'Prefix', "
                '            strSql &= "'" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "', "
                '            strSql &= "'" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "', "
                '            strSql &= "'eProcure' ) "
                '            Common.Insert2Ary(pQuery, strSql)

                '            strSql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                '            strSql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                '            strSql &= "'Last Used No', "
                '            strSql &= "'00000000', "
                '            strSql &= "'" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "', "
                '            strSql &= "'eProcure' ) "
                '            Common.Insert2Ary(pQuery, strSql)
                '        End If

                '        strAssetNo = " (SELECT CAST( CONCAT(RIGHT(YEAR(CURRENT_DATE()),2), " _
                '                    & " CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), " _
                '                    & " (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')))  AS CHAR(1000)) AS CP_PARAM_VALUE FROM (SELECT * FROM COMPANY_PARAM " _
                '                    & " WHERE CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "' AND CP_PARAM_NAME = 'Last Used No') ZZZ )"

                '        strSql = "UPDATE PO_DETAILS SET "
                '        strSql &= "POD_ASSET_NO = " & strAssetNo & " "
                '        strSql &= "WHERE POD_PO_NO = " & strNewPONo & " "
                '        strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                '        strSql &= "AND POD_PO_LINE = '" & ds.Tables(0).Rows(j)("PRD_PR_LINE") & "' "
                '        Common.Insert2Ary(pQuery, strSql)

                '        strSql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(j)("PRD_ASSET_GROUP") & "' "
                '        Common.Insert2Ary(pQuery, strSql)

                '        'intAssetIncrementNo += 1
                '    Next
                'End If

                If strItemLine = "" Then
                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser &
                         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
                    "PRD_GST = PM_LAST_TXN_TAX, " &
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "' " &
                    "AND PM_S_COY_ID = PRD_COY_ID " &
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)

                Else
                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser &
                         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
                    "PRD_GST = PM_LAST_TXN_TAX, " &
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "' " &
                    "AND PM_S_COY_ID = PRD_COY_ID " &
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)
                End If

                'If strItemLine = "" Then
                '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                '    Common.Insert2Ary(pQuery, strSql)
                'Else
                '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'PO', PRD_CONVERT_BY_ID = '" & strLoginUser & _
                '         "', PRD_CONVERT_TO_DOC = " & strNewPONo & ", PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'"
                '    Common.Insert2Ary(pQuery, strSql)
                'End If

                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & PRList(i)(1) & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & PRList(i)(0) & "' AND PRD_COY_ID='" & Common.Parse(strCoyID) & "') = 0"
                Common.Insert2Ary(pQuery, strSql)

                'Update last transacted price
                If strAPP = "APP" Then
                    strSql = "UPDATE PRODUCT_MSTR, PO_DETAILS, PO_MSTR " &
                                            "SET PM_LAST_TXN_PRICE = POD_UNIT_COST, " &
                                            "PM_LAST_TXN_PRICE_CURR = POM_CURRENCY_CODE, " &
                                            "PM_LAST_TXN_TAX = POD_GST, " &
                                            "PM_LAST_TXN_S_COY_ID = '" & strVendor & "' " &
                                            "WHERE POM_PO_NO = " & strNewPONo & " AND POM_B_COY_ID = POD_COY_ID " &
                                            "AND POM_PO_NO = POD_PO_NO AND PM_S_COY_ID = POM_B_COY_ID " &
                                            "AND POD_PRODUCT_CODE = PM_PRODUCT_CODE; "
                    Common.Insert2Ary(pQuery, strSql)
                End If

                ' Here new enhancement on PO Custom will be create from the PR Custom Field.
                ' 29/Aug/2011
                ' Create PO Custom Field from PR Custom Field
                ' PR_CUSTOM_FIELD_DETAILS table
                ' _Yap Backup this portion
                'If strItemLine = "" Then
                '    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                '        & "@num2 := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num2, @num2+1) AS d4, " _
                '        & "@num2 := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num2, 1) AS LINE_NO, " _
                '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
                '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
                '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' "
                'Else
                '    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                '        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                '        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                '        & "@num2 := IF((@PCD_FIELD_NO < PCD_FIELD_NO), @num2, @num2+1) AS d4, " _
                '        & "@num2 := IF(PRD_S_COY_ID = '" & strVendor & "' AND PRD_CURRENCY_CODE ='" & strCurr & "', @num2, 1) AS LINE_NO, " _
                '        & "@PCD_FIELD_NO := PCD_FIELD_NO AS VPCD_FIELD_NO, " _
                '        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                '        & "FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                '        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND " _
                '        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                '        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' "
                'End If

                If strItemLine = "" Then
                    If i > 0 Then
                        If Left(PRList(i)(2), 3) = Left(PRList(i - 1)(2), 3) Then
                            strSql = " SET @num2  = IF(@PCD_PR_LINE = @num2, @num2 + 1, IF(@PCD_PR_LINE < @num2, @num2 + 1,@PCD_PR_LINE)); "
                            Common.Insert2Ary(pQuery, strSql)

                            strSql = " SET @PCD_PR_LINE = 1; "
                            Common.Insert2Ary(pQuery, strSql)
                        End If
                    End If

                    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                        & "@num2 := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num2 + 1, @num2) AS d4, " _
                        & "@num2 AS LINE_NO, " _
                        & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                        & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID ='" & strCoyID &
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "

                Else
                    If i > 0 Then
                        If Left(PRList(i)(2), 3) = Left(PRList(i - 1)(2), 3) Then
                            strSql = " SET @num2  = IF(@PCD_PR_LINE = @num2, @num2 + 1, IF(@PCD_PR_LINE < @num2, @num2 + 1,@PCD_PR_LINE)); "
                            Common.Insert2Ary(pQuery, strSql)

                            strSql = " SET @PCD_PR_LINE = 1; "
                            Common.Insert2Ary(pQuery, strSql)
                        End If
                    End If
                    strSql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (" _
                        & "PCD_PR_INDEX, PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE,PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM (SELECT " _
                        & "(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR) AS PCD_PR_INDEX, " _
                        & "@num2 := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num2 + 1, @num2) AS d4, " _
                        & "@num2 AS LINE_NO, " _
                        & "@PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, " _
                        & "PCD_FIELD_NO, PCD_FIELD_VALUE, 'PO' AS PCD_TYPE " _
                        & "FROM ( SELECT * FROM PR_DETAILS, PR_MSTR, PR_CUSTOM_FIELD_MSTR, PR_CUSTOM_FIELD_DETAILS " _
                        & "WHERE PCM_PR_INDEX = PCD_PR_INDEX AND PCD_FIELD_NO=PCM_FIELD_NO AND PCD_TYPE = 'PR' AND PCM_TYPE = 'PR' AND " _
                        & "PRM_PR_INDEX = PCM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE AND " _
                        & "PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID &
                        "' AND PRD_PR_NO='" & PRList(i)(0) & "' "

                    '& "IF(@PCD_FIELD_NO=1,@num2,PCD_PR_LINE - IF(@PCD_FIELD_NO=0,(PCD_PR_LINE-1),0)) AS LINE_NO, " _

                End If

                If strVendor <> "" Then
                    strSql &= " AND PRD_S_COY_ID ='" & strVendor & "' "
                End If
                If strCurr <> "" Then
                    strSql &= " AND PRD_CURRENCY_CODE ='" & strCurr & "' "
                End If
                If strAddr <> "" Then
                    strSql &= " AND PRM_B_ADDR_CODE ='" & strAddr & "' "
                End If
                strSql &= " ORDER BY PRD_PR_LINE, PCD_FIELD_NO) XXX) ZZZ;"
                Common.Insert2Ary(pQuery, strSql)

                '//for PR Consolidation
                'Dim intCnt As Integer = 0
                'If strVendor <> "" Then
                '    intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' AND PRD_S_COY_ID = '" & strVendor & "'")
                'Else
                '    intCnt = objDb.GetCount("PR_DETAILS", "WHERE PRD_COY_ID ='" & strCoyID & _
                '        "' AND PRD_PR_NO='" & strPRNo & "' ")
                'End If
            Next
            'intPOLine += intCnt
            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)
            strSql = " SET @num2  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
            Common.Insert2Ary(pQuery, strSql)

            ' PR_CUSTOM_FIELD_MSTR table
            strSql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (" _
            & "PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT " _
            & "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), PCM_FIELD_NO, PCM_FIELD_NAME, 'PO' " _
            & "FROM PR_MSTR PM, PR_CUSTOM_FIELD_MSTR WHERE PRM_PR_INDEX = PCM_PR_INDEX AND PRM_COY_ID='" & strCoyID &
            "' AND PRM_PR_No='" & strPRNo & "' AND PCM_TYPE = 'PR'"
            Common.Insert2Ary(pQuery, strSql)

            If Not blnPreviewPO Then
                objUsers.Log_UserActivityNew(strCoyID, strUserID, pQuery, WheelModule.PRMod, WheelUserActivity.AO_CreatePO, strNewPONo, strPRNo)
            End If
            objUsers = Nothing
        End Function
        Function CreateRFQ(ByVal CompanyID As String, ByVal userID As String, ByVal strPRNo As String, intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True) As String
            Dim strCoyID, strLoginUser, strSql, strNewRFQNo, strRFQPrefix As String
            Dim strTermFile, strAttachIndex As String
            Dim intIncrementNo As Integer = 0
            Dim objGlobal As New AppGlobals
            Dim strAryQuery(0) As String
            Dim dteNow As DateTime = Now()
            Dim strUserName As String, strPhone As String, strFax As String, strEmail As String
            Dim strNewRFQNoList As String

            strCoyID = CompanyID
            strLoginUser = userID

            intIncrementNo += 1
            objGlobal.GetLatestDocNo(CompanyID, "RFQ", pQuery, strNewRFQNo, strRFQPrefix, , intIncrementNo)

            If strNewRFQNoList = "" Then
                strNewRFQNoList = strNewRFQNo & "!" & strVendor
            Else
                strNewRFQNoList &= "," & strNewRFQNo & "!" & strVendor
            End If

            strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_NO='" & strNewRFQNo & "' AND RM_COY_ID='" & strCoyID & "'"
            If objDb.Exist(strSql) Then
                Return "error"
            End If

            objGlobal = Nothing

            Dim objUser As New User
            Dim objUsers As New Users

            If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
                strEmail = objUser.Email
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
                strEmail = ""
            End If

            Dim txtDate1 As Date = CType(dteNow, Date)

            strSql = " INSERT INTO RFQ_MSTR " &
                    " (RM_PREFIX,RM_RFQ_NO,RM_COY_ID,RM_RFQ_NAME, " &
                    " RM_EXPIRY_DATE,RM_CREATED_BY,RM_CREATED_ON, " &
                    " RM_CONTACT_PERSON,RM_CONTACT_NUMBER,RM_EMAIL, " &
                    " RM_REQD_QUOTE_VALIDITY,RM_RFQ_OPTION,RM_STATUS,RM_B_DISPLAY_STATUS, " &
                    " RM_PAYMENT_TYPE, RM_PAYMENT_TERM, RM_REMARK) " &
                    " SELECT '" & strRFQPrefix & "', '" & strNewRFQNo & "', '" & CompanyID & "', '', " &
                    " " & Common.ConvertDate(txtDate1.AddDays(3)) & ", '" & strUserID & "', " & Common.ConvertDate(dteNow) & ", " &
                    " '" & Common.Parse(strUserName) & "', '" & strPhone & "', '" & strEmail & "', " &
                    " " & Common.ConvertDate(txtDate1.AddDays(4)) & ", (SELECT (CASE WHEN CM_RFQ_OPTION = 2 THEN 0 ELSE CM_RFQ_OPTION END) AS CM_RFQ_OPTION FROM COMPANY_MSTR WHERE CM_COY_ID = '" & CompanyID & "'), '3', '0', " &
                    " PRM_PAYMENT_TYPE, PRM_PAYMENT_TERM, PRM_EXTERNAL_REMARK" &
                    " FROM PR_MSTR PM WHERE PRM_COY_ID='" & CompanyID & "' AND PRM_PR_No='" & strPRNo & "'"
            Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
            '    & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
            '    "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
            'Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
            'Common.Insert2Ary(pQuery, strSql)

            'Michelle (16/9/2011) - To include the Warranty term (871)
            strSql = " set @num  = 0; INSERT INTO RFQ_DETAIL " &
                    " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " &
                    " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " &
                    " RD_UOM,RD_DELIVERY_LEAD_TIME, " &
                    " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS ) " &
                    " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " &
                    " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " &
                    " PRD_UOM, PR_DETAILS.PRD_ETD, " &
                    " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS " &
                    " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & "'" &
                    " AND PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID = PRM_COY_ID "

            strSql &= " ORDER BY PRD_PR_LINE; set @num  = 0;"
            Common.Insert2Ary(pQuery, strSql)

            strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
                    "PRD_GST = PM_LAST_TXN_TAX, " &
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " &
                    "AND PM_S_COY_ID = PRD_COY_ID " &
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
            Common.Insert2Ary(pQuery, strSql)

            strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser &
                     "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo &
                     "' AND PRD_COY_ID = '" & Common.Parse(strCoyID) & "' "
            Common.Insert2Ary(pQuery, strSql)

            Dim objWheelFile As New FileManagement
            'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)
            strAttachIndex = objWheelFile.copyPRAttachToRFQ(strPRNo, strNewRFQNo)
            If strAttachIndex <> "" Then
                'If blnEnterpriseVersion = True Then
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'Else
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'End If

                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
                    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"

                Common.Insert2Ary(pQuery, strSql)
            End If

            Return strNewRFQNoList
        End Function
        ''//Create PO without consolidation
        ''//to be called by function ApprovePR

        Function CreateRFQMulti(ByVal companyID As String, ByVal userID As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal blnEnterpriseVersion As Boolean = True) As String
            Dim strCoyID, strLoginUser, strSql, strNewRFQNo, strRFQPrefix As String
            Dim strTermFile, strAttachIndex As String
            Dim intIncrementNo As Integer = 0
            Dim objGlobal As New AppGlobals
            Dim strAryQuery(0) As String
            Dim dteNow As DateTime = Now()
            Dim strUserName As String, strPhone As String, strFax As String, strEmail As String
            Dim strNewRFQNoList As String

            strCoyID = companyID
            strLoginUser = userID

            intIncrementNo += 1
            objGlobal.GetLatestDocNo(companyID, "RFQ", pQuery, strNewRFQNo, strRFQPrefix, , intIncrementNo)

            If strNewRFQNoList = "" Then
                strNewRFQNoList = strNewRFQNo & "!" & strVendor
            Else
                strNewRFQNoList &= "," & strNewRFQNo & "!" & strVendor
            End If

            strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_NO='" & strNewRFQNo & "' AND RM_COY_ID='" & strCoyID & "'"

            If objDb.Exist(strSql) Then
                Return "error"
            End If

            objGlobal = Nothing

            Dim objUser As New User
            Dim objUsers As New Users

            If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
                strUserName = objUser.Name
                strPhone = objUser.PhoneNo
                strFax = objUser.FaxNo
                strEmail = objUser.Email
            Else
                strUserName = ""
                strPhone = ""
                strFax = ""
                strEmail = ""
            End If

            Dim txtDate1 As Date = CType(dteNow, Date)
            Dim strSQLPR, strSQLPR2 As String
            Dim strRemark As String

            Dim i As Integer
            For i = 0 To PRList.Count - 1
                If strSQLPR = "" Then
                    strSQLPR = "'" + PRList(i)(0) + "'"
                Else
                    strSQLPR = strSQLPR + ",'" + PRList(i)(0) + "'"
                End If

            Next

            strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & companyID & "' "
            strSQLPR2 &= " AND (PRM_PR_NO IN (" & strSQLPR & ")) "
            strRemark = objDb.GetVal(strSQLPR2)

            Dim objWheelFile As New FileManagement
            'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)
            strAttachIndex = objWheelFile.copyPRAttachToRFQMulti(strSQLPR, strNewRFQNo)
            If strAttachIndex <> "" Then
                'If blnEnterpriseVersion = True Then
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'Else
                '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
                '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
                '    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                'End If

                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
                    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
                    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"

                Common.Insert2Ary(pQuery, strSql)
            End If

            strSql = " INSERT INTO RFQ_MSTR " &
                    " (RM_PREFIX,RM_RFQ_NO,RM_COY_ID,RM_RFQ_NAME, " &
                    " RM_EXPIRY_DATE,RM_CREATED_BY,RM_CREATED_ON, " &
                    " RM_CONTACT_PERSON,RM_CONTACT_NUMBER,RM_EMAIL, " &
                    " RM_REQD_QUOTE_VALIDITY,RM_RFQ_OPTION,RM_STATUS,RM_B_DISPLAY_STATUS, " &
                    " RM_PAYMENT_TYPE, RM_PAYMENT_TERM, RM_REMARK) " &
                    " SELECT '" & strRFQPrefix & "', '" & strNewRFQNo & "', '" & companyID & "', '', " &
                    " " & Common.ConvertDate(txtDate1.AddDays(3)) & ", '" & strUserID & "', " & Common.ConvertDate(dteNow) & ", " &
                    " '" & Common.Parse(strUserName) & "', '" & strPhone & "', '" & strEmail & "', " &
                    " " & Common.ConvertDate(txtDate1.AddDays(4)) & ", '0', '3', '0', " &
                    " PRM_PAYMENT_TYPE, PRM_PAYMENT_TERM, '" & Common.Parse(strRemark) & "'" &
                    " FROM PR_MSTR PM WHERE PRM_COY_ID='" & companyID & "' AND PRM_PR_No='" & strPRNo & "'; "
            Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
            '    & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
            '    "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
            'Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
            'Common.Insert2Ary(pQuery, strSql)

            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)

            ' Group All Duplicate
            'Dim tempItem As String
            'Dim tempQty As Integer = 0
            'Dim j As Integer
            'For i = 0 To PRList.Count - 1
            '    tempItem = PRList(i)(3)
            '    tempQty = PRList(i)(4)
            '    For j = 0 To PRList.Count - 1
            '        If tempItem = PRList(j)(3) Then
            '            tempQty = tempQty + IIf(PRList(j)(4) = "", 0, PRList(j)(4))
            '        End If
            '    Next
            '    PRList.Add(New String() {PRList(i)(0), PRList(i)(1), PRList(i)(2), PRList(i)(3), tempQty, "Done"})
            '    tempQty = 0
            'Next            

            For i = 0 To PRList.Count - 1
                'Michelle (16/9/2011) - To include the Warranty term (871)
                If strItemLine = "" Then
                    strSql = " INSERT INTO RFQ_DETAIL " &
                            " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " &
                            " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " &
                            " RD_UOM,RD_DELIVERY_LEAD_TIME, " &
                            " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS ) " &
                            " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " &
                            " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " &
                            " PRD_UOM, PR_DETAILS.PRD_ETD, " &
                            " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS " &
                            " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID ='" & strCoyID & "'" &
                            " AND PRD_PR_NO='" & PRList(i)(0) & "' AND PRD_COY_ID = PRM_COY_ID "

                    strSql &= " ORDER BY PRD_PR_LINE; "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                       "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                       "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
                       "PRD_GST = PM_LAST_TXN_TAX, " &
                       "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                       "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "' " &
                       "AND PM_S_COY_ID = PRD_COY_ID " &
                       "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser &
                         "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'; "
                    Common.Insert2Ary(pQuery, strSql)
                Else
                    strSql = " INSERT INTO RFQ_DETAIL " &
                            " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " &
                            " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " &
                            " RD_UOM,RD_DELIVERY_LEAD_TIME, " &
                            " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS ) " &
                            " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " &
                            " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " &
                            " PRD_UOM, PR_DETAILS.PRD_ETD, " &
                            " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS " &
                            " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID & "'" &
                            " AND PRD_PR_NO='" & PRList(i)(0) & "' AND PRD_COY_ID = PRM_COY_ID "

                    strSql &= " ORDER BY PRD_PR_LINE; "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                        "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                        "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
                        "PRD_GST = PM_LAST_TXN_TAX, " &
                        "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                        "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "' " &
                        "AND PM_S_COY_ID = PRD_COY_ID " &
                        "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                    Common.Insert2Ary(pQuery, strSql)

                    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser &
                         "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'; "
                    Common.Insert2Ary(pQuery, strSql)
                End If
            Next

            strSql = " set @num  = 0; "
            Common.Insert2Ary(pQuery, strSql)

            Return strNewRFQNoList
        End Function


        Function CreatePO(ByVal strCoyID As String, ByVal UserID As String, ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal strAPP As String = "", Optional ByVal pQueryLast() As String = Nothing, Optional ByVal blnEnterpriseVersion As Boolean = True) As String
            Dim strSql, strNewPONo, strPOPrefix, strTermFile, strAttachIndex As String
            Dim dblTotalCost As String
            Dim strNewPONoList As String
            Dim strVendorLast As String
            Dim strInternalRemark As String
            Dim strExternalRemark As String

            ' Dim dtVendor As DataTable = objDb.FillDt("SELECT DISTINCT PRD_S_COY_ID FROM PR_DETAILS WHERE PRD_PR_NO ='" & strPRNo & "' AND PRD_COY_ID ='" & strCoyID & "'")
            Dim dtVendor As DataTable

            If strItemLine = "" Then
                dtVendor = objDb.FillDt("SELECT DISTINCT PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE, (SELECT PM_LAST_TXN_PRICE_CURR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_CURR, (SELECT PM_LAST_TXN_PRICE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_PRICE,(SELECT PM_LAST_TXN_TAX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_TAX,(SELECT PM_LAST_TXN_S_COY_ID  FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_S_COY_ID FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_NO ='" & strPRNo & "' AND PRD_COY_ID ='" & strCoyID & "' GROUP BY PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE")
            Else
                dtVendor = objDb.FillDt("SELECT DISTINCT PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE, (SELECT PM_LAST_TXN_PRICE_CURR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_CURR, (SELECT PM_LAST_TXN_PRICE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_PRICE,(SELECT PM_LAST_TXN_TAX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_TAX,(SELECT PM_LAST_TXN_S_COY_ID  FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PR_DETAILS.PRD_PRODUCT_CODE AND PM_S_COY_ID = '" & strCoyID & "') AS PM_LAST_TXN_S_COY_ID FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO ='" & strPRNo & "' AND PRD_COY_ID ='" & strCoyID & "' GROUP BY PRD_S_COY_ID, PRD_CURRENCY_CODE, PRM_B_ADDR_CODE")
            End If

            Dim intIncrementNo As Integer = 0
            Dim i As Integer

            strSql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(pQuery, strSql)

            Dim strVendorList As String = ""
            For Each dr As DataRow In dtVendor.Rows
                strVendor = dr("PRD_S_COY_ID")
                strVendorLast = Common.parseNull(dr("PM_LAST_TXN_S_COY_ID"))
                Dim strCURRENCY As String
                Dim strVenCURRENCY As String
                Dim strCURRENCYLast As String
                'Dim strCURRENCY = dr("PRD_CURRENCY_CODE")
                'Dim strCURRENCY = dr("PM_LAST_TXN_CURR")
                Dim strADDR = dr("PRM_B_ADDR_CODE")

                'Michelle (CR0026) - PO cost not correctly captured for multiple POs case
                'dblTotalCost = objDb.GetVal("SELECT ISNULL(PRM_PR_COST,0) FROM PR_MSTR" _
                '& " WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO ='" & strPRNo & "'")

                If strVendorList = "" Then
                    strVendorList = "'" & Common.Parse(strVendor) & "'"
                Else
                    strVendorList = strVendorList & ",'" & Common.Parse(strVendor) & "'"
                End If



                If multiPR = True Then
                    Dim strSQLPR As String
                    If strItemLine = "" Then
                        strVenCURRENCY = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'")
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-18: CH: Rounding issue (Prod issue)
                        'strSQLPR = "SELECT ISNULL(SUM( PRD_UNIT_COST * PRD_ORDERED_QTY + (ISNULL(PRD_GST,0) * (PRD_UNIT_COST * PRD_ORDERED_QTY))/100 ),0)  FROM PR_DETAILS, PR_MSTR " _
                        '         & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID "
                        strSQLPR = "SELECT IFNULL(SUM(ROUND(ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2) + ROUND((IFNULL(PRD_GST,0) * ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2))/100,2),2)),0)  FROM PR_DETAILS, PR_MSTR " _
                                 & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID "
                        For i = 0 To PRList.Count - 1
                            If i = 0 Then
                                strSQLPR &= " AND ((PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            Else
                                strSQLPR &= " OR (PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            End If

                        Next
                        strSQLPR &= " ) "
                        dblTotalCost = objDb.GetVal(strSQLPR)
                    Else
                        strCURRENCYLast = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'") 'Common.parseNull(dr("PM_LAST_TXN_CURR"))
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'strSQLPR = "SELECT ISNULL(SUM( PM_LAST_TXN_PRICE * PRD_ORDERED_QTY + (ISNULL(PM_LAST_TXN_TAX,0) * (PM_LAST_TXN_PRICE * PRD_ORDERED_QTY))/100 ),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                        '         & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE"
                        strSQLPR = "SELECT IFNULL(SUM(ROUND(ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2) + ROUND((IFNULL(PM_LAST_TXN_TAX,0) * ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2))/100,2),2)),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                                 & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE"
                        For i = 0 To PRList.Count - 1
                            If i = 0 Then
                                strSQLPR &= " AND ((PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            Else
                                strSQLPR &= " OR (PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & PRList(i)(0) & "') "
                            End If
                        Next
                        strSQLPR &= " ) "
                        dblTotalCost = objDb.GetVal(strSQLPR)
                    End If

                    strSQLPR = "SELECT CAST(GROUP_CONCAT(PRM_INTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_INTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & strCoyID & "' "
                    For i = 0 To PRList.Count - 1
                        If i = 0 Then
                            strSQLPR &= " AND ((PRM_PR_NO ='" & PRList(i)(0) & "') "
                        Else
                            strSQLPR &= " OR (PRM_PR_NO ='" & PRList(i)(0) & "') "
                        End If
                    Next
                    strSQLPR &= " ) "
                    strInternalRemark = objDb.GetVal(strSQLPR)

                    strSQLPR = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & strCoyID & "' "
                    For i = 0 To PRList.Count - 1
                        If i = 0 Then
                            strSQLPR &= " AND ((PRM_PR_NO ='" & PRList(i)(0) & "') "
                        Else
                            strSQLPR &= " OR (PRM_PR_NO ='" & PRList(i)(0) & "') "
                        End If
                    Next
                    strSQLPR &= " ) "
                    strExternalRemark = objDb.GetVal(strSQLPR)
                Else
                    Dim strSQLPR2 As String
                    If strItemLine = "" Then
                        strVenCURRENCY = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'")
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM( PRD_UNIT_COST * PRD_ORDERED_QTY + (ISNULL(PRD_GST,0) * (PRD_UNIT_COST * PRD_ORDERED_QTY))/100 ),0)  FROM PR_DETAILS, PR_MSTR " _
                        '                & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                        dblTotalCost = objDb.GetVal("SELECT CAST(ISNULL(SUM(ROUND(ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2) + ROUND((ISNULL(PRD_GST,0) * ROUND(PRD_UNIT_COST * PRD_ORDERED_QTY,2))/100,2),2)),0) AS CHAR) FROM PR_DETAILS, PR_MSTR " _
                                        & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                    Else
                        strCURRENCYLast = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE cm_coy_id = '" & strVendor & "'")
                        strCURRENCY = dr("PRD_CURRENCY_CODE")
                        '2015-06-19: CH: Rounding issue (Prod issue)
                        'dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM( PM_LAST_TXN_PRICE * PRD_ORDERED_QTY + (ISNULL(PM_LAST_TXN_TAX,0) * (PM_LAST_TXN_PRICE * PRD_ORDERED_QTY))/100 ),0) FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                        '                & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                        dblTotalCost = objDb.GetVal("SELECT CAST(ISNULL(SUM(ROUND(ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2) + ROUND((ISNULL(PM_LAST_TXN_TAX,0) * ROUND(PM_LAST_TXN_PRICE * PRD_ORDERED_QTY,2))/100,2),2)),0) AS CHAR)  FROM PR_DETAILS, PR_MSTR, PRODUCT_MSTR " _
                                        & " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PM_PRODUCT_CODE = PRD_PRODUCT_CODE AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_S_COY_ID= '" & strVendor & "' AND PRD_CURRENCY_CODE = '" & strCURRENCY & "' AND PRM_B_ADDR_CODE = '" & strADDR & "' AND PRD_PR_NO ='" & strPRNo & "'")
                    End If


                    strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_INTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_INTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & strCoyID & "' "
                    strSQLPR2 &= " AND ((PRM_PR_NO ='" & strPRNo & "')) "
                    strInternalRemark = objDb.GetVal(strSQLPR2)

                    strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & strCoyID & "' "
                    strSQLPR2 &= " AND ((PRM_PR_NO ='" & strPRNo & "')) "
                    strExternalRemark = objDb.GetVal(strSQLPR2)
                End If


                '' ''Dim objGlobal As New AppGlobals
                '' ''intIncrementNo += 1
                '' ''objGlobal.GetLatestDocNo("PO", pQuery, strNewPONo, strPOPrefix, , intIncrementNo)

                intIncrementNo += 1

                '' ''If strNewPONoList = "" Then
                '' ''    strNewPONoList = strNewPONo & "!" & strVendor
                '' ''Else
                '' ''    strNewPONoList &= "," & strNewPONo & "!" & strVendor
                '' ''End If

                '' ''strSql = "SELECT '*' FROM PO_MSTR WHERE POM_PO_NO='" & strNewPONo & "' AND POM_B_COY_ID='" & strCoyID & "'"
                '' ''If objDb.Exist(strSql) Then
                '' ''    Return "error"
                '' ''End If

                '' ''strNewPONo = " (SELECT GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                '' ''       & " RIGHT(LEFT('10000000000000', LENGTH(cp_param_value)+1) + cp_param_value + '" & intIncrementNo & "',LENGTH(cp_param_value)) END " _
                '' ''       & " SEPARATOR '') AS cp_param_value FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO') "

                strNewPONo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                strPOPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO') "

                '' ''objGlobal = Nothing

                Dim objWheelFile As New FileManagement
                '//temporary remark by moo
                If multiPR = True Then
                    For i = 0 To PRList.Count - 1
                        strTermFile = objWheelFile.copyTermCondToPO(strCoyID, strNewPONo)
                        strAttachIndex = objWheelFile.copyPRAttachToPO(strCoyID, PRList(i)(0), strNewPONo)
                        If strAttachIndex <> "" Then
                            strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                            & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                            & "SELECT CDA_COY_ID," & strNewPONo & ",'PO',CDA_HUB_FILENAME," _
                            & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
                            "' AND CDA_DOC_NO='" & PRList(i)(0) & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                            Common.Insert2Ary(pQuery, strSql)
                        End If
                    Next
                Else
                    strTermFile = objWheelFile.copyTermCondToPO(strCoyID, strNewPONo)
                    strAttachIndex = objWheelFile.copyPRAttachToPO(strCoyID, strPRNo, strNewPONo)
                    If strAttachIndex <> "" Then
                        strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
                        & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
                        & "SELECT CDA_COY_ID," & strNewPONo & ",'PO',CDA_HUB_FILENAME," _
                        & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
                        "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
                        Common.Insert2Ary(pQuery, strSql)
                    End If
                End If

                If strItemLine = "" Then
                    CreatePOHeader(strCoyID, UserID, strPRNo, strVendor, strNewPONo, strPOPrefix, pQuery, dblTotalCost, strUserID, False, strTermFile, False, "", strVenCURRENCY, strItemLine, multiPR, PRList, strAPP, strInternalRemark, strExternalRemark)
                Else
                    CreatePOHeader(strCoyID, UserID, strPRNo, strVendorLast, strNewPONo, strPOPrefix, pQuery, dblTotalCost, strUserID, False, strTermFile, False, "", strCURRENCYLast, strItemLine, multiPR, PRList, "", strInternalRemark, strExternalRemark)
                End If
                '//Pass in "strUserID" as parameter -> for consolidation
                '//Originally Last level of AO is consolidator

                If multiPR = True Then
                    CreatePODetailMulti(strCoyID, UserID, strPRNo, intPRIndex, strVendor, strNewPONo, pQuery, 0, strUserID, False, strCURRENCY, strADDR, strItemLine, multiPR, PRList, strAPP, blnEnterpriseVersion)
                Else
                    CreatePODetail(strCoyID, UserID, strPRNo, intPRIndex, strVendor, strNewPONo, pQuery, 0, strUserID, False, strCURRENCY, strADDR, strItemLine, strAPP, blnEnterpriseVersion)
                End If

                If intIncrementNo = 1 Then
                    strSql = " SET @T_NO = CONCAT(" & strNewPONo & ",'!" & strVendor & "'); "
                    Common.Insert2Ary(pQuery, strSql)
                Else
                    strSql = " SET @T_NO = CONCAT(CONCAT(@T_NO,','),CONCAT(" & strNewPONo & ",'!" & strVendor & "')); "
                    Common.Insert2Ary(pQuery, strSql)
                End If
            Next

            '' ''strSql = "UPDATE company_param SET cp_param_value = RIGHT(LEFT('10000000000000', LENGTH(cp_param_value)+1) + cp_param_value + '1', LENGTH(cp_param_value)) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "


            If strItemLine = "" Then
                strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                   "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                   "PRD_GST = PM_LAST_TXN_TAX, " &
                   "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                   "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & strPRNo & "' " &
                   "AND PM_S_COY_ID = PRD_COY_ID " &
                   "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(pQuery, strSql)
            Else
                strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
                    "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
                    "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
                    "PRD_GST = PM_LAST_TXN_TAX, " &
                    "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
                    "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " &
                    "AND PM_S_COY_ID = PRD_COY_ID " &
                    "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(pQuery, strSql)
            End If

            Dim strSqlLast As String
            If multiPR = False Then
                strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID IN ('" & Common.Parse(strCoyID) & "')) = 0"
                Common.Insert2Ary(pQuery, strSql)

                strSqlLast = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' AND PRD_COY_ID IN ('" & Common.Parse(strCoyID) & "')) = 0"
                If pQueryLast Is Nothing Then
                    ReDim pQueryLast(0)
                    pQueryLast(0) = ""
                End If
                Common.Insert2Ary(pQueryLast, strSqlLast)
            End If

            strSql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strCoyID & "' AND CP_PARAM_TYPE = 'PO' "
            Common.Insert2Ary(pQuery, strSql)
            'strNewPONoList = objDb.GetVal(strNewPONo)
            'Dim strPO As String
            'objDb.BatchExecuteNew(pQuery, , strPO, "T_NO")

            Return strNewPONoList
            'objDb.BatchExecute(strAryQuery)

        End Function
        Public Function runQuery(ByRef pQuery() As String, ByVal strPO As String) As String
            Dim strPO_NO As String = ""
            Dim strMsg As String = ""
            'For index = 0 To pQuery.Length - 1
            '    objDb.Execute(pQuery(index))
            'Next
            If Not objDb.BatchExecuteNew(pQuery, , strPO, "T_NO") Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                Dim arrPOInfo, arrPO As Array
                Dim intLowBound, intUpBound, intLoop As Integer
                Dim strPoNo, strVen As String
                Dim strSql1 As String
                Dim tDS As DataSet
                Dim strvendorname As String
                Dim strVendorNameList As String
                Dim objPR1 As New PR


                arrPOInfo = Split(strPO, ",")
                intLowBound = LBound(arrPOInfo)
                intUpBound = UBound(arrPOInfo)

                '' ''Dim strPO_NO As String = objDB.GetVal(" SELECT @PO_NO; ")
                '' ''If strPO_NO <> "" Then
                '' ''    strMsg = "Purchase Order Number " & strPO_NO & " has been created."
                '' ''End If

                For intLoop = 0 To intUpBound
                    'To get the Vendor name
                    arrPO = Split(arrPOInfo(intLoop), "!")
                    strPoNo = arrPO(0) 'Capture the PO No.
                    strVen = arrPO(1)  'Caputre the Vendor code


                    strMsg = "Purchase Order Number " & strPoNo & " has been created."
                    'strSql1 = "SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE  CM_COY_ID='" & strVen & "'"
                    'tDS = objDB.FillDs(strSql1)
                    'If tDS.Tables(0).Rows.Count > 0 Then
                    '    strvendorname = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                    'End If
                    'If intLoop = 0 Then
                    '    ' strMsg = "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                    '    strMsg = "Purchase Order Number " & strPoNo & " has been created."
                    '    strVendorNameList = strvendorname
                    'Else
                    '    strMsg = strMsg & """& vbCrLf & """ & "Purchase Order Number " & strPoNo & " has been created for " & strvendorname & "."
                    '    strVendorNameList = strVendorNameList & ", " & strvendorname
                    'End If

                Next
            End If
            Return strMsg
        End Function
        Public Function PRNoCheck(ByVal companyID As String, ByVal strPRno As String) As String
            Dim PR_No As String = objDb.GetVal("SELECT IFNULL(PRM_PR_NO,'') AS PRM_PR_NO FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID  AND PRM_PR_STATUS = '" & PRStatus.ConvertedToPO & "' AND PRM_COY_ID = '" & companyID & "' AND PRM_PR_NO = '" & strPRno & "'")
            Dim strMsg As String
            If PR_No <> "" Then
                strMsg = "Selected document " & strPRno & " converted."
            Else
                strMsg = ""
            End If

            Return strMsg
        End Function
        'Function CreateRFQ(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True) As String
        '    Dim strCoyID, strLoginUser, strSql, strNewRFQNo, strRFQPrefix As String
        '    Dim strTermFile, strAttachIndex As String
        '    Dim intIncrementNo As Integer = 0
        '    Dim objGlobal As New AppGlobals
        '    Dim strAryQuery(0) As String
        '    Dim dteNow As DateTime = Now()
        '    Dim strUserName As String, strPhone As String, strFax As String, strEmail As String
        '    Dim strNewRFQNoList As String

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strLoginUser = HttpContext.Current.Session("UserId")

        '    intIncrementNo += 1
        '    objGlobal.GetLatestDocNo("RFQ", pQuery, strNewRFQNo, strRFQPrefix, , intIncrementNo)

        '    If strNewRFQNoList = "" Then
        '        strNewRFQNoList = strNewRFQNo & "!" & strVendor
        '    Else
        '        strNewRFQNoList &= "," & strNewRFQNo & "!" & strVendor
        '    End If

        '    strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_NO='" & strNewRFQNo & "' AND RM_COY_ID='" & strCoyID & "'"
        '    If objDb.Exist(strSql) Then
        '        Return "error"
        '    End If

        '    objGlobal = Nothing

        '    Dim objUser As New User
        '    Dim objUsers As New Users

        '    If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
        '        strUserName = objUser.Name
        '        strPhone = objUser.PhoneNo
        '        strFax = objUser.FaxNo
        '        strEmail = objUser.Email
        '    Else
        '        strUserName = ""
        '        strPhone = ""
        '        strFax = ""
        '        strEmail = ""
        '    End If

        '    Dim txtDate1 As Date = CType(dteNow, Date)

        '    strSql = " INSERT INTO RFQ_MSTR " &
        '            " (RM_PREFIX,RM_RFQ_NO,RM_COY_ID,RM_RFQ_NAME, " &
        '            " RM_EXPIRY_DATE,RM_CREATED_BY,RM_CREATED_ON, " &
        '            " RM_CONTACT_PERSON,RM_CONTACT_NUMBER,RM_EMAIL, " &
        '            " RM_REQD_QUOTE_VALIDITY,RM_RFQ_OPTION,RM_STATUS,RM_B_DISPLAY_STATUS, " &
        '            " RM_PAYMENT_TYPE, RM_PAYMENT_TERM, RM_REMARK) " &
        '            " SELECT '" & strRFQPrefix & "', '" & strNewRFQNo & "', '" & HttpContext.Current.Session("CompanyID") & "', '', " &
        '            " " & Common.ConvertDate(txtDate1.AddDays(3)) & ", '" & strUserID & "', " & Common.ConvertDate(dteNow) & ", " &
        '            " '" & Common.Parse(strUserName) & "', '" & strPhone & "', '" & strEmail & "', " &
        '            " " & Common.ConvertDate(txtDate1.AddDays(4)) & ", (SELECT (CASE WHEN CM_RFQ_OPTION = 2 THEN 0 ELSE CM_RFQ_OPTION END) AS CM_RFQ_OPTION FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'), '3', '0', " &
        '            " PRM_PAYMENT_TYPE, PRM_PAYMENT_TERM, PRM_EXTERNAL_REMARK" &
        '            " FROM PR_MSTR PM WHERE PRM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_No='" & strPRNo & "'"
        '    Common.Insert2Ary(pQuery, strSql)

        '    'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
        '    '    & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
        '    '    "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
        '    'Common.Insert2Ary(pQuery, strSql)

        '    'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
        '    'Common.Insert2Ary(pQuery, strSql)

        '    'Michelle (16/9/2011) - To include the Warranty term (871)
        '    strSql = " set @num  = 0; INSERT INTO RFQ_DETAIL " &
        '            " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " &
        '            " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " &
        '            " RD_UOM,RD_DELIVERY_LEAD_TIME, " &
        '            " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS ) " &
        '            " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " &
        '            " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " &
        '            " PRD_UOM, PR_DETAILS.PRD_ETD, " &
        '            " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS " &
        '            " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_COY_ID ='" & strCoyID & "'" &
        '            " AND PRD_PR_NO='" & strPRNo & "' AND PRD_COY_ID = PRM_COY_ID "

        '    strSql &= " ORDER BY PRD_PR_LINE; set @num  = 0;"
        '    Common.Insert2Ary(pQuery, strSql)

        '    strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
        '            "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
        '            "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
        '            "PRD_GST = PM_LAST_TXN_TAX, " &
        '            "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
        '            "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo & "' " &
        '            "AND PM_S_COY_ID = PRD_COY_ID " &
        '            "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
        '    Common.Insert2Ary(pQuery, strSql)

        '    strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser &
        '             "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_PR_LINE IN (" & strItemLine & ") AND PRD_PR_NO = '" & strPRNo &
        '             "' AND PRD_COY_ID = '" & Common.Parse(strCoyID) & "' "
        '    Common.Insert2Ary(pQuery, strSql)

        '    Dim objWheelFile As New FileManagement
        '    'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)
        '    strAttachIndex = objWheelFile.copyPRAttachToRFQ(strPRNo, strNewRFQNo)
        '    If strAttachIndex <> "" Then
        '        'If blnEnterpriseVersion = True Then
        '        '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '        '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
        '        '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
        '        '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
        '        '    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
        '        'Else
        '        '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '        '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
        '        '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
        '        '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
        '        '    "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
        '        'End If

        '        strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '            & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
        '            & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
        '            & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
        '            "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"

        '        Common.Insert2Ary(pQuery, strSql)
        '    End If

        '    Return strNewRFQNoList
        'End Function

        'Function CreateRFQMulti(ByVal strPRNo As String, ByVal intPRIndex As Long, ByVal strVendor As String, ByRef pQuery() As String, ByVal strUserID As String, Optional ByVal strItemLine As String = "", Optional ByVal multiPR As Boolean = False, Optional ByVal PRList As ArrayList = Nothing, Optional ByVal blnEnterpriseVersion As Boolean = True) As String
        '    Dim strCoyID, strLoginUser, strSql, strNewRFQNo, strRFQPrefix As String
        '    Dim strTermFile, strAttachIndex As String
        '    Dim intIncrementNo As Integer = 0
        '    Dim objGlobal As New AppGlobals
        '    Dim strAryQuery(0) As String
        '    Dim dteNow As DateTime = Now()
        '    Dim strUserName As String, strPhone As String, strFax As String, strEmail As String
        '    Dim strNewRFQNoList As String

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strLoginUser = HttpContext.Current.Session("UserId")

        '    intIncrementNo += 1
        '    objGlobal.GetLatestDocNo("RFQ", pQuery, strNewRFQNo, strRFQPrefix, , intIncrementNo)

        '    If strNewRFQNoList = "" Then
        '        strNewRFQNoList = strNewRFQNo & "!" & strVendor
        '    Else
        '        strNewRFQNoList &= "," & strNewRFQNo & "!" & strVendor
        '    End If

        '    strSql = "SELECT '*' FROM RFQ_MSTR WHERE RM_RFQ_NO='" & strNewRFQNo & "' AND RM_COY_ID='" & strCoyID & "'"

        '    If objDb.Exist(strSql) Then
        '        Return "error"
        '    End If

        '    objGlobal = Nothing

        '    Dim objUser As New User
        '    Dim objUsers As New Users

        '    If objUsers.GetUserDetail(strUserID, strCoyID, objUser) Then
        '        strUserName = objUser.Name
        '        strPhone = objUser.PhoneNo
        '        strFax = objUser.FaxNo
        '        strEmail = objUser.Email
        '    Else
        '        strUserName = ""
        '        strPhone = ""
        '        strFax = ""
        '        strEmail = ""
        '    End If

        '    Dim txtDate1 As Date = CType(dteNow, Date)
        '    Dim strSQLPR, strSQLPR2 As String
        '    Dim strRemark As String

        '    Dim i As Integer
        '    For i = 0 To PRList.Count - 1
        '        If strSQLPR = "" Then
        '            strSQLPR = "'" + PRList(i)(0) + "'"
        '        Else
        '            strSQLPR = strSQLPR + ",'" + PRList(i)(0) + "'"
        '        End If

        '    Next

        '    strSQLPR2 = "SELECT CAST(GROUP_CONCAT(PRM_EXTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_EXTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strSQLPR2 &= " AND (PRM_PR_NO IN (" & strSQLPR & ")) "
        '    strRemark = objDb.GetVal(strSQLPR2)

        '    Dim objWheelFile As New FileManagement
        '    'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)
        '    strAttachIndex = objWheelFile.copyPRAttachToRFQMulti(strSQLPR, strNewRFQNo)
        '    If strAttachIndex <> "" Then
        '        'If blnEnterpriseVersion = True Then
        '        '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '        '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
        '        '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
        '        '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
        '        '    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
        '        'Else
        '        '    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '        '    & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
        '        '    & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
        '        '    & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & _
        '        '    "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
        '        'End If

        '        strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '            & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) " _
        '            & "SELECT CDA_COY_ID,'" & strNewRFQNo & "','RFQ',CDA_HUB_FILENAME," _
        '            & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
        '            "' AND CDA_DOC_NO IN (" & strSQLPR & ") AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"

        '        Common.Insert2Ary(pQuery, strSql)
        '    End If

        '    strSql = " INSERT INTO RFQ_MSTR " &
        '            " (RM_PREFIX,RM_RFQ_NO,RM_COY_ID,RM_RFQ_NAME, " &
        '            " RM_EXPIRY_DATE,RM_CREATED_BY,RM_CREATED_ON, " &
        '            " RM_CONTACT_PERSON,RM_CONTACT_NUMBER,RM_EMAIL, " &
        '            " RM_REQD_QUOTE_VALIDITY,RM_RFQ_OPTION,RM_STATUS,RM_B_DISPLAY_STATUS, " &
        '            " RM_PAYMENT_TYPE, RM_PAYMENT_TERM, RM_REMARK) " &
        '            " SELECT '" & strRFQPrefix & "', '" & strNewRFQNo & "', '" & HttpContext.Current.Session("CompanyID") & "', '', " &
        '            " " & Common.ConvertDate(txtDate1.AddDays(3)) & ", '" & strUserID & "', " & Common.ConvertDate(dteNow) & ", " &
        '            " '" & Common.Parse(strUserName) & "', '" & strPhone & "', '" & strEmail & "', " &
        '            " " & Common.ConvertDate(txtDate1.AddDays(4)) & ", '0', '3', '0', " &
        '            " PRM_PAYMENT_TYPE, PRM_PAYMENT_TERM, '" & Common.Parse(strRemark) & "'" &
        '            " FROM PR_MSTR PM WHERE PRM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_No='" & strPRNo & "'; "
        '    Common.Insert2Ary(pQuery, strSql)

        '    'strSql = "UPDATE PR_MSTR SET PRM_PO_INDEX=(SELECT MAX(POM_PO_INDEX) FROM PO_MSTR)," _
        '    '    & "PRM_STATUS_CHANGED_BY='" & strLoginUser & _
        '    '    "', PRM_PR_DATE=" & Common.ConvertDate(Now) & ", PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now) & " WHERE PRM_PR_INDEX=" & intPRIndex
        '    'Common.Insert2Ary(pQuery, strSql)

        '    'strSql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.ConvertedToPO & " WHERE PRM_PR_INDEX=" & intPRIndex & " AND (SELECT COUNT(*) FROM PR_DETAILS WHERE PRD_CONVERT_TO_DOC IS NULL AND PRD_PR_NO = '" & strPRNo & "' ) = 0"
        '    'Common.Insert2Ary(pQuery, strSql)

        '    strSql = " set @num  = 0; "
        '    Common.Insert2Ary(pQuery, strSql)

        '    ' Group All Duplicate
        '    'Dim tempItem As String
        '    'Dim tempQty As Integer = 0
        '    'Dim j As Integer
        '    'For i = 0 To PRList.Count - 1
        '    '    tempItem = PRList(i)(3)
        '    '    tempQty = PRList(i)(4)
        '    '    For j = 0 To PRList.Count - 1
        '    '        If tempItem = PRList(j)(3) Then
        '    '            tempQty = tempQty + IIf(PRList(j)(4) = "", 0, PRList(j)(4))
        '    '        End If
        '    '    Next
        '    '    PRList.Add(New String() {PRList(i)(0), PRList(i)(1), PRList(i)(2), PRList(i)(3), tempQty, "Done"})
        '    '    tempQty = 0
        '    'Next            

        '    For i = 0 To PRList.Count - 1
        '        'Michelle (16/9/2011) - To include the Warranty term (871)
        '        If strItemLine = "" Then
        '            strSql = " INSERT INTO RFQ_DETAIL " &
        '                    " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " &
        '                    " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " &
        '                    " RD_UOM,RD_DELIVERY_LEAD_TIME, " &
        '                    " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS ) " &
        '                    " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " &
        '                    " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " &
        '                    " PRD_UOM, PR_DETAILS.PRD_ETD, " &
        '                    " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS " &
        '                    " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID ='" & strCoyID & "'" &
        '                    " AND PRD_PR_NO='" & PRList(i)(0) & "' AND PRD_COY_ID = PRM_COY_ID "

        '            strSql &= " ORDER BY PRD_PR_LINE; "
        '            Common.Insert2Ary(pQuery, strSql)

        '            strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
        '               "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
        '               "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
        '               "PRD_GST = PM_LAST_TXN_TAX, " &
        '               "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
        '               "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_NO = '" & PRList(i)(0) & "' " &
        '               "AND PM_S_COY_ID = PRD_COY_ID " &
        '               "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
        '            Common.Insert2Ary(pQuery, strSql)

        '            strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser &
        '                 "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_PR_NO = '" & PRList(i)(0) & "'; "
        '            Common.Insert2Ary(pQuery, strSql)
        '        Else
        '            strSql = " INSERT INTO RFQ_DETAIL " &
        '                    " (RD_RFQ_ID,RD_COY_ID,RD_RFQ_LINE, " &
        '                    " RD_VENDOR_ITEM_CODE,RD_QUANTITY,RD_PRODUCT_DESC, RD_PRODUCT_CODE, " &
        '                    " RD_UOM,RD_DELIVERY_LEAD_TIME, " &
        '                    " RD_PRODUCT_NAME, RD_PR_LINE_INDEX, RD_WARRANTY_TERMS ) " &
        '                    " SELECT " & objDb.GetLatestInsertedID("RFQ_MSTR") & ", IFNULL((SELECT PM_S_COY_ID FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = PRD_PRODUCT_CODE),'&nbsp;'), @num := @num + 1, " &
        '                    " PRD_VENDOR_ITEM_CODE, PRD_ORDERED_QTY, PRD_PRODUCT_DESC, PRD_PRODUCT_CODE, " &
        '                    " PRD_UOM, PR_DETAILS.PRD_ETD, " &
        '                    " PRD_PRODUCT_DESC, PRD_PR_LINE_INDEX, PRD_WARRANTY_TERMS " &
        '                    " FROM PR_DETAILS, PR_MSTR WHERE PRM_PR_NO = PRD_PR_NO AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_COY_ID ='" & strCoyID & "'" &
        '                    " AND PRD_PR_NO='" & PRList(i)(0) & "' AND PRD_COY_ID = PRM_COY_ID "

        '            strSql &= " ORDER BY PRD_PR_LINE; "
        '            Common.Insert2Ary(pQuery, strSql)

        '            strSql = "UPDATE PRODUCT_MSTR, PR_DETAILS " &
        '                "SET PRD_UNIT_COST = PM_LAST_TXN_PRICE, " &
        '                "PRD_CURRENCY_CODE = PM_LAST_TXN_PRICE_CURR, " &
        '                "PRD_GST = PM_LAST_TXN_TAX, " &
        '                "PRD_S_COY_ID = PM_LAST_TXN_S_COY_ID " &
        '                "WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "' " &
        '                "AND PM_S_COY_ID = PRD_COY_ID " &
        '                "AND PRD_PRODUCT_CODE = PM_PRODUCT_CODE "
        '            Common.Insert2Ary(pQuery, strSql)

        '            strSql = "UPDATE PR_DETAILS SET PRD_CONVERT_TO_IND = 'RFQ', PRD_CONVERT_BY_ID = '" & strLoginUser &
        '                 "', PRD_CONVERT_TO_DOC = '" & strNewRFQNo & "', PRD_CONVERT_TO_DATE = " & Common.ConvertDate(Now) & " WHERE PRD_COY_ID = '" & Common.Parse(strCoyID) & "' AND PRD_PR_LINE IN (" & PRList(i)(2) & ") AND PRD_PR_NO = '" & PRList(i)(0) & "'; "
        '            Common.Insert2Ary(pQuery, strSql)
        '        End If
        '    Next

        '    strSql = " set @num  = 0; "
        '    Common.Insert2Ary(pQuery, strSql)

        '    Return strNewRFQNoList
        'End Function

        ''//update PR_MSTR,PO_MSTR,PO_DETAILS,SEND MAIL TO VENDOR, BUYER
        'Function PRConsolidation(ByVal strAryPRNo() As String, ByVal strAryPRIndex() As String, ByVal strVendor As String, ByVal strRemark As String, ByVal strUserID As String, ByRef strMsg() As String, ByVal blnPreview As Boolean, ByVal blnRelief As Boolean) As Boolean
        '    Dim strSql, strNewPONo, strPOPrefix, strAryQuery(0), strCoyID, strAllPR As String
        '    Dim objGlobal As New AppGlobals
        '    Dim intLoop, intPOLine As Integer
        '    Dim dblTotalCost As Double

        '    Dim strTermFile, strAttachIndex As String
        '    Dim objWheelFile As New FileManagement

        '    strCoyID = HttpContext.Current.Session("CompanyId")


        '    For intLoop = 0 To strAryPRNo.GetUpperBound(0)
        '        If intLoop = 0 Then
        '            strAllPR = "'" & strAryPRNo(intLoop) & "'"
        '        Else
        '            strAllPR = strAllPR & ",'" & strAryPRNo(intLoop) & "'"
        '        End If
        '    Next

        '    If Not checkPR_Ready(strAllPR, "consolidate", strMsg) Then
        '        Return False
        '    End If

        '    objGlobal.GetLatestDocNo("PO", strAryQuery, strNewPONo, strPOPrefix, blnPreview)
        '    strSql = "SELECT '*' FROM PO_MSTR WHERE POM_PO_NO='" & strNewPONo & "' AND POM_B_COY_ID='" & strCoyID & "'"
        '    If objDb.Exist(strSql) Then
        '        strMsg(0) = Common.MsgTransDup
        '        Return False
        '    End If
        '    '//temporary remark
        '    If Not blnPreview Then
        '        'strTermFile = objWheelFile.copyTermCondToPO(strNewPONo)
        '        For intLoop = 0 To strAryPRNo.GetUpperBound(0)
        '            strAttachIndex = objWheelFile.copyPRAttachToPO(strAryPRNo(intLoop), strNewPONo)
        '            If strAttachIndex <> "" Then
        '                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID,CDA_DOC_NO," _
        '                & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE) " _
        '                & "SELECT CDA_COY_ID,'" & strNewPONo & "','PO',CDA_HUB_FILENAME," _
        '                & "CDA_ATTACH_FILENAME,CDA_FILESIZE FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID &
        '                "' AND CDA_DOC_NO='" & strAryPRNo(intLoop) & "' AND CDA_DOC_TYPE='PR' AND CDA_ATTACH_INDEX IN(" & strAttachIndex & ")"
        '                Common.Insert2Ary(strAryQuery, strSql)
        '                strAttachIndex = ""
        '            End If
        '        Next
        '    End If

        '    dblTotalCost = objDb.GetVal("SELECT ISNULL(SUM(PRM_PR_COST),0) FROM PR_MSTR" _
        '    & " WHERE PRM_COY_ID= '" & strCoyID & "' AND PRM_PR_NO IN(" & strAllPR & ")")

        '    If blnPreview Then
        '        strSql = "DELETE FROM PO_DETAILS_PREVIEW WHERE POD_COY_ID='" & strCoyID & "' AND POD_PO_NO='" & strNewPONo & "'"
        '        Common.Insert2Ary(strAryQuery, strSql)
        '        strSql = "DELETE FROM PO_MSTR_PREVIEW WHERE POM_B_COY_ID='" & strCoyID & "' AND POM_PO_NO='" & strNewPONo & "'"
        '        Common.Insert2Ary(strAryQuery, strSql)
        '    End If

        '    If Not blnPreview Then
        '        strTermFile = objWheelFile.copyTermCondToPO(strNewPONo)
        '    Else
        '        strTermFile = ""
        '    End If

        '    CreatePOHeader(strAryPRNo(0), strVendor, strNewPONo, strPOPrefix, strAryQuery, dblTotalCost, strUserID, blnPreview, strTermFile, True, strRemark)
        '    intPOLine = 0
        '    For intLoop = 0 To strAryPRNo.GetUpperBound(0)
        '        CreatePODetail(strAryPRNo(intLoop), strAryPRIndex(intLoop), "", strNewPONo, strAryQuery, intPOLine, strUserID, blnPreview)
        '    Next
        '    If objDb.BatchExecute(strAryQuery) Then
        '        If blnPreview Then
        '            strMsg(0) = strNewPONo
        '        Else
        '            Dim objMail As New Email
        '            Dim objPR As New PR
        '            Dim strName As String

        '            For intLoop = 0 To strAryPRNo.GetUpperBound(0)
        '                strName = objPR.getRequestorName("PR", strAryPRNo(intLoop), strCoyID)
        '                objMail.sendNotification(EmailType.POCreated, strUserID, strCoyID, strVendor, strAryPRNo(intLoop), strNewPONo, strName)
        '            Next

        '            objPR = Nothing
        '            objMail.sendNotification(EmailType.PORaised, "", strCoyID, strVendor, strNewPONo, "")
        '            objMail = Nothing
        '            strMsg(0) = "Purchase Order Number " & strNewPONo & " has been created."
        '        End If
        '        Return True
        '    Else
        '        strMsg(0) = "Error occurs when creating PO. Kindly contact the Administrator to resolve this. "
        '        Return False
        '    End If
        '    objGlobal = Nothing
        'End Function

        ''//must pass in AO and cannot use session(userID) because of relief assignment
        'Function MassApproval(ByVal strAryPRNo() As String, ByVal strAryPRIndex() As String, ByVal strAO As String, ByRef strReturnMsg() As String, ByVal blnRelief As Boolean) As Boolean
        '    Dim strSql, strSql1, strAryQuery(0), strApprType As String
        '    Dim strCoyID, strAllPR, strAllPRIndex As String
        '    Dim intLoop As Integer
        '    Dim ds As DataSet
        '    Dim strMsg As String

        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    For intLoop = 0 To strAryPRNo.GetUpperBound(0)
        '        If intLoop = 0 Then
        '            strAllPR = "'" & strAryPRNo(intLoop) & "'"
        '            strAllPRIndex = strAryPRIndex(intLoop)
        '        Else
        '            strAllPR = strAllPR & ",'" & strAryPRNo(intLoop) & "'"
        '            strAllPRIndex = strAllPRIndex & "," & strAryPRIndex(intLoop)
        '        End If
        '    Next

        '    strSql = "SELECT PRM_PR_INDEX,PRM_PR_NO,PRM_S_COY_NAME,PRM_S_EMAIL,PRM_PR_STATUS,PRM_CONSOLIDATOR,PRM_STATUS_CHANGED_BY,PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO IN (" & strAllPR & ") And PRM_COY_ID='" & strCoyID & "'"
        '    strSql1 = "SELECT * FROM PR_APPROVAL WHERE PRA_PR_INDEX IN (" & strAllPRIndex & ") AND PRA_FOR = 'PR' "
        '    'strSql1 = "SELECT * FROM PR_APPROVAL WHERE (PRA_AO='" & strAO & "' OR PRA_A_AO='" & _
        '    'strAO & "') AND PRA_PR_INDEX IN (" & strAllPRIndex & ")"

        '    ds = objDb.FillDs(strSql & ";" & strSql1)
        '    ds.Tables(0).TableName = "PR_MSTR"
        '    ds.Tables(1).TableName = "PR_APPROVAL"

        '    If Not ds Is Nothing Then
        '        Dim parentCol As DataColumn
        '        Dim childCol As DataColumn
        '        Dim dvChildView, dvParentView As DataView
        '        Dim PRrow, PRApprrow As DataRow
        '        Dim intCurrentSeq, intLastSeq As Integer
        '        Dim blnHighestLevel, blnCanApprove As Boolean
        '        Dim strActiveAO As String


        '        parentCol = ds.Tables("PR_MSTR").Columns("PRM_PR_INDEX")
        '        childCol = ds.Tables("PR_APPROVAL").Columns("PRA_PR_INDEX")

        '        ' Create DataRelation.
        '        Dim relPR As DataRelation
        '        relPR = New DataRelation("acct", parentCol, childCol)
        '        ' Add the relation to the DataSet.
        '        ds.Relations.Add(relPR)
        '        For Each PRrow In ds.Tables("PR_MSTR").Rows
        '            blnCanApprove = True
        '            If PRrow("PRM_PR_STATUS") = PRStatus.PendingApproval Or
        '            PRrow("PRM_PR_STATUS") = PRStatus.Submitted Or
        '            PRrow("PRM_PR_STATUS") = PRStatus.HeldBy Then
        '                For Each PRApprrow In PRrow.GetChildRows(relPR)
        '                    intLastSeq = PRApprrow("PRA_AO_Action")
        '                    intCurrentSeq = intLastSeq + 1

        '                    '//check whether the PR was already approved by the same AO at the
        '                    '//same approving level.
        '                    If PRApprrow("PRA_Seq") = intCurrentSeq Then
        '                        strApprType = PRApprrow("PRA_APPROVAL_TYPE")
        '                        strActiveAO = Common.parseNull(PRApprrow("PRA_ACTIVE_AO"))
        '                        If strActiveAO <> "" Then
        '                            strMsg = "You have already approved/endorsed PR No. " & PRrow("PRM_PR_NO") & ". Approving of this PR is not allowed."
        '                            Common.Insert2Ary(strReturnMsg, strMsg)
        '                            blnCanApprove = False
        '                            Exit For
        '                        Else
        '                            If Not (UCase(PRApprrow("PRA_AO")) = UCase(strAO) Or
        '                             UCase(Common.parseNull(PRApprrow("PRA_A_AO"))) = UCase(strAO)) Then
        '                                strMsg = "You are not a authorised person to approve PR No. " & PRrow("PRM_PR_NO")
        '                                Common.Insert2Ary(strReturnMsg, strMsg)
        '                                blnCanApprove = False
        '                                Exit For
        '                            End If
        '                        End If
        '                    End If

        '                    ''//check whether login user is current AO                                            
        '                    'If PRApprrow("PRA_Seq") = intCurrentSeq Then
        '                    '    strApprType = Common.parseNull(PRApprrow("PRA_APPROVAL_TYPE"), "1")
        '                    '    If Not (UCase(PRApprrow("PRA_AO")) = UCase(strAO) Or _
        '                    '    UCase(Common.parseNull(PRApprrow("PRA_A_AO"))) = UCase(strAO)) Then
        '                    '        strMsg = "You are not allowed to approve PR No. " & PRrow("PRM_PR_NO")
        '                    '        Common.Insert2Ary(strReturnMsg, strMsg)
        '                    '        Exit For
        '                    '    End If
        '                    'End If
        '                    '//isHighestLevel
        '                    If intCurrentSeq = PRApprrow("PRA_SEQ") Then
        '                        blnHighestLevel = True
        '                    Else
        '                        blnHighestLevel = False
        '                    End If
        '                Next
        '                If blnCanApprove Then
        '                    If strApprType = "1" Then
        '                        strMsg = ApprovePR(PRrow("PRM_PR_NO"), PRrow("PRM_PR_INDEX"), intCurrentSeq, blnHighestLevel, Common.parseNull(PRrow("PRM_CONSOLIDATOR")), "Approved : ", Common.parseNull(PRrow("PRM_BUYER_ID")), blnRelief, strApprType, , "APP")
        '                    Else
        '                        strMsg = ApprovePR(PRrow("PRM_PR_NO"), PRrow("PRM_PR_INDEX"), intCurrentSeq, blnHighestLevel, Common.parseNull(PRrow("PRM_CONSOLIDATOR")), "Endorsed : ", Common.parseNull(PRrow("PRM_BUYER_ID")), blnRelief, strApprType)
        '                    End If

        '                    Common.Insert2Ary(strReturnMsg, strMsg)
        '                End If
        '            Else '//ERROR
        '                '//PR APPROVED/REJECTED/CANCELLED                       
        '                If PRrow("PRM_PR_STATUS") = PRStatus.CancelledBy Then
        '                    strMsg = "PR No. " & PRrow("PRM_PR_NO") & " has already been cancelled. Approving of this PR is not allowed. "
        '                ElseIf PRrow("PRM_PR_STATUS") = PRStatus.RejectedBy Then
        '                    strMsg = "You have already rejected PR No. " & PRrow("PRM_PR_NO") & ". Approving of this PR is not allowed. "
        '                ElseIf PRrow("PRM_PR_STATUS") = PRStatus.Approved Then
        '                    strMsg = "You have already approved PR No. " & PRrow("PRM_PR_NO") & " to PO. Approving of this PR is not allowed."
        '                ElseIf PRrow("PRM_PR_STATUS") = PRStatus.ConvertedToPO Then
        '                    strMsg = "You have already converted PR No. " & PRrow("PRM_PR_NO") & " to PO. Approving of this PR is not allowed."
        '                End If
        '                Common.Insert2Ary(strReturnMsg, strMsg)
        '            End If
        '        Next
        '    End If
        '    Return True
        'End Function

        'Function HasAttachment(ByVal strPRNo As String, Optional ByVal strType As String = "PR") As Boolean
        '    Dim strSql, strCoyID As String
        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strSql = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_DOC_TYPE='" & strType & "' "
        '    If objDb.Exist(strSql) >= 1 Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Function Need2PrintRemark(ByVal intPRIndex As Integer)
        '    Dim lsSql As String
        '    lsSql = "SELECT ISNULL(PRM_PRINT_REMARK,'1') FROM PR_MSTR WHERE PRM_PR_INDEX=" & intPRIndex
        '    If objDb.GetVal(lsSql) = "1" Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function
        'Function Need2PrintCField(ByVal intPRIndex As Integer)
        '    Dim lsSql As String
        '    lsSql = "SELECT PRM_PRINT_CUSTOM_FIELDS FROM PR_MSTR WHERE PRM_PR_INDEX=" & intPRIndex
        '    If objDb.GetVal(lsSql) = "1" Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Function Need2PrintPORemark(ByVal intPOIndex As Integer)
        '    Dim lsSql As String
        '    lsSql = "SELECT ISNULL(POM_PRINT_REMARK,'1') FROM po_mstr WHERE POM_PO_INDEX=" & intPOIndex
        '    If objDb.GetVal(lsSql) = "1" Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Function Need2PrintPOCField(ByVal intPOIndex As Integer, ByVal intPRIndex As Integer)
        '    Dim lsSql As String
        '    Dim strVal As String = ""

        '    lsSql = "SELECT ISNULL(POM_PRINT_CUSTOM_FIELDS,'') FROM po_mstr WHERE POM_PO_INDEX=" & intPOIndex
        '    strVal = objDb.GetVal(lsSql)
        '    If strVal = "" Then
        '        lsSql = "SELECT ISNULL(PRM_PRINT_CUSTOM_FIELDS,'0') FROM PR_MSTR WHERE PRM_PR_INDEX=" & intPRIndex
        '        If objDb.GetVal(lsSql) = "1" Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    ElseIf strVal = "1" Then
        '        Return True
        '    Else
        '        Return False
        '    End If

        'End Function

        'Sub getRFQName(ByVal strRFQID As String, ByRef strRFQNo As String, ByRef strRFQName As String)
        '    Dim strSql As String
        '    strSql = "SELECT RM_RFQ_No,RM_RFQ_Name FROM RFQ_MSTR WHERE RM_RFQ_ID=" & strRFQID
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        strRFQNo = Common.parseNull(tDS.Tables(0).Rows(0).Item("RM_RFQ_No"))
        '        strRFQName = Common.parseNull(tDS.Tables(0).Rows(0).Item("RM_RFQ_Name"))
        '    End If

        'End Sub

        'Function InsertToAMI(ByVal strPONO As String, ByVal strCoyID As String) As Boolean
        '    Dim objAMI As New EAD.DBCom("Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;PASSWORD=password;Initial Catalog=SUNDB;Data Source=hpnc60001;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Use Encryption for Data=False;Tag with column collation when possible=False")
        '    Dim strSql As String
        '    'strSql = "INSERT INTO SPMFORDDEM(ORDER_REF,SUPP_CODE,STATUS,ORDER_CD,TRANS_TYPE,COMMENTS,DEL_ADD,ORDER_DATE,ORDER_VAL,ORDER_QTY)" _
        '    '& "SELECT POM_PO_NO,POM_S_COY_ID,'00','D','0000000000',POM_PO_DATE,'POS', FROM PO_MSTR WHERE POM_PO_NO='" & strPONO & "' AND POM_B_COY_ID='" & strCoyID & "'"
        '    'strSql = "INSERT INTO SPMFDETDEM(REC_TYPE,TRANS_REF,TRANS_LINE,LOCATION,ITEM_CODE,ORD_STATUS,GRN_STATUS,INV_STATUS,DEL_DATE,PURCH_QTY,STOCK_QTY,BASE_COST,TRANS_TYPE)" _
        '    '& "'S',POD_PO_NO,POD_PO_LINE,POD_D_ADDR_CODE,POD_VENDOR_ITEM_CODE,'00','00','00','',POD_ORDERED_QTY,POD_ORDERED_QTY,POD_UNIT_COST)"
        '    ''yIN cHING - HIGH_REF=order no,LOCATION,ITM_TYPE=Q,TRANS_CD,VALUE1,VALUE2,VALUE...,
        '    ''CK - ITEM_CODE
        '    ''BLANK - LINK_TEXT,ID_GRN,ID_INV,SUPP_DESCR,SUPP_ITEM_CODE,PURCH_UNIT
        '    ''HARDCODE - ID_ORDER
        '    ''po_MSTR- ACK_REF - QUOTATION IF ANY
        '    strSql = "Select * from PO_Mstr WHERE POM_PO_NO='" & strPONO & "' AND POM_B_COY_ID='" & strCoyID & "';Select * from PO_Details WHERE POD_PO_NO='" & strPONO & "' AND POD_COY_ID='" & strCoyID & "'"
        '    Dim dsData As DataSet
        '    Dim drRow As DataRow
        '    Dim strPoLine As String
        '    Dim strDelvDate As String
        '    Dim intTotalOrder As Double
        '    dsData = objDb.FillDs(strSql)

        '    intTotalOrder = 0
        '    If dsData.Tables(1).Rows.Count > 0 Then
        '        For Each drRow In dsData.Tables(1).Rows
        '            intTotalOrder = intTotalOrder + drRow("POD_ORDERED_QTY")
        '            strPoLine = Strings.StrDup(3 - Convert.ToString(drRow("POD_PO_LINE")).Length, "0") & Convert.ToString(drRow("POD_PO_LINE"))
        '            If drRow("POD_ETD") = 0 Then
        '                strDelvDate = Format(dsData.Tables(0).Rows(0)("POM_PO_DATE"), "yyyyMMdd")
        '            Else
        '                strDelvDate = Format(DateAdd(DateInterval.Day, drRow("POD_ETD"), dsData.Tables(0).Rows(0)("POM_PO_DATE")), "yyyyMMdd")
        '            End If
        '            strSql = "INSERT INTO SPMFDETDEM(REC_TYPE,TRANS_REF,TRANS_LINE,HIGH_REF,LOCATION,ITEM_CODE,ITM_TYPE,ORD_STATUS,GRN_STATUS,INV_STATUS,DEL_DATE,PURCH_QTY,STOCK_QTY,BASE_COST,TRANS_TYPE)" _
        '            & "VALUES('S','" & drRow("POD_PO_NO") & "','" & strPoLine & "','" _
        '            & drRow("POD_PO_NO") & "','" & drRow("POD_D_ADDR_CODE") & "','" _
        '            & drRow("POD_VENDOR_ITEM_CODE") & "','Q','00','00','00'," _
        '            & strDelvDate & "," & drRow("POD_ORDERED_QTY") & "," &
        '            drRow("POD_ORDERED_QTY") & "," & drRow("POD_UNIT_COST") & ",'?')"
        '        Next
        '    End If
        '    If dsData.Tables(0).Rows.Count > 0 Then
        '        With dsData.Tables(0).Rows(0)
        '            strSql = "INSERT INTO SPMFORDDEM(ORDER_REF,SUPP_CODE,ORDER_DATE,STATUS,ORDER_CD,TRANS_TYPE,DEL_ADD,COMMENTS,ORDER_VAL,ORDER_QTY) VALUES('" _
        '             & .Item("POM_PO_NO") & "','" & .Item("POM_S_COY_ID") & "'," & Format(.Item("POM_PO_DATE"), "yyyyMMdd") _
        '             & ",'00','D','?','0000000000','" & .Item("POM_EXTERNAL_REMARK") &
        '             "'," & .Item("POM_PO_COST") & "," & intTotalOrder & ")"
        '        End With
        '    End If
        'End Function

        ''//caller= View Requisition by buyer or buyer admin
        'Function SearchPRListAll(ByVal strPRNo As String, ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strRole As String, Optional ByVal strStatus As String = "", Optional ByVal strPRType As String = "", Optional ByVal strBuyer As String = "", Optional ByVal strDept As String = "") As DataSet

        '    Dim strSql, strCondition As String
        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    Dim ds As DataSet


        '    'Michelle (21/9/2010) - To change the syntax to cater for MSSQL & MYSQL
        '    strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, PM.PRM_SUBMIT_DATE, " _
        '    & "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME," _
        '    & "PM.PRM_S_COY_NAME ,PM.PRM_REQ_NAME,PRM_PR_STATUS,PRM_PR_TYPE," _
        '    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS_DESC," _
        '    & " PRM_PR_COST AS PR_AMT,PM.PRM_RFQ_INDEX, UM1.UM_USER_NAME " _
        '    & ",PRM_PO_INDEX, (SELECT POM_PO_NO FROM PO_MSTR POM WHERE POM_PO_INDEX=PM.PRM_PO_INDEX) as PO_NO,PRM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, PM.PRM_URGENT " _
        '    & "FROM PR_MSTR PM LEFT OUTER JOIN COMPANY_DEPT_MSTR CD ON CD.CDM_DEPT_INDEX=PM.PRM_Dept_Index " _
        '    & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID=PM.PRM_BUYER_ID " _
        '    & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '    & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.PRM_STATUS_CHANGED_BY " _
        '    & "AND UM.UM_COY_ID='" & strCoyId & "'" 'WHERE PM.PRM_COY_ID='" & strCoyId & "'"
        '    'strSql = "SELECT PM.PRM_PR_Index,PM.PRM_PR_No,PM.PRM_S_Coy_ID, PM.PRM_PR_Date, " _
        '    '& "PM.PRM_CURRENCY_CODE,CD.CDM_DEPT_NAME, (CASE WHEN PRM_S_COY_NAME IS NULL THEN 'MULTIPLE VENDORS'  " _
        '    '& "WHEN PM.PRM_S_COY_NAME = '' THEN 'Multiple Vendors' ELSE PM.PRM_S_COY_NAME END) as PRM_S_COY_NAME,PM.PRM_REQ_NAME,PRM_PR_STATUS," _




        '    strSql = strSql & " WHERE PM.PRM_COY_ID='" & Common.Parse(strCoyId) & "'"


        '    '//nobody except buyer himself can view his draft PR
        '    If strRole = "ADMIN" Then
        '        strSql = strSql & " AND PM.PRM_PR_Status <> " & PRStatus.Draft
        '    End If

        '    If strPRNo <> "" Then
        '        strSql = strSql & " AND PM.PRM_PR_No LIKE '%" & Common.Parse(strPRNo) & "%'"
        '    End If

        '    If strPRType = "CC" Then
        '        strSql = strSql & " AND PM.PRM_PR_TYPE = '" & Common.Parse(strPRType) & "'"
        '    ElseIf strPRType = "NonCont" Then
        '        strSql = strSql & " AND (PM.PRM_PR_TYPE IS NULL OR PM.PRM_PR_TYPE = '')"
        '    End If

        '    'If dteDateFr <> "" Then
        '    '    strSql = strSql & " AND (PRM_CREATED_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & " OR PRM_SUBMIT_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & " OR PRM_PR_Date>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & ")"
        '    'End If

        '    'If dteDateTo <> "" Then
        '    '    strSql = strSql & " AND (PRM_CREATED_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & " OR PRM_SUBMIT_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & " OR PRM_PR_Date<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & ")"
        '    'End If

        '    If dteDateFr <> "" Then
        '        strSql = strSql & " AND (PRM_SUBMIT_DATE>=" & Common.ConvertDate(dteDateFr & " 00:00:00") & ")"
        '    End If

        '    If dteDateTo <> "" Then
        '        strSql = strSql & " AND (PRM_SUBMIT_DATE<=" & Common.ConvertDate(dteDateTo & " 23:59:59") & ")"
        '    End If

        '    If strStatus <> "" Then
        '        strSql = strSql & " AND PM.PRM_PR_Status IN (" & strStatus & ")"
        '    End If

        '    If strBuyer <> "" Then
        '        strSql = strSql & " AND PM.PRM_REQ_NAME" & Common.ParseSQL(strBuyer)
        '    End If

        '    If strDept <> "" Then
        '        strSql = strSql & " AND CD.CDM_DEPT_NAME" & Common.ParseSQL(strDept)
        '    End If

        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function

        'Public Function ValidatePOUserAssign(ByVal strPRNO As String) As Boolean
        '    Dim strsql As String
        '    Dim dsPO As New DataSet

        '    strsql = "SELECT DISTINCT ua_user_id FROM user_assign INNER JOIN user_mstr ON ua_coy_id = um_coy_id AND ua_user_id = um_user_id WHERE ua_fixed_role = 'Purchasing Officer' AND ua_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND ua_type = 'CT' AND um_deleted <> 'Y' AND ua_assign_value IN (SELECT distinct CT_ROOT_PREFIX FROM PR_DETAILS INNER JOIN commodity_type ON ct_id = prd_ct_id WHERE PRD_PR_NO = '" & strPRNO & "') "

        '    dsPO = objDb.FillDs(strsql)


        '    If dsPO.Tables(0).Rows.Count > 0 Then
        '        Return True
        '    End If

        'End Function

    End Class
End Namespace

