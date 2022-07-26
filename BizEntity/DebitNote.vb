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


    Public Class DnValue

        Public Dn_Index As String
        Public Dn_No As String
        Public Dn_Date As String
        Public V_Com_Id As String
        Public B_Com_Id As String
        Public Adds As String
        Public Remark As String
        Public Inv_No As String
        Public Inv_Date As String
        Public Cur As String
        Public Inv_Amt As Double
        Public Ship_Amt As Double
        Public Related_Doc As String
        Public Related_Doc_Amt As Double

    End Class

    Public Class DebitNote
        Dim objDb As New EAD.DBCom

        Public Function getDebitNoteDetail(ByVal doc_no As String, ByVal vcomid As String) As DataSet
            Dim strsql As String

            strsql = "SELECT * FROM debit_note_mstr INNER JOIN debit_note_details " &
                    "WHERE debit_note_mstr.dnm_dn_s_coy_id = debit_note_details.dnd_dn_s_coy_id And debit_note_mstr.dnm_dn_no = debit_note_details.dnd_dn_no " &
                    "AND debit_note_mstr.dnm_dn_b_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' AND debit_note_mstr.dnm_dn_s_coy_id = '" & vcomid & "' " &
                    "AND debit_note_mstr.dnm_inv_no = '" & doc_no & "' "

            getDebitNoteDetail = objDb.FillDs(strsql)
        End Function

        Public Function getDebitNote_byDNNo(ByVal doc_no As String, Optional ByVal comid As String = "", Optional ByVal vcomid As String = "") As DataSet
            Dim strsql As String

            strsql = "SELECT * FROM debit_note_mstr " &
                    "INNER JOIN debit_note_details " &
                    "WHERE(debit_note_mstr.dnm_dn_s_coy_id = debit_note_details.dnd_dn_s_coy_id And debit_note_mstr.dnm_dn_no = debit_note_details.dnd_dn_no) "

            If comid = "" Then
                strsql &= "AND debit_note_mstr.dnm_dn_b_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' AND debit_note_mstr.dnm_dn_s_coy_id = '" & vcomid & "' "
            Else
                strsql &= "AND debit_note_mstr.dnm_dn_b_coy_id = '" & comid & "' AND debit_note_mstr.dnm_dn_s_coy_id = '" & HttpContext.Current.Session("CompanyID") & "' "
            End If
            strsql &= "AND debit_note_mstr.dnm_dn_no = '" & doc_no & "' "
            getDebitNote_byDNNo = objDb.FillDs(strsql)
        End Function

        Public Function getDebitNoteView(ByVal dn_no As String, ByVal inv_no As String, ByVal dn_status As String, ByVal comid As String, ByVal start_date As String, ByVal end_date As String) As DataSet
            Dim ds As DataSet
            Dim strsql, strTemp As String

            strsql = "SELECT DNM_DN_NO,DNM_CREATED_DATE,DNM_INV_NO,DNM_CURRENCY_CODE,DNM_DN_TOTAL,CM_COY_NAME,DNM_DN_S_COY_ID,DNM_DN_B_COY_ID, " &
                    "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=DNM_DN_STATUS AND STATUS_TYPE='DN') AS STATUS_DESC " &
                    "FROM DEBIT_NOTE_MSTR " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID=DNM_DN_B_COY_ID " &
                    "WHERE DNM_DN_S_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' "

            If dn_no <> "" Then
                strTemp = Common.BuildWildCard(dn_no)
                strsql = strsql & "  and DNM_DN_NO" & Common.ParseSQL(strTemp)
            End If

            If inv_no <> "" Then
                strTemp = Common.BuildWildCard(inv_no)
                strsql = strsql & "  and DNM_INV_NO" & Common.ParseSQL(strTemp)
            End If

            If dn_status <> "" Then
                strsql = strsql & " And DNM_DN_STATUS in(" & dn_status & ")"
            End If

            'If comid <> "" Then
            '    strTemp = Common.BuildWildCard(comid)
            '    strsql = strsql & " and CM_COY_NAME" & Common.ParseSQL(strTemp)
            'End If

            'If start_date <> "" And end_date <> "" Then
            '    strsql = strsql & " AND DNM_CREATED_DATE BETWEEN " & Common.ConvertDate(start_date & " 00:00:00") & _
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
        Public Function getRelatedDebitNotes(ByVal strBCoyId As String, ByVal strSCoyId As String, ByVal strInvNo As String) As String
            Dim strsql As String
            Dim ds As DataSet
            strsql = "SELECT * FROM debit_note_mstr WHERE dnm_dn_b_coy_id = '" & strBCoyId & "' AND dnm_dn_s_coy_id = '" & strSCoyId & "' AND dnm_inv_no = '" & strInvNo & "' "

            ds = objDb.FillDs(strsql)
            Dim DN_num As String = ""
            If ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If DN_num = "" Then
                        DN_num = ds.Tables(0).Rows(i).Item("dnm_dn_no")
                    Else
                        DN_num &= "," & ds.Tables(0).Rows(i).Item("dnm_dn_no")
                    End If
                Next
            End If
            'DN_num = Mid(DN_num, 3)
            Return DN_num
        End Function

        Public Function getRelatedDebitNoteTotal(ByVal strBCoyId As String, ByVal strSCoyId As String, ByVal strInvNo As String) As Double
            Dim strsql As String
            Dim ds As DataSet
            'strsql = "SELECT FORMAT(IFNULL(SUM((dnd.dnd_qty * dnd.dnd_unit_cost * (tax_perc/100)) + dnd.dnd_qty * dnd.dnd_unit_cost),0),2) AS RelatedDNTotal " & _
            'strsql = "SELECT FORMAT(IFNULL(SUM(dnm.dnm_dn_total),0),2) AS RelatedDNTotal " & _
            '         "FROM debit_note_mstr dnm JOIN debit_note_details dnd " & _
            '         "INNER JOIN tax ON TAX_COUNTRY_CODE = dnm.dnm_country " & _
            '        "INNER JOIN CODE_MSTR ON CODE_CATEGORY = 'GST' AND TAX_CODE = CODE_ABBR AND CODE_ABBR = dnd_GST_RATE " & _
            '         "WHERE dnm.dnm_dn_s_coy_id = dnd.dnd_dn_s_coy_id AND dnm.dnm_dn_no = dnd.dnd_dn_no " & _
            '         "AND dnm.dnm_dn_b_coy_id = '" & strBCoyId & "' AND dnm.dnm_dn_s_coy_id = '" & strSCoyId & "' AND dnm.dnm_inv_no = '" & strInvNo & "' "
            'Issue 7480 - CH - 23 Mar 2015 (No.55)
            strsql = "SELECT FORMAT(IFNULL(SUM(DNM_DN_TOTAL), 0), 2) AS RelatedDNTotal " &
                    "FROM debit_note_mstr " &
                    "WHERE dnm_inv_no = '" & Common.Parse(strInvNo) & "' AND dnm_dn_s_coy_id = '" & strSCoyId & "' "
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0).Item("RelatedDNTotal")
            End If
        End Function

        Public Function Update_DebitNoteMstr(ByVal userID As String, ByVal companyID As String, ByVal ds As DataSet, ByRef strInvSuccess As String, ByRef strInvFail As String, Optional ByVal isResident As String = "") As Boolean

            Dim objGlobal As New AppGlobals
            Dim DebitNote_num As String
            Dim prefix As String
            Dim i, J As Integer
            Dim strPONo As String
            Dim strBCoyID, strSCoyID, strLoginUser As String
            Dim blnAllowInv As Boolean = True
            Dim DS_InvDetails As DataSet
            Dim dr As DataRow
            Dim objMail As New Email
            Dim strSql As String
            Dim intIncrementNo = 0
            strSCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            Dim strarray(0) As String
            '09/07/2015 - Stage 2 Issue - CH - Change DN to DN_EPROC
            strSql = " SET @DUPLICATE_CHK = ''; SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'DN_EPROC' "
            Common.Insert2Ary(strarray, strSql)

            For i = 0 To ds.Tables(0).Rows.Count - 1

                strSql = ""
                With ds.Tables(0).Rows(i)

                    strBCoyID = .Item("b_com_id")
                    strPONo = .Item("po_no")

                    intIncrementNo = 1
                    '09/07/2015 - Stage 2 Issue - CH - Change DN to DN_EPROC
                    DebitNote_num = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'DN_EPROC' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "
                    '09/07/2015 - Stage 2 Issue - CH - Change DN to DN_EPROC
                    prefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'DN_EPROC') "

                    strSql = "SELECT '*' From Debit_Note_Mstr Where DNM_DN_NO=" & DebitNote_num & " AND DNM_DN_S_COY_ID='" & strSCoyID & "'"
                    If objDb.Exist(strSql) Then
                        Return False
                    End If

                    If blnAllowInv Then
                        '//for audit trail
                        Dim objUsers As New Users
                        objUsers.Log_UserActivityNew(companyID, userID, strarray, WheelModule.Fulfillment, WheelUserActivity.V_DebitNote, DebitNote_num)
                        objUsers = Nothing

                        strSql = "SELECT * FROM PO_MSTR WHERE POM_B_COY_ID='" & strBCoyID & "' and POM_S_COY_ID = '" & strSCoyID & "' AND  POM_PO_NO='" & strPONo & "'"
                        Dim DS_Addr As DataSet = objDb.FillDs(strSql)
                        If DS_Addr.Tables(0).Rows.Count > 0 Then

                            strSql = "INSERT INTO debit_note_mstr (DNM_DN_B_COY_ID,DNM_DN_S_COY_ID,DNM_DN_NO,DNM_DN_DATE,DNM_DN_TYPE,DNM_ADDR_LINE1,DNM_ADDR_LINE2,DNM_ADDR_LINE3,DNM_POSTCODE,DNM_CITY," &
                                    "DNM_STATE,DNM_COUNTRY,DNM_INV_NO,DNM_CURRENCY_CODE,DNM_EXCHANGE_RATE,DNM_REMARKS,DNM_DN_STATUS,DNM_CREATED_BY,DNM_CREATED_DATE,DNM_DN_TOTAL) VALUES " &
                                    "('" & strBCoyID & "','" & strSCoyID & "'," & DebitNote_num & "," & Common.ConvertDate(Date.Today) & ", '" &
                                    .Item("dn_type") & "',"

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
                            '----------------------------------------------------

                            strSql &= "'" & .Item("inv_no") & "','" & .Item("currency") & "'," & Common.Parse(.Item("exchange_rate")) & ",'" & Common.Parse(.Item("remark")) & "', " &
                                      "'1','" & strLoginUser & "'," & Common.ConvertDate(Date.Now) & ", " & Common.Parse(.Item("total")) & ")"
                        End If
                        Common.Insert2Ary(strarray, strSql)

                        J = 0
                        For Each dr In ds.Tables(1).Rows
                            strSql = "INSERT INTO debit_note_details (DND_DN_S_COY_ID,DND_DN_NO,DND_DN_LINE,DND_INV_LINE,DND_QTY,DND_UNIT_COST,DND_GST_RATE,DND_GST_OUTPUT_TAX_CODE,DND_REMARKS) VALUES " &
                                     "('" & strSCoyID & "'," & DebitNote_num & "," & J + 1 & "," & ds.Tables(1).Rows(J).Item("inv_line_no") & "," &
                                     ds.Tables(1).Rows(J).Item("qty") & "," & ds.Tables(1).Rows(J).Item("unit_price") & ",'" & ds.Tables(1).Rows(J).Item("gst_rate") & "','" &
                                     ds.Tables(1).Rows(J).Item("output_tax_code") & "','" & ds.Tables(1).Rows(J).Item("line_remark") & "')"
                            Common.Insert2Ary(strarray, strSql)
                            J += 1
                        Next

                        Dim objCompany As New Companies
                        Dim strInvAppr As String = objCompany.GetInvApprMode(strBCoyID)

                        If strInvAppr = "Y" Then
                            ' insert into DN_APPROVAL
                            Dim objDB As New EAD.DBCom
                            Dim strcond, strtbl As String

                            'Jules 2019.02.07 - Get INV approval workflow assigned to specific buyer.
                            'Jules 2018.10.19 - Replicate Invoice approval workflow.
                            'strtbl = " (PO_DETAILS JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO)  LEFT JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_INDEX = POM_DEPT_INDEX AND CDM_COY_ID = POD_COY_ID "
                            'strcond = " WHERE POD_COY_ID = '" & strBCoyID & "' AND POD_PO_NO = '" & .Item("po_no") & "' AND POM_B_COY_ID='" & strBCoyID & "' "

                            'Dim query As String = "SELECT AGA_GRP_INDEX, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE" &
                            '    " FROM APPROVAL_GRP_FINANCE" &
                            '    " WHERE AGA_GRP_INDEX = " &
                            '    "('" & objDB.Get1ColumnCheckNull(strtbl, "CDM_APPROVAL_GRP_INDEX", strcond) & "')" &
                            '    " ORDER BY AGA_TYPE DESC, AGA_SEQ"
                            'Dim grpIndex = objDB.GetVal("SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE
                            '                            FROM APPROVAL_GRP_FINANCE
                            '                            WHERE AGA_GRP_INDEX = 
                            '                            (
                            '                            SELECT DISTINCT CDM_APPROVAL_GRP_INDEX
                            '                            FROM (PO_DETAILS JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO)  
                            '                            LEFT JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_INDEX = POM_DEPT_INDEX AND CDM_COY_ID = POD_COY_ID 
                            '                            LEFT JOIN APPROVAL_GRP_MSTR ON CDM_APPROVAL_GRP_INDEX = AGM_GRP_INDEX AND AGM_RESIDENT = '" & isResident & "'
                            '                            JOIN APPROVAL_GRP_FINANCE ON AGA_GRP_INDEX = CDM_APPROVAL_GRP_INDEX
                            '                            WHERE POD_COY_ID = '" & strBCoyID & "' 
                            '                            AND POD_PO_NO = '" & .Item("po_no") & "' 
                            '                            AND POM_B_COY_ID='" & strBCoyID & "'
                            '                            AND POM_BUYER_ID = AGA_AO
                            '                            AND AGA_SEQ = 1
                            '                            )")

                            ''Jules 2018.10.25 - Added this for P2P; no approval group found when POM_BUYER_ID = AGA_AO condition in effect.
                            'If grpIndex = "" Then
                            '    grpIndex = objDB.GetVal("SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE
                            '                            FROM APPROVAL_GRP_FINANCE
                            '                            WHERE AGA_GRP_INDEX = 
                            '                            (
                            '                            SELECT DISTINCT CDM_APPROVAL_GRP_INDEX
                            '                            FROM (PO_DETAILS JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO)  
                            '                            LEFT JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = (SELECT CDM_DEPT_CODE FROM COMPANY_DEPT_MSTR WHERE CDM_DEPT_INDEX = POM_DEPT_INDEX) AND CDM_COY_ID = POD_COY_ID 
                            '                            LEFT JOIN APPROVAL_GRP_MSTR ON CDM_APPROVAL_GRP_INDEX = AGM_GRP_INDEX
                            '                            JOIN APPROVAL_GRP_FINANCE ON AGA_GRP_INDEX = CDM_APPROVAL_GRP_INDEX
                            '                            WHERE POD_COY_ID = '" & strBCoyID & "' 
                            '                            AND POD_PO_NO = '" & .Item("po_no") & "' 
                            '                            AND POM_B_COY_ID='" & strBCoyID & "'                                                        
                            '                            AND AGA_SEQ = 1
                            '                            AND AGM_RESIDENT = '" & isResident & "'
                            '                            LIMIT 1
                            '                            )")
                            'End If
                            ''End modification.

                            'Dim query As String = "SELECT AGA_GRP_INDEX, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE" &
                            '    " FROM APPROVAL_GRP_FINANCE" &
                            '    " WHERE AGA_GRP_INDEX = " &
                            '    "('" & grpIndex & "')" &
                            '    " ORDER BY AGA_TYPE DESC, AGA_SEQ"
                            Dim strBuyerID As String = objDB.GetVal("SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO='" & .Item("po_no") & "' AND POM_B_COY_ID='" & strBCoyID & "'")

                            Dim query As String = "SELECT AGA_GRP_INDEX, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE " &
                                                "FROM APPROVAL_GRP_FINANCE " &
                                                "INNER JOIN approval_grp_mstr ON agm_grp_index=aga_grp_index " &
                                                "WHERE agm_type='INV' AND agm_resident='" & isResident & "' AND agm_grp_name LIKE '" & strBuyerID & "%' " &
                                                "ORDER BY AGA_TYPE DESC, AGA_SEQ "
                            'End modification 2019.02.07.

                            Dim dsFinAppGrp As DataSet = objDB.FillDs(query, False)

                            If Not dsFinAppGrp Is Nothing Then
                                If dsFinAppGrp.Tables(0).Rows.Count > 0 Then

                                    Dim dtAO As DataTable = dsFinAppGrp.Tables(0)

                                    For k As Integer = 0 To dtAO.Rows.Count - 1
                                        strSql = "INSERT INTO dn_approval (DNA_DN_INDEX,DNA_AGA_TYPE,DNA_AO,DNA_A_AO,DNA_SEQ,DNA_AO_ACTION, " &
                                                "DNA_APPROVAL_GRP_INDEX,DNA_RELIEF_IND) VALUES (" &
                                                objDB.GetLatestInsertedID("DEBIT_NOTE_MSTR") & "," &
                                                "'" & Common.parseNull(dtAO.Rows(k)("AGA_TYPE")) & "','" & Common.parseNull(dtAO.Rows(k)("AGA_AO")) & "', " &
                                                "'" & Common.parseNull(dtAO.Rows(k)("AGA_A_AO")) & "', " & k + 1 & ",0," & dtAO.Rows(k)("AGA_GRP_INDEX") & ", " &
                                                "'" & Common.parseNull(dtAO.Rows(k)("AGA_RELIEF_IND")) & "') "
                                        Common.Insert2Ary(strarray, strSql)
                                    Next
                                End If
                            Else
                                strSql = "INSERT INTO DN_APPROVAL (DNA_DN_INDEX, DNA_AGA_TYPE, DNA_AO, DNA_A_AO, DNA_SEQ, DNA_AO_ACTION "
                                strSql &= ") SELECT "
                                strSql &= objDB.GetLatestInsertedID("DEBIT_NOTE_MSTR")
                                strSql &= ", 'FO', USERS_USRGRP.UU_USER_ID, '', 1, 0 "
                                strSql &= "FROM USER_GROUP_MSTR INNER JOIN "
                                strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                                strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Officer') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                                strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') LIMIT 1 "
                                Common.Insert2Ary(strarray, strSql)

                                strSql = "INSERT INTO DN_APPROVAL (DNA_DN_INDEX, DNA_AGA_TYPE, DNA_AO, DNA_A_AO, DNA_SEQ, DNA_AO_ACTION "
                                strSql &= ") SELECT "
                                strSql &= objDB.GetLatestInsertedID("DEBIT_NOTE_MSTR")
                                strSql &= ", 'FM', USERS_USRGRP.UU_USER_ID, '', 2, 0 "
                                strSql &= "FROM USER_GROUP_MSTR INNER JOIN "
                                strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                                strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Manager') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                                strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') LIMIT 1 "
                                Common.Insert2Ary(strarray, strSql)
                            End If
                        Else

                            strSql = "INSERT INTO DN_APPROVAL (DNA_DN_INDEX, DNA_AGA_TYPE, DNA_AO, DNA_A_AO, DNA_SEQ, DNA_AO_ACTION "
                            strSql &= ")  SELECT "
                            strSql &= objDb.GetLatestInsertedID("DEBIT_NOTE_MSTR")
                            strSql &= ", 'FO', USERS_USRGRP.UU_USER_ID, '', 1, 0 "
                            strSql &= "FROM USER_MSTR, USER_GROUP_MSTR INNER JOIN "
                            strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                            strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Officer') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                            strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') "
                            strSql &= "AND USER_MSTR.UM_USER_ID = USERS_USRGRP.UU_USER_ID AND USER_MSTR.UM_COY_ID  = USERS_USRGRP.UU_COY_ID AND USER_MSTR.UM_STATUS = 'A' LIMIT 1 "
                            Common.Insert2Ary(strarray, strSql)

                            strSql = "INSERT INTO DN_APPROVAL (DNA_DN_INDEX, DNA_AGA_TYPE, DNA_AO, DNA_A_AO, DNA_SEQ, DNA_AO_ACTION "
                            strSql &= ")  SELECT "
                            strSql &= objDb.GetLatestInsertedID("DEBIT_NOTE_MSTR")
                            strSql &= ", 'FM', USERS_USRGRP.UU_USER_ID, '', 2, 0 "
                            strSql &= "FROM USER_MSTR, USER_GROUP_MSTR INNER JOIN "
                            strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                            strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Manager') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                            strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') AND USER_MSTR.UM_USER_ID = USERS_USRGRP.UU_USER_ID AND USER_MSTR.UM_COY_ID  = USERS_USRGRP.UU_COY_ID AND USER_MSTR.UM_STATUS = 'A' LIMIT 1 "
                            Common.Insert2Ary(strarray, strSql)
                        End If

                        'Issue 7480 - CH - 23 Mar 2015 (No.35)
                        ' delete COMPANY_DOC_ATTACHMENT table
                        strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
                        strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'DN' "
                        strSql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
                        Common.Insert2Ary(strarray, strSql)

                        strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
                        strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'DN' "
                        strSql &= "AND CDA_DOC_NO = " & DebitNote_num & " "
                        Common.Insert2Ary(strarray, strSql)

                        ' update COMPANY_DOC_ATTACHMENT_TEMP table
                        strSql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
                        strSql &= "CDA_DOC_NO = " & DebitNote_num & " "
                        strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'DN' "
                        strSql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
                        Common.Insert2Ary(strarray, strSql)

                        ' insert COMPANY_DOC_ATTACHMENT table
                        strSql = "INSERT INTO company_doc_attachment(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
                        strSql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
                        strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'DN' "
                        strSql &= "AND CDA_DOC_NO = " & DebitNote_num & " "
                        Common.Insert2Ary(strarray, strSql)

                        ' delete COMPANY_DOC_ATTACHMENT_TEMP table
                        strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
                        strSql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'DN' "
                        strSql &= "AND CDA_DOC_NO = " & DebitNote_num & " "
                        Common.Insert2Ary(strarray, strSql)

                        If i = 0 Then
                            strSql = " SET @T_NO = " & DebitNote_num & "; "
                            Common.Insert2Ary(strarray, strSql)
                        Else
                            strSql = " SET @T_NO = CONCAT(CONCAT(@T_NO,',')," & DebitNote_num & "); "
                            Common.Insert2Ary(strarray, strSql)
                        End If
                        '09/07/2015 - Stage 2 Issue - CH - Change DN to DN_EPROC
                        strSql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'DN_EPROC' "
                        Common.Insert2Ary(strarray, strSql)
                    End If
                End With

            Next

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
                            'objMail.sendNotification(EmailType.FOIncomingDN, strLoginUser, ds.Tables(0).Rows(i).Item("b_com_id"), strSCoyID, DebitNote_num, "")
                            objMail.sendNotification(EmailType.FOIncomingDN, strLoginUser, ds.Tables(0).Rows(i).Item("b_com_id"), strSCoyID, strTrans, "")
                        Next
                    Else
                        strInvSuccess = ""
                    End If
                End If
            Else
            End If
            Return True
        End Function

        Public Function getDNTempAttach(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E", Optional ByVal strCompId As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If strCompId = "" Then
                strCompId = HttpContext.Current.Session("CompanyId")
            End If

            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT_TEMP "
            strsql &= "WHERE CDA_COY_ID = '" & strCompId & "' "
            strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'DN' "
            'strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "
            ds = objDb.FillDs(strsql)
            getDNTempAttach = ds
        End Function

        'Issue 7480 - CH - 23 Mar 2015 (No.35)
        Public Sub delDNAttachTemp(Optional ByVal intIndex As Integer = 0, Optional ByVal strDocNo As String = "", Optional ByVal strConnStr As String = Nothing)
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
                    strsql &= " CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'DN' AND CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                End If
            End If

            objDb.Execute(strsql)

        End Sub

        'Issue 7480 - CH - 23 Mar 2015 (No.35)
        Function getDNAttachment(ByVal strDNNo As String, ByVal strSCoyID As String) As DataSet
            Dim ds As DataSet
            Dim strSql As String

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strSCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strDNNo) & "' AND CDA_DOC_TYPE='DN'"
            ds = objDb.FillDs(strSql)

            Return ds
        End Function

        'Public Function deleteDNAttachment(ByVal intIndex As Integer)
        '    Dim strsql As String
        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
        '    objDb.Execute(strsql)
        'End Function

        Function getDebitNoteTracking(ByVal strDnNo As String, ByVal strVendor As String, ByVal strCurr As String, ByVal strStatus As String, Optional ByVal strInvNo As String = "",
            Optional ByVal strStartDate As String = "", Optional ByVal strEndDate As String = "", Optional ByVal strPaySDate As String = "", Optional ByVal strPayEDate As String = "") As DataSet
            Dim strSql, strCoyID, strUserID As String
            Dim strTemp As String
            Dim ds As DataSet

            strCoyID = HttpContext.Current.Session("CompanyId")
            strUserID = HttpContext.Current.Session("UserId")

            'Issue 7480 - CH - 24 Feb 15
            'strSql = "SELECT DNM_DN_INDEX, DNM_DN_NO, DNM_DN_DATE, DNM_INV_NO, CM_COY_NAME, DNM_CURRENCY_CODE, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID, DNM_PAYMENT_TERM, DNM_PAYMENT_DATE, IM_PAYMENT_TERM, " & _
            '        "DNM_DN_STATUS, SUM(DND_QTY * DND_UNIT_COST) AS AMOUNT " & _
            '        "FROM DEBIT_NOTE_MSTR " & _
            '        "INNER JOIN DEBIT_NOTE_DETAILS ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " & _
            '        "INNER JOIN COMPANY_MSTR ON CM_COY_ID = DNM_DN_S_COY_ID " & _
            '        "LEFT JOIN INVOICE_MSTR ON IM_INVOICE_NO = DNM_INV_NO AND IM_S_COY_ID = DNM_DN_S_COY_ID " & _
            '        "WHERE DNM_DN_B_COY_ID = '" & strCoyID & "' "
            strSql = "SELECT DNM_DN_INDEX, DNM_DN_NO, DNM_DN_DATE, DNM_INV_NO, CM_COY_NAME, DNM_CURRENCY_CODE, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID, DNM_PAYMENT_TERM, DNM_PAYMENT_DATE, IM_PAYMENT_TERM, " &
                    "DNM_DN_STATUS, DNM_DN_TOTAL AS AMOUNT " &
                    "FROM DEBIT_NOTE_MSTR " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = DNM_DN_S_COY_ID " &
                    "LEFT JOIN INVOICE_MSTR ON IM_INVOICE_NO = DNM_INV_NO AND IM_S_COY_ID = DNM_DN_S_COY_ID " &
                    "WHERE DNM_DN_B_COY_ID = '" & strCoyID & "' "

            If strDnNo <> "" Then
                strTemp = Common.BuildWildCard(strDnNo)
                strSql &= " AND DNM_DN_NO" & Common.ParseSQL(strTemp)
            End If

            If strInvNo <> "" Then
                strTemp = Common.BuildWildCard(strInvNo)
                strSql &= " AND DNM_INV_NO" & Common.ParseSQL(strTemp)
            End If

            If strStartDate <> "" And strEndDate <> "" Then
                strSql &= " AND DNM_DN_DATE BETWEEN " & Common.ConvertDate(strStartDate & " 00:00:00")
                strSql &= " AND " & Common.ConvertDate(strEndDate & " 23:59:59")
            End If

            If strPaySDate <> "" And strPayEDate <> "" Then
                strSql &= " AND DNM_PAYMENT_DATE BETWEEN " & Common.ConvertDate(strPaySDate & " 00:00:00")
                strSql &= " AND " & Common.ConvertDate(strPayEDate & " 23:59:59")
            End If

            If strVendor <> "" Then
                strTemp = Common.BuildWildCard(strVendor)
                strSql &= " AND CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If

            If strCurr <> "" Then
                strSql &= " AND DNM_CURRENCY_CODE = '" & strCurr & "' "
            End If

            If strStatus = "3,4" Then 'Payment
                'Previous DN Approval
                'strSql &= "AND ("
                'strSql &= "DNM_DN_STATUS IN (3) " 'Approved
                'strSql &= " AND DNM_DN_INDEX IN ("
                'strSql &= " SELECT DNA_DN_INDEX FROM DN_APPROVAL WHERE (DNA_SEQ <= DNA_AO_ACTION OR IM_INVOICE_STATUS IN ('3','4')) AND "
                'strSql &= " (FA_AO = '" & strUserID & "' OR (FA_A_AO = '" & strUserID & "' AND FA_Relief_Ind='O'))"
                'strSql &= ")"
                'strSql &= ") AND IM_FOLDER = 0 "
            Else
                strSql &= "AND DNM_DN_STATUS IN (" & IIf(strStatus = "1", "1,2", strStatus) & ") "

                If strStatus = DNStatus.NewDN Or strStatus = DNStatus.PendingAppr Then
                    strSql &= " AND DNM_DN_INDEX IN ("
                    strSql &= " SELECT DNA_DN_INDEX FROM DN_APPROVAL WHERE DNA_SEQ - 1 = DNA_AO_ACTION AND "
                    strSql &= " (DNA_AO = '" & strUserID & "' OR (DNA_A_AO = '" & strUserID & "' AND DNA_RELIEF_IND='O'))"

                    If strStatus = DNStatus.PendingAppr Then
                        strSql &= " AND (DNA_AGA_TYPE = 'FM' OR (DNA_AGA_TYPE = 'FO' AND DNA_SEQ > DNA_AO_ACTION))"
                    End If

                    strSql &= ")"
                Else
                    strSql &= " AND DNM_DN_INDEX IN ("
                    strSql &= " SELECT DNA_DN_INDEX FROM DN_APPROVAL WHERE DNA_SEQ <= DNA_AO_ACTION AND "
                    strSql &= " (DNA_AO = '" & strUserID & "' OR (DNA_A_AO = '" & strUserID & "' AND DNA_RELIEF_IND='O'))"

                    strSql &= ")"
                End If
            End If

            strSql &= " GROUP BY DNM_DN_INDEX "
            'If dteDateFr <> "" And dteDateTo <> "" Then
            '    strSql_1 &= " AND POM_PO_Date BETWEEN " & Common.ConvertDate(dteDateFr & " 00:00:00")
            '    strSql_1 &= " AND " & Common.ConvertDate(dteDateTo & " 23:59:59")
            'End If

            ds = objDb.FillDs(strSql)

            objDb = Nothing
            Return ds
        End Function

        Public Function getDnMstr(ByVal item As DnValue)
            Dim strSql, strTempAddr, strTempDoc As String
            Dim strTempAmt As Double
            Dim ds As DataSet
            Dim dsDoc As DataSet
            Dim i As Integer
            Dim objGlobal As New AppGlobals

            'Issue 7480 - CH - 25 Mar 2015 (No.64) - Add IM_CREATED_ON
            strSql = "SELECT DISTINCT DNM_DN_INDEX, DNM_DN_NO, DNM_DN_DATE, DNM_ADDR_LINE1, DNM_ADDR_LINE2, DNM_ADDR_LINE3, DNM_POSTCODE, " &
                    "DNM_CITY, DNM_STATE, DNM_COUNTRY, DNM_CURRENCY_CODE, DNM_REMARKS, IM_INVOICE_NO, IM_CREATED_ON, IM_PAYMENT_DATE, IM_INVOICE_TOTAL, IM_SHIP_AMT " &
                    "FROM DEBIT_NOTE_MSTR " &
                    "INNER JOIN INVOICE_MSTR ON DNM_INV_NO = IM_INVOICE_NO AND DNM_DN_S_COY_ID = IM_S_COY_ID " &
                    "WHERE DNM_DN_NO = '" & item.Dn_No & "' AND DNM_DN_S_COY_ID = '" & item.V_Com_Id & "'"

            ds = objDb.FillDs(strSql)

            If ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows(0).Item("DNM_ADDR_LINE1").ToString.Trim <> "" Then
                    strTempAddr = ds.Tables(0).Rows(0).Item("DNM_ADDR_LINE1").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("DNM_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("DNM_ADDR_LINE2").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("DNM_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("DNM_ADDR_LINE3").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("DNM_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("DNM_POSTCODE").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("DNM_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & ds.Tables(0).Rows(0).Item("DNM_CITY").ToString.Trim
                End If

                If ds.Tables(0).Rows(0).Item("DNM_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & objGlobal.getCodeDesc(CodeTable.State, ds.Tables(0).Rows(0).Item("DNM_STATE").ToString.Trim)
                End If

                If ds.Tables(0).Rows(0).Item("DNM_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & objGlobal.getCodeDesc(CodeTable.Country, ds.Tables(0).Rows(0).Item("DNM_COUNTRY").ToString.Trim)
                End If

                item.Dn_Index = ds.Tables(0).Rows(0).Item("DNM_DN_INDEX").ToString.Trim
                item.Dn_No = ds.Tables(0).Rows(0).Item("DNM_DN_NO").ToString.Trim
                item.Dn_Date = Common.parseNull(ds.Tables(0).Rows(0).Item("DNM_DN_DATE"), Date.Today)
                item.Adds = strTempAddr
                item.Remark = Common.parseNull(ds.Tables(0).Rows(0).Item("DNM_REMARKS"))
                item.Inv_No = ds.Tables(0).Rows(0).Item("IM_INVOICE_NO").ToString.Trim
                'Issue 7480 - CH - 25 Mar 2015 (No.64)
                'item.Inv_Date = Common.parseNull(ds.Tables(0).Rows(0).Item("IM_PAYMENT_DATE"), Date.Today)
                item.Inv_Date = Common.parseNull(ds.Tables(0).Rows(0).Item("IM_CREATED_ON"), Date.Today)
                item.Cur = ds.Tables(0).Rows(0).Item("DNM_CURRENCY_CODE").ToString.Trim
                item.Inv_Amt = ds.Tables(0).Rows(0).Item("IM_INVOICE_TOTAL").ToString.Trim
                item.Ship_Amt = Common.parseNull(ds.Tables(0).Rows(0).Item("IM_SHIP_AMT"), 0)
                'item.Related_Doc = ds.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim
                'item.Related_Doc_Amt = ds.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim

                strTempDoc = ""
                strTempAmt = 0
                dsDoc = getRelatedDnNo(item.Inv_No, item.V_Com_Id, item.Dn_No)
                If dsDoc.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsDoc.Tables(0).Rows.Count - 1
                        If strTempDoc <> "" Then
                            strTempDoc &= ", " & dsDoc.Tables(0).Rows(i)("DNM_DN_NO")
                        Else
                            strTempDoc &= dsDoc.Tables(0).Rows(i)("DNM_DN_NO")
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

        Public Function getDnDetail(ByVal strDnNo As String, ByVal strVComId As String) As DataSet
            Dim strSql As String

            'Issue 7480 - CH - 23 Mar 2015 (No.38)
            strSql = "SELECT DEBIT_NOTE_DETAILS.*, DEBIT_NOTE_MSTR.*, INVOICE_DETAILS.*, " &
                    "CASE WHEN IM_GST_INVOICE = 'Y' THEN IFNULL(ID_GST,0) ELSE 0 END AS TAX, " &
                    "CASE WHEN IM_GST_INVOICE = 'Y' THEN " &
                    "IF(DND_GST_RATE <> 'EX', CONCAT(CODE_DESC, ' (', CAST(ID_GST AS UNSIGNED), '%)'), CODE_DESC)  ELSE 'N/A' END AS GST_RATE " &
                    "FROM DEBIT_NOTE_DETAILS " &
                    "INNER JOIN DEBIT_NOTE_MSTR ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                    "INNER JOIN INVOICE_MSTR ON DNM_INV_NO = IM_INVOICE_NO AND DNM_DN_S_COY_ID = IM_S_COY_ID " &
                    "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND DND_INV_LINE = ID_INVOICE_LINE " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = DNM_DN_B_COY_ID " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = DND_GST_RATE " &
                    "WHERE DNM_DN_NO = '" & strDnNo & "' AND DNM_DN_S_COY_ID = '" & strVComId & "'"

            getDnDetail = objDb.FillDs(strSql)

        End Function

        Public Function getRelatedDnNo(ByVal strInvNo As String, ByVal strSCoyId As String, Optional ByVal strDnNo As String = "") As DataSet
            Dim strSql As String
            Dim ds As DataSet

            strSql = "SELECT DNM_DN_NO, SUM(DND_QTY * DND_UNIT_COST) AS AMOUNT " &
                    "FROM DEBIT_NOTE_MSTR " &
                    "INNER JOIN DEBIT_NOTE_DETAILS ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                    "WHERE DNM_INV_NO = '" & strInvNo & "' AND DNM_DN_S_COY_ID = '" & strSCoyId & "' "

            If strDnNo <> "" Then
                strSql &= " AND DNM_DN_NO <> '" & strDnNo & "' "
            End If

            strSql &= "GROUP BY DNM_DN_NO ORDER BY DNM_DN_NO "

            ds = objDb.FillDs(strSql)
            getRelatedDnNo = ds

        End Function

        Function getDnApprFlow(ByVal strDnNo As String, ByVal strSCoyId As String) As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Dim strCoyId As String = HttpContext.Current.Session("CompanyId")

            strSql = "SELECT DNA.*, UMA.UM_USER_NAME AS AO_NAME, UMB.UM_USER_NAME AS AAO_NAME, " &
                    "UMA.UM_INVOICE_APP_LIMIT AS AO_LIMIT,UMB.UM_INVOICE_APP_LIMIT AS AAO_LIMIT " &
                    "FROM DN_APPROVAL DNA " &
                    "INNER JOIN DEBIT_NOTE_MSTR ON DNA.DNA_DN_INDEX = DNM_DN_INDEX " &
                    "LEFT JOIN USER_MSTR UMA ON DNA.DNA_AO = UMA.UM_USER_ID AND UMA.UM_COY_ID = '" & strCoyId & "' " &
                    "LEFT JOIN USER_MSTR UMB ON DNA.DNA_A_AO = UMB.UM_USER_ID AND UMB.UM_COY_ID = '" & strCoyId & "' " &
                    "WHERE DNM_DN_NO = '" & strDnNo & "' AND DNM_DN_S_COY_ID = '" & strSCoyId & "' " &
                    "ORDER BY DNA.DNA_SEQ "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Sub updateAppRemark(ByVal intDnIndex As Long, ByVal strAORemark As String, Optional ByVal intSeq As Integer = Nothing, Optional ByVal strPT As String = "", Optional ByVal aryTemp As ArrayList = Nothing)

            Dim objDB As New EAD.DBCom
            Dim strSql, strLoginUser As String
            Dim i As Integer

            'Issue 7480 - CH - 24 Mar 2015
            If strAORemark <> "" Then
                strSql = "UPDATE DN_APPROVAL SET DNA_AO_REMARK = '" & Common.Parse(strAORemark) & "' " &
                                "WHERE DNA_DN_INDEX=" & intDnIndex

                If intSeq = Nothing Then
                    strSql &= " AND DNA_SEQ = DNA_AO_ACTION + 1"
                Else
                    strSql &= " AND DNA_SEQ = " & intSeq
                End If

                objDB.Execute(strSql)
            End If

            'If strPT <> "" Then
            '    strSql = "UPDATE INVOICE_MSTR SET IM_PAYMENT_TERM = '" & Common.Parse(strPT) & "' " & _
            '    "WHERE IM_INVOICE_INDEX=" & intDnIndex
            '    objDB.Execute(strSql)
            'End If

            If Not aryTemp Is Nothing Then
                For i = 0 To aryTemp.Count - 1
                    strSql = "UPDATE DEBIT_NOTE_DETAILS " &
                            "INNER JOIN DEBIT_NOTE_MSTR ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                            "SET DND_GST_INPUT_TAX_CODE = '" & Common.Parse(aryTemp(i)(1)) & "' " &
                            "WHERE DNM_DN_INDEX = " & intDnIndex & " " &
                            "AND DND_DN_LINE = '" & aryTemp(i)(0) & "'"
                    objDB.Execute(strSql)
                Next
            End If
        End Sub

        Function ApproveDN(ByVal userID As String, ByVal companyID As String, ByVal intDnIndex As Integer, ByVal strDNNo As String, ByVal intCurrentSeq As Integer, ByVal blnHighestLevel As Boolean, ByVal strRole As String,
            ByVal strAORemark As String, ByVal blnRelief As Boolean) As String
            Dim strSql As String
            Dim intDNStatus As Integer
            Dim dblInvoiceAmount As Double = 0
            Dim strSqlAry(0) As String
            Dim strCoyID, strMsg, strLoginUser As String

            strCoyID = HttpContext.Current.Session("CompanyId")
            strLoginUser = HttpContext.Current.Session("UserId")

            strSql = "SELECT DNM_DN_STATUS FROM DEBIT_NOTE_MSTR WHERE DNM_DN_INDEX = " & intDnIndex

            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intDNStatus = tDS.Tables(0).Rows(0).Item("DNM_DN_STATUS")
                If intDNStatus = DNStatus.Approved Then
                    strMsg = "The debit note has already been approved. Approving of this debit note is not allowed. "
                ElseIf intDNStatus = DNStatus.Paid Then
                    strMsg = "The debit note has already been paid. Approving of this debit note is not allowed. "
                ElseIf intDNStatus = DNStatus.PendingAppr Then
                    'If objDB.Get1Column("company_mstr", "cm_inv_appr", " WHERE cm_coy_id='" & strCoyID & "'") = "Y" Then
                    '    If isApproved(intInvoiceIndex, strRole, strLoginUser) Then 'Michelle 24/68/2007 - To remove the error message
                    '        strMsg = "You have already approved this invoice. Approving of this invoice is not allowed."
                    '    End If
                    'End If
                ElseIf intDNStatus = DNStatus.NewDN Then
                    'If objDB.Get1Column("company_mstr", "cm_inv_appr", " WHERE cm_coy_id='" & strCoyID & "'") = "Y" Then
                    '    If objDB.Get1Column("company_mstr", "cm_findept_mode", " WHERE cm_coy_id='" & strCoyID & "'") = "Y" Then
                    '        If isApproved(intInvoiceIndex, strRole, strLoginUser) Then
                    '            strMsg = "You have already approved this invoice. Approving of this invoice is not allowed."
                    '        End If
                    '    End If
                    'End If
                End If
            End If

            If strMsg <> "" Then Return strMsg

            If blnHighestLevel Then
                'Update Debit note status to Approved
                strSql = "UPDATE DEBIT_NOTE_MSTR SET DNM_DN_STATUS=" & DNStatus.Approved & ", " &
                        "DNM_STATUS_CHANGED_BY = '" & strLoginUser & "', DNM_STATUS_CHANGED_ON = NOW() " &
                        "WHERE DNM_DN_INDEX = " & intDnIndex
                Common.Insert2Ary(strSqlAry, strSql)

                'Jules 2018.10.19
                'updateAOAction(intDnIndex, intCurrentSeq, strSqlAry)
                updateAOAction(intDnIndex, intCurrentSeq, strSqlAry, strAORemark, blnRelief, blnHighestLevel,)
                'End modification.
            Else
                strSql = "UPDATE DEBIT_NOTE_MSTR SET DNM_DN_STATUS=" & DNStatus.PendingAppr & ", " &
                        "DNM_STATUS_CHANGED_BY = '" & strLoginUser & "', DNM_STATUS_CHANGED_ON = NOW() " &
                        "WHERE DNM_DN_INDEX = " & intDnIndex
                Common.Insert2Ary(strSqlAry, strSql)

                'Jules 2018.10.19
                'updateAOAction(intDnIndex, intCurrentSeq, strSqlAry)
                updateAOAction(intDnIndex, intCurrentSeq, strSqlAry, strAORemark, blnRelief, blnHighestLevel,)
                'End modification.
            End If

            Dim objUsers As New Users

            If strRole = "FO" Then
                objUsers.Log_UserActivity(userID, companyID, strSqlAry, WheelModule.DebitNoteMod, WheelUserActivity.FO_VerifyDN, strDNNo)
            ElseIf strRole = "FM" Then
                objUsers.Log_UserActivity(userID, companyID, strSqlAry, WheelModule.DebitNoteMod, WheelUserActivity.FM_ApproveDN, strDNNo)
            End If

            objUsers = Nothing

            If Not objDb.BatchExecute(strSqlAry) Then
                strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
            Else
                '//only send mail if transaction successfully created
                Dim objMail As New Email
                If blnHighestLevel Then
                    'objMail.sendNotification(EmailType.IQCApprovedToSK, strSKId, strCoyID, "", strIQCNo, "")
                Else
                    'Send Email to next AO
                    sendMailToNextlvl(strDNNo, intDnIndex, intCurrentSeq + 1)
                End If
            End If

            Return strMsg
        End Function

        'Sub updateAppAction(ByRef pQuery() As String, ByVal intInvoiceIndex As Long, ByVal strAORemark As String, ByVal blnRelief As Boolean, ByVal blnHighestLevel As Boolean, Optional ByVal blnEnterpriseVersion As Boolean = True)
        Sub updateAOAction(ByVal intIndex As Long, ByVal intCurrentSeq As Integer, ByRef pQuery() As String, Optional ByVal strAORemark As String = "", Optional ByVal blnRelief As Boolean = False, Optional ByVal blnHighestLevel As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strSql, strLoginUser As String
            strLoginUser = HttpContext.Current.Session("UserId")

            'Jules 2018.10.19 - DN approval flow must follow Invoice approval flow.
            'strSql = "UPDATE DN_APPROVAL SET DNA_ACTION_DATE = " & Common.ConvertDate(Now) & ", " &
            '        "DNA_ACTIVE_AO = '" & strLoginUser & "' WHERE DNA_DN_INDEX = " & intIndex & " AND DNA_SEQ = " & intCurrentSeq
            'Common.Insert2Ary(pQuery, strSql)

            'strSql = "UPDATE DN_APPROVAL SET DNA_AO_ACTION = " & intCurrentSeq & " WHERE DNA_DN_INDEX=" & intIndex
            'Common.Insert2Ary(pQuery, strSql)

            If blnRelief Then
                strSql = "UPDATE DN_APPROVAL SET DNA_AO_REMARK='" & Common.Parse(strAORemark) & "',DNA_ACTION_DATE=" &
                        Common.ConvertDate(Now) & ",DNA_AO='" & strLoginUser & "',DNA_ON_BEHALFOF = DNA_AO, DNA_ACTIVE_AO='" &
                        strLoginUser & "' WHERE DNA_DN_INDEX=" & intIndex & " AND DNA_SEQ = DNA_AO_ACTION + 1"
            Else
                strSql = "UPDATE DN_APPROVAL SET DNA_AO_REMARK='" & Common.Parse(strAORemark) & "',DNA_ACTION_DATE=" &
                Common.ConvertDate(Now) & ",DNA_ACTIVE_AO='" & strLoginUser & "' WHERE DNA_DN_INDEX=" & intIndex & " AND DNA_SEQ = DNA_AO_ACTION + 1"
            End If

            Common.Insert2Ary(pQuery, strSql)

            If blnHighestLevel And blnEnterpriseVersion = True Then
                Dim minFA_SEQ As String
                minFA_SEQ = objDb.Get1Column("DN_APPROVAL", "MIN(DNA_SEQ) - 1", " WHERE DNA_AGA_TYPE = 'FM' AND DNA_DN_INDEX=" & intIndex & "")
                strSql = "UPDATE DN_APPROVAL SET DNA_AO_ACTION = ('" & minFA_SEQ & "') WHERE DNA_DN_INDEX=" & intIndex
                Common.Insert2Ary(pQuery, strSql)
            Else
                strSql = "UPDATE DN_APPROVAL SET DNA_AO_ACTION = DNA_AO_ACTION + 1 WHERE DNA_DN_INDEX=" & intIndex
                Common.Insert2Ary(pQuery, strSql)
            End If
            'End modification.
        End Sub

        Function updateDN(ByVal dtDn As DataTable, ByVal blnPay As Boolean)
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0), strAryQuery2(0) As String
            Dim objDb As New EAD.DBCom
            Dim strDoc As String = ""
            Dim blnUpdatePO As Boolean = True

            If dtDn.Rows.Count > 0 Then
                For i = 0 To dtDn.Rows.Count - 1
                    strsql = "UPDATE DEBIT_NOTE_MSTR SET " &
                            "DNM_PAYMENT_TERM = '" & Common.Parse(dtDn.Rows(i)("PayTerm")) & "' "

                    If blnPay = True Then
                        strsql &= ", DNM_PAYMENT_DATE = NOW(), " &
                                "DNM_DN_STATUS = " & DNStatus.Paid & ", " &
                                "DNM_STATUS_CHANGED_BY = '" & HttpContext.Current.Session("UserId") & "', " &
                                "DNM_STATUS_CHANGED_ON = NOW() "
                    End If

                    strsql &= " WHERE DNM_DN_INDEX = " & dtDn.Rows(i)("DnIndex") & " "

                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                objDb.BatchExecute(strAryQuery)
            End If

        End Function

        Public Function sendMailToNextlvl(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer)
            Dim strsql, strcond As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim i As Integer

            strBody &= "<P>You have an Debit Note/Debit Advise  (" & strDocNo & ") is waiting for your action. <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            strsql = "SELECT DNA_AO, IFNULL(DNA_A_AO, '') AS DNA_A_AO, B.UM_EMAIL AS AO_EMAIL, ISNULL(C.UM_EMAIL, '') AS AAO_EMAIL, " &
                    "B.UM_USER_NAME AS AO_NAME, ISNULL(C.UM_USER_NAME, '') AS AAO_NAME " &
                    "FROM DN_APPROVAL " &
                    "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = DNA_AO AND B.UM_DELETED <> 'Y' AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = DNA_A_AO AND C.UM_DELETED <> 'Y' AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "WHERE DNA_DN_INDEX = " & intIndex & " AND DNA_SEQ = " & intSeq
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objMail As New AppMail
                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & ", <BR>" & strBody

                objMail.Subject = "Agora : Debit Note/Debit Advise Approval"
                objMail.SendMail()

                If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) <> "" Then
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & ", <BR>" & strBody
                    objMail.Subject = "Agora : Debit Note/Debit Advise Approval"
                    objMail.SendMail()
                End If

            End If
            objCommon = Nothing
        End Function

        'Public Function getDNAttach(ByVal strDocNo As String, Optional ByVal strCompId As String = "", Optional ByVal strInternalExternal As String = "E") As DataSet

        '    Dim strsql As String

        '    Dim ds As New DataSet

        '    If strCompId = "" Then
        '        strCompId = HttpContext.Current.Session("CompanyId")
        '    End If

        '    strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "

        '    strsql &= "WHERE CDA_COY_ID = '" & strCompId & "' "

        '    strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = 'DN' "

        '    'strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "

        '    ds = objDb.FillDs(strsql)

        '    getDNAttach = ds

        'End Function

        Public Function getInvRelatedDN(ByVal strInvNo As String, Optional ByVal strVCompId As String = "") As String
            Dim ds As DataSet
            Dim strSql As String

            strSql = "SELECT DNM_DN_NO, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID, UM_USER_NAME, DNM_CREATED_DATE, DNM_DN_STATUS, STATUS_DESC, SUM(DND_QTY * DND_UNIT_COST) AS AMOUNT " &
                    "FROM DEBIT_NOTE_MSTR " &
                    "INNER JOIN DEBIT_NOTE_DETAILS ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                    "INNER JOIN STATUS_MSTR ON DNM_DN_STATUS = STATUS_NO AND STATUS_TYPE = 'DN' " &
                    "LEFT JOIN USER_MSTR ON DNM_CREATED_BY = UM_USER_ID AND DNM_DN_S_COY_ID = UM_COY_ID " &
                    "WHERE DNM_DN_S_COY_ID = '" & strVCompId & "' AND DNM_INV_NO = '" & Common.Parse(strInvNo) & "' " &
                    "GROUP BY DNM_DN_NO " 'Issue 7480 - CH - 24 Feb 15

            ds = objDb.FillDs(strSql)
            getInvRelatedDN = strSql
        End Function

        Public Function payDebitNote(ByVal userID As String, ByVal companyID As String, ByVal intDnIndex As Long, ByVal strDNNo As String) As String
            Dim strSql, strLoginUser, strMsg As String
            Dim intDNStatus As Integer
            Dim strSqlAry(0) As String
            strLoginUser = HttpContext.Current.Session("UserId")

            strSql = "SELECT DNM_DN_STATUS FROM DEBIT_NOTE_MSTR WHERE DNM_DN_INDEX = " & intDnIndex

            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                intDNStatus = tDS.Tables(0).Rows(0).Item("DNM_DN_STATUS")
                If intDNStatus = DNStatus.Paid Then
                    strMsg = "The debit note has already been paid."
                End If
            End If

            If strMsg <> "" Then Return strMsg

            Dim objUsers As New Users
            objUsers.Log_UserActivity(userID, companyID, strSqlAry, WheelModule.DebitNoteMod, WheelUserActivity.FM_ApprovePayment, strDNNo)
            objUsers = Nothing

            strSql = "UPDATE DEBIT_NOTE_MSTR SET DNM_DN_STATUS = " & DNStatus.Paid & ", DNM_STATUS_CHANGED_BY = '" & strLoginUser & "', DNM_STATUS_CHANGED_ON = NOW() " &
                    "WHERE DNM_DN_INDEX = " & intDnIndex
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

        Public Sub chkDnItemQty(ByVal strInvNo As String, ByVal intItemLine As Integer, ByVal decItemQty As Decimal, ByRef blnQty As Boolean)
            Dim strSql As String
            Dim dsRelatedDN As DataSet
            Dim dsInv As DataSet
            Dim decDNRelQty, decInvQty As Decimal

            'Sum total of Qty & Unit Price of Related DN
            'strSql = "SELECT SUM(IFNULL(DND_QTY,0)) AS DND_QTY, SUM(IFNULL(DND_UNIT_COST,0)) AS DND_UNIT_COST " & _
            '        "FROM DEBIT_NOTE_MSTR " & _
            '        "INNER JOIN DEBIT_NOTE_DETAILS ON DNM_DN_NO = DND_DN_NO AND DNM_DN_S_COY_ID = DND_DN_S_COY_ID " & _
            '        "WHERE DNM_INV_NO = '" & Common.Parse(strInvNo) & "' AND DNM_DN_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '        "AND DND_INV_LINE = " & intItemLine & " " & _
            '        "GROUP BY DNM_INV_NO"
            'dsRelatedDN = objDb.FillDs(strSql)

            'If dsRelatedDN.Tables(0).Rows.Count > 0 Then
            '    decDNRelQty = dsRelatedDN.Tables(0).Rows(0)("DND_QTY")
            'Else 'There no related DN Doc
            '    decDNRelQty = 0
            'End If
            decDNRelQty = 0

            'Get Qty & Unit Price of Inv Doc
            strSql = "SELECT ID_RECEIVED_QTY, ID_UNIT_COST FROM INVOICE_DETAILS " &
                    "WHERE ID_INVOICE_NO = '" & Common.Parse(strInvNo) & "' AND ID_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " &
                    "AND ID_INVOICE_LINE = " & intItemLine

            dsInv = objDb.FillDs(strSql)
            decInvQty = dsInv.Tables(0).Rows(0)("ID_RECEIVED_QTY")

            If (decItemQty + decDNRelQty) > decInvQty Then
                blnQty = False
            Else
                blnQty = True
            End If
        End Sub
    End Class
End Namespace
