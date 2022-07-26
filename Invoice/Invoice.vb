Imports System
Imports System.Web
Imports AgoraLegacy
Imports SSO.Component

Namespace AgoraNEO

    Public Class InvValue

        Public LINE As String
        Public PRINDEX As String
        Public doc_num As String
        Public pm As String
        Public DO_NO As String
        Public adds As String
        Public phone As String
        Public vphone As String
        Public email As String

        Public cur As String
        Public st As String
        Public sm As String
        Public pt As String
        Public GRN_NO As String
        Public po_line As String
        Public po_no As String
        Public Inv_no As String
        Public Vcom_id As String
        Public v_com_name As String
        Public po_index As String
        Public B_com_id As String
        Public pay_date As Date
        Public remark As String
        Public create_by As String
        Public create_on As String
        Public inv_status As String
        Public pay_no As String
        Public your_ref As String
        Public our_ref As String
        Public inv_prefix As String
        Public summit_fo As String
        Public exchange_rate As String
        Public remark_finance As String
        Public print As String
        Public folder As String
        Public approved_date_fm As String
        Public total_amount As Double
        Public download_data As String
        Public external_ind As String
        Public ref_no As String
        Public inv_total As String
        Public REFERENCE_NO As String
        Public Paid As Double
        Public Ordered_amount As Double
        Public tax_reg_no As String
        Public V_attn As String
        Public ven_add As String
        Public bussiness_reg As String
        Public BComName As String
        Public billingMethod As String
        Public invoiceIndex As String
        Public deliveryTerm As String
        Public balShip As Double
        Public payDay As String
        Public VendorCode As String 'Jules 2018.10.17
    End Class


    Public Class Invoice
        Dim objDb As New EAD.DBCom
        Dim objUsr As New Users
        Public Function get_venlist(ByVal rfq_name As String, ByVal list_id As String) As DataSet
            Dim ds As DataSet

            Dim strsql As String = "SELECT RCDLD_V_Coy_ID FROM RFQ_VEN_DISTR_LIST_DETAIL INNER JOIN COMPANY_MSTR ON RCDLD_V_Coy_ID = CM_COY_ID WHERE  " &
                                    " RVDLD_LIST_INDEX ='" & list_id & "' AND CM_STATUS = 'A' "
            ds = objDb.FillDs(strsql)

            Return ds
        End Function
        Public Function get_invoiceview(ByVal userID As String, ByVal status As String, ByVal b_com_id As String, ByVal inv_no As String) As String

            Dim ds As DataSet
            Dim strsql As String
            Dim strTemp As String
            Dim companyID As String = objUsr.getCompanyID(userID)

            strsql = "select IM_INVOICE_NO,IM_CREATED_ON,POM_PO_NO,POM_B_COY_ID,POM_S_COY_ID,IM_INVOICE_STATUS," &
                    " POM_BILLING_METHOD,(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=IM_INVOICE_STATUS AND STATUS_TYPE='Inv') as STATUS_DESC, " &
                    " CM_COY_NAME, POM_CURRENCY_CODE, FORMAT(IM_INVOICE_TOTAL,2) AS IM_INVOICE_TOTAL" &
                    " FROM INVOICE_MSTR INNER JOIN PO_MSTR ON POM_PO_INDEX=IM_PO_Index " &
                    " LEFT JOIN COMPANY_MSTR ON CM_COY_ID=IM_B_COY_ID " &
                    " WHERE POM_S_COY_ID='" & companyID & "' "
            ' " and IM_B_COY_ID = and IM_INVOICE_NO = "

            'meilai

            If status <> "" Then
                strsql = strsql & " And IM_INVOICE_STATUS in(" & status & ")"
            End If

            If b_com_id <> "" Then
                strTemp = Common.BuildWildCard(b_com_id)
                strsql = strsql & " and CM_COY_NAME" & Common.ParseSQL(strTemp)
            End If


            If inv_no <> "" Then
                strTemp = Common.BuildWildCard(inv_no)
                strsql = strsql & "  and IM_INVOICE_NO" & Common.ParseSQL(strTemp)
            End If

            'ds = objDb.FillDs(strsql)

            Return strsql

        End Function

        'Public Function get_unInvItemEn(ByVal doc_no As String, ByVal bcom_name As String) As DataSet

        '    Dim ds As DataSet
        '    Dim grn_uninv As Integer = GRNStatus.Uninvoice
        '    Dim strsql As String = ""
        '    'strsql = "select distinct POM_PO_NO,POM_PO_STATUS,POM_B_COY_ID,POM_PO_INDEX,CDM_DO_No,CDM_GRN_NO,POM_BILLING_METHOD," & _
        '    '                        " POM_PO_DATE,CM_COY_ID,CM_COY_NAME,POM_CURRENCY_CODE, (POM_SHIP_AMT - POM_ACC_SHIP_AMT) AS BALSHIP, " & _
        '    '                        " (SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_DESC=POM_PAYMENT_TERM AND CODE_CATEGORY='PT') AS PAY_DAY " & _
        '    '                        " FROM COMPANY_MSTR,PO_MSTR,COMPANY_DOC_MATCH " & _
        '    '                        " WHERE POM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and CDM_B_COY_ID=POM_B_COY_ID AND CDM_PO_NO=POM_PO_NO "
        '    strsql = "select distinct POM_PO_NO,POM_PO_STATUS,POM_B_COY_ID,POM_PO_INDEX,'' AS CDM_DO_No,'' AS CDM_GRN_NO," & _
        '            "CDM_DO_No AS 'DO Number', CDM_GRN_NO AS 'GRN Number' ,POM_BILLING_METHOD," & _
        '            " POM_PO_DATE,CM_COY_ID,CM_COY_NAME,POM_CURRENCY_CODE, (POM_SHIP_AMT - POM_ACC_SHIP_AMT) AS BALSHIP, " & _
        '            " (SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_DESC=POM_PAYMENT_TERM AND CODE_CATEGORY='PT') AS PAY_DAY " & _
        '            " FROM COMPANY_MSTR,PO_MSTR,COMPANY_DOC_MATCH " & _
        '            " WHERE POM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and CDM_B_COY_ID=POM_B_COY_ID AND CDM_PO_NO=POM_PO_NO "
        '    If bcom_name <> "" Then

        '        strsql = strsql & " and POM_B_COY_ID in(select distinct CM_COY_ID from COMPANY_MSTR where CM_COY_NAME " & Common.ParseSQL(bcom_name) & ")"
        '    End If


        '    'If doc_no <> "" Then
        '    '    strsql = strsql & "  and (CDM_INVOICE_NO" & Common.ParseSQL(doc_no) & " or " & _
        '    '    " POM_PO_NO" & Common.ParseSQL(doc_no) & " or CDM_DO_No" & Common.ParseSQL(doc_no) & " " & _
        '    '    " or CDM_GRN_NO" & Common.ParseSQL(doc_no) & ")"
        '    'End If

        '    If doc_no <> "" Then
        '        strsql = strsql & " and (POM_PO_NO" & Common.ParseSQL(doc_no) & ")"
        '    End If

        '    'strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" & _
        '    '                       " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" & _
        '    '                        " AND POM_PO_STATUS <>'4'" & _
        '    '                        " AND POM_BILLING_METHOD IS NOT NULL " & _
        '    '                        " AND CDM_PO_NO = POM_PO_NO " & _
        '    '                        " AND POM_BILLING_METHOD<>'' " & _
        '    '                        " aND CDM_INVOICE_NO IS NULL " & _
        '    '                        " AND ( " & _
        '    '                        "((POM_BILLING_METHOD='FPO'  AND CDM_GRN_NO IS NOT NULL " & _
        '    '                        " AND (SELECT MAX(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO )" & _
        '    '                        "= (SELECT COUNT(*) FROM PO_DETAILS " & _
        '    '                        " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " & _
        '    '                        " AND( POD_DELIVERED_QTY <> 0 or POD_ORDERED_QTY=POD_DELIVERED_QTY ) AND POD_PO_NO = POM_PO_NO  " & _
        '    '                        " and POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO)))" & _
        '    '                        " or " & _
        '    '                        " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN" & _
        '    '                        " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " & _
        '    '                        " DOM_DO_STATUS IN(2) )and " & _
        '    '                        " 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " & _
        '    '                        " AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " & _
        '    '                        " AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND DOD_DO_QTY > 0 " & _
        '    '                        " AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) )" & _
        '    '                        " OR " & _
        '    '                        " (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " & _
        '    '                        " 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " & _
        '    '                        " AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND AND GM_B_COY_ID=GD_B_COY_ID " & _
        '    '                        " AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " & _
        '    '                        " AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

        '    'Michelle (6/9/2007) - To replace 'MAX(POD_PO_LINE)' with 'COUNT(POD_PO_LINE)' as the maximum doesn't indicate the no. of po line records
        '    'strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" & _
        '    '                       " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" & _
        '    '                        " AND POM_PO_STATUS <>'4'" & _
        '    '                        " AND POM_BILLING_METHOD IS NOT NULL " & _
        '    '                        " AND CDM_PO_NO = POM_PO_NO " & _
        '    '                        " AND POM_BILLING_METHOD<>'' " & _
        '    '                        " aND CDM_INVOICE_NO IS NULL " & _
        '    '                        " AND ( " & _
        '    '                        "((POM_BILLING_METHOD='FPO' AND CDM_GRN_NO IS NOT NULL " & _
        '    '                        " AND (SELECT MAX(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID )" & _
        '    '                        "= (SELECT COUNT(*) FROM PO_DETAILS " & _
        '    '                        " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " & _
        '    '                        " AND POD_PO_NO = POM_PO_NO  " & _
        '    '                        " And POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO)))" & _
        '    '                        " or " & _
        '    '                        " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN" & _
        '    '                        " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " & _
        '    '" DOM_DO_STATUS IN(2) )and " & _
        '    '" 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " & _
        '    '" AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " & _
        '    '" AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND DOD_DO_QTY > 0 " & _
        '    '" AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) )" & _
        '    '" OR " & _
        '    '" (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " & _
        '    '" 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " & _
        '    '" AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND GM_B_COY_ID=GD_B_COY_ID " & _
        '    '" AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " & _
        '    '" AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

        '    ' Michelle (CR0006) - To exlcude where PO has been cancelled ie. POM_PO_STATUS = 5
        '    strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" & _
        '                           " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" & _
        '                            " AND POM_PO_STATUS <>'4' AND POM_PO_STATUS <> '5' " & _
        '                            " AND POM_BILLING_METHOD IS NOT NULL " & _
        '                            " AND CDM_PO_NO = POM_PO_NO " & _
        '                            " AND POM_BILLING_METHOD<>'' " & _
        '                            " aND CDM_INVOICE_NO IS NULL " & _
        '                            " AND ( " & _
        '                            "((POM_BILLING_METHOD='FPO' AND CDM_GRN_NO IS NOT NULL " & _
        '                            " AND (SELECT COUNT(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID )" & _
        '                            "= (SELECT COUNT(*) FROM PO_DETAILS " & _
        '                            " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " & _
        '                            " AND POD_PO_NO = POM_PO_NO  " & _
        '                            " And POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO)))" & _
        '                            " or " & _
        '                            " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN" & _
        '                            " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " & _
        '                            " DOM_DO_STATUS IN(2) )and " & _
        '                            " 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " & _
        '                            " AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " & _
        '                            " AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND DOD_DO_QTY > 0 " & _
        '                            " AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) )" & _
        '                            " OR " & _
        '                            " (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " & _
        '                            " 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " & _
        '                            " AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND GM_B_COY_ID=GD_B_COY_ID " & _
        '                            " AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " & _
        '                            " AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

        '    ds = objDb.FillDs(strsql)
        '    get_unInvItemEn = ds

        'End Function

        Public Function get_unInvItemEn(ByVal userID As String, ByVal doc_no As String, ByVal bcom_name As String) As String

            Dim ds As DataSet
            Dim grn_uninv As Integer = GRNStatus.Uninvoice
            Dim strsql As String = ""
            Dim strsql1 As String = ""
            Dim companyID As String = objUsr.getCompanyID(userID)

            strsql = "select distinct POM_PO_NO,POM_PO_STATUS,POM_B_COY_ID,POM_PO_INDEX,'' AS CDM_DO_No,'' AS CDM_GRN_NO," &
                    "CDM_DO_No AS 'DO_Number', CDM_GRN_NO AS 'GRN_Number' ,POM_BILLING_METHOD," &
                    " POM_PO_DATE,CM_COY_ID,CM_COY_NAME,POM_CURRENCY_CODE, (POM_SHIP_AMT - POM_ACC_SHIP_AMT) AS BALSHIP, " &
                    " (SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_DESC=POM_PAYMENT_TERM AND CODE_CATEGORY='PT') AS PAY_DAY " &
                    " FROM COMPANY_MSTR,PO_MSTR,COMPANY_DOC_MATCH " &
                    " WHERE POM_S_COY_ID='" & companyID & "' and CDM_B_COY_ID=POM_B_COY_ID AND CDM_PO_NO=POM_PO_NO "
            If bcom_name <> "" Then

                strsql = strsql & " and POM_B_COY_ID in(select distinct CM_COY_ID from COMPANY_MSTR where CM_COY_NAME " & Common.ParseSQL(bcom_name) & ")"
            End If

            If doc_no <> "" Then
                strsql = strsql & " and (POM_PO_NO" & Common.ParseSQL(doc_no) & ")"
            End If

            ' Michelle (CR0006) - To exlcude where PO has been cancelled ie. POM_PO_STATUS = 5
            strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" &
                   " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" &
                    " AND POM_PO_STATUS <>'4' AND POM_PO_STATUS <> '5' " &
                    " AND POM_BILLING_METHOD IS NOT NULL " &
                    " AND CDM_PO_NO = POM_PO_NO " &
                    " AND POM_BILLING_METHOD<>'' " &
                    " aND CDM_INVOICE_NO IS NULL " &
                    " AND ( " &
                    "(POM_BILLING_METHOD='DO' AND POM_PO_NO IN" &
                    " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " &
                    " DOM_DO_STATUS IN(2) )and " &
                    " 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " &
                    " AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " &
                    " AND DOD_S_COY_ID = '" & companyID & "' AND DOD_DO_QTY > 0 " &
                    " AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO)))" &
                    " OR " &
                    " (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " &
                    " 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " &
                    " AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND GM_B_COY_ID=GD_B_COY_ID " &
                    " AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " &
                    " AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

            strsql1 = strsql1 & " UNION " _
                    & "SELECT DISTINCT POM_PO_NO,POM_PO_STATUS,POM_B_COY_ID,POM_PO_INDEX,'' AS CDM_DO_No,'' AS CDM_GRN_NO, " _
                    & "'' AS 'DO_Number', '' AS 'GRN_Number' ,POM_BILLING_METHOD, POM_PO_DATE,CM_COY_ID,CM_COY_NAME, " _
                    & "POM_CURRENCY_CODE, (POM_SHIP_AMT - POM_ACC_SHIP_AMT) AS BALSHIP, " _
                    & "(SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_DESC=POM_PAYMENT_TERM AND CODE_CATEGORY='PT') AS PAY_DAY  " _
                    & "FROM COMPANY_MSTR, PO_MSTR, COMPANY_DOC_MATCH " _
                    & "WHERE POM_S_COY_ID='" & companyID & "' AND CDM_B_COY_ID=POM_B_COY_ID AND CDM_PO_NO=POM_PO_NO "
            If bcom_name <> "" Then
                strsql1 = strsql1 & " and POM_B_COY_ID in(select distinct CM_COY_ID from COMPANY_MSTR where CM_COY_NAME " & Common.ParseSQL(bcom_name) & ")"
            End If

            If doc_no <> "" Then
                strsql1 = strsql1 & " and (POM_PO_NO" & Common.ParseSQL(doc_no) & ")"
            End If
            strsql1 = strsql1 & " AND CM_COY_ID = POM_B_COY_ID  " _
                    & "AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS) AND POM_PO_STATUS <>'4' AND POM_PO_STATUS <> '5' " _
                    & "AND POM_BILLING_METHOD IS NOT NULL  AND CDM_PO_NO = POM_PO_NO  AND POM_BILLING_METHOD<>'' " _
                    & "AND CDM_INVOICE_NO IS NULL  AND ( ((POM_BILLING_METHOD='FPO' AND CDM_GRN_NO IS NOT NULL " _
                    & "AND (SELECT COUNT(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID )=  " _
                    & "(SELECT COUNT(*) FROM PO_DETAILS  WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " _
                    & "AND POD_PO_NO = POM_PO_NO   AND POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO))))"
            strsql = strsql & strsql1
            'ds = objDb.FillDs(strsql)
            get_unInvItemEn = strsql

        End Function

        Public Function GetInvDetail(ByVal strInv As String, ByRef Inv() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = "SELECT IM_OUR_REF, IM_REMARK, IM_SHIP_AMT " _
                & "FROM INVOICE_MSTR " _
                & "WHERE IM_INVOICE_NO = '" & Common.Parse(strInv) & "' " _
                & "AND IM_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                Inv(0) = tDS.Tables(0).Rows(j).Item("IM_OUR_REF")
                Inv(1) = tDS.Tables(0).Rows(j).Item("IM_REMARK")
                Inv(2) = tDS.Tables(0).Rows(j).Item("IM_SHIP_AMT")
            Next
        End Function

        Public Function get_unInvItem(ByVal doc_no As String, ByVal bcom_name As String) As DataSet

            Dim ds As DataSet
            Dim grn_uninv As Integer = GRNStatus.Uninvoice
            Dim strsql As String = ""
            strsql = "select distinct POM_PO_NO,POM_PO_STATUS,POM_B_COY_ID,POM_PO_INDEX,CDM_DO_No,CDM_GRN_NO,POM_BILLING_METHOD," &
                                    " POM_PO_DATE,CM_COY_ID,CM_COY_NAME,POM_CURRENCY_CODE, (POM_SHIP_AMT - POM_ACC_SHIP_AMT) AS BALSHIP, " &
                                    " (SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_DESC=POM_PAYMENT_TERM AND CODE_CATEGORY='PT') AS PAY_DAY " &
                                    " FROM COMPANY_MSTR,PO_MSTR,COMPANY_DOC_MATCH " &
                                    " WHERE POM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and CDM_B_COY_ID=POM_B_COY_ID AND CDM_PO_NO=POM_PO_NO "
            If bcom_name <> "" Then

                strsql = strsql & " and POM_B_COY_ID in(select distinct CM_COY_ID from COMPANY_MSTR where CM_COY_NAME " & Common.ParseSQL(bcom_name) & ")"
            End If


            If doc_no <> "" Then
                strsql = strsql & "  and (CDM_INVOICE_NO" & Common.ParseSQL(doc_no) & " or " &
                " POM_PO_NO" & Common.ParseSQL(doc_no) & " or CDM_DO_No" & Common.ParseSQL(doc_no) & " " &
                " or CDM_GRN_NO" & Common.ParseSQL(doc_no) & ")"
            End If

            'strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" & _
            '                       " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" & _
            '                        " AND POM_PO_STATUS <>'4'" & _
            '                        " AND POM_BILLING_METHOD IS NOT NULL " & _
            '                        " AND CDM_PO_NO = POM_PO_NO " & _
            '                        " AND POM_BILLING_METHOD<>'' " & _
            '                        " aND CDM_INVOICE_NO IS NULL " & _
            '                        " AND ( " & _
            '                        "((POM_BILLING_METHOD='FPO'  AND CDM_GRN_NO IS NOT NULL " & _
            '                        " AND (SELECT MAX(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO )" & _
            '                        "= (SELECT COUNT(*) FROM PO_DETAILS " & _
            '                        " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " & _
            '                        " AND( POD_DELIVERED_QTY <> 0 or POD_ORDERED_QTY=POD_DELIVERED_QTY ) AND POD_PO_NO = POM_PO_NO  " & _
            '                        " and POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO)))" & _
            '                        " or " & _
            '                        " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN" & _
            '                        " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " & _
            '                        " DOM_DO_STATUS IN(2) )and " & _
            '                        " 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " & _
            '                        " AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " & _
            '                        " AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND DOD_DO_QTY > 0 " & _
            '                        " AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) )" & _
            '                        " OR " & _
            '                        " (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " & _
            '                        " 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " & _
            '                        " AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND AND GM_B_COY_ID=GD_B_COY_ID " & _
            '                        " AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " & _
            '                        " AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

            'Michelle (6/9/2007) - To replace 'MAX(POD_PO_LINE)' with 'COUNT(POD_PO_LINE)' as the maximum doesn't indicate the no. of po line records
            'strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" & _
            '                       " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" & _
            '                        " AND POM_PO_STATUS <>'4'" & _
            '                        " AND POM_BILLING_METHOD IS NOT NULL " & _
            '                        " AND CDM_PO_NO = POM_PO_NO " & _
            '                        " AND POM_BILLING_METHOD<>'' " & _
            '                        " aND CDM_INVOICE_NO IS NULL " & _
            '                        " AND ( " & _
            '                        "((POM_BILLING_METHOD='FPO' AND CDM_GRN_NO IS NOT NULL " & _
            '                        " AND (SELECT MAX(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID )" & _
            '                        "= (SELECT COUNT(*) FROM PO_DETAILS " & _
            '                        " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " & _
            '                        " AND POD_PO_NO = POM_PO_NO  " & _
            '                        " And POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO)))" & _
            '                        " or " & _
            '                        " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN" & _
            '                        " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " & _
            '" DOM_DO_STATUS IN(2) )and " & _
            '" 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " & _
            '" AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " & _
            '" AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND DOD_DO_QTY > 0 " & _
            '" AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) )" & _
            '" OR " & _
            '" (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " & _
            '" 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " & _
            '" AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND GM_B_COY_ID=GD_B_COY_ID " & _
            '" AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " & _
            '" AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

            ' Michelle (CR0006) - To exlcude where PO has been cancelled ie. POM_PO_STATUS = 5
            strsql = strsql & " AND CM_COY_ID = POM_B_COY_ID" &
                                   " AND POM_PO_NO IN(SELECT POD_PO_NO FROM PO_DETAILS)" &
                                    " AND POM_PO_STATUS <>'4' AND POM_PO_STATUS <> '5' " &
                                    " AND POM_BILLING_METHOD IS NOT NULL " &
                                    " AND CDM_PO_NO = POM_PO_NO " &
                                    " AND POM_BILLING_METHOD<>'' " &
                                    " aND CDM_INVOICE_NO IS NULL " &
                                    " AND ( " &
                                    "((POM_BILLING_METHOD='FPO' AND CDM_GRN_NO IS NOT NULL " &
                                    " AND (SELECT COUNT(POD_PO_LINE) FROM PO_DETAILS WHERE POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID )" &
                                    "= (SELECT COUNT(*) FROM PO_DETAILS " &
                                    " WHERE POD_ORDERED_QTY =  POD_RECEIVED_QTY - POD_REJECTED_QTY + POD_CANCELLED_QTY " &
                                    " AND POD_PO_NO = POM_PO_NO  " &
                                    " And POD_COY_ID = POM_B_COY_ID GROUP BY POD_PO_NO)))" &
                                    " or " &
                                    " (POM_BILLING_METHOD='DO' AND POM_PO_NO IN" &
                                    " (SELECT CDM_PO_NO FROM COMPANY_DOC_MATCH INNER join DO_MSTR on DOM_DO_NO=CDM_DO_NO and " &
                                    " DOM_DO_STATUS IN(2) )and " &
                                    " 0 < (SELECT Count(*) FROM DO_MSTR WHERE DOM_PO_INDEX = POM_PO_INDEX " &
                                    " AND 0 < (SELECT Count(*) FROM DO_DETAILS WHERE DOD_DO_NO = DOM_DO_NO " &
                                    " AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND DOD_DO_QTY > 0 " &
                                    " AND DOD_SHIPPED_QTY >= DOD_DO_QTY GROUP BY DOM_DO_NO) ) )" &
                                    " OR " &
                                    " (POM_BILLING_METHOD= 'GRN' AND CDM_GRN_NO IS NOT NULL  and  " &
                                    " 0 < (SELECT Count(*) FROM GRN_MSTR WHERE GM_PO_INDEX = POM_PO_INDEX AND CDM_GRN_NO=GM_GRN_NO " &
                                    " AND 0 < (SELECT Count(*) FROM GRN_DETAILS WHERE GD_GRN_NO = GM_GRN_NO AND GM_B_COY_ID=GD_B_COY_ID " &
                                    " AND (GM_INVOICE_NO IS NULL OR GM_INVOICE_NO = '') " &
                                    " AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND GM_GRN_STATUS='" & grn_uninv & "' GROUP BY GM_GRN_NO,GM_B_COY_ID))))"

            ds = objDb.FillDs(strsql)
            get_unInvItem = ds

        End Function

        Public Function get_grnprice(ByVal userId As String, ByVal po_no As String, ByVal bcomid As String, ByVal intPOIdx As Integer, Optional ByVal grn_no As String = "") As Double

            'Dim strsql2 As String = "SELECT SUM((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST)as total " & _
            '                        " FROM GRN_DETAILS, PO_MSTR, PO_DETAILS " & _
            '                        " WHERE POM_PO_NO = POD_PO_NO AND POM_B_COY_ID=POD_COY_ID " & _
            '                        " AND GD_RECEIVED_QTY > GD_REJECTED_QTY AND " & _
            '                        " POM_PO_INDEX=" & intPOIdx & " AND GD_B_COY_ID='" & bcomid & "'"
            'If grn_no <> "" Then
            '    strsql2 = strsql2 & " AND GD_GRN_NO='" & grn_no & "'"
            'End If
            'strsql2 = strsql2 & " And POD_PO_LINE=GD_PO_LINE and POM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' GROUP BY GD_GRN_NO"

            Dim strsql2 As String = BuildQueryForUnInv(userId, "price", "GRN", grn_no, bcomid, intPOIdx)

            Dim total As Double

            Dim tDS As DataSet = objDb.FillDs(strsql2)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("total")) Then
                    total = total + tDS.Tables(0).Rows(j).Item("total")
                Else
                    total = 0
                End If

            Next
            Return total
        End Function
        Public Function get_POprice(ByVal item As InvValue)

            Dim total As Double
            Dim gstTotal As Double
            Dim strsql As String = "select POD_GST,POD_UNIT_COST,POD_ORDERED_QTY,POD_CANCELLED_QTY from PO_DETAILS,PO_MSTR " &
                                    " WHERE POD_PO_NO=POM_PO_NO and POM_B_COY_ID=POD_COY_ID and POM_PO_NO='" & item.po_no & "'" &
                                    " AND POM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' and POM_B_COY_ID='" & item.B_com_id & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_UNIT_COST") * (tDS.Tables(0).Rows(j).Item("POD_ORDERED_QTY") - tDS.Tables(0).Rows(j).Item("POD_CANCELLED_QTY"))) Then
                    total = tDS.Tables(0).Rows(j).Item("POD_UNIT_COST") * (tDS.Tables(0).Rows(j).Item("POD_ORDERED_QTY") - tDS.Tables(0).Rows(j).Item("POD_CANCELLED_QTY"))

                Else
                    total = 0
                End If
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("POD_GST")) Then
                    gstTotal = total + (tDS.Tables(0).Rows(j).Item("POD_GST") / 100 * total)
                Else
                    gstTotal = total + (0 / 100 * total)
                End If

                item.Ordered_amount = item.Ordered_amount + gstTotal
            Next

            Dim total2 As Double
            Dim strsql2 As String = "select IM_CREATED_ON,IM_INVOICE_TOTAL from PO_MSTR, INVOICE_MSTR " &
                                    " where IM_PO_Index=POM_PO_INDEX " &
                                    " AND POM_PO_NO='" & item.po_no & "' AND POM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                                    "  and POM_B_COY_ID='" & item.B_com_id & "'"
            tDS = objDb.FillDs(strsql2)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("IM_INVOICE_TOTAL")) Then
                    total2 = tDS.Tables(0).Rows(j).Item("IM_INVOICE_TOTAL")

                Else
                    total2 = 0
                End If
                item.Paid = item.Paid + total2
            Next
            item.total_amount = item.Ordered_amount - item.Paid

        End Function
        Public Function get_DOprice(ByVal userId As String, ByVal DO_NO As String, ByVal buyercomid As String, ByVal intPOIdx As Integer) As Double

            'Dim strsql2 As String = "SELECT SUM(DOD_DO_QTY* POD_UNIT_COST) as total " _
            '             & "FROM DO_DETAILS, PO_MSTR, PO_DETAILS " _
            '             & "WHERE DOD_DO_NO= '" & DO_NO & "' " _
            '             & "AND POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID " _
            '             & "AND POM_PO_INDEX=" & intPOIdx _
            '             & "AND DOD_PO_LINE = POD_PO_LINE " _
            '             & "AND DOD_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " _
            '             & "AND DOD_DO_QTY > 0 " _
            '             & "AND DOD_SHIPPED_QTY >= DOD_DO_QTY " _
            '             & "GROUP BY DOD_DO_NO"

            Dim strsql2 As String = BuildQueryForUnInv(userId, "price", "DO", DO_NO, buyercomid, intPOIdx)

            Dim total As Double
            Dim tDS As DataSet = objDb.FillDs(strsql2)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("total")) Then
                    total = total + tDS.Tables(0).Rows(j).Item("total")
                Else
                    total = total + 0
                End If

            Next

            Return total

        End Function

        Public Function to_price_format(ByVal price As Double) As String
            Return Format(price, "#,##0.00")
        End Function

        Public Function get_invInfo(ByVal item As InvValue) As Boolean

            Dim total As Double
            Dim strsql As String = "select IM_CREATED_ON from PO_MSTR,INVOICE_MSTR,COMPANY_DOC_MATCH " &
                                    " where IM_PO_Index=POM_PO_INDEX AND CDM_GRN_NO='" & item.GRN_NO & "' " &
                                    " AND POM_PO_NO='" & item.po_no & "' AND " &
                                    " POM_B_COY_ID= '" & item.B_com_id & "' and POM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND IM_INVOICE_NO=CDM_INVOICE_NO" &
                                    " AND CDM_B_COY_ID ='" & item.B_com_id & "' AND CDM_S_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                item.create_on = Common.parseNull(tDS.Tables(0).Rows(0).Item("IM_CREATED_ON"))
                Return True
            End If

        End Function
        Public Function get_invWF() As Boolean 'Michelle (13/10/2010) - To check whether need to go thru invoice approval workflow

            Dim strsql As String = "Select CM_INV_APPR from COMPANY_MSTR where CM_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "'"
            Dim strInvWF As String = objDb.GetVal(strsql)
            If strInvWF = "Y" Then
                Return True
            End If
            Return False
        End Function

        Public Function get_Createdinv(ByVal strinv As String, ByVal strbcomid As String) As DataSet
            Dim strsql, strsql1, strsql2, strSql3, strSql4 As String
            Dim strAry() As String
            Dim strInv1, strInv2 As String
            Dim iLoop As Integer

            '//need to make sure no space between ","
            If strinv <> "" Then
                strInv1 = Replace(strinv, ",", "','")
                strInv1 = "'" & strInv1 & "'"
            Else
                strInv1 = "''"
            End If

            If strbcomid <> "" Then
                strInv2 = Replace(strbcomid, ",", "','")
                strInv2 = "'" & strInv2 & "'"
            Else
                strInv2 = "''"
            End If

            Dim strFieldList, strFieldList1 As String
            '//1- invoice created, 2 - invoice already exists

            strFieldList = "IM_INVOICE_TOTAL,CDM_B_COY_ID,CDM_PO_NO,CDM_DO_NO,CDM_GRN_NO,CDM_INVOICE_NO,CM_COY_NAME,POM_BILLING_METHOD"
            strFieldList1 = "IM_INVOICE_TOTAL,CDM_B_COY_ID,CDM_PO_NO,'' AS CDM_DO_NO,'' AS CDM_GRN_NO,CDM_INVOICE_NO,CM_COY_NAME,POM_BILLING_METHOD"

            strsql = "Select " & strFieldList & ",'1' AS msg " &
            " From INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR,PO_MSTR " &
            " where IM_INVOICE_NO in(" & strInv1 & ") and CDM_INVOICE_NO=IM_INVOICE_NO AND POM_PO_INDEX=IM_PO_INDEX And IM_S_COY_ID = CDM_S_COY_ID AND " &
            " CDM_B_COY_ID=IM_B_COY_ID and CM_COY_ID=IM_B_COY_ID AND CDM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND POM_BILLING_METHOD IN ('DO','GRN')"

            strsql1 = "Select " & strFieldList & ",'2' AS msg " &
            " FROM INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR,PO_MSTR " &
            " where IM_INVOICE_NO in(" & strInv2 & ") and CDM_INVOICE_NO=IM_INVOICE_NO AND POM_PO_INDEX=IM_PO_INDEX And IM_S_COY_ID = CDM_S_COY_ID AND " &
            " CDM_B_COY_ID=IM_B_COY_ID and CM_COY_ID=IM_B_COY_ID AND CDM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND POM_BILLING_METHOD IN ('DO','GRN')"

            strSql3 = "Select " & strFieldList1 & ",'1' AS msg " &
            " From INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR,PO_MSTR " &
            " where IM_INVOICE_NO in(" & strInv1 & ") and CDM_INVOICE_NO=IM_INVOICE_NO AND POM_PO_INDEX=IM_PO_INDEX And IM_S_COY_ID = CDM_S_COY_ID AND " &
            " CDM_B_COY_ID=IM_B_COY_ID and CM_COY_ID=IM_B_COY_ID AND CDM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND POM_BILLING_METHOD='FPO'"

            strSql4 = "Select " & strFieldList1 & ",'2' AS msg " &
            " FROM INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR,PO_MSTR " &
            " where IM_INVOICE_NO in(" & strInv2 & ") and CDM_INVOICE_NO=IM_INVOICE_NO AND POM_PO_INDEX=IM_PO_INDEX And IM_S_COY_ID = CDM_S_COY_ID AND " &
            " CDM_B_COY_ID=IM_B_COY_ID and CM_COY_ID=IM_B_COY_ID AND CDM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND POM_BILLING_METHOD='FPO'"

            strsql = strsql & " UNION " & strsql1 & " UNION " & strSql3 & " UNION " & strSql4
            get_Createdinv = objDb.FillDs(strsql)

        End Function
        Function get_poindex(ByVal UserID As String, ByVal po_no As String, ByVal BComID As String)
            Dim companyID = objUsr.getCompanyID(UserID)
            Dim strsql As String = "SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_B_COY_ID='" & BComID & "' and POM_S_COY_ID = '" & companyID & "' AND  POM_PO_NO='" & po_no & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                Return Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_PO_INDEX"))
            End If


        End Function
        Function get_comname(ByVal UserID As String) As String
            Dim companyID = objUsr.getCompanyID(UserID)
            Dim strsql As String = "select CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID='" & companyID & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)

            If tDS.Tables(0).Rows.Count > 0 Then
                Return Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_COY_NAME"))
            End If

        End Function

        Public Function get_poitem(ByVal invno As String, ByVal vcomid As String) As DataSet

            Dim STRSQL As String = "SELECT POM_CURRENCY_CODE FROM PO_MSTR,INVOICE_MSTR WHERE IM_INVOICE_NO='" & invno & "' " &
            " AND IM_S_COY_id='" & vcomid & "' and POM_PO_INDEX=IM_PO_INDEX"
            get_poitem = objDb.FillDs(STRSQL)

        End Function

        Public Function get_podetail(ByVal invno As String, ByVal bcomid As String, ByVal line As String, ByVal vcomid As String, ByVal ITEM As InvValue)

            Dim STRSQL As String = "SELECT POD_PR_LINE,POD_PR_INDEX,POD_REMARK FROM PO_MSTR,INVOICE_MSTR,PO_DETAILS WHERE IM_INVOICE_NO='" & invno & "' " &
            " AND IM_B_COY_ID='" & bcomid & "' AND IM_S_COY_id='" & vcomid & "' and POM_PO_INDEX=IM_PO_INDEX" &
            " and POD_PO_NO=POM_PO_NO and POD_COY_ID=POM_B_COY_ID AND POD_PO_LINE='" & line & "' and POD_COY_ID='" & bcomid & "'"
            Dim tDS As DataSet = objDb.FillDs(STRSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                ITEM.LINE = tDS.Tables(0).Rows(j).Item("POD_PR_LINE").ToString.Trim
                ITEM.PRINDEX = tDS.Tables(0).Rows(j).Item("POD_PR_INDEX").ToString.Trim
                ITEM.remark = tDS.Tables(0).Rows(j).Item("POD_REMARK").ToString.Trim
            Next
        End Function
        Public Function get_custgrn2(ByRef value1() As String, ByRef value2() As String, ByVal pr_line As String, ByVal pr_index As String, ByRef i As Integer)


            Dim strsql As String = "Select PCM_FIELD_NAME, PCD_FIELD_VALUE" &
                                    " From PR_CUSTOM_FIELD_DETAILS, PR_CUSTOM_FIELD_MSTR " &
                                    " Where PCD_PR_INDEX = PCM_PR_INDEX " &
                                    " And PCD_FIELD_NO =PCM_FIELD_NO " &
                                    " And PCD_PR_LINE = '" & pr_line & "' " &
                                    " And PCM_PR_INDEX = '" & pr_index & "' order by PCM_FIELD_NAME"


            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                value1(i) = tDS.Tables(0).Rows(j).Item("PCM_FIELD_NAME").ToString.Trim
                value2(i) = tDS.Tables(0).Rows(j).Item("PCD_FIELD_VALUE").ToString.Trim
                i = i + 1
            Next

        End Function

        Public Function inv_detail(ByVal inv_no As String, ByVal vcomid As String) As DataSet
            'Dim strsql As String = "Select * from INVOICE_DETAILS, INVOICE_MSTR, PO_MSTR where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID and IM_INVOICE_NO='" & inv_no & "' AND ID_INVOICE_NO='" & inv_no & "' and IM_S_COY_ID='" & vcomid & "' AND " & _
            '                        "IM_PO_INDEX = POM_PO_INDEX "

            Dim strsql As String

            'Jules 2018.05.16 - PAMB Scrum 3 - Added Analysis Codes.
            strsql = "SELECT INVOICE_DETAILS.*, INVOICE_MSTR.*, PO_MSTR.*, " &
                    "CASE WHEN ID_GST_RATE = 'N/A' THEN ID_GST_RATE ELSE " &
                    "IF(TAX_PERC = '' OR TAX_PERC IS NULL, CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, "

            'Jules 2018.10.25 - Swap Analysis Code and Analysis Code Description.
            '"(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " &
            '"(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE, " &
            '"(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE " &
            strsql &= "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " &
                    "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE, " &
                    "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE "
            'End modification.

            strsql &= "FROM INVOICE_DETAILS " &
                    "INNER JOIN INVOICE_MSTR ON ID_INVOICE_NO = IM_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                    "INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = IM_B_COY_ID " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = ID_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE IM_INVOICE_NO= '" & inv_no & "' AND IM_S_COY_ID= '" & vcomid & "'"
            inv_detail = objDb.FillDs(strsql)
        End Function
        Public Function inv_detail_string(ByVal inv_no As String, ByVal vcomid As String) As String
            'Dim strsql As String = "Select * from INVOICE_DETAILS, INVOICE_MSTR, PO_MSTR where ID_INVOICE_NO=IM_INVOICE_NO and IM_S_COY_ID=ID_S_COY_ID and IM_INVOICE_NO='" & inv_no & "' AND ID_INVOICE_NO='" & inv_no & "' and IM_S_COY_ID='" & vcomid & "' AND " & _
            '                        "IM_PO_INDEX = POM_PO_INDEX "

            Dim strsql As String

            'Jules 2018.05.16 - PAMB Scrum 3 - Added Analysis Codes.
            strsql = "SELECT INVOICE_DETAILS.*, INVOICE_MSTR.*, PO_MSTR.*, " &
                    "CASE WHEN ID_GST_RATE = 'N/A' THEN ID_GST_RATE ELSE " &
                    "IF(TAX_PERC = '' OR TAX_PERC IS NULL, CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, "

            'Jules 2018.10.25 - Swap Analysis Code and Analysis Code Description.
            '"(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " &
            '"(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE, " &
            '"(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE " &
            strsql &= "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE1) AS FUNDTYPE, " &
                    "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE9) AS PERSONCODE, " &
                    "(SELECT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = ID_ANALYSIS_CODE8) AS PROJECTCODE "
            'End modification.

            strsql &= "FROM INVOICE_DETAILS " &
                    "INNER JOIN INVOICE_MSTR ON ID_INVOICE_NO = IM_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                    "INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                    "INNER JOIN COMPANY_MSTR ON CM_COY_ID = IM_B_COY_ID " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = ID_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE IM_INVOICE_NO= '" & inv_no & "' AND IM_S_COY_ID= '" & vcomid & "'"
            'inv_detail = objDb.FillDs(strsql)
            Return strsql

        End Function
        Public Function get_invmstr(ByVal item As InvValue) As String

            'Dim strsql As String = " select distinct POM_PAYMENT_TERM, POM_PAYMENT_METHOD,POM_B_ADDR_LINE1,POM_B_ADDR_LINE2 " & _
            '",POM_B_ADDR_LINE3,POM_B_POSTCODE,POM_B_CITY,POM_B_STATE,POM_B_COUNTRY  " & _
            '",POM_BUYER_PHONE,POM_CURRENCY_CODE,POM_SHIPMENT_TERM,POM_SHIPMENT_MODE,INVOICE_MSTR.* " & _
            '" From INVOICE_MSTR,PO_MSTR " & _
            '" Where IM_INVOICE_NO ='" & item.Inv_no & "' and IM_S_COY_ID= '" & item.Vcom_id & "'" & _
            '" and POM_PO_INDEX=IM_PO_INDEX"

            Dim strsql As String = " select Distinct PO_MSTR.*,INVOICE_MSTR.IM_INVOICE_INDEX,INVOICE_MSTR.IM_YOUR_REF,INVOICE_MSTR.IM_OUR_REF," &
            " INVOICE_MSTR.IM_S_COY_NAME,INVOICE_MSTR.IM_PAYMENT_DATE,INVOICE_MSTR.IM_CREATED_ON,CMA.CM_BUSINESS_REG_NO,IM_REMARK,CMB.CM_COY_NAME, INVOICE_MSTR.IM_FINANCE_REMARKS " &
            " From INVOICE_MSTR,PO_MSTR,COMPANY_MSTR CMA,COMPANY_MSTR CMB" &
            " Where IM_INVOICE_NO ='" & item.Inv_no & "' and IM_S_COY_ID= '" & item.Vcom_id & "'" &
            " and POM_PO_INDEX=IM_PO_INDEX AND IM_S_COY_ID=CMA.CM_COY_ID AND IM_B_COY_ID=CMB.CM_COY_ID"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            Dim strTempAddr, strDelCode, strDelName As String
            Dim objGlobal As New AppGlobals
            If tDS.Tables(0).Rows.Count > 0 Then

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim <> "" Then
                    strTempAddr = tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<br>" & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim)
                End If

                item.your_ref = tDS.Tables(0).Rows(0).Item("IM_YOUR_REF").ToString.Trim
                item.our_ref = tDS.Tables(0).Rows(0).Item("IM_OUR_REF").ToString.Trim
                item.v_com_name = tDS.Tables(0).Rows(0).Item("IM_S_COY_NAME").ToString.Trim
                item.adds = strTempAddr
                item.po_no = tDS.Tables(0).Rows(0).Item("POM_PO_NO").ToString.Trim
                item.po_index = tDS.Tables(0).Rows(0).Item("POM_PO_INDEX").ToString.Trim
                item.pt = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_TERM").ToString.Trim
                item.pm = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_METHOD").ToString.Trim
                item.phone = tDS.Tables(0).Rows(0).Item("POM_BUYER_PHONE").ToString.Trim
                item.cur = tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE").ToString.Trim
                item.st = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim
                item.sm = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_MODE").ToString.Trim
                item.billingMethod = tDS.Tables(0).Rows(0).Item("POM_BILLING_METHOD").ToString.Trim

                strTempAddr = tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_S_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_S_COUNTRY").ToString.Trim)
                End If

                item.ven_add = strTempAddr
                item.vphone = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_S_PHONE"))
                item.email = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_S_EMAIL"))
                item.bussiness_reg = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_BUSINESS_REG_NO"))
                item.remark = Common.parseNull(tDS.Tables(0).Rows(0).Item("IM_REMARK"))
                item.BComName = tDS.Tables(0).Rows(0).Item("CM_COY_NAME")
                item.pay_date = Common.parseNull(tDS.Tables(0).Rows(0).Item("IM_PAYMENT_DATE"), Date.Today)
                item.create_on = tDS.Tables(0).Rows(0).Item("IM_CREATED_ON")
                item.invoiceIndex = tDS.Tables(0).Rows(0).Item("IM_INVOICE_INDEX").ToString.Trim
                item.remark_finance = Common.parseNull(tDS.Tables(0).Rows(0).Item("IM_FINANCE_REMARKS"))
                item.cur = tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE").ToString.Trim
                If Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE")) = "" Then
                    item.deliveryTerm = ""
                Else
                    strDelCode = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE"))
                    strDelName = objDb.GetVal("SELECT IFNULL(CDT_DEL_NAME,'') FROM COMPANY_DELIVERY_TERM " &
                                "WHERE CDT_DEL_CODE='" & Common.Parse(strDelCode) & "' AND CDT_COY_ID='" & tDS.Tables(0).Rows(0).Item("POM_B_COY_ID") & "'")
                    item.deliveryTerm = strDelCode & IIf(strDelName = "", "", " (" & strDelName & ")")
                End If

                'Jules 2018.10.17
                If Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_VENDOR_CODE")) = "" Then
                    item.VendorCode = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_VENDOR_CODE"))
                End If
                'End modification.
            End If
            objGlobal = Nothing
            Return strsql
        End Function
        Public Function get_invmstr2(ByVal userID As String, ByVal item As InvValue, ByVal bill_meth As String, ByVal bcomid As String)
            Dim companyID As String = objUsr.getCompanyID(userID)
            Dim strsql As String
            strsql = " select Distinct POM_PAYMENT_TERM,POM_CURRENCY_CODE,POM_CREATED_BY,POM_B_COY_ID,POM_BUYER_NAME,POM_BUYER_PHONE, POM_PAYMENT_METHOD, " &
            "POM_B_ADDR_LINE1,POM_B_ADDR_LINE2,POM_B_ADDR_LINE3,POM_B_POSTCODE,POM_B_CITY,POM_B_STATE,POM_B_COUNTRY,POM_EXCHANGE_RATE,(POM_SHIP_AMT - POM_ACC_SHIP_AMT) AS BALSHIP " &
            ",(SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_DESC=POM_PAYMENT_TERM AND CODE_CATEGORY='PT') AS PAY_DAY " &
            ",POM_BUYER_PHONE,POM_CURRENCY_CODE,POM_SHIPMENT_TERM,POM_SHIPMENT_MODE,CM_TAX_REG_NO,POM_S_ATTN,POM_DEL_CODE FROM PO_MSTR A,COMPANY_MSTR B "

            If bill_meth = "GRN" Then
                strsql = strsql & " ,GRN_MSTR D WHERE POM_PO_INDEX = GM_PO_INDEX  AND GM_GRN_NO='" & item.doc_num & "'"

            ElseIf bill_meth = "FPO" Then
                strsql = strsql & " Where POM_PO_NO = '" & item.doc_num & "'"  ' grn_no is doc 

            ElseIf bill_meth = "DO" Then
                strsql = strsql & " ,DO_MSTR WHERE DOM_DO_NO IN('" & item.doc_num & "') AND DOM_PO_INDEX = POM_PO_INDEX " &
                "AND DOM_S_COY_ID = '" & companyID & "' "

            End If
            strsql = strsql & "AND POM_S_COY_ID=CM_COY_ID AND POM_S_COY_ID = '" & companyID & "' " &
            " and POM_B_COY_ID='" & bcomid & "' "

            Dim tDS As DataSet = objDb.FillDs(strsql)
            Dim strTempAddr, strDelCode, strDelName As String
            Dim objGlobal As New AppGlobals
            If tDS.Tables(0).Rows.Count > 0 Then

                strTempAddr = tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>&nbsp;" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>&nbsp;" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>&nbsp;" & tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>&nbsp;" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim)
                End If
                item.adds = strTempAddr
                item.pt = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_TERM").ToString.Trim
                item.pm = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_METHOD").ToString.Trim
                item.cur = tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE").ToString.Trim
                item.st = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim
                item.sm = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_MODE").ToString.Trim
                item.create_by = tDS.Tables(0).Rows(0).Item("POM_BUYER_NAME").ToString.Trim
                item.phone = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_BUYER_PHONE"))
                item.cur = tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE").ToString.Trim
                item.exchange_rate = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_EXCHANGE_RATE"))
                item.tax_reg_no = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_TAX_REG_NO"))
                item.V_attn = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_S_ATTN"))
                item.balShip = Common.parseNull(tDS.Tables(0).Rows(0).Item("BALSHIP"), 0)
                item.payDay = Common.parseNull(tDS.Tables(0).Rows(0).Item("PAY_DAY"))
                If Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE")) = "" Then
                    item.deliveryTerm = ""
                Else
                    strDelCode = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE"))
                    strDelName = objDb.GetVal("SELECT IFNULL(CDT_DEL_NAME,'') FROM COMPANY_DELIVERY_TERM " &
                                "WHERE CDT_DEL_CODE='" & Common.Parse(strDelCode) & "' AND CDT_COY_ID='" & tDS.Tables(0).Rows(0).Item("POM_B_COY_ID") & "'")
                    item.deliveryTerm = strDelCode & IIf(strDelName = "", "", " (" & strDelName & ")")
                End If

            End If

            objGlobal = Nothing
        End Function

        Public Function get_CustFieldGrn(ByVal pr_line_no As String, ByVal pr_number As String, Optional ByVal pr_index As Integer = 0) As DataSet
            Dim strsql As String
            strsql = "Select '*' " &
                    " From PR_CUSTOM_FIELD_DETAILS, PR_CUSTOM_FIELD_MSTR " &
                    " Where PCD_PR_INDEX = PCM_PR_INDEX " &
                    " And PCD_FIELD_NO =PCM_FIELD_NO " &
                    " And PCD_PR_LINE = '" & pr_line_no & "' " &
                    " AND PCM_TYPE='PO' AND PCD_TYPE='PO' " &
                    " And PCM_PR_INDEX = '" & pr_number & "' " &
                    " order by PCM_FIELD_NAME"
            If objDb.Exist(strsql) Then 'PO custom field
                strsql = "Select PCM_FIELD_NAME, PCD_FIELD_VALUE " &
                        "From PR_CUSTOM_FIELD_DETAILS, PR_CUSTOM_FIELD_MSTR " &
                        "Where PCD_PR_INDEX = PCM_PR_INDEX " &
                        "And PCD_FIELD_NO =PCM_FIELD_NO " &
                        "And PCD_PR_LINE = '" & pr_line_no & "' " &
                        "AND PCM_TYPE='PO' AND PCD_TYPE='PO' " &
                        "And PCM_PR_INDEX = '" & pr_number & "' " &
                        "order by PCM_FIELD_NAME"
            Else    'PR custom field
                strsql = "SELECT PCM_FIELD_NAME, PCD_FIELD_VALUE " &
                        "FROM PR_CUSTOM_FIELD_DETAILS, PR_CUSTOM_FIELD_MSTR " &
                        "WHERE PCD_PR_INDEX = PCM_PR_INDEX " &
                        "And PCD_FIELD_NO = PCM_FIELD_NO " &
                        "AND PCD_PR_LINE = '" & pr_line_no & "' AND pcm_type='PR' " &
                        "AND PCM_PR_INDEX = '" & pr_index & "' AND pcd_type='PR' " &
                        "ORDER BY PCM_FIELD_NAME"
            End If
            Dim ds As New DataSet
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        Function get_invlist(ByVal STRINV As String, ByVal STRPO As String, ByVal BILL_METH As String, ByVal bcomid As String, Optional ByVal DOC_NO As String = "") As DataSet
            Dim strsql As String
            If BILL_METH = "FPO" Then
                strsql = "select distinct INVOICE_MSTR.*,CDM_PO_NO,CM_COY_NAME " &
                      " FROM INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR " &
                      " WHERE IM_INVOICE_NO IN(" & STRINV & ") AND IM_PO_INDEX IN(" & STRPO & ") " &
                      " AND CDM_INVOICE_NO=IM_INVOICE_NO " &
                      " and CDM_S_COY_ID=IM_S_Coy_ID and IM_S_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'" &
                      " AND CM_COY_ID = IM_B_COY_ID and IM_B_COY_ID='" & bcomid & "' "
            ElseIf BILL_METH = "GRN" Then
                strsql = "select distinct INVOICE_MSTR.*,CDM_PO_NO,CDM_DO_NO,CDM_GRN_NO,CM_COY_NAME " &
                                   " FROM INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR " &
                                   " WHERE IM_INVOICE_NO IN(" & STRINV & ") AND IM_PO_INDEX IN(" & STRPO & ") " &
                                   " AND CDM_INVOICE_NO=IM_INVOICE_NO " &
                                   " and CDM_S_COY_ID=IM_S_Coy_ID and IM_S_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'" &
                                   " AND CM_COY_ID = IM_B_COY_ID and IM_B_COY_ID='" & bcomid & "'"
            ElseIf BILL_METH = "DO" Then
                strsql = "select distinct INVOICE_MSTR.*,CDM_PO_NO,CDM_DO_NO,CDM_GRN_NO,CM_COY_NAME " &
                                        " FROM INVOICE_MSTR,COMPANY_DOC_MATCH,COMPANY_MSTR " &
                                        " WHERE IM_INVOICE_NO IN(" & STRINV & ") AND IM_PO_INDEX IN(" & STRPO & ") " &
                                        " AND CDM_INVOICE_NO=IM_INVOICE_NO " &
                                        " and CDM_S_COY_ID=IM_S_Coy_ID and IM_S_Coy_ID='" & HttpContext.Current.Session("CompanyId") & "'" &
                                        " AND CM_COY_ID = IM_B_COY_ID and CDM_DO_NO='" & DOC_NO & "' and IM_B_COY_ID='" & bcomid & "'"
            End If

            Dim DS As DataSet
            DS = objDb.FillDs(strsql)
            Return DS

        End Function
