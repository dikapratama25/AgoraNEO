'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports System
Imports AgoraLegacy
Imports System.Configuration
Imports System.Web
Imports SSO.Component
Imports System.Web.UI.WebControls

Namespace AgoraNEO
    Public Class IPPDetails
        Public DocType As String
        Public DocNo As String
        Public DocDate As String
        Public ManualPONo As String
        Public Vendor As String
        Public VAddr As String
        Public Remarks As String
        Public Remark As String
        Public Status As String
        Public Currency As String
        Public PaymentAmt As String
        Public PaymentMethod As String
        Public WHTTax As String
        Public WHTOpt As String
        Public WHTReason As String
        Public PaymentType As String
        Public ExchangeRate As String
        Public LateSubmitReason As String
        Public BankNameAccountNo As String
        Public PaymentDate As String
        Public PaymentNo As String
        Public DocDueDate As String
        Public PRCSReceivedDate As String
        Public PRCSSentDate As String
        Public BankerChequeNo As String
        Public StatusDescription As String
        Public BeneficiaryDetails As String
        Public BillInvApprBy As String
        Public CreditTerm As String
        Public MasterDoc As String
        Public jobGrade As String
        Public GSTRegNo As String
        'Zulham 15/02/2016 - IPP Stage Phase 2
        Public TotalAmtNoGST As String
        Public GSTAmt As String

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

    'Zulham 06/02/2015 Case 8317
    Public Class BillingDetails
        Public DocType As String
        Public DocNo As String
        Public DocDate As String
        Public ManualPONo As String
        Public Vendor As String
        Public VAddr As String
        Public Remarks As String
        Public Remark As String
        Public Status As String
        Public Currency As String
        Public PaymentAmt As String
        Public PaymentAmtWthGST As String
        Public PaymentType As String
        Public ExchangeRate As String
        Public LateSubmitReason As String
        Public BankNameAccountNo As String
        Public PaymentDate As String
        Public PaymentNo As String
        Public DocDueDate As String
        Public StatusDescription As String
        Public BeneficiaryDetails As String
        Public BillInvApprBy As String
        Public jobGrade As String
        Public GSTRegNo As String

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

    Public Class IPP
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals
        Dim strMessage As String

        Public Function GetBranchInfo(ByRef strBranchCode As String, ByRef strBranchName As String, ByVal compid As String) As DataSet
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            If compid = "Own Co." Or compid = "" Then
                compid = HttpContext.Current.Session("CompanyID")
            End If
            'Get branch code & branch description
            strSQL = "SELECT CDM_DEPT_CODE, CDM_DEPT_NAME " _
                & "FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID ='" & Common.Parse(compid) & "' and cdm_deleted = 'N' " _
                & "AND LEFT(cdm_dept_code,2) IN ('HO','BR') AND LOCATE('-',cdm_dept_code,3) AND LOCATE('-',cdm_dept_code,7)  AND LENGTH(cdm_dept_code) = '10' " ' for HLB Dept code only, to prevent GL Entry have NULL GL Code

            If strBranchCode <> "" Then
                strSQL &= "AND CDM_DEPT_CODE LIKE '%" & Common.Parse(strBranchCode) & "%' "
            End If
            If strBranchName <> "" Then
                strSQL &= "AND CDM_DEPT_NAME LIKE '%" & Common.Parse(strBranchName) & "%' "
            End If
            'If strBranchStatus <> "" Then
            '    strSQL &= "AND CB_STATUS LIKE '%" & Common.Parse(strBranchStatus) & "%' "
            'End If

            'strSQL = "SELECT CB_BRANCH_CODE, CB_BRANCH_NAME, CB_STATUS " _
            '& "FROM company_branch WHERE CB_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            'If strBranchCode <> "" Then
            '    strSQL &= "AND CB_BRANCH_CODE LIKE '%" & Common.Parse(strBranchCode) & "%' "
            'End If
            'If strBranchName <> "" Then
            '    strSQL &= "AND CB_BRANCH_NAME LIKE '%" & Common.Parse(strBranchName) & "%' "
            'End If
            'If strBranchStatus <> "" Then
            '    strSQL &= "AND CB_STATUS LIKE '%" & Common.Parse(strBranchStatus) & "%' "
            'End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strBranchCode = ds1.Tables(0).Rows(0).Item("CDM_DEPT_CODE")
                strBranchName = ds1.Tables(0).Rows(0).Item("CDM_DEPT_NAME")
                'strBranchStatus = ds1.Tables(0).Rows(0).Item("CB_STATUS")
            Else
                strBranchCode = "Branch Code"
                strBranchName = "Name"
                ' strBranchStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function GetBranchInfoInfoSearch(ByRef strBranchCode As String, ByRef strBranchName As String, ByRef chkActive As Boolean, ByRef chkInactive As Boolean)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strBranchStatus As String

            'Get branch code & branch description
            strSQL = "SELECT CB_BRANCH_CODE, CB_BRANCH_NAME, CB_STATUS " _
                & "FROM company_branch WHERE CB_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strBranchCode <> "" Then
                strSQL &= "AND CB_BRANCH_CODE LIKE '%" & Common.Parse(strBranchCode) & "%' "
            End If
            If strBranchName <> "" Then
                strSQL &= "AND CB_BRANCH_NAME LIKE '%" & Common.Parse(strBranchName) & "%' "
            End If
            If chkActive = True Then
                strSQL &= "AND CB_STATUS ='A' "
            End If
            If chkInactive = True Then
                strSQL &= "AND CB_STATUS ='I' "
            End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strBranchCode = ds1.Tables(0).Rows(0).Item("CB_BRANCH_CODE")
                strBranchName = ds1.Tables(0).Rows(0).Item("CB_BRANCH_NAME")
                strBranchStatus = ds1.Tables(0).Rows(0).Item("CB_STATUS")
            Else
                strBranchCode = "Branch Code"
                strBranchName = "Name"
                strBranchStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function PopulateBranch() As DataSet
            Dim strSql As String
            Dim dsBranch As DataSet

            strSql = "SELECT CB_BRANCH_CODE, CB_BRANCH_NAME, CB_STATUS " _
                & "FROM company_branch WHERE CB_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "

            dsBranch = objDb.FillDs(strSql)
            PopulateBranch = dsBranch

        End Function

        Public Function AddBranch(ByVal strBranchCode As String, ByVal strBranchName As String, ByVal strBranchStatus As String) As String
            Dim strSQL As String
            'If strBranchName = "" Then
            '    strSQL = "SELECT * FROM company_branch WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '            & "AND CB_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' AND (CB_BRANCH_NAME='' OR CB_BRANCH_NAME IS NULL)"

            'Else
            strSQL = "SELECT * FROM company_branch WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND CB_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' OR CB_BRANCH_NAME='" & Common.Parse(strBranchName) & "'"

            'End If
            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "INSERT INTO company_branch (CB_COY_ID,CB_BRANCH_CODE,CB_BRANCH_NAME,CB_STATUS,CB_ENT_BY,CB_ENT_DATETIME,CB_MOD_BY,CB_MOD_DATETIME) "
                strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                If Common.Parse(strBranchCode) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchCode) & "',"
                End If
                If Common.Parse(strBranchName) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchName) & "',"
                End If
                If Common.Parse(strBranchStatus) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchStatus) & "',"
                End If
                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    AddBranch = WheelMsgNum.Save
                Else
                    AddBranch = WheelMsgNum.NotSave
                End If

            Else
                AddBranch = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function DeleteBranch(ByVal dtBranch As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim drCB As DataRow

            For Each drCB In dtBranch.Rows
                strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_branch_code = cb_cc_code " _
                        & "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_s_coy_id = id_s_coy_id " _
                        & "LEFT JOIN invoice_details_alloc " _
                        & "ON im_invoice_index = ida_invoice_index " _
                        & "WHERE im_invoice_status = 10 " _
                        & "AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND ida_branch_code = '" & Common.Parse(drCB("CBCode")) & "' "
                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM company_branch WHERE CB_BRANCH_CODE='" & Common.Parse(drCB("CBCode")) & "'"
                If objDb.Exist(strSQL) <> 0 Then
                    strSQL = "DELETE FROM company_branch WHERE CB_BRANCH_CODE='" & Common.Parse(drCB("CBCode")) & "'"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function ModBranch(ByVal strBranchCode As String, ByVal strBranchName As String, ByVal strBranchStatus As String) As String
            Dim strSQL As String
            'If strBranchName = "" Then
            '    strSQL = "SELECT * FROM company_branch WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '            & "AND CB_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' AND (CB_BRANCH_NAME='' OR CB_BRANCH_NAME IS NULL) " _
            '            & "AND CB_STATUS='" & Common.Parse(strBranchStatus) & "' "

            'Else
            strSQL = "SELECT * FROM company_branch WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND CB_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' AND CB_BRANCH_NAME='" & Common.Parse(strBranchName) & "' "

            'End If
            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "UPDATE company_branch SET "
                If Common.Parse(strBranchCode) = "" Then
                    strSQL &= "CB_BRANCH_CODE=NULL, "
                Else
                    strSQL &= "CB_BRANCH_CODE='" & Common.Parse(strBranchCode) & "',"
                End If
                If Common.Parse(strBranchName) = "" Then
                    strSQL &= "CB_BRANCH_NAME=NULL, "
                Else
                    strSQL &= "CB_BRANCH_NAME='" & Common.Parse(strBranchName) & "',"
                End If
                If Common.Parse(strBranchStatus) = "" Then
                    strSQL &= "CB_STATUS=NULL, "
                Else
                    strSQL &= "CB_STATUS='" & Common.Parse(strBranchStatus) & "',"
                End If
                strSQL &= "CB_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', CB_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                        & "WHERE CB_BRANCH_CODE='" & Common.Parse(strBranchCode) & "'"
                If objDb.Execute(strSQL) Then
                    ModBranch = WheelMsgNum.Save
                Else
                    ModBranch = WheelMsgNum.NotSave
                End If

            Else
                ModBranch = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function SearchAssetGroupInfo(ByRef strGroupCode As String, ByRef strGroupDesc As String, ByRef chkAsset As Boolean, ByRef chkSub As Boolean, ByRef chkActive As Boolean, ByRef chkInactive As Boolean)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strCodeType As String
            Dim strStatus As String

            'Get branch code & branch description
            strSQL = "SELECT AG_GROUP, AG_GROUP_DESC, AG_GROUP_TYPE, AG_STATUS " _
                & "FROM asset_group WHERE AG_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strGroupCode <> "" Then
                strSQL &= "AND AG_GROUP LIKE '%" & Common.Parse(strGroupCode) & "%' "
            End If
            If strGroupDesc <> "" Then
                strSQL &= "AND AG_GROUP_DESC LIKE '%" & Common.Parse(strGroupDesc) & "%' "
            End If
            If chkAsset = True Or chkSub = True Then
                If chkSub = True And chkAsset = True Then
                    strSQL &= "AND (AG_GROUP_TYPE ='A' OR AG_GROUP_TYPE = 'S') "
                ElseIf chkAsset = True And chkSub = False Then
                    strSQL &= "AND AG_GROUP_TYPE ='A' "
                ElseIf chkSub = True And chkAsset = False Then
                    strSQL &= "AND AG_GROUP_TYPE ='S' "
                End If

            End If

            If chkActive = True Or chkInactive = True Then
                If chkActive = True And chkInactive = True Then
                    strSQL &= "AND (AG_STATUS ='A' OR AG_STATUS = 'I') "
                ElseIf chkActive = True And chkInactive = False Then
                    strSQL &= "AND AG_STATUS ='A' "
                ElseIf chkInactive = True And chkActive = False Then
                    strSQL &= "AND AG_STATUS ='I' "
                End If

            End If


            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strGroupCode = ds1.Tables(0).Rows(0).Item("AG_GROUP")
                strGroupDesc = ds1.Tables(0).Rows(0).Item("AG_GROUP_DESC")
                strCodeType = ds1.Tables(0).Rows(0).Item("AG_GROUP_TYPE")
                strStatus = ds1.Tables(0).Rows(0).Item("AG_STATUS")
            Else
                strGroupCode = "Group/Sub Group Code"
                strGroupDesc = "Group/Sub Group Description"
                strCodeType = "Code Type"
                strStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function PopulateAssetGroup() As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet

            strSql = "SELECT AG_GROUP, AG_GROUP_DESC, AG_GROUP_TYPE, AG_STATUS " _
                & "FROM asset_group WHERE AG_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' "

            dsGroup = objDb.FillDs(strSql)
            PopulateAssetGroup = dsGroup

        End Function

        Public Function DeleteAssetGroup(ByVal dtAGroup As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL, strPRExist, strPOExist, strInvExist As String
            Dim drAG As DataRow

            For Each drAG In dtAGroup.Rows
                strInvExist = "SELECT * FROM invoice_mstr " _
                         & "INNER JOIN invoice_details " _
                         & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                         & "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                         & "AND id_asset_group = '" & Common.Parse(drAG("AGCode")) & "' " _
                         & "OR id_asset_sub_group = '" & Common.Parse(drAG("AGCode")) & "' "
                strPRExist = "SELECT '*' " _
                        & "FROM PR_DETAILS " _
                        & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
                        & "WHERE PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND PRD_ASSET_GROUP = '" & Common.Parse(drAG("AGCode")) & "' AND PRM_PR_STATUS <> '9' "
                strPOExist = "SELECT '*' " _
                        & "FROM PO_DETAILS " _
                        & "LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO " _
                        & "WHERE POD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND POD_ASSET_GROUP = '" & Common.Parse(drAG("AGCode")) & "' AND POM_PO_STATUS <> '12' "
                If objDb.Exist(strInvExist) > 0 Or objDb.Exist(strPRExist) > 0 Or objDb.Exist(strPOExist) > 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM asset_group WHERE AG_GROUP='" & Common.Parse(drAG("AGCode")) & "'"

                If objDb.Exist(strSQL) <> 0 Then
                    strSQL = "DELETE FROM asset_group WHERE AG_GROUP='" & Common.Parse(drAG("AGCode")) & "'"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function GetAssetGroupInfo(ByRef strAGroupCode As String, ByRef strAGroupDesc As String, ByRef strCodeType As String, ByRef strAGroupStatus As String) As DataSet
            Dim strSQL As String
            Dim ds1 As DataSet

            'Get asset group / sub group code & description
            strSQL = "SELECT AG_GROUP, AG_GROUP_DESC, AG_GROUP_TYPE, AG_STATUS " _
                & "FROM asset_group WHERE AG_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strAGroupCode <> "" Then
                strSQL &= "AND AG_GROUP LIKE '%" & Common.Parse(strAGroupCode) & "%' "
            End If
            If strAGroupDesc <> "" Then
                strSQL &= "AND AG_GROUP_DESC LIKE '%" & Common.Parse(strAGroupDesc) & "%' "
            End If
            If strCodeType <> "" Then
                strSQL &= "AND AG_GROUP_TYPE LIKE '%" & Common.Parse(strCodeType) & "%' "
            End If
            If strAGroupStatus <> "" Then
                strSQL &= "AND AG_STATUS LIKE '%" & Common.Parse(strAGroupStatus) & "%' "
            End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strAGroupCode = ds1.Tables(0).Rows(0).Item("AG_GROUP")
                strAGroupDesc = ds1.Tables(0).Rows(0).Item("AG_GROUP_DESC")
                strCodeType = ds1.Tables(0).Rows(0).Item("AG_GROUP_TYPE")
                strAGroupStatus = ds1.Tables(0).Rows(0).Item("AG_STATUS")
            Else
                strAGroupCode = "Group/Sub Group Code"
                strAGroupDesc = "Group/Sub Group Description"
                strCodeType = "Code Type"
                strAGroupStatus = "Status"
            End If

            Return ds1
        End Function

        Public Function AddAssetGroup(ByVal strAGroupCode As String, ByVal strAGroupDesc As String, ByVal strCodeType As String, ByRef strAGStatus As String) As String
            Dim strSQL As String
            strSQL = "SELECT * FROM asset_group WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "'"

            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "INSERT INTO asset_group (AG_COY_ID,AG_GROUP,AG_GROUP_DESC,AG_GROUP_TYPE,AG_STATUS,AG_ENT_BY,AG_ENT_DATETIME,AG_MOD_BY,AG_MOD_DATETIME) "
                strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                If Common.Parse(strAGroupCode) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strAGroupCode) & "',"
                End If
                If Common.Parse(strAGroupDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strAGroupDesc) & "',"
                End If
                If Common.Parse(strCodeType) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strCodeType) & "',"
                End If
                If Common.Parse(strAGStatus) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strAGStatus) & "',"
                End If

                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    AddAssetGroup = WheelMsgNum.Save
                Else
                    AddAssetGroup = WheelMsgNum.NotSave
                End If

            Else
                AddAssetGroup = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function ModAssetGroup(ByVal strAGroupCode As String, ByVal strAGroupDesc As String, ByVal strCodeType As String, ByRef strAGStatus As String) As String
            Dim strSQL As String
            Dim strOStatus, strOGrpTyp As String
            Dim ds As DataSet

            strSQL = "SELECT AG_STATUS, AG_GROUP_TYPE FROM ASSET_GROUP " _
                     & "WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                     & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "'"
            ds = objDb.FillDs(strSQL)
            strOStatus = ds.Tables(0).Rows(0).Item("AG_STATUS")
            strOGrpTyp = ds.Tables(0).Rows(0).Item("AG_GROUP_TYPE")

            If strOStatus <> strAGStatus Or strOGrpTyp <> strCodeType Then
                strSQL = "SELECT * FROM asset_group " _
                         & "WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                         & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "' " _
                         & "AND AG_GROUP_DESC='" & Common.Parse(strAGroupDesc) & "' " _
                         & "AND AG_STATUS = '" & Common.Parse(strAGStatus) & "' " _
                         & "AND AG_GROUP_TYPE = '" & Common.Parse(strCodeType) & "'"
            Else
                strSQL = "SELECT * FROM asset_group " _
                         & "WHERE AG_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                         & "AND AG_GROUP='" & Common.Parse(strAGroupCode) & "' " _
                         & "AND AG_GROUP_DESC='" & Common.Parse(strAGroupDesc) & "'"
            End If

            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "UPDATE asset_group SET "
                If Common.Parse(strAGroupCode) = "" Then
                    strSQL &= "AG_GROUP=NULL, "
                Else
                    strSQL &= "AG_GROUP='" & Common.Parse(strAGroupCode) & "',"
                End If
                If Common.Parse(strAGroupDesc) = "" Then
                    strSQL &= "AG_GROUP_DESC=NULL, "
                Else
                    strSQL &= "AG_GROUP_DESC='" & Common.Parse(strAGroupDesc) & "',"
                End If
                If Common.Parse(strCodeType) = "" Then
                    strSQL &= "AG_GROUP_TYPE=NULL, "
                Else
                    strSQL &= "AG_GROUP_TYPE='" & Common.Parse(strCodeType) & "',"
                End If
                If Common.Parse(strAGStatus) = "" Then
                    strSQL &= "AG_STATUS=NULL, "
                Else
                    strSQL &= "AG_STATUS='" & Common.Parse(strAGStatus) & "',"
                End If
                strSQL &= "AG_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', AG_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                        & "WHERE AG_GROUP='" & Common.Parse(strAGroupCode) & "'"
                If objDb.Execute(strSQL) Then
                    ModAssetGroup = WheelMsgNum.Save
                Else
                    ModAssetGroup = WheelMsgNum.NotSave
                End If

            Else
                ModAssetGroup = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function GetCostCentreInfo(ByRef CostCentreCode As String, ByRef CostCentreDesc As String, ByRef CostCentreStatus As String) As DataSet ', ByRef CostCentreIndicator As Integer)
            Dim strSQL As String
            Dim ds1 As DataSet

            'Get cost centre code & cost centre description
            strSQL = "SELECT Upper(CC_CC_CODE) AS CCCode, CC_CC_DESC AS CCDesc, CC_STATUS AS CCStatus " _
                   & "FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If CostCentreCode <> "" Then
                strSQL &= "AND CC_CC_CODE LIKE '%" & Common.Parse(CostCentreCode) & "%' "
            End If
            If CostCentreDesc <> "" Then
                strSQL &= "AND CC_CC_DESC LIKE '%" & Common.Parse(CostCentreDesc) & "%' "
            End If
            If CostCentreStatus <> "" Then
                strSQL &= "AND CC_STATUS LIKE '%" & Common.Parse(CostCentreStatus) & "%' "
            End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                CostCentreCode = ds1.Tables(0).Rows(0).Item("CCCode")
                CostCentreDesc = ds1.Tables(0).Rows(0).Item("CCDesc")
                CostCentreStatus = ds1.Tables(0).Rows(0).Item("CCStatus")
            Else
                CostCentreCode = "Cost Centre Code"
                CostCentreDesc = "Cost Centre Description"
                CostCentreStatus = "Status"
            End If

            Return ds1
        End Function

        Public Function GetCostCentreInfoSearch(ByRef CostCentreCode As String, ByRef CostCentreDesc As String, ByRef CostCentreActive As Boolean, ByRef CostCentreInactive As Boolean, Optional ByRef CompID As String = "") ', ByRef CostCentreIndicator As Integer)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim CostCentreStatus As String
            '20110628-Jules COd.
            'Get indicator

            'strSQL = "SELECT * " _
            '        & "FROM cost_centre " _
            '        & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '        & "AND(CC_CC_CODE IS NOT NULL AND CC_CC_DESC<>'')"

            'ds = objDb.FillDs(strSQL)

            'If ds.Tables(0).Rows.Count > 0 Then 'If sub location found
            '    CostCentreIndicator = 2
            'Else
            '    CostCentreIndicator = 1
            'End If

            'Get cost centre code & cost centre description
            'strSQL = "SELECT CC_CC_CODE, CC_CC_DESC, CC_STATUS " _
            '        & "FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            strSQL = "SELECT CC_CC_CODE, CC_CC_DESC, CC_STATUS, upper(CC_COY_ID) as 'cc_coy_id' " _
                           & "FROM COST_CENTRE " 'Modified for IPP GST Stage 2A - CH (30/1/2015)


            If CompID <> "--Select--" Then
                strSQL &= "WHERE CC_COY_ID = '" & Common.Parse(CompID) & "' " 'Modified for IPP GST Stage 2A - CH (30/1/2015)
            End If
            If CostCentreCode <> "" Then
                If strSQL.Contains("WHERE") Then
                    strSQL &= "AND CC_CC_CODE LIKE '%" & Common.Parse(CostCentreCode) & "%' "
                Else
                    strSQL &= "WHERE CC_CC_CODE LIKE '%" & Common.Parse(CostCentreCode) & "%' "
                End If
            End If
            If CostCentreDesc <> "" Then
                If strSQL.Contains("WHERE") Then
                    strSQL &= "AND CC_CC_DESC LIKE '%" & Common.Parse(CostCentreDesc) & "%' "
                Else
                    strSQL &= "WHERE CC_CC_DESC LIKE '%" & Common.Parse(CostCentreDesc) & "%' "
                End If
            End If
            If CostCentreActive = True Or CostCentreInactive = True Then
                If CostCentreActive = True And CostCentreInactive = True Then
                    If strSQL.Contains("WHERE") Then
                        strSQL &= "AND (CC_STATUS ='A' or CC_STATUS = 'I') "
                    Else
                        strSQL &= "WHERE (CC_STATUS ='A' or CC_STATUS = 'I') "
                    End If
                ElseIf CostCentreActive = True And CostCentreInactive = False Then
                    If strSQL.Contains("WHERE") Then
                        strSQL &= "AND CC_STATUS ='A' "
                    Else
                        strSQL &= "WHERE CC_STATUS ='A' "
                    End If
                ElseIf CostCentreInactive = True And CostCentreActive = False Then
                    If strSQL.Contains("WHERE") Then
                        strSQL &= "AND CC_STATUS ='I' "
                    Else
                        strSQL &= "WHERE CC_STATUS ='I' "
                    End If
                End If

            End If

            'Zulham 16072018 - PAMB
            If strSQL.Contains("WHERE") Then
                strSQL &= " AND CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
            Else
                strSQL &= " WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
            End If


            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                CostCentreCode = ds1.Tables(0).Rows(0).Item("CC_CC_CODE")
                CostCentreDesc = ds1.Tables(0).Rows(0).Item("CC_CC_DESC")
                CostCentreStatus = ds1.Tables(0).Rows(0).Item("CC_STATUS")
                'CompID = ds1.Tables(0).Rows(0).Item("CC_COY_ID").ToString.ToUpper
            Else
                CostCentreCode = "Cost Centre Code"
                CostCentreDesc = "Cost Centre Description"
                CostCentreStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function PopulateCostCentre(Optional ByVal companyID As String = "") As DataSet
            Dim strSql As String
            Dim dsCodeCentre As DataSet

            'strSql = "SELECT CC_CC_CODE, CC_CC_DESC, CC_STATUS, upper(CC_COY_ID) as 'CC_COY_ID' " _
            '        & "FROM cost_centre WHERE CC_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' "

            'Zulham 14052018 - PAMB
            'added compnyID as a condition
            If companyID = "" Then
                strSql = "SELECT CC_CC_CODE, CC_CC_DESC, CC_STATUS, upper(CC_COY_ID) as 'CC_COY_ID' " _
                            & "FROM COST_CENTRE  WHERE CC_COY_ID in ('hlb','hlisb')   "
            Else
                strSql = "SELECT CC_CC_CODE, CC_CC_DESC, CC_STATUS, upper(CC_COY_ID) as 'CC_COY_ID' " _
                        & "FROM cost_centre WHERE CC_COY_ID='" & companyID & "' "
            End If


            dsCodeCentre = objDb.FillDs(strSql)
            PopulateCostCentre = dsCodeCentre

        End Function

        Public Function AddCostCentre(ByVal strCostCentre As String, ByVal strCostCentreDesc As String, ByVal strCostCentreStatus As String, Optional ByVal strComp As String = "") As String
            Dim strSQL As String
            'If strCostCentreDesc = "" Then
            '    strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '            & "AND CC_CC_CODE='" & Common.Parse(strCostCentre) & "' AND (CC_CC_DESC='' OR CC_CC_DESC IS NULL)"

            'Else
            'strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '        & "AND CC_CC_CODE='" & Common.Parse(strCostCentre) & "'"

            strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(strComp) & "' " _
                   & "AND CC_CC_CODE='" & Common.Parse(strCostCentre) & "'"

            'End If
            If objDb.Exist(strSQL) = 0 Then ' record not exists
                'strSQL = "INSERT INTO cost_centre (CC_COY_ID,CC_CC_CODE,CC_CC_DESC,CC_STATUS,CC_ENT_BY,CC_ENT_DATETIME,CC_MOD_BY,CC_MOD_DATETIME) "
                'strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"

                strSQL = "INSERT INTO cost_centre (CC_COY_ID,CC_CC_CODE,CC_CC_DESC,CC_STATUS,CC_ENT_BY,CC_ENT_DATETIME,CC_MOD_BY,CC_MOD_DATETIME) "
                strSQL &= "VALUES ('" & Common.Parse(strComp) & "',"
                If Common.Parse(strCostCentre) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strCostCentre) & "',"
                End If
                If Common.Parse(strCostCentreDesc) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strCostCentreDesc) & "',"
                End If
                If Common.Parse(strCostCentreStatus) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strCostCentreStatus) & "',"
                End If
                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    AddCostCentre = WheelMsgNum.Save
                Else
                    AddCostCentre = WheelMsgNum.NotSave
                End If

            Else
                AddCostCentre = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function DeleteCostCentre(ByVal dtCostCentre As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim drCC As DataRow

            'strSQL = "SELECT * FROM invoice_mstr " _
            '        & "INNER JOIN invoice_details " _
            '        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
            '        & "LEFT JOIN invoice_details_alloc " _
            '        & "ON im_invoice_index = ida_invoice_index " _
            '        & "LEFT JOIN cost_alloc_detail ON cad_cc_code = id_cost_center " _
            '        & "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
            '        & "AND id_cost_center = ida_cost_center " _
            '        & "AND ida_cost_center = '" & Common.Parse(drCC("CCCode")) & "' "

            For Each drCC In dtCostCentre.Rows
                'check for transactions

                'strSQL = "SELECT * FROM invoice_mstr " & _
                '         "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " & _
                '         "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND id_cost_center = '" & Common.Parse(drCC("CCCode")) & "' "

                strSQL = "SELECT * FROM invoice_mstr " &
                         "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " &
                         "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND id_cost_center = '" & Common.Parse(drCC("CCCode")) & "' "

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                'strSQL = "SELECT * FROM invoice_mstr " & _
                '        "LEFT JOIN invoice_details_alloc ON im_invoice_index = ida_invoice_index " & _
                '        "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND ida_cost_center = '" & Common.Parse(drCC("CCCode")) & "' "

                strSQL = "SELECT * FROM invoice_mstr " &
                        "LEFT JOIN invoice_details_alloc ON im_invoice_index = ida_invoice_index " &
                        "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'  AND ida_cost_center = '" & Common.Parse(drCC("CCCode")) & "' "

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If


                strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_cc_code ='" & Common.Parse(drCC("CCCode")) & "'"

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM ipp_usrgrp_cc WHERE iuc_cc_code = '" & Common.Parse(drCC("CCCode")) & "' "

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "DELETE FROM cost_centre WHERE CC_CC_CODE='" & Common.Parse(drCC("CCCode")) & "'"
                Common.Insert2Ary(strAry, strSQL)


            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function ModCostCentre(ByVal strCCCode As String, ByVal strCCDesc As String, ByVal strCCStatus As String, Optional ByVal strComp As String = "") As String
            Dim strSQL, strSQLcheck As String

            'strSQLcheck = "SELECT * FROM invoice_mstr " & _
            '             "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " & _
            '             "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and im_invoice_status = 10 AND id_cost_center = '" & Common.Parse(strCCCode) & "' "

            strSQLcheck = "SELECT * FROM invoice_mstr " &
                                  "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " &
                                  "WHERE im_b_coy_id = '" & Common.Parse(strComp) & "' and im_invoice_status = 10 AND id_cost_center = '" & Common.Parse(strCCCode) & "' "

            If objDb.Exist(strSQLcheck) <> 0 Then
                Return -99
            End If

            'strSQLcheck = "SELECT * FROM invoice_mstr " & _
            '        "LEFT JOIN invoice_details_alloc ON im_invoice_index = ida_invoice_index " & _
            '        "WHERE im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and im_invoice_status = 10 AND ida_cost_center = '" & Common.Parse(strCCCode) & "' "

            strSQLcheck = "SELECT * FROM invoice_mstr " &
                             "LEFT JOIN invoice_details_alloc ON im_invoice_index = ida_invoice_index " &
                             "WHERE im_b_coy_id = '" & Common.Parse(strComp) & "' and im_invoice_status = 10 AND ida_cost_center = '" & Common.Parse(strCCCode) & "' "


            If objDb.Exist(strSQLcheck) <> 0 Then
                Return -99
            End If

            If objDb.Exist(strSQLcheck) = 0 Then

                'If strCCDesc = "" Then
                '    strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '            & "AND CC_CC_CODE='" & Common.Parse(strCCCode) & "' AND (CC_CC_DESC='' OR CC_CC_DESC IS NULL) " _
                '            & "AND CC_STATUS='" & Common.Parse(strCCStatus) & "' "

                'Else
                '    strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '            & "AND CC_CC_CODE='" & Common.Parse(strCCCode) & "' AND CC_CC_DESC='" & Common.Parse(strCCDesc) & "' " _
                '            & "AND CC_STATUS='" & Common.Parse(strCCStatus) & "' "

                'End If

                If strCCDesc = "" Then
                    strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(strComp) & "' " _
                            & "AND CC_CC_CODE='" & Common.Parse(strCCCode) & "' AND (CC_CC_DESC='' OR CC_CC_DESC IS NULL) " _
                            & "AND CC_STATUS='" & Common.Parse(strCCStatus) & "' "

                Else
                    strSQL = "SELECT * FROM cost_centre WHERE CC_COY_ID='" & Common.Parse(strComp) & "' " _
                            & "AND CC_CC_CODE='" & Common.Parse(strCCCode) & "' AND CC_CC_DESC='" & Common.Parse(strCCDesc) & "' " _
                            & "AND CC_STATUS='" & Common.Parse(strCCStatus) & "' "

                End If


                If objDb.Exist(strSQL) = 0 Then ' record not exists
                    strSQL = "UPDATE cost_centre SET "
                    If Common.Parse(strCCCode) = "" Then
                        strSQL &= "CC_CC_CODE=NULL, "
                    Else
                        strSQL &= "CC_CC_CODE='" & Common.Parse(strCCCode) & "',"
                    End If
                    If Common.Parse(strComp) = "" Then
                        strSQL &= "CC_COY_ID=NULL, "
                    Else
                        strSQL &= "CC_COY_ID='" & Common.Parse(strComp) & "',"
                    End If
                    If Common.Parse(strCCDesc) = "" Then
                        strSQL &= "CC_CC_DESC=NULL, "
                    Else
                        strSQL &= "CC_CC_DESC='" & Common.Parse(strCCDesc) & "',"
                    End If
                    If Common.Parse(strCCStatus) = "" Then
                        strSQL &= "CC_STATUS=NULL, "
                    Else
                        strSQL &= "CC_STATUS='" & Common.Parse(strCCStatus) & "',"
                    End If
                    strSQL &= "CC_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', CC_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                            & "WHERE CC_CC_CODE='" & Common.Parse(strCCCode) & "' AND CC_COY_ID = '" & Common.Parse(strComp) & "'"
                    If objDb.Execute(strSQL) Then
                        ModCostCentre = WheelMsgNum.Save
                    Else
                        ModCostCentre = WheelMsgNum.NotSave
                    End If

                Else
                    ModCostCentre = WheelMsgNum.Duplicate
                End If

            End If
        End Function

        Public Function GetBankInfo(ByRef strBankCode As String, ByRef strBankName As String, ByRef strBankStatus As String)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet

            'Get branch code & branch description
            strSQL = "SELECT BC_BANK_CODE, BC_BANK_NAME, BC_USAGE, BC_STATUS " _
                & "FROM bank_code WHERE BC_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            If strBankCode <> "" Then
                strSQL &= "AND BC_BANK_CODE LIKE '%" & Common.Parse(strBankCode) & "%' "
            End If
            If strBankName <> "" Then
                strSQL &= "AND BC_BANK_NAME LIKE '%" & Common.Parse(strBankName) & "%' "
            End If
            If strBankStatus <> "" Then
                strSQL &= "AND BC_STATUS LIKE '%" & Common.Parse(strBankStatus) & "%' "
            End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strBankCode = ds1.Tables(0).Rows(0).Item("BC_BANK_CODE")
                strBankName = ds1.Tables(0).Rows(0).Item("BC_BANK_NAME")
                strBankStatus = ds1.Tables(0).Rows(0).Item("BC_STATUS")
            Else
                strBankCode = "Bank Code"
                strBankName = "Name"
                strBankStatus = "Status"
            End If

            ds = Nothing
            ds1 = Nothing
        End Function

        Public Function PopulateBankCode() As DataSet
            Dim strSql As String
            Dim dsBankCode As DataSet

            'zULHAM 10072018 - PAMB
            'Chee Hong - 17/12/2014 (IPP GST Stage 2A)
            strSql = "SELECT BC_BANK_CODE,BC_BANK_NAME,BC_USAGE,BC_STATUS " &
                    "FROM BANK_CODE WHERE BC_BANK_CODE <> '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND BC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            dsBankCode = objDb.FillDs(strSql)
            PopulateBankCode = dsBankCode

        End Function

        Public Function SearchBankCodeInfo(ByRef strBankCode As String, ByRef strBankName As String, ByRef chkActive As Boolean, ByRef chkInactive As Boolean)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strUsage As String
            Dim strStatus As String

            'Zulham 10072018 - PAMB
            'Get branch code & branch description
            'Chee Hong - 17/12/2014 (IPP GST Stage 2A)
            strSQL = "SELECT BC_BANK_CODE, BC_BANK_NAME, BC_USAGE, BC_STATUS " &
                    "FROM BANK_CODE WHERE BC_BANK_CODE <> '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND BC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strBankCode <> "" Then
                strSQL &= "AND BC_BANK_CODE LIKE '%" & Common.Parse(strBankCode) & "%' "
            End If
            If strBankName <> "" Then
                strSQL &= "AND BC_BANK_NAME LIKE '%" & Common.Parse(strBankName) & "%' "
            End If

            If chkActive = True Or chkInactive = True Then
                If chkActive = True And chkInactive = True Then
                    strSQL &= "AND (BC_STATUS ='A' or BC_STATUS ='I') "
                ElseIf chkActive = True And chkInactive = False Then
                    strSQL &= "AND BC_STATUS ='A' "
                ElseIf chkInactive = True And chkActive = False Then
                    strSQL &= "AND BC_STATUS ='I' "
                End If

            End If


            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strBankCode = ds1.Tables(0).Rows(0).Item("BC_BANK_CODE")
                strBankName = ds1.Tables(0).Rows(0).Item("BC_BANK_NAME")
                strUsage = ds1.Tables(0).Rows(0).Item("BC_USAGE")
                strStatus = ds1.Tables(0).Rows(0).Item("BC_STATUS")
            Else
                strBankCode = "Bank Code"
                strBankName = "Name"
                strUsage = "Usage"
                strStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function DeleteBankCode(ByVal dtBankCode As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim drBC As DataRow

            For Each drBC In dtBankCode.Rows
                strSQL = "SELECT * FROM ipp_company WHERE ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                        & "AND ic_bank_code = '" & Common.Parse(drBC("BCCode")) & "' "
                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM bank_code WHERE BC_BANK_CODE='" & Common.Parse(drBC("BCCode")) & "'"
                If objDb.Exist(strSQL) <> 0 Then
                    strSQL = "DELETE FROM bank_code WHERE BC_BANK_CODE='" & Common.Parse(drBC("BCCode")) & "'"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function AddBankCode(ByVal strBankCode As String, ByVal strBankName As String, ByVal strUsage As String, ByRef strStatus As String) As String
            Dim strSQL As String
            strSQL = "SELECT * FROM bank_code WHERE BC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                    & "AND BC_BANK_CODE='" & Common.Parse(strBankCode) & "'"

            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "INSERT INTO bank_code (BC_COY_ID,BC_BANK_CODE,BC_BANK_NAME,BC_USAGE,BC_STATUS,BC_ENT_BY,BC_ENT_DATETIME,BC_MOD_BY,BC_MOD_DATETIME) "
                strSQL &= "VALUES ('" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "',"
                If Common.Parse(strBankCode) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBankCode) & "',"
                End If
                If Common.Parse(strBankName) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBankName) & "',"
                End If
                If Common.Parse(strUsage) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strUsage) & "',"
                End If
                If Common.Parse(strStatus) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strStatus) & "',"
                End If

                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ",'" & Common.Parse(HttpContext.Current.Session("UserID")) & "'," & Common.ConvertDate(Now()) & ")"
                If objDb.Execute(strSQL) Then
                    AddBankCode = WheelMsgNum.Save
                Else
                    AddBankCode = WheelMsgNum.NotSave
                End If

            Else
                AddBankCode = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function ModBankCode(ByVal strBankCode As String, ByVal strBankName As String, ByVal strUsage As String, ByRef strStatus As String) As String

            Dim strSQL As String
            Dim strOStatus As String
            strSQL = "SELECT BC_STATUS FROM BANK_CODE " _
                     & "WHERE BC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                     & "AND BC_BANK_CODE = '" & Common.Parse(strBankCode) & "' " _
                     & "AND BC_BANK_NAME = '" & Common.Parse(strBankName) & "' " _
                     & "AND BC_USAGE = '" & Common.Parse(strUsage) & "'"
            strOStatus = objDb.GetVal(strSQL)

            If strOStatus <> strStatus Then
                strSQL = "SELECT * FROM BANK_CODE WHERE BC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                         & "AND BC_BANK_CODE = '" & Common.Parse(strBankCode) & "' AND BC_BANK_NAME = '" & Common.Parse(strBankName) & "' " _
                         & "AND BC_USAGE = '" & Common.Parse(strUsage) & "' AND BC_STATUS = '" & Common.Parse(strStatus) & "'"
            Else
                strSQL = "SELECT * FROM BANK_CODE WHERE BC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                         & "AND BC_BANK_CODE = '" & Common.Parse(strBankCode) & "' AND BC_BANK_NAME = '" & Common.Parse(strBankName) & "' " _
                         & "AND BC_USAGE = '" & Common.Parse(strUsage) & "'"
            End If


            If objDb.Exist(strSQL) = 0 Then ' record not exists
                strSQL = "UPDATE bank_code SET "
                If Common.Parse(strBankCode) = "" Then
                    strSQL &= "BC_BANK_CODE=NULL, "
                Else
                    strSQL &= "BC_BANK_CODE='" & Common.Parse(strBankCode) & "',"
                End If
                If Common.Parse(strBankName) = "" Then
                    strSQL &= "BC_BANK_NAME=NULL, "
                Else
                    strSQL &= "BC_BANK_NAME='" & Common.Parse(strBankName) & "',"
                End If
                If Common.Parse(strUsage) = "" Then
                    strSQL &= "BC_USAGE=NULL, "
                Else
                    strSQL &= "BC_USAGE='" & Common.Parse(strUsage) & "',"
                End If
                If Common.Parse(strStatus) = "" Then
                    strSQL &= "BC_STATUS=NULL, "
                Else
                    strSQL &= "BC_STATUS='" & Common.Parse(strStatus) & "',"
                End If
                strSQL &= "BC_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "', BC_MOD_DATETIME=" & Common.ConvertDate(Now()) & " " _
                        & "WHERE BC_BANK_CODE='" & Common.Parse(strBankCode) & "'"
                If objDb.Execute(strSQL) Then
                    ModBankCode = WheelMsgNum.Save
                Else
                    ModBankCode = WheelMsgNum.NotSave
                End If

            Else
                ModBankCode = WheelMsgNum.Duplicate
            End If


        End Function

        Public Function GetIPPCompanyInfo(ByRef strCoyCode As String, ByRef strCoyName As String, ByRef strCoyType As String, ByRef strCoyStatus As String)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strPaymentMethod As String
            Dim strConIBSGLCode As String
            Dim strNonConIBSGLCode As String
            Dim strBusinessRegNo As String

            'Get asset group / sub group code & description
            strSQL = "SELECT IC_INDEX, IC_COY_ID, IC_OTHER_B_COY_CODE, IC_COY_TYPE, IC_COY_NAME, IC_PAYMENT_METHOD, " _
            & "IC_CON_IBS_CODE, IC_NON_CON_IBS_CODE, IC_STATUS,IC_BUSINESS_REG_NO " _
                & "FROM IPP_COMPANY WHERE IC_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                'Zulham 19072018 - PAMB
                strCoyType = ds1.Tables(0).Rows(0).Item("IC_COY_TYPE")
                strCoyCode = ds1.Tables(0).Rows(0).Item("IC_COY_ID")
                strCoyName = ds1.Tables(0).Rows(0).Item("IC_COY_NAME")
                strPaymentMethod = Common.parseNull(ds1.Tables(0).Rows(0).Item("IC_PAYMENT_METHOD"))
                strConIBSGLCode = Common.parseNull(ds1.Tables(0).Rows(0).Item("IC_CON_IBS_CODE"))
                strNonConIBSGLCode = Common.parseNull(ds1.Tables(0).Rows(0).Item("IC_NON_CON_IBS_CODE"))
                strCoyStatus = ds1.Tables(0).Rows(0).Item("IC_STATUS")
                strBusinessRegNo = ds1.Tables(0).Rows(0).Item("IC_BUSINESS_REG_NO")
            Else
                strCoyType = "Company Type"
                strCoyCode = "Company Code"
                strCoyName = "Company Name"
                strPaymentMethod = "Payment Mode"
                strConIBSGLCode = "Conventional IBS GL Code"
                strNonConIBSGLCode = "Non Conventional IBS GL Code"
                strCoyStatus = "Status"
                strBusinessRegNo = "Bussiness Registration No."
            End If

            ds = Nothing
            ds1 = Nothing
        End Function

        Public Function SearchIPPCoyInfo(ByRef strCoyCode As String, ByRef strCoyName As String, ByRef chkVendor As Boolean, ByRef chkOtherBCoy As Boolean, ByVal chkEmployee As Boolean, ByRef chkActive As Boolean, ByRef chkInactive As Boolean, Optional ByRef strBusinessRegNo As String = "")
            Dim strSQL As String
            Dim strSQL2 As String = ""
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strCoyType As String
            Dim strPaymentMethod As String
            Dim strConIBSGLCode As String
            Dim strNonConIBSGLCode As String
            Dim strCoyStatus As String
            Dim strCompType As String

            'strSQL = "SELECT IC_INDEX, IC_COY_ID, IC_OTHER_B_COY_CODE, IC_COY_TYPE, IC_COY_NAME, IC_PAYMENT_METHOD, " _
            '& "IC_CON_IBS_CODE, IC_NON_CON_IBS_CODE, IC_STATUS,IC_BUSINESS_REG_NO, " _
            '& "(SELECT bc_bank_name FROM bank_code WHERE bc_bank_code = ic_bank_code AND bc_coy_id = 'hlb' AND ic_status = 'A')  AS BC_BANK_NAME,IC_BANK_ACCT " _
            '& "FROM IPP_COMPANY " _
            '& "WHERE IC_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
            'Chee Hong 18/12/2014 (IPP - GST Stage 2A)
            'Zulham 29062015 - HLB_IPP Stage 4(CR)
            'Added ic_bill_gl_code
            strSQL = "SELECT IC_INDEX, IC_COY_ID, IC_OTHER_B_COY_CODE, IC_COY_TYPE, IC_COY_NAME, IC_PAYMENT_METHOD, " &
                    "IC_CON_IBS_CODE, IC_NON_CON_IBS_CODE, IC_STATUS,IC_BUSINESS_REG_NO, " &
                    "IC_TAX_REG_NO, IC_GST_INPUT_TAX_CODE, IC_ADDITIONAL_1, IC_ADDITIONAL_2, IC_ADDITIONAL_3, IC_ADDITIONAL_4, " &
                    "(SELECT bc_bank_name FROM bank_code WHERE bc_bank_code = ic_bank_code AND bc_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' AND ic_status = 'A')  AS BC_BANK_NAME,IC_BANK_ACCT, ic_bill_gl_code " &
                    "FROM IPP_COMPANY WHERE "

            'If strCoyCode <> "" Then
            '    strSQL &= "AND IC_OTHER_B_COY_CODE LIKE '%" & Common.Parse(strCoyCode) & "%' "
            'End If
            'If strCoyName <> "" Then
            '    strSQL &= "AND IC_COY_NAME LIKE '%" & Common.Parse(strCoyName) & "%' "
            'End If
            If strCoyCode <> "" Then
                If strSQL2 = "" Then
                    strSQL2 &= "IC_OTHER_B_COY_CODE LIKE '%" & Common.Parse(strCoyCode) & "%' "
                Else
                    strSQL2 &= "AND IC_OTHER_B_COY_CODE LIKE '%" & Common.Parse(strCoyCode) & "%' "
                End If
            End If
            If strCoyName <> "" Then
                If strSQL2 = "" Then
                    strSQL2 &= "IC_COY_NAME LIKE '%" & Common.Parse(strCoyName) & "%' "
                Else
                    strSQL2 &= "AND IC_COY_NAME LIKE '%" & Common.Parse(strCoyName) & "%' "
                End If
            End If
            If chkVendor = True Or chkOtherBCoy = True Or chkEmployee = True Then
                'Modified by Joon for issue 1598
                If chkVendor = True Then
                    strCompType = "IC_COY_TYPE ='V'"
                End If
                'strSQL &= "AND (IC_COY_TYPE ='V' OR IC_COY_TYPE ='B') "
                If chkOtherBCoy = True Then
                    If strCompType = "" Then
                        strCompType = "IC_COY_TYPE ='B'"
                    Else
                        strCompType &= " OR IC_COY_TYPE ='B'"
                    End If
                End If
                If chkEmployee = True Then
                    If strCompType = "" Then
                        strCompType = "IC_COY_TYPE ='E'"
                    Else
                        strCompType &= " OR IC_COY_TYPE ='E'"
                    End If
                End If
                'If strCompType <> "" Then
                '    strSQL &= "AND (" & strCompType & ") "
                'End If
                If strCompType <> "" Then
                    If strSQL2 = "" Then
                        strSQL2 &= "(" & strCompType & ") "
                    Else
                        strSQL2 &= "AND (" & strCompType & ") "
                    End If
                End If
            End If

            If chkActive = True And chkInactive = False Then
                If strSQL2 = "" Then
                    strSQL2 &= "IC_STATUS ='A' "
                Else
                    strSQL2 &= "AND IC_STATUS ='A' "
                End If
            ElseIf chkActive = False And chkInactive = True Then
                If strSQL2 = "" Then
                    strSQL2 &= "IC_STATUS ='I' "
                Else
                    strSQL2 &= "AND IC_STATUS ='I' "
                End If
                'Zulham 09072018 - PAMB
            ElseIf chkActive = True And chkInactive = True Then
                If strSQL2 = "" Then
                    strSQL2 &= "IC_STATUS in ('A','I') "
                Else
                    strSQL2 &= "AND IC_STATUS in ('A','I') "
                End If
            End If

            'List down only registered companies under buyer's company
            If strSQL2 = "" Then
                strSQL2 &= "IC_COY_ID  = '" & HttpContext.Current.Session("CompanyID") & "' "
            Else
                strSQL2 &= "AND IC_COY_ID  = '" & HttpContext.Current.Session("CompanyID") & "' "
            End If

            If strBusinessRegNo <> "" Then
                If strSQL2 = "" Then
                    strSQL &= "IC_BUSINESS_REG_NO LIKE '%" & Common.Parse(strBusinessRegNo) & "%' "
                Else
                    strSQL &= "AND IC_BUSINESS_REG_NO LIKE '%" & Common.Parse(strBusinessRegNo) & "%' "
                End If
            End If

            strSQL = strSQL & strSQL2

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                'Zulham 19072018 - PAMB
                strCoyType = ds1.Tables(0).Rows(0).Item("IC_COY_TYPE")
                strCoyCode = ds1.Tables(0).Rows(0).Item("IC_COY_ID")
                strCoyName = ds1.Tables(0).Rows(0).Item("IC_COY_NAME")
                strPaymentMethod = Common.parseNull(ds1.Tables(0).Rows(0).Item("IC_PAYMENT_METHOD"))
                strConIBSGLCode = Common.parseNull(ds1.Tables(0).Rows(0).Item("IC_CON_IBS_CODE"))
                strNonConIBSGLCode = Common.parseNull(ds1.Tables(0).Rows(0).Item("IC_NON_CON_IBS_CODE"))
                strCoyStatus = ds1.Tables(0).Rows(0).Item("IC_STATUS")
                strBusinessRegNo = ds1.Tables(0).Rows(0).Item("IC_BUSINESS_REG_NO")
            Else
                strCoyType = "Company Type"
                strCoyCode = "Company Code"
                strCoyName = "Company Name"
                strPaymentMethod = "Payment Mode"
                strConIBSGLCode = "Conventional IBS GL Code"
                strNonConIBSGLCode = "Non Conventional IBS GL Code"
                strCoyStatus = "Status"
                strBusinessRegNo = "Bussiness Reg. No/Staff ID"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function PopulateIPPCompany(Optional ByVal buyerId As String = "") As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet

            'Zulham 15072018 - PAMB
            If buyerId = "" Then
                'Chee Hong 18/12/2014 (IPP - GST Stage 2A)
                'Zulham 29062015 - HLB_IPP Stage 4(CR)
                'Added IC_BILL_GL_CODE
                strSql = "SELECT IC_INDEX, IC_COY_ID, IC_OTHER_B_COY_CODE, IC_COY_TYPE, IC_COY_NAME, IC_PAYMENT_METHOD, " &
                        "IC_CON_IBS_CODE, IC_NON_CON_IBS_CODE, IC_STATUS,IC_BUSINESS_REG_NO, " &
                        "IC_TAX_REG_NO, IC_GST_INPUT_TAX_CODE, IC_ADDITIONAL_1, IC_ADDITIONAL_2, IC_ADDITIONAL_3, IC_ADDITIONAL_4, " &
                        "(SELECT bc_bank_name FROM bank_code WHERE bc_bank_code = ic_bank_code AND bc_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' AND ic_status = 'A')  AS BC_BANK_NAME,IC_BANK_ACCT, IC_BILL_GL_CODE " &
                        "FROM IPP_COMPANY "
            Else
                strSql = "SELECT IC_INDEX, IC_COY_ID, IC_OTHER_B_COY_CODE, IC_COY_TYPE, IC_COY_NAME, IC_PAYMENT_METHOD, " &
                        "IC_CON_IBS_CODE, IC_NON_CON_IBS_CODE, IC_STATUS,IC_BUSINESS_REG_NO, " &
                        "IC_TAX_REG_NO, IC_GST_INPUT_TAX_CODE, IC_ADDITIONAL_1, IC_ADDITIONAL_2, IC_ADDITIONAL_3, IC_ADDITIONAL_4, " &
                        "(SELECT bc_bank_name FROM bank_code WHERE bc_bank_code = ic_bank_code AND bc_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' AND ic_status = 'A')  AS BC_BANK_NAME,IC_BANK_ACCT, IC_BILL_GL_CODE " &
                        "FROM IPP_COMPANY WHERE IC_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"
            End If


            dsGroup = objDb.FillDs(strSql)
            PopulateIPPCompany = dsGroup

        End Function

        Public Function DeleteIPPCompany(ByVal dtIPPCoy As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim drIPPCoy As DataRow

            For Each drIPPCoy In dtIPPCoy.Rows
                If drIPPCoy("IPPCoyType") = "Vendor" Or drIPPCoy("IPPCoyType") = "Employee" Then
                    'strSQL = "SELECT * FROM invoice_details WHERE id_s_coy_id = '" & Common.Parse(drIPPCoy("IPPCoyIndex")) & "'"
                    strSQL = "SELECT * FROM invoice_mstr WHERE im_s_coy_id = '" & Common.Parse(drIPPCoy("IPPCoyIndex")) & "'"
                Else
                    strSQL = "SELECT * FROM invoice_details WHERE id_pay_for = '" & Common.Parse(drIPPCoy("IPPCoyCode")) & "'"

                End If

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "DELETE FROM ipp_company WHERE IC_INDEX='" & Common.Parse(drIPPCoy("IPPCoyIndex")) & "'"
                Common.Insert2Ary(strAry, strSQL)
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function


        Public Function CheckPending(ByVal strFieldToCheck As String, ByVal strIPP As String) As Boolean
            Dim strSQL As String

            If strIPP = "IPP" Then
                strSQL = "SELECT * FROM invoice_mstr WHERE im_s_coy_name='" & strFieldToCheck & "'" _
                        & "AND im_invoice_status = 10"
            ElseIf strIPP = "CC" Then 'argument: cc_cc_code
                'strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_cc_code='" & strFieldToCheck & "'"
                strSQL = "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "LEFT JOIN invoice_details_alloc " _
                        & "ON im_invoice_index = ida_invoice_index " _
                        & "LEFT JOIN cost_alloc_detail ON cad_cc_code = id_cost_center " _
                        & "WHERE im_invoice_status = 10 AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND id_cost_center = ida_cost_center " _
                        & "AND ida_cost_center = '" & strFieldToCheck & "' "
            ElseIf strIPP = "CB" Then
                strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_branch_code = cb_cc_code " _
                        & "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "LEFT JOIN invoice_details_alloc " _
                        & "ON im_invoice_index = ida_invoice_index " _
                        & "WHERE im_invoice_status = 10 " _
                        & "AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND ida_branch_code = '" & strFieldToCheck & "' "
            ElseIf strIPP = "AG" Then
                strSQL = "SELECT * FROM invoice_mstr " _
                        & "INNER JOIN invoice_details " _
                        & "ON im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id " _
                        & "WHERE im_invoice_status = 10 AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                        & "AND id_asset_group = '" & strFieldToCheck & "' " _
                        & "OR id_asset_sub_group = '" & strFieldToCheck & "' "
            End If

            If objDb.Exist(strSQL) <> 0 Then
                Return False
            End If
            Return 0
        End Function
        Public Function GetApprCostAllocDetail(ByVal strCostAllocCode As String, ByVal invindex As String, ByVal InvLine As Integer, Optional ByVal VenIdx As Integer = 0) As DataSet
            Dim strSQL As String
            Dim ds As DataSet
            Dim createdby As String
            createdby = objDb.Get1ColumnCheckNull("invoice_mstr", "im_created_by", " WHERE im_invoice_index='" & invindex & "'")


            'strSQL = "SELECT CAM_CA_CODE As CA_Code,CAM_CA_DESC As CA_Desc,CAD_CC_CODE As CC_Code,CC_CC_DESC As CC_Desc, " _
            '        & "CAD_BRANCH_CODE As Branch_Code,CDM_DEPT_NAME As Branch_Name,CAD_PERCENT As Percentage, NULL As CA_Amount " _
            '        & "FROM COST_ALLOC_MSTR " _
            '        & "INNER JOIN COST_ALLOC_DETAIL ON CAD_CAM_INDEX = CAM_INDEX " _
            '        & "LEFT JOIN COST_CENTRE ON CAD_CC_CODE = CC_CC_CODE AND CAM_COY_ID = CC_COY_ID " _
            '        & "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = cad_branch_code " _
            '        & "WHERE CAM_CA_CODE = '" & strCostAllocCode & "' " _
            '        & "AND CAM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
            '        & "AND CAM_USER_ID = '" & createdby & "' "
            strSQL = "SELECT invoice_details_alloc.ida_cost_center AS CC_Code, " &
                    "invoice_details_alloc.ida_cost_center_desc AS CC_Desc, " &
                    "invoice_details_alloc.ida_branch_code AS Branch_Code, " &
                    "invoice_details_alloc.ida_branch_name AS Branch_Name, " &
                    "invoice_details_alloc.ida_percent AS Percentage, " &
                    "invoice_details_alloc.ida_amount AS CA_Amount, " &
                    "invoice_details.id_cost_alloc_code as CA_Code, " &
                    "COST_ALLOC_MSTR.cam_ca_desc as CA_Desc  " &
                    "FROM invoice_details_alloc  " &
                    "INNER JOIN invoice_mstr ON ida_invoice_index = im_invoice_index  AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND id_invoice_line = ida_invoice_line and id_s_coy_id = '" & VenIdx & "'" &
                    "INNER JOIN COST_ALLOC_MSTR ON id_cost_alloc_code = cam_ca_code AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'and cam_user_id = '" & createdby & "'" &
                    "WHERE ida_invoice_index = '" & invindex & "' AND ida_invoice_line = '" & InvLine & "'"
            ds = objDb.FillDs(strSQL)

            Return ds
        End Function
        Public Function GetCostAllocDetail(ByVal strCostAllocCode As String) As DataSet
            Dim strSQL As String
            Dim ds As DataSet



            strSQL = "SELECT CAM_CA_CODE As CA_Code,CAM_CA_DESC As CA_Desc,CAD_CC_CODE As CC_Code,CC_CC_DESC As CC_Desc, " _
                    & "CAD_BRANCH_CODE As Branch_Code,CBM_BRANCH_NAME As Branch_Name,CAD_PERCENT As Percentage, NULL As CA_Amount " _
                    & "FROM COST_ALLOC_MSTR " _
                    & "INNER JOIN COST_ALLOC_DETAIL ON CAD_CAM_INDEX = CAM_INDEX " _
                    & "LEFT JOIN COST_CENTRE ON CAD_CC_CODE = CC_CC_CODE AND CAM_COY_ID = CC_COY_ID " _
                    & "INNER JOIN COMPANY_BRANCH_MSTR ON CBM_BRANCH_CODE = cad_branch_code " _
                    & "WHERE CAM_CA_CODE = '" & strCostAllocCode & "' " _
                    & "AND CAM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " ' _
            ' & "AND CAM_USER_ID = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' "
            ds = objDb.FillDs(strSQL)

            Return ds
        End Function
        Public Function GetInvCostAllocDetail(ByVal InvIdx As Integer, ByVal InvLine As Integer, Optional ByVal VenIdx As Integer = 0) As DataSet

            Dim strSql As String
            Dim dsDoc As New DataSet
            Dim createdby As String
            createdby = objDb.Get1ColumnCheckNull("invoice_mstr", "im_created_by", " WHERE im_invoice_index='" & InvIdx & "'")

            strSql = "SELECT invoice_details_alloc.ida_cost_center AS CC_Code, " &
                    "invoice_details_alloc.ida_cost_center_desc AS CC_Desc, " &
                    "invoice_details_alloc.ida_branch_code AS Branch_Code, " &
                    "invoice_details_alloc.ida_branch_name AS Branch_Name, " &
                    "invoice_details_alloc.ida_percent AS Percentage, " &
                    "invoice_details_alloc.ida_amount AS CA_Amount, " &
                    "invoice_details.id_cost_alloc_code as CA_Code, " &
                    "COST_ALLOC_MSTR.cam_ca_desc as CA_Desc  " &
                    "FROM invoice_details_alloc  " &
                    "INNER JOIN invoice_mstr ON ida_invoice_index = im_invoice_index  AND im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "INNER JOIN invoice_details ON im_invoice_no = id_invoice_no AND id_invoice_line = ida_invoice_line and id_s_coy_id = '" & VenIdx & "'" &
                    "INNER JOIN COST_ALLOC_MSTR ON id_cost_alloc_code = cam_ca_code AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and cam_user_id = '" & createdby & "'" &
                    "WHERE ida_invoice_index = '" & InvIdx & "' AND ida_invoice_line = '" & InvLine & "'"

            dsDoc = objDb.FillDs(strSql)
            GetInvCostAllocDetail = dsDoc

        End Function

        Public Function GetIPPDetails(ByVal strIPPDocNo As String, ByVal strCoyId As String, ByVal InvIdx As Integer, Optional ByVal AppType As String = "", Optional ByVal strCreatedBy As String = "") As IPPDetails
            Dim dtIPP As New DataTable
            Dim objIPPDetails As New IPPDetails
            Dim strTempAddr As String
            Dim strsql As String
            Dim dtDiff As Long
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            If strCreatedBy = "" Then
                strCreatedBy = Common.Parse(HttpContext.Current.Session("UserID"))
            End If

            'Jules 2018.07.11 - Allow "\" and "#"
            strIPPDocNo = Replace(Replace(strIPPDocNo, "\", "\\"), "#", "\#")

            'Zulham 10072018 - PAMB
            'included im_status_changed_by = '" & strCreatedBy & "' condition
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            If objDb.Exist("Select INVOICE_MSTR.*,INVOICE_DETAILS.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms " &
            "from INVOICE_MSTR, INVOICE_DETAILS, status_mstr ,ipp_company where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID AND im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
            "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
            "and IM_INVOICE_NO='" & strIPPDocNo & "' AND ID_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and (im_created_by = '" & strCreatedBy & "' or im_status_changed_by = '" & strCreatedBy & "') and im_invoice_index = '" & InvIdx & "'") > 0 Then

                strsql = "Select INVOICE_MSTR.*,INVOICE_DETAILS.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO " &
                            "from INVOICE_MSTR, INVOICE_DETAILS, status_mstr ,ipp_company where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID AND im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                            "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                            "and IM_INVOICE_NO='" & strIPPDocNo & "' AND ID_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and (im_created_by = '" & strCreatedBy & "' or im_status_changed_by = '" & strCreatedBy & "') and im_invoice_index = '" & InvIdx & "'"

            Else
                If AppType = "PSDAcceptRejList" Or AppType = "PSDAcceptRejList,dashboard" Then
                    strsql = "Select INVOICE_MSTR.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO " &
                                               "from INVOICE_MSTR, status_mstr , ipp_company where  im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                                               "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                                               "and IM_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'"

                ElseIf AppType = "PSDReceived" Then
                    strsql = "Select INVOICE_MSTR.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO " &
                                               "from INVOICE_MSTR, status_mstr , ipp_company where  im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                                               "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                                               "and IM_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'"
                ElseIf AppType = "PSDAcceptanceDetails" Then
                    strsql = "Select INVOICE_MSTR.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO " &
                                               "from INVOICE_MSTR, status_mstr , ipp_company where  im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                                               "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                                               "and IM_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'"
                ElseIf AppType = "EnterBC" Then
                    strsql = "Select INVOICE_MSTR.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO " &
                                                                  "from INVOICE_MSTR, status_mstr , ipp_company where  im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                                                                  "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                                                                  "and IM_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'"
                Else

                    strsql = "Select INVOICE_MSTR.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO " &
                                               "from INVOICE_MSTR, status_mstr , ipp_company where  im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                                               "AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                                               "and IM_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and (im_created_by = '" & strCreatedBy & "' or im_status_changed_by = '" & strCreatedBy & "') and im_invoice_index = '" & InvIdx & "'"

                End If
            End If
            '------------------------------------------------------------

            dtIPP = objDb.FillDt(strsql)

            If Not dtIPP Is Nothing Then
                With dtIPP.Rows(0)
                    objIPPDetails.DocType = IIf(IsDBNull(.Item("IM_INVOICE_TYPE")), "", .Item("IM_INVOICE_TYPE"))
                    objIPPDetails.DocNo = IIf(IsDBNull(.Item("IM_INVOICE_NO")), "", .Item("IM_INVOICE_NO"))
                    objIPPDetails.DocDate = IIf(IsDBNull(.Item("IM_DOC_DATE")), "", .Item("IM_DOC_DATE"))
                    objIPPDetails.ManualPONo = IIf(IsDBNull(.Item("IM_IPP_PO")), "", .Item("IM_IPP_PO"))
                    objIPPDetails.Vendor = IIf(IsDBNull(.Item("IM_S_COY_NAME")), "", .Item("IM_S_COY_NAME"))
                    objIPPDetails.GSTRegNo = IIf(IsDBNull(.Item("IC_TAX_REG_NO")), "", .Item("IC_TAX_REG_NO"))

                    strTempAddr = .Item("IM_ADDR_LINE1").ToString.Trim

                    If .Item("IM_ADDR_LINE2").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_ADDR_LINE2").ToString.Trim
                    End If

                    If .Item("IM_ADDR_LINE3").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_ADDR_LINE3").ToString.Trim
                    End If

                    If .Item("IM_POSTCODE").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_POSTCODE").ToString.Trim
                    End If
                    If .Item("IM_CITY").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & " " & .Item("IM_CITY").ToString.Trim
                    End If

                    If .Item("IM_STATE").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, .Item("IM_STATE").ToString.Trim)
                    End If

                    If .Item("IM_COUNTRY").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, .Item("IM_COUNTRY").ToString.Trim)
                    End If

                    objIPPDetails.VAddr = strTempAddr
                    'objIPPDetails.VAddrLine1 = IIf(IsDBNull(.Item("IM_ADDR_LINE1")), "", .Item("IM_ADDR_LINE1"))
                    'objIPPDetails.VAddrLine2 = IIf(IsDBNull(.Item("IM_ADDR_LINE2")), "", .Item("IM_ADDR_LINE2"))
                    'objIPPDetails.VAddrLine3 = IIf(IsDBNull(.Item("IM_ADDR_LINE3")), "", .Item("IM_ADDR_LINE3"))
                    'objIPPDetails.VPostcode = IIf(IsDBNull(.Item("IM_POSTCODE")), "", .Item("IM_POSTCODE"))
                    'objIPPDetails.VCity = IIf(IsDBNull(.Item("IM_CITY")), "", .Item("IM_CITY"))
                    'objIPPDetails.VState = IIf(IsDBNull(.Item("IM_STATE")), "", .Item("IM_STATE"))
                    'objIPPDetails.VCountry = IIf(IsDBNull(.Item("IM_COUNTRY")), "", .Item("IM_COUNTRY"))
                    objIPPDetails.Status = IIf(IsDBNull(.Item("IM_INVOICE_STATUS")), "", .Item("IM_INVOICE_STATUS"))
                    objIPPDetails.Currency = IIf(IsDBNull(.Item("IM_CURRENCY_CODE")), "", .Item("IM_CURRENCY_CODE"))
                    objIPPDetails.PaymentAmt = Format(IIf(IsDBNull(.Item("IM_INVOICE_TOTAL")), 0, .Item("IM_INVOICE_TOTAL")), "#,##0.00")
                    objIPPDetails.PaymentMethod = IIf(IsDBNull(.Item("IM_PAYMENT_TERM")), "", .Item("IM_PAYMENT_TERM"))
                    objIPPDetails.WHTTax = IIf(IsDBNull(.Item("IM_WITHHOLDING_TAX")), "", .Item("IM_WITHHOLDING_TAX"))
                    objIPPDetails.WHTOpt = IIf(IsDBNull(.Item("IM_WITHHOLDING_OPT")), "", .Item("IM_WITHHOLDING_OPT"))
                    objIPPDetails.WHTReason = IIf(IsDBNull(.Item("IM_WITHHOLDING_REMARKS")), "", .Item("IM_WITHHOLDING_REMARKS"))
                    objIPPDetails.Remarks = IIf(IsDBNull(.Item("IM_REMARK")), "", .Item("IM_REMARK"))
                    objIPPDetails.Remark = IIf(IsDBNull(.Item("IM_REMARK")), "", .Item("IM_REMARK"))
                    objIPPDetails.LateSubmitReason = IIf(IsDBNull(.Item("IM_LATE_REASON")), "", .Item("IM_LATE_REASON"))
                    objIPPDetails.ExchangeRate = IIf(IsDBNull(.Item("IM_EXCHANGE_RATE")), "1", .Item("IM_EXCHANGE_RATE"))
                    If .Item("IM_PAYMENT_TERM") = "BC" Then
                        objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("im_bank_code")), "", .Item("im_bank_code"))
                    Else
                        objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("im_bank_code")), "", .Item("im_bank_code")) & "[" & IIf(IsDBNull(.Item("im_bank_acct")), "", .Item("im_bank_acct")) & "]"
                    End If

                    objIPPDetails.PaymentDate = IIf(IsDBNull(.Item("im_payment_date")), "", .Item("im_payment_date"))
                    objIPPDetails.PaymentNo = IIf(IsDBNull(.Item("im_payment_no")), "", .Item("im_payment_no"))
                    'objIPPDetails.DocReceivedDate = IIf(IsDBNull(.Item("IM_RECV")), "", .Item("IM_RECV"))
                    objIPPDetails.DocDueDate = IIf(IsDBNull(.Item("IM_DUE_DATE")), "", .Item("IM_DUE_DATE"))
                    objIPPDetails.PRCSSentDate = IIf(IsDBNull(.Item("IM_PRCS_SENT")), "", .Item("IM_PRCS_SENT"))
                    objIPPDetails.PRCSReceivedDate = IIf(IsDBNull(.Item("IM_PRCS_RECV")), "", .Item("IM_PRCS_RECV"))
                    objIPPDetails.PaymentNo = IIf(IsDBNull(.Item("IM_PAYMENT_NO")), "", .Item("IM_PAYMENT_NO"))
                    objIPPDetails.BankerChequeNo = IIf(IsDBNull(.Item("IM_CHEQUE_NO")), "", .Item("IM_CHEQUE_NO"))
                    objIPPDetails.StatusDescription = IIf(IsDBNull(.Item("STATUS_DESC")), "", .Item("STATUS_DESC"))
                    objIPPDetails.BeneficiaryDetails = IIf(IsDBNull(.Item("IM_REMARKS2")), "", .Item("IM_REMARKS2"))
                    objIPPDetails.BillInvApprBy = IIf(IsDBNull(.Item("IM_REMARKS1")), "", .Item("IM_REMARKS1"))
                    objIPPDetails.MasterDoc = IIf(IsDBNull(.Item("IM_IND1")), "", .Item("IM_IND1"))
                    'objIPPDetails.CreditTerm = IIf(IsDBNull(.Item("ic_credit_terms")), "", .Item("ic_credit_terms"))
                    If Not IsDBNull(.Item("IM_DUE_DATE")) Then
                        dtDiff = DateDiff(DateInterval.Day, CDate(.Item("IM_DOC_DATE")), CDate(.Item("IM_DUE_DATE")))
                        objIPPDetails.CreditTerm = dtDiff
                    Else
                        objIPPDetails.CreditTerm = ""
                    End If

                    'Zulham 15/02/2016 - IPP Stage 4 Phase 2\
                    objIPPDetails.TotalAmtNoGST = IIf(IsDBNull(.Item("im_invoice_excl_gst")), "", .Item("im_invoice_excl_gst"))
                    objIPPDetails.GSTAmt = IIf(IsDBNull(.Item("im_invoice_gst")), "", .Item("im_invoice_gst"))

                End With
                GetIPPDetails = objIPPDetails
            Else
                GetIPPDetails = Nothing
            End If
        End Function

        'Added by Joon on 4th May 2012 for issue 1584
        Public Function GetIPPEnqDetails(ByVal strIPPDocNo As String, ByVal strCoyId As String, ByVal InvIdx As Integer) As IPPDetails
            Dim dtIPP As New DataTable
            Dim objIPPDetails As New IPPDetails
            Dim strTempAddr As String
            Dim strsql As String
            Dim dtDiff As Long
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            If objDb.Exist("Select INVOICE_MSTR.*,INVOICE_DETAILS.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms  " &
            "from INVOICE_MSTR, INVOICE_DETAILS, STATUS_MSTR ,ipp_company where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID AND im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
            " AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
            "and IM_INVOICE_NO='" & strIPPDocNo & "' AND ID_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'") > 0 Then

                strsql = "Select INVOICE_MSTR.*,INVOICE_DETAILS.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO  " &
                            "from INVOICE_MSTR, INVOICE_DETAILS, STATUS_MSTR ,ipp_company where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID AND im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                            " AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                            "and IM_INVOICE_NO='" & strIPPDocNo & "' AND ID_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'"

            Else
                strsql = "Select INVOICE_MSTR.*,ipp_company.ic_bank_code,ipp_company.ic_bank_acct,status_mstr.STATUS_DESC,ipp_company.ic_credit_terms,ipp_company.IC_TAX_REG_NO  " &
                                            "from INVOICE_MSTR, STATUS_MSTR , ipp_company where im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                                            " AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
                                            "and IM_INVOICE_NO='" & strIPPDocNo & "' and IM_B_COY_ID='" & strCoyId & "' and im_invoice_index = '" & InvIdx & "'"
            End If
            '----------------------------------------------------
            dtIPP = objDb.FillDt(strsql)

            If Not dtIPP Is Nothing Then
                With dtIPP.Rows(0)
                    objIPPDetails.DocType = IIf(IsDBNull(.Item("IM_INVOICE_TYPE")), "", .Item("IM_INVOICE_TYPE"))
                    objIPPDetails.DocNo = IIf(IsDBNull(.Item("IM_INVOICE_NO")), "", .Item("IM_INVOICE_NO"))
                    objIPPDetails.DocDate = IIf(IsDBNull(.Item("IM_DOC_DATE")), "", .Item("IM_DOC_DATE"))
                    objIPPDetails.ManualPONo = IIf(IsDBNull(.Item("IM_IPP_PO")), "", .Item("IM_IPP_PO"))
                    objIPPDetails.Vendor = IIf(IsDBNull(.Item("IM_S_COY_NAME")), "", .Item("IM_S_COY_NAME"))
                    objIPPDetails.GSTRegNo = IIf(IsDBNull(.Item("IC_TAX_REG_NO")), "", .Item("IC_TAX_REG_NO"))

                    strTempAddr = .Item("IM_ADDR_LINE1").ToString.Trim

                    If .Item("IM_ADDR_LINE2").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_ADDR_LINE2").ToString.Trim
                    End If

                    If .Item("IM_ADDR_LINE3").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_ADDR_LINE3").ToString.Trim
                    End If

                    If .Item("IM_POSTCODE").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_POSTCODE").ToString.Trim
                    End If
                    If .Item("IM_CITY").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & " " & .Item("IM_CITY").ToString.Trim
                    End If

                    If .Item("IM_STATE").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, .Item("IM_STATE").ToString.Trim)
                    End If

                    If .Item("IM_COUNTRY").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, .Item("IM_COUNTRY").ToString.Trim)
                    End If

                    objIPPDetails.VAddr = strTempAddr
                    objIPPDetails.Status = IIf(IsDBNull(.Item("IM_INVOICE_STATUS")), "", .Item("IM_INVOICE_STATUS"))
                    objIPPDetails.Currency = IIf(IsDBNull(.Item("IM_CURRENCY_CODE")), "", .Item("IM_CURRENCY_CODE"))
                    objIPPDetails.PaymentAmt = Format(IIf(IsDBNull(.Item("IM_INVOICE_TOTAL")), 0, .Item("IM_INVOICE_TOTAL")), "#,##0.00")
                    objIPPDetails.PaymentMethod = IIf(IsDBNull(.Item("IM_PAYMENT_TERM")), "", .Item("IM_PAYMENT_TERM"))
                    objIPPDetails.WHTTax = IIf(IsDBNull(.Item("IM_WITHHOLDING_TAX")), "", .Item("IM_WITHHOLDING_TAX"))
                    objIPPDetails.WHTOpt = IIf(IsDBNull(.Item("IM_WITHHOLDING_OPT")), "", .Item("IM_WITHHOLDING_OPT"))
                    objIPPDetails.WHTReason = IIf(IsDBNull(.Item("IM_WITHHOLDING_REMARKS")), "", .Item("IM_WITHHOLDING_REMARKS"))
                    objIPPDetails.Remarks = IIf(IsDBNull(.Item("IM_REMARK")), "", .Item("IM_REMARK"))
                    objIPPDetails.Remark = IIf(IsDBNull(.Item("IM_REMARK")), "", .Item("IM_REMARK"))
                    objIPPDetails.LateSubmitReason = IIf(IsDBNull(.Item("IM_LATE_REASON")), "", .Item("IM_LATE_REASON"))
                    objIPPDetails.ExchangeRate = IIf(IsDBNull(.Item("IM_EXCHANGE_RATE")), "1", .Item("IM_EXCHANGE_RATE"))
                    If .Item("IM_PAYMENT_TERM") = "BC" Then
                        objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("im_bank_code")), "", .Item("im_bank_code"))
                    Else
                        objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("im_bank_code")), "", .Item("im_bank_code")) & "[" & IIf(IsDBNull(.Item("im_bank_acct")), "", .Item("im_bank_acct")) & "]"
                    End If
                    objIPPDetails.PaymentDate = IIf(IsDBNull(.Item("im_payment_date")), "", .Item("im_payment_date"))
                    objIPPDetails.PaymentNo = IIf(IsDBNull(.Item("im_payment_no")), "", .Item("im_payment_no"))
                    'objIPPDetails.DocReceivedDate = IIf(IsDBNull(.Item("IM_RECV")), "", .Item("IM_RECV"))
                    objIPPDetails.DocDueDate = IIf(IsDBNull(.Item("IM_DUE_DATE")), "", .Item("IM_DUE_DATE"))
                    objIPPDetails.PRCSSentDate = IIf(IsDBNull(.Item("IM_PRCS_SENT")), "", .Item("IM_PRCS_SENT"))
                    objIPPDetails.PRCSReceivedDate = IIf(IsDBNull(.Item("IM_PRCS_RECV")), "", .Item("IM_PRCS_RECV"))
                    objIPPDetails.PaymentNo = IIf(IsDBNull(.Item("IM_PAYMENT_NO")), "", .Item("IM_PAYMENT_NO"))
                    objIPPDetails.BankerChequeNo = IIf(IsDBNull(.Item("IM_CHEQUE_NO")), "", .Item("IM_CHEQUE_NO"))
                    objIPPDetails.StatusDescription = IIf(IsDBNull(.Item("STATUS_DESC")), "", .Item("STATUS_DESC"))
                    objIPPDetails.BeneficiaryDetails = IIf(IsDBNull(.Item("IM_REMARKS2")), "", .Item("IM_REMARKS2"))
                    objIPPDetails.BillInvApprBy = IIf(IsDBNull(.Item("IM_REMARKS1")), "", .Item("IM_REMARKS1"))
                    objIPPDetails.MasterDoc = IIf(IsDBNull(.Item("IM_IND1")), "", .Item("IM_IND1"))
                    'objIPPDetails.CreditTerm = IIf(IsDBNull(.Item("ic_credit_terms")), "", .Item("ic_credit_terms"))
                    If Not IsDBNull(.Item("IM_DUE_DATE")) Then
                        dtDiff = DateDiff(DateInterval.Day, CDate(.Item("IM_DOC_DATE")), CDate(.Item("IM_DUE_DATE")))
                        objIPPDetails.CreditTerm = dtDiff
                    Else
                        objIPPDetails.CreditTerm = ""
                    End If

                    'Zulham 16/02/2016 - IPP Stage 4 Phase 2
                    objIPPDetails.TotalAmtNoGST = IIf(IsDBNull(.Item("im_invoice_excl_gst")), "", .Item("im_invoice_excl_gst"))
                    objIPPDetails.GSTAmt = IIf(IsDBNull(.Item("im_invoice_gst")), "", .Item("im_invoice_gst"))

                End With
                GetIPPEnqDetails = objIPPDetails
            Else
                GetIPPEnqDetails = Nothing
            End If
        End Function

        Public Function GetApprIPPDetails(ByVal index As String, ByVal strIPPDocNo As String, ByVal strCoyId As String) As IPPDetails
            Dim dtIPP As New DataTable
            Dim objIPPDetails As New IPPDetails
            Dim strTempAddr As String
            Dim strsql As String
            Dim dtDiff As Long
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            'Zulham 12072018 - PAMB
            strsql = "Select * ,ipp_company.ic_bank_code, ipp_company.ic_bank_acct,ipp_company.ic_credit_terms, ipp_company.IC_TAX_REG_NO " &
            "from INVOICE_MSTR LEFT JOIN PAYMENT_TYPE ON IM_PYMT_TYPE_INDEX = PT_INDEX, INVOICE_DETAILS ,ipp_company, STATUS_MSTR " &
            "where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID AND ic_index = im_s_coy_id AND ic_coy_id = '" & strDefIPPCompID & "' " &
            " AND status_no = im_invoice_status AND status_type = 'INV' AND status_deleted = 'N' " &
            "and IM_B_COY_ID='" & strCoyId & "' and IM_INVOICE_INDEX = '" & index & "'"
            dtIPP = objDb.FillDt(strsql)
            '--------------------------------------------------

            If Not dtIPP Is Nothing Then
                With dtIPP.Rows(0)
                    objIPPDetails.DocType = IIf(IsDBNull(.Item("IM_INVOICE_TYPE")), "", .Item("IM_INVOICE_TYPE"))
                    objIPPDetails.DocNo = IIf(IsDBNull(.Item("IM_INVOICE_NO")), "", .Item("IM_INVOICE_NO"))
                    objIPPDetails.DocDate = IIf(IsDBNull(.Item("IM_DOC_DATE")), "", .Item("IM_DOC_DATE"))
                    objIPPDetails.ManualPONo = IIf(IsDBNull(.Item("IM_IPP_PO")), "", .Item("IM_IPP_PO"))
                    objIPPDetails.Vendor = IIf(IsDBNull(.Item("IM_S_COY_NAME")), "", .Item("IM_S_COY_NAME"))
                    objIPPDetails.PaymentType = IIf(IsDBNull(.Item("PT_PT_DESC")), "", .Item("PT_PT_DESC"))
                    objIPPDetails.GSTRegNo = IIf(IsDBNull(.Item("IC_TAX_REG_NO")), "", .Item("IC_TAX_REG_NO"))

                    strTempAddr = .Item("IM_ADDR_LINE1").ToString.Trim

                    If .Item("IM_ADDR_LINE2").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_ADDR_LINE2").ToString.Trim
                    End If

                    If .Item("IM_ADDR_LINE3").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_ADDR_LINE3").ToString.Trim
                    End If

                    If .Item("IM_POSTCODE").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & .Item("IM_POSTCODE").ToString.Trim
                    End If
                    If .Item("IM_CITY").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & " " & .Item("IM_CITY").ToString.Trim
                    End If

                    If .Item("IM_STATE").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, .Item("IM_STATE").ToString.Trim)
                    End If

                    If .Item("IM_COUNTRY").ToString.Trim <> "" Then
                        strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, .Item("IM_COUNTRY").ToString.Trim)
                    End If

                    objIPPDetails.VAddr = strTempAddr
                    'objIPPDetails.VAddrLine1 = IIf(IsDBNull(.Item("IM_ADDR_LINE1")), "", .Item("IM_ADDR_LINE1"))
                    'objIPPDetails.VAddrLine2 = IIf(IsDBNull(.Item("IM_ADDR_LINE2")), "", .Item("IM_ADDR_LINE2"))
                    'objIPPDetails.VAddrLine3 = IIf(IsDBNull(.Item("IM_ADDR_LINE3")), "", .Item("IM_ADDR_LINE3"))
                    'objIPPDetails.VPostcode = IIf(IsDBNull(.Item("IM_POSTCODE")), "", .Item("IM_POSTCODE"))
                    'objIPPDetails.VCity = IIf(IsDBNull(.Item("IM_CITY")), "", .Item("IM_CITY"))
                    'objIPPDetails.VState = IIf(IsDBNull(.Item("IM_STATE")), "", .Item("IM_STATE"))
                    'objIPPDetails.VCountry = IIf(IsDBNull(.Item("IM_COUNTRY")), "", .Item("IM_COUNTRY"))
                    objIPPDetails.Status = IIf(IsDBNull(.Item("IM_INVOICE_STATUS")), "", .Item("IM_INVOICE_STATUS"))
                    objIPPDetails.Currency = IIf(IsDBNull(.Item("IM_CURRENCY_CODE")), "", .Item("IM_CURRENCY_CODE"))
                    objIPPDetails.PaymentAmt = Format(IIf(IsDBNull(.Item("IM_INVOICE_TOTAL")), 0, .Item("IM_INVOICE_TOTAL")), "#,##0.00")
                    objIPPDetails.PaymentMethod = IIf(IsDBNull(.Item("IM_PAYMENT_TERM")), "", .Item("IM_PAYMENT_TERM"))
                    objIPPDetails.WHTTax = IIf(IsDBNull(.Item("IM_WITHHOLDING_TAX")), "", .Item("IM_WITHHOLDING_TAX"))
                    objIPPDetails.WHTOpt = IIf(IsDBNull(.Item("IM_WITHHOLDING_OPT")), "", .Item("IM_WITHHOLDING_OPT"))
                    objIPPDetails.WHTReason = IIf(IsDBNull(.Item("IM_WITHHOLDING_REMARKS")), "", .Item("IM_WITHHOLDING_REMARKS"))
                    objIPPDetails.Remarks = IIf(IsDBNull(.Item("IM_REMARK")), "", .Item("IM_REMARK"))
                    objIPPDetails.ExchangeRate = IIf(IsDBNull(.Item("IM_EXCHANGE_RATE")), "1", .Item("IM_EXCHANGE_RATE"))
                    objIPPDetails.Remark = IIf(IsDBNull(.Item("IM_REMARK")), "", .Item("IM_REMARK"))
                    objIPPDetails.LateSubmitReason = IIf(IsDBNull(.Item("IM_LATE_REASON")), "", .Item("IM_LATE_REASON"))
                    objIPPDetails.ExchangeRate = IIf(IsDBNull(.Item("IM_EXCHANGE_RATE")), "", .Item("IM_EXCHANGE_RATE"))
                    'objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("ic_bank_code")), "", .Item("ic_bank_code")) & "[" & IIf(IsDBNull(.Item("ic_bank_acct")), "", .Item("ic_bank_acct")) & "]"
                    If .Item("IM_PAYMENT_TERM") = "BC" Then
                        objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("im_bank_code")), "", .Item("im_bank_code"))
                    Else
                        objIPPDetails.BankNameAccountNo = IIf(IsDBNull(.Item("im_bank_code")), "", .Item("im_bank_code")) & "[" & IIf(IsDBNull(.Item("im_bank_acct")), "", .Item("im_bank_acct")) & "]"
                    End If
                    objIPPDetails.PaymentDate = IIf(IsDBNull(.Item("im_payment_date")), "", .Item("im_payment_date"))
                    objIPPDetails.PaymentNo = IIf(IsDBNull(.Item("im_payment_no")), "", .Item("im_payment_no"))
                    'objIPPDetails.DocReceivedDate = IIf(IsDBNull(.Item("IM_RECV")), "", .Item("IM_RECV"))
                    objIPPDetails.DocDueDate = IIf(IsDBNull(.Item("IM_DUE_DATE")), "", .Item("IM_DUE_DATE"))
                    objIPPDetails.BankerChequeNo = IIf(IsDBNull(.Item("IM_CHEQUE_NO")), "", .Item("IM_CHEQUE_NO"))
                    objIPPDetails.PRCSSentDate = IIf(IsDBNull(.Item("IM_PRCS_SENT")), "", .Item("IM_PRCS_SENT"))
                    objIPPDetails.PRCSReceivedDate = IIf(IsDBNull(.Item("IM_PRCS_RECV")), "", .Item("IM_PRCS_RECV"))
                    objIPPDetails.BeneficiaryDetails = IIf(IsDBNull(.Item("IM_REMARKS2")), "", .Item("IM_REMARKS2"))
                    objIPPDetails.BillInvApprBy = IIf(IsDBNull(.Item("IM_REMARKS1")), "", .Item("IM_REMARKS1"))
                    objIPPDetails.StatusDescription = IIf(IsDBNull(.Item("STATUS_DESC")), "", .Item("STATUS_DESC"))
                    objIPPDetails.MasterDoc = IIf(IsDBNull(.Item("IM_IND1")), "", .Item("IM_IND1"))
                    objIPPDetails.jobGrade = IIf(IsDBNull(.Item("IM_ADDITIONAL_1")), "", .Item("IM_ADDITIONAL_1"))
                    'objIPPDetails.CreditTerm = IIf(IsDBNull(.Item("ic_credit_terms")), "", .Item("ic_credit_terms"))
                    If Not IsDBNull(.Item("IM_DUE_DATE")) Then
                        dtDiff = DateDiff(DateInterval.Day, CDate(.Item("IM_DOC_DATE")), CDate(.Item("IM_DUE_DATE")))
                        objIPPDetails.CreditTerm = dtDiff
                    Else
                        objIPPDetails.CreditTerm = ""
                    End If

                    'Zulham 17/02/2016 - IPP Stage 4 Phase 2
                    objIPPDetails.TotalAmtNoGST = IIf(IsDBNull(.Item("im_invoice_excl_gst")), "", .Item("im_invoice_excl_gst"))
                    objIPPDetails.GSTAmt = IIf(IsDBNull(.Item("im_invoice_gst")), "", .Item("im_invoice_gst"))

                End With
                GetApprIPPDetails = objIPPDetails
            Else
                GetApprIPPDetails = Nothing
            End If
        End Function

        Public Function ipp_detail(ByVal ippDocNo As String, ByVal vCoyId As String, ByVal VenIdx As Integer, Optional ByVal frm As String = "", Optional ByVal userid As String = "") As DataSet
            Dim InvIdx As String
            Dim strsql As String
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            If userid = "" Then
                userid = Common.Parse(HttpContext.Current.Session("UserID"))
            End If

            'Jules 2018.07.11 - Allow "\" and "#"
            ippDocNo = Replace(Replace(ippDocNo, "\", "\\"), "#", "\#")

            InvIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & ippDocNo & "' AND im_s_coy_id = '" & VenIdx & "'")

            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            If frm = "PSDAcceptRejList" Then
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " _
                            & "FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                              & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE And cbg_b_coy_id ='" & Common.Parse(strDefIPPCompID) & "' " _
                              & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                               & "AND IM_INVOICE_NO='" & ippDocNo & "' AND ID_INVOICE_NO='" & ippDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' and id_s_coy_id = '" & VenIdx & "' " _
                               & "AND im_invoice_index = '" & InvIdx & "'"
            ElseIf frm = "PSDAcceptanceDetails" Or frm = "PSDAcceptRejList,dashboard" Then
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " _
                            & "FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                              & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE And cbg_b_coy_id ='" & Common.Parse(strDefIPPCompID) & "' " _
                              & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                               & "AND IM_INVOICE_NO='" & ippDocNo & "' AND ID_INVOICE_NO='" & ippDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' and id_s_coy_id = '" & VenIdx & "' " _
                               & "AND im_invoice_index = '" & InvIdx & "'"
            ElseIf frm = "EnterBC" Then
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " _
                        & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " _
                        & "FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                                               & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE And cbg_b_coy_id ='" & Common.Parse(strDefIPPCompID) & "' " _
                                               & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                                                & "AND IM_INVOICE_NO='" & ippDocNo & "' AND ID_INVOICE_NO='" & ippDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' AND  id_s_coy_id = '" & VenIdx & "' " _
                                                & "AND im_invoice_index = '" & InvIdx & "'"


            Else
                'Zulham 10072018 - PAMB
                'added or im_status_changed_by = '" & userid & "' condition
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " _
                            & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " _
                            & "FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                                & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE And cbg_b_coy_id ='" & Common.Parse(strDefIPPCompID) & "' " _
                                & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                                 & "AND IM_INVOICE_NO='" & ippDocNo & "' AND ID_INVOICE_NO='" & ippDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' AND (im_created_by = '" & userid & "' or im_status_changed_by = '" & userid & "') and id_s_coy_id = '" & VenIdx & "' " _
                                 & "AND im_invoice_index = '" & InvIdx & "'"
            End If

            ipp_detail = objDb.FillDs(strsql)
        End Function

        'Added by Joon on 4th May 2012 for issue 1584
        Public Function IPPEnq_detail(ByVal ippDocNo As String, ByVal vCoyId As String, ByVal VenIdx As Integer) As DataSet
            Dim InvIdx As String
            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            'Jules 2018.07.11 - Allow "\" and "#"
            ippDocNo = Replace(Replace(ippDocNo, "\", "\\"), "#", "\#")

            InvIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & ippDocNo & "' AND im_s_coy_id = '" & VenIdx & "'")

            'Jules 2018.07.11 - Added Analysis Codes    
            Dim strsql As String = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " _
                                & "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " _
                                & "FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                                & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE And cbg_b_coy_id ='" & Common.Parse(strDefIPPCompID) & "' " _
                                & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                                & "AND IM_INVOICE_NO='" & ippDocNo & "' AND ID_INVOICE_NO='" & ippDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' AND id_s_coy_id = '" & VenIdx & "' " _
                                & "AND im_invoice_index = '" & InvIdx & "'"

            IPPEnq_detail = objDb.FillDs(strsql)
        End Function

        Public Function PopulateIPPList(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String, Optional ByVal frm As String = "", Optional ByVal role As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            If frm = "PSDAcceptReject" And docstatus = "" Then
                'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " & _
                '"IM_INVOICE_STATUS, im_bank_code, im_bank_acct, ic_bank_code, ic_bank_acct, im_payment_date, im_payment_no, im_prcs_sent, itl_remarks,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " & _
                '"FROM INVOICE_MSTR  " & _
                '"INNER JOIN ipp_trans_log ON im_invoice_index = itl_invoice_index " & _
                '"INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '"WHERE  itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND (itl_remarks = 'Enter PSD Received Date' OR itl_remarks LIKE 'Rejected:%') AND IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '"AND im_invoice_status IN (14,11) "

                'Zulham 18092018 - PAMB UAT
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " &
                "IM_INVOICE_STATUS, im_bank_code, im_bank_acct, ic_bank_code, ic_bank_acct, im_payment_date, im_payment_no, im_prcs_sent, itl_remarks,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " &
                "FROM INVOICE_MSTR  " &
                "INNER JOIN ipp_trans_log ON im_invoice_index = itl_invoice_index " &
                "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                "WHERE  itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND (itl_remarks = 'Enter PSD Received Date' OR itl_remarks LIKE 'Rejected:%') AND IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                "AND im_invoice_status IN (14,11,19) "

            ElseIf frm = "PSDAcceptReject" And docstatus <> "" Then
                'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " & _
                ' "IM_INVOICE_STATUS, im_bank_code, im_bank_acct, ic_bank_code, ic_bank_acct, im_payment_date, im_payment_no, im_prcs_sent, itl_remarks,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " & _
                ' "FROM INVOICE_MSTR  " & _
                ' "INNER JOIN ipp_trans_log ON im_invoice_index = itl_invoice_index " & _
                ' "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                ' "WHERE  itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND (itl_remarks = 'Enter PSD Received Date' OR itl_remarks LIKE 'Rejected:%') AND IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " &
                 "IM_INVOICE_STATUS, im_bank_code, im_bank_acct, ic_bank_code, ic_bank_acct, im_payment_date, im_payment_no, im_prcs_sent, itl_remarks,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " &
                 "FROM INVOICE_MSTR  " &
                 "INNER JOIN ipp_trans_log ON im_invoice_index = itl_invoice_index " &
                 "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                 "WHERE  itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND (itl_remarks = 'Enter PSD Received Date' OR itl_remarks LIKE 'Rejected:%') AND IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

                'ElseIf frm = "IPPList" And role = "IPPTeller" Then
                '    strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no,STATUS_DESC,im_prcs_sent " & _
                '        "FROM INVOICE_MSTR " & _
                '        "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " & _
                '        "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '         "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                '       "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            Else
                'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no,STATUS_DESC,im_prcs_sent,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " & _
                '    "FROM INVOICE_MSTR " & _
                '    "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " & _
                '    "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '     "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                '   "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no,STATUS_DESC,im_prcs_sent,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO " &
                    "FROM INVOICE_MSTR " &
                    "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " &
                    "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                     "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " &
                   "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            End If

            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                If docstatus = "14" Then
                    'If role = "IPPTeller" Then
                    If frm = "PSDAcceptReject" Then
                        strSql &= " AND im_invoice_status IN (14) or (IM_INVOICE_STATUS = 14 and im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "')"
                    Else
                        strSql &= " AND IM_INVOICE_STATUS = " & docstatus & ""
                    End If
                    'Else
                    '    strSql &= " AND (IM_INVOICE_STATUS = " & docstatus & " and im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "')"
                    'End If                    
                Else
                    If InStr(docstatus, "14") And frm = "PSDAcceptReject" Then
                        'docstatus = docstatus.Substring(0, docstatus.Length - 3)
                        strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ") OR (IM_INVOICE_STATUS = 14 and im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "')"
                        ' strSql &= " OR (itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' AND itl_remarks LIKE 'Rejected:%') "
                    Else
                        strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
                    End If

                End If

            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            'If strVen <> "" Then
            '    strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
            'End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If
            If frm = "PSDAcceptReject" Then
                strSql &= " GROUP BY itl_invoice_index "
            End If
            dsGroup = objDb.FillDs(strSql)
            PopulateIPPList = dsGroup
        End Function

        Public Function PopulateIPPListNew(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String, Optional ByVal role As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 9 Feb 2015
            Dim strCoyId As String
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If
            '--------------------------------------------
            'Zulham 29042019 - multiinvoices
            strSql = "SELECT * FROM (" &
                    "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL," &
                    "IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no," &
                    "STATUS_DESC,im_prcs_sent,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO, ISD_DOC_NO, ISD_DOC_AMT " &
                    "FROM INVOICE_MSTR " &
                    "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " &
                    "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                    "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " &
                    "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND IM_DOC_DATE IS NOT NULL "

            'Zulham 05042019 - multiinvoices
            If docno <> "" And HttpContext.Current.Session("strDocNo") Is Nothing Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            ElseIf HttpContext.Current.Session("strDocNo") IsNot Nothing Then
                strSql &= " AND IM_INVOICE_NO IN (" & docno & ")"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If

            strSql &= " UNION " &
                    "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL," &
                    "IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, " &
                    "STATUS_DESC,im_prcs_sent,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO, ISD_DOC_NO, ISD_DOC_AMT " &
                    "FROM INVOICE_MSTR " &
                    "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " &
                    "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                    "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " &
                    "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "
            'Zulham 05042019 - multiinvoices
            If docno <> "" And HttpContext.Current.Session("strDocNo") Is Nothing Then
                strSql &= " AND ISD_DOC_NO LIKE '%" & docno & "%'"
            ElseIf HttpContext.Current.Session("strDocNo") IsNot Nothing Then
                strSql &= " AND ISD_DOC_NO in (" & docno & ")"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If docsdt <> "" Then
                strSql &= " AND ISD_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND ISD_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If

            'Zulham 23042019 - REQ018
            strSql &= "UNION ALL " &
                    "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL," &
                    "IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no," &
                    "STATUS_DESC,im_prcs_sent,IFNULL(IM_ROUTE_TO,'') as IM_ROUTE_TO, ISD_DOC_NO, ISD_DOC_AMT " &
                    "FROM INVOICE_MSTR " &
                    "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " &
                    "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                    "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " &
                    "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND IM_DOC_DATE IS NULL "

            'Zulham 05042019 - multiinvoices
            If docno <> "" And HttpContext.Current.Session("strDocNo") Is Nothing Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            ElseIf HttpContext.Current.Session("strDocNo") IsNot Nothing Then
                strSql &= " AND IM_INVOICE_NO IN (" & docno & ")"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If


            strSql &= ") tb ORDER BY IM_INVOICE_INDEX "

            dsGroup = objDb.FillDs(strSql)
            PopulateIPPListNew = dsGroup
        End Function

        'Added by Joon on 4th May 2012 for issue 1584
        'Public Function PopulateIPPEnqList(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal paysdt As String, ByVal payedt As String, ByVal strVen As String) As DataSet
        'Public Function PopulateIPPEnqList(ByVal docno As String, ByVal payadv As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal paysdt As String, ByVal payedt As String, ByVal strVen As String) As DataSet
        '    Dim strSql As String
        '    Dim dsGroup As DataSet

        '    'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date,im_payment_no " & _
        '    '        "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
        '    '        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
        '    '         "AND NOT IM_INVOICE_TYPE IS NULL AND IM_INVOICE_STATUS NOT IN (10,15)"
        '    strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_CREATED_BY,IM_DOC_DATE,IM_S_COY_NAME,im_bank_code,im_bank_acct,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,im_payment_date,im_payment_no, IM_INVOICE_STATUS " & _
        '            "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
        '            "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
        '             "AND NOT IM_INVOICE_TYPE IS NULL"
        '    If docno <> "" Then
        '        strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
        '    End If
        '    'If doctype <> "" Then
        '    'strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
        '    'End If
        '    If payadv <> "" Then
        '        strSql &= " AND IM_PAYMENT_NO LIKE '%" & payadv & "%'"
        '    End If
        '    If docstatus <> "" Then
        '        strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
        '    End If
        '    If docsdt <> "" Then
        '        strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
        '    End If
        '    If docedt <> "" Then
        '        strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
        '    End If
        '    If paysdt <> "" Then
        '        strSql &= " AND IM_PAYMENT_DATE >= " & Common.ConvertDate(paysdt) & ""
        '    End If
        '    If payedt <> "" Then
        '        strSql &= " AND IM_PAYMENT_DATE <= " & Common.ConvertDate(payedt) & ""
        '    End If
        '    If strVen <> "" Then
        '        strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
        '    End If

        '    dsGroup = objDb.FillDs(strSql)
        '    PopulateIPPEnqList = dsGroup
        'End Function

        Public Function PopulateIPPEnqList(ByVal docno As String, ByVal payadv As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal paysdt As String, ByVal payedt As String, ByVal strVen As String, ByVal strVenAddr As String, Optional ByVal Dept As String = "", Optional ByVal psdsentsdt As String = "", Optional ByVal psdsentedt As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim strSql2 As String = ""
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strCoyId As String
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If
            '--------------------------------------------
            'Jules 2018.12.27 - If user is not FO/FM, user should only see docs raised by them.
            Dim objUsers As New Users
            Dim blnFinanceOfficer = objUsers.checkUserFixedRole(HttpContext.Current.Session("UserID"), HttpContext.Current.Session("CompanyID"), "'Finance Officer'")
            Dim blnFinanceManager = objUsers.checkUserFixedRole(HttpContext.Current.Session("UserID"), HttpContext.Current.Session("CompanyID"), "'Finance Manager'")
            'End modification.

            strSql = "SELECT * FROM ("
            'Zulham 05122018
            'Search document No.
            If Dept = "" Then
                strSql &= "SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE,IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " &
                        "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " &
                        "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " &
                        "IM_INVOICE_TYPE, IM_PRCS_RECV " &
                        "FROM INVOICE_MSTR " &
                        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " &
                        "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                        "WHERE IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                        "AND NOT IM_INVOICE_TYPE IS NULL "

                'Jules 2018.12.27 - If user is not FO/FM, user should only see docs raised by them.
                If Not blnFinanceOfficer AndAlso Not blnFinanceManager Then
                    strSql &= "AND IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
                End If
                'End modification.
            Else
                strSql &= "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE, IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " &
                        "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " &
                        "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " &
                        "IM_INVOICE_TYPE, IM_PRCS_RECV " &
                        "FROM INVOICE_MSTR " &
                        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " &
                        "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "INNER JOIN USER_MSTR um ON IM_CREATED_BY = um.UM_USER_ID " &
                        "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND NOT IM_INVOICE_TYPE IS NULL " &
                        "AND um.UM_DEPT_ID = '" & Dept & "' AND IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
            End If

            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & "'" & Format(CDate(docedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If payadv <> "" Then
                strSql &= " AND IM_PAYMENT_NO LIKE '%" & payadv & "%'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If paysdt <> "" Then
                strSql &= " AND IM_PAYMENT_DATE >= " & Common.ConvertDate(paysdt) & ""
            End If
            If payedt <> "" Then
                strSql &= " AND IM_PAYMENT_DATE <= " & "'" & Format(CDate(payedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If psdsentsdt <> "" Then
                strSql &= " AND IM_PRCS_SENT >= " & Common.ConvertDate(psdsentsdt) & ""
            End If
            If psdsentedt <> "" Then
                strSql &= " AND IM_PRCS_SENT <= " & "'" & Format(CDate(psdsentedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If
            If strVenAddr <> "" Then
                strSql &= " AND ic2.IC_ADDR_LINE1 like '%" & strVenAddr & "%'"
            End If

            strSql &= " UNION "
            'zULHAM 05122018
            'Search Sub-document No.
            If Dept = "" Then
                strSql &= "SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE,IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " &
                        "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " &
                        "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " &
                        "IM_INVOICE_TYPE, IM_PRCS_RECV " &
                        "FROM INVOICE_MSTR " &
                        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " &
                        "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                        "WHERE IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                        "AND NOT IM_INVOICE_TYPE IS NULL "

                'Jules 2018.12.27 - If user is not FO/FM, user should only see docs raised by them.
                If Not blnFinanceOfficer AndAlso Not blnFinanceManager Then
                    strSql &= "AND IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
                End If
                'End modification.
            Else
                strSql &= "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE, IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " &
                        "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " &
                        "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " &
                        "IM_INVOICE_TYPE, IM_PRCS_RECV " &
                        "FROM INVOICE_MSTR " &
                        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " &
                        "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "INNER JOIN USER_MSTR um ON IM_CREATED_BY = um.UM_USER_ID " &
                        "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " &
                        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND NOT IM_INVOICE_TYPE IS NULL " &
                        "AND um.um_dept_id = '" & Dept & "' AND IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
            End If

            If docno <> "" Then
                strSql &= " AND ISD_DOC_NO LIKE '%" & docno & "%'"
            End If
            If docsdt <> "" Then
                strSql &= " AND ISD_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND ISD_DOC_DATE <= " & "'" & Format(CDate(docedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If payadv <> "" Then
                strSql &= " AND IM_PAYMENT_NO LIKE '%" & payadv & "%'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If paysdt <> "" Then
                strSql &= " AND IM_PAYMENT_DATE >= " & Common.ConvertDate(paysdt) & ""
            End If
            If payedt <> "" Then
                strSql &= " AND IM_PAYMENT_DATE <= " & "'" & Format(CDate(payedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If psdsentsdt <> "" Then
                strSql &= " AND IM_PRCS_SENT >= " & Common.ConvertDate(psdsentsdt) & ""
            End If
            If psdsentedt <> "" Then
                strSql &= " AND IM_PRCS_SENT <= " & "'" & Format(CDate(psdsentedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If
            If strVenAddr <> "" Then
                strSql &= " AND ic2.IC_ADDR_LINE1 like '%" & strVenAddr & "%'"
            End If

            strSql &= ") tb ORDER BY IM_INVOICE_INDEX "

            dsGroup = objDb.FillDs(strSql)
            PopulateIPPEnqList = dsGroup
        End Function

        Public Function PopulateIPPApprovalList(ByVal docno As String, ByVal doctype As String, ByVal docsdt As String, ByVal docedt As String) As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            Dim ds As DataSet


            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS " _
                    & "FROM INVOICE_MSTR WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                    & "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and IM_INVOICE_STATUS IN (11,12) "
            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If

            dsGroup = objDb.FillDs(strSql)
            PopulateIPPApprovalList = dsGroup
        End Function
        Public Function PopulateIPPApprRejList(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal vendor As String, ByVal verifiedsdt As String, ByVal verifiededt As String) As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 9 Feb 2015
            Dim strCoyId As String
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If
            '--------------------------------------------

            strSql = "SELECT distinct IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS, " _
                    & "im_payment_date,im_payment_no,ic_bank_code, ic_bank_acct,STATUS_DESC " _
                    & "FROM INVOICE_MSTR " _
                    & "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " _
                    & "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " _
                    & "INNER JOIN ipp_trans_log ON itl_invoice_index = im_invoice_index AND itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                    & "LEFT JOIN finance_approval ON IM_INVOICE_INDEX = fa_invoice_index WHERE " _
                    & " IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND fa_active_ao = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"
            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
                'Else
                '    strSql &= " AND IM_INVOICE_STATUS IN (17,18,14)"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If verifiedsdt <> "" Then
                strSql &= " AND CAST(itl_trans_date AS DATE) >= " & Common.ConvertDate(verifiedsdt) & ""
            End If
            If verifiededt <> "" Then
                strSql &= " AND CAST(itl_trans_date AS DATE)  <= " & Common.ConvertDate(verifiededt) & ""
            End If
            If vendor <> "" Then
                strSql &= " AND IM_S_COY_NAME LIKE '%" & vendor & "%'"
            End If
            dsGroup = objDb.FillDs(strSql)
            PopulateIPPApprRejList = dsGroup
        End Function

        Public Function SearchIPPDocuments(ByVal strDocNo As String, ByVal strDocType As String, ByVal strStartDate As String, ByVal strEndDate As String, ByVal strStatus As String) As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet

            strSql = "SELECT IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS " _
                        & "FROM(INVOICE_MSTR) WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                        & "FROM IPP_COMPANY WHERE IC_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"

            If strDocNo <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & Common.Parse(strDocNo) & "%' "
            End If
            If strDocType <> "" Then
                strSql &= " AND IM_INVOICE_TYPE LIKE '%" & Common.Parse(strDocType) & "%' "
            End If
            If strStartDate <> "" Then
                strSql &= "AND IM_DOC_DATE >= " & Common.ConvertDate(strStartDate) & " "
            End If
            If strEndDate <> "" Then
                strSql &= "AND IM_DOC_DATE <= " & Common.ConvertDate(strEndDate & " 23:59:59.000") & " "
            End If
            If strStatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS = '" & Common.Parse(strStatus) & "'"
            End If

            dsGroup = objDb.FillDs(strSql)
            SearchIPPDocuments = dsGroup
        End Function

        ''''''''''''''''''' Cost Allocation IPP''''''''''''''''''''''''''''

        Public Function GetCostAlloc(ByVal strCostAllocCode As String, ByVal strDesc As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT CAM_INDEX,CAM_COY_ID, CAM_USER_ID, CAM_CA_CODE, CAM_CA_DESC FROM COST_ALLOC_MSTR WHERE CAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CAM_USER_ID = '" & HttpContext.Current.Session("UserId") & "'"

            If strCostAllocCode <> "" Then
                strsql &= " AND CAM_CA_CODE LIKE '%" & strCostAllocCode & "%'"
            End If
            If strDesc <> "" Then
                strsql &= " AND CAM_CA_DESC Like '%" & strDesc & "%'"
            End If

            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                strCostAllocCode = ds.Tables(0).Rows(0).Item("CAM_CA_CODE")
                strDesc = ds.Tables(0).Rows(0).Item("CAM_CA_DESC")

            Else
                strCostAllocCode = "Cost Allocation Code"
                strDesc = "Cost Allocation Description"

            End If
            Return ds
        End Function
        Public Function GetSelectedCACode(ByVal strCACode As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT CAM_INDEX,CAM_COY_ID, CAM_USER_ID, CAM_CA_CODE, CAM_CA_DESC FROM COST_ALLOC_MSTR WHERE CAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CAM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND CAM_CA_CODE = '" & strCACode & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function DelCostAllocCode(ByVal dtCACode As DataTable) As Boolean
            Dim i As Integer
            Dim strSQL As String

            For i = 0 To dtCACode.Rows.Count - 1
                If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND id_cost_alloc_code =  '" & Common.Parse(dtCACode.Rows(i)("CACode")) & "'") > 0 Then
                    Return False
                    Exit Function
                End If

                strSQL = " DELETE FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & Common.Parse(dtCACode.Rows(i)("index")) & "'"

                objDb.Execute(strSQL)

                strSQL = " DELETE FROM COST_ALLOC_MSTR WHERE CAM_INDEX = '" & Common.Parse(dtCACode.Rows(i)("index")) & "' AND CAM_COY_ID = '" & Common.Parse(dtCACode.Rows(i)("CoyId")) & "' AND CAM_USER_ID = '" & Common.Parse(dtCACode.Rows(i)("UsrID")) & "' "

                objDb.Execute(strSQL)


            Next
            Return True

        End Function

        Public Function UpdateCostAllocCode(ByVal dsCACode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String
            Dim CACode As String

            If dsCACode.Tables(0).Rows(0)("Index") IsNot Nothing Then
                CACode = objDb.GetVal("select cam_ca_code from cost_alloc_mstr where cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and cam_index = '" & dsCACode.Tables(0).Rows(0)("Index") & "'")
                If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND id_cost_alloc_code =  '" & CACode & "'") > 0 Then
                    'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "'") > 0 Then
                    Return False
                    Exit Function
                End If
            Else
                'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & CACode & "'") > 0 Then
                If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND id_cost_alloc_code =  '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "'") > 0 Then
                    Return False
                    Exit Function
                End If
            End If



            strSQL = "UPDATE COST_ALLOC_MSTR SET " &
                  "CAM_CA_CODE='" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "'," &
                  "CAM_CA_DESC='" & Common.Parse(dsCACode.Tables(0).Rows(0)("CADesc")) & "'" &
                  " WHERE CAM_INDEX = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("Index")) & "' AND CAM_COY_ID = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CompID")) & "' AND CAM_USER_ID = '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CAUserID")) & "'"


            objDb.Execute(strSQL)

            Return True

        End Function
        Public Function InsertCostAllocCode(ByVal dsCACode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM cost_alloc_mstr WHERE cam_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND cam_ca_code =  '" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "'") > 0 Then
                Return False
                Exit Function
            End If

            strSQL = " INSERT INTO COST_ALLOC_MSTR(" &
                    "CAM_COY_ID, CAM_USER_ID," &
                    "CAM_CA_CODE, CAM_CA_DESC) " &
                    "VALUES('" & Common.Parse(dsCACode.Tables(0).Rows(0)("CompID")) & "','" & Common.Parse(dsCACode.Tables(0).Rows(0)("CAUserID")) & "'," &
                    "'" & Common.Parse(dsCACode.Tables(0).Rows(0)("CACode")) & "','" & Common.Parse(dsCACode.Tables(0).Rows(0)("CADesc")) & "')"


            objDb.Execute(strSQL)
            Return True

        End Function
        Public Sub FillCACode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            strSql = "SELECT CAM_INDEX,CAM_COY_ID, CAM_USER_ID, CONCAT(CAM_CA_CODE , "" : "", CAM_CA_DESC) AS ddlText FROM COST_ALLOC_MSTR WHERE CAM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CAM_USER_ID = '" & HttpContext.Current.Session("UserId") & "'"
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                FillDdl(pDropDownList, "ddlText", "CAM_INDEX", drw)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "---Select---"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            objDb = Nothing
        End Sub
        'Cost Allocation Details''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Function FillDdl(ByRef pDropDownList As UI.WebControls.DropDownList,
                        ByRef pstrText As String,
                        ByVal pstrValue As String,
                        ByRef pDataSource As Object,
                        Optional ByVal pDefaultText As String = "") As Boolean

            pDropDownList.DataSource = pDataSource
            pDropDownList.DataTextField = pstrText
            pDropDownList.DataValueField = pstrValue
            pDropDownList.DataBind()

            If pDefaultText <> "" Then
                pDropDownList.Items.Insert(0, pDefaultText)
            End If
        End Function

        Public Function GetCostAllocDetail2(ByVal strCostAllocCode As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            'strsql = "SELECT CAD_CC_CODE, CAD_BRANCH_CODE, CAD_PERCENT FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX =  '" & strCostAllocCode & "'"

            'strsql = "SELECT CONCAT(CAD.CAD_CC_CODE , "" : "" , CC.CC_CC_DESC) AS CAD_CC_CODE , " & _
            '        "CONCAT(CAD.CAD_BRANCH_CODE , "" : "" , CB.CB_BRANCH_NAME) AS CAD_BRANCH_CODE, " & _
            '        "CAD.CAD_PERCENT FROM COST_ALLOC_DETAIL CAD " & _
            '        "INNER JOIN cost_centre cc ON cc.cc_cc_code = cad.cad_cc_code " & _
            '        "INNER JOIN company_branch cb ON cb.cb_branch_code = cad.cad_branch_code " & _
            '        "WHERE CAD_CAM_INDEX =  '" & strCostAllocCode & "'"

            strsql = "SELECT CAD.CAD_CC_CODE ,CC.CC_CC_DESC, " &
                    "CAD.CAD_BRANCH_CODE ,  CB.CBM_BRANCH_NAME, " &
                    "CAD.CAD_PERCENT FROM COST_ALLOC_DETAIL CAD " &
                    "LEFT JOIN cost_centre cc ON cc.cc_cc_code = cad.cad_cc_code AND cc.cc_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "INNER JOIN COMPANY_BRANCH_MSTR cb ON cb.CBM_BRANCH_CODE = cad.cad_branch_code AND CBM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                    "WHERE CAD_CAM_INDEX =  '" & strCostAllocCode & "' "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function UpdateCostAllocDetail(ByVal dsCAD As DataSet, ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String
            Dim strAry(0) As String

            If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & strCACode & "'") > 0 Then
                Return False
                Exit Function
            Else
                strSQL = "DELETE FROM COST_ALLOC_DETAIL " &
                "where CAD_CAM_INDEX = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "'"

                Common.Insert2Ary(strAry, strSQL)
            End If


            For i = 0 To dsCAD.Tables(0).Rows.Count - 1
                'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & strCACode & "'") > 0 Then
                '    Return False
                '    Exit Function
                'End If

                'If objDb.Exist("SELECT '*' FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "' AND CAD_CC_CODE = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CostCenterCode")) & "' and cad_branch_code = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("BranchCode")) & "'") > 0 Then
                'strSQL = "UPDATE COST_ALLOC_DETAIL SET " & _
                '     "CAD_PERCENT='" & Common.Parse(dsCAD.Tables(0).Rows(i)("Percent")) & "'" & _
                '     " WHERE CAD_CAM_INDEX = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "' AND CAD_CC_CODE = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("CostCenterCode")) & "' AND CAD_BRANCH_CODE = '" & Common.Parse(dsCAD.Tables(0).Rows(i)("BranchCode")) & "'"
                'Else

                strSQL = "INSERT INTO COST_ALLOC_DETAIL(CAD_CAM_INDEX,CAD_CC_CODE, CAD_BRANCH_CODE, CAD_PERCENT) VALUES("
                strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("CAMIDX")) & "', "
                strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("CostCenterCode")) & "', "
                strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("BranchCode")) & "', "
                strSQL &= "'" & Common.Parse(dsCAD.Tables(0).Rows(i)("Percent")) & "')"

                'End If


                Common.Insert2Ary(strAry, strSQL)

            Next

            If objDb.BatchExecute(strAry) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function getCostCentre(Optional ByVal compid As String = "", Optional ByVal strUserInput As String = "", Optional ByVal role As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'strsql = "SELECT  CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE, CC_CC_DESC " _
            '        & "FROM COST_CENTRE " _
            '        & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            If role = "1" Then
                If compid <> "" Then
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                    "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' "
                        Else
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                     & "FROM COST_CENTRE " _
                                     & "WHERE CC_COY_ID='" & Common.Parse(compid) & "' and CC_STATUS = 'A' "
                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                     "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                     "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                     "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " &
                                     "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        Else
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                     & "FROM COST_CENTRE " _
                                     & "WHERE cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " _
                                     & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        End If
                    End If
                Else
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " & _
                            '        "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " & _
                            '        "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " & _
                            '        "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                    "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' "
                        Else
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                            '         & "FROM COST_CENTRE " _
                            '         & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and CC_STATUS = 'A' "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                     & "FROM COST_CENTRE " _
                                     & "WHERE CC_COY_ID='" & Common.Parse(compid) & "' and CC_STATUS = 'A' "

                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " & _
                            '         "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " & _
                            '         "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " & _
                            '         "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " & _
                            '         "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                     "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                     "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                     "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " &
                                     "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        Else
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                            '         & "FROM COST_CENTRE " _
                            '         & "WHERE cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " _
                            '         & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                     & "FROM COST_CENTRE " _
                                     & "WHERE cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " _
                                     & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        End If
                    End If

                End If
            Else
                If compid <> "" Then
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " & _
                            '        "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " & _
                            '        "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " & _
                            '        "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                    "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' "
                        Else
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                            '         & "FROM COST_CENTRE " _
                            '         & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and CC_STATUS = 'A' "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                       & "FROM COST_CENTRE " _
                                       & "WHERE CC_COY_ID='" & Common.Parse(compid) & "' and CC_STATUS = 'A' "
                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " & _
                            '         "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " & _
                            '         "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " & _
                            '         "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " & _
                            '         "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                      "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                      "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                      "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " &
                                      "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        Else
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                            '         & "FROM COST_CENTRE " _
                            '         & "WHERE cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " _
                            '         & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                     & "FROM COST_CENTRE " _
                                     & "WHERE cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " _
                                     & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        End If
                    End If
                Else
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " & _
                            '        "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " & _
                            '        "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " & _
                            '        "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                    "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' "
                        Else
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                            '         & "FROM COST_CENTRE " _
                            '         & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and CC_STATUS = 'A' "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                     & "FROM COST_CENTRE " _
                                     & "WHERE CC_COY_ID='" & Common.Parse(compid) & "' and CC_STATUS = 'A' "

                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " & _
                            '         "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " & _
                            '         "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " & _
                            '         "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " & _
                            '         "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                      "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                      "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                      "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " &
                                      "AND (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        Else
                            'strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                            '         & "FROM COST_CENTRE " _
                            '         & "WHERE cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " _
                            '         & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                            strsql = "SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                      & "FROM COST_CENTRE " _
                                      & "WHERE cc_coy_id='" & Common.Parse(compid) & "' AND cc_status = 'A' " _
                                      & "and (cc_cc_code like '%" & Common.Parse(strUserInput) & "%' or cc_cc_desc like '%" & Common.Parse(strUserInput) & "%') "
                        End If
                    End If
                End If
            End If


            ds = objDb.FillDs(strsql)
            getCostCentre = ds
        End Function
        Public Function getBranch(Optional ByVal compid As String = "", Optional ByVal strUserInput As String = "", Optional ByVal role As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'strsql = "SELECT  CONCAT(CB_BRANCH_CODE, "" : "" , CB_BRANCH_NAME) AS CB_BRANCH_CODE, CB_BRANCH_NAME " _
            '        & "FROM COMPANY_BRANCH " _
            '        & "WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            'strsql = "SELECT  CB_BRANCH_CODE, CB_BRANCH_NAME " _
            '      & "FROM COMPANY_BRANCH " _
            '      & "WHERE CB_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
            If role = "1" Then
                If compid <> "" Then
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                    "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' "
                        Else
                            strsql = "SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                   & "FROM company_branch_mstr " _
                                     & "WHERE cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' "
                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                    "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' " &
                                    "AND (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%') "
                        Else
                            strsql = "SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                           & "FROM company_branch_mstr " _
                                           & "WHERE cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' " _
                                          & "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')"
                        End If
                    End If
                Else
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                     "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                     "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                     "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' "
                        Else
                            strsql = "SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                        & "FROM company_branch_mstr " _
                                       & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' "
                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                        "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                        "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                        "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' " &
                                        "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')"
                        Else
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                   & "FROM company_branch_mstr " _
                                    & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' " _
                                    & "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')"
                        End If
                    End If

                End If
            Else
                If compid <> "" Then
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                   "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                   "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                   "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' "
                        Else
                            strsql = "SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                   & "FROM company_branch_mstr " _
                                     & "WHERE cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' "
                            'strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                            '                & "FROM company_branch_mstr " _
                            '               & "INNER JOIN ipp_usrgrp_branch ON iub_branch_code = cbm_branch_code " _
                            '              & "WHERE cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' "
                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                    "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                    "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                    "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' " &
                                    "AND (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%') "
                        Else
                            strsql = "SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                           & "FROM company_branch_mstr " _
                                           & "WHERE cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' " _
                                          & "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')"
                        End If
                        'strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                        '                     & "FROM company_branch_mstr " _
                        '                    & "INNER JOIN ipp_usrgrp_branch ON iub_branch_code = cbm_branch_code " _
                        '                   & "WHERE cbm_coy_id='" & Common.Parse(compid) & "' AND cbm_status = 'A' " _
                        '                    & "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')" ' for HLB Dept code only, to prevent GL Entry have NULL GL Code
                    End If
                Else
                    If strUserInput = "*" Then
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                     "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                     "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                     "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' "
                        Else
                            strsql = "SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                        & "FROM company_branch_mstr " _
                                       & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' "
                        End If
                    Else
                        If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code  FROM ipp_usrgrp_user " &
                                        "INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index " &
                                        "INNER JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code AND cbm_coy_id = iub_br_coy_id " &
                                        "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' " &
                                        "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')"
                        Else
                            strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                                   & "FROM company_branch_mstr " _
                                    & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' " _
                                    & "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')"
                        End If
                    End If
                    'If strUserInput = "*" Then
                    '    strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                    '                        & "FROM company_branch_mstr " _
                    '                        & "INNER JOIN ipp_usrgrp_branch ON iub_branch_code = cbm_branch_code " _
                    '                        & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' "
                    'Else
                    '    strsql = "SELECT CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                    '                          & "FROM company_branch_mstr " _
                    '                        & "INNER JOIN ipp_usrgrp_branch ON iub_branch_code = cbm_branch_code " _
                    '                    & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' " _
                    '                          & "and (cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' or cbm_branch_name like '%" & Common.Parse(strUserInput) & "%')" ' for HLB Dept code only, to prevent GL Entry have NULL GL Code
                    'End If

                End If
            End If


            ds = objDb.FillDs(strsql)
            getBranch = ds
        End Function
        Public Function InsertCostAllocDetail(ByVal aryCAD As ArrayList, ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String

            'If objDb.Exist("SELECT '*' FROM COST_ALLOC_DETAIL WHERE CBG_B_COY_ID = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "' AND CBG_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
            '    Return False
            '    Exit Function
            'End If

            For i = 0 To aryCAD.Count - 1
                If aryCAD.Item(i)(0) <> "" And aryCAD.Item(i)(2) <> "" Then
                    strSQL = " INSERT INTO COST_ALLOC_DETAIL(" &
                            "CAD_CAM_INDEX, CAD_CC_CODE," &
                            "CAD_BRANCH_CODE, CAD_PERCENT) " &
                            "VALUES('" & strCACode & "','" & Common.Parse(aryCAD.Item(i)(1)) & "'," &
                            "'" & Common.Parse(aryCAD.Item(i)(0)) & "','" & Common.Parse(aryCAD.Item(i)(2)) & "')"


                    objDb.Execute(strSQL)
                End If
            Next
            Return True

        End Function
        Public Function DelCostAllocDetail(ByVal dtCADetail As DataTable, ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND im_invoice_status = 10 and id_cost_alloc_code =  '" & strCACode & "'") > 0 Then
                Return False
                Exit Function
            End If

            For i = 0 To dtCADetail.Rows.Count - 1
                strSQL = " DELETE FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & Common.Parse(dtCADetail.Rows(i)("CADIDX")) & "' AND CAD_CC_CODE = '" & Common.Parse(dtCADetail.Rows(i)("CADCCCode")) & "' AND CAD_BRANCH_CODE = '" & Common.Parse(dtCADetail.Rows(i)("CADBrCode")) & "' "

                objDb.Execute(strSQL)
            Next
            Return True

        End Function
        Public Function GetLastCACode() As String
            Dim LastCACode As String
            Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

            LastCACode = objDb.Get1ColumnCheckNull("COST_ALLOC_MSTR", "CAM_INDEX", " WHERE cam_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND cam_user_id = '" & HttpContext.Current.Session("UserId") & "' ORDER BY CAM_INDEX DESC")
            Return LastCACode
        End Function
        Public Function CheckCADetail(ByVal strCACode As String) As Boolean
            Dim i As Integer
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM COST_ALLOC_DETAIL WHERE CAD_CAM_INDEX = '" & strCACode & "'") > 0 Then
                Return True
                'Exit Function
            Else
                Return False
            End If

        End Function
        ''''''''''''''''''' end of cost allocation IPP'''''''''''''''''''''

        '''''''''''''''''' GL Code IPP '''''''''''''''''''''''''''''''''''
        Public Function GetGLCode(ByRef strGLCode As String, ByRef strDesc As String, ByRef strStatus As String, Optional ByVal blnSortbyDesc As Boolean = False) As DataSet
            Dim ds As DataSet
            Dim strsql As String
            'Modified for IPP GST Stage 2A - CH (30/1/2015)
            Dim strCoyId As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strCoyId = "" Then
                strCoyId = HttpContext.Current.Session("CompanyId")
            End If

            strsql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC, CBG_CC_REQ, CBG_AG_REQ, CBG_STATUS FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & strCoyId & "' AND CBG_STATUS = '" & strStatus & "'"

            If strGLCode <> "" Then
                strsql &= " AND CBG_B_GL_CODE LIKE '%" & strGLCode & "%'"
            End If
            If strDesc <> "" Then
                strsql &= " AND CBG_B_GL_DESC Like '%" & strDesc & "%'"
            End If

            'Jules 2018.11.05
            If blnSortbyDesc Then
                strsql &= " ORDER BY CBG_B_GL_DESC"
            End If
            'End modification.

            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                strGLCode = ds.Tables(0).Rows(0).Item("CBG_B_GL_CODE")
                strDesc = ds.Tables(0).Rows(0).Item("CBG_B_GL_DESC")
                strStatus = ds.Tables(0).Rows(0).Item("CBG_STATUS")
            Else
                strGLCode = "GL Code"
                strDesc = "GL Code Description"
                strStatus = "Status"
            End If
            Return ds
        End Function
        Public Function GetSelectedGLCode(ByVal strGLCode As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC, CBG_CC_REQ, CBG_AG_REQ, CBG_STATUS FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBG_B_GL_CODE = '" & Common.Parse(strGLCode) & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function DelGLCode(ByVal dtGLCode As DataTable) As Boolean
            Dim i, j As Integer
            Dim strSQL As String
            'Modified for IPP GST Stage 2A - CH (30/1/2015)
            Dim strChkCoyId As String = ConfigurationManager.AppSettings("ChkIPPCompID")
            Dim strCoyId() As String
            For i = 0 To dtGLCode.Rows.Count - 1
                'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND ID_B_GL_CODE =  '" & Common.Parse(dtGLCode.Rows(i)("index")) & "'") > 0 Then
                '    Return False
                '    Exit Function
                'End If

                'strSQL = " DELETE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = '" & Common.Parse(dtGLCode.Rows(i)("index")) & "'"
                'objDb.Execute(strSQL)

                'Chee Hong 18/12/2014 - Need to check pending transaction from HLISB too (IPP - GST Stage 2A)
                If strChkCoyId <> "" Then
                    strCoyId = strChkCoyId.Split(",")
                    For j = 0 To strCoyId.Length - 1
                        If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & strCoyId(j) & "' AND ID_B_GL_CODE =  '" & Common.Parse(dtGLCode.Rows(i)("index")) & "'") > 0 Then
                            Return False
                            Exit Function
                        End If
                    Next
                Else
                    If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND ID_B_GL_CODE =  '" & Common.Parse(dtGLCode.Rows(i)("index")) & "'") > 0 Then
                        Return False
                        Exit Function
                    End If
                End If

                strSQL = "DELETE FROM COMPANY_B_GL_CODE WHERE CBG_B_GL_CODE = '" & Common.Parse(dtGLCode.Rows(i)("index")) & "' AND CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                objDb.Execute(strSQL)
            Next
            Return True

        End Function

        Public Function UpdateGLCode(ByVal dsGLCode As DataSet) As Boolean
            Dim i As Integer
            Dim Query(0) As String
            Dim strSQL As String
            Dim strChkCoyId As String = ConfigurationManager.AppSettings("ChkIPPCompID")
            Dim strCoyId() As String

            'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND ID_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
            '    Return False
            '    Exit Function
            'End If

            'If objDb.Exist("SELECT * FROM invoice_mstr INNER JOIN invoice_details_alloc ON ida_invoice_index = im_invoice_index  WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND ida_branch_code  =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
            '    Return False
            '    Exit Function
            'End If
            'Chee Hong 18/12/2014 - Need to check pending transaction from HLISB too (IPP - GST Stage 2A)
            If strChkCoyId <> "" Then
                strCoyId = strChkCoyId.Split(",")
                For i = 0 To strCoyId.Length - 1
                    If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & strCoyId(i) & "'  AND im_invoice_status = 10 AND ID_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
                        Return False
                        Exit Function
                    End If

                    If objDb.Exist("SELECT * FROM invoice_mstr INNER JOIN invoice_details_alloc ON ida_invoice_index = im_invoice_index  WHERE im_b_coy_id = '" & strCoyId(i) & "'  AND im_invoice_status = 10 AND ida_branch_code  =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
                        Return False
                        Exit Function
                    End If
                Next
            Else
                If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND ID_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
                    Return False
                    Exit Function
                End If

                If objDb.Exist("SELECT * FROM invoice_mstr INNER JOIN invoice_details_alloc ON ida_invoice_index = im_invoice_index  WHERE im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'  AND im_invoice_status = 10 AND ida_branch_code  =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
                    Return False
                    Exit Function
                End If
            End If

            strSQL = "UPDATE COMPANY_B_GL_CODE SET " &
                  "CBG_B_GL_DESC='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLDesc")) & "'," &
                  "CBG_CC_REQ='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCCReq")) & "'," &
                  "CBG_AG_REQ='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLAGReq")) & "'," &
                  "CBG_STATUS='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLStatus")) & "'," &
                  "CBG_MOD_BY='" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLModBy")) & "'," &
                  "CBG_MOD_DATETIME=" & Common.ConvertDate(dsGLCode.Tables(0).Rows(0)("GLModDate")) & "" &
                  " WHERE CBG_B_GL_CODE = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "' AND CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

            objDb.Execute(strSQL)

            Return True

        End Function
        Public Function InsertGLCode(ByVal dsGLCode As DataSet) As Boolean
            Dim Query(0) As String
            Dim strSQL As String

            If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "' AND CBG_B_GL_CODE =  '" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "'") > 0 Then
                Return False
                Exit Function
            End If

            strSQL = " INSERT INTO COMPANY_B_GL_CODE(" &
                    "CBG_B_COY_ID,CBG_B_GL_CODE,CBG_B_GL_DESC," &
                    "CBG_CC_REQ,CBG_AG_REQ,CBG_STATUS," &
                    "CBG_ENT_BY,CBG_ENT_DATETIME) " &
                    "VALUES('" & Common.Parse(dsGLCode.Tables(0).Rows(0)("CompID")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCode")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLDesc")) & "'," &
                    "'" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLCCReq")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLAGReq")) & "','" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLStatus")) & "'," &
                     "'" & Common.Parse(dsGLCode.Tables(0).Rows(0)("GLEntBy")) & "'," & Common.ConvertDate(dsGLCode.Tables(0).Rows(0)("GLEntDate")) & ")"


            objDb.Execute(strSQL)
            Return True

        End Function
        '''''''''''''''''' end of gl code IPP '''''''''''''''''''''''''''' 

        '''''''''''''''''' Param IPP ''''''''''''''''''''''''''''''''''''
        Public Function GetIPPParam() As DataSet
            Dim ds As DataSet
            Dim strsql As String

            If objDb.Exist("SELECT '*' FROM IPP_PARAMETER WHERE IP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'") > 0 Then
                strsql = "SELECT IP_PARAM, IP_PARAM_DESC, IP_PARAM_VALUE FROM IPP_PARAMETER WHERE IP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' ORDER BY IP_INDEX"
            Else
                strsql = "SELECT IP_PARAM, IP_PARAM_DESC, IP_PARAM_VALUE FROM IPP_PARAMETER WHERE IP_COY_ID = '' ORDER BY IP_INDEX"
            End If



            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetIPPValue(ByVal strIPP_Param As String)
            Dim strsql, strBCode As String

            If objDb.Exist("SELECT '*' FROM IPP_PARAMETER WHERE IP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND ip_param = '" & strIPP_Param & "' AND ip_param_value <> ''") > 0 Then
                strsql = "SELECT IP_PARAM_VALUE FROM IPP_PARAMETER WHERE IP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND ip_param = '" & strIPP_Param & "' ORDER BY IP_INDEX"
            Else
                Return False
            End If

            strBCode = objDb.GetVal(strsql)
            Return strBCode
        End Function

        Public Function GetInd(ByVal strCoyName As String, ByVal strCoyId As String, Optional ByVal CoyRegNo As String = "", Optional ByVal CoyType As String = "")
            'get index num
            Dim strSql As String
            Dim strInd As Integer

            If CoyType = "E" Then
                strSql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_ID = '" & Common.Parse(strCoyId) & "' and IC_BUSINESS_REG_NO = '" & Common.Parse(CoyRegNo) & "' and ic_coy_type = 'E'"
            ElseIf CoyType = "V" Then

                strSql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_ID = '" & Common.Parse(strCoyId) & "' AND IC_COY_NAME = '" & Common.Parse(strCoyName) & "' and ic_coy_type = 'V'"
            ElseIf CoyType = "B" Then

                strSql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_ID = '" & Common.Parse(strCoyId) & "' AND IC_COY_NAME = '" & Common.Parse(strCoyName) & "' and ic_coy_type = 'B'"
            End If


            strInd = objDb.GetVal(strSql)

            Return strInd
        End Function

        Public Function UpdateIPPParam(ByVal htPageAccess As Hashtable) As Boolean

            Dim myEnumerator As IDictionaryEnumerator = htPageAccess.GetEnumerator()

            Dim objDb As New EAD.DBCom
            Dim strUpdate As String
            Dim strValue As String

            Dim strIPParam As String
            Dim strIPDesc As String
            Dim strAryQuery(0) As String
            Dim strsql As String

            While myEnumerator.MoveNext()
                If myEnumerator.Key = "" Then
                    Exit While
                End If
                strValue = myEnumerator.Value
                strIPParam = myEnumerator.Key.Trim

                strsql = "SELECT DISTINCT(IP_PARAM_DESC) FROM IPP_PARAMETER WHERE IP_PARAM = '" & strIPParam & "'"
                strIPDesc = objDb.GetVal(strsql)

                If objDb.Exist("SELECT '*' FROM IPP_PARAMETER WHERE IP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IP_PARAM = '" & strIPParam & "'") > 0 Then
                    strUpdate = "UPDATE IPP_PARAMETER SET IP_PARAM_VALUE = '" & Common.Parse(strValue) & "', IP_MOD_BY = '" & HttpContext.Current.Session("UserId") & "',IP_MOD_DATETIME = NOW() "
                    strUpdate &= "WHERE IP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strUpdate &= "AND IP_PARAM = '" & Common.Parse(strIPParam) & "'"
                Else
                    strUpdate = "INSERT INTO IPP_PARAMETER(IP_COY_ID,IP_PARAM, IP_PARAM_DESC, IP_PARAM_VALUE, IP_ENT_BY, IP_ENT_DATETIME) VALUES("
                    strUpdate &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                    strUpdate &= "'" & strIPParam & "', "
                    strUpdate &= "'" & Common.Parse(strIPDesc) & "', "
                    strUpdate &= "'" & Common.Parse(strValue) & "', "
                    strUpdate &= "'" & HttpContext.Current.Session("UserId") & "', "
                    strUpdate &= "NOW())"
                End If


                objDb.Execute(strUpdate)
            End While


            Return True
            'objDb.BatchExecute(strAryQuery)
        End Function
        '''''''''''''''''''''''end Param IPP ''''''''''''''''''''''''''''


        Public Function Message(ByVal pg As System.Web.UI.Page, ByVal MsgID As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Invoice Payment")
            Dim strSQL As String
            Dim strMsg As String

            strSQL = "SELECT MM_MESSAGE FROM MESSAGE_MSTR WHERE MM_CODE = '" & MsgID & "'"
            strMsg = objDb.GetVal(strSQL)

            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            vbs = vbs & "Call MsgBox(""" & strMsg & """, " & style & ", """ & title & """)"
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            pg.RegisterStartupScript(rndKey.Next.ToString, vbs)

        End Function


        Public Function GetRecoveryList() As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT frt_index,CAST(frt_gl_date as DATE) as frt_gl_date,COUNT(*) AS Number_of_Record, " &
                     "SUM(CASE WHEN gl_dorc = 'D' THEN CAST(gl_amt AS DECIMAL(30,2)) ELSE 0 END ) AS Total_Debit, " &
                     "SUM(CASE WHEN gl_dorc = 'C' THEN CAST(gl_amt AS DECIMAL(30,2))  ELSE 0 END) AS Total_Credit " &
                     "FROM file_recovery_trans " &
                     "INNER JOIN gl_entry ON CAST(gl_gl_date as DATE) = CAST(frt_gl_date AS DATE) " &
                     "WHERE frt_posted_ind = 'P' GROUP BY frt_gl_date"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function InsertRecoveryList(ByVal PaymentDate As String) As Integer
            Dim Query(0) As String
            Dim strSQL As String


            If objDb.Exist("SELECT '*' FROM file_recovery_trans WHERE CAST(frt_gl_date AS DATE) =  " & PaymentDate & " AND frt_posted_ind <> 'D'") > 0 Then
                Return WheelMsgNum.Duplicate
                Exit Function
            End If

            If objDb.Exist("SELECT DISTINCT '*' FROM gl_entry WHERE CAST(gl_gl_date AS DATE) =  " & PaymentDate & "") > 0 Then
                strSQL = " INSERT INTO file_recovery_trans(" &
                     "frt_coy_id,frt_gl_date," &
                     "frt_posted_ind,frt_posted_date,frt_ent_dt," &
                     "frt_mod_dt) " &
                     "VALUES('" & HttpContext.Current.Session("CompanyID") & "'," & PaymentDate & ",'P'," &
                     "NULL,NOW(),NOW())"

                objDb.Execute(strSQL)
            Else
                Return WheelMsgNum.NotSave
            End If


            'Return True

        End Function

        Public Function DeleteRecoveryList(ByVal LineNo As String)
            Dim Query(0) As String
            Dim strSQL As String


            strSQL = "DELETE FROM file_recovery_trans WHERE frt_index = '" & LineNo & "'"

            objDb.Execute(strSQL)

        End Function

        Public Function ConfirmRecovery(ByVal aryIdx As ArrayList) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim i As Integer

            For i = 0 To aryIdx.Count - 1
                strSQL = " UPDATE file_recovery_trans SET frt_posted_ind = 'C',frt_mod_dt = NOW() WHERE frt_index ='" & aryIdx(i) & "'"

                Common.Insert2Ary(strAry, strSQL)

            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Save
                'Else
                '    Return WheelMsgNum.NotSave
            End If
        End Function
        Public Function GetRecoveryConfirmList(ByVal paySdate As String, ByVal payEdate As String, ByVal revSdate As String, ByVal revEdate As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT frt_index,CAST(frt_gl_date as DATE) as frt_gl_date, " &
                    "IF(frt_posted_date IS NULL, NULL ,CAST(frt_posted_date AS DATE)) AS frt_posted_date, " &
                    "COUNT(*) AS Number_of_Record, " &
                     "SUM(CASE WHEN gl_dorc = 'D' THEN CAST(gl_amt AS DECIMAL(30,2)) ELSE 0 END ) AS Total_Debit, " &
                     "SUM(CASE WHEN gl_dorc = 'C' THEN CAST(gl_amt AS DECIMAL(30,2))  ELSE 0 END) AS Total_Credit " &
                     "FROM file_recovery_trans " &
                     "INNER JOIN gl_entry ON CAST(gl_gl_date as DATE) = CAST(frt_gl_date as DATE) " &
                     "WHERE frt_posted_ind <> 'P' "

            If paySdate <> "" Then
                strsql &= " AND CAST(frt_gl_date as DATE) >= " & Common.ConvertDate(paySdate) & ""
            End If
            If payEdate <> "" Then
                strsql &= " AND CAST(frt_gl_date as DATE) <= " & Common.ConvertDate(payEdate) & ""
            End If
            If revSdate <> "" Then
                strsql &= " AND CAST(frt_posted_date as DATE) >= " & Common.ConvertDate(revSdate) & ""
            End If
            If revEdate <> "" Then
                strsql &= " AND CAST(frt_posted_date as DATE) <= " & Common.ConvertDate(revEdate) & ""
            End If

            strsql &= " GROUP BY frt_gl_date,frt_index ORDER BY frt_posted_ind,frt_gl_date "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function sendRecoveryConfirmMail(ByVal dsRecoveryList As DataSet, ByVal strEmail As String)
            Dim strsql, strcond, strAO As String
            Dim blnRelief As String
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim dsAO As New DataSet
            Dim i, j As Integer
            Dim intApprGrpIdx As Integer
            Dim dsApprGrp As New DataSet


            strBody &= "<P>You have a Daily GL Entry File Recovery waiting for confirmation(s) and processing as follows: " ' & strDocType & " (" & Common.parseNull(dsRecoveryList.Tables(0).Rows(0)("DocNo")) & ") <BR>"


            For i = 0 To dsRecoveryList.Tables(0).Rows.Count - 1
                strBody &= "<P>Payment Date : " & Format(Common.parseNull(dsRecoveryList.Tables(0).Rows(i)("frt_gl_date")), "d/M/yyyy") & " , "
                strBody &= "No. of record : " & Common.parseNull(dsRecoveryList.Tables(0).Rows(i)("Number_of_Record")) & " , "
                strBody &= "Total Debit : " & Format(Common.parseNull(dsRecoveryList.Tables(0).Rows(i)("Total_Debit")), "#,###.00") & " , "
                strBody &= "Total Credit : " & Format(Common.parseNull(dsRecoveryList.Tables(0).Rows(i)("Total_Credit")), "#,###.00") & " <BR>"
            Next

            strBody &= "<P>Confirmation MUST be replied via email to "
            strBody &= ConfigurationSettings.AppSettings("HubAdminEmail") & " before 3.00PM TODAY to ensure successful processing of GL File Recovery data stated above.  No processing can be made if the email response is received after 3.00PM. <BR> <P>Thank you for your cooperation."
            strBody &= "<P>" & objCommon.EmailFooter '& Common.EmailCompGen


            Dim objMail As New AppMail

            objMail.MailTo = Common.parseNull(strEmail)
            objMail.MailCc = ConfigurationSettings.AppSettings("HubAdminEmail")  ' EmailHub 'ConfigurationManager.AppSettings("EmailToHubAdmin")
            objMail.Body = "Dear Sir/Madam, <BR>" & strBody ' & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody

            objMail.Subject = "IPP Daily GL Entry Recovery Confirmation" ' & Common.parseNull(dsRecoveryList.Tables(0).Rows(0)("DocNo")) & " Created"
            objMail.SendMail()

            objCommon = Nothing
        End Function
        Public Function GetHoliday(ByVal strYear As String, ByVal strCountry As String, ByVal strState As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT hm_index,hm_date,DATE_FORMAT(hm_date,'%W') AS hm_day,hm_desc FROM holiday_mstr where hm_year = '" & strYear & "'  AND hm_country = '" & strCountry & "' AND IFNULL(hm_state,'') = '" & strState & "'"


            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function GetSelectedHoliday(ByVal intHolidayIndex As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT hm_index,hm_date,DATE_FORMAT(hm_date,'%W') AS hm_day,hm_desc FROM holiday_mstr where hm_index = '" & intHolidayIndex & "' "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function UpdateHoliday(ByVal aryHoliday As ArrayList, ByVal strHolidayIndex As String, ByVal strYear As String, ByVal strCountry As String, ByVal strState As String)
            Dim i As Integer
            Dim strSQL As String

            strSQL = "UPDATE holiday_mstr SET hm_year='" & strYear & "', " &
            "hm_date=" & Common.ConvertDate(aryHoliday.Item(0)(1)) & ", " &
            "hm_desc='" & Common.Parse(aryHoliday.Item(0)(2)) & "', " &
            "hm_country='" & strCountry & "', " &
            "hm_state='" & strState & "', " &
            "hm_mod_dt=NOW() " &
            "WHERE hm_index='" & strHolidayIndex & "'"

            objDb.Execute(strSQL)

        End Function

        Public Function SaveHoliday(ByVal aryHoliday As ArrayList, ByVal strYear As String, ByVal strCountry As String, ByVal strState As String)
            Dim i As Integer
            Dim strSQL, strAryQuery(0) As String

            For i = 0 To aryHoliday.Count - 1

                If aryHoliday.Item(i)(1) <> "" Or aryHoliday.Item(i)(2) <> "" Then
                    strSQL = " INSERT INTO holiday_mstr(" &
                    "hm_year, hm_date, hm_desc, hm_country, hm_state, hm_ent_dt,hm_mod_dt) " &
                    "VALUES('" & Common.ConvertDate(aryHoliday.Item(i)(1)).Substring(1, 4) & "'," &
                    "" & Common.ConvertDate(aryHoliday.Item(i)(1)) & "," &
                    "'" & Common.Parse(aryHoliday.Item(i)(2)) & "'," &
                    "'" & strCountry & "'," &
                    "'" & strState & "'," &
                    " NOW()," &
                    " NOW())"

                    Common.Insert2Ary(strAryQuery, strSQL)
                End If

            Next
            objDb.BatchExecute(strAryQuery)
        End Function
        Public Function DelHoliday(ByVal strHolidayIndex As String) As Boolean
            Dim strsql, strInvIndex As String


            strsql = "DELETE FROM holiday_mstr WHERE hm_index = '" & strHolidayIndex & "' "

            If objDb.Execute(strsql) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function GetIPPUserGroup(Optional ByVal strUserGroup As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            If strUserGroup <> "" Then
                strsql = "SELECT IUM_GRP_INDEX,IUM_GRP_NAME FROM ipp_usrgrp_mstr " &
                                   "LEFT JOIN ipp_usrgrp_branch ON IUB_GRP_INDEX = IUM_GRP_INDEX " &
                                   "LEFT JOIN ipp_usrgrp_cc ON IUC_GRP_INDEX = IUM_GRP_INDEX " &
                                   "LEFT JOIN ipp_usrgrp_user ON IUU_GRP_INDEX = IUM_GRP_INDEX " &
                                   "LEFT JOIN user_mstr ON um_user_id = IUU_USER_ID " &
                                   "WHERE IUM_GRP_NAME like '%" & strUserGroup & "%' AND IUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                                      "GROUP BY ium_grp_index"
            Else
                strsql = "SELECT IUM_GRP_INDEX,IUM_GRP_NAME FROM ipp_usrgrp_mstr " &
                   "LEFT JOIN ipp_usrgrp_branch ON IUB_GRP_INDEX = IUM_GRP_INDEX " &
                   "LEFT JOIN ipp_usrgrp_cc ON IUC_GRP_INDEX = IUM_GRP_INDEX " &
                   "LEFT JOIN ipp_usrgrp_user ON IUU_GRP_INDEX = IUM_GRP_INDEX " &
                   "LEFT JOIN user_mstr ON um_user_id = IUU_USER_ID " &
                   "WHERE IUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                   "GROUP BY ium_grp_index"
            End If



            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetIPPUserGroup2(Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'strsql = "SELECT IUB_BRANCH_CODE FROM ipp_usrgrp_mstr " & _
            '         "LEFT JOIN ipp_usrgrp_branch ON IUB_GRP_INDEX = IUM_GRP_INDEX " & _
            '         "where IUM_GRP_INDEX = '" & strUserGroupIndex & "' "

            strsql = "SELECT IUB_BRANCH_CODE,IUB_BR_COY_ID,cbm_branch_name FROM ipp_usrgrp_mstr " &
                    "LEFT JOIN ipp_usrgrp_branch ON IUB_GRP_INDEX = IUM_GRP_INDEX " &
                    "LEFT JOIN company_branch_mstr ON iub_branch_code = cbm_branch_code and iub_br_coy_id = cbm_coy_id " &
                    "WHERE IUM_GRP_INDEX = '" & strUserGroupIndex & "'" 'AND cbm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetIPPUserGroup3(Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'strsql = "SELECT IUC_CC_CODE FROM ipp_usrgrp_mstr " & _
            '         "LEFT JOIN ipp_usrgrp_cc ON IUC_GRP_INDEX = IUM_GRP_INDEX " & _
            '         "where IUM_GRP_INDEX = '" & strUserGroupIndex & "' "

            strsql = "SELECT IUC_CC_CODE, cc_cc_desc FROM ipp_usrgrp_mstr " &
                    " LEFT JOIN ipp_usrgrp_cc ON IUC_GRP_INDEX = IUM_GRP_INDEX " &
                    " LEFT JOIN cost_centre ON cc_cc_code = iuc_cc_code " &
                    " WHERE IUM_GRP_INDEX = '" & strUserGroupIndex & "' AND cc_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetIPPUserGroup4(Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            strsql = "SELECT um_user_name ,UM_USER_ID FROM ipp_usrgrp_mstr " &
                     "LEFT JOIN ipp_usrgrp_user ON IUU_GRP_INDEX = IUM_GRP_INDEX " &
                     "LEFT JOIN user_mstr ON um_user_id = IUU_USER_ID and um_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' where IUM_GRP_INDEX = '" & strUserGroupIndex & "' "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Function AddIPPUserGroup(ByVal strValGroup As String, ByVal strMode As String)
            Dim strCoyId, strUserID, strSQL, strSqlAry(0) As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            strSQL = "SELECT * FROM ipp_usrgrp_mstr WHERE ium_grp_name = '" & strValGroup & "' AND ium_coy_id = '" & strCoyId & "'"
            If objDb.Exist(strSQL) > 0 Then
                AddIPPUserGroup = WheelMsgNum.Duplicate
            Else
                If strMode = "add" Then
                    strSQL = "INSERT INTO ipp_usrgrp_mstr(IUM_GRP_NAME, IUM_COY_ID, IUM_ENT_BY, IUM_ENT_DATETIME) VALUES('" & Common.Parse(strValGroup) & "','" & Common.Parse(strCoyId) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ")"
                Else
                    strSQL = "INSERT INTO ipp_usrgrp_mstr(IUM_GRP_NAME, IUM_COY_ID, IUM_MOD_BY, IUM_MOD_DATETIME) VALUES('" & Common.Parse(strValGroup) & "','" & Common.Parse(strCoyId) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ")"
                End If

                Common.Insert2Ary(strSqlAry, strSQL)
                If strSqlAry(0) <> String.Empty Then
                    objDb.BatchExecute(strSqlAry)
                    AddIPPUserGroup = WheelMsgNum.Save
                End If
            End If
        End Function

        Function UpdateIPPUserGroup(ByVal strlistindex As String, ByVal strValGroup As String, ByVal strMode As String, Optional ByVal strOld As String = "")
            Dim strsql, strUserID, strCoyId As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            If UCase(strOld) <> UCase(strValGroup) Then
                If objDb.Exist("SELECT * FROM ipp_usrgrp_mstr WHERE ium_grp_name = '" & strValGroup & "' AND ium_coy_id = '" & strCoyId & "'") > 0 Then
                    UpdateIPPUserGroup = WheelMsgNum.Duplicate
                    Exit Function
                End If

            End If
            strsql = "update ipp_usrgrp_mstr set "
            strsql &= "IUM_GRP_NAME = '" & Common.Parse(strValGroup) & "',IUM_MOD_BY='" & strUserID & "', IUM_MOD_DATETIME=" & Common.ConvertDate(Now) & " "
            strsql &= "WHERE IUM_GRP_INDEX ='" & Common.Parse(strlistindex) & "' "

            If objDb.Execute(strsql) Then
                UpdateIPPUserGroup = WheelMsgNum.Save
            Else
                UpdateIPPUserGroup = WheelMsgNum.NotSave
            End If
        End Function
        Function DelIPPUserGroup(ByVal strIndex As String)
            Dim strdelFav As String
            Dim strAryQuery(0) As String

            strdelFav = "Delete from ipp_usrgrp_user where IUU_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)
            strdelFav = "Delete from ipp_usrgrp_cc where IUC_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)
            strdelFav = "Delete from ipp_usrgrp_branch where IUB_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)
            strdelFav = "Delete from ipp_usrgrp_mstr where IUM_GRP_INDEX = '" & strIndex & "' "
            Common.Insert2Ary(strAryQuery, strdelFav)

            If objDb.BatchExecute(strAryQuery) Then
                DelIPPUserGroup = WheelMsgNum.Delete
            Else
                DelIPPUserGroup = WheelMsgNum.NotDelete
            End If
        End Function
        Public Function GetIPPUserGroupBranch(Optional ByVal strUserGroupIndex As String = "", Optional ByVal strBranchCode As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String


            'strsql = "SELECT IUB_BRANCH_CODE,CDM_DEPT_INDEX " & _
            '                  "FROM(ipp_usrgrp_branch) " & _
            '                  "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUB_GRP_INDEX " & _
            '                  "LEFT JOIN company_dept_mstr ON cdm_dept_code = IUB_BRANCH_CODE " & _
            '                  "where cdm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' "

            'strsql = "SELECT IUB_BRANCH_CODE,IUB_BR_COY_ID,cbm_branch_name,CBM_BRANCH_INDEX " & _
            '        "FROM ipp_usrgrp_branch " & _
            '         "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUB_GRP_INDEX " & _
            '        "LEFT JOIN company_branch_mstr ON cbm_branch_code = IUB_BRANCH_CODE  AND cbm_coy_id = iub_br_coy_id " & _
            '        "WHERE cbm_status = 'A' "

            'Modified for IPP Gst Stage 2A - CH - 6 Feb 2015
            strsql = "SELECT IUB_BRANCH_CODE,IUB_BR_COY_ID,cbm_branch_name,CBM_BRANCH_INDEX " &
                    "FROM ipp_usrgrp_branch " &
                     "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUB_GRP_INDEX " &
                    "LEFT JOIN company_branch_mstr ON cbm_branch_code = IUB_BRANCH_CODE  AND cbm_coy_id = iub_br_coy_id " &
                    "WHERE cbm_status = 'A' AND IUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strBranchCode <> "" Then
                strsql &= "and CBM_BRANCH_INDEX = '" & strBranchCode & "' "
            End If

            If strUserGroupIndex <> "" Then
                strsql &= "and IUB_GRP_INDEX = '" & strUserGroupIndex & "' "
            End If

            strsql &= "GROUP BY CBM_BRANCH_INDEX,IUB_BR_COY_ID "

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetIPPUserGroupBranch2(Optional ByVal strUserGroupIndex As String = "", Optional ByVal strBranchCodeIndex As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'If strUserGroupIndex = "" Then
            '    strsql = "SELECT IUM_GRP_NAME " & _
            '              "FROM(ipp_usrgrp_branch) " & _
            '              "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUB_GRP_INDEX " & _
            '              "LEFT JOIN company_branch_mstr ON cbm_branch_code = IUB_BRANCH_CODE " & _
            '              "WHERE CBM_BRANCH_INDEX = '" & strBranchCodeIndex & "' " & _
            '              "GROUP BY IUB_GRP_INDEX "
            'Else
            '    strsql = "SELECT IUM_GRP_NAME " & _
            '       "FROM(ipp_usrgrp_branch) " & _
            '       "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUB_GRP_INDEX " & _
            '       "LEFT JOIN company_branch_mstr ON cbm_branch_code = IUB_BRANCH_CODE " & _
            '       "where IUB_GRP_INDEX = '" & strUserGroupIndex & "' AND " & _
            '        "CBM_BRANCH_INDEX = '" & strBranchCodeIndex & "' " & _
            '        "GROUP BY IUB_GRP_INDEX "
            'End If

            'Modified for IPP GST Stage 2A - CH - 6 Feb 2015
            If strUserGroupIndex = "" Then
                strsql = "SELECT IUM_GRP_NAME " &
                        "FROM IPP_USRGRP_BRANCH " &
                        "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUB_GRP_INDEX " &
                        "LEFT JOIN COMPANY_BRANCH_MSTR ON CBM_BRANCH_CODE = IUB_BRANCH_CODE " &
                        "WHERE CBM_BRANCH_INDEX = '" & strBranchCodeIndex & "' " &
                        "AND IUM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
                        "GROUP BY IUB_GRP_INDEX "
            Else
                strsql = "SELECT IUM_GRP_NAME " &
                        "FROM IPP_USRGRP_BRANCH " &
                        "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUB_GRP_INDEX " &
                        "LEFT JOIN COMPANY_BRANCH_MSTR ON CBM_BRANCH_CODE = IUB_BRANCH_CODE " &
                        "WHERE IUB_GRP_INDEX = '" & strUserGroupIndex & "' AND " &
                        "CBM_BRANCH_INDEX = '" & strBranchCodeIndex & "' " &
                        "AND IUM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " &
                        "GROUP BY IUB_GRP_INDEX "
            End If

            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Public Function getUserGroupTypeAhead(Optional ByVal compid As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT IUM_GRP_INDEX, IUM_GRP_NAME " _
              & "FROM ipp_usrgrp_mstr " _
              & "WHERE IUM_ENT_BY = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND IUM_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

            ds = objDb.FillDs(strsql)
            getUserGroupTypeAhead = ds
        End Function
        Public Function getBranchTypeAhead(Optional ByVal strForm As String = "", Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If strForm = "IPPUsrGrpBranchAdd" Then
                strsql = "SELECT  CDM_DEPT_INDEX,CDM_DEPT_CODE, CDM_DEPT_NAME " _
                           & "FROM COMPANY_DEPT_MSTR " _
                           & "WHERE CDM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_DELETED = 'N' " _
                           & "AND LEFT(cdm_dept_code,2) IN ('HO','BR') AND LOCATE('-',cdm_dept_code,3) AND LOCATE('-',cdm_dept_code,7)  AND LENGTH(cdm_dept_code) = '10' " _
                           & "AND cdm_dept_code NOT IN ( SELECT IUB_BRANCH_CODE FROM ipp_usrgrp_branch  where IUB_GRP_INDEX = '" & strUserGroupIndex & "')" ' for HLB Dept code only, to prevent GL Entry have NULL GL Code

            Else
                strsql = "SELECT  CDM_DEPT_INDEX,CDM_DEPT_CODE, CDM_DEPT_NAME " _
                           & "FROM COMPANY_DEPT_MSTR " _
                           & "WHERE CDM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_DELETED = 'N' " _
                           & "AND LEFT(cdm_dept_code,2) IN ('HO','BR') AND LOCATE('-',cdm_dept_code,3) AND LOCATE('-',cdm_dept_code,7)  AND LENGTH(cdm_dept_code) = '10' " ' for HLB Dept code only, to prevent GL Entry have NULL GL Code
            End If

            ds = objDb.FillDs(strsql)
            getBranchTypeAhead = ds
        End Function
        'Modified for IPP GST Stage 2A - CH (30/1/2015)
        Public Function getBranchCodeTypeAhead(Optional ByVal strForm As String = "", Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT CBM_BRANCH_CODE FROM COMPANY_BRANCH_MSTR " &
                    "WHERE CBM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CBM_STATUS = 'A' "

            ds = objDb.FillDs(strsql)
            getBranchCodeTypeAhead = ds
        End Function
        '------------------------------------------
        Function AddIPPUserGroupBranch(ByVal strValGroup As String, ByVal strMode As String)
            Dim strCoyId, strUserID, strSQL, strSqlAry(0) As String
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            'strSQL = "SELECT * FROM ipp_usrgrp_mstr WHERE ium_grp_name = '" & strValGroup & "' AND ium_coy_id = '" & strCoyId & "'"
            'If objDb.Exist(strSQL) > 0 Then
            '    AddIPPUserGroupBranch = WheelMsgNum.Duplicate
            'Else
            If strMode = "add" Then
                strSQL = "INSERT INTO ipp_usrgrp_mstr(IUM_GRP_NAME, IUM_COY_ID, IUM_ENT_BY, IUM_ENT_DATETIME) VALUES('" & Common.Parse(strValGroup) & "','" & Common.Parse(strCoyId) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ")"
            Else
                strSQL = "INSERT INTO ipp_usrgrp_mstr(IUM_GRP_NAME, IUM_COY_ID, IUM_MOD_BY, IUM_MOD_DATETIME) VALUES('" & Common.Parse(strValGroup) & "','" & Common.Parse(strCoyId) & "','" & strUserID & "'," & Common.ConvertDate(Now) & ")"
            End If

            Common.Insert2Ary(strSqlAry, strSQL)
            If strSqlAry(0) <> String.Empty Then
                objDb.BatchExecute(strSqlAry)
                AddIPPUserGroupBranch = WheelMsgNum.Save
            End If
            'End If
        End Function

        Public Function getIPPUserGroupBranchList(ByVal compid As String, ByVal strUserInput As String, ByVal strUserInput2 As String, ByVal strUserGroupIndex As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            'strsql = "SELECT  CDM_DEPT_CODE, CDM_DEPT_NAME " _
            '                    & "FROM COMPANY_DEPT_MSTR " _
            '                    & "WHERE CDM_COY_ID='" & Common.Parse(compid) & "' AND CDM_DELETED = 'N' " _
            '                    & "AND LEFT(cdm_dept_code,2) IN ('HO','BR') AND LOCATE('-',cdm_dept_code,3) AND LOCATE('-',cdm_dept_code,7)  AND LENGTH(cdm_dept_code) = '10' " _
            '                    & "AND cdm_dept_code NOT IN ( SELECT IUB_BRANCH_CODE FROM ipp_usrgrp_branch where IUB_GRP_INDEX = '" & strUserGroupIndex & "') " ' for HLB Dept code only, to prevent GL Entry have NULL GL Code

            'Modified by CH on 4 Feb 2015 for IPP Stage 2A issue
            strsql = "SELECT cbm_branch_code,cbm_branch_name,cbm_coy_id " _
                        & "FROM company_branch_mstr " _
                        & "WHERE (cbm_coy_id = '" & Common.Parse(compid) & "' OR cbm_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' OR cbm_coy_id = '" & HttpContext.Current.Session("CompanyID") & "') AND cbm_status = 'A' " _
                        & "AND cbm_branch_type IN ('HO','BR') " _
                        & "AND cbm_branch_code NOT IN ( SELECT IUB_BRANCH_CODE FROM ipp_usrgrp_branch  " _
                        & "WHERE IUB_GRP_INDEX = '" & strUserGroupIndex & "' AND IUB_BR_COY_ID = cbm_coy_ID AND IUB_BRANCH_CODE = cbm_branch_Code) "

            If strUserInput <> "" Then
                strsql &= "and cbm_branch_code like '%" & Common.Parse(strUserInput) & "%' "
            End If

            If strUserInput2 <> "" Then
                strsql &= "and cbm_branch_name like '%" & Common.Parse(strUserInput2) & "%' "
            End If

            'If (strUserInput <> "" Or strUserInput2 <> "") Then 'And (strUserInput <> "*" Or strUserInput2 <> "*") Then

            '    strsql = "SELECT  CDM_DEPT_CODE, CDM_DEPT_NAME " _
            '                    & "FROM COMPANY_DEPT_MSTR " _
            '                    & "WHERE CDM_COY_ID='" & Common.Parse(compid) & "' AND CDM_DELETED = 'N' " _
            '                    & "AND LEFT(cdm_dept_code,2) IN ('HO','BR') AND LOCATE('-',cdm_dept_code,3) AND LOCATE('-',cdm_dept_code,7)  AND LENGTH(cdm_dept_code) = '10' and (cdm_dept_code = '" & Common.Parse(strUserInput) & "' " _
            '                    & "OR cdm_dept_name = '" & strUserInput2 & "') AND cdm_dept_code NOT IN ( SELECT IUB_BRANCH_CODE FROM ipp_usrgrp_branch where IUB_GRP_INDEX = '" & strUserGroupIndex & "')" ' for HLB Dept code only, to prevent GL Entry have NULL GL Code

            'Else
            '    strsql = "SELECT  CDM_DEPT_CODE, CDM_DEPT_NAME " _
            '              & "FROM COMPANY_DEPT_MSTR " _
            '              & "WHERE CDM_COY_ID='" & Common.Parse(compid) & "' AND CDM_DELETED = 'N' " _
            '              & "AND LEFT(cdm_dept_code,2) IN ('HO','BR') AND LOCATE('-',cdm_dept_code,3) AND LOCATE('-',cdm_dept_code,7)  AND LENGTH(cdm_dept_code) = '10' " _
            '              & "AND cdm_dept_code NOT IN ( SELECT IUB_BRANCH_CODE FROM ipp_usrgrp_branch where IUB_GRP_INDEX = '" & strUserGroupIndex & "')"
            '    ' & "OR cdm_dept_name like '%" & strUserInput2 & "%')" ' for HLB Dept code only, to prevent GL Entry have NULL GL Code
            'End If

            ds = objDb.FillDs(strsql)
            getIPPUserGroupBranchList = ds
        End Function

        Function DelIPPUserGroupBranch(ByVal strUserGroupIndex As String, ByVal strBranchCode As String, ByVal strCoyID As String) As Boolean
            Dim strdelFav As String
            Dim strAryQuery(0) As String

            'If strUserGroupIndex = "" Then
            '    strdelFav = "Delete from ipp_usrgrp_branch where IUB_BRANCH_CODE = '" & strBranchCode & "' and IUB_BR_COY_ID = '" & strCoyID & "'"
            'Else
            '    strdelFav = "Delete from ipp_usrgrp_branch where IUB_GRP_INDEX = '" & strUserGroupIndex & "' and IUB_BR_COY_ID = '" & strCoyID & "' " & _
            '    "and IUB_BRANCH_CODE = '" & strBranchCode & "' "
            'End If
            'Modified for IPP Gst Stage 2A - CH - 6 Feb 2015
            If strUserGroupIndex = "" Then
                strdelFav = "DELETE IPP_USRGRP_BRANCH FROM IPP_USRGRP_BRANCH " &
                            "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUB_GRP_INDEX " &
                            "WHERE IUB_BRANCH_CODE = '" & strBranchCode & "' and IUB_BR_COY_ID = '" & strCoyID & "' " &
                            "AND IUM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            Else
                strdelFav = "DELETE FROM IPP_USRGRP_BRANCH WHERE IUB_GRP_INDEX = '" & strUserGroupIndex & "' and IUB_BR_COY_ID = '" & strCoyID & "' " &
                "AND IUB_BRANCH_CODE = '" & strBranchCode & "' "
            End If
            '------------------------------------------------

            If objDb.Execute(strdelFav) Then
                DelIPPUserGroupBranch = WheelMsgNum.Delete
            Else
                DelIPPUserGroupBranch = WheelMsgNum.NotDelete
            End If
        End Function
        Public Function getCostCentreTypeAhead(Optional ByVal strForm As String = "", Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If strForm = "IPPUsrGrpCCAdd" Then
                strsql = "SELECT CC_CC_CODE, CC_CC_DESC " _
                           & "FROM COST_CENTRE " _
                           & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and CC_STATUS = 'A' " _
                           & "and CC_CC_CODE NOT IN ( SELECT IUC_CC_CODE FROM ipp_usrgrp_CC  where IUC_GRP_INDEX = '" & strUserGroupIndex & "')"

            Else
                strsql = "SELECT CC_CC_CODE, CC_CC_DESC " _
           & "FROM COST_CENTRE " _
           & "WHERE CC_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and CC_STATUS = 'A' " 'and CC_CC_CODE like '%" & Common.Parse(strUserInput) & "%'"
            End If



            ds = objDb.FillDs(strsql)
            getCostCentreTypeAhead = ds
        End Function

        Public Function GetIPPUserGroupCC(Optional ByVal strUserGroupIndex As String = "", Optional ByVal strCCCode As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String


            'strsql = "SELECT IUC_CC_CODE,cc_cc_code,cc_cc_desc " & _
            '                  "FROM(ipp_usrgrp_CC) " & _
            '                  "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUC_GRP_INDEX " & _
            '                  "LEFT JOIN cost_centre ON cc_cc_code = IUC_CC_CODE " & _
            '                  "where CC_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'Modified for IPP GST Stage 2A - CH - 6 Feb 2015
            strsql = "SELECT IUC_CC_CODE, CC_CC_CODE, CC_CC_DESC FROM IPP_USRGRP_CC " &
                    "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUC_GRP_INDEX " &
                    "LEFT JOIN COST_CENTRE ON CC_CC_CODE = IUC_CC_CODE " &
                    "WHERE IUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strUserGroupIndex <> "" Then
                strsql &= "AND IUC_GRP_INDEX = '" & strUserGroupIndex & "' "
            End If

            If strCCCode <> "" Then
                strsql &= "AND CC_CC_CODE = '" & strCCCode & "' "
            End If

            strsql &= "GROUP BY CC_CC_CODE"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetIPPUserGroupCC2(Optional ByVal strUserGroupIndex As String = "", Optional ByVal strCCCode As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'If strUserGroupIndex = "" Then
            '    strsql = "SELECT IUM_GRP_NAME, cc_cc_desc " & _
            '                       "FROM(ipp_usrgrp_cc) " & _
            '                       "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUC_GRP_INDEX " & _
            '                       "LEFT JOIN cost_centre ON cc_cc_code = IUC_CC_CODE " & _
            '                       "WHERE cc_cc_code = '" & strCCCode & "' " & _
            '    "GROUP BY IUC_GRP_INDEX "
            'Else
            '    strsql = "SELECT IUM_GRP_NAME, CC_CC_DESC " & _
            '       "FROM(ipp_usrgrp_cc) " & _
            '       "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUC_GRP_INDEX " & _
            '       "LEFT JOIN cost_centre ON cc_cc_code = IUC_CC_CODE " & _
            '       "where IUC_GRP_INDEX = '" & strUserGroupIndex & "' AND " & _
            '        "cc_cc_code = '" & strCCCode & "' " & _
            '        "GROUP BY IUC_GRP_INDEX "
            'End If
            'If strUserGroupIndex = "" Then
            '    strsql = "SELECT IUM_GRP_NAME " & _
            '                       "FROM(ipp_usrgrp_cc) " & _
            '                       "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUC_GRP_INDEX " & _
            '                       "LEFT JOIN cost_centre ON cc_cc_code = IUC_CC_CODE " & _
            '                       "WHERE cc_cc_code = '" & strCCCode & "' " & _
            '    "GROUP BY IUC_GRP_INDEX "
            'Else
            '    strsql = "SELECT IUM_GRP_NAME " & _
            '       "FROM(ipp_usrgrp_cc) " & _
            '       "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUC_GRP_INDEX " & _
            '       "LEFT JOIN cost_centre ON cc_cc_code = IUC_CC_CODE " & _
            '       "where IUC_GRP_INDEX = '" & strUserGroupIndex & "' AND " & _
            '        "cc_cc_code = '" & strCCCode & "' " & _
            '        "GROUP BY IUC_GRP_INDEX "
            'End If
            'Modified for IPP Gst Stage 2A - CH - 6 Feb 2015
            If strUserGroupIndex = "" Then
                strsql = "SELECT IUM_GRP_NAME " &
                        "FROM IPP_USRGRP_CC " &
                        "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUC_GRP_INDEX " &
                        "LEFT JOIN COST_CENTRE ON CC_CC_CODE = IUC_CC_CODE " &
                        "WHERE CC_CC_CODE = '" & strCCCode & "' AND IUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                        "GROUP BY IUC_GRP_INDEX "
            Else
                strsql = "SELECT IUM_GRP_NAME " &
                        "FROM IPP_USRGRP_CC " &
                        "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUC_GRP_INDEX " &
                        "LEFT JOIN COST_CENTRE ON CC_CC_CODE = IUC_CC_CODE " &
                        "where IUC_GRP_INDEX = '" & strUserGroupIndex & "' AND " &
                        "CC_CC_CODE = '" & strCCCode & "' " &
                        "GROUP BY IUC_GRP_INDEX "
            End If

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Function DelIPPUserGroupCostCenter(ByVal strUserGroupIndex As String, ByVal strCCCode As String) As Boolean
            Dim strdelFav As String
            Dim strAryQuery(0) As String

            If strUserGroupIndex = "" Then
                strdelFav = "DELETE IPP_USRGRP_CC FROM IPP_USRGRP_CC " &
                            "LEFT JOIN IPP_USRGRP_MSTR ON IUM_GRP_INDEX = IUC_GRP_INDEX " &
                            "WHERE IUC_CC_CODE = '" & strCCCode & "' AND IUM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            Else
                strdelFav = "Delete from ipp_usrgrp_cc where IUC_GRP_INDEX = '" & strUserGroupIndex & "' " &
                "and IUC_CC_CODE = '" & strCCCode & "' "

            End If


            If objDb.Execute(strdelFav) Then
                DelIPPUserGroupCostCenter = WheelMsgNum.Delete
            Else
                DelIPPUserGroupCostCenter = WheelMsgNum.NotDelete
            End If
        End Function
        Public Function getIPPUserGroupCCList(ByVal compid As String, ByVal strUserInput As String, ByVal strUserInput2 As String, ByVal strUserGroupIndex As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            'zULHAM 12072018 - PAMB 
            'Modified by CH on 4 Feb 2015 for IPP Stage 2A issue
            strsql = "SELECT  cc_cc_code, cc_cc_desc " _
                     & "FROM cost_centre " _
                     & "WHERE CC_STATUS = 'A' AND CC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                     & "AND cc_cc_code NOT IN ( SELECT IUC_CC_CODE FROM ipp_usrgrp_CC where IUC_GRP_INDEX = '" & strUserGroupIndex & "') " ' for HLB Dept code only, to prevent GL Entry have NULL GL Code

            If strUserInput <> "" Then
                strsql &= "and cc_cc_code = '" & Common.Parse(strUserInput) & "' "
            End If
            If strUserInput2 <> "" Then
                strsql &= "and cc_cc_desc = '" & Common.Parse(strUserInput2) & "' "
            End If

            ds = objDb.FillDs(strsql)
            getIPPUserGroupCCList = ds
        End Function
        Public Function getUserTypeAhead(Optional ByVal strForm As String = "", Optional ByVal strUserGroupIndex As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If strForm = "IPPUsrGrpUserAdd" Then
                strsql = "SELECT UM_USER_ID, UM_USER_NAME,UM_DEPT_ID " _
                           & "FROM user_mstr " _
                           & "WHERE UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and UM_DELETED = 'N' " _
                           & "and UM_USER_ID NOT IN ( SELECT IUU_USER_ID FROM ipp_usrgrp_user  where IUU_GRP_INDEX = '" & strUserGroupIndex & "')"

            Else
                strsql = "SELECT UM_USER_ID, UM_USER_NAME,UM_DEPT_ID " _
           & "FROM user_mstr " _
           & "WHERE UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and UM_DELETED = 'N' " 'and CC_CC_CODE like '%" & Common.Parse(strUserInput) & "%'"
            End If



            ds = objDb.FillDs(strsql)
            getUserTypeAhead = ds
        End Function

        Public Function GetIPPUserGroupUser(Optional ByVal strUserGroupIndex As String = "", Optional ByVal strUserID As String = "", Optional ByVal strUserName As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String


            strsql = "SELECT IUU_USER_ID,UM_USER_ID,UM_USER_NAME " &
                              "FROM(ipp_usrgrp_user) " &
                              "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUU_GRP_INDEX " &
                              "LEFT JOIN user_mstr ON um_user_id = IUU_USER_ID " &
                              "WHERE UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            If strUserGroupIndex <> "" Then
                strsql &= "and IUU_GRP_INDEX = '" & strUserGroupIndex & "' "

            End If

            If strUserID <> "" Then
                strsql &= "and UM_USER_ID = '" & strUserID & "' "
            End If

            If strUserName <> "" Then
                strsql &= "and UM_USER_NAME = '" & strUserID & "' "
            End If

            strsql &= "GROUP BY UM_USER_ID"

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Public Function GetIPPUserGroupUser2(Optional ByVal strUserGroupIndex As String = "", Optional ByVal strCCCode As String = "") As DataSet
            Dim ds As DataSet
            Dim strsql As String

            If strUserGroupIndex = "" Then
                strsql = "SELECT IUM_GRP_NAME " &
                                   "FROM(ipp_usrgrp_user) " &
                                   "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUU_GRP_INDEX " &
                                   "LEFT JOIN user_mstr ON um_user_id = IUU_USER_ID " &
                                   "WHERE um_user_id = '" & strCCCode & "' " &
                "GROUP BY IUU_GRP_INDEX "
            Else
                strsql = "SELECT IUM_GRP_NAME " &
                   "FROM(ipp_usrgrp_user) " &
                   "LEFT JOIN ipp_usrgrp_mstr ON IUM_GRP_INDEX = IUU_GRP_INDEX " &
                   "LEFT JOIN user_mstr ON um_user_id = IUU_USER_ID " &
                   "where IUU_GRP_INDEX = '" & strUserGroupIndex & "' AND " &
                    "um_user_id = '" & strCCCode & "' " &
                    "GROUP BY IUU_GRP_INDEX "
            End If

            ds = objDb.FillDs(strsql)
            Return ds
        End Function
        Function DelIPPUserGroupUser(ByVal strUserGroupIndex As String, ByVal strCCCode As String) As Boolean
            Dim strdelFav As String
            Dim strAryQuery(0) As String

            If strUserGroupIndex = "" Then
                strdelFav = "Delete from ipp_usrgrp_user where IUU_USER_ID = '" & strCCCode & "' "

            Else
                strdelFav = "Delete from ipp_usrgrp_user where IUU_GRP_INDEX = '" & strUserGroupIndex & "' " &
                "and IUU_USER_ID = '" & strCCCode & "' "

            End If


            If objDb.Execute(strdelFav) Then
                DelIPPUserGroupUser = WheelMsgNum.Delete
            Else
                DelIPPUserGroupUser = WheelMsgNum.NotDelete
            End If
        End Function
        Public Function getIPPUserGroupUserList(ByVal compid As String, ByVal strUserInput As String, ByVal strUserInput2 As String, ByVal strUserInput3 As String, ByVal strUserGroupIndex As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            ' for HLB Dept code only, to prevent GL Entry have NULL GL Code
            strsql = "SELECT  um_user_id, um_user_name,um_dept_id " _
                     & "FROM user_mstr " _
                     & "WHERE UM_COY_ID='" & Common.Parse(compid) & "' AND UM_DELETED = 'N' " _
                     & "AND um_user_id NOT IN ( SELECT IUU_USER_ID FROM ipp_usrgrp_user where IUU_GRP_INDEX = '" & strUserGroupIndex & "') " _
                     & " and um_user_id IN (SELECT DISTINCT(uu_user_id) FROM users_usrgrp " _
                & "INNER JOIN user_group_mstr ON ugm_usrgrp_id = uu_usrgrp_id AND uu_coy_id = '" & Common.Parse(compid) & "' " _
                & "WHERE ugm_fixed_role IN ('IPP Officer' , 'IPP Officer(F)', 'Billing Officer', 'Billing Officer(F)') )   " 'Modified for IPP GST Stage 2A - CH (30/1/2015)


            If strUserInput <> "" Then
                strsql &= " and um_user_id = '" & Common.Parse(strUserInput) & "'"
            End If
            If strUserInput2 <> "" Then
                strsql &= " and um_user_name = '" & Common.Parse(strUserInput2) & "'"
            End If
            If strUserInput3 <> "" Then
                strsql &= " and um_dept_id = '" & Common.Parse(strUserInput3) & "'"
            End If

            ds = objDb.FillDs(strsql)
            getIPPUserGroupUserList = ds
        End Function

        Public Function GetIPPAudit(ByVal strDocIndex As String) As DataSet ', ByRef CostCentreIndicator As Integer)
            Dim strSQL As String
            Dim ds1 As DataSet

            'Get cost centre code & cost centre description
            strSQL = "SELECT * FROM ipp_trans_log where itl_invoice_index = '" & strDocIndex & "'"


            ds1 = objDb.FillDs(strSQL)

            Return ds1
        End Function

        Public Function PopulateIPPFVPendingFYFA(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal vendor As String) As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strCoyId As String
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If

            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS, " _
                    & "im_payment_date,im_payment_no,ic_bank_code, ic_bank_acct,STATUS_DESC, im_route_to " _
                    & "FROM INVOICE_MSTR " _
                    & "INNER JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'INV' and status_deleted = 'N' " _
                    & "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " _
                    & "LEFT JOIN finance_approval ON IM_INVOICE_INDEX = fa_invoice_index WHERE " _
                    & "IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND fa_active_ao = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
                    & "AND im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"
            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            Else
                strSql &= " AND IM_INVOICE_STATUS IN (14)"
            End If

            If vendor <> "" Then
                strSql &= " AND IM_S_COY_NAME LIKE '%" & vendor & "%'"
            End If
            dsGroup = objDb.FillDs(strSql)
            PopulateIPPFVPendingFYFA = dsGroup
        End Function

        Public Function GetWHTReceipt(ByVal strDocNo As String, ByVal strReceiptNo As String, ByVal strVendor As String) As DataSet
            Dim strSQL, strTemp As String
            Dim ds As DataSet

            strSQL = "SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, IM_SECTION, IM_RECEIPT_NO, IM_RECEIPT_DATE, IM_S_COY_NAME, IM_WITHHOLDING_TAX, IM_DOC_DATE, IM_CURRENCY_CODE, " &
                    "CASE WHEN (IM_WITHHOLDING_OPT = 3 OR IM_WITHHOLDING_OPT = '') THEN 0 ELSE " &
                    "((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX, 0)) / 100) * IFNULL(IM_EXCHANGE_RATE, 1) END AS WHT_AMT " &
                    "FROM INVOICE_MSTR " &
                    "WHERE IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND (IM_WITHHOLDING_OPT = 1 OR IM_WITHHOLDING_OPT = 2 OR IM_WITHHOLDING_OPT = 3) " &
                    "AND IM_INVOICE_STATUS = 4 "

            If strDocNo = "" And strReceiptNo = "" Then
                strSQL &= "AND (IM_RECEIPT_NO IS NULL OR  IM_RECEIPT_NO = '') "
            Else
                If strDocNo <> "" Then
                    strTemp = Common.BuildWildCard(strDocNo)
                    strSQL &= " AND IM_INVOICE_NO" & Common.ParseSQL(strTemp)
                End If

                If strReceiptNo <> "" Then
                    strTemp = Common.BuildWildCard(strReceiptNo)
                    strSQL &= " AND IM_RECEIPT_NO" & Common.ParseSQL(strTemp)
                End If
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSQL &= " AND IM_S_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            strSQL &= " ORDER BY IM_INVOICE_NO "

            ds = objDb.FillDs(strSQL)
            Return ds
        End Function

        Public Function UpdateWHTReceipt(ByVal dsWHT As DataSet) As String
            Dim strCoyId, strUserID, strSQL, strSqlAry(0) As String
            Dim i As Integer
            strCoyId = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            If dsWHT.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsWHT.Tables(0).Rows.Count - 1
                    strSQL = "UPDATE INVOICE_MSTR SET " &
                            "IM_SECTION = '" & Common.Parse(dsWHT.Tables(0).Rows(i)("IM_SECTION")) & "', " &
                            "IM_RECEIPT_NO = '" & Common.Parse(dsWHT.Tables(0).Rows(i)("IM_RECEIPT_NO")) & "', " &
                            "IM_RECEIPT_DATE = " & Common.ConvertDate(dsWHT.Tables(0).Rows(i)("IM_RECEIPT_DATE")) & " " &
                            "WHERE IM_INVOICE_INDEX = '" & dsWHT.Tables(0).Rows(i)("IM_INVOICE_INDEX") & "'"
                    Common.Insert2Ary(strSqlAry, strSQL)
                Next
            Else
                Return objGlobal.GetErrorMessage("00003")
            End If

            If objDb.BatchExecute(strSqlAry) Then
                UpdateWHTReceipt = objGlobal.GetErrorMessage("00003")
            Else
                UpdateWHTReceipt = objGlobal.GetErrorMessage("00007")
            End If
            'End If
        End Function

#Region "Zulham March 20, 2013"
        Public Function PopulatePSDList(ByVal docno As String, ByVal doctype As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String) As DataSet
            Dim strSql, strIPPO, strCoyId As String
            Dim dsGroup As DataSet
            Dim blnIPPO As Boolean
            Dim objUsers As New Users
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If
            '------------------------------------------------

            strIPPO = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_Officer)
            strIPPO = "'" & Replace(strIPPO, "_", " ") & "'"
            blnIPPO = objUsers.checkUserFixedRole(HttpContext.Current.Session("UserID"), HttpContext.Current.Session("CompanyID"), strIPPO)

            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_CREATED_BY,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv, 'Y' AS IND " &
                     "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "WHERE IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " &
                    "AND IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND IM_INVOICE_STATUS IN (17,18) "
            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If strVen <> "" Then
                strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
            End If

            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            strSql &= " UNION " &
                    "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_CREATED_BY,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL, " &
                    "IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_BANK_CODE,IM_BANK_ACCT,IC_BANK_CODE,IC_BANK_ACCT, " &
                    "IM_PAYMENT_DATE, IM_PAYMENT_NO, IM_PRCS_SENT, IM_PRCS_RECV, 'N' AS IND " &
                    "FROM INVOICE_MSTR " &
                    "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                    "INNER JOIN FINANCE_APPROVAL ON IM_INVOICE_INDEX = FA_INVOICE_INDEX AND FA_AGA_TYPE = 'AO' " &
                    "AND (IM_STATUS_CHANGED_BY = FA_AO OR IM_STATUS_CHANGED_BY = FA_A_AO) " &
                    "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND IM_INVOICE_STATUS IN (18, 17) " &
                    "AND IM_STATUS_CHANGED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "

            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If strVen <> "" Then
                strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
            End If

            If blnIPPO = False Then
                'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
                strSql &= " UNION " &
                        "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_CREATED_BY,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL, " &
                        "IM_PAYMENT_TERM,IM_INVOICE_STATUS,IM_BANK_CODE,IM_BANK_ACCT,IC_BANK_CODE,IC_BANK_ACCT, " &
                        "IM_PAYMENT_DATE, IM_PAYMENT_NO, IM_PRCS_SENT, IM_PRCS_RECV, 'N' AS IND " &
                        "FROM INVOICE_MSTR " &
                        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' " &
                        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                        "AND IM_INVOICE_STATUS IN (17) " &
                        "AND IM_STATUS_CHANGED_BY <> '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "

                If docno <> "" Then
                    strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
                End If
                If doctype <> "" Then
                    strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
                End If
                If docsdt <> "" Then
                    strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
                End If
                If docedt <> "" Then
                    strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
                End If
                If strVen <> "" Then
                    strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
                End If
            End If

            dsGroup = objDb.FillDs(strSql)
            PopulatePSDList = dsGroup
        End Function

        Public Function PopulatePSDRecv(ByVal docno As String, ByVal doctype As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String) As DataSet
            Dim strSql, strCoyId As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If


            'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv, im_created_by " & _
            '         "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND IM_INVOICE_STATUS IN (17) "
            'Zulham 02012018
            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv, im_created_by " &
                     "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND IM_INVOICE_STATUS IN (17) or (IM_INVOICE_STATUS = 14 AND im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "')"
            '------------------------------------------------

            'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv " & _
            '         "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND IM_INVOICE_STATUS IN (17) "
            '"AND IM_INVOICE_STATUS IN (17)  OR (IM_INVOICE_STATUS = 14 AND im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "') "
            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If strVen <> "" Then
                strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
            End If

            strSql &= " order by im_prcs_sent"

            dsGroup = objDb.FillDs(strSql)
            PopulatePSDRecv = dsGroup

        End Function

        Public Function PopulateFyfa(ByVal docno As String, ByVal doctype As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String) As DataSet
            Dim strSql, strCoyId As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If

            'strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv " & _
            '         "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND IM_INVOICE_STATUS = 14 AND im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' order by im_prcs_sent"
            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,IM_PAYMENT_TERM,IM_INVOICE_STATUS,im_bank_code,im_bank_acct,ic_bank_code,ic_bank_acct,im_payment_date, im_payment_no, im_prcs_sent, im_prcs_recv " &
                    "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND IM_INVOICE_STATUS = 14 AND im_route_to = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' order by im_prcs_sent"
            '--------------------------------------------------

            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            If strVen <> "" Then
                strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
            End If

            dsGroup = objDb.FillDs(strSql)
            PopulateFyfa = dsGroup

        End Function

        Public Function GetHolidays() As DataSet
            Dim strSql As String = ""
            Dim dsHols As DataSet

            strSql = "SELECT hm_date FROM company_mstr cm, sso.holiday_mstr hm " &
                    "WHERE cm.cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND cm.cm_country = hm.hm_country " &
                    "AND cm.cm_state = hm.hm_state" &
                    " UNION " &
                    "SELECT hm_date FROM company_mstr cm, sso.holiday_mstr hm " &
                    "WHERE cm.cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND cm.cm_country = hm.hm_country"
            dsHols = objDb.FillDs(strSql)
            GetHolidays = dsHols

        End Function

        Public Function UpdatePRCSSentDate(ByVal IM_INVOICE_INdex As Integer, ByVal IM_PRCS_SENT As String) As Boolean

            Dim strAry(0) As String
            Dim strSQL As String
            Dim i As Integer

            strSQL = " UPDATE invoice_mstr SET IM_INVOICE_STATUS = 17,im_status_changed_by = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', im_status_changed_on = Now(), im_prcs_sent_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', im_prcs_sent_upd_date = Now(), IM_PRCS_SENT = " & Common.ConvertDate(IM_PRCS_SENT) & "  WHERE IM_INVOICE_INdex ='" & IM_INVOICE_INdex & "'"
            Common.Insert2Ary(strAry, strSQL)


            If objDb.BatchExecute(strAry) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function UpdatePRCSRecvDate(ByVal IM_INVOICE_INdex As Integer, ByVal IM_PRCS_RECV As String) As Boolean

            Dim strAry(0) As String
            Dim strSQL As String
            Dim i As Integer

            'Zulham 28062018 - PAMB
            If HttpContext.Current.Session("CompanyId").ToString.ToUpper <> "PAMB" Then
                strSQL = " UPDATE invoice_mstr SET im_invoice_status = 11,im_status_changed_by = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', im_status_changed_on = Now(), IM_PRCS_RECV = " & IM_PRCS_RECV & ", im_prcs_RECV_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', im_prcs_RECV_upd_date = Now() WHERE IM_INVOICE_INdex ='" & IM_INVOICE_INdex & "'"
            Else
                strSQL = " UPDATE invoice_mstr SET im_status_changed_by = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', im_status_changed_on = Now(), IM_PRCS_RECV = " & IM_PRCS_RECV & ", im_prcs_RECV_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "', im_prcs_RECV_upd_date = Now() WHERE IM_INVOICE_INdex ='" & IM_INVOICE_INdex & "'"
            End If

            Common.Insert2Ary(strAry, strSQL)

            If objDb.BatchExecute(strAry) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function AcceptPSD(ByVal IM_INVOICE_INdex As Integer, ByVal IM_REMARKS As String) As Boolean

            Dim strAry(0) As String
            Dim strSQL As String
            Dim i As Integer

            strSQL = " UPDATE invoice_mstr SET IM_INVOICE_STATUS = 12, im_remarks = '" & Common.Parse(IM_REMARKS) & "' WHERE IM_INVOICE_INdex ='" & IM_INVOICE_INdex & "'"

            Common.Insert2Ary(strAry, strSQL)

            If objDb.BatchExecute(strAry) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function RejectPSD(ByVal IM_INVOICE_INdex As Integer, ByVal IM_REMARKS As String) As Boolean

            Dim strAry(0) As String
            Dim strSQL As String
            Dim i As Integer

            strSQL = " UPDATE invoice_mstr SET IM_INVOICE_STATUS = 14, im_remarks = '" & Common.Parse(IM_REMARKS) & "' WHERE IM_INVOICE_INdex ='" & IM_INVOICE_INdex & "'"

            Common.Insert2Ary(strAry, strSQL)

            If objDb.BatchExecute(strAry) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function GetInvMstr(ByVal InvIndex As Integer) As DataTable
            Dim strSql As String
            Dim dtGroup As DataTable

            strSql = "SELECT im_due_date, im_prcs_sent " &
                     "FROM INVOICE_MSTR " &
                    "WHERE IM_invoice_index = '" & InvIndex & "' "

            dtGroup = objDb.FillDt(strSql)
            GetInvMstr = dtGroup
        End Function

        Public Function AddAuditTrailRecord(ByVal InvIndex As Integer, ByVal role As String, Optional ByVal stateDesc As String = "") As Boolean

            Dim strSQL As String
            strSQL = "INSERT INTO ipp_trans_log (itl_invoice_index,itl_performed_by,itl_user_id,itl_trans_date,itl_remarks) " &
                     "VALUES(" & InvIndex & ",'" & role & "','" & Common.Parse(HttpContext.Current.Session("UserId")) & "',Now()" &
                     ",'" & stateDesc & "')"
            objDb.Execute(strSQL)
            Return True

        End Function

        Public Function sendMailToIPPTeller(ByVal dsDoc As DataSet, ByVal intInvIdx As Integer, Optional ByVal intApprGrpIdx As String = "")
            Dim strsql, strcond, strAO, strteller As String
            Dim blnRelief As String
            Dim ds, dsteller As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim dsAO As New DataSet
            Dim i, j As Integer
            'Dim intApprGrpIdx As Integer
            Dim dsApprGrp As New DataSet

            'intApprGrpIdx = objDB.GetVal("SELECT CDM_IPP_APPROVAL_GRP_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND um_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "')")

            If Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_type")) <> "" Then
                If Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_type")) = "INV" Then
                    strDocType = "Invoice"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_type")) = "BILL" Then
                    strDocType = "Bill"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_type")) = "CN" Then
                    strDocType = "Credit Note"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_type")) = "DN" Then
                    strDocType = "Debit Note"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_type")) = "LETTER" Then
                    strDocType = "Letter"
                End If
            End If


            strBody &= "<P>The following Payment Document has been rejected by Finance Teller. <BR>"
            'strBody &= "<P>Document(Type) : " & strDocType & "<BR>"
            strBody &= "Document No.      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_no")) & "<BR>"
            strBody &= "Document Date     : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("im_doc_date")) & "<BR>"
            strBody &= "Vendor(Name)      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("im_s_coy_name")) & "<BR>"
            strBody &= "<P>For more details, please login to "
            strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            'strsql = "SELECT aga_relief_ind FROM approval_grp_ipp " & _
            '         "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' and aga_type = 'AO' order by aga_seq"

            strsql = "SELECT FA_RELIEF_IND FROM finance_approval WHERE FA_INVOICE_INDEX = '" & intInvIdx & "' AND FA_SEQ = 1 AND FA_APPROVAL_GRP_INDEX = '" & intApprGrpIdx & "'"
            blnRelief = objDB.GetVal(strsql)

            If blnRelief = "O" Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            Dim objMail As New AppMail
            'Getting email for Teller
            'IPP Gst Stage 2A - CH - 12 Feb 2015
            strteller = "SELECT * FROM user_mstr um, invoice_mstr im " &
                        "WHERE(im.im_created_by = um.um_user_id) " &
                        "AND um.um_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                        "AND im.im_invoice_index = " & intInvIdx
            dsteller = objDB.FillDs(strteller)

            If dsteller.Tables(0).Rows.Count > 0 Then
                If Common.parseNull(dsteller.Tables(0).Rows(0)("um_email")) <> "" Then
                    objMail.MailTo = Common.parseNull(dsteller.Tables(0).Rows(0)("um_email"))
                    objMail.Body = "Dear " & Common.parseNull(dsteller.Tables(0).Rows(0)("um_user_name")) & " (IPP Teller), <BR>" & strBody
                End If
            End If


            strsql = "SELECT FA.FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL " &
                     "FROM FINANCE_APPROVAL FA " &
                     "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                     "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                     "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = 1"

            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                If blnRelief Then
                    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        'objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (IPP Verifier), <BR>" & strBody
                    Else
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                        'objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (Finance Officer), <BR>" & strBody
                    End If
                Else
                    objMail.MailTo = Common.parseNull(dsteller.Tables(0).Rows(0)("um_email"))
                    objMail.Body = "Dear " & Common.parseNull(dsteller.Tables(0).Rows(0)("um_user_name")) & " (IPP Teller), <BR>" & strBody
                End If

                objMail.Subject = "IPP Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("im_invoice_no")) & " Rejected"
                objMail.SendMail()
            End If
            objCommon = Nothing
        End Function

        Public Function ApproveResubmittedDoc(ByVal strIPPDocIdx As String, ByVal strRemark As String, ByRef blnRelief As Boolean, ByVal role As String, Optional ByVal paymentmethod As String = "", Optional ByVal exchangeRate As String = "", Optional ByVal paymenttype As String = "", Optional ByVal strTime As String = "", Optional ByVal intApprGrpIndex As String = "") As Boolean
            Dim TimeNow As String = DateTime.Now.ToLocalTime.ToString("HH:mm")
            Dim strSQL, strSqlAry(0) As String
            Dim strDateTime As String

            strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 11," &
                "IM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," &
                "IM_STATUS_CHANGED_ON = NOW(), im_route_to = Null " &
                "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
            Common.Insert2Ary(strSqlAry, strSQL)

            Dim currentfaaction As Integer
            'Dim currentseq As String
            currentfaaction = objDb.GetVal("SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & strIPPDocIdx & "' ")
            'currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_ao = '" & HttpContext.Current.Session("UserId") & "' and fa_seq = '" & currentfaaction + 1 & "'")
            'If currentseq = "" Then
            '    currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO = '" & HttpContext.Current.Session("UserId") & "' and fa_seq = '" & currentfaaction + 1 & "'")
            'End If
            'Common.Insert2Ary(strSqlAry, strSQL)
            'If currentfaaction = 0 Then
            '    strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION= 0, fa_active_ao = Null, fa_action_date = null, FA_AO_REMARK = '" & Common.Parse(strRemark) & "' WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "'"
            'Else
            '    strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION= 1, fa_active_ao = Null, fa_action_date = null, FA_AO_REMARK = '" & Common.Parse(strRemark) & "' WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "'"
            'End If

            If currentfaaction = 0 Then
                strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION= 0, fa_active_ao = Null, fa_action_date = null, FA_AO_REMARK = null WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_aga_type = 'FO' "
            Else
                strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION= 1, fa_active_ao = Null, fa_action_date = null, FA_AO_REMARK = null WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_aga_type = 'FO' "
            End If

            Common.Insert2Ary(strSqlAry, strSQL)

            If objDb.BatchExecute(strSqlAry) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function PopulateBCList(ByVal docno As String, ByVal doctype As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String, Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 27 May 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            strSql = "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO,IM_INVOICE_TYPE,IM_DOC_DATE,IM_S_COY_NAME,IM_CURRENCY_CODE,IM_INVOICE_TOTAL,im_payment_date, im_payment_no,im_cheque_no " &
                     "FROM INVOICE_MSTR INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " &
                    "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND (IM_payment_term IN ('BC') OR (IM_payment_term IN ('TT') AND im_withholding_opt = 2)) and im_invoice_status = 4"
            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            Else
                strSql &= " AND im_cheque_no is null "
            End If
            If doctype <> "" Then
                strSql &= " AND IM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            End If
            'If strVen <> "" Then
            '    strSql &= " AND IM_S_COY_NAME = '" & strVen & "'"
            'End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If

            dsGroup = objDb.FillDs(strSql)
            PopulateBCList = dsGroup
        End Function

        Public Function EditBankersCheque(ByVal BCNo As String, ByVal invIdx As Integer) As Boolean

            Dim strSQL As String
            strSQL = "Update invoice_mstr set im_cheque_no = '" & BCNo & "' where im_invoice_index = " & invIdx
            objDb.Execute(strSQL)
            Return True

        End Function

        Public Function GetHOBR(ByVal strComp As String, ByVal HOBR As String, ByRef strHOBRCode As String, ByRef strHOBRName As String, ByRef chkActive As Boolean, ByRef chkInactive As Boolean, ByVal strBranchGLCode As String, Optional ByVal cbm_index As Integer = 0)
            Dim strSQL As String
            Dim ds As DataSet
            Dim ds1 As DataSet
            Dim strBranchStatus As String
            If Not cbm_index = 0 Then
                strSQL = "SELECT cbm.* FROM company_branch_mstr cbm " _
                        & "where cbm_branch_index in ('" & Common.Parse(cbm_index) & "')"
                ds1 = objDb.FillDs(strSQL)
                Return ds1
                Exit Function
            End If
            If strComp <> "" Then
                strSQL = "SELECT cbm.* FROM company_branch_mstr cbm " _
                    & "where cbm_coy_id in ('" & Common.Parse(strComp) & "')"
            Else
                strSQL = "SELECT cbm.* FROM company_branch_mstr cbm " _
                    & "where cbm_coy_id in ('" & HttpContext.Current.Session("CompanyID") & "')"
            End If
            If HOBR <> "" Then
                strSQL &= "and cbm_branch_type = '" & Common.Parse(HOBR) & "'"
            End If
            If strHOBRCode <> "" Then
                strSQL &= "AND CBM_BRANCH_CODE LIKE '%" & Common.Parse(strHOBRCode) & "%' "
            End If
            If strHOBRName <> "" Then
                strSQL &= "AND CBM_BRANCH_NAME LIKE '%" & Common.Parse(strHOBRName) & "%' "
            End If
            If chkActive = True Then
                strSQL &= "AND CBM_STATUS ='A' "
            End If
            If chkInactive = True Then
                strSQL &= "AND CBM_STATUS ='I' "
            End If
            If strBranchGLCode <> "" Then
                strSQL &= "AND CBM_GL_CODE LIKE '%" & Common.Parse(strBranchGLCode) & "%' "
            End If

            ds1 = objDb.FillDs(strSQL)
            If ds1.Tables(0).Rows.Count > 0 Then
                strComp = ds1.Tables(0).Rows(0).Item("CBM_COY_ID")
                HOBR = ds1.Tables(0).Rows(0).Item("CBM_branch_type")
                strHOBRCode = ds1.Tables(0).Rows(0).Item("CBM_BRANCH_CODE")
                strHOBRName = ds1.Tables(0).Rows(0).Item("CBM_BRANCH_NAME")
                strBranchStatus = ds1.Tables(0).Rows(0).Item("CBM_STATUS")
            Else
                strComp = "Company"
                HOBR = "Branch Type"
                strHOBRCode = "Branch Code"
                strHOBRName = "Name"
                strBranchStatus = "Status"
            End If

            ds = Nothing
            'ds1 = Nothing
            Return ds1
        End Function

        Public Function AddCompBranchMstr(ByVal strComp As String, ByVal strBranchType As String, ByVal strBranchCode As String, ByVal strBranchName As String, ByVal strBranchStatus As String, ByVal strGLCode As String) As String
            Dim strSQL As String

            strSQL = "SELECT * FROM company_branch_mstr WHERE CBm_COY_ID='" & strComp & "' " _
                    & "AND CBm_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' " 'OR CBm_BRANCH_NAME='" & Common.Parse(strBranchName) & "'"

            If objDb.Exist(strSQL) = 0 Then ' record does not exist
                strSQL = "INSERT INTO company_branch_mstr (CBm_COY_ID,cbm_branch_type,CBm_BRANCH_CODE,CBm_BRANCH_NAME,cbm_gl_code,CBm_STATUS,CBm_ENT_BY,CBm_MOD_BY) "
                strSQL &= "VALUES ("
                If Common.Parse(strComp) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strComp) & "',"
                End If
                If Common.Parse(strBranchType) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchType) & "',"
                End If
                If Common.Parse(strBranchCode) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchCode) & "',"
                End If
                If Common.Parse(strBranchName) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchName) & "',"
                End If
                If Common.Parse(strGLCode) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strGLCode) & "',"
                End If
                If Common.Parse(strBranchStatus) = "" Then
                    strSQL &= "NULL, "
                Else
                    strSQL &= "'" & Common.Parse(strBranchStatus) & "',"
                End If
                strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "','" & Common.Parse(HttpContext.Current.Session("UserID")) & "')"
                If objDb.Execute(strSQL) Then
                    AddCompBranchMstr = WheelMsgNum.Save
                Else
                    AddCompBranchMstr = WheelMsgNum.NotSave
                End If

            Else
                AddCompBranchMstr = WheelMsgNum.Duplicate
            End If

        End Function

        Public Function DeleteCompBranchMstr(ByVal dtBranch As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSQL As String
            Dim drCB As DataRow

            For Each drCB In dtBranch.Rows
                strSQL = "SELECT * FROM invoice_details " _
                        & "WHERE " _
                        & "id_pay_for = '" & Common.Parse(drCB("CBCoy")) & "' " _
                        & "AND id_branch_code = '" & Common.Parse(drCB("CBCode")) & "' "

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM ipp_usrgrp_branch WHERE iub_branch_code = '" & Common.Parse(drCB("CBCode")) & "' "

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM cost_alloc_detail WHERE cad_branch_code = '" & Common.Parse(drCB("CBCode")) & "' "

                If objDb.Exist(strSQL) <> 0 Then
                    Return -99
                End If

                strSQL = "SELECT * FROM company_branch_mstr WHERE CBm_BRANCH_CODE='" & Common.Parse(drCB("CBCode")) & "' and CBm_COY_ID = '" & Common.Parse(drCB("CBCoy")) & "'"
                If objDb.Exist(strSQL) <> 0 Then
                    'strSQL = "DELETE FROM company_branch WHERE CBm_BRANCH_CODE='" & Common.Parse(drCB("CBCode")) & "'"
                    strSQL = "DELETE FROM company_branch_mstr WHERE CBm_BRANCH_INDEX='" & Common.Parse(drCB("CBM_Index")) & "'"
                    Common.Insert2Ary(strAry, strSQL)
                End If
            Next
            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        Public Function ModCompBranchMstr(ByVal strBranchType As String, ByVal strBranchCode As String, ByVal strBranchName As String, ByVal strBranchStatus As String, ByVal cbm_index As Integer, ByVal glCode As String) As String
            Dim strSQL As String

            'Two records of same comp and ho/br code shouldnt exist
            'strSQL = "SELECT * FROM company_branch_mstr WHERE CBm_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
            '        & "AND CBm_BRANCH_CODE='" & Common.Parse(strBranchCode) & "' "
            'If objDb.Exist(strSQL) = 0 Then ' record does not exist
            strSQL = "UPDATE company_branch_mstr SET "
            If Common.Parse(strBranchType) = "" Then
                strSQL &= "CBm_BRANCH_type=NULL, "
            Else
                strSQL &= "CBm_BRANCH_type='" & Common.Parse(strBranchType) & "',"
            End If
            If Common.Parse(strBranchCode) = "" Then
                strSQL &= "CBm_BRANCH_CODE=NULL, "
            Else
                strSQL &= "CBm_BRANCH_CODE='" & Common.Parse(strBranchCode) & "',"
            End If
            If Common.Parse(strBranchName) = "" Then
                strSQL &= "CBm_BRANCH_NAME=NULL, "
            Else
                strSQL &= "CBm_BRANCH_NAME='" & Common.Parse(strBranchName) & "',"
            End If
            If Common.Parse(glCode) = "" Then
                strSQL &= "CBm_gl_code=NULL, "
            Else
                strSQL &= "CBm_gl_code='" & Common.Parse(glCode) & "',"
            End If
            If Common.Parse(strBranchStatus) = "" Then
                strSQL &= "CBm_STATUS=NULL, "
            Else
                strSQL &= "CBm_STATUS='" & Common.Parse(strBranchStatus) & "',"
            End If
            strSQL &= "CBm_MOD_BY='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
                    & "WHERE CBm_BRANCH_index= " & Common.Parse(cbm_index)
            If objDb.Execute(strSQL) Then
                ModCompBranchMstr = WheelMsgNum.Save
            Else
                ModCompBranchMstr = WheelMsgNum.NotSave
            End If
            'Else
            'ModCompBranchMstr = WheelMsgNum.Duplicate
            'End If

        End Function
#End Region
#Region "PAMB Scrum 1" 'Added by Jules 2018.04.11
        Public Function GetAnalysisCode(ByVal companyID As String, ByRef strAnalysisCode As String, ByRef strDesc As String, ByVal strDeptCode As String, ByRef strStatus As String, Optional strCategory As String = "IPP", Optional getStrSQL As Boolean = True) As Result
            Dim ds As DataSet
            Dim strsql As String
            Dim strCoyId As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strCoyId = "" Then
                strCoyId = companyID
            End If

            strsql = "SELECT AC_ANALYSIS_CODE, AC_ANALYSIS_CODE_DESC, AC_DEPT_CODE, AC_STATUS FROM analysis_code WHERE AC_B_COY_ID = '" & strCoyId & "' AND AC_DEPT_CODE = '" & strDeptCode & "' AND AC_STATUS = '" & strStatus & "'"

            If strAnalysisCode <> "" Then
                strsql &= " AND AC_ANALYSIS_CODE LIKE '%" & strAnalysisCode & "%'"
            End If
            If strDesc <> "" Then
                strsql &= " AND AC_ANALYSIS_CODE_DESC Like '%" & strDesc & "%'"
            End If

            strsql &= " ORDER BY AC_ANALYSIS_CODE_DESC "

            Dim result As Result = New Result
            If Not getStrSQL Then
                ds = objDb.FillDs(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    If strCategory = "eProcure" AndAlso strAnalysisCode = "" AndAlso strDesc = "" Then
                    Else
                        strAnalysisCode = ds.Tables(0).Rows(0).Item("AC_ANALYSIS_CODE")
                        strDesc = ds.Tables(0).Rows(0).Item("AC_ANALYSIS_CODE_DESC")
                        strStatus = ds.Tables(0).Rows(0).Item("AC_STATUS")
                    End If
                Else
                    strAnalysisCode = "Analysis Code"
                    strDesc = "Analysis Code Description"
                    strStatus = "Status"
                End If
            Else
                ds = Nothing
            End If
            result.strSql = strsql
            result.ds = ds
            Return result
        End Function
#End Region

        Public Function getIPPTempAttach(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'IPP' "
            strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "
            ds = objDb.FillDs(strsql)
            getIPPTempAttach = ds
        End Function
        Public Function deleteAttachment(ByVal strDocNo As String, ByVal docType As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = '" & docType & "' "

            ds = objDb.FillDs(strsql)

            ds = Nothing
        End Function

        Public Sub updateAttachment(ByVal invoiceIndex As String, ByVal sessionId As String)
            Dim strSQL As String
            strSQL = "Update COMPANY_DOC_ATTACHMENT_TEMP set cda_doc_no ='" & invoiceIndex & "' where cda_doc_no ='" & sessionId & "'"
            objDb.Execute(strSQL)
        End Sub

        'Zulham 18102018 - PAMB
        Public Function getCostCentreMultiGL() As DataSet

            Dim dsCC As New DataSet

            If objDb.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                dsCC = objDb.FillDs("SELECT CONCAT(CC_CC_DESC, "":"" ,CC_CC_CODE) AS CC_CC_CODE  FROM ipp_usrgrp_user " &
                                     "INNER JOIN ipp_usrgrp_cc ON iuc_grp_index = iuu_grp_index " &
                                     "INNER JOIN COST_CENTRE ON iuc_cc_code = CC_CC_CODE " &
                                     "WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' and cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND cc_status = 'A' ")
            Else
                dsCC = objDb.FillDs("SELECT CONCAT(CC_CC_DESC, "":"" ,CC_CC_CODE ) AS CC_CC_CODE " _
                                    & "FROM COST_CENTRE " _
                                    & "WHERE cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND cc_status = 'A' ")
            End If

            Return dsCC

        End Function

        'Zulham 23042019 - REQ018
        'Public Sub updateInvoiceMstrTotal(ByVal strInvoiceNo As String, ByVal strVendorId As String)
        '    Dim sql As String
        '    Dim dt As New DataTable
        '    Dim objIPPMain As New IPPMain

        '    dt = objIPPMain.getItemDetailsAmtandGST(strInvoiceNo, strVendorId)
        '    If Not dt Is Nothing Then
        '        If Not dt.Rows.Count = 0 Then
        '            sql = "update invoice_mstr 
        '                   set im_invoice_total = '" & CDec(dt.Rows(0).Item(0)) + CDec(dt.Rows(0).Item(1)) & "',
        '                   im_invoice_excl_gst = '" & CDec(dt.Rows(0).Item(1)) & "', 
        '                   im_invoice_gst = '" & CDec(dt.Rows(0).Item(0)) & "'
        '                   where im_invoice_no = '" & strInvoiceNo & "'
        '                   and im_s_coy_id = '" & strVendorId & "'"
        '            objDb.Execute(sql)
        '        End If
        '    End If

        'End Sub

        'Zulham 16042019 - REQ011
        Public Function PopulateIPPEnqDetailsList(ByVal docno As String, ByVal payadv As String, ByVal docstatus As String, ByVal docsdt As String,
                                                  ByVal docedt As String, ByVal paysdt As String, ByVal payedt As String, ByVal strVen As String,
                                                  ByVal strVenAddr As String, Optional ByVal Dept As String = "", Optional ByVal psdsentsdt As String = "",
                                                  Optional ByVal psdsentedt As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim strSql2 As String = ""
            Dim dsGroup As DataSet
            Dim strCoyId As String
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If
            '--------------------------------------------

            strSql = "SELECT * FROM ("

            If Dept = "" Then
                strSql &= "SELECT IC_BUSINESS_REG_NO,IM_S_COY_NAME, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, (ID_RECEIVED_QTY * ID_UNIT_COST) + ID_GST_VALUE 'IM_INVOICE_TOTAL',
                            IM_INVOICE_NO, '' AS 'PO_NUMBER', ID_PRODUCT_DESC, CONCAT(AC1.AC_ANALYSIS_CODE_DESC,':',ID_ANALYSIS_CODE1) 'FUNDTYPE', CONCAT(id_cost_center_desc,':',id_cost_center) 'COSTCENTER',
                            CONCAT(AC3.AC_ANALYSIS_CODE_DESC,':',ID_ANALYSIS_CODE8) 'ProjectCODE',CONCAT(AC2.AC_ANALYSIS_CODE_DESC,':',ID_ANALYSIS_CODE9) 'PERSONCODE', IM_INVOICE_STATUS, im_invoice_index
                            From INVOICE_MSTR 
                            INNER Join IPP_COMPANY ON IM_S_COY_ID = IC_INDEX And IC_COY_ID = '" & Common.Parse(strCoyId) & "' 
                            Join INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO And IM_S_COY_ID = ID_S_COY_ID
                            INNER Join COMPANY_B_GL_CODE ON ID_B_GL_CODE = CBG_B_GL_CODE And CBG_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            LEFT Join ANALYSIS_CODE AC1 ON AC1.AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1 And AC1.AC_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            LEFT Join ANALYSIS_CODE AC2 ON AC2.AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9 And AC2.AC_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            LEFT Join ANALYSIS_CODE AC3 ON AC3.AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8 And AC3.AC_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            WHERE IM_B_COY_ID = '" & Common.Parse(strCoyId) & "' 
                            And Not IM_INVOICE_TYPE Is NULL"
            Else
                strSql &= "SELECT IC_BUSINESS_REG_NO,IM_S_COY_NAME, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, (ID_RECEIVED_QTY * ID_UNIT_COST) + ID_GST_VALUE 'IM_INVOICE_TOTAL',
                            IM_INVOICE_NO, '' AS 'PO_NUMBER', ID_PRODUCT_DESC, CONCAT(AC1.AC_ANALYSIS_CODE_DESC,':',ID_ANALYSIS_CODE1) 'FUNDTYPE', CONCAT(id_cost_center_desc,':',id_cost_center) 'COSTCENTER',
                            CONCAT(AC3.AC_ANALYSIS_CODE_DESC,':',ID_ANALYSIS_CODE8) 'ProjectCODE', CONCAT(AC2.AC_ANALYSIS_CODE_DESC,':',ID_ANALYSIS_CODE9) 'PERSONCODE', IM_INVOICE_STATUS, im_invoice_index
                            From INVOICE_MSTR 
                            INNER Join IPP_COMPANY ON IM_S_COY_ID = IC_INDEX And IC_COY_ID = '" & Common.Parse(strCoyId) & "' 
                            Join INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO And IM_S_COY_ID = ID_S_COY_ID
                            INNER Join COMPANY_B_GL_CODE ON ID_B_GL_CODE = CBG_B_GL_CODE And CBG_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            LEFT Join ANALYSIS_CODE AC1 ON AC1.AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1 And AC1.AC_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            LEFT Join ANALYSIS_CODE AC2 ON AC2.AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9 And AC2.AC_B_COY_ID = '" & Common.Parse(strCoyId) & "'
                            LEFT Join ANALYSIS_CODE AC3 ON AC3.AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8 And AC3.AC_B_COY_ID = '" & Common.Parse(strCoyId) & "'                            
                            INNER Join USER_MSTR um ON IM_CREATED_BY = um.UM_USER_ID
                            WHERE IM_B_COY_ID = '" & Common.Parse(strCoyId) & "' 
                            And Not IM_INVOICE_TYPE Is NULL
                            And um.UM_DEPT_ID = '" & Dept & "' AND IM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
            End If

            If docno <> "" Then
                strSql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If docsdt <> "" Then
                strSql &= " AND IM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND IM_DOC_DATE <= " & "'" & Format(CDate(docedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If payadv <> "" Then
                strSql &= " AND IM_PAYMENT_NO LIKE '%" & payadv & "%'"
            End If
            If docstatus <> "" Then
                strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If paysdt <> "" Then
                strSql &= " AND IM_PAYMENT_DATE >= " & Common.ConvertDate(paysdt) & ""
            End If
            If payedt <> "" Then
                strSql &= " AND IM_PAYMENT_DATE <= " & "'" & Format(CDate(payedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If psdsentsdt <> "" Then
                strSql &= " AND IM_PRCS_SENT >= " & Common.ConvertDate(psdsentsdt) & ""
            End If
            If psdsentedt <> "" Then
                strSql &= " AND IM_PRCS_SENT <= " & "'" & Format(CDate(psdsentedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            End If
            If strVenAddr <> "" Then
                strSql &= " AND ic2.IC_ADDR_LINE1 like '%" & strVenAddr & "%'"
            End If

            strSql &= ") tb ORDER BY IM_INVOICE_INDEX "

            dsGroup = objDb.FillDs(strSql)
            PopulateIPPEnqDetailsList = dsGroup
        End Function

    End Class
End Namespace