Imports System
Imports System.Web
Imports System.Web.UI.WebControls

Namespace AgoraNEO
    Public Enum EnumBCMAction
        SubmitPR
        CancelPR
        RejectPR
        AcceptPO
        CancelPO_BF
        RejectPO
        CancelPO_AF
        InvoiceCreated
        SubmitPO
    End Enum
    Public Class BudgetControl

        Dim strMessage As String
        'Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Public Property Message() As String
            Get
                Message = strMessage
            End Get
            Set(ByVal Value As String)
                strMessage = Value
            End Set
        End Property

        'Public Function SearcAccMstr(ByVal pDept As String,
        '                             ByVal pLevel As Integer,
        '                             ByVal pSearchCriteria As String,
        '                             Optional ByVal pParent As String = "") As DataSet
        '    Dim strSQL As String
        '    Dim ds As DataSet
        '    Dim objDb As New EAD.DBCom
        '    strSQL = "SELECT AM_ACCT_CODE,AM_ACCT_DESC,AM_INIT_BUDGET from ACCOUNT_MSTR WHERE  AM_DELETED='N' AND AM_DEPT_INDEX =" & Common.Parse(pDept) & " AND AM_LEVEL=" & Common.Parse(pLevel) & " AND AM_COY_ID='" & ctx.Session("CompanyId") & "'"

        '    If pSearchCriteria <> "" Then
        '        strSQL &= " AND AM_ACCT_CODE " & Common.ParseSQL(pSearchCriteria)
        '    End If

        '    If pParent <> "" Then
        '        Dim str As String = " SELECT AM_ACCT_INDEX FROM ACCOUNT_MSTR WHERE AM_DELETED='N' AND AM_ACCT_CODE='" & Common.Parse(pParent) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDept) & "' AND AM_LEVEL=" & pLevel - 1 & "  AND AM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '        Dim sParent As String = objDb.GetVal(str)
        '        strSQL &= " AND AM_PARENT_ACCT_INDEX='" & Common.Parse(sParent) & "'"
        '    End If


        '    strSQL &= " ORDER BY AM_ACCT_CODE"
        '    ds = objDb.FillDs(strSQL)
        '    Return ds
        'End Function


        '//Need to pass strBCoy for
        '//pEnumBCMAction.RejectPO,pEnumBCMAction.AcceptPO,pEnumBCMAction.InvoiceCreated()
        'Andhika 22-04-2021'
        Public Sub BCMCalc(ByVal strDocType As String, ByVal CompanyID As String, ByVal strDocNo As String, ByVal pEnumBCMAction As EnumBCMAction, ByRef strAryQuery() As String, Optional ByVal strBCoy As String = "", Optional ByVal strInvSql As String = "")

            Dim strSql, strCoyID, strYear As String
            Dim objDB As New EAD.DBCom
            Dim tDS As DataSet
            Dim strAcct As String
            Dim dblAmt As Double

            strCoyID = CompanyID
            Select Case strDocType
                Case "PR"
                    strSql = "SELECT PRD_ACCT_INDEX,SUM((PRD_ORDERED_QTY*PRD_UNIT_COST) + " _
                    & "((PRD_ORDERED_QTY*PRD_UNIT_COST)* (PRD_GST/100))) AS amt, PRM_SUBMIT_DATE as doc_dt  FROM PR_DETAILS " _
                    & "INNER JOIN PR_MSTR ON (PR_DETAILS.PRD_PR_NO = PR_MSTR.PRM_PR_NO AND PR_DETAILS.PRD_COY_ID = PR_MSTR.PRM_COY_ID) " _
                    & "WHERE PRD_COY_ID='" & strCoyID & "' AND PRD_PR_NO='" & strDocNo & "' GROUP BY PRD_ACCT_INDEX, PRM_PR_DATE"
                Case "PO" 'for AcceptPO, RejectPO
                    '//Buyer has to cancel full PO before Vendor acceptance,so we can use POD_ORDERED_QTY to calc BCM
                    strSql = "SELECT POD_ACCT_INDEX,SUM((POD_ORDERED_QTY*POD_UNIT_COST) + " _
                    & "((POD_ORDERED_QTY*POD_UNIT_COST)* (POD_GST/100))) AS amt, POM_SUBMIT_DATE as doc_dt, CM_BUDGET_FROM_DATE as budgets_dt FROM PO_DETAILS " _
                    & "INNER JOIN PO_MSTR ON (PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID) " _
                    & "INNER JOIN COMPANY_MSTR ON COMPANY_MSTR.CM_COY_ID =  PO_MSTR.POM_B_COY_ID "
                    If strBCoy = "" Then
                        strSql = strSql & "WHERE POM_B_COY_ID='" & strCoyID & "' AND POM_PO_NO='" & strDocNo & "' GROUP BY POD_ACCT_INDEX, POM_PO_DATE, CM_BUDGET_FROM_DATE"
                    Else
                        strSql = strSql & "WHERE POD_COY_ID='" & strBCoy & "' AND POD_PO_NO='" & strDocNo & "' GROUP BY POD_ACCT_INDEX, POM_PO_DATE, CM_BUDGET_FROM_DATE"
                    End If
                Case "CR"
                    '//For PO cancellation
                    '//PO cancellation after PO accepted only for those outstanding items.
                    ' strSql = "SELECT POD_ACCT_INDEX, SUM(PCD_CANCELLED_QTY*POD_UNIT_COST) AS amt " _
                    ' & "FROM PO_CR_MSTR LEFT JOIN PO_CR_DETAILS ON PCM_CR_NO = PCD_CR_NO AND PCM_B_COY_ID = PCD_COY_ID " _
                    ' & "LEFT JOIN PO_DETAILS ON POD_COY_ID = PCM_B_COY_ID AND PCD_PO_LINE = POD_PO_LINE " _
                    '& "WHERE POD_COY_ID = '" & strCoyID & "' AND PCM_CR_NO = '" & strDocNo & "' GROUP BY POD_ACCT_INDEX"
                    strSql = "SELECT POD_ACCT_INDEX, SUM((PCD_CANCELLED_QTY*POD_UNIT_COST)  + " _
                    & "((PCD_CANCELLED_QTY*POD_UNIT_COST)* (POD_GST/100))) AS amt, POM_SUBMIT_DATE as doc_dt " &
                    "FROM PO_CR_MSTR INNER JOIN PO_CR_DETAILS ON PCM_CR_NO = PCD_CR_NO AND PCM_B_COY_ID = PCD_COY_ID " &
                    "INNER JOIN PO_MSTR ON POM_PO_INDEX=PCM_PO_INDEX " &
                    "INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO=POM_PO_NO " &
                    "AND PCD_PO_LINE = POD_PO_LINE WHERE POD_COY_ID = '" & strCoyID & "' AND PCM_CR_NO = '" & strDocNo & "' GROUP BY POD_ACCT_INDEX, POM_PO_DATE"
                Case "INV"
                    '//Remark By Moo
                    '//cant directly get from Invoice Table because "Insert to Invoice_detail" not commited yet
                    '//when BCM is called
                    'strSql = "SELECT ID_ACCT_INDEX, SUM(ID_RECEIVED_QTY * ID_UNIT_COST) AS amt FROM INVOICE_DETAILS " _
                    '& "WHERE ID_INVOICE_NO = '" & strDocNo & "'AND ID_S_COY_ID='" & strCoyID & "' GROUP BY ID_ACCT_INDEX "
                    strSql = strInvSql
            End Select

            Select Case pEnumBCMAction
                Case pEnumBCMAction.SubmitPR '"SubmitPR"
                    '//refer to CheckBCM

                    tDS = objDB.FillDs(strSql)
                    For i As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(i).Item("PRD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(i).Item("PRD_ACCT_INDEX")) <> "" Then
                            strAcct = Common.Parse(tDS.Tables(0).Rows(i).Item("PRD_ACCT_INDEX"))
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(i).Item("amt"), 0)
                            strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT = AM_RESERVED_AMT + " & dblAmt &
                                " WHERE AM_COY_ID = '" & strCoyID & "' AND AM_ACCT_INDEX = " & strAcct
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next

                Case pEnumBCMAction.SubmitPO
                    '//refer to CheckBCM

                    tDS = objDB.FillDs(strSql)
                    For i As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(i).Item("POD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(i).Item("POD_ACCT_INDEX")) <> "" Then
                            strAcct = Common.Parse(tDS.Tables(0).Rows(i).Item("POD_ACCT_INDEX"))
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(i).Item("amt"), 0)
                            strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT = AM_RESERVED_AMT + " & dblAmt &
                                " WHERE AM_COY_ID = '" & strCoyID & "' AND AM_ACCT_INDEX = " & strAcct
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next

                Case pEnumBCMAction.CancelPR, pEnumBCMAction.RejectPR '"CancelPR", "RejectPR"
                    'Michelle (3/5/2010) - Check whether should deduct from last year or this year budget
                    Dim strBCM As String
                    Dim objBCM As New BudgetControl
                    Dim drw As DataView
                    Dim dtBudgetS As DateTime = Now()
                    Dim dtDoc As DateTime

                    drw = objBCM.GetBCM()

                    If Not drw Is Nothing Then
                        dtBudgetS = drw.Table.Rows(0)("CM_BUDGET_FROM_DATE")
                    End If
                    tDS = objDB.FillDs(strSql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(j).Item("PRD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(j).Item("PRD_ACCT_INDEX")) <> "" Then
                            strAcct = tDS.Tables(0).Rows(j).Item("PRD_ACCT_INDEX")
                            dtDoc = tDS.Tables(0).Rows(j).Item("doc_dt")
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("amt"), 0)

                            If dtDoc >= dtBudgetS Then
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt
                            Else 'Deduct from the BCF
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_BCF = AM_BCF - " & dblAmt &
                                         ", AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt
                            End If

                            strSql = strSql & " WHERE AM_COY_ID='" & strCoyID & "'AND AM_ACCT_INDEX='" & strAcct & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next
                Case pEnumBCMAction.CancelPO_BF '"CancelPO"
                    'Michelle (3/5/2010) - Check whether should deduct from last year or this year budget
                    Dim strBCM As String
                    Dim objBCM As New BudgetControl
                    Dim drw As DataView
                    Dim dtBudgetS As DateTime = Now()
                    Dim dtDoc As DateTime

                    drw = objBCM.GetBCM()

                    If Not drw Is Nothing Then
                        dtBudgetS = drw.Table.Rows(0)("CM_BUDGET_FROM_DATE")
                    End If

                    tDS = objDB.FillDs(strSql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) <> "" Then
                            strAcct = tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("amt"), 0)
                            dtDoc = tDS.Tables(0).Rows(j).Item("doc_dt")
                            If dtDoc >= dtBudgetS Then
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt
                            Else 'Deduct from the BCF
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_BCF = AM_BCF - " & dblAmt &
                                         ", AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt
                            End If

                            strSql = strSql & " WHERE AM_COY_ID='" & strCoyID & "'AND AM_ACCT_INDEX='" & strAcct & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next
                Case pEnumBCMAction.AcceptPO '"AcceptPO"
                    '//take out "auto-gen" account id
                    tDS = objDB.FillDs(strSql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) <> "" Then
                            strAcct = tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("amt"), 0)
                            strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt &
                            ",AM_COMMITTED_AMT=AM_COMMITTED_AMT+" & dblAmt & " WHERE AM_COY_ID='" &
                            strBCoy & "'AND AM_ACCT_INDEX='" & strAcct & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next
                Case pEnumBCMAction.RejectPO
                    'Michelle (3/5/2010) - Check whether should deduct from last year or this year budget
                    Dim dtBudgetS As DateTime = Now()
                    Dim dtDoc As DateTime

                    tDS = objDB.FillDs(strSql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) <> "" Then
                            strAcct = tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("amt"), 0)
                            dtDoc = tDS.Tables(0).Rows(j).Item("doc_dt")
                            dtBudgetS = tDS.Tables(0).Rows(j).Item("budgets_dt")
                            If dtDoc >= dtBudgetS Then
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt
                            Else
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_BCF = AM_BCF - " & dblAmt &
                                         ", AM_RESERVED_AMT=AM_RESERVED_AMT-" & dblAmt

                            End If
                            strSql = strSql & " WHERE AM_COY_ID='" & strBCoy & "'AND AM_ACCT_INDEX='" & strAcct & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next
                Case pEnumBCMAction.CancelPO_AF '"CancelPO_AF" '//may need to change,PO cancellation only for those outstanding items.
                    'Michelle (3/5/2010) - Check whether should deduct from last year or this year budget
                    Dim strBCM As String
                    Dim objBCM As New BudgetControl
                    Dim drw As DataView
                    Dim dtBudgetS As DateTime = Now()
                    Dim dtDoc As DateTime

                    drw = objBCM.GetBCM()

                    If Not drw Is Nothing Then
                        dtBudgetS = drw.Table.Rows(0)("CM_BUDGET_FROM_DATE")
                    End If

                    tDS = objDB.FillDs(strSql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) <> "" Then
                            strAcct = tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("amt"), 0)
                            dtDoc = tDS.Tables(0).Rows(j).Item("doc_dt")
                            If dtDoc >= dtBudgetS Then
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_COMMITTED_AMT=AM_COMMITTED_AMT-" & dblAmt
                            Else
                                strSql = "UPDATE ACCOUNT_MSTR SET AM_BCF=AM_BCF-" & dblAmt &
                                         ", AM_COMMITTED_AMT=AM_COMMITTED_AMT-" & dblAmt
                            End If
                            strSql = strSql & " WHERE AM_COY_ID='" & strCoyID & "'AND AM_ACCT_INDEX='" & strAcct & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next
                Case pEnumBCMAction.InvoiceCreated
                    tDS = objDB.FillDs(strSql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) AndAlso CStr(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) <> "" Then
                            strAcct = tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")
                            dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("amt"), 0)
                            strSql = "UPDATE ACCOUNT_MSTR SET AM_COMMITTED_AMT = AM_COMMITTED_AMT - " & dblAmt &
                            ", AM_UTILIZED_AMT = AM_UTILIZED_AMT + " & dblAmt & " WHERE AM_COY_ID='" & strBCoy & "'AND AM_ACCT_INDEX='" & strAcct & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Next
            End Select
        End Sub
        'Andhika 22-04-2021'
        Function checkBCM(ByVal strDocNo As String, ByVal CompanyID As String, ByRef dt As DataTable, ByRef strBCM As String) As Boolean
            Dim objDB As New EAD.DBCom
            Dim strSql, strCoyID, strAcctCode As String
            Dim intAcct As Integer
            Dim dblAmt, dblOpBudget As Double
            'Dim htBCM As New Hashtable
            Dim blnUpdate As Boolean
            Dim dtr As DataRow
            Dim blnExceed As Boolean = False

            dt.Columns.Add("Acct_Index", Type.GetType("System.Int32"))
            dt.Columns.Add("Acct_Code", Type.GetType("System.String"))
            dt.Columns.Add("Acct_Amount", Type.GetType("System.Double"))

            strCoyID = CompanyID
            strSql = "SELECT ISNULL(CM_BCM_SET,0) FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "'"
            strBCM = objDB.GetVal(strSql)

            strSql = "SELECT PRD_ACCT_INDEX, AM_ACCT_CODE, SUM((PRD_ORDERED_QTY*PRD_UNIT_COST) + " _
            & "((PRD_ORDERED_QTY*PRD_UNIT_COST)* (PRD_GST/100))) AS AMT FROM PR_DETAILS " _
            & "LEFT JOIN ACCOUNT_MSTR ON PRD_ACCT_INDEX = AM_ACCT_INDEX " _
            & "WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_PR_NO = '" & strDocNo & "' GROUP BY PRD_ACCT_INDEX, AM_ACCT_CODE "

            Dim tDS As DataSet = objDB.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                blnUpdate = True
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("PRD_ACCT_INDEX")) Then
                    intAcct = tDS.Tables(0).Rows(j).Item("PRD_ACCT_INDEX")
                    strAcctCode = tDS.Tables(0).Rows(j).Item("AM_ACCT_CODE")
                    dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("AMT"), 0)

                    '// 1 - absolute , 2 - advisory
                    If strBCM = "1" Or strBCM = "2" Then '//advisory- warn,absolute-prompt
                        'Michelle (3/5/2010) - Operating Budget = Initial + BCF - Reserved - Committed - Utilized
                        'strSql = "SELECT ISNULL(SUM(AM_Init_Budget - AM_Reserved_Amt - AM_Committed_Amt - " _
                        strSql = "SELECT IFNULL(SUM(AM_Init_Budget + IFNULL(AM_BCF,0) - AM_Reserved_Amt - AM_Committed_Amt - " _
                         & "AM_Utilized_Amt),0) FROM ACCOUNT_MSTR WHERE AM_COY_ID = '" & strCoyID &
                         "' AND AM_ACCT_INDEX = " & intAcct
                        dblOpBudget = objDB.GetValDecimal(strSql)
                        If dblOpBudget < dblAmt Then
                            blnExceed = True
                            dtr = dt.NewRow()
                            dtr("Acct_Index") = intAcct
                            dtr("Acct_Code") = strAcctCode
                            dtr("Acct_Amount") = -(dblOpBudget - dblAmt)
                            dt.Rows.Add(dtr)
                            'If strBCM = 1 Then
                            '    blnUpdate = False
                            'Else
                            '    blnUpdate = True
                            'End If
                        End If
                    End If

                    'If blnUpdate Then
                    '    strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT = AM_RESERVED_AMT + " & dblAmt & _
                    '    " WHERE AM_COY_ID = '" & strCoyID & "' AND AM_ACCT_INDEX = " & intAcct
                    '    Common.Insert2Ary(strAryQuery, strSql)
                    'End If
                End If
            Next
            Return blnExceed
        End Function

        Function checkBCMPO(ByVal strDocNo As String, ByRef dt As DataTable, ByRef strBCM As String) As Boolean
            Dim objDB As New EAD.DBCom
            Dim strSql, strCoyID, strAcctCode As String
            Dim intAcct As Integer
            Dim dblAmt, dblOpBudget As Double
            'Dim htBCM As New Hashtable
            Dim blnUpdate As Boolean
            Dim dtr As DataRow
            Dim blnExceed As Boolean = False

            dt.Columns.Add("Acct_Index", Type.GetType("System.Int32"))
            dt.Columns.Add("Acct_Code", Type.GetType("System.String"))
            dt.Columns.Add("Acct_Amount", Type.GetType("System.Double"))

            strCoyID = HttpContext.Current.Session("CompanyId")
            strSql = "SELECT ISNULL(CM_BCM_SET,0) FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "'"
            strBCM = objDB.GetVal(strSql)

            'Dim strPO_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_CONVERT_TO_DOC = '" & strDocNo & "'")
            'If strPO_No <> "" Then
            '    strBCM = "999"
            'End If

            'strSql = "SELECT PRD_ACCT_INDEX, AM_ACCT_CODE, SUM((PRD_ORDERED_QTY*PRD_UNIT_COST) + " _
            '& "((PRD_ORDERED_QTY*PRD_UNIT_COST)* (PRD_GST/100))) AS AMT FROM PR_DETAILS " _
            '& "LEFT JOIN ACCOUNT_MSTR ON PRD_ACCT_INDEX = AM_ACCT_INDEX " _
            '& "WHERE PRD_COY_ID = '" & strCoyID & "' AND PRD_PR_NO = '" & strDocNo & "' GROUP BY PRD_ACCT_INDEX, AM_ACCT_CODE "

            strSql = "SELECT POD_ACCT_INDEX, AM_ACCT_CODE, SUM((POD_ORDERED_QTY*POD_UNIT_COST) + " _
            & "((POD_ORDERED_QTY*POD_UNIT_COST)* (POD_GST/100))) AS AMT FROM PO_DETAILS " _
            & "LEFT JOIN ACCOUNT_MSTR ON POD_ACCT_INDEX = AM_ACCT_INDEX " _
            & "WHERE POD_COY_ID = '" & strCoyID & "' AND POD_PO_NO = '" & strDocNo & "' GROUP BY POD_ACCT_INDEX, AM_ACCT_CODE "

            Dim tDS As DataSet = objDB.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                blnUpdate = True
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")) Then
                    intAcct = tDS.Tables(0).Rows(j).Item("POD_ACCT_INDEX")
                    strAcctCode = tDS.Tables(0).Rows(j).Item("AM_ACCT_CODE")
                    dblAmt = Common.parseNull(tDS.Tables(0).Rows(j).Item("AMT"), 0)

                    '// 1 - absolute , 2 - advisory
                    If strBCM = "1" Or strBCM = "2" Then '//advisory- warn,absolute-prompt
                        'Michelle (3/5/2010) - Operating Budget = Initial + BCF - Reserved - Committed - Utilized
                        'strSql = "SELECT ISNULL(SUM(AM_Init_Budget - AM_Reserved_Amt - AM_Committed_Amt - " _
                        strSql = "SELECT ISNULL(SUM(AM_Init_Budget + IFNULL(AM_BCF,0) - AM_Reserved_Amt - AM_Committed_Amt - " _
                         & "AM_Utilized_Amt),0) FROM ACCOUNT_MSTR WHERE AM_COY_ID = '" & strCoyID &
                         "' AND AM_ACCT_INDEX = " & intAcct
                        dblOpBudget = objDB.GetVal(strSql)
                        If dblOpBudget < dblAmt Then
                            blnExceed = True
                            dtr = dt.NewRow()
                            dtr("Acct_Index") = intAcct
                            dtr("Acct_Code") = strAcctCode
                            dtr("Acct_Amount") = -(dblOpBudget - dblAmt)
                            dt.Rows.Add(dtr)
                            'If strBCM = 1 Then
                            '    blnUpdate = False
                            'Else
                            '    blnUpdate = True
                            'End If
                        End If
                    End If

                    'If blnUpdate Then
                    '    strSql = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT = AM_RESERVED_AMT + " & dblAmt & _
                    '    " WHERE AM_COY_ID = '" & strCoyID & "' AND AM_ACCT_INDEX = " & intAcct
                    '    Common.Insert2Ary(strAryQuery, strSql)
                    'End If
                End If
            Next
            Return blnExceed
        End Function

        'Public Function getBCMList() As DataTable
        '    Dim ob As New EAD.DBCom(ConfigurationManager.AppSettings("SHAPE"))
        '    'Dim strSql = "Shape{select * from account_mstr where am_parent_acct_index is null} append ({select * from account_mstr where am_parent_acct_index is not null} relate AM_ACCT_INDEX to AM_PARENT_ACCT_INDEX)"
        '    'Dim strSql = "Shape{select * from account_mstr where am_parent_acct_index is null AND AM_LEVEL=1} append " _
        '    '& "((Shape{select * from account_mstr where am_parent_acct_index is not null AND AM_LEVEL=2} append " _
        '    '& "({select * from account_mstr where am_parent_acct_index is not null AND AM_LEVEL=3} " _
        '    '& "relate AM_ACCT_INDEX to AM_PARENT_ACCT_INDEX)) " _
        '    '& "relate AM_ACCT_INDEX to AM_PARENT_ACCT_INDEX)"

        '    '   > > strQuery=SHAPE {SELECT * FROM TABLE1 WHERE REFID=''} AS TopLevel APPEND
        '    '> > (( SHAPE {SELECT * FROM TABLE1} AS SecondLevel APPEND ({SELECT * FROM
        '    '> > TABLE1} AS ThirdLevel RELATE 'CID' TO 'REFID') AS ThirdLevel) AS
        '    '> > SecondLevel RELATE 'CID' TO 'REFID') AS SecondLevel

        '    Dim strSql1, strSql2, strSql3, strSql4 As String


        '    strSql4 = "Shape{select * from account_mstr where am_parent_acct_index is not null AND AM_LEVEL=2} append " _
        '    & "({select * from account_mstr where am_parent_acct_index is not null AND AM_LEVEL=3} " _
        '    & "relate AM_ACCT_INDEX to AM_PARENT_ACCT_INDEX)"

        '    strSql3 = "Shape{select * from account_mstr where am_parent_acct_index is null AND AM_LEVEL=1} append " _
        '    & "(( " & strSql4 & " ) " _
        '    & " relate AM_ACCT_INDEX to AM_PARENT_ACCT_INDEX) "

        '    strSql2 = "Shape{select * from company_dept_mstr where cdm_coy_id='mbb'} append " _
        '    & "(( " & strSql3 & " ) " _
        '    & " relate CDM_DEPT_INDEX to AM_DEPT_INDEX) "

        '    'strSql1 = "Shape{select * from company_dept_mstr} append "
        '    Dim dtLevel1 As DataTable
        '    Dim drLevel1, drLevel2, drLevel3, drLevel4, drResult As DataRow
        '    Dim drsLevel2, drsLevel3, drsLevel4 As DataRow()
        '    dtLevel1 = ob.FillDs(strSql2).Tables(0)
        '    ' Dim dvMatch As DataView = dv.CreateChildView("tableChapter1")
        '    'Response.Write(Now)

        '    Dim dtResult As New DataTable
        '    dtResult.Columns.Add("Acct_List", Type.GetType("System.String"))
        '    dtResult.Columns.Add("Acct_Index", Type.GetType("System.Int32"))

        '    For Each drLevel1 In dtLevel1.Rows '//department
        '        drsLevel2 = drLevel1.GetChildRows("tableChapter1")
        '        If drsLevel2.Length > 0 Then '//account code
        '            For Each drLevel2 In drsLevel2
        '                '	drLevel2.TABLE.CHILDRELATIONS.ITEM(0).RelationName	"tableChapter1Chapter1"	String
        '                drsLevel3 = drLevel2.GetChildRows("tableChapter1Chapter1")
        '                ' drsLevel2.
        '                If drsLevel3.Length > 0 Then
        '                    For Each drLevel3 In drsLevel3
        '                        drsLevel4 = drLevel3.GetChildRows("tableChapter1Chapter1Chapter1")
        '                        If drsLevel4.Length > 0 Then
        '                            For Each drLevel4 In drsLevel4
        '                                'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "-" & drLevel3(2) & "-" & drLevel4(2) & "<br>")
        '                                drResult = dtResult.NewRow
        '                                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & _
        '                                "-" & drLevel2("AM_ACCT_DESC") & ":" & drLevel2("AM_ACCT_code") & _
        '                                "-" & drLevel3("AM_ACCT_DESC") & ":" & drLevel3("AM_ACCT_code") & _
        '                                "-" & drLevel4("AM_ACCT_DESC") & ":" & drLevel4("AM_ACCT_code")
        '                                drResult("Acct_Index") = drLevel2("AM_ACCT_INDEX")
        '                                dtResult.Rows.Add(drResult)
        '                            Next
        '                        Else '// no project name
        '                            'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "-" & drLevel3(2) & "<br>")
        '                            drResult = dtResult.NewRow
        '                            drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & _
        '                            "-" & drLevel2("AM_ACCT_DESC") & ":" & drLevel2("AM_ACCT_code") & _
        '                            "-" & drLevel3("AM_ACCT_DESC") & ":" & drLevel3("AM_ACCT_code")
        '                            drResult("Acct_Index") = drLevel2("AM_ACCT_INDEX")
        '                            dtResult.Rows.Add(drResult)
        '                        End If

        '                    Next
        '                Else '//No sub account
        '                    'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "<br>")
        '                    drResult = dtResult.NewRow
        '                    drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & "-" & drLevel2("AM_ACCT_DESC") & ":" & drLevel2("AM_ACCT_code")
        '                    drResult("Acct_Index") = drLevel2("AM_ACCT_INDEX")
        '                    dtResult.Rows.Add(drResult)
        '                End If
        '            Next
        '        End If
        '    Next
        '    Return dtResult
        'End Function

        Public Function getBCMListByUserFrmPR(ByVal strUser As String, Optional ByVal strCode As String = "") As DataSet

            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String

            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
            & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' AND AM_DELETED='N' ORDER BY CDM_DEPT_CODE"

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX " &
            " WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX " &
            " WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2  AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX " &
            " WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (" & strSqlLevel2 & ") Q left outer join " _
            & "(" & strSqlLevel3 & ") W " _
            & " ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.AM_DEPT_INDEX, E.AM_ACCT_INDEX, E.AM_ACCT_CODE, E.AM_ACCT_DESC, E.AM_PARENT_ACCT_INDEX, E.AU_USER_ID, R.Q_AM_DEPT_INDEX, R.Q_AM_ACCT_INDEX, R.Q_AM_ACCT_CODE, R.Q_AM_ACCT_DESC, Q_AM_PARENT_ACCT_INDEX, R.Q_AU_USER_ID, R.W_AM_DEPT_INDEX, R.W_AM_ACCT_INDEX, R.W_AM_ACCT_CODE, R.W_AM_ACCT_DESC, R.W_AM_PARENT_ACCT_INDEX, R.W_AU_USER_ID from (" & strSqlLevel1 & ") E left outer join " _
            & "( " & strSql4 & " ) R " _
            & " ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX"

            strSql2 = "select X.*, Y.* from (" & strSqlDept & ") X RIGHT OUTER JOIN " _
            & "( " & strSql3 & " ) Y " _
            & " ON X.CDM_DEPT_INDEX = Y.AM_DEPT_INDEX "

            'strSql1 = "Shape{select * from company_dept_mstr} append "
            Dim dtLevel1 As DataTable
            Dim dsBCM As DataSet
            Dim dsTempBCM As New DataSet
            dtLevel1 = ob.FillDs(strSql2).Tables(0)
            dsBCM = BuildBCMList(dtLevel1, True)

            Dim dtTempTable As New DataTable
            Dim dtTempTable1 As New DataTable
            If strCode <> "" Then
                Dim drFinalResult As DataRow()
                Dim drTemp As DataRow
                '//to make sure dtTempTable and dtResult have the same structure
                dtTempTable = dsBCM.Tables(0).Clone 'dtResult.Clone
                drFinalResult = dsBCM.Tables(0).Select("ACCT_code" & Common.ParseSQL(strCode))
                If drFinalResult.Length > 0 Then
                    For Each drTemp In drFinalResult
                        dtTempTable.ImportRow(drTemp)
                    Next
                    'Else
                    'dtTempTable = Nothing
                End If
            Else
                dtTempTable = dsBCM.Tables(0).Copy
            End If
            dsTempBCM.Tables.Add(dtTempTable) '//selected List
            dtTempTable1 = dsBCM.Tables(1).Copy
            dsTempBCM.Tables.Add(dtTempTable1) '//available list
            Return dsTempBCM
        End Function

        Public Function getBCMListByUser(ByVal strUser As String, Optional ByVal strCode As String = "") As DataSet

            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String

            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
            & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' AND AM_DELETED='N' ORDER BY CDM_DEPT_CODE"

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2  AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (" & strSqlLevel2 & ") Q inner join " _
            & "(" & strSqlLevel3 & ") W " _
            & " ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.AM_DEPT_INDEX, E.AM_ACCT_INDEX, E.AM_ACCT_CODE, E.AM_ACCT_DESC, E.AM_PARENT_ACCT_INDEX, E.AU_USER_ID, R.Q_AM_DEPT_INDEX, R.Q_AM_ACCT_INDEX, R.Q_AM_ACCT_CODE, R.Q_AM_ACCT_DESC, Q_AM_PARENT_ACCT_INDEX, R.Q_AU_USER_ID, R.W_AM_DEPT_INDEX, R.W_AM_ACCT_INDEX, R.W_AM_ACCT_CODE, R.W_AM_ACCT_DESC, R.W_AM_PARENT_ACCT_INDEX, R.W_AU_USER_ID from (" & strSqlLevel1 & ") E inner join " _
            & "( " & strSql4 & " ) R " _
            & " ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX"

            strSql2 = "select X.*, Y.* from (" & strSqlDept & ") X inner join " _
                            & "( " & strSql3 & " ) Y " _
                            & " ON X.CDM_DEPT_INDEX = Y.AM_DEPT_INDEX "

            'strSql1 = "Shape{select * from company_dept_mstr} append "
            Dim dtLevel1 As DataTable
            Dim dsBCM As DataSet
            Dim dsTempBCM As New DataSet
            dtLevel1 = ob.FillDs(strSql2).Tables(0)
            dsBCM = BuildBCMList(dtLevel1, True)

            Dim dtTempTable As New DataTable
            Dim dtTempTable1 As New DataTable
            If strCode <> "" Then
                Dim drFinalResult As DataRow()
                Dim drTemp As DataRow
                '//to make sure dtTempTable and dtResult have the same structure
                dtTempTable = dsBCM.Tables(0).Clone 'dtResult.Clone
                drFinalResult = dsBCM.Tables(0).Select("ACCT_code" & Common.ParseSQL(strCode))
                If drFinalResult.Length > 0 Then
                    For Each drTemp In drFinalResult
                        dtTempTable.ImportRow(drTemp)
                    Next
                    'Else
                    'dtTempTable = Nothing
                End If
            Else
                dtTempTable = dsBCM.Tables(0).Copy
            End If
            dsTempBCM.Tables.Add(dtTempTable) '//selected List
            dtTempTable1 = dsBCM.Tables(1).Copy
            dsTempBCM.Tables.Add(dtTempTable1) '//available list
            Return dsTempBCM
        End Function

        Public Function getBCMListByUser(ByVal strUser As String, ByVal strCode As String, ByVal strDesc As String) As DataSet

            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String

            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
            & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' AND AM_DELETED='N' ORDER BY CDM_DEPT_CODE"

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2  AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (" & strSqlLevel2 & ") Q INNER JOIN " _
            & "(" & strSqlLevel3 & ") W " _
            & "ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.AM_DEPT_INDEX, E.AM_ACCT_INDEX, E.AM_ACCT_CODE, E.AM_ACCT_DESC, E.AM_PARENT_ACCT_INDEX, E.AU_USER_ID, R.* from (" & strSqlLevel1 & ") E INNER JOIN " _
            & "( " & strSql4 & " ) R " _
            & "ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX"

            strSql2 = "select Y.*, U.* from (" & strSqlDept & ") Y INNER JOIN " _
            & "( " & strSql3 & " ) U " _
            & "ON Y.CDM_DEPT_INDEX = U.AM_DEPT_INDEX "

            'strSql1 = "Shape{select * from company_dept_mstr} append "
            Dim dtLevel1 As DataTable
            Dim dsBCM As DataSet
            Dim dsTempBCM As New DataSet
            dtLevel1 = ob.FillDs(strSql2).Tables(0)
            'dsBCM = BuildBCMList(dtLevel1, True)

            dsBCM = BuildBCMList(dtLevel1, strDesc, True)

            Dim dtTempTable As New DataTable
            Dim dtTempTable1 As New DataTable
            If strCode <> "" Or strDesc <> "" Then
                Dim drFinalResult As DataRow()
                Dim drTemp As DataRow
                '//to make sure dtTempTable and dtResult have the same structure
                dtTempTable = dsBCM.Tables(0).Clone    'dtResult.Clone

                Dim filterStr As String = ""

                If Not strCode.Equals("") Then
                    filterStr = filterStr & "ACCT_code" & Common.ParseSQL(strCode)
                    If Not strDesc.Equals("") Then
                        Dim parseDesc As String = Common.ParseSQL(strDesc)
                        filterStr &= "AND (L1 " & parseDesc & " OR L2 " & parseDesc & " OR L3 " & parseDesc + " )"
                    End If
                Else
                    Dim parseDesc As String = Common.ParseSQL(strDesc)
                    filterStr = filterStr & " L1 " & parseDesc & " OR L2 " & parseDesc & " OR L3 " & parseDesc
                End If

                drFinalResult = dsBCM.Tables(0).Select(filterStr)

                'drFinalResult = dsBCM.Tables(0).Select("ACCT_code" & Common.ParseSQL(strCode))
                If drFinalResult.Length > 0 Then
                    For Each drTemp In drFinalResult
                        dtTempTable.ImportRow(drTemp)
                    Next
                End If
            Else
                dtTempTable = dsBCM.Tables(0).Copy
            End If
            dsTempBCM.Tables.Add(dtTempTable)    '//selected List
            dtTempTable1 = dsBCM.Tables(1).Copy
            dsTempBCM.Tables.Add(dtTempTable1)    '//available list
            Return dsTempBCM
        End Function

        Public Function getBCMListByUserNew(ByVal strUser As String, ByVal companyID As String, ByVal strCode As String, ByVal strDesc As String, ByVal Optional getStrSQL As Boolean = True) As Result
            Dim Res = New Result
            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = companyID

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String

            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
            & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' AND AM_DELETED='N' AND CDM_DELETED = 'N' ORDER BY CDM_DEPT_CODE" 'Jules 2018.12.24 - Filter out deleted departments

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2  AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (" & strSqlLevel2 & ") Q LEFT JOIN " _
            & "(" & strSqlLevel3 & ") W " _
            & "ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.AM_DEPT_INDEX, E.AM_ACCT_INDEX, E.AM_ACCT_CODE, E.AM_ACCT_DESC, E.AM_PARENT_ACCT_INDEX, E.AU_USER_ID, R.* from (" & strSqlLevel1 & ") E LEFT JOIN " _
            & "( " & strSql4 & " ) R " _
            & "ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX"

            strSql2 = "select Y.*, U.* from (" & strSqlDept & ") Y INNER JOIN " _
            & "( " & strSql3 & " ) U " _
            & "ON Y.CDM_DEPT_INDEX = U.AM_DEPT_INDEX "

            'strSql1 = "Shape{select * from company_dept_mstr} append "
            Dim strSQL As String = String.Format("" &
                "SELECT " &
                "    CASE " &
                "    WHEN A.AM_ACCT_Desc IS NOT NULL THEN A.AM_ACCT_Desc " &
                "    WHEN A.Q_AM_ACCT_Desc IS NOT NULL THEN A.Q_AM_ACCT_Desc " &
                "    ELSE A.AM_ACCT_Code END AS Acct_List, " &
                "    CASE " &
                "    WHEN A.W_AM_ACCT_INDEX IS NOT NULL THEN A.W_AM_ACCT_INDEX " &
                "    WHEN A.Q_AM_ACCT_INDEX IS NOT NULL THEN A.Q_AM_ACCT_INDEX " &
                "    ELSE A.AM_ACCT_INDEX END AS Acct_Index, " &
                "    CASE " &
                "    WHEN A.W_AM_ACCT_CODE IS NOT NULL THEN A.W_AM_ACCT_CODE " &
                "    WHEN A.Q_AM_ACCT_CODE IS NOT NULL THEN A.Q_AM_ACCT_CODE " &
                "    ELSE A.AM_ACCT_CODE END AS Acct_Code, " &
                "    A.AM_ACCT_Desc AS L1, " &
                "    A.Q_AM_ACCT_Desc AS L2, " &
                "    A.W_AM_ACCT_Desc AS L3 " &
                "    FROM ( " &
                "       {0} " &
                "       WHERE U.AU_User_id IS NOT NULL " &
                ") A {1}", strSql2, If(Not String.IsNullOrEmpty(strCode), String.Format("HAVING Acct_Index ='{0}'", strCode), String.Empty))

            If Not getStrSQL Then
                Dim dtLevel1 As DataTable
                Dim dsBCM As DataSet
                Dim dsTempBCM As New DataSet
                dtLevel1 = ob.FillDs(strSql2).Tables(0)
                'dsBCM = BuildBCMList(dtLevel1, True)

                dsBCM = BuildBCMListNew(dtLevel1, strDesc, True)

                Dim dtTempTable As New DataTable
                Dim dtTempTable1 As New DataTable
                If strCode <> "" Or strDesc <> "" Then
                    Dim drFinalResult As DataRow()
                    Dim drTemp As DataRow
                    '//to make sure dtTempTable and dtResult have the same structure
                    dtTempTable = dsBCM.Tables(0).Clone    'dtResult.Clone

                    Dim filterStr As String = ""

                    If Not strCode.Equals("") Then
                        filterStr = filterStr & "ACCT_code" & Common.ParseSQL(strCode)
                        If Not strDesc.Equals("") Then
                            Dim parseDesc As String = Common.ParseSQL(strDesc)
                            filterStr &= "AND (L1 " & parseDesc & " OR L2 " & parseDesc & " OR L3 " & parseDesc + " )"
                        End If
                    Else
                        Dim parseDesc As String = Common.ParseSQL(strDesc)
                        filterStr = filterStr & " L1 " & parseDesc & " OR L2 " & parseDesc & " OR L3 " & parseDesc
                    End If

                    drFinalResult = dsBCM.Tables(0).Select(filterStr)

                    'drFinalResult = dsBCM.Tables(0).Select("ACCT_code" & Common.ParseSQL(strCode))
                    If drFinalResult.Length > 0 Then
                        For Each drTemp In drFinalResult
                            dtTempTable.ImportRow(drTemp)
                        Next
                    End If
                Else
                    dtTempTable = dsBCM.Tables(0).Copy
                End If
                dsTempBCM.Tables.Add(dtTempTable)    '//selected List
                dtTempTable1 = dsBCM.Tables(1).Copy
                dsTempBCM.Tables.Add(dtTempTable1)    '//available list


                Res.ds = dsTempBCM
                Res.strSql = strSQL
                Return Res
            Else
                Res.ds = Nothing
                Res.strSql = strSQL
                Return Res
            End If
        End Function

        '//26-08-2005(By Moo)
        '//drResult("Acct_List") --> Change From display Account Code to display Account Desc:Account Code
        '//End 26-08-2005(By Moo)
        Private Function BuildBCMList(ByVal dtLevel1 As DataTable, Optional ByVal blnByUser As Boolean = False) As DataSet
            Dim dtAvailList As New DataTable
            Dim dtSelectedList As New DataTable
            Dim drLevel1, drLevel2, drLevel3, drLevel4, drResult As DataRow
            Dim drsLevel2, drsLevel3, drsLevel4 As DataRow()
            Dim ds As New DataSet

            dtAvailList.Columns.Add("Acct_List", Type.GetType("System.String"))
            dtAvailList.Columns.Add("Acct_Index", Type.GetType("System.Int32"))
            dtAvailList.Columns.Add("Acct_Code", Type.GetType("System.String"))
            dtSelectedList = dtAvailList.Clone

            For Each drLevel1 In dtLevel1.Rows
                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        drResult = dtAvailList.NewRow
                    Else
                        drResult = dtSelectedList.NewRow
                    End If
                Else
                    drResult = dtSelectedList.NewRow
                End If

                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") &
                "-" & Common.parseNull(drLevel1("AM_ACCT_Desc")) & ":" & drLevel1("AM_ACCT_Code") &
                "-" & Common.parseNull(drLevel1("Q_AM_ACCT_Desc")) & ":" & drLevel1("Q_AM_ACCT_Code") &
                "-" & Common.parseNull(drLevel1("W_AM_ACCT_Desc")) & ":" & drLevel1("W_AM_ACCT_Code")
                drResult("Acct_Index") = drLevel1("W_AM_ACCT_INDEX")
                drResult("Acct_Code") = drLevel1("W_AM_ACCT_code")

                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        dtAvailList.Rows.Add(drResult)
                    Else
                        dtSelectedList.Rows.Add(drResult)
                    End If
                Else          '//for getBCMListByCompany (no filtering)
                    dtSelectedList.Rows.Add(drResult)
                End If

            Next

            'For Each drLevel1 In dtLevel1.Rows    '//department
            '    drsLevel2 = drLevel1.GetChildRows("tableChapter1")
            '    If drsLevel2.Length > 0 Then    '//account code
            '        For Each drLevel2 In drsLevel2
            '            '	drLevel2.TABLE.CHILDRELATIONS.ITEM(0).RelationName	"tableChapter1Chapter1"	String
            '            drsLevel3 = drLevel2.GetChildRows("tableChapter1Chapter1")
            '            ' drsLevel2.
            '            If drsLevel3.Length > 0 Then
            '                'htT1.Add(drLevel2("AM_ACCT_INDEX"), drsLevel3.Length)
            '                For Each drLevel3 In drsLevel3
            '                    drsLevel4 = drLevel3.GetChildRows("tableChapter1Chapter1Chapter1")
            '                    If drsLevel4.Length > 0 Then
            '                        For Each drLevel4 In drsLevel4
            '                            'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "-" & drLevel3(2) & "-" & drLevel4(2) & "<br>")

            '                            If blnByUser Then
            '                                If IsDBNull(drLevel4("AU_USER_ID")) Then
            '                                    drResult = dtAvailList.NewRow
            '                                Else
            '                                    drResult = dtSelectedList.NewRow
            '                                End If
            '                            Else          '//for getBCMListByCompany (no filtering)
            '                                drResult = dtSelectedList.NewRow
            '                            End If

            '                            drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & _
            '                            "-" & Common.parseNull(drLevel2("AM_ACCT_Desc")) & ":" & drLevel2("AM_ACCT_Code") & _
            '                            "-" & Common.parseNull(drLevel3("AM_ACCT_Desc")) & ":" & drLevel3("AM_ACCT_Code") & _
            '                            "-" & Common.parseNull(drLevel4("AM_ACCT_Desc")) & ":" & drLevel4("AM_ACCT_Code")
            '                            drResult("Acct_Index") = drLevel4("AM_ACCT_INDEX")
            '                            drResult("Acct_Code") = drLevel4("AM_ACCT_code")

            '                            'If strCode <> "" Then
            '                            '    If checkSearchKey(strCode, drResult("Acct_Code")) Then
            '                            '        dtAvailList.Rows.Add(drResult)
            '                            '    End If
            '                            'Else
            '                            If blnByUser Then
            '                                If IsDBNull(drLevel4("AU_USER_ID")) Then
            '                                    dtAvailList.Rows.Add(drResult)
            '                                Else
            '                                    dtSelectedList.Rows.Add(drResult)
            '                                End If
            '                            Else          '//for getBCMListByCompany (no filtering)
            '                                dtSelectedList.Rows.Add(drResult)
            '                            End If
            '                            'End If

            '                        Next
            '                    Else          '// no project name
            '                        'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "-" & drLevel3(2) & "<br>")
            '                        If blnByUser Then
            '                            If IsDBNull(drLevel3("AU_USER_ID")) Then
            '                                drResult = dtAvailList.NewRow
            '                            Else
            '                                drResult = dtSelectedList.NewRow
            '                            End If
            '                        Else          '//for getBCMListByCompany (no filtering)
            '                            drResult = dtSelectedList.NewRow
            '                        End If

            '                        drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & _
            '                        "-" & Common.parseNull(drLevel2("AM_ACCT_Desc")) & ":" & drLevel2("AM_ACCT_Code") & _
            '                        "-" & Common.parseNull(drLevel3("AM_ACCT_Desc")) & ":" & drLevel3("AM_ACCT_Code")
            '                        drResult("Acct_Index") = drLevel3("AM_ACCT_INDEX")
            '                        drResult("Acct_Code") = drLevel3("AM_ACCT_code")
            '                        'If strCode <> "" Then
            '                        '    If checkSearchKey(strCode, drResult("Acct_Code")) Then
            '                        '        dtAvailList.Rows.Add(drResult)
            '                        '    End If
            '                        'Else
            '                        If blnByUser Then
            '                            If IsDBNull(drLevel3("AU_USER_ID")) Then
            '                                dtAvailList.Rows.Add(drResult)
            '                            Else
            '                                dtSelectedList.Rows.Add(drResult)
            '                            End If
            '                        Else
            '                            dtSelectedList.Rows.Add(drResult)
            '                        End If
            '                        'End If
            '                    End If

            '                Next
            '            Else       '//No sub account
            '                'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "<br>")
            '                If blnByUser Then
            '                    If IsDBNull(drLevel2("AU_USER_ID")) Then
            '                        drResult = dtAvailList.NewRow
            '                    Else
            '                        drResult = dtSelectedList.NewRow
            '                    End If
            '                Else       '//for getBCMListByCompany (no filtering)
            '                    drResult = dtSelectedList.NewRow
            '                End If

            '                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & "-" & Common.parseNull(drLevel2("AM_ACCT_Desc")) & ":" & drLevel2("AM_ACCT_Code")
            '                drResult("Acct_Index") = drLevel2("AM_ACCT_INDEX")
            '                drResult("Acct_Code") = drLevel2("AM_ACCT_code")
            '                'If strCode <> "" Then
            '                '    If checkSearchKey(strCode, drResult("Acct_Code")) Then
            '                '        dtAvailList.Rows.Add(drResult)
            '                '    End If
            '                'Else
            '                If blnByUser Then
            '                    If IsDBNull(drLevel2("AU_USER_ID")) Then
            '                        dtAvailList.Rows.Add(drResult)
            '                    Else
            '                        dtSelectedList.Rows.Add(drResult)
            '                    End If
            '                Else
            '                    dtSelectedList.Rows.Add(drResult)
            '                End If
            '                'End If
            '            End If
            '        Next
            '    End If
            'Next

            ds.Tables.Add(dtSelectedList)
            If blnByUser Then
                ds.Tables.Add(dtAvailList)
            End If
            Return ds
        End Function

        Private Function BuildBCMList(ByVal dtLevel1 As DataTable, ByVal desc As String, Optional ByVal blnByUser As Boolean = False) As DataSet
            Dim dtAvailList As New DataTable
            Dim dtSelectedList As New DataTable
            Dim drLevel1, drLevel2, drLevel3, drLevel4, drResult As DataRow
            Dim drsLevel2, drsLevel3, drsLevel4 As DataRow()
            Dim ds As New DataSet

            dtAvailList.Columns.Add("Acct_List", Type.GetType("System.String"))
            dtAvailList.Columns.Add("Acct_Index", Type.GetType("System.Int32"))
            dtAvailList.Columns.Add("Acct_Code", Type.GetType("System.String"))

            '[Trick] Add 3 more cols
            dtAvailList.Columns.Add("L1", Type.GetType("System.String"))
            dtAvailList.Columns.Add("L2", Type.GetType("System.String"))
            dtAvailList.Columns.Add("L3", Type.GetType("System.String"))

            dtSelectedList = dtAvailList.Clone

            For Each drLevel1 In dtLevel1.Rows
                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        drResult = dtAvailList.NewRow
                    Else
                        drResult = dtSelectedList.NewRow
                    End If
                Else
                    drResult = dtSelectedList.NewRow
                End If

                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") &
                "-" & Common.parseNull(drLevel1("AM_ACCT_Desc")) & ":" & drLevel1("AM_ACCT_Code") &
                "-" & Common.parseNull(drLevel1("Q_AM_ACCT_Desc")) & ":" & drLevel1("Q_AM_ACCT_Code") &
                "-" & Common.parseNull(drLevel1("W_AM_ACCT_Desc")) & ":" & drLevel1("W_AM_ACCT_Code")
                drResult("Acct_Index") = drLevel1("W_AM_ACCT_INDEX")
                drResult("Acct_Code") = drLevel1("W_AM_ACCT_code")

                '[Trick]
                drResult("L1") = Common.parseNull(drLevel1("AM_ACCT_Desc"))
                drResult("L2") = Common.parseNull(drLevel1("Q_AM_ACCT_Desc"))
                drResult("L3") = Common.parseNull(drLevel1("W_AM_ACCT_Desc"))

                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        dtAvailList.Rows.Add(drResult)
                    Else
                        dtSelectedList.Rows.Add(drResult)
                    End If
                Else          '//for getBCMListByCompany (no filtering)
                    dtSelectedList.Rows.Add(drResult)
                End If

            Next
            'For Each drLevel1 In dtLevel1.Rows    '//department
            '    drsLevel2 = drLevel1.GetChildRows("tableChapter1")

            '    If drsLevel2.Length > 0 Then    '//account code
            '        For Each drLevel2 In drsLevel2
            '            '	drLevel2.TABLE.CHILDRELATIONS.ITEM(0).RelationName	"tableChapter1Chapter1"	String
            '            drsLevel3 = drLevel2.GetChildRows("tableChapter1Chapter1")
            '            ' drsLevel2.
            '            If drsLevel3.Length > 0 Then
            '                'htT1.Add(drLevel2("AM_ACCT_INDEX"), drsLevel3.Length)
            '                For Each drLevel3 In drsLevel3
            '                    drsLevel4 = drLevel3.GetChildRows("tableChapter1Chapter1Chapter1")
            '                    If drsLevel4.Length > 0 Then
            '                        For Each drLevel4 In drsLevel4
            '                            'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "-" & drLevel3(2) & "-" & drLevel4(2) & "<br>")

            '                            If blnByUser Then
            '                                If IsDBNull(drLevel4("AU_USER_ID")) Then
            '                                    drResult = dtAvailList.NewRow
            '                                Else
            '                                    drResult = dtSelectedList.NewRow
            '                                End If
            '                            Else          '//for getBCMListByCompany (no filtering)
            '                                drResult = dtSelectedList.NewRow
            '                            End If

            '                            drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & _
            '                            "-" & Common.parseNull(drLevel2("AM_ACCT_Desc")) & ":" & drLevel2("AM_ACCT_Code") & _
            '                            "-" & Common.parseNull(drLevel3("AM_ACCT_Desc")) & ":" & drLevel3("AM_ACCT_Code") & _
            '                            "-" & Common.parseNull(drLevel4("AM_ACCT_Desc")) & ":" & drLevel4("AM_ACCT_Code")
            '                            drResult("Acct_Index") = drLevel4("AM_ACCT_INDEX")
            '                            drResult("Acct_Code") = drLevel4("AM_ACCT_code")

            '                            '[Trick]
            '                            drResult("L1") = Common.parseNull(drLevel2("AM_ACCT_Desc"))
            '                            drResult("L2") = Common.parseNull(drLevel3("AM_ACCT_Desc"))
            '                            drResult("L3") = Common.parseNull(drLevel4("AM_ACCT_Desc"))


            '                            'If strCode <> "" Then
            '                            '    If checkSearchKey(strCode, drResult("Acct_Code")) Then
            '                            '        dtAvailList.Rows.Add(drResult)
            '                            '    End If
            '                            'Else
            '                            If blnByUser Then
            '                                If IsDBNull(drLevel4("AU_USER_ID")) Then
            '                                    dtAvailList.Rows.Add(drResult)
            '                                Else
            '                                    dtSelectedList.Rows.Add(drResult)
            '                                End If
            '                            Else          '//for getBCMListByCompany (no filtering)
            '                                dtSelectedList.Rows.Add(drResult)
            '                            End If
            '                            'End If

            '                        Next
            '                    Else          '// no project name
            '                        'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "-" & drLevel3(2) & "<br>")
            '                        If blnByUser Then
            '                            If IsDBNull(drLevel3("AU_USER_ID")) Then
            '                                drResult = dtAvailList.NewRow
            '                            Else
            '                                drResult = dtSelectedList.NewRow
            '                            End If
            '                        Else          '//for getBCMListByCompany (no filtering)
            '                            drResult = dtSelectedList.NewRow
            '                        End If

            '                        drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & _
            '                        "-" & Common.parseNull(drLevel2("AM_ACCT_Desc")) & ":" & drLevel2("AM_ACCT_Code") & _
            '                        "-" & Common.parseNull(drLevel3("AM_ACCT_Desc")) & ":" & drLevel3("AM_ACCT_Code")
            '                        drResult("Acct_Index") = drLevel3("AM_ACCT_INDEX")
            '                        drResult("Acct_Code") = drLevel3("AM_ACCT_code")

            '                        '[Trick]
            '                        drResult("L1") = Common.parseNull(drLevel2("AM_ACCT_Desc"))
            '                        drResult("L2") = Common.parseNull(drLevel3("AM_ACCT_Desc"))

            '                        'If strCode <> "" Then
            '                        '    If checkSearchKey(strCode, drResult("Acct_Code")) Then
            '                        '        dtAvailList.Rows.Add(drResult)
            '                        '    End If
            '                        'Else
            '                        If blnByUser Then
            '                            If IsDBNull(drLevel3("AU_USER_ID")) Then
            '                                dtAvailList.Rows.Add(drResult)
            '                            Else
            '                                dtSelectedList.Rows.Add(drResult)
            '                            End If
            '                        Else
            '                            dtSelectedList.Rows.Add(drResult)
            '                        End If
            '                        'End If
            '                    End If

            '                Next
            '            Else       '//No sub account
            '                'Response.Write("level- " & dr(2) & "-" & drLevel2(2) & "<br>")
            '                If blnByUser Then
            '                    If IsDBNull(drLevel2("AU_USER_ID")) Then
            '                        drResult = dtAvailList.NewRow
            '                    Else
            '                        drResult = dtSelectedList.NewRow
            '                    End If
            '                Else       '//for getBCMListByCompany (no filtering)
            '                    drResult = dtSelectedList.NewRow
            '                End If

            '                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & "-" & Common.parseNull(drLevel2("AM_ACCT_Desc")) & ":" & drLevel2("AM_ACCT_Code")
            '                drResult("Acct_Index") = drLevel2("AM_ACCT_INDEX")
            '                drResult("Acct_Code") = drLevel2("AM_ACCT_code")

            '                '[Trick]
            '                drResult("L1") = Common.parseNull(drLevel2("AM_ACCT_Desc"))

            '                'If strCode <> "" Then
            '                '    If checkSearchKey(strCode, drResult("Acct_Code")) Then
            '                '        dtAvailList.Rows.Add(drResult)
            '                '    End If
            '                'Else
            '                If blnByUser Then
            '                    If IsDBNull(drLevel2("AU_USER_ID")) Then
            '                        dtAvailList.Rows.Add(drResult)
            '                    Else
            '                        dtSelectedList.Rows.Add(drResult)
            '                    End If
            '                Else
            '                    dtSelectedList.Rows.Add(drResult)
            '                End If
            '                'End If
            '            End If
            '        Next
            '    End If
            'Next

            ds.Tables.Add(dtSelectedList)
            If blnByUser Then
                ds.Tables.Add(dtAvailList)
            End If
            Return ds
        End Function

        Private Function BuildBCMListNew(ByVal dtLevel1 As DataTable, ByVal desc As String, Optional ByVal blnByUser As Boolean = False) As DataSet
            Dim dtAvailList As New DataTable
            Dim dtSelectedList As New DataTable
            Dim drLevel1, drLevel2, drLevel3, drLevel4, drResult As DataRow
            Dim drsLevel2, drsLevel3, drsLevel4 As DataRow()
            Dim ds As New DataSet

            dtAvailList.Columns.Add("Acct_List", Type.GetType("System.String"))
            dtAvailList.Columns.Add("Acct_Index", Type.GetType("System.Int32"))
            dtAvailList.Columns.Add("Acct_Code", Type.GetType("System.String"))

            '[Trick] Add 3 more cols
            dtAvailList.Columns.Add("L1", Type.GetType("System.String"))
            dtAvailList.Columns.Add("L2", Type.GetType("System.String"))
            dtAvailList.Columns.Add("L3", Type.GetType("System.String"))

            dtSelectedList = dtAvailList.Clone

            For Each drLevel1 In dtLevel1.Rows
                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        If IsDBNull(drLevel1("Q_AU_USER_ID")) Then
                            If IsDBNull(drLevel1("AU_USER_ID")) Then
                                drResult = dtAvailList.NewRow
                            Else
                                drResult = dtSelectedList.NewRow
                            End If
                        Else
                            drResult = dtSelectedList.NewRow
                        End If
                    Else
                        drResult = dtSelectedList.NewRow
                    End If
                Else
                    drResult = dtSelectedList.NewRow
                End If

                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE")
                If Not IsDBNull(drLevel1("AM_ACCT_Desc")) Then
                    drResult("Acct_List") &= "-" & Common.parseNull(drLevel1("AM_ACCT_Desc")) & ":" & drLevel1("AM_ACCT_Code")
                End If
                If Not IsDBNull(drLevel1("Q_AM_ACCT_Desc")) Then
                    drResult("Acct_List") &= "-" & Common.parseNull(drLevel1("Q_AM_ACCT_Desc")) & ":" & drLevel1("Q_AM_ACCT_Code")
                End If
                If Not IsDBNull(drLevel1("W_AM_ACCT_Desc")) Then
                    drResult("Acct_List") &= "-" & Common.parseNull(drLevel1("W_AM_ACCT_Desc")) & ":" & drLevel1("W_AM_ACCT_Code")
                End If
                If IsDBNull(drLevel1("W_AM_ACCT_INDEX")) Then
                    If IsDBNull(drLevel1("Q_AM_ACCT_INDEX")) Then
                        If IsDBNull(drLevel1("AM_ACCT_INDEX")) Then
                        Else
                            drResult("Acct_Index") = drLevel1("AM_ACCT_INDEX")
                            drResult("Acct_Code") = drLevel1("AM_ACCT_CODE")
                        End If
                    Else
                        drResult("Acct_Index") = drLevel1("Q_AM_ACCT_INDEX")
                        drResult("Acct_Code") = drLevel1("Q_AM_ACCT_CODE")
                    End If
                Else
                    drResult("Acct_Index") = drLevel1("W_AM_ACCT_INDEX")
                    drResult("Acct_Code") = drLevel1("W_AM_ACCT_CODE")
                End If

                '[Trick]
                drResult("L1") = Common.parseNull(drLevel1("AM_ACCT_Desc"))
                drResult("L2") = Common.parseNull(drLevel1("Q_AM_ACCT_Desc"))
                drResult("L3") = Common.parseNull(drLevel1("W_AM_ACCT_Desc"))

                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        If IsDBNull(drLevel1("Q_AU_USER_ID")) Then
                            If IsDBNull(drLevel1("AU_USER_ID")) Then
                                dtAvailList.Rows.Add(drResult)
                            Else
                                dtSelectedList.Rows.Add(drResult)
                            End If
                        Else
                            dtSelectedList.Rows.Add(drResult)
                        End If
                    Else
                        dtSelectedList.Rows.Add(drResult)
                    End If
                Else          '//for getBCMListByCompany (no filtering)
                    dtSelectedList.Rows.Add(drResult)
                End If

            Next

            ds.Tables.Add(dtSelectedList)
            If blnByUser Then
                ds.Tables.Add(dtAvailList)
            End If
            Return ds
        End Function

        '//Remark By Moo
        'Public Function checkSearchKey(ByVal pInString As String, ByVal strCompare As String) As Boolean
        '    checkSearchKey = False
        '    If InStr(pInString, "*") > 0 Then
        '        If InStr(strCompare, pInString.Substring(0, pInString.Length - 1)) > 0 Then
        '            checkSearchKey = True
        '        End If
        '    ElseIf InStr(pInString, "?") > 0 Then
        '        If strCompare.Length = pInString.Length Then
        '            If InStr(strCompare, pInString.Substring(0, pInString.Length - 1)) > 0 Then
        '                checkSearchKey = True
        '            End If
        '        End If
        '    Else
        '        If strCompare.ToLower = pInString.ToLower Then
        '            checkSearchKey = True
        '        End If
        '    End If
        'End Function

        Public Function getBCMListByCompany() As DataTable

            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String

            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
              & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' ORDER BY CDM_DEPT_CODE"

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A " _
            & " WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A WHERE " _
            & " AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2 AND AM_DELETED='N'"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A WHERE " _
            & " AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX from (" & strSqlLevel2 & ") Q INNER JOIN " _
            & "(" & strSqlLevel3 & ") W " _
            & "ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.*, R.* from (" & strSqlLevel1 & ") E INNER JOIN " _
            & "( " & strSql4 & " ) R " _
            & "ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX "

            strSql2 = "select T.*, Y.* from (" & strSqlDept & ") T INNER JOIN " _
            & "( " & strSql3 & " ) Y " _
            & "ON T.CDM_DEPT_INDEX = Y.AM_DEPT_INDEX"


            Dim dtLevel1, dtResult As DataTable
            dtLevel1 = ob.FillDs(strSql2).Tables(0)
            dtResult = BuildBCMList(dtLevel1).Tables(0)

            Return dtResult
        End Function

        Public Function AddAccount(ByVal pCode As String, ByVal pDesc As String, ByVal pBudget As String,
           ByVal pDept As String, ByVal pLevel As Integer, ByVal pParent As String) As Boolean
            Dim strSQL As String
            Dim objDb As New EAD.DBCom
            Dim ctx As Web.HttpContext = Web.HttpContext.Current

            If pLevel > 1 Then
                If pParent <> "" Then
                    Dim str As String = " SELECT AM_ACCT_INDEX FROM ACCOUNT_MSTR WHERE AM_DELETED='N' AND AM_ACCT_CODE='" & pParent & "' AND AM_DEPT_INDEX='" & pDept & "' AND AM_LEVEL=" & pLevel - 1 & " AND AM_COY_ID='" & ctx.Session("CompanyId") & "'"
                    pParent = objDb.GetVal(str)
                End If
            Else
                pParent = ""
            End If




            strSQL = "INSERT INTO ACCOUNT_MSTR(AM_ACCT_CODE,AM_ACCT_DESC,AM_INIT_BUDGET," &
             "AM_DEPT_INDEX,AM_COY_ID,AM_LEVEL," &
             "AM_PARENT_ACCT_INDEX,AM_DELETED,AM_ENT_BY,AM_ENT_DATETIME) " &
               "VALUES('" &
               Common.Parse(pCode) & "','" & Common.Parse(pDesc) & "'," & Common.ConvertMoney(pBudget) & ",'" &
               Common.Parse(pDept) & "','" & ctx.Session("CompanyId") & "'," & pLevel & ",'" &
               Common.Parse(pParent) & "','N','" & ctx.Session("UserId") & "'," & Common.ConvertDate(Now()) & ")"

            If objDb.Execute(strSQL) Then
                strMessage = Common.RecordSave
                Return True
            Else
                strMessage = Common.RecordNotSave
                Return False
            End If
        End Function

        Public Function SaveAccount(ByVal pCode As String, ByVal pDesc As String, ByVal pBudget As String,
           ByVal pLevel As String, ByVal pDeptI As String) As Boolean
            Dim strSQL As String
            Dim objDb As New EAD.DBCom
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            strSQL = "UPDATE ACCOUNT_MSTR SET " &
               "AM_ACCT_DESC='" & Common.Parse(pDesc) & "'," &
               "AM_INIT_BUDGET=" & Common.ConvertMoney(pBudget) & "," &
               "AM_MOD_BY='" & ctx.Session("UserId") & "'," &
               "AM_MOD_DATETIME=" & Common.ConvertDate(Now()) &
               " WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND " &
               " AM_LEVEL=" & Common.Parse(pLevel) & " AND " &
               " AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND " &
               " AM_COY_ID='" & ctx.Session("CompanyId") & "'"

            If objDb.Execute(strSQL) Then
                strMessage = Common.RecordSave
                Return True
            Else
                strMessage = Common.RecordNotSave
                Return False
            End If
        End Function

        Public Function DeleteAccount(ByVal pCode As String, ByVal pDeptI As String) As Boolean
            Dim strSQL, Query(0) As String
            Dim objDb As New EAD.DBCom
            Dim ctx As Web.HttpContext = Web.HttpContext.Current
            'strSQL = "UPDATE ACCOUNT_MSTR SET AM_DELETED='Y' WHERE " & _
            '         " AM_ACCT_CODE='" & pCode & "' AND " & _
            '         " AM_DEPT_INDEX='" & pDeptI & "' AND " & _
            '         " AM_COY_ID='" & ctx.Session("CompanyId") & "'"

            'Delete proj code
            strSQL = " UPDATE ACCOUNT_MSTR SET AM_DELETED='Y' WHERE AM_PARENT_ACCT_INDEX IN (" &
               " SELECT AM_ACCT_INDEX FROM ACCOUNT_MSTR WHERE AM_PARENT_ACCT_INDEX IN (" &
               " SELECT AM_ACCT_INDEX from ACCOUNT_MSTR WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "'))"
            Common.Insert2Ary(Query, strSQL)

            'Delete sub acc code
            strSQL = " UPDATE ACCOUNT_MSTR SET AM_DELETED='Y' WHERE AM_PARENT_ACCT_INDEX IN (" &
               " SELECT AM_ACCT_INDEX from ACCOUNT_MSTR WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "')"
            Common.Insert2Ary(Query, strSQL)

            'Delete acc code
            strSQL = " UPDATE ACCOUNT_MSTR SET AM_DELETED ='Y' WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "'"
            Common.Insert2Ary(Query, strSQL)

            If objDb.BatchExecute(Query) Then
                strMessage = Common.RecordDelete
                Return True
            Else
                strMessage = Common.RecordNotDelete
                Return False
            End If
        End Function

        Public Function DeleteBCMAccount(ByVal pCode As String, ByVal pDeptI As String) As Boolean
            Dim strSQL, Query(0) As String
            Dim objDb As New EAD.DBCom
            Dim ctx As Web.HttpContext = Web.HttpContext.Current

            'Delete proj code
            strSQL = " UPDATE ACCOUNT_MSTR SET AM_DELETED='Y' WHERE AM_PARENT_ACCT_INDEX IN (" &
                "SELECT AM_ACCT_INDEX FROM (SELECT * FROM ACCOUNT_MSTR WHERE AM_PARENT_ACCT_INDEX IN " &
                "(SELECT AM_ACCT_INDEX FROM (SELECT * FROM ACCOUNT_MSTR WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "') AS t)) AS s)"
            Common.Insert2Ary(Query, strSQL)

            'Delete sub acc code
            strSQL = "UPDATE ACCOUNT_MSTR SET AM_DELETED='Y' WHERE AM_PARENT_ACCT_INDEX IN " &
                "(SELECT AM_ACCT_INDEX FROM (SELECT * FROM ACCOUNT_MSTR WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "') AS t) "
            Common.Insert2Ary(Query, strSQL)

            'Delete acc code
            strSQL = " UPDATE ACCOUNT_MSTR SET AM_DELETED ='Y' WHERE AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND AM_DEPT_INDEX='" & Common.Parse(pDeptI) & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "'"
            Common.Insert2Ary(Query, strSQL)

            If objDb.BatchExecute(Query) Then
                strMessage = Common.RecordDelete
                Return True
            Else
                strMessage = Common.RecordNotDelete
                Return False
            End If
        End Function

        Public Function IsExist(ByVal pCode As String, ByVal pDept As String, ByVal pLevel As String, ByVal pParent As String) As Boolean
            Dim strSQL As String
            Dim objDb As New EAD.DBCom
            Dim ctx As Web.HttpContext = Web.HttpContext.Current

            If pParent <> "" Then
                Dim str As String = " SELECT AM_ACCT_INDEX FROM ACCOUNT_MSTR WHERE AM_DELETED='N' AND AM_ACCT_CODE='" & Common.Parse(pParent) &
                  "' AND AM_DEPT_INDEX='" & pDept & "'  AND AM_COY_ID='" & ctx.Session("CompanyId") & "' "
                pParent = objDb.GetVal(str)
            End If

            strSQL = "SELECT * FROM ACCOUNT_MSTR WHERE " &
               " AM_ACCT_CODE='" & Common.Parse(pCode) & "' AND " &
               " AM_DEPT_INDEX='" & Common.Parse(pDept) & "' AND " &
               " AM_PARENT_ACCT_INDEX='" & Common.Parse(pParent) & "' AND " &
               " AM_COY_ID='" & ctx.Session("CompanyId") & "' AND" &
               " AM_LEVEL=" & pLevel & " AND" &
               " AM_DELETED='N'"

            'not found
            If objDb.Exist(strSQL) = 0 Then
                Return False
            Else
                strMessage = Common.RecordDuplicate
                Return True
            End If

        End Function

        'Name       : UpdateAccUserByUser
        'Author     : kk
        'Descption  : Update User_userGroup
        'LastUpadte : 22 Nov 2004
        'Public Function UpdateAccUserByUser(ByRef pSelectedAcc As String, ByVal pUser As String) As Boolean

        '    Dim strSQL, Query(0) As String
        '    Dim objDb As New EAD.DBCom

        '    'Delete old Accoutn users
        '    strSQL = "DELETE FROM ACCOUNT_USERS WHERE AU_USER_ID ='" & Common.Parse(pUser) & "' AND AU_COY_ID='" & ctx.Session("CompanyID") & "'"
        '    Common.Insert2Ary(Query, strSQL)

        '    If pSelectedAcc <> "" Then
        '        Dim i As Integer
        '        Dim spSelectedAcc As Array
        '        spSelectedAcc = Split(pSelectedAcc, ",")

        '        'add new user user group
        '        For i = 0 To spSelectedAcc.Length - 1
        '            strSQL = "INSERT INTO ACCOUNT_USERS VALUES('" & spSelectedAcc(i) & "','" & Common.Parse(pUser) & "','" &
        '               ctx.Session("CompanyId") & "','" & ctx.Session("UserID") & "'," & Common.ConvertDate(Now) & ")"
        '            Common.Insert2Ary(Query, strSQL)
        '        Next
        '    End If

        '    If objDb.BatchExecute(Query) Then
        '        strMessage = Common.RecordSave
        '        Return True
        '    Else
        '        strMessage = Common.RecordNotSave
        '        Return False
        '    End If
        'End Function

        'Name       : UpdateAccUserByUser
        'Author     : kk
        'Descption  : Update User_userGroup
        'LastUpadte : 22 Nov 2004
        'Public Function UpdateAccUserByAcc(ByRef pSelectedUser As String, ByVal pAccIndex As String) As Boolean

        '    Dim strSQL, Query(0) As String
        '    Dim objDb As New EAD.DBCom

        '    'Delete old Accoutn users
        '    strSQL = "DELETE FROM ACCOUNT_USERS WHERE AU_ACCT_INDEX ='" & Common.Parse(pAccIndex) & "' AND AU_COY_ID='" & ctx.Session("CompanyID") & "'"
        '    Common.Insert2Ary(Query, strSQL)

        '    If pSelectedUser <> "" Then
        '        Dim i As Integer
        '        Dim spSelectedUser As Array
        '        spSelectedUser = Split(pSelectedUser, ",")

        '        'add new user user group
        '        For i = 0 To spSelectedUser.Length - 1
        '            strSQL = "INSERT INTO ACCOUNT_USERS VALUES('" & Common.Parse(pAccIndex) & "','" & spSelectedUser(i) & "','" &
        '               ctx.Session("CompanyId") & "','" & ctx.Session("UserID") & "'," & Common.ConvertDate(Now) & ")"
        '            Common.Insert2Ary(Query, strSQL)
        '        Next
        '    End If

        '    If objDb.BatchExecute(Query) Then
        '        strMessage = Common.RecordSave
        '        Return True
        '    Else
        '        strMessage = Common.RecordNotSave
        '        Return False
        '    End If
        'End Function

        'Name       : DelAccUserByUser
        'Author     : kk
        'Descption  : Delete Account User (Buyer assigned) base on the user id
        'LastUpadte : 18 Dec 2004
        'Public Function DelAccUserByUser(ByVal pUserId As String) As Boolean
        '    Dim strSQL As String
        '    Dim objDb As New EAD.DBCom

        '    strSQL = "DELETE FROM ACCOUNT_USERS WHERE AU_USER_ID='" & Common.Parse(pUserId) & "' AND AU_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    If objDb.Execute(strSQL) Then
        '        strMessage = Common.RecordDelete
        '        Return True
        '    Else
        '        strMessage = Common.RecordNotDelete
        '        Return False

        '    End If
        'End Function

        'Name       : DelAccUserByAcc
        'Author     : kk
        'Descption  : Delete Account User (Buyer assigned) base on the accout index
        'LastUpadte : 19 Dec 2004
        'Public Function DelAccUserByAcc(ByVal pAccIndex As String) As Boolean
        '    Dim strSQL As String
        '    Dim objDb As New EAD.DBCom

        '    strSQL = "DELETE FROM ACCOUNT_USERS WHERE AU_ACCT_INDEX='" & Common.Parse(pAccIndex) & "' AND AU_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    If objDb.Execute(strSQL) Then
        '        strMessage = Common.RecordDelete
        '        Return True
        '    Else
        '        strMessage = Common.RecordNotDelete
        '        Return False

        '    End If
        'End Function

        'Public Function SearchAccUserByUser(ByVal pDeptName As String, ByVal pUserName As String, ByVal pUserID As String) As DataSet
        '    Dim strSQL, strRole As String
        '    Dim ds As DataSet
        '    Dim objDb As New EAD.DBCom
        '    'All user 
        '    'strSQL = "SELECT CDM_DEPT_NAME,CDM_DEPT_CODE,UM_USER_NAME,UM_USER_ID FROM USER_MSTR,COMPANY_DEPT_MSTR " & _
        '    '             " WHERE um_dept_id=CDM_DEPT_CODE AND (UM_DELETED='N' AND CDM_DELETED='N') AND  UM_COY_ID='demo' "

        '    strRole = Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer), "_", " ") & "','" &
        '      Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Officer), "_", " ") & "','" &
        '    Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Manager), "_", " ")
        '    'Only buyer fixed role
        '    strSQL = "SELECT DISTINCT(UM_USER_ID),CDM_DEPT_NAME,CDM_DEPT_CODE,UM_USER_NAME FROM USER_MSTR,COMPANY_DEPT_MSTR,USERS_USRGRP,USER_GROUP_MSTR " &
        '       " WHERE UU_USER_ID=UM_USER_ID AND um_dept_id=CDM_DEPT_CODE AND CDM_COY_ID = UM_COY_ID AND UGM_USRGRP_ID=UU_USRGRP_ID AND (UM_DELETED='N' AND CDM_DELETED='N') AND (UU_COY_ID='" & ctx.Session("CompanyId") & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "') " &
        '       " AND UGM_FIXED_ROLE IN ('" & strRole & "') "

        '    If pDeptName <> "" Then
        '        strSQL &= " AND UPPER(CDM_DEPT_NAME)" & Common.ParseSQL(pDeptName)
        '    End If

        '    If pUserName <> "" Then
        '        strSQL &= " AND UPPER(UM_USER_NAME)" & Common.ParseSQL(pUserName)
        '    End If

        '    If pUserID <> "" Then
        '        strSQL &= " AND UPPER(UM_USER_ID)" & Common.ParseSQL(pUserID)
        '    End If
        '    strSQL &= " ORDER BY CDM_DEPT_NAME"

        '    ds = objDb.FillDs(strSQL)
        '    SearchAccUserByUser = ds
        'End Function




        'Name       : GetBindInfo
        'Author     : kk
        'Descption  : Get the info that required to show in  BCM Assignment by Acc code
        'LastUpadte : 18 Dec 2004
        'Public Function GetBindInfo(ByVal pAccIndex As String) As DataTable
        '    Dim strSQL As String
        '    Dim objDb As New EAD.DBCom

        '    strSQL = " SELECT AM_ACCT_DESC,CDM_DEPT_NAME,CDM_DEPT_CODE FROM ACCOUNT_MSTR, COMPANY_DEPT_MSTR " &
        '       " WHERE CDM_DEPT_INDEX=AM_DEPT_INDEX AND CDM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_ACCT_INDEX='" & Common.Parse(pAccIndex) & "'"

        '    Return objDb.FillDt(strSQL)

        'End Function

        'Name       : getUserListbyAcc
        'Author     : kk
        'Descption  : Search selected buyer in dt(0) and avail buyer in dt(1)
        'LastUpadte : 22/11/2004
        'Public Function getUserListbyAcc(ByVal pAccIndex As String, ByVal pDeptCode As String) As DataSet
        '    Dim strSQL, strRole As String
        '    Dim objDB As New EAD.DBCom
        '    Dim ds As DataSet
        '    strRole = Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer), "_", " ") & "','" &
        '     Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Officer), "_", " ") & "','" &
        '     Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Manager), "_", " ")

        '    'To get selected buyer from a dept of a comp
        '    'strSQL = " SELECT UU_USRGRP_ID,CDM_DEPT_NAME,CDM_DEPT_CODE,UM_USER_NAME,UM_USER_ID " & _
        '    '" FROM USER_MSTR, COMPANY_DEPT_MSTR, USERS_USRGRP, USER_GROUP_MSTR " & _
        '    '" WHERE UU_USER_ID = UM_USER_ID And um_dept_id = CDM_DEPT_CODE And UGM_USRGRP_ID = UU_USRGRP_ID " & _
        '    '" AND (UM_DELETED='N' AND CDM_DELETED='N') AND (UU_COY_ID='" & ctx.Session("CompanyId") & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "') " & _
        '    '" AND CDM_DEPT_CODE='" & pDeptCode & "' " & _
        '    '" AND UGM_FIXED_ROLE='Buyer' AND UM_USER_ID IN (" & _
        '    '" SELECT AU_USER_ID FROM ACCOUNT_USERS " & _
        '    '" WHERE AU_COY_ID='" & ctx.Session("CompanyId") & "' " & _
        '    '" AND  AU_ACCT_INDEX='" & pAccIndex & "')"

        '    strSQL = " SELECT Distinct(UM_USER_ID)," &
        '           objDB.Concat(" : ", "", "CDM_DEPT_NAME", "UM_USER_NAME", "UM_USER_ID") & " AS UM_USER_NAME,CDM_DEPT_CODE " &
        '     " FROM USER_MSTR, COMPANY_DEPT_MSTR, USERS_USRGRP, USER_GROUP_MSTR " &
        '     " WHERE (UU_USER_ID = UM_USER_ID AND UU_COY_ID= UM_COY_ID AND UM_COY_ID=CDM_COY_ID)  And um_dept_id = CDM_DEPT_CODE And UGM_USRGRP_ID = UU_USRGRP_ID  " &
        '     " AND (UM_DELETED<>'Y' AND CDM_DELETED='N') AND UU_COY_ID='" & ctx.Session("CompanyId") & "' " &
        '     " AND UGM_FIXED_ROLE IN ('" & strRole & "') " &
        '     " AND UM_USER_ID  IN ( SELECT AU_USER_ID FROM ACCOUNT_USERS " &
        '     " WHERE AU_COY_ID='" & ctx.Session("CompanyId") & "'  AND  AU_ACCT_INDEX='" & Common.Parse(pAccIndex) & "') "

        '    ds = objDB.FillDs(strSQL)

        '    'To get avail buyer from a dept of a comp
        '    'strSQL = " SELECT UU_USRGRP_ID,CDM_DEPT_NAME,CDM_DEPT_CODE,UM_USER_NAME,UM_USER_ID " & _
        '    '" FROM USER_MSTR, COMPANY_DEPT_MSTR, USERS_USRGRP, USER_GROUP_MSTR " & _
        '    '" WHERE UU_USER_ID = UM_USER_ID And um_dept_id = CDM_DEPT_CODE And UGM_USRGRP_ID = UU_USRGRP_ID " & _
        '    '" AND (UM_DELETED='N' AND CDM_DELETED='N') AND (UU_COY_ID='" & ctx.Session("CompanyId") & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "') " & _
        '    '" AND CDM_DEPT_CODE='" & pDeptCode & "' " & _
        '    '" AND UGM_FIXED_ROLE='Buyer' AND UM_USER_ID NOT IN (" & _
        '    '" SELECT AU_USER_ID FROM ACCOUNT_USERS " & _
        '    '" WHERE AU_COY_ID='" & ctx.Session("CompanyId") & "' " & _
        '    '" AND  AU_ACCT_INDEX='" & pAccIndex & "')"


        '    strSQL = " SELECT Distinct(UM_USER_ID)," &
        '   objDB.Concat(" : ", "", "CDM_DEPT_NAME", "UM_USER_NAME", "UM_USER_ID") & " AS UM_USER_NAME,CDM_DEPT_CODE " &
        '    " FROM USER_MSTR, COMPANY_DEPT_MSTR, USERS_USRGRP, USER_GROUP_MSTR " &
        '    " WHERE (UU_USER_ID = UM_USER_ID AND UU_COY_ID= UM_COY_ID AND UM_COY_ID=CDM_COY_ID)  And um_dept_id = CDM_DEPT_CODE And UGM_USRGRP_ID = UU_USRGRP_ID  " &
        '    " AND (UM_DELETED<>'Y' AND CDM_DELETED='N') AND UU_COY_ID='" & ctx.Session("CompanyId") & "' " &
        '    " AND UGM_FIXED_ROLE IN ('" & strRole & "') " &
        '    " AND UM_USER_ID NOT IN ( SELECT AU_USER_ID FROM ACCOUNT_USERS " &
        '    " WHERE AU_COY_ID='" & ctx.Session("CompanyId") & "'  AND  AU_ACCT_INDEX='" & Common.Parse(pAccIndex) & "') "

        '    objDB.FillDsIn(ds, strSQL, "Avail")

        '    getUserListbyAcc = ds
        'End Function

        'Name       : GetAccBuyer
        'Author     : kk
        'Descption  : Get all buyer belong to a Acc code
        'LastUpadte : 19 Dec 2004
        'Public Function GetAccBuyer(ByVal pIndex As String) As DataSet
        '    Dim objBCM As New BudgetControl
        '    Dim objDb As New EAD.DBCom
        '    Dim ds As DataSet
        '    Dim strSQL, strRole As String
        '    'Only select buyer, Active and (not delete or suspend user)
        '    strRole = Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer), "_", " ") & "','" &
        '     Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Officer), "_", " ") & "','" &
        '     Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Manager), "_", " ")

        '    strSQL = " SELECT DISTINCT(UM_USER_ID),UM_USER_NAME,UM_DELETED,UM_STATUS " &
        '       " FROM ACCOUNT_USERS, USER_MSTR, USERS_USRGRP, USER_GROUP_MSTR " &
        '       " WHERE(AU_USER_ID = UM_USER_ID) " &
        '       " AND (UU_USER_ID = UM_USER_ID AND UU_COY_ID=UM_COY_ID) " &
        '       " AND (UGM_USRGRP_ID = UU_USRGRP_ID AND " &
        '       " UGM_FIXED_ROLE IN ('" & strRole & "'))" &
        '       " AND (UM_DELETED<>'Y' AND UM_STATUS='A') " &
        '       " AND  UM_COY_ID='" & ctx.Session("CompanyId") & "' ANd AU_ACCT_INDEX='" & Common.Parse(pIndex) & "'" &
        '       " ORDER BY UM_USER_NAME"
        '    ds = objDb.FillDs(strSQL)
        '    Return ds
        'End Function


        'Name       : IsWithinBudget
        'Author     : kk
        'Descption  : check the initial budget in exceeded limit
        'LastUpadte : 23/12/2004
        Public Function IsWithinBudget(ByVal pAddAction As Boolean, ByVal pAccCode As String, ByVal pParent As String, ByVal pAmt As Double, ByVal pDeptIndex As Integer, ByVal pLevel As Integer) As Boolean
            Dim objDb As New EAD.DBCom
            Dim strSQL As String
            Dim dblIniBudget, dblAssBudget, dblAmtLeft As Double
            Dim iAccIndex As Integer

            strSQL = "SELECT AM_ACCT_INDEX FROM ACCOUNT_MSTR WHERE AM_ACCT_CODE='" & Common.Parse(pParent) & "' AND AM_DEPT_INDEX=" & Common.Parse(pDeptIndex) & " AND AM_LEVEL=" & pLevel - 1 & " AND AM_DELETED<>'Y'"
            iAccIndex = objDb.GetVal(strSQL)

            dblIniBudget = getInitialBudget(iAccIndex)
            dblAssBudget = getAssignedBudget(iAccIndex, pAccCode, pDeptIndex, pAddAction)
            dblAmtLeft = Math.Round(dblIniBudget - dblAssBudget, 2)

            If pAmt > dblAmtLeft Then
                Return False
            Else
                Return True
            End If
        End Function

        'Name       : GetAccDept
        'Author     : kk
        'Descption  : Get Acc code's dept
        'LastUpadte : 19 Dec 2004
        Public Function GetAccDept(ByVal pIndex As String) As DataSet
            Dim objBCM As New BudgetControl
            Dim objDb As New EAD.DBCom
            Dim ds As DataSet
            Dim strSQL As String
            strSQL = "SELECT DISTINCT(CDM_DEPT_NAME),CDM_DEPT_INDEX FROM ACCOUNT_MSTR,COMPANY_DEPT_MSTR " &
               "WHERE CDM_DEPT_INDEX=AM_DEPT_INDEX AND AM_DELETED<>'Y' AND CDM_DELETED<>'Y' AND AM_ACCT_INDEX='" & pIndex & "'"
            ds = objDb.FillDs(strSQL)
            Return ds
        End Function

        'Name       : IsWithinBudget
        'Author     : kk
        'Descption  : check the initial budget is greater than sum up lower lever's budget
        'LastUpadte : 23/12/2004
        Public Function IsBudgetGreater(ByVal pAccCode As String, ByVal pAmt As Double, ByVal pDeptIndex As Integer, ByVal pLevel As Integer) As Boolean
            Dim objDb As New EAD.DBCom
            Dim strSQL As String
            Dim dblIniBudget, dblAssBudget, dblAmtLeft As Double
            Dim iAccIndex As Integer

            strSQL = "SELECT AM_ACCT_INDEX FROM ACCOUNT_MSTR WHERE AM_ACCT_CODE='" & Common.Parse(pAccCode) & "' AND AM_DEPT_INDEX=" & Common.Parse(pDeptIndex) & " AND AM_LEVEL=" & pLevel & " AND AM_DELETED<>'Y'"
            If objDb.Exist(strSQL) Then
                iAccIndex = objDb.GetVal(strSQL)
            Else
                iAccIndex = 0
            End If

            'dblIniBudget = getInitialBudget(iAccIndex)
            dblAssBudget = getAssignedBudget(iAccIndex, pAccCode, pDeptIndex, False)
            'dblAmtLeft = Math.Round(dblIniBudget - dblAssBudget, 2)

            If pAmt < dblAssBudget Then
                Return False
            Else
                Return True
            End If
        End Function

        'Name       : getInitialBudget
        'Author     : kk
        'Descption  : Get initial budget
        'LastUpadte : 23/12/2004
        Public Function getInitialBudget(ByVal pAccIndex As String) As Double
            Dim strSQL, sValue As String
            Dim objDb As New EAD.DBCom

            strSQL = " SELECT AM_INIT_BUDGET FROM ACCOUNT_MSTR " &
               " WHERE AM_ACCT_INDEX='" & Common.Parse(pAccIndex) & "' AND AM_DELETED<>'Y'"

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                sValue = IIf(IsDBNull(tDS.Tables(0).Rows(j).Item(0)), "", tDS.Tables(0).Rows(j).Item(0))
            Next

            If sValue <> "" Then
                Return Convert.ToDouble(sValue)
            Else
                Return 0
            End If

        End Function

        'Name       : getAssignedBudget
        'Author     : kk
        'Descption  : Get assigned budget
        'LastUpadte : 23/12/2004
        Public Function getAssignedBudget(ByVal pAccIndex As String, ByVal pAccCode As String, ByVal pDeptIndex As String, ByVal pAddAction As Boolean) As Double
            Dim strSQL, sValue As String
            Dim objDb As New EAD.DBCom

            If pAddAction Then
                strSQL = " SELECT ISNULL(SUM(AM_INIT_BUDGET),0) FROM ACCOUNT_MSTR " &
                   " WHERE AM_PARENT_ACCT_INDEX='" & Common.Parse(pAccIndex) & "' AND AM_DEPT_INDEX=" & Common.Parse(pDeptIndex) & " AND AM_DELETED<>'Y' "       ' AND AM_LEVEL=2 "
            Else
                strSQL = " SELECT ISNULL(SUM(AM_INIT_BUDGET),0) FROM ACCOUNT_MSTR " &
                  " WHERE AM_PARENT_ACCT_INDEX='" & pAccIndex & "' AND AM_ACCT_CODE  <> '" & Common.Parse(pAccCode) & "' AND AM_DEPT_INDEX=" & Common.Parse(pDeptIndex) & " AND AM_DELETED<>'Y' "       ' AND AM_LEVEL=2 "
            End If
            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                sValue = IIf(IsDBNull(tDS.Tables(0).Rows(j).Item(0)), "", tDS.Tables(0).Rows(j).Item(0))
            Next


            If sValue <> "" Then
                Return Convert.ToDouble(sValue)
            Else
                Return 0
            End If

        End Function

        'Name       : GetViewAll
        'Author     : kk
        'Descption  : Search for view budget all 4 level
        'LastUpadte : 
        'Public Function GetViewAll() As DataTable
        '    Dim strSQL As String
        '    Dim objDb As New EAD.DBCom
        '    Dim ds1, ds2, ds3 As DataSet

        '    '--Level Acc code"
        '    strSQL = "SELECT " &
        '    " CDM_DEPT_NAME,AM_ACCT_CODE, ISNULL(AM_BCF,0) AS AM_BCF, AM_INIT_BUDGET,AM_RESERVED_AMT,AM_COMMITTED_AMT,AM_UTILIZED_AMT " &
        '    " FROM ACCOUNT_MSTR ,COMPANY_DEPT_MSTR " &
        '    " WHERE AM_DEPT_INDEX=CDM_DEPT_INDEX AND AM_DELETED='N'AND AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=1 "

        '    ds1 = objDb.FillDs(strSQL)


        '    '--Level Sub Acc code"
        '    strSQL = "SELECT " &
        '    " L2.CDM_DEPT_NAME,L1.AM_ACCT_CODE,L2.AM_ACCT_CODE AS AM_SUB_ACCT_CODE, ISNULL(L2.AM_BCF,0) AS AM_BCF,L2.AM_INIT_BUDGET,L2.AM_RESERVED_AMT,L2.AM_COMMITTED_AMT,L2.AM_UTILIZED_AMT FROM (" &
        '    " SELECT AM_PARENT_ACCT_INDEX,CDM_DEPT_NAME,AM_ACCT_CODE, AM_BCF,AM_INIT_BUDGET,AM_RESERVED_AMT,AM_COMMITTED_AMT,AM_UTILIZED_AMT " &
        '    " FROM ACCOUNT_MSTR ,COMPANY_DEPT_MSTR " &
        '    " where AM_DEPT_INDEX=CDM_DEPT_INDEX AND AM_DELETED='N'AND AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=2 " &
        '    " )L2, ACCOUNT_MSTR L1 " &
        '     " WHERE L2.AM_PARENT_ACCT_INDEX=L1.AM_ACCT_INDEX AND AM_DELETED='N' "
        '    ds2 = objDb.FillDs(strSQL)


        '    '--Level proj proj
        '    strSQL = "SELECT " &
        '    " L2.CDM_DEPT_NAME,AM_ACCT_CODE,AM_SUB_ACCT_CODE,AM_PROJ_CODE,L2.AM_INIT_BUDGET, ISNULL(L2.AM_BCF,0) AS AM_BCF,L2.AM_RESERVED_AMT,L2.AM_COMMITTED_AMT,L2.AM_UTILIZED_AMT FROM (" &
        '    " SELECT CDM_DEPT_INDEX,L3.AM_ACCT_INDEX,L3.CDM_DEPT_NAME,L2.AM_ACCT_CODE AS AM_SUB_ACCT_CODE,L3.AM_ACCT_CODE AS AM_PROJ_CODE,L3.AM_ACCT_DESC,L3.AM_BCF,L3.AM_INIT_BUDGET,L3.AM_RESERVED_AMT,L3.AM_COMMITTED_AMT,L3.AM_UTILIZED_AMT,L3.AM_PARENT_ACCT_INDEX AS L3,L2.AM_PARENT_ACCT_INDEX AS L2 FROM (" &
        '    " SELECT CDM_DEPT_INDEX,AM_ACCT_INDEX,CDM_DEPT_NAME,AM_ACCT_CODE ,AM_ACCT_DESC, AM_BCF,AM_INIT_BUDGET,AM_RESERVED_AMT,AM_COMMITTED_AMT,AM_UTILIZED_AMT,AM_PARENT_ACCT_INDEX " &
        '    " FROM ACCOUNT_MSTR ,COMPANY_DEPT_MSTR " &
        '    " where AM_DEPT_INDEX=CDM_DEPT_INDEX AND AM_DELETED='N'AND AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=3 " &
        '    " )L3, ACCOUNT_MSTR L2 " &
        '    " WHERE L3.AM_PARENT_ACCT_INDEX=L2.AM_ACCT_INDEX AND AM_DELETED='N' " &
        '    " )L2, ACCOUNT_MSTR L1 " &
        '    " WHERE L2=L1.AM_ACCT_INDEX AND AM_DELETED='N' "

        '    ds3 = objDb.FillDs(strSQL)

        '    ds3.Merge(ds1, False)
        '    ds3.Merge(ds2, False)


        '    Dim dtAll As DataTable
        '    Dim SortExp As String = "CDM_DEPT_NAME,AM_ACCT_CODE,AM_SUB_ACCT_CODE,AM_PROJ_CODE,AM_INIT_BUDGET"

        '    dtAll = ds3.Tables(0).Clone
        '    'Sorting
        '    For Each row As DataRow In ds3.Tables(0).Select("", SortExp, DataViewRowState.CurrentRows)
        '        dtAll.ImportRow(row)
        '    Next

        '    Return dtAll
        'End Function

        'Name       : SearchBudget
        'Author     : kk
        'Descption  : Search for view budget base on 4 level
        'LastUpadte : 23/12/2004
        'Public Function SearchBudget(ByVal pFind As String, ByVal pLevel As Integer) As DataSet
        '    Dim strSQL As String
        '    If pLevel = 0 Then
        '        '--Dept Level
        '        strSQL = "SELECT CDM_DEPT_INDEX, CDM_DEPT_NAME,isnull(AM_BCF,0)as AM_BCF,isnull(AM_INIT_BUDGET,0)as AM_INIT_BUDGET, L.* " &
        '        " FROM( " &
        '        " select AM_LEVEL AS AM_ACCT_INDEX,AM_LEVEL AS AM_ACCT_CODE, " &
        '        " AM_LEVEL AS AM_SUB_ACCT_CODE, " &
        '        " AM_LEVEL AS AM_PROJ_CODE, " &
        '        " AM_LEVEL AS AM_ACCT_DESC, " &
        '        " SUM(AM_BCF) AS AM_BCF, " &
        '        " SUM(AM_INIT_BUDGET) AS AM_INIT_BUDGET, " &
        '        " SUM(AM_RESERVED_AMT) AS AM_RESERVED_AMT," &
        '        " SUM(AM_COMMITTED_AMT) AS AM_COMMITTED_AMT," &
        '        " SUM(AM_UTILIZED_AMT) AS AM_UTILIZED_AMT," &
        '        " AM_LEVEL, AM_DEPT_INDEX" &
        '        " from account_mstr where AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=1 AND AM_DELETED<>'Y'" &
        '        " group by AM_DEPT_INDEX,AM_LEVEL)L RIGHT JOIN COMPANY_DEPT_MSTR s ON L.AM_DEPT_INDEX=s.CDM_DEPT_INDEX " &
        '        " WHERE CDM_DELETED='N' AND CDM_COY_ID='" & ctx.Session("CompanyId") & "' "

        '        If pFind <> "" Then
        '            strSQL &= " AND CDM_DEPT_NAME " & Common.ParseSQL(pFind)
        '        End If
        '        'Zulham 30102018
        '        strSQL &= " GROUP BY CDM_DEPT_CODE ORDER BY CDM_DEPT_NAME"

        '    ElseIf pLevel = 1 Then
        '        '--Level Acc code"
        '        strSQL = "SELECT CDM_DEPT_INDEX,AM_ACCT_INDEX, " &
        '        " CDM_DEPT_NAME,AM_ACCT_CODE,AM_ACCT_DESC,AM_BCF,AM_INIT_BUDGET,AM_RESERVED_AMT,AM_COMMITTED_AMT,AM_UTILIZED_AMT,AM_PARENT_ACCT_INDEX,AM_LEVEL AS AM_SUB_ACCT_CODE,AM_LEVEL AS AM_PROJ_CODE " &
        '        " FROM ACCOUNT_MSTR ,COMPANY_DEPT_MSTR " &
        '        " WHERE AM_DEPT_INDEX=CDM_DEPT_INDEX AND AM_DELETED='N'AND AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=1 "

        '        If pFind <> "" Then
        '            strSQL &= "AND AM_LEVEL=1 AND AM_ACCT_CODE " & Common.ParseSQL(pFind)
        '        End If

        '        strSQL &= " ORDER BY CDM_DEPT_NAME,AM_ACCT_CODE"

        '    ElseIf pLevel = 2 Then
        '        '--Level Sub Acc code"
        '        strSQL = "SELECT CDM_DEPT_INDEX,L2.AM_ACCT_INDEX,  " &
        '        " L2.CDM_DEPT_NAME,L1.AM_ACCT_CODE,L2.AM_ACCT_CODE AS AM_SUB_ACCT_CODE,L2.AM_ACCT_DESC,L2.AM_BCF,L2.AM_INIT_BUDGET,L2.AM_RESERVED_AMT,L2.AM_COMMITTED_AMT,L2.AM_UTILIZED_AMT,L2.AM_PARENT_ACCT_INDEX, AM_LEVEL AS AM_PROJ_CODE FROM (" &
        '        " SELECT AM_ACCT_INDEX,CDM_DEPT_INDEX,CDM_DEPT_NAME,AM_ACCT_CODE,AM_ACCT_DESC,AM_BCF,AM_INIT_BUDGET,AM_RESERVED_AMT,AM_COMMITTED_AMT,AM_UTILIZED_AMT,AM_PARENT_ACCT_INDEX " &
        '        " FROM ACCOUNT_MSTR ,COMPANY_DEPT_MSTR " &
        '        " where AM_DEPT_INDEX=CDM_DEPT_INDEX AND AM_DELETED='N'AND AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=2 "

        '        If pFind <> "" Then
        '            strSQL &= " AND AM_ACCT_CODE " & Common.ParseSQL(pFind)
        '        End If

        '        strSQL &= " )L2, ACCOUNT_MSTR L1 " &
        '         " WHERE L2.AM_PARENT_ACCT_INDEX=L1.AM_ACCT_INDEX AND AM_DELETED='N' " &
        '         " ORDER BY L2.CDM_DEPT_NAME,L1.AM_ACCT_CODE "

        '    ElseIf pLevel = 3 Then
        '        '--Level proj proj
        '        strSQL = "SELECT L2.AM_ACCT_INDEX,CDM_DEPT_INDEX,L2.AM_ACCT_INDEX,CDM_DEPT_INDEX," &
        '        " L2.CDM_DEPT_NAME,AM_ACCT_CODE,AM_SUB_ACCT_CODE,AM_PROJ_CODE,L2.AM_ACCT_DESC,L2.AM_BCF,L2.AM_INIT_BUDGET,L2.AM_RESERVED_AMT,L2.AM_COMMITTED_AMT,L2.AM_UTILIZED_AMT FROM (" &
        '        " SELECT CDM_DEPT_INDEX,L3.AM_ACCT_INDEX,L3.CDM_DEPT_NAME,L2.AM_ACCT_CODE AS AM_SUB_ACCT_CODE,L3.AM_ACCT_CODE AS AM_PROJ_CODE,L3.AM_ACCT_DESC,L3.AM_BCF,L3.AM_INIT_BUDGET,L3.AM_RESERVED_AMT,L3.AM_COMMITTED_AMT,L3.AM_UTILIZED_AMT,L3.AM_PARENT_ACCT_INDEX AS L3,L2.AM_PARENT_ACCT_INDEX AS L2 FROM (" &
        '        " SELECT CDM_DEPT_INDEX,AM_ACCT_INDEX,CDM_DEPT_NAME,AM_ACCT_CODE ,AM_ACCT_DESC,AM_BCF,AM_INIT_BUDGET,AM_RESERVED_AMT,AM_COMMITTED_AMT,AM_UTILIZED_AMT,AM_PARENT_ACCT_INDEX " &
        '        " FROM ACCOUNT_MSTR ,COMPANY_DEPT_MSTR " &
        '        " where AM_DEPT_INDEX=CDM_DEPT_INDEX AND AM_DELETED='N'AND AM_COY_ID='" & ctx.Session("CompanyId") & "' AND AM_LEVEL=3 " &
        '        " )L3, ACCOUNT_MSTR L2 " &
        '        " WHERE L3.AM_PARENT_ACCT_INDEX=L2.AM_ACCT_INDEX AND AM_DELETED='N' " &
        '        " )L2, ACCOUNT_MSTR L1 " &
        '        " WHERE L2=L1.AM_ACCT_INDEX AND AM_DELETED='N' "

        '        If pFind <> "" Then
        '            strSQL &= " AND AM_PROJ_CODE " & Common.ParseSQL(pFind)
        '        End If

        '        strSQL &= " ORDER BY L2.CDM_DEPT_NAME,L1.AM_ACCT_CODE "
        '    End If

        '    Dim objDb As New EAD.DBCom
        '    Dim ds As DataSet
        '    ds = objDb.FillDs(strSQL)
        '    Return ds
        'End Function

        'Public Function ResetBCM() As Boolean
        '    Dim objDb As New EAD.DBCom
        '    Dim strSQL As String
        '    'Michelle (3/5/2010)
        '    'strSQL = "UPDATE ACCOUNT_MSTR SET AM_RESERVED_AMT=0,AM_COMMITTED_AMT=0,AM_UTILIZED_AMT=0 WHERE AM_DELETED='N'"
        '    strSQL = "UPDATE ACCOUNT_MSTR SET AM_BCF = (AM_RESERVED_AMT + AM_COMMITTED_AMT), " &
        '             " AM_UTILIZED_AMT=0 WHERE AM_DELETED='N' AND AM_COY_ID='" & ctx.Session("CompanyId") & "'"

        '    If objDb.Execute(strSQL) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        Public Function GetBCM() As DataView
            Dim objDb As New EAD.DBCom

            Dim companyID = String.Empty '[wip]

            Dim strBCM As String
            Dim drwBCM As New DataView

            strBCM = "SELECT CM_BCM_SET, CM_BUDGET_FROM_DATE, CM_BUDGET_TO_DATE FROM COMPANY_MSTR "
            strBCM &= "where CM_COY_ID = '" & companyID & "' AND CM_DELETED<>'Y' "

            drwBCM = objDb.GetView(strBCM)
            GetBCM = drwBCM

        End Function

        'Public Function UpdateBCMMode(ByVal strBCMSet As String) As Boolean
        '    Dim objDb As New EAD.DBCom
        '    Dim strSql As String

        '    strSql = "UPDATE COMPANY_MSTR "
        '    strSql &= "SET CM_BCM_SET ='" & strBCMSet & "' "
        '    strSql &= "WHERE CM_COY_ID ='" & ctx.Session("CompanyId") & "' AND CM_DELETED<>'Y'"

        '    If objDb.Execute(strSql) Then
        '        strMessage = Common.RecordSave
        '    Else
        '        strMessage = Common.RecordNotSave
        '    End If


        'End Function



        'Public Function UpdateBCMStartDt(ByVal strCurrentDate As String) As Boolean
        '    Dim objDb As New EAD.DBCom
        '    Dim strSql As String
        '    strSql = "UPDATE COMPANY_MSTR "
        '    strSql &= "SET CM_BUDGET_FROM_DATE = " & Common.ConvertDate(strCurrentDate) & " "
        '    strSql &= "WHERE CM_COY_ID ='" & ctx.Session("CompanyId") & "' AND CM_DELETED<>'Y' "

        '    If objDb.Execute(strSql) Then
        '        strMessage = Common.RecordSave
        '        Return True
        '    Else
        '        strMessage = Common.RecordNotSave
        '        Return False
        '    End If

        'End Function

        'Public Function UpdateBCMEndDt(ByVal strCurrentDate As String) As Boolean
        '    Dim objDb As New EAD.DBCom
        '    Dim strSql As String
        '    strSql = "UPDATE COMPANY_MSTR "
        '    strSql &= "SET CM_BUDGET_TO_DATE = " & Common.ConvertDate(strCurrentDate) & " "
        '    strSql &= "WHERE CM_COY_ID ='" & ctx.Session("CompanyId") & "' AND CM_DELETED<>'Y' "

        '    If objDb.Execute(strSql) Then
        '        strMessage = Common.RecordSave
        '        Return True
        '    Else
        '        strMessage = Common.RecordNotSave
        '        Return False
        '    End If

        'End Function

        'Public Function GetDeptTotalInitBudget(ByVal pDeptId As String) As String
        '    Dim objDb As New EAD.DBCom
        '    Dim strSql As String
        '    Dim dblIB As Double

        '    strSql = "SELECT SUM(AM_INIT_BUDGET)AS SUM " &
        '       "FROM ACCOUNT_MSTR " &
        '       "WHERE  AM_DELETED<>'Y' AND AM_LEVEL=1  AND " &
        '       "AM_COY_ID='" & ctx.Session("CompanyID") & "' AND AM_DEPT_INDEX= " & pDeptId

        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        dblIB = IIf(IsDBNull(tDS.Tables(0).Rows(j).Item("SUM")), 0, tDS.Tables(0).Rows(j).Item("SUM"))
        '    Next

        '    Return Format(dblIB, "##,##0.00")



        'End Function

        Public Function getBCMListByCompanyNew() As DataTable

            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String

            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
              & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' AND CDM_DELETED='N' ORDER BY CDM_DEPT_CODE" 'Jules 2018.11.05 - Filter out deleted records.

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A " _
            & " WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A WHERE " _
            & " AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2 AND AM_DELETED='N'"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A WHERE " _
            & " AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX from (" & strSqlLevel2 & ") Q LEFT JOIN " _
            & "(" & strSqlLevel3 & ") W " _
            & "ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.*, R.* from (" & strSqlLevel1 & ") E LEFT JOIN " _
            & "( " & strSql4 & " ) R " _
            & "ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX "

            strSql2 = "select T.*, Y.* from (" & strSqlDept & ") T INNER JOIN " _
            & "( " & strSql3 & " ) Y " _
            & "ON T.CDM_DEPT_INDEX = Y.AM_DEPT_INDEX " _
            & "ORDER BY CDM_DEPT_CODE"


            Dim dtLevel1, dtResult As DataTable
            dtLevel1 = ob.FillDs(strSql2).Tables(0)
            dtResult = BuildBCMListNew(dtLevel1).Tables(0)

            Return dtResult
        End Function

        Private Function BuildBCMListNew(ByVal dtLevel1 As DataTable, Optional ByVal blnByUser As Boolean = False) As DataSet
            Dim dtAvailList As New DataTable
            Dim dtSelectedList As New DataTable
            Dim drLevel1, drLevel2, drLevel3, drLevel4, drResult As DataRow
            Dim drsLevel2, drsLevel3, drsLevel4 As DataRow()
            Dim ds As New DataSet

            dtAvailList.Columns.Add("Acct_List", Type.GetType("System.String"))
            dtAvailList.Columns.Add("Acct_Index", Type.GetType("System.Int32"))
            dtAvailList.Columns.Add("Acct_Code", Type.GetType("System.String"))
            dtSelectedList = dtAvailList.Clone

            For Each drLevel1 In dtLevel1.Rows
                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        If IsDBNull(drLevel1("Q_AU_USER_ID")) Then
                            If IsDBNull(drLevel1("AU_USER_ID")) Then
                                drResult = dtAvailList.NewRow
                            Else
                                drResult = dtSelectedList.NewRow
                            End If
                        Else
                            drResult = dtSelectedList.NewRow
                        End If
                    Else
                        drResult = dtSelectedList.NewRow
                    End If
                Else
                    drResult = dtSelectedList.NewRow
                End If

                'drResult("Acct_List") = drLevel1("CDM_DEPT_CODE") & "-" & _
                'Common.parseNull(drLevel1("AM_ACCT_Desc")) & ":" & drLevel1("AM_ACCT_Code") & "-" & _
                'Common.parseNull(drLevel1("Q_AM_ACCT_Desc")) & ":" & drLevel1("Q_AM_ACCT_Code") & "-" & _
                'Common.parseNull(drLevel1("W_AM_ACCT_Desc")) & ":" & drLevel1("W_AM_ACCT_Code")
                'drResult("Acct_Index") = drLevel1("W_AM_ACCT_INDEX")
                'drResult("Acct_Code") = drLevel1("W_AM_ACCT_code")
                drResult("Acct_List") = drLevel1("CDM_DEPT_CODE")
                If Not IsDBNull(drLevel1("AM_ACCT_Desc")) Then
                    drResult("Acct_List") &= "-" & Common.parseNull(drLevel1("AM_ACCT_Desc")) & ":" & drLevel1("AM_ACCT_Code")
                End If
                If Not IsDBNull(drLevel1("Q_AM_ACCT_Desc")) Then
                    drResult("Acct_List") &= "-" & Common.parseNull(drLevel1("Q_AM_ACCT_Desc")) & ":" & drLevel1("Q_AM_ACCT_Code")
                End If
                If Not IsDBNull(drLevel1("W_AM_ACCT_Desc")) Then
                    drResult("Acct_List") &= "-" & Common.parseNull(drLevel1("W_AM_ACCT_Desc")) & ":" & drLevel1("W_AM_ACCT_Code")
                End If

                If IsDBNull(drLevel1("W_AM_ACCT_INDEX")) Then
                    If IsDBNull(drLevel1("Q_AM_ACCT_INDEX")) Then
                        If IsDBNull(drLevel1("AM_ACCT_INDEX")) Then
                        Else
                            drResult("Acct_Index") = drLevel1("AM_ACCT_INDEX")
                            drResult("Acct_Code") = drLevel1("AM_ACCT_CODE")
                        End If
                    Else
                        drResult("Acct_Index") = drLevel1("Q_AM_ACCT_INDEX")
                        drResult("Acct_Code") = drLevel1("Q_AM_ACCT_CODE")
                    End If
                Else
                    drResult("Acct_Index") = drLevel1("W_AM_ACCT_INDEX")
                    drResult("Acct_Code") = drLevel1("W_AM_ACCT_CODE")
                End If

                If blnByUser Then
                    If IsDBNull(drLevel1("W_AU_USER_ID")) Then
                        If IsDBNull(drLevel1("Q_AU_USER_ID")) Then
                            If IsDBNull(drLevel1("AU_USER_ID")) Then
                                dtAvailList.Rows.Add(drResult)
                            Else
                                dtSelectedList.Rows.Add(drResult)
                            End If
                        Else
                            dtSelectedList.Rows.Add(drResult)
                        End If
                    Else
                        dtSelectedList.Rows.Add(drResult)
                    End If
                Else          '//for getBCMListByCompany (no filtering)
                    dtSelectedList.Rows.Add(drResult)
                End If

            Next

            ds.Tables.Add(dtSelectedList)
            If blnByUser Then
                ds.Tables.Add(dtAvailList)
            End If
            Return ds
        End Function

        Public Function getBCMListByUserNew(ByVal strUser As String, Optional ByVal strCode As String = "") As DataSet

            'Dim strUser As String = HttpContext.Current.Session("UserId")
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            Dim ob As New EAD.DBCom

            Dim strSql1, strSql2, strSql3, strSql4 As String

            Dim strSqlDept, strSqlLevel1, strSqlLevel2, strSqlLevel3 As String
            'Zulham 06122018
            strSqlDept = "SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A, " _
            & " COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & strCoyId & "' AND CDM_DELETED = 'N' AND AM_DELETED='N' ORDER BY CDM_DEPT_CODE"

            strSqlLevel1 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel2 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=2  AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            strSqlLevel3 = "SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A " _
            & "LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX AND AU_USER_ID='" & strUser &
            "' WHERE AM_COY_ID='" & strCoyId & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE"

            'strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (" & strSqlLevel2 & ") Q inner join " _
            '& "(" & strSqlLevel3 & ") W " _
            '& " ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql4 = "select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (" & strSqlLevel2 & ") Q left join " _
            & "(" & strSqlLevel3 & ") W " _
            & " ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX"

            strSql3 = "select E.AM_DEPT_INDEX, E.AM_ACCT_INDEX, E.AM_ACCT_CODE, E.AM_ACCT_DESC, E.AM_PARENT_ACCT_INDEX, E.AU_USER_ID, R.Q_AM_DEPT_INDEX, R.Q_AM_ACCT_INDEX, R.Q_AM_ACCT_CODE, R.Q_AM_ACCT_DESC, Q_AM_PARENT_ACCT_INDEX, R.Q_AU_USER_ID, R.W_AM_DEPT_INDEX, R.W_AM_ACCT_INDEX, R.W_AM_ACCT_CODE, R.W_AM_ACCT_DESC, R.W_AM_PARENT_ACCT_INDEX, R.W_AU_USER_ID from (" & strSqlLevel1 & ") E left join " _
            & "( " & strSql4 & " ) R " _
            & " ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX"

            strSql2 = "select X.*, Y.* from (" & strSqlDept & ") X inner join " _
                            & "( " & strSql3 & " ) Y " _
                            & " ON X.CDM_DEPT_INDEX = Y.AM_DEPT_INDEX "

            'strSql1 = "Shape{select * from company_dept_mstr} append "
            Dim dtLevel1 As DataTable
            Dim dsBCM As DataSet
            Dim dsTempBCM As New DataSet
            dtLevel1 = ob.FillDs(strSql2).Tables(0)
            dsBCM = BuildBCMListNew(dtLevel1, True)

            Dim dtTempTable As New DataTable
            Dim dtTempTable1 As New DataTable
            If strCode <> "" Then
                Dim drFinalResult As DataRow()
                Dim drTemp As DataRow
                '//to make sure dtTempTable and dtResult have the same structure
                dtTempTable = dsBCM.Tables(0).Clone 'dtResult.Clone
                drFinalResult = dsBCM.Tables(0).Select("ACCT_code" & Common.ParseSQL(strCode))
                If drFinalResult.Length > 0 Then
                    For Each drTemp In drFinalResult
                        dtTempTable.ImportRow(drTemp)
                    Next
                    'Else
                    'dtTempTable = Nothing
                End If
            Else
                dtTempTable = dsBCM.Tables(0).Copy
            End If
            dsTempBCM.Tables.Add(dtTempTable) '//selected List
            dtTempTable1 = dsBCM.Tables(1).Copy
            dsTempBCM.Tables.Add(dtTempTable1) '//available list
            Return dsTempBCM
        End Function

        ''Jules 2014.02.03 - For Capex Enhancement.
        Public Function GetBR_GL_CC(ByVal strBranchCode As String, ByVal strGLCode As String, ByVal strCostCenter As String, ByVal strInterfaceCode As String, Optional ByVal blnFiltered As Boolean = False, Optional ByVal blnDistinct As Boolean = False) As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom
            Dim strsql As String
            Dim strTemp As String
            Dim strCon As String = ""

            If blnDistinct Then
                'strsql = "SELECT DISTINCT p.cdm_dept_code AS 'Branch Code' "
                strsql = "SELECT DISTINCT Z.CDM_DEPT_CODE AS 'Branch Code', Z.AM_ACCT_CODE AS 'GL Code' FROM " &
                        "(SELECT P.CDM_DEPT_CODE, P.AM_ACCT_CODE, Q.AM_ACCT_INDEX "
            Else
                'strsql = "SELECT p.cdm_dept_code AS 'Branch Code', p.am_acct_code AS 'GL Code', q.am_acct_code AS 'Cost Center', " & _
                '        "Q.AM_ACCT_DESC AS 'Cost Center Description', q.am_ACCT_INDEX AS 'Acct Index', M.IM_MAPPING_CODE AS 'Interface Code' "
                strsql = "SELECT Z.CDM_DEPT_CODE AS 'Branch Code', Z.P_AM_ACCT_CODE AS 'GL Code', Z.Q_AM_ACCT_CODE AS 'Cost Center', Z.AM_ACCT_DESC AS 'Cost Center Description' " &
                         ",Z.AM_ACCT_INDEX AS 'Acct Index',M.IM_MAPPING_CODE AS 'Interface Code' " &
                         "FROM (" &
                         "SELECT P.CDM_DEPT_CODE, P.AM_ACCT_CODE AS 'P_AM_ACCT_CODE', Q.AM_ACCT_CODE AS 'Q_AM_ACCT_CODE', Q.AM_ACCT_DESC,Q.AM_ACCT_INDEX "
            End If

            strsql &= "FROM " &
                    "(SELECT cdm_dept_code, AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR " &
                    "INNER JOIN COMPANY_DEPT_MSTR ON AM_COY_ID = CDM_COY_ID AND AM_DEPT_INDEX = cdm_dept_index WHERE " &
                    "AM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND AM_LEVEL=1 AND AM_DELETED='N') P " &
                    "INNER JOIN " &
                    "(SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A WHERE " &
                    "AM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND AM_LEVEL=2 AND AM_DELETED='N') Q ON P.AM_ACCT_INDEX = Q.AM_PARENT_ACCT_INDEX " &
                    "LEFT JOIN " &
                    "(SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX FROM ACCOUNT_MSTR A WHERE " &
                    "AM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE) W " &
                    "ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX) Z " &
                    "LEFT JOIN " &
                    "(SELECT IM_COY_ID,IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING I WHERE " &
                    "IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND IM_MAPPING_CODE <> '') M " &
                    "ON M.IM_ACCT_INDEX=Z.AM_ACCT_INDEX "

            If blnFiltered Then
                If strCon <> "" Then
                    strCon &= "AND M.IM_MAPPING_CODE IS NOT NULL AND M.IM_MAPPING_CODE <> '' "
                Else
                    strCon &= "WHERE M.IM_MAPPING_CODE IS NOT NULL AND M.IM_MAPPING_CODE <> ''"
                End If
            End If

            If strBranchCode <> "" Then
                strTemp = Common.BuildWildCard(strBranchCode)
                If strCon <> "" Then
                    strCon &= "AND Z.CDM_DEPT_CODE" & Common.ParseSQL(strTemp) & " "
                Else
                    strCon &= "WHERE Z.CDM_DEPT_CODE" & Common.ParseSQL(strTemp) & " "
                End If
            End If

            If strGLCode <> "" Then
                strTemp = Common.BuildWildCard(strGLCode)
                If strCon <> "" Then
                    strCon &= "AND Z.P_AM_ACCT_CODE" & Common.ParseSQL(strTemp) & " "
                Else
                    strCon &= "WHERE Z.P_AM_ACCT_CODE" & Common.ParseSQL(strTemp) & " "
                End If
            End If

            If strCostCenter <> "" Then
                strTemp = Common.BuildWildCard(strCostCenter)
                If strCon <> "" Then
                    strCon &= "AND Z.Q_AM_ACCT_CODE" & Common.ParseSQL(strTemp) & " "
                Else
                    strCon &= "WHERE Z.Q_AM_ACCT_CODE" & Common.ParseSQL(strTemp) & " "
                End If
            End If

            If strInterfaceCode <> "" Then
                strTemp = Common.BuildWildCard(strInterfaceCode)
                If strCon <> "" Then
                    strCon &= "AND M.IM_MAPPING_CODE" & Common.ParseSQL(strTemp) & " "
                Else
                    strCon &= "WHERE M.IM_MAPPING_CODE" & Common.ParseSQL(strTemp) & " "
                End If
            End If

            If blnDistinct Then
                strsql &= strCon & " ORDER BY Z.CDM_DEPT_CODE"
            Else
                strsql &= strCon & " ORDER BY Z.CDM_DEPT_CODE, Z.P_AM_ACCT_CODE, Z.Q_AM_ACCT_CODE, Z.AM_ACCT_DESC"
            End If
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        ''Jules 2014.02.03 - For Capex Enhancement.
        Public Function InsertAuditTrailInterfaceMapping(ByVal str_im_interface_index As String, ByVal str_im_acct_index As String, ByVal str_im_mapping_code As String,
        ByVal str_br_code As String, ByVal str_gl_code As String, ByVal str_cc As String, ByVal str_cc_desc As String, ByVal str_action As String)
            Dim objDb As New EAD.DBCom
            Dim strSql As String = "", pQuery(0) As String

            strSql = "INSERT INTO au_interface_mapping "
            strSql &= "(AU_IM_INTERFACE_INDEX,AU_IM_COY_ID,AU_IM_ACCT_INDEX,AU_IM_MAPPING_CODE,AU_BR_CODE,AU_GL_CODE,AU_CC,AU_CC_DESC,AU_USER,AU_DATE,AU_ACTION) "
            strSql &= "VALUES ("
            strSql &= "'" & str_im_interface_index & "', "
            strSql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
            strSql &= "'" & Common.Parse(str_im_acct_index) & "', "
            strSql &= "'" & Common.Parse(str_im_mapping_code) & "', "
            strSql &= "'" & Common.Parse(str_br_code) & "', "
            strSql &= "'" & Common.Parse(str_gl_code) & "', "
            strSql &= "'" & Common.Parse(str_cc) & "', "
            strSql &= "'" & Common.Parse(str_cc_desc) & "', "
            strSql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
            strSql &= Common.ConvertDate(Now()) & ", "
            strSql &= "'" & Common.Parse(str_action) & "')"
            Common.Insert2Ary(pQuery, strSql)
            objDb.BatchExecute(pQuery)
        End Function

        ''Jules 2014.02.03 - For Capex Enhancement.
        Public Function GetAccountMapping(ByVal strFromBranch As String, ByVal strFromGLCode As String, ByVal strFromCostCenter As String, ByVal strFromInterfaceCode As String,
        ByVal strToBranch As String, ByVal strToGLCode As String, ByVal strToCostCenter As String, ByVal strToInterfaceCode As String,
        ByVal strFromAcctIndex As String, ByVal strToAcctIndex As String) As DataSet
            Dim ds As DataSet
            Dim objDb As New EAD.DBCom
            Dim strsql As String
            Dim strTemp As String
            'Dim strCon As String = ""


            'If strFromAcctIndex <> "" And strToAcctIndex <> "" Then
            If strFromAcctIndex <> "" Then
                ''Jules 2014.05.08 - To ensure Account Mapping screen displays latest Interface Code.
                'strsql = "SELECT AM_ACCT_MAP_INDEX, AM_F_ACCT_INDEX, AM_F_BR_CODE, AM_F_GL_CODE, AM_F_CC, AM_F_CC_DESC, AM_F_MAP_CODE, " & _
                '        "AM_T_ACCT_INDEX, AM_T_BR_CODE, AM_T_GL_CODE, AM_T_CC, AM_T_CC_DESC, AM_T_MAP_CODE " & _
                '        "FROM ACCOUNT_MAPPING " & _
                '        "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                strsql = "SELECT AM_ACCT_MAP_INDEX,AM_COY_ID, AM_F_ACCT_INDEX, AM_F_BR_CODE, AM_F_GL_CODE, AM_F_CC, F.IM_MAPPING_CODE AS FRMAPCODE, " &
                        "AM_T_ACCT_INDEX, AM_T_BR_CODE, AM_T_GL_CODE, AM_T_CC, T.IM_MAPPING_CODE AS TMAPCODE " &
                        "FROM (SELECT AM_ACCT_MAP_INDEX,AM_COY_ID,AM_F_ACCT_INDEX,AM_F_BR_CODE,AM_F_GL_CODE,AM_F_CC, " &
                        "AM_T_ACCT_INDEX,AM_T_BR_CODE,AM_T_GL_CODE,AM_T_CC " &
                        "FROM ACCOUNT_MAPPING WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') A " &
                        "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') F " &
                        "ON A.AM_F_ACCT_INDEX = F.IM_ACCT_INDEX " &
                        "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "') T " &
                        "ON A.AM_T_ACCT_INDEX = T.IM_ACCT_INDEX " &
                        "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            Else
                ''Jules 2014.05.08 - To ensure Account Mapping screen displays latest Interface Code.
                'strsql = "SET @row_num = 0; " & _
                '        "SELECT AM_ACCT_MAP_INDEX, AM_F_ACCT_INDEX, CONCAT(AM_F_BR_CODE, ' : ', AM_F_GL_CODE, ' : ', AM_F_CC, ' : ', AM_F_MAP_CODE) AS FROMCODE, " & _
                '        "AM_T_ACCT_INDEX, CONCAT(AM_T_BR_CODE, ' : ', AM_T_GL_CODE, ' : ', AM_T_CC, ' : ', AM_T_MAP_CODE) As TOCODE " & _
                '        ", @row_num := @row_num + 1 AS row_index " & _
                '        "FROM ACCOUNT_MAPPING " & _
                '        "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "             
                strsql = "SET @row_num = 0; " &
                        "SELECT AM_ACCT_MAP_INDEX, AM_COY_ID, AM_F_ACCT_INDEX, CONCAT(AM_F_BR_CODE, ' : ', AM_F_GL_CODE, ' : ', AM_F_CC, ' : ', F.IM_MAPPING_CODE) AS FROMCODE, " &
                        "AM_T_ACCT_INDEX, CONCAT(AM_T_BR_CODE, ' : ', AM_T_GL_CODE, ' : ', AM_T_CC, ' : ', T.IM_MAPPING_CODE) As TOCODE, " &
                        "@row_num := @row_num + 1 AS row_index " &
                        "FROM (SELECT AM_ACCT_MAP_INDEX,AM_COY_ID,AM_F_ACCT_INDEX,AM_F_BR_CODE,AM_F_GL_CODE,AM_F_CC, " &
                        "AM_T_ACCT_INDEX,AM_T_BR_CODE,AM_T_GL_CODE,AM_T_CC " &
                        "FROM ACCOUNT_MAPPING WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') A " &
                        "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') F " &
                        "ON A.AM_F_ACCT_INDEX = F.IM_ACCT_INDEX " &
                        "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') T " &
                        "ON A.AM_T_ACCT_INDEX = T.IM_ACCT_INDEX " &
                        "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            End If

            If strFromBranch <> "" Then
                strTemp = Common.BuildWildCard(strFromBranch)
                strsql &= "AND AM_F_BR_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strFromGLCode <> "" Then
                strTemp = Common.BuildWildCard(strFromGLCode)
                strsql &= "AND AM_F_GL_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strFromCostCenter <> "" Then
                strTemp = Common.BuildWildCard(strFromCostCenter)
                strsql &= "AND AM_F_CC" & Common.ParseSQL(strTemp) & " "
            End If

            If strFromInterfaceCode <> "" Then
                strTemp = Common.BuildWildCard(strFromInterfaceCode)
                'strsql &= "AND AM_F_MAP_CODE" & Common.ParseSQL(strTemp) & " "
                strsql &= "AND F.IM_MAPPING_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strToBranch <> "" Then
                strTemp = Common.BuildWildCard(strToBranch)
                strsql &= "AND AM_T_BR_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strToGLCode <> "" Then
                strTemp = Common.BuildWildCard(strToGLCode)
                strsql &= "AND AM_T_GL_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strToCostCenter <> "" Then
                strTemp = Common.BuildWildCard(strToCostCenter)
                strsql &= "AND AM_T_CC" & Common.ParseSQL(strTemp) & " "
            End If

            If strToInterfaceCode <> "" Then
                strTemp = Common.BuildWildCard(strToInterfaceCode)
                'strsql &= "AND AM_T_MAP_CODE" & Common.ParseSQL(strTemp) & " "
                strsql &= "AND T.IM_MAPPING_CODE" & Common.ParseSQL(strTemp) & " "
            End If

            If strFromAcctIndex <> "" Then
                strsql &= "AND AM_F_ACCT_INDEX =" & Common.Parse(strFromAcctIndex) & " "
            End If

            If strToAcctIndex <> "" Then
                strsql &= "AND AM_T_ACCT_INDEX =" & Common.Parse(strToAcctIndex) & " "
            End If

            If strFromAcctIndex = "" And strToAcctIndex = "" Then
                'strsql &= "ORDER BY row_index;"            
                strsql &= "ORDER BY FROMCODE"
            End If

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        ''Jules 2014.02.25 - For Capex Enhancement.
        Public Function SaveAccountMapping(ByVal aryDoc As ArrayList) As Boolean
            Dim i As Integer
            Dim ds As New DataSet
            Dim strsql, strsqlA, strFromAcctIdx, strFromCCDesc, strToCCDesc, strToAcctIdx, strAryQuery(0) As String
            Dim objBC As New BudgetControl
            Dim objDb As New EAD.DBCom


            For i = 0 To aryDoc.Count - 1
                ''Check From account exists
                If aryDoc.Item(i)(0) <> "" And aryDoc.Item(i)(2) <> "" Then
                    ds = objBC.GetBR_GL_CC(aryDoc.Item(i)(0), "", aryDoc.Item(i)(2), "")
                    If ds.Tables(0).Rows.Count > 0 Then
                        ''Get the Acct Index
                        strFromAcctIdx = ds.Tables(0).Rows(0).Item(4).ToString
                        strFromCCDesc = ds.Tables(0).Rows(0).Item(3).ToString
                        ''Check To account exists
                        ds = objBC.GetBR_GL_CC(aryDoc.Item(i)(4), "", aryDoc.Item(i)(6), "")
                        If ds.Tables(0).Rows.Count > 0 Then
                            strToAcctIdx = ds.Tables(0).Rows(0).Item(4).ToString
                            strToCCDesc = ds.Tables(0).Rows(0).Item(3).ToString

                            ''Check if mapping exists
                            strsql = "SELECT * FROM Account_Mapping A " &
                                    "RIGHT JOIN " &
                                    "(SELECT * FROM account_mapping WHERE AM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                                    "AND AM_F_BR_CODE = '" & Common.Parse(aryDoc.Item(i)(0)) & "' " &
                                    "AND AM_F_GL_CODE = '" & Common.Parse(aryDoc.Item(i)(1)) & "'  AND AM_F_CC = '" & Common.Parse(aryDoc.Item(i)(2)) & "') B " &
                                    "ON A.AM_F_ACCT_INDEX=B.AM_F_ACCT_INDEX " &
                                    "WHERE A.AM_T_BR_CODE = '" & Common.Parse(aryDoc.Item(i)(4)) & "' AND A.AM_T_GL_CODE = '" & Common.Parse(aryDoc.Item(i)(5)) & "' " &
                                    "AND A.AM_T_CC = '" & Common.Parse(aryDoc.Item(i)(6)) & "'"
                            ds = objDb.FillDs(strsql)
                            If ds.Tables(0).Rows.Count = 0 Then
                                strsql = "INSERT INTO ACCOUNT_MAPPING(" &
                                        "AM_COY_ID,AM_F_ACCT_INDEX,AM_F_BR_CODE,AM_F_GL_CODE,AM_F_CC,AM_F_CC_DESC,AM_F_MAP_CODE,AM_T_ACCT_INDEX," &
                                        "AM_T_BR_CODE,AM_T_GL_CODE,AM_T_CC,AM_T_CC_DESC,AM_T_MAP_CODE) " &
                                        "VALUES('" & HttpContext.Current.Session("CompanyId") & "'," &
                                        "'" & Common.Parse(strFromAcctIdx) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(0)) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(1)) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(2)) & "'," &
                                        "'" & Common.Parse(strFromCCDesc) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(3)) & "'," &
                                        "'" & Common.Parse(strToAcctIdx) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(4)) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(5)) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(6)) & "'," &
                                        "'" & Common.Parse(strToCCDesc) & "'," &
                                        "'" & Common.Parse(aryDoc.Item(i)(7)) & "')"

                                'Common.Insert2Ary(strAryQuery, strsql)
                                'objDb.BatchExecute(strAryQuery)
                                If objDb.Execute(strsql) Then

                                    ''Insert audit record
                                    'strsqlA = "SELECT * FROM account_mapping WHERE AM_F_BR_CODE ='" & aryDoc.Item(i)(0) & "' AND AM_F_GL_CODE = '" & aryDoc.Item(i)(1) & "' AND AM_F_CC = '" & aryDoc.Item(i)(2) & "'"
                                    strsqlA = "SELECT AM_ACCT_MAP_INDEX,AM_COY_ID, AM_F_ACCT_INDEX, AM_F_BR_CODE, AM_F_GL_CODE, AM_F_CC, AM_F_CC_DESC, F.IM_MAPPING_CODE AS FRMAPCODE, " &
                                            "AM_T_ACCT_INDEX, AM_T_BR_CODE, AM_T_GL_CODE, AM_T_CC, AM_T_CC_DESC, T.IM_MAPPING_CODE AS TMAPCODE " &
                                            "FROM (SELECT AM_ACCT_MAP_INDEX,AM_COY_ID,AM_F_ACCT_INDEX,AM_F_BR_CODE,AM_F_GL_CODE,AM_F_CC,AM_F_CC_DESC,  " &
                                            "AM_T_ACCT_INDEX,AM_T_BR_CODE,AM_T_GL_CODE,AM_T_CC,AM_T_CC_DESC " &
                                            "FROM ACCOUNT_MAPPING WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') A " &
                                            "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') F " &
                                            "ON A.AM_F_ACCT_INDEX = F.IM_ACCT_INDEX " &
                                            "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') T " &
                                            "ON A.AM_T_ACCT_INDEX = T.IM_ACCT_INDEX " &
                                            "WHERE AM_F_BR_CODE ='" & aryDoc.Item(i)(0) & "' AND AM_F_GL_CODE = '" & aryDoc.Item(i)(1) & "' AND AM_F_CC = '" & aryDoc.Item(i)(2) & "' " &
                                            "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                                    ds = objDb.FillDs(strsqlA)
                                    If ds.Tables(0).Rows.Count > 0 Then
                                        strsqlA = "INSERT INTO AU_ACCOUNT_MAPPING (" &
                                        "AU_AM_ACCT_MAP_INDEX,AU_AM_COY_ID, AU_AM_F_ACCT_INDEX, AU_AM_F_BR_CODE, AU_AM_F_GL_CODE, AU_AM_F_CC, AU_AM_F_CC_DESC, AU_AM_F_MAP_CODE, " &
                                        "AU_AM_T_ACCT_INDEX, AU_AM_T_BR_CODE, AU_AM_T_GL_CODE, AU_AM_T_CC, AU_AM_T_CC_DESC, AU_AM_T_MAP_CODE, AU_USER, AU_DATE, AU_ACTION) " &
                                        "VALUES ('" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_ACCT_MAP_INDEX")) & "'," &
                                        "'" & HttpContext.Current.Session("CompanyId") & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_ACCT_INDEX")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_BR_CODE")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_GL_CODE")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_CC")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_CC_DESC")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("FRMAPCODE")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_ACCT_INDEX")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_BR_CODE")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_GL_CODE")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_CC")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_CC_DESC")) & "'," &
                                        "'" & Common.Parse(ds.Tables(0).Rows(0).Item("TMAPCODE")) & "'," &
                                        "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "'," &
                                        Common.ConvertDate(Now()) & "," &
                                        "'N')"

                                        'Common.Insert2Ary(strAryQuery, strsqlA)
                                        'objDb.BatchExecute(strAryQuery)
                                        objDb.Execute(strsqlA)
                                    Else
                                        Return False

                                    End If
                                End If
                            Else
                                Return False
                            End If

                        End If
                    End If
                End If
            Next
            Return True
        End Function

        ''Jules 2014.02.25 - For Capex Enhancement.
        Public Function GetLatestLineNoForAccountMapping() As String
            Dim objDb As New EAD.DBCom
            Dim lineno As String
            lineno = objDb.GetMax("ACCOUNT_MAPPING", "AM_ACCT_MAP_INDEX", " WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            Return lineno
        End Function

        ''Jules 2014.02.25 - For Capex Enhancement.
        Public Function UpdateAccountMapping(ByVal aryDoc As ArrayList) As Boolean
            Dim ds, dsCheck As DataSet
            Dim objDb As New EAD.DBCom
            Dim objBC As New BudgetControl
            Dim strsql, strsqlA As String
            Dim strAryQuery(0) As String

            strsql = "SELECT AM_ACCT_MAP_INDEX, AM_F_ACCT_INDEX, AM_F_BR_CODE, AM_F_GL_CODE, AM_F_CC, AM_F_CC_DESC, AM_F_MAP_CODE, " &
                    "AM_T_ACCT_INDEX, AM_T_BR_CODE, AM_T_GL_CODE, AM_T_CC, AM_T_CC_DESC, AM_T_MAP_CODE " &
                    "FROM ACCOUNT_MAPPING " &
                    "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND AM_ACCT_MAP_INDEX = " & aryDoc.Item(0)(8) & ""
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                'Check To account exists
                strsql = "SELECT * FROM interface_mapping WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                        "AND IM_MAPPING_CODE='" & Common.Parse(aryDoc.Item(0)(7)) & "'"
                ds = objDb.FillDs(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    dsCheck = objBC.GetBR_GL_CC(Common.Parse(aryDoc.Item(0)(4)), Common.Parse(aryDoc.Item(0)(5)), Common.Parse(aryDoc.Item(0)(6)), Common.Parse(aryDoc.Item(0)(7)))
                    If dsCheck.Tables(0).Rows.Count > 0 Then
                        strsql = "UPDATE account_mapping " &
                                "SET AM_T_BR_CODE = '" & Common.Parse(aryDoc.Item(0)(4)) & "', AM_T_GL_CODE = '" & Common.Parse(aryDoc.Item(0)(5)) & "', " &
                                "AM_T_CC = '" & Common.Parse(aryDoc.Item(0)(6)) & "', AM_T_CC_DESC = '" & Common.Parse(dsCheck.Tables(0).Rows(0).Item(3)) & "', " &
                                "AM_T_MAP_CODE = '" & Common.Parse(aryDoc.Item(0)(7)) & "', AM_T_ACCT_INDEX = " & Common.Parse(ds.Tables(0).Rows(0).Item("IM_ACCT_INDEX")) &
                                " WHERE AM_ACCT_MAP_INDEX = " & aryDoc.Item(0)(8) & " AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        'Common.Insert2Ary(strAryQuery, strsql)
                        'objDb.BatchExecute(strAryQuery)
                        If objDb.Execute(strsql) Then

                            ''Insert audit record
                            'strsqlA = "SELECT * FROM account_mapping WHERE AM_ACCT_MAP_INDEX = " & aryDoc.Item(0)(8) & ""
                            strsqlA = "SELECT AM_ACCT_MAP_INDEX,AM_COY_ID, AM_F_ACCT_INDEX, AM_F_BR_CODE, AM_F_GL_CODE, AM_F_CC, AM_F_CC_DESC, F.IM_MAPPING_CODE AS FRMAPCODE, " &
                                    "AM_T_ACCT_INDEX, AM_T_BR_CODE, AM_T_GL_CODE, AM_T_CC, AM_T_CC_DESC, T.IM_MAPPING_CODE AS TMAPCODE " &
                                    "FROM (SELECT AM_ACCT_MAP_INDEX,AM_COY_ID,AM_F_ACCT_INDEX,AM_F_BR_CODE,AM_F_GL_CODE,AM_F_CC,AM_F_CC_DESC,  " &
                                    "AM_T_ACCT_INDEX,AM_T_BR_CODE,AM_T_GL_CODE,AM_T_CC,AM_T_CC_DESC " &
                                    "FROM ACCOUNT_MAPPING WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') A " &
                                    "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') F " &
                                    "ON A.AM_F_ACCT_INDEX = F.IM_ACCT_INDEX " &
                                    "INNER JOIN (SELECT IM_ACCT_INDEX,IM_MAPPING_CODE FROM INTERFACE_MAPPING WHERE IM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "') T " &
                                    "ON A.AM_T_ACCT_INDEX = T.IM_ACCT_INDEX " &
                                    "WHERE AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                                    "AND AM_ACCT_MAP_INDEX = " & aryDoc.Item(0)(8) & ""
                            ds = objDb.FillDs(strsqlA)
                            If ds.Tables(0).Rows.Count > 0 Then
                                strsqlA = "INSERT INTO AU_ACCOUNT_MAPPING (" &
                                "AU_AM_ACCT_MAP_INDEX,AU_AM_COY_ID, AU_AM_F_ACCT_INDEX, AU_AM_F_BR_CODE, AU_AM_F_GL_CODE, AU_AM_F_CC, AU_AM_F_CC_DESC, AU_AM_F_MAP_CODE, " &
                                "AU_AM_T_ACCT_INDEX, AU_AM_T_BR_CODE, AU_AM_T_GL_CODE, AU_AM_T_CC, AU_AM_T_CC_DESC, AU_AM_T_MAP_CODE, AU_USER, AU_DATE, AU_ACTION) " &
                                "VALUES ('" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_ACCT_MAP_INDEX")) & "'," &
                                "'" & HttpContext.Current.Session("CompanyId") & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_ACCT_INDEX")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_BR_CODE")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_GL_CODE")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_CC")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_CC_DESC")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("FRMAPCODE")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_ACCT_INDEX")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_BR_CODE")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_GL_CODE")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_CC")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_CC_DESC")) & "'," &
                                "'" & Common.Parse(ds.Tables(0).Rows(0).Item("TMAPCODE")) & "'," &
                                "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "'," &
                                Common.ConvertDate(Now()) & "," &
                                "'M')"

                                'Common.Insert2Ary(strAryQuery, strsqlA)
                                'objDb.BatchExecute(strAryQuery)
                                objDb.Execute(strsqlA)
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
            End If
            Return True
        End Function

        ''Jules 2014.02.25 - For Capex Enhancement.
        Public Function DeleteAccountMapping(ByVal aryDoc As ArrayList) As Boolean
            Dim objDb As New EAD.DBCom
            Dim objBC As New BudgetControl
            Dim strSQL, strsqlA As String
            Dim i As Integer
            Dim ds As DataSet

            For i = 0 To aryDoc.Count - 1
                strSQL = "SELECT * FROM ACCOUNT_MAPPING WHERE AM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                        "AND AM_ACCT_MAP_INDEX = " & aryDoc.Item(i)(0) & ""
                ds = objDb.FillDs(strSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    strsqlA = "INSERT INTO AU_ACCOUNT_MAPPING (" &
                             "AU_AM_ACCT_MAP_INDEX,AU_AM_COY_ID, AU_AM_F_ACCT_INDEX, AU_AM_F_BR_CODE, AU_AM_F_GL_CODE, AU_AM_F_CC, AU_AM_F_CC_DESC, AU_AM_F_MAP_CODE, " &
                             "AU_AM_T_ACCT_INDEX, AU_AM_T_BR_CODE, AU_AM_T_GL_CODE, AU_AM_T_CC, AU_AM_T_CC_DESC, AU_AM_T_MAP_CODE, AU_USER, AU_DATE, AU_ACTION) " &
                             "VALUES ('" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_ACCT_MAP_INDEX")) & "'," &
                             "'" & HttpContext.Current.Session("CompanyId") & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_ACCT_INDEX")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_BR_CODE")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_GL_CODE")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_CC")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_CC_DESC")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_F_MAP_CODE")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_ACCT_INDEX")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_BR_CODE")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_GL_CODE")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_CC")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_CC_DESC")) & "'," &
                             "'" & Common.Parse(ds.Tables(0).Rows(0).Item("AM_T_MAP_CODE")) & "'," &
                             "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "'," &
                             Common.ConvertDate(Now()) & "," &
                             "'D')"

                    strSQL = "DELETE FROM ACCOUNT_MAPPING WHERE AM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " &
                            "AND AM_ACCT_MAP_INDEX = " & aryDoc.Item(i)(0) & ""

                    If objDb.Execute(strSQL) Then
                        objDb.Execute(strsqlA)
                    Else
                        Return False
                    End If

                End If
            Next
            'If objDb.Execute(strSQL) Then
            'strMessage = Common.RecordDelete
            Return True
            'Else
            '    strMessage = Common.RecordNotDelete
            '    Return False
            'End If
        End Function

    End Class
End Namespace


















