'Copyright ?2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports System
Imports AgoraNEO
Imports System.Web.UI.WebControls
Imports System.Configuration 'Jules 2015-Feb-24: GST IPP Stage 2A
Imports System.Web

Namespace AgoraNEO
    Public Class IPPMain
        Dim objDb As New EAD.DBCom
        Dim objGlobal As New AppGlobals
        Public Function exchangeRate(ByVal index As String) As Boolean
            Dim sql As String
            sql = "SELECT im_exchange_rate FROM invoice_mstr "
            If objDb.Get1ColumnCheckNull("invoice_mstr", "im_exchange_rate", " WHERE im_invoice_index = " & index & "") = "" Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function chkBillInvApprovedBy(ByVal index As String) As Boolean
            Dim sql As String
            sql = "SELECT im_remarks1 FROM invoice_mstr "
            If objDb.Get1ColumnCheckNull("invoice_mstr", "im_remarks1", " WHERE im_invoice_index = " & index & "") = "" Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function isHighestLevel(ByVal invindex As String)
            Dim sql As String
            sql = "SELECT MAX(FA_SEQ),fa_ao_action FROM finance_approval WHERE " &
            "fa_invoice_index = '" & invindex & "'"
            Dim ds As DataSet = objDb.FillDs(sql)
            If ds.Tables(0).Rows.Count > 0 Then
                If Common.parseNull(ds.Tables(0).Rows(0).Item("MAX(FA_SEQ)")) = Common.parseNull(ds.Tables(0).Rows(0).Item("FA_AO_ACTION"), 0) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Public Function modPaymentType(ByVal index As String, ByVal code As String, ByVal desc As String, ByVal status As String)
            Dim sql, chksql As String
            sql = "UPDATE PAYMENT_TYPE SET PT_PT_CODE='" & code & "', PT_PT_DESC='" & desc & "', " &
            "PT_STATUS='" & status & "' WHERE PT_INDEX='" & index & "'"
            If objDb.Execute(sql) Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function getPaymentType(ByVal userID As String, ByVal companyID As String, ByVal code As String, ByVal desc As String, ByVal status As String) As DataSet
            Dim sql As String
            Dim ds As New DataSet
            sql = "SELECT * FROM PAYMENT_TYPE WHERE PT_COY_ID='" & companyID & "' "
            If code <> "" Then
                sql &= "AND PT_PT_CODE LIKE '%" & code & "%' "
            End If
            If desc <> "" Then
                sql &= "AND PT_PT_DESC LIKE '%" & desc & "%' "
            End If
            If status <> "" Then
                sql &= "AND PT_STATUS = '" & status & "' "
            End If
            ds = objDb.FillDs(sql)
            Return ds

        End Function
        Public Function addPaymentType(ByVal userID As String, ByVal companyID As String, ByVal code As String, ByVal desc As String, ByVal status As String)
            Dim strsql As String
            Dim sqlcheck As String
            sqlcheck = "SELECT * FROM PAYMENT_TYPE WHERE PT_COY_ID='" & companyID & "'" &
            " AND PT_PT_CODE='" & code & "'"
            If objDb.Exist(sqlcheck) Then
                Return False
                Exit Function
            End If

            strsql = "INSERT INTO PAYMENT_TYPE(PT_COY_ID,PT_PT_CODE,PT_PT_DESC,PT_STATUS,PT_ENT_BY,PT_ENT_DATETIME)" &
            " VALUES ('" & companyID & "','" & code & "','" & desc & "','" & status & "','" & userID & "'," &
            "NOW())"
            If objDb.Execute(strsql) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function chkPymtPendingRec(ByVal userID As String, ByVal companyID As String, ByVal index As String) As Boolean
            Dim sql As String
            sql = "SELECT * FROM invoice_mstr WHERE im_invoice_status ='10' AND im_b_coy_id='" & companyID & "'" &
            " AND im_pymt_type_index='" & index & "'"
            If objDb.Exist(sql) Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function getCompanyTypeahead(ByVal userID As String, ByVal companyID As String, Optional ByVal form As String = "", Optional ByVal strUserInput As String = "") As DataSet
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If
            '------------------------------------------------

            If strUserInput = "*" Then
                'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
                'strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX " _
                '                 & "FROM IPP_COMPANY " _
                '                 & "WHERE IC_COY_TYPE = 'V' AND IC_COY_ID='" & Common.Parse(companyID) & "' "
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX " _
                                 & "FROM IPP_COMPANY " _
                                 & "WHERE IC_COY_TYPE = 'V' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' "
            Else
                'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
                'strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX " _
                ' & "FROM IPP_COMPANY " _
                ' & "WHERE IC_COY_TYPE = 'V' AND IC_COY_ID='" & Common.Parse(companyID) & "' AND IC_COY_NAME LIKE '" & Common.Parse(strUserInput) & "%'"
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX " _
                 & "FROM IPP_COMPANY " _
                 & "WHERE IC_COY_TYPE = 'V' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' AND IC_COY_NAME LIKE '" & Common.Parse(strUserInput) & "%'"
            End If

            If form = "" Then
                strsql = strsql & " AND IC_STATUS = 'A'"
            End If

            ds = objDb.FillDs(strsql)
            getCompanyTypeahead = ds
        End Function

        'Added by Joon on 03 May 2012 for issue 1598
        Public Function getCompanyTypeahead1(ByVal userID As String, ByVal companyID As String, Optional ByVal form As String = "", Optional ByVal strUserInput As String = "", Optional ByVal strCoyType As String = "", Optional ByVal strIsNostro As String = "") As DataSet
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If
            '------------------------------------------------
            'Zulham 15/02/2015 8317
            'Zulham 21/12/2015 (Stage 4 Phase 2) - added ic_business_reg_no to all queries
            'zULHAM 27112018
            If strUserInput = "*" And strCoyType = "V" Then
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                                & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                                & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                                & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                                & "WHERE IC_COY_TYPE = 'V' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' " _
                                & "And CM1.CODE_CATEGORY = 'S' " _
                                & "And CM1.CODE_VALUE = IC_COUNTRY " _
                                & "And CM1.CODE_ABBR = IC_STATE " _
                                & "And CM2.CODE_CATEGORY = 'CT' " _
                                & "And CM2.CODE_ABBR = IC_COUNTRY "
            ElseIf strUserInput = "*" And strCoyType = "E" Then
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                                & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                                & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                                & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                                & "WHERE IC_COY_TYPE = 'E' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' " _
                                & "And CM1.CODE_CATEGORY = 'S' " _
                                & "And CM1.CODE_VALUE = IC_COUNTRY " _
                                & "And CM1.CODE_ABBR = IC_STATE " _
                                & "And CM2.CODE_CATEGORY = 'CT' " _
                                & "And CM2.CODE_ABBR = IC_COUNTRY "
                'Zulham 05/02/2015 IPP GST Stage 2A (8317)
            ElseIf strUserInput = "*" And strCoyType = "R" Then
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                                & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                                & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                                & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                                & "WHERE IC_COY_TYPE = 'B' AND IC_COY_ID='" & Common.Parse(companyID) & "' AND IC_COY_NAME LIKE '%" & Common.Parse(strUserInput) & "%'" _
                                & "And CM1.CODE_CATEGORY = 'S' " _
                                & "And CM1.CODE_VALUE = IC_COUNTRY " _
                                & "And CM1.CODE_ABBR = IC_STATE " _
                                & "And CM2.CODE_CATEGORY = 'CT' " _
                                & "And CM2.CODE_ABBR = IC_COUNTRY "
            ElseIf strUserInput <> "" And strCoyType = "V" Then
                'Zulham 15/02/2015 8317
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                                & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                                & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                                & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                                & "WHERE IC_COY_TYPE = 'V' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' AND IC_COY_NAME LIKE '%" & Common.Parse(strUserInput) & "%'" _
                                & "And CM1.CODE_CATEGORY = 'S' " _
                                & "And CM1.CODE_VALUE = IC_COUNTRY " _
                                & "And CM1.CODE_ABBR = IC_STATE " _
                                & "And CM2.CODE_CATEGORY = 'CT' " _
                                & "And CM2.CODE_ABBR = IC_COUNTRY "
            ElseIf strUserInput <> "" And strCoyType = "E" Then
                'Zulham 15/02/2015 8317
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                                & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                                & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                                & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                                & "WHERE IC_COY_TYPE = 'E' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' AND IC_COY_NAME LIKE '%" & Common.Parse(strUserInput) & "%'" _
                                & "And CM1.CODE_CATEGORY = 'S' " _
                                & "And CM1.CODE_VALUE = IC_COUNTRY " _
                                & "And CM1.CODE_ABBR = IC_STATE " _
                                & "And CM2.CODE_CATEGORY = 'CT' " _
                                & "And CM2.CODE_ABBR = IC_COUNTRY "
                'Zulham 05/02/2015 IPP GST Stage 2A (8317)
            ElseIf strUserInput <> "" And strCoyType = "R" Then
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                                & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                                & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                                & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                                & "WHERE IC_COY_TYPE = 'B' AND IC_COY_ID='" & Common.Parse(strCoyId) & "' AND IC_COY_NAME LIKE '%" & Common.Parse(strUserInput) & "%'" _
                                & "And CM1.CODE_CATEGORY = 'S' " _
                                & "And CM1.CODE_VALUE = IC_COUNTRY " _
                                & "And CM1.CODE_ABBR = IC_STATE " _
                                & "And CM2.CODE_CATEGORY = 'CT' " _
                                & "And CM2.CODE_ABBR = IC_COUNTRY "
            Else
                strsql = "SELECT IC_COY_ID, IC_COY_NAME,IC_INDEX,IC_ADDR_LINE1,IC_ADDR_LINE2,IC_ADDR_LINE3, " _
                               & "IC_POSTCODE,IC_CITY, CM1.CODE_DESC AS 'IC_STATE', CM2.CODE_DESC AS 'IC_COUNTRY',IC_PAYMENT_METHOD,CASE WHEN IC_PAYMENT_METHOD = 'BC' THEN IC_bank_code " _
                               & "ELSE CONCAT(IC_bank_code,'[',IC_bank_acct,']') END AS 'IC_bank_code',ic_resident_type, ic_tax_reg_no, ifnull(ic_nostro_flag,'') 'ic_nostro_flag', ifnull(ic_business_reg_no,'') 'ic_business_reg_no' " _
                               & "FROM IPP_COMPANY, CODE_MSTR CM1, CODE_MSTR CM2 " _
                               & "WHERE (IC_COY_TYPE = 'E' or IC_COY_TYPE = 'V') AND IC_COY_ID='" & Common.Parse(strCoyId) & "' AND IC_COY_NAME LIKE '" & Common.Parse(strUserInput) & "%'" _
                               & "And CM1.CODE_CATEGORY = 'S' " _
                               & "And CM1.CODE_VALUE = IC_COUNTRY " _
                               & "And CM1.CODE_ABBR = IC_STATE " _
                               & "And CM2.CODE_CATEGORY = 'CT' " _
                               & "And CM2.CODE_ABBR = IC_COUNTRY "
            End If

            If form = "" Then
                strsql = strsql & " AND IC_STATUS = 'A'"
            End If

            'Zulham 26/02/2015 Case 8317
            If strIsNostro = "Yes" Then
                'Zulham 01042015 Case 8603
                'strsql = strsql & " AND IC_NOSTRO_FLAG = 'Y'"
                strsql = strsql & " AND ic_payment_method='NOSTRO' "
            ElseIf strIsNostro = "No" Then
                If strCoyType = "V" Then
                    'Zulham 01042015 Case 8603
                    'strsql = strsql & " AND (IC_NOSTRO_FLAG = 'N' or IC_NOSTRO_FLAG is null) and (ic_bill_gl_code IS NOT NULL AND TRIM(ic_bill_gl_code) <> '')"
                    strsql = strsql & " AND (Trim(ic_payment_method) = '' or ic_payment_method <> 'NOSTRO') and (ic_bill_gl_code IS NOT NULL AND TRIM(ic_bill_gl_code) <> '')"
                Else
                    'For related comp and employees 
                    'Zulham 01042015 Case 8603
                    strsql = strsql & " AND (Trim(ic_payment_method) = '' or ic_payment_method <> 'NOSTRO')"
                End If
            End If

            ds = objDb.FillDs(strsql)
            getCompanyTypeahead1 = ds
        End Function
        Public Function getCompanyAddrTypeahead(ByVal userID As String, ByVal companyID As String, Optional ByVal form As String = "", Optional ByVal strUserInput As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If strUserInput = "*" Then
                strsql = "SELECT IC_COY_ID, IC_ADDR_LINE1 " _
                                & "FROM IPP_COMPANY " _
                                & "WHERE (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID='" & Common.Parse(companyID) & "' "
            Else
                strsql = "SELECT IC_COY_ID,  IC_ADDR_LINE1 " _
                & "FROM IPP_COMPANY " _
                & "WHERE (IC_COY_TYPE = 'E' OR IC_COY_TYPE = 'V') AND IC_COY_ID='" & Common.Parse(companyID) & "' AND IC_ADDR_LINE1 LIKE '" & Common.Parse(strUserInput) & "%'"

            End If

            If form = "" Then
                strsql = strsql & " AND IC_STATUS = 'A'"
            End If

            ds = objDb.FillDs(strsql)
            getCompanyAddrTypeahead = ds
        End Function
        Public Function GetVendorAddress(ByVal userID As String, ByVal companyID As String, ByRef strVenName As String, ByVal blnIPPFinTeller As Boolean, ByVal VendorIndex As String) As DataSet
            Dim strSQL, strCoyId As String
            Dim ds As DataSet
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If

            'If blnIPPFinTeller Then
            'If strMode = "modify" Then
            'Zulham 05/02/2015 IPP GST Stage 2A (8317)
            strSQL = "  SELECT COM.IC_ADDR_LINE1,COM.IC_ADDR_LINE2,COM.IC_ADDR_LINE3,COM.IC_POSTCODE,COM.IC_CITY,CM.CODE_DESC AS IC_STATE2,com.ic_state,CM2.CODE_DESC AS IC_COUNTRY2,com.ic_country,COM.IC_PAYMENT_METHOD,COM.ic_bank_code,COM.ic_bank_acct,COM.ic_credit_terms " _
                              & "FROM IPP_COMPANY COM " _
                              & "INNER JOIN CODE_MSTR CM ON CM.CODE_ABBR = COM.IC_STATE AND CM.CODE_CATEGORY = 'S' " _
                              & "INNER JOIN CODE_MSTR CM2 ON CM2.CODE_ABBR = COM.IC_COUNTRY AND CM2.CODE_CATEGORY = 'CT' " _
                              & "WHERE (COM.IC_COY_TYPE = 'V' OR COM.IC_COY_TYPE = 'E' OR COM.IC_COY_TYPE = 'B') AND IC_STATUS = 'A' AND IC_INDEX = '" & VendorIndex & "' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "'" 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            'Else
            '    strSQL = "  SELECT COM.IC_ADDR_LINE1,COM.IC_ADDR_LINE2,COM.IC_ADDR_LINE3,COM.IC_POSTCODE,COM.IC_CITY,CM.CODE_DESC AS IC_STATE2,com.ic_state,CM2.CODE_DESC AS IC_COUNTRY2,com.ic_country,COM.IC_PAYMENT_METHOD,COM.ic_bank_code,COM.ic_bank_acct,COM.ic_credit_terms " _
            '      & "FROM IPP_COMPANY COM " _
            '      & "INNER JOIN CODE_MSTR CM ON CM.CODE_ABBR = COM.IC_STATE AND CM.CODE_CATEGORY = 'S' " _
            '      & "INNER JOIN CODE_MSTR CM2 ON CM2.CODE_ABBR = COM.IC_COUNTRY AND CM2.CODE_CATEGORY = 'CT' " _
            '      & "WHERE (COM.IC_COY_TYPE = 'V' OR COM.IC_COY_TYPE = 'E') AND IC_STATUS = 'A' AND IC_INDEX = '" & strVenName & "' AND IC_COY_ID = '" & Common.Parse(companyID) & "'"
            'End If

            'Else
            '    strSQL = "  SELECT COM.IC_ADDR_LINE1,COM.IC_ADDR_LINE2,COM.IC_ADDR_LINE3,COM.IC_POSTCODE,COM.IC_CITY,CM.CODE_DESC AS IC_STATE2,com.ic_state,CM2.CODE_DESC AS IC_COUNTRY2,com.ic_country,COM.IC_PAYMENT_METHOD,COM.ic_bank_code,COM.ic_bank_acct,COM.ic_credit_terms " _
            '            & "FROM IPP_COMPANY COM " _
            '            & "INNER JOIN CODE_MSTR CM ON CM.CODE_ABBR = COM.IC_STATE AND CM.CODE_CATEGORY = 'S' " _
            '            & "INNER JOIN CODE_MSTR CM2 ON CM2.CODE_ABBR = COM.IC_COUNTRY AND CM2.CODE_CATEGORY = 'CT' " _
            '            & "WHERE COM.IC_COY_TYPE = 'V' AND IC_STATUS = 'A' AND IC_COY_NAME = '" & strVenName & "' AND IC_COY_ID = '" & Common.Parse(companyID) & "'"

            'End If

            ds = objDb.FillDs(strSQL)
            Return ds

        End Function

        Public Function PopPayFor(ByVal userID As String, ByVal companyID As String) As DataSet
            'Yap 2011 Inv
            Dim strSql As String
            Dim dsDoc As New DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            Dim strCoyId As String
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If

            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            strSql = "SELECT IC_OTHER_B_COY_CODE FROM IPP_COMPANY WHERE IC_COY_ID = '" & strCoyId & "' AND IC_COY_TYPE = 'B' AND IC_STATUS = 'A' AND IC_OTHER_B_COY_CODE <> '" & companyID & "'"
            'strSql = "SELECT IC_OTHER_B_COY_CODE FROM IPP_COMPANY WHERE IC_COY_ID = '" & strCoyId & "' AND IC_COY_TYPE = 'B' AND IC_STATUS = 'A'"
            'strSql = "SELECT IC_OTHER_B_COY_CODE FROM IPP_COMPANY WHERE IC_COY_ID = '" & companyID & "' AND IC_COY_TYPE = 'B' AND IC_STATUS = 'A'"

            dsDoc = objDb.FillDs(strSql)
            PopPayFor = dsDoc

        End Function

        Public Function PopUOM() As DataSet

            Dim strSql As String
            Dim dsDoc As New DataSet

            strSql = "SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_DELETED = 'N' AND CODE_CATEGORY = 'UOM' ORDER BY code_desc"

            dsDoc = objDb.FillDs(strSql)
            PopUOM = dsDoc

        End Function

        Public Function getGLCodeTypeAhead(ByVal userID As String, ByVal companyID As String, Optional ByVal compid As String = "", Optional ByVal strUserInput As String = "") As DataSet
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH - 2 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If
            If compid <> "" Then
                If strUserInput = "*" Then
                    strsql = "SELECT CONCAT(CBG_B_GL_CODE,':',CBG_B_GL_DESC) as CBG_B_GL_CODE " _
                                      & "FROM COMPANY_B_GL_CODE " _
                                      & "WHERE CBG_B_COY_ID ='" & Common.Parse(compid) & "' AND CBG_STATUS = 'A' "
                Else
                    strsql = "SELECT CONCAT(CBG_B_GL_CODE,':',CBG_B_GL_DESC) as CBG_B_GL_CODE " _
                  & "FROM COMPANY_B_GL_CODE " _
                  & "WHERE CBG_B_COY_ID ='" & Common.Parse(compid) & "' AND CBG_STATUS = 'A' and CBG_B_GL_CODE like '" & strUserInput & "%'"
                End If

            Else
                If strUserInput = "*" Then
                    strsql = "SELECT CONCAT(CBG_B_GL_CODE,':',CBG_B_GL_DESC) as CBG_B_GL_CODE " _
                                      & "FROM COMPANY_B_GL_CODE " _
                                      & "WHERE CBG_B_COY_ID ='" & Common.Parse(strCoyId) & "' AND CBG_STATUS = 'A' "
                Else
                    strsql = "SELECT CONCAT(CBG_B_GL_CODE,':',CBG_B_GL_DESC) as CBG_B_GL_CODE " _
                  & "FROM COMPANY_B_GL_CODE " _
                  & "WHERE CBG_B_COY_ID ='" & Common.Parse(strCoyId) & "' AND CBG_STATUS = 'A' and CBG_B_GL_CODE like '" & Common.Parse(strUserInput) & "%'"
                End If
            End If

            ds = objDb.FillDs(strsql)
            getGLCodeTypeAhead = ds
        End Function

        Public Function getGLCodeTypeAhead2(ByVal userID As String, ByVal companyID As String, Optional ByVal strUserInput As String = "", Optional ByVal strChk As String = "Y", Optional ByVal strNA As String = "N", Optional ByVal strDefCoyId As String = "N") As DataSet
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH (30/1/2015)
            If strDefCoyId = "Y" Then
                strCoyId = ConfigurationManager.AppSettings("DefIPPCompID")
            Else
                strCoyId = companyID
            End If

            If strNA = "Y" Then
                strsql = "SELECT 'N/A' AS CBG_B_GL_CODE, 'N/A' AS CBG_B_GL_DESC " &
                        "UNION "
            Else
                strsql = ""
            End If

            If strUserInput = "*" Then
                strsql &= "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC " &
                        "FROM COMPANY_B_GL_CODE " &
                        "WHERE CBG_B_COY_ID ='" & Common.Parse(strCoyId) & "' AND CBG_STATUS = 'A' "

                If strChk = "Y" Then
                    strsql &= "AND CBG_B_GL_CODE NOT IN (SELECT DISTINCT IGG_GL_CODE FROM IPP_GLRULE_GL " &
                            "INNER JOIN IPP_GLRULE ON IGG_GLRULE_INDEX WHERE IG_COY_ID = '" & Common.Parse(strCoyId) & "')"
                End If

            Else
                strsql &= "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC " &
                        "FROM COMPANY_B_GL_CODE " &
                        "WHERE CBG_B_COY_ID ='" & Common.Parse(strCoyId) & "' AND CBG_STATUS = 'A' AND CBG_B_GL_CODE LIKE '" & Common.Parse(strUserInput) & "%' "

                If strChk = "Y" Then
                    strsql &= "AND CBG_B_GL_CODE NOT IN (SELECT DISTINCT IGG_GL_CODE FROM IPP_GLRULE_GL " &
                            "INNER JOIN IPP_GLRULE ON IGG_GLRULE_INDEX WHERE IG_COY_ID = '" & Common.Parse(strCoyId) & "')"
                End If
            End If

            ds = objDb.FillDs(strsql)
            'If ds.Tables(0).Rows.Count > 0 Then
            '    Return ds
            'End If

            'strsql = "SELECT '" & strUserInput & "' AS CBG_B_GL_CODE, '' AS CBG_B_GL_DESC "
            'ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function getAssetGroupTypeAhead(ByVal userID As String, ByVal companyID As String, Optional ByVal compid As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            If compid <> "" Then
                strsql = "SELECT AG_GROUP " _
                  & "FROM ASSET_GROUP " _
                  & "WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID='" & Common.Parse(compid) & "' AND AG_STATUS='A'"
            Else
                strsql = "SELECT AG_GROUP " _
                  & "FROM ASSET_GROUP " _
                  & "WHERE AG_GROUP_TYPE = 'A' AND AG_COY_ID='" & Common.Parse(companyID) & "' AND AG_STATUS='A'"
            End If


            ds = objDb.FillDs(strsql)
            getAssetGroupTypeAhead = ds
        End Function
        Public Function getAssetSubGroupTypeAhead(ByVal userID As String, ByVal companyID As String, Optional ByVal compid As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            If compid <> "" Then
                strsql = "SELECT AG_GROUP " _
                  & "FROM ASSET_GROUP " _
                  & "WHERE AG_GROUP_TYPE = 'S' AND AG_COY_ID='" & Common.Parse(compid) & "' AND AG_STATUS='A'"
            Else
                strsql = "SELECT AG_GROUP " _
                  & "FROM ASSET_GROUP " _
                  & "WHERE AG_GROUP_TYPE = 'S' AND AG_COY_ID='" & Common.Parse(companyID) & "' AND AG_STATUS='A'"
            End If


            ds = objDb.FillDs(strsql)
            getAssetSubGroupTypeAhead = ds
        End Function

        Public Function getCostAllocTypeAhead(ByVal userID As String, ByVal companyID As String, Optional ByVal compid As String = "", Optional ByVal strUserInput As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            If compid <> "" Then
                If strUserInput = "*" Then
                    strsql = "SELECT CAM_CA_CODE " _
                                     & "FROM COST_ALLOC_MSTR " _
                                     & "WHERE CAM_USER_ID = '" & Common.Parse(compid) & "' AND CAM_COY_ID ='" & Common.Parse(compid) & "' "

                Else
                    strsql = "SELECT CAM_CA_CODE " _
                 & "FROM COST_ALLOC_MSTR " _
                 & "WHERE CAM_USER_ID = '" & Common.Parse(compid) & "' AND CAM_COY_ID ='" & Common.Parse(compid) & "' and CAM_CA_CODE like '" & Common.Parse(strUserInput) & "%'"

                End If

            Else
                If strUserInput = "*" Then
                    strsql = "SELECT CAM_CA_CODE " _
                                     & "FROM COST_ALLOC_MSTR " _
                                     & "WHERE CAM_USER_ID = '" & Common.Parse(userID) & "' AND CAM_COY_ID ='" & Common.Parse(companyID) & "' "

                Else
                    strsql = "SELECT CAM_CA_CODE " _
                 & "FROM COST_ALLOC_MSTR " _
                 & "WHERE CAM_USER_ID = '" & Common.Parse(userID) & "' AND CAM_COY_ID ='" & Common.Parse(companyID) & "'  and CAM_CA_CODE like '" & Common.Parse(strUserInput) & "%'"

                End If

            End If

            ds = objDb.FillDs(strsql)
            getCostAllocTypeAhead = ds
        End Function
        Public Function getLateSubmitCheck(ByVal userID As String, ByVal companyID As String) As String
            Dim strsql As String
            Dim ds As New DataSet
            Dim LateSubmit As String

            strsql = "SELECT IP_PARAM_VALUE FROM IPP_PARAMETER WHERE IP_COY_ID = '" & Common.Parse(companyID) & "' AND IP_PARAM = 'LATE_SUBMIT_CHECK'"
            LateSubmit = objDb.GetVal(strsql)

            Return LateSubmit
        End Function

        Public Function getIPPCompanyIndex(ByVal userID As String, ByVal companyID As String, ByRef strVenName As String, ByVal blnIPPFinTeller As Boolean, Optional ByVal strVenStatus As String = "A") As Integer
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            Dim CompIdx As Integer
            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If

            'If blnIPPFinTeller Then
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            strsql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & strVenName & "' AND (IC_COY_TYPE = 'V' OR IC_COY_TYPE = 'E') AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' "
            If strVenStatus = "A" Then
                strsql &= "AND IC_STATUS = 'A'"

            End If
            'Else
            ''strsql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & strVenName & "' AND IC_COY_TYPE = 'V' AND IC_STATUS = 'A' AND IC_COY_ID = '" & Common.Parse(companyID) & "' "
            'strsql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & strVenName & "' AND IC_COY_TYPE = 'V' AND IC_COY_ID = '" & Common.Parse(companyID) & "' "
            'If strVenStatus = "A" Then
            '    strsql &= "AND IC_STATUS = 'A'"
            'End If
            'End If
            CompIdx = CInt(objDb.GetVal(strsql))

            Return CompIdx
        End Function

        Public Function SaveApprDoc(ByVal userID As String, ByVal companyID As String, ByVal invindex As String, ByVal wthtax As String, ByVal wthopt As String, ByVal wthremark As String, ByVal exchangerate As String, ByVal remark As String, ByVal internalremark As String, Optional ByVal aryDocDetails As ArrayList = Nothing, Optional ByVal strBillInvApprBy As String = "", Optional ByVal paymentmethod As String = "") As Boolean
            Dim strsql, strsql2, strsql3 As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            Dim strDocNo, strVenIdx As String

            strsql = "UPDATE INVOICE_MSTR SET "
            If wthtax <> "" Then
                strsql &= "IM_WITHHOLDING_TAX ='" & wthtax & "',"
            End If

            strsql &= "IM_WITHHOLDING_OPT='" & wthopt & "',"
            If wthremark <> "" Then
                strsql &= "IM_WITHHOLDING_REMARKS='" & Common.Parse(wthremark) & "',"
            End If
            If exchangerate <> "" Then
                strsql &= "IM_EXCHANGE_RATE='" & exchangerate & "',"
            End If
            If remark <> "" Then
                strsql &= "IM_REMARK='" & Common.Parse(internalremark) & "',"
            End If
            If internalremark <> "" Then
                strsql &= "IM_REMARKS='" & Common.Parse(remark) & "',"
            End If
            If strBillInvApprBy <> "" Then
                strsql &= "IM_REMARKS1='" & Common.Parse(strBillInvApprBy) & "',"
            End If
            strsql = strsql.Substring(0, strsql.Length - 1)
            strsql &= " WHERE IM_INVOICE_INDEX = " & invindex & " and im_b_coy_id = '" & companyID & "'"

            Common.Insert2Ary(strAryQuery, strsql)

            strDocNo = objDb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = " & invindex & "")
            strVenIdx = objDb.GetVal("select im_s_coy_id from invoice_mstr where im_invoice_index = " & invindex & "")

            For i = 0 To aryDocDetails.Count - 1
                If aryDocDetails(i)(2) <> "" Then
                    'Zulham Feb 05, 2014
                    'strsql3 = "UPDATE INVOICE_DETAILS SET ID_DR_EXCHANGE_RATE = '" & aryDocDetails(i)(3) & "' , ID_DR_CURRENCY = '" & aryDocDetails(i)(2) & "' " & _
                    '                          "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                    If paymentmethod = "TT" Or paymentmethod.ToString.ToUpper.Contains("NOSTRO") Then
                        strsql3 = "UPDATE INVOICE_DETAILS SET ID_DR_CURRENCY = '" & Common.parseNull(aryDocDetails(i)(2)) & "', id_gsT_value = '" & aryDocDetails(i)(4) & "', id_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', id_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "' " &
                        "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                    Else
                        strsql3 = "UPDATE INVOICE_DETAILS SET ID_DR_EXCHANGE_RATE = '" & Common.parseNull(aryDocDetails(i)(3)) & "' , ID_DR_CURRENCY = '" & Common.parseNull(aryDocDetails(i)(2)) & "', id_gsT_value = '" & aryDocDetails(i)(4) & "', id_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', id_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "' " &
                        "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                    End If
                    'End
                    Common.Insert2Ary(strAryQuery, strsql3)
                End If
            Next

            strsql2 = "UPDATE FINANCE_APPROVAL SET FA_AO_REMARK='" & Common.Parse(remark) & "' " &
                "WHERE FA_INVOICE_INDEX=" & invindex & " AND FA_SEQ = FA_AO_ACTION + 1"

            Common.Insert2Ary(strAryQuery, strsql2)

            If objDb.BatchExecute(strAryQuery) Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function UpdateIPPDocDetail(ByVal userID As String, ByVal companyID As String, ByVal aryDoc As ArrayList, ByVal Docno As String, ByVal venidx As String, Optional ByVal oldDocno As String = "", Optional ByVal MasterDoc As String = "N", Optional ByVal strIsGST As String = "")
            Dim i, j, intCostAllocIndex As Integer
            Dim strPayFor, strSQL, strAryQuery(0), strInvNo As String
            Dim InvLineAmt, Amount, ttlAmount As Double
            Dim dsDocLine As DataSet
            Dim GST As New GST

            'Jules 2018.07.09 - Allow "\" and "#"
            strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Replace(Replace(oldDocno, "\", "\\"), "#", "\#")) & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' and im_s_coy_id = '" & venidx & "'")

            If InStr(aryDoc.Item(i)(8), ":") Then
                aryDoc.Item(i)(8) = aryDoc.Item(i)(8).Substring(0, InStr(aryDoc.Item(i)(8), ":") - 1)
            End If

            If aryDoc.Item(i)(3) <> "" Then
                'Zulham 29062015 - HLB-IPP Stage 4(CR)
                'Disabled payfor ddl selected value is set to nothing for some reason
                If aryDoc.Item(i)(1) = "Own Co." Then
                    strPayFor = Common.Parse(companyID)
                ElseIf aryDoc.Item(i)(1) Is Nothing And Common.Parse(companyID).Trim.ToUpper = "HLISB" Then
                    strPayFor = Common.Parse(companyID)
                Else
                    strPayFor = Common.Parse(aryDoc.Item(i)(1))
                End If

                'Get the Tax Rate and the Percentage
                Dim gstTaxRate As String = "0"
                Dim gstPercentage As String = "0"
                GST.getGSTInfobyRate_ForIPP(Common.Parse(aryDoc.Item(i)(22)), gstPercentage, gstTaxRate)
                'End

                'aryDoc(i)(14) = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(aryDoc.Item(i)(13)) & "' and cc_coy_id = '" & Common.Parse(companyID) & "'")
                aryDoc(i)(14) = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(aryDoc.Item(i)(13)) & "' and cc_coy_id = '" & Common.Parse(Common.Parse(aryDoc.Item(i)(1))) & "'")
                aryDoc(i)(15) = objDb.GetVal("SELECT cbm_branch_name FROM company_branch_mstr WHERE cbm_branch_code = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and cbm_coy_id = '" & strPayFor & "'")
                'Zulham PAMB - 23/04/2018 - Added ID_GIFT column
                'Zulham - 30012019
                If strIsGST = "" Then
                    'Jules 2018.07.09 - Allow "\" and "#"
                    strSQL = "UPDATE INVOICE_DETAILS SET ID_PRODUCT_DESC='" & Common.Parse(aryDoc.Item(i)(3)) & "', " &
                                "ID_UOM='" & Common.Parse(aryDoc.Item(i)(4)) & "', " &
                                "ID_RECEIVED_QTY='" & Common.Parse(aryDoc.Item(i)(5)) & "', " &
                                "ID_UNIT_COST='" & Common.Parse(aryDoc.Item(i)(6)) & "', " &
                                "ID_B_GL_CODE='" & Common.Parse(aryDoc.Item(i)(8)) & "', " &
                                "ID_PAY_FOR='" & strPayFor & "', " &
                                "ID_REF_NO='" & Common.Parse(aryDoc.Item(i)(2)) & "', " &
                                "ID_COST_CENTER='" & Common.Parse(aryDoc.Item(i)(13)).Trim & "', " &
                                "ID_COST_CENTER_DESC='" & Common.Parse(aryDoc.Item(i)(14)).Trim & "', " &
                                "ID_BRANCH_CODE='" & Common.Parse(aryDoc.Item(i)(12)) & "', " &
                                "ID_BRANCH_CODE_NAME='" & Common.Parse(aryDoc.Item(i)(15)) & "', " &
                                "ID_COST_ALLOC_CODE='" & Common.Parse(aryDoc.Item(i)(11)) & "', " &
                                "ID_ASSET_GROUP='" & Common.Parse(aryDoc.Item(i)(9)) & "', " &
                                "ID_ASSET_GROUP_DESC='" & Common.Parse(aryDoc.Item(i)(16)) & "', " &
                                "ID_ASSET_SUB_GROUP='" & Common.Parse(aryDoc.Item(i)(10)) & "', " &
                                "id_glrule_category='" & Common.Parse(aryDoc.Item(i)(18)) & "', " &
                                "id_glrule_category_index='" & Common.Parse(aryDoc.Item(i)(19)) & "', " &
                                "id_gift='" & Common.Parse(aryDoc.Item(i)(37)) & "', " &
                                "ID_ASSET_SUB_GROUP_DESC='" & Common.Parse(aryDoc.Item(i)(17)) & "' " &
                                "WHERE ID_INVOICE_LINE='" & Common.Parse(aryDoc.Item(i)(0)) & "' AND ID_S_COY_ID='" & venidx & "' AND ID_INVOICE_NO='" & Common.Parse(Replace(Replace(oldDocno, "\", "\\"), "#", "\#")) & "'"
                Else

                    Dim strReimb = ""
                    Select Case aryDoc.Item(i)(20)
                        Case ""
                            strReimb = "N/A"
                        Case "R"
                            strReimb = "R"
                        Case "D"
                            strReimb = "D"
                    End Select

                    'mimi 2018-04-17 : withholding Tax
                    'Jules 2018.04.16 - PAMB Scrum 1 - Added Category and Analysis Code.
                    Dim strWithholdingTax = ""
                    If aryDoc.Item(i)(35) = "3" Then
                        strWithholdingTax &= "ID_WITHHOLDING_REMARKS ='" & Common.Parse(aryDoc.Item(i)(36)) & "'"
                    Else
                        If Common.Parse(aryDoc.Item(i)(24)) = "" Then
                            strWithholdingTax &= "ID_WITHHOLDING_TAX=NULL"
                        Else
                            strWithholdingTax &= "ID_WITHHOLDING_TAX='" & Common.Parse(aryDoc.Item(i)(24)) & "'"
                        End If

                    End If

                    'Jules 2018.07.09 - Allow "\" and "#"
                    'Zulham 07052015 IPP GST Stage 1
                    'Issue with gst amount haveing ','
                    strSQL = "UPDATE INVOICE_DETAILS SET ID_PRODUCT_DESC='" & Common.Parse(aryDoc.Item(i)(3)) & "', " &
                                "ID_UOM='" & Common.Parse(aryDoc.Item(i)(4)) & "', " &
                                "ID_RECEIVED_QTY='" & Common.Parse(aryDoc.Item(i)(5)) & "', " &
                                "ID_UNIT_COST='" & Common.Parse(aryDoc.Item(i)(6)) & "', " &
                                "ID_B_GL_CODE='" & Common.Parse(aryDoc.Item(i)(8)) & "', " &
                                "ID_PAY_FOR='" & strPayFor & "', " &
                                "ID_REF_NO='" & Common.Parse(aryDoc.Item(i)(2)) & "', " &
                                "ID_COST_CENTER='" & Common.Parse(aryDoc.Item(i)(13)).Trim & "', " &
                                "ID_COST_CENTER_DESC='" & Common.Parse(aryDoc.Item(i)(14)) & "', " &
                                "ID_BRANCH_CODE='" & Common.Parse(aryDoc.Item(i)(12)) & "', " &
                                "ID_BRANCH_CODE_NAME='" & Common.Parse(aryDoc.Item(i)(15)) & "', " &
                                "ID_COST_ALLOC_CODE='" & Common.Parse(aryDoc.Item(i)(11)) & "', " &
                                "ID_ASSET_GROUP='" & Common.Parse(aryDoc.Item(i)(9)) & "', " &
                                "ID_ASSET_GROUP_DESC='" & Common.Parse(aryDoc.Item(i)(16)) & "', " &
                                "ID_ASSET_SUB_GROUP='" & Common.Parse(aryDoc.Item(i)(10)) & "', " &
                                "id_glrule_category='" & Common.Parse(aryDoc.Item(i)(18)) & "', " &
                                "id_glrule_category_index='" & Common.Parse(aryDoc.Item(i)(19)) & "', " &
                                "ID_ASSET_SUB_GROUP_DESC='" & Common.Parse(aryDoc.Item(i)(17)) & "', " &
                                "id_gst_reimb ='" & Common.Parse(strReimb) & "', " &
                                "id_gst_value ='" & IIf(Common.Parse(aryDoc.Item(i)(21)) = "", 0, aryDoc.Item(i)(21).ToString.Replace(",", "")) & "', " &
                                "id_gst_input_tax_code ='" & Common.Parse(aryDoc.Item(i)(22)) & "', " &
                                "id_gst_output_tax_code ='" & Common.Parse(aryDoc.Item(i)(23)) & "', " &
                                "id_gst_rate ='" & Common.Parse(gstTaxRate) & "', " &
                                "id_gst ='" & Common.Parse(gstPercentage) & "', " &
                                "id_gift ='" & Common.Parse(aryDoc.Item(i)(37)) & "', " &
                                strWithholdingTax & ", " &  'mimi 2018-04-16 Scrum 1 : add holding tax
                                "ID_CATEGORY ='" & Common.Parse(aryDoc.Item(i)(25)) & "', " &
                                "ID_ANALYSIS_CODE1 ='" & Common.Parse(aryDoc.Item(i)(26)) & "', " &
                                "ID_ANALYSIS_CODE2 ='" & Common.Parse(aryDoc.Item(i)(27)) & "', " &
                                "ID_ANALYSIS_CODE3 ='" & Common.Parse(aryDoc.Item(i)(28)) & "', " &
                                "ID_ANALYSIS_CODE4 ='" & Common.Parse(aryDoc.Item(i)(29)) & "', " &
                                "ID_ANALYSIS_CODE5 ='" & Common.Parse(aryDoc.Item(i)(30)) & "', " &
                                "ID_ANALYSIS_CODE6 ='" & Common.Parse(aryDoc.Item(i)(31)) & "', " &
                                "ID_ANALYSIS_CODE7 ='" & Common.Parse(aryDoc.Item(i)(32)) & "', " &
                                "ID_ANALYSIS_CODE8 ='" & Common.Parse(aryDoc.Item(i)(33)) & "', " &
                                "ID_ANALYSIS_CODE9 ='" & Common.Parse(aryDoc.Item(i)(34)) & "', " &
                                "ID_WITHHOLDING_OPT ='" & Common.Parse(aryDoc.Item(i)(35)) & "' " & 'mimi 2018-04-18 : added withholding opt
                                "WHERE ID_INVOICE_LINE='" & Common.Parse(aryDoc.Item(i)(0)) & "' AND ID_S_COY_ID='" & venidx & "' AND ID_INVOICE_NO='" & Common.Parse(Replace(Replace(oldDocno, "\", "\\"), "#", "\#")) & "'"
                End If
            End If
            'objDb.Execute(strSQL)

            Common.Insert2Ary(strAryQuery, strSQL)

            strSQL = "update invoice_mstr set im_ind1 = '" & MasterDoc & "' where im_invoice_index = " & strInvNo
            Common.Insert2Ary(strAryQuery, strSQL)

            If aryDoc.Item(i)(11) <> "" Then
                intCostAllocIndex = CInt(objDb.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & aryDoc.Item(i)(11) & "' AND cam_coy_id = '" & Common.Parse(companyID) & "'"))
                dsDocLine = GetCostAllocDetail(userID, companyID, intCostAllocIndex)

                strSQL = " DELETE FROM invoice_details_ALLOC WHERE IDA_INVOICE_INDEX = '" & strInvNo & "'and ida_invoice_line = '" & Common.Parse(aryDoc.Item(i)(0)) & "'"
                Common.Insert2Ary(strAryQuery, strSQL)

                For j = 0 To dsDocLine.Tables(0).Rows.Count - 1

                    InvLineAmt = CDbl(Common.Parse(aryDoc.Item(i)(7)))
                    Amount = Format(Common.Parse(aryDoc.Item(i)(7)) * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00")
                    ttlAmount = ttlAmount + Amount

                    If j = dsDocLine.Tables(0).Rows.Count - 1 And ttlAmount <> InvLineAmt Then
                        Amount = Format(Common.Parse(aryDoc.Item(i)(7)) * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00") _
                        + (InvLineAmt - ttlAmount)
                    End If

                    strSQL = " INSERT INTO invoice_details_alloc(IDA_INVOICE_INDEX, IDA_INVOICE_LINE, IDA_COST_CENTER, IDA_COST_CENTER_DESC, IDA_BRANCH_CODE, IDA_BRANCH_NAME, IDA_PERCENT, IDA_AMOUNT)" &
                             "VALUES('" & strInvNo & "'," &
                             "" & aryDoc.Item(i)(0) & "," &
                             "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_CC_CODE")) & "'," &
                             "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CC_CC_DESC")) & "'," &
                             "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_BRANCH_CODE")) & "'," &
                             "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CBM_BRANCH_NAME")) & "'," &
                             "" & dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") & "," &
                             "" & Amount & ")"
                    Common.Insert2Ary(strAryQuery, strSQL)
                Next
            End If
            'ttlAmount = 0

            objDb.BatchExecute(strAryQuery)
        End Function
        Public Function SaveIPPDocDetail(ByVal userID As String, ByVal companyID As String, ByVal aryDoc As ArrayList, ByVal Docno As String, ByVal venidx As String, Optional ByVal MasterDoc As String = "N")
            Dim i, j, intCostAllocIndex As Integer
            Dim strPayFor, strSQL, strAryQuery(0), strReimb As String
            Dim InvLineAmt, Amount, ttlAmount As Double
            Dim GST As New GST
            Dim dsDocLine As DataSet
            Dim strInvNo As String
            strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Docno) & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' and im_s_coy_id = '" & venidx & "'")
            For i = 0 To aryDoc.Count - 1
                If aryDoc.Item(i)(3) <> "" Then
                    'Zulham 29062015 - HLB-IPP Stage 4(CR)
                    'Disabled payfor ddl selected value is set to nothing for some reason
                    If aryDoc.Item(i)(1) = "Own Co." Then
                        strPayFor = Common.Parse(companyID)
                    ElseIf aryDoc.Item(i)(1) Is Nothing And Common.Parse(companyID).Trim.ToUpper = "HLISB" Then
                        strPayFor = Common.Parse(companyID)
                    Else
                        strPayFor = Common.Parse(aryDoc.Item(i)(1))
                    End If


                    'aryDoc(i)(14) = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(aryDoc.Item(i)(13)) & "' and cc_coy_id = '" & Common.Parse(companyID) & "'")
                    aryDoc(i)(14) = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(aryDoc.Item(i)(13)) & "' and cc_coy_id = '" & Common.Parse(Common.Parse(aryDoc.Item(i)(1))) & "'")
                    aryDoc(i)(15) = objDb.GetVal("SELECT cbm_branch_name FROM company_branch_mstr WHERE cbm_branch_code = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and cbm_coy_id = '" & strPayFor & "'")

                    If InStr(aryDoc.Item(i)(8), ":") Then
                        aryDoc.Item(i)(8) = aryDoc.Item(i)(8).Substring(0, InStr(aryDoc.Item(i)(8), ":") - 1)
                    End If

                    Select Case aryDoc.Item(i)(20)
                        Case ""
                            strReimb = "N/A"
                        Case "R"
                            strReimb = "R"
                        Case "D"
                            strReimb = "D"
                    End Select

                    'Dim strWithholdingTax = ""
                    'If aryDoc.Item(i)(35) = "3" Then
                    '    strWithholdingTax &= "ID_WITHHOLDING_REMARKS ='" & Common.Parse(aryDoc.Item(i)(36))
                    'Else
                    '    strWithholdingTax &= "ID_WITHHOLDING_TAX='" & Common.Parse(aryDoc.Item(i)(24))
                    'End If

                    'Get the Tax Rate and the Percentage
                    Dim gstTaxRate As String = "0"
                    Dim gstPercentage As String = "0"
                    GST.getGSTInfobyRate_ForIPP(Common.Parse(aryDoc.Item(i)(22)), gstPercentage, gstTaxRate)
                    'End

                    'Jules 2018.04.16 - PAMB Scrum 1 - Added Category and Analysis Codes.
                    'mimi 2018-04-13 : PAMB Scrum 1 - Add Withholding Tax
                    'Zulham PAMB 23/04/2018 - Added gift
                    'Zulham 30012019
                    strSQL = " INSERT INTO INVOICE_DETAILS(" &
                       "ID_INVOICE_NO, ID_S_COY_ID, ID_INVOICE_LINE, ID_PRODUCT_DESC, ID_UOM, ID_RECEIVED_QTY, " &
                       "ID_UNIT_COST, ID_B_GL_CODE, ID_PAY_FOR, ID_REF_NO, ID_COST_CENTER, ID_COST_CENTER_DESC, " &
                       "ID_BRANCH_CODE, ID_BRANCH_CODE_NAME, ID_COST_ALLOC_CODE, ID_ASSET_GROUP, ID_ASSET_GROUP_DESC, " &
                       "ID_ASSET_SUB_GROUP, ID_ASSET_SUB_GROUP_DESC, id_glrule_category, id_glrule_category_index,id_gst_reimb,id_gst_value,id_gst_input_tax_code,id_gst_output_tax_code,ID_WITHHOLDING_TAX,ID_GST_RATE, ID_GST, " &
                       "ID_CATEGORY, ID_ANALYSIS_CODE1, ID_ANALYSIS_CODE2, ID_ANALYSIS_CODE3, ID_ANALYSIS_CODE4, ID_ANALYSIS_CODE5, ID_ANALYSIS_CODE6, ID_ANALYSIS_CODE7, ID_ANALYSIS_CODE8, ID_ANALYSIS_CODE9, ID_WITHHOLDING_OPT, ID_GIFT, ID_WITHHOLDING_REMARKS) " &
                       "VALUES('" & Common.Parse(Docno) & "'," &
                       "'" & Common.Parse(venidx) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(0)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(3)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(4)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(5)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(6)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(8)) & "'," &
                       "'" & strPayFor & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(2)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(13)).Trim & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(14)).Trim & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(12)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(15)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(11)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(9)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(16)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(10)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(17)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(18)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(19)) & "'," &
                       "'" & Common.Parse(strReimb) & "'," &
                       "'" & IIf(Common.Parse(aryDoc.Item(i)(21)) = "", 0, aryDoc.Item(i)(21).ToString.Replace(",", "")) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(22)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(23)) & "'," &
                       IIf(Common.Parse(aryDoc.Item(i)(24)) = "", "NULL", aryDoc.Item(i)(24).ToString.Replace(",", "")) & "," &  'mimi 2018-04-13 : PAMB Scrum 1 - Add Withholding Tax
                       "'" & Common.Parse(gstTaxRate) & "'," &
                       "'" & Common.Parse(gstPercentage) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(25)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(26)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(27)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(28)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(29)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(30)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(31)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(32)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(33)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(34)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(35)) & "'," & 'mimi 2018-04-18 : added withholding opt
                       "'" & Common.Parse(aryDoc.Item(i)(37)) & "'," & 'Zulham PAMB 23/04/2018 - Added gift
                       "'" & Common.Parse(aryDoc.Item(i)(36)) & "')" 'mimi 2018-04-18 : added witholding remarks

                    'objDb.Execute(strSQL)
                    Common.Insert2Ary(strAryQuery, strSQL)
                    'Common.Insert2Ary(boolAryQuery, "False")
                    If aryDoc.Item(i)(11) <> "" Then
                        intCostAllocIndex = CInt(objDb.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & aryDoc.Item(i)(11) & "' AND cam_coy_id = '" & Common.Parse(companyID) & "'"))
                        dsDocLine = GetCostAllocDetail(userID, companyID, intCostAllocIndex)
                        'strInvNo = "SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("DocNo")) & "' and im_b_coy_id = '" & Common.Parse(companyID)
                        'strInvNo = (objDb.GetLatestInsertedID2("INVOICE_MSTR", "IM_INVOICE_INDEX")) + 1
                        For j = 0 To dsDocLine.Tables(0).Rows.Count - 1

                            InvLineAmt = CDbl(Common.Parse(aryDoc.Item(i)(7)))
                            Amount = Format(Common.Parse(aryDoc.Item(i)(7)) * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00")
                            ttlAmount = ttlAmount + Amount

                            If j = dsDocLine.Tables(0).Rows.Count - 1 And ttlAmount <> InvLineAmt Then
                                Amount = Format(Common.Parse(aryDoc.Item(i)(7)) * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00") _
                                + (InvLineAmt - ttlAmount)
                            End If

                            strSQL = " INSERT INTO invoice_details_alloc(IDA_INVOICE_INDEX, IDA_INVOICE_LINE, IDA_COST_CENTER, IDA_COST_CENTER_DESC, IDA_BRANCH_CODE, IDA_BRANCH_NAME, IDA_PERCENT, IDA_AMOUNT)" &
                                     "VALUES('" & strInvNo & "'," &
                                     "" & aryDoc.Item(i)(0) & "," &
                                     "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_CC_CODE")) & "'," &
                                     "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CC_CC_DESC")) & "'," &
                                     "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_BRANCH_CODE")) & "'," &
                                     "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CBM_BRANCH_NAME")) & "'," &
                                     "" & dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") & "," &
                                     "" & Amount & ")"
                            Common.Insert2Ary(strAryQuery, strSQL)
                        Next
                    End If
                    ttlAmount = 0
                End If

            Next

            strSQL = "update invoice_mstr set im_ind1 = '" & MasterDoc & "' where im_invoice_index = " & strInvNo
            Common.Insert2Ary(strAryQuery, strSQL)

            objDb.BatchExecute(strAryQuery)
        End Function
        Public Function SaveIPPDocHeader(ByVal userID As String, ByVal companyID As String, ByVal dsDoc As DataSet, Optional ByVal blnSave As Boolean = False, Optional ByVal strOldDoc As String = "", Optional ByVal blnUpdate As Boolean = False)
            Dim strSQL As String
            Dim strAryQuery(0) As String
            Dim strUserDept As String
            Dim strIPPOF As String
            Dim blnIPPOF As Boolean
            Dim objUsers As New Users
            Dim strUserID As String

            strIPPOF = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_OfficerF)
            strIPPOF = "'" & Replace(strIPPOF, strIPPOF, "IPP Officer(F)") & "'"
            blnIPPOF = objUsers.checkUserFixedRole(userID, companyID, strIPPOF)

            'Zulham 08/06/2018 - PAMB
            strOldDoc = formatBackslash(strOldDoc)
            strOldDoc = Replace(strOldDoc, "#", "\#") 'Jules 2018.07.09

            If blnIPPOF = True Then
                strUserID = Common.parseNull(userID)
            End If

            strUserDept = objDb.GetVal("SELECT CDM_DEPT_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & Common.Parse(userID) & "' AND um_coy_id = '" & Common.Parse(companyID) & "')")

            If blnSave Then
                'For update
                Dim invidx As String
                'If dsDoc.Tables(0).Rows(0)("OldVenCompIDX") <> dsDoc.Tables(0).Rows(0)("VenCompIDX") Then
                If invidx = "" Then
                    invidx = objDb.GetVal("SELECT IM_INVOICE_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & strOldDoc & "' AND IM_B_COY_ID='" & Common.Parse(companyID) & "' and IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("OldVenCompIDX") & "'")

                End If
                'Else
                If invidx = "" Then
                    invidx = objDb.GetVal("SELECT IM_INVOICE_INDEX FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & strOldDoc & "' AND IM_B_COY_ID='" & Common.Parse(companyID) & "' and IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
                End If

                'End If
                If blnIPPOF = True Then
                    'Jules 2018.07.09 - Allow "#"
                    'Zulham 26/01/2016 - IPP Stage 4 Phase 2
                    'Added 2 lines for TotalAmtNoGST & GSTAmt
                    'Zulham 15/05/2017
                    'added 3 lines for withholding(wht) tax, wht option, wht remarks 
                    strSQL = "UPDATE INVOICE_MSTR SET IM_INVOICE_NO='" & Common.parseNull(Replace(formatBackslash(dsDoc.Tables(0).Rows(0)("DocNo")), "#", "\#")) & "'," &
                                    "IM_S_COY_ID='" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," &
                                    "IM_S_COY_NAME='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," &
                                    "IM_REMARK='" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," &
                                    "IM_INVOICE_TOTAL='" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," &
                                    "IM_PAYMENT_TERM='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("PaymentMethod")) & "'," &
                                    "IM_INVOICE_TYPE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," &
                                    "IM_DOC_DATE=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDate")) & "," &
                                    "IM_IPP_PO='" & Common.Parse(dsDoc.Tables(0).Rows(0)("ManualPONo")) & "'," &
                                    "IM_ADDR_LINE1='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," &
                                    "IM_ADDR_LINE2='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," &
                                    "IM_ADDR_LINE3='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," &
                                    "IM_POSTCODE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," &
                                    "IM_CITY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," &
                                    "IM_STATE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," &
                                    "IM_COUNTRY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," &
                                    "IM_LATE_REASON='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("LateSubmit"))) & "'," &
                                    "IM_CURRENCY_CODE='" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," &
                                    "IM_WITHHOLDING_TAX=" & dsDoc.Tables(0).Rows(0)("WHT") & "," &
                                    "IM_WITHHOLDING_OPT='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "'," &
                                    "IM_WITHHOLDING_REMARKS='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("NoWHTReason"))) & "', " &
                                    "IM_BANK_CODE = '" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," &
                                    "IM_DUE_DATE=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDueDate")) & "," &
                                    "IM_PRCS_SENT=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSSentDate")) & "," &
                                    "IM_PRCS_SENT_ID='" & strUserID & "'," &
                                    "IM_PRCS_RECV=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSReceivedDate")) & "," &
                                    "IM_PRCS_RECV_ID='" & strUserID & "'," &
                                    "IM_BANK_ACCT = '" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," &
                                    "IM_REMARKS2 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "', " &
                                    "im_ind1 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " &
                                    "im_company_category = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " &
                                    "im_resident_type = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " &
                                    "im_additional_1 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("EmpId")) & "', " &
                                    "im_invoice_excl_gst = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("TotalAmtNoGST")) & "', " &
                                    "im_withholding_tax = " & Common.Parse(dsDoc.Tables(0).Rows(0)("WHT")) & ", " &
                                    "im_withholding_opt = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "', " &
                                    "im_withholding_remarks = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("NoWHTReason")) & "', " &
                                    "im_invoice_gst = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("GSTAmt")) & "' " &
                                    "WHERE IM_INVOICE_INDEX='" & invidx & "'"
                Else
                    'Jules 2018.07.09 - Allow "#"
                    'Zulham 26/01/2016 - IPP Stage 4 Phase 2
                    'Added 2 lines for TotalAmtNoGST & GSTAmt
                    'Zulham 15/05/2017
                    'added 3 lines for withholding(wht) tax, wht option, wht remarks 
                    strSQL = "UPDATE INVOICE_MSTR SET IM_INVOICE_NO='" & Common.parseNull(Replace(formatBackslash(dsDoc.Tables(0).Rows(0)("DocNo")), "#", "\#")) & "'," &
                "IM_S_COY_ID='" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," &
                "IM_S_COY_NAME='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," &
                "IM_REMARK='" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," &
                "IM_INVOICE_TOTAL='" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," &
                "IM_PAYMENT_TERM='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("PaymentMethod")) & "'," &
                "IM_INVOICE_TYPE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," &
                "IM_DOC_DATE=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDate")) & "," &
                "IM_IPP_PO='" & Common.Parse(dsDoc.Tables(0).Rows(0)("ManualPONo")) & "'," &
                "IM_ADDR_LINE1='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," &
                "IM_ADDR_LINE2='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," &
                "IM_ADDR_LINE3='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," &
                "IM_POSTCODE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," &
                "IM_CITY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," &
                "IM_STATE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," &
                "IM_COUNTRY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," &
                "IM_LATE_REASON='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("LateSubmit"))) & "'," &
                "IM_CURRENCY_CODE='" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," &
                "IM_WITHHOLDING_TAX=" & dsDoc.Tables(0).Rows(0)("WHT") & "," &
                "IM_WITHHOLDING_OPT='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "'," &
                "IM_WITHHOLDING_REMARKS='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("NoWHTReason"))) & "', " &
                "IM_BANK_CODE = '" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," &
                "IM_DUE_DATE=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDueDate")) & "," &
                "IM_PRCS_SENT=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSSentDate")) & "," &
                "IM_PRCS_RECV=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSReceivedDate")) & "," &
                "IM_BANK_ACCT = '" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," &
                "IM_REMARKS2 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "', " &
                "im_ind1 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " &
                "im_company_category = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " &
                "im_resident_type = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " &
                "im_additional_1 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("EmpId")) & "', " &
                "im_invoice_excl_gst = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("TotalAmtNoGST")) & "', " &
                "im_withholding_tax = " & Common.Parse(dsDoc.Tables(0).Rows(0)("WHT")) & ", " &
                "im_withholding_opt = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "', " &
                "im_withholding_remarks = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("NoWHTReason")) & "', " &
                "im_invoice_gst = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("GSTAmt")) & "' " &
                "WHERE IM_INVOICE_INDEX='" & invidx & "'"
                End If

                Common.Insert2Ary(strAryQuery, strSQL)

                'Jules 2018.07.09 - Allow "\" and "#"
                'change vendor index at invoice detail
                strSQL = "UPDATE INVOICE_DETAILS SET ID_S_COY_ID='" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," &
                "ID_INVOICE_NO='" & Common.parseNull(Replace(formatBackslash(dsDoc.Tables(0).Rows(0)("DocNo")), "#", "\#")) & "' " &
                "WHERE ID_INVOICE_NO='" & strOldDoc & "' and ID_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("OldVenCompIDX") & "'"
                Common.Insert2Ary(strAryQuery, strSQL)

                'Dim intApprGrpIdx As Integer
                'Dim dsApprGrp As New DataSet
                'Dim intApprType As Integer

                ''If intInvIdx = 0 Then
                ''    intInvIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strOldDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' AND IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
                ''End If
                'SubmitIPPDoc(Common.parseNull(invidx), , strAryQuery)
                'SaveFinanceApproval(Common.parseNull(invidx), dsDoc, strAryQuery)

                '' If objDb.BatchExecuteWithFirstInsertedId(strAryQuery, boolAryQuery) Then

                Dim strStatus As String
                strStatus = getIPPStatus(userID, companyID, invidx)

                If strStatus = "14" And blnUpdate = False Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 10, " &
                             "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                             "IM_STATUS_CHANGED_ON = NOW()," &
                             "IM_BANK_CODE = '" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," &
                             "IM_BANK_ACCT = '" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'" &
                             "WHERE IM_INVOICE_INDEX = " & invidx & "  and im_b_coy_id = '" & companyID & "'"

                    Common.Insert2Ary(strAryQuery, strSQL)
                End If

                'If objDb.BatchExecute(strAryQuery) Then
                '    ' If strfrm = "Submit" Then
                '    ' strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("DocNo")) & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' AND IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
                '    sendMailToAO(dsDoc, invidx)
                '    'End If

                '    Return True
                'Else
                '    Return False
                'End If

            Else
                ' If strUserDept = "" Then
                If blnIPPOF = True Then
                    '"im_invoice_excl_gst = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("TotalAmtNoGST")) & "' " & _
                    '"im_invoice_gst = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("GSTAmt")) & "' " & _

                    'Jules 2018.07.09 - Allow "#"
                    'Zulham 26/01/2016 - IPP GST Stage 4 Phase 2
                    'Added 2 fields; im_invoice_excl_gst & im_invoice_gst
                    strSQL = " INSERT INTO INVOICE_MSTR(" &
                                            "IM_INVOICE_NO,IM_S_COY_ID,IM_S_COY_NAME,IM_B_COY_ID,IM_REMARK, " &
                                            "IM_CREATED_BY,IM_CREATED_ON,IM_INVOICE_STATUS,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " &
                                            "IM_INVOICE_TYPE,IM_DOC_DATE,IM_IPP_PO,IM_ADDR_LINE1, " &
                                            "IM_ADDR_LINE2,IM_ADDR_LINE3,IM_POSTCODE,IM_CITY,IM_STATE,IM_COUNTRY,IM_LATE_REASON, " &
                                            "IM_CURRENCY_CODE, IM_WITHHOLDING_TAX, IM_WITHHOLDING_OPT, IM_WITHHOLDING_REMARKS, IM_EXCHG_RATE,IM_BANK_CODE,IM_BANK_ACCT,IM_DUE_DATE,IM_PRCS_SENT,IM_PRCS_SENT_ID,IM_PRCS_RECV,IM_PRCS_RECV_ID,IM_REMARKS2,im_ind1,im_company_category,im_resident_type,im_additional_1,im_invoice_excl_gst,im_invoice_gst,IM_DEPT_INDEX) " &
                                            "VALUES('" & Common.parseNull(Replace(formatBackslash(dsDoc.Tables(0).Rows(0)("DocNo")), "#", "\#")) & "'," &
                                            "'" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," &
                                            "'" & Common.parseNull(companyID) & "'," &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," &
                                            "'" & userID & "'," &
                                            "NOW(),10," &
                                            "'" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("PaymentMethod")) & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," &
                                            "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDate") & " 00:00:00") & "," &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ManualPONo")) & "'," &
                                            "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," &
                                            "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," &
                                            "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("LateSubmit")) & "'," &
                                            "'" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," &
                                            "" & dsDoc.Tables(0).Rows(0)("WHT") & "," &
                                            "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "'," &
                                            "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("NoWHTReason"))) & "'," &
                                            "" & dsDoc.Tables(0).Rows(0)("ExchangeRate") & "," &
                                            "'" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," &
                                            "'" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," &
                                            "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDueDate") & " 00:00:00") & "," &
                                            "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSSentDate") & " 00:00:00") & "," &
                                            "'" & strUserID & "'," &
                                            "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSReceivedDate") & " 00:00:00") & "," &
                                            "'" & strUserID & "'," &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "'," &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("EmpId")) & "', " &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("TotalAmtNoGST")) & "', " &
                                            "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("GSTAmt")) & "' "
                Else
                    'Jules 2018.07.09 - Allow "#"
                    'Zulham 26/01/2016 - IPP GST Stage 4 Phase 2
                    'Added 2 fields; im_invoice_excl_gst & im_invoice_gst
                    strSQL = " INSERT INTO INVOICE_MSTR(" &
                        "IM_INVOICE_NO,IM_S_COY_ID,IM_S_COY_NAME,IM_B_COY_ID,IM_REMARK, " &
                        "IM_CREATED_BY,IM_CREATED_ON,IM_INVOICE_STATUS,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " &
                        "IM_INVOICE_TYPE,IM_DOC_DATE,IM_IPP_PO,IM_ADDR_LINE1, " &
                        "IM_ADDR_LINE2,IM_ADDR_LINE3,IM_POSTCODE,IM_CITY,IM_STATE,IM_COUNTRY,IM_LATE_REASON, " &
                        "IM_CURRENCY_CODE, IM_WITHHOLDING_TAX, IM_WITHHOLDING_OPT, IM_WITHHOLDING_REMARKS, IM_EXCHG_RATE,IM_BANK_CODE,IM_BANK_ACCT,IM_DUE_DATE,IM_PRCS_SENT,IM_PRCS_RECV,IM_REMARKS2,im_ind1,im_company_category,im_resident_type,im_additional_1,im_invoice_excl_gst,im_invoice_gst,IM_DEPT_INDEX) " &
                        "VALUES('" & Common.parseNull(Replace(formatBackslash(dsDoc.Tables(0).Rows(0)("DocNo")), "#", "\#")) & "'," &
                        "'" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," &
                        "'" & Common.parseNull(companyID) & "'," &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," &
                        "'" & userID & "'," &
                        "NOW(),10," &
                        "'" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("PaymentMethod")) & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," &
                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDate") & " 00:00:00") & "," &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ManualPONo")) & "'," &
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," &
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," &
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("LateSubmit")) & "'," &
                        "'" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," &
                        "" & dsDoc.Tables(0).Rows(0)("WHT") & "," &
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "'," &
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("NoWHTReason"))) & "'," &
                        "" & dsDoc.Tables(0).Rows(0)("ExchangeRate") & "," &
                        "'" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," &
                        "'" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," &
                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDueDate") & " 00:00:00") & "," &
                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSSentDate") & " 00:00:00") & "," &
                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSReceivedDate") & " 00:00:00") & "," &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "'," &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("EmpId")) & "', " &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("TotalAmtNoGST")) & "', " &
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("GSTAmt")) & "' "
                End If


                If strUserDept = "" Then

                    strSQL = strSQL & " , NULL)"
                Else

                    strSQL = strSQL & " ," & Common.parseNull(strUserDept) & ")"
                End If

                Common.Insert2Ary(strAryQuery, strSQL)
                strSQL = "SET @abc = LAST_INSERT_ID();"
                Common.Insert2Ary(strAryQuery, strSQL)
            End If
            objDb.BatchExecute(strAryQuery)
        End Function
        Public Function SaveIPPSubDoc(ByVal aryDoc As ArrayList, ByVal MstrDocIndex As Integer, Optional ByVal isUpdate As Boolean = False)
            Dim i, j, intCostAllocIndex As Integer
            Dim strPayFor, strSQL, strAryQuery(0) As String
            Dim InvLineAmt, Amount, ttlAmount As Double
            Dim dsDocLine As DataSet
            Dim strInvNo As String
            If Not aryDoc Is Nothing Then
                If Not aryDoc.Count = 0 Then
                    strSQL = "delete from ipp_sub_doc where isd_mstr_doc_index = " & MstrDocIndex
                    Common.Insert2Ary(strAryQuery, strSQL)
                    For i = 0 To aryDoc.Count - 1
                        If isUpdate = False Then
                            Dim _0 = CDec(aryDoc.Item(i)(3))
                            'Jules 2018.07.09 - Allow "\" and "#"
                            strSQL = " INSERT INTO IPP_SUB_DOC(" &
                                        "ISD_MSTR_DOC_INDEX, ISD_DOC_NO, ISD_DOC_DATE, ISD_DOC_AMT, ISD_DOC_GST_VALUE) " &
                                        "VALUES(" & MstrDocIndex & "," &
                                        "'" & Common.Parse(Replace(Replace(aryDoc(i)(1).ToString, "\", "\\"), "#", "\#")) & "'," &
                                        "" & Common.ConvertDate(aryDoc(i)(2).ToString) & "," &
                                        "" & CDec(aryDoc.Item(i)(3)) & "," & CDec(aryDoc.Item(i)(5)) & ")"
                        Else
                            'Jules 2018.07.09 - Allow "\" and "#"
                            strSQL = " Update IPP_SUB_DOC " &
                                      "set ISD_DOC_NO ='" & Common.Parse(Replace(Replace(aryDoc(i)(1).ToString, "\", "\\"), "#", "\#")) & "', ISD_DOC_DATE = " & Common.ConvertDate(aryDoc(i)(2).ToString) & ", ISD_DOC_AMT =" & CDec(aryDoc.Item(i)(3)) & ", ISD_DOC_GST_VALUE =" & CDec(aryDoc.Item(i)(5)) &
                                      " Where isd_sub_doc_index =" & Common.Parse(aryDoc.Item(i)(4)) & ""
                        End If

                        Common.Insert2Ary(strAryQuery, strSQL)
                    Next

                    strSQL = "update invoice_mstr set im_ind1 = 'Y' where im_invoice_index = " & MstrDocIndex
                    Common.Insert2Ary(strAryQuery, strSQL)

                    objDb.BatchExecute(strAryQuery)
                End If
            End If
        End Function
        'Zulham 26092018 - PAMB 
        'UAT U00003
        Public Function SaveIPPDoc(ByVal userID As String, ByVal companyID As String, ByRef dsDoc As DataSet, ByRef strfrm As String, ByRef strAction As String, ByRef intInvIdx As Integer, ByRef strOldDocNo As String, Optional ByVal intApprGrpIdx As String = "", Optional ByRef type As String = "", Optional ByVal isResident As String = "", Optional ByVal exchangeRate As Decimal = 1) As Boolean
            Dim i As Integer
            Dim strSQL As String
            Dim strPayFor As String
            Dim dsDocLine As New DataSet
            Dim strInvNo As String
            Dim Amount, ttlAmount, InvLineAmt As Decimal
            Dim j As Integer
            Dim intCostAllocIndex As Integer
            Dim strAryQuery(0) As String, boolAryQuery(0) As String
            Dim strUserDept As String
            Dim strStatus As String
            Dim strIPPO, strIPPOF As String
            Dim blnIPPO, blnIPPOF As Boolean
            Dim objUsers As New Users

            strIPPO = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_Officer)
            strIPPOF = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_OfficerF)
            strIPPO = "'" & Replace(strIPPO, "_", " ") & "'"
            strIPPOF = "'" & Replace(strIPPOF, strIPPOF, "IPP Officer(F)") & "'"

            blnIPPO = objUsers.checkUserFixedRole(userID, companyID, strIPPO)
            blnIPPOF = objUsers.checkUserFixedRole(userID, companyID, strIPPOF)

            Dim dsApprGrp As New DataSet
            Dim intApprType As Integer

            If intInvIdx = 0 Then
                intInvIdx = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strOldDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' AND IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
            End If
            SubmitIPPDoc(userID, companyID, Common.parseNull(intInvIdx), , strAryQuery, blnIPPO, blnIPPOF)
            'Zulham 26092018 - PAMB
            'UAT U00003
            SaveFinanceApproval(userID, companyID, Common.parseNull(intInvIdx), dsDoc, strAryQuery, intApprGrpIdx, type, isResident, CDec(dsDoc.Tables(0).Rows(0)("PaymentAmt")), exchangeRate) 'Zulham 27062018 - PAMB. Added type 

            If blnIPPO Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(intInvIdx), "IPP Teller", strAryQuery, "Submit Document")
            ElseIf blnIPPOF Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(intInvIdx), "Finance Teller", strAryQuery, "Submit Document")
            End If

            If objDb.BatchExecute(strAryQuery) Then
                'Zulham 03122018
                sendMailToIPPOfficer(userID, companyID, intInvIdx, "5", "submit")
                Return True
            Else
                Return False
            End If

        End Function

        Public Function GetCostAllocDetail(ByVal userID As String, ByVal companyID As String, ByVal intCostAlloc As Integer) As DataSet

            Dim strSql As String
            Dim dsDoc As New DataSet


            strSql = "SELECT cad_cam_index,cad_cc_code, IFNULL(cc_cc_desc,'') AS cc_cc_desc ,cad_branch_code, cbm_branch_name,cad_percent FROM cost_alloc_detail " &
                     "LEFT JOIN cost_centre ON cc_cc_code = cad_cc_code AND cc_coy_id = '" & companyID & "'" &
                     "INNER JOIN company_branch_mstr ON cbm_branch_code = cad_branch_code AND cbm_coy_id = '" & companyID & "'" &
                     "WHERE cad_cam_index =  '" & intCostAlloc & "'"


            dsDoc = objDb.FillDs(strSql)
            GetCostAllocDetail = dsDoc

        End Function

        Public Function SubmitIPPDoc(ByVal userID As String, ByVal companyID As String, ByVal strIPPDocNo As String, Optional ByVal dsDoc As DataSet = Nothing, Optional ByRef pQuery() As String = Nothing, Optional ByVal blnIPPO As Boolean = False, Optional ByVal blnIPPOF As Boolean = False) As Boolean

            Dim strSQL As String
            Dim strInvNo As String

            'strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strIPPDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'")

            If dsDoc Is Nothing Then
                If blnIPPO Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 16,IM_SUBMIT_DATE=NOW(), " &
                                          "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                                          "IM_STATUS_CHANGED_ON = NOW()" &
                                          "WHERE IM_INVOICE_INDEX = '" & strIPPDocNo & "' and im_b_coy_id = '" & companyID & "'"
                Else
                    'Zulham 11072018 - PAMB
                    'Send the doc to tax officer first
                    If companyID.ToString.ToUpper = "PAMB" Then
                        strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 19,IM_SUBMIT_DATE=NOW(), " &
                                                              "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                                                              "IM_STATUS_CHANGED_ON = NOW()" &
                                                              "WHERE IM_INVOICE_INDEX = '" & strIPPDocNo & "' and im_b_coy_id = '" & companyID & "'"
                    Else
                        strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 11,IM_SUBMIT_DATE=NOW(), " &
                                                              "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                                                              "IM_STATUS_CHANGED_ON = NOW()" &
                                                              "WHERE IM_INVOICE_INDEX = '" & strIPPDocNo & "' and im_b_coy_id = '" & companyID & "'"
                    End If
                End If
            End If
            Common.Insert2Ary(pQuery, strSQL)
            Return True

        End Function


        Public Function VoidIPPDoc(ByVal userID As String, ByVal companyID As String, ByVal strIPPDocNo As String) As Boolean

            Dim strSQL As String
            Dim strInvNo As String

            strInvNo = strIPPDocNo 'objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strIPPDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'")

            strSQL = "UPDATE INVOICE_MSTR SET " &
                  "IM_INVOICE_STATUS = 15," &
                  "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                  "IM_STATUS_CHANGED_ON = NOW()" &
                  " WHERE IM_INVOICE_INDEX = '" & strInvNo & "' and im_b_coy_id = '" & companyID & "'"


            objDb.Execute(strSQL)

            Return True

        End Function

        Public Function DelInvDetailsNAllocDetails(ByVal userID As String, ByVal companyID As String, ByVal dtInv As DataTable, ByVal strIPPDocNo As String, ByVal intVenCompIdx As Integer) As Boolean
            Dim i As Integer
            Dim strSQL As String
            Dim strInvIndex As String

            'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN invoice_details ON id_invoice_no = im_invoice_no  AND id_s_coy_id = im_s_coy_id WHERE im_b_coy_id = '" & companyID & "'  AND im_invoice_status = 10 AND id_cost_alloc_code =  '" & strCACode & "'") > 0 Then
            '    Return False
            '    Exit Function
            'End If

            strInvIndex = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strIPPDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'")

            strSQL = "DELETE FROM invoice_details WHERE id_invoice_no = '" & strIPPDocNo & "' and id_s_coy_id = '" & intVenCompIdx & "' and id_invoice_line = '" & Common.Parse(dtInv.Rows(i)("InvLineNo")) & "'"
            objDb.Execute(strSQL)

            For i = 0 To dtInv.Rows.Count - 1
                strSQL = " DELETE FROM invoice_details_alloc WHERE ida_invoice_INDEX = '" & strInvIndex & "'"

                objDb.Execute(strSQL)
            Next



            Return True

        End Function

        Public Function GetIPPDoc(ByVal userID As String, ByVal companyID As String, ByVal index As Integer) As DataSet

            Dim strSql As String
            Dim dsDoc As New DataSet
            'Modified for IPP GST Stage 2A - CH - 10 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strCoyId As String
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If
            '---------------------------------------------------

            'strSql = "SELECT * FROM invoice_mstr " & _
            '         "WHERE im_invoice_index  =  '" & index & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'"

            'strSql = "SELECT im.*, CM.CODE_DESC AS IM_STATE2,CM2.CODE_DESC AS IM_COUNTRY2 ,ic_bank_code,ic_bank_acct,ic_addr_line1,ic_addr_line2,ic_addr_line3,ic_postcode,ic_city,CM3.CODE_DESC AS IC_STATE,CM4.CODE_DESC AS IC_COUNTRY,IC_PAYMENT_METHOD,IC_COY_NAME,ic_credit_terms FROM invoice_mstr im " & _
            '"INNER JOIN CODE_MSTR CM ON CM.CODE_ABBR = im.IM_STATE AND CM.CODE_CATEGORY = 'S' " & _
            '"INNER JOIN CODE_MSTR CM2 ON CM2.CODE_ABBR = im.IM_COUNTRY AND CM2.CODE_CATEGORY = 'CT' " & _
            '"INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(companyID) & "' " & _
            '          "INNER JOIN CODE_MSTR CM3 ON CM3.CODE_ABBR = IC_STATE AND CM3.CODE_CATEGORY = 'S' " & _
            '"INNER JOIN CODE_MSTR CM4 ON CM4.CODE_ABBR = IC_COUNTRY AND CM4.CODE_CATEGORY = 'CT' " & _
            '    "WHERE im_invoice_index  =  '" & index & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'"

            'Modified for IPP GST Stage 2A - CH - 10 Feb 2015
            'Zulham 27/01/2016 - IPP Stage 4 Phase 2
            'Added 2 more fields; 
            strSql = "SELECT im.*, CM.CODE_DESC AS IM_STATE2,CM2.CODE_DESC AS IM_COUNTRY2 ,ic_bank_code,ic_bank_acct,ic_addr_line1,ic_addr_line2,ic_addr_line3,ic_postcode,ic_city,CM3.CODE_DESC AS IC_STATE,CM4.CODE_DESC AS IC_COUNTRY,IC_PAYMENT_METHOD,IC_COY_NAME,ic_credit_terms,im_invoice_excl_gst,im_invoice_gst FROM invoice_mstr im " &
            "INNER JOIN CODE_MSTR CM ON CM.CODE_ABBR = im.IM_STATE AND CM.CODE_CATEGORY = 'S' " &
            "INNER JOIN CODE_MSTR CM2 ON CM2.CODE_ABBR = im.IM_COUNTRY AND CM2.CODE_CATEGORY = 'CT' " &
            "INNER JOIN ipp_company ON im_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
                      "INNER JOIN CODE_MSTR CM3 ON CM3.CODE_ABBR = IC_STATE AND CM3.CODE_CATEGORY = 'S' " &
            "INNER JOIN CODE_MSTR CM4 ON CM4.CODE_ABBR = IC_COUNTRY AND CM4.CODE_CATEGORY = 'CT' " &
                "WHERE im_invoice_index  =  '" & index & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'"
            REM " = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("TotalAmtNoGST")) & "', " & _
            REM " = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("GSTAmt")) & "' " & _

            dsDoc = objDb.FillDs(strSql)
            GetIPPDoc = dsDoc

        End Function
        Public Function GetDocDetailTotalAmt(ByVal strDocNo As String, ByVal VenIdx As Integer) As String
            Return objDb.Get1ColumnCheckNull("invoice_details", "SUM(ID_RECEIVED_QTY * ID_UNIT_COST) AS ID_Amount", " WHERE id_invoice_no  =  '" & strDocNo & "' and id_s_coy_id = " & VenIdx & "")
        End Function
        Public Function DelIPPDocDetail(ByVal userID As String, ByVal companyID As String, ByVal slineno As String, ByVal invno As String, ByVal venidx As String)
            Dim strsql, strInvIndex As String

            strInvIndex = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & invno & "' and im_b_coy_id = '" & Common.Parse(companyID) & "'")

            strsql = "DELETE FROM INVOICE_DETAILS WHERE ID_INVOICE_NO='" & invno & "' " &
            "AND ID_S_COY_ID='" & venidx & "' AND ID_INVOICE_LINE='" & slineno & "'"
            objDb.Execute(strsql)

            strsql = " DELETE FROM invoice_details_ALLOC WHERE IDA_INVOICE_INDEX = '" & strInvIndex & "'and ida_invoice_line = '" & slineno & "'"
            objDb.Execute(strsql)
        End Function
        Public Function GetLatestLineNo(ByVal userID As String, ByVal companyID As String, ByVal invno As String, ByVal ven As String, ByVal blnIPPFinTeller As Boolean) As String
            Dim strsql As String
            Dim lineno As String
            Dim venidx As Integer
            venidx = getIPPCompanyIndex(userID, companyID, ven, blnIPPFinTeller, "I")
            strsql = "SELECT MAX(ID_INVOICE_LINE) FROM INVOICE_details WHERE " &
            "ID_INVOICE_NO = '" & invno & "' AND " &
            "ID_S_COY_ID = '" & venidx & "'"
            lineno = objDb.GetMax("INVOICE_DETAILS", "ID_INVOICE_LINE", " WHERE ID_INVOICE_NO = '" & invno & "' AND ID_S_COY_ID = '" & venidx & "'")
            Return lineno

        End Function
        Public Function GetIPPDocDetails(ByVal userID As String, ByVal companyID As String, ByVal strDocNo As String, ByVal VenIdx As Integer, Optional ByVal lineno As String = "", Optional ByVal oldstrDocNo As String = "") As DataSet
            Dim strSql As String
            Dim dsDoc As New DataSet
            'Modified for IPP GST Stage 2A - CH - 10 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strCoyId As String
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If

            'Jules 2018.07.10 - Allow "\" and "#" in Document No.
            oldstrDocNo = Replace(Replace(oldstrDocNo, "\", "\\"), "#", "\#")

            'If lineno <> "" Then
            '    strSql = "SELECT invoice_details.*,(ID_RECEIVED_QTY * ID_UNIT_COST) AS ID_Amount,cbg_b_gl_desc,IF(invoice_details.ID_BRANCH_CODE = '' AND invoice_details.ID_BRANCH_CODE_NAME = '','', CONCAT(invoice_details.ID_BRANCH_CODE,':',invoice_details.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(invoice_details.ID_COST_CENTER = '' AND invoice_details.ID_COST_CENTER_DESC = '','',CONCAT(invoice_details.ID_COST_CENTER,':',invoice_details.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM invoice_details " & _
            '            "INNER JOIN company_b_gl_code ON cbg_b_gl_code = id_b_gl_code AND cbg_b_coy_id = '" & Common.Parse(companyID) & "' " & _
            '         "WHERE id_invoice_no  =  '" & oldstrDocNo & "' and id_s_coy_id = " & VenIdx & " and id_invoice_line = '" & lineno & "'"

            'Else
            '    strSql = "SELECT invoice_details.*,(ID_RECEIVED_QTY * ID_UNIT_COST) AS ID_Amount,cbg_b_gl_desc,IF(invoice_details.ID_BRANCH_CODE = '' AND invoice_details.ID_BRANCH_CODE_NAME = '','', CONCAT(invoice_details.ID_BRANCH_CODE,':',invoice_details.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(invoice_details.ID_COST_CENTER = '' AND invoice_details.ID_COST_CENTER_DESC = '','',CONCAT(invoice_details.ID_COST_CENTER,':',invoice_details.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM invoice_details " & _
            '    "INNER JOIN company_b_gl_code ON cbg_b_gl_code = id_b_gl_code AND cbg_b_coy_id = '" & Common.Parse(companyID) & "' " & _
            '                         "WHERE id_invoice_no  =  '" & oldstrDocNo & "' and id_s_coy_id = " & VenIdx & ""

            'End If

            'Jules 2018.07.11 - Added Analysis Codes.
            'Jules 2018.07.05 - Added Withholding Tax. 
            'Modified for IPP GST Stage 2A - CH - 10 Feb 2015
            If lineno <> "" Then
                strSql = "SELECT invoice_details.*,(ID_RECEIVED_QTY * ID_UNIT_COST) AS ID_Amount,cbg_b_gl_desc,IF(invoice_details.ID_BRANCH_CODE = '' AND invoice_details.ID_BRANCH_CODE_NAME = '','', CONCAT(invoice_details.ID_BRANCH_CODE,':',invoice_details.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(invoice_details.ID_COST_CENTER = '' AND invoice_details.ID_COST_CENTER_DESC = '','',CONCAT(invoice_details.ID_COST_CENTER,':',invoice_details.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " &
                        "(SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) FROM TAX_MSTR, TAX WHERE TM_COY_ID = '" & companyID & "' AND TM_DELETED <> 'Y' AND TM_TAX_RATE = TAX_CODE AND TM_TAX_CODE = ID_GST_INPUT_TAX_CODE LIMIT 1) AS INPUT_GST, " &
                        "(SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) FROM TAX_MSTR, TAX WHERE TM_COY_ID = '" & companyID & "' AND TM_DELETED <> 'Y' AND TM_TAX_RATE = TAX_CODE AND TM_TAX_CODE = ID_GST_OUTPUT_TAX_CODE LIMIT 1) AS OUTPUT_GST, " &
                        "(SELECT CASE WHEN IFNULL(ID_WITHHOLDING_OPT,'0') = '3' THEN CAST(ID_WITHHOLDING_REMARKS AS CHAR(1000)) ELSE CAST(ID_WITHHOLDING_TAX AS UNSIGNED) END)  AS ID_WITHHOLDING_TAX2, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " &
                        "FROM invoice_details " &
                        "INNER JOIN company_b_gl_code ON cbg_b_gl_code = id_b_gl_code AND cbg_b_coy_id = '" & Common.Parse(strCoyId) & "' " &
                        "WHERE id_invoice_no  =  '" & oldstrDocNo & "' and id_s_coy_id = " & VenIdx & " and id_invoice_line = '" & lineno & "'"

            Else
                strSql = "SELECT invoice_details.*,(ID_RECEIVED_QTY * ID_UNIT_COST) AS ID_Amount,cbg_b_gl_desc,IF(invoice_details.ID_BRANCH_CODE = '' AND invoice_details.ID_BRANCH_CODE_NAME = '','', CONCAT(invoice_details.ID_BRANCH_CODE,':',invoice_details.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(invoice_details.ID_COST_CENTER = '' AND invoice_details.ID_COST_CENTER_DESC = '','',CONCAT(invoice_details.ID_COST_CENTER,':',invoice_details.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2, " &
                        "(SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) FROM TAX_MSTR, TAX WHERE TM_COY_ID = '" & companyID & "' AND TM_DELETED <> 'Y' AND TM_TAX_RATE = TAX_CODE AND TM_TAX_CODE = ID_GST_INPUT_TAX_CODE LIMIT 1) AS INPUT_GST, " &
                        "(SELECT IF(TAX_PERC = '', TM_TAX_CODE, CONCAT(TM_TAX_CODE, ' (', TAX_PERC, '%)')) FROM TAX_MSTR, TAX WHERE TM_COY_ID = '" & companyID & "' AND TM_DELETED <> 'Y' AND TM_TAX_RATE = TAX_CODE AND TM_TAX_CODE = ID_GST_OUTPUT_TAX_CODE LIMIT 1) AS OUTPUT_GST, " &
                        "(SELECT CASE WHEN IFNULL(ID_WITHHOLDING_OPT,'0') = '3' THEN CAST(ID_WITHHOLDING_REMARKS AS CHAR(1000)) ELSE CAST(ID_WITHHOLDING_TAX AS UNSIGNED) END)  AS ID_WITHHOLDING_TAX2, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE2) AS PRODUCTTYPE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE3) AS CHANNEL, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE4) AS REINSURANCECO, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE5) AS ASSETCODE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE, " &
                        "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE " &
                        "FROM invoice_details " &
                        "INNER JOIN company_b_gl_code ON cbg_b_gl_code = id_b_gl_code AND cbg_b_coy_id = '" & Common.Parse(strCoyId) & "' " &
                        "WHERE id_invoice_no  =  '" & oldstrDocNo & "' and id_s_coy_id = " & VenIdx & ""

            End If

            dsDoc = objDb.FillDs(strSql)
            GetIPPDocDetails = dsDoc

        End Function

        Public Function IsAlphaNum(ByVal pCheck As String) As Boolean
            IsAlphaNum = True
            Dim i As Long, j As Long
            If Len(pCheck) <> 0 Then

                Dim strCheck As String = "0123456789"
                strCheck = strCheck & "abcdefghijklmnopqrstuvwxyz"
                strCheck = strCheck & "ABCEDFGHIJKLMNOPQRSTUVWXYZ"

                Dim ch As Char
                For i = 1 To Len(pCheck)
                    ch = Mid(pCheck, i, 1)
                    For j = 1 To Len(strCheck)
                        If ch = Mid(strCheck, j, 1) Then Exit For
                    Next
                    If j = (Len(strCheck)) + 1 Then
                        IsAlphaNum = False
                        Exit Function
                    End If
                Next
            End If
        End Function
        Public Function IsNumeric(ByVal pCheck As String) As Boolean
            IsNumeric = True
            Dim i As Long, j As Long
            If Len(pCheck) <> 0 Then

                Dim strCheck As String = "0123456789"

                Dim ch As Char
                For i = 1 To Len(pCheck)
                    ch = Mid(pCheck, i, 1)
                    For j = 1 To Len(strCheck)
                        If ch = Mid(strCheck, j, 1) Then Exit For
                    Next
                    If j = (Len(strCheck)) + 1 Then
                        IsNumeric = False
                        Exit Function
                    End If
                Next
            End If
        End Function
        Public Function IsDecimal(ByVal pCheck As String) As Boolean
            IsDecimal = True
            Dim i As Long, j As Long
            If Len(pCheck) <> 0 Then

                Dim strCheck As String = "0123456789."

                Dim ch As Char
                For i = 1 To Len(pCheck)
                    ch = Mid(pCheck, i, 1)
                    For j = 1 To Len(strCheck)
                        If ch = Mid(strCheck, j, 1) Then Exit For
                    Next
                    If j = (Len(strCheck)) + 1 Then
                        IsDecimal = False
                        Exit Function
                    End If
                Next
            End If
        End Function

        Public Function CheckApprovalFlow(ByVal userID As String, ByVal companyID As String) As Boolean
            If objDb.Exist("SELECT '*' FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & Common.Parse(userID) & "' AND um_coy_id = '" & Common.Parse(companyID) & "')") > 0 Then
                Return False 'Not pop
            End If
            Return True
        End Function
        Public Function CheckDuplicatedEntry(ByVal userID As String, ByVal companyID As String, ByRef InvVen As String, ByRef DocNo As String, Optional ByVal InvIdx As Integer = 0) As Boolean
            Dim strsql As String

            If InvIdx = 0 Then
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE im_invoice_no = '" & DocNo & "' AND im_s_coy_id = '" & InvVen & "' AND im_b_coy_id = '" & Common.Parse(companyID) & "'") > 0 Then
                    Return True  'pop
                End If
            Else
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE im_invoice_index != " & InvIdx & " and im_invoice_no = '" & DocNo & "' AND im_s_coy_id = '" & InvVen & "' AND im_b_coy_id = '" & Common.Parse(companyID) & "'") > 0 Then
                    Return True  'pop
                End If
            End If



            Return False
        End Function

        Public Function CheckDuplicatedEntry2(ByVal userID As String, ByVal companyID As String, ByRef InvAmt As Decimal, ByRef InvDate As String, ByRef InvVen As String, ByRef DocNo As String, Optional ByVal InvIdx As Integer = 0) As Boolean
            Dim strsql As String


            If InvIdx = 0 Then
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE im_doc_date = " & InvDate & " AND im_invoice_total = " & InvAmt & " AND im_s_coy_id = '" & InvVen & "' AND im_b_coy_id = '" & Common.Parse(companyID) & "'") > 0 Then
                    Return True 'pop
                End If
            Else
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE im_invoice_index != " & InvIdx & " AND im_doc_date = " & InvDate & " AND im_invoice_total = " & InvAmt & " AND im_s_coy_id = '" & InvVen & "' AND im_b_coy_id = '" & Common.Parse(companyID) & "'") > 0 Then
                    Return True 'pop
                End If
            End If



            Return False
        End Function
        'Zulham 26092018 - PAMB
        'UAT U00003
        Public Function SaveFinanceApproval(ByVal userID As String, ByVal companyID As String, ByVal intInvIdx As String, ByVal dsDoc As DataSet, ByRef pQuery() As String, Optional ByVal intApprGrpIdx As String = "", Optional ByVal type As String = "", Optional ByVal isResident As String = "", Optional ByVal invoiceTotal As Decimal = 0.0, Optional ByVal exchangeRate As Decimal = 1.0)
            Dim i As Integer
            Dim strSQL As String
            Dim dsApprGrp As New DataSet
            Dim intApprType As Integer


            'Zulham 27062018 - PAMB
            'Zulham 26092018 - PAMB
            'UAT U00003
            If Not type.ToUpper = "E2P" Then
                strSQL = "SELECT * FROM approval_grp_ipp " &
                     "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' AND AGA_TYPE = 'AO' " &
                     "UNION ALL " &
                     "SELECT * FROM approval_grp_ipp " &
                     "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' AND AGA_TYPE = 'FO' " &
                     "UNION ALL " &
                     "SELECT * FROM approval_grp_ipp " &
                     "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' AND AGA_TYPE = 'FM' "
            Else
                'Seq: AO,TAX Verifier, FO, FM
                strSQL = "(SELECT DISTINCT 'AO' AS 'AGA_TYPE', AGA_AO 'AGA_AO', AGA_A_AO AS 'AGA_A_AO',NULL AS 'AGA_A_AO_2',NULL AS 'AGA_A_AO_3',NULL AS 'AGA_A_AO_4', " &
                        "AGA_GRP_INDEX AS 'AGA_GRP_INDEX', AGA_RELIEF_IND AS 'AGA_RELIEF_IND' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_ao On AGA_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGA_AO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGA_A_AO = um2.UM_USER_ID  " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX  " &
                        "AND AGM_TYPE='" & type.ToUpper & "' " &
                        "AND AGM_RESIDENT = '" & isResident.ToUpper & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & Common.Parse(companyID) & "' " &
                        "And um1.um_invoice_app_limit < " & invoiceTotal * exchangeRate & " " &
                        "And AGA_GRP_INDEX = '" & intApprGrpIdx & "' " &
                        "ORDER BY AGA_SEQ) " &
                        "UNION ALL " &
                        "(SELECT DISTINCT 'AO' AS 'AGA_TYPE', AGA_AO 'AGA_AO', AGA_A_AO AS 'AGA_A_AO',NULL AS 'AGA_A_AO_2',NULL AS 'AGA_A_AO_3',NULL AS 'AGA_A_AO_4', " &
                        "AGA_GRP_INDEX AS 'AGA_GRP_INDEX', AGA_RELIEF_IND AS 'AGA_RELIEF_IND' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_ao On AGA_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGA_AO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGA_A_AO = um2.UM_USER_ID  " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX  " &
                        "AND AGM_TYPE='" & type.ToUpper & "' " &
                        "AND AGM_RESIDENT = '" & isResident.ToUpper & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & Common.Parse(companyID) & "' " &
                        "And um1.um_invoice_app_limit >= " & invoiceTotal * exchangeRate & " " &
                        "And AGA_GRP_INDEX = '" & intApprGrpIdx & "' " &
                        "ORDER BY AGA_SEQ LIMIT 1) " &
                        "UNION ALL " &
                        "(SELECT DISTINCT 'FO' AS 'AGA_TYPE', AGFO_FO 'AGA_AO', AGFO_A_FO AS 'AGA_A_AO',AGFO_A_FO_2 AS 'AGA_A_AO_2',AGFO_A_FO_3 AS 'AGA_A_AO_3',AGFO_A_FO_4 AS 'AGA_A_AO_4', " &
                        "AGFO_GRP_INDEX AS 'AGA_GRP_INDEX', AGFO_RELIEF_IND AS 'AGA_RELIEF_IND' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FO On AGFO_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFO_FO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX  " &
                        "AND AGM_TYPE='" & type.ToUpper & "'  " &
                        "AND AGM_RESIDENT = '" & isResident.ToUpper & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & Common.Parse(companyID) & "' " &
                        "AND AGFO_GRP_INDEX = '" & intApprGrpIdx & "' " &
                        "AND AGFO_SEQ = '1' " &
                        "ORDER BY AGFO_SEQ " &
                        "LIMIT 1) " &
                        "UNION ALL " &
                        "(SELECT DISTINCT 'FO' AS 'AGA_TYPE', AGFO_FO 'AGA_AO', AGFO_A_FO AS 'AGA_A_AO',AGFO_A_FO_2 AS 'AGA_A_AO_2',AGFO_A_FO_3 AS 'AGA_A_AO_3',AGFO_A_FO_4 AS 'AGA_A_AO_4', " &
                        "AGFO_GRP_INDEX AS 'AGA_GRP_INDEX', AGFO_RELIEF_IND AS 'AGA_RELIEF_IND' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FO On AGFO_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFO_FO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX  " &
                        "AND AGM_TYPE='" & type.ToUpper & "'  " &
                        "AND AGM_RESIDENT = '" & isResident.ToUpper & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & Common.Parse(companyID) & "' " &
                        "AND AGFO_GRP_INDEX = '" & intApprGrpIdx & "' " &
                        "AND AGFO_SEQ <> '1' " &
                        "And um1.um_invoice_app_limit < " & invoiceTotal * exchangeRate & " " &
                        "ORDER BY AGFO_SEQ) " &
                        "UNION ALL " &
                        "(SELECT DISTINCT 'FO' AS 'AGA_TYPE', AGFO_FO 'AGA_AO', AGFO_A_FO AS 'AGA_A_AO',AGFO_A_FO_2 AS 'AGA_A_AO_2',AGFO_A_FO_3 AS 'AGA_A_AO_3',AGFO_A_FO_4 AS 'AGA_A_AO_4', " &
                        "AGFO_GRP_INDEX AS 'AGA_GRP_INDEX', AGFO_RELIEF_IND AS 'AGA_RELIEF_IND' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FO On AGFO_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFO_FO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX  " &
                        "AND AGM_TYPE='" & type.ToUpper & "'  " &
                        "AND AGM_RESIDENT = '" & isResident.ToUpper & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & Common.Parse(companyID) & "' " &
                        "AND AGFO_GRP_INDEX = '" & intApprGrpIdx & "' " &
                        "AND AGFO_SEQ <> '1' " &
                        "And um1.um_invoice_app_limit >= " & invoiceTotal * exchangeRate & " " &
                        "ORDER BY AGFO_SEQ LIMIT 1) " &
                        "UNION ALL " &
                        "(SELECT DISTINCT 'FM' AS 'AGA_TYPE', AGFM_FM 'AGA_AO', AGFM_A_FM AS 'AGA_A_AO',AGFM_A_FM_2 AS 'AGA_A_AO_2',AGFM_A_FM_3 AS 'AGA_A_AO_3',AGFM_A_FM_4 AS 'AGA_A_AO_4',  " &
                        "AGFM_GRP_INDEX AS 'AGA_GRP_INDEX', AGFM_RELIEF_IND AS 'AGA_RELIEF_IND'    " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FM On AGFM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFM_FM = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX  " &
                        "AND AGM_TYPE='" & type.ToUpper & "'  " &
                        "AND AGM_RESIDENT = '" & isResident.ToUpper & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & Common.Parse(companyID) & "' " &
                        "AND AGFM_GRP_INDEX = '" & intApprGrpIdx & "' " &
                        "ORDER BY AGFM_SEQ) "
            End If


            dsApprGrp = objDb.FillDs(strSQL)

            Dim blnChk As Boolean = False

            If objDb.Exist("SELECT '*' FROM FINANCE_APPROVAL WHERE FA_INVOICE_INDEX = '" & intInvIdx & "'") > 0 Then
                strSQL = "DELETE FROM finance_approval WHERE FA_INVOICE_INDEX = '" & intInvIdx & "'"
                Common.Insert2Ary(pQuery, strSQL)

            End If

            For i = 0 To dsApprGrp.Tables(0).Rows.Count - 1

                strSQL = " INSERT INTO finance_approval (FA_INVOICE_INDEX,FA_AGA_TYPE,FA_AO,FA_A_AO,FA_A_AO_2,FA_A_AO_3,FA_A_AO_4,FA_SEQ,FA_AO_ACTION,FA_APPROVAL_TYPE,FA_APPROVAL_GRP_INDEX,FA_RELIEF_IND)" &
                        "VALUES('" & intInvIdx & "'," &
                           "'" & dsApprGrp.Tables(0).Rows(i)("AGA_TYPE") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_AO") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_A_AO") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_A_AO_2") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_A_AO_3") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_A_AO_4") & "'," &
                         "" & i + 1 & "," &
                         "0 ," & intApprType & "," &
                        "'" & dsApprGrp.Tables(0).Rows(i)("AGA_GRP_INDEX") & "'," &
                           "'" & dsApprGrp.Tables(0).Rows(i)("AGA_RELIEF_IND") & "')"

                Common.Insert2Ary(pQuery, strSQL)

            Next

        End Function

        'Public Function Message(ByVal pg As System.Web.UI.Page, ByVal MsgID As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Invoice Payment")
        '    Dim strSQL As String
        '    Dim strMsg As String

        '    strSQL = "SELECT MM_MESSAGE FROM MESSAGE_MSTR WHERE MM_CODE = '" & MsgID & "'"
        '    strMsg = objDb.GetVal(strSQL)

        '    Dim vbs As String
        '    vbs = vbs & "<script language=""vbs"">"
        '    vbs = vbs & "Call MsgBox(""" & strMsg & """, " & style & ", """ & title & """)"
        '    vbs = vbs & "</script>"
        '    Dim rndKey As New Random
        '    pg.RegisterStartupScript(rndKey.Next.ToString, vbs)

        'End Function

        'Public Shared Sub Message(ByVal pg As System.Web.UI.Page, ByVal MsgID As String, ByVal pRedirect As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "Procurement")
        '    Dim strSQL As String
        '    Dim strMsg As String

        '    strSQL = "SELECT MM_MESSAGE FROM MESSAGE_MSTR WHERE MM_CODE = '" & MsgID & "'"
        '    strMsg = objDb.GetVal(strSQL)

        '    Dim vbs As String
        '    vbs = vbs & "<script language=""vbs"">"
        '    vbs = vbs & "Call MsgBox(""" & strMsg & """, " & style & ", """ & title & """)"
        '    vbs = vbs & vbLf & "window.location=""" & pRedirect & """"
        '    vbs = vbs & "</script>"
        '    Dim rndKey As New Random
        '    pg.RegisterStartupScript(rndKey.Next.ToString, vbs)
        'End Sub
        Public Function sendMailToIPPOfficer(ByVal userID As String, ByVal companyID As String, ByVal intInvIdx As Integer, ByVal role As String, ByVal mailtype As String, Optional ByVal intApprGrpIdx As String = "", Optional ByVal from As String = "")
            Dim strsql, strcond, strAO, currentseq As String
            Dim blnRelief As String
            Dim ds As New DataSet
            Dim dsDoc As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim dsAO As New DataSet
            Dim i, j As Integer
            'Dim intApprGrpIdx As Integer
            Dim dsApprGrp As New DataSet
            Dim strRouteTo As String
            Dim chkAO As String

            'get invoice detail
            dsDoc = GetIPPDoc(userID, companyID, intInvIdx)

            'intApprGrpIdx = objDB.GetVal("SELECT CDM_IPP_APPROVAL_GRP_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & dsDoc.Tables(0).Rows(0)("IM_CREATED_BY") & "' AND um_coy_id = '" & Common.Parse(companyID) & "')")

            If Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) <> "" Then
                If Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "INV" Then
                    strDocType = "Invoice"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "BILL" Then
                    strDocType = "Bill"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "CN" Then
                    strDocType = "Credit Note"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "DN" Then
                    strDocType = "Debit Note"
                ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "LETTER" Then
                    strDocType = "Letter"
                End If
            End If

            'Zulham 03122018
            If role = "3" Then
                role = "Finance Approver"
            ElseIf role = "2" Then
                role = "Finance Verifier"
            ElseIf role = "5" Then
                role = "IPP Verifier"
            ElseIf role = "6" Then
                role = "Source Verifier"
            End If

            'Zulham 04122018
            If mailtype = "approve" Then
                strBody &= "<P>The following Payment Document has been approved by the Finance Manager: " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & ") <BR>"
            ElseIf mailtype = "reject" Then
                'Zulham 17072018 - PAMB
                strBody &= "<P>The following Payment Document has been rejected by " & role.Replace("IPP", "E2P") & ": " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & ") <BR>"
            ElseIf mailtype = "verify" Or mailtype = "submit" Then
                strBody &= "<P>You have an outstanding " & strDocType.ToLower & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & ") waiting for approval.<BR>"
            End If

            'Zulham 03122018
            If Not mailtype = "verify" And Not mailtype = "submit" Then
                strBody &= "<P>Document(Type) : " & strDocType & "<BR>"
                strBody &= "Document No.      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & "<BR>"
                strBody &= "Document Date     : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_CREATED_ON")) & "<BR>"
                strBody &= "Vendor(Name)      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_S_COY_NAME")) & "<BR>"
                strBody &= "<P>For more details, please login to "
                strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            Else
                strBody &= "<P>For more details, please login to "
                strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            End If

            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

            strsql = "SELECT FA_RELIEF_IND FROM FINANCE_APPROVAL WHERE FA_INVOICE_INDEX = '" & intInvIdx & "' AND FA_SEQ = 1 AND FA_APPROVAL_GRP_INDEX = '" & intApprGrpIdx & "'"
            blnRelief = objDB.GetVal(strsql)

            If blnRelief = "O" Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            If role = "Finance Verifier" Then 'Finance Verifier
                strsql = "SELECT IFNULL(im_route_to,'') as im_route_to FROM invoice_mstr WHERE im_invoice_index = '" & intInvIdx & "' "
                strRouteTo = objDB.GetVal(strsql)

                'Zulham 03122018
                If Not mailtype = "verify" Then
                    If strRouteTo = "" Then
                        strsql = "SELECT IM_CREATED_BY AS FA_AO, UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, " &
                                    "'' AS FA_A_AO,'' AS AAO_NAME,'' AS AAO_EMAIL " &
                                    "FROM INVOICE_MSTR LEFT JOIN USER_MSTR UM ON IM_CREATED_BY = UM_USER_ID " &
                                    "WHERE IM_B_COY_ID='" & companyID & "' " &
                                    "AND IM_INVOICE_INDEX = " & intInvIdx & " AND UM.UM_COY_ID = '" & companyID & "'"
                        ds = objDB.FillDs(strsql)
                    Else
                        strsql = "SELECT im_prcs_recv_id AS FA_AO, UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, " &
                              "'' AS FA_A_AO,'' AS AAO_NAME,'' AS AAO_EMAIL " &
                              "FROM INVOICE_MSTR LEFT JOIN USER_MSTR UM ON im_prcs_recv_id = UM_USER_ID " &
                              "WHERE IM_B_COY_ID='" & companyID & "' " &
                              "AND IM_INVOICE_INDEX = " & intInvIdx & " AND UM.UM_COY_ID = '" & companyID & "'"
                        ds = objDB.FillDs(strsql)
                    End If
                ElseIf mailtype = "verify" Then
                    strsql = "Select um_email AS 'AO_EMAIL', um_user_name AS 'AO_NAME' " &
                            "FROM finance_approval, user_mstr " &
                            "WHERE fa_ao = um_user_id " &
                            "AND um_coy_id = '" & companyID & "' " &
                            "AND fa_invoice_index =  '" & intInvIdx & "' " &
                            "AND fa_seq = (SELECT MAX(fa_seq) FROM finance_approval WHERE fa_invoice_index = '" & intInvIdx & "' AND fa_ao = '" & userID & "' )+1 "
                    ds = objDB.FillDs(strsql)
                End If


                If ds.Tables(0).Rows.Count > 0 Then
                    Dim objMail As New AppMail

                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    'Zulham 03122018
                    If Not mailtype = "verify" Then
                        If strRouteTo = "" Then
                            objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Document Owner),  <BR>" & strBody
                        Else
                            objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Finance Teller),  <BR>" & strBody
                        End If
                    Else
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & ",  <BR>" & strBody
                    End If

                    'Agora -  E2P Document Pending Approval.
                    'Zulham 03122018
                    If mailtype = "reject" Then
                        objMail.Subject = "E2P Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & " Rejected" 'Zulham 14072018 - PAMB
                    ElseIf mailtype = "verify" Then
                        objMail.Subject = "Agora - E2P Document Pending Approval."
                    End If
                    objMail.SendMail()
                End If

            End If

            If role = "Finance Approver" Then 'Finance Approver

                strsql = "SELECT FA.FA_AGA_TYPE AS FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL, " &
                         "IFNULL(FA.FA_A_AO_2,'') AS FA_A_AO_2, IFNULL(UM3.UM_USER_NAME,'') AS AAO_NAME2, IFNULL(UM3.UM_EMAIL,'') AS AAO_EMAIL2, " &
                         "IFNULL(FA.FA_A_AO_3,'') AS FA_A_AO_3, IFNULL(UM4.UM_USER_NAME,'') AS AAO_NAME3, IFNULL(UM4.UM_EMAIL,'') AS AAO_EMAIL3, " &
                         "IFNULL(FA.FA_A_AO_4,'') AS FA_A_AO_4, IFNULL(UM5.UM_USER_NAME,'') AS AAO_NAME4, IFNULL(UM5.UM_EMAIL,'') AS AAO_EMAIL4,MAX(FA_SEQ)  " &
                         "FROM FINANCE_APPROVAL FA " &
                         "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_ACTIVE_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " &
                         "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " &
                         "LEFT JOIN USER_MSTR AS UM3 ON UM3.UM_USER_ID = FA.FA_A_AO_2 AND UM3.UM_STATUS = 'A' AND UM3.UM_DELETED = 'N' AND UM3.UM_COY_ID = '" & companyID & "' " &
                         "LEFT JOIN USER_MSTR AS UM4 ON UM4.UM_USER_ID = FA.FA_A_AO_3 AND UM4.UM_STATUS = 'A' AND UM4.UM_DELETED = 'N' AND UM4.UM_COY_ID = '" & companyID & "' " &
                         "LEFT JOIN USER_MSTR AS UM5 ON UM5.UM_USER_ID = FA.FA_A_AO_4 AND UM5.UM_STATUS = 'A' AND UM5.UM_DELETED = 'N' AND UM5.UM_COY_ID = '" & companyID & "' " &
                         "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_AGA_TYPE = 'FO' "

                ds = objDB.FillDs(strsql)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim objMail As New AppMail
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Finance Verifier), <BR>" & strBody
                    If mailtype = "reject" Then
                        objMail.Subject = "E2P Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & " Rejected" 'Zulham 14072018 - PAMB
                    End If
                    objMail.SendMail()
                End If

            End If

            If role = "IPP Verifier" Then
                'Zulham 03122018
                If Not mailtype = "submit" Then
                    strsql = "SELECT IM_CREATED_BY AS FA_AO, UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, " &
                    "'' AS FA_A_AO,'' AS AAO_NAME,'' AS AAO_EMAIL " &
                    "FROM INVOICE_MSTR LEFT JOIN USER_MSTR UM ON IM_CREATED_BY = UM_USER_ID " &
                    "WHERE IM_B_COY_ID='" & companyID & "' " &
                    "AND IM_INVOICE_INDEX = " & intInvIdx & " AND UM.UM_COY_ID = '" & companyID & "'"
                Else
                    If from = "PSD Received" Then
                        strsql = "SELECT um_email AS 'AO_EMAIL', um_user_name AS 'AO_NAME' " &
                                "From finance_approval, user_mstr " &
                                "WHERE fa_ao = um_user_id " &
                                "And um_coy_id = '" & companyID & "' " &
                                "And fa_invoice_index =  " & intInvIdx & " " &
                                "AND fa_aga_type = 'fo' " &
                                "ORDER BY fa_seq " &
                                "LIMIT 1"
                    Else
                        strsql = "SELECT um_email as 'AO_EMAIL', um_user_name AS 'AO_NAME' " &
                                "From finance_approval, user_mstr " &
                                "WHERE fa_ao = um_user_id " &
                                "And um_coy_id = '" & companyID & "' " &
                                "And fa_invoice_index = " & intInvIdx & " " &
                                "AND fa_seq = 1"
                    End If

                End If


                ds = objDB.FillDs(strsql)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim objMail As New AppMail

                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    If Not mailtype = "submit" Then
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (E2P Teller), <BR>" & strBody 'Zulham 14072018 - PAMB
                    ElseIf mailtype = "submit" Then
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & ", <BR>" & strBody 'Zulham 14072018 - PAMB
                    End If

                    If mailtype = "reject" Then
                        objMail.Subject = "E2P Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & " Rejected" 'Zulham 14072018 - PAMB
                    ElseIf mailtype = "submit" Then
                        objMail.Subject = "Agora - E2P Document Pending Approval."
                    End If
                    objMail.SendMail()
                End If

            End If

            'Zulham 03122018
            If role = "Source Verifier" Then

                strsql = "Select um_email AS 'AO_EMAIL', um_user_name AS 'AO_NAME' " &
                        "FROM finance_approval, user_mstr " &
                        "WHERE fa_ao = um_user_id " &
                        "AND um_coy_id = '" & companyID & "' " &
                        "AND fa_invoice_index =  '" & intInvIdx & "' " &
                        "AND fa_seq = (SELECT MAX(fa_seq) FROM finance_approval WHERE fa_invoice_index = '" & intInvIdx & "' AND fa_ao = '" & userID & "' )+1 "
                ds = objDB.FillDs(strsql)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim objMail As New AppMail
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & ",  <BR>" & strBody
                    objMail.Subject = "Agora - E2P Document Pending Approval."
                    objMail.SendMail()
                End If

            End If

            objCommon = Nothing
        End Function


        Public Function ReqCCnAG(ByVal userID As String, ByVal companyID As String, ByVal strGLCode As String) As DataSet
            Dim strsql, strCoyId As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH - 2 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If

            strsql = "SELECT cbg_cc_req,cbg_ag_req " _
                  & "FROM company_b_gl_code " _
                  & "WHERE cbg_b_coy_id = '" & Common.Parse(strCoyId) & "' AND cbg_status = 'A' AND cbg_b_gl_code = '" & strGLCode & "'" 'Modified for IPP GST Stage 2A - CH - 2 Feb 2015

            ds = objDb.FillDs(strsql)
            ReqCCnAG = ds
        End Function

        Function getIPPApprFlow(ByVal userID As String, ByVal companyID As String, ByVal intInvoiceNo As String, ByVal strCompanyId As String) As DataSet
            Dim objDb As New EAD.DBCom
            Dim strSql, strsql2, strsql3 As String
            Dim ds As DataSet
            Dim strCoyId As String = companyID
            Dim strDeptIdx As String
            Dim strInvStatus As String
            Dim currentfaaction As Integer
            Dim currentseq As String
            Dim role As String
            Dim blnAO As Boolean
            Dim strAOAction As String

            Dim strIPPOF As String
            Dim blnIPPOF As Boolean
            Dim objUsers As New Users
            Dim strUserID As String

            strIPPOF = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_Officer)
            strIPPOF = "'" & Replace(strIPPOF, strIPPOF, "IPP Officer") & "'"
            blnIPPOF = objUsers.checkUserFixedRole(userID, companyID, strIPPOF)

            currentfaaction = objDb.GetVal("SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "' ")
            currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' and fa_ao = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            role = objDb.Get1Column("FINANCE_APPROVAL", "FA_AGA_TYPE", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' and fa_seq = '" & currentfaaction + 1 & "'")
            blnAO = objDb.Exist("SELECT * FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "' AND fa_aga_type = 'AO'")
            strAOAction = objDb.Get1Column("FINANCE_APPROVAL", "FA_AO_ACTION", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' AND fa_aga_type = 'AO'")
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' and FA_A_AO = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' and FA_A_AO_2 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' and FA_A_AO_3 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvoiceNo & "' and FA_A_AO_4 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If

            'strsql2 = "SELECT cdm_dept_index FROM invoice_mstr " & _
            '          "INNER JOIN company_dept_mstr ON cdm_dept_index = im_dept_index " & _
            '          "WHERE im_invoice_index = '" & intInvoiceNo & "'"

            'strDeptIdx = objDb.GetVal(strsql2)

            strsql3 = "SELECT im_invoice_status FROM invoice_mstr " &
                  "WHERE im_invoice_index = '" & intInvoiceNo & "'"

            strInvStatus = objDb.GetVal(strsql3)

            If blnAO = False Then 'for finance user approval workflow panel
                strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                        "'Finance Verifier' AS 'Action', " &
                        "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                        "FROM FINANCE_APPROVAL FA " &
                        "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                        "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                        "AND fa_aga_type = 'FO' " &
                            "UNION ALL " &
                        "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                        "'Finance Approver' AS 'Action', " &
                        "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                        "FROM FINANCE_APPROVAL FA " &
                        "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                        "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                        "AND fa_aga_type = 'FM' " &
                            "UNION ALL " &
                        "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                        "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                        "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                        "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                        "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',1 AS FA_SEQ2 " &
                        "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                        "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  ORDER BY FA_SEQ2 "


                'strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME,CASE WHEN fa_aga_type = 'FO' THEN 'Finance Verifier' WHEN fa_aga_type = 'FM' THEN 'Finance Approver' END AS 'Action' ,FA.FA_SEQ + 1 AS FA_SEQ2 " & _
                '                   "FROM FINANCE_APPROVAL FA " & _
                '                   "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & strCompanyId & "', INVOICE_MSTR " & _
                '                   "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & strCompanyId & "'  AND fa_action_date IS NOT NULL " & _
                '                   "UNION ALL " & _
                '                   "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ,im_created_by AS FA_ACTIVE_AO, " & _
                '                   "im_created_on AS FA_ACTION_DATE,(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,im_remark AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
                '                   "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',1 as FA_SEQ2 " & _
                '                   "FROM invoice_mstr " & _
                '                   "LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & strCompanyId & "' " & _
                '                   "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  ORDER BY FA_SEQ "

            Else 'for source dept approval workflow panel
                If strInvStatus = "17" Then
                    strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'IPP Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'AO' " &
                                 "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FO' " &
                                  "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Approver' AS 'Action', " &
                             "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FM' " &
                                  "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ, " &
                             "im_prcs_sent_id AS FA_ACTIVE_AO, im_prcs_sent_upd_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Sent Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,IF(uU_usrgrp_id = 'IPP Approving Officer','IPP Verifier','IPP Teller') AS 'Action',3 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_sent_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "JOIN USERS_USRGRP ON uu_user_id = UMA.UM_USER_ID AND uu_coy_id = UM_COY_ID AND uU_usrgrp_id LIKE '%officer%' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_sent_upd_date IS NOT NULL " &
                             "ORDER BY FA_SEQ2 "

                ElseIf strInvStatus = "11" Or strInvStatus = "12" Or strInvStatus = "13" Or strInvStatus = "4" Then
                    strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'IPP Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'AO' " &
                                 "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 3 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FO' " &
                                  "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Approver' AS 'Action', " &
                             "FA.FA_SEQ + 3 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FM' " &
                                  "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ, " &
                             "im_prcs_sent_id AS FA_ACTIVE_AO, im_prcs_sent_upd_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Sent Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,IF(uU_usrgrp_id = 'IPP Approving Officer','IPP Verifier','IPP Teller') AS 'Action',3 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_sent_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "JOIN USERS_USRGRP ON uu_user_id = UMA.UM_USER_ID AND uu_coy_id = UM_COY_ID AND uU_usrgrp_id LIKE '%officer%' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_sent_upd_date IS NOT NULL " &
                                "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 4 AS FA_SEQ, " &
                             "im_prcs_recv_id AS FA_ACTIVE_AO, im_prcs_recv_upd_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Received Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',4 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_recv_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_recv_upd_date IS NOT NULL " &
                             "ORDER BY FA_SEQ2 "


                ElseIf strInvStatus = "14" And (role = "FO" Or role = "FM") Then
                    'Zulham 15072018 - PAMB
                    'shows itl trans date + remarks for sixth query
                    'Zulham 15102018 - PAMB SST
                    strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'IPP Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'AO' " &
                                    "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 3 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FO' " &
                                    "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Approver' AS 'Action', " &
                             "FA.FA_SEQ + 3 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FM' " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ, " &
                             "itl_user_id AS FA_ACTIVE_AO, itl_trans_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,itl_remarks AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',3 AS FA_SEQ2 " &
                             "FROM invoice_mstr " &
                             "LEFT OUTER JOIN ipp_trans_log ON itl_invoice_index = im_invoice_index AND itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'IPP Teller') " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON itl_user_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' " &
                               "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 4 AS FA_SEQ, " &
                             "itl_user_id AS FA_ACTIVE_AO, itl_trans_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',4 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN ipp_trans_log ON itl_invoice_index = im_invoice_index  AND itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'Finance Teller') LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_recv_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' " &
                             "ORDER BY FA_SEQ2 "
                    'AND im_prcs_recv_upd_date IS NOT NULL 
                ElseIf strInvStatus = "14" And role = "AO" And strAOAction <> "0" Then
                    strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'IPP Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'AO' " &
                                    "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FO' " &
                                    "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Approver' AS 'Action', " &
                             "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FM' " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ,  " &
                             "itl_user_id AS FA_ACTIVE_AO, itl_trans_date AS FA_ACTION_DATE,  " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval  " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,itl_remarks AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index,  " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',3 AS FA_SEQ2  " &
                             "FROM(invoice_mstr) " &
                            "LEFT OUTER JOIN ipp_trans_log ON itl_invoice_index = im_invoice_index AND itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'Finance Teller') " &
                            "LEFT OUTER JOIN USER_MSTR UMA ON itl_user_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                            "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' " &
                             "ORDER BY FA_SEQ2 "
                ElseIf strInvStatus = "14" And role = "AO" And strAOAction = "0" Then
                    Dim chkLastActionUser As String

                    chkLastActionUser = objDb.GetVal("SELECT ITL_PERFORMED_BY FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND " &
                    "itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "')")

                    If chkLastActionUser = "Finance Teller" Then
                        strSql = "SELECT itl_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4,  " &
                             "FA_SEQ,itl_user_id  AS FA_ACTIVE_AO,itl_trans_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK, " &
                             "'' AS fa_approval_type, '' AS fa_approval_grp_index, '' AS fa_on_behalfof,'' AS fa_relief_ind, " &
                             "UM_USER_NAME AS AO_NAME, 'IPP Verifier' AS 'Action',FINANCE_APPROVAL.FA_SEQ + 1 AS FA_SEQ2   " &
                             "FROM(ipp_trans_log) " &
                             "LEFT OUTER JOIN USER_MSTR ON itl_user_id=UM_USER_ID AND UM_COY_ID='" & companyID & "' " &
                             "LEFT JOIN FINANCE_APPROVAL ON itl_invoice_index = fa_invoice_index AND fa_aga_type = 'AO' " &
                             "WHERE itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'IPP Verifier') " &
                                "UNION ALL  " &
                             "SELECT itl_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, " &
                             "0 AS FA_SEQ,itl_user_id  AS FA_ACTIVE_AO,itl_trans_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK, " &
                             "'' AS fa_approval_type, '' AS fa_approval_grp_index, '' AS fa_on_behalfof,'' AS fa_relief_ind, " &
                             "UM_USER_NAME AS AO_NAME, 'IPP Teller' AS 'Action',3 AS FA_SEQ2   " &
                             "FROM(ipp_trans_log) " &
                             "LEFT OUTER JOIN USER_MSTR ON itl_user_id=UM_USER_ID AND UM_COY_ID='" & companyID & "' " &
                             "WHERE itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'IPP Teller') " &
                                "UNION ALL " &
                             "SELECT itl_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, " &
                             "0 AS FA_SEQ,itl_user_id  AS FA_ACTIVE_AO,itl_trans_date AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK, " &
                             "'' AS fa_approval_type, '' AS fa_approval_grp_index, '' AS fa_on_behalfof,'' AS fa_relief_ind, " &
                             "UM_USER_NAME AS AO_NAME, 'Finance Teller' AS 'Action',4 AS FA_SEQ2  " &
                             "FROM(ipp_trans_log) " &
                             "LEFT OUTER JOIN USER_MSTR ON itl_user_id=UM_USER_ID AND UM_COY_ID='" & companyID & "' " &
                             "WHERE itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'Finance Teller') " &
                                "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                             "ORDER BY FA_SEQ2 "
                    Else
                        strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                                                     "'IPP Verifier' AS 'Action', " &
                                                     "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                                                     "FROM FINANCE_APPROVAL FA " &
                                                     "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                                                     "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                                                     "AND fa_aga_type = 'AO' " &
                                                            "UNION ALL " &
                                                     "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                                                     "'Finance Verifier' AS 'Action', " &
                                                     "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                                                     "FROM FINANCE_APPROVAL FA " &
                                                     "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                                                     "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                                                     "AND fa_aga_type = 'FO' " &
                                                            "UNION ALL " &
                                                     "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                                                     "'Finance Approver' AS 'Action', " &
                                                     "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                                                     "FROM FINANCE_APPROVAL FA " &
                                                     "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                                                     "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                                                     "AND fa_aga_type = 'FM' " &
                                                            "UNION ALL " &
                                                     "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                                                     "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                                                     "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                                                     "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                                                     "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                                                     "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                                                     "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                                                     "ORDER BY FA_SEQ2 "
                    End If

                Else
                    strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'IPP Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'AO' " &
                                    "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Verifier' AS 'Action', " &
                             "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FO' " &
                                    "UNION ALL " &
                             "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " &
                             "'Finance Approver' AS 'Action', " &
                             "FA.FA_SEQ + 2 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "'  AND fa_action_date IS NOT NULL " &
                             "AND fa_aga_type = 'FM' " &
                                    "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                             "ORDER BY FA_SEQ2 "

                End If

            End If

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function ApproveIPPDoc(ByVal userID As String, ByVal companyID As String, ByVal strIPPDocIdx As String, ByVal strRemark As String, ByRef blnRelief As Boolean, ByVal role As String, Optional ByVal paymentmethod As String = "", Optional ByVal exchangeRate As String = "",
                                      Optional ByVal paymenttype As String = "", Optional ByVal strTime As String = "", Optional ByVal intApprGrpIndex As String = "", Optional ByVal strBillInvApprBy As String = "",
                                      Optional ByVal aryDocDetails As ArrayList = Nothing, Optional ByVal chkConvertToRentas As Boolean = False, Optional ByVal currentStatus As Integer = 0, Optional ByVal blnCanApprove As Boolean = False) As Boolean
            Dim TimeNow As String = DateTime.Now.ToString("HH:mm")
            Dim strSQL, strSqlAry(0) As String
            Dim strDateTime As String
            Dim i As Integer
            Dim strDocNo, strVenIdx As String

            strSQL = " SET @DUPLICATE_CHK =''; update invoice_mstr set im_invoice_status = im_invoice_status wHERE IM_INVOICE_INDEX = " & strIPPDocIdx
            Common.Insert2Ary(strSqlAry, strSQL)

            Dim status = ""
            If role.ToString = "2" Then
                status = "12"
            ElseIf role.ToString = "3" Then
                status = "13"
            End If

            strSQL = " SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF(IM_INVOICE_STATUS = '" & status & "' ,'ACK', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM INVOICE_MSTR WHERE IM_INVOICE_INDEX = " & strIPPDocIdx
            Common.Insert2Ary(strSqlAry, strSQL)


            If role = "2" Then ' FO
                If currentStatus = "19" Or currentStatus = "14" Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 11," &
                    "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                    "IM_STATUS_CHANGED_ON = NOW(), " &
                    "IM_ROUTE_TO = Null " &
                    "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                Else
                    'Zulham 04072018 - PAMB
                    If blnCanApprove Then
                        strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 12," &
                    "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                    "IM_STATUS_CHANGED_ON = NOW(),IM_REMARKS1 = '" & Common.Parse(strBillInvApprBy) & "' " &
                    "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                    End If
                End If
            ElseIf role = "FinanceTeller" Then ' Finance Teller
                'Zulham 28062018 - PAMB
                'Redirect to Tax Officer
                If Not companyID.ToString.ToUpper = "PAMB" Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 11," &
                    "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                    "IM_STATUS_CHANGED_ON = NOW(), " &
                    "IM_ROUTE_TO = Null " &
                    "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                Else
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 19," &
                    "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                    "IM_STATUS_CHANGED_ON = NOW(), " &
                    "IM_ROUTE_TO = Null " &
                    "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                End If
                'End
            ElseIf role = "3" Then ' FM
                If paymentmethod = "RENTAS" Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_PAYMENT_TERM = 'RENTAS' " &
                             "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"

                    Common.Insert2Ary(strSqlAry, strSQL)
                End If
                If IsLastFM(strIPPDocIdx) Then
                    'zulham 11/03/2015 IPP GST Stage 2B
                    If paymentmethod = "RENTAS" Or paymentmethod = "TT" Or paymentmethod.ToUpper.Contains("NOSTRO") Then 'set it to paid
                        'Zulham 18092018 - PAMB
                        'Set the status to 13 instead of 4
                        If CDate(TimeNow) > CDate(strTime) Then
                            strDateTime = DateTime.Today.AddDays(+1).ToString("yyyy-MM-dd") & " " & DateTime.Now.ToString("HH:mm:ss") 'DateTime.Today.TimeOfDay.ToString

                            strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 13," &
                                                  "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                                                  "IM_STATUS_CHANGED_ON = NOW()," &
                                                  "IM_PAYMENT_DATE = '" & strDateTime & "'," &
                                                  "IM_FM_APPROVED_DATE = NOW(),IM_REMARKS1 = '" & Common.Parse(strBillInvApprBy) & "' " &
                                                  "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                        Else
                            strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 13," &
                          "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                          "IM_STATUS_CHANGED_ON = NOW()," &
                          "IM_PAYMENT_DATE = NOW()," &
                          "IM_FM_APPROVED_DATE = NOW(), IM_REMARKS1 = '" & Common.Parse(strBillInvApprBy) & "'  " &
                          "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                        End If

                    Else ' set it to approved
                        strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 13," &
                        "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                        "IM_STATUS_CHANGED_ON = NOW()," &
                        "IM_FM_APPROVED_DATE = NOW(),IM_REMARKS1 = '" & Common.Parse(strBillInvApprBy) & "'  " &
                        "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                    End If
                Else
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 12," &
                            "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                            "IM_STATUS_CHANGED_ON = NOW() ,IM_REMARKS1 = '" & Common.Parse(strBillInvApprBy) & "' " &
                            "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"

                End If
            ElseIf role = "5" Then 'AO
                'Zulham 26102018 - PAMB
                If blnCanApprove Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 17," &
                  "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                  "IM_STATUS_CHANGED_ON = NOW(),IM_REMARKS1 = '" & Common.Parse(strBillInvApprBy) & "', " &
                  "IM_PRCS_SENT = NOW(), " &
                  "IM_PRCS_SENT_UPD_DATE = NOW(), " &
                  "IM_PRCS_SENT_ID = '" & userID & "' " &
                  "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                End If
            End If

            Common.Insert2Ary(strSqlAry, strSQL)

            'Zulham 17042015 IPP GST STAGE 2B
            'Check for TT & Nostro to add the exchange rate
            If (paymentmethod = "TT" Or paymentmethod.ToUpper.Contains("NOSTRO")) And exchangeRate <> "" Then
                strSQL = "UPDATE INVOICE_MSTR SET IM_EXCHANGE_RATE ='" & exchangeRate & "' " &
                "WHERE IM_INVOICE_INDEX = " & strIPPDocIdx & " and im_b_coy_id = '" & companyID & "'"
                Common.Insert2Ary(strSqlAry, strSQL)
            End If

            'for update line item's exchange rate & currency for related company only
            If (role = "2" Or role = "3") And aryDocDetails IsNot Nothing Then
                strDocNo = objDb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = " & strIPPDocIdx & "")
                strVenIdx = objDb.GetVal("select im_s_coy_id from invoice_mstr where im_invoice_index = " & strIPPDocIdx & "")

                For i = 0 To aryDocDetails.Count - 1
                    If (aryDocDetails(i)(2) = "" Or aryDocDetails(i)(2) = "MYR") And aryDocDetails(i)(3) = "" Then
                        'IPP Gst Stage 2A - CH - 12 Feb 2015
                        strSQL = "UPDATE INVOICE_DETAILS SET ID_DR_EXCHANGE_RATE = NULL , ID_DR_CURRENCY = NULL, id_gsT_value = '" & CDec(aryDocDetails(i)(4)) & "', id_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', id_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "' " &
                              "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"

                    Else
                        If paymentmethod = "TT" Then
                            'IPP Gst Stage 2A - CH - 12 Feb 2015
                            strSQL = "UPDATE INVOICE_DETAILS SET ID_DR_CURRENCY = '" & Common.parseNull(aryDocDetails(i)(2)) & "', id_gsT_value = '" & CDec(aryDocDetails(i)(4)) & "', id_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', id_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "' " &
                            "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                        Else
                            'IPP Gst Stage 2A - CH - 12 Feb 2015
                            strSQL = "UPDATE INVOICE_DETAILS SET ID_DR_EXCHANGE_RATE = '" & Common.parseNull(aryDocDetails(i)(3)) & "' , ID_DR_CURRENCY = '" & Common.parseNull(aryDocDetails(i)(2)) & "', id_gsT_value = '" & CDec(aryDocDetails(i)(4)) & "', id_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', id_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "'  " &
                            "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                        End If


                    End If

                    Common.Insert2Ary(strSqlAry, strSQL)
                Next
            End If

            If aryDocDetails IsNot Nothing Then
                For i = 0 To aryDocDetails.Count - 1
                    strDocNo = objDb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = " & strIPPDocIdx & "")
                    strVenIdx = objDb.GetVal("select im_s_coy_id from invoice_mstr where im_invoice_index = " & strIPPDocIdx & "")
                    'IPP Gst Stage 2A - CH - 12 Feb 2015
                    strSQL = "UPDATE INVOICE_DETAILS SET id_gsT_value = '" & CDec(aryDocDetails(i)(4)) & "', id_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', id_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "'" &
                        "WHERE ID_INVOICE_NO = '" & strDocNo & "' AND ID_S_COY_ID = '" & strVenIdx & "' AND ID_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                    Common.Insert2Ary(strSqlAry, strSQL)
                Next
            End If

            If role = "2" Or role = "3" Or role = "5" Then

                Dim currentfaaction As Integer
                Dim currentseq As String
                currentfaaction = objDb.GetVal("SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & strIPPDocIdx & "' ")
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_ao = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
                If currentseq = "" Then
                    currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
                End If
                If currentseq = "" Then
                    currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO_2 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
                End If
                If currentseq = "" Then
                    currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO_3 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
                End If
                If currentseq = "" Then
                    currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO_4 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
                End If
                '-----
                If currentseq = "" Then
                    HttpContext.Current.Session("Verified") = "1"
                    Return False
                End If
                '-----
                If blnRelief = True Then
                    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," &
                             "FA_AO ='" & userID & "'," &
                             "FA_ACTION_DATE = NOW()," &
                             "FA_ON_BEHALFOF ='" & userID & "'," &
                             "FA_ACTIVE_AO ='" & userID & "'," &
                             "FA_ACTION_DATE = NOW()" &
                             "WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & userID & "' OR fa_a_ao = '" & userID & "')"
                Else

                    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," &
                             "FA_ACTION_DATE = NOW()," &
                             "FA_ACTIVE_AO ='" & userID & "'," &
                             "FA_ACTION_DATE = NOW()" &
                             " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & userID & "' OR fa_a_ao = '" & userID & "' " &
                             "OR fa_a_ao_2 = '" & userID & "' OR fa_a_ao_3 = '" & userID & "' OR fa_a_ao_4 = '" & userID & "')"
                End If
                Common.Insert2Ary(strSqlAry, strSQL)
                'update finance approval sequence
                'Dim currentseq As String
                'currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_ao = '" & userID & "'")
                'If currentseq = "" Then
                '    currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO = '" & userID & "'")
                'End If
                strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION='" & currentseq & "' WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "'"
                Common.Insert2Ary(strSqlAry, strSQL)
                If role = "2" Then
                    strSQL = "UPDATE FINANCE_APPROVAL SET " &
                    "FA_ACTIVE_AO = NULL,FA_ACTION_DATE = NULL, " &
                    "FA_AO_REMARK=NULL " &
                    "WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_aga_type = 'FM '"
                    Common.Insert2Ary(strSqlAry, strSQL)
                End If

            End If

            'Zulham 02012018
            If role = "FinanceTeller" And currentStatus.ToString.Trim = "14" Then
                Dim seq As String = ""
                seq = objDb.GetVal("SELECT MAX(fa_seq) 'seq'
                                    FROM finance_approval
                                    WHERE fa_invoice_index = '" & strIPPDocIdx & "'
                                    AND fa_aga_type = 'ao'")
                strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_ACTION='" & seq & "' WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "'"
                Common.Insert2Ary(strSqlAry, strSQL)
                strSQL = "UPDATE FINANCE_APPROVAL SET FA_AO_REMARK='', FA_ACTION_DATE = NULL, FA_ACTIVE_AO = NULL WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' AND FA_SEQ > '" & seq & "'"
                Common.Insert2Ary(strSqlAry, strSQL)
            End If

            If role = "2" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "Finance Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            ElseIf role = "3" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "Finance Approver", strSqlAry, "" & Common.Parse(strRemark) & "")
                If chkConvertToRentas = True Then
                    AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "Finance Approver", strSqlAry, "Convert IBG to Rentas")
                End If
            ElseIf role = "5" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "IPP Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            End If

            'If objDb.BatchExecuteForDup(strSqlAry) Then 'Dec 5, 2013
            Dim pTranNo As String = ""
            If objDb.BatchExecuteForIPP(strSqlAry, True, pTranNo, role) Then 'Dec 5, 2013
                'send mail to next approving officer
                'If IsLastFM2(strIPPDocIdx) = False Then
                '    sendMailToNextAO(strIPPDocIdx, role, intApprGrpIndex, currentseq)
                'Else
                'generate RENTAS or TT file         
                'If paymentmethod = "RENTAS" Then
                '    generateRENTAS(strIPPDocIdx, strTime, paymenttype)
                'ElseIf paymentmethod = "TT" Then
                '    generateTT(strIPPDocIdx, strTime, paymenttype)
                'End If
                'send mail to notify IPP officer
                'sendMailToIPPOfficer(strIPPDocIdx, role, "approve", intApprGrpIndex)
                'End If
                'Zulham 23102018 - PAMB
                If role = "3" Then
                    If Not pTranNo = "Generated" Then
                        If paymentmethod = "RENTAS" Then
                            'generateRENTAS(strIPPDocIdx, strTime, paymenttype)
                        ElseIf paymentmethod = "TT" Then
                            'generateTT(strIPPDocIdx, strTime, paymenttype)
                        End If
                    Else
                        Return False
                    End If
                End If

                Return True
            Else
                Return False
            End If


        End Function

        Public Function RejectIPPDoc(ByVal userID As String, ByVal companyID As String, ByVal strIPPDocIdx As String, ByVal strRemark As String, ByRef blnRelief As Boolean, ByVal role As String, Optional ByVal strRouteTo As String = "") As Boolean

            Dim strSQL, strSqlAry(0) As String
            Dim currentfaaction As Integer
            Dim currentseq As String
            Dim strDocOwner As String
            Dim strFinanceVerifier As String
            Dim strFinanceTeller As String
            Dim blnAO As Boolean
            'Zulham 07032019
            Dim rejSeq As String = ""

            currentfaaction = objDb.GetVal("SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & strIPPDocIdx & "' ")
            currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and fa_ao = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            blnAO = objDb.Exist("SELECT * FROM finance_approval WHERE fa_invoice_index = '" & strIPPDocIdx & "' AND fa_aga_type = 'AO'")

            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO_2 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO_3 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If
            If currentseq = "" Then
                currentseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & strIPPDocIdx & "' and FA_A_AO_4 = '" & userID & "' and fa_seq = '" & currentfaaction + 1 & "'")
            End If

            strDocOwner = objDb.GetVal("SELECT im_created_by FROM invoice_mstr WHERE im_invoice_index = '" & strIPPDocIdx & "' ")

            If role = "5" Then 'AO
                strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," &
                        "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                        "IM_STATUS_CHANGED_ON = NOW() " &
                        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
                ', IM_ROUTE_TO = '" & strDocOwner & "'
                Common.Insert2Ary(strSqlAry, strSQL)
            ElseIf role = "3" Then 'FM
                strFinanceVerifier = objDb.GetVal("SELECT fa_active_ao FROM finance_approval WHERE fa_aga_type = 'FO' AND fa_invoice_index = '" & strIPPDocIdx & "' ")
                'Zulham 07032019
                rejSeq = objDb.GetVal("SELECT fa_seq -1 'fa_seq' FROM finance_approval WHERE fa_aga_type = 'FO' AND (fa_ao = '" & strFinanceVerifier & "' or fa_a_ao = '" & strFinanceVerifier & "' ) ")

                strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," &
                        "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                        "IM_STATUS_CHANGED_ON = NOW(), IM_ROUTE_TO = '" & strFinanceVerifier & "' " &
                        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"

                Common.Insert2Ary(strSqlAry, strSQL)
            ElseIf role = "2" Then 'FO
                If strRouteTo = "O" Then
                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," &
                             "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                             "IM_STATUS_CHANGED_ON = NOW(),im_prcs_sent =NULL, " &
                             "im_prcs_recv =NULL, IM_ROUTE_TO = NULL " &
                             "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
                    'im_prcs_sent_id = NULL,im_prcs_sent_upd_date = NULL
                    ',im_prcs_recv_id = NULL,im_prcs_recv_upd_date = NULL
                    ', IM_ROUTE_TO = '" & strDocOwner & "' 
                Else
                    strFinanceTeller = objDb.GetVal("SELECT im_prcs_recv_id FROM invoice_mstr WHERE im_invoice_index = '" & strIPPDocIdx & "' ")

                    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," &
                        "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                        "IM_STATUS_CHANGED_ON = NOW(), IM_ROUTE_TO = '" & strFinanceTeller & "', " &
                         "im_prcs_recv =NULL " &
                        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
                End If

                ',im_prcs_recv_id = NULL,im_prcs_recv_upd_date = NULL
                Common.Insert2Ary(strSqlAry, strSQL)

            ElseIf role = "FinanceTeller" Then
                strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," &
                        "IM_STATUS_CHANGED_BY ='" & userID & "'," &
                        "IM_STATUS_CHANGED_ON = NOW(),im_prcs_sent =NULL,im_prcs_recv =NULL, IM_ROUTE_TO = NULL " &
                        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"

                Common.Insert2Ary(strSqlAry, strSQL)

            End If

            'If role = "FinanceTeller" Then
            '    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = Null," & _
            '    "FA_ACTION_DATE = Null," & _
            '    "FA_ACTIVE_AO = Null," & _
            '    "FA_AO_ACTION = 0," & _
            '    "FA_ACTION_DATE = Null" & _
            '    " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & ""
            'End If

            If role = "5" Then
                strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," &
                           "FA_ACTION_DATE = NOW()," &
                           "FA_ACTIVE_AO ='" & userID & "' " &
                             " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & userID & "' or fa_a_ao = '" & userID & "' " &
                           "OR fa_a_ao_2 = '" & userID & "' OR fa_a_ao_3 = '" & userID & "' OR fa_a_ao_4 = '" & userID & "')"
            End If
            If role = "2" Then
                strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," &
                           "FA_ACTION_DATE = NOW()," &
                           "FA_ACTIVE_AO ='" & userID & "' " &
                             " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & userID & "' or fa_a_ao = '" & userID & "' " &
                           "OR fa_a_ao_2 = '" & userID & "' OR fa_a_ao_3 = '" & userID & "' OR fa_a_ao_4 = '" & userID & "')"
            End If
            If role = "3" Then
                strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," &
                           "FA_ACTION_DATE = NOW()," &
                           "FA_ACTIVE_AO ='" & userID & "'," &
                           "FA_AO_ACTION = '" & currentseq & "' " &
                             " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & userID & "' or fa_a_ao = '" & userID & "' " &
                           "OR fa_a_ao_2 = '" & userID & "' OR fa_a_ao_3 = '" & userID & "' OR fa_a_ao_4 = '" & userID & "')"
            End If

            Common.Insert2Ary(strSqlAry, strSQL)

            'clear rejected info 
            If role = "2" Then
                strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = NULL," &
                           "FA_ACTION_DATE = NULL," &
                           "FA_ACTIVE_AO = NULL " &
                           "WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_aga_type = 'FM'"
            End If

            Common.Insert2Ary(strSqlAry, strSQL)

            If role = "3" Then
                If blnAO = True Then 'set the fa_ao_action = 0 for Finance Approver reject if the document is from source dept
                    'Zulham 07032019
                    If rejSeq.Trim = "" Then
                        strSQL = " UPDATE FINANCE_APPROVAL SET " &
                                    "FA_AO_ACTION = 1" &
                                      " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " "
                    Else
                        strSQL = " UPDATE FINANCE_APPROVAL SET " &
                                    "FA_AO_ACTION = '" & rejSeq & "'" &
                                      " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " "
                    End If
                Else 'set the fa_ao_action = 0 for Finance Approver reject if the document is from Finance
                    strSQL = " UPDATE FINANCE_APPROVAL SET " &
                                    "FA_AO_ACTION = 0" &
                                      " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " "
                End If


                Common.Insert2Ary(strSqlAry, strSQL)
            End If

            If role = "2" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "Finance Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            ElseIf role = "3" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "Finance Approver", strSqlAry, "" & Common.Parse(strRemark) & "")
            ElseIf role = "5" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "IPP Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            ElseIf role = "FinanceTeller" Then
                AddAuditTrailRecordInsert2Ary(userID, companyID, Common.parseNull(strIPPDocIdx), "Finance Teller", strSqlAry, "" & Common.Parse(strRemark) & "")
            End If

            If role = "FinanceTeller" Then
                If objDb.BatchExecute(strSqlAry) Then
                    'get the file's owner id using docIndexNum
                    Dim initiator = objDb.GetVal("Select im_created_by from invoice_mstr where im_invoice_index = " & strIPPDocIdx)

                    'strSQL = "update invoice_mstr set im_route_to = '" & initiator & "', im_remarks3 = '" & strRemark & "' where IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
                    strSQL = "update invoice_mstr set  im_remarks3 = '" & Common.Parse(strRemark) & "' where IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
                    objDb.Execute(strSQL)
                    Dim i As New IPP
                    i.sendMailToIPPTeller(HttpContext.Current.Session("PSDIPPDoc"), strIPPDocIdx)
                    Return True
                End If

            ElseIf objDb.BatchExecute(strSqlAry) Then
                sendMailToIPPOfficer(userID, companyID, strIPPDocIdx, role, "reject")
                Return True
            Else
                Return False
            End If

        End Function
        Public Function CheckRelief(Optional ByVal intApprGrpIdx As String = "") As Boolean
            Dim blnRelief As String
            Dim strSQL As String
            'Dim intApprGrpIdx As String

            'intApprGrpIdx = objDb.GetVal("SELECT CDM_IPP_APPROVAL_GRP_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & Common.Parse(userID) & "' AND um_coy_id = '" & Common.Parse(companyID) & "')")

            strSQL = "SELECT aga_relief_ind FROM approval_grp_ipp " &
                     "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' and aga_type = 'AO' order by aga_seq"

            blnRelief = objDb.GetVal(strSQL)

            If blnRelief = "O" Then
                blnRelief = True
                Return True
            Else
                blnRelief = False
                Return False
            End If


        End Function
        Public Function getApprovalList(ByVal userID As String, ByVal companyID As String, ByVal docno As String, ByVal doctype As String, ByVal startdt As String, ByVal enddt As String, ByVal vendor As String) As DataSet
            Dim sql As String
            Dim ds As New DataSet
            Dim deptCode = objDb.GetVal("select um_dept_id from user_mstr where um_user_id='" & userID & "' and um_coy_id = '" & Common.Parse(companyID) & "'")

            'Zulham 30102018
            sql = "SELECT * FROM invoice_mstr LEFT JOIN finance_approval ON im_invoice_index = fa_invoice_index " &
            "WHERE im_b_coy_id='" & companyID & "' AND im_invoice_status IN ('16') AND " &
            "(fa_ao = '" & userID & "' or fa_a_ao = '" & userID & "' ) AND fa_ao_action = (fa_seq - 1)"
            ' sql = "SELECT * FROM invoice_mstr LEFT JOIN finance_approval ON im_invoice_index = fa_invoice_index " &
            ' "LEFT JOIN user_mstr ON um_user_id = im_created_by AND um_coy_id = '" & companyID & "' " &
            '"WHERE im_b_coy_id='" & companyID & "' AND im_invoice_status IN ('16') AND " &
            '"(fa_ao = '" & userID & "' or fa_a_ao = '" & userID & "' ) AND fa_ao_action = (fa_seq - 1) " &
            '"AND um_dept_id = ( SELECT um_dept_id FROM user_mstr WHERE um_user_id = '" & userID & "' and um_coy_id = '" & companyID & "')"

            If docno <> "" Then
                sql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                sql &= " AND IM_INVOICE_TYPE = '" & doctype & "'"
            End If
            If startdt <> "" And enddt <> "" Then
                sql &= " AND IM_SUBMIT_DATE >= " & Common.ConvertDate(startdt) & " AND IM_SUBMIT_DATE <= " & Common.ConvertDate(enddt)
            End If
            If vendor <> "" Then
                sql &= " AND IM_S_COY_NAME LIKE '%" & vendor & "%'"
            End If
            ds = objDb.FillDs(sql)
            Return ds
        End Function
        Public Function getApprIPPDetail(ByVal userID As String, ByVal companyID As String, ByVal docno As String, ByVal index As String, ByVal compid As String) As DataSet
            Dim sql As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strDefIPPCompID = companyID
            End If

            'sql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC ,ipp_company.ic_bank_code, ipp_company.ic_bank_acct,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME = '','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC = '','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM INVOICE_MSTR IM,ipp_company,INVOICE_DETAILS ID " & _
            '"LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE " & _
            '"WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID AND cbg_b_coy_id ='" & Common.Parse(companyID) & "'" & _
            '"AND ic_index = im_s_coy_id AND ic_coy_id = '" & Common.Parse(companyID) & "'" & _
            '"AND IM_INVOICE_NO='" & docno & "' AND ID_INVOICE_NO='" & docno & "' AND IM_B_COY_ID='" & compid & "' " & _
            '"AND IM_INVOICE_INDEX = '" & index & "'"
            sql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC ,ipp_company.ic_bank_code, ipp_company.ic_bank_acct,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME = '','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC = '','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM INVOICE_MSTR IM,ipp_company,INVOICE_DETAILS ID " &
            "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE " &
            "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID AND cbg_b_coy_id ='" & Common.Parse(strDefIPPCompID) & "'" &
            "AND ic_index = im_s_coy_id AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "'" &
            "AND IM_INVOICE_NO='" & docno & "' AND ID_INVOICE_NO='" & docno & "' AND IM_B_COY_ID='" & compid & "' " &
            "AND IM_INVOICE_INDEX = '" & index & "'"
            ds = objDb.FillDs(sql)
            Return ds
        End Function

        Public Function getSubDocDetail(ByVal index As String) As DataSet
            Dim sql As String
            Dim ds As New DataSet
            sql = "SELECT * FROM IPP_SUB_DOC " &
                "WHERE ISD_MSTR_DOC_INDEX = '" & index & "'"
            ds = objDb.FillDs(sql)
            Return ds
        End Function

        Public Function IsLastFM(ByVal index As String) As Boolean
            Dim lastfmseq, currentfmseq As String

            currentfmseq = objDb.GetVal("SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & index & "' ")
            lastfmseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX ='" & index & "' ORDER BY FA_SEQ DESC")
            If lastfmseq = currentfmseq + 1 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function IsLastFM2(ByVal index As String) As Boolean ' for checking b4 send email after update the fa_ao_action
            Dim lastfmseq, currentfmseq As String
            'Dim currentfaaction As Integer

            currentfmseq = objDb.GetVal("SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & index & "' ")
            lastfmseq = objDb.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX ='" & index & "' ORDER BY FA_SEQ DESC")
            If lastfmseq = currentfmseq Then
                Return True
            Else
                Return False
            End If

        End Function
        Public Function generateTT(ByVal userID As String, ByVal companyID As String, ByVal docindex As String, Optional ByVal strTime As String = "", Optional ByVal strPaymentType As String = "")
            Dim strSQL As String
            Dim dsdocdetail As New DataSet
            Dim dsdoc As New DataSet
            Dim dsapplicantdetail As New DataSet
            Dim dsbankdetail As New DataSet
            Dim dslinedetail As New DataSet
            Dim arySQL(0) As String
            Dim PANo As String = ""
            Dim PANoPrefix As String = ""
            Dim PANewNo As String = ""
            Dim todayDate As String
            Dim TimeNow As String = DateTime.Now.ToString("HH:mm")
            Dim strDateTime As String
            Dim strWaiveBankCharges, BankCharge As String
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            Dim strCoyId As String
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If
            '--------------------------------------------

            todayDate = Date.Today
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            strSQL = "SELECT * FROM INVOICE_MSTR LEFT JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX " &
                        "LEFT JOIN BANK_CODE ON IC_BANK_CODE = BC_BANK_CODE " &
                        "LEFT JOIN CODE_MSTR ON IC_COUNTRY = CODE_ABBR " &
                        "WHERE IM_INVOICE_STATUS = 4 AND IM_INVOICE_TYPE IS NOT NULL AND " &
                        "IM_B_COY_ID ='" & companyID & "' " &
                        "AND IC_COY_ID='" & strCoyId & "' " &
                        "AND IM_PAYMENT_TERM = 'TT' AND IM_INVOICE_INDEX = '" & docindex & "' " &
                        "AND CODE_CATEGORY='CT'"
            dsdocdetail = objDb.FillDs(strSQL)

            strSQL = "SELECT * FROM COMPANY_MSTR WHERE CM_COY_ID='" & companyID & "'"
            dsapplicantdetail = objDb.FillDs(strSQL)
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            strSQL = "SELECT * FROM IPP_COMPANY WHERE IC_COY_ID='" & strCoyId & "' AND IC_INDEX = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND ic_bank_country = '' "
            If objDb.Exist(strSQL) Then
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                strSQL = "SELECT *,'' AS CODE_DESC FROM IPP_COMPANY " &
                           "LEFT JOIN BANK_CODE ON IC_BANK_CODE = BC_BANK_CODE " &
                           "WHERE IC_INDEX = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' " &
                           "AND IC_COY_ID='" & strCoyId & "'"
                dsbankdetail = objDb.FillDs(strSQL)
            Else
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                strSQL = "SELECT * FROM IPP_COMPANY LEFT JOIN CODE_MSTR ON IC_BANK_COUNTRY = CODE_ABBR " &
                                           "LEFT JOIN BANK_CODE ON IC_BANK_CODE = BC_BANK_CODE " &
                                           "WHERE IC_INDEX = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' " &
                                           "AND IC_COY_ID='" & strCoyId & "' AND CODE_CATEGORY='CT'"
                dsbankdetail = objDb.FillDs(strSQL)

            End If


            strSQL = "SELECT * FROM invoice_details WHERE id_invoice_no = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_NO") & "' AND id_s_coy_id='" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "'"
            dslinedetail = objDb.FillDs(strSQL)

            strWaiveBankCharges = objDb.GetVal("SELECT IFNULL(ic_waive_charges,'') AS ic_waive_charges FROM ipp_company WHERE ic_Coy_type = 'V' and ic_index = '" & dsdocdetail.Tables(0).Rows(0)("im_s_coy_id") & "' ")
            If strWaiveBankCharges = "N" And dsdocdetail.Tables(0).Rows(0)("im_invoice_total") <= 10000 Then
                'BankCharge = objDb.Get1ColumnCheckNull("IPP_PARAMETER", "IP_PARAM_VALUE", " WHERE IP_PARAM='TT1_CHARGE' AND IP_COY_ID='hlb'")
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                BankCharge = objDb.Get1ColumnCheckNull("IPP_PARAMETER", "IP_PARAM_VALUE", " WHERE IP_PARAM='TT1_CHARGE' AND IP_COY_ID='" & companyID & "'")
            ElseIf strWaiveBankCharges = "N" And dsdocdetail.Tables(0).Rows(0)("im_invoice_total") > 10000 Then
                'BankCharge = objDb.Get1ColumnCheckNull("IPP_PARAMETER", "IP_PARAM_VALUE", " WHERE IP_PARAM='TT2_CHARGE' AND IP_COY_ID='hlb'")
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                BankCharge = objDb.Get1ColumnCheckNull("IPP_PARAMETER", "IP_PARAM_VALUE", " WHERE IP_PARAM='TT2_CHARGE' AND IP_COY_ID='" & companyID & "'")
            Else
                BankCharge = 0
            End If

            If CDate(TimeNow) > CDate(strTime) Then

                strDateTime = DateTime.Today.AddDays(+1).ToString("yyyy-MM-dd")
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' AND im_payment_no IS NOT NULL") Then
                    strSQL = "SELECT IM_PAYMENT_NO FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' and im_payment_no is not null group by IM_PAYMENT_NO"
                    dsdoc = objDb.FillDs(strSQL)
                    PANo = dsdoc.Tables(0).Rows(0).Item("IM_PAYMENT_NO")
                Else
                    objGlobal.GetLatestDocNo(companyID, "Payment", arySQL, PANo, PANoPrefix, , 1, PANewNo)
                    'PANo = PANo.Substring(0, 2) & Today.Year.ToString.Substring(2, 2) & PANo.Substring(2, 6)
                    'Issue 8317 - CH - 1 Apr 2015
                    PANo = PANoPrefix & Today.Year.ToString.Substring(2, 2) & PANewNo
                End If
            Else
                strDateTime = DateTime.Today.ToString("yyyy-MM-dd")
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' AND im_payment_no IS NOT NULL") Then
                    strSQL = "SELECT IM_PAYMENT_NO FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' and im_payment_no is not null group by IM_PAYMENT_NO"
                    dsdoc = objDb.FillDs(strSQL)
                    PANo = dsdoc.Tables(0).Rows(0).Item("IM_PAYMENT_NO")
                Else
                    objGlobal.GetLatestDocNo(companyID, "Payment", arySQL, PANo, PANoPrefix, , 1, PANewNo)
                    'PANo = PANo.Substring(0, 2) & Today.Year.ToString.Substring(2, 2) & PANo.Substring(2, 6)
                    'Issue 8317 - CH - 1 Apr 2015
                    PANo = PANoPrefix & Today.Year.ToString.Substring(2, 2) & PANewNo
                End If
            End If


            'update value and date to company param
            strSQL = "UPDATE invoice_mstr SET im_payment_no = '" & PANo & "' WHERE im_invoice_index = '" & docindex & "' "
            Common.Insert2Ary(arySQL, strSQL)
            'strSQL = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & PANo & "' WHERE CP_PARAM_TYPE='Payment' AND CP_PARAM_NAME='Last Update Date' AND CP_COY_ID = 'HLB'"
            'Common.Insert2Ary(arySQL, strSQL)


            If arySQL(0) <> Nothing Then
                objDb.BatchExecute(arySQL)
            End If

            'add the default to FIN
            'FIN = "fin" & FIN & "klm"
            'Dim objex As New AppExcel

            'If strPaymentType <> "Credit Note" Then
            '    objex.WritecellTT(dsdocdetail, dslinedetail, dsapplicantdetail, dsbankdetail, PANo, BankCharge)
            'End If


        End Function
        Public Function generateRENTAS(ByVal userID As String, ByVal companyID As String, ByVal docindex As String, Optional ByVal strTime As String = "", Optional ByVal strPaymentType As String = "")
            Dim strSQL, bankCharge As String
            Dim dsdocdetail As New DataSet
            Dim dsdoc As New DataSet
            Dim dslinedetail As New DataSet
            Dim arySQL(0) As String
            Dim PANo As String = ""
            Dim PANoPrefix As String = ""
            Dim PANewNo As String = ""
            Dim todayDate As String
            Dim TimeNow As String = DateTime.Now.ToString("HH:mm")
            Dim strDateTime As String
            Dim strWaiveBankCharges As String
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            Dim strCoyId As String
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            If strDefIPPCompID = "" Then
                strCoyId = companyID
            Else
                strCoyId = strDefIPPCompID
            End If

            todayDate = Date.Today
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            strSQL = "SELECT * FROM INVOICE_MSTR LEFT JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX " &
            "LEFT JOIN BANK_CODE ON IC_BANK_CODE = BC_BANK_CODE " &
            "WHERE IM_INVOICE_STATUS = 4 AND IM_INVOICE_TYPE IS NOT NULL AND " &
            "IM_B_COY_ID ='" & companyID & "' " &
            "AND IC_COY_ID='" & strCoyId & "' " &
            "AND IM_PAYMENT_TERM = 'RENTAS' AND IM_INVOICE_INDEX = '" & docindex & "'"
            dsdocdetail = objDb.FillDs(strSQL)
            strSQL = "SELECT * FROM invoice_details LEFT JOIN COMPANY_B_GL_CODE ON ID_B_GL_CODE = CBG_B_GL_CODE WHERE id_invoice_no = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_NO") & "' AND id_s_coy_id='" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "'"
            dslinedetail = objDb.FillDs(strSQL)

            strWaiveBankCharges = objDb.GetVal("SELECT IFNULL(ic_waive_charges,'') AS ic_waive_charges FROM ipp_company WHERE ic_Coy_type = 'V' and ic_index = '" & dsdocdetail.Tables(0).Rows(0)("im_s_coy_id") & "' ")
            If strWaiveBankCharges = "N" Then
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                bankCharge = objDb.Get1ColumnCheckNull("IPP_PARAMETER", "IP_PARAM_VALUE", " WHERE IP_PARAM='RENTAS_CHARGE' AND IP_COY_ID='" & companyID & "'")
                'bankCharge = objDb.Get1ColumnCheckNull("IPP_PARAMETER", "IP_PARAM_VALUE", " WHERE IP_PARAM='RENTAS_CHARGE' AND IP_COY_ID='hlb'")
            Else
                bankCharge = 0
            End If

            'FIN = objDb.Get1ColumnCheckNull("COMPANY_PARAM", "CP_PARAM_VALUE", " WHERE CP_PARAM_TYPE='FIN' AND CP_PARAM_NAME='Last Used No' AND CP_COY_ID = 'HLB'")

            strSQL = "SELECT * FROM invoice_details WHERE id_invoice_no = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_INVOICE_NO") & "' AND id_s_coy_id='" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "'"
            dslinedetail = objDb.FillDs(strSQL)

            'If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE im_payment_date = CURRENT_DATE AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = 'hlb' AND im_payment_no IS NOT NULL") Then
            '    strSQL = "SELECT IM_PAYMENT_NO FROM invoice_mstr WHERE im_payment_date = CURRENT_DATE AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = 'hlb' and im_payment_no is not null group by IM_PAYMENT_NO"
            '    dsdoc = objDb.FillDs(strSQL)
            '    PANo = dsdoc.Tables(0).Rows(0).Item("IM_PAYMENT_NO")
            'Else
            '    objGlobal.GetLatestDocNo("Payment", arySQL, PANo, PANoPrefix, , 1)
            '    PANo = PANo.Substring(0, 2) & Today.Year.ToString.Substring(2, 2) & PANo.Substring(2, 6)
            'End If
            If CDate(TimeNow) > CDate(strTime) Then

                strDateTime = DateTime.Today.AddDays(+1).ToString("yyyy-MM-dd")
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' AND im_payment_no IS NOT NULL") Then
                    strSQL = "SELECT IM_PAYMENT_NO FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' and im_payment_no is not null group by IM_PAYMENT_NO"
                    dsdoc = objDb.FillDs(strSQL)
                    PANo = dsdoc.Tables(0).Rows(0).Item("IM_PAYMENT_NO")
                Else
                    'CH - Issue 8317 - 20 Mar 2015
                    objGlobal.GetLatestDocNo(companyID, "Payment", arySQL, PANo, PANoPrefix, , 1, PANewNo)
                    'PANo = PANo.Substring(0, 2) & Today.Year.ToString.Substring(2, 2) & PANo.Substring(2, 6)
                    PANo = PANoPrefix & Today.Year.ToString.Substring(2, 2) & PANewNo
                End If
            Else
                strDateTime = DateTime.Today.ToString("yyyy-MM-dd")
                'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
                If objDb.Exist("SELECT '*' FROM invoice_mstr WHERE CAST(im_payment_date as DATE)= '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' AND im_payment_no IS NOT NULL") Then
                    strSQL = "SELECT IM_PAYMENT_NO FROM invoice_mstr WHERE CAST(im_payment_date as DATE) = '" & strDateTime & "' AND im_invoice_status = 4 AND im_s_coy_id = '" & dsdocdetail.Tables(0).Rows(0).Item("IM_S_COY_ID") & "' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & companyID & "' and im_payment_no is not null group by IM_PAYMENT_NO"
                    dsdoc = objDb.FillDs(strSQL)
                    PANo = dsdoc.Tables(0).Rows(0).Item("IM_PAYMENT_NO")
                Else
                    'CH - Issue 8317 - 20 Mar 2015
                    objGlobal.GetLatestDocNo(companyID, "Payment", arySQL, PANo, PANoPrefix, , 1, PANewNo)
                    'PANo = PANo.Substring(0, 2) & Today.Year.ToString.Substring(2, 2) & PANo.Substring(2, 6)
                    PANo = PANoPrefix & Today.Year.ToString.Substring(2, 2) & PANewNo
                End If
            End If

            'update value and date to company param
            strSQL = "UPDATE invoice_mstr SET im_payment_no = '" & PANo & "' WHERE im_invoice_index = '" & docindex & "' "
            Common.Insert2Ary(arySQL, strSQL)
            'strSQL = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & PANo & "' WHERE CP_PARAM_TYPE='Payment' AND CP_PARAM_NAME='Last Update Date' AND CP_COY_ID = 'HLB'"
            'Common.Insert2Ary(arySQL, strSQL)

            If arySQL(0) <> Nothing Then
                objDb.BatchExecute(arySQL)
            End If


            'add the default to FIN
            'FIN = "fin" & FIN & "klm"        
            'Dim objex As New AppExcel
            'If strPaymentType <> "Credit Note" Then
            '    objex.WritecellRENTAS(dsdocdetail, dslinedetail, PANo, bankCharge)
            'End If


            'If FIN = "0000" Then
            '    FIN = "0001"
            '    'update value and date to company param
            '    strSQL = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & FIN & "' WHERE CP_PARAM_TYPE='FIN' AND CP_PARAM_NAME='Last Used No' AND CP_COY_ID = 'HLB'"
            '    Common.Insert2Ary(arySQL, strSQL)
            '    strSQL = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & todayDate & "' WHERE CP_PARAM_TYPE='FIN' AND CP_PARAM_NAME='Last Update Date' AND CP_COY_ID = 'HLB'"
            '    Common.Insert2Ary(arySQL, strSQL)
            'Else
            '    Dim lastUpdateDate As String
            '    lastUpdateDate = objDb.Get1ColumnCheckNull("COMPANY_PARAM", "CP_PARAM_VALUE", " WHERE CP_PARAM_TYPE='FIN' AND CP_PARAM_NAME='Last Update Date' AND CP_COY_ID = 'HLB'")

            '    If lastUpdateDate <> todayDate Then
            '        If FIN = "9999" Then
            '            FIN = "1"
            '        Else
            '            FIN = CInt(FIN) + 1
            '        End If
            '        FIN = addLeadingZero(FIN, "3")
            '        'update value and date to company param
            '        strSQL = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & FIN & "' WHERE CP_PARAM_TYPE='FIN' AND CP_PARAM_NAME='Last Used No' AND CP_COY_ID = 'HLB'"
            '        Common.Insert2Ary(arySQL, strSQL)
            '        strSQL = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE='" & todayDate & "' WHERE CP_PARAM_TYPE='FIN' AND CP_PARAM_NAME='Last Update Date' AND CP_COY_ID = 'HLB'"
            '        Common.Insert2Ary(arySQL, strSQL)
            '    End If
            'End If

        End Function
        Public Function addLeadingZero(ByVal value As String, ByVal place As String) As String
            Dim leadingZero As String = ""
            Dim i As Integer
            Dim totalplace As Integer
            Dim valuelength As String
            valuelength = value.Length
            totalplace = CInt(place) - CInt(valuelength)
            For i = 0 To totalplace
                leadingZero &= "0"
            Next
            Return leadingZero & value.ToString
        End Function
        Public Function checkDept(ByVal userID As String, ByVal companyID As String) As Boolean
            Dim sql As String
            sql = "SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & userID & "' AND um_coy_id='" & companyID & "'"
            If objDb.Get1ColumnCheckNull("user_mstr", "um_dept_id", " WHERE um_user_id='" & userID & "' AND um_coy_id='" & companyID & "'") = "" Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function getIPPStatus(ByVal userID As String, ByVal companyID As String, ByRef strDocNo As String) As Integer
            Dim strsql As String
            Dim ds As New DataSet
            Dim intStatus As Integer

            strsql = "SELECT im_invoice_status FROM invoice_mstr WHERE  im_invoice_index = '" & strDocNo & "' AND im_b_coy_id = '" & companyID & "'"
            intStatus = CInt(objDb.GetVal(strsql))

            Return intStatus
        End Function

        Public Function GetSUMCostAllocDetail(ByVal intCostAlloc As Integer) As Decimal
            Dim dblTtlPct As Decimal
            Dim strSql As String
            Dim dsDoc As New DataSet


            strSql = "SELECT IFNULL(SUM(cad_percent),0) Total_Percentage FROM cost_alloc_detail " &
                     "WHERE cad_cam_index =  '" & intCostAlloc & "'"

            dblTtlPct = CDec(Common.parseNull(objDb.GetVal(strSql)))
            Return dblTtlPct

        End Function
        Public Function CheckCostAllocDetail(ByVal intCostAlloc As Integer) As DataSet

            Dim strSql As String
            Dim dsDoc As New DataSet


            strSql = "SELECT * FROM cost_alloc_detail " &
                     "WHERE cad_cam_index =  '" & intCostAlloc & "'"


            dsDoc = objDb.FillDs(strSql)
            CheckCostAllocDetail = dsDoc

        End Function
        Public Function CheckBankCode(ByVal userID As String, ByVal companyID As String, ByRef InvIdx As String) As Boolean
            Dim strsql, strCoyId As String
            'Modified for IPP GST Stage 2A - CH - 11 Feb 2015
            strCoyId = ConfigurationManager.AppSettings("DefIPPCompID")
            If strCoyId = "" Then
                strCoyId = companyID
            End If

            'If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN ipp_company ON ic_index = im_s_coy_id " & _
            '"AND ic_coy_id = '" & Common.Parse(companyID) & "' " & _
            '"INNER JOIN bank_code ON bc_bank_code = ic_bank_code AND bc_coy_id = '" & Common.Parse(companyID) & "' " & _
            '"AND bc_status = 'A' WHERE im_invoice_index = '" & InvIdx & "'") > 0 Then
            '    Return False
            'End If
            If objDb.Exist("SELECT '*' FROM invoice_mstr INNER JOIN ipp_company ON ic_index = im_s_coy_id " &
            "AND ic_coy_id = '" & Common.Parse(strCoyId) & "' " &
            "INNER JOIN bank_code ON bc_bank_code = ic_bank_code AND bc_coy_id = '" & Common.Parse(strCoyId) & "' " &
            "AND bc_status = 'A' WHERE im_invoice_index = '" & InvIdx & "'") > 0 Then
                Return False
            End If

            Return True

        End Function
        'Zulham 21092018 - E2P UAT
        Public Function getIPPApprovalWorkflowList(ByVal userID As String, ByVal companyID As String, Optional ByVal type As String = "", Optional ByVal isResident As String = "", Optional ByVal invoiceAmount As Decimal = 0.0, Optional exchangeRate As Decimal = 1) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            'Zulham 12072018 - PAMB
            'Check for user's role. if it's finance teller, skip ao validator
            'role: E2P Teller(F)
            Dim objUsers As New Users
            Dim boolFinTeller = objUsers.checkUserFixedRole(userID, companyID, "'IPP Officer(F)'")


            'Zulham 26062018 - PAMB
            If type = "" Then
                'Zulham 8317 23/02/2015
                strsql = "SELECT DISTINCT AGB_GRP_INDEX ,AGM_GRP_NAME "
                strsql &= "FROM APPROVAL_GRP_BUYER "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON  AGB_GRP_INDEX = AGM_GRP_INDEX AND AGM_TYPE='IPP' "
                strsql &= "INNER JOIN approval_grp_ipp ON aga_grp_index = AGB_GRP_INDEX "
                strsql &= "WHERE agb_buyer = '" & Common.Parse(userID) & "' "
                strsql &= "and agm_coy_id = '" & Common.Parse(companyID) & "' "
            ElseIf type = "E2P" Then
                If Not boolFinTeller Then
                    'Zulham 17072018 - PAMB
                    'Added conditions for invoiceAmount
                    'Zulham 21092018 - E2P UAT
                    If Not isResident = "" Then
                        strsql = "Select DISTINCT AGB_GRP_INDEX ,AGM_GRP_NAME "
                        strsql &= "From APPROVAL_GRP_BUYER "
                        strsql &= "INNER Join APPROVAL_GRP_MSTR ON  AGB_GRP_INDEX = AGM_GRP_INDEX And AGM_TYPE='" & type & "' "
                        strsql &= "Left OUTER JOIN approval_grp_ao ON AGA_GRP_INDEX = AGB_GRP_INDEX  "
                        strsql &= "Join approval_grp_FO fo1 ON fo1.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Join approval_grp_FO fo2 ON fo2.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Left OUTER JOIN user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGA_AO = um1.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGA_A_AO = um2.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um3 ON um3.UM_COY_ID = AGM_COY_ID And fo1.AGFO_FO = um3.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um4 ON um4.UM_COY_ID = AGM_COY_ID And fo1.AGFO_A_FO = um4.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um5 ON um5.UM_COY_ID = AGM_COY_ID And fo2.AGFO_FO = um5.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um6 ON um6.UM_COY_ID = AGM_COY_ID And fo2.AGFO_A_FO = um6.UM_USER_ID "
                        strsql &= "WHERE agb_buyer = '" & Common.Parse(userID) & "' "
                        strsql &= "And agm_coy_id = '" & Common.Parse(companyID) & "' "
                        strsql &= "And agm_resident = '" & Common.Parse(isResident) & "' "
                        strsql &= "And fo1.agfo_seq = 1 "
                        strsql &= " And fo2.agfo_seq > 1 "
                        strsql &= " And (UM1.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= "Or UM2.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                        strsql &= "And (UM3.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= "Or UM4.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                        strsql &= "And (UM5.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= " Or UM6.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                    Else
                        strsql = "Select DISTINCT AGB_GRP_INDEX ,AGM_GRP_NAME "
                        strsql &= "From APPROVAL_GRP_BUYER "
                        strsql &= "INNER Join APPROVAL_GRP_MSTR ON  AGB_GRP_INDEX = AGM_GRP_INDEX And AGM_TYPE='" & type & "' "
                        strsql &= "Left OUTER JOIN approval_grp_ao ON AGA_GRP_INDEX = AGB_GRP_INDEX  "
                        strsql &= "Join approval_grp_FO fo1 ON fo1.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Join approval_grp_FO fo2 ON fo2.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Left OUTER JOIN user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGA_AO = um1.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGA_A_AO = um2.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um3 ON um3.UM_COY_ID = AGM_COY_ID And fo1.AGFO_FO = um3.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um4 ON um4.UM_COY_ID = AGM_COY_ID And fo1.AGFO_A_FO = um4.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um5 ON um5.UM_COY_ID = AGM_COY_ID And fo2.AGFO_FO = um5.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um6 ON um6.UM_COY_ID = AGM_COY_ID And fo2.AGFO_A_FO = um6.UM_USER_ID "
                        strsql &= "WHERE agb_buyer = '" & Common.Parse(userID) & "' "
                        strsql &= "And agm_coy_id = '" & Common.Parse(companyID) & "' "
                        strsql &= "And fo1.agfo_seq = 1 "
                        strsql &= " And fo2.agfo_seq > 1 "
                        strsql &= "And (UM1.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= "Or UM2.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                        strsql &= "And (UM3.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= "Or UM4.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                        strsql &= "And (UM5.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= " Or UM6.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                    End If
                Else
                    'Zulham 17072018 - PAMB
                    'Added conditions for invoiceAmount
                    'Zulham 21092018 - E2P UAT
                    If Not isResident = "" Then
                        strsql = "Select DISTINCT AGB_GRP_INDEX ,AGM_GRP_NAME "
                        strsql &= "From APPROVAL_GRP_BUYER "
                        strsql &= "INNER Join APPROVAL_GRP_MSTR ON  AGB_GRP_INDEX = AGM_GRP_INDEX And AGM_TYPE='" & type & "' "
                        strsql &= "Join approval_grp_FO fo1 ON fo1.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Join approval_grp_FO fo2 ON fo2.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Left OUTER JOIN user_mstr um3 ON um3.UM_COY_ID = AGM_COY_ID And fo1.AGFO_FO = um3.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um4 ON um4.UM_COY_ID = AGM_COY_ID And fo1.AGFO_A_FO = um4.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um5 ON um5.UM_COY_ID = AGM_COY_ID And fo2.AGFO_FO = um5.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um6 ON um6.UM_COY_ID = AGM_COY_ID And fo2.AGFO_A_FO = um6.UM_USER_ID "
                        strsql &= "WHERE agb_buyer = '" & Common.Parse(userID) & "' "
                        strsql &= "And agm_coy_id = '" & Common.Parse(companyID) & "' "
                        strsql &= "And agm_resident = '" & Common.Parse(isResident) & "' "
                        strsql &= "And fo1.agfo_seq = 1 "
                        strsql &= "And fo2.agfo_seq > 1 "
                        strsql &= "And (UM3.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= "Or UM4.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                        strsql &= "And (UM5.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= " Or UM6.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                    Else
                        strsql = "Select DISTINCT AGB_GRP_INDEX ,AGM_GRP_NAME "
                        strsql &= "From APPROVAL_GRP_BUYER "
                        strsql &= "INNER Join APPROVAL_GRP_MSTR ON  AGB_GRP_INDEX = AGM_GRP_INDEX And AGM_TYPE='" & type & "' "
                        strsql &= "Join approval_grp_FO fo1 ON fo1.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Join approval_grp_FO fo2 ON fo2.AGFO_GRP_INDEX = AGB_GRP_INDEX "
                        strsql &= "Left OUTER JOIN user_mstr um3 ON um3.UM_COY_ID = AGM_COY_ID And fo1.AGFO_FO = um3.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um4 ON um4.UM_COY_ID = AGM_COY_ID And fo1.AGFO_A_FO = um4.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um5 ON um5.UM_COY_ID = AGM_COY_ID And fo2.AGFO_FO = um5.UM_USER_ID "
                        strsql &= "Left OUTER JOIN user_mstr um6 ON um6.UM_COY_ID = AGM_COY_ID And fo2.AGFO_A_FO = um6.UM_USER_ID "
                        strsql &= "WHERE agb_buyer = '" & Common.Parse(userID) & "' "
                        strsql &= "And agm_coy_id = '" & Common.Parse(companyID) & "' "
                        strsql &= "And fo1.agfo_seq = 1 "
                        strsql &= "And fo2.agfo_seq > 1 "
                        strsql &= "And (UM3.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= "Or UM4.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                        strsql &= "And (UM5.um_invoice_app_limit > " & invoiceAmount * exchangeRate & " "
                        strsql &= " Or UM6.um_invoice_app_limit > " & invoiceAmount * exchangeRate & ") "
                    End If
                End If
            End If

            ds = objDb.FillDs(strsql)
            getIPPApprovalWorkflowList = ds
        End Function
        Public Function getIPPApprovalWorkflow(ByVal userID As String, ByVal companyID As String, ByVal intWorkflowIndex As Integer, Optional ByVal appType As String = "", Optional ByVal invoiceTotal As Decimal = 0.0) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'Zulham 05072018 - PAMB
            'Added an argument for appType
            If appType = "" Then
                strsql = "SET @rownum = 0; " &
                       "SELECT @rownum:=@rownum+1 AS AGA_SEQ,AGA_AO, AGA_A_AO,AO_NAME, AAO_NAME, AAO_NAME2, AAO_NAME3, AAO_NAME4,AGA_TYPE FROM (  " &
                       "SELECT  AGA_SEQ,AGA_AO,IFNULL(AGA_A_AO, '') AS AGA_A_AO,B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME," &
                       "D.UM_USER_NAME AS AAO_NAME2, E.UM_USER_NAME AS AAO_NAME3, F.UM_USER_NAME AS AAO_NAME4, " &
                       "AGA_TYPE  " &
                       "FROM approval_grp_ipp  " &
                       "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & companyID & "' " &
                       "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & companyID & "' " &
                       "LEFT OUTER JOIN USER_MSTR D ON AGA_A_AO_2 = D.UM_USER_ID AND D.UM_COY_ID = '" & companyID & "' " &
                       "LEFT OUTER JOIN USER_MSTR E ON AGA_A_AO_3 = E.UM_USER_ID AND E.UM_COY_ID = '" & companyID & "' " &
                       "LEFT OUTER JOIN USER_MSTR F ON AGA_A_AO_4 = F.UM_USER_ID AND F.UM_COY_ID = '" & companyID & "' " &
                       "WHERE AGA_GRP_INDEX = '" & intWorkflowIndex & "' AND AGA_TYPE = 'AO'  " &
                            "UNION ALL " &
                       "SELECT AGA_SEQ,AGA_AO,IFNULL(AGA_A_AO, '') AS AGA_A_AO,B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, " &
                       "D.UM_USER_NAME AS AAO_NAME2, E.UM_USER_NAME AS AAO_NAME3, F.UM_USER_NAME AS AAO_NAME4, " &
                       "AGA_TYPE  " &
                       "FROM approval_grp_ipp " &
                       "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & companyID & "' " &
                       "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & companyID & "' " &
                       "LEFT OUTER JOIN USER_MSTR D ON AGA_A_AO_2 = D.UM_USER_ID AND D.UM_COY_ID = '" & companyID & "' " &
                       "LEFT OUTER JOIN USER_MSTR E ON AGA_A_AO_3 = E.UM_USER_ID AND E.UM_COY_ID = '" & companyID & "' " &
                       "LEFT OUTER JOIN USER_MSTR F ON AGA_A_AO_4 = F.UM_USER_ID AND F.UM_COY_ID = '" & companyID & "' " &
                       "WHERE AGA_GRP_INDEX = '" & intWorkflowIndex & "' AND AGA_TYPE <> 'AO') ZZZ "
            ElseIf appType = "E2P" Then
                'Seq: AO,TAX Verifier, FO, FM
                strsql = "SET @rownum = 0; " &
                        "Select @rownum:=@rownum+1 AS AGA_SEQ,AGA_AO, AGA_A_AO,AO_NAME, AAO_NAME, AAO_NAME2, AAO_NAME3, AAO_NAME4,AGA_TYPE FROM ( " &
                        "(SELECT DISTINCT 'AO' AS 'AGA_TYPE', AGA_AO 'AGA_AO', AGA_A_AO AS 'AGA_A_AO',NULL AS 'AGA_A_AO_2',NULL AS 'AGA_A_AO_3',NULL AS 'AGA_A_AO_4',  " &
                        "AGA_GRP_INDEX AS 'AGA_GRP_INDEX', AGA_RELIEF_IND AS 'AGA_RELIEF_IND' " &
                        ", um1.um_user_name AS 'AO_NAME', um2.um_user_name 'AAO_NAME', '' AS 'AAO_NAME2', '' AS 'AAO_NAME3', '' AS 'AAO_NAME4' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_ao On AGA_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGA_AO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGA_A_AO = um2.UM_USER_ID  " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX   " &
                        "And AGM_TYPE='" & appType & "'  " &
                        "And AGM_GRP_INDEX = '" & intWorkflowIndex & "'  " &
                        "And AGB_BUYER = '" & Common.Parse(userID) & "'  " &
                        "And AGM_COY_ID = '" & companyID & "'  " &
                        "And um1.um_invoice_app_limit <  " & invoiceTotal & "   " &
                        "ORDER BY AGA_SEQ)  " &
                            "UNION ALL  " &
                        "(SELECT DISTINCT 'AO' AS 'AGA_TYPE', AGA_AO 'AGA_AO', AGA_A_AO AS 'AGA_A_AO',NULL AS 'AGA_A_AO_2',NULL AS 'AGA_A_AO_3',NULL AS 'AGA_A_AO_4',  " &
                        "AGA_GRP_INDEX AS 'AGA_GRP_INDEX', AGA_RELIEF_IND AS 'AGA_RELIEF_IND',  " &
                        "um1.um_user_name AS 'AO_NAME', um2.um_user_name 'AAO_NAME', '' AS 'AAO_NAME2', '' AS 'AAO_NAME3', '' AS 'AAO_NAME4' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_ao On AGA_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGA_AO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGA_A_AO = um2.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX   " &
                        "And AGM_TYPE='" & appType & "'  " &
                        "And AGM_GRP_INDEX = '" & intWorkflowIndex & "'  " &
                        "And AGB_BUYER = '" & Common.Parse(userID) & "'  " &
                        "And AGM_COY_ID = '" & companyID & "'  " &
                        "And um1.um_invoice_app_limit >=  " & invoiceTotal & "   " &
                        "ORDER BY AGA_SEQ LIMIT 1)  " &
                            "UNION ALL  " &
                        "(SELECT DISTINCT 'FO' AS 'AGA_TYPE', AGFO_FO 'AGA_AO', AGFO_A_FO AS 'AGA_A_AO',AGFO_A_FO_2 AS 'AGA_A_AO_2',AGFO_A_FO_3 AS 'AGA_A_AO_3',AGFO_A_FO_4 AS 'AGA_A_AO_4',  " &
                        "AGFO_GRP_INDEX AS 'AGA_GRP_INDEX', AGFO_RELIEF_IND AS 'AGA_RELIEF_IND',  " &
                        "um1.um_user_name AS 'AO_NAME', um2.um_user_name 'AAO_NAME', um3.um_user_name AS 'AAO_NAME2', um4.um_user_name AS 'AAO_NAME3', um5.um_user_name AS 'AAO_NAME4'  " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FO On AGFO_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFO_FO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX   " &
                        "And AGM_TYPE='" & appType & "'   " &
                        "And AGM_GRP_INDEX = '" & intWorkflowIndex & "'  " &
                        "And AGB_BUYER = '" & Common.Parse(userID) & "'  " &
                        "And AGM_COY_ID = '" & companyID & "'  " &
                        "And AGFO_SEQ = '1'  " &
                        "ORDER BY AGFO_SEQ  " &
                        "LIMIT 1)  " &
                            "UNION ALL  " &
                        "(SELECT DISTINCT 'FO' AS 'AGA_TYPE', AGFO_FO 'AGA_AO', AGFO_A_FO AS 'AGA_A_AO',AGFO_A_FO_2 AS 'AGA_A_AO_2',AGFO_A_FO_3 AS 'AGA_A_AO_3',AGFO_A_FO_4 AS 'AGA_A_AO_4',  " &
                        "AGFO_GRP_INDEX AS 'AGA_GRP_INDEX', AGFO_RELIEF_IND AS 'AGA_RELIEF_IND',  " &
                        "um1.um_user_name AS 'AO_NAME', um2.um_user_name 'AAO_NAME', um3.um_user_name AS 'AAO_NAME2', um4.um_user_name AS 'AAO_NAME3', um5.um_user_name AS 'AAO_NAME4' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FO On AGFO_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFO_FO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX   " &
                        "And AGM_TYPE='" & appType & "'   " &
                        "And AGM_GRP_INDEX = '" & intWorkflowIndex & "'  " &
                        "And AGB_BUYER = '" & Common.Parse(userID) & "'  " &
                        "And AGM_COY_ID = '" & companyID & "'  " &
                        "And AGFO_SEQ <> '1'  " &
                        "And um1.um_invoice_app_limit <  " & invoiceTotal & "   " &
                        "ORDER BY AGFO_SEQ)  " &
                            "UNION ALL  " &
                        "(SELECT DISTINCT 'FO' AS 'AGA_TYPE', AGFO_FO 'AGA_AO', AGFO_A_FO AS 'AGA_A_AO',AGFO_A_FO_2 AS 'AGA_A_AO_2',AGFO_A_FO_3 AS 'AGA_A_AO_3',AGFO_A_FO_4 AS 'AGA_A_AO_4',  " &
                        "AGFO_GRP_INDEX AS 'AGA_GRP_INDEX', AGFO_RELIEF_IND AS 'AGA_RELIEF_IND',  " &
                        "um1.um_user_name AS 'AO_NAME', um2.um_user_name 'AAO_NAME', um3.um_user_name AS 'AAO_NAME2', um4.um_user_name AS 'AAO_NAME3', um5.um_user_name AS 'AAO_NAME4' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FO On AGFO_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFO_FO = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFO_A_FO_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX   " &
                        "And AGM_TYPE='" & appType & "'   " &
                        "And AGM_GRP_INDEX = '" & intWorkflowIndex & "'  " &
                        "And AGB_BUYER = '" & Common.Parse(userID) & "'  " &
                        "And AGM_COY_ID = '" & companyID & "'  " &
                        "And AGFO_SEQ <> '1'  " &
                        "And um1.um_invoice_app_limit >=  " & invoiceTotal & "   " &
                        "ORDER BY AGFO_SEQ LIMIT 1)  " &
                            "UNION ALL  " &
                        "(SELECT DISTINCT 'FM' AS 'AGA_TYPE', AGFM_FM 'AGA_AO', AGFM_A_FM AS 'AGA_A_AO',AGFM_A_FM_2 AS 'AGA_A_AO_2',AGFM_A_FM_3 AS 'AGA_A_AO_3',AGFM_A_FM_4 AS 'AGA_A_AO_4',   " &
                        "AGFM_GRP_INDEX AS 'AGA_GRP_INDEX', AGFM_RELIEF_IND AS 'AGA_RELIEF_IND',  " &
                        "um1.um_user_name AS 'AO_NAME', um2.um_user_name 'AAO_NAME', um3.um_user_name AS 'AAO_NAME2', um4.um_user_name AS 'AAO_NAME3', um5.um_user_name AS 'AAO_NAME4' " &
                        "From approval_grp_mstr " &
                        "Join approval_grp_buyer On AGM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join approval_grp_FM On AGFM_GRP_INDEX = AGB_GRP_INDEX  " &
                        "Join user_mstr um1 ON um1.UM_COY_ID = AGM_COY_ID And AGFM_FM = um1.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um2 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM = um2.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um3 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM_2 = um3.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um4 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM_3 = um4.UM_USER_ID " &
                        "Left OUTER JOIN user_mstr um5 ON um2.UM_COY_ID = AGM_COY_ID And AGFM_A_FM_4 = um5.UM_USER_ID " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX   " &
                        "And AGM_TYPE='" & appType & "'   " &
                        "And AGM_GRP_INDEX = '" & intWorkflowIndex & "'  " &
                        "And AGB_BUYER = '" & Common.Parse(userID) & "'  " &
                        "And AGM_COY_ID = '" & companyID & "'  " &
                        "ORDER BY AGFM_SEQ) " &
                        ")ZZZ "
            End If

            ds = objDb.FillDs(strsql)
            getIPPApprovalWorkflow = ds
        End Function
        Public Function SaveMultiGLDebits(ByVal userID As String, ByVal companyID As String, ByVal dsItemDetails As DataSet, ByVal dsHeader As DataSet, ByVal strDocNo As String, ByVal strOldDocNo As String,
        ByVal strVendor As String, Optional ByRef strAryQuery() As String = Nothing, Optional ByVal CategoryIndex As Integer = 0, Optional ByVal isGST As String = "") As Boolean
            Dim i, j, intCostAllocIndex As Integer
            Dim strPayFor, strSQL As String
            Dim InvLineAmt, Amount, ttlAmount As Double
            Dim dsDocLine As DataSet
            Dim strInvNo As String
            Dim strCC, strHOBR As String
            'Zulham 12022019
            Dim strVendorIdx As String = 0

            'Jules 2018.07.20 - PAMB allow "\" and "#"
            strDocNo = Replace(Replace(strDocNo, "\", "\\"), "#", "\#")
            strOldDocNo = Replace(Replace(strOldDocNo, "\", "\\"), "#", "\#")
            'End modification.

            'Zulham 12022019
            If Not dsHeader Is Nothing Then
                If strDocNo <> strOldDocNo Then
                    strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strOldDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
                Else
                    strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo")) & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
                End If
            End If


            'strSQL = "DELETE FROM invoice_details WHERE id_invoice_no = '" & strOldDocNo & "' and id_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "' "
            'Common.Insert2Ary(strAryQuery, strSQL)

            For i = 0 To dsItemDetails.Tables(0).Rows.Count - 1
                strPayFor = Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("PayFor"))
                'strCC = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostCenter"))) & "' and cc_coy_id = '" & Common.Parse(companyID) & "'")
                strCC = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostCenter"))) & "' and cc_coy_id = '" & Common.Parse(strPayFor) & "'")
                strHOBR = objDb.GetVal("SELECT cbm_branch_name FROM company_branch_mstr WHERE cbm_branch_code = '" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("HOBR"))) & "' and cbm_coy_id = '" & strPayFor & "'")

                'Zulham 12022019
                If dsHeader Is Nothing Then
                    strVendorIdx = objDb.GetVal("SELECT im_s_coy_id FROM invoice_mstr WHERE im_s_coy_name = '" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("VendorName")) & "' and im_invoice_no = '" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("InvoiceNo")) & "' ")
                End If


                If isGST = "" Then
                    'Jules 2018.07.19 - PAMB - Added Gift, Category, Analysis Codes, Withholding Option and Withholding Tax.
                    strSQL = " INSERT INTO INVOICE_DETAILS(" &
                    "ID_INVOICE_NO, ID_S_COY_ID, ID_INVOICE_LINE, ID_PRODUCT_DESC, ID_UOM, ID_RECEIVED_QTY, " &
                    "ID_UNIT_COST, ID_B_GL_CODE, ID_PAY_FOR, ID_REF_NO, ID_COST_CENTER, ID_COST_CENTER_DESC, " &
                    "ID_BRANCH_CODE, ID_BRANCH_CODE_NAME, ID_COST_ALLOC_CODE, ID_ASSET_GROUP, ID_ASSET_GROUP_DESC, " &
                    "ID_ASSET_SUB_GROUP, ID_ASSET_SUB_GROUP_DESC, id_glrule_category, id_glrule_category_index, id_gst_reimb, " &
                    "id_gift, ID_CATEGORY, ID_ANALYSIS_CODE1, ID_ANALYSIS_CODE2, ID_ANALYSIS_CODE3, ID_ANALYSIS_CODE4, ID_ANALYSIS_CODE5, ID_ANALYSIS_CODE8, ID_ANALYSIS_CODE9, " &
                    "ID_WITHHOLDING_OPT,ID_WITHHOLDING_TAX,ID_WITHHOLDING_REMARKS) " &
                    "VALUES('" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo")) & "'," &
                    "'" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("VendorID")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ItemLine")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ItemDesc")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("UOM")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Quantity")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("UnitCost")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("GLCode")) & "'," &
                    "'" & strPayFor & "'," &
                    "''," &
                    "'" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostCenter"))).Trim & "'," &
                    "'" & Common.Parse(Common.parseNull(strCC)) & "'," &
                    "'" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("HOBR"))) & "'," &
                    "'" & Common.Parse(Common.parseNull(strHOBR)) & "'," &
                    "'" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostAlloc"))) & "'," &
                    "'','','','','" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("RulesCategory"))) & "'," & CategoryIndex & ",'N/A'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Gift")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Category")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("FundType")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ProductType")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Channel")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ReinsuranceCo")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("AssetCode")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ProjectCode")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("PersonCode")) & "'," &
                    "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) & "'," &
                    IIf(Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) <> "" AndAlso Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) <> "3", Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTTax")), "NULL") & "," &
                    IIf(Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) <> "" AndAlso Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) = "3", "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTTax")) & "'", "") & "')"
                Else
                    'Elicit infos about GST
                    Dim GSTInputTax As String = ""
                    Dim GSTOutputTax As String = ""
                    Dim GSTReimbursement As String = ""
                    Dim GSTTaxRate As String = ""
                    Dim gstPercentage As String = ""
                    Dim GSTValue = 0.0
                    Dim gst As New GST
                    If Not dsItemDetails.Tables(0).Rows(i).Item("InputTax") Is DBNull.Value Then
                        If dsItemDetails.Tables(0).Rows(i).Item("InputTax").ToString.Contains("(") Then
                            GSTInputTax = dsItemDetails.Tables(0).Rows(i).Item("InputTax").ToString.Split("(")(0)
                            'gstPercentage = dsItemDetails.Tables(0).Rows(i).Item("InputTax").ToString.Split("(")(1).Substring(0, 1)
                            gst.getGSTInfobyRate_ForIPP(Common.Parse(GSTInputTax), gstPercentage, GSTTaxRate)
                        Else
                            GSTInputTax = dsItemDetails.Tables(0).Rows(i).Item("InputTax").ToString.Trim
                            gstPercentage = 0
                        End If
                    End If
                    If Not dsItemDetails.Tables(0).Rows(i).Item("OutputTax") Is DBNull.Value Then
                        If dsItemDetails.Tables(0).Rows(i).Item("OutputTax").ToString.Contains("(") Then
                            GSTOutputTax = dsItemDetails.Tables(0).Rows(i).Item("OutputTax").ToString.Split("(")(0)
                        Else
                            GSTOutputTax = dsItemDetails.Tables(0).Rows(i).Item("OutputTax").ToString.Trim
                        End If
                    End If
                    If Not dsItemDetails.Tables(0).Rows(i).Item("Reimbursement") Is DBNull.Value Then
                        If Not dsItemDetails.Tables(0).Rows(i).Item("Reimbursement").ToString.Trim = "N/A" Then
                            GSTReimbursement = dsItemDetails.Tables(0).Rows(i).Item("Reimbursement").ToString.Substring(0, 1).ToUpper
                        Else
                            GSTReimbursement = "N/A"
                        End If
                    Else
                        GSTReimbursement = "N/A"
                    End If
                    If Not dsItemDetails.Tables(0).Rows(i).Item("GSTAmount") Is DBNull.Value Then
                        GSTValue = CType(dsItemDetails.Tables(0).Rows(i).Item("GSTAmount"), Double)
                    End If

                    'End

                    'Zulham 12022019
                    Dim strInvoiceNo As String = ""

                    If dsHeader IsNot Nothing Then
                        strInvoiceNo = Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo"))
                        strVendorIdx = Common.Parse(dsHeader.Tables(0).Rows(0).Item("VendorID"))
                    Else
                        strInvoiceNo = Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("InvoiceNo"))
                    End If

                    'Jules 2018.07.19 - PAMB - Added Gift, Category, Analysis Codes, Withholding Option and Withholding Tax.
                    'Zulham 12022019
                    strSQL = " INSERT INTO INVOICE_DETAILS(" &
                             "ID_INVOICE_NO, ID_S_COY_ID, ID_INVOICE_LINE, ID_PRODUCT_DESC, ID_UOM, ID_RECEIVED_QTY, " &
                             "ID_UNIT_COST, ID_B_GL_CODE, ID_PAY_FOR, ID_REF_NO, ID_COST_CENTER, ID_COST_CENTER_DESC, " &
                             "ID_BRANCH_CODE, ID_BRANCH_CODE_NAME, ID_COST_ALLOC_CODE, ID_ASSET_GROUP, ID_ASSET_GROUP_DESC, " &
                             "ID_ASSET_SUB_GROUP, ID_ASSET_SUB_GROUP_DESC, id_glrule_category, id_glrule_category_index, id_gst_reimb, id_gst_value, id_gst_input_tax_code, id_gst_output_tax_code, id_gst ,id_gst_rate," &
                             "id_gift, ID_CATEGORY, ID_ANALYSIS_CODE1, ID_ANALYSIS_CODE2, ID_ANALYSIS_CODE3, ID_ANALYSIS_CODE4, ID_ANALYSIS_CODE5, ID_ANALYSIS_CODE8, ID_ANALYSIS_CODE9, " &
                             "ID_WITHHOLDING_OPT,ID_WITHHOLDING_TAX,ID_WITHHOLDING_REMARKS) " &
                             "VALUES('" & strInvoiceNo & "'," &
                             "'" & strVendorIdx & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ItemLine")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ItemDesc")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("UOM")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Quantity")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("UnitCost")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("GLCode")) & "'," &
                             "'" & strPayFor & "'," &
                             "''," &
                             "'" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostCenter"))).Trim & "'," &
                             "'" & Common.Parse(Common.parseNull(strCC)) & "'," &
                             "'" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("HOBR"))) & "'," &
                             "'" & Common.Parse(Common.parseNull(strHOBR)) & "'," &
                             "'" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostAlloc"))) & "'," &
                             "'','','','','" & Common.Parse(Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("RulesCategory"))) & "'," & CategoryIndex & ",'" & GSTReimbursement & "'," & GSTValue & ",'" & GSTInputTax & "','" & GSTOutputTax & "'," & gstPercentage & ",'" & GSTTaxRate & "', " &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Gift")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Category")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("FundType")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ProductType")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Channel")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ReinsuranceCo")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("AssetCode")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("ProjectCode")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("PersonCode")) & "'," &
                             "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) & "'," &
                             IIf(Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) <> "" AndAlso Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) <> "3", Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTTax")), "NULL") & "," &
                             IIf(Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) <> "" AndAlso Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTOption")) = "3", "'" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("WHTTax")) & "'", "NULL") & ")"
                End If
                'objDb.Execute(strSQL)
                Common.Insert2Ary(strAryQuery, strSQL)
                'Common.Insert2Ary(boolAryQuery, "False")
                If Common.parseNull(dsItemDetails.Tables(0).Rows(i).Item("CostAlloc")) <> "" Then

                    'strSQL = " DELETE FROM invoice_details_alloc WHERE ida_invoice_INDEX = '" & strInvNo & "'"
                    'Common.Insert2Ary(strAryQuery, strSQL)

                    intCostAllocIndex = CInt(objDb.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("CostAlloc")) & "' AND cam_coy_id = '" & Common.Parse(companyID) & "'"))
                    dsDocLine = GetCostAllocDetail(userID, companyID, intCostAllocIndex)
                    'strInvNo = "SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("DocNo")) & "' and im_b_coy_id = '" & Common.Parse(companyID)
                    'strInvNo = (objDb.GetLatestInsertedID2("INVOICE_MSTR", "IM_INVOICE_INDEX")) + 1
                    For j = 0 To dsDocLine.Tables(0).Rows.Count - 1

                        InvLineAmt = CDbl(Common.Parse(dsItemDetails.Tables(0).Rows(i).Item("Quantity") * dsItemDetails.Tables(0).Rows(i).Item("UnitCost")))
                        Amount = Format(InvLineAmt * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00")
                        ttlAmount = ttlAmount + Amount

                        If j = dsDocLine.Tables(0).Rows.Count - 1 And ttlAmount <> InvLineAmt Then
                            Amount = Format(InvLineAmt * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00") _
                            + (InvLineAmt - ttlAmount)
                        End If

                        strSQL = " INSERT INTO invoice_details_alloc(IDA_INVOICE_INDEX, IDA_INVOICE_LINE, IDA_COST_CENTER, IDA_COST_CENTER_DESC, IDA_BRANCH_CODE, IDA_BRANCH_NAME, IDA_PERCENT, IDA_AMOUNT)" &
                                 "VALUES('" & strInvNo & "'," &
                                 "" & dsItemDetails.Tables(0).Rows(i).Item("ItemLine") & "," &
                                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_CC_CODE")) & "'," &
                                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CC_CC_DESC")) & "'," &
                                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_BRANCH_CODE")) & "'," &
                                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CBM_BRANCH_NAME")) & "'," &
                                 "" & dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") & "," &
                                 "" & Amount & ")"
                        Common.Insert2Ary(strAryQuery, strSQL)
                    Next
                End If
                ttlAmount = 0
                ' End If

            Next
            'If objDb.BatchExecute(strAryQuery) Then
            Return True
            'End If
        End Function

        Public Function DelIMultiGLDebits(ByVal userID As String, ByVal companyID As String, ByVal dsHeader As DataSet, ByVal strDocNo As String, ByVal strOldDocNo As String, ByVal strVendor As String, ByVal VendorIndex As String)
            Dim strsql, strInvNo As String
            Dim strAryQuery(0) As String

            'Jules 2018.07.20 - PAMB allow "\" and "#"
            strDocNo = Replace(Replace(strDocNo, "\", "\\"), "#", "\#")
            strOldDocNo = Replace(Replace(strOldDocNo, "\", "\\"), "#", "\#")
            'End modification.

            If strDocNo <> strOldDocNo Then
                strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & strOldDocNo & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
            Else
                strInvNo = objDb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(dsHeader.Tables(0).Rows(0).Item("DocNo")) & "' and im_b_coy_id = '" & Common.Parse(companyID) & "' and im_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "'")
            End If

            'strsql = "DELETE FROM invoice_details WHERE id_invoice_no = '" & strOldDocNo & "' and id_s_coy_id = '" & dsHeader.Tables(0).Rows(0).Item("VendorID") & "' "
            strsql = "DELETE FROM invoice_details WHERE id_invoice_no = '" & strOldDocNo & "' and id_s_coy_id = '" & VendorIndex & "' "
            Common.Insert2Ary(strAryQuery, strsql)
            'objDb.Execute(strsql)

            strsql = " DELETE FROM invoice_details_alloc WHERE ida_invoice_INDEX = '" & strInvNo & "'"
            Common.Insert2Ary(strAryQuery, strsql)
            'objDb.Execute(strsql)

            If objDb.BatchExecute(strAryQuery) Then
                Return True
            End If

        End Function

        Public Function AddAuditTrailRecordInsert2Ary(ByVal userID As String, ByVal companyID As String, ByVal InvIndex As Integer, ByVal role As String, ByRef pQuery() As String, Optional ByVal stateDesc As String = "") As Boolean

            Dim strSQL As String
            strSQL = "INSERT INTO ipp_trans_log (itl_invoice_index,itl_performed_by,itl_user_id,itl_trans_date,itl_remarks) " &
                     "VALUES(" & InvIndex & ",'" & role & "','" & Common.Parse(userID) & "',Now()" &
                     ",'" & stateDesc & "')"

            Common.Insert2Ary(pQuery, strSQL)
        End Function

        Public Function ChkGLRuleCode(ByVal userID As String, ByVal companyID As String, ByVal strIndex As String, ByVal strRuleCode As String, ByVal strMode As String, ByRef strMsg As String, Optional ByVal aryCategory As ArrayList = Nothing) As Boolean
            Dim strSql, strCoyId, strCategory As String
            Dim i As Integer
            strCoyId = companyID

            If strMode = "a" Then
                strSql = "SELECT '*' FROM IPP_GLRULE " &
                       "WHERE IG_COY_ID = '" & strCoyId & "' AND IG_GLRULE_CODE = '" & Common.Parse(strRuleCode) & "' "

                If objDb.Exist(strSql) > 0 Then
                    'strMsg = strRuleCode & " " & objGlobal.GetErrorMessage("00177")
                    strMsg = objGlobal.GetErrorMessage("00002")
                    Return False
                End If
            ElseIf strMode = "m" Then
                'Check record whether is tied to transaction or not
                If aryCategory.Count > 0 Then
                    For i = 0 To aryCategory.Count - 1
                        If aryCategory(i)(3) <> "" Then
                            strSql = "SELECT IGC_GLRULE_CATEGORY FROM IPP_GLRULE_CATEGORY " &
                                    "WHERE IGC_GLRULE_CATEGORY_INDEX = " & aryCategory(i)(3)
                            strCategory = objDb.GetVal(strSql)

                            If objDb.Exist(strSql) > 0 Then
                                If strCategory <> aryCategory(i)(0) Then
                                    strSql = "SELECT '*' FROM INVOICE_DETAILS " &
                                            "WHERE ID_GLRULE_CATEGORY_INDEX = " & aryCategory(i)(3)

                                    If objDb.Exist(strSql) > 0 Then
                                        strMsg = objGlobal.GetErrorMessage("00373")
                                        Return False
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If

                'Check Rule Code whether is exist or not
                strSql = "SELECT '*' FROM IPP_GLRULE WHERE IG_GLRULE_INDEX <> '" & strIndex & "' " &
                        "AND IG_COY_ID = '" & strCoyId & "' AND IG_GLRULE_CODE = '" & Common.Parse(strRuleCode) & "' "

                If objDb.Exist(strSql) > 0 Then
                    'strMsg = strRuleCode & " " & objGlobal.GetErrorMessage("00177")
                    strMsg = objGlobal.GetErrorMessage("00002")
                    Return False
                End If
            ElseIf strMode = "d" Then
                'Check record whether is tied to transaction or not
                strSql = "SELECT '*' FROM INVOICE_DETAILS " &
                        "WHERE ID_GLRULE_CATEGORY_INDEX IN " &
                        "(SELECT IGC_GLRULE_CATEGORY_INDEX FROM IPP_GLRULE_CATEGORY " &
                        "INNER JOIN IPP_GLRULE ON IGC_GLRULE_INDEX = IG_GLRULE_INDEX " &
                        "WHERE IG_GLRULE_INDEX = '" & strIndex & "')"

                If objDb.Exist(strSql) > 0 Then
                    strMsg = objGlobal.GetErrorMessage("00373")
                    Return False
                End If
            End If

            Return True
        End Function

        Public Function ChkRuleInfo(ByVal userID As String, ByVal companyID As String, ByVal strIndex As String, ByVal strInfo As String, ByVal strType As String, ByVal strMode As String, ByRef strReturnVal As String) As Boolean
            Dim strSql, strCoyId, strUserId As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            strCoyId = companyID

            If strType = "RC" Then
                If strMode = "new" Then
                    strSql = "SELECT IG_GLRULE_CODE FROM IPP_GLRULE_CATEGORY INNER JOIN IPP_GLRULE ON IGC_GLRULE_INDEX = IG_GLRULE_INDEX " &
                            "WHERE IGC_GLRULE_CATEGORY = '" & Common.Parse(strInfo) & "' AND IG_COY_ID = '" & strCoyId & "'"
                ElseIf strMode = "edit" Then
                    strSql = "SELECT IG_GLRULE_CODE FROM IPP_GLRULE_CATEGORY INNER JOIN IPP_GLRULE ON IGC_GLRULE_INDEX = IG_GLRULE_INDEX " &
                            "WHERE IGC_GLRULE_INDEX <> '" & strIndex & "' AND IGC_GLRULE_CATEGORY = '" & Common.Parse(strInfo) & "' AND IG_COY_ID = '" & strCoyId & "'"
                End If
            ElseIf strType = "GL" Then
                If strMode = "new" Then
                    strSql = "SELECT IG_GLRULE_CODE FROM IPP_GLRULE_GL INNER JOIN IPP_GLRULE ON IGG_GLRULE_INDEX = IG_GLRULE_INDEX " &
                            "WHERE IGG_GL_CODE = '" & Common.Parse(strInfo) & "' AND IG_COY_ID = '" & strCoyId & "'"
                ElseIf strMode = "edit" Then
                    strSql = "SELECT IG_GLRULE_CODE FROM IPP_GLRULE_GL INNER JOIN IPP_GLRULE ON IGG_GLRULE_INDEX = IG_GLRULE_INDEX " &
                            "WHERE IG_GLRULE_INDEX <> '" & strIndex & "' AND IGG_GL_CODE = '" & Common.Parse(strInfo) & "' AND IG_COY_ID = '" & strCoyId & "'"
                End If
            End If

            If objDb.Exist(strSql) > 0 Then
                ChkRuleInfo = False
                strReturnVal = objDb.GetVal(strSql)
            Else
                ChkRuleInfo = True
                strReturnVal = ""
            End If

        End Function

        Public Function SaveGLRuleCode(ByVal userID As String, ByVal companyID As String, ByVal strIndex As String, ByVal strRuleCode As String, ByVal strMode As String, ByVal aryRule As ArrayList, ByVal aryGL As ArrayList, ByRef strMsg As String, ByRef strNewIndex As String) As Boolean
            Dim strSql, strCoyId, strUserId, strLatestId As String
            Dim strAryQuery(0) As String
            Dim i As Integer
            strCoyId = companyID
            strUserId = userID

            If strMode = "new" Then
                strSql = "INSERT INTO IPP_GLRULE (IG_GLRULE_CODE, IG_COY_ID, IG_ENT_BY, IG_ENT_DATETIME) " &
                        "VALUES ('" & Common.Parse(strRuleCode) & "', '" & strCoyId & "', '" & strUserId & "' , NOW())"
                Common.Insert2Ary(strAryQuery, strSql)

                strLatestId = "(SELECT IG_GLRULE_INDEX FROM IPP_GLRULE WHERE IG_COY_ID = '" & strCoyId & "' AND IG_GLRULE_CODE = '" & Common.Parse(strRuleCode) & "' LIMIT 1)"

                For i = 0 To aryRule.Count - 1
                    If aryRule(i)(0) <> "" Then
                        strSql = "INSERT INTO IPP_GLRULE_CATEGORY (IGC_GLRULE_INDEX, IGC_COY_ID, IGC_GLRULE_CATEGORY, IGC_GLRULE_CATEGORY_REMARK, IGC_GLRULE_CATEGORY_ACTIVE) " &
                                "VALUES(" & strLatestId & ", '" & strCoyId & "', '" & Common.Parse(aryRule(i)(0)) & "', '" & Common.Parse(aryRule(i)(1)) & "', '" & aryRule(i)(2) & "')"
                        Common.Insert2Ary(strAryQuery, strSql)
                    End If
                Next

                For i = 0 To aryGL.Count - 1
                    If aryGL(i)(0) <> "" Then
                        strSql = "INSERT INTO IPP_GLRULE_GL (IGG_GLRULE_INDEX, IGG_GL_CODE) " &
                                "VALUES(" & strLatestId & ", '" & Common.Parse(aryGL(i)(0)) & "')"
                        Common.Insert2Ary(strAryQuery, strSql)
                    End If
                Next
            Else
                strSql = "UPDATE IPP_GLRULE SET IG_GLRULE_CODE = '" & Common.Parse(strRuleCode) & "', IG_MOD_BY = '" & strUserId & "', IG_MOD_DATETIME = NOW() " &
                        "WHERE IG_GLRULE_INDEX = '" & strIndex & "'"
                Common.Insert2Ary(strAryQuery, strSql)

                'strSql = "DELETE FROM IPP_GLRULE_CATEGORY " & _
                '        "WHERE IGC_GLRULE_INDEX = '" & strIndex & "'"
                'Common.Insert2Ary(strAryQuery, strSql)

                For i = 0 To aryRule.Count - 1
                    If aryRule(i)(3) <> "" Then
                        If aryRule(i)(0) <> "" Then
                            strSql = "UPDATE IPP_GLRULE_CATEGORY SET " &
                                    "IGC_GLRULE_CATEGORY = '" & Common.Parse(aryRule(i)(0)) & "', " &
                                    "IGC_GLRULE_CATEGORY_REMARK = '" & Common.Parse(aryRule(i)(1)) & "', " &
                                    "IGC_GLRULE_CATEGORY_ACTIVE = '" & aryRule(i)(2) & "' " &
                                    "WHERE IGC_GLRULE_CATEGORY_INDEX = '" & aryRule(i)(3) & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        Else
                            strSql = "DELETE FROM IPP_GLRULE_CATEGORY " &
                                    "WHERE IGC_GLRULE_CATEGORY_INDEX = '" & aryRule(i)(3) & "'"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    Else
                        If aryRule(i)(0) <> "" Then
                            strSql = "INSERT INTO IPP_GLRULE_CATEGORY (IGC_GLRULE_INDEX, IGC_COY_ID, IGC_GLRULE_CATEGORY, IGC_GLRULE_CATEGORY_REMARK, IGC_GLRULE_CATEGORY_ACTIVE) " &
                                    "VALUES('" & strIndex & "', '" & strCoyId & "', '" & Common.Parse(aryRule(i)(0)) & "', '" & Common.Parse(aryRule(i)(1)) & "', '" & aryRule(i)(2) & "')"
                            Common.Insert2Ary(strAryQuery, strSql)
                        End If
                    End If
                Next

                strSql = "DELETE FROM IPP_GLRULE_GL " &
                       "WHERE IGG_GLRULE_INDEX = '" & strIndex & "'"
                Common.Insert2Ary(strAryQuery, strSql)

                For i = 0 To aryGL.Count - 1
                    If aryGL(i)(0) <> "" Then
                        strSql = "INSERT INTO IPP_GLRULE_GL (IGG_GLRULE_INDEX, IGG_GL_CODE) " &
                                "VALUES('" & strIndex & "', '" & Common.Parse(aryGL(i)(0)) & "')"
                        Common.Insert2Ary(strAryQuery, strSql)
                    End If
                Next
            End If

            If objDb.BatchExecute(strAryQuery) Then
                strSql = "SELECT IG_GLRULE_INDEX FROM IPP_GLRULE WHERE IG_COY_ID = '" & strCoyId & "' AND IG_GLRULE_CODE = '" & Common.Parse(strRuleCode) & "' LIMIT 1"
                strNewIndex = objDb.GetVal(strSql)
                strMsg = objGlobal.GetErrorMessage("00003")
                Return True
            Else
                strMsg = objGlobal.GetErrorMessage("00007")
                Return False
            End If
        End Function

        Public Function DeleteGLRuleCode(ByVal userID As String, ByVal companyID As String, ByVal ds As DataSet) As String
            Dim strSql, strMsg As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ChkGLRuleCode(userID, companyID, ds.Tables(0).Rows(i)("RuleIndex"), "", "d", strMsg) Then
                    strSql = "DELETE FROM IPP_GLRULE WHERE IG_GLRULE_INDEX = '" & ds.Tables(0).Rows(i)("RuleIndex") & "'"
                    Common.Insert2Ary(strAryQuery, strSql)

                    strSql = "DELETE FROM IPP_GLRULE_CATEGORY WHERE IGC_GLRULE_INDEX = '" & ds.Tables(0).Rows(i)("RuleIndex") & "'"
                    Common.Insert2Ary(strAryQuery, strSql)

                    strSql = "DELETE FROM IPP_GLRULE_GL WHERE IGG_GLRULE_INDEX = '" & ds.Tables(0).Rows(i)("RuleIndex") & "'"
                    Common.Insert2Ary(strAryQuery, strSql)
                Else
                    Return strMsg
                End If
            Next

            If objDb.BatchExecute(strAryQuery) Then
                DeleteGLRuleCode = objGlobal.GetErrorMessage("00004")
            Else
                DeleteGLRuleCode = objGlobal.GetErrorMessage("00008")
            End If
        End Function

        Public Function GetGLRuleCodeInfo(ByVal userID As String, ByVal companyID As String, ByVal strIndex As String) As DataSet
            Dim strSql, strSqlM, strSqlGC, strSqlGL As String
            Dim ds As DataSet

            strSqlM = "SELECT IG_GLRULE_CODE FROM IPP_GLRULE WHERE IG_GLRULE_INDEX = '" & strIndex & "'"

            strSqlGC = "SELECT IGC_GLRULE_CATEGORY, IGC_GLRULE_CATEGORY_REMARK, IGC_GLRULE_CATEGORY_ACTIVE, IGC_GLRULE_CATEGORY_INDEX FROM IPP_GLRULE_CATEGORY WHERE IGC_GLRULE_INDEX = '" & strIndex & "'"

            strSqlGL = "SELECT IGG_GL_CODE, CBG_B_GL_DESC AS IGG_GL_DESC FROM IPP_GLRULE_GL " &
                    "INNER JOIN COMPANY_B_GL_CODE ON IGG_GL_CODE = CBG_B_GL_CODE AND CBG_B_COY_ID = '" & companyID & "' " &
                    "WHERE IGG_GLRULE_INDEX = '" & strIndex & "'"

            strSql = strSqlM & ";" & strSqlGC & ";" & strSqlGL
            ds = objDb.FillDs(strSql)
            ds.Tables(0).TableName = "MSTR"
            ds.Tables(1).TableName = "RULE"
            ds.Tables(2).TableName = "GL"
            Return ds
        End Function

        Public Function GetGLRuleCodeList(ByVal userID As String, ByVal companyID As String, ByVal strRuleCode As String) As DataSet
            Dim strSql, strCoyId As String
            Dim ds As DataSet
            'Modified for IPP GST Stage 2A - CH (30/1/2015)
            strCoyId = ConfigurationManager.AppSettings("DefIPPCompID")
            If strCoyId = "" Then
                strCoyId = companyID
            End If

            strSql = "SELECT IG_GLRULE_INDEX, IG_GLRULE_CODE, IGC_GLRULE_CATEGORY, IGC_GLRULE_CATEGORY_REMARK, " &
                    "IGG_GL_CODE, CBG_B_GL_DESC FROM " &
                    "(SELECT RC.IG_GLRULE_INDEX AS IG_GLRULE_INDEX, RC.IG_GLRULE_CODE, IGC_GLRULE_CATEGORY, " &
                    "IGC_GLRULE_CATEGORY_REMARK, IGG_GL_CODE " &
                    "FROM " &
                    "(SELECT IG_GLRULE_INDEX, IG_GLRULE_CODE, IGC_GLRULE_CATEGORY, IGC_GLRULE_CATEGORY_REMARK, " &
                    "(CASE IG_GLRULE_INDEX WHEN @ruleIndex THEN @rcRow := @rcRow + 1 ELSE @rcRow := 1 AND @ruleIndex := IG_GLRULE_INDEX END) AS RC_ROW " &
                    "FROM IPP_GLRULE " &
                    "INNER JOIN (SELECT @rcRow := 0, @ruleIndex := '') r " &
                    "INNER JOIN IPP_GLRULE_CATEGORY ON IG_GLRULE_INDEX = IGC_GLRULE_INDEX " &
                    "WHERE IGC_GLRULE_CATEGORY_ACTIVE = 'Y' AND IG_COY_ID = '" & strCoyId & "' ORDER BY IGC_GLRULE_INDEX) RC " &
                    "LEFT JOIN " &
                    "(SELECT IG_GLRULE_INDEX, IG_GLRULE_CODE, IGG_GL_CODE, " &
                    "(CASE IG_GLRULE_INDEX WHEN @glIndex THEN @glRow := @glRow + 1 ELSE @glRow := 1 AND @glIndex := IG_GLRULE_INDEX END) AS GL_ROW " &
                    "FROM IPP_GLRULE " &
                    "INNER JOIN (SELECT @glRow := 0, @glIndex := '') r " &
                    "INNER JOIN IPP_GLRULE_GL ON IG_GLRULE_INDEX = IGG_GLRULE_INDEX " &
                    "WHERE IG_COY_ID = '" & strCoyId & "') GL " &
                    "ON RC.IG_GLRULE_INDEX = GL.IG_GLRULE_INDEX AND RC_ROW = GL_ROW " &
                    "UNION " &
                    "SELECT GL.IG_GLRULE_INDEX AS IG_GLRULE_INDEX, GL.IG_GLRULE_CODE, IGC_GLRULE_CATEGORY, " &
                    "IGC_GLRULE_CATEGORY_REMARK, IGG_GL_CODE " &
                    "FROM " &
                    "(SELECT IG_GLRULE_INDEX, IG_GLRULE_CODE, IGC_GLRULE_CATEGORY, IGC_GLRULE_CATEGORY_REMARK, " &
                    "(CASE IG_GLRULE_INDEX WHEN @ruleIndex THEN @rcRow := @rcRow + 1 ELSE @rcRow := 1 AND @ruleIndex := IG_GLRULE_INDEX END) AS RC_ROW " &
                    "FROM IPP_GLRULE " &
                    "INNER JOIN (SELECT @rcRow := 0, @ruleIndex := '') r " &
                    "INNER JOIN IPP_GLRULE_CATEGORY ON IG_GLRULE_INDEX = IGC_GLRULE_INDEX " &
                    "WHERE IGC_GLRULE_CATEGORY_ACTIVE = 'Y' AND IG_COY_ID = '" & strCoyId & "' ORDER BY IGC_GLRULE_INDEX) RC " &
                    "RIGHT JOIN " &
                    "(SELECT IG_GLRULE_INDEX, IG_GLRULE_CODE, IGG_GL_CODE, " &
                    "(CASE IG_GLRULE_INDEX WHEN @glIndex THEN @glRow := @glRow + 1 ELSE @glRow := 1 AND @glIndex := IG_GLRULE_INDEX END) AS GL_ROW " &
                    "FROM IPP_GLRULE " &
                    "INNER JOIN (SELECT @glRow := 0, @glIndex := '') r " &
                    "INNER JOIN IPP_GLRULE_GL ON IG_GLRULE_INDEX = IGG_GLRULE_INDEX " &
                    "WHERE IG_COY_ID = '" & strCoyId & "') GL " &
                    "ON RC.IG_GLRULE_INDEX = GL.IG_GLRULE_INDEX AND RC_ROW = GL_ROW) tb " &
                    "LEFT JOIN COMPANY_B_GL_CODE ON CBG_B_COY_ID = '" & strCoyId & "' AND CBG_B_GL_CODE = IGG_GL_CODE "

            If strRuleCode <> "" Then
                strSql &= "WHERE IG_GLRULE_CODE LIKE '%" & Common.Parse(strRuleCode) & "%' "
            End If

            strSql &= "ORDER BY IG_GLRULE_CODE, IG_GLRULE_INDEX "
            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function GetBranchCostCentreList(ByVal userID As String, ByVal companyID As String, ByVal strCoyId As String, ByVal strBranchCode As String) As DataSet
            Dim strSql, strTemp As String
            Dim ds As DataSet

            strSql = "SELECT BCC_COY_ID, BCC_BRANCH_CODE, CBM_BRANCH_NAME FROM BRANCH_COST_CENTRE " &
                    "INNER JOIN COMPANY_BRANCH_MSTR ON CBM_BRANCH_CODE = BCC_BRANCH_CODE AND CBM_COY_ID = BCC_COY_ID "

            If strCoyId = "" Then
                strSql &= "WHERE (BCC_COY_ID = '" & companyID & "' OR BCC_COY_ID = '" & companyID & "') "
            Else
                strSql &= "WHERE BCC_COY_ID = '" & strCoyId & "' "
            End If

            If strBranchCode <> "" Then
                strTemp = Common.BuildWildCard(strBranchCode)
                strSql &= " AND BCC_BRANCH_CODE" & Common.ParseSQL(strTemp)
            End If

            strSql &= " GROUP BY BCC_COY_ID, BCC_BRANCH_CODE " &
                    "ORDER BY BCC_COY_ID, BCC_BRANCH_CODE "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function GetCCListByBranch(ByVal strCoyId As String, ByVal strBranchCode As String) As DataSet
            Dim strSql, strTemp As String
            Dim ds As DataSet

            strSql = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " &
                    "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " &
                    "WHERE BCC_COY_ID = '" & strCoyId & "' AND BCC_BRANCH_CODE = '" & Common.Parse(strBranchCode) & "' " &
                    "ORDER BY CC_CC_CODE "
            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function GetSubDoc(ByVal mstrDocIdx As Int64) As DataSet
            Dim strSql, strTemp As String
            Dim ds As DataSet

            strSql = "SELECT * from IPP_SUB_DOC where isd_mstr_doc_index =" & mstrDocIdx & " order by isd_sub_doc_index "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function DeleteBranchWithCostCentre(ByVal ds As DataSet) As String
            Dim strSql, strMsg As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To ds.Tables(0).Rows.Count - 1
                strSql = "DELETE FROM BRANCH_COST_CENTRE " &
                        "WHERE BCC_BRANCH_CODE = '" & ds.Tables(0).Rows(i)("BCC_BRANCH_CODE") & "' AND BCC_COY_ID = '" & ds.Tables(0).Rows(i)("BCC_COY_ID") & "'"
                Common.Insert2Ary(strAryQuery, strSql)
            Next

            If objDb.BatchExecute(strAryQuery) Then
                DeleteBranchWithCostCentre = objGlobal.GetErrorMessage("00004")
            Else
                DeleteBranchWithCostCentre = objGlobal.GetErrorMessage("00008")
            End If
        End Function

        Public Function GetBranchCostCentre(ByVal strMode As String, ByVal strCoyId As String, ByVal strBranchCode As String, ByVal strCostCentre As String, ByVal strDesc As String) As DataSet
            Dim strSql, strTemp As String
            Dim ds As DataSet

            If strMode = "A" Then
                strSql = "SELECT CC_CC_CODE, CC_CC_DESC, NULL AS BCC_BR_CC_INDEX FROM COST_CENTRE " &
                        "WHERE CC_COY_ID = '" & strCoyId & "' AND CC_STATUS = 'A' "

                If strCostCentre <> "" Then
                    strTemp = Common.BuildWildCard(strCostCentre)
                    strSql &= " AND CC_CC_CODE" & Common.ParseSQL(strTemp)
                End If

                If strDesc <> "" Then
                    strTemp = Common.BuildWildCard(strDesc)
                    strSql &= " AND CC_CC_DESC" & Common.ParseSQL(strTemp)
                End If

                strSql &= " AND CC_CC_CODE NOT IN " &
                        "(SELECT BCC_CC_CODE FROM BRANCH_COST_CENTRE " &
                        "WHERE BCC_BRANCH_CODE = '" & Common.Parse(strBranchCode) & "' AND BCC_COY_ID = '" & strCoyId & "') " &
                        "ORDER BY CC_CC_CODE "
            Else
                strSql = "SELECT CC_CC_CODE, CC_CC_DESC, BCC_BR_CC_INDEX " &
                        "FROM BRANCH_COST_CENTRE " &
                        "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID AND CC_STATUS = 'A' " &
                        "WHERE BCC_BRANCH_CODE = '" & Common.Parse(strBranchCode) & "' AND BCC_COY_ID = '" & strCoyId & "' "

                If strCostCentre <> "" Then
                    strTemp = Common.BuildWildCard(strCostCentre)
                    strSql &= " AND CC_CC_CODE" & Common.ParseSQL(strTemp)
                End If

                If strDesc <> "" Then
                    strTemp = Common.BuildWildCard(strDesc)
                    strSql &= " AND CC_CC_DESC" & Common.ParseSQL(strTemp)
                End If

                strSql &= " ORDER BY CC_CC_CODE "
            End If

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        Public Function InsertBranchCostCentre(ByVal ds As DataSet) As String
            Dim strSql, strMsg As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To ds.Tables(0).Rows.Count - 1
                strSql = "INSERT INTO BRANCH_COST_CENTRE (BCC_COY_ID, BCC_BRANCH_CODE, BCC_CC_CODE) " &
                        "VALUES ('" & ds.Tables(0).Rows(i)("BCC_COY_ID") & "','" & Common.Parse(ds.Tables(0).Rows(i)("BCC_BRANCH_CODE")) & "','" & Common.Parse(ds.Tables(0).Rows(i)("BCC_CC_CODE")) & "')"
                Common.Insert2Ary(strAryQuery, strSql)
            Next

            If objDb.BatchExecute(strAryQuery) Then
                InsertBranchCostCentre = objGlobal.GetErrorMessage("00003")
            Else
                InsertBranchCostCentre = objGlobal.GetErrorMessage("00007")
            End If
        End Function

        Public Function DeleteBranchCostCentre(ByVal ds As DataSet) As String
            Dim strSql, strMsg As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To ds.Tables(0).Rows.Count - 1
                strSql = "DELETE FROM BRANCH_COST_CENTRE WHERE BCC_BR_CC_INDEX = '" & ds.Tables(0).Rows(i)("BCC_BR_CC_INDEX") & "'"
                Common.Insert2Ary(strAryQuery, strSql)
            Next

            If objDb.BatchExecute(strAryQuery) Then
                DeleteBranchCostCentre = objGlobal.GetErrorMessage("00004")
            Else
                DeleteBranchCostCentre = objGlobal.GetErrorMessage("00008")
            End If
        End Function
        Public Function getRuleCategory(Optional ByVal glcode As String = "", Optional ByVal searchstr As String = "", Optional ByVal igcCoyId As String = "", Optional ByVal BatchUpload As Boolean = False) As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            If Not searchstr = "" Then
                strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" &
                " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & glcode & "' and igc_glrule_category like '%" & searchstr & "%' GROUP BY igc_glrule_category"
                ds = objDb.FillDs(strSql)
            Else
                If igcCoyId = "" Then
                    strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" &
                    " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                    "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & glcode & "' GROUP BY igc_glrule_category"
                    ds = objDb.FillDs(strSql)
                Else
                    If BatchUpload = True Then
                        strSql = "SELECT igc_glrule_category_index, igc_glrule_category,igc_glrule_category_remark,igg_gl_code FROM ipp_glrule,ipp_glrule_category,Ipp_glrule_gl " &
                                " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                                "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And  igc_Coy_Id LIKE '" & igcCoyId & "' ORDER BY igg_gl_code, igc_glrule_category"
                    Else
                        strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" &
                        " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                        "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igc_Coy_Id LIKE '" & igcCoyId & "' GROUP BY igc_glrule_category"
                    End If
                    ds = objDb.FillDs(strSql)
                End If
            End If

            Return ds
        End Function
        Public Function getRuleCategoryGLCode(Optional ByVal igcCoyId As String = "") As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT igg_gl_code FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl " &
                    " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                    "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And  igc_Coy_Id LIKE '" & igcCoyId & "' GROUP BY igg_gl_code"
            ds = objDb.FillDs(strSql)

            Return ds
        End Function
        'Public Function sendMailToNextAO(ByVal intInvIdx As Integer, ByVal role As String, Optional ByVal intApprGrpIdx As String = "", Optional ByVal currentseq As String = "")
        '    Dim strsql, strcond, strAO As String
        '    Dim blnRelief As String
        '    Dim ds As New DataSet
        '    Dim dsDoc As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common
        '    Dim objDB As New EAD.DBCom
        '    Dim strDocType As String
        '    Dim dsAO As New DataSet
        '    Dim i, j As Integer
        '    'Dim intApprGrpIdx As Integer
        '    Dim dsApprGrp As New DataSet

        '    'get invoice detail
        '    dsDoc = GetIPPDoc(intInvIdx)


        '    'intApprGrpIdx = objDB.GetVal("SELECT CDM_IPP_APPROVAL_GRP_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & dsDoc.Tables(0).Rows(0)("IM_CREATED_BY") & "' AND um_coy_id = '" & Common.Parse(companyID) & "')")

        '    If Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) <> "" Then
        '        If Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "INV" Then
        '            strDocType = "Invoice"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "BILL" Then
        '            strDocType = "Bill"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "CN" Then
        '            strDocType = "Credit Note"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "DN" Then
        '            strDocType = "Debit Note"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_TYPE")) = "LETTER" Then
        '            strDocType = "Letter"
        '        End If
        '    End If

        '    strsql = "SELECT FA_RELIEF_IND FROM FINANCE_APPROVAL WHERE FA_INVOICE_INDEX = '" & intInvIdx & "' AND FA_SEQ = 1 AND FA_APPROVAL_GRP_INDEX = '" & intApprGrpIdx & "'"
        '    blnRelief = objDB.GetVal(strsql)

        '    If blnRelief = "O" Then
        '        blnRelief = True
        '    Else
        '        blnRelief = False
        '    End If

        '    'currentseq = objDB.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvIdx & "' and fa_ao = '" & userID & "'")
        '    'If currentseq = "" Then
        '    '    currentseq = objDB.Get1Column("FINANCE_APPROVAL", "FA_SEQ", " WHERE FA_INVOICE_INDEX='" & intInvIdx & "' and FA_A_AO = '" & userID & "'")
        '    'End If
        '    'strsql = "SELECT FA.FA_AGA_TYPE AS FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL " & _
        '    '                     "FROM FINANCE_APPROVAL FA " & _
        '    '                     "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " & _
        '    '                     "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " & _
        '    '                     "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = '" & CInt(currentseq) + 1 & "'"
        '    strsql = "SELECT FA.FA_AGA_TYPE AS FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL, " & _
        '                          "IFNULL(FA.FA_A_AO_2,'') AS FA_A_AO_2, IFNULL(UM3.UM_USER_NAME,'') AS AAO_NAME2, IFNULL(UM3.UM_EMAIL,'') AS AAO_EMAIL2, " & _
        '                          "IFNULL(FA.FA_A_AO_3,'') AS FA_A_AO_3, IFNULL(UM4.UM_USER_NAME,'') AS AAO_NAME3, IFNULL(UM4.UM_EMAIL,'') AS AAO_EMAIL3, " & _
        '                          "IFNULL(FA.FA_A_AO_4,'') AS FA_A_AO_4, IFNULL(UM5.UM_USER_NAME,'') AS AAO_NAME4, IFNULL(UM5.UM_EMAIL,'') AS AAO_EMAIL4 " & _
        '                                   "FROM FINANCE_APPROVAL FA " & _
        '                                   "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " & _
        '                                   "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " & _
        '                                   "LEFT JOIN USER_MSTR AS UM3 ON UM3.UM_USER_ID = FA.FA_A_AO_2 AND UM3.UM_STATUS = 'A' AND UM3.UM_DELETED = 'N' AND UM3.UM_COY_ID = '" & companyID & "' " & _
        '                                   "LEFT JOIN USER_MSTR AS UM4 ON UM4.UM_USER_ID = FA.FA_A_AO_3 AND UM4.UM_STATUS = 'A' AND UM4.UM_DELETED = 'N' AND UM4.UM_COY_ID = '" & companyID & "' " & _
        '                                   "LEFT JOIN USER_MSTR AS UM5 ON UM5.UM_USER_ID = FA.FA_A_AO_4 AND UM5.UM_STATUS = 'A' AND UM5.UM_DELETED = 'N' AND UM5.UM_COY_ID = '" & companyID & "' " & _
        '                                   "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = '" & CInt(currentseq) + 1 & "'"
        '    ds = objDB.FillDs(strsql)

        '    If ds.Tables(0).Rows(0)("FA_AO") = "FM" Then
        '        strBody &= "<P>You have a Payment Document waiting for approval as follows: " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & ") <BR>"
        '    ElseIf ds.Tables(0).Rows(0)("FA_AO") = "FO" Then
        '        strBody &= "<P>You have a Payment Document waiting for verification as follows: " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & ") <BR>"
        '    ElseIf ds.Tables(0).Rows(0)("FA_AO") = "AO" Then
        '        strBody &= "<P>You have a Payment Document waiting for approval as follows: " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & ") <BR>"
        '    End If
        '    strBody &= "<P>Document(Type) : " & strDocType & "<BR>"
        '    strBody &= "Document No.      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & "<BR>"
        '    strBody &= "Document Date     : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_CREATED_ON")) & "<BR>"
        '    strBody &= "Vendor(Name)      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_S_COY_NAME")) & "<BR>"
        '    strBody &= "<P>For more details, please login to "
        '    strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

        '    If ds.Tables(0).Rows.Count > 0 Then
        '        Dim objMail As New AppMail
        '        If blnRelief Then
        '            If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
        '                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " , <BR>" & strBody
        '            Else
        '                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '                objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) & _
        '                            ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL2")) & _
        '                            ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL3")) & _
        '                            ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL4"))
        '                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " , <BR>" & strBody
        '            End If
        '        Else
        '            objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '            objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " , <BR>" & strBody
        '        End If

        '        objMail.Subject = "IPP Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("IM_INVOICE_NO")) & " Created"
        '        objMail.SendMail()
        '    End If
        '    objCommon = Nothing
        'End Function
        'Public Function sendMailToAO(ByVal dsDoc As DataSet, ByVal intInvIdx As Integer, Optional ByVal intApprGrpIdx As String = "")
        '    Dim strsql, strcond, strAO As String
        '    Dim blnRelief As String
        '    Dim ds As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common
        '    Dim objDB As New EAD.DBCom
        '    Dim strDocType As String
        '    Dim dsAO As New DataSet
        '    Dim i, j As Integer
        '    'Dim intApprGrpIdx As Integer
        '    Dim dsApprGrp As New DataSet

        '    'intApprGrpIdx = objDB.GetVal("SELECT CDM_IPP_APPROVAL_GRP_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & Common.Parse(userID) & "' AND um_coy_id = '" & Common.Parse(companyID) & "')")

        '    If Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) <> "" Then
        '        If Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "INV" Then
        '            strDocType = "Invoice"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "BILL" Then
        '            strDocType = "Bill"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "CN" Then
        '            strDocType = "Credit Note"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "DN" Then
        '            strDocType = "Debit Note"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "LETTER" Then
        '            strDocType = "Letter"
        '        End If
        '    End If


        '    strBody &= "<P>You have a Payment Document waiting for approval as follows: " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & ") <BR>"
        '    strBody &= "<P>Document(Type) : " & strDocType & "<BR>"
        '    strBody &= "Document No.      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & "<BR>"
        '    strBody &= "Document Date     : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocDate")) & "<BR>"
        '    strBody &= "Vendor(Name)      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "<BR>"
        '    strBody &= "<P>For more details, please login to "
        '    strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

        '    'strsql = "SELECT aga_relief_ind FROM approval_grp_ipp " & _
        '    '         "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' and aga_type = 'AO' order by aga_seq"

        '    strsql = "SELECT FA_RELIEF_IND FROM finance_approval WHERE FA_INVOICE_INDEX = '" & intInvIdx & "' AND FA_SEQ = 1 AND FA_APPROVAL_GRP_INDEX = '" & intApprGrpIdx & "'"
        '    blnRelief = objDB.GetVal(strsql)

        '    If blnRelief = "O" Then
        '        blnRelief = True
        '    Else
        '        blnRelief = False
        '    End If


        '    'strsql = "SELECT FA.FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL " & _
        '    '         "FROM FINANCE_APPROVAL FA " & _
        '    '         "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " & _
        '    '         "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " & _
        '    '         "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = 1"

        '    strsql = "SELECT FA.FA_AGA_TYPE AS FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL, " & _
        '                        "IFNULL(FA.FA_A_AO_2,'') AS FA_A_AO_2, IFNULL(UM3.UM_USER_NAME,'') AS AAO_NAME2, IFNULL(UM3.UM_EMAIL,'') AS AAO_EMAIL2, " & _
        '                        "IFNULL(FA.FA_A_AO_3,'') AS FA_A_AO_3, IFNULL(UM4.UM_USER_NAME,'') AS AAO_NAME3, IFNULL(UM4.UM_EMAIL,'') AS AAO_EMAIL3, " & _
        '                        "IFNULL(FA.FA_A_AO_4,'') AS FA_A_AO_4, IFNULL(UM5.UM_USER_NAME,'') AS AAO_NAME4, IFNULL(UM5.UM_EMAIL,'') AS AAO_EMAIL4 " & _
        '                                 "FROM FINANCE_APPROVAL FA " & _
        '                                 "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " & _
        '                                 "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " & _
        '                                 "LEFT JOIN USER_MSTR AS UM3 ON UM3.UM_USER_ID = FA.FA_A_AO_2 AND UM3.UM_STATUS = 'A' AND UM3.UM_DELETED = 'N' AND UM3.UM_COY_ID = '" & companyID & "' " & _
        '                                 "LEFT JOIN USER_MSTR AS UM4 ON UM4.UM_USER_ID = FA.FA_A_AO_3 AND UM4.UM_STATUS = 'A' AND UM4.UM_DELETED = 'N' AND UM4.UM_COY_ID = '" & companyID & "' " & _
        '                                 "LEFT JOIN USER_MSTR AS UM5 ON UM5.UM_USER_ID = FA.FA_A_AO_4 AND UM5.UM_STATUS = 'A' AND UM5.UM_DELETED = 'N' AND UM5.UM_COY_ID = '" & companyID & "' " & _
        '                                 "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = 1"

        '    ds = objDB.FillDs(strsql)

        '    If ds.Tables(0).Rows.Count > 0 Then
        '        Dim objMail As New AppMail
        '        If blnRelief Then
        '            If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
        '                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
        '            Else
        '                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '                objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) & _
        '                         ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL2")) & _
        '                         ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL3")) & _
        '                         ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL4"))
        '                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
        '            End If
        '        Else
        '            objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '            objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
        '        End If

        '        objMail.Subject = "IPP Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & " Created"
        '        objMail.SendMail()
        '    End If
        '    objCommon = Nothing
        'End Function
        'Public Function sendMailToFO(ByVal dsDoc As DataSet, ByVal intInvIdx As Integer, Optional ByVal intApprGrpIdx As String = "")
        '    Dim strsql, strcond, strAO As String
        '    Dim blnRelief As String
        '    Dim ds As New DataSet
        '    Dim strBody As String
        '    Dim objCommon As New Common
        '    Dim objDB As New EAD.DBCom
        '    Dim strDocType As String
        '    Dim dsAO As New DataSet
        '    Dim i, j As Integer
        '    'Dim intApprGrpIdx As Integer
        '    Dim dsApprGrp As New DataSet

        '    'intApprGrpIdx = objDB.GetVal("SELECT CDM_IPP_APPROVAL_GRP_INDEX FROM company_dept_mstr WHERE cdm_coy_id = '" & Common.Parse(companyID) & "' AND cdm_deleted = 'N' AND CDM_IPP_APPROVAL_GRP_INDEX IS NOT NULL AND CDM_IPP_APPROVAL_GRP_INDEX <> 0 AND cdm_dept_code = (SELECT UM_DEPT_ID FROM user_mstr WHERE um_user_id = '" & Common.Parse(userID) & "' AND um_coy_id = '" & Common.Parse(companyID) & "')")

        '    If Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) <> "" Then
        '        If Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "INV" Then
        '            strDocType = "Invoice"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "BILL" Then
        '            strDocType = "Bill"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "CN" Then
        '            strDocType = "Credit Note"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "DN" Then
        '            strDocType = "Debit Note"
        '        ElseIf Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) = "LETTER" Then
        '            strDocType = "Letter"
        '        End If
        '    End If


        '    strBody &= "<P>You have a Payment Document waiting for verification as follows: " & strDocType & " (" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & ") <BR>"
        '    strBody &= "<P>Document(Type) : " & strDocType & "<BR>"
        '    strBody &= "Document No.      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & "<BR>"
        '    strBody &= "Document Date     : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocDate")) & "<BR>"
        '    strBody &= "Vendor(Name)      : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "<BR>"
        '    strBody &= "<P>For more details, please login to "
        '    strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
        '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen

        '    'strsql = "SELECT aga_relief_ind FROM approval_grp_ipp " & _
        '    '         "WHERE aga_grp_index  =  '" & intApprGrpIdx & "' and aga_type = 'AO' order by aga_seq"

        '    strsql = "SELECT FA_RELIEF_IND FROM finance_approval WHERE FA_INVOICE_INDEX = '" & intInvIdx & "' AND FA_SEQ = 1 AND FA_APPROVAL_GRP_INDEX = '" & intApprGrpIdx & "'"
        '    blnRelief = objDB.GetVal(strsql)

        '    If blnRelief = "O" Then
        '        blnRelief = True
        '    Else
        '        blnRelief = False
        '    End If


        '    'strsql = "SELECT FA.FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL " & _
        '    '         "FROM FINANCE_APPROVAL FA " & _
        '    '         "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " & _
        '    '         "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " & _
        '    '         "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = 1"
        '    strsql = "SELECT FA.FA_AGA_TYPE AS FA_AO,UM.UM_USER_NAME AS AO_NAME,UM.UM_EMAIL AS AO_EMAIL, IFNULL(FA.FA_A_AO,'') AS FA_A_AO, IFNULL(UM2.UM_USER_NAME,'') AS AAO_NAME, IFNULL(UM2.UM_EMAIL,'') AS AAO_EMAIL, " & _
        '                           "IFNULL(FA.FA_A_AO_2,'') AS FA_A_AO_2, IFNULL(UM3.UM_USER_NAME,'') AS AAO_NAME2, IFNULL(UM3.UM_EMAIL,'') AS AAO_EMAIL2, " & _
        '                           "IFNULL(FA.FA_A_AO_3,'') AS FA_A_AO_3, IFNULL(UM4.UM_USER_NAME,'') AS AAO_NAME3, IFNULL(UM4.UM_EMAIL,'') AS AAO_EMAIL3, " & _
        '                           "IFNULL(FA.FA_A_AO_4,'') AS FA_A_AO_4, IFNULL(UM5.UM_USER_NAME,'') AS AAO_NAME4, IFNULL(UM5.UM_EMAIL,'') AS AAO_EMAIL4 " & _
        '                                    "FROM FINANCE_APPROVAL FA " & _
        '                                    "INNER JOIN USER_MSTR AS UM ON UM.UM_USER_ID = FA.FA_AO AND UM.UM_STATUS = 'A' AND UM.UM_DELETED = 'N' AND UM.UM_COY_ID = '" & companyID & "' " & _
        '                                    "LEFT JOIN USER_MSTR AS UM2 ON UM2.UM_USER_ID = FA.FA_A_AO AND UM2.UM_STATUS = 'A' AND UM2.UM_DELETED = 'N' AND UM2.UM_COY_ID = '" & companyID & "' " & _
        '                                    "LEFT JOIN USER_MSTR AS UM3 ON UM3.UM_USER_ID = FA.FA_A_AO_2 AND UM3.UM_STATUS = 'A' AND UM3.UM_DELETED = 'N' AND UM3.UM_COY_ID = '" & companyID & "' " & _
        '                                    "LEFT JOIN USER_MSTR AS UM4 ON UM4.UM_USER_ID = FA.FA_A_AO_3 AND UM4.UM_STATUS = 'A' AND UM4.UM_DELETED = 'N' AND UM4.UM_COY_ID = '" & companyID & "' " & _
        '                                    "LEFT JOIN USER_MSTR AS UM5 ON UM5.UM_USER_ID = FA.FA_A_AO_4 AND UM5.UM_STATUS = 'A' AND UM5.UM_DELETED = 'N' AND UM5.UM_COY_ID = '" & companyID & "' " & _
        '                                    "WHERE FA.FA_INVOICE_INDEX = " & intInvIdx & " AND FA.FA_SEQ = '1'"

        '    ds = objDB.FillDs(strsql)

        '    If ds.Tables(0).Rows.Count > 0 Then
        '        Dim objMail As New AppMail
        '        If blnRelief Then
        '            If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
        '                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Finance Officer), <BR>" & strBody
        '            Else
        '                objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '                objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) & _
        '                                  ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL2")) & _
        '                                  ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL3")) & _
        '                                  ";" & Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL4"))
        '                objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Finance Officer), <BR>" & strBody
        '            End If
        '        Else
        '            objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
        '            objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Finance Officer), <BR>" & strBody
        '        End If

        '        objMail.Subject = "IPP Document : " & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & " Created"
        '        objMail.SendMail()
        '    End If
        '    objCommon = Nothing
        'End Function
        Public Function DeleteSubDoc(ByVal SubDocIndex As Int64) As String
            Dim strSql, strMsg As String
            Dim strAryQuery(0) As String

            strSql = "DELETE FROM IPP_SUB_DOC " &
                        "WHERE isd_sub_doc_index = " & SubDocIndex & ""
            Common.Insert2Ary(strAryQuery, strSql)

            If objDb.BatchExecute(strAryQuery) Then
                DeleteSubDoc = objGlobal.GetErrorMessage("00004")
            Else
                DeleteSubDoc = objGlobal.GetErrorMessage("00008")
            End If
        End Function

        Public Sub getReverseCharge(ByVal userID As String, ByVal companyID As String, ByRef strReverseChargeInput As String, ByRef strReverseChargeOutput As String)
            Dim strSql As String
            strSql = "SELECT IFNULL(IP_PARAM_VALUE, '') FROM IPP_PARAMETER " &
                    "INNER JOIN TAX_MSTR ON IP_PARAM_VALUE = TM_TAX_CODE AND TM_COY_ID = '" & companyID & "' " &
                    "WHERE IP_COY_ID = '" & companyID & "' AND IP_PARAM = 'REVERSE_CHARGE_INPUT' " &
                    "AND TM_CATEGORY = 'IPP' AND TM_DELETED = 'N'"
            strReverseChargeInput = objDb.GetVal(strSql)

            strSql = "SELECT IFNULL(IP_PARAM_VALUE, '') FROM IPP_PARAMETER " &
                    "INNER JOIN TAX_MSTR ON IP_PARAM_VALUE = TM_TAX_CODE AND TM_COY_ID = '" & companyID & "' " &
                    "WHERE IP_COY_ID = '" & companyID & "' AND IP_PARAM = 'REVERSE_CHARGE_OUTPUT' " &
                    "AND TM_CATEGORY = 'IPP' AND TM_DELETED = 'N'"
            strReverseChargeOutput = objDb.GetVal(strSql)
        End Sub

        'Modified for IPP GST Stage 2A - CH (30/1/2015)
        Public Function ChkValidBranchCode(ByVal userID As String, ByVal companyID As String, ByVal strBranchCode As String) As Boolean
            Dim strSql As String
            strSql = "SELECT '*' FROM COMPANY_BRANCH_MSTR " &
                    "WHERE CBM_COY_ID = '" & companyID & "' AND CBM_STATUS = 'A' " &
                    "AND CBM_BRANCH_CODE = '" & Common.Parse(strBranchCode) & "'"
            If objDb.Exist(strSql) > 0 Then
                ChkValidBranchCode = True
            Else
                ChkValidBranchCode = False
            End If

        End Function

        Public Function ChkValidCostCenter(ByVal userID As String, ByVal companyID As String, ByVal strCostCenter As String) As Boolean
            Dim strSql As String
            strSql = "SELECT '*' FROM COST_CENTRE " &
                    "WHERE CC_COY_ID = '" & companyID & "' AND CC_STATUS = 'A' " &
                    "AND CC_CC_CODE = '" & Common.Parse(strCostCenter) & "'"
            If objDb.Exist(strSql) > 0 Then
                ChkValidCostCenter = True
            Else
                ChkValidCostCenter = False
            End If

        End Function

        Public Function ChkValidGLCode(ByVal userID As String, ByVal companyID As String, ByVal strGLCode As String) As Boolean
            Dim strSql As String
            'Modified for IPP Gst Stage 2A - CH - 6 Feb 2015
            strSql = "SELECT * FROM COMPANY_B_GL_CODE " &
                    "WHERE CBG_B_COY_ID = '" & companyID & "' AND CBG_STATUS = 'A' " &
                    "AND CBG_B_GL_CODE = '" & Common.Parse(strGLCode) & "'"
            If objDb.Exist(strSql) > 0 Then
                ChkValidGLCode = True
            Else
                ChkValidGLCode = False
            End If

        End Function
        '---------------------------------------------

        'Yap: 2015-Feb-02: GST IPP Stage 2A
        Public Function GenXMLInit(ByVal userID As String, ByVal companyID As String, ByVal intSeqN As Integer, ByVal intTranN As Integer, ByVal strBRN As String, ByVal SOAPUserName As String, ByVal SOAPPassword As String) As String
            Dim strUserName, strPassword As String

            strUserName = SOAPUserName
            strPassword = SOAPPassword

            Dim xmlContent As String = ""
            xmlContent = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.eai.hlb.com"">"
            xmlContent += "<soapenv:Header>" & vbCrLf
            xmlContent += "<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">" & vbCrLf
            xmlContent += "<wsse:UsernameToken xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" & vbCrLf
            xmlContent += "<wsse:Username>" & strUserName & "</wsse:Username>" & vbCrLf
            xmlContent += "<wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">" & strPassword & "</wsse:Password>" & vbCrLf
            xmlContent += "</wsse:UsernameToken>" & vbCrLf
            xmlContent += "</wsse:Security>" & vbCrLf

            xmlContent += "</soapenv:Header>" & vbCrLf
            xmlContent += "<soapenv:Body>" & vbCrLf
            xmlContent += "<ws:GSTInq>" & vbCrLf
            xmlContent += "<ServiceHeader>" & vbCrLf
            xmlContent += "<SC_QueueID></SC_QueueID>" & vbCrLf
            xmlContent += "<SC_Version>1.0</SC_Version>" & vbCrLf
            xmlContent += "<SC_TransType>120020</SC_TransType>" & vbCrLf
            xmlContent += "<SC_ApplName>EPROC</SC_ApplName>" & vbCrLf
            xmlContent += "<SC_ApplID>EPROC</SC_ApplID>" & vbCrLf
            xmlContent += "<SC_ApplTransID>" & intTranN & "</SC_ApplTransID>" & vbCrLf
            xmlContent += "<SC_TransDate>" & Date.Today.ToString("dd") & Date.Today.ToString("MM") & Date.Today.ToString("yy") & "</SC_TransDate>" & vbCrLf
            xmlContent += "<SC_TransTime>" & DateTime.Today.ToString("hh") & DateTime.Today.ToString("mm") & DateTime.Today.ToString("ss") & "</SC_TransTime>" & vbCrLf
            xmlContent += "<SC_TransUserID></SC_TransUserID>" & vbCrLf
            xmlContent += "<SC_TransUserInfo></SC_TransUserInfo>" & vbCrLf
            xmlContent += "<SC_ApplUserID>" & userID & "</SC_ApplUserID>" & vbCrLf
            xmlContent += "<SC_TellerID>43000</SC_TellerID>" & vbCrLf
            xmlContent += "<SC_BranchCode>900</SC_BranchCode>" & vbCrLf
            xmlContent += "<SC_CtrlUnit>F1</SC_CtrlUnit>" & vbCrLf
            xmlContent += "<SC_Locale>EN-US</SC_Locale>" & vbCrLf
            xmlContent += "<SC_CountryCode>MY</SC_CountryCode>" & vbCrLf
            xmlContent += "<SC_ServiceID>GSTInq</SC_ServiceID>" & vbCrLf


            xmlContent += "</ServiceHeader>" & vbCrLf

            xmlContent += "<ServiceBody>" & vbCrLf
            xmlContent += "<Request>" & vbCrLf
            xmlContent += "<JournalSequence>" & intSeqN & "</JournalSequence>" & vbCrLf
            xmlContent += "<CurrCode>MYR</CurrCode>" & vbCrLf
            xmlContent += "<IDNum>" & strBRN & "</IDNum>" & vbCrLf
            xmlContent += "<IDType>BRN</IDType>" & vbCrLf
            xmlContent += "</Request>" & vbCrLf
            xmlContent += "</ServiceBody>" & vbCrLf

            xmlContent += "</ws:GSTInq>" & vbCrLf
            xmlContent += "</soapenv:Body>" & vbCrLf
            xmlContent += "</soapenv:Envelope>"

            Return xmlContent
        End Function

        Public Shared Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
            Return True
        End Function

        'Yap: 2015-Feb-02: GST IPP Stage 2A
        Public Function SendSoap(ByVal XSTr As String, ByVal intSeqN As Integer) As String
            Try
                'Dim strURI As New Uri("https://gateway.jpj.gov.my:15051/SirimSub")

                'Jules 2015-Feb-24: GST IPP Stage 2A
                'Dim JPJURI As String = "https://203.223.153.94:50002/" ''''System.Configuration.ConfigurationManager.AppSettings("JPJURI")
                Dim EAIURI As String = ConfigurationSettings.AppSettings("EAIIP") & ":" & ConfigurationSettings.AppSettings("EAIPort") & ""
                'Jules-End.

                Dim strURI As New Uri(EAIURI)

                System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications

                Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.Create(strURI), Net.HttpWebRequest)

                request.ContentType = "text/xml; charset=utf-8"

                request.Method = "POST"

                request.Accept = "text/xml"

                Dim enconding As New System.Text.UTF8Encoding

                Dim byteData As Byte() = enconding.GetBytes(XSTr)

                request.ContentLength = byteData.Length

                Dim postreqstream As IO.Stream = request.GetRequestStream

                postreqstream.Write(byteData, 0, byteData.Length)

                postreqstream.Flush()

                postreqstream.Close()

                Dim response As Net.HttpWebResponse = DirectCast(request.GetResponse, Net.HttpWebResponse)

                Dim reader As New IO.StreamReader(response.GetResponseStream)

                Dim theresult As String = reader.ReadToEnd

                Return Replace(theresult, """", "")

            Catch ex As Exception
                Return "ERROR"
            End Try

        End Function

        'Yap: 2015-Feb-02: GST IPP Stage 2A
        Public Function InsertWebLog(ByVal userID As String, ByVal companyID As String, ByVal intSeqN As Integer, ByVal strRegN As String) As String
            Dim strSql, strMsg As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            strSql = "INSERT INTO ipp_webservtrans_log (IWL_JOURNAL_SEQ, IWL_IDNUM, IWL_IDTYPE, IWL_REQUEST_DATE, IWL_USER_ID, IWL_ENT_BY, IWL_ENT_DATE,IWL_REQUEST_STATUS)" &
                     "VALUES (" & intSeqN & ", '" & strRegN & "', 'BRN', NOW(), '" & userID & "', '" & userID & "', NOW(),'0')"

            Common.Insert2Ary(strAryQuery, strSql)
            objDb.BatchExecute(strAryQuery)
        End Function

        'Yap: 2015-Feb-02: GST IPP Stage 2A
        Public Function UpdateWebLog(ByVal intSeqN As Integer, ByVal strRegN As String, ByVal strRes As String, ByVal strResError As String) As String
            Dim strSql, strMsg As String
            Dim i As Integer

            If strRes = "GD" Then
                strSql = "UPDATE ipp_webservtrans_log SET IWL_REQUEST_STATUS = 'GD' WHERE IWL_JOURNAL_SEQ = '" & intSeqN & "' AND IWL_IDNUM = '" & strRegN & "' AND IWL_REQUEST_DATE = NOW();"
                objDb.Execute(strSql)
            Else
                strSql = "UPDATE ipp_webservtrans_log SET IWL_REQUEST_STATUS = 'ERROR', IWL_REMARKS = '" & strResError & "' WHERE IWL_JOURNAL_SEQ = '" & intSeqN & "' AND IWL_IDNUM = '" & strRegN & "' AND IWL_REQUEST_DATE = NOW();"
                objDb.Execute(strSql)
            End If
        End Function

        'Zulham 05/01/2016 - IPP STAGE 4 PHASE 2
        'To check CN/DN invoices that it's tied to
        'CN/DN status cannot precede the invoices statuses
        Public Function checkStatus(ByVal CNDNInvoiceNo As String, ByVal coyIdx As String, ByVal CNDNInvoiceStatus As String) As Boolean
            Dim exist As String
            exist = objDb.GetVal("SELECT '*' FROM invoice_mstr, invoice_details " &
            "WHERE(im_invoice_no = id_ref_no And im_s_coy_id = id_s_coy_id) " &
            "AND id_invoice_no = '" & CNDNInvoiceNo & "' AND id_s_coy_id = '" & coyIdx & "' " &
            "AND im_invoice_status = '" & CNDNInvoiceStatus & "'")

            If exist.Trim.Length > 0 Then
                Return False
            Else
                exist = objDb.GetVal("SELECT '*' FROM invoice_mstr, invoice_details " &
                "WHERE(im_invoice_no = id_ref_no And im_s_coy_id = id_s_coy_id) " &
                "AND id_invoice_no = '" & CNDNInvoiceNo & "' AND id_s_coy_id = '" & coyIdx & "' and im_invoice_Status = '14' limit 1")
                If exist.Trim = "*" Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Function
        'Zulham 15/02/2016 - IPP Stage 4 Phase 2
        Public Function getItemDetailGSTAmount(ByVal userID As String, ByVal companyID As String, ByVal invoiceNo As String, ByVal coyIdx As String, Optional ByVal mode As String = "", Optional ByVal invoiceLine As String = "") As Double

            Dim gstAmount As Double = 0.0
            Dim exist As String = ""
            Dim counter As Integer = 0

            'exist = objDb.GetVal("SELECT '*' AS 'id_gst_value' FROM invoice_details, invoice_mstr " & _
            '                       "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " & _
            '                       "id_invoice_no = '" & invoiceNo & "' and id_s_coy_id = '" & coyIdx & "' " & _
            '                       "and im_b_coy_id = '" & companyID & "'")
            exist = objDb.GetVal("SELECT '*' AS 'id_gst_value' " &
                               "FROM invoice_details, invoice_mstr, tax_mstr tm1, tax t1, tax_mstr tm2, tax t2 " &
                               "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " &
                               "id_invoice_no = '" & invoiceNo & "' AND id_s_coy_id = '" & coyIdx & "' " &
                               "AND t1.tax_code = tm1.tm_tax_rate " &
                               "AND t2.tax_code = tm2.tm_tax_rate " &
                               "AND tm1.tm_coy_id = '" & companyID & "' " &
                               "AND tm2.tm_coy_id = '" & companyID & "' " &
                               "AND tm1.tm_tax_code = id_gst_input_tax_code " &
                               "AND tm2.tm_tax_code = id_gst_output_tax_code " &
                               "AND (t1.tax_perc <> t2.tax_perc OR id_gst_input_tax_code = 'TX4') " &
                               "AND im_b_coy_id = '" & companyID & "'")

            If mode = "" Then
                If exist.Trim.Length > 0 Then
                    'gstAmount = objDb.GetVal("SELECT SUM(IFNULL(id_gst_value,0.0)) AS 'id_gst_value' FROM invoice_details, invoice_mstr " & _
                    '            "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " & _
                    '            "id_invoice_no = '" & invoiceNo & "' and id_s_coy_id = '" & coyIdx & "' " & _
                    '            "and im_b_coy_id = '" & companyID & "'")
                    gstAmount = objDb.GetVal("SELECT SUM(IFNULL(id_gst_value,0.0)) AS 'id_gst_value' " &
                               "FROM invoice_details, invoice_mstr, tax_mstr tm1, tax t1, tax_mstr tm2, tax t2 " &
                               "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " &
                               "id_invoice_no = '" & invoiceNo & "' AND id_s_coy_id = '" & coyIdx & "' " &
                               "AND t1.tax_code = tm1.tm_tax_rate " &
                               "AND t2.tax_code = tm2.tm_tax_rate " &
                               "AND tm1.tm_coy_id = '" & companyID & "' " &
                               "AND tm2.tm_coy_id = '" & companyID & "' " &
                               "AND tm1.tm_tax_code = id_gst_input_tax_code " &
                               "AND tm2.tm_tax_code = id_gst_output_tax_code " &
                               "AND (t1.tax_perc <> t2.tax_perc OR id_gst_input_tax_code = 'TX4') " &
                               "AND im_b_coy_id = '" & companyID & "'")

                    Return gstAmount
                Else
                    Return 0.0
                End If
            Else
                If exist.Trim.Length > 0 Then
                    'Zulha m 05/04/2016 - IM5/IM^ Enhancement
                    'Added counter
                    'counter = objDb.GetVal("SELECT count(*) AS 'id_gst_value' FROM invoice_details, invoice_mstr " & _
                    '    "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " & _
                    '    "id_invoice_no = '" & invoiceNo & "' and id_s_coy_id = '" & coyIdx & "' " & _
                    '    "and im_b_coy_id = '" & companyID & "'")

                    'Zulham 08/09/2016 - Prod Issue
                    '"HLISB - IPP Nostro Charges" email
                    counter = objDb.GetVal("SELECT count(*) AS 'id_gst_value' " &
                            "FROM invoice_details, invoice_mstr, tax_mstr tm1, tax t1, tax_mstr tm2, tax t2 " &
                            "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " &
                            "id_invoice_no = '" & invoiceNo & "' AND id_s_coy_id = '" & coyIdx & "' " &
                            "AND t1.tax_code = tm1.tm_tax_rate " &
                            "AND t2.tax_code = tm2.tm_tax_rate " &
                            "AND tm1.tm_coy_id = '" & companyID & "' " &
                            "AND tm2.tm_coy_id = '" & companyID & "' " &
                            "AND tm1.tm_tax_code = id_gst_input_tax_code " &
                            "AND tm2.tm_tax_code = id_gst_output_tax_code " &
                            "AND (t1.tax_perc <> t2.tax_perc OR id_gst_input_tax_code = 'TX4') " &
                            "AND im_b_coy_id = '" & companyID & "' and id_invoice_line <> '" & invoiceLine & "' ")

                    If counter > 0 Then
                        'gstAmount = objDb.GetVal("SELECT SUM(IFNULL(id_gst_value,0.0)) AS 'id_gst_value' FROM invoice_details, invoice_mstr " & _
                        '            "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " & _
                        '            "id_invoice_no = '" & invoiceNo & "' and id_s_coy_id = '" & coyIdx & "' " & _
                        '            "and im_b_coy_id = '" & companyID & "' and id_invoice_line <> '" & invoiceLine & "'")
                        'Zulham 08/09/2016 - Prod Issue
                        '"HLISB - IPP Nostro Charges" email
                        gstAmount = objDb.GetVal("SELECT SUM(IFNULL(id_gst_value,0.0)) AS 'id_gst_value' " &
                                "FROM invoice_details, invoice_mstr, tax_mstr tm1, tax t1, tax_mstr tm2, tax t2 " &
                                "WHERE im_invoice_no = id_invoice_no AND im_s_coy_id = id_s_coy_id AND " &
                                "id_invoice_no = '" & invoiceNo & "' AND id_s_coy_id = '" & coyIdx & "' " &
                                "AND t1.tax_code = tm1.tm_tax_rate " &
                                "AND t2.tax_code = tm2.tm_tax_rate " &
                                "AND tm1.tm_coy_id = '" & companyID & "' " &
                                "AND tm2.tm_coy_id = '" & companyID & "' " &
                                "AND tm1.tm_tax_code = id_gst_input_tax_code " &
                                "AND tm2.tm_tax_code = id_gst_output_tax_code " &
                                "AND (t1.tax_perc <> t2.tax_perc OR id_gst_input_tax_code = 'TX4') " &
                                "AND im_b_coy_id = '" & companyID & "' and id_invoice_line <> '" & invoiceLine & "' ")
                    ElseIf counter = 0 Then
                        Return 0.0
                    End If

                    Return gstAmount
                Else
                    Return 0.0
                End If
            End If


        End Function
        'Zulham 16/02/2016 - IPP Stage 4 Phase 2
        Public Function getCNDNRefNo(ByVal userID As String, ByVal companyID As String, ByVal invoiceNo As String, ByVal vendorId As String, Optional ByVal status As String = "") As DataSet
            Dim strsql, ifExist As String
            Dim objDB As New EAD.DBCom
            Dim ds As New DataSet

            If status = "" Then
                ifExist = objDB.GetVal("SELECT DISTINCT('*') AS 'id_ref_no' FROM invoice_details, invoice_mstr WHERE id_ref_no = '" & invoiceNo & "' " &
                        "AND im_invoice_no = id_invoice_no AND id_s_coy_id = im_s_coy_id AND " &
                        "im_b_coy_id = '" & companyID & "' AND im_s_coy_id = '" & vendorId & "' and im_invoice_status not in ('14','10','15')")

                If ifExist.Trim.Length > 0 Then
                    strsql = "SELECT DISTINCT(id_ref_no) AS 'id_ref_no' FROM invoice_details, invoice_mstr WHERE id_ref_no = '" & invoiceNo & "' " &
                                      "AND im_invoice_no = id_invoice_no AND id_s_coy_id = im_s_coy_id AND " &
                                      "im_b_coy_id = '" & companyID & "' AND im_s_coy_id = '" & vendorId & "' and im_invoice_status not in ('14','10','15')"

                    ds = objDB.FillDs(strsql)
                    getCNDNRefNo = ds
                End If
            Else
                ifExist = objDB.GetVal("SELECT DISTINCT('*') AS 'id_ref_no' FROM invoice_details, invoice_mstr WHERE id_invoice_no = '" & invoiceNo & "' " &
                                       "AND im_invoice_no = id_invoice_no AND id_s_coy_id = im_s_coy_id AND " &
                                       "im_b_coy_id = '" & companyID & "' AND im_s_coy_id = '" & vendorId & "' and im_invoice_status in (" & status & ")")

                If ifExist.Trim.Length > 0 Then
                    strsql = "SELECT DISTINCT(id_ref_no) AS 'id_ref_no' FROM invoice_details, invoice_mstr WHERE id_invoice_no = '" & invoiceNo & "' " &
                                      "AND im_invoice_no = id_invoice_no AND id_s_coy_id = im_s_coy_id AND " &
                                      "im_b_coy_id = '" & companyID & "' AND im_s_coy_id = '" & vendorId & "' and im_invoice_status in (" & status & ")"

                    ds = objDB.FillDs(strsql)
                    getCNDNRefNo = ds
                End If
            End If

            Return getCNDNRefNo

        End Function
        'Zulham 04/09/2016 - IPP Stage 4 Phase 2
        Public Function getItemDetailsAmtandGST(ByVal invoiceNo As String, ByVal vendId As String) As DataTable
            Dim strSql As String
            Dim dtGroup As DataTable
            Dim ifExist As String = ""

            'Jules 2018.07.11 - PAMB Allow "\" and "#"
            invoiceNo = Replace(Replace(invoiceNo, "\", "\\"), "#", "\#")

            ifExist = objDb.GetVal("SELECT DISTINCT('*') FROM invoice_details WHERE id_invoice_no = '" & invoiceNo & "' and " &
                                   "id_s_coy_id = " & vendId & "")

            If ifExist.Trim.Length > 0 Then
                'Zulham 15/03/2016 - IPP GST Stage 4 Phase 2
                'Ignore the gst value if input tax code = im1 & im3
                strSql = "SELECT SUM(id_gst_value) AS 'id_gst_value', SUM(total_amount) AS 'total_amount' FROM (SELECT IF(id_gst_input_tax_code IN ('IM1','IM3') OR ((id_gst_output_tax_code IS NOT NULL AND TRIM(id_gst_output_tax_code) <> '' AND TRIM(id_gst_output_tax_code) <> 'N/A') AND id_gst_input_tax_code NOT IN ('IM1','IM3','TX4')), " &
                         "ROUND(0,2),ROUND(SUM(id_gst_value),2)) as 'id_gst_value', round(sum(id_received_qty*id_unit_cost),2) as 'total_amount' FROM invoice_details WHERE id_invoice_no = '" & invoiceNo & "' and " &
                         "id_s_coy_id = " & vendId & " GROUP BY id_gst_input_tax_code) zzz "

                dtGroup = objDb.FillDt(strSql)
                getItemDetailsAmtandGST = dtGroup
                Return getItemDetailsAmtandGST
            Else
                Return Nothing
            End If

        End Function

        Public Function RetFITR(ByVal userID As String, ByVal companyID As String, ByVal strCode As String, Optional ByVal dateFrom As String = "", Optional ByVal dateTo As String = "") As DataSet
            Dim strsql As String
            Dim objDB As New EAD.DBCom
            Dim ds As New DataSet

            strsql = "SELECT FM_FITR_INDEX, FM_FITR_CODE, FM_FITR_DESC, " &
                    "FM_FITR_RECOVERABLE, FM_FITR_IRRECOVERABLE, FM_VALID_FROM, FM_VALID_TO " &
                    "FROM FITR_MSTR " &
                    "WHERE FM_COY_ID = '" & companyID & "' AND FM_DELETED = 'N' "

            If strCode <> "" Then
                strsql &= "AND FM_FITR_CODE LIKE '%" & Common.Parse(strCode) & "%' "
            End If

            If dateFrom <> "" Then
                strsql &= "AND FM_VALID_FROM >= " & Common.ConvertDate(dateFrom) & " "
            End If

            If dateTo <> "" Then
                strsql &= "AND FM_VALID_TO <= " & Common.ConvertDate(dateTo & " 23:59:59.000") & " "
            End If

            strsql &= "ORDER BY FM_VALID_FROM "

            ds = objDB.FillDs(strsql)
            RetFITR = ds
        End Function

        Public Function AddFITRInfo(ByVal userID As String, ByVal companyID As String, ByVal strFITRCode As String, ByVal strDesc As String, ByVal decFITRR As Decimal, ByVal decFITRI As Decimal, ByVal dateFrom As String, ByVal dateTo As String, ByVal strMode As String, Optional ByVal strIndex As String = "")
            Dim strCoyId, strUserID, strSQL As String
            Dim ds As New DataSet
            Dim strAry(0) As String
            strCoyId = companyID
            strUserID = userID

            If strMode = "add" Then
                strSQL = "SELECT '*' FROM FITR_MSTR " &
                        "WHERE FM_FITR_CODE = '" & Common.Parse(strFITRCode) & "' AND FM_COY_ID = '" & strCoyId & "' " &
                        "AND FM_DELETED <> 'Y'"

                If objDb.Exist(strSQL) > 0 Then
                    Return -99
                End If

                strSQL = "INSERT INTO FITR_MSTR (FM_FITR_CODE, FM_FITR_DESC, FM_FITR_RECOVERABLE, FM_FITR_IRRECOVERABLE, FM_VALID_FROM, FM_VALID_TO, FM_DELETED, FM_ENT_BY, FM_ENT_DATETIME, FM_COY_ID) " &
                        "VALUES ('" & Common.Parse(strFITRCode) & "', '" & Common.Parse(strDesc) & "', " & decFITRR & ", " & decFITRI & ", " & Common.ConvertDate(dateFrom) & "," & Common.ConvertDate(dateTo & " 23:59:59.000") & ", 'N', '" & strUserID & "', " & Common.ConvertDate(Now) & ", '" & strCoyId & "')"

                Common.Insert2Ary(strAry, strSQL)
            Else

                strSQL = "UPDATE FITR_MSTR SET " &
                         "FM_FITR_DESC = '" & Common.Parse(strDesc) & "', " &
                         "FM_FITR_RECOVERABLE = " & decFITRR & ", " &
                         "FM_FITR_IRRECOVERABLE = " & decFITRI & ", " &
                         "FM_VALID_FROM = " & Common.ConvertDate(dateFrom) & ", " &
                         "FM_VALID_TO = " & Common.ConvertDate(dateTo & " 23:59:59.000") & ", " &
                         "FM_MOD_BY = '" & strUserID & "', " &
                         "FM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                         "WHERE FM_FITR_INDEX = '" & strIndex & "'"

                Common.Insert2Ary(strAry, strSQL)
            End If

            If objDb.BatchExecute(strAry) Then
                AddFITRInfo = WheelMsgNum.Save
            Else
                AddFITRInfo = WheelMsgNum.NotSave
            End If

        End Function

        Public Function delFITRInfo(ByVal userID As String, ByVal companyID As String, ByVal dtFITR As DataTable) As Integer
            Dim strAry(0) As String
            Dim strSql As String
            Dim drFITR As DataRow

            For Each drFITR In dtFITR.Rows
                strSql = "UPDATE FITR_MSTR SET " &
                        "FM_DELETED = 'Y', " &
                        "FM_MOD_BY = '" & userID & "', " &
                        "FM_MOD_DATETIME = " & Common.ConvertDate(Now) & " " &
                        "WHERE FM_FITR_INDEX = '" & drFITR("FITRIndex") & "'"
                Common.Insert2Ary(strAry, strSql)
            Next

            If objDb.BatchExecute(strAry) Then
                Return WheelMsgNum.Delete
            Else
                Return WheelMsgNum.NotDelete
            End If
        End Function

        'Jules 2018.04.12 - PAMB Scrum 1
        Public Function getAnalysisCodeTypeAhead(ByVal userID As String, ByVal companyID As String, ByVal strDeptCode As String, Optional ByVal compid As String = "", Optional ByVal strUserInput As String = "") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            If compid = "" Then
                compid = ConfigurationManager.AppSettings("DefIPPCompID")

                If compid = "" Then
                    compid = companyID
                End If
            End If

            If strUserInput = "*" Then
                strsql = "SELECT CONCAT(AC_ANALYSIS_CODE,':',AC_ANALYSIS_CODE_DESC) AS AC_ANALYSIS_CODE " _
                        & "FROM analysis_code " _
                        & "WHERE AC_B_COY_ID ='" & compid & "' AND AC_DEPT_CODE = '" & strDeptCode & "' AND AC_STATUS = 'O' "
            Else
                strsql = "SELECT CONCAT(AC_ANALYSIS_CODE,':',AC_ANALYSIS_CODE_DESC) AS AC_ANALYSIS_CODE " _
                        & "FROM analysis_code " _
                        & "WHERE AC_B_COY_ID ='" & compid & "' AND AC_DEPT_CODE = '" & strDeptCode & "' AND AC_STATUS = 'O' " _
                        & "AND AC_ANALYSIS_CODE LIKE '" & strUserInput & "%'"
            End If

            ds = objDb.FillDs(strsql)
            getAnalysisCodeTypeAhead = ds
        End Function

        Public Function getGLCodeAnalysisCodeMatrix(ByVal userID As String, ByVal companyID As String, ByVal glcode As String) As DataSet
            Dim strSql As String
            Dim ds As New DataSet

            strSql = "SELECT * FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & companyID & "' AND CBGCAC_B_GL_CODE = '" & glcode & "'"
            ds = objDb.FillDs(strSql)

            Return ds
        End Function
        'End modification.

        'Jules 2018.07.11 - Allow search by Description; ignore L6 (tax code) and L7 (cost centre)
        'Zulham 16/05/2018 - PAMB
        Public Function getAnalysisCode(ByRef analysisCode As String, Optional ByRef analysisCodeType As String = "", Optional ByRef analysisDesc As String = "") As DataSet
            Dim strSql As String = ""
            Dim ds As New DataSet

            If Not analysisCode = "" And Not analysisCodeType = "" And analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_analysis_code ='" & analysisCode & "' and ac_dept_code ='" & analysisCodeType & "' and ac_dept_code not in ('L6','L7')"
            ElseIf Not analysisCode = "" And analysisCodeType = "" And analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_analysis_code ='" & analysisCode & "' and ac_dept_code not in ('L6','L7')"
            ElseIf analysisCode = "" And Not analysisCodeType = "" And analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_dept_code ='" & analysisCodeType & "' and ac_dept_code not in ('L6','L7')"
            ElseIf Not analysisCode = "" And Not analysisCodeType = "" And Not analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_analysis_code ='" & analysisCode & "' and ac_dept_code ='" & analysisCodeType & "' and ac_analysis_code_desc LIKE '%" & analysisDesc & "%' and ac_dept_code not in ('L6','L7')"
            ElseIf Not analysisCode = "" And analysisCodeType = "" And Not analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_analysis_code ='" & analysisCode & "' and ac_analysis_code_desc LIKE '%" & analysisDesc & "%' and ac_dept_code not in ('L6','L7')"
            ElseIf analysisCode = "" And Not analysisCodeType = "" And analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_dept_code ='" & analysisCodeType & "' and ac_analysis_code_desc LIKE '%" & analysisDesc & "%' and ac_dept_code not in ('L6','L7')"
            ElseIf analysisCode = "" And analysisCodeType = "" And Not analysisDesc = "" Then
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_analysis_code_desc LIKE '%" & analysisDesc & "%' and ac_dept_code not in ('L6','L7')"
            Else
                strSql = "select ac_analysis_code, ac_analysis_code_desc, ac_dept_code from analysis_code where ac_dept_code not in ('L6','L7')"
            End If

            ds = objDb.FillDs(strSql)
            Return ds

        End Function
        Public Function updateAnalysisCode(ByVal analysisCode As String, ByVal analysisCodeDesc As String, ByVal codeType As String, ByVal oldAnalysisCode As String, ByVal oldCodeType As String) As Integer

            Dim strsql As String
            Dim strAry(0) As String

            'Jules 2018.07.11 - Do not allow user to modify analysis code.
            'strsql = "UPDATE analysis_code SET " &
            '             "AC_ANALYSIS_CODE = '" & Common.Parse(analysisCode) & "', " &
            '             "AC_ANALYSIS_CODE_DESC = '" & analysisCodeDesc & "', " &
            '             "AC_DEPT_CODE = '" & codeType & "', " &
            '             "AC_LAST_UPDATE_DATE = NOW() " &
            '             "WHERE AC_ANALYSIS_CODE = '" & oldAnalysisCode & "' AND AC_DEPT_CODE ='" & oldCodeType & "'"
            strsql = "UPDATE analysis_code SET " &
                         "AC_ANALYSIS_CODE_DESC = '" & analysisCodeDesc & "', " &
                         "AC_DEPT_CODE = '" & codeType & "', " &
                         "AC_LAST_UPDATE_DATE = NOW() " &
                         "WHERE AC_ANALYSIS_CODE = '" & oldAnalysisCode & "' AND AC_DEPT_CODE ='" & oldCodeType & "'"

            Common.Insert2Ary(strAry, strsql)

            If objDb.BatchExecute(strAry) Then
                updateAnalysisCode = WheelMsgNum.Save
            Else
                updateAnalysisCode = WheelMsgNum.NotSave
            End If

        End Function
        Public Function addAnalysisCode(ByVal userID As String, ByVal companyID As String, ByVal analysisCode As String, ByVal analysisCodeDesc As String, ByVal codeType As String) As Integer

            Dim strsql As String
            Dim strAry(0) As String
            'Zulham 04122018
            strsql = "Insert into analysis_code (AC_B_COY_ID,AC_ANALYSIS_CODE,AC_ANALYSIS_CODE_DESC,AC_DEPT_CODE,AC_LAST_UPDATE_DATE, AC_STATUS) VALUES (" &
                        "'" & Common.Parse(companyID.ToString.ToUpper) & "', " &
                        "'" & Common.Parse(analysisCode) & "', " &
                        "'" & analysisCodeDesc & "', " &
                        " '" & codeType & "', " &
                        "NOW(), 'O' )"

            Common.Insert2Ary(strAry, strsql)

            If objDb.BatchExecute(strAry) Then
                addAnalysisCode = WheelMsgNum.Save
            Else
                addAnalysisCode = WheelMsgNum.NotSave
            End If

        End Function

        Public Function deleteAnalysisCode(ByVal userID As String, ByVal companyID As String, ByVal analysisCode As String, ByVal codeType As String) As Integer

            Dim strsql As String
            Dim strAry(0) As String

            strsql = "DELETE FROM ANALYSIS_CODE WHERE " &
                     "AC_ANALYSIS_CODE = '" & analysisCode & "' AND AC_DEPT_CODE ='" & codeType & "' AND AC_B_COY_ID='" & Common.Parse(companyID.ToString.ToUpper) & "'"

            Common.Insert2Ary(strAry, strsql)

            If objDb.BatchExecute(strAry) Then
                deleteAnalysisCode = WheelMsgNum.Delete
            Else
                deleteAnalysisCode = WheelMsgNum.NotDelete
            End If

        End Function

        Public Function addGLCodeMatrix(ByVal userID As String, ByVal companyID As String, ByVal GLCode As String) As Integer

            Dim strsql As String
            Dim strAry(0) As String

            strsql = "Insert into company_b_gl_code_analysis_code (CBGCAC_B_COY_ID, CBGCAC_B_GL_CODE, CBGCAC_STATUS) VALUES (" &
                        "'" & Common.Parse(companyID.ToString.ToUpper) & "', " &
                        "'" & Common.Parse(GLCode) & "', " &
                        "'O') "

            Common.Insert2Ary(strAry, strsql)

            If objDb.BatchExecute(strAry) Then
                addGLCodeMatrix = WheelMsgNum.Save
            Else
                addGLCodeMatrix = WheelMsgNum.NotSave
            End If

        End Function

        Public Sub updateCodeAnalysisStatus(ByVal userID As String, ByVal companyID As String, ByVal itemSelected As Boolean, ByVal index As Integer, ByVal glCode As String)

            Dim strsql As String
            Dim strAry(0) As String

            strsql = "update company_b_gl_code_analysis_code set cbgcac_analysis_code" & index + 1 & " = '" & IIf(itemSelected, "M", "P") & "' where cbgcac_b_gl_code = '" & glCode & "' and cbgcac_b_coy_id = '" & companyID & "'"

            Common.Insert2Ary(strAry, strsql)

            objDb.BatchExecute(strAry)

        End Sub

        Public Function getGLAnalysisCode(ByVal userID As String, ByVal companyID As String, ByRef glCode As String) As DataSet
            Dim strSql As String = ""
            Dim ds As New DataSet

            If glCode = "" Then
                strSql = "SELECT cbgcac_b_gl_code, cbgcac_analysis_code1, cbgcac_analysis_code2, cbgcac_analysis_code3, cbgcac_analysis_code4, " &
                        "cbgcac_analysis_code5, cbgcac_analysis_code6, cbgcac_analysis_code7, cbgcac_analysis_code8, cbgcac_analysis_code9 " &
                        "From company_b_gl_code_analysis_code " &
                        "Where cbgcac_b_coy_id = '" & companyID & "' " &
                        "ORDER BY cbgcac_b_gl_code"
            Else
                strSql = "SELECT cbgcac_b_gl_code, cbgcac_analysis_code1, cbgcac_analysis_code2, cbgcac_analysis_code3, cbgcac_analysis_code4, " &
                        "cbgcac_analysis_code5, cbgcac_analysis_code6, cbgcac_analysis_code7, cbgcac_analysis_code8, cbgcac_analysis_code9 " &
                        "From company_b_gl_code_analysis_code " &
                        "Where cbgcac_b_coy_id = '" & companyID & "' and cbgcac_b_gl_code ='" & glCode & "'" &
                        "ORDER BY cbgcac_b_gl_code"
            End If

            ds = objDb.FillDs(strSql)
            Return ds

        End Function

        Public Function deleteGLCode(ByVal userID As String, ByVal companyID As String, ByVal glCode As String) As Integer

            Dim strsql As String
            Dim strAry(0) As String

            strsql = "DELETE FROM company_b_gl_code_analysis_code WHERE " &
                     "cbgcac_b_gl_code = '" & glCode & "' and cbgcac_b_coy_id ='" & Common.Parse(companyID.ToString.ToUpper) & "'"

            Common.Insert2Ary(strAry, strsql)

            If objDb.BatchExecute(strAry) Then
                deleteGLCode = WheelMsgNum.Delete
            Else
                deleteGLCode = WheelMsgNum.NotDelete
            End If

        End Function

        Public Function formatBackslash(ByRef docNo As String) As String
            If docNo.Contains("\") Then
                Dim str As String() = docNo.Split("\")
                For i As Integer = 0 To str.Length - 1
                    If Not i = str.Length - 1 Then
                        formatBackslash += str(i) & "\\"
                    Else
                        formatBackslash += str(i)
                    End If
                Next
            Else
                formatBackslash = docNo
            End If
            Return formatBackslash
        End Function

        Public Function checkApprovalLimit(ByVal userID As String, ByVal companyID As String, ByVal isResident As String, ByVal invoiceTotal As Decimal) As Boolean

            Dim sqlCheckAO, sqlCheckFO, sqlCheckTaxOfficer As String

            'Zulham 11072018 - PAMB
            'Check for user's role. if it's finance teller, skip ao validator
            'role: E2P Teller(F)
            Dim objUsers As New Users
            Dim boolFinTeller = objUsers.checkUserFixedRole(userID, companyID, "'IPP Officer(F)'")

            If Not boolFinTeller Then
                sqlCheckAO = "(SELECT DISTINCT UM_USER_ID, UM_INVOICE_APP_LIMIT " &
                    "FROM approval_grp_mstr, approval_grp_buyer, approval_grp_ao, user_mstr " &
                    "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX " &
                    "AND AGM_TYPE='E2P' " &
                    "AND AGM_RESIDENT = '" & isResident & "' " &
                    "AND AGB_BUYER = '" & userID & "' " &
                    "AND AGM_COY_ID = '" & companyID & "' " &
                    "AND AGA_GRP_INDEX = AGB_GRP_INDEX " &
                    "AND (AGA_AO = UM_USER_ID " &
                    "OR AGA_A_AO = UM_USER_ID) " &
                    "AND UM_INVOICE_APP_LIMIT >= " & invoiceTotal & " " &
                    "ORDER BY AGA_SEQ)"

                If objDb.Exist(sqlCheckAO) = 0 Then
                    Return False
                    Exit Function
                End If
            End If

            sqlCheckTaxOfficer = "(SELECT DISTINCT UM_USER_ID, UM_INVOICE_APP_LIMIT " &
                        "From approval_grp_mstr, approval_grp_buyer, approval_grp_fo, user_mstr " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX " &
                        "AND AGM_TYPE='E2P' " &
                        "AND AGM_RESIDENT = '" & isResident & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & companyID & "' " &
                        "AND AGFO_GRP_INDEX = AGB_GRP_INDEX " &
                        "AND (AGFO_FO = UM_USER_ID " &
                        "OR AGFO_A_FO = UM_USER_ID) " &
                        "AND UM_INVOICE_APP_LIMIT >= " & invoiceTotal & " " &
                        "AND AGFO_SEQ = 1 " &
                        "ORDER BY AGFO_SEQ)"

            If objDb.Exist(sqlCheckTaxOfficer) = 0 Then
                Return False
                Exit Function
            End If

            sqlCheckFO = "(SELECT DISTINCT UM_USER_ID, UM_INVOICE_APP_LIMIT " &
                        "From approval_grp_mstr, approval_grp_buyer, approval_grp_fo, user_mstr " &
                        "WHERE AGB_GRP_INDEX = AGM_GRP_INDEX " &
                        "AND AGM_TYPE='E2P' " &
                        "AND AGM_RESIDENT = '" & isResident & "' " &
                        "AND AGB_BUYER = '" & userID & "' " &
                        "AND AGM_COY_ID = '" & companyID & "' " &
                        "AND AGFO_GRP_INDEX = AGB_GRP_INDEX " &
                        "AND (AGFO_FO = UM_USER_ID " &
                        "OR AGFO_A_FO = UM_USER_ID) " &
                        "AND UM_INVOICE_APP_LIMIT >= " & invoiceTotal & " " &
                        "AND AGFO_SEQ <> 1 " &
                        "ORDER BY AGFO_SEQ)"

            If objDb.Exist(sqlCheckFO) = 0 Then
                Return False
                Exit Function
            End If

            Return True

        End Function
        'Zulham 2102018 - PAMB
        Public Function canApprove(ByVal userID As String, ByVal companyID As String, ByVal invoiceTotal As Decimal, Optional ByVal exchangeRate As Double = 1.0) As Boolean
            Dim checkLimit As String
            checkLimit = "SELECT COUNT(um_invoice_app_limit) " &
                        "From user_mstr " &
                        "WHERE um_coy_id = '" & companyID & "' " &
                        "And um_user_id = '" & userID & "' " &
                        "GROUP BY um_invoice_app_limit " &
                        "HAVING um_invoice_app_limit >= '" & invoiceTotal * exchangeRate & "'"

            If objDb.Exist(checkLimit) = 0 Then
                Return False
                Exit Function
            Else
                Return True
            End If

        End Function

        'Zulham 13082018 - PAMB
        Function getE2PApprFlow(ByVal userID As String, ByVal companyID As String, ByVal intInvoiceNo As String, ByVal strCompanyId As String) As DataSet
            Dim objDb As New EAD.DBCom
            Dim strSql, strsql2, strsql3 As String
            Dim ds As DataSet
            Dim strCoyId As String = companyID
            Dim strDeptIdx As String
            Dim strInvStatus As String
            Dim currentfaaction As Integer
            Dim currentseq As String
            Dim role As String
            Dim blnAO As Boolean
            Dim strAOAction As String

            Dim strIPPOF As String
            Dim blnIPPOF As Boolean
            Dim objUsers As New Users
            Dim strUserID As String

            strIPPOF = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_Officer)
            strIPPOF = "'" & Replace(strIPPOF, strIPPOF, "IPP Officer") & "'"
            blnIPPOF = objUsers.checkUserFixedRole(userID, companyID, strIPPOF)

            'get AOs number
            Dim aoCount As Integer = 0
            aoCount = objDb.GetVal("SELECT COUNT(*)
                                FROM finance_approval
                                WHERE fa_invoice_index = '" & intInvoiceNo & "'
                                AND fa_aga_type = 'ao' ")
            'Zulham 04012019
            strSql = "SELECT FA.*,CONCAT(UMA.UM_USER_NAME,IF(um2.um_user_name IS NULL, '','/'),IF(um2.um_user_name IS NULL, '', um2.um_user_name)) AS AO_NAME, " &
                             "CONCAT(UMA.UM_USER_NAME,IF(um2.um_user_name IS NULL, '','/'),IF(um2.um_user_name IS NULL, '', um2.um_user_name)) AS 'Action', " &
                             "FA.FA_SEQ + 1 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "LEFT OUTER JOIN USER_MSTR UM2 ON FA.FA_A_AO=UM2.UM_USER_ID AND UM2.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "' " &
                             "AND fa_aga_type = 'AO' " &
                                 "UNION ALL " &
                             "SELECT FA.*,CONCAT(UMA.UM_USER_NAME,IF(um2.um_user_name IS NULL, '','/'),IF(um2.um_user_name IS NULL, '', um2.um_user_name)) AS AO_NAME, " &
                             "CONCAT(UMA.UM_USER_NAME,IF(um2.um_user_name IS NULL, '','/'),IF(um2.um_user_name IS NULL, '', um2.um_user_name)) AS 'Action', " &
                             "FA.FA_SEQ + 3 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "LEFT OUTER JOIN USER_MSTR UM2 ON FA.FA_A_AO=UM2.UM_USER_ID AND UM2.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "' " &
                             "AND fa_aga_type = 'FO' " &
                                  "UNION ALL " &
                             "SELECT FA.*,CONCAT(UMA.UM_USER_NAME,IF(um2.um_user_name IS NULL, '','/'),IF(um2.um_user_name IS NULL, '', um2.um_user_name)) AS AO_NAME, " &
                             "CONCAT(UMA.UM_USER_NAME,IF(um2.um_user_name IS NULL, '','/'),IF(um2.um_user_name IS NULL, '', um2.um_user_name)) AS 'Action', " &
                             "FA.FA_SEQ + 3 AS FA_SEQ2 " &
                             "FROM FINANCE_APPROVAL FA " &
                             "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "LEFT OUTER JOIN USER_MSTR UM2 ON FA.FA_A_AO=UM2.UM_USER_ID AND UM2.UM_COY_ID='" & companyID & "', INVOICE_MSTR " &
                             "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & companyID & "' " &
                             "AND fa_aga_type = 'FM' " &
                                  "UNION ALL " &
                             "SELECT im_invoice_index,'' AS FA_AGA_TYPE,im_created_by AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " &
                             "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " &
                             "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                             "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                             "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,um_user_name AS 'Action',1 AS FA_SEQ2 " &
                             "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                             "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " &
                                    "UNION ALL " &
                            "SELECT im_invoice_index,'' AS FA_AGA_TYPE,im_prcs_recv_id AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 4 AS FA_SEQ, " &
                            "im_prcs_recv_id AS FA_ACTIVE_AO, im_prcs_recv_upd_date AS FA_ACTION_DATE, " &
                            "(SELECT DISTINCT fa_ao_action FROM finance_approval " &
                            "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Received Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " &
                            "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,um_user_name AS 'Action'," & aoCount & "+2 AS FA_SEQ2 " &
                            "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_recv_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & companyID & "' " &
                            "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_recv_upd_date IS NOT NULL " &
                            "ORDER BY FA_SEQ2 "

            ds = objDb.FillDs(strSql)
            Return ds
        End Function

        'Zulham 09012018
        Public Function getIPPApprovalListForExcel(ByVal userID As String, ByVal companyID As String, ByVal docno As String, ByVal doctype As String, ByVal startdt As String, ByVal enddt As String, ByVal vendor As String) As DataSet
            Dim ds As New DataSet
            Dim sql As String

            sql = "SELECT Vendor_Code, Vendor_Name, GL_Code, GL_Description, Currency, Payment_Amount, Document_No, Payment_Description, IFNULL(Fund_Type,'') 'Fund_Type', IFNULL(Cost_Center,'') 'Cost_Center', " &
                  "IFNULL(Project_Code,'') 'Project_Code', IFNULL(Person_Code,'') 'Person_Code', STATUS FROM " &
                  "(SELECT ic_business_reg_no 'Vendor_Code', ic_coy_name 'Vendor_Name', id_b_gl_code 'GL_Code', cbg_b_gl_desc 'GL_Description', im_currency_code 'Currency', " &
                  "id_unit_cost + id_gst_value 'Payment_Amount', im_invoice_no 'Document_No', id_product_desc 'Payment_Description', " &
                  "(SELECT CONCAT(IFNULL(ac_analysis_code_desc,''),' : ',ac_analysis_code) FROM analysis_code WHERE ac_analysis_code = id_analysis_code1 AND ac_dept_code = 'L1') 'Fund_Type', " &
                  "(SELECT CONCAT(ac_analysis_code_desc,' : ',ac_analysis_code) FROM analysis_code WHERE ac_analysis_code = id_analysis_code1 AND ac_dept_code = 'L7') 'Cost_Center', " &
                  "(SELECT CONCAT(ac_analysis_code_desc,' : ',ac_analysis_code) FROM analysis_code WHERE ac_analysis_code = id_analysis_code1 AND ac_dept_code = 'L8') 'Project_Code', " &
                  "(SELECT CONCAT(ac_analysis_code_desc,' : ',ac_analysis_code) FROM analysis_code WHERE ac_analysis_code = id_analysis_code1 AND ac_dept_code = 'L9') 'Person_Code', " &
                  "status_remark 'Status' " &
                  "FROM invoice_mstr  " &
                  "LEFT JOIN finance_approval ON im_invoice_index = fa_invoice_index " &
                  "LEFT JOIN ipp_company ON im_s_coy_id = ic_index " &
                  "JOIN invoice_Details ON im_s_coy_id = id_S_coy_id AND im_invoice_no = id_invoice_no " &
                  "LEFT JOIN company_b_gl_code ON id_b_gl_code = cbg_b_gl_code AND cbg_b_coy_id = '" & companyID & "' " &
                  "JOIN status_mstr ON status_no = im_invoice_status AND status_type = 'inv' " &
                  "WHERE im_b_coy_id='" & companyID & "' " &
                  "AND im_invoice_status IN ('16') " &
                  "AND (fa_ao = '" & userID & "' OR fa_a_ao = '" & userID & "' )  " &
                  "AND fa_ao_action = (fa_seq - 1)"

            If docno <> "" Then
                sql &= " AND IM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                sql &= " AND IM_INVOICE_TYPE = '" & doctype & "'"
            End If
            If startdt <> "" And enddt <> "" Then
                sql &= " AND IM_SUBMIT_DATE >= " & Common.ConvertDate(startdt) & " AND IM_SUBMIT_DATE <= " & Common.ConvertDate(enddt)
            End If
            If vendor <> "" Then
                sql &= " AND IM_S_COY_NAME LIKE '%" & vendor & "%'"
            End If

            sql &= ")zzz"

            ds = objDb.FillDs(sql)

            Return ds
        End Function
        'Zulham 17012019
        Public Sub updateFinanceApproval(ByVal strUserID As String)

            Dim ds As New DataSet
            Dim strAryQuery(0) As String
            Dim sql As String = ""
            sql = "SELECT DISTINCT fa_invoice_index, (SELECT fa_seq FROM finance_approval WHERE fa_invoice_index = im_invoice_index AND fa_action_date IS NULL AND fa_ao = '" & strUserID & "') 'seq'
                    FROM invoice_mstr, finance_approval
                    WHERE im_invoice_index = fa_invoice_index
                    AND fa_ao_action <> (SELECT fa_seq FROM finance_approval WHERE fa_invoice_index = im_invoice_index AND fa_action_date IS NULL AND fa_ao = '" & strUserID & "')
                    AND im_invoice_stAtus = 12"
            ds = objDb.FillDs(sql)
            If Not ds.Tables(0).Rows.Count = 0 Then
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    sql = "update finance_approval 
                           set fa_ao_action ='" & ds.Tables(0).Rows(i).Item(1) & "'
                           where fa_invoice_index = " & ds.Tables(0).Rows(i).Item(0)
                    Common.Insert2Ary(strAryQuery, sql)

                    sql = "update finance_approval
                           set fa_active_ao = '" & strUserID & "',
                           fa_action_date = NOW(),
                           fa_ao_remark = 'Fin Approval: Submitted'
                           where fa_invoice_index =" & ds.Tables(0).Rows(i).Item(0) & "
                           and fa_ao = '" & strUserID & "'
                           and fa_seq = " & ds.Tables(0).Rows(i).Item(1)
                    Common.Insert2Ary(strAryQuery, sql)

                    sql = "INSERT INTO ipp_trans_log (itl_invoice_index,itl_performed_by,itl_user_id,itl_trans_date,itl_remarks) " &
                         "VALUES(" & ds.Tables(0).Rows(i).Item(0) & ",'Finance Verifier','" & Common.Parse(strUserID) & "',Now()" &
                         ",'Fin Approval: Submitted')"
                    Common.Insert2Ary(strAryQuery, sql)

                    objDb.BatchExecute(strAryQuery)
                Next
            End If

            ''Zulham 25012019
            'sql = "update invoice_details
            '       set id_gst_output_tax_code ='N/A'
            '       where id_gst_output_tax_Code like '%select%' "
            'Common.Insert2Ary(strAryQuery, sql)

        End Sub

        Public Sub deleteMultiInvoiceHeader(ByVal userID As String, ByVal companyID As String, ByVal invoiceNo As String, ByVal companyName As String)
            Dim sql As String = ""

            sql = "Delete 
                   from invoice_mstr
                   where im_invoice_no = '" & invoiceNo & "'
                   and im_s_coy_name = '" & companyName & "'
                   and im_created_by = '" & userID & "'"

            objDb.Execute(sql)

        End Sub
    End Class
End Namespace