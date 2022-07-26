'Jules 2015.02.02 Agora Stage 2
Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraNEO
Imports SSO.Component
Imports System.IO

Namespace AgoraNEO

    Public Class CnValue

        Public Cn_Index As String
        Public Cn_No As String
        Public Cn_Date As String
        Public V_Com_Id As String
        Public B_Com_Id As String
        Public Adds As String
        Public Status As String
        Public Remark As String
        Public Ack_Remark As String
        Public Inv_No As String
        Public Inv_Date As String
        Public Cur As String
        Public Inv_Amt As Double
        Public Ship_Amt As Double
        Public Related_Doc As String
        Public Related_Doc_Amt As Double

    End Class

    Public Class CreditNote
        Dim objDb As New EAD.DBCom

        Public Function getRelatedCreditNotes(ByVal strBCoyId As String, ByVal strSCoyId As String, ByVal strInvNo As String) As String
            Dim strsql As String
            Dim ds As DataSet
            strsql = "SELECT * FROM credit_note_mstr WHERE cnm_cn_b_coy_id = '" & strBCoyId & "' AND cnm_cn_s_coy_id = '" & strSCoyId & "' AND cnm_inv_no = '" & strInvNo & "' "

            ds = objDb.FillDs(strsql)
            Dim CN_num As String = ""
            If ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If CN_num = "" Then
                        CN_num = ds.Tables(0).Rows(i).Item("cnm_cn_no")
                    Else
                        CN_num &= "," & ds.Tables(0).Rows(i).Item("cnm_cn_no")
                    End If
                Next
            End If
            'CN_num = Mid(CN_num, 3)
            Return CN_num
        End Function

        Public Function getRelatedCreditNoteTotal(ByVal strBCoyId As String, ByVal strSCoyId As String, ByVal strInvNo As String) As Double
            Dim strsql As String
            Dim ds As DataSet
            'strsql = "SELECT IFNULL(SUM((cnd.cnd_qty * cnd.cnd_unit_cost * (cnd.cnd_gst_rate/100)) " & _
            '"+ cnd.cnd_qty * cnd.cnd_unit_cost),0) AS RelatedCNTotal " & _
            'strsql = "SELECT FORMAT(IFNULL(SUM(cnm.cnm_cn_total),0),2) AS RelatedCNTotal " & _
            '         "FROM credit_note_mstr cnm JOIN credit_note_details cnd " & _
            '         "WHERE cnm.cnm_cn_no = cnd.cnd_cn_no AND cnm.cnm_cn_s_coy_id = cnd.cnd_cn_s_coy_id " & _
            '         "AND cnm.cnm_cn_b_coy_id = '" & strBCoyId & "' AND cnm.cnm_cn_s_coy_id = '" & strSCoyId & "' AND cnm.cnm_inv_no = '" & strInvNo & "' "
            'Issue 7480 - CH - 23 Mar 2015 (No.55)
            strsql = "SELECT FORMAT(IFNULL(SUM(CNM_CN_TOTAL), 0), 2) AS RelatedCNTotal " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "WHERE CNM_INV_NO = '" & Common.Parse(strInvNo) & "' AND CNM_CN_S_COY_ID = '" & strSCoyId & "' "
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0).Item("RelatedCNTotal")
            End If
        End Function

        Public Function getCreditNoteView(ByVal cn_no As String, ByVal inv_no As String, ByVal cn_status As String, ByVal comid As String, ByVal start_date As String, ByVal end_date As String) As DataSet
            Dim ds As DataSet
            Dim strsql, strTemp As String

            strsql = "SELECT CNM_CN_NO,CNM_CREATED_DATE,CNM_INV_NO,CNM_CURRENCY_CODE,CNM_CN_TOTAL,CM_COY_NAME,CNM_CN_S_COY_ID,CNM_CN_B_COY_ID, " &
                    "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=CNM_CN_STATUS AND STATUS_TYPE='CN') AS STATUS_DESC " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID=CNM_CN_B_COY_ID " &
                    "WHERE CNM_CN_S_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "

            If cn_no <> "" Then
                strTemp = Common.BuildWildCard(cn_no)
                strsql = strsql & "  and CNM_CN_NO" & Common.ParseSQL(strTemp)
            End If

            If inv_no <> "" Then
                strTemp = Common.BuildWildCard(inv_no)
                strsql = strsql & "  and CNM_INV_NO" & Common.ParseSQL(strTemp)
            End If

            If cn_status <> "" Then
                strsql = strsql & " And CNM_CN_STATUS in(" & cn_status & ")"
            End If

            'If comid <> "" Then
            '    strTemp = Common.BuildWildCard(comid)
            '    strsql = strsql & " and CM_COY_NAME" & Common.ParseSQL(strTemp)
            'End If

            'If start_date <> "" And end_date <> "" Then
            '    strsql = strsql & " AND CNM_CREATED_DATE BETWEEN " & Common.ConvertDate(start_date & " 00:00:00") & _
            '    " AND " & Common.ConvertDate(end_date & " 23:59:59")
            'End If

            If start_date <> "" Then
                strsql &= "AND DNM_CREATED_DATE >= " & Common.ConvertDate(start_date) & " "
            End If

            If end_date <> "" Then
                strsql &= "AND DNM_CREATED_DATE <= " & Common.ConvertDate(end_date & " 23:59:59.000") & " "
            End If

            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function Update_CreditNoteMstr(ByVal ds As DataSet, ByRef strInvSuccess As String, ByRef strInvFail As String, ByRef strMsg As String) As Boolean

            Dim objGlobal As New AppGlobals
            Dim CreditNote_num As String
            Dim prefix As String
            Dim i, J As Integer
            Dim strPONo As String
            Dim strBCoyID, strSCoyID, strLoginUser, strInvNo As String
            Dim decTotal, decInvTotal, decLatestAmt, decOutAmt As Decimal
            Dim blnAllowInv As Boolean = True
            Dim DS_InvDetails As DataSet
            Dim dr As DataRow
            Dim objMail As New Email
            Dim strSql As String
            Dim intIncrementNo = 0
            Dim blnQty, blnPrice As Boolean
            strSCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            Dim strarray(0) As String
            '09/07/2015 - Stage 2 Issue - CH - Change CN to CN_EPROC
            strSql = " SET @DUPLICATE_CHK = ''; SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'CN_EPROC' "
            Common.Insert2Ary(strarray, strSql)

            'For i = 0 To ds.Tables(0).Rows.Count - 1

            strSql = ""
            With ds.Tables(0).Rows(0)

                strBCoyID = .Item("b_com_id")
                strPONo = .Item("po_no")
                strInvNo = .Item("inv_no")
                decTotal = CDec(.Item("total"))
                decInvTotal = CDec(.Item("inv_total"))

                intIncrementNo = 1
                '09/07/2015 - Stage 2 Issue - CH - Change CN to CN_EPROC
                CreditNote_num = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                   & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                   & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'CN_EPROC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "
                '09/07/2015 - Stage 2 Issue - CH - Change CN to CN_EPROC
                prefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'CN_EPROC') "

                strSql = "SELECT '*' From Credit_Note_Mstr Where CNM_CN_NO=" & CreditNote_num & " AND CNM_CN_S_COY_ID='" & strSCoyID & "'"
                If objDb.Exist(strSql) Then
                    Return False
                End If

                If blnAllowInv Then
                    '//for audit trail
                    Dim objUsers As New Users
                    objUsers.Log_UserActivityNew(strLoginUser, strSCoyID, strarray, WheelModule.Fulfillment, WheelUserActivity.V_CreditNote, CreditNote_num)
                    objUsers = Nothing

                    strSql = "SELECT * FROM PO_MSTR WHERE POM_B_COY_ID='" & strBCoyID & "' and POM_S_COY_ID = '" & strSCoyID & "' AND  POM_PO_NO='" & strPONo & "'"
                    Dim DS_Addr As DataSet = objDb.FillDs(strSql)
                    If DS_Addr.Tables(0).Rows.Count > 0 Then

                        strSql = "INSERT INTO credit_note_mstr (CNM_CN_B_COY_ID,CNM_CN_S_COY_ID,CNM_CN_NO,CNM_CN_DATE,CNM_CN_TYPE,CNM_ADDR_LINE1,CNM_ADDR_LINE2,CNM_ADDR_LINE3,CNM_POSTCODE,CNM_CITY," &
                                "CNM_STATE,CNM_COUNTRY,CNM_INV_NO,CNM_CURRENCY_CODE,CNM_EXCHANGE_RATE,CNM_REMARKS,CNM_CN_STATUS,CNM_CREATED_BY,CNM_CREATED_DATE,CNM_CN_TOTAL) VALUES " &
                                "('" & strBCoyID & "','" & strSCoyID & "'," & CreditNote_num & "," & Common.ConvertDate(Date.Today) & ", '" &
                                .Item("cn_type") & "',"

                        'Issue 7480 - CH - 25 Mar 2015 (No.61)
                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If

                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If

                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If

                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If

                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If

                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If

                        If DS_Addr.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim <> "" Then
                            strSql &= "'" & DS_Addr.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim & "',"
                        Else
                            strSql &= "'',"
                        End If
                        '---------------------------------------------------

                        strSql &= "'" & .Item("inv_no") & "','" & .Item("currency") & "'," & Common.Parse(.Item("exchange_rate")) & ",'" & Common.Parse(.Item("remark")) & "', " &
                                  "'1','" & strLoginUser & "'," & Common.ConvertDate(Date.Now) & ", " & Common.Parse(.Item("total")) & ")"
                    End If
                    Common.Insert2Ary(strarray, strSql)

                    J = 0
                    For Each dr In ds.Tables(1).Rows
                        'Issue 7480 - CH - 23 Mar 2015 (No.42)
                        strSql = "INSERT INTO credit_note_details (CND_CN_S_COY_ID,CND_CN_NO,CND_CN_LINE,CND_INV_LINE,CND_QTY,CND_UNIT_COST,CND_GST_RATE,CND_GST_INPUT_TAX_CODE,CND_GST_OUTPUT_TAX_CODE,CND_REMARKS) VALUES " &
                                 "('" & strSCoyID & "'," & CreditNote_num & "," & J + 1 & "," & ds.Tables(1).Rows(J).Item("inv_line_no") & "," &
                                 ds.Tables(1).Rows(J).Item("qty") & "," & ds.Tables(1).Rows(J).Item("unit_price") & ",'" & ds.Tables(1).Rows(J).Item("gst_rate") & "','','" &
                                 ds.Tables(1).Rows(J).Item("output_tax_code") & "','" & Common.Parse(ds.Tables(1).Rows(J).Item("remarks")) & "')"
                        Common.Insert2Ary(strarray, strSql)
                        J += 1
                    Next

                    'Issue 7480 - CH - 23 Mar 2015 (No.35)
                    ' delete COMPANY_DOC_ATTACHMENT table
                    strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
                    strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "AND CDA_DOC_TYPE = 'CN' "
                    strSql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
                    Common.Insert2Ary(strarray, strSql)

                    strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
                    strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "AND CDA_DOC_TYPE = 'CN' "
                    strSql &= "AND CDA_DOC_NO = " & CreditNote_num & " "
                    Common.Insert2Ary(strarray, strSql)

                    ' update COMPANY_DOC_ATTACHMENT_TEMP table
                    strSql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
                    strSql &= "CDA_DOC_NO = " & CreditNote_num & " "
                    strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "AND CDA_DOC_TYPE = 'CN' "
                    strSql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
                    Common.Insert2Ary(strarray, strSql)

                    ' insert COMPANY_DOC_ATTACHMENT table
                    strSql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
                    strSql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
                    strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "AND CDA_DOC_TYPE = 'CN' "
                    strSql &= "AND CDA_DOC_NO = " & CreditNote_num & " "
                    Common.Insert2Ary(strarray, strSql)

                    ' delete COMPANY_DOC_ATTACHMENT_TEMP table
                    strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
                    strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "AND CDA_DOC_TYPE = 'CN' "
                    strSql &= "AND CDA_DOC_NO = " & CreditNote_num & " "
                    Common.Insert2Ary(strarray, strSql)

                    Dim objCompany As New Companies
                    Dim strInvAppr As String = objCompany.GetInvApprMode(strBCoyID)

                    If i = 0 Then
                        strSql = " SET @T_NO = " & CreditNote_num & "; "
                        Common.Insert2Ary(strarray, strSql)
                    Else
                        strSql = " SET @T_NO = CONCAT(CONCAT(@T_NO,',')," & CreditNote_num & "); "
                        Common.Insert2Ary(strarray, strSql)
                    End If
                    '09/07/2015 - Stage 2 Issue - CH - Change CN to CN_EPROC
                    strSql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'CN_EPROC' "
                    Common.Insert2Ary(strarray, strSql)
                End If
            End With

            'CH - Issue 7480 (13 Apr 2015) : Current Amt + CN Related Total Amt cannot bigger than Invoice total amt 
            'Check before submit CN
            'Dim decLatestAmt As Decimal
            decLatestAmt = getRelatedCreditNoteTotal(strBCoyID, strSCoyID, strInvNo)
            If decTotal > (decInvTotal - decLatestAmt) Then
                strMsg = "ErrorAmt"
                Return False
            End If

            'CH - 13 Apr 2015 : Check outstanding Qty & Price of each item
            For i = 0 To ds.Tables(1).Rows.Count - 1
                chkCnItemQtyPrice(strInvNo, ds.Tables(1).Rows(i).Item("inv_line_no"), ds.Tables(1).Rows(i).Item("qty"), ds.Tables(1).Rows(i).Item("unit_price"), blnQty, blnPrice)
                If blnQty = False Then
                    strMsg &= "<li>Item " & ds.Tables(1).Rows(i).Item("inv_line_no") & ". Total of all Credit Note Item Qty has exceed invoice item Qty.<ul type='disc'></ul></li>"
                    'intMsg = 2
                    Return False
                End If
                If blnPrice = False Then
                    strMsg &= "<li>Item " & ds.Tables(1).Rows(i).Item("inv_line_no") & ". Total of all Credit Note Item Unit Price has exceed invoice item Unit Price.<ul type='disc'></ul></li>"
                    'intMsg = 3
                    Return False
                End If

                If blnQty = True And blnPrice = True Then
                    decOutAmt = getCnLineItemAmt(strInvNo, ds.Tables(1).Rows(i).Item("inv_line_no"))
                    If (ds.Tables(1).Rows(i).Item("amt") + ds.Tables(1).Rows(i).Item("gst_amt")) > decOutAmt Then
                        strMsg &= "<li>Item " & ds.Tables(1).Rows(i).Item("inv_line_no") & ". Total of all Credit Note Item Amount has exceed invoice item Amount.<ul type='disc'></ul></li>"
                        'intMsg = 3
                        Return False
                    End If
                End If
            Next

            'Next

            If blnAllowInv Then
                Dim strTrans As String = ""
                If objDb.BatchExecuteForDup(strarray, , strTrans, "T_NO") Then
                    If strTrans <> "Generated" Then
                        strInvSuccess = strTrans

                        strBCoyID = ""
                        For i = 0 To ds.Tables(0).Rows.Count - 1
                            strSql = ""

                            If InStr(strBCoyID, ds.Tables(0).Rows(i).Item("b_com_id")) > 0 Then
                                Continue For
                            End If

                            If strBCoyID = "" Then
                                strBCoyID = ds.Tables(0).Rows(i).Item("b_com_id")
                            Else
                                strBCoyID = strBCoyID & "," & ds.Tables(0).Rows(i).Item("b_com_id")
                            End If
                            'Agora Gst Stage 2 - CH - 11 Feb 2015
                            'objMail.sendNotification(EmailType.FMIncomingCN, strLoginUser, ds.Tables(0).Rows(i).Item("b_com_id"), strSCoyID, CreditNote_num, strInvNo) 
                            objMail.sendNotification(EmailType.FMIncomingCN, strLoginUser, ds.Tables(0).Rows(i).Item("b_com_id"), strSCoyID, strTrans, strInvNo)
                        Next
                    Else
                        strInvSuccess = ""
                    End If
                End If
            Else
            End If
            Return True
        End Function

        Public Function getCNTempAttach(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E", Optional ByVal strCompId As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If strCompId = "" Then
                strCompId = HttpContext.Current.Session("CompanyId")
            End If

            'Issue 7480 - CH - 23 Mar 2015
            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDA_COY_ID = '" & strCompId & "' "
            strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'CN' "
            strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "
            ds = objDb.FillDs(strsql)
            getCNTempAttach = ds
        End Function

        'Issue 7480 - CH - 23 Mar 2015 (No.35)
        Public Sub delCNAttachTemp(Optional ByVal intIndex As Integer = 0, Optional ByVal strDocNo As String = "", Optional ByVal strConnStr As String = Nothing)
            Dim strsql As String
            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "

            If intIndex > 0 Or strDocNo <> "" Then
                strsql &= "WHERE "

                If intIndex > 0 Then
                    strsql &= " CDA_ATTACH_INDEX = " & intIndex
                End If
                If strDocNo <> "" Then
                    strsql &= " CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'CN' AND CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                End If
            End If

            objDb.Execute(strsql)

        End Sub

        'Issue 7480 - CH - 23 Mar 2015 (No.35)
        Function getCNAttachment(ByVal strCNNo As String, ByVal strSCoyID As String) As DataSet
            Dim ds As DataSet
            Dim strSql As String

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strSCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strCNNo) & "' AND CDA_DOC_TYPE='CN'"
            ds = objDb.FillDs(strSql)

            Return ds
        End Function

        'Public Function deleteCNAttachment(ByVal intIndex As Integer)
        '    Dim strsql As String
        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
        '    objDb.Execute(strsql)
        'End Function

        Function getCebitNoteTracking(ByVal strCnNo As String, ByVal strVendor As String, ByVal strCurr As String, ByVal strStatus As String, Optional ByVal strInvNo As String = "",
                    Optional ByVal strStartDate As String = "", Optional ByVal strEndDate As String = "", Optional ByVal strAckSDate As String = "", Optional ByVal strAckEDate As String = "") As DataSet
            Dim strSql, strCoyID, strUserID As String
            Dim strTemp As String
            Dim ds As DataSet

            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")
            'strSql = "SELECT CNM_CN_INDEX, CNM_CN_NO, CNM_CN_DATE, CNM_CREATED_DATE, CNM_INV_NO, CM_COY_NAME, CNM_CURRENCY_CODE, CNM_CN_B_COY_ID, CNM_CN_S_COY_ID, " & _
            '        "CNM_CN_STATUS, SUM(CND_QTY * CND_UNIT_COST) AS AMOUNT " & _
            '        "FROM CREDIT_NOTE_MSTR " & _
            '        "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_S_COY_ID = CND_CN_S_COY_ID AND CNM_CN_NO = CND_CN_NO " & _
            '        "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CNM_CN_S_COY_ID " & _
            '        "WHERE CNM_CN_B_COY_ID = '" & strCoyID & "' AND CNM_CN_STATUS IN (" & strStatus & ") "

            'Jules 2019.01.02 - Allow only those involved in the workflow to see the CN.
            'strSql = "SELECT CNM_CN_INDEX, CNM_CN_NO, CNM_CN_DATE, CNM_CREATED_DATE, CNM_INV_NO, CM_COY_NAME, CNM_CURRENCY_CODE, CNM_CN_B_COY_ID, CNM_CN_S_COY_ID, " & _
            '        "CNM_CN_STATUS, CNM_CN_TOTAL AS AMOUNT " & _
            '        "FROM CREDIT_NOTE_MSTR " & _
            '        "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CNM_CN_S_COY_ID " & _
            '        "WHERE CNM_CN_B_COY_ID = '" & strCoyID & "' AND CNM_CN_STATUS IN (" & strStatus & ") "
            strSql = "SELECT DISTINCT CNM_CN_INDEX, CNM_CN_NO, CNM_CN_DATE, CNM_CREATED_DATE, CNM_INV_NO, CM_COY_NAME, CNM_CURRENCY_CODE, CNM_CN_B_COY_ID, CNM_CN_S_COY_ID, " &
                    "CNM_CN_STATUS, CNM_CN_TOTAL AS AMOUNT " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CNM_CN_S_COY_ID " &
                    "INNER JOIN INVOICE_MSTR ON IM_INVOICE_NO=CNM_INV_NO AND CNM_CN_S_COY_ID=IM_S_COY_ID " &
                    "INNER JOIN FINANCE_APPROVAL ON FA_INVOICE_INDEX=IM_INVOICE_INDEX " &
                    "WHERE CNM_CN_B_COY_ID = '" & strCoyID & "' AND CNM_CN_STATUS IN (" & strStatus & ") " &
                    "AND (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_RELIEF_IND='O')) "
            'End modification.

            If strCnNo <> "" Then
                strTemp = Common.BuildWildCard(strCnNo)
                strSql &= " AND CNM_CN_NO" & Common.ParseSQL(strTemp)
            End If

            If strInvNo <> "" Then
                strTemp = Common.BuildWildCard(strInvNo)
                strSql &= " AND CNM_INV_NO" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" And strEndDate <> "" Then
                strSql &= " AND CNM_CREATED_DATE BETWEEN " & Common.ConvertDate(strStartDate & " 00:00:00")
                strSql &= " AND " & Common.ConvertDate(strEndDate & " 23:59:59")
            End If

            If strAckSDate <> "" And strAckEDate <> "" Then
                strSql &= " AND CNM_STATUS_CHANGED_ON BETWEEN " & Common.ConvertDate(strAckSDate & " 00:00:00")
                strSql &= " AND " & Common.ConvertDate(strAckEDate & " 23:59:59")
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql &= " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strCurr <> "" Then
                strSql &= " AND CNM_CURRENCY_CODE = '" & strCurr & "' "
            End If

            'If strStatus = "3,4" Then 'Payment
            '    'Previous DN Approval
            '    'strSql &= "AND ("
            '    'strSql &= "DNM_DN_STATUS IN (3) " 'Approved
            '    'strSql &= " AND DNM_DN_INDEX IN ("
            '    'strSql &= " SELECT DNA_DN_INDEX FROM DN_APPROVAL WHERE (DNA_SEQ <= DNA_AO_ACTION OR IM_INVOICE_STATUS IN ('3','4')) AND "
            '    'strSql &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
            '    'strSql &= ")"
            '    'strSql &= ") AND IM_FOLDER = 0 "
            'Else
            '    strSql &= "AND DNM_DN_STATUS IN (" & IIf(strStatus = "1", "1,2", strStatus) & ") "

            '    If strStatus = DNStatus.NewDN Or strStatus = DNStatus.PendingAppr Then
            '        strSql &= " AND DNM_DN_INDEX IN ("
            '        strSql &= " SELECT DNA_DN_INDEX FROM DN_APPROVAL WHERE DNA_SEQ - 1 = DNA_AO_ACTION AND "
            '        strSql &= " (DNA_AO = '" & strUserID & "' OR (DNA_A_AO = '" & strUserID & "' AND DNA_RELIEF_IND='O'))"

            '        If strStatus = DNStatus.PendingAppr Then
            '            strSql &= " AND (DNA_AGA_TYPE = 'FM' OR (DNA_AGA_TYPE = 'FO' AND DNA_SEQ > DNA_AO_ACTION))"
            '        End If

            '        strSql &= ")"
            '    Else
            '        strSql &= " AND DNM_DN_INDEX IN ("
            '        strSql &= " SELECT DNA_DN_INDEX FROM DN_APPROVAL WHERE DNA_SEQ <= DNA_AO_ACTION AND "
            '        strSql &= " (DNA_AO = '" & strUserID & "' OR (DNA_A_AO = '" & strUserID & "' AND DNA_RELIEF_IND='O'))"

            '        strSql &= ")"
            '    End If
            'End If

            strSql &= " GROUP BY CNM_CN_INDEX "
            'If dteDateFr <> "" And dteDateTo <> "" Then
            '    strSql_1 &= " AND POM_PO_Date BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00")
            '    strSql_1 &= " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            'End If

            ds = objDb.FillDs(strSql)

            objDb = Nothing
            Return ds
        End Function

        Public Function getCnMstr(ByVal item As CnValue)
            Dim strSql, strTempAddr, strTempDoc As String
            Dim strTempAmt As Double
            Dim ds As DataSet
            Dim dsDoc As DataSet
            Dim i As Integer
            Dim objGlobal As New AppGlobals

            'Issue 7480 - CH - 25 Mar 2015 (No.64) - Add IM_CREATED_ON
            strSql = "SELECT DISTINCT CNM_CN_INDEX, CNM_CN_NO, CNM_CN_DATE, CNM_ADDR_LINE1, CNM_ADDR_LINE2, CNM_ADDR_LINE3, CNM_POSTCODE, CNM_CN_STATUS, " &
                    "CNM_CITY, CNM_STATE, CNM_COUNTRY, CNM_CURRENCY_CODE, CNM_REMARKS, CNM_ACK_REMARKS, IM_INVOICE_NO, IM_CREATED_ON, IM_PAYMENT_DATE, IM_INVOICE_TOTAL, IM_SHIP_AMT " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "INNER JOIN INVOICE_MSTR ON CNM_INV_NO = IM_INVOICE_NO AND CNM_CN_S_COY_ID = IM_S_COY_ID " &
                    "WHERE CNM_CN_NO = '" & item.Cn_No & "' AND CNM_CN_S_COY_ID = '" & item.V_Com_Id & "'"

            ds = objDb.FillDs(strSql)

            If ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows(0).Item("CNM_ADDR_LINE1").ToString.Trim <> "" Then
                    strTempAddr = ds.Tables(0).Rows(0).Item("CNM_ADDR_LINE1").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("CNM_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("CNM_ADDR_LINE2").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("CNM_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("CNM_ADDR_LINE3").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("CNM_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("CNM_POSTCODE").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("CNM_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("CNM_CITY").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("CNM_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & objGlobal.getCodeDesc(CodeTable.State, ds.Tables(0).Rows(0).Item("CNM_STATE").ToString.Trim)
                End If

                If ds.Tables(0).Rows(0).Item("CNM_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & objGlobal.getCodeDesc(CodeTable.Country, ds.Tables(0).Rows(0).Item("CNM_COUNTRY").ToString.Trim)
                End If

                item.Cn_Index = ds.Tables(0).Rows(0).Item("CNM_CN_INDEX").ToString.Trim
                item.Cn_No = ds.Tables(0).Rows(0).Item("CNM_CN_NO").ToString.Trim
                item.Cn_Date = Common.parseNull(ds.Tables(0).Rows(0).Item("CNM_CN_DATE"), Date.Today)
                item.Adds = strTempAddr
                item.Ack_Remark = Common.parseNull(ds.Tables(0).Rows(0).Item("CNM_ACK_REMARKS"))
                item.Remark = Common.parseNull(ds.Tables(0).Rows(0).Item("CNM_REMARKS"))
                item.Status = Common.parseNull(ds.Tables(0).Rows(0).Item("CNM_CN_STATUS"))
                item.Inv_No = ds.Tables(0).Rows(0).Item("IM_INVOICE_NO").ToString.Trim
                'Issue 7480 - CH - 25 Mar 2015 (No.64)
                'item.Inv_Date = Common.parseNull(ds.Tables(0).Rows(0).Item("IM_PAYMENT_DATE"), Date.Today)
                item.Inv_Date = Common.parseNull(ds.Tables(0).Rows(0).Item("IM_CREATED_ON"), Date.Today)
                item.Cur = ds.Tables(0).Rows(0).Item("CNM_CURRENCY_CODE").ToString.Trim
                item.Inv_Amt = ds.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL").ToString.Trim
                item.Ship_Amt = Common.parseNull(ds.Tables(0).Rows(0).Item("IM_SHIP_AMT"), 0)

                'item.Related_Doc = ds.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim
                'item.Related_Doc_Amt = ds.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim

                strTempDoc = ""
                strTempAmt = 0
                dsDoc = getRelatedCnNo(item.Inv_No, item.V_Com_Id, item.Cn_No)
                If dsDoc.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsDoc.Tables(0).Rows.Count - 1
                        If strTempDoc <> "" Then
                            'Issue 7480 - CH - 25 Mar 2015 (No.60)
                            strTempDoc &= ", " & dsDoc.Tables(0).Rows(i)("CNM_CN_NO")
                        Else
                            'Issue 7480 - CH - 25 Mar 2015 (No.60)
                            strTempDoc &= dsDoc.Tables(0).Rows(i)("CNM_CN_NO")
                        End If

                        strTempAmt = strTempAmt + dsDoc.Tables(0).Rows(i)("AMOUNT")
                    Next
                    item.Related_Doc = strTempDoc
                    item.Related_Doc_Amt = strTempAmt
                Else
                    item.Related_Doc = ""
                    item.Related_Doc_Amt = 0
                End If
            End If
            objGlobal = Nothing

        End Function

        Public Function getCnDetail(ByVal strCnNo As String, ByVal strVComId As String) As DataSet
            Dim strSql As String

            'Issue 7480 - CH - 23 Mar 2015 (No.38)
            strSql = "SELECT CREDIT_NOTE_DETAILS.*, CREDIT_NOTE_MSTR.*, INVOICE_DETAILS.*, " &
                    "CASE WHEN IM_GST_INVOICE = 'Y' THEN IFNULL(ID_GST,0) ELSE 0 END AS TAX, " &
                    "CASE WHEN IM_GST_INVOICE = 'Y' THEN " &
                    "IF(CND_GST_RATE <> 'EX', CONCAT(CODE_DESC, ' (', CAST(ID_GST AS UNSIGNED), '%)'), CODE_DESC)  ELSE 'N/A' END AS GST_RATE " &
                    "FROM CREDIT_NOTE_DETAILS " &
                    "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                    "INNER JOIN INVOICE_MSTR ON CNM_INV_NO = IM_INVOICE_NO AND CNM_CN_S_COY_ID = IM_S_COY_ID " &
                    "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CNM_CN_B_COY_ID " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CND_GST_RATE " &
                    "WHERE CNM_CN_NO = '" & strCnNo & "' AND CNM_CN_S_COY_ID = '" & strVComId & "'"

            getCnDetail = objDb.FillDs(strSql)

        End Function

        Public Function getRelatedCnNo(ByVal strInvNo As String, ByVal strSCoyId As String, Optional ByVal strCnNo As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet

            strSql = "SELECT CNM_CN_NO, SUM(CND_QTY * CND_UNIT_COST) AS AMOUNT " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "INNER JOIN CREDIT_NOTE_DETAILS ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                    "WHERE CNM_INV_NO = '" & strInvNo & "' AND CNM_CN_S_COY_ID = '" & strSCoyId & "' "

            If strCnNo <> "" Then
                strSql &= " AND CNM_CN_NO <> '" & strCnNo & "' "
            End If

            strSql &= "GROUP BY CNM_CN_NO ORDER BY CNM_CN_NO "

            ds = objDb.FillDs(strSql)
            getRelatedCnNo = ds

        End Function

        Public Sub updateCnInfo(ByVal intCnIndex As Long, ByVal strAckRemark As String, Optional ByVal aryTemp As ArrayList = Nothing)
            Dim strSql, strLoginUser As String
            Dim i As Integer

            If strAckRemark <> "" Then
                strSql = "UPDATE CREDIT_NOTE_MSTR SET CNM_ACK_REMARKS = '" & Common.Parse(strAckRemark) & "' " &
                        "WHERE CNM_CN_INDEX=" & intCnIndex
                objDb.Execute(strSql)
            End If

            If Not aryTemp Is Nothing Then
                For i = 0 To aryTemp.Count - 1
                    strSql = "UPDATE CREDIT_NOTE_DETAILS " &
                            "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                            "SET CND_GST_INPUT_TAX_CODE = '" & Common.Parse(aryTemp(i)(1)) & "' " &
                            "WHERE CNM_CN_INDEX = " & intCnIndex & " " &
                            "AND CND_CN_LINE = '" & aryTemp(i)(0) & "'"
                    objDb.Execute(strSql)
                Next
            End If
        End Sub

        Public Function ackCreditNote(ByVal userID As String, ByVal companyID As String, ByVal intCnIndex As Long, ByVal strCNNo As String, ByVal strAckRemark As String, Optional ByVal aryTemp As ArrayList = Nothing) As String
            Dim strSql, strLoginUser, strMsg As String
            Dim intCNStatus As Integer
            Dim strSqlAry(0) As String
            strLoginUser = HttpContext.Current.Session("UserId")

            strSql = "SELECT CNM_CN_STATUS FROM CREDIT_NOTE_MSTR WHERE CNM_CN_INDEX = " & intCnIndex

            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intCNStatus = tDS.Tables(0).Rows(0).Item("CNM_CN_STATUS")
                If intCNStatus = CNStatus.AckCN Then
                    strMsg = "The credit note has already been acknowledged."
                End If
            End If

            If strMsg <> "" Then Return strMsg

            updateCnInfo(intCnIndex, strAckRemark, aryTemp)

            Dim objUsers As New Users
            objUsers.Log_UserActivity(userID, companyID, strSqlAry, WheelModule.CreditNoteMod, WheelUserActivity.FM_AckCN, strCNNo)
            objUsers = Nothing

            strSql = "UPDATE CREDIT_NOTE_MSTR SET CNM_CN_STATUS = " & CNStatus.AckCN & ", CNM_STATUS_CHANGED_BY = '" & strLoginUser & "', CNM_STATUS_CHANGED_ON = NOW() " &
                    "WHERE CNM_CN_INDEX = " & intCnIndex
            Common.Insert2Ary(strSqlAry, strSql)

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                ''//only send mail if transaction successfully created
                'Dim objMail As New Email
                'If blnHighestLevel Then
                '    'objMail.sendNotification(EmailType.IQCApprovedToSK, strSKId, strCoyID, "", strIQCNo, "")
                'Else
                '    'Send Email to next AO
                '    sendMailToNextlvl(strDNNo, intDnIndex, intCurrentSeq + 1)
                'End If
            End If

            Return strMsg
        End Function

        Public Function getInvRelatedCN(ByVal strInvNo As String, Optional ByVal strVCompId As String = "") As String
            Dim ds As DataSet
            Dim strSql As String

            strSql = "SELECT CNM_CN_NO, CNM_CN_B_COY_ID, CNM_CN_S_COY_ID, UM_USER_NAME, CNM_CREATED_DATE, CNM_CN_STATUS, STATUS_DESC, SUM(CND_QTY * CND_UNIT_COST) AS AMOUNT " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "INNER JOIN CREDIT_NOTE_DETAILS ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                    "INNER JOIN STATUS_MSTR ON CNM_CN_STATUS = STATUS_NO AND STATUS_TYPE = 'CN' " &
                    "LEFT JOIN USER_MSTR ON CNM_CREATED_BY = UM_USER_ID AND CNM_CN_S_COY_ID = UM_COY_ID " &
                    "WHERE CNM_CN_S_COY_ID = '" & strVCompId & "' AND CNM_INV_NO = '" & Common.Parse(strInvNo) & "' " &
                    "GROUP BY CNM_CN_NO " 'Issue 7480 - CH - 24 Feb 15

            ds = objDb.FillDs(strSql)
            getInvRelatedCN = strSql
        End Function

        Public Function chkInvPendingAckCN(ByVal strInvNo As String, Optional ByVal strVCompId As String = "") As Boolean
            Dim strSql As String

            strSql = "SELECT * FROM CREDIT_NOTE_MSTR WHERE CNM_INV_NO = '" & Common.Parse(strInvNo) & "' " &
                    "AND CNM_CN_S_COY_ID = '" & Common.Parse(strVCompId) & "' AND CNM_CN_STATUS = '1'"
            If objDb.Exist(strSql) > 0 Then
                chkInvPendingAckCN = True
            Else
                chkInvPendingAckCN = False
            End If
        End Function

        Public Sub chkCnItemQtyPrice(ByVal strInvNo As String, ByVal intItemLine As Integer, ByVal decItemQty As Decimal, ByVal decItemPrice As Decimal, ByRef blnQty As Boolean, ByRef blnPrice As Boolean)
            Dim strSql As String
            Dim dsRelatedCN As DataSet
            Dim dsInv As DataSet
            Dim decCNRelQty, decCNRelPrice, decInvQty, decInvPrice As Decimal

            'Sum total of Qty & Unit Price of Related CN
            strSql = "SELECT SUM(IFNULL(CND_QTY,0)) AS CND_QTY, SUM(IFNULL(CND_UNIT_COST,0)) AS CND_UNIT_COST " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO = CND_CN_NO AND CNM_CN_S_COY_ID = CND_CN_S_COY_ID " &
                    "WHERE CNM_INV_NO = '" & Common.Parse(strInvNo) & "' AND CNM_CN_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND CND_INV_LINE = " & intItemLine & " " &
                    "GROUP BY CNM_INV_NO"
            dsRelatedCN = objDb.FillDs(strSql)

            If dsRelatedCN.Tables(0).Rows.Count > 0 Then
                'decCNRelQty = dsRelatedCN.Tables(0).Rows(0)("CND_QTY")
                decCNRelPrice = dsRelatedCN.Tables(0).Rows(0)("CND_UNIT_COST")
            Else 'There no related CN Doc
                'decCNRelQty = 0
                decCNRelPrice = 0
            End If
            decCNRelQty = 0

            'Get Qty & Unit Price of Inv Doc
            strSql = "SELECT ID_RECEIVED_QTY, ID_UNIT_COST FROM INVOICE_DETAILS " &
                    "WHERE ID_INVOICE_NO = '" & Common.Parse(strInvNo) & "' AND ID_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND ID_INVOICE_LINE = " & intItemLine

            dsInv = objDb.FillDs(strSql)
            decInvQty = dsInv.Tables(0).Rows(0)("ID_RECEIVED_QTY")
            decInvPrice = dsInv.Tables(0).Rows(0)("ID_UNIT_COST")

            If (decItemQty + decCNRelQty) > decInvQty Then
                blnQty = False
            Else
                blnQty = True
            End If

            If decItemPrice > decInvPrice Then
                blnPrice = False
            Else
                blnPrice = True
            End If

            'If (decItemPrice + decCNRelPrice) > decInvPrice Then
            '    blnPrice = False
            'Else
            '    blnPrice = True
            'End If

        End Sub

        Public Function getCnLineItemAmt(ByVal strInvNo As String, ByVal intItemLine As Integer) As Decimal
            Dim strSql As String
            Dim decRelCNAmt As Decimal
            Dim decInvAmt As Decimal

            'Sum AMT of Related CN
            strSql = "SELECT IFNULL(SUM(AMT + GSTAMT), 0) FROM " &
                    "(SELECT ROUND((CND_QTY * CND_UNIT_COST), 2) AS AMT, " &
                    "ROUND(((CND_QTY * CND_UNIT_COST) * IF(TAX_PERC = '' OR TAX_PERC IS NULL, 0, TAX_PERC)) / 100, 2) AS GSTAMT " &
                    "FROM CREDIT_NOTE_MSTR " &
                    "INNER JOIN CREDIT_NOTE_DETAILS ON CNM_CN_NO = CND_CN_NO AND CNM_CN_S_COY_ID = CND_CN_S_COY_ID " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CNM_CN_B_COY_ID " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CND_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE CNM_INV_NO = '" & Common.Parse(strInvNo) & "' AND CNM_CN_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND CND_INV_LINE = " & intItemLine & " ) tb"
            decRelCNAmt = objDb.GetVal(strSql)

            'Get INV Line item AMT
            strSql = "SELECT ROUND((ID_RECEIVED_QTY * ID_UNIT_COST), 2) + " &
                    "ROUND(((ID_RECEIVED_QTY * ID_UNIT_COST) * ID_GST) / 100, 2) " &
                    "FROM INVOICE_DETAILS " &
                    "WHERE ID_INVOICE_NO = '" & Common.Parse(strInvNo) & "' " &
                    "AND ID_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND ID_INVOICE_LINE = " & intItemLine
            decInvAmt = objDb.GetVal(strSql)

            Return decInvAmt - decRelCNAmt
        End Function

    End Class
End Namespace
