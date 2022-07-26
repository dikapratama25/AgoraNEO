Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO

Namespace AgoraNEO
    Public Class PurchaseOrder_AO
        Dim objDb As New EAD.DBCom

        Function getPOListForApproval(ByVal UserID As String, ByVal CompanyID As String, ByVal strPoNo As String, ByVal strVendor As String,
                ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strReliefOn As String,
                Optional ByVal strAction As String = "new", Optional ByVal strStatus As String = "", Optional ByVal strAOAction As String = "", Optional ByVal strInclude As String = "", Optional ByVal strIncludeHold As String = "", Optional getStrSQL As Boolean = False) As Result

            Dim strSql, strSqlReliefO, strSqlReliefC, strCondition, strCondition1, strSqlAttached As String
            Dim strUser As String = UserID
            Dim strCoyId As String = CompanyID
            Dim ds As DataSet
            Dim strTemp As String

            If strAction = "new" Then
                '//current approval AO of the PO is login user and PO not rejected by previous AO
                'strCondition = " AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status NOT IN(" & POStatus_new.RejectedBy & "," & POStatus_new.CancelledBy & "))"
                strCondition = " AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status IN (" & strStatus & "))"
                strCondition1 = "(PA.PRA_AO = '" & strUser & "' " _
                & "OR (PA.PRA_A_AO = '" & strUser & "' AND PA.PRA_Relief_Ind='O'))"
            ElseIf strAction = "app" Then
                If strAOAction = "" Then
                    strCondition = " AND (PA.PRA_Seq <= PA.PRA_AO_Action)"
                Else
                    If strAOAction = "Approved" Then
                        strCondition = " AND (PA.PRA_Seq <= PA.PRA_AO_Action) AND (SUBSTRING(PRA_AO_REMARK,1,8) = '" & strAOAction & "' OR SUBSTRING(PRA_AO_REMARK,1,8) = 'Endorsed') "
                    Else
                        strCondition = " AND (PA.PRA_Seq <= PA.PRA_AO_Action) AND SUBSTRING(PRA_AO_REMARK,1,8) = '" & strAOAction & "' "
                    End If
                End If
                '//ownership already taken by AAO, no need to check for relief ind
                strCondition1 = "(((PA.PRA_AO = '" & strUser & "' " _
                & "OR PA.PRA_A_AO = '" & strUser & "') AND PRA_RELIEF_IND='O') OR ((PA.PRA_AO = '" & strUser & "' " _
                & "OR PA.PRA_ON_BEHALFOF = '" & strUser & "') AND PRA_RELIEF_IND<>'O'))"
                '//set to empty string to prevent mistake during parameter passing
                strReliefOn = ""
            End If

            '// 1 PR MANY ATTACHMENT
            '//MAYBE USE ATTCHED IND
            strSqlAttached = "SELECT CDA_DOC_NO FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyId & "' AND CDA_DOC_TYPE='PR'"

            strSqlReliefO = "SELECT DISTINCT PM.POM_PO_Index,PM.POM_PO_No, PM.POM_PO_DATE, PM.POM_STATUS_CHANGED_ON, PM.POM_S_Coy_ID, POM_B_COY_ID,  PM.POM_CREATED_Date,PM.POM_BUYER_ID, PM.POM_RFQ_INDEX, " _
           & "PM.POM_CURRENCY_CODE, PM.POM_S_COY_NAME, PM.POM_PO_STATUS,(CASE WHEN PM.POM_PO_STATUS = '10' THEN 'Rejected' WHEN PM.POM_PO_STATUS = '11' THEN 'On Hold' ELSE 'Approved' END) AS STAT,PM.POM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
           & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC," _
           & "PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME, PM.POM_URGENT, " _
           & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & CompanyID & "') AS NAME, PM.POM_SUBMIT_DATE " _
           & "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX " _
           & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID " _
           & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
           & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.POM_STATUS_CHANGED_BY " _
           & "AND UM.UM_COY_ID='" & strCoyId & "' WHERE PA.PRA_FOR = 'PO' AND " & strCondition1 &
           " AND PM.POM_B_COY_ID='" & strCoyId & "'" & strCondition

            If strPoNo <> "" Then
                strTemp = Common.BuildWildCard(strPoNo)
                strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_No" & Common.ParseSQL(strTemp)
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSqlReliefO = strSqlReliefO & " AND PM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If dteDateFr <> "" Then
                strSqlReliefO = strSqlReliefO & " AND PM.POM_CREATED_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
            End If

            'If dteDateFr = "" And dteDateTo <> "" Then
            If dteDateTo <> "" Then
                strSqlReliefO = strSqlReliefO & " AND PM.POM_CREATED_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
            End If

            If strAction = "app" And strStatus <> "" Then
                strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_Status IN (" & strStatus & ")"
            End If

            If (strAOAction = "Approved" Or strAOAction = "") And strInclude = "" Then
                strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_Status NOT IN(10)"
            End If

            If (strAOAction = "Approved" Or strAOAction = "") And strIncludeHold = "" Then
                strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_Status NOT IN(11)"
            End If
            '//For Relief Ind=Open


            Dim strSqlCheck, strReliefList As String

            If UCase(strReliefOn) = "WHEELALL" Then '//in case need to show all PR
                Dim dvCheck As DataView
                Dim objPR2 As New PurchaseReq2
                '//For Relief Ind=Controlled             
                dvCheck = objPR2.getReliefList(UserID, CompanyID, "PO")
                Dim intCnt, intLoop As Integer
                If Not dvCheck Is Nothing Then
                    intCnt = dvCheck.Count
                    For intLoop = 0 To intCnt - 1
                        If Not IsDBNull(dvCheck.Item(intLoop)("RAM_USER_ID")) Then
                            If strReliefList = "" Then
                                strReliefList = "'" & dvCheck.Item(intLoop)("RAM_USER_ID") & "'"
                            Else
                                strReliefList = strReliefList & ",'" & dvCheck(intLoop)("RAM_USER_ID") & "'"
                            End If
                        End If
                    Next
                End If
            ElseIf strReliefOn <> "" Then
                strReliefList = "'" & strReliefOn & "'"
            Else
                strReliefList = ""
            End If

            If strReliefList <> "" Then
                strSqlReliefC = "SELECT DISTINCT PM.POM_PO_Index,PM.POM_PO_No,PM.POM_S_Coy_ID, PM.POM_CREATED_Date,PM.POM_BUYER_ID,PM.POM_STATUS_CHANGED_ON, PM.POM_RFQ_INDEX, " _
                & "PM.POM_CURRENCY_CODE, PM.POM_S_COY_NAME, PM.POM_PO_STATUS,(CASE WHEN PM.POM_PO_STATUS = '10' THEN 'Rejected' WHEN PM.POM_PO_STATUS = '11' THEN 'On Hold' ELSE 'Approved' END) AS STAT,PM.POM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
                & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC," _
                & "PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME, PM.POM_URGENT, " _
                & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(CompanyID) & "') AS NAME, PM.POM_SUBMIT_DATE " _
                & "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX " _
                & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID " _
                & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
                & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.POM_STATUS_CHANGED_BY " _
                & "AND UM.UM_COY_ID='" & strCoyId & "' " _
                & "WHERE PA.PRA_AO IN(" & strReliefList & ") AND PA.PRA_FOR = 'PO' " _
                & "AND PA.PRA_A_AO = '" & strUser & "' " _
                & "And PA.PRA_Relief_Ind='C' AND PM.POM_B_COY_ID='" & strCoyId & "'" & strCondition


                If strPoNo <> "" Then
                    strTemp = Common.BuildWildCard(strPoNo)
                    strSqlReliefC = strSqlReliefC & " AND PM.POM_PO_No" & Common.ParseSQL(strTemp)
                End If

                If strVendor <> "" Then
                    strTemp = Common.BuildWildCard(strVendor)
                    strSqlReliefC = strSqlReliefC & " AND PM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                End If

                If dteDateFr <> "" Then
                    strSqlReliefC = strSqlReliefC & " AND POM_CREATED_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
                End If

                'If dteDateFr = "" And dteDateTo <> "" Then
                If dteDateTo <> "" Then
                    strSqlReliefC = strSqlReliefC & " AND POM_CREATED_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
                End If


                If strAction = "app" And strStatus <> "" Then
                    strSqlReliefC = strSqlReliefC & " AND PM.POM_PO_Status IN (" & strStatus & ")"
                End If

                If (strAOAction = "Approved" Or strAOAction = "") And strInclude = "" Then
                    strSqlReliefC = strSqlReliefC & " AND PM.POM_PO_Status NOT IN(10)"
                End If

                If (strAOAction = "Approved" Or strAOAction = "") And strIncludeHold = "" Then
                    strSqlReliefC = strSqlReliefC & " AND PM.POM_PO_Status NOT IN(11)"
                End If
            End If
            '//For Relief Ind=Controlled


            If UCase(strReliefOn) = "WHEELALL" Then '//show all
                If strSqlReliefC <> "" Then
                    strSql = strSqlReliefO & " UNION " & strSqlReliefC
                Else
                    strSql = strSqlReliefO
                End If
            ElseIf strReliefOn <> "" Then '//show other ao's
                strSql = strSqlReliefC
            Else '//show own pr
                strSql = strSqlReliefO
            End If

            Dim result As Result = New Result
            If Not getStrSQL Then
                ds = objDb.FillDs(strSql)
            Else
                ds = Nothing
            End If
            result.strSql = strSql
            result.ds = ds
            getPOListForApproval = result
        End Function

        'Function MassApprovalPO(ByVal strAryPONo() As String, ByVal strAryPOIndex() As String, ByVal strAO As String, ByRef strReturnMsg() As String, ByVal blnRelief As Boolean) As Boolean
        '    Dim strSql, strSql1, strAryQuery(0), strApprType As String
        '    Dim strCoyID, strAllPO, strAllPOIndex As String
        '    Dim intLoop As Integer
        '    Dim ds As DataSet
        '    Dim strMsg As String
        '    Dim dsTemp As DataSet

        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    For intLoop = 0 To strAryPONo.GetUpperBound(0)
        '        If intLoop = 0 Then
        '            strAllPO = "'" & strAryPONo(intLoop) & "'"
        '            strAllPOIndex = strAryPOIndex(intLoop)
        '        Else
        '            strAllPO = strAllPO & ",'" & strAryPONo(intLoop) & "'"
        '            strAllPOIndex = strAllPOIndex & "," & strAryPOIndex(intLoop)
        '        End If
        '    Next

        '    strSql = "SELECT POM_PO_INDEX,POM_PO_NO,POM_S_COY_NAME,POM_S_EMAIL,POM_PO_STATUS,POM_STATUS_CHANGED_BY,POM_BUYER_ID, POM_S_COY_ID FROM PO_MSTR WHERE POM_PO_NO IN (" & strAllPO & ") And POM_B_COY_ID='" & strCoyID & "'"
        '    strSql1 = "SELECT * FROM PR_APPROVAL WHERE PRA_PR_INDEX IN (" & strAllPOIndex & ") AND PRA_FOR = 'PO' "

        '    ds = objDb.FillDs(strSql & ";" & strSql1)
        '    ds.Tables(0).TableName = "PO_MSTR"
        '    ds.Tables(1).TableName = "PR_APPROVAL"

        '    If Not ds Is Nothing Then
        '        Dim parentCol As DataColumn
        '        Dim childCol As DataColumn
        '        Dim dvChildView, dvParentView As DataView
        '        Dim POrow, POApprrow As DataRow
        '        Dim intCurrentSeq, intLastSeq As Integer
        '        Dim blnHighestLevel, blnCanApprove As Boolean
        '        Dim strActiveAO As String

        '        parentCol = ds.Tables("PO_MSTR").Columns("POM_PO_INDEX")
        '        childCol = ds.Tables("PR_APPROVAL").Columns("PRA_PR_INDEX")

        '        ' Create DataRelation.
        '        Dim relPO As DataRelation
        '        relPO = New DataRelation("acct", parentCol, childCol)
        '        ' Add the relation to the DataSet.
        '        ds.Relations.Add(relPO)
        '        For Each POrow In ds.Tables("PO_MSTR").Rows
        '            blnCanApprove = True
        '            If POrow("POM_PO_STATUS") = POStatus_new.PendingApproval Or _
        '            POrow("POM_PO_STATUS") = POStatus_new.Submitted Or _
        '            POrow("POM_PO_STATUS") = POStatus_new.HeldBy Then
        '                For Each POApprrow In POrow.GetChildRows(relPO)
        '                    dsTemp = getPOListForApproval(POrow("POM_PO_No"), POrow("POM_S_COY_NAME"), "", "", "", , POStatus_new.Submitted & "," & POStatus_new.PendingApproval & "," & POStatus_new.HeldBy)
        '                    If dsTemp.Tables(0).Rows.Count = 0 Then
        '                        strMsg = "You have already approved/endorsed PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed."
        '                        Common.Insert2Ary(strReturnMsg, strMsg)
        '                        blnCanApprove = False
        '                        Exit Function
        '                    End If
        '                    intLastSeq = POApprrow("PRA_AO_Action")
        '                    intCurrentSeq = intLastSeq + 1

        '                    '//check whether the PO was already approved by the same AO at the
        '                    '//same approving level.
        '                    If POApprrow("PRA_Seq") = intCurrentSeq Then
        '                        strApprType = POApprrow("PRA_APPROVAL_TYPE")
        '                        strActiveAO = Common.parseNull(POApprrow("PRA_ACTIVE_AO"))
        '                        If strActiveAO <> "" Then
        '                            strMsg = "You have already approved/endorsed PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed."
        '                            Common.Insert2Ary(strReturnMsg, strMsg)
        '                            blnCanApprove = False
        '                            Exit For
        '                        Else
        '                            If Not (UCase(POApprrow("PRA_AO")) = UCase(strAO) Or _
        '                             UCase(Common.parseNull(POApprrow("PRA_A_AO"))) = UCase(strAO)) Then
        '                                strMsg = "You are not a authorised person to approve PO No. " & POrow("POM_PO_NO")
        '                                Common.Insert2Ary(strReturnMsg, strMsg)
        '                                blnCanApprove = False
        '                                Exit For
        '                            End If
        '                        End If
        '                    End If

        '                    If intCurrentSeq = POApprrow("PRA_SEQ") Then
        '                        blnHighestLevel = True
        '                    Else
        '                        blnHighestLevel = False
        '                    End If
        '                Next
        '                If blnCanApprove Then
        '                    If strApprType = "1" Then
        '                        strMsg = ApprovePO(POrow("POM_PO_NO"), POrow("POM_PO_INDEX"), intCurrentSeq, blnHighestLevel, "Approved : ", Common.parseNull(POrow("POM_BUYER_ID")), blnRelief, strApprType, POrow("POM_S_COY_ID"))
        '                    Else
        '                        strMsg = ApprovePO(POrow("POM_PO_NO"), POrow("POM_PO_INDEX"), intCurrentSeq, blnHighestLevel, "Endorsed : ", Common.parseNull(POrow("POM_BUYER_ID")), blnRelief, strApprType, POrow("POM_S_COY_ID"))
        '                    End If

        '                    Common.Insert2Ary(strReturnMsg, strMsg)
        '                End If
        '            Else '//ERROR
        '                '//PO APPROVED/REJECTED/CANCELLED                       
        '                If POrow("POM_PO_STATUS") = POStatus_new.Cancelled Or POrow("POM_PO_STATUS") = POStatus_new.CancelledBy Then
        '                    strMsg = "PO No. " & POrow("POM_PO_NO") & " has already been cancelled. Approving of this PO is not allowed. "
        '                ElseIf POrow("POM_PO_STATUS") = POStatus_new.RejectedBy Then
        '                    strMsg = "You have already rejected PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed. "
        '                ElseIf POrow("POM_PO_STATUS") = POStatus_new.Approved Then
        '                    strMsg = "You have already approved PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed."
        '                ElseIf POrow("POM_PO_STATUS") = POStatus_new.NewPO Then
        '                    strMsg = "You have already approved PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed."
        '                End If
        '                Common.Insert2Ary(strReturnMsg, strMsg)
        '            End If
        '        Next
        '    End If
        '    Return True
        'End Function
        Function GetPOForApprDetails(ByVal CompanyID As String, ByVal strPoNo As String, ByVal strPOIndex As String) As String
            Dim strSql, strSqlPOM, strSqlPOD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPOField As String
            Dim ds, ds1 As DataSet
            strCoyID = CompanyID


            strSqlPOD = "SELECT PD.*, (SELECT CONCAT(COMPANY_B_GL_CODE.CBG_B_GL_DESC, '(', POD_B_GL_CODE ,')')) AS gl_codedesc , " _
                         & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.POD_D_COUNTRY) AS CT, " _
                         & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.POD_D_STATE AND CODE_VALUE=PD.POD_D_COUNTRY) AS STATE, CONCAT(PD.POD_ASSET_GROUP, CONCAT(' ',PD.POD_ASSET_NO)) AS ASSET_CODE, " _
                         & "CASE WHEN POD_GST_RATE = 'N/A' THEN POD_GST_RATE ELSE IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, CASE WHEN IFNULL(POD_GIFT,'N') = 'N' THEN 'No' ELSE 'Yes' END AS GIFT, " _
                         & "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE) AS FUNDTYPE, " _
                         & "CASE WHEN POD_PERSON_CODE = 'N/A' THEN POD_PERSON_CODE ELSE (SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE) END personcodedesc, " _
                         & "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE) AS projectcodedesc " _
                         & "FROM PO_DETAILS PD " _
                         & "LEFT JOIN COMPANY_B_GL_CODE " _
                         & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.POD_B_GL_CODE AND CBG_B_COY_ID =  POD_COY_ID " _
                         & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = POD_GST_RATE " _
                         & "LEFT JOIN TAX ON TAX_CODE = POD_GST_RATE AND TAX_COUNTRY_CODE = 'MY' " _
                         & "WHERE POD_COY_ID='" & strCoyID & "' AND POD_PO_NO='" & strPoNo & "' ORDER BY POD_PO_LINE"


            Return strSqlPOD

        End Function
        Function getPOForAppr(ByVal CompanyID As String, ByVal strPoNo As String, ByVal strPOIndex As String) As DataSet
            Dim strSql, strSqlPOM, strSqlPOD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
            Dim strPOField As String
            Dim ds, ds1 As DataSet
            strCoyID = CompanyID

            strPOField = "PM.POM_PO_NO, PM.POM_PO_INDEX, PM.POM_STATUS_CHANGED_ON, PM.POM_PO_DATE, PM.POM_BUYER_NAME,  PM.POM_BUYER_PHONE, PM.POM_B_ADDR_LINE1, PM.POM_B_ADDR_LINE2, PM.POM_B_ADDR_LINE3, " _
            & "PM.POM_B_POSTCODE, PM.POM_B_CITY, PM.POM_INTERNAL_REMARK, PM.POM_S_COY_ID, PM.POM_S_COY_NAME, PM.POM_CURRENCY_CODE,PM.POM_BUYER_ID, PM.POM_PAYMENT_TERM, PM.POM_PAYMENT_METHOD, PM.POM_SHIPMENT_TERM, PM.POM_SHIPMENT_MODE, " _
            & "PM.POM_S_ADDR_LINE1,PM.POM_S_ADDR_LINE2,PM.POM_S_ADDR_LINE3,PM.POM_S_POSTCODE,PM.POM_S_CITY,PM.POM_S_STATE,PM.POM_S_COUNTRY,PM.POM_S_PHONE,PM.POM_S_FAX,PM.POM_S_EMAIL,PM.POM_SHIP_VIA,PM.POM_FREIGHT_TERMS,PM.POM_S_ATTN,PM.POM_CREATED_DATE, " _
            & "PM.POM_External_Remark, PM.POM_PO_Status,PM.POM_RFQ_INDEX, PM.POM_SHIP_AMT, PM.POM_SUBMIT_DATE "

            strSqlPOM = "SELECT " & strPOField & ", CM.CM_TAX_CALC_BY, " _
            & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.POM_B_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.POM_B_STATE AND CODE_VALUE=PM.POM_B_COUNTRY) AS STATE, PM.POM_URGENT, " _
            & "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = PM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & strCoyID & "') AS NAME " _
            & "FROM PO_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.POM_S_COY_ID=CM.CM_COY_ID " _
            & "LEFT JOIN RFQ_MSTR ON PM.POM_RFQ_INDEX=RM_RFQ_ID " _
            & "WHERE " _
            & "POM_B_COY_ID='" & strCoyID & "' AND POM_PO_No='" & strPoNo & "'"

            'Jules 2018.11.05 - Swapped Analysis Code display: "Analysis Code Description : Analysis Code"
            'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift & Analysis Codes.
            strSqlPOD = "SELECT PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.POD_D_COUNTRY) AS CT, " _
            & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.POD_D_STATE AND CODE_VALUE=PD.POD_D_COUNTRY) AS STATE, CONCAT(PD.POD_ASSET_GROUP, CONCAT(' ',PD.POD_ASSET_NO)) AS ASSET_CODE, " _
            & "CASE WHEN POD_GST_RATE = 'N/A' THEN POD_GST_RATE ELSE IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, CASE WHEN IFNULL(POD_GIFT,'N') = 'N' THEN 'No' ELSE 'Yes' END AS GIFT, " _
            & "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE) AS FUNDTYPE, " _
            & "CASE WHEN POD_PERSON_CODE = 'N/A' THEN POD_PERSON_CODE ELSE (SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE) END PERSONCODE, " _
            & "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE) AS PROJECTCODE " _
            & "FROM PO_DETAILS PD " _
            & "LEFT JOIN COMPANY_B_GL_CODE " _
            & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.POD_B_GL_CODE AND CBG_B_COY_ID =  POD_COY_ID " _
            & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = POD_GST_RATE " _
            & "LEFT JOIN TAX ON TAX_CODE = POD_GST_RATE AND TAX_COUNTRY_CODE = 'MY' " _
            & "WHERE POD_COY_ID='" & strCoyID & "' AND POD_PO_NO='" & strPoNo & "' ORDER BY POD_PO_LINE"

            strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX=" & strPOIndex
            strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX=" & strPOIndex
            'strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PR' AND PCD_PR_INDEX=(SELECT PRM_PR_INDEX FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = '" & strCoyID & "' AND PRD_CONVERT_TO_DOC  = (SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_PO_INDEX = '" & strPOIndex & "')) "
            'strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PR' AND PCM_PR_INDEX=(SELECT PRM_PR_INDEX FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = '" & strCoyID & "' AND PRD_CONVERT_TO_DOC  = (SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_PO_INDEX = '" & strPOIndex & "')) "
            strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPoNo & "' AND CDA_DOC_TYPE='PO'"

            strSql = strSqlPOM & ";" & strSqlPOD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
            ds = objDb.FillDs(strSql)

            ds.Tables(0).TableName = "PO_MSTR"
            ds.Tables(1).TableName = "PO_DETAILS"
            ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
            ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
            ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
            Return ds
        End Function

        'Function RejectPO(ByVal strPONo As String, ByVal intPOIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal strUserID As String, ByVal blnRelief As Boolean) As String
        '    Dim strSql, strSqlAry(0) As String
        '    Dim strCoyID, strLoginUser As String
        '    Dim intPOStatus As Integer
        '    Dim strMsg As String
        '    Dim objPR2 As New PurchaseReq2

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strSql = "SELECT POM_PO_STATUS,POM_STATUS_CHANGED_BY FROM PO_MSTR WHERE POM_PO_Index=" & intPOIndex
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        intPOStatus = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS")
        '        If intPOStatus = POStatus_new.Cancelled Or intPOStatus = POStatus_new.CancelledBy Then
        '            strMsg = "The PO has already been cancelled. Rejecting of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.Void Then
        '            strMsg = "The PO has already been void. Rejecting of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.RejectedBy And tDS.Tables(0).Rows(0).Item("POM_STATUS_CHANGED_BY") = strUserID Then
        '            strMsg = "You have already rejected this PO."
        '        ElseIf intPOStatus = POStatus_new.Approved Then '//approved '//NEED TO PR-APPROVAL
        '            strMsg = "You have already approved this PO. Rejecting of this PO is not allowed."
        '        End If
        '    End If

        '    If intPOStatus = POStatus_new.PendingApproval And objPR2.isApproved(intPOIndex, intCurrentSeq, strUserID, "PO") Then
        '        strMsg = "You have already approved this PO. Rejecting of this PO is not allowed."
        '    End If

        '    strLoginUser = HttpContext.Current.Session("UserId") 'is a AO
        '    If strMsg = "" Then
        '        Dim objUsers As New Users
        '        objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_RejectPO, strPONo)
        '        objUsers = Nothing
        '        strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.RejectedBy & _
        '        ",POM_STATUS_CHANGED_BY='" & strUserID & "',POM_STATUS_CHANGED_ON=" & _
        '        Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex
        '        Common.Insert2Ary(strSqlAry, strSql)
        '        objPR2.updateAOAction(intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")

        '        'Dim objBCM As New BudgetControl
        '        'objBCM.BCMCalc("PR", strPRNo, EnumBCMAction.RejectPR, strSqlAry)
        '        'objBCM = Nothing

        '        If objDb.BatchExecute(strSqlAry) Then
        '            Dim objMail As New Email
        '            Dim objPR As New PR
        '            Dim strName As String
        '            strName = objPR.getRequestorName("PO", strPONo, strCoyID)
        '            objMail.sendNotification(EmailType.PORejectedBy, strUserID, strCoyID, "", strPONo, strName)
        '            sendMailToPrevAO(strPONo, intPOIndex, intCurrentSeq, strName)
        '            objMail = Nothing
        '            objPR = Nothing
        '            strMsg = "Purchase Order Number " & strPONo & " has been rejected."
        '            '//SEND MAIL TO BUYER
        '        Else
        '            strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        '        End If
        '        Return strMsg

        '    Else
        '        Return strMsg
        '    End If
        'End Function

        'Public Function sendMailToPrevAO(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, ByVal strRequestor As String)
        '    Dim strsql, strRole As String
        '    Dim i As Integer
        '    Dim ds As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common

        '    strRole = "Approving Officer"
        '    strBody &= "<P>PO (" & strDocNo & ") has been rejected by your Approving Officer. <BR>"
        '    'strBody &= "<P>Requestor Name : " & strRequestor & "<BR>"
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
        '    strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_FOR = 'PO' AND PRA_SEQ < " & intSeq
        '    ds = objDb.FillDs(strsql)

        '    Dim objMail As New AppMail
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))
        '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Approving Officer), <BR>" & strBody

        '        objMail.Subject = "Agora : PO Rejected"
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
        '    strBody &= "<P>PO (" & strDocNo & ") has been held by your Approving Officer. <BR>"
        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

        '    strsql = "SELECT PRA_ACTIVE_AO, UM_EMAIL, UM_USER_NAME FROM PR_APPROVAL "
        '    strsql &= "LEFT JOIN USER_MSTR ON PRA_ACTIVE_AO = UM_USER_ID AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_FOR = 'PO' AND PRA_SEQ < " & intSeq
        '    ds = objDb.FillDs(strsql)

        '    Dim objMail As New AppMail
        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))
        '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Approving Officer), <BR>" & strBody

        '        objMail.Subject = "Agora : PO Held"
        '        objMail.SendMail()
        '    Next

        '    objMail = Nothing
        '    objCommon = Nothing
        'End Function

        'Function HoldPO(ByVal strPONo As String, ByVal intPOIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal strUserID As String, ByVal blnRelief As Boolean, Optional ByVal dsPOD As DataSet = Nothing) As String
        '    Dim strSql, strSqlAry(0) As String
        '    Dim strCoyID, strLoginUser As String
        '    Dim intPOStatus As Integer
        '    Dim strMsg As String
        '    Dim objPR2 As New PurchaseReq2

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strSql = "SELECT POM_PO_STATUS,POM_STATUS_CHANGED_BY FROM PO_MSTR WHERE POM_PO_Index=" & intPOIndex
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        intPOStatus = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS")
        '        If intPOStatus = POStatus_new.Cancelled Or intPOStatus = POStatus_new.CancelledBy Then
        '            strMsg = "The PO has already been cancelled. Holding of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.Void Then
        '            strMsg = "The PO has already been void. Holding of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.HeldBy And tDS.Tables(0).Rows(0).Item("POM_STATUS_CHANGED_BY") = strUserID Then
        '            strMsg = "You have already held this PO."
        '        ElseIf intPOStatus = POStatus_new.Approved Then '//approved '//NEED TO PR-APPROVAL
        '            strMsg = "You have already approved this PO. Holding of this PO is not allowed."
        '        End If
        '    End If

        '    strLoginUser = HttpContext.Current.Session("UserId") 'is a AO

        '    If intPOStatus = POStatus_new.PendingApproval And objPR2.isApproved(intPOIndex, intCurrentSeq, strLoginUser, "PO") Then
        '        strMsg = "You have already approved this PO. Holding of this PO is not allowed."
        '    End If

        '    If dsPOD Is Nothing Then
        '        Dim intEDD As Integer
        '        Dim PO_NO As String
        '        strSql = "SELECT PO_DETAILS.POD_ETD AS POD_ETD, PO_MSTR.POM_PO_NO AS POM_PO_NO FROM PO_DETAILS, PO_MSTR WHERE POM_PO_INDEX = " & intPOIndex & " AND POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO"
        '        Dim tDS2 As DataSet = objDb.FillDs(strSql)
        '        Dim rowi As Integer
        '        If tDS2.Tables(0).Rows.Count > 0 Then
        '            For rowi = 0 To tDS2.Tables(0).Rows.Count - 1
        '                intEDD = tDS2.Tables(0).Rows(rowi).Item("POD_ETD")
        '                PO_NO = tDS2.Tables(0).Rows(rowi).Item("POM_PO_NO")
        '                If intEDD < 2 Then
        '                    strMsg = "Cannot do mass approval as EDD of PO " & PO_NO & " is invalid."
        '                End If
        '            Next
        '        End If
        '    End If

        '    If strMsg = "" Then
        '        Dim objUsers As New Users
        '        objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_RejectPO, strPONo)
        '        objUsers = Nothing

        '        Dim dds As DataTable
        '        Dim i As Integer
        '        If dsPOD Is Nothing Then

        '        Else
        '            For i = 0 To dsPOD.Tables(0).Rows.Count - 1
        '                If Common.Parse(Trim(dsPOD.Tables(0).Rows(i)("ProductCode"))) <> "" And Common.Parse(Trim(dsPOD.Tables(0).Rows(i)("ProductCode"))) <> "&nbsp;" Then
        '                    strSql = " UPDATE PO_DETAILS " & _
        '                            " SET POD_ETD = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ETD")) & _
        '                            " WHERE POD_PO_NO = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("PONo")) & "' " & _
        '                            " AND POD_PO_LINE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("Line")) & _
        '                            " AND POD_PRODUCT_CODE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ProductCode")) & ""
        '                    Common.Insert2Ary(strSqlAry, strSql)
        '                Else
        '                    strSql = " UPDATE PO_DETAILS " & _
        '                            " SET POD_ETD = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ETD")) & _
        '                            " WHERE POD_PO_NO = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("PONo")) & "' " & _
        '                            " AND POD_PO_LINE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("Line")) & _
        '                            " AND POD_PRODUCT_DESC = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("ProductDesc")) & "' "
        '                    Common.Insert2Ary(strSqlAry, strSql)
        '                End If
        '            Next
        '        End If

        '        strSql = "UPDATE PO_DETAILS, PO_MSTR " & _
        '                "SET POD_ETD = DateDiff(CURDATE(), POM_CREATED_DATE) " & _
        '                "WHERE POM_PO_INDEX = " & intPOIndex & " AND POM_B_COY_ID = POD_COY_ID " & _
        '                "AND POM_PO_NO = POD_PO_NO AND ADDDATE(POM_CREATED_DATE, POD_ETD) < CURDATE() "

        '        Common.Insert2Ary(strSqlAry, strSql)

        '        strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.HeldBy & _
        '        ",POM_STATUS_CHANGED_BY='" & strUserID & "',POM_STATUS_CHANGED_ON=" & _
        '        Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex
        '        Common.Insert2Ary(strSqlAry, strSql)
        '        strSql = "UPDATE PR_APPROVAL SET PRA_AO_REMARK='" & Common.Parse(strAORemark) & "',PRA_ACTION_DATE=" & _
        '       Common.ConvertDate(Now) & " WHERE PRA_PR_INDEX=" & intPOIndex & " AND PRA_SEQ=" & intCurrentSeq
        '        Common.Insert2Ary(strSqlAry, strSql)
        '        'objPR2.updateAOAction(intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")

        '        'Dim objBCM As New BudgetControl
        '        'objBCM.BCMCalc("PR", strPRNo, EnumBCMAction.RejectPR, strSqlAry)
        '        'objBCM = Nothing

        '        If objDb.BatchExecute(strSqlAry) Then
        '            Dim objMail As New Email
        '            Dim objPR As New PR
        '            Dim strName As String
        '            strName = objPR.getRequestorName("PO", strPONo, strCoyID)
        '            objMail.sendNotification(EmailType.POHeld, strUserID, strCoyID, "", strPONo, strName)
        '            sendMailToPrevAOHold(strPONo, intPOIndex, intCurrentSeq, strName)
        '            objMail = Nothing
        '            objPR = Nothing
        '            strMsg = "Purchase Order Number " & strPONo & " has been put on hold."
        '            '//SEND MAIL TO BUYER
        '        Else
        '            strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        '        End If
        '        Return strMsg

        '    Else
        '        Return strMsg
        '    End If
        'End Function

        Function ApprovePO(ByVal userid As String, ByVal companyid As String, ByVal strPONo As String, ByVal intPOIndex As Long, ByVal intCurrentSeq As Integer,
        ByVal blnHighestLevel As Boolean, ByVal strAORemark As String,
        ByVal strBuyer As String, ByVal blnRelief As Boolean, ByVal strApprType As String, ByVal strSCoyID As String, Optional ByVal dsPOD As DataSet = Nothing) As String
            Dim strSql, strSql1 As String
            Dim strSqlAry(0) As String
            Dim strCoyID, strMsg, strPO, strVendor, strLoginUser As String
            Dim intPOStatus As Integer
            Dim strvendorname As String
            Dim strMsg1 As String
            Dim strVendorNameList As String
            Dim arrPOInfo, arrPo As Array
            Dim intLowBound, intUpBound, intLoop As Integer
            Dim strVen, strVenList As String
            Dim objPR2 As New PurchaseReq2

            If strApprType = "1" Then
                strApprType = "approved"
            Else
                strApprType = "endorsed"
            End If
            strCoyID = companyid
            strLoginUser = userid  'is a AO
            strSql = "SELECT POM_PO_STATUS,POM_STATUS_CHANGED_BY,POM_S_COY_ID,POM_S_COY_NAME, POM_BUYER_NAME FROM PO_MSTR WHERE POM_PO_Index=" & intPOIndex
            Dim tDS As DataSet = objDb.FillDs(strSql)

            If tDS.Tables(0).Rows.Count > 0 Then

                intPOStatus = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS")
                strVendor = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_S_COY_ID"))
                If intPOStatus = POStatus_new.Cancelled Or intPOStatus = POStatus_new.CancelledBy Then
                    strMsg = "The PO has already been cancelled. Approving of this PO is not allowed. "
                ElseIf intPOStatus = POStatus_new.RejectedBy Then
                    strMsg = "You have already rejected this PO. Approving of this PO is not allowed. "
                ElseIf intPOStatus = POStatus_new.Approved Then
                    strMsg = "You have already approved/endorsed this PO."
                End If
            End If

            If dsPOD Is Nothing Then
                Dim intEDD As Integer
                Dim PO_NO As String
                strSql = "SELECT PO_DETAILS.POD_ETD AS POD_ETD, PO_MSTR.POM_PO_NO AS POM_PO_NO FROM PO_DETAILS, PO_MSTR WHERE POM_PO_INDEX = " & intPOIndex & " AND POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO"
                Dim tDS2 As DataSet = objDb.FillDs(strSql)
                Dim rowi As Integer
                If tDS2.Tables(0).Rows.Count > 0 Then
                    For rowi = 0 To tDS2.Tables(0).Rows.Count - 1
                        intEDD = tDS2.Tables(0).Rows(rowi).Item("POD_ETD")
                        PO_NO = tDS2.Tables(0).Rows(rowi).Item("POM_PO_NO")
                        If intEDD < 2 Then
                            strMsg = "Cannot do mass approval as EDD of PO " & PO_NO & " is invalid."
                        End If
                    Next
                End If
            End If

            strCoyID = companyid

            'If (intPOStatus = POStatus_new.PendingApproval Or intPOStatus = POStatus_new.Submitted) And objPR2.isApproved(intPOIndex, intCurrentSeq, strLoginUser, "PO") Then
            '    strMsg = "You have already approved/endorsed this PO. Approving of this PO is not allowed."
            'End If

            If strMsg <> "" Then
                Return strMsg
            End If
            If blnHighestLevel Then
                '//update PO status to Approved/Endorsed, status_changed_by,status_changed_date
                strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.NewPO &
                ",POM_STATUS_CHANGED_BY='" & strLoginUser & "',POM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & ", POM_PO_DATE=" & Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex

                Common.Insert2Ary(strSqlAry, strSql)

                Dim dds As DataTable
                Dim i As Integer
                If dsPOD Is Nothing Then

                Else
                    For i = 0 To dsPOD.Tables(0).Rows.Count - 1
                        If Common.Parse(Trim(dsPOD.Tables(0).Rows(i)("ProductCode"))) <> "" And Common.Parse(Trim(dsPOD.Tables(0).Rows(i)("ProductCode"))) <> "&nbsp;" Then
                            strSql = " UPDATE PO_DETAILS " &
                                    " SET POD_ETD = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ETD")) &
                                    " WHERE POD_PO_NO = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("PONo")) & "' " &
                                    " AND POD_PO_LINE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("Line")) &
                                    " AND POD_PRODUCT_CODE = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("ProductCode")) & "' "
                            Common.Insert2Ary(strSqlAry, strSql)
                        Else
                            strSql = " UPDATE PO_DETAILS " &
                                    " SET POD_ETD = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ETD")) &
                                    " WHERE POD_PO_NO = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("PONo")) & "' " &
                                    " AND POD_PO_LINE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("Line")) &
                                    " AND POD_PRODUCT_DESC = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("ProductDesc")) & "' "
                            Common.Insert2Ary(strSqlAry, strSql)
                        End If
                    Next
                End If

                strSql = "UPDATE PO_DETAILS, PO_MSTR " &
                        "SET POD_ETD = DateDiff(CURDATE(), POM_CREATED_DATE) " &
                        "WHERE POM_PO_INDEX = " & intPOIndex & " AND POM_B_COY_ID = POD_COY_ID " &
                        "AND POM_PO_NO = POD_PO_NO AND ADDDATE(POM_CREATED_DATE, POD_ETD) < CURDATE() "

                Common.Insert2Ary(strSqlAry, strSql)

                'Update last transacted price
                strSql = "UPDATE PRODUCT_MSTR, PO_DETAILS, PO_MSTR " &
                        "SET PM_LAST_TXN_PRICE = POD_UNIT_COST, " &
                        "PM_LAST_TXN_PRICE_CURR = POM_CURRENCY_CODE, " &
                        "PM_LAST_TXN_TAX = POD_GST, " &
                        "PM_LAST_TXN_S_COY_ID = '" & strSCoyID & "' " &
                        "WHERE POM_PO_INDEX = '" & intPOIndex & "' AND POM_B_COY_ID = POD_COY_ID " &
                        "AND POM_PO_NO = POD_PO_NO AND PM_S_COY_ID = POM_B_COY_ID " &
                        "AND POD_PRODUCT_CODE = PM_PRODUCT_CODE "
                Common.Insert2Ary(strSqlAry, strSql)

                objPR2.updateAOAction(userid, intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")
                '//send mail to consolidator
                '//return message = PO Approved/endorsed
                strMsg = "PO No. " & strPONo & " has been " & strApprType & ". "
            Else
                'Update the EDD
                Dim i As Integer
                If dsPOD Is Nothing Then

                Else
                    For i = 0 To dsPOD.Tables(0).Rows.Count - 1
                        If Common.Parse(Trim(dsPOD.Tables(0).Rows(i)("ProductCode"))) <> "" And Common.Parse(Trim(dsPOD.Tables(0).Rows(i)("ProductCode"))) <> "&nbsp;" Then
                            strSql = " UPDATE PO_DETAILS " &
                                    " SET POD_ETD = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ETD")) &
                                    " WHERE POD_PO_NO = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("PONo")) & "' " &
                                    " AND POD_PO_LINE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("Line")) &
                                    " AND POD_PRODUCT_CODE = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("ProductCode")) & "' "
                            Common.Insert2Ary(strSqlAry, strSql)
                        Else
                            strSql = " UPDATE PO_DETAILS " &
                                    " SET POD_ETD = " & Common.Parse(dsPOD.Tables(0).Rows(i)("ETD")) &
                                    " WHERE POD_PO_NO = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("PONo")) & "' " &
                                    " AND POD_PO_LINE = " & Common.Parse(dsPOD.Tables(0).Rows(i)("Line")) &
                                    " AND POD_PRODUCT_DESC = '" & Common.Parse(dsPOD.Tables(0).Rows(i)("ProductDesc")) & "' "
                            Common.Insert2Ary(strSqlAry, strSql)
                        End If
                    Next
                End If
                '//update PO status, status_changed_by,status_changed_date
                strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.PendingApproval &
                ",POM_STATUS_CHANGED_BY='" & strLoginUser & "',POM_STATUS_CHANGED_ON=" &
                Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex

                Common.Insert2Ary(strSqlAry, strSql)
                objPR2.updateAOAction(userid, intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")
                '//notify next AO
                '//return message = PO Approved/endorsed
                strMsg = "PO No. " & strPONo & " has been " & strApprType & ". "
            End If
            Dim objUsers As New Users
            objUsers.Log_UserActivity(userid, companyid, strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_ApprovePO, strPONo)
            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                Dim objMail As New Email
                If blnHighestLevel Then
                    Dim strName, strPoList As String
                    strName = tDS.Tables(0).Rows(0).Item("POM_BUYER_NAME")

                    'For intLoop = 0 To intUpBound 'Sending email to vendors
                    '    arrPo = Split(arrPOInfo(intLoop), "!")
                    '    strPoNo = arrPo(0) 'Capture the PO No.
                    '    strVen = arrPo(1)  'Caputre the Vendor code
                    '    If intLoop = 0 Then
                    '        strPoList = strPoNo
                    '    Else
                    '        strPoList = strPoList & ", " & strPoNo
                    '    End If

                    ' objMail.sendNotification(EmailType.PORaised, "", strCoyID, strSCoyID, strPONo, "")
                    'Next
                    'If intUpBound > 0 Then
                    '    strVendorNameList = strVendorNameList & " respectively"
                    'End If
                    'objMail.sendNotification(EmailType.POApproved, strLoginUser, strCoyID, strSCoyID, strPONo, strPoList, strName, strVendorNameList)
                Else
                    '//next ao
                    Dim objPR As New PR
                    objPR.sendMailToAO(userid, companyid, strPONo, intPOIndex, intCurrentSeq + 1, "PO")
                    objPR = Nothing
                End If
            End If

            Return strMsg
        End Function
    End Class
End Namespace