#Region "Function Added By Moo"
        Function getUnInvoicePOLine(ByVal userId As String, ByVal strPONo As String, ByVal strBCoyID As String, ByRef blnAllowInv As Boolean, ByRef strInv As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            'strsql = "SELECT POD_PRODUCT_DESC,POD_PO_LINE,POD_B_ITEM_CODE,POD_UOM,POD_GST,POD_DELIVERED_QTY AS QTY," & _
            '" POD_UNIT_COST,POD_WARRANTY_TERMS,POD_ACCT_INDEX,POD_PR_INDEX,POD_PR_LINE,POD_ORDERED_QTY FROM PO_DETAILS WHERE " & _
            '" POD_PO_NO='" & strPONo & "' AND POD_COY_ID='" & strBCoyID & "' AND (POD_ORDERED_QTY <> POD_CANCELLED_QTY)"

            strsql = BuildQueryForUnInv(userId, "line", "FPO", strPONo, strBCoyID)
            ds = objDb.FillDs(strsql)

            '//to prevent double invoice
            strsql = "SELECT DISTINCT ISNULL(CDM_INVOICE_NO,'') FROM COMPANY_DOC_MATCH Where CDM_B_COY_ID='" &
            strBCoyID & "' AND CDM_PO_NO='" & strPONo & "'"
            strInv = objDb.GetVal(strsql)
            If strInv <> "" Then
                blnAllowInv = False 'already invoice, so not allow to create invoice
            Else
                blnAllowInv = True
            End If
            getUnInvoicePOLine = ds
        End Function

        Function getUnInvoiceGRNLine(ByVal intPOIndex As Integer, ByVal strGRNNo As String, ByVal strBCoyID As String, ByRef blnAllowInv As Boolean, ByRef strInv As String, Optional ByVal UserID As String = "", Optional ByRef sqlQueryUnINV As String = Nothing, Optional ByVal AllowSorting As Boolean = False) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            '//exclude those fully rejected item
            'strsql = "SELECT POD_PRODUCT_DESC,POD_PO_LINE,POD_B_ITEM_CODE,POD_UOM,POD_GST,(GD_RECEIVED_QTY-GD_REJECTED_QTY) AS QTY," & _
            '" POD_UNIT_COST,POD_WARRANTY_TERMS,POD_ACCT_INDEX,POD_PR_INDEX,POD_PR_LINE,POD_ORDERED_QTY FROM GRN_DETAILS A, PO_DETAILS B,PO_MSTR C WHERE " & _
            '" A.GD_PO_LINE=B.POD_PO_LINE AND GD_GRN_NO='" & strGRNNo & "' AND GD_B_COY_ID='" & strBCoyID & "'" & _
            '" AND B.POD_COY_ID=C.POM_B_COY_ID AND B.POD_PO_NO=C.POM_PO_NO AND (GD_RECEIVED_QTY-GD_REJECTED_QTY) > 0 AND C.POM_PO_INDEX=" & intPOIndex
            strsql = BuildQueryForUnInv(UserID, "line", "GRN", strGRNNo, strBCoyID, intPOIndex, AllowSorting)
            ds = objDb.FillDs(strsql)

            If Not IsNothing(sqlQueryUnINV) Then
                sqlQueryUnINV = strsql
            End If


            strsql = "SELECT ISNULL(CDM_INVOICE_NO,'') FROM COMPANY_DOC_MATCH Where CDM_B_COY_ID='" &
            strBCoyID & "' AND CDM_GRN_NO='" & strGRNNo & "'"
            strInv = objDb.GetVal(strsql)

            If strInv <> "" Then
                blnAllowInv = False
            Else
                blnAllowInv = True
            End If
            getUnInvoiceGRNLine = ds
        End Function

        Function getUnInvoiceDOLine(ByVal intPOIndex As Integer, ByVal strDONo As String, ByVal strBCoyID As String, ByRef blnAllowInv As Boolean, ByRef strInv As String) As DataSet
            Dim ds As DataSet
            Dim strsql As String

            '//exclude those fully rejected item
            'strsql = "SELECT POD_PRODUCT_DESC,POD_PO_LINE,POD_B_ITEM_CODE,POD_UOM,POD_GST,DOD_DO_QTY AS QTY," & _
            '" POD_UNIT_COST,POD_WARRANTY_TERMS,POD_ACCT_INDEX,POD_PR_INDEX,POD_PR_LINE,POD_ORDERED_QTY FROM DO_DETAILS A, PO_DETAILS B,PO_MSTR C WHERE " & _
            '" A.DOD_PO_LINE=B.POD_PO_LINE AND DOD_DO_NO='" & strDONo & "' AND POM_B_COY_ID='" & strBCoyID & "'" & _
            '" AND B.POD_COY_ID=C.POM_B_COY_ID AND B.POD_PO_NO=C.POM_PO_NO AND DOD_DO_QTY > 0 AND C.POM_PO_INDEX=" & intPOIndex

            strsql = BuildQueryForUnInv("line", "DO", strDONo, strBCoyID, intPOIndex)
            ds = objDb.FillDs(strsql)

            strsql = "SELECT ISNULL(CDM_INVOICE_NO,'') FROM COMPANY_DOC_MATCH Where CDM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CDM_DO_NO='" & strDONo & "'"
            strInv = objDb.GetVal(strsql)
            If strInv <> "" Then
                blnAllowInv = False
            Else
                blnAllowInv = True
            End If
            getUnInvoiceDOLine = ds
        End Function

        'Zulham 17072018 - PAMB
        Public Function Update_InvMstr(ByVal userId As String, ByVal ds As DataSet, ByVal GRN_STATUS As String, ByVal do_status As String, ByRef strInvSuccess As String, ByRef strInvFail As String, Optional ByVal aryTaxCode As ArrayList = Nothing, Optional ByVal strGSTInv As String = "N", Optional ByVal isResident As String = "", Optional ByVal Doc_No As String = "") As Boolean

            Dim OBJGLB As New AppGlobals
            Dim objTrans As New Tracking
            Dim preArray(0) As String
            Dim Inv_num As String
            Dim prefix As String
            Dim check As String
            Dim qty As Integer
            Dim i, J As Integer
            Dim DS_PO As DataSet
            Dim intPOIndex As Integer
            Dim strPONo As String
            Dim strBCoyID, strSCoyID, strLoginUser As String
            Dim blnAllowInv As Boolean
            Dim dr As DataRow
            Dim strTempInv As String
            Dim objBCM As New BudgetControl
            Dim objMail As New Email
            Dim strSql As String
            Dim intIncrementNo = 0
            Dim blnSTaxCode As Boolean
            Dim strSupplyTC As String
            strLoginUser = userId
            strSCoyID = objUsr.getCompanyID(strLoginUser)

            Dim strarray(0) As String
            strSql = " SET @DUPLICATE_CHK = ''; SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'Invoice' "
            Common.Insert2Ary(strarray, strSql)

            For i = 0 To ds.Tables(0).Rows.Count - 1
                '' ''Dim strarray(0) As String

                '' ''strSql = "UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CP_PARAM_TYPE = 'Invoice' "
                '' ''Common.Insert2Ary(strarray, strSql)

                strSql = ""
                With ds.Tables(0).Rows(i)
                    '' ''OBJGLB.GetLatestDocNo("Invoice", strarray, Inv_num, prefix)

                    strBCoyID = .Item("b_com_id")
                    intPOIndex = get_poindex(strLoginUser, .Item("po_no"), .Item("b_com_id"))

                    strPONo = .Item("po_no")

                    intIncrementNo = 1

                    Inv_num = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'Invoice' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

                    prefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'Invoice') "

                    strSql = "SELECT '*' From Invoice_Mstr Where IM_invoice_No=" & Inv_num & " AND IM_S_COY_ID='" & strSCoyID & "'"
                    If objDb.Exist(strSql) Then
                        strInvFail = ""
                        strInvSuccess = ""
                        Return False
                    End If

                    If .Item("bill_meth") = "GRN" Then
                        strSql = " SELECT CAST(@DUPLICATE_CHK := IFNULL(CDM_INVOICE_NO,'') AS CHAR(1000)) FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & .Item("b_com_id") & "' AND CDM_GRN_NO='" & .Item("grn_no") & "'"
                    ElseIf .Item("bill_meth") = "FPO" Then
                        strSql = " SELECT DISTINCT CAST(@DUPLICATE_CHK := IFNULL(CDM_INVOICE_NO,'') AS CHAR(1000)) FROM COMPANY_DOC_MATCH WHERE CDM_B_COY_ID='" & .Item("b_com_id") & "' AND CDM_PO_NO='" & .Item("po_no") & "'"
                    ElseIf ds.Tables(0).Rows(i)("bill_meth") = "DO" Then
                        strSql = " SELECT CAST(@DUPLICATE_CHK := IFNULL(CDM_INVOICE_NO,'') AS CHAR(1000)) FROM COMPANY_DOC_MATCH WHERE CDM_S_COY_ID='" & .Item("b_com_id") & "' AND CDM_DO_NO='" & .Item("do_no") & "'"
                    End If
                    Common.Insert2Ary(strarray, strSql)

                    If .Item("bill_meth") = "GRN" Then
                        DS_PO = getUnInvoiceGRNLine(intPOIndex, .Item("grn_no"), .Item("b_com_id"), blnAllowInv, strTempInv, strLoginUser)

                        If blnAllowInv Then
                            strSql = "Update GRN_MSTR SET GM_INVOICE_NO=" & Inv_num & ",GM_GRN_STATUS = '" & GRN_STATUS & "' " &
                            " WHERE GM_GRN_NO='" & .Item("grn_no") & "' and GM_B_COY_ID='" & .Item("b_com_id") & "' and GM_S_COY_ID='" & strSCoyID & "'"
                            Common.Insert2Ary(strarray, strSql)

                            strSql = "UPDATE DO_MSTR SET DOM_DO_STATUS='" & do_status & "' WHERE DOM_PO_INDEX =" & intPOIndex & " And DOM_DO_NO='" & .Item("do_no") & "' AND DOM_DO_STATUS<>" & DOStatus.Rejected
                            Common.Insert2Ary(strarray, strSql)

                            strSql = objTrans.updateDocMatchingNew(.Item("po_no"), Inv_num, .Item("grn_no"), "INV", strSCoyID, .Item("b_com_id"), "GRN")
                            Common.Insert2Ary(strarray, strSql)

                            strSql = BuildQueryForUnInv(strLoginUser, "bcm", "GRN", .Item("grn_no"), .Item("b_com_id"), intPOIndex)
                            objBCM.BCMCalc("INV", String.Empty, "", EnumBCMAction.InvoiceCreated, strarray, .Item("b_com_id"), strSql)
                        End If
                    ElseIf .Item("bill_meth") = "FPO" Then
                        DS_PO = getUnInvoicePOLine(userId, .Item("po_no"), .Item("b_com_id"), blnAllowInv, strTempInv)

                        If blnAllowInv Then
                            Dim grn_no As String
                            'Michelle (4/6/2013) - Issue 1987
                            'grn_no = objDb.Get1Column("GRN_MSTR A,GRN_DETAILS B", "GM_GRN_NO", " WHERE A.GM_B_COY_ID=B.GD_B_COY_ID AND A.GM_GRN_NO=B.GD_GRN_NO AND A.GM_PO_INDEX=" & intPOIndex & " GROUP BY GM_B_COY_ID,GM_GRN_NO HAVING SUM(GD_RECEIVED_QTY)>SUM(GD_REJECTED_QTY)")
                            'strSql = "Update GRN_MSTR SET GM_INVOICE_NO=" & Inv_num & ",GM_GRN_STATUS = '" & GRN_STATUS & "' " & _
                            '" WHERE GM_GRN_NO IN('" & grn_no & "')" & " And GM_B_COY_ID='" & .Item("b_com_id") & "'"
                            strSql = "UPDATE GRN_MSTR C,(SELECT GM_GRN_NO FROM GRN_MSTR A,GRN_DETAILS B WHERE A.GM_B_COY_ID=B.GD_B_COY_ID AND A.GM_GRN_NO=B.GD_GRN_NO AND A.GM_PO_INDEX=" & intPOIndex &
                                     " GROUP BY GM_B_COY_ID,GM_GRN_NO HAVING SUM(GD_RECEIVED_QTY)>SUM(GD_REJECTED_QTY) And GM_B_COY_ID='" & .Item("b_com_id") & "') AS D " &
                                     "SET C.GM_INVOICE_NO=" & Inv_num & ",C.GM_GRN_STATUS = '" & GRN_STATUS & "' " &
                                     " WHERE C.GM_GRN_NO = D.GM_GRN_NO AND C.GM_B_COY_ID='" & .Item("b_com_id") & "'"

                            ' And CDM_S_COY_ID='" & strSCoyID & "')"
                            Common.Insert2Ary(strarray, strSql)
                            '"' AND DOM_DO_STATUS<>" & DOStatus.Rejected
                            'strSql = "Update DO_MSTR SET DOM_DO_STATUS = '" & do_status & "' " & _
                            '" WHERE DOM_DO_NO IN(SELECT CDM_DO_NO FROM COMPANY_DOC_MATCH WHERE CDM_PO_NO = '" & _
                            '.Item("po_no") & "' And CDM_B_COY_ID='" & .Item("b_com_id") & "' And CDM_S_COY_ID='" & strSCoyID & "')"

                            strSql = "Update DO_MSTR SET DOM_DO_STATUS = '" & do_status & "' " &
                            " WHERE DOM_PO_INDEX=" & intPOIndex & " AND DOM_DO_STATUS<>" & DOStatus.Rejected
                            Common.Insert2Ary(strarray, strSql)

                            strSql = objTrans.updateDocMatchingNew(.Item("po_no"), Inv_num, .Item("po_no"), "INV", strSCoyID, .Item("b_com_id"), "FPO")
                            Common.Insert2Ary(strarray, strSql)

                            strSql = BuildQueryForUnInv(strLoginUser, "bcm", "FPO", .Item("po_no"), .Item("b_com_id"), intPOIndex)
                            objBCM.BCMCalc("INV", String.Empty, "", EnumBCMAction.InvoiceCreated, strarray, .Item("b_com_id"), strSql)
                        End If
                    ElseIf ds.Tables(0).Rows(i)("bill_meth") = "DO" Then
                        DS_PO = getUnInvoiceDOLine(intPOIndex, .Item("do_no"), .Item("b_com_id"), blnAllowInv, strTempInv)

                        If blnAllowInv Then
                            strSql = "UPDATE DO_MSTR SET DOM_DO_STATUS='" & do_status & "' WHERE DOM_PO_INDEX =" & intPOIndex & " And DOM_DO_NO='" & .Item("do_no") & "'"
                            Common.Insert2Ary(strarray, strSql)

                            strSql = objTrans.updateDocMatchingNew(.Item("po_no"), Inv_num, .Item("do_no"), "INV", strSCoyID, .Item("b_com_id"), "DO")
                            Common.Insert2Ary(strarray, strSql)

                            strSql = BuildQueryForUnInv(strLoginUser, "bcm", "DO", .Item("do_no"), .Item("b_com_id"), intPOIndex)
                            objBCM.BCMCalc("INV", String.Empty, "", EnumBCMAction.InvoiceCreated, strarray, .Item("b_com_id"), strSql)
                        End If
                    End If

                    If blnAllowInv Then
                        'objBCM.BCMCalc("INV", Inv_num, EnumBCMAction.InvoiceCreated, strarray)
                        '//for audit trail

                        Dim objUsers As New Users
                        objUsers.Log_UserActivityNew(String.Empty, String.Empty, strarray, WheelModule.Fulfillment, WheelUserActivity.V_Invoice, Inv_num)
                        objUsers = Nothing

                        '' ''If strInvSuccess = "" Then
                        '' ''    strInvSuccess = Trim(Inv_num)
                        '' ''Else
                        '' ''    strInvSuccess = strInvSuccess & "," & Trim(Inv_num)
                        '' ''End If
                        Dim dblTax As Double = (((CDbl(Replace(.Item("amount"), ",", "")) + CDbl(Replace(.Item("ShipAmt"), ",", "")))) / 100) * CInt(IIf(.Item("Tax") = "", 0, .Item("Tax")))

                        strSql = "INSERT INTO INVOICE_MSTR" &
                        "(IM_INVOICE_NO,IM_S_COY_ID,IM_S_COY_NAME,IM_PO_INDEX " &
                        ",IM_B_COY_ID,IM_REMARK,IM_CREATED_BY,IM_CREATED_ON,IM_INVOICE_STATUS,IM_YOUR_REF " &
                        ",IM_OUR_REF,IM_WITHHOLDING_TAX,IM_INVOICE_PREFIX,IM_PRINTED,IM_FOLDER,IM_INVOICE_TOTAL,IM_PAYMENT_DATE, IM_SHIP_AMT, IM_GST_INVOICE) VALUES " &
                        "(" & Inv_num & ",'" & strSCoyID & "','" & Common.Parse(get_comname(strLoginUser)) & "'," & intPOIndex & ", " &
                        "'" & .Item("b_com_id") & "','" & Common.Parse(.Item("remark")) & "','" & strLoginUser & "', " &
                        "" & Common.ConvertDate(Date.Now) & ",'" & .Item("inv_status") & "','" & Common.Parse(.Item("doc")) & "'," &
                        "'" & Common.Parse(.Item("ref")) & "'," & IIf(.Item("Tax") = "", "NULL", "'" & .Item("Tax") & "'") & "," & prefix & " " &
                        ",'0','0'," & (CDbl(Replace(.Item("amount"), ",", "")) + CDbl(Replace(.Item("ShipAmt"), ",", ""))) + dblTax & "," &
                        Common.ConvertDate(DateAdd(DateInterval.Day, CInt(.Item("pay_day")), Date.Today)) &
                        "," & .Item("ShipAmt") & ", '" & strGSTInv & "')"
                        Common.Insert2Ary(strarray, strSql)

                        'Update the accumulated shipping & handling in PO mstr
                        strSql = "UPDATE PO_MSTR SET POM_ACC_SHIP_AMT = POM_ACC_SHIP_AMT + " &
                                 .Item("ShipAmt") & " WHERE POM_PO_INDEX = " & .Item("POM_PO_INDEX")
                        Common.Insert2Ary(strarray, strSql)

                        If aryTaxCode Is Nothing Then
                            blnSTaxCode = False
                        Else
                            If aryTaxCode.Count > 0 Then
                                blnSTaxCode = True
                            Else
                                blnSTaxCode = False
                            End If
                        End If

                        J = 0
                        Dim dblTaxAmt As Double
                        For Each dr In DS_PO.Tables(0).Rows
                            strSupplyTC = ""
                            If blnSTaxCode = True Then
                                strSupplyTC = getSupplyTaxCodeFromArray(dr("POD_PO_LINE"), aryTaxCode)
                            End If

                            'Chee Hong - 05/12/2014 - Calculate GST Amt
                            If IsDBNull(dr("POD_GST")) Then
                                dblTaxAmt = 0
                            Else
                                If dr("POD_GST") > 0 Then
                                    '2015-06-24: CH: Rounding issue (Prod issue)
                                    'dblTaxAmt = (dr("POD_UNIT_COST") * dr("QTY")) * dr("POD_GST") / 100
                                    dblTaxAmt = CDec(Format((CDec(Format(dr("POD_UNIT_COST") * dr("QTY"), "###0.00"))) * dr("POD_GST") / 100, "###0.00"))
                                Else
                                    dblTaxAmt = 0
                                End If
                            End If

                            If IsDBNull(dr("POD_WARRANTY_TERMS")) And IsDBNull(dr("POD_ACCT_INDEX")) Then
                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH 'Include ID_GST_INPUT_TAX_CODE
                                strSql = "Insert into INVOICE_DETAILS (ID_INVOICE_NO,ID_INVOICE_LINE,ID_S_COY_ID,ID_PO_LINE," &
                                " ID_PRODUCT_DESC,ID_B_ITEM_CODE,ID_UOM," &
                                " ID_GST,ID_RECEIVED_QTY,ID_UNIT_COST,ID_B_CATEGORY_CODE,ID_B_GL_CODE,ID_ASSET_CODE,ID_GST_RATE,ID_GST_VALUE,ID_GST_INPUT_TAX_CODE,ID_GST_OUTPUT_TAX_CODE," &
                                "ID_GIFT,ID_ANALYSIS_CODE1,ID_ANALYSIS_CODE8,ID_ANALYSIS_CODE9) " &
                                " VALUES " &
                                "(" & Inv_num & "," & J + 1 & ",'" & strSCoyID & "'," & dr("POD_PO_LINE") & ",'" & Common.Parse(dr("POD_PRODUCT_DESC")) & "'," &
                                "'" & Common.Parse(Common.parseNull(dr("POD_B_ITEM_CODE"))) & "','" & Common.Parse(dr("POD_UOM")) & "','" & Common.Parse(dr("POD_GST")) &
                                "'," & dr("QTY") & "," & dr("POD_UNIT_COST") & ",'" & Common.Parse(Common.parseNull(dr("POD_B_CATEGORY_CODE"))) &
                                "','" & Common.Parse(Common.parseNull(dr("POD_B_GL_CODE"))) & "','" & Common.Parse(Common.parseNull(dr("ASSET_CODE"))) & "'" &
                                "," & IIf(IsDBNull(dr("POD_GST_RATE")), "NULL", "'" & dr("POD_GST_RATE") & "'")
                                'strSql &= "," & IIf(IsDBNull(dr("POD_TAX_VALUE")), "NULL", dr("POD_TAX_VALUE")) & _
                                strSql &= "," & dblTaxAmt
                                If Common.parseNull(dr("POD_GST_INPUT_TAX_CODE")) = "" Then
                                    strSql &= ",NULL"
                                Else
                                    strSql &= ",'" & Common.Parse(dr("POD_GST_INPUT_TAX_CODE")) & "'"
                                End If
                                strSql &= "," & IIf(blnSTaxCode = False, "NULL", "'" & Common.Parse(strSupplyTC) & "'") '& ")"

                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                strSql &= "," & IIf(IsDBNull(dr("POD_GIFT")), "NULL", "'" & dr("POD_GIFT") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_FUND_TYPE")), "NULL", "'" & dr("POD_FUND_TYPE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PROJECT_CODE")), "NULL", "'" & dr("POD_PROJECT_CODE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PERSON_CODE")), "NULL", "'" & dr("POD_PERSON_CODE") & "'") & ")"
                                'End modification.

                                Common.Insert2Ary(strarray, strSql)

                            ElseIf IsDBNull(dr("POD_WARRANTY_TERMS")) Then
                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH 'Include ID_GST_INPUT_TAX_CODE
                                strSql = "Insert into INVOICE_DETAILS (ID_INVOICE_NO,ID_INVOICE_LINE,ID_S_COY_ID,ID_PO_LINE," &
                                " ID_PRODUCT_DESC,ID_B_ITEM_CODE,ID_UOM," &
                                " ID_GST,ID_RECEIVED_QTY,ID_UNIT_COST,ID_ACCT_INDEX,ID_B_CATEGORY_CODE,ID_B_GL_CODE,ID_ASSET_CODE,ID_GST_RATE,ID_GST_VALUE,ID_GST_INPUT_TAX_CODE,ID_GST_OUTPUT_TAX_CODE," &
                                "ID_GIFT,ID_ANALYSIS_CODE1,ID_ANALYSIS_CODE8,ID_ANALYSIS_CODE9) " &
                                " VALUES " &
                                "(" & Inv_num & "," & J + 1 & ",'" & strSCoyID & "'," & dr("POD_PO_LINE") & ",'" & Common.Parse(dr("POD_PRODUCT_DESC")) & "'," &
                                "'" & Common.Parse(Common.parseNull(dr("POD_B_ITEM_CODE"))) & "','" & Common.Parse(dr("POD_UOM")) & "','" & Common.Parse(dr("POD_GST")) &
                                "'," & dr("QTY") & "," & dr("POD_UNIT_COST") & ",'" &
                                Common.Parse(Common.parseNull(dr("POD_ACCT_INDEX"))) & "','" & Common.Parse(Common.parseNull(dr("POD_B_CATEGORY_CODE"))) &
                                "','" & Common.Parse(Common.parseNull(dr("POD_B_GL_CODE"))) & "','" & Common.Parse(Common.parseNull(dr("ASSET_CODE"))) & "'" &
                                "," & IIf(IsDBNull(dr("POD_GST_RATE")), "NULL", "'" & dr("POD_GST_RATE") & "'")
                                'strSql &= "," & IIf(IsDBNull(dr("POD_TAX_VALUE")), "NULL", dr("POD_TAX_VALUE")) & _
                                strSql &= "," & dblTaxAmt
                                If Common.parseNull(dr("POD_GST_INPUT_TAX_CODE")) = "" Then
                                    strSql &= ",NULL"
                                Else
                                    strSql &= ",'" & Common.Parse(dr("POD_GST_INPUT_TAX_CODE")) & "'"
                                End If
                                strSql &= "," & IIf(blnSTaxCode = False, "NULL", "'" & Common.Parse(strSupplyTC) & "'") '& ")"

                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                strSql &= "," & IIf(IsDBNull(dr("POD_GIFT")), "NULL", "'" & dr("POD_GIFT") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_FUND_TYPE")), "NULL", "'" & dr("POD_FUND_TYPE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PROJECT_CODE")), "NULL", "'" & dr("POD_PROJECT_CODE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PERSON_CODE")), "NULL", "'" & dr("POD_PERSON_CODE") & "'") & ")"
                                'End modification.

                                Common.Insert2Ary(strarray, strSql)

                            ElseIf IsDBNull(dr("POD_ACCT_INDEX")) Then
                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH 'Include ID_GST_INPUT_TAX_CODE
                                strSql = "Insert into INVOICE_DETAILS (ID_INVOICE_NO,ID_INVOICE_LINE,ID_S_COY_ID,ID_PO_LINE," &
                                " ID_PRODUCT_DESC,ID_B_ITEM_CODE,ID_UOM," &
                                " ID_GST,ID_RECEIVED_QTY,ID_UNIT_COST,ID_WARRANTY_TERMS,ID_B_CATEGORY_CODE,ID_B_GL_CODE,ID_ASSET_CODE,ID_GST_RATE,ID_GST_VALUE,ID_GST_INPUT_TAX_CODE,ID_GST_OUTPUT_TAX_CODE," &
                                "ID_GIFT,ID_ANALYSIS_CODE1,ID_ANALYSIS_CODE8,ID_ANALYSIS_CODE9) " &
                                " VALUES " &
                                "(" & Inv_num & "," & J + 1 & ",'" & strSCoyID & "'," & dr("POD_PO_LINE") & ",'" & Common.Parse(dr("POD_PRODUCT_DESC")) & "'," &
                                "'" & Common.Parse(Common.parseNull(dr("POD_B_ITEM_CODE"))) & "','" & Common.Parse(dr("POD_UOM")) & "','" & Common.Parse(dr("POD_GST")) &
                                "'," & dr("QTY") & "," & dr("POD_UNIT_COST") & ",'" & dr("POD_WARRANTY_TERMS") & "'," &
                                "'" & Common.Parse(Common.parseNull(dr("POD_B_CATEGORY_CODE"))) &
                                "','" & Common.Parse(Common.parseNull(dr("POD_B_GL_CODE"))) & "','" & Common.Parse(Common.parseNull(dr("ASSET_CODE"))) & "'" &
                                "," & IIf(IsDBNull(dr("POD_GST_RATE")), "NULL", "'" & dr("POD_GST_RATE") & "'")
                                'strSql &= "," & IIf(IsDBNull(dr("POD_TAX_VALUE")), "NULL", dr("POD_TAX_VALUE")) & _
                                strSql &= "," & dblTaxAmt
                                If Common.parseNull(dr("POD_GST_INPUT_TAX_CODE")) = "" Then
                                    strSql &= ",NULL"
                                Else
                                    strSql &= ",'" & Common.Parse(dr("POD_GST_INPUT_TAX_CODE")) & "'"
                                End If
                                strSql &= "," & IIf(blnSTaxCode = False, "NULL", "'" & Common.Parse(strSupplyTC) & "'") '& ")"

                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                strSql &= "," & IIf(IsDBNull(dr("POD_GIFT")), "NULL", "'" & dr("POD_GIFT") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_FUND_TYPE")), "NULL", "'" & dr("POD_FUND_TYPE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PROJECT_CODE")), "NULL", "'" & dr("POD_PROJECT_CODE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PERSON_CODE")), "NULL", "'" & dr("POD_PERSON_CODE") & "'") & ")"
                                'End modification.

                                Common.Insert2Ary(strarray, strSql)
                            Else
                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH 'Include ID_GST_INPUT_TAX_CODE
                                strSql = "Insert into INVOICE_DETAILS (ID_INVOICE_NO,ID_INVOICE_LINE,ID_S_COY_ID,ID_PO_LINE," &
                                " ID_PRODUCT_DESC,ID_B_ITEM_CODE,ID_UOM," &
                                " ID_GST,ID_RECEIVED_QTY,ID_UNIT_COST,ID_WARRANTY_TERMS,ID_B_CATEGORY_CODE,ID_B_GL_CODE,ID_ACCT_INDEX,ID_ASSET_CODE,ID_GST_RATE,ID_GST_VALUE,ID_GST_INPUT_TAX_CODE,ID_GST_OUTPUT_TAX_CODE," &
                                "ID_GIFT,ID_ANALYSIS_CODE1,ID_ANALYSIS_CODE8,ID_ANALYSIS_CODE9) " &
                                " VALUES " &
                                "(" & Inv_num & "," & J + 1 & ",'" & strSCoyID & "'," & dr("POD_PO_LINE") & ",'" & Common.Parse(dr("POD_PRODUCT_DESC")) & "'," &
                                "'" & Common.Parse(Common.parseNull(dr("POD_B_ITEM_CODE"))) & "','" & Common.Parse(dr("POD_UOM")) & "','" & Common.Parse(dr("POD_GST")) &
                                "'," & dr("QTY") & "," & dr("POD_UNIT_COST") & ",'" & dr("POD_WARRANTY_TERMS") & "'," &
                                "'" & Common.Parse(Common.parseNull(dr("POD_B_CATEGORY_CODE"))) &
                                "','" & Common.Parse(Common.parseNull(dr("POD_B_GL_CODE"))) & "','" & Common.Parse(Common.parseNull(dr("POD_ACCT_INDEX"))) & "','" & Common.Parse(Common.parseNull(dr("ASSET_CODE"))) & "'" &
                                "," & IIf(IsDBNull(dr("POD_GST_RATE")), "NULL", "'" & dr("POD_GST_RATE") & "'")
                                'strSql &= "," & IIf(IsDBNull(dr("POD_TAX_VALUE")), "NULL", dr("POD_TAX_VALUE")) & _
                                strSql &= "," & dblTaxAmt
                                If Common.parseNull(dr("POD_GST_INPUT_TAX_CODE")) = "" Then
                                    strSql &= ",NULL"
                                Else
                                    strSql &= ",'" & Common.Parse(dr("POD_GST_INPUT_TAX_CODE")) & "'"
                                End If
                                strSql &= "," & IIf(blnSTaxCode = False, "NULL", "'" & Common.Parse(strSupplyTC) & "'") '& ")"

                                'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift & Analysis Codes.
                                strSql &= "," & IIf(IsDBNull(dr("POD_GIFT")), "NULL", "'" & dr("POD_GIFT") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_FUND_TYPE")), "NULL", "'" & dr("POD_FUND_TYPE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PROJECT_CODE")), "NULL", "'" & dr("POD_PROJECT_CODE") & "'")
                                strSql &= "," & IIf(IsDBNull(dr("POD_PERSON_CODE")), "NULL", "'" & dr("POD_PERSON_CODE") & "'") & ")"
                                'End modification.

                                Common.Insert2Ary(strarray, strSql)

                            End If

                            J += 1
                        Next

                        Dim objCompany As New Companies
                        Dim strInvAppr As String = objCompany.GetInvApprMode(strBCoyID)

                        If strInvAppr = "Y" Then

                            ' insert into INVOICE_APPROVAL

                            'Michelle (22/9/2010) -
                            Dim objDB As New EAD.DBCom
                            Dim strcond, strtbl As String

                            'Zulham 17072018 - PAMB
                            'Craven 13/7/2011 - To query join another table
                            'strtbl = " (PO_DETAILS JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO)  LEFT JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_INDEX = POM_DEPT_INDEX AND CDM_COY_ID = POD_COY_ID "
                            'strcond = " WHERE POD_COY_ID = '" & strBCoyID & "' AND POD_PO_NO = '" & .Item("po_no") & "' AND POM_B_COY_ID='" & strBCoyID & "' AND "

                            'Jules 2019.01.04 - Get INV workflow assigned to buyer.
                            'Zulham 17072018 - PAMB
                            'Dim grpIndex = objDB.GetVal("SELECT AGA_GRP_INDEX, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE
                            '                            FROM APPROVAL_GRP_FINANCE
                            '                            WHERE AGA_GRP_INDEX = 
                            '                            (SELECT CDM_APPROVAL_GRP_INDEX
                            '                            FROM COMPANY_DEPT_MSTR
                            '                            JOIN APPROVAL_GRP_MSTR ON CDM_APPROVAL_GRP_INDEX = AGM_GRP_INDEX AND AGM_RESIDENT = '" & isResident & "'
                            '                            WHERE CDM_COY_ID = '" & strBCoyID & "'
                            '                            AND CDM_DEPT_CODE IN 
                            '                            (SELECT CDM_DEPT_CODE
                            '                            FROM (PO_DETAILS JOIN PO_MSTR ON POD_PO_NO = POM_PO_NO)  
                            '                            LEFT JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_INDEX = POM_DEPT_INDEX AND CDM_COY_ID = POD_COY_ID 
                            '                            LEFT JOIN APPROVAL_GRP_MSTR ON CDM_APPROVAL_GRP_INDEX = AGM_GRP_INDEX
                            '                            WHERE POD_COY_ID = '" & strBCoyID & "' AND POD_PO_NO = '" & .Item("po_no") & "' AND POM_B_COY_ID='" & strBCoyID & "' ))
                            '                            ORDER BY AGA_TYPE DESC, AGA_SEQ")
                            'Dim grpIndex = objDB.GetVal("SELECT AGA_GRP_INDEX, AGA_SEQ, AGA_AO, AGA_A_AO, AGA_RELIEF_IND, AGA_TYPE
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
                            '                            AND POM_BUYER_ID = AGA_AO
                            '                            AND AGA_SEQ = 1
                            '                            AND AGM_RESIDENT = '" & isResident & "'
                            '                            LIMIT 1
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
                            'End modification.

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
                            'End modification 2019.01.04

                            Dim dsFinAppGrp As DataSet = objDB.FillDs(query, False)

                            If Not dsFinAppGrp Is Nothing Then
                                If dsFinAppGrp.Tables(0).Rows.Count > 0 Then

                                    Dim dtAO As DataTable = dsFinAppGrp.Tables(0)

                                    For k As Integer = 0 To dtAO.Rows.Count - 1
                                        strSql = "INSERT INTO FINANCE_APPROVAL (FA_INVOICE_INDEX, FA_AGA_TYPE, FA_AO, FA_A_AO, FA_SEQ, FA_AO_ACTION, "
                                        strSql &= "FA_APPROVAL_GRP_INDEX, FA_RELIEF_IND) VALUES ("
                                        'Michelle (22/9/2010) - To cater for MYSQL
                                        'strSql &= "IDENT_CURRENT('INVOICE_MSTR'), "
                                        strSql &= objDB.GetLatestInsertedID("INVOICE_MSTR")
                                        strSql &= ", '" & Common.parseNull(dtAO.Rows(k)("AGA_TYPE")) & "', " ' FA_AGA_TYPE
                                        strSql &= "'" & Common.parseNull(dtAO.Rows(k)("AGA_AO")) & "', " ' FA_AO
                                        strSql &= "'" & Common.parseNull(dtAO.Rows(k)("AGA_A_AO")) & "', " ' FA_A_AO
                                        strSql &= k + 1 & ", 0, " ' FA_SEQ, FA_AO_ACTION
                                        strSql &= dtAO.Rows(k)("AGA_GRP_INDEX") & ", " ' FA_APPROVAL_GRP_INDEX
                                        strSql &= "'" & Common.parseNull(dtAO.Rows(k)("AGA_RELIEF_IND")) & "') " ' FA_RELIEF_IND
                                        Common.Insert2Ary(strarray, strSql)
                                    Next
                                End If
                            Else
                                strSql = "INSERT INTO FINANCE_APPROVAL (FA_INVOICE_INDEX, FA_AGA_TYPE, FA_AO, FA_A_AO, FA_SEQ, FA_AO_ACTION "
                                strSql &= ") SELECT "
                                strSql &= objDB.GetLatestInsertedID("INVOICE_MSTR")
                                strSql &= ", 'FO', USERS_USRGRP.UU_USER_ID, '', 1, 0 "
                                strSql &= "FROM USER_GROUP_MSTR INNER JOIN "
                                strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                                strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Officer') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                                strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') LIMIT 1 "
                                Common.Insert2Ary(strarray, strSql)

                                strSql = "INSERT INTO FINANCE_APPROVAL (FA_INVOICE_INDEX, FA_AGA_TYPE, FA_AO, FA_A_AO, FA_SEQ, FA_AO_ACTION "
                                strSql &= ") SELECT "
                                strSql &= objDB.GetLatestInsertedID("INVOICE_MSTR")
                                strSql &= ", 'FM', USERS_USRGRP.UU_USER_ID, '', 2, 0 "
                                strSql &= "FROM USER_GROUP_MSTR INNER JOIN "
                                strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                                strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Manager') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                                strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') LIMIT 1 "
                                Common.Insert2Ary(strarray, strSql)
                            End If
                        Else
                            'Michelle (23/8/2007) - To cater for those without invoice approval
                            strSql = "INSERT INTO FINANCE_APPROVAL (FA_INVOICE_INDEX, FA_AGA_TYPE, FA_AO, FA_A_AO, FA_SEQ, FA_AO_ACTION "
                            strSql &= ")  SELECT "
                            strSql &= objDb.GetLatestInsertedID("INVOICE_MSTR")
                            strSql &= ", 'FO', USERS_USRGRP.UU_USER_ID, '', 1, 0 "
                            strSql &= "FROM USER_MSTR, USER_GROUP_MSTR INNER JOIN "
                            strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                            strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Officer') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                            strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') "
                            strSql &= "AND USER_MSTR.UM_USER_ID = USERS_USRGRP.UU_USER_ID AND USER_MSTR.UM_COY_ID  = USERS_USRGRP.UU_COY_ID AND USER_MSTR.UM_STATUS = 'A' LIMIT 1 "
                            Common.Insert2Ary(strarray, strSql)

                            strSql = "INSERT INTO FINANCE_APPROVAL (FA_INVOICE_INDEX, FA_AGA_TYPE, FA_AO, FA_A_AO, FA_SEQ, FA_AO_ACTION "
                            strSql &= ")  SELECT "
                            strSql &= objDb.GetLatestInsertedID("INVOICE_MSTR")
                            strSql &= ", 'FM', USERS_USRGRP.UU_USER_ID, '', 2, 0 "
                            strSql &= "FROM USER_MSTR, USER_GROUP_MSTR INNER JOIN "
                            strSql &= "USERS_USRGRP ON USER_GROUP_MSTR.UGM_USRGRP_ID = USERS_USRGRP.UU_USRGRP_ID "
                            strSql &= "WHERE (USER_GROUP_MSTR.UGM_FIXED_ROLE = 'Finance Manager') AND (USERS_USRGRP.UU_COY_ID = '" & strBCoyID & "') AND  "
                            strSql &= "(USER_GROUP_MSTR.UGM_TYPE = 'BUYER') AND USER_MSTR.UM_USER_ID = USERS_USRGRP.UU_USER_ID AND USER_MSTR.UM_COY_ID  = USERS_USRGRP.UU_COY_ID AND USER_MSTR.UM_STATUS = 'A' LIMIT 1 "
                            Common.Insert2Ary(strarray, strSql)
                        End If

                        'Modified for Agora GST Stage 2 - CH - 3/3/2015
                        ' delete COMPANY_DOC_ATTACHMENT table
                        strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
                        strSql &= "WHERE CDA_COY_ID = '" & strSCoyID & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'INV' "
                        strSql &= "AND CDA_DOC_NO = '" & Doc_No & "' "
                        Common.Insert2Ary(strarray, strSql)

                        strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
                        strSql &= "WHERE CDA_COY_ID = '" & strSCoyID & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'INV' "
                        strSql &= "AND CDA_DOC_NO = " & Inv_num & " "
                        Common.Insert2Ary(strarray, strSql)

                        ' update COMPANY_DOC_ATTACHMENT_TEMP table
                        strSql = "UPDATE COMPANY_DOC_ATTACHMENT_TEMP SET "
                        strSql &= "CDA_DOC_NO = " & Inv_num & " "
                        strSql &= "WHERE CDA_COY_ID = '" & strSCoyID & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'INV' "
                        strSql &= "AND CDA_DOC_NO = '" & Doc_No & "' "
                        Common.Insert2Ary(strarray, strSql)

                        ' insert COMPANY_DOC_ATTACHMENT table
                        strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT(CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS) "
                        strSql &= "SELECT CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CDA_STATUS FROM company_doc_attachment_temp "
                        strSql &= "WHERE CDA_COY_ID = '" & strSCoyID & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'INV' "
                        strSql &= "AND CDA_DOC_NO = " & Inv_num & " "
                        Common.Insert2Ary(strarray, strSql)

                        ' delete COMPANY_DOC_ATTACHMENT_TEMP table
                        strSql = "DELETE FROM COMPANY_DOC_ATTACHMENT_TEMP "
                        strSql &= "WHERE CDA_COY_ID = '" & strSCoyID & "' "
                        strSql &= "AND CDA_DOC_TYPE = 'INV' "
                        strSql &= "AND CDA_DOC_NO = " & Inv_num & " "
                        Common.Insert2Ary(strarray, strSql)
                        '---------------------------------------------------------

                        If i = 0 Then
                            strSql = " SET @T_NO = " & Inv_num & "; "
                            Common.Insert2Ary(strarray, strSql)
                        Else
                            strSql = " SET @T_NO = CONCAT(CONCAT(@T_NO,',')," & Inv_num & "); "
                            Common.Insert2Ary(strarray, strSql)
                        End If

                        strSql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & strSCoyID & "' AND CP_PARAM_TYPE = 'Invoice' "
                        Common.Insert2Ary(strarray, strSql)
                    Else
                        If strInvFail = "" Then
                            strInvFail = Trim(strTempInv)
                        Else
                            strInvFail = strInvFail & "," & Trim(strTempInv)
                        End If
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

                            strPONo = ds.Tables(0).Rows(i).Item("po_no")

                            Dim strInv As String = objDb.GetVal("SELECT CAST(GROUP_CONCAT(DISTINCT CDM_INVOICE_NO) AS CHAR(1000)) AS INV FROM COMPANY_DOC_MATCH " &
                                    " WHERE CDM_INVOICE_NO IN ('" & strTrans.Replace(",", "','") & "') " &
                                    " AND CDM_B_COY_ID = '" & ds.Tables(0).Rows(i).Item("b_com_id") & "' AND CDM_S_COY_ID = '" & strSCoyID & "' ")

                            Dim strPO As String = objDb.GetVal("SELECT CAST(GROUP_CONCAT(DISTINCT CDM_PO_NO) AS CHAR(1000)) AS PO FROM COMPANY_DOC_MATCH " &
                                    " WHERE CDM_INVOICE_NO IN ('" & strTrans.Replace(",", "','") & "') " &
                                    " AND CDM_B_COY_ID = '" & ds.Tables(0).Rows(i).Item("b_com_id") & "' AND CDM_S_COY_ID = '" & strSCoyID & "' ")

                            'intPOIndex = get_poindex(ds.Tables(0).Rows(i).Item("po_no"), ds.Tables(0).Rows(i).Item("b_com_id"))

                            'sendNotification(EmailType.InvoiceCreated, strLoginUser, ds.Tables(0).Rows(i).Item("b_com_id"), strSCoyID, strInv, strPO)
                        Next
                    Else
                        strInvSuccess = ""
                    End If
                End If
            Else
            End If
            OBJGLB = Nothing
            objTrans = Nothing
            objBCM = Nothing
            Return True
        End Function
        Function BuildQueryForUnInv(ByVal userID As String, ByVal strAction As String, ByVal strBillMethod As String, ByVal strDocNo As String, ByVal strBCoyID As String, Optional ByVal intPOIndex As Integer = 0, Optional ByVal AllowSetSorting As Boolean = False) As String
            'strAction - Get Line Items Details,Get Remaining Amt, Get BCM
            Dim strSql As String
            Dim strTableList, strFieldList, strGrpBy, strOrderBy As String
            Dim companyID = objUsr.getCompanyID(userID)
            'SUM((PRD_ORDERED_QTY*PRD_UNIT_COST) + " _
            '& "((PRD_ORDERED_QTY*PRD_UNIT_COST)* (POD_GST/100)))
            Select Case strBillMethod
                Case "FPO"
                    ' TODO add filter to vendor company
                    strTableList = " FROM PO_DETAILS INNER JOIN PO_MSTR ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = '" & strBCoyID & "' " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = POD_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE " &
                    " POD_PO_NO='" & strDocNo & "' AND POD_COY_ID='" & strBCoyID & "' AND POM_S_COY_ID = '" & companyID & "' AND (POD_ORDERED_QTY <> POD_CANCELLED_QTY)"
                    Select Case LCase(strAction)
                        Case "line"
                            'Jules 2019.03.22 - Added Gift and Analysis Codes.
                            'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH - Include POD_GST_INPUT_TAX_CODE
                            strFieldList = "POM_PO_INDEX,POD_PRODUCT_DESC,POD_PO_LINE,POD_B_ITEM_CODE,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_UOM,POD_GST,POD_GST_RATE,POD_TAX_VALUE,POD_GST_INPUT_TAX_CODE,POD_DELIVERED_QTY AS QTY," &
                            " POD_UNIT_COST,POD_WARRANTY_TERMS,POD_ACCT_INDEX,POD_PR_INDEX,POD_PR_LINE,POD_ORDERED_QTY, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE, " &
                            " IF((TAX_PERC = '' OR TAX_PERC IS NULL), IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST_RATE, " &
                            " POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE "
                            strGrpBy = ""
                            strOrderBy = "ORDER BY POD_PO_LINE"
                        Case "price"
                            '//delivered = order-cancel
                            '2015-06-22: CH: Rounding issue (Prod issue)
                            'strFieldList = "POM_PO_INDEX,SUM((POD_DELIVERED_QTY * POD_UNIT_COST) + ((POD_DELIVERED_QTY * POD_UNIT_COST) * (POD_GST/100))) as total, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            strFieldList = "POM_PO_INDEX,SUM(ROUND(POD_DELIVERED_QTY * POD_UNIT_COST,2) + ROUND(ROUND(POD_DELIVERED_QTY * POD_UNIT_COST,2) * (POD_GST/100),2)) AS total, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            strGrpBy = "GROUP BY POD_PO_NO,POD_COY_ID"
                            strOrderBy = ""
                        Case "bcm"
                            strFieldList = "POM_PO_INDEX,POD_ACCT_INDEX,SUM((POD_ORDERED_QTY*POD_UNIT_COST) + ((POD_ORDERED_QTY*POD_UNIT_COST) * (POD_GST/100))) as amt, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE"
                            strGrpBy = "GROUP BY POD_ACCT_INDEX"
                            strOrderBy = ""
                    End Select
                Case "DO"
                    strTableList = " FROM DO_DETAILS A, PO_DETAILS B " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = '" & strBCoyID & "' " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = B.POD_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY, " &
                    "PO_MSTR C WHERE " &
                    " C.POM_S_COY_ID = A.DOD_S_COY_ID " &
                    " AND A.DOD_PO_LINE=B.POD_PO_LINE AND DOD_DO_NO='" & strDocNo & "' AND POM_B_COY_ID='" & strBCoyID & "'" &
                    " AND B.POD_COY_ID=C.POM_B_COY_ID AND B.POD_PO_NO=C.POM_PO_NO AND DOD_DO_QTY > 0 AND C.POM_PO_INDEX=" & intPOIndex

                    Select Case LCase(strAction)
                        Case "line"
                            'Jules 2019.03.22 - Added Gift and Analysis Codes.
                            'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH - Include POD_GST_INPUT_TAX_CODE
                            strFieldList = "POM_PO_INDEX,POD_PRODUCT_DESC,POD_PO_LINE,POD_B_ITEM_CODE,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_UOM,POD_GST,POD_GST_RATE,POD_TAX_VALUE,POD_GST_INPUT_TAX_CODE,DOD_DO_QTY AS QTY," &
                            " POD_UNIT_COST,POD_WARRANTY_TERMS,POD_ACCT_INDEX,POD_PR_INDEX,POD_PR_LINE,POD_ORDERED_QTY, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE, " &
                            " IF((TAX_PERC = '' OR TAX_PERC IS NULL), IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST_RATE, " &
                             " POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE "
                            strOrderBy = "ORDER BY POD_PO_LINE"
                        Case "price"
                            '2015-06-22: CH: Rounding issue (Prod issue)
                            'strFieldList = "POM_PO_INDEX,SUM((DOD_DO_QTY* POD_UNIT_COST) + ((DOD_DO_QTY* POD_UNIT_COST) * (POD_GST/100))) as total, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            strFieldList = "POM_PO_INDEX,SUM(ROUND(DOD_DO_QTY* POD_UNIT_COST,2) + ROUND(ROUND(DOD_DO_QTY* POD_UNIT_COST,2) * (POD_GST/100),2)) AS total, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            'strFieldList = "SUM(DOD_DO_QTY* POD_UNIT_COST) as total "
                            strGrpBy = " GROUP BY DOD_DO_NO,DOD_S_COY_ID"
                            strOrderBy = ""
                        Case "bcm"
                            strFieldList = "POM_PO_INDEX,POD_ACCT_INDEX,SUM((DOD_DO_QTY* POD_UNIT_COST) + ((DOD_DO_QTY* POD_UNIT_COST) * (POD_GST/100))) AS amt, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            strGrpBy = "GROUP BY POD_ACCT_INDEX"
                            strOrderBy = ""
                    End Select

                Case "GRN"
                    strTableList = " FROM GRN_DETAILS A, PO_DETAILS B " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = '" & strBCoyID & "' " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = B.POD_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = CM_COUNTRY, " &
                    "PO_MSTR C WHERE " &
                    " A.GD_PO_LINE=B.POD_PO_LINE AND GD_GRN_NO='" & strDocNo & "' AND GD_B_COY_ID='" & strBCoyID & "'" &
                    " AND B.POD_COY_ID=C.POM_B_COY_ID AND B.POD_PO_NO=C.POM_PO_NO AND (GD_RECEIVED_QTY-GD_REJECTED_QTY) > 0 AND C.POM_PO_INDEX=" & intPOIndex

                    Select Case LCase(strAction)
                        Case "line"
                            'Jules 2018.05.15 - PAMB Scrum 3 - Added Gift and Analysis Codes.
                            'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH - Include POD_GST_INPUT_TAX_CODE
                            strFieldList = "POM_PO_INDEX,POD_PRODUCT_DESC,POD_PO_LINE,POD_B_ITEM_CODE,POD_B_CATEGORY_CODE,POD_B_GL_CODE,POD_UOM,POD_GST,POD_GST_RATE,POD_TAX_VALUE,POD_GST_INPUT_TAX_CODE,(GD_RECEIVED_QTY-GD_REJECTED_QTY) AS QTY," &
                            " POD_UNIT_COST,POD_WARRANTY_TERMS,POD_ACCT_INDEX,POD_PR_INDEX,POD_PR_LINE,POD_ORDERED_QTY, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE, " &
                            " IF((TAX_PERC = '' OR TAX_PERC IS NULL), IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST_RATE, " &
                            " POD_GIFT, POD_FUND_TYPE, POD_PERSON_CODE, POD_PROJECT_CODE "
                            strOrderBy = "ORDER BY POD_PO_LINE"
                        Case "price"
                            '2015-06-22: CH: Rounding issue (Prod issue)
                            'strFieldList = "POM_PO_INDEX,SUM(((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST) + ROUND(((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST) * (POD_GST/100),2)) as total, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            strFieldList = "POM_PO_INDEX,SUM(ROUND((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST,2) + ROUND(ROUND((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST,2) * (POD_GST/100),2)) AS total, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            'strFieldList = "SUM((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST) as total"
                            strGrpBy = " GROUP BY GD_GRN_NO,GD_B_COY_ID"
                            strOrderBy = ""
                        Case "bcm"
                            strFieldList = "POM_PO_INDEX,POD_ACCT_INDEX,SUM(((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST) + ROUND(((GD_RECEIVED_QTY - GD_REJECTED_QTY) * POD_UNIT_COST) * (POD_GST/100),2)) AS amt, CONCAT(POD_ASSET_GROUP, CONCAT(' ',POD_ASSET_NO)) AS ASSET_CODE "
                            strGrpBy = "GROUP BY POD_ACCT_INDEX"
                            strOrderBy = ""
                    End Select
            End Select
            strSql = "SELECT " & strFieldList & " " & strTableList & " " & strGrpBy & " "
            strSql += Convert.ToString(IIf(AllowSetSorting, strOrderBy, ""))
            Return strSql
        End Function

        Function checkMassApp(ByVal struserid As String)
            Dim strSQL As String

            strSQL = "SELECT UM_INVOICE_MASS_APP FROM USERS_USRGRP,USER_GROUP_MSTR GM, USER_MSTR M " &
                     "where UU_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' " &
                     "AND UM_INVOICE_MASS_APP='Y' AND M.UM_USER_ID = '" & struserid & "' " &
                     "AND UU_USER_ID = M.UM_USER_ID " &
                     "AND M.UM_COY_ID = UU_COY_ID " &
                     "AND UU_USRGRP_ID = GM.UGM_USRGRP_ID " &
                     "AND GM.UGM_FIXED_ROLE = 'Finance Officer'"

            If objDb.Exist(strSQL) > 0 Then
                checkMassApp = 1
            Else
                checkMassApp = 0
            End If
        End Function

        Function getSupplyTaxCodeFromArray(ByVal intPOLine As Integer, ByVal AryTemp As ArrayList) As String
            Dim i As Integer

            For i = 0 To AryTemp.Count - 1
                If intPOLine = AryTemp(i)(0) Then
                    Return AryTemp(i)(1)
                End If
            Next
        End Function

        'Modified for Agora GST Stage 2 - CH - 2/2/2015
        Public Sub delInvAttachTemp(Optional ByVal intIndex As Integer = 0, Optional ByVal strDocNo As String = "", Optional ByVal strConnStr As String = Nothing)
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
                    strsql &= " CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'INV' AND CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                End If
            End If

            objDb.Execute(strsql)

        End Sub

        Public Function getInvTempAttach(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E") As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT_TEMP " &
                    "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
                    "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'INV' "

            ds = objDb.FillDs(strsql)
            getInvTempAttach = ds
        End Function

        Function getInvAttachment(ByVal strInvNo As String, ByVal strSCoyID As String) As DataSet
            Dim ds As DataSet
            Dim strSql As String

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strSCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strInvNo) & "' AND CDA_DOC_TYPE='INV'"
            ds = objDb.FillDs(strSql)

            Return ds
        End Function
        Function getInvAttachmentString(ByVal strInvNo As String, ByVal strSCoyID As String) As String
            Dim ds As DataSet
            Dim strSql As String

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strSCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strInvNo) & "' AND CDA_DOC_TYPE='INV'"
            ds = objDb.FillDs(strSql)

            Return strSql
        End Function
        '--------------------------------------

        Public Function GetInvDetailForCNDN(ByVal strInvNo As String, ByVal strVcomid As String) As DataSet
            Dim strsql As String

            strsql = "SELECT INVOICE_DETAILS.*, INVOICE_MSTR.*, PO_MSTR.*, " &
                    "CASE WHEN IM_GST_INVOICE = 'Y' THEN " &
                    "IF(ID_GST_RATE <> 'EX', CONCAT(CODE_DESC, ' (', CAST(ID_GST AS UNSIGNED), '%)'), CODE_DESC)  ELSE 'N/A' END AS GST_RATE " &
                    "FROM INVOICE_DETAILS " &
                    "INNER JOIN INVOICE_MSTR ON ID_INVOICE_NO = IM_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                    "INNER JOIN PO_MSTR ON IM_PO_INDEX = POM_PO_INDEX " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = ID_GST_RATE " &
                    "WHERE IM_INVOICE_NO= '" & Common.Parse(strInvNo) & "' AND IM_S_COY_ID= '" & strVcomid & "'"
            GetInvDetailForCNDN = objDb.FillDs(strsql)
        End Function
#End Region
    End Class
End Namespace